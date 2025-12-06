using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace Client
{
    public class TcpClientHelper
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _receiveThread;
        private Thread _heartbeatThread;

        private readonly ConcurrentQueue<string> _sendQueue = new();
        private readonly CancellationTokenSource _cts = new();

        private bool _isConnected = false;
        private string _serverIp;
        private int _serverPort;

        // ========= CALLBACK =============
        public event Action<string> OnMessageReceived;
        public event Action OnDisconnected;
        public event Action OnConnected;

        // ========= LOGGING =============
        private readonly string _logPath = "client-log.txt";

        // ========= CONNECT =============
        public void Connect(string ip, int port)
        {
            _serverIp = ip;
            _serverPort = port;

            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        Log("Connecting...");
                        _client = new TcpClient();
                        _client.Connect(ip, port);
                        _stream = _client.GetStream();

                        _isConnected = true;
                        OnConnected?.Invoke();

                        StartReceiving();
                        StartHeartbeat();
                        StartSendWorker();

                        Log("Connected");
                        break;
                    }
                    catch
                    {
                        Log("Connect failed. Retry in 3s...");
                        Thread.Sleep(3000);
                    }
                }
            });
        }

        // ========= QUEUE BASED SEND =============
        public void Send(string message)
        {
            if (!_isConnected) return;
            _sendQueue.Enqueue(message);
        }

        private void StartSendWorker()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                while (_isConnected && !_cts.IsCancellationRequested)
                {
                    if (_sendQueue.TryDequeue(out string msg))
                    {
                        try
                        {
                            byte[] data = Encoding.UTF8.GetBytes(msg + "\n");
                            _stream.Write(data, 0, data.Length);
                        }
                        catch
                        {
                            Log("Send failed.");
                            TriggerDisconnect();
                        }
                    }

                    Thread.Sleep(10);
                }
            });
        }

        // ========= RECEIVING =============
        private void StartReceiving()
        {
            _receiveThread = new Thread(() =>
            {
                byte[] buffer = new byte[4096];

                while (_isConnected && !_cts.IsCancellationRequested)
                {
                    try
                    {
                        int bytes = _stream.Read(buffer, 0, buffer.Length);
                        if (bytes <= 0)
                        {
                            TriggerDisconnect();
                            break;
                        }

                        string message = Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
                        UIInvoke(() => OnMessageReceived?.Invoke(message));
                    }
                    catch
                    {
                        TriggerDisconnect();
                    }
                }
            });

            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }

        // ========= HEARTBEAT ============
        private void StartHeartbeat()
        {
            _heartbeatThread = new Thread(() =>
            {
                while (_isConnected && !_cts.IsCancellationRequested)
                {
                    Send("{\"type\":\"PING\"}");
                    Thread.Sleep(3000);
                }
            });

            _heartbeatThread.IsBackground = true;
            _heartbeatThread.Start();
        }

        // ========= DISCONNECT ============
        private void TriggerDisconnect()
        {
            if (!_isConnected) return;

            _isConnected = false;
            Log("Disconnected");

            UIInvoke(() => OnDisconnected?.Invoke());

            Connect(_serverIp, _serverPort); // AUTO RECONNECT
        }

        // ========= UI SAFE INVOKE ============
        private void UIInvoke(Action action)
        {
            if (System.Windows.Forms.Application.OpenForms.Count > 0)
            {
                var form = System.Windows.Forms.Application.OpenForms[0];
                if (form.InvokeRequired)
                    form.BeginInvoke(action);
                else
                    action();
            }
            else action();
        }

        // ========= LOGGING =============
        private void Log(string msg)
        {
            File.AppendAllText(_logPath, $"[{DateTime.Now}] {msg}\n");
        }
    }
}
