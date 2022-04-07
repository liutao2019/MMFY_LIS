namespace dcl.client.control
{
    partial class SelectDicInstrument
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
            this.colitr_mid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_seq = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.popupContainerControl1.Size = new System.Drawing.Size(633, 222);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicInstrument);
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsSource;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(633, 222);
            this.gridControl2.TabIndex = 2;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colitr_mid,
            this.colitr_name,
            this.colitr_type,
            this.colitr_no,
            this.colitr_wb,
            this.colitr_py,
            this.colitr_incode,
            this.colitr_seq});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colitr_seq, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colitr_mid
            // 
            this.colitr_mid.Caption = "仪器代码";
            this.colitr_mid.FieldName = "ItrEname";
            this.colitr_mid.Name = "colitr_mid";
            this.colitr_mid.OptionsColumn.AllowEdit = false;
            this.colitr_mid.Visible = true;
            this.colitr_mid.VisibleIndex = 1;
            this.colitr_mid.Width = 80;
            // 
            // colitr_name
            // 
            this.colitr_name.Caption = "仪器名称";
            this.colitr_name.FieldName = "ItrName";
            this.colitr_name.Name = "colitr_name";
            this.colitr_name.OptionsColumn.AllowEdit = false;
            this.colitr_name.Visible = true;
            this.colitr_name.VisibleIndex = 2;
            this.colitr_name.Width = 147;
            // 
            // colitr_type
            // 
            this.colitr_type.Caption = "组别";
            this.colitr_type.FieldName = "ItrTypeName";
            this.colitr_type.Name = "colitr_type";
            this.colitr_type.OptionsColumn.AllowEdit = false;
            this.colitr_type.Visible = true;
            this.colitr_type.VisibleIndex = 0;
            this.colitr_type.Width = 50;
            // 
            // colitr_no
            // 
            this.colitr_no.Caption = "编码";
            this.colitr_no.FieldName = "ItrId";
            this.colitr_no.Name = "colitr_no";
            this.colitr_no.OptionsColumn.AllowEdit = false;
            this.colitr_no.Visible = true;
            this.colitr_no.VisibleIndex = 3;
            this.colitr_no.Width = 49;
            // 
            // colitr_wb
            // 
            this.colitr_wb.Caption = "五笔";
            this.colitr_wb.FieldName = "WbCode";
            this.colitr_wb.Name = "colitr_wb";
            this.colitr_wb.OptionsColumn.AllowEdit = false;
            this.colitr_wb.Visible = true;
            this.colitr_wb.VisibleIndex = 4;
            this.colitr_wb.Width = 90;
            // 
            // colitr_py
            // 
            this.colitr_py.Caption = "拼音";
            this.colitr_py.FieldName = "PyCode";
            this.colitr_py.Name = "colitr_py";
            this.colitr_py.OptionsColumn.AllowEdit = false;
            this.colitr_py.Visible = true;
            this.colitr_py.VisibleIndex = 5;
            this.colitr_py.Width = 90;
            // 
            // colitr_incode
            // 
            this.colitr_incode.Caption = "输入码";
            this.colitr_incode.FieldName = "CCode";
            this.colitr_incode.Name = "colitr_incode";
            this.colitr_incode.OptionsColumn.AllowEdit = false;
            this.colitr_incode.Visible = true;
            this.colitr_incode.VisibleIndex = 6;
            this.colitr_incode.Width = 106;
            // 
            // colitr_seq
            // 
            this.colitr_seq.Caption = "排序";
            this.colitr_seq.FieldName = "SortNo";
            this.colitr_seq.Name = "colitr_seq";
            this.colitr_seq.OptionsColumn.AllowEdit = false;
            // 
            // SelectDicInstrument
            // 
            this.colDisplay = "";
            this.colInCode = "";
            this.colPY = "";
            this.colValue = "";
            this.colWB = "";
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "PyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "ItrId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "ItrEname", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colExtend1", this.bsSource, "ItrName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colSeq", this.bsSource, "SortNo", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "WbCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "ItrId", true));
            this.Name = "SelectDicInstrument";
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
        private DevExpress.XtraGrid.Columns.GridColumn colitr_type;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_mid;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_name;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_no;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_wb;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_py;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_incode;
        //private lis.client.control.dsBasicTableAdapters.dict_instrmtTableAdapter dict_instrmtTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_seq;

    }
}
