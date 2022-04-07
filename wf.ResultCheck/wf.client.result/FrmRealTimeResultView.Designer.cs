namespace dcl.client.result
{
    partial class FrmRealTimeResultView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRealTimeResultView));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bindingSource1 = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_a = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_b = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_c = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_d = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_cno = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_msg = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itr_mid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_sid_int = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.chkAutoRefresh = new DevExpress.XtraEditors.CheckEdit();
            this.rdoResultType = new DevExpress.XtraEditors.RadioGroup();
            this.lblTime = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtItr = new dcl.client.control.SelectDicInstrument();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtSID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.pnlBottomToolbar = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoRefresh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoResultType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).BeginInit();
            this.pnlBottomToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bindingSource1;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 33);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1151, 505);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.OddRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridView1.Appearance.OddRow.Options.UseBackColor = true;
            this.gridView1.Appearance.OddRow.Options.UseForeColor = true;
            this.gridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.Blue;
            this.gridView1.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gridView1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridView1.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.col_res_sid,
            this.col_itm_ecd,
            this.col_res_chr_a,
            this.col_res_chr_b,
            this.col_res_chr_c,
            this.col_res_chr_d,
            this.col_res_date,
            this.col_res_cno,
            this.col_msg,
            this.col_itr_mid,
            this.col_res_sid_int});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.ImmediateUpdateRowPosition = false;
            this.gridView1.OptionsBehavior.KeepFocusedRowOnUpdate = false;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsView.AllowCellMerge = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.col_res_date, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "样本号(整型)";
            this.gridColumn1.FieldName = "ResSidInt";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn1.OptionsColumn.FixedWidth = true;
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Width = 130;
            // 
            // col_res_sid
            // 
            this.col_res_sid.AppearanceCell.Options.UseTextOptions = true;
            this.col_res_sid.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_res_sid.Caption = "标本号";
            this.col_res_sid.FieldName = "ObrSid";
            this.col_res_sid.Name = "col_res_sid";
            this.col_res_sid.OptionsColumn.AllowEdit = false;
            this.col_res_sid.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_sid.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.col_res_sid.OptionsColumn.AllowMove = false;
            this.col_res_sid.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_sid.OptionsColumn.FixedWidth = true;
            this.col_res_sid.OptionsColumn.ReadOnly = true;
            this.col_res_sid.OptionsFilter.AllowFilter = false;
            this.col_res_sid.Visible = true;
            this.col_res_sid.VisibleIndex = 0;
            this.col_res_sid.Width = 135;
            // 
            // col_itm_ecd
            // 
            this.col_itm_ecd.Caption = "项目代码";
            this.col_itm_ecd.FieldName = "ItmEcd";
            this.col_itm_ecd.Name = "col_itm_ecd";
            this.col_itm_ecd.OptionsColumn.AllowEdit = false;
            this.col_itm_ecd.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_itm_ecd.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_itm_ecd.OptionsColumn.FixedWidth = true;
            this.col_itm_ecd.OptionsFilter.AllowAutoFilter = false;
            this.col_itm_ecd.OptionsFilter.AllowFilter = false;
            this.col_itm_ecd.Visible = true;
            this.col_itm_ecd.VisibleIndex = 1;
            this.col_itm_ecd.Width = 100;
            // 
            // col_res_chr_a
            // 
            this.col_res_chr_a.Caption = "结果A";
            this.col_res_chr_a.FieldName = "ObrValue";
            this.col_res_chr_a.Name = "col_res_chr_a";
            this.col_res_chr_a.OptionsColumn.AllowEdit = false;
            this.col_res_chr_a.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_a.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_a.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_res_chr_a.OptionsColumn.FixedWidth = true;
            this.col_res_chr_a.OptionsColumn.ReadOnly = true;
            this.col_res_chr_a.OptionsFilter.AllowAutoFilter = false;
            this.col_res_chr_a.OptionsFilter.AllowFilter = false;
            this.col_res_chr_a.Visible = true;
            this.col_res_chr_a.VisibleIndex = 2;
            this.col_res_chr_a.Width = 80;
            // 
            // col_res_chr_b
            // 
            this.col_res_chr_b.Caption = "结果B";
            this.col_res_chr_b.FieldName = "ObrValue2";
            this.col_res_chr_b.Name = "col_res_chr_b";
            this.col_res_chr_b.OptionsColumn.AllowEdit = false;
            this.col_res_chr_b.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_b.OptionsColumn.FixedWidth = true;
            this.col_res_chr_b.Visible = true;
            this.col_res_chr_b.VisibleIndex = 3;
            this.col_res_chr_b.Width = 80;
            // 
            // col_res_chr_c
            // 
            this.col_res_chr_c.Caption = "结果3";
            this.col_res_chr_c.FieldName = "ObrValue3";
            this.col_res_chr_c.Name = "col_res_chr_c";
            this.col_res_chr_c.OptionsColumn.AllowEdit = false;
            this.col_res_chr_c.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_c.Width = 90;
            // 
            // col_res_chr_d
            // 
            this.col_res_chr_d.Caption = "结果4";
            this.col_res_chr_d.FieldName = "ObrValue4";
            this.col_res_chr_d.Name = "col_res_chr_d";
            this.col_res_chr_d.OptionsColumn.AllowEdit = false;
            this.col_res_chr_d.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_d.Width = 90;
            // 
            // col_res_date
            // 
            this.col_res_date.Caption = "结果日期";
            this.col_res_date.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.col_res_date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.col_res_date.FieldName = "ObrDate";
            this.col_res_date.Name = "col_res_date";
            this.col_res_date.OptionsColumn.AllowEdit = false;
            this.col_res_date.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_date.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_date.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_res_date.OptionsColumn.FixedWidth = true;
            this.col_res_date.OptionsColumn.ReadOnly = true;
            this.col_res_date.OptionsFilter.AllowAutoFilter = false;
            this.col_res_date.OptionsFilter.AllowFilter = false;
            this.col_res_date.Visible = true;
            this.col_res_date.VisibleIndex = 5;
            this.col_res_date.Width = 60;
            // 
            // col_res_cno
            // 
            this.col_res_cno.Caption = "仪器通道";
            this.col_res_cno.FieldName = "ObrMacCode";
            this.col_res_cno.Name = "col_res_cno";
            this.col_res_cno.OptionsColumn.AllowEdit = false;
            this.col_res_cno.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_cno.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_cno.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_res_cno.OptionsColumn.FixedWidth = true;
            this.col_res_cno.OptionsColumn.ReadOnly = true;
            this.col_res_cno.OptionsFilter.AllowAutoFilter = false;
            this.col_res_cno.OptionsFilter.AllowFilter = false;
            this.col_res_cno.Visible = true;
            this.col_res_cno.VisibleIndex = 4;
            this.col_res_cno.Width = 60;
            // 
            // col_msg
            // 
            this.col_msg.Caption = "备注";
            this.col_msg.FieldName = "Msg";
            this.col_msg.Name = "col_msg";
            this.col_msg.OptionsColumn.AllowEdit = false;
            this.col_msg.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_msg.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_msg.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.col_msg.OptionsColumn.FixedWidth = true;
            this.col_msg.OptionsColumn.ReadOnly = true;
            this.col_msg.OptionsFilter.AllowAutoFilter = false;
            this.col_msg.OptionsFilter.AllowFilter = false;
            this.col_msg.Visible = true;
            this.col_msg.VisibleIndex = 6;
            this.col_msg.Width = 91;
            // 
            // col_itr_mid
            // 
            this.col_itr_mid.Caption = "仪器代码";
            this.col_itr_mid.Name = "col_itr_mid";
            this.col_itr_mid.OptionsColumn.AllowEdit = false;
            this.col_itr_mid.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_itr_mid.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_itr_mid.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_itr_mid.OptionsColumn.FixedWidth = true;
            this.col_itr_mid.OptionsColumn.ReadOnly = true;
            this.col_itr_mid.OptionsFilter.AllowAutoFilter = false;
            this.col_itr_mid.OptionsFilter.AllowFilter = false;
            this.col_itr_mid.Width = 87;
            // 
            // col_res_sid_int
            // 
            this.col_res_sid_int.AppearanceCell.Options.UseTextOptions = true;
            this.col_res_sid_int.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.col_res_sid_int.Caption = "样本号int";
            this.col_res_sid_int.Name = "col_res_sid_int";
            this.col_res_sid_int.OptionsColumn.AllowEdit = false;
            this.col_res_sid_int.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_sid_int.OptionsFilter.AllowAutoFilter = false;
            this.col_res_sid_int.OptionsFilter.AllowFilter = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupControl2);
            this.panel1.Controls.Add(this.groupControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1157, 601);
            this.panel1.TabIndex = 1;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gridControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 60);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1157, 541);
            this.groupControl2.TabIndex = 14;
            this.groupControl2.Text = "结果";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtDate);
            this.groupControl1.Controls.Add(this.chkAutoRefresh);
            this.groupControl1.Controls.Add(this.rdoResultType);
            this.groupControl1.Controls.Add(this.lblTime);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txtItr);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.txtSID);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1157, 60);
            this.groupControl1.TabIndex = 13;
            this.groupControl1.Text = "查询条件";
            // 
            // txtDate
            // 
            this.txtDate.EditValue = null;
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(89, 31);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDate.Size = new System.Drawing.Size(133, 26);
            this.txtDate.TabIndex = 4;
            this.txtDate.Leave += new System.EventHandler(this.txtDate_Leave);
            // 
            // chkAutoRefresh
            // 
            this.chkAutoRefresh.Location = new System.Drawing.Point(986, 32);
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.Properties.Caption = "自动刷新";
            this.chkAutoRefresh.Size = new System.Drawing.Size(93, 19);
            this.chkAutoRefresh.TabIndex = 11;
            this.chkAutoRefresh.ToolTip = "每6秒更新数据一次";
            this.chkAutoRefresh.CheckedChanged += new System.EventHandler(this.chkIncludeNonItrResult_CheckedChanged);
            // 
            // rdoResultType
            // 
            this.rdoResultType.EditValue = 0;
            this.rdoResultType.Location = new System.Drawing.Point(778, 26);
            this.rdoResultType.Name = "rdoResultType";
            this.rdoResultType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoResultType.Properties.Appearance.Options.UseBackColor = true;
            this.rdoResultType.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoResultType.Properties.Columns = 2;
            this.rdoResultType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "仪器原始结果"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "全部结果")});
            this.rdoResultType.Size = new System.Drawing.Size(232, 29);
            this.rdoResultType.TabIndex = 12;
            this.rdoResultType.SelectedIndexChanged += new System.EventHandler(this.rdoResultType_SelectedIndexChanged);
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(32, 32);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(48, 14);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "测定日期";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(256, 32);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "测定仪器";
            // 
            // txtItr
            // 
            this.txtItr.AddEmptyRow = true;
            this.txtItr.BindByValue = false;
            this.txtItr.colDisplay = "";
            this.txtItr.colExtend1 = null;
            this.txtItr.colInCode = "";
            this.txtItr.colPY = "";
            this.txtItr.colSeq = "";
            this.txtItr.colValue = "";
            this.txtItr.colWB = "";
            this.txtItr.displayMember = null;
            this.txtItr.EnterMoveNext = true;
            this.txtItr.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtItr.KeyUpDownMoveNext = false;
            this.txtItr.LoadDataOnDesignMode = true;
            this.txtItr.Location = new System.Drawing.Point(314, 31);
            this.txtItr.MaximumSize = new System.Drawing.Size(500, 21);
            this.txtItr.MinimumSize = new System.Drawing.Size(50, 21);
            this.txtItr.Name = "txtItr";
            this.txtItr.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtItr.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtItr.Readonly = false;
            this.txtItr.SaveSourceID = false;
            this.txtItr.SelectFilter = null;
            this.txtItr.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtItr.SelectOnly = true;
            this.txtItr.ShowAllInstrmt = true;
            this.txtItr.Size = new System.Drawing.Size(133, 21);
            this.txtItr.TabIndex = 6;
            this.txtItr.UseExtend = false;
            this.txtItr.valueMember = null;
            this.txtItr.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.txtItr_ValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(663, 32);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(89, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "例：1-5,7,15-20";
            this.labelControl3.Visible = false;
            // 
            // txtSID
            // 
            this.txtSID.EnterMoveNextControl = true;
            this.txtSID.Location = new System.Drawing.Point(525, 31);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(133, 20);
            this.txtSID.TabIndex = 7;
            this.txtSID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSID_KeyDown);
            this.txtSID.Leave += new System.EventHandler(this.txtSID_Leave);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(480, 32);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 14);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "样本号";
            // 
            // pnlBottomToolbar
            // 
            this.pnlBottomToolbar.Controls.Add(this.sysToolBar1);
            this.pnlBottomToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBottomToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlBottomToolbar.Name = "pnlBottomToolbar";
            this.pnlBottomToolbar.Size = new System.Drawing.Size(1157, 63);
            this.pnlBottomToolbar.TabIndex = 2;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1157, 63);
            this.sysToolBar1.TabIndex = 1;
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar1_OnBtnExportClicked);
            // 
            // FrmRealTimeResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 664);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlBottomToolbar);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "FrmRealTimeResultView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "仪器结果实时视窗";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRealTimeResultView_FormClosed);
            this.Load += new System.EventHandler(this.FrmResultoView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoRefresh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoResultType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).EndInit();
            this.pnlBottomToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_sid;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_cno;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_a;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_date;
        private DevExpress.XtraGrid.Columns.GridColumn col_itr_mid;
        private DevExpress.XtraGrid.Columns.GridColumn col_msg;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_sid_int;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlBottomToolbar;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private DevExpress.XtraEditors.LabelControl lblTime;
        private dcl.client.control.SelectDicInstrument txtItr;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtSID;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DevExpress.XtraGrid.Columns.GridColumn col_itm_ecd;
        private DevExpress.XtraEditors.CheckEdit chkAutoRefresh;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_b;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_c;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_d;
        private DevExpress.XtraEditors.RadioGroup rdoResultType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}