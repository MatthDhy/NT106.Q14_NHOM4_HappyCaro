using System;
using System.Windows.Forms;

namespace Client.Forms
{
    public partial class RegisterForm : Form
    {
        private readonly ClientRequest _clientRequest;
        private readonly ClientDispatcher _dispatcher;

        private bool _showPassword = false;

        public RegisterForm(ClientRequest request, ClientDispatcher dispatcher)
        {
            InitializeComponent();
            _clientRequest = request;
            _dispatcher = dispatcher;

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPass.UseSystemPasswordChar = true;

            // Gắn event dispatcher
            _dispatcher.OnRegisterSuccess += HandleRegisterSuccess;
            _dispatcher.OnRegisterFail += HandleRegisterFail;

            lnkLogin.LinkClicked -= lnkLogin_LinkClicked;
            lnkLogin.LinkClicked += lnkLogin_LinkClicked;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập","Lỗi",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtPassword.Text.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải từ 6 kí tự", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtPassword.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("Mật khẩu không trùng khớp", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            btnRegister.Enabled = false;

            _clientRequest.Register(txtUsername.Text.Trim(), txtPassword.Text, txtEmail.Text.Trim());
        }

        private void HandleRegisterSuccess()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(HandleRegisterSuccess));
                return;
            }

            btnRegister.Enabled = true;
            MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Unsubscribe();

            var login = new LoginForm(_clientRequest, _dispatcher);
            login.Show();
            this.Close();
        }

        private void HandleRegisterFail(string payload)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(HandleRegisterFail), payload);
                return;
            }

            btnRegister.Enabled = true;

            string msg = "Đăng ký thất bại.";

            try
            {
                var err = JsonHelper.Deserialize<ErrorResponse>(payload);
                if (err != null) msg = err.Error;
            }
            catch { }

            MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnEye_Click(object sender, EventArgs e)
        {
            bool isHidden = txtPassword.UseSystemPasswordChar;

            txtPassword.UseSystemPasswordChar = !isHidden;
            txtConfirmPass.UseSystemPasswordChar = !isHidden;

            btnEye.Image = isHidden ? Properties.Resources.hide : Properties.Resources.view;
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Tìm xem có LoginForm nào đang mở sẵn không
            foreach (Form f in Application.OpenForms)
            {
                if (f is LoginForm)
                {
                    f.Show();
                    f.BringToFront();
                    this.Close();
                    return; // Thoát luôn, không new thêm cái nào nữa
                }
            }

            // Nếu không tìm thấy cái nào thì mới tạo mới
            var login = new LoginForm(_clientRequest, _dispatcher);
            login.Show();
            this.Close();
        }

        private void Unsubscribe()
        {
            _dispatcher.OnRegisterSuccess -= HandleRegisterSuccess;
            _dispatcher.OnRegisterFail -= HandleRegisterFail;
        }

        private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Unsubscribe();
        }

        private class ErrorResponse
        {
            public string Error { get; set; }
        }
    }
}
