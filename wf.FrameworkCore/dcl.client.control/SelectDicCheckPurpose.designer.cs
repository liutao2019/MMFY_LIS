namespace dcl.client.control
{
    partial class SelectDicCheckPurpose
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
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colchk_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colchk_cname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colchk_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colchk_wb = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicCheckPurpose);
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
            this.colchk_id,
            this.colchk_cname,
            this.colchk_py,
            this.colchk_wb});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colchk_id
            // 
            this.colchk_id.Caption = "编码";
            this.colchk_id.FieldName = "PurpId";
            this.colchk_id.Name = "colchk_id";
            this.colchk_id.Visible = true;
            this.colchk_id.VisibleIndex = 0;
            // 
            // colchk_cname
            // 
            this.colchk_cname.Caption = "检查目的";
            this.colchk_cname.FieldName = "PurpName";
            this.colchk_cname.Name = "colchk_cname";
            this.colchk_cname.Visible = true;
            this.colchk_cname.VisibleIndex = 1;
            // 
            // colchk_py
            // 
            this.colchk_py.Caption = "拼音";
            this.colchk_py.FieldName = "PyCode";
            this.colchk_py.Name = "colchk_py";
            this.colchk_py.Visible = true;
            this.colchk_py.VisibleIndex = 2;
            // 
            // colchk_wb
            // 
            this.colchk_wb.Caption = "五笔";
            this.colchk_wb.FieldName = "WbCode";
            this.colchk_wb.Name = "colchk_wb";
            this.colchk_wb.Visible = true;
            this.colchk_wb.VisibleIndex = 3;
            // 
            // SelectDicCheckPurpose
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "WbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "PurpId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "PyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "PurpName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "CCode", true));
            this.Name = "SelectDicCheckPurpose";
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
    //private lis.client.control.dsBasicTableAdapters.dict_departTableAdapter dict_departTableAdapter;
    private DevExpress.XtraGrid.Columns.GridColumn colchk_id;
    private DevExpress.XtraGrid.Columns.GridColumn colchk_cname;
    private DevExpress.XtraGrid.Columns.GridColumn colchk_py;
    private DevExpress.XtraGrid.Columns.GridColumn colchk_wb;
    //private lis.client.control.dsBasicTableAdapters.dict_checkbTableAdapter dict_checkbTableAdapter;

  }
}
