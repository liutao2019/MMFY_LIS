namespace dcl.client.qc
{
    partial class FrmControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmControl));
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colqcm_mid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lue_instrmt = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colqcm_itm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colqcm_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colqcm_meas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colqcm_c_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colqcm_ns = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colqcm_reson = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.colqcm_fun = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.qcm_rem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lue_instrmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 77);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1320, 928);
            this.panelControl2.TabIndex = 4;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.lue_instrmt});
            this.gridControl1.Size = new System.Drawing.Size(1316, 924);
            this.gridControl1.TabIndex = 3;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 23;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colqcm_mid,
            this.colqcm_itm,
            this.colqcm_date,
            this.colqcm_meas,
            this.colqcm_c_no,
            this.colqcm_ns,
            this.colqcm_reson,
            this.colqcm_fun,
            this.qcm_rem});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colqcm_mid
            // 
            this.colqcm_mid.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_mid.AppearanceCell.Options.UseFont = true;
            this.colqcm_mid.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_mid.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_mid.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_mid.AppearanceHeader.Options.UseFont = true;
            this.colqcm_mid.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_mid.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_mid.Caption = "测定仪器";
            this.colqcm_mid.ColumnEdit = this.lue_instrmt;
            this.colqcm_mid.FieldName = "QresItrId";
            this.colqcm_mid.Name = "colqcm_mid";
            this.colqcm_mid.OptionsColumn.AllowEdit = false;
            this.colqcm_mid.Visible = true;
            this.colqcm_mid.VisibleIndex = 0;
            // 
            // lue_instrmt
            // 
            this.lue_instrmt.AutoHeight = false;
            this.lue_instrmt.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lue_instrmt.DisplayMember = "ItrEname";
            this.lue_instrmt.Name = "lue_instrmt";
            this.lue_instrmt.ValueMember = "ItrId";
            // 
            // colqcm_itm
            // 
            this.colqcm_itm.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_itm.AppearanceCell.Options.UseFont = true;
            this.colqcm_itm.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_itm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_itm.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_itm.AppearanceHeader.Options.UseFont = true;
            this.colqcm_itm.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_itm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_itm.Caption = "测定项目";
            this.colqcm_itm.FieldName = "ItmEcode";
            this.colqcm_itm.Name = "colqcm_itm";
            this.colqcm_itm.OptionsColumn.AllowEdit = false;
            this.colqcm_itm.Visible = true;
            this.colqcm_itm.VisibleIndex = 1;
            // 
            // colqcm_date
            // 
            this.colqcm_date.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_date.AppearanceCell.Options.UseFont = true;
            this.colqcm_date.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_date.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_date.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_date.AppearanceHeader.Options.UseFont = true;
            this.colqcm_date.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_date.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_date.Caption = "测定时间";
            this.colqcm_date.FieldName = "QresDate";
            this.colqcm_date.Name = "colqcm_date";
            this.colqcm_date.OptionsColumn.AllowEdit = false;
            this.colqcm_date.Visible = true;
            this.colqcm_date.VisibleIndex = 2;
            // 
            // colqcm_meas
            // 
            this.colqcm_meas.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_meas.AppearanceCell.Options.UseFont = true;
            this.colqcm_meas.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_meas.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_meas.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_meas.AppearanceHeader.Options.UseFont = true;
            this.colqcm_meas.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_meas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_meas.Caption = "测定值";
            this.colqcm_meas.FieldName = "QresValue";
            this.colqcm_meas.Name = "colqcm_meas";
            this.colqcm_meas.OptionsColumn.AllowEdit = false;
            this.colqcm_meas.Visible = true;
            this.colqcm_meas.VisibleIndex = 3;
            // 
            // colqcm_c_no
            // 
            this.colqcm_c_no.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_c_no.AppearanceCell.Options.UseFont = true;
            this.colqcm_c_no.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_c_no.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_c_no.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_c_no.AppearanceHeader.Options.UseFont = true;
            this.colqcm_c_no.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_c_no.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_c_no.Caption = "测定水平";
            this.colqcm_c_no.FieldName = "QresLevel";
            this.colqcm_c_no.Name = "colqcm_c_no";
            this.colqcm_c_no.OptionsColumn.AllowEdit = false;
            this.colqcm_c_no.Visible = true;
            this.colqcm_c_no.VisibleIndex = 4;
            // 
            // colqcm_ns
            // 
            this.colqcm_ns.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_ns.AppearanceCell.Options.UseFont = true;
            this.colqcm_ns.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_ns.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_ns.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_ns.AppearanceHeader.Options.UseFont = true;
            this.colqcm_ns.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_ns.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_ns.Caption = "失控规则";
            this.colqcm_ns.FieldName = "QresRunawayRule";
            this.colqcm_ns.Name = "colqcm_ns";
            this.colqcm_ns.OptionsColumn.AllowEdit = false;
            this.colqcm_ns.Visible = true;
            this.colqcm_ns.VisibleIndex = 5;
            // 
            // colqcm_reson
            // 
            this.colqcm_reson.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_reson.AppearanceCell.Options.UseFont = true;
            this.colqcm_reson.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_reson.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_reson.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_reson.AppearanceHeader.Options.UseFont = true;
            this.colqcm_reson.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_reson.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_reson.Caption = "失控原因";
            this.colqcm_reson.ColumnEdit = this.repositoryItemComboBox1;
            this.colqcm_reson.FieldName = "QresReasons";
            this.colqcm_reson.Name = "colqcm_reson";
            this.colqcm_reson.Visible = true;
            this.colqcm_reson.VisibleIndex = 6;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // colqcm_fun
            // 
            this.colqcm_fun.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_fun.AppearanceCell.Options.UseFont = true;
            this.colqcm_fun.AppearanceCell.Options.UseTextOptions = true;
            this.colqcm_fun.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_fun.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcm_fun.AppearanceHeader.Options.UseFont = true;
            this.colqcm_fun.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcm_fun.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcm_fun.Caption = "解决措施";
            this.colqcm_fun.ColumnEdit = this.repositoryItemComboBox2;
            this.colqcm_fun.FieldName = "QresProcess";
            this.colqcm_fun.Name = "colqcm_fun";
            this.colqcm_fun.Visible = true;
            this.colqcm_fun.VisibleIndex = 7;
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // qcm_rem
            // 
            this.qcm_rem.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_rem.AppearanceCell.Options.UseFont = true;
            this.qcm_rem.AppearanceCell.Options.UseTextOptions = true;
            this.qcm_rem.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_rem.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.qcm_rem.AppearanceHeader.Options.UseFont = true;
            this.qcm_rem.AppearanceHeader.Options.UseTextOptions = true;
            this.qcm_rem.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.qcm_rem.Caption = "备注";
            this.qcm_rem.FieldName = "QresRemark";
            this.qcm_rem.Name = "qcm_rem";
            this.qcm_rem.Visible = true;
            this.qcm_rem.VisibleIndex = 8;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1320, 77);
            this.sysToolBar1.TabIndex = 5;
            this.sysToolBar1.OnBtnSingleAuditClicked += new System.EventHandler(this.sysToolBar1_OnBtnSingleAuditClicked);
            this.sysToolBar1.OnBtnSinglePrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnSinglePrintClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 1005);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.sysToolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FrmControl";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "失控数据";
            this.Load += new System.EventHandler(this.FrmControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lue_instrmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_mid;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_itm;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_date;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_meas;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_c_no;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_ns;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_reson;
        private DevExpress.XtraGrid.Columns.GridColumn colqcm_fun;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lue_instrmt;
        private DevExpress.XtraGrid.Columns.GridColumn qcm_rem;


    }
}