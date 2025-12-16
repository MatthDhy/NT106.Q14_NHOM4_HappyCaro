using System.Drawing;
using System.Windows.Forms;

namespace ServerCore
{
    partial class FormServer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelIP = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.listClients = new System.Windows.Forms.ListBox();
            this.listRooms = new System.Windows.Forms.ListBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelTop.Controls.Add(this.labelIP);
            this.panelTop.Controls.Add(this.lblIP);
            this.panelTop.Controls.Add(this.labelPort);
            this.panelTop.Controls.Add(this.txtPort);
            this.panelTop.Controls.Add(this.btnStart);
            this.panelTop.Controls.Add(this.btnStop);
            this.panelTop.Controls.Add(this.lblStatus);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(900, 70);
            this.panelTop.TabIndex = 2;
            // 
            // labelIP
            // 
            this.labelIP.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelIP.ForeColor = System.Drawing.Color.Black;
            this.labelIP.Location = new System.Drawing.Point(15, 12);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(75, 23);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "Server IP:";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblIP.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblIP.Location = new System.Drawing.Point(90, 12);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(0, 23);
            this.lblIP.TabIndex = 1;
            // 
            // labelPort
            // 
            this.labelPort.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelPort.Location = new System.Drawing.Point(15, 40);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(55, 23);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "Port:";
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtPort.Location = new System.Drawing.Point(94, 40);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(60, 24);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "8888";
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(249, 20);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(80, 30);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.IndianRed;
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(335, 20);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 30);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(430, 25);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(154, 25);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status: STOPPED";
            // 
            // splitMain
            // 
            this.splitMain.BackColor = System.Drawing.Color.White;
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 70);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.listClients);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.listRooms);
            this.splitMain.Size = new System.Drawing.Size(900, 420);
            this.splitMain.SplitterDistance = 121;
            this.splitMain.TabIndex = 0;
            // 
            // listClients
            // 
            this.listClients.BackColor = System.Drawing.Color.White;
            this.listClients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listClients.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listClients.ItemHeight = 23;
            this.listClients.Location = new System.Drawing.Point(0, 0);
            this.listClients.Name = "listClients";
            this.listClients.Size = new System.Drawing.Size(121, 420);
            this.listClients.TabIndex = 0;
            // 
            // listRooms
            // 
            this.listRooms.BackColor = System.Drawing.Color.White;
            this.listRooms.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listRooms.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.listRooms.ItemHeight = 23;
            this.listRooms.Location = new System.Drawing.Point(0, 0);
            this.listRooms.Name = "listRooms";
            this.listRooms.Size = new System.Drawing.Size(775, 420);
            this.listRooms.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 10F);
            this.txtLog.ForeColor = System.Drawing.Color.LightGreen;
            this.txtLog.Location = new System.Drawing.Point(0, 490);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(900, 160);
            this.txtLog.TabIndex = 1;
            // 
            // FormServer
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(900, 650);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.panelTop);
            this.Name = "FormServer";
            this.Text = "HappyCaro Server";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.ListBox listClients;
        private System.Windows.Forms.ListBox listRooms;

        private System.Windows.Forms.TextBox txtLog;
    }
}
