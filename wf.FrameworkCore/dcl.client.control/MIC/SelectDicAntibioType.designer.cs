namespace dcl.client.control
{
    partial class SelectDicAntibioType
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicAntibioType);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
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
            this.colchk_id.FieldName = "TpID";
            this.colchk_id.Name = "colchk_id";
            this.colchk_id.OptionsColumn.AllowEdit = false;
            this.colchk_id.OptionsColumn.AllowFocus = false;
            this.colchk_id.OptionsColumn.ReadOnly = true;
            this.colchk_id.Visible = true;
            this.colchk_id.VisibleIndex = 0;
            this.colchk_id.Width = 73;
            // 
            // colchk_cname
            // 
            this.colchk_cname.Caption = "中文名称";
            this.colchk_cname.FieldName = "TpCName";
            this.colchk_cname.Name = "colchk_cname";
            this.colchk_cname.OptionsColumn.AllowEdit = false;
            this.colchk_cname.OptionsColumn.AllowFocus = false;
            this.colchk_cname.OptionsColumn.ReadOnly = true;
            this.colchk_cname.Visible = true;
            this.colchk_cname.VisibleIndex = 1;
            this.colchk_cname.Width = 329;
            // 
            // colchk_py
            // 
            this.colchk_py.Caption = "拼音";
            this.colchk_py.FieldName = "TpPY";
            this.colchk_py.Name = "colchk_py";
            this.colchk_py.OptionsColumn.AllowEdit = false;
            this.colchk_py.OptionsColumn.AllowFocus = false;
            this.colchk_py.OptionsColumn.ReadOnly = true;
            this.colchk_py.Visible = true;
            this.colchk_py.VisibleIndex = 2;
            this.colchk_py.Width = 20;
            // 
            // colchk_wb
            // 
            this.colchk_wb.Caption = "五笔";
            this.colchk_wb.FieldName = "TpWB";
            this.colchk_wb.Name = "colchk_wb";
            this.colchk_wb.OptionsColumn.AllowEdit = false;
            this.colchk_wb.OptionsColumn.AllowFocus = false;
            this.colchk_wb.OptionsColumn.ReadOnly = true;
            this.colchk_wb.Visible = true;
            this.colchk_wb.VisibleIndex = 3;
            this.colchk_wb.Width = 20;
            // 
            // SelectDicAntibioType
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "TpWB", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "TpID", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "TpPY", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "TpCName", true));
            this.Name = "SelectDicAntibioType";
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
    private DevExpress.XtraGrid.Columns.GridColumn colchk_id;
    private DevExpress.XtraGrid.Columns.GridColumn colchk_cname;
    private DevExpress.XtraGrid.Columns.GridColumn colchk_py;
    private DevExpress.XtraGrid.Columns.GridColumn colchk_wb;
        //private lis.client.control.dsBasicTableAdapters.dict_checkbTableAdapter dict_checkbTableAdapter;

    }
}
