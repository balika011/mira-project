namespace MiraToolkit.Controls.Generic
{
    partial class frmDeviceSelector
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnEnterIP = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.miraSelector = new ucMiraSelector();
            this.SuspendLayout();
            // 
            // btnEnterIP
            // 
            this.btnEnterIP.Location = new System.Drawing.Point(12, 292);
            this.btnEnterIP.Name = "btnEnterIP";
            this.btnEnterIP.Size = new System.Drawing.Size(75, 23);
            this.btnEnterIP.TabIndex = 0;
            this.btnEnterIP.Text = "Enter IP";
            this.btnEnterIP.UseVisualStyleBackColor = true;
            this.btnEnterIP.Click += new System.EventHandler(this.btnEnterIP_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(140, 292);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Seach";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // listView1
            // 
            this.miraSelector.Location = new System.Drawing.Point(13, 13);
            this.miraSelector.Name = "miraSelector";
            this.miraSelector.Size = new System.Drawing.Size(202, 273);
            this.miraSelector.TabIndex = 2;
            // 
            // frmDeviceSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 327);
            this.Controls.Add(this.miraSelector);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.btnEnterIP);
            this.Name = "frmDeviceSelector";
            this.Text = "Device Selector";
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Button btnEnterIP;
        private System.Windows.Forms.Button btnSearch;
        private ucMiraSelector miraSelector;
    }
}