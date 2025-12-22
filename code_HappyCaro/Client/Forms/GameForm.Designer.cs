namespace Client.Forms
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlBackground = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.lblHastag = new System.Windows.Forms.Label();
            this.btnExit = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnSurrender = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnPlayAgain = new Guna.UI2.WinForms.Guna2GradientButton();
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
            this.pnlBackground.Controls.Add(this.lblHastag);
            this.pnlBackground.Controls.Add(this.btnExit);
            this.pnlBackground.Controls.Add(this.btnSurrender);
            this.pnlBackground.Controls.Add(this.btnPlayAgain);
            this.pnlBackground.Controls.Add(this.grpChat);
            this.pnlBackground.Controls.Add(this.lblTurn);
            this.pnlBackground.Controls.Add(this.grpOpponentInfo);
            this.pnlBackground.Controls.Add(this.grpMyInfo);
            this.pnlBackground.Controls.Add(this.pnlBoard);
            this.pnlBackground.Controls.Add(this.btnMusic);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.FillColor = System.Drawing.Color.BlueViolet;
            this.pnlBackground.FillColor2 = System.Drawing.Color.Indigo;
            this.pnlBackground.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(1517, 987);
            this.pnlBackground.TabIndex = 0;
            // 
            // lblHastag
            // 
            this.lblHastag.AutoSize = true;
            this.lblHastag.BackColor = System.Drawing.Color.Transparent;
            this.lblHastag.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHastag.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblHastag.Location = new System.Drawing.Point(588, 917);
            this.lblHastag.Name = "lblHastag";
            this.lblHastag.Size = new System.Drawing.Size(327, 35);
            this.lblHastag.TabIndex = 21;
            this.lblHastag.Text = "#Vui là chính, thắng là mười";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.FillColor = System.Drawing.Color.Transparent;
            this.btnExit.IconColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(1452, 0);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(66, 48);
            this.btnExit.TabIndex = 6;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSurrender
            // 
            this.btnSurrender.Animated = true;
            this.btnSurrender.AutoRoundedCorners = true;
            this.btnSurrender.BackColor = System.Drawing.Color.Transparent;
            this.btnSurrender.BorderRadius = 29;
            this.btnSurrender.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.btnSurrender.FillColor2 = System.Drawing.Color.IndianRed;
            this.btnSurrender.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSurrender.ForeColor = System.Drawing.Color.White;
            this.btnSurrender.Location = new System.Drawing.Point(59, 636);
            this.btnSurrender.Margin = new System.Windows.Forms.Padding(4);
            this.btnSurrender.Name = "btnSurrender";
            this.btnSurrender.Size = new System.Drawing.Size(231, 61);
            this.btnSurrender.TabIndex = 5;
            this.btnSurrender.Text = "ĐẦU HÀNG";
            this.btnSurrender.Click += new System.EventHandler(this.btnSurrender_Click);
            // 
            // btnPlayAgain
            // 
            this.btnPlayAgain.Animated = true;
            this.btnPlayAgain.AutoRoundedCorners = true;
            this.btnPlayAgain.BackColor = System.Drawing.Color.Transparent;
            this.btnPlayAgain.BorderRadius = 29;
            this.btnPlayAgain.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnPlayAgain.FillColor2 = System.Drawing.Color.Green;
            this.btnPlayAgain.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPlayAgain.ForeColor = System.Drawing.Color.White;
            this.btnPlayAgain.Location = new System.Drawing.Point(59, 636);
            this.btnPlayAgain.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlayAgain.Name = "btnPlayAgain";
            this.btnPlayAgain.Size = new System.Drawing.Size(231, 61);
            this.btnPlayAgain.TabIndex = 7;
            this.btnPlayAgain.Text = "VÁN MỚI";
            this.btnPlayAgain.Visible = false;
            this.btnPlayAgain.Click += new System.EventHandler(this.btnPlayAgain_Click);
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
            this.grpChat.Location = new System.Drawing.Point(1166, 453);
            this.grpChat.Margin = new System.Windows.Forms.Padding(4);
            this.grpChat.Name = "grpChat";
            this.grpChat.Size = new System.Drawing.Size(338, 413);
            this.grpChat.TabIndex = 4;
            this.grpChat.Text = "Trò chuyện";
            // 
            // btnSend
            // 
            this.btnSend.BorderRadius = 10;
            this.btnSend.FillColor = System.Drawing.Color.DeepSkyBlue;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(256, 353);
            this.btnSend.Margin = new System.Windows.Forms.Padding(4);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(72, 48);
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
            this.txtMessage.Location = new System.Drawing.Point(13, 353);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.PlaceholderText = "Nhập tin nhắn...";
            this.txtMessage.SelectedText = "";
            this.txtMessage.Size = new System.Drawing.Size(234, 48);
            this.txtMessage.TabIndex = 1;
            // 
            // txtChatLog
            // 
            this.txtChatLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtChatLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChatLog.ForeColor = System.Drawing.Color.White;
            this.txtChatLog.Location = new System.Drawing.Point(13, 67);
            this.txtChatLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtChatLog.Name = "txtChatLog";
            this.txtChatLog.ReadOnly = true;
            this.txtChatLog.Size = new System.Drawing.Size(315, 273);
            this.txtChatLog.TabIndex = 0;
            this.txtChatLog.Text = "";
            // 
            // lblTurn
            // 
            this.lblTurn.BackColor = System.Drawing.Color.Transparent;
            this.lblTurn.Font = new System.Drawing.Font("Segoe UI Black", 20F, System.Drawing.FontStyle.Bold);
            this.lblTurn.ForeColor = System.Drawing.Color.Yellow;
            this.lblTurn.Location = new System.Drawing.Point(391, 13);
            this.lblTurn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTurn.Name = "lblTurn";
            this.lblTurn.Size = new System.Drawing.Size(694, 67);
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
            this.grpOpponentInfo.Location = new System.Drawing.Point(1179, 133);
            this.grpOpponentInfo.Margin = new System.Windows.Forms.Padding(4);
            this.grpOpponentInfo.Name = "grpOpponentInfo";
            this.grpOpponentInfo.Size = new System.Drawing.Size(300, 293);
            this.grpOpponentInfo.TabIndex = 2;
            this.grpOpponentInfo.Text = "ĐỐI THỦ";
            // 
            // lblOpponentSymbol
            // 
            this.lblOpponentSymbol.AutoSize = true;
            this.lblOpponentSymbol.BackColor = System.Drawing.Color.Transparent;
            this.lblOpponentSymbol.Font = new System.Drawing.Font("Segoe UI Black", 40F, System.Drawing.FontStyle.Bold);
            this.lblOpponentSymbol.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lblOpponentSymbol.Location = new System.Drawing.Point(95, 133);
            this.lblOpponentSymbol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOpponentSymbol.Name = "lblOpponentSymbol";
            this.lblOpponentSymbol.Size = new System.Drawing.Size(106, 106);
            this.lblOpponentSymbol.TabIndex = 1;
            this.lblOpponentSymbol.Text = "O";
            // 
            // lblOpponentName
            // 
            this.lblOpponentName.AutoSize = true;
            this.lblOpponentName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblOpponentName.Location = new System.Drawing.Point(26, 67);
            this.lblOpponentName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
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
            this.grpMyInfo.Location = new System.Drawing.Point(26, 133);
            this.grpMyInfo.Margin = new System.Windows.Forms.Padding(4);
            this.grpMyInfo.Name = "grpMyInfo";
            this.grpMyInfo.Size = new System.Drawing.Size(300, 293);
            this.grpMyInfo.TabIndex = 1;
            this.grpMyInfo.Text = "BẠN (Player)";
            // 
            // lblMySymbol
            // 
            this.lblMySymbol.AutoSize = true;
            this.lblMySymbol.Font = new System.Drawing.Font("Tahoma", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMySymbol.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblMySymbol.Location = new System.Drawing.Point(103, 133);
            this.lblMySymbol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMySymbol.Name = "lblMySymbol";
            this.lblMySymbol.Size = new System.Drawing.Size(97, 97);
            this.lblMySymbol.TabIndex = 1;
            this.lblMySymbol.Text = "X";
            // 
            // lblMyName
            // 
            this.lblMyName.AutoSize = true;
            this.lblMyName.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblMyName.Location = new System.Drawing.Point(26, 67);
            this.lblMyName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMyName.Name = "lblMyName";
            this.lblMyName.Size = new System.Drawing.Size(148, 38);
            this.lblMyName.TabIndex = 0;
            this.lblMyName.Text = "Username";
            // 
            // pnlBoard
            // 
            this.pnlBoard.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlBoard.Location = new System.Drawing.Point(351, 94);
            this.pnlBoard.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBoard.Name = "pnlBoard";
            this.pnlBoard.Size = new System.Drawing.Size(785, 810);
            this.pnlBoard.TabIndex = 0;
            // 
            // btnMusic
            // 
            this.btnMusic.BackColor = System.Drawing.Color.Transparent;
            this.btnMusic.BorderRadius = 15;
            this.btnMusic.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btnMusic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMusic.ForeColor = System.Drawing.Color.White;
            this.btnMusic.Location = new System.Drawing.Point(26, 27);
            this.btnMusic.Margin = new System.Windows.Forms.Padding(4);
            this.btnMusic.Name = "btnMusic";
            this.btnMusic.Size = new System.Drawing.Size(104, 53);
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
            this.ClientSize = new System.Drawing.Size(1517, 987);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameForm_FormClosing);
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
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
        private Guna.UI2.WinForms.Guna2GroupBox grpOpponentInfo;
        private System.Windows.Forms.Label lblOpponentName;
        private System.Windows.Forms.Label lblTurn;
        private Guna.UI2.WinForms.Guna2GroupBox grpChat;
        private System.Windows.Forms.RichTextBox txtChatLog;
        private Guna.UI2.WinForms.Guna2TextBox txtMessage;
        private Guna.UI2.WinForms.Guna2Button btnSend;
        private Guna.UI2.WinForms.Guna2GradientButton btnSurrender;
        private Guna.UI2.WinForms.Guna2ControlBox btnExit;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2Button btnMusic;
        private Guna.UI2.WinForms.Guna2GradientButton btnPlayAgain;
        private System.Windows.Forms.Label lblHastag;
        private System.Windows.Forms.Label lblOpponentSymbol;
        private System.Windows.Forms.Label lblMySymbol;
    }
}