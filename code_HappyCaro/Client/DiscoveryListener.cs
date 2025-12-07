using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class DiscoveryListener
    {
        public event Action<string, int>? OnServerFound;

        private bool _listening = false;

        public async void StartListening(int port = 8888)
        {
            _listening = true;
            UdpClient udp = new UdpClient(port);

            while (_listening)
            {
                try
                {
                    var result = await udp.ReceiveAsync();
                    string msg = Encoding.UTF8.GetString(result.Buffer);

                    string[] parts = msg.Split("|");
                    if (parts.Length == 3 && parts[0] == "HAPPY_CARO_SERVER")
                    {
                        string ip = parts[1];
                        int serverPort = int.Parse(parts[2]);

                        OnServerFound?.Invoke(ip, serverPort);
                    }
                }
                catch { }
            }
        }

        public void Stop()
        {
            _listening = false;
        }
    }
}
