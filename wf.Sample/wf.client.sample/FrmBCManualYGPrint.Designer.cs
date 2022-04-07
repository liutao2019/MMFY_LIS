namespace dcl.client.sample
{
    partial class FrmBCManualYGPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBCManualYGPrint));
            this.bcManualYg1 = new dcl.client.sample.BCManualYG();
            this.SuspendLayout();
            // 
            // bcManualYg1
            // 
            this.bcManualYg1.DeptCode = null;
            this.bcManualYg1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcManualYg1.IsAlone = false;
            this.bcManualYg1.Location = new System.Drawing.Point(0, 0);
            this.bcManualYg1.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.bcManualYg1.Name = "bcManualYg1";
            this.bcManualYg1.Size = new System.Drawing.Size(1248, 793);
            this.bcManualYg1.TabIndex = 0;
            // 
            // FrmBCManualYGPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 793);
            this.Controls.Add(this.bcManualYg1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmBCManualYGPrint";
            this.Text = "环境监测条码打印";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private BCManualYG bcManualYg1;
    }
}