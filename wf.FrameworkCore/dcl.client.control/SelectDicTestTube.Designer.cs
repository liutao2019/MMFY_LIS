namespace dcl.client.control
{
    partial class SelectDicTestTube
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colcuv_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcuv_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_wb = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicTestTube);
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
            this.colcuv_code,
            this.colcuv_name,
            this.colsam_incode,
            this.colsam_py,
            this.colsam_wb});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // colcuv_code
            // 
            this.colcuv_code.Caption = "编码";
            this.colcuv_code.FieldName = "TubCode";
            this.colcuv_code.Name = "colcuv_code";
            this.colcuv_code.Visible = true;
            this.colcuv_code.VisibleIndex = 0;
            // 
            // colcuv_name
            // 
            this.colcuv_name.Caption = "容器名称";
            this.colcuv_name.FieldName = "TubName";
            this.colcuv_name.Name = "colcuv_name";
            this.colcuv_name.Visible = true;
            this.colcuv_name.VisibleIndex = 1;
            // 
            // colsam_incode
            // 
            this.colsam_incode.Caption = "最大采集量";
            this.colsam_incode.FieldName = "TubMaxCapcity";
            this.colsam_incode.Name = "colsam_incode";
            this.colsam_incode.Visible = true;
            this.colsam_incode.VisibleIndex = 2;
            // 
            // colsam_py
            // 
            this.colsam_py.Caption = "拼音";
            this.colsam_py.FieldName = "TubPyCode";
            this.colsam_py.Name = "colsam_py";
            this.colsam_py.Visible = true;
            this.colsam_py.VisibleIndex = 3;
            // 
            // colsam_wb
            // 
            this.colsam_wb.Caption = "五笔";
            this.colsam_wb.FieldName = "TubWbCode";
            this.colsam_wb.Name = "colsam_wb";
            this.colsam_wb.Visible = true;
            this.colsam_wb.VisibleIndex = 4;
            // 
            // SelectDicTestTube
            // 
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "TubCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "TubWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "TubCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "TubPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "TubName", true));
            this.Name = "SelectDicTestTube";
            this.Size = new System.Drawing.Size(136, 20);
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
    private DevExpress.XtraGrid.Columns.GridColumn colcuv_code;
    private DevExpress.XtraGrid.Columns.GridColumn colcuv_name;
    private DevExpress.XtraGrid.Columns.GridColumn colsam_incode;
    private DevExpress.XtraGrid.Columns.GridColumn colsam_py;
    private DevExpress.XtraGrid.Columns.GridColumn colsam_wb;

  }
}
