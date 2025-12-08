using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class TcpClientHelper : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private readonly ConcurrentQueue<byte[]> _sendQueue = new ConcurrentQueue<byte[]>();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _sendTask;
        private Task _receiveTask;
        private Task _heartbeatTask;

        public event Action<MessageEnvelope> OnEnvelopeReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;
        public bool IsConnected => _client?.Connected ?? false;

        private string _serverIp;
        private int _serverPort;

        public void Connect(string ip, int port)
        {
            _serverIp = ip; _serverPort = port;
            _ = Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        _client = new TcpClient();
                        await _client.ConnectAsync(ip, port);
                        _stream = _client.GetStream();
                        OnConnected?.Invoke();

                        _sendTask = Task.Run(SendLoop);
                        _receiveTask = Task.Run(ReceiveLoop);
                        _heartbeatTask = Task.Run(HeartbeatLoop);

                        break;
                    }
                    catch
                    {
                        await Task.Delay(3000);
                    }
                }
            });
        }

        public void EnqueueSend(MessageEnvelope env)
        {
            string json = JsonHelper.Serialize(env);
            var body = Encoding.UTF8.GetBytes(json);
            var prefix = BitConverter.GetBytes(body.Length);
            var packet = new byte[prefix.Length + body.Length];
            Buffer.BlockCopy(prefix, 0, packet, 0, prefix.Length);
            Buffer.BlockCopy(body, 0, packet, prefix.Length, body.Length);
            _sendQueue.Enqueue(packet);
        }

        private async Task SendLoop()
        {
            while (!_cts.IsCancellationRequested && _client?.Connected == true)
            {
                try
                {
                    if (_sendQueue.TryDequeue(out var pkt))
                    {
                        await _stream.WriteAsync(pkt, 0, pkt.Length, _cts.Token);
                    }
                    else await Task.Delay(10, _cts.Token);
                }
                catch
                {
                    TriggerDisconnect();
                }
            }
        }

        private async Task ReceiveLoop()
        {
            try
            {
                var lenBuf = new byte[4];
                while (!_cts.IsCancellationRequested && _client?.Connected == true)
                {
                    int read = 0;
                    while (read < 4)
                    {
                        int r = await _stream.ReadAsync(lenBuf, read, 4 - read, _cts.Token);
                        if (r == 0) { TriggerDisconnect(); return; }
                        read += r;
                    }

                    int len = BitConverter.ToInt32(lenBuf, 0);
                    if (len <= 0) continue;

                    var body = new byte[len];
                    int total = 0;
                    while (total < len)
                    {
                        int r = await _stream.ReadAsync(body, total, len - total, _cts.Token);
                        if (r == 0) { TriggerDisconnect(); return; }
                        total += r;
                    }

                    var json = Encoding.UTF8.GetString(body);
                    try
                    {
                        var env = JsonHelper.Deserialize<MessageEnvelope>(json);
                        OnEnvelopeReceived?.Invoke(env);
                    }
                    catch
                    {
                        // ignore malformed
                    }
                }
            }
            catch
            {
                TriggerDisconnect();
            }
        }

        private async Task HeartbeatLoop()
        {
            while (!_cts.IsCancellationRequested && _client?.Connected == true)
            {
                try
                {
                    EnqueueSend(new MessageEnvelope { Type = MessageType.PING, Payload = "" });
                    await Task.Delay(3000, _cts.Token);
                }
                catch { await Task.Delay(3000); }
            }
        }

        private void TriggerDisconnect()
        {
            try { _cts.Cancel(); } catch { }
            try { _client?.Close(); } catch { }
            OnDisconnected?.Invoke();

            // attempt reconnect
            _cts = new CancellationTokenSource();
            Task.Delay(1000).ContinueWith(_ => Connect(_serverIp, _serverPort));
        }

        public void Dispose()
        {
            try { _cts.Cancel(); } catch { }
            try { _client?.Close(); } catch { }
        }
    }
}
