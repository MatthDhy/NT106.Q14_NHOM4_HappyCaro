using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.Json;
using Client.Forms;

namespace Client.Forms
{
    public partial class GameForm : Form
    {
        private const int CONS_WIDTH = 35;  // Kích thước ô cờ to
        private const int CONS_HEIGHT = 35;
        private const int BOARD_SIZE = 15;

        private readonly ClientDispatcher _dispatcher;
        private readonly ClientRequest _req;

        private int _roomId;
        private string _myUsername;
        private string _opponentUsername;
        private bool _isMyTurn;
        private string _mySymbol;

        private List<List<Button>> _matrix;

        public GameForm() { InitializeComponent(); }

        public GameForm(ClientDispatcher dispatcher, ClientRequest request, int roomId, string myUser, string oppUser, string firstTurnUser)
        {
            InitializeComponent();
            _dispatcher = dispatcher;
            _req = request;

            _roomId = roomId;
            _myUsername = myUser;
            _opponentUsername = oppUser;

            _isMyTurn = string.Equals(_myUsername, firstTurnUser, StringComparison.OrdinalIgnoreCase);
            _mySymbol = _isMyTurn ? "X" : "O";

            lblMyName.Text = _myUsername;
            lblMySymbol.Text = _mySymbol;
            lblMySymbol.ForeColor = (_mySymbol == "X") ? Color.Red : Color.Blue;

            lblOpponentName.Text = string.IsNullOrEmpty(_opponentUsername) ? "Đang đợi..." : _opponentUsername;
            lblOpponentSymbol.Text = (_mySymbol == "X") ? "O" : "X";
            lblOpponentSymbol.ForeColor = (lblOpponentSymbol.Text == "X") ? Color.Red : Color.Blue;

            DrawChessBoard();
            UpdateTurnLabel();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _dispatcher.OnGameUpdate += OnGameUpdate;
            _dispatcher.OnGameEnd += OnGameEnd;
            _dispatcher.OnChatReceived += OnChatReceived;
            _dispatcher.OnRoomJoined += OnRoomJoinedGame;
        }

        private void UnregisterEvents()
        {
            _dispatcher.OnGameUpdate -= OnGameUpdate;
            _dispatcher.OnGameEnd -= OnGameEnd;
            _dispatcher.OnChatReceived -= OnChatReceived;
            _dispatcher.OnRoomJoined -= OnRoomJoinedGame;
        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            UpdateMusicButton();
        }

        // --- BÀN CỜ ---
        private void DrawChessBoard()
        {
            _matrix = new List<List<Button>>();
            pnlBoard.Controls.Clear();
            pnlBoard.Enabled = true;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                _matrix.Add(new List<Button>());
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    Button btn = new Button()
                    {
                        Width = CONS_WIDTH,
                        Height = CONS_HEIGHT,
                        Location = new Point(j * CONS_WIDTH, i * CONS_HEIGHT),
                        Tag = new Point(j, i),
                        BackColor = Color.WhiteSmoke,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Arial", 16, FontStyle.Bold), // Font chữ to
                        Margin = Padding.Empty,
                        Padding = Padding.Empty
                    };
                    btn.FlatAppearance.BorderColor = Color.Silver;
                    btn.Click += Btn_Click;
                    pnlBoard.Controls.Add(btn);
                    _matrix[i].Add(btn);
                }
            }
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            if (!_isMyTurn) return;

            Button btn = sender as Button;
            if (btn == null || !string.IsNullOrEmpty(btn.Text)) return;

            Point point = (Point)btn.Tag;
            _req.SendMove(_roomId, point.X, point.Y);
        }

        // --- CẬP NHẬT TỪ SERVER ---
        private void OnGameUpdate(string json)
        {
            this.Invoke((MethodInvoker)delegate {
                try
                {
                    var data = JsonHelper.Deserialize<JsonElement>(json);
                    int x = GetInt(data, "x");
                    int y = GetInt(data, "y");
                    string symbol = GetString(data, "symbol");
                    string nextTurn = GetString(data, "nextTurn");

                    UpdateBoardUI(x, y, symbol);

                    _isMyTurn = string.Equals(nextTurn, _myUsername, StringComparison.OrdinalIgnoreCase);
                    UpdateTurnLabel();
                }
                catch { }
            });
        }

        private void UpdateBoardUI(int x, int y, string symbol)
        {
            if (_matrix != null && y >= 0 && y < _matrix.Count && x >= 0 && x < _matrix[y].Count)
            {
                Button btn = _matrix[y][x];
                btn.Text = symbol;
                btn.ForeColor = (symbol == "X") ? Color.Red : Color.Blue;
            }
        }

        private void OnGameEnd(string json)
        {
            this.Invoke((MethodInvoker)delegate {
                try
                {
                    var data = JsonHelper.Deserialize<JsonElement>(json);
                    string winner = GetString(data, "winner");
                    string reason = GetString(data, "endReason");

                    string msg = (winner == _myUsername) ? "BẠN THẮNG!" : "BẠN THUA!";
                    if (reason == "DRAW_BY_FULL_BOARD" || reason == "DRAW") msg = "HÒA CỜ!";

                    MessageBox.Show($"{msg}\n({reason})", "Kết thúc ván đấu");

                    pnlBoard.Enabled = false;

                    btnSurrender.Visible = false;
                    btnPlayAgain.Visible = true;
                }
                catch { }
            });
        }

        // --- CHAT ---
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                _req.Chat(_roomId, txtMessage.Text);
                AppendChat(_myUsername, txtMessage.Text);
                txtMessage.Clear();
                txtMessage.Focus();
            }
        }

        private void OnChatReceived(string json)
        {
            this.Invoke((MethodInvoker)delegate {
                try
                {
                    var data = JsonHelper.Deserialize<JsonElement>(json);
                    string from = GetString(data, "from");
                    string text = GetString(data, "text");
                    if (string.IsNullOrEmpty(text)) text = GetString(data, "message");

                    if (from != _myUsername)
                    {
                        AppendChat(from, text);
                    }
                }
                catch { }
            });
        }

        private void AppendChat(string from, string text)
        {
            txtChatLog.SelectionStart = txtChatLog.TextLength;
            txtChatLog.SelectionLength = 0;
            txtChatLog.SelectionColor = (from == _myUsername) ? Color.Cyan : Color.Yellow;
            txtChatLog.AppendText($"{from}: ");
            txtChatLog.SelectionColor = Color.White;
            txtChatLog.AppendText($"{text}\n");
            txtChatLog.ScrollToCaret();
        }

        // --- CÁC NÚT CHỨC NĂNG ---
        private void btnPlayAgain_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSurrender_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đầu hàng?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _req.Surrender();
            }
        }

        private void btnExit_Click(object sender, EventArgs e) => this.Close();

        private void btnMusic_Click(object sender, EventArgs e)
        {
            try { AudioManager.Toggle(); UpdateMusicButton(); } catch { }
        }

        private void UpdateMusicButton()
        {
            try
            {
                if (AudioManager.IsMuted)
                {
                    btnMusic.Text = "OFF";
                    btnMusic.FillColor = Color.Gray;
                }
                else
                {
                    btnMusic.Text = "ON";
                    btnMusic.FillColor = Color.FromArgb(255, 192, 128);
                }
            }
            catch { }
        }

        private void OnRoomJoinedGame(string json)
        {
            this.Invoke((MethodInvoker)delegate {
                try
                {
                    var data = JsonHelper.Deserialize<JsonElement>(json);
                    string p2 = GetString(data, "player2");
                    if (string.IsNullOrEmpty(p2)) p2 = GetString(data, "Player2");

                    if (!string.IsNullOrEmpty(p2) && p2 != _myUsername)
                    {
                        _opponentUsername = p2;
                        lblOpponentName.Text = p2;
                        UpdateTurnLabel();
                        AppendSystemLog($"Người chơi {p2} đã vào phòng.");
                    }
                }
                catch { }
            });
        }

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterEvents();
            _req.LeaveRoom();
        }

        private void UpdateTurnLabel()
        {
            if (string.IsNullOrEmpty(_opponentUsername))
            {
                lblTurn.Text = "Đang tìm đối thủ...";
                lblTurn.ForeColor = Color.White;
            }
            else if (_isMyTurn)
            {
                lblTurn.Text = "Đến lượt BẠN!";
                lblTurn.ForeColor = Color.Cyan;
            }
            else
            {
                lblTurn.Text = $"Đến lượt {_opponentUsername}...";
                lblTurn.ForeColor = Color.Yellow;
            }
        }

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

        private void AppendSystemLog(string text)
        {
            txtChatLog.SelectionStart = txtChatLog.TextLength;
            txtChatLog.SelectionLength = 0;
            txtChatLog.SelectionColor = Color.Gray;
            txtChatLog.AppendText($"[System]: {text}\n");
        }
    }
}