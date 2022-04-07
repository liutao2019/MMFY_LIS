namespace dcl.client.users
{
    partial class FrmDictOperationLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDictOperationLog));
            this.cboModule = new DevExpress.XtraEditors.ComboBoxEdit();
            this.selectDictSysUser1 = new dcl.client.control.SelectDictSysUser();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.timeTo = new DevExpress.XtraEditors.DateEdit();
            this.timeFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.bsOperatiopnLog = new System.Windows.Forms.BindingSource(this.components);
            this.gcOperationLog = new DevExpress.XtraGrid.GridControl();
            this.gvOperationLog = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colPat_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_itr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coPat_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_sex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_c_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperatiopnLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOperationLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOperationLog)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboModule
            // 
            this.cboModule.EnterMoveNextControl = true;
            this.cboModule.Location = new System.Drawing.Point(87, 147);
            this.cboModule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboModule.Name = "cboModule";
            this.cboModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboModule.Properties.ImmediatePopup = true;
            this.cboModule.Properties.Items.AddRange(new object[] {
            "项目字典",
            "组合字典",
            "仪器组合",
            "结果调整"});
            this.cboModule.Size = new System.Drawing.Size(146, 24);
            this.cboModule.TabIndex = 45;
            // 
            // selectDictSysUser1
            // 
            this.selectDictSysUser1.AddEmptyRow = true;
            this.selectDictSysUser1.BindByValue = false;
            this.selectDictSysUser1.colDisplay = "";
            this.selectDictSysUser1.colExtend1 = null;
            this.selectDictSysUser1.colInCode = "";
            this.selectDictSysUser1.colPY = "";
            this.selectDictSysUser1.colSeq = "";
            this.selectDictSysUser1.colValue = "";
            this.selectDictSysUser1.colWB = "";
            this.selectDictSysUser1.displayMember = null;
            this.selectDictSysUser1.EnterMoveNext = true;
            this.selectDictSysUser1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDictSysUser1.KeyUpDownMoveNext = false;
            this.selectDictSysUser1.LoadDataOnDesignMode = true;
            this.selectDictSysUser1.Location = new System.Drawing.Point(87, 112);
            this.selectDictSysUser1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectDictSysUser1.MaximumSize = new System.Drawing.Size(571, 27);
            this.selectDictSysUser1.MinimumSize = new System.Drawing.Size(57, 27);
            this.selectDictSysUser1.Name = "selectDictSysUser1";
            this.selectDictSysUser1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDictSysUser1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDictSysUser1.Readonly = false;
            this.selectDictSysUser1.SaveSourceID = false;
            this.selectDictSysUser1.SelectFilter = null;
            this.selectDictSysUser1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDictSysUser1.SelectOnly = true;
            this.selectDictSysUser1.Size = new System.Drawing.Size(146, 27);
            this.selectDictSysUser1.TabIndex = 44;
            this.selectDictSysUser1.UseExtend = false;
            this.selectDictSysUser1.valueMember = null;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(36, 114);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(45, 18);
            this.labelControl8.TabIndex = 43;
            this.labelControl8.Text = "操作人";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(21, 149);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 18);
            this.labelControl6.TabIndex = 14;
            this.labelControl6.Text = "操作模块";
            // 
            // timeTo
            // 
            this.timeTo.EditValue = null;
            this.timeTo.Location = new System.Drawing.Point(87, 80);
            this.timeTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeTo.Name = "timeTo";
            this.timeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeTo.Size = new System.Drawing.Size(146, 24);
            this.timeTo.TabIndex = 1;
            // 
            // timeFrom
            // 
            this.timeFrom.EditValue = null;
            this.timeFrom.Location = new System.Drawing.Point(87, 48);
            this.timeFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.timeFrom.Name = "timeFrom";
            this.timeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeFrom.Size = new System.Drawing.Size(146, 24);
            this.timeFrom.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(66, 82);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(15, 18);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "到";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(21, 50);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 18);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "修改日期";
            // 
            // bsOperatiopnLog
            // 
            this.bsOperatiopnLog.DataSource = typeof(dcl.entity.EntitySysOperationLog);
            // 
            // gcOperationLog
            // 
            this.gcOperationLog.DataSource = this.bsOperatiopnLog;
            this.gcOperationLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcOperationLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcOperationLog.Location = new System.Drawing.Point(2, 27);
            this.gcOperationLog.MainView = this.gvOperationLog;
            this.gcOperationLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcOperationLog.Name = "gcOperationLog";
            this.gcOperationLog.Size = new System.Drawing.Size(1106, 731);
            this.gcOperationLog.TabIndex = 3;
            this.gcOperationLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOperationLog});
            // 
            // gvOperationLog
            // 
            this.gvOperationLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPat_Date,
            this.colPat_itr_id,
            this.colPat_sid,
            this.coPat_name,
            this.colPat_sex,
            this.colPat_c_name,
            this.gridColumn1,
            this.gridColumn2});
            this.gvOperationLog.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvOperationLog.GridControl = this.gcOperationLog;
            this.gvOperationLog.Name = "gvOperationLog";
            this.gvOperationLog.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvOperationLog.OptionsView.ShowGroupPanel = false;
            // 
            // colPat_Date
            // 
            this.colPat_Date.Caption = "操作时间";
            this.colPat_Date.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colPat_Date.FieldName = "OperatDate";
            this.colPat_Date.Name = "colPat_Date";
            this.colPat_Date.OptionsColumn.AllowEdit = false;
            this.colPat_Date.OptionsFilter.AllowFilter = false;
            this.colPat_Date.Visible = true;
            this.colPat_Date.VisibleIndex = 0;
            this.colPat_Date.Width = 125;
            // 
            // colPat_itr_id
            // 
            this.colPat_itr_id.Caption = "操作人";
            this.colPat_itr_id.FieldName = "OperatUserName";
            this.colPat_itr_id.Name = "colPat_itr_id";
            this.colPat_itr_id.OptionsColumn.AllowEdit = false;
            this.colPat_itr_id.OptionsFilter.AllowFilter = false;
            this.colPat_itr_id.Visible = true;
            this.colPat_itr_id.VisibleIndex = 1;
            this.colPat_itr_id.Width = 100;
            // 
            // colPat_sid
            // 
            this.colPat_sid.Caption = "IP";
            this.colPat_sid.FieldName = "OperatServername";
            this.colPat_sid.Name = "colPat_sid";
            this.colPat_sid.OptionsColumn.AllowEdit = false;
            this.colPat_sid.OptionsFilter.AllowFilter = false;
            this.colPat_sid.Visible = true;
            this.colPat_sid.VisibleIndex = 2;
            this.colPat_sid.Width = 100;
            // 
            // coPat_name
            // 
            this.coPat_name.Caption = "操作模块";
            this.coPat_name.FieldName = "OperatModule";
            this.coPat_name.Name = "coPat_name";
            this.coPat_name.OptionsColumn.AllowEdit = false;
            this.coPat_name.OptionsFilter.AllowFilter = false;
            this.coPat_name.Visible = true;
            this.coPat_name.VisibleIndex = 3;
            this.coPat_name.Width = 120;
            // 
            // colPat_sex
            // 
            this.colPat_sex.Caption = "操作类别";
            this.colPat_sex.FieldName = "OperatGroup";
            this.colPat_sex.Name = "colPat_sex";
            this.colPat_sex.OptionsColumn.AllowEdit = false;
            this.colPat_sex.OptionsFilter.AllowFilter = false;
            this.colPat_sex.Visible = true;
            this.colPat_sex.VisibleIndex = 4;
            this.colPat_sex.Width = 100;
            // 
            // colPat_c_name
            // 
            this.colPat_c_name.Caption = "操作状态";
            this.colPat_c_name.FieldName = "OperatAction";
            this.colPat_c_name.Name = "colPat_c_name";
            this.colPat_c_name.OptionsColumn.AllowEdit = false;
            this.colPat_c_name.OptionsFilter.AllowFilter = false;
            this.colPat_c_name.Visible = true;
            this.colPat_c_name.VisibleIndex = 5;
            this.colPat_c_name.Width = 69;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "操作内容";
            this.gridColumn1.FieldName = "OperatObject";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 6;
            this.gridColumn1.Width = 158;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "详细信息";
            this.gridColumn2.FieldName = "OperatContent";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 7;
            this.gridColumn2.Width = 413;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.sysToolBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1354, 81);
            this.panel3.TabIndex = 4;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1354, 81);
            this.sysToolBar1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cboModule);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.selectDictSysUser1);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.timeFrom);
            this.groupControl1.Controls.Add(this.timeTo);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 81);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(244, 760);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "查询条件";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gcOperationLog);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(244, 81);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1110, 760);
            this.groupControl2.TabIndex = 6;
            this.groupControl2.Text = "操作记录";
            // 
            // FrmDictOperationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 841);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panel3);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmDictOperationLog";
            this.Text = "资料修改记录";
            this.Load += new System.EventHandler(this.FrmOperationLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cboModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperatiopnLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOperationLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOperationLog)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.DateEdit timeTo;
        private DevExpress.XtraEditors.DateEdit timeFrom;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private System.Windows.Forms.BindingSource bsOperatiopnLog;
        private control.SelectDictSysUser selectDictSysUser1;
        private DevExpress.XtraEditors.ComboBoxEdit cboModule;
        private DevExpress.XtraGrid.GridControl gcOperationLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gvOperationLog;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_id;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_Date;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_itr_id;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_sid;
        private DevExpress.XtraGrid.Columns.GridColumn coPat_name;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_sex;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_c_name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private System.Windows.Forms.Panel panel3;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}