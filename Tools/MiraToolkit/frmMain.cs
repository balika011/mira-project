using MiraToolkit.Core;
using MiraToolkit.Controls.Generic;

using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;

using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading.Tasks;

namespace MiraToolkit
{
    public partial class frmMain : Form
    {
        // List of the current devices
        List<MiraDevice> m_Devices;
        CountdownEvent m_countdown;
        object m_lockObj = new object();
        List<string> m_activeHosts;

        public frmMain()
        {
            InitializeComponent();

            m_Devices = new List<MiraDevice>();
            m_activeHosts = new List<string>();

            Program.DockPanel = dockPanel;
        }

        private async void mmuConnect_Click(object sender, EventArgs e)
        {
            var x = new frmDeviceSelector(ConnectToDevice);
            x.ShowDialog();
        }

        private void ConnectToDevice(string ip)
        {
            var s_Device = new MiraDevice(ip, 9999);

            var s_Result = s_Device.Connection.Connect();

            if (!s_Result)
            {
                MessageBox.Show($"Could not connect to: {s_Device.Hostname}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadUIForDevice(s_Device);

            m_Devices.Add(s_Device);
        }

        private void LoadUIForDevice(MiraDevice p_Device)
        {
            //var s_DevConsoleLog = new MiraConsole(p_Device, 9998, "dev_console_log.txt");

            //p_Device.AddConsole(9998);

            new Controls.FileTransfer.ucFileTransfer(p_Device).Show(Program.DockPanel, DockState.DockRight);
        }
    }
}
