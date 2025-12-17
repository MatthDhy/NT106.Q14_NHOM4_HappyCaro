using System;
using System.Drawing;
using System.Windows.Forms;

namespace Client.Forms
{
    public class InputRoomIdForm : Form
    {
        public int RoomId { get; private set; }
        private TextBox txtRoomId;
        private Button btnOk;
        private Button btnCancel;
        private Label lblMessage;

        public InputRoomIdForm()
        {
            // Cài đặt Form
            this.Text = "Nhập Mã Phòng";
            this.Size = new Size(350, 180);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Label hướng dẫn
            lblMessage = new Label() { Text = "Nhập mã phòng bạn muốn vào:", Location = new Point(20, 20), AutoSize = true };

            // Ô nhập liệu
            txtRoomId = new TextBox() { Location = new Point(20, 50), Width = 290, Font = new Font("Arial", 12) };
            // Chỉ cho nhập số
            txtRoomId.KeyPress += (s, e) => { if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true; };

            // Nút OK
            btnOk = new Button() { Text = "Vào Phòng", Location = new Point(120, 90), Width = 90, DialogResult = DialogResult.OK };
            btnOk.Click += BtnOk_Click;

            // Nút Hủy
            btnCancel = new Button() { Text = "Hủy", Location = new Point(220, 90), Width = 90, DialogResult = DialogResult.Cancel };

            this.Controls.Add(lblMessage);
            this.Controls.Add(txtRoomId);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnOk;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtRoomId.Text, out int id))
            {
                RoomId = id;
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã phòng hợp lệ (số).");
                // Ngăn form đóng lại bằng cách set DialogResult về None
                this.DialogResult = DialogResult.None;
            }
        }
    }
}