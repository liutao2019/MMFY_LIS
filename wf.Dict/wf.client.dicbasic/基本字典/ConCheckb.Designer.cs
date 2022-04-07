namespace dcl.client.dicbasic
{
    partial class ConCheckb
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bsDiagnos = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colchk_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltype_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colchk_cname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.selectDicPubProfession1 = new dcl.client.control.SelectDicPubProfession();
            this.buttonEdit5 = new DevExpress.XtraEditors.ButtonEdit();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.buttonEdit2 = new DevExpress.XtraEditors.ButtonEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.dsBasicDictBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDiagnos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasicDictBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(945, 732);
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
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(941, 728);
            this.splitContainerControl1.SplitterPosition = 443;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsDiagnos;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gridLevelNode1.RelationName = "Level1";
            this.gridControl1.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(443, 728);
            this.gridControl1.TabIndex = 48;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsDiagnos
            // 
            this.bsDiagnos.DataSource = typeof(dcl.entity.EntityDicCheckPurpose);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colchk_id,
            this.coltype_name,
            this.colchk_cname});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // colchk_id
            // 
            this.colchk_id.Caption = "编码";
            this.colchk_id.FieldName = "PurpId";
            this.colchk_id.Name = "colchk_id";
            this.colchk_id.Visible = true;
            this.colchk_id.VisibleIndex = 0;
            // 
            // coltype_name
            // 
            this.coltype_name.Caption = "组别";
            this.coltype_name.FieldName = "TypeName";
            this.coltype_name.Name = "coltype_name";
            this.coltype_name.Visible = true;
            this.coltype_name.VisibleIndex = 1;
            // 
            // colchk_cname
            // 
            this.colchk_cname.Caption = "检查目的";
            this.colchk_cname.FieldName = "PurpName";
            this.colchk_cname.Name = "colchk_cname";
            this.colchk_cname.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "PurpName", "记录总数:{0:0.##}")});
            this.colchk_cname.Visible = true;
            this.colchk_cname.VisibleIndex = 2;
            this.colchk_cname.Width = 157;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(492, 728);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "基本信息";
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.textEdit1);
            this.layoutControl1.Controls.Add(this.selectDicPubProfession1);
            this.layoutControl1.Controls.Add(this.buttonEdit5);
            this.layoutControl1.Controls.Add(this.memoEdit1);
            this.layoutControl1.Controls.Add(this.buttonEdit2);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(2, 27);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(488, 699);
            this.layoutControl1.TabIndex = 22;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // textEdit1
            // 
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsDiagnos, "PurpId", true));
            this.textEdit1.Enabled = false;
            this.textEdit1.Location = new System.Drawing.Point(78, 15);
            this.textEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.ReadOnly = true;
            this.textEdit1.Size = new System.Drawing.Size(395, 24);
            this.textEdit1.StyleController = this.layoutControl1;
            this.textEdit1.TabIndex = 30;
            // 
            // selectDicPubProfession1
            // 
            this.selectDicPubProfession1.AddEmptyRow = true;
            this.selectDicPubProfession1.BindByValue = true;
            this.selectDicPubProfession1.colDisplay = "";
            this.selectDicPubProfession1.colExtend1 = null;
            this.selectDicPubProfession1.colInCode = "";
            this.selectDicPubProfession1.colPY = "";
            this.selectDicPubProfession1.colSeq = "";
            this.selectDicPubProfession1.colValue = "";
            this.selectDicPubProfession1.colWB = "";
            this.selectDicPubProfession1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsDiagnos, "ProId", true));
            this.selectDicPubProfession1.displayMember = "";
            this.selectDicPubProfession1.EnterMoveNext = true;
            this.selectDicPubProfession1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDicPubProfession1.KeyUpDownMoveNext = false;
            this.selectDicPubProfession1.LoadDataOnDesignMode = true;
            this.selectDicPubProfession1.Location = new System.Drawing.Point(78, 440);
            this.selectDicPubProfession1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectDicPubProfession1.MaximumSize = new System.Drawing.Size(571, 26);
            this.selectDicPubProfession1.MinimumSize = new System.Drawing.Size(57, 26);
            this.selectDicPubProfession1.Name = "selectDicPubProfession1";
            this.selectDicPubProfession1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDicPubProfession1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDicPubProfession1.Readonly = false;
            this.selectDicPubProfession1.SaveSourceID = false;
            this.selectDicPubProfession1.SelectFilter = null;
            this.selectDicPubProfession1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDicPubProfession1.SelectOnly = true;
            this.selectDicPubProfession1.Size = new System.Drawing.Size(395, 26);
            this.selectDicPubProfession1.TabIndex = 29;
            this.selectDicPubProfession1.UseExtend = false;
            this.selectDicPubProfession1.valueMember = "";
            // 
            // buttonEdit5
            // 
            this.buttonEdit5.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsDiagnos, "WbCode", true));
            this.buttonEdit5.Enabled = false;
            this.buttonEdit5.EnterMoveNextControl = true;
            this.buttonEdit5.Location = new System.Drawing.Point(313, 470);
            this.buttonEdit5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEdit5.Name = "buttonEdit5";
            this.buttonEdit5.Size = new System.Drawing.Size(158, 24);
            this.buttonEdit5.StyleController = this.layoutControl1;
            this.buttonEdit5.TabIndex = 27;
            // 
            // memoEdit1
            // 
            this.memoEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsDiagnos, "PurpName", true));
            this.memoEdit1.Location = new System.Drawing.Point(78, 45);
            this.memoEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(395, 389);
            this.memoEdit1.StyleController = this.layoutControl1;
            this.memoEdit1.TabIndex = 20;
            this.memoEdit1.Leave += new System.EventHandler(this.memoEdit1_Leave);
            // 
            // buttonEdit2
            // 
            this.buttonEdit2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsDiagnos, "PyCode", true));
            this.buttonEdit2.Enabled = false;
            this.buttonEdit2.EnterMoveNextControl = true;
            this.buttonEdit2.Location = new System.Drawing.Point(80, 470);
            this.buttonEdit2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonEdit2.Name = "buttonEdit2";
            this.buttonEdit2.Size = new System.Drawing.Size(160, 24);
            this.buttonEdit2.StyleController = this.layoutControl1;
            this.buttonEdit2.TabIndex = 8;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlGroup1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.emptySpaceItem1,
            this.layoutControlItem6,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(488, 699);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.buttonEdit5;
            this.layoutControlItem2.CustomizationFormText = "五笔码";
            this.layoutControlItem2.Location = new System.Drawing.Point(233, 457);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(0, 24);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(114, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 1, 1);
            this.layoutControlItem2.Size = new System.Drawing.Size(231, 24);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "五笔码";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(60, 18);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.buttonEdit2;
            this.layoutControlItem3.CustomizationFormText = "拼音码";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 457);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(0, 24);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(114, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 1, 1);
            this.layoutControlItem3.Size = new System.Drawing.Size(233, 24);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Text = "拼音码";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(60, 18);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.memoEdit1;
            this.layoutControlItem4.CustomizationFormText = "检查目的";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(464, 395);
            this.layoutControlItem4.Text = "检查目的";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(60, 18);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 481);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(464, 194);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.selectDicPubProfession1;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 425);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(464, 32);
            this.layoutControlItem6.Text = "专业组别";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(60, 18);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.textEdit1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(464, 30);
            this.layoutControlItem1.Text = "编码";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(60, 18);
            // 
            // dsBasicDictBindingSource
            // 
            this.dsBasicDictBindingSource.Position = 0;
            // 
            // ConCheckb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "ConCheckb";
            this.Size = new System.Drawing.Size(945, 732);
            this.Load += new System.EventHandler(this.on_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDiagnos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasicDictBindingSource)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion


    private DevExpress.XtraEditors.PanelControl panelControl2;
    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    private System.Windows.Forms.BindingSource bsDiagnos;
    private System.Windows.Forms.BindingSource dsBasicDictBindingSource;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colchk_id;
        private DevExpress.XtraGrid.Columns.GridColumn coltype_name;
        private DevExpress.XtraGrid.Columns.GridColumn colchk_cname;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private control.SelectDicPubProfession selectDicPubProfession1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit5;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.ButtonEdit buttonEdit2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}
