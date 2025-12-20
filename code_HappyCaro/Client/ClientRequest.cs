using Client;
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
                Payload = JsonHelper.Serialize(payload) 
            });
        }

        // AUTH ==================================================
        public void Login(string username, string password)
            => Send(MessageType.AUTH_LOGIN, new { username, password });

        public void Register(string username, string password, string email)
            => Send(MessageType.AUTH_REGISTER, new { username, password, email });

        public void ForgotPassword(string email)
            => Send(MessageType.AUTH_RESET_REQUEST, new { email });

        public void VerifyReset(string email, string token, string newPassword)
            => Send(MessageType.AUTH_RESET_VERIFY,
                new { email, token, newPassword });

        // AUTH - VERIFY OTP ONLY
        public void CheckOTPOnly(string email, string otp)
    => Send(MessageType.AUTH_OTP_VERIFY, new { Email = email, Otp = otp });

        public void Logout()
            => Send(MessageType.AUTH_LOGOUT, new { });

        // ROOM ==================================================
        public void CreateRoom()
            => Send(MessageType.ROOM_CREATE, new { });

        public void JoinRoom(int roomId)
            => Send(MessageType.ROOM_JOIN, roomId);

        public void LeaveRoom()
            => Send(MessageType.ROOM_LEAVE, new { });

        public void RequestRoomList()
            => Send(MessageType.ROOM_LIST, new { });

        // GAME ==================================================
        public void SendMove(int roomId, int x, int y)
            => Send(MessageType.GAME_MOVE, new { roomId, x, y });

        public void Surrender()
            => Send(MessageType.GAME_SURRENDER, new { });

        // CHAT ==================================================
        public void Chat(int roomId, string text)
            => Send(MessageType.CHAT_SEND, new { roomId, text });

        // Gửi yêu cầu kết bạn với username
        public void AddFriend(string friendName)
            => Send(MessageType.FRIEND_ADD, new { friendName });

        // Yêu cầu lấy danh sách bạn bè (nếu cần gọi thủ công)
        public void RequestFriendList()
            => Send(MessageType.FRIEND_LIST, new { });

        // RANK ==================================================
        public void RequestRanking()
            => Send(MessageType.REQUEST_RANKING, new { });
    }
}
