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
        //private DatabaseHelper db;
        public LoginForm()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            //db = new DatabaseHelper();
        }
        public Boolean isValid()
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


        private void lnkForgotPass_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void lnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var reg = new RegisterForm();

            this.Hide(); // Ẩn login

            reg.FormClosed += (s, args) => this.Show(); // Khi reg đóng, show lại login

            reg.Show();

        }

        

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Hash password trước khi truyền vào DB
            //string hashedPassword = Security.HashPassword(password);

            //User loggedUser = db.Login(username, hashedPassword);

            //if (loggedUser != null)
            //{
            //    MessageBox.Show("Đăng nhập thành công!");
            //    MainForm main = new MainForm(loggedUser, db);
            //    main.Show();
            //    this.Hide();
            //}
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu!");
            }
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
        
        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        
    }
}
