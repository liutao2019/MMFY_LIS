namespace dcl.client.report
{
    partial class FrmServerReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmServerReport));
            this.gcServerReport = new DevExpress.XtraGrid.GridControl();
            this.gvServerReport = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sysReport = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.gcServerReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvServerReport)).BeginInit();
            this.SuspendLayout();
            // 
            // gcServerReport
            // 
            this.gcServerReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcServerReport.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.gcServerReport.Location = new System.Drawing.Point(0, 0);
            this.gcServerReport.MainView = this.gvServerReport;
            this.gcServerReport.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.gcServerReport.Name = "gcServerReport";
            this.gcServerReport.Size = new System.Drawing.Size(756, 527);
            this.gcServerReport.TabIndex = 0;
            this.gcServerReport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvServerReport});
            // 
            // gvServerReport
            // 
            this.gvServerReport.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gvServerReport.GridControl = this.gcServerReport;
            this.gvServerReport.Name = "gvServerReport";
            this.gvServerReport.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvServerReport.OptionsView.ShowGroupPanel = false;
            this.gvServerReport.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "服务器版本";
            this.gridColumn1.FieldName = "reportName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // sysReport
            // 
            this.sysReport.AutoCloseButton = true;
            this.sysReport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysReport.Location = new System.Drawing.Point(0, 527);
            this.sysReport.Margin = new System.Windows.Forms.Padding(13, 15, 13, 15);
            this.sysReport.Name = "sysReport";
            this.sysReport.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysReport.NotWriteLogButtonNameList")));
            this.sysReport.ShowItemToolTips = false;
            this.sysReport.Size = new System.Drawing.Size(756, 145);
            this.sysReport.TabIndex = 1;
            this.sysReport.OnBtnConfirmClicked += new System.EventHandler(this.sysReport_OnBtnConfirmClicked);
            // 
            // FrmServerReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 672);
            this.Controls.Add(this.gcServerReport);
            this.Controls.Add(this.sysReport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmServerReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "服务器版本";
            this.Load += new System.EventHandler(this.FrmServerReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcServerReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvServerReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcServerReport;
        private DevExpress.XtraGrid.Views.Grid.GridView gvServerReport;
        private dcl.client.common.SysToolBar sysReport;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}