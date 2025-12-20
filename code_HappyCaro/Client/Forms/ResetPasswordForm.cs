using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Client.Forms
{
    public partial class ResetPasswordForm: Form
    {
        private readonly Client.ClientRequest _request;
        private readonly Client.ClientDispatcher _dispatcher;
        private readonly string _email;
        private readonly string _otp;

        public ResetPasswordForm(Client.ClientRequest request, Client.ClientDispatcher dispatcher, string email, string otp)
        {
            InitializeComponent();
            _request = request;
            _dispatcher = dispatcher;
            _email = email;
            _otp = otp;

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPass.UseSystemPasswordChar = true;
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            string newPass = txtPassword.Text.Trim();
            string confirmPass = txtConfirmPass.Text.Trim();

            if (newPass.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải trên 6 kí tự", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _request.VerifyReset(_email, _otp, newPass);

            MessageBox.Show("Yêu cầu đổi mật khẩu đã được gửi đi.", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }

        private void btnBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnEye_Click(object sender, EventArgs e)
        {
            bool isHidden = txtPassword.UseSystemPasswordChar;

            txtPassword.UseSystemPasswordChar = !isHidden;
            txtConfirmPass.UseSystemPasswordChar = !isHidden;

            btnEye.Image = isHidden ? Properties.Resources.hide : Properties.Resources.view;
        }

    }
}
