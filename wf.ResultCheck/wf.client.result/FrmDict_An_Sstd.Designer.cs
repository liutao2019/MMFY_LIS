namespace dcl.client.result
{
    partial class FrmDict_An_Sstd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDict_An_Sstd));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.dsBasicDictBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colst_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_cname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colst_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.dsMic = new System.Windows.Forms.BindingSource(this.components);
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colanti_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_rzone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_izone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_szone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_hstd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_mstd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_lstd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSort = new DevExpress.XtraEditors.TextEdit();
            this.labelControl42 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasicDictBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.dsBasicDictBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(994, 895);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // dsBasicDictBindingSource
            // 
            this.dsBasicDictBindingSource.DataMember = "an_stype";
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colst_id,
            this.colst_cname,
            this.colst_py,
            this.colst_wb});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
            // 
            // colst_id
            // 
            this.colst_id.Caption = "编码";
            this.colst_id.FieldName = "AtypeId";
            this.colst_id.Name = "colst_id";
            this.colst_id.OptionsColumn.AllowEdit = false;
            this.colst_id.Visible = true;
            this.colst_id.VisibleIndex = 0;
            this.colst_id.Width = 52;
            // 
            // colst_cname
            // 
            this.colst_cname.Caption = "药敏卡名称";
            this.colst_cname.FieldName = "AtypeName";
            this.colst_cname.Name = "colst_cname";
            this.colst_cname.OptionsColumn.AllowEdit = false;
            this.colst_cname.Visible = true;
            this.colst_cname.VisibleIndex = 1;
            this.colst_cname.Width = 222;
            // 
            // colst_py
            // 
            this.colst_py.Caption = "拼音码";
            this.colst_py.FieldName = "APyCode";
            this.colst_py.Name = "colst_py";
            this.colst_py.OptionsColumn.AllowEdit = false;
            this.colst_py.Visible = true;
            this.colst_py.VisibleIndex = 2;
            // 
            // colst_wb
            // 
            this.colst_wb.Caption = "五笔码";
            this.colst_wb.FieldName = "AWbCode";
            this.colst_wb.Name = "colst_wb";
            this.colst_wb.OptionsColumn.AllowEdit = false;
            this.colst_wb.Visible = true;
            this.colst_wb.VisibleIndex = 3;
            this.colst_wb.Width = 95;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1006, 952);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gridControl1);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(994, 895);
            this.xtraTabPage1.Text = "药敏卡";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.panel2);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(994, 895);
            this.xtraTabPage2.Text = "抗生素";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridControl2);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(994, 895);
            this.panel2.TabIndex = 7;
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.dsMic;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControl2.Location = new System.Drawing.Point(0, 87);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(994, 808);
            this.gridControl2.TabIndex = 4;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // dsMic
            // 
            this.dsMic.DataSource = typeof(dcl.entity.EntityDicMicAntidetail);
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colanti_id,
            this.gridColumn1,
            this.gridColumn2,
            this.colss_rzone,
            this.colss_izone,
            this.colss_szone,
            this.colss_hstd,
            this.colss_mstd,
            this.colss_lstd});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ColumnAutoWidth = false;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.DoubleClick += new System.EventHandler(this.gridView2_DoubleClick);
            // 
            // colanti_id
            // 
            this.colanti_id.Caption = "药敏卡名称";
            this.colanti_id.FieldName = "AtypeName";
            this.colanti_id.Name = "colanti_id";
            this.colanti_id.OptionsColumn.AllowEdit = false;
            this.colanti_id.Visible = true;
            this.colanti_id.VisibleIndex = 0;
            this.colanti_id.Width = 222;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "名称";
            this.gridColumn1.FieldName = "AntCname";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 192;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "简称";
            this.gridColumn2.FieldName = "AnsAntiCode";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 72;
            // 
            // colss_rzone
            // 
            this.colss_rzone.Caption = "Zone.R";
            this.colss_rzone.FieldName = "AnsZoneDurgfast";
            this.colss_rzone.Name = "colss_rzone";
            this.colss_rzone.OptionsColumn.AllowEdit = false;
            this.colss_rzone.Visible = true;
            this.colss_rzone.VisibleIndex = 3;
            this.colss_rzone.Width = 72;
            // 
            // colss_izone
            // 
            this.colss_izone.Caption = "Zone.I";
            this.colss_izone.FieldName = "AnsZoneIntermed";
            this.colss_izone.Name = "colss_izone";
            this.colss_izone.OptionsColumn.AllowEdit = false;
            this.colss_izone.Visible = true;
            this.colss_izone.VisibleIndex = 4;
            this.colss_izone.Width = 63;
            // 
            // colss_szone
            // 
            this.colss_szone.Caption = "Zone.S";
            this.colss_szone.FieldName = "AnsZoneSensitive";
            this.colss_szone.Name = "colss_szone";
            this.colss_szone.OptionsColumn.AllowEdit = false;
            this.colss_szone.Visible = true;
            this.colss_szone.VisibleIndex = 5;
            this.colss_szone.Width = 77;
            // 
            // colss_hstd
            // 
            this.colss_hstd.Caption = "敏感MIC";
            this.colss_hstd.FieldName = "AnsStdUpperLimit";
            this.colss_hstd.Name = "colss_hstd";
            this.colss_hstd.OptionsColumn.AllowEdit = false;
            this.colss_hstd.Visible = true;
            this.colss_hstd.VisibleIndex = 6;
            this.colss_hstd.Width = 67;
            // 
            // colss_mstd
            // 
            this.colss_mstd.Caption = "中介MIC";
            this.colss_mstd.FieldName = "AnsStdMiddleLimit";
            this.colss_mstd.Name = "colss_mstd";
            this.colss_mstd.OptionsColumn.AllowEdit = false;
            this.colss_mstd.Visible = true;
            this.colss_mstd.VisibleIndex = 7;
            this.colss_mstd.Width = 77;
            // 
            // colss_lstd
            // 
            this.colss_lstd.Caption = "耐药MIC";
            this.colss_lstd.FieldName = "AnsStdLowerLimit";
            this.colss_lstd.Name = "colss_lstd";
            this.colss_lstd.OptionsColumn.AllowEdit = false;
            this.colss_lstd.Visible = true;
            this.colss_lstd.VisibleIndex = 8;
            this.colss_lstd.Width = 103;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSort);
            this.panel1.Controls.Add(this.labelControl42);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(994, 87);
            this.panel1.TabIndex = 6;
            // 
            // txtSort
            // 
            this.txtSort.EnterMoveNextControl = true;
            this.txtSort.Location = new System.Drawing.Point(141, 19);
            this.txtSort.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(271, 36);
            this.txtSort.TabIndex = 67;
            this.txtSort.EditValueChanged += new System.EventHandler(this.txtSort_EditValueChanged);
            // 
            // labelControl42
            // 
            this.labelControl42.Location = new System.Drawing.Point(24, 27);
            this.labelControl42.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.labelControl42.Name = "labelControl42";
            this.labelControl42.Size = new System.Drawing.Size(96, 29);
            this.labelControl42.TabIndex = 68;
            this.labelControl42.Text = "数据检索";
            // 
            // FrmDict_An_Sstd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 952);
            this.Controls.Add(this.xtraTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmDict_An_Sstd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "药敏卡（双击选择药敏卡）";
            this.Load += new System.EventHandler(this.FrmDict_An_Sstd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasicDictBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsMic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource dsBasicDictBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colst_id;
        private DevExpress.XtraGrid.Columns.GridColumn colst_cname;
        private DevExpress.XtraGrid.Columns.GridColumn colst_py;
        private DevExpress.XtraGrid.Columns.GridColumn colst_wb;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.TextEdit txtSort;
        private DevExpress.XtraEditors.LabelControl labelControl42;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn colanti_id;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn colss_rzone;
        private DevExpress.XtraGrid.Columns.GridColumn colss_izone;
        private DevExpress.XtraGrid.Columns.GridColumn colss_szone;
        private DevExpress.XtraGrid.Columns.GridColumn colss_hstd;
        private DevExpress.XtraGrid.Columns.GridColumn colss_mstd;
        private DevExpress.XtraGrid.Columns.GridColumn colss_lstd;
        private System.Windows.Forms.BindingSource dsMic;
        private System.Windows.Forms.Panel panel2;
    }
}