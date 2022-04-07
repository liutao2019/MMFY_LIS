namespace dcl.client.qc
{
    partial class FrmDiversion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDiversion));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bdqcvalueset = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colqcr_itr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit1 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bddict_instrmt = new System.Windows.Forms.BindingSource();
            this.colqcr_itm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit2 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.colqcr_meas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colqcr_cast_meas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.bdqcvalue = new System.Windows.Forms.BindingSource();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdqcvalueset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bddict_instrmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdqcvalue)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bdqcvalueset;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 60);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2,
            this.ctlRepositoryItemLookUpEdit1,
            this.ctlRepositoryItemLookUpEdit2});
            this.gridControl1.Size = new System.Drawing.Size(647, 493);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bdqcvalueset
            // 
            this.bdqcvalueset.DataSource = typeof(dcl.entity.EntityDicQcConvert);
            // 
            // gridView1
            // 
            this.gridView1.ColumnPanelRowHeight = 23;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colqcr_itr_id,
            this.colqcr_itm_ecd,
            this.colqcr_meas,
            this.colqcr_cast_meas});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colqcr_itr_id
            // 
            this.colqcr_itr_id.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_itr_id.AppearanceCell.Options.UseFont = true;
            this.colqcr_itr_id.AppearanceCell.Options.UseTextOptions = true;
            this.colqcr_itr_id.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_itr_id.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_itr_id.AppearanceHeader.Options.UseFont = true;
            this.colqcr_itr_id.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcr_itr_id.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_itr_id.Caption = "仪器";
            this.colqcr_itr_id.ColumnEdit = this.ctlRepositoryItemLookUpEdit1;
            this.colqcr_itr_id.FieldName = "ItrId";
            this.colqcr_itr_id.Name = "colqcr_itr_id";
            this.colqcr_itr_id.Visible = true;
            this.colqcr_itr_id.VisibleIndex = 0;
            // 
            // ctlRepositoryItemLookUpEdit1
            // 
            this.ctlRepositoryItemLookUpEdit1.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit1.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItrId", "编码", 46, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItrEname", "代码", 43, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItrName", "名称", 54, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.ctlRepositoryItemLookUpEdit1.DataSource = this.bddict_instrmt;
            this.ctlRepositoryItemLookUpEdit1.DisplayMember = "ItrEname";
            this.ctlRepositoryItemLookUpEdit1.DropDownRows = 15;
            this.ctlRepositoryItemLookUpEdit1.Name = "ctlRepositoryItemLookUpEdit1";
            this.ctlRepositoryItemLookUpEdit1.NullText = "";
            this.ctlRepositoryItemLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit1.ValueMember = "ItrId";
            // 
            // bddict_instrmt
            // 
            this.bddict_instrmt.DataSource = typeof(dcl.entity.EntityDicInstrument);
            // 
            // colqcr_itm_ecd
            // 
            this.colqcr_itm_ecd.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_itm_ecd.AppearanceCell.Options.UseFont = true;
            this.colqcr_itm_ecd.AppearanceCell.Options.UseTextOptions = true;
            this.colqcr_itm_ecd.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_itm_ecd.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_itm_ecd.AppearanceHeader.Options.UseFont = true;
            this.colqcr_itm_ecd.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcr_itm_ecd.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_itm_ecd.Caption = "项目";
            this.colqcr_itm_ecd.ColumnEdit = this.ctlRepositoryItemLookUpEdit2;
            this.colqcr_itm_ecd.FieldName = "ItmId";
            this.colqcr_itm_ecd.Name = "colqcr_itm_ecd";
            this.colqcr_itm_ecd.Visible = true;
            this.colqcr_itm_ecd.VisibleIndex = 1;
            // 
            // ctlRepositoryItemLookUpEdit2
            // 
            this.ctlRepositoryItemLookUpEdit2.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit2.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmId", "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmEcode", "代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmName", "名称")});
            this.ctlRepositoryItemLookUpEdit2.DisplayMember = "ItmEcode";
            this.ctlRepositoryItemLookUpEdit2.DropDownRows = 15;
            this.ctlRepositoryItemLookUpEdit2.Name = "ctlRepositoryItemLookUpEdit2";
            this.ctlRepositoryItemLookUpEdit2.NullText = "";
            this.ctlRepositoryItemLookUpEdit2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit2.ValueMember = "ItmId";
            // 
            // colqcr_meas
            // 
            this.colqcr_meas.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_meas.AppearanceCell.Options.UseFont = true;
            this.colqcr_meas.AppearanceCell.Options.UseTextOptions = true;
            this.colqcr_meas.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_meas.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_meas.AppearanceHeader.Options.UseFont = true;
            this.colqcr_meas.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcr_meas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_meas.Caption = "原始值";
            this.colqcr_meas.FieldName = "ItmValue";
            this.colqcr_meas.Name = "colqcr_meas";
            this.colqcr_meas.Visible = true;
            this.colqcr_meas.VisibleIndex = 2;
            // 
            // colqcr_cast_meas
            // 
            this.colqcr_cast_meas.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_cast_meas.AppearanceCell.Options.UseFont = true;
            this.colqcr_cast_meas.AppearanceCell.Options.UseTextOptions = true;
            this.colqcr_cast_meas.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_cast_meas.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.colqcr_cast_meas.AppearanceHeader.Options.UseFont = true;
            this.colqcr_cast_meas.AppearanceHeader.Options.UseTextOptions = true;
            this.colqcr_cast_meas.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colqcr_cast_meas.Caption = "表示值";
            this.colqcr_cast_meas.FieldName = "ItmConvertValue";
            this.colqcr_cast_meas.Name = "colqcr_cast_meas";
            this.colqcr_cast_meas.Visible = true;
            this.colqcr_cast_meas.VisibleIndex = 3;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itr_id", "编码", 46, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itr_mid", "代码", 43, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itr_name", "名称", 54, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.repositoryItemLookUpEdit1.DataSource = this.bddict_instrmt;
            this.repositoryItemLookUpEdit1.DisplayMember = "itr_mid";
            this.repositoryItemLookUpEdit1.DropDownRows = 15;
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.PopupWidth = 350;
            this.repositoryItemLookUpEdit1.ValueMember = "itr_id";
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itm_id", "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itm_ecd", "代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itm_name", "名称")});
            this.repositoryItemLookUpEdit2.DisplayMember = "itm_ecd";
            this.repositoryItemLookUpEdit2.DropDownRows = 15;
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.NullText = "";
            this.repositoryItemLookUpEdit2.PopupWidth = 350;
            this.repositoryItemLookUpEdit2.ValueMember = "itm_id";
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(647, 60);
            this.sysToolBar1.TabIndex = 2;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.simpleButton1_Click);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.simpleButton2_Click);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.simpleButton3_Click);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.simpleButton4_Click);
            // 
            // FrmDiversion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.sysToolBar1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "FrmDiversion";
            this.Size = new System.Drawing.Size(647, 553);
            this.Load += new System.EventHandler(this.FrmDiversion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdqcvalueset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bddict_instrmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdqcvalue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource bdqcvalueset;
        private DevExpress.XtraGrid.Columns.GridColumn colqcr_itr_id;
        private DevExpress.XtraGrid.Columns.GridColumn colqcr_itm_ecd;
        private DevExpress.XtraGrid.Columns.GridColumn colqcr_meas;
        private DevExpress.XtraGrid.Columns.GridColumn colqcr_cast_meas;
        private System.Windows.Forms.BindingSource bddict_instrmt;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private System.Windows.Forms.BindingSource bdqcvalue;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private lis.client.control.ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit1;
        private lis.client.control.ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit2;
    }
}