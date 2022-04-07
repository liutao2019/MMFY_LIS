namespace dcl.client.control
{
  partial class SelectDicCombine
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.bsSource = new System.Windows.Forms.BindingSource();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colcom_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_feecode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Size = new System.Drawing.Size(130, 20);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(433, 222);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicCombine);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.gridControl2.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(433, 222);
            this.gridControl2.TabIndex = 2;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colcom_id,
            this.colcom_feecode,
            this.colcom_name,
            this.colcom_type,
            this.colcom_wb,
            this.colcom_py,
            this.colcom_incode});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colcom_id
            // 
            this.colcom_id.Caption = "编码";
            this.colcom_id.FieldName = "ComId";
            this.colcom_id.Name = "colcom_id";
            this.colcom_id.OptionsColumn.AllowEdit = false;
            this.colcom_id.OptionsColumn.AllowFocus = false;
            this.colcom_id.OptionsColumn.ReadOnly = true;
            this.colcom_id.Visible = true;
            this.colcom_id.VisibleIndex = 0;
            // 
            // colcom_feecode
            // 
            this.colcom_feecode.Caption = "收费编码";
            this.colcom_feecode.FieldName = "ComHisCode";
            this.colcom_feecode.Name = "colcom_feecode";
            this.colcom_feecode.OptionsColumn.AllowEdit = false;
            this.colcom_feecode.OptionsColumn.AllowFocus = false;
            this.colcom_feecode.OptionsColumn.ReadOnly = true;
            this.colcom_feecode.Visible = true;
            this.colcom_feecode.VisibleIndex = 6;
            // 
            // colcom_name
            // 
            this.colcom_name.Caption = "组合名称";
            this.colcom_name.FieldName = "ComName";
            this.colcom_name.Name = "colcom_name";
            this.colcom_name.OptionsColumn.AllowEdit = false;
            this.colcom_name.OptionsColumn.AllowFocus = false;
            this.colcom_name.OptionsColumn.ReadOnly = true;
            this.colcom_name.Visible = true;
            this.colcom_name.VisibleIndex = 1;
            // 
            // colcom_type
            // 
            this.colcom_type.Caption = "组别";
            this.colcom_type.FieldName = "PTypeName";
            this.colcom_type.Name = "colcom_type";
            this.colcom_type.OptionsColumn.AllowEdit = false;
            this.colcom_type.OptionsColumn.AllowFocus = false;
            this.colcom_type.OptionsColumn.ReadOnly = true;
            this.colcom_type.Visible = true;
            this.colcom_type.VisibleIndex = 2;
            // 
            // colcom_wb
            // 
            this.colcom_wb.Caption = "五笔";
            this.colcom_wb.FieldName = "ComWbCode";
            this.colcom_wb.Name = "colcom_wb";
            this.colcom_wb.OptionsColumn.AllowEdit = false;
            this.colcom_wb.OptionsColumn.AllowFocus = false;
            this.colcom_wb.OptionsColumn.ReadOnly = true;
            this.colcom_wb.Visible = true;
            this.colcom_wb.VisibleIndex = 3;
            // 
            // colcom_py
            // 
            this.colcom_py.Caption = "拼音";
            this.colcom_py.FieldName = "ComPyCode";
            this.colcom_py.Name = "colcom_py";
            this.colcom_py.OptionsColumn.AllowEdit = false;
            this.colcom_py.OptionsColumn.AllowFocus = false;
            this.colcom_py.OptionsColumn.ReadOnly = true;
            this.colcom_py.Visible = true;
            this.colcom_py.VisibleIndex = 4;
            // 
            // colcom_incode
            // 
            this.colcom_incode.Caption = "输入码";
            this.colcom_incode.FieldName = "ComCCode";
            this.colcom_incode.Name = "colcom_incode";
            this.colcom_incode.OptionsColumn.AllowEdit = false;
            this.colcom_incode.OptionsColumn.AllowFocus = false;
            this.colcom_incode.OptionsColumn.ReadOnly = true;
            this.colcom_incode.Visible = true;
            this.colcom_incode.VisibleIndex = 5;
            // 
            // SelectDicCombine
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "ComId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "ComPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "ComCCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "ComName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "ComSortNo", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "ComId", true));
            this.Name = "SelectDicCombine";
            this.Size = new System.Drawing.Size(130, 21);
            this.Leave += new System.EventHandler(this.SelectDict_Combine_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource bsSource;
    private DevExpress.XtraGrid.GridControl gridControl2;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    private DevExpress.XtraGrid.Columns.GridColumn colcom_id;
    private DevExpress.XtraGrid.Columns.GridColumn colcom_name;
    private DevExpress.XtraGrid.Columns.GridColumn colcom_type;
    private DevExpress.XtraGrid.Columns.GridColumn colcom_wb;
    private DevExpress.XtraGrid.Columns.GridColumn colcom_py;
    private DevExpress.XtraGrid.Columns.GridColumn colcom_incode;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_feecode;
    }
}
