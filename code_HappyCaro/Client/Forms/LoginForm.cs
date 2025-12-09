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
        public LoginForm()
        {
            InitializeComponent();
        }
        private void RemovePlaceholderEvent(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (txt.ForeColor == Color.Gray)
            {
                txt.Text = "";
                txt.ForeColor = Color.Black;
            }
        }
        private void AddPlaceholderEvent(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                SetPlaceholder(txt, "Tên đăng nhập");
            }
        }
        private void RemovePasswordPlaceholderEvent(object sender, EventArgs e)
        {
            if (txtPassword.ForeColor == Color.Gray)
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.Black;
                txtPassword.UseSystemPasswordChar = true;
            }
        }
        private void AddPasswordPlaceholderEvent(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                txtPassword.UseSystemPasswordChar = false;
                SetPlaceholder(txtPassword, "Mật khẩu");
            }
        }
        private void SetPlaceholder(TextBox txt, string text)
        {
            txt.Text = text;
            txt.ForeColor = Color.Gray;
        }
        
        

        private void LoginForm_Load(object sender, EventArgs e)
        {
            SetPlaceholder(txtUsername, "Tên đăng nhập");
            SetPlaceholder(txtPassword, "Mật khẩu");

            // Gán sự kiện
            txtUsername.Enter += RemovePlaceholderEvent;
            txtUsername.Leave += AddPlaceholderEvent;

            txtPassword.Enter += RemovePasswordPlaceholderEvent;
            txtPassword.Leave += AddPasswordPlaceholderEvent;

            this.ActiveControl = lblLogin;
        }

        
    }
}
