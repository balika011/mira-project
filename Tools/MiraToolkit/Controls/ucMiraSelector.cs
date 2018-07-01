using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiraToolkit.Controls
{
    public enum DeviceType
    {
        PS4,
        PS4SLIM,
        PS4PRO
    }

    public partial class ucMiraSelector : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public delegate void ConnectCallback(string ip);
        private ConnectCallback m_cb;

        private void loadImages()
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.ColorDepth = ColorDepth.Depth32Bit;

            imageList.Images.Add("ps4", Properties.Resources.ps4);
            imageList.Images.Add("ps4slim", Properties.Resources.ps4slim);
            imageList.Images.Add("ps4pro", Properties.Resources.ps4pro);

            this.lstDevices.LargeImageList = imageList;
            this.lstDevices.SmallImageList = imageList;
        }

        private void createHeaders()
        {
            ColumnHeader NameHeader = new ColumnHeader();
            NameHeader.Text = "Name";
            NameHeader.Width = 226;

            ColumnHeader IPHeader = new ColumnHeader();
            IPHeader.Text = "IP";
            IPHeader.Width = 0;

            this.lstDevices.Columns.AddRange(new ColumnHeader[2] { NameHeader, IPHeader });
        }

        public ucMiraSelector(ConnectCallback cb)
        {
            InitializeComponent();

            loadImages();
            createHeaders();

            this.lstDevices.Font = new Font("Segoe UI", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

            this.lstDevices.ItemActivate += new
                   System.EventHandler(this.lstDevices_ItemActivate);

            this.m_cb = cb;
        }

        private void lstDevices_ItemActivate(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView lw = (System.Windows.Forms.ListView)sender;

            this.m_cb(lw.SelectedItems[0].SubItems[1].Text);
        }

        public void AddDevice(DeviceType type, string name, string ip)
        {
            this.lstDevices.BeginUpdate();

            int imageIndex = 0;

            switch(type)
            {
                case DeviceType.PS4:
                    imageIndex = 0;
                    break;
                case DeviceType.PS4SLIM:
                    imageIndex = 1;
                    break;
                case DeviceType.PS4PRO:
                    imageIndex = 2;
                    break;
            }

            var device = new ListViewItem(name, imageIndex);

            var ipSubView = new ListViewItem.ListViewSubItem(device, ip);
            ipSubView.Font = new Font("Segoe UI", 9f);

            device.UseItemStyleForSubItems = false;
            device.SubItems.Add(ipSubView);

            this.lstDevices.Items.Add(device);

            this.lstDevices.EndUpdate();
        }
    }
}
