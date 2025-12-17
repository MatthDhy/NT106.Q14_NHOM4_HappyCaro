using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.Ocsp;
namespace Client.Forms
{
    public partial class ForgotPasswordForm: Form
    {
        private readonly Client.ClientRequest _request;
        public ForgotPasswordForm(Client.ClientRequest request)
        {
            InitializeComponent();
            _request = request;
        }

        private async void btnSendOTP_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ Email!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gửi request lên Server thông qua ClientRequest
            // Biến 'request' thường được khởi tạo từ Form chính hoặc Login Form
            _request.ForgotPassword(email);

            MessageBox.Show("Yêu cầu gửi mã OTP đã được gửi đi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnSendOTP.Enabled = false; // Đợi server phản hồi
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

            

            ResetPasswordForm resetForm = new ResetPasswordForm(_request, email, otp);
            resetForm.Show();

            // Đóng form hiện tại
            this.Close();
        }
        private void btnBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
