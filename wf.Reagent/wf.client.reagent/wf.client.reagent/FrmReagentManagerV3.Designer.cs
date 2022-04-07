namespace wf.client.reagent
{
    partial class FrmReagentManagerV3
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReagentManagerV3));
            this.gridControlReagentList = new DevExpress.XtraGrid.GridControl();
            this.bsReagent = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewReagentList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.coldrea_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrea_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrea_group = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridControlReagent = new DevExpress.XtraGrid.GridControl();
            this.gridViewReagent = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRunit_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDrea_package = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlReagentList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsReagent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReagentList)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlReagent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReagent)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlReagentList
            // 
            this.gridControlReagentList.DataSource = this.bsReagent;
            this.gridControlReagentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlReagentList.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControlReagentList.Location = new System.Drawing.Point(0, 22);
            this.gridControlReagentList.MainView = this.gridViewReagentList;
            this.gridControlReagentList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControlReagentList.Name = "gridControlReagentList";
            this.gridControlReagentList.Size = new System.Drawing.Size(665, 440);
            this.gridControlReagentList.TabIndex = 1;
            this.gridControlReagentList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewReagentList});
            this.gridControlReagentList.DoubleClick += new System.EventHandler(this.gridControlReagentList_DoubleClick);
            this.gridControlReagentList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlReagentList_KeyDown);
            this.gridControlReagentList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridControlReagentList_KeyPress);
            // 
            // bsReagent
            // 
            this.bsReagent.DataSource = typeof(dcl.entity.EntityReaSetting);
            // 
            // gridViewReagentList
            // 
            this.gridViewReagentList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.coldrea_id,
            this.colrea_name,
            this.colrea_group,
            this.col_wb,
            this.col_py,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn7,
            this.colRunit_name,
            this.colDrea_package});
            this.gridViewReagentList.GridControl = this.gridControlReagentList;
            this.gridViewReagentList.Name = "gridViewReagentList";
            this.gridViewReagentList.OptionsBehavior.Editable = false;
            this.gridViewReagentList.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridViewReagentList.OptionsView.ColumnAutoWidth = false;
            this.gridViewReagentList.OptionsView.ShowGroupPanel = false;
            this.gridViewReagentList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.gridViewReagentList_MouseUp);
            // 
            // coldrea_id
            // 
            this.coldrea_id.Caption = "编码";
            this.coldrea_id.FieldName = "Drea_id";
            this.coldrea_id.Name = "coldrea_id";
            this.coldrea_id.OptionsColumn.AllowEdit = false;
            this.coldrea_id.OptionsColumn.AllowFocus = false;
            this.coldrea_id.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.coldrea_id.OptionsFilter.AllowAutoFilter = false;
            this.coldrea_id.OptionsFilter.AllowFilter = false;
            this.coldrea_id.Visible = true;
            this.coldrea_id.VisibleIndex = 0;
            this.coldrea_id.Width = 65;
            // 
            // colrea_name
            // 
            this.colrea_name.Caption = "试剂名称";
            this.colrea_name.FieldName = "Drea_name";
            this.colrea_name.Name = "colrea_name";
            this.colrea_name.OptionsColumn.AllowEdit = false;
            this.colrea_name.OptionsColumn.AllowFocus = false;
            this.colrea_name.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colrea_name.OptionsFilter.AllowAutoFilter = false;
            this.colrea_name.OptionsFilter.AllowFilter = false;
            this.colrea_name.Visible = true;
            this.colrea_name.VisibleIndex = 1;
            this.colrea_name.Width = 132;
            // 
            // colrea_group
            // 
            this.colrea_group.Caption = "试剂组别";
            this.colrea_group.FieldName = "Rea_group";
            this.colrea_group.Name = "colrea_group";
            this.colrea_group.OptionsColumn.AllowEdit = false;
            this.colrea_group.OptionsColumn.AllowFocus = false;
            this.colrea_group.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colrea_group.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colrea_group.OptionsFilter.AllowAutoFilter = false;
            this.colrea_group.OptionsFilter.AllowFilter = false;
            this.colrea_group.Visible = true;
            this.colrea_group.VisibleIndex = 4;
            this.colrea_group.Width = 90;
            // 
            // col_wb
            // 
            this.col_wb.Caption = "五笔";
            this.col_wb.FieldName = "wb_code";
            this.col_wb.Name = "col_wb";
            this.col_wb.OptionsColumn.AllowEdit = false;
            this.col_wb.OptionsColumn.AllowFocus = false;
            this.col_wb.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_wb.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.col_wb.OptionsFilter.AllowAutoFilter = false;
            this.col_wb.OptionsFilter.AllowFilter = false;
            this.col_wb.Width = 68;
            // 
            // col_py
            // 
            this.col_py.Caption = "拼音";
            this.col_py.FieldName = "py_code";
            this.col_py.Name = "col_py";
            this.col_py.OptionsColumn.AllowEdit = false;
            this.col_py.OptionsColumn.AllowFocus = false;
            this.col_py.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.col_py.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.col_py.OptionsFilter.AllowAutoFilter = false;
            this.col_py.OptionsFilter.AllowFilter = false;
            this.col_py.Width = 67;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "生产厂商";
            this.gridColumn2.FieldName = "Rpdt_name";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 6;
            this.gridColumn2.Width = 60;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "方法学";
            this.gridColumn3.FieldName = "Drea_method";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 8;
            this.gridColumn3.Width = 60;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "备注";
            this.gridColumn4.FieldName = "Drea_remark";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 9;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "库存";
            this.gridColumn1.FieldName = "Rri_Count";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 5;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "供货商";
            this.gridColumn7.FieldName = "Rsupplier_name";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(665, 22);
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
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(871, 462);
            this.panel1.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gridControlReagentList);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(206, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(665, 462);
            this.panel3.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridControlReagent);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(206, 462);
            this.panel2.TabIndex = 2;
            // 
            // gridControlReagent
            // 
            this.gridControlReagent.DataSource = this.bsReagent;
            this.gridControlReagent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlReagent.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControlReagent.Location = new System.Drawing.Point(0, 0);
            this.gridControlReagent.MainView = this.gridViewReagent;
            this.gridControlReagent.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.gridControlReagent.Name = "gridControlReagent";
            this.gridControlReagent.Size = new System.Drawing.Size(206, 462);
            this.gridControlReagent.TabIndex = 2;
            this.gridControlReagent.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewReagent});
            this.gridControlReagent.DoubleClick += new System.EventHandler(this.gridControlPatReagent_DoubleClick);
            this.gridControlReagent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControlPatReagent_KeyDown);
            this.gridControlReagent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridControlReagentList_KeyPress);
            // 
            // gridViewReagent
            // 
            this.gridViewReagent.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn5});
            this.gridViewReagent.GridControl = this.gridControlReagent;
            this.gridViewReagent.Name = "gridViewReagent";
            this.gridViewReagent.OptionsBehavior.Editable = false;
            this.gridViewReagent.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridViewReagent.OptionsLayout.Columns.AddNewColumns = false;
            this.gridViewReagent.OptionsView.ShowGroupPanel = false;
            this.gridViewReagent.OptionsView.ShowIndicator = false;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "当前组合";
            this.gridColumn6.FieldName = "Drea_name";
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
            this.gridColumn5.FieldName = "Drea_id";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // colRunit_name
            // 
            this.colRunit_name.Caption = "单位";
            this.colRunit_name.FieldName = "Runit_name";
            this.colRunit_name.Name = "colRunit_name";
            this.colRunit_name.Visible = true;
            this.colRunit_name.VisibleIndex = 3;
            // 
            // colDrea_package
            // 
            this.colDrea_package.Caption = "规格";
            this.colDrea_package.FieldName = "Drea_package";
            this.colDrea_package.Name = "colDrea_package";
            this.colDrea_package.Visible = true;
            this.colDrea_package.VisibleIndex = 2;
            // 
            // FrmReagentManagerV3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 462);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmReagentManagerV3";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.FrmReagentManagerV3_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmReagentManagerV3_FormClosing);
            this.Load += new System.EventHandler(this.FrmReagentSelect_Load);
            this.VisibleChanged += new System.EventHandler(this.FrmReagentManagerV3_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmReagentSelect_KeyDown);
            this.Leave += new System.EventHandler(this.FrmReagentManagerV3_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlReagentList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsReagent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReagentList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlReagent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewReagent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlReagentList;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewReagentList;
        private DevExpress.XtraGrid.Columns.GridColumn coldrea_id;
        private DevExpress.XtraGrid.Columns.GridColumn colrea_name;
        private DevExpress.XtraGrid.Columns.GridColumn colrea_group;
        private DevExpress.XtraGrid.Columns.GridColumn col_wb;
        private DevExpress.XtraGrid.Columns.GridColumn col_py;
        private System.Windows.Forms.BindingSource bsReagent;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraGrid.GridControl gridControlReagent;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewReagent;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn colRunit_name;
        private DevExpress.XtraGrid.Columns.GridColumn colDrea_package;
    }
}