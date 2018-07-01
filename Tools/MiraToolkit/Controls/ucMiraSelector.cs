using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace MiraToolkit.Controls
{
    public enum DeviceType
    {
        LOADING,
        PS4,
        PS4SLIM,
        PS4PRO
    }

    public partial class ucMiraSelector : UserControl
    {
        public delegate void ConnectCallback(string ip);
        private ConnectCallback m_cb;

        private void loadImages()
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.ColorDepth = ColorDepth.Depth32Bit;

            imageList.Images.Add("loading", Properties.Resources.question);
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

        public ucMiraSelector()
        {
            InitializeComponent();

            loadImages();
            createHeaders();

            this.lstDevices.Font = new Font("Segoe UI", 10f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);

            this.lstDevices.ItemActivate += this.lstDevices_ItemActivate;
        }

        public void setConnectCallback(ConnectCallback cb)
        {
            this.m_cb = cb;
        }

        private void lstDevices_ItemActivate(object sender, EventArgs e)
        {
            System.Windows.Forms.ListView lw = (System.Windows.Forms.ListView)sender;

            if (this.m_cb != null)
                this.m_cb(lw.SelectedItems[0].SubItems[1].Text);
        }

        public void AddDevice(string ip, DeviceType type, string name)
        {
            this.lstDevices.BeginUpdate();

            int imageIndex = 0;

            switch(type)
            {
                case DeviceType.LOADING:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("loading");
                    break;
                case DeviceType.PS4:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("ps4");
                    break;
                case DeviceType.PS4SLIM:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("ps4slim");
                    break;
                case DeviceType.PS4PRO:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("ps4pro");
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

        public void RemoveDevice(string ip)
        {
            this.lstDevices.BeginUpdate();

            var index = -1;

            for (var i = 0; i < this.lstDevices.Items.Count; i++)
            {
                if (this.lstDevices.Items[i].SubItems[1].Text == ip)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
                this.lstDevices.Items.RemoveAt(index);

            this.lstDevices.EndUpdate();

        }

        public void UpdateDevice(string ip, DeviceType type, string name)
        {
            this.lstDevices.BeginUpdate();

            int imageIndex = 0;

            switch (type)
            {
                case DeviceType.LOADING:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("loading");
                    break;
                case DeviceType.PS4:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("ps4");
                    break;
                case DeviceType.PS4SLIM:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("ps4slim");
                    break;
                case DeviceType.PS4PRO:
                    imageIndex = this.lstDevices.LargeImageList.Images.IndexOfKey("ps4pro");
                    break;
            }

            for (var i = 0; i < this.lstDevices.Items.Count; i++)
            {
                if (this.lstDevices.Items[i].SubItems[1].Text == ip)
                {
                    this.lstDevices.Items[i].Text = name;
                    this.lstDevices.Items[i].ImageIndex = imageIndex;
                    break;
                }
            }

            this.lstDevices.EndUpdate();
        }
    }
}
