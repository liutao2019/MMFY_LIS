namespace dcl.client.dicbasic
{
    partial class ConItem_Prop
  {
    /// <summary> 
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override  void  Dispose(bool disposing)
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
            this.bsItem = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colitm_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltype_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_seq1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.bsItem_Prop = new System.Windows.Forms.BindingSource();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colpro_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_ecd1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_prop = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_seq = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem_Prop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1334, 900);
            this.panelControl2.TabIndex = 1;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl15);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1330, 896);
            this.splitContainerControl1.SplitterPosition = 624;
            this.splitContainerControl1.TabIndex = 69;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsItem;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.gridControl1.Size = new System.Drawing.Size(499, 896);
            this.gridControl1.TabIndex = 53;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsItem
            // 
            this.bsItem.DataSource = typeof(dcl.entity.EntityDicItmItem);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colitm_id,
            this.colitm_name,
            this.colitm_ecd,
            this.coltype_name,
            this.gridColumn2,
            this.colitm_seq1});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            this.gridView1.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView1_RowStyle);
            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // colitm_id
            // 
            this.colitm_id.Caption = "编码";
            this.colitm_id.FieldName = "ItmId";
            this.colitm_id.Name = "colitm_id";
            this.colitm_id.Visible = true;
            this.colitm_id.VisibleIndex = 0;
            this.colitm_id.Width = 45;
            // 
            // colitm_name
            // 
            this.colitm_name.Caption = "项目名称";
            this.colitm_name.FieldName = "ItmName";
            this.colitm_name.Name = "colitm_name";
            this.colitm_name.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ItmName", "记录总数:{0:0.##}")});
            this.colitm_name.Visible = true;
            this.colitm_name.VisibleIndex = 3;
            this.colitm_name.Width = 62;
            // 
            // colitm_ecd
            // 
            this.colitm_ecd.Caption = "项目代码";
            this.colitm_ecd.FieldName = "ItmEcode";
            this.colitm_ecd.Name = "colitm_ecd";
            this.colitm_ecd.Visible = true;
            this.colitm_ecd.VisibleIndex = 2;
            this.colitm_ecd.Width = 60;
            // 
            // coltype_name
            // 
            this.coltype_name.Caption = "组别";
            this.coltype_name.FieldName = "ProName";
            this.coltype_name.Name = "coltype_name";
            this.coltype_name.Visible = true;
            this.coltype_name.VisibleIndex = 1;
            this.coltype_name.Width = 50;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "特征";
            this.gridColumn2.FieldName = "propCount";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 58;
            // 
            // colitm_seq1
            // 
            this.colitm_seq1.AppearanceCell.Options.UseTextOptions = true;
            this.colitm_seq1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colitm_seq1.Caption = "序号";
            this.colitm_seq1.FieldName = "ItmSortNo";
            this.colitm_seq1.Name = "colitm_seq1";
            this.colitm_seq1.Width = 46;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(18, 20);
            this.labelControl15.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(0, 18);
            this.labelControl15.TabIndex = 47;
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsItem_Prop;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.First.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.Last.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.Next.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.NextPage.Enabled = false;
            this.gridControl2.EmbeddedNavigator.Buttons.NextPage.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.Prev.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.PrevPage.Visible = false;
            this.gridControl2.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridControl2.EmbeddedNavigator.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 6, true, true, "", "Add"),
            new DevExpress.XtraEditors.NavigatorCustomButton(-1, 7, true, true, "", "Delete")});
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl2.EmbeddedNavigator.TextStringFormat = "";
            this.gridControl2.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridControl2_EmbeddedNavigator_ButtonClick);
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gridControl2.Size = new System.Drawing.Size(825, 896);
            this.gridControl2.TabIndex = 0;
            this.gridControl2.UseEmbeddedNavigator = true;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            this.gridControl2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl2_KeyDown);
            // 
            // bsItem_Prop
            // 
            this.bsItem_Prop.DataSource = typeof(dcl.entity.EntityDefItmProperty);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colpro_id,
            this.colitm_ecd1,
            this.colitm_prop,
            this.colitm_incode,
            this.colitm_py,
            this.colitm_wb,
            this.colitm_seq,
            this.gridColumn1});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ColumnAutoWidth = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.gridView2_InitNewRow);
            this.gridView2.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView2_CellValueChanged);
            // 
            // colpro_id
            // 
            this.colpro_id.Caption = "编码";
            this.colpro_id.FieldName = "PtyId";
            this.colpro_id.Name = "colpro_id";
            this.colpro_id.OptionsColumn.AllowEdit = false;
            this.colpro_id.OptionsColumn.FixedWidth = true;
            this.colpro_id.Width = 64;
            // 
            // colitm_ecd1
            // 
            this.colitm_ecd1.Caption = "项目代码";
            this.colitm_ecd1.FieldName = "PtyItmEname";
            this.colitm_ecd1.Name = "colitm_ecd1";
            this.colitm_ecd1.OptionsColumn.FixedWidth = true;
            this.colitm_ecd1.Width = 83;
            // 
            // colitm_prop
            // 
            this.colitm_prop.Caption = "项目特征";
            this.colitm_prop.FieldName = "PtyItmProperty";
            this.colitm_prop.Name = "colitm_prop";
            this.colitm_prop.OptionsColumn.FixedWidth = true;
            this.colitm_prop.Visible = true;
            this.colitm_prop.VisibleIndex = 0;
            this.colitm_prop.Width = 96;
            // 
            // colitm_incode
            // 
            this.colitm_incode.Caption = "输入码";
            this.colitm_incode.FieldName = "PtyCCode";
            this.colitm_incode.Name = "colitm_incode";
            this.colitm_incode.Visible = true;
            this.colitm_incode.VisibleIndex = 3;
            // 
            // colitm_py
            // 
            this.colitm_py.Caption = "拼音码";
            this.colitm_py.FieldName = "PtyPyCode";
            this.colitm_py.Name = "colitm_py";
            this.colitm_py.OptionsColumn.FixedWidth = true;
            this.colitm_py.OptionsColumn.ReadOnly = true;
            this.colitm_py.Visible = true;
            this.colitm_py.VisibleIndex = 1;
            this.colitm_py.Width = 60;
            // 
            // colitm_wb
            // 
            this.colitm_wb.Caption = "五笔码";
            this.colitm_wb.FieldName = "PtyWbCode";
            this.colitm_wb.Name = "colitm_wb";
            this.colitm_wb.OptionsColumn.FixedWidth = true;
            this.colitm_wb.OptionsColumn.ReadOnly = true;
            this.colitm_wb.Visible = true;
            this.colitm_wb.VisibleIndex = 2;
            this.colitm_wb.Width = 60;
            // 
            // colitm_seq
            // 
            this.colitm_seq.Caption = "排序";
            this.colitm_seq.FieldName = "PtySortNo";
            this.colitm_seq.Name = "colitm_seq";
            this.colitm_seq.OptionsColumn.FixedWidth = true;
            this.colitm_seq.Visible = true;
            this.colitm_seq.VisibleIndex = 4;
            this.colitm_seq.Width = 60;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "公共";
            this.gridColumn1.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn1.FieldName = "PtyItmFlag";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.FixedWidth = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 5;
            this.gridColumn1.Width = 51;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit1.ValueChecked = 1;
            this.repositoryItemCheckEdit1.ValueUnchecked = 0;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = 6;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // ConItem_Prop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "ConItem_Prop";
            this.Size = new System.Drawing.Size(1334, 900);
            this.Load += new System.EventHandler(this.on_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem_Prop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion


    private DevExpress.XtraEditors.PanelControl panelControl2;
    private DevExpress.XtraBars.BarButtonItem barButtonItem1;
    private System.Windows.Forms.BindingSource bsItem_Prop;
    private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    private System.Windows.Forms.BindingSource bsItem;
    private DevExpress.XtraGrid.GridControl gridControl2;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_prop;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_py;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_wb;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_seq;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_ecd1;
    private DevExpress.XtraGrid.Columns.GridColumn colpro_id;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
    private DevExpress.XtraEditors.LabelControl labelControl15;
    private DevExpress.XtraGrid.GridControl gridControl1;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_id;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_name;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_ecd;
    private DevExpress.XtraGrid.Columns.GridColumn coltype_name;
    private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_incode;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_seq1;
  }
}
