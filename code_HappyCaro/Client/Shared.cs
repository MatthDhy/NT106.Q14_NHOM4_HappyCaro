using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Client
{
    public enum MessageType
    {
        // ===============================
        // AUTHENTICATION
        // ===============================
        AUTH_LOGIN,
        AUTH_LOGIN_OK,
        AUTH_LOGIN_FAIL,

        AUTH_REGISTER,
        AUTH_REGISTER_OK,
        AUTH_REGISTER_FAIL,

        AUTH_LOGOUT,
        AUTH_LOGOUT_OK,

        AUTH_RESET_REQUEST,
        AUTH_RESET_OK,
        AUTH_RESET_FAIL,

        AUTH_RESET_VERIFY,
        AUTH_RESET_VERIFY_OK,
        AUTH_RESET_VERIFY_FAIL,


        // ===============================
        // ROOM SYSTEM
        // ===============================
        ROOM_CREATE,
        ROOM_CREATE_OK,
        ROOM_CREATE_FAIL,

        ROOM_JOIN,
        ROOM_JOIN_OK,
        ROOM_JOIN_FAIL,

        ROOM_LEAVE,
        ROOM_LEAVE_OK,

        ROOM_LIST,          // yêu cầu list phòng
        ROOM_UPDATE,        // cập nhật danh sách phòng (server đẩy xuống client)


        // ===============================
        // GAME SYSTEM
        // ===============================
        GAME_MOVE,          // người chơi đánh 1 nước
        GAME_UPDATE,        // server broadcast cập nhật nước đi
        GAME_END,           // ván cờ kết thúc
        GAME_SURRENDER,     // người chơi đầu hàng
        GAME_SURRENDER_OK,
        GAME_SURRENDER_RECV,


        // ===============================
        // CHAT (in-room chat)
        // ===============================
        CHAT_SEND,
        CHAT_RECV,

        // ===============================
        // HEARTBEAT (PING - PONG)
        // ===============================
        PING,
        PONG,

        // ===============================
        // GENERAL / ERROR
        // ===============================
        ERROR
    }


    public class MessageEnvelope
    {
        public MessageType Type { get; set; }
        public string Payload { get; set; } // JSON string
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RankPoint { get; set; }
    }

    public class Match
    {
        public int Id { get; set; }
        public int Player1 { get; set; }
        public int Player2 { get; set; }
        public int Winner { get; set; }
        public DateTime PlayedAt { get; set; }
    }

    public class Friend
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public string Status { get; set; }
    }

    public static class JsonHelper
    {
        public static string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
