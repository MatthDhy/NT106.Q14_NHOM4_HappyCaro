using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore.ServerCore
{
    public class Server
    {
        private TcpListener _listener;
        private readonly int _port;
        private CancellationTokenSource _cts;
        private Task _acceptTask;

        public static Action<string> OnLog;
        public static Action OnClientListChanged;
        public static Action OnRoomListChanged;

        public static List<ClientConnection> Clients { get; } = new List<ClientConnection>();

        public bool IsRunning { get; private set; }

        public Server(int port)
        {
            _port = port;
        }

        public void Start()
        {
            if (IsRunning) return;

            try
            {
                _cts = new CancellationTokenSource();
                _listener = new TcpListener(IPAddress.Any, _port);
                _listener.Start();
                IsRunning = true;

                Log($"Server started on port {_port}");
                _acceptTask = Task.Run(() => AcceptLoopAsync(_cts.Token));
            }
            catch (Exception ex)
            {
                Log($"Server start failed: {ex.Message}");
            }
        }

        public void Stop()
        {
            if (!IsRunning) return;

            try
            {
                _cts.Cancel();
                _listener.Stop();
                IsRunning = false;

                // Disconnect all clients
                lock (Clients)
                {
                    foreach (var c in Clients.ToArray())
                    {
                        try { c.ForceDisconnect(); } catch { }
                    }
                    Clients.Clear();
                }

                Log("Server stopped.");
                OnClientListChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Log($"Error stopping server: {ex.Message}");
            }
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var tcp = await _listener.AcceptTcpClientAsync();
                    Log($"Client connected: {tcp.Client.RemoteEndPoint}");

                    // Create client connection (it will add itself to Clients)
                    var cc = new ClientConnection(tcp);
                    OnClientListChanged?.Invoke();
                }
                catch (ObjectDisposedException) { break; }
                catch (Exception ex)
                {
                    if (token.IsCancellationRequested) break;
                    Log($"Accept error: {ex.Message}");
                }
            }
        }

        public static void Log(string msg)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");
            try { OnLog?.Invoke(msg); } catch { }
        }

        public static void Broadcast(MessageEnvelope env)
        {
            string json = JsonHelper.Serialize(env);
            byte[] body = Encoding.UTF8.GetBytes(json);
            byte[] prefix = BitConverter.GetBytes(body.Length);

            lock (Clients)
            {
                foreach (var c in Clients.ToArray())
                {
                    try
                    {
                        c.SendRaw(prefix, body);
                    }
                    catch (Exception ex)
                    {
                        Log($"Broadcast error to {c.Username}: {ex.Message}");
                    }
                }
            }
        }
    }
}
