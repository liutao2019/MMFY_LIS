namespace dcl.client.control
{
  partial class SelectDicReaSupplier
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
            this.bsSource = new System.Windows.Forms.BindingSource();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colsupplier_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsupplier_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpy_ccode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpy_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colwb_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicReaSupplier);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.colsupplier_name,
            this.colsupplier_id,
            this.colpy_ccode,
            this.colpy_code,
            this.colwb_code,
            this.gridColumn1});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colsupplier_name
            // 
            this.colsupplier_name.Caption = "供货商";
            this.colsupplier_name.FieldName = "Rsupplier_name";
            this.colsupplier_name.Name = "colsupplier_name";
            this.colsupplier_name.OptionsColumn.AllowEdit = false;
            this.colsupplier_name.Visible = true;
            this.colsupplier_name.VisibleIndex = 1;
            this.colsupplier_name.Width = 85;
            // 
            // colsupplier_id
            // 
            this.colsupplier_id.Caption = "代码";
            this.colsupplier_id.FieldName = "Rsupplier_id";
            this.colsupplier_id.Name = "colsupplier_id";
            this.colsupplier_id.OptionsColumn.AllowEdit = false;
            this.colsupplier_id.Visible = true;
            this.colsupplier_id.VisibleIndex = 0;
            // 
            // colpy_ccode
            // 
            this.colpy_ccode.Caption = "输入码";
            this.colpy_ccode.FieldName = "py_code";
            this.colpy_ccode.Name = "colpy_ccode";
            this.colpy_ccode.OptionsColumn.AllowEdit = false;
            this.colpy_ccode.Visible = true;
            this.colpy_ccode.VisibleIndex = 2;
            // 
            // colpy_code
            // 
            this.colpy_code.Caption = "拼音";
            this.colpy_code.FieldName = "py_code";
            this.colpy_code.Name = "colpy_code";
            this.colpy_code.OptionsColumn.AllowEdit = false;
            this.colpy_code.Visible = true;
            this.colpy_code.VisibleIndex = 3;
            // 
            // colwb_code
            // 
            this.colwb_code.Caption = "五笔";
            this.colwb_code.FieldName = "wb_code";
            this.colwb_code.Name = "colwb_code";
            this.colwb_code.OptionsColumn.AllowEdit = false;
            this.colwb_code.Visible = true;
            this.colwb_code.VisibleIndex = 4;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "sort_no";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            // 
            // SelectDicReaSupplier
            // 
            this.colDisplay = "";
            this.colExtend1 = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "wb_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "py_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "py_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "Rsupplier_name", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colExtend1", this.bsSource, "py_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "Rsupplier_id", true));
            this.Name = "SelectDicReaSupplier";
            this.Size = new System.Drawing.Size(130, 21);
            this.Leave += new System.EventHandler(this.SelectDicPubDept_Leave);
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
    private DevExpress.XtraGrid.Columns.GridColumn colsupplier_name;
    private DevExpress.XtraGrid.Columns.GridColumn colsupplier_id;
    private DevExpress.XtraGrid.Columns.GridColumn colpy_ccode;
    private DevExpress.XtraGrid.Columns.GridColumn colpy_code;
    private DevExpress.XtraGrid.Columns.GridColumn colwb_code;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;

  }
}
