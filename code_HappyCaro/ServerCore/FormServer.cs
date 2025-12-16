using ServerCore.ServerCore;
using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace ServerCore
{
    public partial class FormServer : Form
    {
        private Server _server;
        private DiscoveryBroadcaster _discovery;
        private bool _lanMode = false; // false = LOCAL | true = LAN


        public FormServer()
        {
            InitializeComponent();
            LoadIPAddress();

            Server.OnLog = Log;
            Server.OnClientListChanged = RefreshClientList;
            RoomManager.OnRoomListChanged = RefreshRoomList;
        }

        private void LoadIPAddress()
        {
            lblIP.Text = Dns.GetHostEntry(Dns.GetHostName())
                           .AddressList.FirstOrDefault(ip =>
                                ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                           .ToString()
                           ?? "127.0.0.1";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtPort.Text, out int port))
            {
                Log("Invalid port.");
                return;
            }

            _server = new Server(port);
            _server.Start();

            if (_lanMode)
            {
                string ip = lblIP.Text; // IP LAN đã load sẵn
                _discovery = new DiscoveryBroadcaster(ip, port);
                _discovery.Start();
            }

            btnStart.Enabled = false;
            btnStop.Enabled = true;

            lblStatus.Text = "Status: RUNNING";
            lblStatus.ForeColor = System.Drawing.Color.MediumSeaGreen;

            Log($"Server started at {lblIP.Text}:{port}");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_server == null) return;
            _discovery?.Stop();
            _discovery = null;
            _server.Stop();

            btnStart.Enabled = true;
            btnStop.Enabled = false;

            lblStatus.Text = "Status: STOPPED";
            lblStatus.ForeColor = System.Drawing.Color.IndianRed;

            listClients.Items.Clear();
            listRooms.Items.Clear();

            Log("Server stopped.");
        }

        private void Log(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(Log), msg);
                return;
            }

            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
        }

        public void RefreshClientList()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshClientList));
                return;
            }

            listClients.Items.Clear();

            foreach (var c in Server.Clients)
                listClients.Items.Add($"{c.Username ?? "(null)"} - {c.EndPoint}");
        }

        public void RefreshRoomList()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshRoomList));
                return;
            }

            var rms = GameCore.RoomManager.GetRoomSnapshot();

            listRooms.Items.Clear();

            foreach (var r in rms)
                listRooms.Items.Add(
                    $"Room {r.Id}: {r.Player1?.Username ?? "-"} vs {r.Player2?.Username ?? "-"} - {r.Status}"
                );
        }

    }
}
