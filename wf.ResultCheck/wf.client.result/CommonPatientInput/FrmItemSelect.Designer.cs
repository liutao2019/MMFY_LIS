namespace dcl.client.result.CommonPatientInput
{
    partial class FrmItemSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmItemSelect));
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.bsItem = new System.Windows.Forms.BindingSource();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colcom_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsItem;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControl2.Location = new System.Drawing.Point(0, 36);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(913, 631);
            this.gridControl2.TabIndex = 4;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            this.gridControl2.EditorKeyDown += new System.Windows.Forms.KeyEventHandler(this.gridControl2_EditorKeyDown);
            this.gridControl2.DoubleClick += new System.EventHandler(this.gridControl2_DoubleClick);
            // 
            // bsItem
            // 
            this.bsItem.DataSource = typeof(dcl.entity.EntityDicItmItem);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colcom_id,
            this.colcom_name,
            this.colcom_type,
            this.colcom_wb,
            this.colcom_py,
            this.colcom_incode});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridView2_KeyDown);
            // 
            // colcom_id
            // 
            this.colcom_id.Caption = "编码";
            this.colcom_id.FieldName = "ItmId";
            this.colcom_id.Name = "colcom_id";
            this.colcom_id.Visible = true;
            this.colcom_id.VisibleIndex = 0;
            this.colcom_id.Width = 58;
            // 
            // colcom_name
            // 
            this.colcom_name.Caption = "项目代码";
            this.colcom_name.FieldName = "ItmEcode";
            this.colcom_name.Name = "colcom_name";
            this.colcom_name.Visible = true;
            this.colcom_name.VisibleIndex = 1;
            this.colcom_name.Width = 130;
            // 
            // colcom_type
            // 
            this.colcom_type.Caption = "项目名称";
            this.colcom_type.FieldName = "ItmName";
            this.colcom_type.Name = "colcom_type";
            this.colcom_type.Visible = true;
            this.colcom_type.VisibleIndex = 2;
            this.colcom_type.Width = 193;
            // 
            // colcom_wb
            // 
            this.colcom_wb.Caption = "五笔";
            this.colcom_wb.FieldName = "ItmWbCode";
            this.colcom_wb.Name = "colcom_wb";
            this.colcom_wb.Width = 68;
            // 
            // colcom_py
            // 
            this.colcom_py.Caption = "拼音";
            this.colcom_py.FieldName = "ItmPyCode";
            this.colcom_py.Name = "colcom_py";
            this.colcom_py.Width = 67;
            // 
            // colcom_incode
            // 
            this.colcom_incode.Caption = "输入码";
            this.colcom_incode.FieldName = "ItmHisCode";
            this.colcom_incode.Name = "colcom_incode";
            this.colcom_incode.Width = 71;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(913, 36);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // FrmItemSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 667);
            this.Controls.Add(this.gridControl2);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmItemSelect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "选择项目";
            this.Load += new System.EventHandler(this.FrmItemSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_id;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_name;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_type;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_wb;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_py;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_incode;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.BindingSource bsItem;
    }
}