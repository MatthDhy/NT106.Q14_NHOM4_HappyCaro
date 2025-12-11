using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Forms;

namespace Client
{
    internal static class Program
    {
        // Fallback nếu Discovery không tìm thấy server
        private const string DEFAULT_IP = "127.0.0.1";
        private const int DEFAULT_PORT = 18123;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // -------------------------------------------------------
            // 1) Khởi tạo TCP + Dispatcher + Request
            // -------------------------------------------------------
            var tcp = new TcpClientHelper();
            var dispatcher = new ClientDispatcher(tcp);
            var request = new ClientRequest(tcp);

            // -------------------------------------------------------
            // 2) Khởi tạo Discovery Listener
            // -------------------------------------------------------
            var discovery = new DiscoveryListener();
            bool serverFound = false;

            discovery.OnServerFound += (ip, port) =>
            {
                if (serverFound) return; // chỉ connect lần đầu
                serverFound = true;

                MessageBox.Show(
                    $"Tìm thấy server Caro:\nIP: {ip}\nPort: {port}",
                    "Server Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                tcp.Connect(ip, port);
            };

            // Bắt đầu chờ broadcast
            discovery.StartListening();

            // -------------------------------------------------------
            // 3) Fallback sau 3 giây nếu không thấy server
            // -------------------------------------------------------
            _ = Task.Run(async () =>
            {
                await Task.Delay(3000);

                if (!serverFound)
                {
                    MessageBox.Show(
                        $"Không tìm thấy server qua LAN.\nKết nối bằng IP mặc định: {DEFAULT_IP}:{DEFAULT_PORT}",
                        "Không tìm thấy Server",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    tcp.Connect(DEFAULT_IP, DEFAULT_PORT);
                }
            });

            // -------------------------------------------------------
            // 4) Khi app đóng → cleanup
            // -------------------------------------------------------
            Application.ApplicationExit += (s, e) =>
            {
                try { discovery.Stop(); } catch { }
                try { tcp.Dispose(); } catch { }
            };

            // -------------------------------------------------------
            // 5) Mở form Login
            // -------------------------------------------------------
            Application.Run(new LoginForm(request, dispatcher));
        }
    }
}
