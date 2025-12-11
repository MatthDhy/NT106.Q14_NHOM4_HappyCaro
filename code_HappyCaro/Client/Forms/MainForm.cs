using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class MainForm : Form
    {
        private readonly ClientDispatcher _dispatcher;
        private readonly ClientRequest _request;
        private readonly UserInfo _user;

        public MainForm(ClientDispatcher dispatcher, string username, int rank, int wins, int losses)
        {
            InitializeComponent();

            _dispatcher = dispatcher;
            _request = new ClientRequest(dispatcher.Tcp);

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
            lblWinRate.Text = $"Thắng: {_user.Wins}/{_user.Wins + _user.Losses}";

            // Load bảng xếp hạng
            _request.RequestRanking();
        }

        // ==========================================================
        // RANKING
        // ==========================================================

        private void OnRankingReceived(string json)
        {
            if (InvokeRequired) { Invoke(new Action<string>(OnRankingReceived), json); return; }

            var list = JsonHelper.Deserialize<List<RankingItem>>(json);
            dgvRanking.Rows.Clear();

            if (list == null) return;

            int rank = 1;
            foreach (var item in list)
            {
                dgvRanking.Rows.Add(rank++, item.Username, item.RankPoint);
            }
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

            var data = JsonHelper.Deserialize<RoomCreatedResponse>(json);

            OpenGameForm(data.RoomId, isOwner: true);
        }

        // ==========================================================
        // JOIN PHÒNG
        // ==========================================================

        private void btnFindRoom_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng tìm phòng sẽ được phát triển sau!", "Thông báo");
        }

        private void OnRoomJoined(string json)
        {
            if (InvokeRequired) { Invoke(new Action<string>(OnRoomJoined), json); return; }

            var info = JsonHelper.Deserialize<RoomJoinedResponse>(json);

            OpenGameForm(info.RoomId, isOwner: false);
        }

        // ==========================================================
        // PLAY NOW (ghép ngẫu nhiên)
        // ==========================================================

        private void btnPlayNow_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tính năng ghép ngẫu nhiên sẽ được phát triển sau!", "Thông báo");
        }

        // ==========================================================
        // CHUYỂN QUA GAMEFORM
        // ==========================================================

        private void OpenGameForm(int roomId, bool isOwner)
        {
            var game = new GameForm(_dispatcher, _request, _user, roomId, isOwner);

            game.FormClosed += (s, e) =>
            {
                this.Show();
                _request.RequestRanking();
            };

            game.Show();
            this.Hide();
        }

        // ==========================================================
        // FORM EXIT
        // ==========================================================

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _dispatcher.OnRankingReceived -= OnRankingReceived;
            _dispatcher.OnRoomCreated -= OnRoomCreated;
            _dispatcher.OnRoomJoined -= OnRoomJoined;
        }
    }

    // ==========================================================
    // SUPPORT CLASSES
    // ==========================================================

    public class RankingItem
    {
        public string Username { get; set; }
        public int RankPoint { get; set; }
    }

    public class RoomCreatedResponse
    {
        public int RoomId { get; set; }
    }

    public class RoomJoinedResponse
    {
        public int RoomId { get; set; }
    }
}
