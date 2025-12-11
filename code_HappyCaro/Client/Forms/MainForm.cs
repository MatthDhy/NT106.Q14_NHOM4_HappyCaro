using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json; // Cần cài NuGet Newtonsoft.Json hoặc dùng System.Text.Json tuỳ project bạn
using Client.Forms;

namespace Client.Forms
{
    public partial class MainForm : Form
    {
        private TcpClientHelper _client; // Biến kết nối mạng
        private UserInfo _currentUser;   // Thông tin người chơi hiện tại

        // Constructor nhận client và user info từ Login Form
        public MainForm(TcpClientHelper client, string username, int rank, int wins, int losses)
        {
            InitializeComponent();
            _client = client;

            _currentUser = new UserInfo
            {
                Username = username,
                RankPoint = rank,
                Wins = wins,
                Losses = losses
            };

            // Đăng ký nhận tin nhắn từ server
            if (_client != null)
            {
                _client.OnEnvelopeReceived += HandleServerMessage;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin user lên giao diện
            lblUsername.Text = _currentUser.Username;
            lblRankPoint.Text = $"Điểm: {_currentUser.RankPoint}";

            int totalMatches = _currentUser.Wins + _currentUser.Losses + _currentUser.Draws;
            lblWinRate.Text = $"Trận: {totalMatches} (Thắng: {_currentUser.Wins})";

            // Yêu cầu server gửi bảng xếp hạng
            RequestRanking();
        }

        private void RequestRanking()
        {
            if (_client != null)
            {
                _client.EnqueueSend(new MessageEnvelope
                {
                    Type = MessageType.REQUEST_RANKING,
                    Payload = ""
                });
            }
        }

        private void HandleServerMessage(MessageEnvelope env)
        {
            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    switch (env.Type)
                    {
                        case MessageType.RANKING_DATA:
                            UpdateRankingList(env.Payload);
                            break;

                        case MessageType.ROOM_CREATE_OK:
                            // Chuyển sang GameForm ở chế độ chờ
                            var data = JsonHelper.Deserialize<dynamic>(env.Payload);
                            int newRoomId = data.roomId; // Tuỳ cách bạn parse json
                            OpenGameForm(newRoomId, _currentUser.Username, "Waiting...", _currentUser.Username);
                            break;

                        case MessageType.ROOM_JOIN_OK:
                            // Chuyển sang GameForm để chơi
                            var roomData = JsonHelper.Deserialize<dynamic>(env.Payload);
                            // Parse dữ liệu từ server trả về để mở GameForm
                            // Ví dụ: OpenGameForm(roomId, p1, p2...);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xử lý tin nhắn: " + ex.Message);
                }
            });
        }

        private void UpdateRankingList(string jsonPayload)
        {
            // Giả sử server gửi về List<UserInfo>
            // Bạn cần class RankingItem tương ứng với JSON server gửi
            var rankingList = JsonHelper.Deserialize<List<RankingItem>>(jsonPayload);

            dgvRanking.Rows.Clear();
            int rank = 1;
            if (rankingList != null)
            {
                foreach (var item in rankingList)
                {
                    dgvRanking.Rows.Add(rank++, item.username, item.rank);
                }
            }
        }

        // Tạo phòng mới
        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            _client.EnqueueSend(new MessageEnvelope
            {
                Type = MessageType.ROOM_CREATE,
                Payload = ""
            });
        }

        // Mở danh sách phòng (Có thể mở 1 Form con hoặc Dialog)
        private void btnFindRoom_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng đang phát triển: Hiện danh sách phòng.");
            // Logic: Gửi request lấy danh sách phòng -> Server trả về -> Hiển thị lên 1 form mới
        }

        // Chơi nhanh (ghép cặp ngẫu nhiên - Tuỳ chọn)
        private void btnPlayNow_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang tìm đối thủ ngẫu nhiên...");
        }

        private void OpenGameForm(int roomId, string p1, string p2, string firstTurn)
        {
            // Code mở GameForm như bạn đã làm ở phần trước
            // GameForm game = new GameForm(_client, roomId, ...);
            // game.Show();
            // this.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Đóng toàn bộ ứng dụng
        }
    }
    // Giả sử bạn có class UserInfo để lưu trữ thông tin người chơi nhận từ Login
    public class UserInfo
    {
        public string Username { get; set; }
        public int RankPoint { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
    }
}