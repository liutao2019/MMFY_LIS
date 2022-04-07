namespace dcl.client.result
{
    partial class FrmUndoRepotReson
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUndoRepotReson));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bsSex = new System.Windows.Forms.BindingSource(this.components);
            this.txt_Reson = new DevExpress.XtraEditors.MemoEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Reson.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "security_normal.ico");
            this.imageList1.Images.SetKeyName(1, "security_help.ico");
            this.imageList1.Images.SetKeyName(2, "security_infor.ico");
            this.imageList1.Images.SetKeyName(3, "security_import.ico");
            this.imageList1.Images.SetKeyName(4, "security_alert.ico");
            this.imageList1.Images.SetKeyName(5, "tab_danger.ico");
            // 
            // txt_Reson
            // 
            this.txt_Reson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_Reson.Location = new System.Drawing.Point(3, 44);
            this.txt_Reson.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txt_Reson.Name = "txt_Reson";
            this.txt_Reson.Size = new System.Drawing.Size(711, 216);
            this.txt_Reson.TabIndex = 154;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.txt_Reson);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(717, 263);
            this.groupControl2.TabIndex = 52;
            this.groupControl2.Text = "反审原因";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupControl2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(717, 263);
            this.panel1.TabIndex = 54;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 263);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(13, 15, 13, 15);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.QuickOption = false;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(717, 145);
            this.sysToolBar1.TabIndex = 53;
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.sysToolBar1_OnBtnConfirmClicked);
            // 
            // FrmUndoRepotReson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 408);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.sysToolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "FrmUndoRepotReson";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "反审原因";
            this.Load += new System.EventHandler(this.FrmUndoRepotReson_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Reson.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingSource bsSex;
        private System.Windows.Forms.ImageList imageList1;
        public DevExpress.XtraEditors.MemoEdit txt_Reson;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Panel panel1;
    }
}