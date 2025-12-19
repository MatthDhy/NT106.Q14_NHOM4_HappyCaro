using System;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class LoginForm : Form
    {
        private readonly ClientRequest _clientRequest;
        private readonly ClientDispatcher _dispatcher;

        public LoginForm(ClientRequest clientRequest, ClientDispatcher dispatcher)
        {
            InitializeComponent();
            _clientRequest = clientRequest;
            _dispatcher = dispatcher;

            // Đăng ký sự kiện dispatcher
            _dispatcher.OnLoginSuccess += HandleLoginSuccess;
            _dispatcher.OnLoginFail += HandleLoginFail;

            txtPassword.UseSystemPasswordChar = true;
            lnkRegister.LinkClicked += lnkRegister_LinkClicked;
            lnkForgotPass.LinkClicked += lnkForgotPass_LinkClicked;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            btnLogin.Enabled = false;
            _clientRequest.Login(txtUsername.Text.Trim(), txtPassword.Text);
        }

        private void HandleLoginSuccess(string payload)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(HandleLoginSuccess), payload);
                return;
            }

            btnLogin.Enabled = true;

            try
            {
                var user = JsonHelper.Deserialize<UserInfo>(payload);

                if (user == null)
                {
                    MessageBox.Show("Dữ liệu phản hồi không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Unsubscribe();

                var main = new MainForm(_dispatcher, user.Username, user.RankPoint, user.Wins, user.Losses);
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý login: " + ex.Message);
            }
        }

        private void HandleLoginFail(string payload)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(HandleLoginFail), payload);
                return;
            }

            btnLogin.Enabled = true;

            string msg = "Sai tên đăng nhập hoặc mật khẩu.";

            try
            {
                var err = JsonHelper.Deserialize<ErrorResponse>(payload);
                if (err != null && !string.IsNullOrEmpty(err.Error))
                    msg = err.Error;
            }
            catch { }

            MessageBox.Show(msg, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ckbShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !ckbShowPass.Checked;
        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var reg = new RegisterForm(_clientRequest, _dispatcher);
            //reg.FormClosed += (s, t) => this.Show();
            this.Hide();
            reg.Show();
        }

        private void lnkForgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ForgotPasswordForm f = new ForgotPasswordForm(this._clientRequest);
            f.Show();
        }

        private void Unsubscribe()
        {
            _dispatcher.OnLoginSuccess -= HandleLoginSuccess;
            _dispatcher.OnLoginFail -= HandleLoginFail;
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Unsubscribe();
            Application.Exit();
        }

        private class ErrorResponse
        {
            public string Error { get; set; }
        }
    }
}
