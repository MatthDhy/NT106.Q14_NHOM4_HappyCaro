using System.Text.Json;

namespace Client
{
    public class ClientRequest
    {
        private readonly TcpClientHelper _tcp;

        public ClientRequest(TcpClientHelper tcp)
        {
            _tcp = tcp;
        }

        private void Send(MessageType type, object payload)
        {
            _tcp.EnqueueSend(new MessageEnvelope
            {
                Type = type,
                Payload = JsonSerializer.Serialize(payload)
            });
        }

        // AUTH ==============================================================
        public void Login(string username, string password)
            => Send(MessageType.AUTH_LOGIN, new { username, password });

        public void Register(string username, string password)
            => Send(MessageType.AUTH_REGISTER, new { username, password });

        public void ForgotPassword(string email)
            => Send(MessageType.AUTH_RESET_REQUEST, new { Email = email });

        public void VerifyReset(string email, string token, string newPassword)
            => Send(MessageType.AUTH_RESET_VERIFY, new { Email = email, Token = token, NewPassword = newPassword });

        public void Logout()
            => Send(MessageType.AUTH_LOGOUT, new { });

        // ROOM ==============================================================
        public void CreateRoom()
            => Send(MessageType.ROOM_CREATE, new { });

        public void JoinRoom(int roomId)
            => Send(MessageType.ROOM_JOIN, new { RoomId = roomId });

        public void LeaveRoom()
            => Send(MessageType.ROOM_LEAVE, new { });

        public void RequestRoomList()
            => Send(MessageType.ROOM_LIST, new { });

        // GAME ==============================================================
        public void SendMove(int x, int y)
            => Send(MessageType.GAME_MOVE, new { x, y });

        public void Surrender()
            => Send(MessageType.GAME_SURRENDER, new { });

        // CHAT ==============================================================
        public void Chat(string message)
            => Send(MessageType.CHAT_SEND, new { message });

        // RANKING ============================================================
        public void RequestRanking()
            => Send(MessageType.REQUEST_RANKING, new { });
    }
}
