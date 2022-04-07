namespace dcl.client.control
{
  partial class SelectDicPubIcd
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coldiag_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldiag_diag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldiag_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldiag_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldiag_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldiag_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Size = new System.Drawing.Size(136, 20);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl1);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicPubIcd);
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(432, 264);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coldiag_id,
            this.coldiag_diag,
            this.coldiag_code,
            this.coldiag_incode,
            this.coldiag_py,
            this.coldiag_wb});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // coldiag_id
            // 
            this.coldiag_id.Caption = "编码";
            this.coldiag_id.FieldName = "IcdId";
            this.coldiag_id.Name = "coldiag_id";
            this.coldiag_id.OptionsColumn.AllowEdit = false;
            this.coldiag_id.OptionsColumn.AllowFocus = false;
            this.coldiag_id.Visible = true;
            this.coldiag_id.VisibleIndex = 0;
            // 
            // coldiag_diag
            // 
            this.coldiag_diag.Caption = "诊断";
            this.coldiag_diag.FieldName = "IcdName";
            this.coldiag_diag.Name = "coldiag_diag";
            this.coldiag_diag.OptionsColumn.AllowEdit = false;
            this.coldiag_diag.OptionsColumn.AllowFocus = false;
            this.coldiag_diag.Visible = true;
            this.coldiag_diag.VisibleIndex = 1;
            // 
            // coldiag_code
            // 
            this.coldiag_code.Caption = "代码";
            this.coldiag_code.FieldName = "IcdGbCode";
            this.coldiag_code.Name = "coldiag_code";
            this.coldiag_code.OptionsColumn.AllowEdit = false;
            this.coldiag_code.OptionsColumn.AllowFocus = false;
            this.coldiag_code.Visible = true;
            this.coldiag_code.VisibleIndex = 2;
            // 
            // coldiag_incode
            // 
            this.coldiag_incode.Caption = "输入码";
            this.coldiag_incode.FieldName = "IcdCCode";
            this.coldiag_incode.Name = "coldiag_incode";
            this.coldiag_incode.OptionsColumn.AllowEdit = false;
            this.coldiag_incode.OptionsColumn.AllowFocus = false;
            this.coldiag_incode.Visible = true;
            this.coldiag_incode.VisibleIndex = 3;
            // 
            // coldiag_py
            // 
            this.coldiag_py.Caption = "拼音";
            this.coldiag_py.FieldName = "IcdPyCode";
            this.coldiag_py.Name = "coldiag_py";
            this.coldiag_py.OptionsColumn.AllowEdit = false;
            this.coldiag_py.OptionsColumn.AllowFocus = false;
            this.coldiag_py.Visible = true;
            this.coldiag_py.VisibleIndex = 4;
            // 
            // coldiag_wb
            // 
            this.coldiag_wb.Caption = "五笔";
            this.coldiag_wb.FieldName = "IcdWbCode";
            this.coldiag_wb.Name = "coldiag_wb";
            this.coldiag_wb.OptionsColumn.AllowEdit = false;
            this.coldiag_wb.OptionsColumn.AllowFocus = false;
            this.coldiag_wb.Visible = true;
            this.coldiag_wb.VisibleIndex = 5;
            // 
            // SelectDicPubIcd
            // 
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "IcdWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "IcdPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "IcdCCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "IcdName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "IcdSortNo", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colExtend1", this.bsSource, "IcdGbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "IcdName", true));
            this.Name = "SelectDicPubIcd";
            this.Size = new System.Drawing.Size(136, 21);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.BindingSource bsSource;
    private DevExpress.XtraGrid.GridControl gridControl1;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    private DevExpress.XtraGrid.Columns.GridColumn coldiag_id;
    private DevExpress.XtraGrid.Columns.GridColumn coldiag_diag;
    private DevExpress.XtraGrid.Columns.GridColumn coldiag_code;
    private DevExpress.XtraGrid.Columns.GridColumn coldiag_incode;
    private DevExpress.XtraGrid.Columns.GridColumn coldiag_py;
    private DevExpress.XtraGrid.Columns.GridColumn coldiag_wb;

  }
}
