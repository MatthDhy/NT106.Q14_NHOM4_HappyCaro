using System;
using System.Collections.Generic;
using System.Data.SqlClient; // Thư viện để nối SQL
using System.Threading.Tasks;

namespace ServerCore.ServerCore
{
    // Model User
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Rank { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public User() { } // Constructor rỗng

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Email = username + "@game.com";
            Rank = "Newbie";
            Wins = 0;
            Losses = 0;
        }
    }

    public class Services
    {
        // ============================================================
        // 1. DATABASE (SQL SERVER VERSION)
        // ============================================================
        public static class Database
        {
            // CHÚ Ý: Bạn sửa chuỗi này cho đúng máy bạn
            // Server=.;  nghĩa là máy localhost
            // Database=GameDB; tên database bạn vừa tạo
            // Integrated Security=True; dùng tài khoản Windows để đăng nhập
            private static string connectionString = @"Server=.;Database=GameDB;Integrated Security=True;";

            // Hàm lấy User từ SQL
            public static User GetUser(string username)
            {
                User user = null;

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT * FROM Users WHERE Username = @u";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // Dùng tham số @u để chống hack SQL Injection
                            cmd.Parameters.AddWithValue("@u", username);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    user = new User();
                                    user.Username = reader["Username"].ToString();
                                    user.Password = reader["Password"].ToString();
                                    user.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                                    user.Rank = reader["Rank"].ToString();
                                    user.Wins = Convert.ToInt32(reader["Wins"]);
                                    user.Losses = Convert.ToInt32(reader["Losses"]);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SQL Error (GetUser): " + ex.Message);
                }

                return user;
            }

            // Hàm lấy User bằng Email (cho chức năng quên mật khẩu)
            public static User GetUserByEmail(string email)
            {
                User user = null;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT * FROM Users WHERE Email = @e";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@e", email);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    user = new User();
                                    user.Username = reader["Username"].ToString();
                                    user.Password = reader["Password"].ToString();
                                    user.Email = reader["Email"].ToString();
                                    user.Rank = reader["Rank"].ToString();
                                    user.Wins = Convert.ToInt32(reader["Wins"]);
                                    user.Losses = Convert.ToInt32(reader["Losses"]);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine("SQL Error: " + ex.Message); }
                return user;
            }

            // Hàm đăng ký (Insert vào SQL)
            public static bool Register(string username, string password)
            {
                // Kiểm tra xem user đã tồn tại chưa
                if (GetUser(username) != null) return false;

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = @"INSERT INTO Users (Username, Password, Email, Rank, Wins, Losses) 
                                         VALUES (@u, @p, @e, 'Newbie', 0, 0)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@u", username);
                            cmd.Parameters.AddWithValue("@p", password);
                            cmd.Parameters.AddWithValue("@e", username + "@game.com"); // Email tạm

                            cmd.ExecuteNonQuery(); // Thực thi lệnh Insert
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SQL Error (Register): " + ex.Message);
                    return false;
                }
            }

            // Hàm cập nhật mật khẩu mới (dùng cho Reset Pass)
            public static void UpdatePassword(string username, string newPass)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE Users SET Password = @p WHERE Username = @u";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@p", newPass);
                            cmd.Parameters.AddWithValue("@u", username);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex) { Console.WriteLine("Update Pass Error: " + ex.Message); }
            }
        }

        // ============================================================
        // 2. AUTH (Chức năng Reset Pass - Có cập nhật để lưu vào DB)
        // ============================================================
        public static class Auth
        {
            // Vẫn lưu Token tạm trên RAM (vì token chỉ sống ngắn hạn)
            private static Dictionary<string, string> _resetTokens = new Dictionary<string, string>();

            public static async Task<bool> HandleForgotPassword(string email)
            {
                await Task.Delay(50); // Giả lập mạng

                // Check SQL xem email có tồn tại không
                var user = Database.GetUserByEmail(email);
                if (user == null) return true; // Fake success để bảo mật

                string token = "123456"; // Mã cố định để test

                if (_resetTokens.ContainsKey(email)) _resetTokens[email] = token;
                else _resetTokens.Add(email, token);

                Console.WriteLine($"[SQL-AUTH] Token for {email}: {token}");
                return true;
            }

            public static bool HandleResetVerification(string email, string token, string newPassword)
            {
                if (_resetTokens.ContainsKey(email) && _resetTokens[email] == token)
                {
                    var user = Database.GetUserByEmail(email);
                    if (user != null)
                    {
                        // CẬP NHẬT MẬT KHẨU VÀO SQL
                        Database.UpdatePassword(user.Username, newPassword);

                        _resetTokens.Remove(email);
                        return true;
                    }
                }
                return false;
            }
        }
    }
}