namespace dcl.client.control
{
  partial class SelectDicPubDept
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
            this.coldep_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldep_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldep_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldep_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldep_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldep_type = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicPubDept);
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
            this.coldep_name,
            this.coldep_code,
            this.coldep_incode,
            this.coldep_py,
            this.coldep_wb,
            this.coldep_type,
            this.gridColumn1});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // coldep_name
            // 
            this.coldep_name.Caption = "科室名称";
            this.coldep_name.FieldName = "DeptName";
            this.coldep_name.Name = "coldep_name";
            this.coldep_name.OptionsColumn.AllowEdit = false;
            this.coldep_name.Visible = true;
            this.coldep_name.VisibleIndex = 0;
            this.coldep_name.Width = 85;
            // 
            // coldep_code
            // 
            this.coldep_code.Caption = "代码";
            this.coldep_code.FieldName = "DeptCode";
            this.coldep_code.Name = "coldep_code";
            this.coldep_code.OptionsColumn.AllowEdit = false;
            this.coldep_code.Visible = true;
            this.coldep_code.VisibleIndex = 1;
            // 
            // coldep_incode
            // 
            this.coldep_incode.Caption = "输入码";
            this.coldep_incode.FieldName = "DeptCCode";
            this.coldep_incode.Name = "coldep_incode";
            this.coldep_incode.OptionsColumn.AllowEdit = false;
            this.coldep_incode.Visible = true;
            this.coldep_incode.VisibleIndex = 2;
            // 
            // coldep_py
            // 
            this.coldep_py.Caption = "拼音";
            this.coldep_py.FieldName = "DeptPyCode";
            this.coldep_py.Name = "coldep_py";
            this.coldep_py.OptionsColumn.AllowEdit = false;
            this.coldep_py.Visible = true;
            this.coldep_py.VisibleIndex = 3;
            // 
            // coldep_wb
            // 
            this.coldep_wb.Caption = "五笔";
            this.coldep_wb.FieldName = "DeptWbCode";
            this.coldep_wb.Name = "coldep_wb";
            this.coldep_wb.OptionsColumn.AllowEdit = false;
            this.coldep_wb.Visible = true;
            this.coldep_wb.VisibleIndex = 4;
            // 
            // coldep_type
            // 
            this.coldep_type.Caption = "科别";
            this.coldep_type.FieldName = "DeptAccesType";
            this.coldep_type.Name = "coldep_type";
            this.coldep_type.OptionsColumn.AllowEdit = false;
            this.coldep_type.Visible = true;
            this.coldep_type.VisibleIndex = 5;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "DeptSortNo";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            // 
            // SelectDicPubDept
            // 
            this.colDisplay = "";
            this.colExtend1 = "dep_code";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "DeptWbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "DeptPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "DeptCCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "DeptName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colExtend1", this.bsSource, "DeptCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "DeptCode", true));
            this.Name = "SelectDicPubDept";
            this.Size = new System.Drawing.Size(130, 21);
            this.Leave += new System.EventHandler(this.SelectDicPubDept_Leave);
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
    private DevExpress.XtraGrid.Columns.GridColumn coldep_name;
    private DevExpress.XtraGrid.Columns.GridColumn coldep_code;
    private DevExpress.XtraGrid.Columns.GridColumn coldep_incode;
    private DevExpress.XtraGrid.Columns.GridColumn coldep_py;
    private DevExpress.XtraGrid.Columns.GridColumn coldep_wb;
    private DevExpress.XtraGrid.Columns.GridColumn coldep_type;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;

  }
}
