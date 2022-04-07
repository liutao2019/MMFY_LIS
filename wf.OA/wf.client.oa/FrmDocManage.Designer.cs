namespace dcl.client.oa
{
    partial class FrmDocManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDocManage));
            this.orderMain = new dcl.client.oa.OrderDetail();
            this.orderDetail = new dcl.client.oa.OrderDetail();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // orderMain
            // 
            this.orderMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderMain.Location = new System.Drawing.Point(19, 60);
            this.orderMain.Margin = new System.Windows.Forms.Padding(0);
            this.orderMain.Name = "orderMain";
            this.orderMain.QuickOption = false;
            this.orderMain.ShowSearch = true;
            this.orderMain.Size = new System.Drawing.Size(1795, 585);
            this.orderMain.TabIndex = 2;
            // 
            // orderDetail
            // 
            this.orderDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderDetail.Enabled = false;
            this.orderDetail.Location = new System.Drawing.Point(19, 60);
            this.orderDetail.Margin = new System.Windows.Forms.Padding(0);
            this.orderDetail.Name = "orderDetail";
            this.orderDetail.QuickOption = false;
            this.orderDetail.ShowSearch = true;
            this.orderDetail.Size = new System.Drawing.Size(1795, 565);
            this.orderDetail.TabIndex = 2;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.orderMain);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.groupControl1.Size = new System.Drawing.Size(1833, 664);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "文档分类";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.orderDetail);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 664);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Padding = new System.Windows.Forms.Padding(16, 16, 16, 16);
            this.groupControl2.Size = new System.Drawing.Size(1833, 644);
            this.groupControl2.TabIndex = 5;
            this.groupControl2.Text = "文档资料";
            // 
            // FrmDocManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1833, 1308);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmDocManage";
            this.Text = "文档资料管理";
            this.Load += new System.EventHandler(this.FrmDocManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private OrderDetail orderMain;
        private OrderDetail orderDetail;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}