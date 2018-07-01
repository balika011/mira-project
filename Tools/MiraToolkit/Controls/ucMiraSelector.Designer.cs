namespace MiraToolkit.Controls
{
    partial class ucMiraSelector
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lstDevices = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lstDevices
            // 
            this.lstDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDevices.Location = new System.Drawing.Point(0, 0);
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(392, 409);
            this.lstDevices.TabIndex = 0;
            this.lstDevices.UseCompatibleStateImageBehavior = false;
            this.lstDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left)));
            this.lstDevices.BackColor = System.Drawing.Color.White;
            this.lstDevices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstDevices.FullRowSelect = true;
            this.lstDevices.GridLines = true;
            this.lstDevices.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstDevices.Margin = new System.Windows.Forms.Padding(10);
            this.lstDevices.MultiSelect = false;
            this.lstDevices.ShowItemToolTips = true;
            this.lstDevices.TileSize = new System.Drawing.Size(209, 42);
            this.lstDevices.View = System.Windows.Forms.View.Tile;
            // 
            // ucMiraSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 409);
            this.Controls.Add(this.lstDevices);
            this.Name = "ucMiraSelector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstDevices;
    }
}
