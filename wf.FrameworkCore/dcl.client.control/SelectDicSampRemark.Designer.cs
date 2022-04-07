namespace dcl.client.control
{
    partial class SelectDicSampRemark
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
            this.colref_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colref_name = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.popupContainerControl1.Size = new System.Drawing.Size(368, 179);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicSampRemark);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(368, 179);
            this.gridControl2.TabIndex = 2;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colref_id,
            this.colref_name});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colref_id
            // 
            this.colref_id.Caption = "编码";
            this.colref_id.FieldName = "RemId";
            this.colref_id.Name = "colref_id";
            this.colref_id.OptionsColumn.AllowEdit = false;
            this.colref_id.Visible = true;
            this.colref_id.VisibleIndex = 0;
            // 
            // colref_name
            // 
            this.colref_name.Caption = "标本备注";
            this.colref_name.FieldName = "RemContent";
            this.colref_name.Name = "colref_name";
            this.colref_name.OptionsColumn.AllowEdit = false;
            this.colref_name.Visible = true;
            this.colref_name.VisibleIndex = 1;
            // 
            // SelectDicSampRemark
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "RemWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "RemPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "RemId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "RemContent", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "RemId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "RemCCode", true));
            this.Name = "SelectDicSampRemark";
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
    private DevExpress.XtraGrid.Columns.GridColumn colref_id;
    private DevExpress.XtraGrid.Columns.GridColumn colref_name;

  }
}
