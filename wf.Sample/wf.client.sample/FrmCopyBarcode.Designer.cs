namespace dcl.client.sample
{
    partial class FrmCopyBarcode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCopyBarcode));
            this.txtMessageContent = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageContent.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtMessageContent
            // 
            this.txtMessageContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessageContent.Location = new System.Drawing.Point(0, 0);
            this.txtMessageContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMessageContent.Name = "txtMessageContent";
            this.txtMessageContent.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtMessageContent.Size = new System.Drawing.Size(523, 401);
            this.txtMessageContent.TabIndex = 3;
            // 
            // FrmCopyBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 401);
            this.Controls.Add(this.txtMessageContent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmCopyBarcode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "复制信息";
            this.Load += new System.EventHandler(this.FrmCopyBarcode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageContent.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit txtMessageContent;
    }
}