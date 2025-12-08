using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerCore.ServerCore
{
    public static class GameCore
    {
        public static RoomManager RoomManager = new RoomManager();

        public static void ProcessMove(ClientConnection player, string payload)
        {
            try
            {
                var mv = JsonHelper.Deserialize<MovePayload>(payload);
                int roomId = mv.roomId;
                int x = mv.x;
                int y = mv.y;

                var room = RoomManager.GetRoom(roomId);
                if (room == null)
                {
                    player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Room not found." }));
                    return;
                }

                lock (room.Lock)
                {
                    if (room.Status != "PLAYING")
                    {
                        player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Game not playing." }));
                        return;
                    }

                    if (player != room.CurrentPlayer)
                    {
                        player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Not your turn." }));
                        return;
                    }

                    if (x < 0 || x >= room.BoardSize || y < 0 || y >= room.BoardSize)
                    {
                        player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Invalid coords." }));
                        return;
                    }

                    if (room.Board[x, y] != 0)
                    {
                        player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Cell occupied." }));
                        return;
                    }

                    int mark = player == room.Player1 ? 1 : 2;
                    room.Board[x, y] = mark;
                    int step = ++room.MoveCount;

                    // Persist move (async, not blocking)
                    _ = Services.Database.SaveMove(room.MatchId, player.Username, x, y, step);

                    // toggle turn
                    room.CurrentPlayer = player == room.Player1 ? room.Player2 : room.Player1;

                    // broadcast update
                    var body = JsonHelper.Serialize(new { roomId, x, y, symbol = mark == 1 ? "X" : "O", player = player.Username, nextTurn = room.CurrentPlayer?.Username });
                    var env = new MessageEnvelope { Type = MessageType.GAME_UPDATE, Payload = body };
                    room.Player1?.SendEnvelope(MessageType.GAME_UPDATE, body);
                    room.Player2?.SendEnvelope(MessageType.GAME_UPDATE, body);

                    // win check
                    if (GameLogic.CheckWin(room.Board, x, y))
                    {
                        room.Status = "FINISHED";
                        var winner = player;
                        var loser = (player == room.Player1 ? room.Player2 : room.Player1);

                        Task.Run(() => RoomManager.HandleGameEnd(room, winner, loser, "WIN_BY_MOVE"));

                        var endBody = JsonHelper.Serialize(new { roomId, winner = player.Username, endReason = "WIN_BY_MOVE" });
                        room.Player1?.SendEnvelope(MessageType.GAME_END, endBody);
                        room.Player2?.SendEnvelope(MessageType.GAME_END, endBody);

                        RoomManager.RemoveRoom(room);
                        return;
                    }

                    // draw check
                    bool full = true;
                    for (int i = 0; i < room.BoardSize && full; i++)
                        for (int j = 0; j < room.BoardSize; j++)
                            if (room.Board[i, j] == 0) { full = false; break; }

                    if (full)
                    {
                        room.Status = "FINISHED";
                        Task.Run(() => RoomManager.HandleGameEnd(room, null, null, "DRAW_BY_FULL_BOARD"));
                        var drawBody = JsonHelper.Serialize(new { roomId, winner = (string)null, draw = true, endReason = "DRAW_BY_FULL_BOARD" });
                        room.Player1?.SendEnvelope(MessageType.GAME_END, drawBody);
                        room.Player2?.SendEnvelope(MessageType.GAME_END, drawBody);
                        RoomManager.RemoveRoom(room);
                    }
                }
            }
            catch (Exception ex)
            {
                Server.Log($"Error ProcessMove: {ex.Message}");
                player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Internal error" }));
            }
        }
    }

    public class Room
    {
        public int Id { get; set; }
        public ClientConnection Player1 { get; set; }
        public ClientConnection Player2 { get; set; }
        public ClientConnection CurrentPlayer { get; set; }
        public string Status { get; set; } = "WAITING";
        public readonly object Lock = new object();
        public int BoardSize { get; set; } = 15;
        public int[,] Board { get; set; }
        public int MoveCount { get; set; } = 0;
        public int MatchId { get; set; } = 0;

        public Room()
        {
            Board = new int[BoardSize, BoardSize];
        }
    }

    public class RoomManager
    {
        public static Action OnRoomListChanged;
        private readonly object _lock = new object();
        private readonly Dictionary<int, Room> _rooms = new Dictionary<int, Room>();
        private int _autoId = 1;

        public Room CreateRoom(ClientConnection creator)
        {
            if (creator == null) return null;
            if (IsInRoom(creator))
            {
                creator.SendEnvelope(MessageType.ROOM_CREATE_FAIL, JsonHelper.Serialize(new { message = "Already in room" }));
                return null;
            }

            Room room;
            lock (_lock)
            {
                room = new Room { Id = _autoId++, Player1 = creator, CurrentPlayer = creator, Status = "WAITING" };
                _rooms[room.Id] = room;
            }

            creator.SendEnvelope(MessageType.ROOM_CREATE_OK, JsonHelper.Serialize(new { roomId = room.Id }));
            BroadcastRoomList();
            return room;
        }

        private bool IsInRoom(ClientConnection c)
        {
            lock (_lock) return _rooms.Values.Any(r => r.Player1 == c || r.Player2 == c);
        }

        public Room GetRoom(int id)
        {
            lock (_lock) return _rooms.ContainsKey(id) ? _rooms[id] : null;
        }

        public void JoinRoom(ClientConnection joiner, string payload)
        {
            if (!int.TryParse(payload, out int roomId))
            {
                joiner.SendEnvelope(MessageType.ROOM_JOIN_FAIL, JsonHelper.Serialize(new { message = "Invalid room id" }));
                return;
            }

            var room = GetRoom(roomId);
            if (room == null) { joiner.SendEnvelope(MessageType.ROOM_JOIN_FAIL, JsonHelper.Serialize(new { message = "Room not found" })); return; }

            lock (room.Lock)
            {
                if (room.Player2 != null || room.Status != "WAITING")
                {
                    joiner.SendEnvelope(MessageType.ROOM_JOIN_FAIL, JsonHelper.Serialize(new { message = "Room full or started" }));
                    return;
                }

                if (room.Player1 == joiner) { joiner.SendEnvelope(MessageType.ROOM_JOIN_FAIL, JsonHelper.Serialize(new { message = "Cannot join your own room" })); return; }

                room.Player2 = joiner;
                room.Status = "PLAYING";
                room.Board = new int[room.BoardSize, room.BoardSize];
                // default CurrentPlayer remains Player1 (X)
                room.MatchId = Services.Database.CreateMatch(room.Player1.Username, room.Player2.Username);

                var okBody = JsonHelper.Serialize(new { roomId = room.Id, player1 = room.Player1.Username, player2 = room.Player2.Username, firstTurn = room.CurrentPlayer.Username });
                room.Player1?.SendEnvelope(MessageType.ROOM_JOIN_OK, okBody);
                room.Player2?.SendEnvelope(MessageType.ROOM_JOIN_OK, okBody);
            }

            BroadcastRoomList();
        }

        public void LeaveRoom(ClientConnection player)
        {
            if (player == null) return;

            Room found = null;
            lock (_lock)
            {
                foreach (var r in _rooms.Values)
                {
                    lock (r.Lock)
                    {
                        if (r.Player1 == player || r.Player2 == player) { found = r; break; }
                    }
                }
            }

            if (found == null) return;

            lock (found.Lock)
            {
                var other = found.Player1 == player ? found.Player2 : found.Player1;
                if (found.Status == "PLAYING" && other != null)
                {
                    HandleGameEnd(found, other, player, "OPPONENT_LEFT");
                    other.SendEnvelope(MessageType.GAME_END, JsonHelper.Serialize(new { roomId = found.Id, winner = other.Username, opponentLeft = true, endReason = "OPPONENT_LEFT" }));
                }
                found.Status = "FINISHED";
                RemoveRoom(found);
            }

            BroadcastRoomList();
        }

        public void Surrender(ClientConnection player)
        {
            if (player == null) return;

            Room found = null;
            lock (_lock)
            {
                foreach (var r in _rooms.Values)
                {
                    lock (r.Lock)
                    {
                        if (r.Player1 == player || r.Player2 == player) { found = r; break; }
                    }
                }
            }

            if (found == null || found.Status != "PLAYING")
            {
                player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Cannot surrender" }));
                return;
            }

            lock (found.Lock)
            {
                var winner = found.Player1 == player ? found.Player2 : found.Player1;
                var loser = player;
                if (winner != null)
                {
                    HandleGameEnd(found, winner, loser, "SURRENDER");
                    player.SendEnvelope(MessageType.GAME_SURRENDER_OK, JsonHelper.Serialize(new { ok = true }));
                    winner.SendEnvelope(MessageType.GAME_SURRENDER_RECV, JsonHelper.Serialize(new { message = $"{player.Username} surrendered." }));
                    var end = JsonHelper.Serialize(new { roomId = found.Id, winner = winner.Username, endReason = "SURRENDER" });
                    found.Player1?.SendEnvelope(MessageType.GAME_END, end);
                    found.Player2?.SendEnvelope(MessageType.GAME_END, end);
                }
                else
                {
                    player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Opponent disconnected" }));
                }
                found.Status = "FINISHED";
                RemoveRoom(found);
            }
        }

        public void RemoveRoom(Room room)
        {
            if (room == null) return;
            lock (_lock)
            {
                if (_rooms.ContainsKey(room.Id)) _rooms.Remove(room.Id);
            }
            Server.Log($"Room {room.Id} removed.");
            BroadcastRoomList();
        }

        public void HandleClientDisconnected(ClientConnection client)
        {
            if (client == null) return;
            Room toRemove = null;
            lock (_lock)
            {
                foreach (var r in _rooms.Values)
                {
                    lock (r.Lock)
                    {
                        if (r.Player1 == client || r.Player2 == client) { toRemove = r; break; }
                    }
                }
            }

            if (toRemove != null)
            {
                lock (toRemove.Lock)
                {
                    var other = toRemove.Player1 == client ? toRemove.Player2 : toRemove.Player1;
                    if (toRemove.Status == "PLAYING" && other != null)
                    {
                        HandleGameEnd(toRemove, other, client, "DISCONNECT");
                        other.SendEnvelope(MessageType.GAME_END, JsonHelper.Serialize(new { roomId = toRemove.Id, opponentLeft = true, winner = other.Username, endReason = "DISCONNECT" }));
                    }
                    toRemove.Status = "FINISHED";
                    lock (_lock) { if (_rooms.ContainsKey(toRemove.Id)) _rooms.Remove(toRemove.Id); }
                    Server.Log($"Room {toRemove.Id} closed due to disconnect.");
                }
            }
            BroadcastRoomList();
        }

        public void ChatInRoom(ClientConnection player, string payload)
        {
            try
            {
                var dyn = JsonHelper.Deserialize<dynamic>(payload);
                int roomId = dyn.GetProperty("roomId").GetInt32();
                string text = dyn.GetProperty("text").GetString();
                var room = GetRoom(roomId);
                if (room == null) { player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Room not found" })); return; }

                var packet = JsonHelper.Serialize(new { from = player.Username, text = text });
                room.Player1?.SendEnvelope(MessageType.CHAT_RECV, packet);
                room.Player2?.SendEnvelope(MessageType.CHAT_RECV, packet);
            }
            catch
            {
                player.SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message = "Chat error" }));
            }
        }

        public void SendRoomList(ClientConnection client)
        {
            var json = GetRoomListJson();
            client.SendEnvelope(MessageType.ROOM_UPDATE, json);
        }

        private string GetRoomListJson()
        {
            var list = new List<object>();
            lock (_lock)
            {
                foreach (var r in _rooms.Values)
                {
                    if (r.Status == "WAITING" || r.Status == "PLAYING")
                        list.Add(new { id = r.Id, p1 = r.Player1?.Username, p2 = r.Player2?.Username, status = r.Status });
                }
            }
            return JsonHelper.Serialize(list);
        }

        public void BroadcastRoomList()
        {
            var json = GetRoomListJson();
            Server.Broadcast(new MessageEnvelope { Type = MessageType.ROOM_UPDATE, Payload = json });
        }

        public static void HandleGameEnd(Room room, ClientConnection winner, ClientConnection loser, string endReason)
        {
            Task.Run(async () =>
            {
                try
                {
                    if (winner != null && loser != null)
                    {
                        const int RANK_DELTA = 10;
                        Services.Database.UpdateUserStats(winner.Username, RANK_DELTA, true);
                        Services.Database.UpdateUserStats(loser.Username, -RANK_DELTA, false);
                        await Services.Database.SaveMatch(room.Player1.Username, room.Player2.Username, winner.Username, endReason);
                    }
                    else if (endReason == "DRAW_BY_FULL_BOARD")
                    {
                        await Services.Database.SaveMatch(room.Player1.Username, room.Player2.Username, null, endReason);
                    }
                }
                catch (Exception ex) { Server.Log($"HandleGameEnd error: {ex.Message}"); }
            });
        }

        // helpers
        public List<object> GetSafeList()
        {
            lock (_lock)
            {
                return _rooms.Values.Select(r => new { r.Id, Player1 = r.Player1?.Username ?? "-", Player2 = r.Player2?.Username ?? "-", r.Status }).Cast<object>().ToList();
            }
        }

        public List<Room> GetRoomSnapshot()
        {
            lock (_lock) return _rooms.Values.ToList();
        }
    }

    public static class GameLogic
    {
        public static bool CheckWin(int[,] board, int x, int y)
        {
            int n = board.GetLength(0);
            int target = board[x, y];
            if (target == 0) return false;

            (int dx, int dy)[] dirs = { (1, 0), (0, 1), (1, 1), (1, -1) };
            foreach (var (dx, dy) in dirs)
            {
                int count = 1;
                int cx = x + dx, cy = y + dy;
                while (cx >= 0 && cx < n && cy >= 0 && cy < n && board[cx, cy] == target) { count++; cx += dx; cy += dy; }
                cx = x - dx; cy = y - dy;
                while (cx >= 0 && cx < n && cy >= 0 && cy < n && board[cx, cy] == target) { count++; cx -= dx; cy -= dy; }
                if (count >= 5) return true;
            }
            return false;
        }
    }
}
