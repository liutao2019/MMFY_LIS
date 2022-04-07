namespace dcl.client.users
{
    partial class FrmOperationLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOperationLog));
            this.reportStateMonitor1 = new dcl.client.users.reportStateMonitor();
            this.gdPatients = new DevExpress.XtraGrid.GridControl();
            this.bsPatients = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colPat_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_Date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_itr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coPat_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_sex = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPat_c_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtType = new dcl.client.control.SelectDicLabProfession();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.txtPat_chk_code = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtInstrmt = new dcl.client.control.SelectDicInstrument();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.timeTo = new DevExpress.XtraEditors.DateEdit();
            this.timeFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtPatName = new DevExpress.XtraEditors.TextEdit();
            this.txtPatID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtIdType = new dcl.client.control.SelectDicPubIdent();
            this.gdSysOperationLog = new DevExpress.XtraGrid.GridControl();
            this.gvLog = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.checkEdit = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.tabControl = new DevExpress.XtraTab.XtraTabControl();
            this.layoutControlItem20 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.plCommand = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.gdPatients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPat_chk_code.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysOperationLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.plCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // reportStateMonitor1
            // 
            this.reportStateMonitor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportStateMonitor1.Location = new System.Drawing.Point(871, 45);
            this.reportStateMonitor1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.reportStateMonitor1.Name = "reportStateMonitor1";
            this.reportStateMonitor1.Size = new System.Drawing.Size(512, 387);
            this.reportStateMonitor1.TabIndex = 3;
            // 
            // gdPatients
            // 
            this.gdPatients.DataSource = this.bsPatients;
            this.gdPatients.Dock = System.Windows.Forms.DockStyle.Left;
            this.gdPatients.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gdPatients.Location = new System.Drawing.Point(15, 45);
            this.gdPatients.MainView = this.gridView1;
            this.gdPatients.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gdPatients.Name = "gdPatients";
            this.gdPatients.Size = new System.Drawing.Size(856, 387);
            this.gdPatients.TabIndex = 2;
            this.gdPatients.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsPatients
            // 
            this.bsPatients.DataSource = typeof(dcl.entity.EntityPidReportMain);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPat_id,
            this.colPat_Date,
            this.colPat_itr_id,
            this.colPat_sid,
            this.coPat_name,
            this.colPat_sex,
            this.colPat_c_name});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gdPatients;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // colPat_id
            // 
            this.colPat_id.Caption = "pat_id";
            this.colPat_id.FieldName = "RepId";
            this.colPat_id.Name = "colPat_id";
            this.colPat_id.OptionsColumn.AllowEdit = false;
            this.colPat_id.OptionsFilter.AllowFilter = false;
            // 
            // colPat_Date
            // 
            this.colPat_Date.Caption = "录入时间";
            this.colPat_Date.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colPat_Date.FieldName = "RepInDate";
            this.colPat_Date.Name = "colPat_Date";
            this.colPat_Date.OptionsColumn.AllowEdit = false;
            this.colPat_Date.OptionsFilter.AllowFilter = false;
            this.colPat_Date.Visible = true;
            this.colPat_Date.VisibleIndex = 0;
            this.colPat_Date.Width = 125;
            // 
            // colPat_itr_id
            // 
            this.colPat_itr_id.Caption = "仪器代码";
            this.colPat_itr_id.FieldName = "RepItrId";
            this.colPat_itr_id.Name = "colPat_itr_id";
            this.colPat_itr_id.OptionsColumn.AllowEdit = false;
            this.colPat_itr_id.OptionsFilter.AllowFilter = false;
            this.colPat_itr_id.Visible = true;
            this.colPat_itr_id.VisibleIndex = 1;
            this.colPat_itr_id.Width = 94;
            // 
            // colPat_sid
            // 
            this.colPat_sid.Caption = "样本号";
            this.colPat_sid.FieldName = "RepSid";
            this.colPat_sid.Name = "colPat_sid";
            this.colPat_sid.OptionsColumn.AllowEdit = false;
            this.colPat_sid.OptionsFilter.AllowFilter = false;
            this.colPat_sid.Visible = true;
            this.colPat_sid.VisibleIndex = 2;
            this.colPat_sid.Width = 100;
            // 
            // coPat_name
            // 
            this.coPat_name.Caption = "姓名";
            this.coPat_name.FieldName = "PidName";
            this.coPat_name.Name = "coPat_name";
            this.coPat_name.OptionsColumn.AllowEdit = false;
            this.coPat_name.OptionsFilter.AllowFilter = false;
            this.coPat_name.Visible = true;
            this.coPat_name.VisibleIndex = 3;
            this.coPat_name.Width = 88;
            // 
            // colPat_sex
            // 
            this.colPat_sex.Caption = "性别";
            this.colPat_sex.FieldName = "PidSex";
            this.colPat_sex.Name = "colPat_sex";
            this.colPat_sex.OptionsColumn.AllowEdit = false;
            this.colPat_sex.OptionsFilter.AllowFilter = false;
            this.colPat_sex.Visible = true;
            this.colPat_sex.VisibleIndex = 4;
            this.colPat_sex.Width = 51;
            // 
            // colPat_c_name
            // 
            this.colPat_c_name.Caption = "组合项目";
            this.colPat_c_name.FieldName = "PidComName";
            this.colPat_c_name.Name = "colPat_c_name";
            this.colPat_c_name.OptionsColumn.AllowEdit = false;
            this.colPat_c_name.OptionsFilter.AllowFilter = false;
            this.colPat_c_name.Visible = true;
            this.colPat_c_name.VisibleIndex = 5;
            this.colPat_c_name.Width = 400;
            // 
            // txtType
            // 
            this.txtType.AddEmptyRow = true;
            this.txtType.AutoScroll = true;
            this.txtType.BindByValue = false;
            this.txtType.colDisplay = "";
            this.txtType.colExtend1 = null;
            this.txtType.colInCode = "";
            this.txtType.colPY = "";
            this.txtType.colSeq = "";
            this.txtType.colValue = "";
            this.txtType.colWB = "";
            this.txtType.displayMember = null;
            this.txtType.EnterMoveNext = true;
            this.txtType.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtType.KeyUpDownMoveNext = false;
            this.txtType.LoadDataOnDesignMode = true;
            this.txtType.Location = new System.Drawing.Point(98, 137);
            this.txtType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtType.MaximumSize = new System.Drawing.Size(714, 33);
            this.txtType.MinimumSize = new System.Drawing.Size(71, 33);
            this.txtType.Name = "txtType";
            this.txtType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtType.Readonly = false;
            this.txtType.SaveSourceID = false;
            this.txtType.SelectFilter = null;
            this.txtType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtType.SelectOnly = true;
            this.txtType.Size = new System.Drawing.Size(182, 33);
            this.txtType.TabIndex = 2;
            this.txtType.UseExtend = false;
            this.txtType.valueMember = null;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(15, 139);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(72, 22);
            this.labelControl8.TabIndex = 43;
            this.labelControl8.Text = "物理组别";
            // 
            // txtPat_chk_code
            // 
            this.txtPat_chk_code.EnterMoveNextControl = true;
            this.txtPat_chk_code.Location = new System.Drawing.Point(98, 343);
            this.txtPat_chk_code.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPat_chk_code.Name = "txtPat_chk_code";
            this.txtPat_chk_code.Size = new System.Drawing.Size(182, 28);
            this.txtPat_chk_code.TabIndex = 7;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(34, 345);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(54, 22);
            this.labelControl7.TabIndex = 17;
            this.labelControl7.Text = "审核人";
            // 
            // txtInstrmt
            // 
            this.txtInstrmt.AddEmptyRow = true;
            this.txtInstrmt.AutoScroll = true;
            this.txtInstrmt.BindByValue = false;
            this.txtInstrmt.colDisplay = "";
            this.txtInstrmt.colExtend1 = null;
            this.txtInstrmt.colInCode = "";
            this.txtInstrmt.colPY = "";
            this.txtInstrmt.colSeq = "";
            this.txtInstrmt.colValue = "";
            this.txtInstrmt.colWB = "";
            this.txtInstrmt.displayMember = null;
            this.txtInstrmt.EnterMoveNext = true;
            this.txtInstrmt.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtInstrmt.KeyUpDownMoveNext = false;
            this.txtInstrmt.LoadDataOnDesignMode = true;
            this.txtInstrmt.Location = new System.Drawing.Point(98, 180);
            this.txtInstrmt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInstrmt.MaximumSize = new System.Drawing.Size(714, 33);
            this.txtInstrmt.MinimumSize = new System.Drawing.Size(71, 33);
            this.txtInstrmt.Name = "txtInstrmt";
            this.txtInstrmt.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtInstrmt.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtInstrmt.Readonly = false;
            this.txtInstrmt.SaveSourceID = false;
            this.txtInstrmt.SelectFilter = null;
            this.txtInstrmt.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtInstrmt.SelectOnly = true;
            this.txtInstrmt.ShowAllInstrmt = false;
            this.txtInstrmt.Size = new System.Drawing.Size(182, 33);
            this.txtInstrmt.TabIndex = 3;
            this.txtInstrmt.UseExtend = false;
            this.txtInstrmt.valueMember = null;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(15, 182);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(72, 22);
            this.labelControl6.TabIndex = 14;
            this.labelControl6.Text = "仪器代码";
            // 
            // timeTo
            // 
            this.timeTo.EditValue = null;
            this.timeTo.Location = new System.Drawing.Point(98, 98);
            this.timeTo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timeTo.Name = "timeTo";
            this.timeTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeTo.Size = new System.Drawing.Size(182, 28);
            this.timeTo.TabIndex = 1;
            // 
            // timeFrom
            // 
            this.timeFrom.EditValue = null;
            this.timeFrom.Location = new System.Drawing.Point(98, 59);
            this.timeFrom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.timeFrom.Name = "timeFrom";
            this.timeFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timeFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.timeFrom.Size = new System.Drawing.Size(182, 28);
            this.timeFrom.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(71, 99);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(18, 22);
            this.labelControl5.TabIndex = 13;
            this.labelControl5.Text = "到";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(15, 61);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(72, 22);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "录入日期";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(52, 307);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 22);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "姓名";
            // 
            // txtPatName
            // 
            this.txtPatName.EnterMoveNextControl = true;
            this.txtPatName.Location = new System.Drawing.Point(98, 304);
            this.txtPatName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPatName.Name = "txtPatName";
            this.txtPatName.Size = new System.Drawing.Size(182, 28);
            this.txtPatName.TabIndex = 6;
            // 
            // txtPatID
            // 
            this.txtPatID.EnterMoveNextControl = true;
            this.txtPatID.Location = new System.Drawing.Point(98, 265);
            this.txtPatID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPatID.Name = "txtPatID";
            this.txtPatID.Size = new System.Drawing.Size(182, 28);
            this.txtPatID.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(32, 266);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(55, 22);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "病人ID";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 225);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(55, 22);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "ID类型";
            // 
            // txtIdType
            // 
            this.txtIdType.AddEmptyRow = true;
            this.txtIdType.AutoScroll = true;
            this.txtIdType.BindByValue = false;
            this.txtIdType.colDisplay = "";
            this.txtIdType.colExtend1 = null;
            this.txtIdType.colInCode = "";
            this.txtIdType.colPY = "";
            this.txtIdType.colSeq = "";
            this.txtIdType.colValue = "";
            this.txtIdType.colWB = "";
            this.txtIdType.displayMember = null;
            this.txtIdType.EnterMoveNext = true;
            this.txtIdType.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtIdType.KeyUpDownMoveNext = false;
            this.txtIdType.LoadDataOnDesignMode = true;
            this.txtIdType.Location = new System.Drawing.Point(98, 222);
            this.txtIdType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtIdType.MaximumSize = new System.Drawing.Size(714, 33);
            this.txtIdType.MinimumSize = new System.Drawing.Size(71, 33);
            this.txtIdType.Name = "txtIdType";
            this.txtIdType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtIdType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtIdType.Readonly = false;
            this.txtIdType.SaveSourceID = false;
            this.txtIdType.SelectFilter = null;
            this.txtIdType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtIdType.SelectOnly = true;
            this.txtIdType.Size = new System.Drawing.Size(182, 33);
            this.txtIdType.TabIndex = 4;
            this.txtIdType.UseExtend = false;
            this.txtIdType.valueMember = null;
            // 
            // gdSysOperationLog
            // 
            this.gdSysOperationLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSysOperationLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gdSysOperationLog.Location = new System.Drawing.Point(15, 91);
            this.gdSysOperationLog.MainView = this.gvLog;
            this.gdSysOperationLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gdSysOperationLog.Name = "gdSysOperationLog";
            this.gdSysOperationLog.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.checkEdit});
            this.gdSysOperationLog.Size = new System.Drawing.Size(1368, 376);
            this.gdSysOperationLog.TabIndex = 2;
            this.gdSysOperationLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLog});
            // 
            // gvLog
            // 
            this.gvLog.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gvLog.GridControl = this.gdSysOperationLog;
            this.gvLog.Name = "gvLog";
            this.gvLog.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvLog.OptionsView.ColumnAutoWidth = false;
            this.gvLog.OptionsView.RowAutoHeight = true;
            this.gvLog.OptionsView.ShowColumnHeaders = false;
            this.gvLog.OptionsView.ShowGroupPanel = false;
            // 
            // checkEdit
            // 
            this.checkEdit.AutoHeight = false;
            this.checkEdit.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.checkEdit.Name = "checkEdit";
            this.checkEdit.PictureChecked = ((System.Drawing.Image)(resources.GetObject("checkEdit.PictureChecked")));
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(15, 45);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.Size = new System.Drawing.Size(1368, 46);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabControl_SelectedPageChanged);
            // 
            // layoutControlItem20
            // 
            this.layoutControlItem20.Control = this.txtIdType;
            this.layoutControlItem20.CustomizationFormText = "I D  类型";
            this.layoutControlItem20.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem20.MaxSize = new System.Drawing.Size(0, 24);
            this.layoutControlItem20.MinSize = new System.Drawing.Size(164, 24);
            this.layoutControlItem20.Name = "layoutControlItem20";
            this.layoutControlItem20.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 1, 1);
            this.layoutControlItem20.Size = new System.Drawing.Size(219, 24);
            this.layoutControlItem20.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem20.Text = "I D  类型";
            this.layoutControlItem20.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem20.TextSize = new System.Drawing.Size(48, 20);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtIdType;
            this.layoutControlItem1.CustomizationFormText = "I D  类型";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 96);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(0, 24);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(164, 24);
            this.layoutControlItem1.Name = "layoutControlItem20";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 1, 1);
            this.layoutControlItem1.Size = new System.Drawing.Size(219, 24);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "I D  类型";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(48, 20);
            // 
            // plCommand
            // 
            this.plCommand.Controls.Add(this.sysToolBar1);
            this.plCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.plCommand.Location = new System.Drawing.Point(0, 0);
            this.plCommand.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.plCommand.Name = "plCommand";
            this.plCommand.Size = new System.Drawing.Size(1692, 99);
            this.plCommand.TabIndex = 5;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(6, 9, 6, 9);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1692, 99);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtPat_chk_code);
            this.groupControl1.Controls.Add(this.txtType);
            this.groupControl1.Controls.Add(this.labelControl7);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.txtPatName);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.txtInstrmt);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.txtPatID);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.timeFrom);
            this.groupControl1.Controls.Add(this.txtIdType);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.timeTo);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 99);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(294, 929);
            this.groupControl1.TabIndex = 6;
            this.groupControl1.Text = "过滤条件";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.reportStateMonitor1);
            this.groupControl2.Controls.Add(this.gdPatients);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(294, 99);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.groupControl2.Size = new System.Drawing.Size(1398, 447);
            this.groupControl2.TabIndex = 7;
            this.groupControl2.Text = "报告信息";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.gdSysOperationLog);
            this.groupControl3.Controls.Add(this.tabControl);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(294, 546);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.groupControl3.Size = new System.Drawing.Size(1398, 482);
            this.groupControl3.TabIndex = 8;
            this.groupControl3.Text = "修改记录";
            // 
            // FrmOperationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1692, 1028);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.plCommand);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmOperationLog";
            this.Text = "资料修改记录";
            this.Load += new System.EventHandler(this.FrmOperationLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdPatients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPat_chk_code.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysOperationLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem20)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.plCommand.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gdPatients;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_itr_id;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_Date;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_sid;
        private DevExpress.XtraGrid.Columns.GridColumn coPat_name;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_sex;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_c_name;
        private DevExpress.XtraTab.XtraTabControl tabControl;
        protected dcl.client.control.SelectDicPubIdent txtIdType;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem20;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        protected DevExpress.XtraEditors.TextEdit txtPatID;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        protected DevExpress.XtraEditors.TextEdit txtPatName;
        private DevExpress.XtraEditors.DateEdit timeTo;
        private DevExpress.XtraEditors.DateEdit timeFrom;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Columns.GridColumn colPat_id;
        private DevExpress.XtraGrid.GridControl gdSysOperationLog;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView gvLog;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit checkEdit;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private dcl.client.control.SelectDicInstrument txtInstrmt;
        protected DevExpress.XtraEditors.TextEdit txtPat_chk_code;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private dcl.client.users.reportStateMonitor reportStateMonitor1;
        private dcl.client.control.SelectDicLabProfession txtType;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private System.Windows.Forms.BindingSource bsPatients;
        private System.Windows.Forms.Panel plCommand;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
    }
}