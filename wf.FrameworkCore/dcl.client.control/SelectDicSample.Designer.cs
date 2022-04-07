namespace dcl.client.control
{
    partial class SelectDicSample
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colsam_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsam_custom_type = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            this.popupContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Size = new System.Drawing.Size(215, 20);
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Controls.Add(this.gridControl1);
            // 
            // bsSource
            // 
            this.bsSource.DataSource = typeof(dcl.entity.EntityDicSample);
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(432, 264);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colsam_id,
            this.colsam_name,
            this.colsam_code,
            this.colsam_incode,
            this.colsam_py,
            this.colsam_wb,
            this.colsam_custom_type});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // colsam_id
            // 
            this.colsam_id.Caption = "编码";
            this.colsam_id.FieldName = "SamId";
            this.colsam_id.Name = "colsam_id";
            this.colsam_id.OptionsColumn.AllowEdit = false;
            this.colsam_id.OptionsColumn.AllowFocus = false;
            this.colsam_id.OptionsColumn.ReadOnly = true;
            this.colsam_id.Visible = true;
            this.colsam_id.VisibleIndex = 0;
            // 
            // colsam_name
            // 
            this.colsam_name.Caption = "标本名称";
            this.colsam_name.FieldName = "SamName";
            this.colsam_name.Name = "colsam_name";
            this.colsam_name.OptionsColumn.AllowEdit = false;
            this.colsam_name.OptionsColumn.AllowFocus = false;
            this.colsam_name.OptionsColumn.ReadOnly = true;
            this.colsam_name.Visible = true;
            this.colsam_name.VisibleIndex = 1;
            // 
            // colsam_code
            // 
            this.colsam_code.Caption = "代码";
            this.colsam_code.FieldName = "SamCode";
            this.colsam_code.Name = "colsam_code";
            this.colsam_code.OptionsColumn.AllowEdit = false;
            this.colsam_code.OptionsColumn.AllowFocus = false;
            this.colsam_code.OptionsColumn.ReadOnly = true;
            this.colsam_code.Visible = true;
            this.colsam_code.VisibleIndex = 2;
            // 
            // colsam_incode
            // 
            this.colsam_incode.Caption = "输入码";
            this.colsam_incode.FieldName = "SamCCode";
            this.colsam_incode.Name = "colsam_incode";
            this.colsam_incode.OptionsColumn.AllowEdit = false;
            this.colsam_incode.OptionsColumn.AllowFocus = false;
            this.colsam_incode.OptionsColumn.ReadOnly = true;
            this.colsam_incode.Visible = true;
            this.colsam_incode.VisibleIndex = 3;
            // 
            // colsam_py
            // 
            this.colsam_py.Caption = "拼音";
            this.colsam_py.FieldName = "SamPyCode";
            this.colsam_py.Name = "colsam_py";
            this.colsam_py.OptionsColumn.AllowEdit = false;
            this.colsam_py.OptionsColumn.AllowFocus = false;
            this.colsam_py.OptionsColumn.ReadOnly = true;
            this.colsam_py.Visible = true;
            this.colsam_py.VisibleIndex = 4;
            // 
            // colsam_wb
            // 
            this.colsam_wb.Caption = "五笔";
            this.colsam_wb.FieldName = "SamWbCode";
            this.colsam_wb.Name = "colsam_wb";
            this.colsam_wb.OptionsColumn.AllowEdit = false;
            this.colsam_wb.OptionsColumn.AllowFocus = false;
            this.colsam_wb.OptionsColumn.ReadOnly = true;
            this.colsam_wb.Visible = true;
            this.colsam_wb.VisibleIndex = 5;
            // 
            // colsam_custom_type
            // 
            this.colsam_custom_type.Caption = "分组";
            this.colsam_custom_type.FieldName = "SamCustomType";
            this.colsam_custom_type.Name = "colsam_custom_type";
            this.colsam_custom_type.OptionsColumn.AllowEdit = false;
            this.colsam_custom_type.OptionsColumn.AllowFocus = false;
            this.colsam_custom_type.OptionsColumn.ReadOnly = true;
            // 
            // SelectDicSample
            // 
            this.DataBindings.Add(new System.Windows.Forms.Binding("colPY", this.bsSource, "SamPyCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colInCode", this.bsSource, "SamCCode", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colDisplay", this.bsSource, "SamName", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colValue", this.bsSource, "SamId", true));
            this.DataBindings.Add(new System.Windows.Forms.Binding("colWB", this.bsSource, "SamId", true));
            this.Name = "SelectDicSample";
            this.Size = new System.Drawing.Size(215, 21);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            this.popupContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsSource;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colsam_id;
        private DevExpress.XtraGrid.Columns.GridColumn colsam_name;
        private DevExpress.XtraGrid.Columns.GridColumn colsam_code;
        private DevExpress.XtraGrid.Columns.GridColumn colsam_incode;
        private DevExpress.XtraGrid.Columns.GridColumn colsam_py;
        private DevExpress.XtraGrid.Columns.GridColumn colsam_wb;
        private DevExpress.XtraGrid.Columns.GridColumn colsam_custom_type;

    }
}
