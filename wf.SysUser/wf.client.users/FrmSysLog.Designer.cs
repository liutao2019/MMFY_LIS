namespace dcl.client.users
{
    partial class FrmSysLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSysLog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.timeTo = new DevExpress.XtraEditors.DateEdit();
            this.timeFrom = new DevExpress.XtraEditors.DateEdit();
            this.cboModule = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cboLoginId = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gdSysLog = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colModule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLoginID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMessage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLoginId.Properties)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sysToolBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1354, 81);
            this.panel1.TabIndex = 0;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1354, 81);
            this.sysToolBar1.TabIndex = 1;
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar1_OnBtnExportClicked);
            // 
            // timeTo
            // 
            this.timeTo.EditValue = null;
            this.timeTo.EnterMoveNextControl = true;
            this.timeTo.Location = new System.Drawing.Point(86, 141);
            this.timeTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeTo.Name = "timeTo";
            this.timeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeTo.Size = new System.Drawing.Size(160, 24);
            this.timeTo.TabIndex = 3;
            // 
            // timeFrom
            // 
            this.timeFrom.EditValue = null;
            this.timeFrom.EnterMoveNextControl = true;
            this.timeFrom.Location = new System.Drawing.Point(86, 109);
            this.timeFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeFrom.Name = "timeFrom";
            this.timeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeFrom.Size = new System.Drawing.Size(160, 24);
            this.timeFrom.TabIndex = 2;
            // 
            // cboModule
            // 
            this.cboModule.EnterMoveNextControl = true;
            this.cboModule.Location = new System.Drawing.Point(86, 45);
            this.cboModule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboModule.Name = "cboModule";
            this.cboModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboModule.Properties.ImmediatePopup = true;
            this.cboModule.Size = new System.Drawing.Size(160, 24);
            this.cboModule.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(65, 142);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(15, 18);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "至";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 46);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "功能模块";
            // 
            // cboLoginId
            // 
            this.cboLoginId.EnterMoveNextControl = true;
            this.cboLoginId.Location = new System.Drawing.Point(86, 77);
            this.cboLoginId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboLoginId.Name = "cboLoginId";
            this.cboLoginId.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLoginId.Size = new System.Drawing.Size(160, 24);
            this.cboLoginId.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(20, 78);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 18);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "操作人员";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(20, 110);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 18);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "操作时间";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gdSysLog);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(2, 27);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1086, 614);
            this.panel4.TabIndex = 3;
            // 
            // gdSysLog
            // 
            this.gdSysLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSysLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gdSysLog.Location = new System.Drawing.Point(0, 0);
            this.gdSysLog.MainView = this.gridView1;
            this.gdSysLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gdSysLog.Name = "gdSysLog";
            this.gdSysLog.Size = new System.Drawing.Size(1086, 614);
            this.gdSysLog.TabIndex = 0;
            this.gdSysLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colModule,
            this.colTime,
            this.colLoginID,
            this.colIP,
            this.colType,
            this.colMessage});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gdSysLog;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colModule
            // 
            this.colModule.Caption = "模块";
            this.colModule.FieldName = "LogModule";
            this.colModule.Name = "colModule";
            this.colModule.OptionsColumn.AllowEdit = false;
            this.colModule.OptionsColumn.FixedWidth = true;
            this.colModule.Visible = true;
            this.colModule.VisibleIndex = 0;
            this.colModule.Width = 100;
            // 
            // colTime
            // 
            this.colTime.Caption = "时间";
            this.colTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colTime.FieldName = "LogTime";
            this.colTime.Name = "colTime";
            this.colTime.OptionsColumn.AllowEdit = false;
            this.colTime.OptionsColumn.FixedWidth = true;
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 1;
            this.colTime.Width = 125;
            // 
            // colLoginID
            // 
            this.colLoginID.Caption = "用户";
            this.colLoginID.FieldName = "LogLoginID";
            this.colLoginID.Name = "colLoginID";
            this.colLoginID.OptionsColumn.AllowEdit = false;
            this.colLoginID.OptionsColumn.FixedWidth = true;
            this.colLoginID.Visible = true;
            this.colLoginID.VisibleIndex = 2;
            this.colLoginID.Width = 100;
            // 
            // colIP
            // 
            this.colIP.Caption = "IP地址";
            this.colIP.FieldName = "LogIP";
            this.colIP.Name = "colIP";
            this.colIP.OptionsColumn.AllowEdit = false;
            this.colIP.OptionsColumn.FixedWidth = true;
            this.colIP.Visible = true;
            this.colIP.VisibleIndex = 3;
            this.colIP.Width = 120;
            // 
            // colType
            // 
            this.colType.Caption = "操作类型";
            this.colType.FieldName = "LogType";
            this.colType.Name = "colType";
            this.colType.OptionsColumn.AllowEdit = false;
            this.colType.OptionsColumn.FixedWidth = true;
            this.colType.Visible = true;
            this.colType.VisibleIndex = 4;
            this.colType.Width = 100;
            // 
            // colMessage
            // 
            this.colMessage.Caption = "详细";
            this.colMessage.FieldName = "LogMessage";
            this.colMessage.Name = "colMessage";
            this.colMessage.OptionsColumn.AllowEdit = false;
            this.colMessage.Visible = true;
            this.colMessage.VisibleIndex = 5;
            this.colMessage.Width = 400;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 81);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1354, 647);
            this.panelControl1.TabIndex = 4;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.timeTo);
            this.groupControl1.Controls.Add(this.cboModule);
            this.groupControl1.Controls.Add(this.timeFrom);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.cboLoginId);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(260, 643);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "查询条件";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(262, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1090, 643);
            this.groupControl2.TabIndex = 5;
            this.groupControl2.Text = "日志记录";
            // 
            // FrmSysLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 728);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmSysLog";
            this.Text = "系统日志";
            this.Load += new System.EventHandler(this.FrmSysLog_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLoginId.Properties)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cboLoginId;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboModule;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.DateEdit timeTo;
        private DevExpress.XtraEditors.DateEdit timeFrom;
        private DevExpress.XtraGrid.GridControl gdSysLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colModule;
        private DevExpress.XtraGrid.Columns.GridColumn colTime;
        private DevExpress.XtraGrid.Columns.GridColumn colLoginID;
        private DevExpress.XtraGrid.Columns.GridColumn colIP;
        private DevExpress.XtraGrid.Columns.GridColumn colType;
        private DevExpress.XtraGrid.Columns.GridColumn colMessage;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}