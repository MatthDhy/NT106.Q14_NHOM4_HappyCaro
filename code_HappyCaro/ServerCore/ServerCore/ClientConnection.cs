using ServerCore.ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class ClientConnection
    {
        private TcpClient client;
        private NetworkStream stream;
        private readonly object sendLock = new object();
        public static List<ClientConnection> AllClients = new List<ClientConnection>();


        public string Username = "Unknown";
        public bool Connected => client?.Connected ?? false;

        public ClientConnection(TcpClient client)
        {
            lock (AllClients)
                AllClients.Add(this);
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            stream = client.GetStream();

            // start listening without waiting
            Listen();
        }
        private async Task Listen()
        {
            byte[] lengthBuffer = new byte[4];

            try
            {
                while (Connected)
                {
                    // 1. Đọc 4 bytes độ dài (Length Header)
                    int totalBytesRead = 0;
                    while (totalBytesRead < 4 && Connected)
                    {
                        int bytesRead = await stream.ReadAsync(lengthBuffer, totalBytesRead, 4 - totalBytesRead);
                        if (bytesRead == 0) throw new SocketException(); // Disconnected
                        totalBytesRead += bytesRead;
                    }

                    int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                    if (messageLength <= 0) continue; // Invalid length or empty message

                    // 2. Đọc Body dựa trên độ dài đã biết
                    byte[] messageBuffer = new byte[messageLength];
                    totalBytesRead = 0;

                    while (totalBytesRead < messageLength && Connected)
                    {
                        int bytesRead = await stream.ReadAsync(messageBuffer, totalBytesRead, messageLength - totalBytesRead);
                        if (bytesRead == 0) throw new SocketException(); // Disconnected
                        totalBytesRead += bytesRead;
                    }

                    string json = Encoding.UTF8.GetString(messageBuffer, 0, messageLength);

                    MessageEnvelope msg = null;
                    try
                    {
                        msg = JsonHelper.Deserialize<MessageEnvelope>(json);
                    }
                    catch (Exception ex)
                    {
                        Server.Log($"Deserialize Error for {Username}: {ex.Message} - Data: {json}");
                        // Tiếp tục vòng lặp để tránh ngắt kết nối do 1 tin nhắn lỗi
                        continue;
                    }

                    if (msg != null)
                    {
                        HandleMessage(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Server.Log($"Listen error for {Username}: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        private async void HandleMessage(MessageEnvelope msg)
        {
            if (msg == null)
            {
                Server.Log("Received NULL message.");
                return;
            }

            try
            {
                switch (msg.Type)
                {
                    // ========================================
                    // 1. AUTH MESSAGES
                    // ========================================
                    case MessageType.AUTH_LOGIN:
                        AuthHandlers.HandleLogin(this, msg.Payload);
                        break;

                    case MessageType.AUTH_REGISTER:
                        AuthHandlers.HandleRegister(this, msg.Payload);
                        break;

                    case MessageType.AUTH_LOGOUT:
                        AuthHandlers.HandleLogout(this);
                        break;

                    case MessageType.AUTH_RESET_REQUEST:
                        var resetRequestPayload = JsonHelper.Deserialize<AuthRequestPayload>(msg.Payload);
                        string requestEmail = resetRequestPayload?.Email;

                        if (string.IsNullOrEmpty(requestEmail))
                        {
                            Send(new { Type = MessageType.AUTH_RESET_FAIL, Payload = "Invalid email provided." });
                            return;
                        }

                        // Gọi AuthHandler để xử lý việc gửi email.
                        bool emailSuccess = await Services.Auth.HandleForgotPassword(requestEmail);

                        // Phản hồi mơ hồ để bảo mật
                        string requestResponseMsg = "If your email is in our system, a password reset code has been sent.";

                        Send(new
                        {
                            Type = emailSuccess ? MessageType.AUTH_RESET_OK : MessageType.AUTH_RESET_FAIL,
                            Payload = requestResponseMsg
                        });
                        break;

                    case MessageType.AUTH_RESET_VERIFY:
                        var verifyPayload = JsonHelper.Deserialize<ResetVerifyPayload>(msg.Payload);

                        if (verifyPayload == null || string.IsNullOrEmpty(verifyPayload.Email) ||
                            string.IsNullOrEmpty(verifyPayload.Token) || string.IsNullOrEmpty(verifyPayload.NewPassword))
                        {
                            Send(new { Type = MessageType.AUTH_RESET_VERIFY_FAIL, Payload = "Missing fields." });
                            return;
                        }

                        // Gọi AuthHandler để xác minh mã và reset
                        bool resetSuccess = Services.Auth.HandleResetVerification(
                            verifyPayload.Email,
                            verifyPayload.Token,
                            verifyPayload.NewPassword
                        );

                        if (resetSuccess)
                        {
                            Send(new { Type = MessageType.AUTH_RESET_VERIFY_OK, Payload = "Password successfully reset. You can now login." });
                        }
                        else
                        {
                            Send(new { Type = MessageType.AUTH_RESET_VERIFY_FAIL, Payload = "Verification failed. Token invalid, expired, or used." });
                        }

                        break;

                    // ========================================
                    // 2. ROOM SYSTEM
                    // ========================================
                    case MessageType.ROOM_CREATE:
                        if (Username == "Unknown")
                        {
                            Send(new { Type = MessageType.ERROR, Payload = "You must login first" });
                            return;
                        }
                        GameCore.RoomManager.CreateRoom(this);
                        break;

                    case MessageType.ROOM_JOIN:
                        if (Username == "Unknown") { Send(new { Type = MessageType.ERROR, Payload = "You must login first" }); return; }
                        try { GameCore.RoomManager.JoinRoom(this, msg.Payload); }
                        catch { Send(new { Type = MessageType.ROOM_JOIN_FAIL, Payload = "Invalid Room ID." }); }
                        break;

                    case MessageType.ROOM_LEAVE:
                        GameCore.RoomManager.LeaveRoom(this);
                        break;

                    case MessageType.ROOM_LIST:
                        GameCore.RoomManager.BroadcastRoomList();
                        break;


                    // ========================================
                    // 3. IN_MATCH OPERATIONS
                    // ========================================
                    case MessageType.GAME_MOVE:
                        GameCore.ProcessMove(this, msg.Payload);
                        break;

                    case MessageType.GAME_SURRENDER:
                        GameCore.RoomManager.Surrender(this);
                        break;

                    case MessageType.CHAT_SEND:
                        GameCore.RoomManager.ChatInRoom(this, msg.Payload);
                        break;


                    // ========================================
                    // 4. HEARTBEAT
                    // ========================================
                    case MessageType.PING:
                        Send(new { Type = MessageType.PONG });
                        break;


                    // ========================================
                    // 5. UNKNOWN
                    // ========================================
                    default:
                        Server.Log($"[WARNING] Unknown message type: {msg.Type}");
                        Send(new
                        {
                            Type = MessageType.ERROR,
                            Payload = $"Unknown message type: {msg.Type}"
                        });
                        break;
                }
            }
            catch (Exception ex)
            {
                Server.Log($"Error handling message {msg.Type}: {ex.Message}");

                Send(new
                {
                    Type = MessageType.ERROR,
                    Payload = "Internal server error while processing message."
                });
            }
        }

        public void Send(object data)
        {
            if (!Connected) return; // Nếu đã ngắt kết nối thì không gửi nữa

            string json = JsonHelper.Serialize(data);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            int length = jsonBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);

            byte[] finalBytes = new byte[lengthBytes.Length + jsonBytes.Length];
            Buffer.BlockCopy(lengthBytes, 0, finalBytes, 0, lengthBytes.Length);
            Buffer.BlockCopy(jsonBytes, 0, finalBytes, lengthBytes.Length, jsonBytes.Length);

            try
            {
                lock (sendLock)
                {
                    // Sử dụng Write thay vì WriteAsync vì hàm này đang nằm trong lock
                    stream.Write(finalBytes, 0, finalBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Server.Log($"Send Error for {Username}: {ex.Message}");
                // Disconnect ngay khi gửi lỗi
                Disconnect();
            }
        }
        private void Disconnect()
        {
            // Đảm bảo chỉ Disconnect 1 lần
            if (!Connected) return;

            lock (AllClients)
                AllClients.Remove(this);

            // Xử lý game trước khi đóng socket hoàn toàn
            GameCore.RoomManager.HandleClientDisconnected(this);

            try { client.Close(); } catch { }
            try { stream?.Close(); } catch { }

            Server.OnClientListChanged?.Invoke();
            Server.Log($"Client disconnected: {Username}");
        }
    }
}
