namespace dcl.client.users
{
    partial class FrmExceptionLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExceptionLog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.timeTo = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.timeFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gdSysLog = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModule = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtDetail = new DevExpress.XtraEditors.MemoEdit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetail.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sysToolBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 1047);
            this.panel1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1965, 77);
            this.panel1.TabIndex = 1;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(13, 15, 13, 15);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1965, 77);
            this.sysToolBar1.TabIndex = 1;
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.timeTo);
            this.panel2.Controls.Add(this.labelControl1);
            this.panel2.Controls.Add(this.timeFrom);
            this.panel2.Controls.Add(this.labelControl4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1965, 99);
            this.panel2.TabIndex = 2;
            // 
            // timeTo
            // 
            this.timeTo.EditValue = null;
            this.timeTo.Location = new System.Drawing.Point(394, 29);
            this.timeTo.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.timeTo.Name = "timeTo";
            this.timeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeTo.Size = new System.Drawing.Size(217, 36);
            this.timeTo.TabIndex = 8;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(373, 36);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(9, 29);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "-";
            // 
            // timeFrom
            // 
            this.timeFrom.EditValue = null;
            this.timeFrom.Location = new System.Drawing.Point(143, 29);
            this.timeFrom.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.timeFrom.Name = "timeFrom";
            this.timeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeFrom.Size = new System.Drawing.Size(217, 36);
            this.timeFrom.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(26, 46);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(96, 29);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "操作时间";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gdSysLog);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 99);
            this.panel3.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(867, 948);
            this.panel3.TabIndex = 3;
            // 
            // gdSysLog
            // 
            this.gdSysLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSysLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.gdSysLog.Location = new System.Drawing.Point(0, 0);
            this.gdSysLog.MainView = this.gridView1;
            this.gdSysLog.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.gdSysLog.Name = "gdSysLog";
            this.gdSysLog.Size = new System.Drawing.Size(867, 948);
            this.gdSysLog.TabIndex = 1;
            this.gdSysLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTime,
            this.colModule});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gdSysLog;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // colTime
            // 
            this.colTime.Caption = "时间";
            this.colTime.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colTime.FieldName = "Time";
            this.colTime.Name = "colTime";
            this.colTime.OptionsColumn.AllowEdit = false;
            this.colTime.OptionsColumn.FixedWidth = true;
            this.colTime.Visible = true;
            this.colTime.VisibleIndex = 0;
            // 
            // colModule
            // 
            this.colModule.Caption = "模块";
            this.colModule.FieldName = "Module";
            this.colModule.Name = "colModule";
            this.colModule.OptionsColumn.AllowEdit = false;
            this.colModule.Visible = true;
            this.colModule.VisibleIndex = 1;
            this.colModule.Width = 260;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtDetail);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(867, 99);
            this.panel4.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1098, 948);
            this.panel4.TabIndex = 4;
            // 
            // txtDetail
            // 
            this.txtDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetail.Location = new System.Drawing.Point(0, 0);
            this.txtDetail.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.Properties.ReadOnly = true;
            this.txtDetail.Size = new System.Drawing.Size(1098, 948);
            this.txtDetail.TabIndex = 0;
            // 
            // FrmExceptionLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1965, 1124);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "FrmExceptionLog";
            this.Text = "异常日志";
            this.Load += new System.EventHandler(this.FrmExceptionLog_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtDetail.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.DateEdit timeFrom;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraEditors.MemoEdit txtDetail;
        private DevExpress.XtraGrid.GridControl gdSysLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colModule;
        private DevExpress.XtraGrid.Columns.GridColumn colTime;
        private DevExpress.XtraEditors.DateEdit timeTo;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}