namespace Client.Forms
{
    partial class MainForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlBackground = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.btnPlayNow = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnFindRoom = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnCreateRoom = new Guna.UI2.WinForms.Guna2GradientButton();
            this.grpRanking = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvRanking = new Guna.UI2.WinForms.Guna2DataGridView();
            this.colRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPoint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpFriends = new Guna.UI2.WinForms.Guna2GroupBox();
            this.listFriends = new System.Windows.Forms.ListBox();
            this.grpUserInfo = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lblWinRate = new System.Windows.Forms.Label();
            this.lblRankPoint = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.pBoxAvatar = new Guna.UI2.WinForms.Guna2CirclePictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
            this.btnExit = new Guna.UI2.WinForms.Guna2ControlBox();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.pnlBackground.SuspendLayout();
            this.grpRanking.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRanking)).BeginInit();
            this.grpFriends.SuspendLayout();
            this.grpUserInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAvatar)).BeginInit();
            this.SuspendLayout();
            this.btnMusic = new Guna.UI2.WinForms.Guna2Button();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.btnPlayNow);
            this.pnlBackground.Controls.Add(this.btnFindRoom);
            this.pnlBackground.Controls.Add(this.btnCreateRoom);
            this.pnlBackground.Controls.Add(this.grpRanking);
            this.pnlBackground.Controls.Add(this.grpFriends);
            this.pnlBackground.Controls.Add(this.grpUserInfo);
            this.pnlBackground.Controls.Add(this.lblTitle);
            this.pnlBackground.Controls.Add(this.btnMinimize);
            this.pnlBackground.Controls.Add(this.btnExit);
            this.pnlBackground.Controls.Add(this.btnMusic);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(50)))), ((int)(((byte)(255)))));
            this.pnlBackground.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(255)))));
            this.pnlBackground.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(1280, 720);
            this.pnlBackground.TabIndex = 0;
            // 
            // btnPlayNow
            // 
            this.btnPlayNow.Animated = true;
            this.btnPlayNow.AutoRoundedCorners = true;
            this.btnPlayNow.BackColor = System.Drawing.Color.Transparent;
            this.btnPlayNow.BorderRadius = 29;
            this.btnPlayNow.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPlayNow.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPlayNow.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPlayNow.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPlayNow.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPlayNow.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnPlayNow.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(180)))), ((int)(((byte)(0)))));
            this.btnPlayNow.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnPlayNow.ForeColor = System.Drawing.Color.White;
            this.btnPlayNow.Location = new System.Drawing.Point(450, 400);
            this.btnPlayNow.Name = "btnPlayNow";
            this.btnPlayNow.Size = new System.Drawing.Size(400, 60);
            this.btnPlayNow.TabIndex = 8;
            this.btnPlayNow.Text = "CHƠI NGAY (GHÉP NGẪU NHIÊN)";
            this.btnPlayNow.Click += new System.EventHandler(this.btnPlayNow_Click);
            // 
            // btnFindRoom
            // 
            this.btnFindRoom.Animated = true;
            this.btnFindRoom.AutoRoundedCorners = true;
            this.btnFindRoom.BackColor = System.Drawing.Color.Transparent;
            this.btnFindRoom.BorderRadius = 29;
            this.btnFindRoom.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFindRoom.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFindRoom.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFindRoom.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFindRoom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFindRoom.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnFindRoom.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(77)))));
            this.btnFindRoom.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnFindRoom.ForeColor = System.Drawing.Color.White;
            this.btnFindRoom.Location = new System.Drawing.Point(450, 300);
            this.btnFindRoom.Name = "btnFindRoom";
            this.btnFindRoom.Size = new System.Drawing.Size(400, 60);
            this.btnFindRoom.TabIndex = 7;
            this.btnFindRoom.Text = "TÌM PHÒNG";
            this.btnFindRoom.Click += new System.EventHandler(this.btnFindRoom_Click);
            // 
            // btnCreateRoom
            // 
            this.btnCreateRoom.Animated = true;
            this.btnCreateRoom.AutoRoundedCorners = true;
            this.btnCreateRoom.BackColor = System.Drawing.Color.Transparent;
            this.btnCreateRoom.BorderRadius = 29;
            this.btnCreateRoom.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateRoom.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateRoom.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreateRoom.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreateRoom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCreateRoom.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnCreateRoom.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.btnCreateRoom.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnCreateRoom.ForeColor = System.Drawing.Color.White;
            this.btnCreateRoom.Location = new System.Drawing.Point(450, 200);
            this.btnCreateRoom.Name = "btnCreateRoom";
            this.btnCreateRoom.Size = new System.Drawing.Size(400, 60);
            this.btnCreateRoom.TabIndex = 6;
            this.btnCreateRoom.Text = "TẠO PHÒNG";
            this.btnCreateRoom.Click += new System.EventHandler(this.btnCreateRoom_Click);
            // 
            // grpRanking
            // 
            this.grpRanking.BackColor = System.Drawing.Color.Transparent;
            this.grpRanking.BorderColor = System.Drawing.Color.White;
            this.grpRanking.BorderRadius = 15;
            this.grpRanking.Controls.Add(this.dgvRanking);
            this.grpRanking.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grpRanking.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpRanking.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grpRanking.ForeColor = System.Drawing.Color.White;
            this.grpRanking.Location = new System.Drawing.Point(900, 150);
            this.grpRanking.Name = "grpRanking";
            this.grpRanking.Size = new System.Drawing.Size(350, 540);
            this.grpRanking.TabIndex = 5;
            this.grpRanking.Text = "Bảng xếp hạng";
            // 
            // dgvRanking
            // 
            this.dgvRanking.AllowUserToAddRows = false;
            this.dgvRanking.AllowUserToDeleteRows = false;
            this.dgvRanking.AllowUserToResizeColumns = false;
            this.dgvRanking.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(200)))), ((int)(((byte)(207)))));
            this.dgvRanking.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRanking.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvRanking.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRanking.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRanking.ColumnHeadersHeight = 35;
            this.dgvRanking.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRank,
            this.colName,
            this.colPoint});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(133)))), ((int)(((byte)(147)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRanking.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRanking.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(199)))), ((int)(((byte)(206)))));
            this.dgvRanking.Location = new System.Drawing.Point(15, 60);
            this.dgvRanking.Name = "dgvRanking";
            this.dgvRanking.ReadOnly = true;
            this.dgvRanking.RowHeadersVisible = false;
            this.dgvRanking.RowHeadersWidth = 51;
            this.dgvRanking.RowTemplate.Height = 30;
            this.dgvRanking.Size = new System.Drawing.Size(320, 460);
            this.dgvRanking.TabIndex = 0;
            this.dgvRanking.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.WetAsphalt;
            this.dgvRanking.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(200)))), ((int)(((byte)(207)))));
            this.dgvRanking.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvRanking.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvRanking.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvRanking.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvRanking.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvRanking.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(199)))), ((int)(((byte)(206)))));
            this.dgvRanking.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.dgvRanking.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvRanking.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.dgvRanking.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRanking.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRanking.ThemeStyle.HeaderStyle.Height = 35;
            this.dgvRanking.ThemeStyle.ReadOnly = true;
            this.dgvRanking.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.dgvRanking.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRanking.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.dgvRanking.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRanking.ThemeStyle.RowsStyle.Height = 30;
            this.dgvRanking.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(133)))), ((int)(((byte)(147)))));
            this.dgvRanking.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            // 
            // colRank
            // 
            this.colRank.HeaderText = "#";
            this.colRank.MinimumWidth = 6;
            this.colRank.Name = "colRank";
            this.colRank.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.HeaderText = "Tên";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colPoint
            // 
            this.colPoint.HeaderText = "Điểm";
            this.colPoint.MinimumWidth = 6;
            this.colPoint.Name = "colPoint";
            this.colPoint.ReadOnly = true;
            // 
            // grpFriends
            // 
            this.grpFriends.BackColor = System.Drawing.Color.Transparent;
            this.grpFriends.BorderColor = System.Drawing.Color.White;
            this.grpFriends.BorderRadius = 15;
            this.grpFriends.Controls.Add(this.listFriends);
            this.grpFriends.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grpFriends.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpFriends.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grpFriends.ForeColor = System.Drawing.Color.White;
            this.grpFriends.Location = new System.Drawing.Point(30, 370);
            this.grpFriends.Name = "grpFriends";
            this.grpFriends.Size = new System.Drawing.Size(300, 320);
            this.grpFriends.TabIndex = 4;
            this.grpFriends.Text = "Danh sách bạn bè";
            // 
            // listFriends
            // 
            this.listFriends.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.listFriends.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listFriends.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listFriends.ForeColor = System.Drawing.Color.Black;
            this.listFriends.FormattingEnabled = true;
            this.listFriends.ItemHeight = 23;
            this.listFriends.Location = new System.Drawing.Point(15, 60);
            this.listFriends.Name = "listFriends";
            this.listFriends.Size = new System.Drawing.Size(270, 207);
            this.listFriends.TabIndex = 0;
            // 
            // grpUserInfo
            // 
            this.grpUserInfo.BackColor = System.Drawing.Color.Transparent;
            this.grpUserInfo.BorderColor = System.Drawing.Color.White;
            this.grpUserInfo.BorderRadius = 15;
            this.grpUserInfo.Controls.Add(this.lblWinRate);
            this.grpUserInfo.Controls.Add(this.lblRankPoint);
            this.grpUserInfo.Controls.Add(this.lblUsername);
            this.grpUserInfo.Controls.Add(this.pBoxAvatar);
            this.grpUserInfo.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.grpUserInfo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.grpUserInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.grpUserInfo.ForeColor = System.Drawing.Color.White;
            this.grpUserInfo.Location = new System.Drawing.Point(30, 150);
            this.grpUserInfo.Name = "grpUserInfo";
            this.grpUserInfo.Size = new System.Drawing.Size(300, 200);
            this.grpUserInfo.TabIndex = 3;
            this.grpUserInfo.Text = "Thông tin cá nhân";
            // 
            // lblWinRate
            // 
            this.lblWinRate.AutoSize = true;
            this.lblWinRate.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblWinRate.Location = new System.Drawing.Point(110, 130);
            this.lblWinRate.Name = "lblWinRate";
            this.lblWinRate.Size = new System.Drawing.Size(92, 23);
            this.lblWinRate.TabIndex = 3;
            this.lblWinRate.Text = "Thắng: 0/0";
            // 
            // lblRankPoint
            // 
            this.lblRankPoint.AutoSize = true;
            this.lblRankPoint.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRankPoint.ForeColor = System.Drawing.Color.Yellow;
            this.lblRankPoint.Location = new System.Drawing.Point(110, 100);
            this.lblRankPoint.Name = "lblRankPoint";
            this.lblRankPoint.Size = new System.Drawing.Size(95, 23);
            this.lblRankPoint.TabIndex = 2;
            this.lblRankPoint.Text = "Điểm: 1000";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblUsername.Location = new System.Drawing.Point(110, 60);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(128, 32);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username";
            // 
            // pBoxAvatar
            // 
            this.pBoxAvatar.ImageRotate = 0F;
            this.pBoxAvatar.Location = new System.Drawing.Point(15, 60);
            this.pBoxAvatar.Name = "pBoxAvatar";
            this.pBoxAvatar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            this.pBoxAvatar.Size = new System.Drawing.Size(80, 80);
            this.pBoxAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pBoxAvatar.TabIndex = 0;
            this.pBoxAvatar.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Black", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(445, 50);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(432, 81);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "HAPPY CARO";
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
            this.btnMinimize.FillColor = System.Drawing.Color.Transparent;
            this.btnMinimize.IconColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(1184, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(45, 29);
            this.btnMinimize.TabIndex = 1;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.FillColor = System.Drawing.Color.Transparent;
            this.btnExit.IconColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(1235, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(45, 29);
            this.btnExit.TabIndex = 0;
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl1.TargetControl = this.pnlBackground;
            this.guna2DragControl1.UseTransparentDrag = true;
            // 
            // btnMusic
            // 
            this.btnMusic.BorderRadius = 15; // Nút tròn
            this.btnMusic.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128))))); // Màu cam nhạt
            this.btnMusic.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMusic.ForeColor = System.Drawing.Color.White;
            this.btnMusic.Location = new System.Drawing.Point(30, 30); // Vị trí góc trái trên
            this.btnMusic.Name = "btnMusic";
            this.btnMusic.Size = new System.Drawing.Size(60, 40);
            this.btnMusic.TabIndex = 20;
            this.btnMusic.Text = "Nhạc: ON";
            this.btnMusic.Click += new System.EventHandler(this.btnMusic_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.grpRanking.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRanking)).EndInit();
            this.grpFriends.ResumeLayout(false);
            this.grpUserInfo.ResumeLayout(false);
            this.grpUserInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxAvatar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientPanel pnlBackground;
        private Guna.UI2.WinForms.Guna2ControlBox btnExit;
        private Guna.UI2.WinForms.Guna2ControlBox btnMinimize;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2GroupBox grpUserInfo;
        private Guna.UI2.WinForms.Guna2CirclePictureBox pBoxAvatar;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblWinRate;
        private System.Windows.Forms.Label lblRankPoint;
        private Guna.UI2.WinForms.Guna2GroupBox grpFriends;
        private System.Windows.Forms.ListBox listFriends;
        private Guna.UI2.WinForms.Guna2GroupBox grpRanking;
        private Guna.UI2.WinForms.Guna2DataGridView dgvRanking;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPoint;
        private Guna.UI2.WinForms.Guna2GradientButton btnCreateRoom;
        private Guna.UI2.WinForms.Guna2GradientButton btnFindRoom;
        private Guna.UI2.WinForms.Guna2GradientButton btnPlayNow;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2Button btnMusic;
    }
}