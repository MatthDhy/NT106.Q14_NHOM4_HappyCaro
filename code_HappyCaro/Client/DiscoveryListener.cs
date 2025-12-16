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
        private bool _found = false;


        public event Action<string, int> OnServerFound;


        public void Start()
        {
            _udp = new UdpClient(DISCOVERY_PORT);
            Task.Run(async () =>
            {
                while (!_found)
                {
                    var res = await _udp.ReceiveAsync();
                    var msg = Encoding.UTF8.GetString(res.Buffer);
                    var p = msg.Split('|');
                    if (p.Length == 3 && p[0] == "HAPPY_CARO_SERVER")
                    {
                        _found = true;
                        OnServerFound?.Invoke(p[1], int.Parse(p[2]));
                        _udp.Close();
                    }
                }
            });
        }

        public void Stop()
        {
            try { _udp?.Close(); } catch { }
        }

    }
}
