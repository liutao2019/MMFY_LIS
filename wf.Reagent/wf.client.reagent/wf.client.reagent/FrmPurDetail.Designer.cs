namespace wf.client.reagent
{
    partial class FrmPurDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPurDetail));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bsDetail = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colRpcd_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpcd_reacount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRpcd_price = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReagentName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReagentPackage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSupName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInvoiceNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOutPackage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit1 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.colBatchNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colReport = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit2 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.colStoreCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colValidDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTemparate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ctlRepositoryItemLookUpEdit3 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.colHasStoreCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnitName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gridControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1267, 505);
            this.panelControl1.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsDetail;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ctlRepositoryItemLookUpEdit1,
            this.ctlRepositoryItemLookUpEdit2,
            this.ctlRepositoryItemLookUpEdit3,
            this.repositoryItemLookUpEdit1});
            this.gridControl1.Size = new System.Drawing.Size(1263, 501);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsDetail
            // 
            this.bsDetail.DataSource = typeof(dcl.entity.EntityReaPurchaseDetail);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRpcd_no,
            this.colRpcd_reacount,
            this.colRpcd_price,
            this.colReagentName,
            this.colReagentPackage,
            this.colSupName,
            this.colInvoiceNo,
            this.colOutPackage,
            this.colBatchNo,
            this.colReport,
            this.colStoreCount,
            this.colValidDate,
            this.colTemparate,
            this.colHasStoreCount,
            this.colUnitName});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colRpcd_no
            // 
            this.colRpcd_no.Caption = "采购单号";
            this.colRpcd_no.FieldName = "Rpcd_no";
            this.colRpcd_no.Name = "colRpcd_no";
            this.colRpcd_no.OptionsColumn.AllowEdit = false;
            this.colRpcd_no.Visible = true;
            this.colRpcd_no.VisibleIndex = 1;
            this.colRpcd_no.Width = 82;
            // 
            // colRpcd_reacount
            // 
            this.colRpcd_reacount.Caption = "采购数量";
            this.colRpcd_reacount.FieldName = "Rpcd_reacount";
            this.colRpcd_reacount.Name = "colRpcd_reacount";
            this.colRpcd_reacount.OptionsColumn.AllowEdit = false;
            this.colRpcd_reacount.Visible = true;
            this.colRpcd_reacount.VisibleIndex = 6;
            this.colRpcd_reacount.Width = 84;
            // 
            // colRpcd_price
            // 
            this.colRpcd_price.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colRpcd_price.AppearanceHeader.Options.UseForeColor = true;
            this.colRpcd_price.Caption = "单价";
            this.colRpcd_price.FieldName = "Rpcd_price";
            this.colRpcd_price.Name = "colRpcd_price";
            this.colRpcd_price.Visible = true;
            this.colRpcd_price.VisibleIndex = 11;
            this.colRpcd_price.Width = 41;
            // 
            // colReagentName
            // 
            this.colReagentName.Caption = "试剂名称";
            this.colReagentName.FieldName = "ReagentName";
            this.colReagentName.Name = "colReagentName";
            this.colReagentName.OptionsColumn.AllowEdit = false;
            this.colReagentName.Visible = true;
            this.colReagentName.VisibleIndex = 2;
            this.colReagentName.Width = 82;
            // 
            // colReagentPackage
            // 
            this.colReagentPackage.Caption = "规格";
            this.colReagentPackage.FieldName = "ReagentPackage";
            this.colReagentPackage.Name = "colReagentPackage";
            this.colReagentPackage.OptionsColumn.AllowEdit = false;
            this.colReagentPackage.Visible = true;
            this.colReagentPackage.VisibleIndex = 4;
            this.colReagentPackage.Width = 56;
            // 
            // colSupName
            // 
            this.colSupName.Caption = "供货商";
            this.colSupName.FieldName = "SupName";
            this.colSupName.Name = "colSupName";
            this.colSupName.OptionsColumn.AllowEdit = false;
            this.colSupName.Visible = true;
            this.colSupName.VisibleIndex = 3;
            this.colSupName.Width = 82;
            // 
            // colInvoiceNo
            // 
            this.colInvoiceNo.Caption = "发票号";
            this.colInvoiceNo.FieldName = "InvoiceNo";
            this.colInvoiceNo.Name = "colInvoiceNo";
            this.colInvoiceNo.Visible = true;
            this.colInvoiceNo.VisibleIndex = 12;
            this.colInvoiceNo.Width = 88;
            // 
            // colOutPackage
            // 
            this.colOutPackage.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colOutPackage.AppearanceHeader.Options.UseForeColor = true;
            this.colOutPackage.Caption = "外包装";
            this.colOutPackage.ColumnEdit = this.ctlRepositoryItemLookUpEdit1;
            this.colOutPackage.FieldName = "OutPackage";
            this.colOutPackage.Name = "colOutPackage";
            this.colOutPackage.Visible = true;
            this.colOutPackage.VisibleIndex = 13;
            this.colOutPackage.Width = 88;
            // 
            // ctlRepositoryItemLookUpEdit1
            // 
            this.ctlRepositoryItemLookUpEdit1.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit1.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.ctlRepositoryItemLookUpEdit1.DisplayMember = "EvaContent";
            this.ctlRepositoryItemLookUpEdit1.Name = "ctlRepositoryItemLookUpEdit1";
            this.ctlRepositoryItemLookUpEdit1.NullText = "";
            this.ctlRepositoryItemLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit1.ValueMember = "EvaContent";
            // 
            // colBatchNo
            // 
            this.colBatchNo.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colBatchNo.AppearanceHeader.Options.UseForeColor = true;
            this.colBatchNo.Caption = "批号";
            this.colBatchNo.FieldName = "BatchNo";
            this.colBatchNo.Name = "colBatchNo";
            this.colBatchNo.Visible = true;
            this.colBatchNo.VisibleIndex = 10;
            this.colBatchNo.Width = 88;
            // 
            // colReport
            // 
            this.colReport.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colReport.AppearanceHeader.Options.UseForeColor = true;
            this.colReport.Caption = "检验报告";
            this.colReport.ColumnEdit = this.ctlRepositoryItemLookUpEdit2;
            this.colReport.FieldName = "Report";
            this.colReport.Name = "colReport";
            this.colReport.Visible = true;
            this.colReport.VisibleIndex = 14;
            this.colReport.Width = 88;
            // 
            // ctlRepositoryItemLookUpEdit2
            // 
            this.ctlRepositoryItemLookUpEdit2.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit2.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.ctlRepositoryItemLookUpEdit2.DisplayMember = "EvaContent";
            this.ctlRepositoryItemLookUpEdit2.Name = "ctlRepositoryItemLookUpEdit2";
            this.ctlRepositoryItemLookUpEdit2.NullText = "";
            this.ctlRepositoryItemLookUpEdit2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit2.ValueMember = "EvaContent";
            // 
            // colStoreCount
            // 
            this.colStoreCount.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colStoreCount.AppearanceHeader.Options.UseForeColor = true;
            this.colStoreCount.Caption = "入库数";
            this.colStoreCount.FieldName = "StoreCount";
            this.colStoreCount.Name = "colStoreCount";
            this.colStoreCount.Visible = true;
            this.colStoreCount.VisibleIndex = 8;
            this.colStoreCount.Width = 63;
            // 
            // colValidDate
            // 
            this.colValidDate.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colValidDate.AppearanceHeader.Options.UseForeColor = true;
            this.colValidDate.Caption = "有效期";
            this.colValidDate.FieldName = "ValidDate";
            this.colValidDate.Name = "colValidDate";
            this.colValidDate.Visible = true;
            this.colValidDate.VisibleIndex = 9;
            this.colValidDate.Width = 93;
            // 
            // colTemparate
            // 
            this.colTemparate.AppearanceHeader.ForeColor = System.Drawing.Color.Red;
            this.colTemparate.AppearanceHeader.Options.UseForeColor = true;
            this.colTemparate.Caption = "到达温度";
            this.colTemparate.ColumnEdit = this.ctlRepositoryItemLookUpEdit3;
            this.colTemparate.FieldName = "Temparate";
            this.colTemparate.Name = "colTemparate";
            this.colTemparate.Visible = true;
            this.colTemparate.VisibleIndex = 15;
            this.colTemparate.Width = 164;
            // 
            // ctlRepositoryItemLookUpEdit3
            // 
            this.ctlRepositoryItemLookUpEdit3.ActionButtonIndex = 1;
            this.ctlRepositoryItemLookUpEdit3.AutoHeight = false;
            this.ctlRepositoryItemLookUpEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ctlRepositoryItemLookUpEdit3.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("EvaContent", "内容")});
            this.ctlRepositoryItemLookUpEdit3.DisplayMember = "EvaContent";
            this.ctlRepositoryItemLookUpEdit3.Name = "ctlRepositoryItemLookUpEdit3";
            this.ctlRepositoryItemLookUpEdit3.NullText = "";
            this.ctlRepositoryItemLookUpEdit3.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.ctlRepositoryItemLookUpEdit3.ValueMember = "EvaContent";
            // 
            // colHasStoreCount
            // 
            this.colHasStoreCount.Caption = "已入库数目";
            this.colHasStoreCount.FieldName = "HasStoreCount";
            this.colHasStoreCount.Name = "colHasStoreCount";
            this.colHasStoreCount.OptionsColumn.AllowEdit = false;
            this.colHasStoreCount.Visible = true;
            this.colHasStoreCount.VisibleIndex = 7;
            this.colHasStoreCount.Width = 85;
            // 
            // colUnitName
            // 
            this.colUnitName.Caption = "单位";
            this.colUnitName.FieldName = "UnitName";
            this.colUnitName.Name = "colUnitName";
            this.colUnitName.OptionsColumn.AllowEdit = false;
            this.colUnitName.Visible = true;
            this.colUnitName.VisibleIndex = 5;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.DisplayMember = "EvaContent";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.ValueMember = "EvaContent";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.simpleButton2);
            this.panelControl2.Controls.Add(this.simpleButton1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 505);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1267, 46);
            this.panelControl2.TabIndex = 1;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(685, 7);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(87, 27);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "取消";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(278, 7);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(87, 27);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "确定";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmPurDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1267, 551);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPurDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "采购明细";
            this.Load += new System.EventHandler(this.FrmPurDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctlRepositoryItemLookUpEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource bsDetail;
        private DevExpress.XtraGrid.Columns.GridColumn colRpcd_no;
        private DevExpress.XtraGrid.Columns.GridColumn colRpcd_reacount;
        private DevExpress.XtraGrid.Columns.GridColumn colRpcd_price;
        private DevExpress.XtraGrid.Columns.GridColumn colReagentName;
        private DevExpress.XtraGrid.Columns.GridColumn colReagentPackage;
        private DevExpress.XtraGrid.Columns.GridColumn colSupName;
        private DevExpress.XtraGrid.Columns.GridColumn colInvoiceNo;
        private DevExpress.XtraGrid.Columns.GridColumn colOutPackage;
        private DevExpress.XtraGrid.Columns.GridColumn colBatchNo;
        private DevExpress.XtraGrid.Columns.GridColumn colReport;
        private DevExpress.XtraGrid.Columns.GridColumn colStoreCount;
        private DevExpress.XtraGrid.Columns.GridColumn colValidDate;
        private DevExpress.XtraGrid.Columns.GridColumn colTemparate;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraGrid.Columns.GridColumn colHasStoreCount;
        private DevExpress.XtraGrid.Columns.GridColumn colUnitName;
        private lis.client.control.ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit1;
        private lis.client.control.ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit2;
        private lis.client.control.ctlRepositoryItemLookUpEdit ctlRepositoryItemLookUpEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
    }
}