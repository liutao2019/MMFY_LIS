namespace dcl.client.result
{
    partial class FrmResultView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmResultView));
            this.barSave = new dcl.client.common.SysToolBar();
            this.gdSysLog = new DevExpress.XtraGrid.GridControl();
            this.bsResult = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIsd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEcd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coChr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItrId = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // barSave
            // 
            this.barSave.AutoCloseButton = true;
            this.barSave.AutoEnableButtons = false;
            this.barSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barSave.Location = new System.Drawing.Point(0, 992);
            this.barSave.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.barSave.Name = "barSave";
            this.barSave.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("barSave.NotWriteLogButtonNameList")));
            this.barSave.QuickOption = false;
            this.barSave.ShowItemToolTips = false;
            this.barSave.Size = new System.Drawing.Size(1406, 145);
            this.barSave.TabIndex = 0;
            this.barSave.OnBtnSaveClicked += new System.EventHandler(this.barSave_OnBtnSaveClicked);
            // 
            // gdSysLog
            // 
            this.gdSysLog.DataSource = this.bsResult;
            this.gdSysLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSysLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gdSysLog.Location = new System.Drawing.Point(0, 0);
            this.gdSysLog.MainView = this.gridView1;
            this.gdSysLog.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gdSysLog.Name = "gdSysLog";
            this.gdSysLog.Size = new System.Drawing.Size(1406, 992);
            this.gdSysLog.TabIndex = 2;
            this.gdSysLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsResult
            // 
            this.bsResult.DataSource = typeof(dcl.entity.EntityObrResult);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIsd,
            this.colEcd,
            this.coChr,
            this.colItrId});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gdSysLog;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colIsd
            // 
            this.colIsd.Caption = "样本号";
            this.colIsd.FieldName = "ObrSid";
            this.colIsd.Name = "colIsd";
            this.colIsd.OptionsColumn.AllowEdit = false;
            this.colIsd.OptionsColumn.FixedWidth = true;
            this.colIsd.Visible = true;
            this.colIsd.VisibleIndex = 0;
            this.colIsd.Width = 95;
            // 
            // colEcd
            // 
            this.colEcd.Caption = "项目代码";
            this.colEcd.FieldName = "ItmEname";
            this.colEcd.Name = "colEcd";
            this.colEcd.OptionsColumn.AllowEdit = false;
            this.colEcd.OptionsColumn.FixedWidth = true;
            this.colEcd.Visible = true;
            this.colEcd.VisibleIndex = 2;
            this.colEcd.Width = 146;
            // 
            // coChr
            // 
            this.coChr.Caption = "测定结果";
            this.coChr.FieldName = "ObrValue";
            this.coChr.Name = "coChr";
            this.coChr.OptionsColumn.AllowEdit = false;
            this.coChr.OptionsColumn.FixedWidth = true;
            this.coChr.Visible = true;
            this.coChr.VisibleIndex = 3;
            this.coChr.Width = 209;
            // 
            // colItrId
            // 
            this.colItrId.Caption = "仪器编码";
            this.colItrId.FieldName = "ObrItrId";
            this.colItrId.Name = "colItrId";
            this.colItrId.OptionsColumn.AllowEdit = false;
            this.colItrId.Visible = true;
            this.colItrId.VisibleIndex = 1;
            this.colItrId.Width = 111;
            // 
            // FrmResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1406, 1137);
            this.Controls.Add(this.gdSysLog);
            this.Controls.Add(this.barSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmResultView";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "结果确认";
            this.Load += new System.EventHandler(this.FrmResultView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private dcl.client.common.SysToolBar barSave;
        private DevExpress.XtraGrid.GridControl gdSysLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colIsd;
        private DevExpress.XtraGrid.Columns.GridColumn colEcd;
        private DevExpress.XtraGrid.Columns.GridColumn coChr;
        private DevExpress.XtraGrid.Columns.GridColumn colItrId;
        private System.Windows.Forms.BindingSource bsResult;
    }
}