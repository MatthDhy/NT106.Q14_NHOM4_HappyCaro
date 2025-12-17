using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.Forms;

namespace Client
{
    internal static class Program
    {
        private const string LOCAL_IP = "127.0.0.1";
        private const int PORT = 8888;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var tcp = new TcpClientHelper();
            var dispatcher = new ClientDispatcher(tcp);
            var request = new ClientRequest(tcp);

            // ================================
            // CHỌN MODE Ở ĐÂY
            // ================================
            NetworkMode mode = NetworkMode.LanWithFallback;
            // NetworkMode.Local;
            // NetworkMode.Lan;
            // NetworkMode.Internet;

            // Form chính (login)
            LoginForm loginForm = null;

            tcp.OnConnected += () =>
            {
                // đảm bảo chạy trên UI thread
                if (loginForm != null && loginForm.IsHandleCreated)
                    return;

                Application.OpenForms[0]?.BeginInvoke(new Action(() =>
                {
                    loginForm = new LoginForm(request, dispatcher);
                    loginForm.Show();
                }));
            };

            tcp.OnDisconnected += () =>
            {
                MessageBox.Show(
                    "Mất kết nối tới server",
                    "Network",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            };

            // Form rỗng giữ message loop
            var bootstrap = new Form
            {
                WindowState = FormWindowState.Minimized,
                ShowInTaskbar = false
            };

            // ===== KẾT NỐI =====
            switch (mode)
            {
                case NetworkMode.Local:
                    tcp.Connect(LOCAL_IP, PORT);
                    break;

                case NetworkMode.Lan:
                    StartLan(tcp);
                    break;

                case NetworkMode.LanWithFallback:
                    StartLanWithFallback(tcp);
                    break;

                case NetworkMode.Internet:
                    tcp.Connect("your.public.ip", PORT);
                    break;
            }

            Application.Run(bootstrap);
        }

        static void StartLan(TcpClientHelper tcp)
        {
            var discovery = new DiscoveryListener();

            discovery.OnServerFound += (ip, port) =>
            {
                discovery.Stop();
                tcp.Connect(ip, port);
            };

            discovery.Start();
        }

        static void StartLanWithFallback(TcpClientHelper tcp)
        {
            var discovery = new DiscoveryListener();
            bool connected = false;

            discovery.OnServerFound += (ip, port) =>
            {
                if (connected) return;
                connected = true;

                discovery.Stop();
                tcp.Connect(ip, port);
            };

            discovery.Start();

            Task.Delay(3000).ContinueWith(_ =>
            {
                if (!connected && !tcp.IsConnected)
                {
                    discovery.Stop();
                    tcp.Connect(LOCAL_IP, PORT);
                }
            });
        }
    }
}
