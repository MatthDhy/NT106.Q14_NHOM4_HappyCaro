using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class RegisterForm : Form
    {
        // Thêm trường để lưu trữ đối tượng ClientRequest
        private readonly ClientRequest _clientRequest;
        private readonly ClientDispatcher _clientDispatcher; // Cần để đăng ký sự kiện nhận phản hồi

        // Cập nhật constructor để nhận TcpClientHelper và ClientRequest
        public RegisterForm(ClientRequest clientRequest, ClientDispatcher clientDispatcher)
        {
            InitializeComponent();
            _clientDispatcher.OnRegisterSuccess += OnRegisterSuccess;
            _clientDispatcher.OnRegisterFail += OnRegisterFail; // Lưu trữ để đăng ký/hủy đăng ký sự kiện

            // Đăng ký sự kiện nhận phản hồi từ server

            // Cài đặt mật khẩu ẩn
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPass.UseSystemPasswordChar = true;
        }

        // Phương thức kiểm tra tính hợp lệ của dữ liệu
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtConfirmPass.Text))
            {
                MessageBox.Show("Vui lòng xác nhận mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtPassword.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Mặc dù trong file gốc chỉ có kiểm tra null/empty cho Email, 
            // nên thêm kiểm tra định dạng email cơ bản
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool IsValidEmail(string email)
        {
            // Regex đơn giản để kiểm tra định dạng email
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }


        private void OnRegisterSuccess()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OnRegisterSuccess));
                return;
            }

            MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Hủy đăng ký sự kiện trước khi đóng Form
            UnsubscribeDispatcherEvents();
            this.Close();
        }
        private void OnRegisterFail(string payload)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(OnRegisterFail), payload);
                return;
            }

            string errorMessage = "Đăng ký thất bại.";
            try
            {
                // Giả sử có JsonHelper.Deserialize và ErrorResponse class
                var errorObj = JsonHelper.Deserialize<ErrorResponse>(payload);
                if (errorObj != null && !string.IsNullOrEmpty(errorObj.Error))
                {
                    errorMessage = errorObj.Error;
                }
            }
            catch { /* Bỏ qua lỗi Deserialize */ }

            MessageBox.Show($"Đăng ký thất bại: {errorMessage}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            btnRegister.Enabled = true; // Kích hoạt lại nút đăng ký
        }
        private void UnsubscribeDispatcherEvents()
        {
            _clientDispatcher.OnRegisterSuccess -= OnRegisterSuccess;
            _clientDispatcher.OnRegisterFail -= OnRegisterFail;
        }
        private class ErrorResponse
        {
            public string Error { get; set; }
        }


        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!IsValid()) return;

            // Vô hiệu hóa nút để tránh gửi nhiều lần
            btnRegister.Enabled = false;

            // Gửi yêu cầu đăng ký qua ClientRequest
            _clientRequest.Register(txtUsername.Text, txtPassword.Text);

        }

        private void btnEye_Click(object sender, EventArgs e)
        {
           
            bool isHidden = txtPassword.UseSystemPasswordChar;
            // Toggle
            
            txtPassword.UseSystemPasswordChar = !isHidden;
            txtConfirmPass.UseSystemPasswordChar = !isHidden;

            // Đổi icon
            btnEye.Image = isHidden ? Properties.Resources.hide : Properties.Resources.view;

        }
        private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnsubscribeDispatcherEvents();
        }


    }
}
