namespace dcl.client.control
{
  partial class SelectDicSampStatus
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
            this.colst_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_stat = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_wb = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicSState);
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
            this.colst_id,
            this.colst_stat,
            this.colst_incode,
            this.colst_py,
            this.colst_wb});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // colst_id
            // 
            this.colst_id.Caption = "编码";
            this.colst_id.FieldName = "StauId";
            this.colst_id.Name = "colst_id";
            this.colst_id.OptionsColumn.AllowFocus = false;
            this.colst_id.Visible = true;
            this.colst_id.VisibleIndex = 0;
            this.colst_id.Width = 90;
            // 
            // colst_stat
            // 
            this.colst_stat.Caption = "标本状态";
            this.colst_stat.FieldName = "StauName";
            this.colst_stat.Name = "colst_stat";
            this.colst_stat.OptionsColumn.AllowFocus = false;
            this.colst_stat.Visible = true;
            this.colst_stat.VisibleIndex = 1;
            this.colst_stat.Width = 140;
            // 
            // colst_incode
            // 
            this.colst_incode.Caption = "输入码";
            this.colst_incode.FieldName = "CCode";
            this.colst_incode.Name = "colst_incode";
            this.colst_incode.OptionsColumn.AllowFocus = false;
            this.colst_incode.Visible = true;
            this.colst_incode.VisibleIndex = 2;
            this.colst_incode.Width = 59;
            // 
            // colst_py
            // 
            this.colst_py.Caption = "拼音";
            this.colst_py.FieldName = "PyCode";
            this.colst_py.Name = "colst_py";
            this.colst_py.OptionsColumn.AllowFocus = false;
            this.colst_py.Visible = true;
            this.colst_py.VisibleIndex = 3;
            this.colst_py.Width = 65;
            // 
            // colst_wb
            // 
            this.colst_wb.Caption = "五笔";
            this.colst_wb.FieldName = "WbCode";
            this.colst_wb.Name = "colst_wb";
            this.colst_wb.OptionsColumn.AllowFocus = false;
            this.colst_wb.Visible = true;
            this.colst_wb.VisibleIndex = 4;
            this.colst_wb.Width = 74;
            // 
            // SelectDicSampStatus
            // 
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "WbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "PyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "CCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "StauName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "SortNo", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "StauName", true));
            this.Name = "SelectDicSampStatus";
            this.SelectOnly = false;
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
    //private lis.client.control.dsBasicTableAdapters.dict_s_stateTableAdapter dict_s_stateTableAdapter;
    private DevExpress.XtraGrid.Columns.GridColumn colst_id;
    private DevExpress.XtraGrid.Columns.GridColumn colst_stat;
    private DevExpress.XtraGrid.Columns.GridColumn colst_incode;
    private DevExpress.XtraGrid.Columns.GridColumn colst_py;
    private DevExpress.XtraGrid.Columns.GridColumn colst_wb;

  }
}
