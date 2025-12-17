namespace Client.Forms
{
    partial class ForgotPasswordForm
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
            this.pnlBackground = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.btnBack = new System.Windows.Forms.LinkLabel();
            this.btnVerify = new Guna.UI2.WinForms.Guna2GradientButton();
            this.btnSendOTP = new Guna.UI2.WinForms.Guna2GradientButton();
            this.txtOTP = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.btnBack);
            this.pnlBackground.Controls.Add(this.btnVerify);
            this.pnlBackground.Controls.Add(this.btnSendOTP);
            this.pnlBackground.Controls.Add(this.txtOTP);
            this.pnlBackground.Controls.Add(this.txtEmail);
            this.pnlBackground.Controls.Add(this.lblTitle);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(50)))), ((int)(((byte)(255)))));
            this.pnlBackground.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(255)))));
            this.pnlBackground.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(832, 463);
            this.pnlBackground.TabIndex = 0;
            // 
            // btnBack
            // 
            this.btnBack.AutoSize = true;
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.btnBack.LinkColor = System.Drawing.Color.White;
            this.btnBack.Location = new System.Drawing.Point(365, 400);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(71, 23);
            this.btnBack.TabIndex = 5;
            this.btnBack.TabStop = true;
            this.btnBack.Text = "Quay lại";
            this.btnBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnBack_LinkClicked);
            // 
            // btnVerify
            // 
            this.btnVerify.Animated = true;
            this.btnVerify.AutoRoundedCorners = true;
            this.btnVerify.BackColor = System.Drawing.Color.Transparent;
            this.btnVerify.BorderRadius = 27;
            this.btnVerify.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnVerify.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(250)))));
            this.btnVerify.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnVerify.ForeColor = System.Drawing.Color.White;
            this.btnVerify.Location = new System.Drawing.Point(300, 325);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(210, 57);
            this.btnVerify.TabIndex = 4;
            this.btnVerify.Text = "Xác nhận OTP";
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // btnSendOTP
            // 
            this.btnSendOTP.Animated = true;
            this.btnSendOTP.AutoRoundedCorners = true;
            this.btnSendOTP.BackColor = System.Drawing.Color.Transparent;
            this.btnSendOTP.BorderRadius = 21;
            this.btnSendOTP.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnSendOTP.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(250)))));
            this.btnSendOTP.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSendOTP.ForeColor = System.Drawing.Color.White;
            this.btnSendOTP.Location = new System.Drawing.Point(315, 190);
            this.btnSendOTP.Name = "btnSendOTP";
            this.btnSendOTP.Size = new System.Drawing.Size(180, 45);
            this.btnSendOTP.TabIndex = 2;
            this.btnSendOTP.Text = "Gửi mã OTP";
            this.btnSendOTP.Click += new System.EventHandler(this.btnSendOTP_Click);
            // 
            // txtOTP
            // 
            this.txtOTP.BackColor = System.Drawing.Color.Transparent;
            this.txtOTP.BorderRadius = 5;
            this.txtOTP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtOTP.DefaultText = "";
            this.txtOTP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.txtOTP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.txtOTP.Location = new System.Drawing.Point(240, 260);
            this.txtOTP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOTP.Name = "txtOTP";
            this.txtOTP.PlaceholderText = "Nhập mã OTP";
            this.txtOTP.SelectedText = "";
            this.txtOTP.Size = new System.Drawing.Size(329, 45);
            this.txtOTP.TabIndex = 3;
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.Transparent;
            this.txtEmail.BorderRadius = 5;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F);
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(150)))), ((int)(((byte)(200)))));
            this.txtEmail.Location = new System.Drawing.Point(240, 130);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderText = "Nhập Email đã đăng ký";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(329, 45);
            this.txtEmail.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Black", 30F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(200, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(402, 67);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Quên mật khẩu";
            // 
            // ForgotPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 463);
            this.Controls.Add(this.pnlBackground);
            this.Name = "ForgotPasswordForm";
            this.Text = "Quên mật khẩu";
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientPanel pnlBackground;
        private System.Windows.Forms.Label lblTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtOTP;
        private Guna.UI2.WinForms.Guna2GradientButton btnSendOTP;
        private Guna.UI2.WinForms.Guna2GradientButton btnVerify;
        private System.Windows.Forms.LinkLabel btnBack;
    }
}