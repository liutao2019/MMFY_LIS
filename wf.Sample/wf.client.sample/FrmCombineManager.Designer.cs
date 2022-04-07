namespace dcl.client.sample
{
    partial class FrmCombineManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCombineManager));
            this.gridControlCombineList = new DevExpress.XtraGrid.GridControl();
            this.bsType = new System.Windows.Forms.BindingSource();
            this.gridViewCombineList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridControlPatCombine = new DevExpress.XtraGrid.GridControl();
            this.gridViewCurrent = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCombineList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCombineList)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPatCombine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCurrent)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlCombineList
            // 
            this.gridControlCombineList.DataSource = this.bsType;
            this.gridControlCombineList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlCombineList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControlCombineList.Location = new System.Drawing.Point(0, 36);
            this.gridControlCombineList.MainView = this.gridViewCombineList;
            this.gridControlCombineList.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControlCombineList.Name = "gridControlCombineList";
            this.gridControlCombineList.Size = new System.Drawing.Size(1047, 921);
            this.gridControlCombineList.TabIndex = 1;
            this.gridControlCombineList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewCombineList});
            this.gridControlCombineList.DoubleClick += new System.EventHandler(this.gridControlCombineList_DoubleClick);
            this.gridControlCombineList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlCombineList_KeyDown);
            this.gridControlCombineList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridControlCombineList_KeyPress);
            // 
            // bsType
            // 
            this.bsType.AllowNew = false;
            this.bsType.DataSource = typeof(dcl.entity.EntityDicCombine);
            // 
            // gridViewCombineList
            // 
            this.gridViewCombineList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn7,
            this.colcom_id,
            this.colcom_name,
            this.colcom_type,
            this.colcom_wb,
            this.colcom_py,
            this.colcom_incode,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridViewCombineList.GridControl = this.gridControlCombineList;
            this.gridViewCombineList.Name = "gridViewCombineList";
            this.gridViewCombineList.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridViewCombineList.OptionsView.ColumnAutoWidth = false;
            this.gridViewCombineList.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "急";
            this.gridColumn7.FieldName = "Urgent";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn7.Width = 35;
            // 
            // colcom_id
            // 
            this.colcom_id.Caption = "编码";
            this.colcom_id.FieldName = "ComId";
            this.colcom_id.Name = "colcom_id";
            this.colcom_id.OptionsColumn.AllowEdit = false;
            this.colcom_id.OptionsColumn.AllowFocus = false;
            this.colcom_id.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colcom_id.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colcom_id.OptionsFilter.AllowAutoFilter = false;
            this.colcom_id.OptionsFilter.AllowFilter = false;
            this.colcom_id.Visible = true;
            this.colcom_id.VisibleIndex = 0;
            this.colcom_id.Width = 65;
            // 
            // colcom_name
            // 
            this.colcom_name.Caption = "组合名称";
            this.colcom_name.FieldName = "ComName";
            this.colcom_name.Name = "colcom_name";
            this.colcom_name.OptionsColumn.AllowEdit = false;
            this.colcom_name.OptionsColumn.AllowFocus = false;
            this.colcom_name.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colcom_name.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colcom_name.OptionsFilter.AllowAutoFilter = false;
            this.colcom_name.OptionsFilter.AllowFilter = false;
            this.colcom_name.Visible = true;
            this.colcom_name.VisibleIndex = 1;
            this.colcom_name.Width = 100;
            // 
            // colcom_type
            // 
            this.colcom_type.Caption = "专业组别";
            this.colcom_type.FieldName = "PTypeName";
            this.colcom_type.Name = "colcom_type";
            this.colcom_type.OptionsColumn.AllowEdit = false;
            this.colcom_type.OptionsColumn.AllowFocus = false;
            this.colcom_type.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colcom_type.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colcom_type.OptionsFilter.AllowAutoFilter = false;
            this.colcom_type.OptionsFilter.AllowFilter = false;
            this.colcom_type.Visible = true;
            this.colcom_type.VisibleIndex = 2;
            this.colcom_type.Width = 90;
            // 
            // colcom_wb
            // 
            this.colcom_wb.Caption = "五笔";
            this.colcom_wb.FieldName = "ComWbCode";
            this.colcom_wb.Name = "colcom_wb";
            this.colcom_wb.OptionsColumn.AllowEdit = false;
            this.colcom_wb.OptionsColumn.AllowFocus = false;
            this.colcom_wb.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colcom_wb.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colcom_wb.OptionsFilter.AllowAutoFilter = false;
            this.colcom_wb.OptionsFilter.AllowFilter = false;
            this.colcom_wb.Width = 68;
            // 
            // colcom_py
            // 
            this.colcom_py.Caption = "拼音";
            this.colcom_py.FieldName = "ComPyCode";
            this.colcom_py.Name = "colcom_py";
            this.colcom_py.OptionsColumn.AllowEdit = false;
            this.colcom_py.OptionsColumn.AllowFocus = false;
            this.colcom_py.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colcom_py.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colcom_py.OptionsFilter.AllowAutoFilter = false;
            this.colcom_py.OptionsFilter.AllowFilter = false;
            this.colcom_py.Width = 67;
            // 
            // colcom_incode
            // 
            this.colcom_incode.Caption = "输入码";
            this.colcom_incode.FieldName = "ComCCode";
            this.colcom_incode.Name = "colcom_incode";
            this.colcom_incode.OptionsColumn.AllowEdit = false;
            this.colcom_incode.OptionsColumn.AllowFocus = false;
            this.colcom_incode.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colcom_incode.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colcom_incode.OptionsFilter.AllowAutoFilter = false;
            this.colcom_incode.OptionsFilter.AllowFilter = false;
            this.colcom_incode.Width = 71;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "物理组别";
            this.gridColumn1.FieldName = "CTypeName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            this.gridColumn1.Width = 100;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "价格";
            this.gridColumn2.FieldName = "ComPrice";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 60;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "成本";
            this.gridColumn3.FieldName = "ComCost";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 5;
            this.gridColumn3.Width = 60;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "备注";
            this.gridColumn4.FieldName = "ComRemark";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1047, 36);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1430, 957);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gridControlCombineList);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(383, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1047, 957);
            this.panel3.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridControlPatCombine);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(383, 957);
            this.panel2.TabIndex = 2;
            // 
            // gridControlPatCombine
            // 
            this.gridControlPatCombine.DataSource = this.bsType;
            this.gridControlPatCombine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlPatCombine.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControlPatCombine.Location = new System.Drawing.Point(0, 0);
            this.gridControlPatCombine.MainView = this.gridViewCurrent;
            this.gridControlPatCombine.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControlPatCombine.Name = "gridControlPatCombine";
            this.gridControlPatCombine.Size = new System.Drawing.Size(383, 957);
            this.gridControlPatCombine.TabIndex = 2;
            this.gridControlPatCombine.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewCurrent});
            this.gridControlPatCombine.DoubleClick += new System.EventHandler(this.gridControlPatCombine_DoubleClick);
            this.gridControlPatCombine.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlPatCombine_KeyDown);
            this.gridControlPatCombine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridControlCombineList_KeyPress);
            // 
            // gridViewCurrent
            // 
            this.gridViewCurrent.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn5});
            this.gridViewCurrent.GridControl = this.gridControlPatCombine;
            this.gridViewCurrent.Name = "gridViewCurrent";
            this.gridViewCurrent.OptionsBehavior.Editable = false;
            this.gridViewCurrent.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridViewCurrent.OptionsLayout.Columns.AddNewColumns = false;
            this.gridViewCurrent.OptionsView.ColumnAutoWidth = false;
            this.gridViewCurrent.OptionsView.ShowGroupPanel = false;
            this.gridViewCurrent.OptionsView.ShowIndicator = false;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "当前组合";
            this.gridColumn6.FieldName = "ComName";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn6.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn6.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn6.OptionsFilter.AllowFilter = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            this.gridColumn6.Width = 146;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "组合ID";
            this.gridColumn5.FieldName = "ComId";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // FrmCombineManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1430, 957);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCombineManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "组合选择";
            this.Deactivate += new System.EventHandler(this.FrmCombineManager_Deactivate);
            this.Load += new System.EventHandler(this.FrmCombineSelect_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmCombineSelect_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCombineList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCombineList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlPatCombine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCurrent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlCombineList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewCombineList;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_id;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_name;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_type;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_wb;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_py;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_incode;
        private System.Windows.Forms.BindingSource bsType;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gridControlPatCombine;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewCurrent;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
    }
}