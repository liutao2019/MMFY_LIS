using dcl.entity;

namespace dcl.client.control
{
  partial class SelectDicLabProfession
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
            this.coltype_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltype_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltype_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltype_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.popupContainerEdit1.Size = new System.Drawing.Size(113, 24);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(433, 222);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicPubProfession);
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
            this.coltype_id,
            this.coltype_name,
            this.coltype_py,
            this.coltype_wb,
            this.gridColumn1});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // coltype_id
            // 
            this.coltype_id.Caption = "编码";
            this.coltype_id.FieldName = "ProId";
            this.coltype_id.Name = "coltype_id";
            this.coltype_id.Visible = true;
            this.coltype_id.VisibleIndex = 0;
            // 
            // coltype_name
            // 
            this.coltype_name.Caption = "名称";
            this.coltype_name.FieldName = "ProName";
            this.coltype_name.Name = "coltype_name";
            this.coltype_name.Visible = true;
            this.coltype_name.VisibleIndex = 1;
            // 
            // coltype_py
            // 
            this.coltype_py.Caption = "拼音";
            this.coltype_py.FieldName = "ProPyCode";
            this.coltype_py.Name = "coltype_py";
            this.coltype_py.Visible = true;
            this.coltype_py.VisibleIndex = 2;
            // 
            // coltype_wb
            // 
            this.coltype_wb.Caption = "五笔";
            this.coltype_wb.FieldName = "ProWbCode";
            this.coltype_wb.Name = "coltype_wb";
            this.coltype_wb.Visible = true;
            this.coltype_wb.VisibleIndex = 3;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "医院";
            this.gridColumn1.FieldName = "ProOrgName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 4;
            // 
            // SelectDicLabProfession
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "ProName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "ProPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "ProId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "ProWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "ProId", true));
            this.Name = "SelectDicLabProfession";
            this.Size = new System.Drawing.Size(113, 21);
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
    private DevExpress.XtraGrid.Columns.GridColumn coltype_id;
    private DevExpress.XtraGrid.Columns.GridColumn coltype_name;
    private DevExpress.XtraGrid.Columns.GridColumn coltype_py;
    private DevExpress.XtraGrid.Columns.GridColumn coltype_wb;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;

  }
}
