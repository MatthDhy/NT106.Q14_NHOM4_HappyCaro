using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.Json;
using Client.Forms;

namespace Client.Forms
{
    public partial class GameForm : Form
    {
        // --- CẤU HÌNH ---
        private const int CONS_WIDTH = 30;
        private const int CONS_HEIGHT = 30;
        private const int BOARD_SIZE = 15;

        // --- MẠNG ---
        private readonly ClientDispatcher _dispatcher;
        private readonly ClientRequest _req;

        // --- TRẠNG THÁI GAME ---
        private int _roomId;
        private string _myUsername;
        private string _opponentUsername;
        private bool _isMyTurn;
        private string _mySymbol;
        private bool[,] _lockedBoard;
        private Graphics _g;

        public GameForm() { InitializeComponent(); }

        // Constructor khớp với MainForm
        public GameForm(ClientDispatcher dispatcher, ClientRequest request, int roomId, string myUser, string oppUser, string firstTurnUser)
        {
            InitializeComponent();
            _dispatcher = dispatcher;
            _req = request;

            _roomId = roomId;
            _myUsername = myUser;
            _opponentUsername = oppUser;

            _isMyTurn = (_myUsername == firstTurnUser);
            _mySymbol = _isMyTurn ? "X" : "O";

            // Setup giao diện
            lblMyName.Text = _myUsername;
            lblMySymbol.Text = _mySymbol;
            lblMySymbol.ForeColor = (_mySymbol == "X") ? Color.Red : Color.Blue;

            lblOpponentName.Text = string.IsNullOrEmpty(_opponentUsername) ? "Đang đợi..." : _opponentUsername;
            lblOpponentSymbol.Text = (_mySymbol == "X") ? "O" : "X";
            lblOpponentSymbol.ForeColor = (lblOpponentSymbol.Text == "X") ? Color.Red : Color.Blue;

            _lockedBoard = new bool[BOARD_SIZE, BOARD_SIZE];
            UpdateTurnLabel();

            // Đăng ký sự kiện
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _dispatcher.OnGameUpdate += OnGameUpdate;
            _dispatcher.OnGameEnd += OnGameEnd;
            _dispatcher.OnChatReceived += OnChatReceived;
            _dispatcher.OnRoomJoined += OnRoomJoinedGame;
            // Nếu có sự kiện đầu hàng
            // _dispatcher.OnOpponentSurrender += OnOpponentSurrender; 
        }

        private void UnregisterEvents()
        {
            _dispatcher.OnGameUpdate -= OnGameUpdate;
            _dispatcher.OnGameEnd -= OnGameEnd;
            _dispatcher.OnChatReceived -= OnChatReceived;
            _dispatcher.OnRoomJoined -= OnRoomJoinedGame;
        }

        // --- XỬ LÝ SỰ KIỆN TỪ SERVER ---
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

                    DrawMove(x, y, symbol);
                    _isMyTurn = (nextTurn == _myUsername);
                    UpdateTurnLabel();
                }
                catch { }
            });
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

                    MessageBox.Show($"{msg}\n({reason})", "Kết thúc");
                    pnlBoard.Enabled = false;
                }
                catch { }
            });
        }

        private void OnChatReceived(string json)
        {
            this.Invoke((MethodInvoker)delegate {
                try
                {
                    var data = JsonHelper.Deserialize<JsonElement>(json);
                    string from = GetString(data, "from");
                    string text = GetString(data, "message");
                    if (string.IsNullOrEmpty(text)) text = GetString(data, "text");

                    AppendChat(from, text);
                }
                catch { }
            });
        }

        private void OnRoomJoinedGame(string json)
        {
            this.Invoke((MethodInvoker)delegate {
                try
                {
                    var data = JsonHelper.Deserialize<JsonElement>(json);
                    string p2 = GetString(data, "player2");
                    // Kiểm tra cả viết hoa viết thường
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

        private void GameForm_Load(object sender, EventArgs e)
        {
            _g = pnlBoard.CreateGraphics();
            DrawChessBoard();
            UpdateMusicButton();
        }

        // --- VẼ BÀN CỜ ---
        private void pnlBoard_Paint(object sender, PaintEventArgs e) => DrawChessBoard();

        private void DrawChessBoard()
        {
            Graphics g = pnlBoard.CreateGraphics();
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i <= BOARD_SIZE; i++)
            {
                g.DrawLine(pen, 0, i * CONS_HEIGHT, BOARD_SIZE * CONS_WIDTH, i * CONS_HEIGHT);
                g.DrawLine(pen, i * CONS_WIDTH, 0, i * CONS_WIDTH, BOARD_SIZE * CONS_HEIGHT);
            }
        }

        private void DrawMove(int x, int y, string symbol)
        {
            _lockedBoard[x, y] = true;
            int drawX = x * CONS_WIDTH;
            int drawY = y * CONS_HEIGHT;
            Font font = new Font("Arial", 16, FontStyle.Bold);
            Brush brush = (symbol == "X") ? Brushes.Red : Brushes.Blue;
            _g.DrawString(symbol, font, brush, drawX + 5, drawY + 2);
        }

        private void pnlBoard_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isMyTurn) return;

            int x = e.X / CONS_WIDTH;
            int y = e.Y / CONS_HEIGHT;

            if (x >= 0 && x < BOARD_SIZE && y >= 0 && y < BOARD_SIZE && !_lockedBoard[x, y])
            {
                _req.SendMove(x, y);
            }
        }

        // --- CÁC NÚT CHỨC NĂNG ---
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                _req.Chat(txtMessage.Text);
                txtMessage.Clear();
            }
        }

        private void btnSurrender_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đầu hàng?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _req.Surrender();
            }
        }

        private void btnExit_Click(object sender, EventArgs e) => this.Close();

        // --- NHẠC VÀ UI ---
        private void btnMusic_Click(object sender, EventArgs e)
        {
            // Cần có class AudioManager đã tạo ở các bước trước
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

        private void GameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterEvents();
            _req.LeaveRoom();
        }

        // --- HELPER JSON ---
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

        private void AppendChat(string from, string text)
        {
            txtChatLog.SelectionColor = (from == _myUsername) ? Color.Cyan : Color.Yellow;
            txtChatLog.AppendText($"{from}: ");
            txtChatLog.SelectionColor = Color.White;
            txtChatLog.AppendText($"{text}\n");
            txtChatLog.ScrollToCaret();
        }

        private void AppendSystemLog(string text)
        {
            txtChatLog.SelectionColor = Color.Gray;
            txtChatLog.AppendText($"[System]: {text}\n");
        }
    }
}