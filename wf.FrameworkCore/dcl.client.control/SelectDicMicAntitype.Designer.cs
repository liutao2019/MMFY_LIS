namespace dcl.client.control
{
    partial class SelectDicMicAntitype
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
            this.colst_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_cname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_wb = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicMicAntitype);
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
            this.colst_id,
            this.colst_cname,
            this.colst_py,
            this.colst_wb});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colst_id
            // 
            this.colst_id.Caption = "编码";
            this.colst_id.FieldName = "AtypeId";
            this.colst_id.Name = "colst_id";
            this.colst_id.Visible = true;
            this.colst_id.VisibleIndex = 0;
            this.colst_id.Width = 50;
            // 
            // colst_cname
            // 
            this.colst_cname.Caption = "药敏分类";
            this.colst_cname.FieldName = "AtypeName";
            this.colst_cname.Name = "colst_cname";
            this.colst_cname.Visible = true;
            this.colst_cname.VisibleIndex = 1;
            this.colst_cname.Width = 145;
            // 
            // colst_py
            // 
            this.colst_py.Caption = "拼音";
            this.colst_py.FieldName = "APyCode";
            this.colst_py.Name = "colst_py";
            this.colst_py.Visible = true;
            this.colst_py.VisibleIndex = 2;
            this.colst_py.Width = 106;
            // 
            // colst_wb
            // 
            this.colst_wb.Caption = "五笔";
            this.colst_wb.FieldName = "AWbCode";
            this.colst_wb.Name = "colst_wb";
            this.colst_wb.Visible = true;
            this.colst_wb.VisibleIndex = 3;
            this.colst_wb.Width = 111;
            // 
            // SelectDicMicAntitype
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "AWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "APyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "ACCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "AtypeId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "ASortNo", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "AtypeName", true));
            this.Name = "SelectDicMicAntitype";
            this.SelectOnly = false;
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
    private DevExpress.XtraGrid.Columns.GridColumn colst_id;
    private DevExpress.XtraGrid.Columns.GridColumn colst_cname;
    private DevExpress.XtraGrid.Columns.GridColumn colst_py;
    private DevExpress.XtraGrid.Columns.GridColumn colst_wb;

  }
}
