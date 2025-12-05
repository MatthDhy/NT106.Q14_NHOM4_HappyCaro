using Server;
using ServerCore.ServerCore;
using System;

namespace ServerCore
{
    public static class AuthHandlers
    {
        // =============================
        // HANDLE LOGIN
        // =============================
        public static void HandleLogin(ClientConnection client, string payload)
        {
            try
            {
                var data = JsonHelper.Deserialize<dynamic>(payload);

                string username = Convert.ToString(data.username);
                string password = Convert.ToString(data.password);

                // Kiểm tra trong database
                var user = Services.Database.GetUser(username);

                if (user == null)
                {
                    client.Send(new
                    {
                        Type = MessageType.AUTH_LOGIN_FAIL,
                        Payload = new { ok = false, reason = "UserNotFound" }
                    });
                    return;
                }

                if (user.Password != password)
                {
                    client.Send(new
                    {
                        Type = MessageType.AUTH_LOGIN_FAIL,
                        Payload = new { ok = false, reason = "WrongPassword" }
                    });
                    return;
                }

                // Lưu username vào ClientConnection
                client.Username = username;

                client.Send(new
                {
                    Type = MessageType.AUTH_LOGIN_OK,
                    Payload = new
                    {
                        ok = true,
                        username = username,
                        rank = user.Rank,
                        wins = user.Wins,
                        losses = user.Losses
                    }
                });

                Server.Log($"User LOGIN: {username}");
                Server.OnClientListChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Server.Log($"Login Error: {ex.Message}");
            }
        }

        // =============================
        // HANDLE REGISTER
        // =============================
        public static void HandleRegister(ClientConnection client, string payload)
        {
            try
            {
                var data = JsonHelper.Deserialize<dynamic>(payload);

                string username = Convert.ToString(data.username);
                string password = Convert.ToString(data.password);

                if (Services.Database.GetUser(username) != null)
                {
                    client.Send(new
                    {
                        Type = MessageType.AUTH_REGISTER_FAIL,
                        Payload = new { ok = false, reason = "UsernameExists" }
                    });
                    return;
                }

                bool ok = Services.Database.Register(username, password);

                client.Send(new
                {
                    Type = MessageType.AUTH_REGISTER_OK,
                    Payload = new { ok = ok }
                });

                Server.Log($"User REGISTER: {username}");
            }
            catch (Exception ex)
            {
                Server.Log($"Register Error: {ex.Message}");
            }
        }

        // =============================
        // HANDLE LOGOUT
        // =============================
        public static void HandleLogout(ClientConnection client)
        {
            string name = client.Username;

            client.Username = "Unknown";

            client.Send(new
            {
                Type = MessageType.AUTH_LOGOUT_OK,
                Payload = new { ok = true }
            });

            Server.Log($"User LOGOUT: {name}");
            Server.OnClientListChanged?.Invoke();
        }
    }
}
