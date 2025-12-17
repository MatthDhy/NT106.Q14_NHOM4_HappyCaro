using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServerCore.ServerCore
{
    public enum MessageType
    {
        // AUTH
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

        // ROOMS
        ROOM_CREATE,
        ROOM_CREATE_OK,
        ROOM_CREATE_FAIL,
        ROOM_JOIN,
        ROOM_JOIN_OK,
        ROOM_JOIN_FAIL,
        ROOM_LEAVE,
        ROOM_LEAVE_OK,
        ROOM_LIST,
        ROOM_UPDATE,

        // GAME
        GAME_MOVE,
        GAME_UPDATE,
        GAME_END,
        GAME_SURRENDER,
        GAME_SURRENDER_OK,
        GAME_SURRENDER_RECV,
        REQUEST_RANKING,
        RANKING_DATA,

        // CHAT
        CHAT_SEND,
        CHAT_RECV,

        // HEARTBEAT
        PING,
        PONG,

        // GENERAL
        ERROR
    }

    public class MessageEnvelope
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageType Type { get; set; }
        // Payload is always a JSON string (may be "null" or "{}")
        public string Payload { get; set; }
    }

    public class MovePayload
    {
        public int roomId { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class AuthRequestPayload
    {
        public string Email { get; set; }
    }

    public class ResetVerifyPayload
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

    public class RankingItem
    {
        public string username { get; set; }
        public int rank { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int draws { get; set; }
    }

    public static class JsonHelper
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public static string Serialize(object obj) => JsonSerializer.Serialize(obj, Options);
        public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);
        public static object Deserialize(string json, Type type) => JsonSerializer.Deserialize(json, type, Options);
    }


}
