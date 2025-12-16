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
        private const int DEFAULT_PORT = 8888;

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
            NetworkMode mode = NetworkMode.Local;
            // NetworkMode.Lan;
            // NetworkMode.Internet;

            switch (mode)
            {
                case NetworkMode.Local:
                    StartLocal(tcp);
                    break;

                case NetworkMode.Lan:
                    StartLan(tcp);
                    break;

                case NetworkMode.Internet:
                    StartInternet(tcp, "your.public.ip", 8888);
                    break;
            }

            Application.Run(new LoginForm(request, dispatcher));
        }
        static void StartLocal(TcpClientHelper tcp)
        {
            tcp.Connect("127.0.0.1", 8888);
        }

        static void StartLan(TcpClientHelper tcp)
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
                    MessageBox.Show(
                        "Không tìm thấy server LAN",
                        "LAN",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            });

            Application.ApplicationExit += (s, e) =>
            {
                discovery.Stop();
                tcp.Dispose();
            };
        }
        static void StartInternet(TcpClientHelper tcp, string ip, int port)
        {
            tcp.Connect(ip, port);
        }

    }
}
