using System.Text.Json;
using System.Text.Json.Serialization;

namespace Client
{
    public enum MessageType
    {
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
        GAME_MOVE,
        GAME_UPDATE,
        GAME_END,
        GAME_SURRENDER,
        GAME_SURRENDER_OK,
        GAME_SURRENDER_RECV,
        REQUEST_RANKING,
        RANKING_DATA,
        CHAT_SEND,
        CHAT_RECV,
        PING,
        PONG,
        ERROR
    }

    public class MessageEnvelope
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageType Type { get; set; }
        public string Payload { get; set; }
    }

    public class RankingItem
    {
        public string username { get; set; }
        public int rank { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int draws { get; set; }
    }
    public class UserInfo
    {
        public string Username { get; set; }
        public int RankPoint { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
    }
    public static class JsonHelper
    {
        public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public static string Serialize(object obj) => JsonSerializer.Serialize(obj, Options);
        public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);
    }

    public enum NetworkMode
    {
        Local, // Client & Server same machine
        Lan, // Auto-discovery via UDP
        LanWithFallback,
        Internet // Manual IP / domain
    }
}
