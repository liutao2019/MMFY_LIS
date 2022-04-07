namespace dcl.client.result
{
    partial class FrmDict_Antibio1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDict_Antibio1));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.dsBasicDictBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colanti_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_rzone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_izone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_szone = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_hstd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_mstd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colss_lstd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSort = new DevExpress.XtraEditors.TextEdit();
            this.labelControl42 = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasicDictBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.dsBasicDictBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1129, 934);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // dsBasicDictBindingSource
            // 
            this.dsBasicDictBindingSource.DataSource = typeof(dcl.entity.EntityDicMicAntidetail);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colanti_id,
            this.gridColumn1,
            this.gridColumn2,
            this.colss_rzone,
            this.colss_izone,
            this.colss_szone,
            this.colss_hstd,
            this.colss_mstd,
            this.colss_lstd});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.DoubleClick += new System.EventHandler(this.gridView1_DoubleClick);
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
            // simpleButton1
            // 
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.simpleButton1.Location = new System.Drawing.Point(399, 17);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(162, 56);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "关闭";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 1021);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1129, 85);
            this.panelControl1.TabIndex = 2;
            // 
            // simpleButton2
            // 
            this.simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.simpleButton2.Location = new System.Drawing.Point(93, 17);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(162, 56);
            this.simpleButton2.TabIndex = 2;
            this.simpleButton2.Text = "全选";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSort);
            this.panel1.Controls.Add(this.labelControl42);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1129, 87);
            this.panel1.TabIndex = 3;
            // 
            // txtSort
            // 
            this.txtSort.EnterMoveNextControl = true;
            this.txtSort.Location = new System.Drawing.Point(141, 19);
            this.txtSort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(271, 36);
            this.txtSort.TabIndex = 67;
            this.txtSort.EditValueChanged += new System.EventHandler(this.txtSort_EditValueChanged);
            // 
            // labelControl42
            // 
            this.labelControl42.Location = new System.Drawing.Point(24, 27);
            this.labelControl42.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl42.Name = "labelControl42";
            this.labelControl42.Size = new System.Drawing.Size(96, 29);
            this.labelControl42.TabIndex = 68;
            this.labelControl42.Text = "数据检索";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 87);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1129, 934);
            this.panel2.TabIndex = 4;
            // 
            // FrmDict_Antibio1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 1106);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "FrmDict_Antibio1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "抗生素（双击添加抗生素）";
            this.Load += new System.EventHandler(this.FrmDict_Antibio_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsBasicDictBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource dsBasicDictBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colanti_id;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn colss_rzone;
        private DevExpress.XtraGrid.Columns.GridColumn colss_izone;
        private DevExpress.XtraGrid.Columns.GridColumn colss_szone;
        private DevExpress.XtraGrid.Columns.GridColumn colss_hstd;
        private DevExpress.XtraGrid.Columns.GridColumn colss_mstd;
        private DevExpress.XtraGrid.Columns.GridColumn colss_lstd;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.TextEdit txtSort;
        private DevExpress.XtraEditors.LabelControl labelControl42;
        private System.Windows.Forms.Panel panel2;
    }
}