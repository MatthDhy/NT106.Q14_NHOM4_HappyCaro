using Client.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;
using System.Linq;

namespace Client.Forms
{
    public partial class MainForm : Form
    {
        private readonly ClientDispatcher _dispatcher;
        private readonly ClientRequest _request;
        private readonly UserInfo _user;
        private bool _isQuickMatch = false;
        private bool _isLogout = false;

        // Constructor này KHỚP 100% với LoginForm hiện tại của bạn
        public MainForm(ClientDispatcher dispatcher, string username, int rank, int wins, int losses)
        {
            InitializeComponent();

            _dispatcher = dispatcher;
            _request = new ClientRequest(dispatcher.Tcp); // Tạo request từ kết nối có sẵn
            _request.RequestFriendList();


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
            _dispatcher.OnRoomUpdate += OnRoomListReceived;
            _dispatcher.OnAddFriendResult += OnAddFriendResult;
            _dispatcher.OnFriendRequestList += OnFriendRequestList;
            _dispatcher.OnFriendActionResult += OnFriendActionResult;
            _dispatcher.OnFriendList += OnFriendListReceived;



            _request.GetFriendRequests();

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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Đánh dấu là đang logout để không tắt luôn App
                _isLogout = true;

                // Gửi lệnh logout lên server (để server xóa session nếu cần)
                try { _request.Logout(); } catch { }

                // Đóng MainForm
                this.Close();
            }
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
        // GHÉP NGẪU NHIÊN (PLAY NOW)
        // ==========================================================
        private void btnPlayNow_Click(object sender, EventArgs e)
        {
            // Bật cờ ghép ngẫu nhiên để hàm OnRoomListReceived biết mà xử lý
            _isQuickMatch = true;

            // Gửi yêu cầu lấy danh sách phòng
            _request.RequestRoomList();

            // Hiện trạng thái chờ
            this.Cursor = Cursors.WaitCursor;
        }

        // Hàm này sẽ được gọi khi nhận MessageType.ROOM_UPDATE từ server
        private void OnRoomListReceived(string json)
        {
            // Nếu không phải đang ấn nút Play Now thì bỏ qua (để tránh tự vào phòng khi server update list)
            if (!_isQuickMatch) return;

            _isQuickMatch = false; // Reset cờ

            // Chuyển về luồng UI
            if (InvokeRequired)
            {
                Invoke(new Action(() => { this.Cursor = Cursors.Default; }));
                Invoke(new Action<string>(ProcessQuickMatch), json);
            }
            else
            {
                this.Cursor = Cursors.Default;
                ProcessQuickMatch(json);
            }
        }

        private void ProcessQuickMatch(string json)
        {
            try
            {
                // Deserialize danh sách phòng
                var list = JsonHelper.Deserialize<List<JsonElement>>(json);

                if (list != null && list.Count > 0)
                {
                    // Tìm phòng đầu tiên có status = "WAITING"
                    var waitingRoom = list.FirstOrDefault(r => GetString(r, "status") == "WAITING");

                    // Nếu tìm thấy phòng
                    if (waitingRoom.ValueKind != JsonValueKind.Undefined)
                    {
                        int roomId = GetInt(waitingRoom, "id");
                        // Tự động vào phòng
                        _request.JoinRoom(roomId);
                        return;
                    }
                }

                // Nếu không có phòng nào Waiting -> Tự tạo phòng mới
                _request.CreateRoom();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghép phòng: " + ex.Message);
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
                int roomId = 0;
                if (data.TryGetProperty("roomId", out var id)) roomId = id.GetInt32();

                MessageBox.Show($"Tạo phòng thành công!\nMÃ PHÒNG: {roomId}\nHãy gửi mã này cho bạn của bạn.", "Thông báo Mã Phòng");

                OpenGameForm(roomId, _user.Username, "", _user.Username);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi tạo phòng: " + ex.Message); }
        }

        private void btnFindRoom_Click(object sender, EventArgs e)
        {
            // Tạo form nhập mã
            InputRoomIdForm inputForm = new InputRoomIdForm();

            // Nếu người dùng nhập xong và bấm OK
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                int roomId = inputForm.RoomId;

                // Gửi yêu cầu vào phòng tới Server
                _request.JoinRoom(roomId);
            }
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

        private void btnAddFriend_Click(object sender, EventArgs e)
        {
            // Hiện hộp thoại nhập tên đơn giản
            string friendName = ShowInputDialog("Nhập tên người muốn kết bạn:", "Thêm bạn");

            if (!string.IsNullOrWhiteSpace(friendName))
            {
                // Không cho phép tự kết bạn với chính mình
                if (friendName.Trim() == _user.Username)
                {
                    MessageBox.Show("Không thể kết bạn với chính mình!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _request.AddFriend(friendName.Trim());
            }
        }

        private void OnFriendRequestList(string json)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnFriendRequestList), json);
                return;
            }

            var list = JsonHelper.Deserialize<List<FriendRequestItem>>(json);
            if (list == null || list.Count == 0) return;

            foreach (var req in list)
            {
                var rs = MessageBox.Show(
                    $"{req.username} muốn kết bạn với bạn",
                    "Lời mời kết bạn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (rs == DialogResult.Yes)
                    _request.AcceptFriend(req.userId);
                else
                    _request.RejectFriend(req.userId);
            }
        }


        private void OnFriendActionResult(bool success, string message)
        {
            MessageBox.Show(message,
                success ? "Thành công" : "Lỗi",
                MessageBoxButtons.OK,
                success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
        }


        private void OnAddFriendResult(bool success, string message)
        {
            if (InvokeRequired) { Invoke(new Action<bool, string>(OnAddFriendResult), success, message); return; }

            if (success)
            {
                MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(message, "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnFriendListReceived(string json)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnFriendListReceived), json);
                return;
            }

            listFriends.Items.Clear();

            var friends = JsonHelper.Deserialize<List<string>>(json);
            if (friends == null) return;

            foreach (var f in friends)
                listFriends.Items.Add(f);
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _dispatcher.OnRankingReceived -= OnRankingReceived;
            _dispatcher.OnRoomCreated -= OnRoomCreated;
            _dispatcher.OnRoomJoined -= OnRoomJoined;
            _dispatcher.OnRoomUpdate -= OnRoomListReceived;
            _dispatcher.OnAddFriendResult -= OnAddFriendResult;

            if (_isLogout)
            {
                LoginForm login = new LoginForm(_request, _dispatcher);
                login.Show();
            }
            else
            {
                Application.Exit();
            }
        }

        private string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 200,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label() { Left = 30, Top = 20, Text = text, AutoSize = true, Font = new Font("Segoe UI", 10) };
            TextBox textBox = new TextBox() { Left = 30, Top = 50, Width = 320, Font = new Font("Segoe UI", 10) };
            Button confirmation = new Button() { Text = "Ok", Left = 250, Width = 100, Top = 100, DialogResult = DialogResult.OK, Height = 35 };

            confirmation.BackColor = Color.Teal;
            confirmation.ForeColor = Color.White;
            confirmation.FlatStyle = FlatStyle.Flat;

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
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

    public class FriendRequestItem
    {
        public int userId { get; set; }
        public string username { get; set; }
    }

}