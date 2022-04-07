namespace dcl.client.notifyclient
{
    partial class FrmOverTimeWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOverTimeWarning));
            this.coChr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barSave = new dcl.client.common.SysToolBar();
            this.gdSysLog = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIsd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEcd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.timer1 = new System.Windows.Forms.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // coChr
            // 
            this.coChr.Caption = "姓名";
            this.coChr.FieldName = "pat_name";
            this.coChr.Name = "coChr";
            this.coChr.OptionsColumn.AllowEdit = false;
            this.coChr.OptionsColumn.FixedWidth = true;
            this.coChr.Visible = true;
            this.coChr.VisibleIndex = 2;
            this.coChr.Width = 60;
            // 
            // barSave
            // 
            this.barSave.AutoCloseButton = true;
            this.barSave.AutoEnableButtons = false;
            this.barSave.Dock = System.Windows.Forms.DockStyle.Top;
            this.barSave.Location = new System.Drawing.Point(0, 0);
            this.barSave.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.barSave.Name = "barSave";
            this.barSave.NotWriteLogButtonNameList = null;
            this.barSave.QuickOption = false;
            this.barSave.ShowItemToolTips = false;
            this.barSave.Size = new System.Drawing.Size(1454, 131);
            this.barSave.TabIndex = 0;
            // 
            // gdSysLog
            // 
            this.gdSysLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSysLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gdSysLog.Location = new System.Drawing.Point(0, 131);
            this.gdSysLog.MainView = this.gridView1;
            this.gdSysLog.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gdSysLog.Name = "gdSysLog";
            this.gdSysLog.Size = new System.Drawing.Size(1454, 494);
            this.gdSysLog.TabIndex = 2;
            this.gdSysLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIsd,
            this.colEcd,
            this.coChr,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gdSysLog;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colIsd
            // 
            this.colIsd.Caption = "样本号";
            this.colIsd.FieldName = "pat_sid";
            this.colIsd.Name = "colIsd";
            this.colIsd.OptionsColumn.AllowEdit = false;
            this.colIsd.OptionsColumn.FixedWidth = true;
            this.colIsd.Visible = true;
            this.colIsd.VisibleIndex = 1;
            this.colIsd.Width = 65;
            // 
            // colEcd
            // 
            this.colEcd.Caption = "序号";
            this.colEcd.FieldName = "pat_host_order";
            this.colEcd.Name = "colEcd";
            this.colEcd.OptionsColumn.AllowEdit = false;
            this.colEcd.OptionsColumn.FixedWidth = true;
            this.colEcd.Width = 47;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "病人ID";
            this.gridColumn1.FieldName = "pat_in_no";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 79;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "组合";
            this.gridColumn2.FieldName = "pat_c_name";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 83;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "超时时间(分钟)";
            this.gridColumn3.FieldName = "timepassed";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 6;
            this.gridColumn3.Width = 100;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "超时类型";
            this.gridColumn4.FieldName = "overtype";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            this.gridColumn4.Width = 91;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "仪器";
            this.gridColumn5.FieldName = "itr_mid";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 70;
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmOverTimeWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 625);
            this.Controls.Add(this.gdSysLog);
            this.Controls.Add(this.barSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOverTimeWarning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "报告超时提醒信息";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmOverTimeWarning_FormClosing);
            this.Load += new System.EventHandler(this.FrmResultView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).EndInit();
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
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private System.Windows.Forms.Timer timer1;

    }
}