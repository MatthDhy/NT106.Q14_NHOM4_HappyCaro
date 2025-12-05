using Server;
using ServerCore.ServerCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ServerCore
{
    public static class GameCore
    {
        public static RoomManager RoomManager = new RoomManager();

        // ==========================
        // HANDLE MOVE FROM PLAYER
        // ==========================
        public static void ProcessMove(ClientConnection player, string payload)
        {
            try
            {
                var data = JsonHelper.Deserialize<MovePayload>(payload);

                int roomId = Convert.ToInt32(data.roomId);
                int x = Convert.ToInt32(data.x);
                int y = Convert.ToInt32(data.y);

                var room = RoomManager.GetRoom(roomId);
                if (room == null)
                {
                    player.Send(new { Type = MessageType.ERROR, Payload = "Room not found." });
                    return;
                }

                lock (room.Lock)
                {
                    if (room.Status != "PLAYING")
                    {
                        player.Send(new { Type = MessageType.ERROR, Payload = "Game is not playing." });
                        return;
                    }

                    //Kiểm tra lượt chơi
                    if (player != room.CurrentPlayer)
                    {
                        player.Send(new { Type = MessageType.ERROR, Payload = "Not your turn." });
                        return;
                    }

                    // check valid range
                    if (x < 0 || x >= room.BoardSize || y < 0 || y >= room.BoardSize)
                    {
                        player.Send(new { Type = MessageType.ERROR, Payload = $"Invalid coordinates: ({x},{y})" });
                        return;
                    }

                    // check empty cell
                    if (room.Board[x, y] != 0)
                    {
                        player.Send(new { Type = MessageType.ERROR, Payload = "Cell already marked." });
                        return;
                    }

                    // determine mark (1 cho Player1/X, 2 cho Player2/O)
                    int mark = (player == room.Player1 ? 1 : 2);
                    room.Board[x, y] = mark;

                    // logic chuyển lượt
                    room.CurrentPlayer = (player == room.Player1 ? room.Player2 : room.Player1);

                    // broadcast move
                    var update = new
                    {
                        Type = MessageType.GAME_UPDATE,
                        Payload = JsonHelper.Serialize(new
                        {
                            roomId,
                            x,
                            y,
                            symbol = mark == 1 ? "X" : "O",
                            player = player.Username,
                            nextTurn = room.CurrentPlayer?.Username
                        })
                    };

                    room.Player1?.Send(update);
                    room.Player2?.Send(update);

                    // --- XỬ LÝ KẾT THÚC TRẬN ĐẤU ---

                    ClientConnection winner = player;
                    ClientConnection loser = (player == room.Player1 ? room.Player2 : room.Player1);

                    // check win
                    if (GameLogic.CheckWin(room.Board, x, y))
                    {
                        room.Status = "FINISHED";

                        RoomManager.HandleGameEnd(room, winner, loser, "WIN_BY_MOVE");
                        RoomManager.RemoveRoom(room); // Xóa phòng sau khi xử lý

                        var endMsg = new
                        {
                            Type = MessageType.GAME_END,
                            Payload = JsonHelper.Serialize(new
                            {
                                roomId,
                                winner = player.Username,
                                endReason = "WIN_BY_MOVE"
                            })
                        };

                        room.Player1?.Send(endMsg);
                        room.Player2?.Send(endMsg);

                        return;
                    }

                    // check full board => draw
                    bool full = true;
                    for (int i = 0; i < room.BoardSize; i++)
                        for (int j = 0; j < room.BoardSize; j++)
                            if (room.Board[i, j] == 0)
                            {
                                full = false;
                                break;
                            }

                    if (full)
                    {
                        room.Status = "FINISHED";

                        RoomManager.HandleGameEnd(room, null, null, "DRAW_BY_FULL_BOARD");
                        RoomManager.RemoveRoom(room); // Xóa phòng sau khi xử lý

                        var drawMsg = new
                        {
                            Type = MessageType.GAME_END,
                            Payload = JsonHelper.Serialize(new
                            {
                                roomId,
                                winner = (string)null,
                                draw = true,
                                endReason = "DRAW_BY_FULL_BOARD"
                            })
                        };

                        room.Player1?.Send(drawMsg);
                        room.Player2?.Send(drawMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Server.Log($"Error ProcessMove: {ex.Message}");
                player.Send(new { Type = MessageType.ERROR, Payload = "Internal error during move processing." });
            }
        }
    }


    // ================================
    // ROOM MANAGER
    // ================================
    public class RoomManager
    {
        private readonly object roomLock = new object();
        private Dictionary<int, Room> rooms = new Dictionary<int, Room>();
        private int autoId = 1;

        public Room CreateRoom(ClientConnection creator)
        {
            // Bổ sung: Kiểm tra xem người chơi đã ở trong phòng nào chưa (tùy chọn)
            // ...

            if (creator == null) return null;

            Room room;

            lock (roomLock)
            {
                room = new Room
                {
                    Id = autoId++,
                    Player1 = creator,
                    Status = "WAITING"
                };
                // Bổ sung: Thiết lập Player1 là người chơi hiện tại khi tạo phòng
                room.CurrentPlayer = creator;

                rooms[room.Id] = room;
            }

            Server.Log($"Room {room.Id} created by {creator.Username}");

            creator.Send(new
            {
                Type = MessageType.ROOM_CREATE_OK,
                Payload = JsonHelper.Serialize(new
                {
                    roomId = room.Id
                })
            });

            BroadcastRoomList();
            return room;
        }

        public void JoinRoom(ClientConnection joiner, string payload)
        {
            if (joiner == null) return;

            if (!int.TryParse(payload, out int roomId))
            {
                joiner.Send(new { Type = MessageType.ROOM_JOIN_FAIL, Payload = "Invalid Room ID format." });
                return;
            }

            var room = GetRoom(roomId);
            if (room == null)
            {
                joiner.Send(new { Type = MessageType.ROOM_JOIN_FAIL, Payload = "Room not found" });
                return;
            }

            lock (room.Lock)
            {
                if (room.Player2 != null || room.Status != "WAITING")
                {
                    joiner.Send(new { Type = MessageType.ROOM_JOIN_FAIL, Payload = "Room full or game started." });
                    return;
                }

                if (room.Player1 == joiner) // Ngăn người chơi tự tham gia
                {
                    joiner.Send(new { Type = MessageType.ROOM_JOIN_FAIL, Payload = "Cannot join your own room." });
                    return;
                }

                room.Player2 = joiner;
                room.Status = "PLAYING";

                // Bổ sung: Cập nhật người chơi đi đầu (đã được thiết lập mặc định là Player1 trong constructor)
                // Hoặc random ở đây: room.CurrentPlayer = (new Random().Next(2) == 0) ? room.Player1 : room.Player2;

                room.Board = new int[room.BoardSize, room.BoardSize]; // reset board

                // notify success
                var okMsg = new
                {
                    Type = MessageType.ROOM_JOIN_OK,
                    Payload = JsonHelper.Serialize(new
                    {
                        roomId = room.Id,
                        player1 = room.Player1.Username, //X
                        player2 = room.Player2.Username, //O
                        firstTurn = room.CurrentPlayer.Username
                    })
                };

                room.Player1.Send(okMsg);
                room.Player2.Send(okMsg);
            }

            BroadcastRoomList();
        }

        public void LeaveRoom(ClientConnection player)
        {
            if (player == null) return;

            Room roomToLeave = null;

            lock (roomLock)
            {
                foreach (var room in rooms.Values)
                {
                    lock (room.Lock)
                    {
                        if (room.Player1 == player || room.Player2 == player)
                        {
                            roomToLeave = room;
                            break;
                        }
                    }
                }
            }

            if (roomToLeave != null)
            {
                lock (roomToLeave.Lock)
                {
                    var other = roomToLeave.Player1 == player ? roomToLeave.Player2 : roomToLeave.Player1;

                    if (roomToLeave.Status == "PLAYING" && other != null)
                    {
                        // 1. Cập nhật Rank & Match History
                        RoomManager.HandleGameEnd(roomToLeave, other, player, "OPPONENT_LEFT");

                        // 2. Thông báo cho đối thủ (other)
                        other.Send(new
                        {
                            Type = MessageType.GAME_END,
                            Payload = JsonHelper.Serialize(new
                            {
                                roomId = roomToLeave.Id,
                                winner = other.Username,
                                opponentLeft = true,
                                endReason = "OPPONENT_LEFT"
                            })
                        });
                    }

                    // 3. Xóa phòng
                    roomToLeave.Status = "FINISHED";
                    RemoveRoom(roomToLeave);
                }
            }

            BroadcastRoomList();
        }

        // Hàm này xử lý khi người chơi đầu hàng
        public void Surrender(ClientConnection player)
        {
            if (player == null) return;

            Room roomToSurrender = null;

            lock (roomLock)
            {
                foreach (var room in rooms.Values)
                {
                    lock (room.Lock)
                    {
                        if (room.Player1 == player || room.Player2 == player)
                        {
                            roomToSurrender = room;
                            break;
                        }
                    }
                }
            }

            if (roomToSurrender != null && roomToSurrender.Status == "PLAYING")
            {
                lock (roomToSurrender.Lock)
                {
                    var winner = roomToSurrender.Player1 == player ? roomToSurrender.Player2 : roomToSurrender.Player1;
                    var loser = player;

                    if (winner != null)
                    {
                        // 1. Cập nhật Rank & Match History
                        RoomManager.HandleGameEnd(roomToSurrender, winner, loser, "SURRENDER");

                        // 2. Thông báo cho cả hai
                        var endMsg = new
                        {
                            Type = MessageType.GAME_END,
                            Payload = JsonHelper.Serialize(new
                            {
                                roomId = roomToSurrender.Id,
                                winner = winner.Username,
                                endReason = "SURRENDER"
                            })
                        };
                        roomToSurrender.Player1?.Send(endMsg);
                        roomToSurrender.Player2?.Send(endMsg);
                    }
                    else
                    {
                        // Trường hợp người còn lại đã bị ngắt kết nối
                        player.Send(new { Type = MessageType.ERROR, Payload = "Opponent already disconnected." });
                    }

                    // 3. Xóa phòng
                    roomToSurrender.Status = "FINISHED";
                    RemoveRoom(roomToSurrender);
                }
            }
            else
            {
                player.Send(new { Type = MessageType.ERROR, Payload = "Cannot surrender: not in a playing room." });
            }

            // BroadcastRoomList() sẽ được gọi trong RemoveRoom(roomToSurrender);
        }

        public void RemoveRoom(Room room)
        {
            if (room == null) return;
            lock (roomLock)
            {
                if (rooms.ContainsKey(room.Id))
                {
                    rooms.Remove(room.Id);
                    Server.Log($"Room {room.Id} removed.");
                }
            }
            BroadcastRoomList();
        }

        public void HandleClientDisconnected(ClientConnection client)
        {
            if (client == null) return;

            Room roomToRemove = null;

            lock (roomLock)
            {
                foreach (var room in rooms.Values)
                {
                    lock (room.Lock)
                    {
                        if (room.Player1 == client || room.Player2 == client)
                        {
                            roomToRemove = room;
                            break; // Chỉ có thể ở trong 1 phòng
                        }
                    }
                }

                if (roomToRemove != null)
                {
                    lock (roomToRemove.Lock)
                    {
                        var other = roomToRemove.Player1 == client ? roomToRemove.Player2 : roomToRemove.Player1;

                        ClientConnection winner = other;
                        ClientConnection loser = client;

                        // 1. Cập nhật Rank & Match History (Chỉ khi trận đấu đang diễn ra và có đối thủ)
                        if (roomToRemove.Status == "PLAYING" && winner != null)
                        {
                            RoomManager.HandleGameEnd(roomToRemove, winner, loser, "DISCONNECT");
                        }

                        // 2. Thông báo cho người chơi còn lại (nếu có)
                        if (other != null)
                        {
                            other.Send(new
                            {
                                Type = MessageType.GAME_END,
                                Payload = JsonHelper.Serialize(new
                                {
                                    roomId = roomToRemove.Id,
                                    opponentLeft = true, // Thông báo đối thủ đã rời đi
                                    winner = other.Username,
                                    endReason = "DISCONNECT"
                                })
                            });
                        }

                        // 3. Xóa phòng
                        roomToRemove.Status = "FINISHED";
                        rooms.Remove(roomToRemove.Id);
                        Server.Log($"Room {roomToRemove.Id} closed due to player disconnection.");
                    }
                }
            }

            // Sau khi xử lý phòng, cập nhật danh sách phòng cho tất cả mọi người
            BroadcastRoomList();
        }

        public void ChatInRoom(ClientConnection player, string payload)
        {
            if (player == null) return;

            var data = JsonHelper.Deserialize<dynamic>(payload);
            int roomId;
            string text = data.text;

            try
            {
                roomId = Convert.ToInt32(data.roomId);
            }
            catch
            {
                player.Send(new { Type = MessageType.ERROR, Payload = "Invalid Room ID for chat." });
                return;
            }

            var room = GetRoom(roomId);
            if (room == null)
            {
                player.Send(new { Type = MessageType.ERROR, Payload = "Room not found for chat." });
                return;
            }

            var packet = new
            {
                Type = MessageType.CHAT_RECV,
                Payload = JsonHelper.Serialize(new
                {
                    from = player.Username,
                    text
                })
            };

            // Gửi cho cả Player1 và Player2
            room.Player1?.Send(packet);
            room.Player2?.Send(packet);
        }

        public Room GetRoom(int id)
        {
            lock (roomLock)
                return rooms.ContainsKey(id) ? rooms[id] : null;
        }

        public void SendRoomList(ClientConnection client)
        {
            var list = GetRoomListJson();
            client.Send(new { Type = MessageType.ROOM_UPDATE, Payload = list });
        }

        private string GetRoomListJson()
        {
            var list = new List<object>();

            lock (roomLock)
            {
                foreach (var r in rooms.Values)
                {
                    // Chỉ hiển thị các phòng Đang Chờ hoặc Đang Chơi
                    if (r.Status == "WAITING" || r.Status == "PLAYING")
                    {
                        list.Add(new
                        {
                            id = r.Id,
                            p1 = r.Player1?.Username,
                            p2 = r.Player2?.Username,
                            status = r.Status
                        });
                    }
                }
            }

            return JsonHelper.Serialize(list);
        }


        public void BroadcastRoomList()
        {
            var listJson = GetRoomListJson();

            var msg = new
            {
                Type = MessageType.ROOM_UPDATE,
                Payload = listJson
            };

            // Chỉ gửi cho những client không ở trong phòng (hoặc gửi cho tất cả, tùy logic)
            // Ở đây ta dùng Server.Broadcast để gửi cho tất cả
            Server.Broadcast(msg);
        }

        // Đã chuyển thành static để GameCore.ProcessMove có thể gọi.
        public static void HandleGameEnd(Room room, ClientConnection winner, ClientConnection loser, string endReason)
        {
            // Trận đấu là Hòa hoặc chỉ có 1 người chơi
            if (endReason == "DRAW_BY_FULL_BOARD" || winner == null)
            {
                winner = null;
                loser = null; // Cả hai đều không thắng/thua
            }

            // Cập nhật Rank và Match History
            Task.Run(async () =>
            {
                // Trận đấu là Hòa hoặc có Winner/Loser rõ ràng
                if (winner != null && loser != null)
                {
                    // **TÍNH ĐIỂM RANK** (Ví dụ đơn giản: +10 cho Thắng, -10 cho Thua)
                    const int RANK_CHANGE = 10;

                    Services.Database.UpdateUserStats(winner.Username, RANK_CHANGE, true);
                    Services.Database.UpdateUserStats(loser.Username, -RANK_CHANGE, false);

                    // **LƯU MATCH HISTORY**
                    await Services.Database.SaveMatch(
                        room.Player1.Username,
                        room.Player2.Username,
                        winner.Username,
                        endReason
                    );
                }
                // Trận đấu là Hòa (Winner/Loser = null)
                else if (endReason == "DRAW_BY_FULL_BOARD")
                {
                    // Cập nhật Rank (tùy chọn: 0 điểm, hoặc tính Elo nhẹ)

                    // **LƯU MATCH HISTORY** (Winner là null)
                    await Services.Database.SaveMatch(
                        room.Player1.Username,
                        room.Player2.Username,
                        null,
                        endReason
                    );
                }
                // Trường hợp người chơi còn lại không có (ví dụ: tạo phòng rồi người tạo rời luôn) - không cần lưu match
            });
        }
    }


    // ================================
    // GAME LOGIC (WIN CHECK)
    // ================================
    public static class GameLogic
    {
        // Kiểm tra 5 quân cờ liên tiếp
        public static bool CheckWin(int[,] board, int x, int y)
        {
            int n = board.GetLength(0);
            int target = board[x, y];
            if (target == 0) return false;

            // 4 hướng: Ngang, Dọc, Chéo chính, Chéo phụ
            var dirs = new (int dx, int dy)[]
            {
                (1,0), (0,1), (1,1), (1,-1)
            };

            foreach (var (dx, dy) in dirs)
            {
                int count = 1;

                // Kiểm tra về phía +dx, +dy
                int cx = x + dx, cy = y + dy;
                while (cx >= 0 && cx < n && cy >= 0 && cy < n && board[cx, cy] == target)
                {
                    count++; cx += dx; cy += dy;
                }

                // Kiểm tra về phía -dx, -dy
                cx = x - dx; cy = y - dy;
                while (cx >= 0 && cx < n && cy >= 0 && cy < n && board[cx, cy] == target)
                {
                    count++; cx -= dx; cy -= dy;
                }

                if (count >= 5) return true;
            }

            return false;
        }
    }
}
