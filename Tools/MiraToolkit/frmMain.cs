using MiraToolkit.Core;

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

        private Tuple<string, bool> IsMiraIP(string ip)
        {
            bool success = false;

            var s_Device = new MiraDevice(ip, 9999);

            if (s_Device.Connection.Connect())
            {
                s_Device.Connection.Disconnect();
                success = true;
            }

            return new Tuple<string, bool>(ip, success);
        }

        private async void mmuConnect_Click(object sender, EventArgs e)
        {
            var selector = new Controls.ucMiraSelector(ConnectToDevice);

            /*
            selector.AddDevice(MiraToolkit.Controls.DeviceType.PS4, "My PS4 @ 1.76", "192.168.0.1");
            selector.AddDevice(MiraToolkit.Controls.DeviceType.PS4SLIM, "Bro's PS4 Slim @ 4.50", "192.168.0.2");
            selector.AddDevice(MiraToolkit.Controls.DeviceType.PS4PRO, "PS4 Pro @ 5.05", "192.168.0.3");
            */

            List<Task<PingReply>> pingTasks = new List<Task<PingReply>>();

            Console.WriteLine("Scanning Network");

            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var x in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (x.Address.AddressFamily == AddressFamily.InterNetwork && x.IsDnsEligible)
                    {
                        Console.WriteLine(" IPAddress ........ : {0:x}", x.Address.ToString());
                        var subnet = x.Address.ToString();
                        subnet = subnet.Substring(0, subnet.LastIndexOf(".") + 1);

                        List<Task> tasks = new List<Task>();
                        for (int i = 1; i <= 255; i++)
                        {
                            string ip = subnet + i.ToString();

                            Ping ping = new Ping();

                            pingTasks.Add(ping.SendPingAsync(ip, 100));
                        }
                    }
                }
            }
            
            await Task.WhenAll(pingTasks.ToArray());

            Console.WriteLine("Network Scan Complete");

            List<Task<Tuple<string, bool>>> connectTasks = new List<Task<Tuple<string, bool>>>();

            foreach (var pingTask in pingTasks)
            {
                if (pingTask.Result.Status.Equals(IPStatus.Success))
                {
                    Console.WriteLine("{0} is up: ({1} ms)", pingTask.Result.Address.ToString(), pingTask.Result.RoundtripTime);

                    var task = new Task<Tuple<string, bool>>(() => IsMiraIP(pingTask.Result.Address.ToString()));
                    task.Start();

                    connectTasks.Add(task);
                }
            }

            await Task.WhenAll(connectTasks.ToArray());

            foreach (var connectTask in connectTasks)
            {
                if (connectTask.Result.Item2)
                {
                    // TODO: Detect if it's a PS4 / Slim or Pro, get the hostname.
                    selector.AddDevice(MiraToolkit.Controls.DeviceType.PS4, "PS4", connectTask.Result.Item1);
                }
            }

            Console.WriteLine("Connect Complete");

            selector.Show(Program.DockPanel, DockState.DockLeft);
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
