using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore.ServerCore
{
    public class ClientConnection
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly object _sendLock = new object();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public static List<ClientConnection> AllClients = Server.Clients;

        public string Username { get; set; } = "Unknown";
        public bool Connected => _client?.Connected ?? false;

        public ClientConnection(TcpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _stream = _client.GetStream();

            lock (AllClients) AllClients.Add(this);

            _ = Task.Run(() => ReceiveLoopAsync(_cts.Token));
        }

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                var lenBuf = new byte[4];
                while (!token.IsCancellationRequested && Connected)
                {
                    // Read 4-byte length
                    int read = 0;
                    while (read < 4)
                    {
                        int r = await _stream.ReadAsync(lenBuf, read, 4 - read, token);
                        if (r == 0) throw new IOException("Remote closed");
                        read += r;
                    }

                    int messageLength = BitConverter.ToInt32(lenBuf, 0);
                    if (messageLength <= 0) continue;

                    var buffer = new byte[messageLength];
                    int total = 0;
                    while (total < messageLength)
                    {
                        int r = await _stream.ReadAsync(buffer, total, messageLength - total, token);
                        if (r == 0) throw new IOException("Remote closed");
                        total += r;
                    }

                    string json = Encoding.UTF8.GetString(buffer, 0, messageLength);

                    MessageEnvelope env;
                    try
                    {
                        env = JsonHelper.Deserialize<MessageEnvelope>(json);
                    }
                    catch (Exception ex)
                    {
                        Server.Log($"Deserialize error from {Username}: {ex.Message} -- data: {json}");
                        // notify client error
                        SendError("Malformed message");
                        continue;
                    }

                    _ = Task.Run(() => HandleMessageAsync(env));
                }
            }
            catch (Exception ex)
            {
                Server.Log($"ReceiveLoop ended for {Username}: {ex.Message}");
            }
            finally
            {
                DisconnectInternal();
            }
        }

        private async Task HandleMessageAsync(MessageEnvelope env)
        {
            try
            {
                switch (env.Type)
                {
                    case MessageType.PING:
                        SendEnvelope(MessageType.PONG, null);
                        break;
                    // AUTH & ROOM & GAME handled by other modules
                    default:
                        // Forward to handlers in ServerCore root (static classes)
                        Server.Log($"Message from {Username}: {env.Type}");
                        HandleByDispatcher(env);
                        break;
                }
            }
            catch (Exception ex)
            {
                Server.Log($"HandleMessage error for {Username}: {ex.Message}");
                SendError("Internal server error");
            }
        }

        private void HandleByDispatcher(MessageEnvelope env)
        {
            // Simple dispatcher - call static handlers
            try
            {
                switch (env.Type)
                {
                    case MessageType.AUTH_LOGIN:
                        AuthHandlers.HandleLogin(this, env.Payload);
                        break;
                    case MessageType.AUTH_REGISTER:
                        AuthHandlers.HandleRegister(this, env.Payload);
                        break;
                    case MessageType.AUTH_LOGOUT:
                        AuthHandlers.HandleLogout(this);
                        break;
                    case MessageType.AUTH_RESET_REQUEST:
                        {
                            var payload = JsonHelper.Deserialize<AuthRequestPayload>(env.Payload);
                            _ = Services.Auth.HandleForgotPasswordAsync(payload.Email)
                                .ContinueWith(t =>
                                {
                                    var respMsg = "If your email exists, a reset code has been sent.";
                                    SendEnvelope(t.Result ? MessageType.AUTH_RESET_OK : MessageType.AUTH_RESET_FAIL, JsonHelper.Serialize(new { message = respMsg }));
                                });
                        }
                        break;
                    case MessageType.AUTH_RESET_VERIFY:
                        {
                            var p = JsonHelper.Deserialize<ResetVerifyPayload>(env.Payload);
                            bool ok = Services.Auth.HandleResetVerification(p.Email, p.Token, p.NewPassword);
                            SendEnvelope(ok ? MessageType.AUTH_RESET_VERIFY_OK : MessageType.AUTH_RESET_VERIFY_FAIL, JsonHelper.Serialize(new { ok }));
                        }
                        break;
                    case MessageType.ROOM_CREATE:
                        GameCore.RoomManager.CreateRoom(this);
                        break;
                    case MessageType.ROOM_JOIN:
                        GameCore.RoomManager.JoinRoom(this, env.Payload);
                        break;
                    case MessageType.ROOM_LEAVE:
                        GameCore.RoomManager.LeaveRoom(this);
                        break;
                    case MessageType.ROOM_LIST:
                        GameCore.RoomManager.SendRoomList(this);
                        break;
                    case MessageType.GAME_MOVE:
                        GameCore.ProcessMove(this, env.Payload);
                        break;
                    case MessageType.GAME_SURRENDER:
                        GameCore.RoomManager.Surrender(this);
                        break;
                    case MessageType.CHAT_SEND:
                        GameCore.RoomManager.ChatInRoom(this, env.Payload);
                        break;
                    case MessageType.REQUEST_RANKING:
                        var list = Services.Database.GetRanking();
                        var pList = list.ConvertAll(u => new RankingItem { username = u.Username, rank = u.RankPoint, wins = u.Wins, losses = u.Losses, draws = u.Draws });
                        SendEnvelope(MessageType.RANKING_DATA, JsonHelper.Serialize(pList));
                        break;
                    default:
                        SendError($"Unhandled message type: {env.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Server.Log($"Dispatcher error: {ex.Message}");
                SendError("Dispatcher error");
            }
        }

        public void SendEnvelope(MessageType type, string payloadJson)
        {
            var env = new MessageEnvelope { Type = type, Payload = payloadJson ?? "" };
            string json = JsonHelper.Serialize(env);
            byte[] body = Encoding.UTF8.GetBytes(json);
            byte[] prefix = BitConverter.GetBytes(body.Length);
            SendRaw(prefix, body);
        }

        public void SendRaw(byte[] prefix, byte[] body)
        {
            if (!Connected) return;
            lock (_sendLock)
            {
                try
                {
                    _stream.Write(prefix, 0, prefix.Length);
                    _stream.Write(body, 0, body.Length);
                }
                catch (Exception ex)
                {
                    Server.Log($"SendRaw error to {Username}: {ex.Message}");
                    DisconnectInternal();
                }
            }
        }

        public void SendError(string message)
        {
            SendEnvelope(MessageType.ERROR, JsonHelper.Serialize(new { message }));
        }

        public void ForceDisconnect()
        {
            try { _cts.Cancel(); } catch { }
            try { _client.Close(); } catch { }
            DisconnectInternal();
        }

        private void DisconnectInternal()
        {
            lock (AllClients)
            {
                if (AllClients.Contains(this)) AllClients.Remove(this);
            }

            // Inform room manager
            GameCore.RoomManager.HandleClientDisconnected(this);

            Server.OnClientListChanged?.Invoke();
            Server.Log($"Client disconnected: {Username}");
        }

        public string EndPoint
        {
            get
            {
                try { return _client.Client.RemoteEndPoint?.ToString() ?? "Unknown"; }
                catch { return "Unknown"; }
            }
        }
    }
}
