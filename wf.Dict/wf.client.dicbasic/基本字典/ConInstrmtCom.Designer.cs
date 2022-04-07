namespace dcl.client.dicbasic
{
    partial class ConInstrmtCom
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
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bsInstrmt = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colitr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_mid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltype_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_seq = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.gcCombineIn = new DevExpress.XtraGrid.GridControl();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl3 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.btnDelAllUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddAllUser = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gcCombineNotIn = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtItemSort = new DevExpress.XtraEditors.TextEdit();
            this.lueType = new dcl.client.control.SelectDicLabProfession();
            this.label1 = new System.Windows.Forms.Label();
            this.colitr_no = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInstrmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCombineIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).BeginInit();
            this.splitContainerControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCombineNotIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSort.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1229, 731);
            this.panelControl2.TabIndex = 1;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1225, 727);
            this.splitContainerControl1.SplitterPosition = 358;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsInstrmt;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(358, 727);
            this.gridControl1.TabIndex = 47;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsInstrmt
            // 
            this.bsInstrmt.DataSource = typeof(dcl.entity.EntityDicInstrument);
            this.bsInstrmt.CurrentChanged += new System.EventHandler(this.bsInstrmt_CurrentChanged);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colitr_id,
            this.colitr_mid,
            this.coltype_name,
            this.colitr_seq});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colitr_id
            // 
            this.colitr_id.Caption = "编码";
            this.colitr_id.FieldName = "ItrId";
            this.colitr_id.Name = "colitr_id";
            this.colitr_id.Visible = true;
            this.colitr_id.VisibleIndex = 0;
            // 
            // colitr_mid
            // 
            this.colitr_mid.Caption = "仪器代码";
            this.colitr_mid.FieldName = "ItrEname";
            this.colitr_mid.Name = "colitr_mid";
            this.colitr_mid.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ItrEname", "记录总数:{0:0.##}")});
            this.colitr_mid.Visible = true;
            this.colitr_mid.VisibleIndex = 1;
            this.colitr_mid.Width = 128;
            // 
            // coltype_name
            // 
            this.coltype_name.Caption = "实验组别";
            this.coltype_name.FieldName = "ItrTypeName";
            this.coltype_name.Name = "coltype_name";
            this.coltype_name.Visible = true;
            this.coltype_name.VisibleIndex = 2;
            // 
            // colitr_seq
            // 
            this.colitr_seq.AppearanceCell.Options.UseTextOptions = true;
            this.colitr_seq.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colitr_seq.Caption = "序号";
            this.colitr_seq.FieldName = "SortNo";
            this.colitr_seq.Name = "colitr_seq";
            this.colitr_seq.Width = 44;
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.groupControl3);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.splitContainerControl3);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(861, 727);
            this.splitContainerControl2.SplitterPosition = 487;
            this.splitContainerControl2.TabIndex = 4;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // groupControl3
            // 
            this.groupControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl3.Appearance.Options.UseFont = true;
            this.groupControl3.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl3.AppearanceCaption.Options.UseFont = true;
            this.groupControl3.Controls.Add(this.gcCombineIn);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(487, 727);
            this.groupControl3.TabIndex = 5;
            this.groupControl3.Text = "已包含组合";
            // 
            // gcCombineIn
            // 
            this.gcCombineIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCombineIn.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcCombineIn.Location = new System.Drawing.Point(2, 27);
            this.gcCombineIn.MainView = this.gridView4;
            this.gcCombineIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcCombineIn.Name = "gcCombineIn";
            this.gcCombineIn.Size = new System.Drawing.Size(483, 698);
            this.gcCombineIn.TabIndex = 0;
            this.gcCombineIn.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView4});
            this.gcCombineIn.DoubleClick += new System.EventHandler(this.gcCombineIn_DoubleClick);
            // 
            // gridView4
            // 
            this.gridView4.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn8});
            this.gridView4.GridControl = this.gcCombineIn;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView4.OptionsView.ShowFooter = true;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            this.gridView4.OptionsView.ShowIndicator = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "编码";
            this.gridColumn3.FieldName = "CombineComId";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 56;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "组合名称";
            this.gridColumn4.FieldName = "ComName";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ComName", "记录总数:{0:0.##}")});
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 127;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.Caption = "开始标本号";
            this.gridColumn5.FieldName = "StartSid";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 85;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn6.Caption = "结束标本号";
            this.gridColumn6.FieldName = "EndSid";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 86;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "组合代码";
            this.gridColumn8.FieldName = "ComCode";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 4;
            this.gridColumn8.Width = 129;
            // 
            // splitContainerControl3
            // 
            this.splitContainerControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl3.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl3.Name = "splitContainerControl3";
            this.splitContainerControl3.Panel1.Controls.Add(this.groupControl2);
            this.splitContainerControl3.Panel1.Text = "Panel1";
            this.splitContainerControl3.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl3.Panel2.Text = "Panel2";
            this.splitContainerControl3.Size = new System.Drawing.Size(368, 727);
            this.splitContainerControl3.SplitterPosition = 71;
            this.splitContainerControl3.TabIndex = 5;
            this.splitContainerControl3.Text = "splitContainerControl3";
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.btnDelAllUser);
            this.groupControl2.Controls.Add(this.btnDelUser);
            this.groupControl2.Controls.Add(this.btnAddUser);
            this.groupControl2.Controls.Add(this.btnAddAllUser);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(71, 727);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "操作";
            // 
            // btnDelAllUser
            // 
            this.btnDelAllUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnDelAllUser.Location = new System.Drawing.Point(5, 394);
            this.btnDelAllUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelAllUser.Name = "btnDelAllUser";
            this.btnDelAllUser.Size = new System.Drawing.Size(60, 30);
            this.btnDelAllUser.TabIndex = 3;
            this.btnDelAllUser.Text = ">>";
            this.btnDelAllUser.Click += new System.EventHandler(this.btnDelAllUser_Click);
            // 
            // btnDelUser
            // 
            this.btnDelUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnDelUser.Location = new System.Drawing.Point(5, 342);
            this.btnDelUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelUser.Name = "btnDelUser";
            this.btnDelUser.Size = new System.Drawing.Size(60, 30);
            this.btnDelUser.TabIndex = 2;
            this.btnDelUser.Text = ">";
            this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnAddUser.Location = new System.Drawing.Point(5, 288);
            this.btnAddUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(60, 30);
            this.btnAddUser.TabIndex = 1;
            this.btnAddUser.Text = "<";
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnAddAllUser
            // 
            this.btnAddAllUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnAddAllUser.Location = new System.Drawing.Point(5, 236);
            this.btnAddAllUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddAllUser.Name = "btnAddAllUser";
            this.btnAddAllUser.Size = new System.Drawing.Size(60, 30);
            this.btnAddAllUser.TabIndex = 0;
            this.btnAddAllUser.Text = "<<";
            this.btnAddAllUser.Click += new System.EventHandler(this.btnAddAllUser_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.gcCombineNotIn);
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(291, 727);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "未包含组合";
            // 
            // gcCombineNotIn
            // 
            this.gcCombineNotIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCombineNotIn.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcCombineNotIn.Location = new System.Drawing.Point(2, 66);
            this.gcCombineNotIn.MainView = this.gridView2;
            this.gcCombineNotIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcCombineNotIn.Name = "gcCombineNotIn";
            this.gcCombineNotIn.Size = new System.Drawing.Size(287, 659);
            this.gcCombineNotIn.TabIndex = 0;
            this.gcCombineNotIn.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            this.gcCombineNotIn.DoubleClick += new System.EventHandler(this.gcCombineNotIn_DoubleClick);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn7});
            this.gridView2.GridControl = this.gcCombineNotIn;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowFooter = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "编码";
            this.gridColumn1.FieldName = "CombineComId";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 59;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "组合名称";
            this.gridColumn2.FieldName = "ComName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ComName", "记录总数:{0:0.##}")});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 136;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "组合代码";
            this.gridColumn7.FieldName = "ComCode";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 92;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtItemSort);
            this.panel1.Controls.Add(this.lueType);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 27);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(287, 39);
            this.panel1.TabIndex = 1;
            // 
            // txtItemSort
            // 
            this.txtItemSort.EnterMoveNextControl = true;
            this.txtItemSort.Location = new System.Drawing.Point(249, 9);
            this.txtItemSort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtItemSort.Name = "txtItemSort";
            this.txtItemSort.Size = new System.Drawing.Size(131, 24);
            this.txtItemSort.TabIndex = 67;
            this.txtItemSort.TextChanged += new System.EventHandler(this.txtItemSort_TextChanged);
            // 
            // lueType
            // 
            this.lueType.AddEmptyRow = true;
            this.lueType.BindByValue = true;
            this.lueType.colDisplay = "";
            this.lueType.colExtend1 = null;
            this.lueType.colInCode = "";
            this.lueType.colPY = "";
            this.lueType.colSeq = "";
            this.lueType.colValue = "";
            this.lueType.colWB = "";
            this.lueType.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsInstrmt, "ItrLabId", true));
            this.lueType.displayMember = "";
            this.lueType.EnterMoveNext = true;
            this.lueType.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueType.KeyUpDownMoveNext = false;
            this.lueType.LoadDataOnDesignMode = true;
            this.lueType.Location = new System.Drawing.Point(53, 9);
            this.lueType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueType.MaximumSize = new System.Drawing.Size(571, 25);
            this.lueType.MinimumSize = new System.Drawing.Size(57, 25);
            this.lueType.Name = "lueType";
            this.lueType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueType.Readonly = false;
            this.lueType.SaveSourceID = false;
            this.lueType.SelectFilter = null;
            this.lueType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueType.SelectOnly = true;
            this.lueType.Size = new System.Drawing.Size(178, 25);
            this.lueType.TabIndex = 1;
            this.lueType.UseExtend = false;
            this.lueType.valueMember = "";
            this.lueType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.lueType_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "过滤";
            // 
            // colitr_no
            // 
            this.colitr_no.Caption = "itr_no";
            this.colitr_no.FieldName = "itr_no";
            this.colitr_no.Name = "colitr_no";
            this.colitr_no.Width = 57;
            // 
            // ConInstrmtCom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "ConInstrmtCom";
            this.Size = new System.Drawing.Size(1229, 731);
            this.Load += new System.EventHandler(this.on_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInstrmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCombineIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).EndInit();
            this.splitContainerControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCombineNotIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtItemSort.Properties)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion


    private DevExpress.XtraEditors.PanelControl panelControl2;
    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    private System.Windows.Forms.BindingSource bsInstrmt;
    private DevExpress.XtraGrid.Columns.GridColumn colitr_no;
    private DevExpress.XtraGrid.GridControl gridControl1;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    private DevExpress.XtraGrid.Columns.GridColumn colitr_id;
    private DevExpress.XtraGrid.Columns.GridColumn colitr_mid;
    private DevExpress.XtraGrid.Columns.GridColumn coltype_name;
    private DevExpress.XtraGrid.Columns.GridColumn colitr_seq;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraGrid.GridControl gcCombineIn;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl3;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnDelAllUser;
        private DevExpress.XtraEditors.SimpleButton btnDelUser;
        private DevExpress.XtraEditors.SimpleButton btnAddUser;
        private DevExpress.XtraEditors.SimpleButton btnAddAllUser;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl gcCombineNotIn;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private System.Windows.Forms.Panel panel1;
        private control.SelectDicLabProfession lueType;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtItemSort;
    }
}
