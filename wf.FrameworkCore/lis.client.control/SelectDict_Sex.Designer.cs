namespace lis.client.control
{
    partial class SelectDict_Sex
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
            this.bsSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsBasic1 = new lis.client.control.dsBasic();
            this.dsBasic = new lis.client.control.dsBasic();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coltype_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltype_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dict_typeTableAdapter = new lis.client.control.dsBasicTableAdapters.dict_typeTableAdapter();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dict_sexTableAdapter = new lis.client.control.dsBasicTableAdapters.dict_sexTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dtSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasic1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Size = new System.Drawing.Size(220, 24);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(200, 222);
            // 
            // bsSource
            // 
            this.bsSource.DataMember = "dict_sex";
            this.bsSource.DataSource = this.dsBasic1;
            // 
            // dsBasic1
            // 
            this.dsBasic1.DataSetName = "dsBasic";
            this.dsBasic1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dsBasic
            // 
            this.dsBasic.DataSetName = "dsBasic";
            this.dsBasic.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.MaximumSize = new System.Drawing.Size(200, 0);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(200, 222);
            this.gridControl2.TabIndex = 2;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coltype_id,
            this.coltype_name});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // coltype_id
            // 
            this.coltype_id.Caption = "编码";
            this.coltype_id.FieldName = "type_id";
            this.coltype_id.Name = "coltype_id";
            this.coltype_id.OptionsColumn.AllowEdit = false;
            this.coltype_id.Visible = true;
            this.coltype_id.VisibleIndex = 0;
            // 
            // coltype_name
            // 
            this.coltype_name.Caption = "名称";
            this.coltype_name.FieldName = "type_name";
            this.coltype_name.Name = "coltype_name";
            this.coltype_name.OptionsColumn.AllowEdit = false;
            this.coltype_name.Visible = true;
            this.coltype_name.VisibleIndex = 1;
            // 
            // dict_typeTableAdapter
            // 
            this.dict_typeTableAdapter.ClearBeforeFill = true;
            // 
            // dict_sexTableAdapter
            // 
            this.dict_sexTableAdapter.ClearBeforeFill = true;
            // 
            // SelectDict_Sex
            // 
            this.colDisplay = "type_name";
            this.colInCode = "type_id";
            this.colPY = "type_py";
            this.colValue = "type_id";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "type_name", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "type_py", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "type_id", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "type_wb", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "type_id", true));
            this.Name = "SelectDict_Sex";
            this.Size = new System.Drawing.Size(75, 7);
            this.onBeforeFilter += new lis.client.control.HopePopSelect.beforeFilter(this.SelectDict_Type_onBeforeFilter);
            ((System.ComponentModel.ISupportInitialize)(this.dtSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasic1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource bsSource;
    private DevExpress.XtraGrid.GridControl gridControl2;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    private DevExpress.XtraGrid.Columns.GridColumn coltype_id;
    private DevExpress.XtraGrid.Columns.GridColumn coltype_name;
    private dsBasic dsBasic;
    private lis.client.control.dsBasicTableAdapters.dict_typeTableAdapter dict_typeTableAdapter;
    private System.Windows.Forms.BindingSource bindingSource1;
    private dsBasic dsBasic1;
    private lis.client.control.dsBasicTableAdapters.dict_sexTableAdapter dict_sexTableAdapter;

  }
}
