using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Forms;

namespace Client
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // --- Khởi tạo Dependencies ---
            var tcpClientHelper = new TcpClientHelper();
            var clientDispatcher = new ClientDispatcher(tcpClientHelper);
            var clientRequest = new ClientRequest(tcpClientHelper);

            // --- Cố gắng kết nối ngay lập tức (Nếu lỗi sẽ hiển thị MessageBox) ---
            try
            {
                // Thay đổi IP và Port tại đây nếu cần
                tcpClientHelper.Connect("127.0.0.1", 12345);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối Server! Debugging có thể không hoạt động hoàn chỉnh.\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Vẫn tiếp tục mở Form để debug UI, nhưng chức năng mạng sẽ thất bại.
            }

            // --- Mở LoginForm và truyền các dependencies ---
            var loginForm = new LoginForm(clientRequest, clientDispatcher);

            Application.Run(loginForm);

            // --- Dọn dẹp ---
            tcpClientHelper.Dispose();
        }
    }
}
