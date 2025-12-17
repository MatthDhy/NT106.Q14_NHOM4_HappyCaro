using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class DiscoveryListener
    {
        private const int DISCOVERY_PORT = 9998;
        private UdpClient _udp;
        private bool _running;

        public event Action<string, int> OnServerFound;

        public void Start()
        {
            _running = true;
            _udp = new UdpClient(new IPEndPoint(IPAddress.Any, DISCOVERY_PORT));

            Task.Run(async () =>
            {
                while (_running)
                {
                    try
                    {
                        var res = await _udp.ReceiveAsync();
                        var msg = Encoding.UTF8.GetString(res.Buffer);
                        var p = msg.Split('|');

                        if (p.Length == 3 && p[0] == "HAPPY_CARO_SERVER")
                        {
                            _running = false;
                            OnServerFound?.Invoke(p[1], int.Parse(p[2]));
                        }
                    }
                    catch
                    {
                        break;
                    }
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
