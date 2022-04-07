namespace dcl.client.control
{
  partial class SelectDicItmItem
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
            this.colitm_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_wb = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicItmItem);
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
            this.colitm_id,
            this.colitm_name,
            this.colitm_ecd,
            this.colitm_py,
            this.colitm_wb});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colitm_id
            // 
            this.colitm_id.Caption = "编号";
            this.colitm_id.FieldName = "ItmId";
            this.colitm_id.Name = "colitm_id";
            this.colitm_id.OptionsColumn.AllowEdit = false;
            this.colitm_id.OptionsColumn.AllowFocus = false;
            this.colitm_id.OptionsColumn.ReadOnly = true;
            this.colitm_id.Visible = true;
            this.colitm_id.VisibleIndex = 0;
            // 
            // colitm_name
            // 
            this.colitm_name.Caption = "项目名称";
            this.colitm_name.FieldName = "ItmName";
            this.colitm_name.Name = "colitm_name";
            this.colitm_name.OptionsColumn.AllowEdit = false;
            this.colitm_name.OptionsColumn.AllowFocus = false;
            this.colitm_name.OptionsColumn.ReadOnly = true;
            this.colitm_name.Visible = true;
            this.colitm_name.VisibleIndex = 1;
            // 
            // colitm_ecd
            // 
            this.colitm_ecd.Caption = "代码";
            this.colitm_ecd.FieldName = "ItmEcode";
            this.colitm_ecd.Name = "colitm_ecd";
            this.colitm_ecd.OptionsColumn.AllowEdit = false;
            this.colitm_ecd.OptionsColumn.AllowFocus = false;
            this.colitm_ecd.OptionsColumn.ReadOnly = true;
            this.colitm_ecd.Visible = true;
            this.colitm_ecd.VisibleIndex = 2;
            // 
            // colitm_py
            // 
            this.colitm_py.Caption = "拼音";
            this.colitm_py.FieldName = "ItmPyCode";
            this.colitm_py.Name = "colitm_py";
            this.colitm_py.OptionsColumn.AllowEdit = false;
            this.colitm_py.OptionsColumn.AllowFocus = false;
            this.colitm_py.OptionsColumn.ReadOnly = true;
            this.colitm_py.Visible = true;
            this.colitm_py.VisibleIndex = 3;
            // 
            // colitm_wb
            // 
            this.colitm_wb.Caption = "五笔";
            this.colitm_wb.FieldName = "ItmWbCode";
            this.colitm_wb.Name = "colitm_wb";
            this.colitm_wb.OptionsColumn.AllowEdit = false;
            this.colitm_wb.OptionsColumn.AllowFocus = false;
            this.colitm_wb.OptionsColumn.ReadOnly = true;
            this.colitm_wb.Visible = true;
            this.colitm_wb.VisibleIndex = 4;
            // 
            // SelectDicItmItem
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "ItmWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "ItmPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "ItmId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "ItmEcode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "ItmSortNo", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "ItmId", true));
            this.Name = "SelectDicItmItem";
            this.Size = new System.Drawing.Size(130, 21);
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
    private DevExpress.XtraGrid.Columns.GridColumn colitm_id;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_name;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_ecd;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_py;
    private DevExpress.XtraGrid.Columns.GridColumn colitm_wb;

  }
}
