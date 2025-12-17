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

        // .NET 4.8 KHÔNG DÙNG new()
        private readonly ConcurrentQueue<byte[]> _sendQueue =
            new ConcurrentQueue<byte[]>();

        private CancellationTokenSource _cts;

        public event Action<MessageEnvelope> OnEnvelopeReceived;
        public event Action OnConnected;
        public event Action OnDisconnected;

        public bool IsConnected
        {
            get { return _client != null && _client.Connected; }
        }

        private string _ip;
        private int _port;

        public void Connect(string ip, int port)
        {
            _ip = ip;
            _port = port;

            Task.Run(async () =>
            {
                try
                {
                    _cts = new CancellationTokenSource();

                    _client = new TcpClient();
                    await _client.ConnectAsync(ip, port);

                    _stream = _client.GetStream();

                    if (OnConnected != null)
                        OnConnected();

                    Task.Run((Func<Task>)SendLoop);
                    Task.Run((Func<Task>)ReceiveLoop);
                }
                catch
                {
                    if (OnDisconnected != null)
                        OnDisconnected();
                }
            });
        }

        public void EnqueueSend(MessageEnvelope env)
        {
            if (!IsConnected) return;

            string json = JsonHelper.Serialize(env);
            byte[] body = Encoding.UTF8.GetBytes(json);
            byte[] len = BitConverter.GetBytes(body.Length);

            byte[] packet = new byte[len.Length + body.Length];
            Buffer.BlockCopy(len, 0, packet, 0, 4);
            Buffer.BlockCopy(body, 0, packet, 4, body.Length);

            _sendQueue.Enqueue(packet);
        }

        private async Task SendLoop()
        {
            try
            {
                while (!_cts.IsCancellationRequested && IsConnected)
                {
                    byte[] pkt;
                    if (_sendQueue.TryDequeue(out pkt))
                    {
                        await _stream.WriteAsync(pkt, 0, pkt.Length);
                    }
                    else
                    {
                        await Task.Delay(5);
                    }
                }
            }
            catch
            {
                TriggerDisconnect();
            }
        }

        private async Task ReceiveLoop()
        {
            try
            {
                byte[] lenBuf = new byte[4];

                while (!_cts.IsCancellationRequested && IsConnected)
                {
                    await ReadExact(lenBuf, 4);
                    int len = BitConverter.ToInt32(lenBuf, 0);

                    byte[] body = new byte[len];
                    await ReadExact(body, len);

                    string json = Encoding.UTF8.GetString(body);
                    MessageEnvelope env =
                        JsonHelper.Deserialize<MessageEnvelope>(json);

                    if (OnEnvelopeReceived != null)
                        OnEnvelopeReceived(env);
                }
            }
            catch
            {
                TriggerDisconnect();
            }
        }

        private async Task ReadExact(byte[] buf, int size)
        {
            int read = 0;
            while (read < size)
            {
                int r = await _stream.ReadAsync(buf, read, size - read);
                if (r == 0)
                    throw new Exception("Disconnected");

                read += r;
            }
        }

        private void TriggerDisconnect()
        {
            Dispose();

            if (OnDisconnected != null)
                OnDisconnected();
        }

        public void Dispose()
        {
            try
            {
                if (_cts != null)
                    _cts.Cancel();
            }
            catch { }

            try
            {
                if (_stream != null)
                    _stream.Close();
            }
            catch { }

            try
            {
                if (_client != null)
                    _client.Close();
            }
            catch { }
        }
    }
}
