namespace dcl.client.result.PatControl
{
    partial class ControlResultMerge
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bsPatients = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRefreshLeft = new DevExpress.XtraEditors.SimpleButton();
            this.treeRight = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn10 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn4 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn5 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn6 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn8 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn7 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn9 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn11 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnSearchCopy = new DevExpress.XtraEditors.SimpleButton();
            this.cbKeepOpt = new System.Windows.Forms.CheckBox();
            this.chkGetNonePatInfo = new System.Windows.Forms.CheckBox();
            this.txtPatDate = new DevExpress.XtraEditors.DateEdit();
            this.txtPatInstructment = new dcl.client.control.SelectDicInstrument();
            this.txtPatType = new dcl.client.control.SelectDicLabProfession();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearchRight = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCopy = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.treeRight);
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl3);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(799, 544);
            this.splitContainerControl1.SplitterPosition = 384;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsPatients;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 110);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(384, 434);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl1_KeyDown);
            this.gridControl1.Leave += new System.EventHandler(this.gridControl1_Leave);
            this.gridControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gridControl1_MouseDoubleClick);
            // 
            // bsPatients
            // 
            this.bsPatients.DataSource = typeof(dcl.entity.EntityPidReportMain);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsDetail.EnableMasterViewMode = false;
            this.gridView1.OptionsDetail.ShowDetailTabs = false;
            this.gridView1.OptionsDetail.SmartDetailExpand = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "样本号";
            this.gridColumn1.FieldName = "RepSid";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowMove = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 61;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "序号";
            this.gridColumn2.FieldName = "RepSerialNum";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsColumn.AllowMove = false;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 41;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "姓名";
            this.gridColumn3.FieldName = "PidName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.AllowMove = false;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 77;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "目标样本号";
            this.gridColumn4.FieldName = "DestRepSid";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsColumn.AllowMove = false;
            this.gridColumn4.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 69;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "病人ID";
            this.gridColumn5.FieldName = "PidInNo";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "部门";
            this.gridColumn6.FieldName = "PidDeptName";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.btnRefreshLeft);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(384, 110);
            this.panelControl1.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "目标仪器：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "日    期：";
            // 
            // btnRefreshLeft
            // 
            this.btnRefreshLeft.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnRefreshLeft.Appearance.Options.UseFont = true;
            this.btnRefreshLeft.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnRefreshLeft.Location = new System.Drawing.Point(3, 79);
            this.btnRefreshLeft.Name = "btnRefreshLeft";
            this.btnRefreshLeft.Size = new System.Drawing.Size(75, 28);
            this.btnRefreshLeft.TabIndex = 1;
            this.btnRefreshLeft.Text = "刷新";
            this.btnRefreshLeft.Click += new System.EventHandler(this.btnRefreshLeft_Click);
            // 
            // treeRight
            // 
            this.treeRight.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn10,
            this.treeListColumn2,
            this.treeListColumn3,
            this.treeListColumn4,
            this.treeListColumn5,
            this.treeListColumn6,
            this.treeListColumn8,
            this.treeListColumn7,
            this.treeListColumn9,
            this.treeListColumn11});
            this.treeRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeRight.KeyFieldName = "parent_res_sid";
            this.treeRight.Location = new System.Drawing.Point(0, 110);
            this.treeRight.Name = "treeRight";
            this.treeRight.OptionsBehavior.AllowExpandOnDblClick = false;
            this.treeRight.OptionsView.ShowIndicator = false;
            this.treeRight.ParentFieldName = "res_sid_int";
            this.treeRight.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.treeRight.Size = new System.Drawing.Size(410, 434);
            this.treeRight.TabIndex = 0;
            this.treeRight.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeRight_MouseDoubleClick);
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "样本号";
            this.treeListColumn1.FieldName = "res_sid";
            this.treeListColumn1.MinWidth = 38;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.OptionsColumn.AllowMove = false;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 76;
            // 
            // treeListColumn10
            // 
            this.treeListColumn10.Caption = "选择";
            this.treeListColumn10.ColumnEdit = this.repositoryItemCheckEdit1;
            this.treeListColumn10.FieldName = "select_res";
            this.treeListColumn10.Name = "treeListColumn10";
            this.treeListColumn10.Visible = true;
            this.treeListColumn10.VisibleIndex = 1;
            this.treeListColumn10.Width = 60;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.CheckedChanged += new System.EventHandler(this.repositoryItemCheckEdit1_CheckedChanged);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "项目代码";
            this.treeListColumn2.FieldName = "res_itm_ecd";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowEdit = false;
            this.treeListColumn2.OptionsColumn.AllowMove = false;
            this.treeListColumn2.OptionsColumn.AllowSort = false;
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 2;
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "结果";
            this.treeListColumn3.FieldName = "res_chr";
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.OptionsColumn.AllowEdit = false;
            this.treeListColumn3.OptionsColumn.AllowMove = false;
            this.treeListColumn3.OptionsColumn.AllowSort = false;
            this.treeListColumn3.Visible = true;
            this.treeListColumn3.VisibleIndex = 3;
            // 
            // treeListColumn4
            // 
            this.treeListColumn4.Caption = "treeListColumn4";
            this.treeListColumn4.FieldName = "pat_date";
            this.treeListColumn4.Name = "treeListColumn4";
            // 
            // treeListColumn5
            // 
            this.treeListColumn5.Caption = "treeListColumn5";
            this.treeListColumn5.FieldName = "res_itr_id";
            this.treeListColumn5.Name = "treeListColumn5";
            // 
            // treeListColumn6
            // 
            this.treeListColumn6.Caption = "病人ID";
            this.treeListColumn6.FieldName = "pat_in_no";
            this.treeListColumn6.Name = "treeListColumn6";
            this.treeListColumn6.OptionsColumn.AllowEdit = false;
            this.treeListColumn6.Visible = true;
            this.treeListColumn6.VisibleIndex = 4;
            // 
            // treeListColumn8
            // 
            this.treeListColumn8.Caption = "姓名";
            this.treeListColumn8.FieldName = "pat_name";
            this.treeListColumn8.Name = "treeListColumn8";
            this.treeListColumn8.OptionsColumn.AllowEdit = false;
            this.treeListColumn8.Visible = true;
            this.treeListColumn8.VisibleIndex = 5;
            // 
            // treeListColumn7
            // 
            this.treeListColumn7.Caption = "部门";
            this.treeListColumn7.FieldName = "pat_dep_name";
            this.treeListColumn7.Name = "treeListColumn7";
            this.treeListColumn7.OptionsColumn.AllowEdit = false;
            this.treeListColumn7.Visible = true;
            this.treeListColumn7.VisibleIndex = 6;
            // 
            // treeListColumn9
            // 
            this.treeListColumn9.Caption = "treeListColumn9";
            this.treeListColumn9.FieldName = "res_id";
            this.treeListColumn9.Name = "treeListColumn9";
            // 
            // treeListColumn11
            // 
            this.treeListColumn11.Caption = "项目编号";
            this.treeListColumn11.FieldName = "res_itm_id";
            this.treeListColumn11.Name = "treeListColumn11";
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.btnSearchCopy);
            this.panelControl3.Controls.Add(this.cbKeepOpt);
            this.panelControl3.Controls.Add(this.chkGetNonePatInfo);
            this.panelControl3.Controls.Add(this.txtPatDate);
            this.panelControl3.Controls.Add(this.txtPatInstructment);
            this.panelControl3.Controls.Add(this.txtPatType);
            this.panelControl3.Controls.Add(this.label3);
            this.panelControl3.Controls.Add(this.label2);
            this.panelControl3.Controls.Add(this.label1);
            this.panelControl3.Controls.Add(this.btnSearchRight);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(410, 110);
            this.panelControl3.TabIndex = 3;
            // 
            // btnSearchCopy
            // 
            this.btnSearchCopy.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnSearchCopy.Appearance.Options.UseFont = true;
            this.btnSearchCopy.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnSearchCopy.Location = new System.Drawing.Point(81, 79);
            this.btnSearchCopy.Name = "btnSearchCopy";
            this.btnSearchCopy.Size = new System.Drawing.Size(75, 28);
            this.btnSearchCopy.TabIndex = 10;
            this.btnSearchCopy.Text = "复制刷新";
            this.btnSearchCopy.Click += new System.EventHandler(this.btnSearchCopy_Click);
            // 
            // cbKeepOpt
            // 
            this.cbKeepOpt.AutoSize = true;
            this.cbKeepOpt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbKeepOpt.Location = new System.Drawing.Point(227, 10);
            this.cbKeepOpt.Name = "cbKeepOpt";
            this.cbKeepOpt.Size = new System.Drawing.Size(144, 18);
            this.cbKeepOpt.TabIndex = 9;
            this.cbKeepOpt.Text = "选项不随报告信息改变";
            this.cbKeepOpt.UseVisualStyleBackColor = true;
            this.cbKeepOpt.CheckedChanged += new System.EventHandler(this.cbKeepOpt_CheckedChanged);
            // 
            // chkGetNonePatInfo
            // 
            this.chkGetNonePatInfo.AutoSize = true;
            this.chkGetNonePatInfo.Checked = true;
            this.chkGetNonePatInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGetNonePatInfo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkGetNonePatInfo.Location = new System.Drawing.Point(227, 36);
            this.chkGetNonePatInfo.Name = "chkGetNonePatInfo";
            this.chkGetNonePatInfo.Size = new System.Drawing.Size(156, 18);
            this.chkGetNonePatInfo.TabIndex = 8;
            this.chkGetNonePatInfo.Text = "只获取无病人资料的结果";
            this.chkGetNonePatInfo.UseVisualStyleBackColor = true;
            // 
            // txtPatDate
            // 
            this.txtPatDate.EditValue = null;
            this.txtPatDate.EnterMoveNextControl = true;
            this.txtPatDate.Location = new System.Drawing.Point(81, 5);
            this.txtPatDate.Name = "txtPatDate";
            this.txtPatDate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtPatDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPatDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPatDate.Size = new System.Drawing.Size(129, 20);
            this.txtPatDate.TabIndex = 6;
            this.txtPatDate.EditValueChanged += new System.EventHandler(this.txtPatDate_EditValueChanged);
            this.txtPatDate.Leave += new System.EventHandler(this.txtPatDate_Leave);
            // 
            // txtPatInstructment
            // 
            this.txtPatInstructment.AutoScroll = true;
            this.txtPatInstructment.BindByValue = false;
            this.txtPatInstructment.colDisplay = "";
            this.txtPatInstructment.colExtend1 = null;
            this.txtPatInstructment.colInCode = "";
            this.txtPatInstructment.colPY = "";
            this.txtPatInstructment.colSeq = "";
            this.txtPatInstructment.colValue = "";
            this.txtPatInstructment.colWB = "";
            this.txtPatInstructment.displayMember = null;
            this.txtPatInstructment.EnterMoveNext = true;
            this.txtPatInstructment.KeyUpDownMoveNext = true;
            this.txtPatInstructment.LoadDataOnDesignMode = true;
            this.txtPatInstructment.Location = new System.Drawing.Point(81, 54);
            this.txtPatInstructment.MaximumSize = new System.Drawing.Size(500, 21);
            this.txtPatInstructment.MinimumSize = new System.Drawing.Size(50, 21);
            this.txtPatInstructment.Name = "txtPatInstructment";
            this.txtPatInstructment.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPatInstructment.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtPatInstructment.Readonly = false;
            this.txtPatInstructment.SaveSourceID = false;
            this.txtPatInstructment.SelectFilter = null;
            this.txtPatInstructment.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPatInstructment.SelectOnly = true;
            this.txtPatInstructment.ShowAllInstrmt = false;
            this.txtPatInstructment.Size = new System.Drawing.Size(129, 21);
            this.txtPatInstructment.TabIndex = 4;
            this.txtPatInstructment.UseExtend = false;
            this.txtPatInstructment.valueMember = null;
            this.txtPatInstructment.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.txtPatInstructment_ValueChanged);
            // 
            // txtPatType
            // 
            this.txtPatType.AutoScroll = true;
            this.txtPatType.BindByValue = false;
            this.txtPatType.colDisplay = "";
            this.txtPatType.colExtend1 = null;
            this.txtPatType.colInCode = "";
            this.txtPatType.colPY = "";
            this.txtPatType.colSeq = "";
            this.txtPatType.colValue = "";
            this.txtPatType.colWB = "";
            this.txtPatType.displayMember = null;
            this.txtPatType.EnterMoveNext = true;
            this.txtPatType.KeyUpDownMoveNext = true;
            this.txtPatType.LoadDataOnDesignMode = true;
            this.txtPatType.Location = new System.Drawing.Point(81, 29);
            this.txtPatType.MaximumSize = new System.Drawing.Size(500, 21);
            this.txtPatType.MinimumSize = new System.Drawing.Size(50, 21);
            this.txtPatType.Name = "txtPatType";
            this.txtPatType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPatType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtPatType.Readonly = false;
            this.txtPatType.SaveSourceID = false;
            this.txtPatType.SelectFilter = null;
            this.txtPatType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPatType.SelectOnly = true;
            this.txtPatType.Size = new System.Drawing.Size(129, 21);
            this.txtPatType.TabIndex = 2;
            this.txtPatType.UseExtend = false;
            this.txtPatType.valueMember = null;
            this.txtPatType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.txtPatType_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "日    期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "源仪器：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "物理组别：";
            // 
            // btnSearchRight
            // 
            this.btnSearchRight.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnSearchRight.Appearance.Options.UseFont = true;
            this.btnSearchRight.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnSearchRight.Location = new System.Drawing.Point(3, 79);
            this.btnSearchRight.Name = "btnSearchRight";
            this.btnSearchRight.Size = new System.Drawing.Size(75, 28);
            this.btnSearchRight.TabIndex = 1;
            this.btnSearchRight.Text = "合并刷新";
            this.btnSearchRight.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.btnCopy);
            this.panelControl2.Controls.Add(this.simpleButton1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 544);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(799, 36);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCopy
            // 
            this.btnCopy.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.btnCopy.Appearance.Options.UseFont = true;
            this.btnCopy.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnCopy.Location = new System.Drawing.Point(86, 4);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 28);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "复制";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("宋体", 12F);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.simpleButton1.Location = new System.Drawing.Point(5, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 28);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "合并(&C)";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // ControlResultMerge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.panelControl2);
            this.Name = "ControlResultMerge";
            this.Size = new System.Drawing.Size(799, 580);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTreeList.TreeList treeRight;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton btnRefreshLeft;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btnSearchRight;
        private System.Windows.Forms.Label label1;
        protected dcl.client.control.SelectDicLabProfession txtPatType;
        protected dcl.client.control.SelectDicInstrument txtPatInstructment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        protected DevExpress.XtraEditors.DateEdit txtPatDate;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn4;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkGetNonePatInfo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn6;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn8;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn7;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn9;
        private System.Windows.Forms.CheckBox cbKeepOpt;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn10;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn11;
        private DevExpress.XtraEditors.SimpleButton btnSearchCopy;
        private DevExpress.XtraEditors.SimpleButton btnCopy;
        private System.Windows.Forms.BindingSource bsPatients;
    }
}
