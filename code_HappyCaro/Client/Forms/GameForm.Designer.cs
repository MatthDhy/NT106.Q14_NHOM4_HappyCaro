namespace Client.Forms
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlBackground = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.btnExit = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnSurrender = new Guna.UI2.WinForms.Guna2GradientButton();
            this.grpChat = new Guna.UI2.WinForms.Guna2GroupBox();
            this.btnSend = new Guna.UI2.WinForms.Guna2Button();
            this.txtMessage = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtChatLog = new System.Windows.Forms.RichTextBox();
            this.lblTurn = new System.Windows.Forms.Label();
            this.grpOpponentInfo = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lblOpponentSymbol = new System.Windows.Forms.Label();
            this.lblOpponentName = new System.Windows.Forms.Label();
            this.grpMyInfo = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lblMySymbol = new System.Windows.Forms.Label();
            this.lblMyName = new System.Windows.Forms.Label();
            this.pnlBoard = new System.Windows.Forms.Panel();
            this.btnMusic = new Guna.UI2.WinForms.Guna2Button();
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.pnlBackground.SuspendLayout();
            this.grpChat.SuspendLayout();
            this.grpOpponentInfo.SuspendLayout();
            this.grpMyInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.btnExit);
            this.pnlBackground.Controls.Add(this.btnSurrender);
            this.pnlBackground.Controls.Add(this.grpChat);
            this.pnlBackground.Controls.Add(this.lblTurn);
            this.pnlBackground.Controls.Add(this.grpOpponentInfo);
            this.pnlBackground.Controls.Add(this.grpMyInfo);
            this.pnlBackground.Controls.Add(this.pnlBoard);
            this.pnlBackground.Controls.Add(this.btnMusic);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.pnlBackground.FillColor2 = System.Drawing.Color.Indigo;
            this.pnlBackground.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(1440, 900);
            this.pnlBackground.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.FillColor = System.Drawing.Color.Transparent;
            this.btnExit.IconColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(1389, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(51, 36);
            this.btnExit.TabIndex = 6;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSurrender
            // 
            this.btnSurrender.Animated = true;
            this.btnSurrender.AutoRoundedCorners = true;
            this.btnSurrender.BackColor = System.Drawing.Color.Transparent;
            this.btnSurrender.BorderRadius = 27;
            this.btnSurrender.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.btnSurrender.FillColor2 = System.Drawing.Color.Red;
            this.btnSurrender.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSurrender.ForeColor = System.Drawing.Color.White;
            this.btnSurrender.Location = new System.Drawing.Point(608, 750);
            this.btnSurrender.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSurrender.Name = "btnSurrender";
            this.btnSurrender.Size = new System.Drawing.Size(225, 56);
            this.btnSurrender.TabIndex = 5;
            this.btnSurrender.Text = "XIN THUA";
            this.btnSurrender.Click += new System.EventHandler(this.btnSurrender_Click);
            // 
            // grpChat
            // 
            this.grpChat.BackColor = System.Drawing.Color.Transparent;
            this.grpChat.BorderColor = System.Drawing.Color.White;
            this.grpChat.BorderRadius = 15;
            this.grpChat.Controls.Add(this.btnSend);
            this.grpChat.Controls.Add(this.txtMessage);
            this.grpChat.Controls.Add(this.txtChatLog);
            this.grpChat.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grpChat.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpChat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpChat.ForeColor = System.Drawing.Color.White;
            this.grpChat.Location = new System.Drawing.Point(1012, 400);
            this.grpChat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpChat.Name = "grpChat";
            this.grpChat.Size = new System.Drawing.Size(394, 438);
            this.grpChat.TabIndex = 4;
            this.grpChat.Text = "Trò chuyện";
            // 
            // btnSend
            // 
            this.btnSend.BorderRadius = 10;
            this.btnSend.FillColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(292, 375);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(90, 50);
            this.btnSend.TabIndex = 2;
            this.btnSend.Text = "Gửi";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.BorderRadius = 10;
            this.txtMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMessage.DefaultText = "";
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMessage.Location = new System.Drawing.Point(11, 375);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.PlaceholderText = "Nhập tin nhắn...";
            this.txtMessage.SelectedText = "";
            this.txtMessage.Size = new System.Drawing.Size(270, 50);
            this.txtMessage.TabIndex = 1;
            // 
            // txtChatLog
            // 
            this.txtChatLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtChatLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChatLog.ForeColor = System.Drawing.Color.White;
            this.txtChatLog.Location = new System.Drawing.Point(11, 62);
            this.txtChatLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChatLog.Name = "txtChatLog";
            this.txtChatLog.ReadOnly = true;
            this.txtChatLog.Size = new System.Drawing.Size(371, 300);
            this.txtChatLog.TabIndex = 0;
            this.txtChatLog.Text = "";
            // 
            // lblTurn
            // 
            this.lblTurn.BackColor = System.Drawing.Color.Transparent;
            this.lblTurn.Font = new System.Drawing.Font("Segoe UI Black", 20F, System.Drawing.FontStyle.Bold);
            this.lblTurn.ForeColor = System.Drawing.Color.Yellow;
            this.lblTurn.Location = new System.Drawing.Point(467, 38);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(507, 62);
            this.lblTurn.TabIndex = 3;
            this.lblTurn.Text = "Đợi người chơi...";
            this.lblTurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpOpponentInfo
            // 
            this.grpOpponentInfo.BackColor = System.Drawing.Color.Transparent;
            this.grpOpponentInfo.BorderRadius = 15;
            this.grpOpponentInfo.Controls.Add(this.lblOpponentSymbol);
            this.grpOpponentInfo.Controls.Add(this.lblOpponentName);
            this.grpOpponentInfo.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpOpponentInfo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpOpponentInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grpOpponentInfo.ForeColor = System.Drawing.Color.White;
            this.grpOpponentInfo.Location = new System.Drawing.Point(1012, 125);
            this.grpOpponentInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpOpponentInfo.Name = "grpOpponentInfo";
            this.grpOpponentInfo.Size = new System.Drawing.Size(394, 250);
            this.grpOpponentInfo.TabIndex = 2;
            this.grpOpponentInfo.Text = "ĐỐI THỦ";
            // 
            // lblOpponentSymbol
            // 
            this.lblOpponentSymbol.AutoSize = true;
            this.lblOpponentSymbol.Font = new System.Drawing.Font("Segoe UI Black", 40F, System.Drawing.FontStyle.Bold);
            this.lblOpponentSymbol.ForeColor = System.Drawing.Color.Blue;
            this.lblOpponentSymbol.Location = new System.Drawing.Point(135, 112);
            this.lblOpponentSymbol.Name = "lblOpponentSymbol";
            this.lblOpponentSymbol.Size = new System.Drawing.Size(106, 106);
            this.lblOpponentSymbol.TabIndex = 1;
            this.lblOpponentSymbol.Text = "O";
            // 
            // lblOpponentName
            // 
            this.lblOpponentName.AutoSize = true;
            this.lblOpponentName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblOpponentName.Location = new System.Drawing.Point(22, 75);
            this.lblOpponentName.Name = "lblOpponentName";
            this.lblOpponentName.Size = new System.Drawing.Size(144, 38);
            this.lblOpponentName.TabIndex = 0;
            this.lblOpponentName.Text = "Waiting...";
            // 
            // grpMyInfo
            // 
            this.grpMyInfo.BackColor = System.Drawing.Color.Transparent;
            this.grpMyInfo.BorderRadius = 15;
            this.grpMyInfo.Controls.Add(this.lblMySymbol);
            this.grpMyInfo.Controls.Add(this.lblMyName);
            this.grpMyInfo.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.grpMyInfo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpMyInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grpMyInfo.ForeColor = System.Drawing.Color.White;
            this.grpMyInfo.Location = new System.Drawing.Point(34, 125);
            this.grpMyInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpMyInfo.Name = "grpMyInfo";
            this.grpMyInfo.Size = new System.Drawing.Size(394, 250);
            this.grpMyInfo.TabIndex = 1;
            this.grpMyInfo.Text = "BẠN (Player)";
            // 
            // lblMySymbol
            // 
            this.lblMySymbol.AutoSize = true;
            this.lblMySymbol.Font = new System.Drawing.Font("Segoe UI Black", 40F, System.Drawing.FontStyle.Bold);
            this.lblMySymbol.ForeColor = System.Drawing.Color.Red;
            this.lblMySymbol.Location = new System.Drawing.Point(135, 112);
            this.lblMySymbol.Name = "lblMySymbol";
            this.lblMySymbol.Size = new System.Drawing.Size(101, 106);
            this.lblMySymbol.TabIndex = 1;
            this.lblMySymbol.Text = "X";
            // 
            // lblMyName
            // 
            this.lblMyName.AutoSize = true;
            this.lblMyName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblMyName.Location = new System.Drawing.Point(22, 75);
            this.lblMyName.Name = "lblMyName";
            this.lblMyName.Size = new System.Drawing.Size(148, 38);
            this.lblMyName.TabIndex = 0;
            this.lblMyName.Text = "Username";
            // 
            // pnlBoard
            // 
            this.pnlBoard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBoard.Location = new System.Drawing.Point(467, 125);
            this.pnlBoard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(507, 564);
            this.pnlBoard.TabIndex = 0;
            this.pnlBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBoard_Paint);
            this.pnlBoard.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlBoard_MouseClick);
            // 
            // btnMusic
            // 
            this.btnMusic.BorderRadius = 15;
            this.btnMusic.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMusic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMusic.ForeColor = System.Drawing.Color.White;
            this.btnMusic.Location = new System.Drawing.Point(30, 30);
            this.btnMusic.Name = "btnMusic";
            this.btnMusic.Size = new System.Drawing.Size(81, 40);
            this.btnMusic.TabIndex = 20;
            this.btnMusic.Text = "Nhạc";
            this.btnMusic.Click += new System.EventHandler(this.btnMusic_Click);
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl1.TargetControl = this.pnlBackground;
            this.guna2DragControl1.UseTransparentDrag = true;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1440, 900);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.pnlBackground.ResumeLayout(false);
            this.grpChat.ResumeLayout(false);
            this.grpOpponentInfo.ResumeLayout(false);
            this.grpOpponentInfo.PerformLayout();
            this.grpMyInfo.ResumeLayout(false);
            this.grpMyInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientPanel pnlBackground;
        private System.Windows.Forms.Panel pnlBoard;
        private Guna.UI2.WinForms.Guna2GroupBox grpMyInfo;
        private System.Windows.Forms.Label lblMyName;
        private System.Windows.Forms.Label lblMySymbol;
        private Guna.UI2.WinForms.Guna2GroupBox grpOpponentInfo;
        private System.Windows.Forms.Label lblOpponentName;
        private System.Windows.Forms.Label lblOpponentSymbol;
        private System.Windows.Forms.Label lblTurn;
        private Guna.UI2.WinForms.Guna2GroupBox grpChat;
        private System.Windows.Forms.RichTextBox txtChatLog;
        private Guna.UI2.WinForms.Guna2TextBox txtMessage;
        private Guna.UI2.WinForms.Guna2Button btnSend;
        private Guna.UI2.WinForms.Guna2GradientButton btnSurrender;
        private Guna.UI2.WinForms.Guna2ControlBox btnExit;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2Button btnMusic;
    }
}