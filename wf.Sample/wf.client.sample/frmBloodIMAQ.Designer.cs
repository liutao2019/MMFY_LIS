namespace dcl.client.sample
{
    partial class frmBloodIMAQ
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBloodIMAQ));
            this.cameraControl2 = new Camera_NET.CameraControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.btncsetting = new System.Windows.Forms.ToolStripMenuItem();
            this.btnso = new System.Windows.Forms.ToolStripComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cameraControl2
            // 
            this.cameraControl2.DirectShowLogFilepath = "";
            this.cameraControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraControl2.Location = new System.Drawing.Point(0, 0);
            this.cameraControl2.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.cameraControl2.Name = "cameraControl2";
            this.cameraControl2.Size = new System.Drawing.Size(594, 448);
            this.cameraControl2.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btncsetting,
            this.btnso});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 83);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // btncsetting
            // 
            this.btncsetting.Name = "btncsetting";
            this.btncsetting.Size = new System.Drawing.Size(184, 36);
            this.btncsetting.Text = "画质调节";
            this.btncsetting.Click += new System.EventHandler(this.btncsetting_Click);
            // 
            // btnso
            // 
            this.btnso.Name = "btnso";
            this.btnso.Size = new System.Drawing.Size(121, 39);
            // 
            // frmBloodIMAQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 448);
            this.ControlBox = false;
            this.Controls.Add(this.cameraControl2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "frmBloodIMAQ";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "照片采集";
            this.TopMost = true;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private Camera_NET.CameraControl cameraControl2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btncsetting;
        private System.Windows.Forms.ToolStripComboBox btnso;
    }
}