using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace ServerCore
{
    public class DiscoveryBroadcaster
    {
        private UdpClient udp;
        private Timer timer;
        private string serverIp;
        private int port;
        private const int DISCOVERY_PORT = 9998;

        public DiscoveryBroadcaster(string serverIp, int port)
        {
            this.serverIp = serverIp ?? "0.0.0.0";
            this.port = port;

            udp = new UdpClient();
            udp.EnableBroadcast = true;

            timer = new Timer(2000);
            timer.Elapsed += Broadcast;
            timer.AutoReset = true;
        }

        public void Start()
        {
            try
            {
                timer.Start();
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
                timer.Stop();
                udp.Close();
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
                string msg = $"HappyCaroServer|{serverIp}|{port}";
                byte[] dat = Encoding.UTF8.GetBytes(msg);
                udp.Send(dat, dat.Length, new IPEndPoint(IPAddress.Broadcast, DISCOVERY_PORT));
            }
            catch (Exception ex)
            {
                // ignore send errors but log at debug level
                Server.Log($"Discovery broadcast error: {ex.Message}");
            }
        }
    }
}