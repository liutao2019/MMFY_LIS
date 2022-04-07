namespace dcl.client.msgclient
{
  partial class SelectDicPubDoctor
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
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.bsSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coldoc_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldoc_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldoc_deptname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldoc_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldoc_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldoc_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Size = new System.Drawing.Size(473, 20);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl2);
            this.popupContainerControl1.Location = new System.Drawing.Point(20, 48);
            this.popupContainerControl1.Size = new System.Drawing.Size(433, 222);
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
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicDoctor);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coldoc_name,
            this.coldoc_code,
            this.coldoc_deptname,
            this.gridColumn2,
            this.coldoc_incode,
            this.coldoc_py,
            this.coldoc_wb,
            this.gridColumn1});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // coldoc_name
            // 
            this.coldoc_name.Caption = "姓名";
            this.coldoc_name.FieldName = "DoctorName";
            this.coldoc_name.Name = "coldoc_name";
            this.coldoc_name.Visible = true;
            this.coldoc_name.VisibleIndex = 0;
            // 
            // coldoc_code
            // 
            this.coldoc_code.Caption = "工号";
            this.coldoc_code.FieldName = "DoctorCode";
            this.coldoc_code.Name = "coldoc_code";
            this.coldoc_code.Visible = true;
            this.coldoc_code.VisibleIndex = 1;
            // 
            // coldoc_deptname
            // 
            this.coldoc_deptname.Caption = "科室";
            this.coldoc_deptname.FieldName = "DoctorDeptName";
            this.coldoc_deptname.Name = "coldoc_deptname";
            this.coldoc_deptname.Visible = true;
            this.coldoc_deptname.VisibleIndex = 5;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "电话";
            this.gridColumn2.FieldName = "DoctorTel";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 6;
            // 
            // coldoc_incode
            // 
            this.coldoc_incode.Caption = "输入码";
            this.coldoc_incode.FieldName = "CCode";
            this.coldoc_incode.Name = "coldoc_incode";
            this.coldoc_incode.Visible = true;
            this.coldoc_incode.VisibleIndex = 2;
            // 
            // coldoc_py
            // 
            this.coldoc_py.Caption = "拼音";
            this.coldoc_py.FieldName = "PyCode";
            this.coldoc_py.Name = "coldoc_py";
            this.coldoc_py.Visible = true;
            this.coldoc_py.VisibleIndex = 3;
            // 
            // coldoc_wb
            // 
            this.coldoc_wb.Caption = "五笔";
            this.coldoc_wb.FieldName = "WbCode";
            this.coldoc_wb.Name = "coldoc_wb";
            this.coldoc_wb.Visible = true;
            this.coldoc_wb.VisibleIndex = 4;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "排序";
            this.gridColumn1.FieldName = "SortNo";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            // 
            // SelectDicPubDoctor
            // 
            this.colDisplay = "";
            this.colExtend1 = "doc_code";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "WbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "DoctorId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "DoctorId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "PyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "CCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "DoctorName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colExtend1", this.bsSource, "DoctorCode", true));
            this.Name = "SelectDicPubDoctor";
            this.Size = new System.Drawing.Size(473, 21);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraGrid.GridControl gridControl2;
    private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
    private System.Windows.Forms.BindingSource bsSource;
    //private lis.client.control.dsBasicTableAdapters.dict_doctorTableAdapter dict_doctorTableAdapter;
    private DevExpress.XtraGrid.Columns.GridColumn coldoc_name;
    private DevExpress.XtraGrid.Columns.GridColumn coldoc_code;
    private DevExpress.XtraGrid.Columns.GridColumn coldoc_incode;
    private DevExpress.XtraGrid.Columns.GridColumn coldoc_py;
    private DevExpress.XtraGrid.Columns.GridColumn coldoc_wb;
    private DevExpress.XtraGrid.Columns.GridColumn coldoc_deptname;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;

  }
}
