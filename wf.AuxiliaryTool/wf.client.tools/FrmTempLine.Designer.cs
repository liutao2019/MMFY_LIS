namespace dcl.client.tools
{
    partial class FrmTempLine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTempLine));
            this.conTLine1 = new dcl.client.tools.ConTempLine();
            this.SuspendLayout();
            // 
            // conTLine1
            // 
            this.conTLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conTLine1.Location = new System.Drawing.Point(0, 0);
            this.conTLine1.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.conTLine1.Name = "conTLine1";
            this.conTLine1.Size = new System.Drawing.Size(1255, 568);
            this.conTLine1.TabIndex = 0;
            // 
            // FrmTempLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 568);
            this.Controls.Add(this.conTLine1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmTempLine";
            this.Text = "温度变化图";
            this.Load += new System.EventHandler(this.FrmTempLine_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public ConTempLine conTLine1;
    }
}