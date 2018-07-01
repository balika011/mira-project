using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiraToolkit.Controls;
using MiraToolkit.Core;

namespace MiraToolkit.Controls.Generic
{
    public partial class frmDeviceSelector : Form
    {
        public delegate void ConnectCallback(string ip);
        ConnectCallback m_cb;

        public frmDeviceSelector(ConnectCallback callback)
        {
            InitializeComponent();

            this.miraSelector.setConnectCallback(miraSelector_ConnectCallback);
            this.m_cb = callback;
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

        private async void btnEnterIP_Click(object sender, EventArgs e)
        {
            var s_Dialog = new frmIpAddress();

            if (s_Dialog.ShowDialog() != DialogResult.OK)
                return;

            this.miraSelector.AddDevice(s_Dialog.IPAddress, MiraToolkit.Controls.DeviceType.LOADING, "Loading...");

            var task = new Task<Tuple<string, bool>>(() => IsMiraIP(s_Dialog.IPAddress));
            task.Start();

            await task;

            if (!task.Result.Item2)
            {
                MessageBox.Show($"Could not connect to: {s_Dialog.IPAddress}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.miraSelector.RemoveDevice(s_Dialog.IPAddress);
                return;
            }

            // TODO: Detect if it's a PS4 / Slim or Pro, get the hostname.
            this.miraSelector.UpdateDevice(s_Dialog.IPAddress, MiraToolkit.Controls.DeviceType.PS4, "PS4");
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            /*this.miraSelector.AddDevice(MiraToolkit.Controls.DeviceType.PS4, "My PS4 @ 1.76", "192.168.0.1");
            this.miraSelector.AddDevice(MiraToolkit.Controls.DeviceType.PS4SLIM, "Bro's PS4 Slim @ 4.50", "192.168.0.2");
            this.miraSelector.AddDevice(MiraToolkit.Controls.DeviceType.PS4PRO, "PS4 Pro @ 5.05", "192.168.0.3");*/

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

                    this.miraSelector.AddDevice(pingTask.Result.Address.ToString(), MiraToolkit.Controls.DeviceType.LOADING, "Loading...");

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
                    this.miraSelector.UpdateDevice(connectTask.Result.Item1, MiraToolkit.Controls.DeviceType.PS4, "PS4");
                }
                else
                {
                    this.miraSelector.RemoveDevice(connectTask.Result.Item1);
                }
            }

            Console.WriteLine("Connect Complete");
        }

        private void miraSelector_ConnectCallback(string ip)
        {
            this.Close();

            if (this.m_cb != null)
                this.m_cb(ip);
        }
    }
}
