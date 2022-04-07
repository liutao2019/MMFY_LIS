namespace dcl.client.control
{
  partial class SelectDicPubOrganize
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
            this.colhos_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhos_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhos_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhos_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhos_wb = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.popupContainerEdit1.Size = new System.Drawing.Size(155, 20);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(433, 222);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicPubOrganize);
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
            this.colhos_id,
            this.colhos_name,
            this.colhos_code,
            this.colhos_py,
            this.colhos_wb});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colhos_id
            // 
            this.colhos_id.Caption = "编码";
            this.colhos_id.FieldName = "OrgId";
            this.colhos_id.Name = "colhos_id";
            this.colhos_id.Visible = true;
            this.colhos_id.VisibleIndex = 0;
            // 
            // colhos_name
            // 
            this.colhos_name.Caption = "名称";
            this.colhos_name.FieldName = "OrgName";
            this.colhos_name.Name = "colhos_name";
            this.colhos_name.Visible = true;
            this.colhos_name.VisibleIndex = 1;
            // 
            // colhos_code
            // 
            this.colhos_code.Caption = "代码";
            this.colhos_code.FieldName = "OrgCode";
            this.colhos_code.Name = "colhos_code";
            this.colhos_code.Visible = true;
            this.colhos_code.VisibleIndex = 2;
            // 
            // colhos_py
            // 
            this.colhos_py.Caption = "拼音";
            this.colhos_py.FieldName = "OrgPyCode";
            this.colhos_py.Name = "colhos_py";
            this.colhos_py.Visible = true;
            this.colhos_py.VisibleIndex = 3;
            // 
            // colhos_wb
            // 
            this.colhos_wb.Caption = "五笔";
            this.colhos_wb.FieldName = "OrgWbCode";
            this.colhos_wb.Name = "colhos_wb";
            this.colhos_wb.Visible = true;
            this.colhos_wb.VisibleIndex = 4;
            // 
            // SelectDicPubOrganize
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = ""; 
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "OrgWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "OrgId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "OrgPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "OrgCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "OrgName", true));
            this.Name = "SelectDicPubOrganize";
            this.Size = new System.Drawing.Size(155, 20);
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
    //private lis.client.control.dsBasicTableAdapters.dict_hospitalTableAdapter dict_hospitalTableAdapter;
    private DevExpress.XtraGrid.Columns.GridColumn colhos_id;
    private DevExpress.XtraGrid.Columns.GridColumn colhos_name;
    private DevExpress.XtraGrid.Columns.GridColumn colhos_code;
    private DevExpress.XtraGrid.Columns.GridColumn colhos_py;
    private DevExpress.XtraGrid.Columns.GridColumn colhos_wb;

  }
}
