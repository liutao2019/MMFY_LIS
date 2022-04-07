namespace dcl.client.dicbasic
{
    partial class FrmDictMitmNoResultView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDictMitmNoResultView));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridControlSingle = new DevExpress.XtraGrid.GridControl();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewSingle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_selected = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.col_itm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.bsItem = new System.Windows.Forms.BindingSource(this.components);
            this.col_res_cno = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_mit_dec = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_mit_pos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_mit_rlen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_mit_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_mit_flag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.col_msg = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_a = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_b = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_c = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_chr_d = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itr_mid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_sid_int = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_res_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtItr = new dcl.client.control.SelectDicInstrument();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtSID = new DevExpress.XtraEditors.TextEdit();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.lblTime = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.pnlTopToolbar = new System.Windows.Forms.Panel();
            this.pnlBottomToolbar = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSingle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSingle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            this.pnlBottomToolbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridControlSingle);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 79);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2148, 1218);
            this.panel1.TabIndex = 5;
            // 
            // gridControlSingle
            // 
            this.gridControlSingle.DataSource = this.bindingSource1;
            this.gridControlSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlSingle.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControlSingle.Location = new System.Drawing.Point(192, 82);
            this.gridControlSingle.MainView = this.gridViewSingle;
            this.gridControlSingle.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControlSingle.Name = "gridControlSingle";
            this.gridControlSingle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemCheckEdit2,
            this.repositoryItemLookUpEdit1,
            this.repositoryItemSpinEdit1});
            this.gridControlSingle.Size = new System.Drawing.Size(1956, 1136);
            this.gridControlSingle.TabIndex = 0;
            this.gridControlSingle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSingle});
            this.gridControlSingle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridViewSingle_MouseDown);
            // 
            // gridViewSingle
            // 
            this.gridViewSingle.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridViewSingle.Appearance.OddRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridViewSingle.Appearance.OddRow.Options.UseBackColor = true;
            this.gridViewSingle.Appearance.OddRow.Options.UseForeColor = true;
            this.gridViewSingle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_selected,
            this.col_itm_ecd,
            this.col_res_cno,
            this.col_mit_dec,
            this.col_mit_pos,
            this.col_mit_rlen,
            this.col_mit_type,
            this.col_mit_flag,
            this.col_msg,
            this.col_res_chr_a,
            this.col_res_chr_b,
            this.col_res_chr_c,
            this.col_res_chr_d,
            this.col_res_date,
            this.col_itr_mid,
            this.col_res_sid_int,
            this.gridColumn1,
            this.col_res_sid});
            this.gridViewSingle.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewSingle.GridControl = this.gridControlSingle;
            this.gridViewSingle.Name = "gridViewSingle";
            this.gridViewSingle.OptionsBehavior.ImmediateUpdateRowPosition = false;
            this.gridViewSingle.OptionsBehavior.KeepFocusedRowOnUpdate = false;
            this.gridViewSingle.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewSingle.OptionsSelection.MultiSelect = true;
            this.gridViewSingle.OptionsView.AllowCellMerge = true;
            this.gridViewSingle.OptionsView.ColumnAutoWidth = false;
            this.gridViewSingle.OptionsView.ShowGroupPanel = false;
            this.gridViewSingle.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.col_res_date, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gridViewSingle.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gridViewPatientList_CustomDrawColumnHeader);
            // 
            // col_selected
            // 
            this.col_selected.ColumnEdit = this.repositoryItemCheckEdit1;
            this.col_selected.FieldName = "ResSelect";
            this.col_selected.Name = "col_selected";
            this.col_selected.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_selected.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_selected.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.col_selected.OptionsFilter.AllowAutoFilter = false;
            this.col_selected.OptionsFilter.AllowFilter = false;
            this.col_selected.Visible = true;
            this.col_selected.VisibleIndex = 0;
            this.col_selected.Width = 20;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit1.ValueChecked = "1";
            this.repositoryItemCheckEdit1.ValueUnchecked = "0";
            // 
            // col_itm_ecd
            // 
            this.col_itm_ecd.Caption = "项目代码";
            this.col_itm_ecd.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.col_itm_ecd.FieldName = "ItmID";
            this.col_itm_ecd.Name = "col_itm_ecd";
            this.col_itm_ecd.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_itm_ecd.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_itm_ecd.OptionsFilter.AllowAutoFilter = false;
            this.col_itm_ecd.OptionsFilter.AllowFilter = false;
            this.col_itm_ecd.Visible = true;
            this.col_itm_ecd.VisibleIndex = 2;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ProName", "专业组"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmEcode", "项目代码", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmId", "编码", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.repositoryItemLookUpEdit1.DataSource = this.bsItem;
            this.repositoryItemLookUpEdit1.DisplayMember = "ItmEcode";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.PopupWidth = 280;
            this.repositoryItemLookUpEdit1.ValueMember = "ItmId";
            // 
            // bsItem
            // 
            this.bsItem.DataMember = "dict_item";
            // 
            // col_res_cno
            // 
            this.col_res_cno.Caption = "通道码";
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
            this.col_res_cno.VisibleIndex = 3;
            this.col_res_cno.Width = 93;
            // 
            // col_mit_dec
            // 
            this.col_mit_dec.Caption = "小数位";
            this.col_mit_dec.FieldName = "MitDec";
            this.col_mit_dec.Name = "col_mit_dec";
            this.col_mit_dec.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_mit_dec.Visible = true;
            this.col_mit_dec.VisibleIndex = 4;
            // 
            // col_mit_pos
            // 
            this.col_mit_pos.Caption = "起始位置";
            this.col_mit_pos.FieldName = "MitPos";
            this.col_mit_pos.Name = "col_mit_pos";
            this.col_mit_pos.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_mit_pos.Visible = true;
            this.col_mit_pos.VisibleIndex = 5;
            // 
            // col_mit_rlen
            // 
            this.col_mit_rlen.Caption = "结果长度";
            this.col_mit_rlen.FieldName = "MitRlen";
            this.col_mit_rlen.Name = "col_mit_rlen";
            this.col_mit_rlen.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_mit_rlen.Visible = true;
            this.col_mit_rlen.VisibleIndex = 6;
            // 
            // col_mit_type
            // 
            this.col_mit_type.Caption = "结果类型";
            this.col_mit_type.FieldName = "MitType";
            this.col_mit_type.Name = "col_mit_type";
            this.col_mit_type.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_mit_type.Visible = true;
            this.col_mit_type.VisibleIndex = 7;
            // 
            // col_mit_flag
            // 
            this.col_mit_flag.Caption = "双向标志";
            this.col_mit_flag.ColumnEdit = this.repositoryItemSpinEdit1;
            this.col_mit_flag.FieldName = "MitFlag";
            this.col_mit_flag.Name = "col_mit_flag";
            this.col_mit_flag.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_mit_flag.Visible = true;
            this.col_mit_flag.VisibleIndex = 8;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // col_msg
            // 
            this.col_msg.Caption = "备注";
            this.col_msg.FieldName = "Msg";
            this.col_msg.Name = "col_msg";
            this.col_msg.OptionsColumn.AllowEdit = false;
            this.col_msg.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_msg.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_msg.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_msg.OptionsColumn.FixedWidth = true;
            this.col_msg.OptionsColumn.ReadOnly = true;
            this.col_msg.OptionsFilter.AllowAutoFilter = false;
            this.col_msg.OptionsFilter.AllowFilter = false;
            this.col_msg.Visible = true;
            this.col_msg.VisibleIndex = 9;
            this.col_msg.Width = 91;
            // 
            // col_res_chr_a
            // 
            this.col_res_chr_a.Caption = "结果1";
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
            this.col_res_chr_a.VisibleIndex = 10;
            this.col_res_chr_a.Width = 90;
            // 
            // col_res_chr_b
            // 
            this.col_res_chr_b.Caption = "结果2";
            this.col_res_chr_b.FieldName = "ObrValue2";
            this.col_res_chr_b.Name = "col_res_chr_b";
            this.col_res_chr_b.OptionsColumn.AllowEdit = false;
            this.col_res_chr_b.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_b.Visible = true;
            this.col_res_chr_b.VisibleIndex = 11;
            this.col_res_chr_b.Width = 90;
            // 
            // col_res_chr_c
            // 
            this.col_res_chr_c.Caption = "结果3";
            this.col_res_chr_c.FieldName = "ObrValue3";
            this.col_res_chr_c.Name = "col_res_chr_c";
            this.col_res_chr_c.OptionsColumn.AllowEdit = false;
            this.col_res_chr_c.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_c.Visible = true;
            this.col_res_chr_c.VisibleIndex = 12;
            this.col_res_chr_c.Width = 90;
            // 
            // col_res_chr_d
            // 
            this.col_res_chr_d.Caption = "结果4";
            this.col_res_chr_d.FieldName = "ObrValue4";
            this.col_res_chr_d.Name = "col_res_chr_d";
            this.col_res_chr_d.OptionsColumn.AllowEdit = false;
            this.col_res_chr_d.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_chr_d.Visible = true;
            this.col_res_chr_d.VisibleIndex = 13;
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
            this.col_res_date.VisibleIndex = 14;
            this.col_res_date.Width = 132;
            // 
            // col_itr_mid
            // 
            this.col_itr_mid.Caption = "仪器代码";
            this.col_itr_mid.FieldName = "ObrItrId";
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
            this.col_res_sid_int.FieldName = "ResSidInt";
            this.col_res_sid_int.Name = "col_res_sid_int";
            this.col_res_sid_int.OptionsColumn.AllowEdit = false;
            this.col_res_sid_int.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_res_sid_int.OptionsFilter.AllowAutoFilter = false;
            this.col_res_sid_int.OptionsFilter.AllowFilter = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "样本号";
            this.gridColumn1.FieldName = "ResSidInt";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // col_res_sid
            // 
            this.col_res_sid.Caption = "样本号";
            this.col_res_sid.FieldName = "ObrSid";
            this.col_res_sid.Name = "col_res_sid";
            this.col_res_sid.OptionsColumn.AllowEdit = false;
            this.col_res_sid.Visible = true;
            this.col_res_sid.VisibleIndex = 1;
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtItr);
            this.panel3.Controls.Add(this.btnRefresh);
            this.panel3.Controls.Add(this.labelControl3);
            this.panel3.Controls.Add(this.labelControl2);
            this.panel3.Controls.Add(this.txtSID);
            this.panel3.Controls.Add(this.txtDate);
            this.panel3.Controls.Add(this.lblTime);
            this.panel3.Controls.Add(this.labelControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(192, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1956, 82);
            this.panel3.TabIndex = 3;
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
            this.txtItr.Location = new System.Drawing.Point(518, 15);
            this.txtItr.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtItr.MaximumSize = new System.Drawing.Size(928, 42);
            this.txtItr.MinimumSize = new System.Drawing.Size(93, 42);
            this.txtItr.Name = "txtItr";
            this.txtItr.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtItr.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtItr.Readonly = false;
            this.txtItr.SaveSourceID = false;
            this.txtItr.SelectFilter = null;
            this.txtItr.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtItr.SelectOnly = true;
            this.txtItr.ShowAllInstrmt = false;
            this.txtItr.Size = new System.Drawing.Size(242, 42);
            this.txtItr.TabIndex = 11;
            this.txtItr.UseExtend = false;
            this.txtItr.valueMember = null;
            this.txtItr.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.txtItr_ValueChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(1781, 13);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(161, 56);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(1240, 21);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(171, 29);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "例：1-5,7,15-20";
            this.labelControl3.Visible = false;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(811, 19);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(120, 29);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "样  本 号：";
            // 
            // txtSID
            // 
            this.txtSID.EnterMoveNextControl = true;
            this.txtSID.Location = new System.Drawing.Point(941, 13);
            this.txtSID.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(286, 36);
            this.txtSID.TabIndex = 7;
            this.txtSID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSID_KeyDown);
            this.txtSID.Leave += new System.EventHandler(this.txtSID_Leave);
            // 
            // txtDate
            // 
            this.txtDate.EditValue = null;
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(141, 13);
            this.txtDate.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDate.Size = new System.Drawing.Size(205, 36);
            this.txtDate.TabIndex = 4;
            this.txtDate.Leave += new System.EventHandler(this.txtDate_Leave);
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(13, 19);
            this.lblTime.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(120, 29);
            this.lblTime.TabIndex = 5;
            this.lblTime.Text = "测定日期：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(390, 19);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 29);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "测定仪器：";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(192, 1218);
            this.panel2.TabIndex = 2;
            this.panel2.Visible = false;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(2148, 79);
            this.sysToolBar1.TabIndex = 1;
            // 
            // pnlTopToolbar
            // 
            this.pnlTopToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopToolbar.Location = new System.Drawing.Point(0, 0);
            this.pnlTopToolbar.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.pnlTopToolbar.Name = "pnlTopToolbar";
            this.pnlTopToolbar.Size = new System.Drawing.Size(2148, 79);
            this.pnlTopToolbar.TabIndex = 7;
            this.pnlTopToolbar.Visible = false;
            // 
            // pnlBottomToolbar
            // 
            this.pnlBottomToolbar.Controls.Add(this.sysToolBar1);
            this.pnlBottomToolbar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomToolbar.Location = new System.Drawing.Point(0, 1297);
            this.pnlBottomToolbar.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.pnlBottomToolbar.Name = "pnlBottomToolbar";
            this.pnlBottomToolbar.Size = new System.Drawing.Size(2148, 79);
            this.pnlBottomToolbar.TabIndex = 6;
            // 
            // FrmDictMitmNoResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2148, 1376);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlTopToolbar);
            this.Controls.Add(this.pnlBottomToolbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FrmDictMitmNoResultView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "仪器结果实时视窗";
            this.Load += new System.EventHandler(this.FrmDictMitmNoResultView_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSingle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSingle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            this.pnlBottomToolbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControlSingle;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewSingle;
        private DevExpress.XtraGrid.Columns.GridColumn col_itm_ecd;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_a;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_b;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_c;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_chr_d;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_date;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_cno;
        private DevExpress.XtraGrid.Columns.GridColumn col_msg;
        private DevExpress.XtraGrid.Columns.GridColumn col_itr_mid;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_sid_int;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtSID;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private DevExpress.XtraEditors.LabelControl lblTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Panel panel2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Panel pnlTopToolbar;
        private System.Windows.Forms.Panel pnlBottomToolbar;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn col_selected;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn col_res_sid;
        private DevExpress.XtraGrid.Columns.GridColumn col_mit_dec;
        private DevExpress.XtraGrid.Columns.GridColumn col_mit_pos;
        private DevExpress.XtraGrid.Columns.GridColumn col_mit_rlen;
        private DevExpress.XtraGrid.Columns.GridColumn col_mit_type;
        private DevExpress.XtraGrid.Columns.GridColumn col_mit_flag;
        private System.Windows.Forms.BindingSource bsItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private control.SelectDicInstrument txtItr;
    }
}