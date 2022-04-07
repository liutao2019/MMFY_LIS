namespace dcl.client.control
{
  partial class SelectDicPubIdent
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
            this.colno_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colno_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colno_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colno_wb = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicPubIdent);
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
            this.colno_id,
            this.colno_name,
            this.colno_py,
            this.colno_wb});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colno_id
            // 
            this.colno_id.Caption = "编码";
            this.colno_id.FieldName = "IdtId";
            this.colno_id.Name = "colno_id";
            this.colno_id.Visible = true;
            this.colno_id.VisibleIndex = 0;
            // 
            // colno_name
            // 
            this.colno_name.Caption = "名称";
            this.colno_name.FieldName = "IdtName";
            this.colno_name.Name = "colno_name";
            this.colno_name.Visible = true;
            this.colno_name.VisibleIndex = 1;
            // 
            // colno_py
            // 
            this.colno_py.Caption = "拼音";
            this.colno_py.FieldName = "IdtPyCode";
            this.colno_py.Name = "colno_py";
            this.colno_py.Visible = true;
            this.colno_py.VisibleIndex = 2;
            // 
            // colno_wb
            // 
            this.colno_wb.Caption = "五笔";
            this.colno_wb.FieldName = "IdtWbCode";
            this.colno_wb.Name = "colno_wb";
            this.colno_wb.Visible = true;
            this.colno_wb.VisibleIndex = 3;
            // 
            // SelectDicPubIdent
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "IdtName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "IdtPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "IdtId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "IdtWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "IdtCCode", true));
            this.Name = "SelectDicPubIdent";
            this.Size = new System.Drawing.Size(130, 20);
            this.Leave += new System.EventHandler(this.SelectDicPubIdent_Leave);
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
    private DevExpress.XtraGrid.Columns.GridColumn colori_id;
    private DevExpress.XtraGrid.Columns.GridColumn colori_name;
    private DevExpress.XtraGrid.Columns.GridColumn colori_py;
    private DevExpress.XtraGrid.Columns.GridColumn colori_wb;
    private DevExpress.XtraGrid.Columns.GridColumn colno_id;
    private DevExpress.XtraGrid.Columns.GridColumn colno_name;
    private DevExpress.XtraGrid.Columns.GridColumn colno_py;
    private DevExpress.XtraGrid.Columns.GridColumn colno_wb;

  }
}
