namespace dcl.client.control
{
  partial class SelectDicReaSetting
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
            this.colrea_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrea_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpy_ccode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpy_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colwb_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.bsSource.DataSource = typeof(dcl.entity.EntityReaSetting);
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
            this.colrea_name,
            this.colrea_id,
            this.colpy_ccode,
            this.colpy_code,
            this.colwb_code,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colrea_name
            // 
            this.colrea_name.Caption = "名称";
            this.colrea_name.FieldName = "Drea_name";
            this.colrea_name.Name = "colrea_name";
            this.colrea_name.OptionsColumn.AllowEdit = false;
            this.colrea_name.Visible = true;
            this.colrea_name.VisibleIndex = 1;
            this.colrea_name.Width = 85;
            // 
            // colrea_id
            // 
            this.colrea_id.Caption = "代码";
            this.colrea_id.FieldName = "Drea_id";
            this.colrea_id.Name = "colrea_id";
            this.colrea_id.OptionsColumn.AllowEdit = false;
            this.colrea_id.Visible = true;
            this.colrea_id.VisibleIndex = 0;
            // 
            // colpy_ccode
            // 
            this.colpy_ccode.Caption = "输入码";
            this.colpy_ccode.FieldName = "py_code";
            this.colpy_ccode.Name = "colpy_ccode";
            this.colpy_ccode.OptionsColumn.AllowEdit = false;
            this.colpy_ccode.Visible = true;
            this.colpy_ccode.VisibleIndex = 2;
            // 
            // colpy_code
            // 
            this.colpy_code.Caption = "拼音";
            this.colpy_code.FieldName = "py_code";
            this.colpy_code.Name = "colpy_code";
            this.colpy_code.OptionsColumn.AllowEdit = false;
            this.colpy_code.Visible = true;
            this.colpy_code.VisibleIndex = 3;
            // 
            // colwb_code
            // 
            this.colwb_code.Caption = "五笔";
            this.colwb_code.FieldName = "wb_code";
            this.colwb_code.Name = "colwb_code";
            this.colwb_code.OptionsColumn.AllowEdit = false;
            this.colwb_code.Visible = true;
            this.colwb_code.VisibleIndex = 4;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "sort_no";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "试剂组别";
            this.gridColumn2.FieldName = "Rea_group";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 5;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "规格";
            this.gridColumn3.FieldName = "Drea_package";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 6;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "供货商";
            this.gridColumn4.FieldName = "Rsupplier_name";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 7;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "单位";
            this.gridColumn5.FieldName = "Runit_name";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 8;
            // 
            // SelectDicReaSetting
            // 
            this.colDisplay = "";
            this.colExtend1 = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "wb_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "py_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "py_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "Drea_name", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colExtend1", this.bsSource, "py_code", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "Drea_id", true));
            this.Name = "SelectDicReaSetting";
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
    private DevExpress.XtraGrid.Columns.GridColumn colrea_name;
    private DevExpress.XtraGrid.Columns.GridColumn colrea_id;
    private DevExpress.XtraGrid.Columns.GridColumn colpy_ccode;
    private DevExpress.XtraGrid.Columns.GridColumn colpy_code;
    private DevExpress.XtraGrid.Columns.GridColumn colwb_code;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}
