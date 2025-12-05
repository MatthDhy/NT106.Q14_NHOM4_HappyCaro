using ServerCore.ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    internal class Server
    {
        private TcpListener listener;
        private int port;
        private bool isRunning;

        public static Action<string> OnLog;
        public static Action OnClientListChanged;
        public static Action OnRoomListChanged;
        public Server(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                isRunning = true;

                Log($"Server started on port {port}");
                AcceptLoop();
            }
            catch (Exception ex)
            {
                Log("Server start failed: " + ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                isRunning = false;
                listener.Stop();

                Log("Server stopped.");
            }
            catch (Exception ex)
            {
                Log("Error stopping server: " + ex.Message);
            }
        }
        private async Task AcceptLoop()
        {
            while (isRunning)
            {
                try
                {
                    var tcp = await listener.AcceptTcpClientAsync();
                    Log("Client connected: " + tcp.Client.RemoteEndPoint);

                    // ClientConnection constructor sẽ tự động thêm client vào AllClients với lock
                    new ClientConnection(tcp);

                    OnClientListChanged?.Invoke();
                }
                catch (Exception ex) // Bắt Exception để tránh crash server
                {
                    if (!isRunning) return;
                    Log("Accept error: " + ex.Message);
                }
            }
        }
        public static void Log(string msg)
        {
            Console.WriteLine(msg);
            OnLog?.Invoke(msg);
        }

        public static void Broadcast(object data)
        {
            string json = JsonHelper.Serialize(data);
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            // BẮT BUỘC: DUYỆT QUA DANH SÁCH CLIENT PHẢI CÓ LOCK
            lock (ClientConnection.AllClients)
            {
                foreach (var c in ClientConnection.AllClients)
                {
                    try
                    {
                        c.Send(data); // Hàm Send đã được tối ưu
                    }
                    catch (Exception ex)
                    {
                        Server.Log($"Broadcast send error to {c.Username}: {ex.Message}");
                        // Có thể thêm logic để ngắt kết nối client này nếu lỗi
                    }
                }
            }
        }

    }
}
