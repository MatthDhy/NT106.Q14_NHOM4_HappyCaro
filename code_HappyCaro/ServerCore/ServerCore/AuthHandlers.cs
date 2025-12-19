using System;

namespace ServerCore.ServerCore
{
    public static class AuthHandlers
    {
        public static void HandleLogin(ClientConnection client, string payloadJson)
        {
            try
            {
                var data = JsonHelper.Deserialize<dynamic>(payloadJson);
                string username = data?.GetProperty("username").GetString();
                string password = data?.GetProperty("password").GetString();

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    client.SendEnvelope(MessageType.AUTH_LOGIN_FAIL, JsonHelper.Serialize(new { ok = false, reason = "MissingFields" }));
                    return;
                }

                var user = Services.Database.GetUser(username);
                if (user == null)
                {
                    client.SendEnvelope(MessageType.AUTH_LOGIN_FAIL, JsonHelper.Serialize(new { ok = false, reason = "UserNotFound" }));
                    return;
                }

                string hash = Services.Auth.HashPassword(password);
                if (!string.Equals(hash, user.PasswordHash, StringComparison.Ordinal))
                {
                    client.SendEnvelope(MessageType.AUTH_LOGIN_FAIL, JsonHelper.Serialize(new { ok = false, reason = "WrongPassword" }));
                    return;
                }

                client.Username = username;
                client.SendEnvelope(MessageType.AUTH_LOGIN_OK, JsonHelper.Serialize(new { ok = true, username = username, rank = user.RankPoint, wins = user.Wins, losses = user.Losses, draws = user.Draws }));
                Server.Log($"User LOGIN: {username}");
                Server.OnClientListChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Server.Log($"Login Error: {ex.Message}");
                client.SendEnvelope(MessageType.AUTH_LOGIN_FAIL, JsonHelper.Serialize(new { ok = false, reason = "ServerError" }));
            }
        }

        public static void HandleRegister(ClientConnection client, string payloadJson)
        {
            try
            {
                var data = JsonHelper.Deserialize<dynamic>(payloadJson);
                string username = data?.GetProperty("username").GetString();
                string password = data?.GetProperty("password").GetString();
                string email = data?.GetProperty("email").GetString();


                if (string.IsNullOrEmpty(username) ||
                    string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(email))
                {
                    client.SendEnvelope(
                        MessageType.AUTH_REGISTER_FAIL,
                        JsonHelper.Serialize(new { ok = false, reason = "MissingFields" })
                    );
                    return;
                }

                if (Services.Database.GetUser(username) != null)
                {
                    client.SendEnvelope(MessageType.AUTH_REGISTER_FAIL, JsonHelper.Serialize(new { ok = false, reason = "UsernameExists" }));
                    return;
                }

                string hash = Services.Auth.HashPassword(password);
                bool ok = Services.Database.Register(username, hash, email);

                client.SendEnvelope(ok ? MessageType.AUTH_REGISTER_OK : MessageType.AUTH_REGISTER_FAIL, JsonHelper.Serialize(new { ok }));
                Server.Log($"User REGISTER: {username}");
            }
            catch (Exception ex)
            {
                Server.Log($"Register Error: {ex.Message}");
                client.SendEnvelope(
        MessageType.AUTH_REGISTER_FAIL,
        JsonHelper.Serialize(new { ok = false, reason = ex.Message })
    );
            }
        }

        public static void HandleLogout(ClientConnection client)
        {
            string name = client.Username;
            client.Username = "Unknown";
            client.SendEnvelope(MessageType.AUTH_LOGOUT_OK, JsonHelper.Serialize(new { ok = true }));
            Server.Log($"User LOGOUT: {name}");
            Server.OnClientListChanged?.Invoke();
        }
    }
}
