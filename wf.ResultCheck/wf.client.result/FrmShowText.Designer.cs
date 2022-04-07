namespace dcl.client.result
{
    partial class FrmShowText
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShowText));
            this.medShow = new DevExpress.XtraEditors.MemoEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExample = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.medShow.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // medShow
            // 
            this.medShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.medShow.Location = new System.Drawing.Point(0, 0);
            this.medShow.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.medShow.Name = "medShow";
            this.medShow.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.medShow.Properties.Appearance.Options.UseFont = true;
            this.medShow.Size = new System.Drawing.Size(1330, 870);
            this.medShow.TabIndex = 161;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExample);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 870);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1330, 83);
            this.panel1.TabIndex = 162;
            // 
            // btnExample
            // 
            this.btnExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExample.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnExample.Location = new System.Drawing.Point(1053, 10);
            this.btnExample.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnExample.Name = "btnExample";
            this.btnExample.Size = new System.Drawing.Size(234, 60);
            this.btnExample.TabIndex = 152;
            this.btnExample.Text = "备注范文";
            this.btnExample.Click += new System.EventHandler(this.btnExample_Click);
            // 
            // FrmShowText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 953);
            this.Controls.Add(this.medShow);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmShowText";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文本显示";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmShowText_FormClosing);
            this.Load += new System.EventHandler(this.FrmShowText_Load);
            ((System.ComponentModel.ISupportInitialize)(this.medShow.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit medShow;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnExample;
    }
}