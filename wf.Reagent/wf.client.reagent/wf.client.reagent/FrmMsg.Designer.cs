namespace wf.client.reagent
{
    partial class FrmMsg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMsg));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Close = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xrStoreHigh = new DevExpress.XtraTab.XtraTabPage();
            this.gcStoreHigh = new DevExpress.XtraGrid.GridControl();
            this.gvStoreHigh = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xrStoreLow = new DevExpress.XtraTab.XtraTabPage();
            this.gcStoreLow = new DevExpress.XtraGrid.GridControl();
            this.gvStoreLow = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xrDueSoon = new DevExpress.XtraTab.XtraTabPage();
            this.gcDueSoon = new DevExpress.XtraGrid.GridControl();
            this.gvDueSoon = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xrExpired = new DevExpress.XtraTab.XtraTabPage();
            this.gcExpired = new DevExpress.XtraGrid.GridControl();
            this.gvExpired = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xrStoreHigh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcStoreHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvStoreHigh)).BeginInit();
            this.xrStoreLow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcStoreLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvStoreLow)).BeginInit();
            this.xrDueSoon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDueSoon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDueSoon)).BeginInit();
            this.xrExpired.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcExpired)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExpired)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btn_Close);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 480);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(888, 40);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_Close
            // 
            this.btn_Close.Location = new System.Drawing.Point(790, 6);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(75, 23);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.Text = "关闭";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xrStoreHigh;
            this.xtraTabControl1.Size = new System.Drawing.Size(888, 480);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xrStoreHigh,
            this.xrStoreLow,
            this.xrDueSoon,
            this.xrExpired});
            // 
            // xrStoreHigh
            // 
            this.xrStoreHigh.Controls.Add(this.gcStoreHigh);
            this.xrStoreHigh.Name = "xrStoreHigh";
            this.xrStoreHigh.Size = new System.Drawing.Size(882, 451);
            this.xrStoreHigh.Text = "库存低";
            // 
            // gcStoreHigh
            // 
            this.gcStoreHigh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcStoreHigh.Location = new System.Drawing.Point(0, 0);
            this.gcStoreHigh.MainView = this.gvStoreHigh;
            this.gcStoreHigh.Name = "gcStoreHigh";
            this.gcStoreHigh.Size = new System.Drawing.Size(882, 451);
            this.gcStoreHigh.TabIndex = 0;
            this.gcStoreHigh.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvStoreHigh});
            // 
            // gvStoreHigh
            // 
            this.gvStoreHigh.GridControl = this.gcStoreHigh;
            this.gvStoreHigh.Name = "gvStoreHigh";
            this.gvStoreHigh.OptionsView.ColumnAutoWidth = false;
            this.gvStoreHigh.OptionsView.ShowDetailButtons = false;
            this.gvStoreHigh.OptionsView.ShowGroupPanel = false;
            this.gvStoreHigh.OptionsView.ShowIndicator = false;
            // 
            // xrStoreLow
            // 
            this.xrStoreLow.Controls.Add(this.gcStoreLow);
            this.xrStoreLow.Name = "xrStoreLow";
            this.xrStoreLow.Size = new System.Drawing.Size(882, 451);
            this.xrStoreLow.Text = "库存高";
            // 
            // gcStoreLow
            // 
            this.gcStoreLow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcStoreLow.Location = new System.Drawing.Point(0, 0);
            this.gcStoreLow.MainView = this.gvStoreLow;
            this.gcStoreLow.Name = "gcStoreLow";
            this.gcStoreLow.Size = new System.Drawing.Size(882, 451);
            this.gcStoreLow.TabIndex = 0;
            this.gcStoreLow.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvStoreLow});
            // 
            // gvStoreLow
            // 
            this.gvStoreLow.GridControl = this.gcStoreLow;
            this.gvStoreLow.Name = "gvStoreLow";
            this.gvStoreLow.OptionsView.ColumnAutoWidth = false;
            this.gvStoreLow.OptionsView.ShowDetailButtons = false;
            this.gvStoreLow.OptionsView.ShowGroupPanel = false;
            this.gvStoreLow.OptionsView.ShowIndicator = false;
            // 
            // xrDueSoon
            // 
            this.xrDueSoon.Controls.Add(this.gcDueSoon);
            this.xrDueSoon.Name = "xrDueSoon";
            this.xrDueSoon.Size = new System.Drawing.Size(882, 451);
            this.xrDueSoon.Text = "即将到期";
            // 
            // gcDueSoon
            // 
            this.gcDueSoon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDueSoon.Location = new System.Drawing.Point(0, 0);
            this.gcDueSoon.MainView = this.gvDueSoon;
            this.gcDueSoon.Name = "gcDueSoon";
            this.gcDueSoon.Size = new System.Drawing.Size(882, 451);
            this.gcDueSoon.TabIndex = 0;
            this.gcDueSoon.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDueSoon});
            // 
            // gvDueSoon
            // 
            this.gvDueSoon.GridControl = this.gcDueSoon;
            this.gvDueSoon.Name = "gvDueSoon";
            this.gvDueSoon.OptionsView.ColumnAutoWidth = false;
            this.gvDueSoon.OptionsView.ShowDetailButtons = false;
            this.gvDueSoon.OptionsView.ShowGroupPanel = false;
            this.gvDueSoon.OptionsView.ShowIndicator = false;
            // 
            // xrExpired
            // 
            this.xrExpired.Controls.Add(this.gcExpired);
            this.xrExpired.Name = "xrExpired";
            this.xrExpired.Size = new System.Drawing.Size(882, 451);
            this.xrExpired.Text = "已到期";
            // 
            // gcExpired
            // 
            this.gcExpired.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcExpired.Location = new System.Drawing.Point(0, 0);
            this.gcExpired.MainView = this.gvExpired;
            this.gcExpired.Name = "gcExpired";
            this.gcExpired.Size = new System.Drawing.Size(882, 451);
            this.gcExpired.TabIndex = 0;
            this.gcExpired.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExpired});
            // 
            // gvExpired
            // 
            this.gvExpired.GridControl = this.gcExpired;
            this.gvExpired.Name = "gvExpired";
            this.gvExpired.OptionsView.ColumnAutoWidth = false;
            this.gvExpired.OptionsView.ShowDetailButtons = false;
            this.gvExpired.OptionsView.ShowGroupPanel = false;
            this.gvExpired.OptionsView.ShowIndicator = false;
            // 
            // FrmMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 520);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMsg";
            this.Text = "警示窗口";
            this.Load += new System.EventHandler(this.FrmMsg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xrStoreHigh.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcStoreHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvStoreHigh)).EndInit();
            this.xrStoreLow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcStoreLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvStoreLow)).EndInit();
            this.xrDueSoon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDueSoon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDueSoon)).EndInit();
            this.xrExpired.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcExpired)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExpired)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Close;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xrStoreHigh;
        private DevExpress.XtraTab.XtraTabPage xrStoreLow;
        private DevExpress.XtraTab.XtraTabPage xrDueSoon;
        private DevExpress.XtraTab.XtraTabPage xrExpired;
        private DevExpress.XtraGrid.GridControl gcStoreHigh;
        private DevExpress.XtraGrid.Views.Grid.GridView gvStoreHigh;
        private DevExpress.XtraGrid.GridControl gcStoreLow;
        private DevExpress.XtraGrid.Views.Grid.GridView gvStoreLow;
        private DevExpress.XtraGrid.GridControl gcDueSoon;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDueSoon;
        private DevExpress.XtraGrid.GridControl gcExpired;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExpired;
    }
}