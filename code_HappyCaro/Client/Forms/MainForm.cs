using Client.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Drawing;

namespace Client.Forms
{
    public partial class MainForm : Form
    {
        private readonly ClientDispatcher _dispatcher;
        private readonly ClientRequest _request;
        private readonly UserInfo _user;

        // Constructor này KHỚP 100% với LoginForm hiện tại của bạn
        public MainForm(ClientDispatcher dispatcher, string username, int rank, int wins, int losses)
        {
            InitializeComponent();

            _dispatcher = dispatcher;
            _request = new ClientRequest(dispatcher.Tcp); // Tạo request từ kết nối có sẵn

            _user = new UserInfo
            {
                Username = username,
                RankPoint = rank,
                Wins = wins,
                Losses = losses
            };

            // Đăng ký sự kiện
            _dispatcher.OnRankingReceived += OnRankingReceived;
            _dispatcher.OnRoomCreated += OnRoomCreated;
            _dispatcher.OnRoomJoined += OnRoomJoined;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblUsername.Text = _user.Username;
            lblRankPoint.Text = $"Điểm: {_user.RankPoint}";
            lblWinRate.Text = $"Thắng: {_user.Wins}";

            // Load nhạc nền (nếu có file)
            try
            {
                string musicPath = System.IO.Path.Combine(Application.StartupPath, "Sounds", "background.wav");
                AudioManager.Init(musicPath);
            }
            catch { }

            try
            {
                // Đường dẫn đến thư mục Avatars
                string avatarPath = Path.Combine(Application.StartupPath, "Avatars");

                if (Directory.Exists(avatarPath))
                {
                    // Lấy danh sách tất cả file ảnh (.png, .jpg, .jpeg)
                    // GetFiles trả về danh sách đường dẫn
                    var files = Directory.GetFiles(avatarPath, "*.*");

                    if (files.Length > 0)
                    {
                        Random rnd = new Random();
                        int index = rnd.Next(0, files.Length); 

                        pBoxAvatar.Image = Image.FromFile(files[index]);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            // Lấy bảng xếp hạng
            _request.RequestRanking();
        }

        private void btnMusic_Click(object sender, EventArgs e)
        {
            try
            {
                AudioManager.Toggle(); // Gọi hàm bật/tắt
                UpdateMusicButton();   // Cập nhật màu nút
            }
            catch { }
        }

        private void UpdateMusicButton()
        {
            if (btnMusic == null) return;

            if (AudioManager.IsMuted)
            {
                btnMusic.Text = "OFF";
                btnMusic.FillColor = System.Drawing.Color.Gray;
            }
            else
            {
                btnMusic.Text = "ON";
                btnMusic.FillColor = System.Drawing.Color.FromArgb(255, 192, 128);
            }
        }

        // ==========================================================
        // XỬ LÝ RANKING
        // ==========================================================
        private void OnRankingReceived(string json)
        {
            if (InvokeRequired) { Invoke(new Action<string>(OnRankingReceived), json); return; }

            try
            {
                var list = JsonHelper.Deserialize<List<RankingItem>>(json);
                dgvRanking.Rows.Clear();
                if (list == null) return;

                int rank = 1;
                foreach (var item in list)
                {
                    dgvRanking.Rows.Add(rank++, item.username, item.rank);
                }
            }
            catch { }
        }

        // ==========================================================
        // TẠO PHÒNG
        // ==========================================================
        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            _request.CreateRoom();
        }

        private void OnRoomCreated(string json)
        {
            if (InvokeRequired) { Invoke(new Action<string>(OnRoomCreated), json); return; }

            try
            {
                var data = JsonHelper.Deserialize<JsonElement>(json);
                int roomId = GetInt(data, "roomId");

                // Tạo phòng: Mình là chủ, đối thủ chưa có, mình đi trước
                OpenGameForm(roomId, _user.Username, "", _user.Username);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tạo phòng: " + ex.Message); }
        }

        // ==========================================================
        // VÀO PHÒNG (JOIN)
        // ==========================================================
        private void btnFindRoom_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng tìm phòng đang phát triển!");
            // Bạn có thể mở form nhập ID phòng ở đây sau này
        }

        private void OnRoomJoined(string json)
        {
            if (InvokeRequired) { Invoke(new Action<string>(OnRoomJoined), json); return; }

            try
            {
                // Parse dữ liệu chi tiết để biết đối thủ là ai
                var data = JsonHelper.Deserialize<JsonElement>(json);
                int roomId = GetInt(data, "roomId");
                string p1 = GetString(data, "player1");
                string p2 = GetString(data, "player2");
                string firstTurn = GetString(data, "firstTurn");

                // Xác định tên đối thủ
                string opponent = (p1 == _user.Username) ? p2 : p1;

                OpenGameForm(roomId, _user.Username, opponent, firstTurn);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi vào phòng: " + ex.Message); }
        }

        private void btnPlayNow_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang tìm đối thủ...", "Thông báo");
        }

        // ==========================================================
        // MỞ GAME FORM
        // ==========================================================
        private void OpenGameForm(int roomId, string myUser, string oppUser, string firstTurn)
        {
            // Truyền Dispatcher và Request đã có sang GameForm
            var game = new GameForm(_dispatcher, _request, roomId, myUser, oppUser, firstTurn);

            game.FormClosed += (s, e) =>
            {
                this.Show();
                _request.RequestRanking(); // Cập nhật lại rank
            };

            game.Show();
            this.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hủy đăng ký sự kiện để tránh lỗi bộ nhớ
            _dispatcher.OnRankingReceived -= OnRankingReceived;
            _dispatcher.OnRoomCreated -= OnRoomCreated;
            _dispatcher.OnRoomJoined -= OnRoomJoined;
            Application.Exit();
        }

        // --- HÀM HỖ TRỢ ĐỌC JSON AN TOÀN ---
        private string GetString(JsonElement elem, string key)
        {
            if (elem.TryGetProperty(key, out var val)) return val.GetString();
            if (elem.TryGetProperty(key.ToLower(), out var val2)) return val2.GetString();
            return "";
        }
        private int GetInt(JsonElement elem, string key)
        {
            if (elem.TryGetProperty(key, out var val)) return val.GetInt32();
            if (elem.TryGetProperty(key.ToLower(), out var val2)) return val2.GetInt32();
            return 0;
        }
    }

    // Class UserInfo hỗ trợ (nếu chưa có trong file khác)
    public class UserInfo
    {
        public string Username { get; set; }
        public int RankPoint { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}