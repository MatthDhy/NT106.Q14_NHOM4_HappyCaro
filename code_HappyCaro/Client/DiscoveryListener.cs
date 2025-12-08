using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class DiscoveryListener
    {
        public event Action<string, int> OnServerFound;
        private UdpClient _udp;
        private bool _running = false;
        private const int DISCOVERY_PORT = 9998;

        public void StartListening()
        {
            if (_running) return;
            _running = true;
            _udp = new UdpClient(DISCOVERY_PORT);
            _ = Task.Run(async () =>
            {
                while (_running)
                {
                    try
                    {
                        var res = await _udp.ReceiveAsync();
                        string msg = Encoding.UTF8.GetString(res.Buffer);
                        var parts = msg.Split('|');
                        if (parts.Length == 3 && parts[0] == "HAPPY_CARO_SERVER")
                        {
                            string ip = parts[1];
                            if (int.TryParse(parts[2], out int port))
                                OnServerFound?.Invoke(ip, port);
                        }
                    }
                    catch { await Task.Delay(100); }
                }
            });
        }

        public void Stop()
        {
            _running = false;
            try { _udp?.Close(); } catch { }
        }
    }
}
