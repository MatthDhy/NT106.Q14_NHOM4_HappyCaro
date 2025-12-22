using System;
using System.Text.Json;

namespace Client
{
    public class ClientDispatcher
    {
        private readonly TcpClientHelper _tcp;

        public ClientDispatcher(TcpClientHelper tcp)
        {
            _tcp = tcp;
            _tcp.OnEnvelopeReceived += Dispatch;
        }

        public TcpClientHelper Tcp => _tcp;

        private void Dispatch(MessageEnvelope env)
        {
            switch (env.Type)
            {
                // AUTH ###################################################
                case MessageType.AUTH_LOGIN_OK:
                    OnLoginSuccess?.Invoke(env.Payload);
                    break;

                case MessageType.AUTH_LOGIN_FAIL:
                    OnLoginFail?.Invoke(env.Payload);
                    break;

                case MessageType.AUTH_REGISTER_OK:
                    OnRegisterSuccess?.Invoke();
                    break;

                case MessageType.AUTH_REGISTER_FAIL:
                    OnRegisterFail?.Invoke(env.Payload);
                    break;
                


                case MessageType.AUTH_RESET_OK:
                    OnResetRequested?.Invoke();
                    break;

                case MessageType.AUTH_RESET_VERIFY_OK:
                    OnResetSuccess?.Invoke();
                    break;

                case MessageType.AUTH_RESET_VERIFY_FAIL:
                    OnResetFail?.Invoke();
                    break;
                
                case MessageType.AUTH_OTP_VERIFY_OK:
                    OnVerifyOTPSuccess?.Invoke(env.Payload);
                    break;

                case MessageType.AUTH_OTP_VERIFY_FAIL:
                    OnVerifyOTPFail?.Invoke(env.Payload);
                    break;


                // ROOM ###################################################
                case MessageType.ROOM_LIST:
                    OnRoomList?.Invoke(env.Payload);
                    break;

                case MessageType.ROOM_CREATE_OK:
                    OnRoomCreated?.Invoke(env.Payload);
                    break;

                case MessageType.ROOM_JOIN_OK:
                    OnRoomJoined?.Invoke(env.Payload);
                    break;

                case MessageType.ROOM_JOIN_FAIL:
                    OnRoomJoinFail?.Invoke(env.Payload);
                    break;

                case MessageType.ROOM_UPDATE:
                    OnRoomUpdate?.Invoke(env.Payload);
                    break;

                // GAME ###################################################
                case MessageType.GAME_UPDATE:
                    OnGameUpdate?.Invoke(env.Payload);
                    break;

                case MessageType.GAME_END:
                    OnGameEnd?.Invoke(env.Payload);
                    break;

                case MessageType.GAME_SURRENDER_RECV:
                    OnOpponentSurrender?.Invoke();
                    break;

                // CHAT ###################################################
                case MessageType.CHAT_RECV:
                    OnChatReceived?.Invoke(env.Payload);
                    break;

                case MessageType.FRIEND_ACTION_RESULT:
                    {
                        var data = JsonHelper.Deserialize<JsonElement>(env.Payload);
                        bool success = data.GetProperty("success").GetBoolean();
                        string message = data.GetProperty("message").GetString();
                        OnFriendActionResult?.Invoke(success, message);
                        break;
                    }

                case MessageType.FRIEND_REQUEST_LIST:
                    OnFriendRequestList?.Invoke(env.Payload);
                    break;

                case MessageType.FRIEND_LIST:
                    OnFriendList?.Invoke(env.Payload);
                    break;



                // RANK ##################################################
                case MessageType.RANKING_DATA:
                    OnRankingReceived?.Invoke(env.Payload);
                    break;

                // ERROR #################################################
                case MessageType.ERROR:
                    OnError?.Invoke(env.Payload);
                    break;
                    default:
            // Nếu server trả về một Type lạ hoặc Type lỗi mà bạn chưa handle
            Console.WriteLine($"[DEBUG] Nhận tin nhắn chưa xác định: {env.Type} - Payload: {env.Payload}");
            break;
            }
        }

        // AUTH EVENTS
        public event Action<string> OnLoginSuccess;
        public event Action<string> OnLoginFail;
        public event Action OnRegisterSuccess;
        public event Action<string> OnRegisterFail;
        public event Action OnResetRequested;
        public event Action OnResetSuccess;
        public event Action OnResetFail;

        // OTP EVENTS
        public event Action<string> OnVerifyOTPSuccess; 
        public event Action<string> OnVerifyOTPFail;
        // FRIEND
        public event Action<string> OnFriendList;
        public event Action<string> OnFriendRequestList;
        public event Action<bool, string> OnFriendActionResult;



        // ROOM EVENTS
        public event Action<string> OnRoomList;
        public event Action<string> OnRoomCreated;
        public event Action<string> OnRoomJoined;
        public event Action<string> OnRoomJoinFail;
        public event Action<string> OnRoomUpdate;

        // GAME EVENTS
        public event Action<string> OnGameUpdate;
        public event Action<string> OnGameEnd;
        public event Action OnOpponentSurrender;

        // CHAT
        public event Action<string> OnChatReceived;
        public event Action<bool, string> OnAddFriendResult;

        // RANKING
        public event Action<string> OnRankingReceived;

        // ERROR
        public event Action<string> OnError;
    }
}
