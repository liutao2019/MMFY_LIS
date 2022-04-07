namespace dcl.client.control
{
    partial class SelectDicCheckType
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
            this.colfet_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colfet_cont = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.popupContainerEdit1.Size = new System.Drawing.Size(130, 24);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(368, 179);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicCheckType);
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
            this.colfet_id,
            this.colfet_cont});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colfet_id
            // 
            this.colfet_id.Caption = "编码";
            this.colfet_id.FieldName = "CheckTypeId";
            this.colfet_id.Name = "colfet_id";
            this.colfet_id.OptionsColumn.AllowEdit = false;
            this.colfet_id.OptionsColumn.AllowFocus = false;
            this.colfet_id.Visible = true;
            this.colfet_id.VisibleIndex = 0;
            // 
            // colfet_cont
            // 
            this.colfet_cont.Caption = "费用类别";
            this.colfet_cont.FieldName = "CheckTypeName";
            this.colfet_cont.Name = "colfet_cont";
            this.colfet_cont.OptionsColumn.AllowEdit = false;
            this.colfet_cont.OptionsColumn.AllowFocus = false;
            this.colfet_cont.Visible = true;
            this.colfet_cont.VisibleIndex = 1;
            // 
            // SelectDicCheckType
            // 
            this.colDisplay = "CheckTypeName";
            this.colInCode = "CheckTypeId";
            this.colPY = "CheckTypePyCode";
            this.colValue = "CheckTypeId";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "FeesTypeName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "FeesTypeId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "FeesTypePyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "FeesTypeWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "FeesTypeId", true));
            this.Name = "SelectDicCheckType";
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
    private DevExpress.XtraGrid.Columns.GridColumn colfet_id;
    private DevExpress.XtraGrid.Columns.GridColumn colfet_cont;

  }
}
