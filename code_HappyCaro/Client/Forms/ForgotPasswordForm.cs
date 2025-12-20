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
    public partial class ForgotPasswordForm: Form
    {
        private readonly Client.ClientRequest _request;
        private readonly Client.ClientDispatcher _dispatcher;

        public ForgotPasswordForm(Client.ClientRequest request, Client.ClientDispatcher dispatcher)
        {
            InitializeComponent();
            _request = request;
            _dispatcher = dispatcher;

            // Đăng ký sự kiện: Khi server báo OK thì mới mở Form Reset
            _dispatcher.OnVerifyOTPSuccess += HandleVerifyOTPSuccess;
            _dispatcher.OnVerifyOTPFail += HandleVerifyOTPFail;
            _dispatcher.OnResetRequested += HandleResetRequested;
        }

        private async void btnSendOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ Email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnSendOTP.Enabled = false;
            _request.ForgotPassword(email);
        }

        

        private void btnVerify_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string otp = txtOTP.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(otp))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Email và mã OTP!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnVerify.Enabled = false;
            _request.CheckOTPOnly(email, otp);

        }
        
        private void HandleVerifyOTPSuccess(string payload)
        {
            if (InvokeRequired) { Invoke(new Action<string>(HandleVerifyOTPSuccess), payload); return; }

            btnVerify.Enabled = true;

            try
            {
                // QUAN TRỌNG: Hủy đăng ký trước khi ẩn/đóng form giống LoginForm
                Unsubscribe();

                ResetPasswordForm resetForm = new ResetPasswordForm(_request, _dispatcher, txtEmail.Text.Trim(), txtOTP.Text.Trim());

                // Đảm bảo nếu người dùng quay lại từ form Reset, ta đăng ký lại sự kiện
                resetForm.FormClosed += (s, args) => {
                    if (!this.IsDisposed)
                    {
                        _dispatcher.OnVerifyOTPSuccess += HandleVerifyOTPSuccess;
                        _dispatcher.OnVerifyOTPFail += HandleVerifyOTPFail;
                        _dispatcher.OnResetRequested += HandleResetRequested;
                    }
                };

                resetForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chuyển sang form đặt lại mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnVerify.Enabled = true;
            }
        }
        private void HandleVerifyOTPFail(string payload)
        {
            if (InvokeRequired) { Invoke(new Action<string>(HandleVerifyOTPFail), payload); return; }

            btnVerify.Enabled = true;

            string msg = "Mã OTP không chính xác hoặc đã hết hạn.";
            try
            {
                // Parse lỗi từ JSON payload giống LoginForm
                var err = JsonHelper.Deserialize<ErrorResponse>(payload);
                if (err != null && !string.IsNullOrEmpty(err.Error))
                    msg = err.Error;
            }
            catch { }

            MessageBox.Show(msg, "Xác thực thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void HandleResetRequested()
        {
            if (InvokeRequired) { Invoke(new Action(HandleResetRequested)); return; }

            btnSendOTP.Enabled = true;
            MessageBox.Show("Yêu cầu gửi mã OTP đã được gửi đi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Unsubscribe()
        {
            _dispatcher.OnVerifyOTPSuccess -= HandleVerifyOTPSuccess;
            _dispatcher.OnVerifyOTPFail -= HandleVerifyOTPFail;
            _dispatcher.OnResetRequested -= HandleResetRequested;
        }
        private void ForgotPasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Unsubscribe();
        }
        private void btnBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
        private class ErrorResponse
        {
            public string Error { get; set; }
        }
    }
}
