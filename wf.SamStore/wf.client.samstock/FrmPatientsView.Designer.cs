namespace dcl.client.samstock
{
    partial class FrmPatientsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPatientsView));
            this.dsLabBindingSource = new System.Windows.Forms.BindingSource();
            this.gcSamDetail = new DevExpress.XtraGrid.GridControl();
            this.gvSamDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.dsLabBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSamDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSamDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // dsLabBindingSource
            // 
            this.dsLabBindingSource.DataMember = "patients";
            // 
            // gcSamDetail
            // 
            this.gcSamDetail.DataSource = this.dsLabBindingSource;
            this.gcSamDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcSamDetail.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcSamDetail.Location = new System.Drawing.Point(0, 0);
            this.gcSamDetail.MainView = this.gvSamDetail;
            this.gcSamDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcSamDetail.Name = "gcSamDetail";
            this.gcSamDetail.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2});
            this.gcSamDetail.Size = new System.Drawing.Size(1000, 717);
            this.gcSamDetail.TabIndex = 1;
            this.gcSamDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvSamDetail});
            // 
            // gvSamDetail
            // 
            this.gvSamDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn18,
            this.gridColumn21});
            this.gvSamDetail.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvSamDetail.GridControl = this.gcSamDetail;
            this.gvSamDetail.Name = "gvSamDetail";
            this.gvSamDetail.OptionsBehavior.Editable = false;
            this.gvSamDetail.OptionsCustomization.AllowColumnMoving = false;
            this.gvSamDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvSamDetail.OptionsSelection.MultiSelect = true;
            this.gvSamDetail.OptionsView.ShowGroupPanel = false;
            this.gvSamDetail.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn15, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "条码号";
            this.gridColumn8.FieldName = "pat_bar_code";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 0;
            this.gridColumn8.Width = 124;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "姓名";
            this.gridColumn9.FieldName = "pat_name";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 124;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "性别";
            this.gridColumn10.FieldName = "pat_sex";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 124;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "年龄";
            this.gridColumn11.FieldName = "pat_age";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 3;
            this.gridColumn11.Width = 124;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "病人ID";
            this.gridColumn12.FieldName = "pat_in_no";
            this.gridColumn12.Name = "gridColumn12";
            // 
            // gridColumn13
            // 
            this.gridColumn13.Caption = "科室";
            this.gridColumn13.FieldName = "pat_dep_name";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 4;
            this.gridColumn13.Width = 124;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "归档时间";
            this.gridColumn14.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn14.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn14.FieldName = "ssd_createtime";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 5;
            this.gridColumn14.Width = 185;
            // 
            // gridColumn15
            // 
            this.gridColumn15.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn15.Caption = "顺序号";
            this.gridColumn15.FieldName = "ssd_seq";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 6;
            this.gridColumn15.Width = 108;
            // 
            // gridColumn16
            // 
            this.gridColumn16.Caption = "标本名称";
            this.gridColumn16.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.gridColumn16.FieldName = "pat_sam_id";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 7;
            this.gridColumn16.Width = 108;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sam_id", "样本ID"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("sam_name", "样本名称")});
            this.repositoryItemLookUpEdit1.DisplayMember = "sam_name";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.ValueMember = "sam_id";
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "标本状态";
            this.gridColumn18.FieldName = "patflagstatus";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 8;
            this.gridColumn18.Width = 108;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "孔位消息";
            this.gridColumn21.FieldName = "ssd_num_xy";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.OptionsColumn.AllowEdit = false;
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 9;
            this.gridColumn21.Width = 119;
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("status_name", "名称"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("status_id", "ID")});
            this.repositoryItemLookUpEdit2.DisplayMember = "status_name";
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.NullText = "[空]";
            this.repositoryItemLookUpEdit2.ValueMember = "status_id";
            // 
            // FrmPatientsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 717);
            this.Controls.Add(this.gcSamDetail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FrmPatientsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "病人资料浏览";
            ((System.ComponentModel.ISupportInitialize)(this.dsLabBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSamDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSamDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource dsLabBindingSource;
        private DevExpress.XtraGrid.GridControl gcSamDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvSamDetail;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
    }
}