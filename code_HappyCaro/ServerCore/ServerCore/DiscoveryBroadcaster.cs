using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace ServerCore.ServerCore
{
    public class DiscoveryBroadcaster
    {
        private UdpClient _udp;
        private Timer _timer;
        private readonly string _serverIp;
        private readonly int _port;
        private const int DISCOVERY_PORT = 9998;

        public DiscoveryBroadcaster(string serverIp, int port)
        {
            _serverIp = serverIp ?? "0.0.0.0";
            _port = port;
            _udp = new UdpClient();
            _udp.EnableBroadcast = true;

            _timer = new Timer(2000);
            _timer.Elapsed += Broadcast;
            _timer.AutoReset = true;
        }

        public void Start()
        {
            try
            {
                _timer.Start();
                Server.Log("LAN Discovery started.");
            }
            catch (Exception ex)
            {
                Server.Log($"Discovery start error: {ex.Message}");
            }
        }

        public void Stop()
        {
            try
            {
                _timer.Stop();
                _udp.Close();
                Server.Log("LAN Discovery stopped.");
            }
            catch (Exception ex)
            {
                Server.Log($"Discovery stop error: {ex.Message}");
            }
        }

        private void Broadcast(object sender, ElapsedEventArgs e)
        {
            try
            {
                string msg = $"HAPPY_CARO_SERVER|{_serverIp}|{_port}";
                byte[] dat = Encoding.UTF8.GetBytes(msg);
                _udp.Send(dat, dat.Length, new IPEndPoint(IPAddress.Broadcast, DISCOVERY_PORT));
            }
            catch (Exception ex)
            {
                Server.Log($"Discovery broadcast error: {ex.Message}");
            }
        }
    }
}
