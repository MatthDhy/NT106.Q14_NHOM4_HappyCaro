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
    public partial class LoginForm : Form
    {
        private readonly ClientRequest _clientRequest;
        private readonly ClientDispatcher _clientDispatcher; // Thay thế TcpClientHelper

        // CẬP NHẬT: Constructor nhận ClientDispatcher
        public LoginForm(ClientRequest clientRequest, ClientDispatcher clientDispatcher)
        {
            InitializeComponent();
            _clientRequest = clientRequest;
            _clientDispatcher = clientDispatcher;

            // ĐĂNG KÝ SỰ KIỆN CỦA DISPATCHER
            _clientDispatcher.OnLoginSuccess += OnLoginSuccess;
            _clientDispatcher.OnLoginFail += OnLoginFail;

            txtPassword.UseSystemPasswordChar = true;
            this.FormClosing += LoginForm_FormClosing;
        }
        public Boolean IsValid()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsValid()) return;

            btnLogin.Enabled = false;
            // SỬ DỤNG ClientRequest để gửi
            _clientRequest.Login(txtUsername.Text.Trim(), txtPassword.Text);
        }

        private void OnLoginSuccess(string payload)
        {
            // Dispatcher đảm bảo phương thức này được gọi khi có MessageType.AUTH_LOGIN_OK
            // Tuy nhiên, CẦN XỬ LÝ THREAD NẾU Dispatcher KHÔNG ĐẢM BẢO CHẠY TRÊN UI THREAD
            // Vì ClientDispatcher không tự Invoke, chúng ta phải tự Invoke ở đây.
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnLoginSuccess), payload);
                return;
            }

            try
            {
                // Deserialize UserInfo từ Payload
                var userInfo = JsonHelper.Deserialize<UserInfo>(payload);

                if (userInfo != null)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Hủy đăng ký sự kiện của form này
                    UnsubscribeDispatcherEvents();

                    var mainForm = new MainForm(
                        _clientDispatcher, 
                        userInfo.Username,
                        userInfo.RankPoint,
                        userInfo.Wins,
                        userInfo.Losses
                    );

                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Lỗi dữ liệu phản hồi từ Server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnLogin.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý dữ liệu đăng nhập: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
            }
        }
        private void OnLoginFail(string payload)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnLoginFail), payload);
                return;
            }

            string errorMessage = "Sai tên đăng nhập hoặc mật khẩu.";
            try
            {
                var errorObj = JsonHelper.Deserialize<ErrorResponse>(payload);
                if (errorObj != null && !string.IsNullOrEmpty(errorObj.Error))
                {
                    errorMessage = errorObj.Error;
                }
            }
            catch { /* Bỏ qua lỗi Deserialize */ }

            MessageBox.Show(errorMessage, "Lỗi Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnLogin.Enabled = true;
        }
        private void UnsubscribeDispatcherEvents()
        {
            _clientDispatcher.OnLoginSuccess -= OnLoginSuccess;
            _clientDispatcher.OnLoginFail -= OnLoginFail;
            // Hủy đăng ký sự kiện khác nếu có
        }
        private class ErrorResponse
        {
            public string Error { get; set; }
        }

        private void ckbShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbShowPass.Checked) 
            { 
                txtPassword.UseSystemPasswordChar  = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }
        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Cần truyền ClientRequest và ClientDispatcher (hoặc TcpClientHelper)
            var reg = new RegisterForm(_clientRequest, _clientDispatcher); // Truyền TcpClientHelper cho RegisterForm
            this.Hide();
            reg.FormClosed += (s, args) => this.Show();
            reg.Show();
        }
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnsubscribeDispatcherEvents(); // Hủy đăng ký khi đóng
            Application.Exit();
        }


    }
}
