using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;


namespace ServerCore.ServerCore
{
    public static class Services
    {
        // ======================================================
        //  AUTHENTICATION SERVICES
        // ======================================================
        public static class Auth
        {
            // Token + thời gian hết hạn
            private class ResetTokenInfo
            {
                public string Token;
                public DateTime ExpireAt;
            }

            private static Dictionary<string, ResetTokenInfo> _resetTokens = new Dictionary<string, ResetTokenInfo>();


            // ============================
            // HASH PASSWORD (giữ nguyên)
            // ============================
            public static string HashPassword(string password)
            {
                using (var sha = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] hash = sha.ComputeHash(bytes);
                    return BytesToHex(hash);
                }
            }

            private static string BytesToHex(byte[] data)
            {
                StringBuilder sb = new StringBuilder(data.Length * 2);
                for (int i = 0; i < data.Length; i++)
                    sb.Append(data[i].ToString("X2"));
                return sb.ToString();
            }


            // ============================
            // GỬI EMAIL RESET TOKEN
            // ============================
            private static async Task SendResetEmailAsync(string toEmail, string token)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    "HappyCaro Support",
                    "huygiavirus@gmail.com"
                ));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = "HappyCaro - Reset Password";

                message.Body = new TextPart("plain")
                {
                    Text =
            $@"Hello,

Your password reset code is: {token}

This code will expire in 5 minutes.
If you did not request this, please ignore this email.

HappyCaro Team!"
                };

                using (var client = new SmtpClient())
                {
                    // Gmail SMTP
                    await client.ConnectAsync(
                        "smtp.gmail.com",
                        587,
                        MailKit.Security.SecureSocketOptions.StartTls
                    );

                    await client.AuthenticateAsync(
                        "huygiavirus@gmail.com",
                        "fzsgvqadglzvccoz" // app password (không có dấu cách)
                    );

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }




            // ============================
            // REQUEST FORGOT PASSWORD
            // ============================
            public static async Task<bool> HandleForgotPasswordAsync(string email)
            {
                var user = Database.GetUserByEmail(email);
                if (user == null)
                    return true;

                string token = new Random().Next(100000, 999999).ToString();

                lock (_resetTokens)
                {
                    _resetTokens[email] = new ResetTokenInfo
                    {
                        Token = token,
                        ExpireAt = DateTime.Now.AddMinutes(5)
                    };
                }

                try
                {
                    await SendResetEmailAsync(email, token);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SMTP ERROR: " + ex.Message);
                    return false;
                }
            }

            // ============================
            // VERIFY OTP
            // ============================
            public static bool VerifyOTP(string email, string otp)
            {
                lock (_resetTokens)
                {
                    if (!_resetTokens.ContainsKey(email)) return false;
                    var info = _resetTokens[email];

                    // Kiểm tra mã và thời gian
                    if (info.Token != otp || DateTime.Now > info.ExpireAt)
                    {
                        if (DateTime.Now > info.ExpireAt) _resetTokens.Remove(email);
                        return false;
                    }
                    return true;
                }
            }

            // ============================
            // VERIFY TOKEN + RESET PASSWORD
            // ============================
            public static bool HandleResetVerification(string email, string token, string newPassword)
            {
                ResetTokenInfo info = null;

                lock (_resetTokens)
                {
                    if (!_resetTokens.ContainsKey(email))
                        return false;

                    info = _resetTokens[email];
                }

                // 1) Kiểm tra token đúng
                if (info.Token != token)
                    return false;

                // 2) Kiểm tra hết hạn
                if (DateTime.Now > info.ExpireAt)
                {
                    lock (_resetTokens)
                    {
                        _resetTokens.Remove(email);
                    }
                    return false; // expired
                }

                // 3) Token OK → xóa token
                lock (_resetTokens)
                {
                    _resetTokens.Remove(email);
                }

                // 4) Cập nhật mật khẩu
                var user = Database.GetUserByEmail(email);
                if (user == null) return false;

                string hash = HashPassword(newPassword);
                Database.UpdatePassword(user.Username, hash);

                return true;
            }
        }

        // ======================================================
        //  DATABASE ACCESS
        // ======================================================
        public static class Database
        {
            private static readonly string _conn = ConfigurationManager.ConnectionStrings["CaroDb"].ConnectionString;

            public static SqlConnection GetConnection()
            {
                return new SqlConnection(_conn);
            }

            public class DbUser
            {
                public int Id;
                public string Username;
                public string PasswordHash;
                public string Email;
                public int RankPoint;
                public int Wins;
                public int Losses;
                public int Draws;
            }

            // =========================================
            // GET USER BY USERNAME
            // =========================================
            public static DbUser GetUser(string username)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = "SELECT TOP 1 * FROM Users WHERE Username = @u";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@u", username);

                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        return new DbUser
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            Username = r["Username"].ToString(),
                            PasswordHash = r["PasswordHash"].ToString(),
                            Email = r["Email"].ToString(),
                            RankPoint = Convert.ToInt32(r["RankPoint"]),
                            Wins = Convert.ToInt32(r["Wins"]),
                            Losses = Convert.ToInt32(r["Losses"]),
                            Draws = Convert.ToInt32(r["Draws"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:GetUser " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
                return null;
            }

            // =========================================
            // GET USER BY EMAIL
            // =========================================
            public static DbUser GetUserByEmail(string email)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = "SELECT TOP 1 * FROM Users WHERE Email = @e";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@e", email);

                    SqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        return new DbUser
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            Username = r["Username"].ToString(),
                            PasswordHash = r["PasswordHash"].ToString(),
                            Email = r["Email"].ToString(),
                            RankPoint = Convert.ToInt32(r["RankPoint"]),
                            Wins = Convert.ToInt32(r["Wins"]),
                            Losses = Convert.ToInt32(r["Losses"]),
                            Draws = Convert.ToInt32(r["Draws"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:GetUserByEmail " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
                return null;
            }

            // =========================================
            // REGISTER USER
            // =========================================
            public static bool Register(string username, string passwordHash, string email)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = @"INSERT INTO Users 
                    (Username, PasswordHash, Email, RankPoint, Wins, Losses, Draws) 
                    VALUES (@u, @p, @e, 1000, 0, 0, 0)";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", passwordHash);
                    cmd.Parameters.AddWithValue("@e", email);
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:Register " + ex.Message);
                    return false;
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
            }

            // =========================================
            // UPDATE PASSWORD
            // =========================================
            public static void UpdatePassword(string username, string passwordHash)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = "UPDATE Users SET PasswordHash = @p WHERE Username = @u";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@p", passwordHash);
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:UpdatePassword " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
            }

            // =========================================
            // CREATE MATCH
            // =========================================
            public static int CreateMatch(string playerX, string playerO)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = @"INSERT INTO Matches (RoomId, PlayerXId, PlayerOId, StartedAt)
                                 OUTPUT INSERTED.Id 
                                 VALUES (0, @px, @po, GETDATE())";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@px", GetUserId(playerX));
                    cmd.Parameters.AddWithValue("@po", GetUserId(playerO));

                    object id = cmd.ExecuteScalar();
                    return Convert.ToInt32(id);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:CreateMatch " + ex.Message);
                    return 0;
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
            }

            // =========================================
            // SAVE MOVE
            // =========================================
            public static async Task SaveMove(int matchId, string username, int x, int y, int stepNumber)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    await conn.OpenAsync();

                    string q = @"INSERT INTO Moves (MatchId, PlayerId, X, Y, StepNumber, CreatedAt)
                                 VALUES (@m, @pid, @x, @y, @s, GETDATE())";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@m", matchId);
                    cmd.Parameters.AddWithValue("@pid", GetUserId(username));
                    cmd.Parameters.AddWithValue("@x", x);
                    cmd.Parameters.AddWithValue("@y", y);
                    cmd.Parameters.AddWithValue("@s", stepNumber);

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:SaveMove " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
            }

            // =========================================
            // SAVE MATCH
            // =========================================
            public static async Task SaveMatch(string playerX, string playerO, string winnerUsername, string endReason)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    await conn.OpenAsync();

                    string q = @"INSERT INTO Matches (RoomId, PlayerXId, PlayerOId, WinnerId, EndReason, StartedAt, EndedAt)
                                 VALUES (0, @px, @po, @w, @r, GETDATE(), GETDATE())";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@px", GetUserId(playerX));
                    cmd.Parameters.AddWithValue("@po", GetUserId(playerO));

                    if (string.IsNullOrEmpty(winnerUsername))
                        cmd.Parameters.AddWithValue("@w", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@w", GetUserId(winnerUsername));

                    cmd.Parameters.AddWithValue("@r", endReason ?? "");

                    await cmd.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:SaveMatch " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
            }

            // =========================================
            // UPDATE USER STATS
            // =========================================
            public static void UpdateUserStats(string username, int deltaRank, bool isWinner)
            {
                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = @"UPDATE Users
                                 SET RankPoint = RankPoint + @dr,
                                     Wins = Wins + @w,
                                     Losses = Losses + @l,
                                     Draws = Draws + @d
                                 WHERE Username = @u";

                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@dr", deltaRank);
                    cmd.Parameters.AddWithValue("@w", isWinner ? 1 : 0);
                    cmd.Parameters.AddWithValue("@l", isWinner ? 0 : 1);
                    cmd.Parameters.AddWithValue("@d", 0);
                    cmd.Parameters.AddWithValue("@u", username);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:UpdateUserStats " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
            }

            // =========================================
            // GET RANKING
            // =========================================
            public static List<DbUser> GetRanking()
            {
                List<DbUser> list = new List<DbUser>();
                SqlConnection conn = null;

                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = "SELECT TOP 100 * FROM Users ORDER BY RankPoint DESC, Wins DESC";
                    SqlCommand cmd = new SqlCommand(q, conn);

                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        DbUser u = new DbUser();
                        u.Id = Convert.ToInt32(r["Id"]);
                        u.Username = r["Username"].ToString();
                        u.Email = r["Email"].ToString();
                        u.RankPoint = Convert.ToInt32(r["RankPoint"]);
                        u.Wins = Convert.ToInt32(r["Wins"]);
                        u.Losses = Convert.ToInt32(r["Losses"]);
                        u.Draws = Convert.ToInt32(r["Draws"]);
                        list.Add(u);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:GetRanking " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }

                return list;
            }

            // =========================================
            // GET TOP RANK X USERS
            // =========================================
            public static List<DbUser> GetTopRank(int limit)
            {
                List<DbUser> list = new List<DbUser>();
                SqlConnection conn = null;

                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = "SELECT TOP (@lim) * FROM Users ORDER BY RankPoint DESC";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@lim", limit);

                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        DbUser u = new DbUser();
                        u.Id = Convert.ToInt32(r["Id"]);
                        u.Username = r["Username"].ToString();
                        u.Email = r["Email"].ToString();
                        u.RankPoint = Convert.ToInt32(r["RankPoint"]);
                        u.Wins = Convert.ToInt32(r["Wins"]);
                        u.Losses = Convert.ToInt32(r["Losses"]);
                        u.Draws = Convert.ToInt32(r["Draws"]);
                        list.Add(u);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:GetTopRank " + ex.Message);
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }

                return list;
            }

            // =========================================
            // GET USER ID
            // =========================================
            private static int GetUserId(string username)
            {
                if (string.IsNullOrEmpty(username))
                    return 0;

                SqlConnection conn = null;
                try
                {
                    conn = new SqlConnection(_conn);
                    conn.Open();

                    string q = "SELECT Id FROM Users WHERE Username = @u";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@u", username);

                    object res = cmd.ExecuteScalar();
                    if (res == null)
                        return 0;

                    return Convert.ToInt32(res);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DB:GetUserId " + ex.Message);
                    return 0;
                }
                finally
                {
                    if (conn != null) conn.Dispose();
                }
            }
        }
    }
}
