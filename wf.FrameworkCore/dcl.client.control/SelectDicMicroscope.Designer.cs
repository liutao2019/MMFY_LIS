namespace dcl.client.control
{
  partial class SelectDicMicroscope
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
            this.colugr_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colugr_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colugr_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colugr_seq = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 52);
            this.popupContainerControl1.Size = new System.Drawing.Size(433, 241);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicMicroscope);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(433, 241);
            this.gridControl2.TabIndex = 2;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colugr_name,
            this.colugr_py,
            this.colugr_wb,
            this.colugr_seq});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colugr_name
            // 
            this.colugr_name.Caption = "名称";
            this.colugr_name.FieldName = "MicroName";
            this.colugr_name.Name = "colugr_name";
            this.colugr_name.Visible = true;
            this.colugr_name.VisibleIndex = 0;
            // 
            // colugr_py
            // 
            this.colugr_py.Caption = "拼音";
            this.colugr_py.FieldName = "MicroPyCode";
            this.colugr_py.Name = "colugr_py";
            this.colugr_py.Visible = true;
            this.colugr_py.VisibleIndex = 1;
            // 
            // colugr_wb
            // 
            this.colugr_wb.Caption = "五笔";
            this.colugr_wb.FieldName = "MicroWbCode";
            this.colugr_wb.Name = "colugr_wb";
            this.colugr_wb.Visible = true;
            this.colugr_wb.VisibleIndex = 2;
            // 
            // colugr_seq
            // 
            this.colugr_seq.Caption = "排序";
            this.colugr_seq.FieldName = "MicroSortNo";
            this.colugr_seq.Name = "colugr_seq";
            this.colugr_seq.Visible = true;
            this.colugr_seq.VisibleIndex = 3;
            // 
            // SelectDicItmMicroscope
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "MicroWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "MicroId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "MicroPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "MicroName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "MicroId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "MicroSortNo", true));
            this.Name = "SelectDicItmMicroscope";
            this.Size = new System.Drawing.Size(130, 20);
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
    private DevExpress.XtraGrid.Columns.GridColumn colugr_name;
    private DevExpress.XtraGrid.Columns.GridColumn colugr_py;
    private DevExpress.XtraGrid.Columns.GridColumn colugr_wb;
    private DevExpress.XtraGrid.Columns.GridColumn colugr_seq;

  }
}
