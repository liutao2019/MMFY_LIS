namespace dcl.client.msgsend
{
    partial class FrmShowInMsg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmShowInMsg));
            this.gcLookData = new DevExpress.XtraGrid.GridControl();
            this.gvLookData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colpat_select = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colpat_dep_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_bed_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_in_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_sex_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_age_exp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_result = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.colpat_c_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_chk_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_chk_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colmsg_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colmsg_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itr_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_host_order = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // gcLookData
            // 
            this.gcLookData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLookData.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcLookData.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcLookData.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcLookData.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcLookData.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcLookData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcLookData.Location = new System.Drawing.Point(0, 0);
            this.gcLookData.MainView = this.gvLookData;
            this.gcLookData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcLookData.Name = "gcLookData";
            this.gcLookData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit2,
            this.repositoryItemCheckEdit1});
            this.gcLookData.Size = new System.Drawing.Size(1340, 494);
            this.gcLookData.TabIndex = 307;
            this.gcLookData.UseEmbeddedNavigator = true;
            this.gcLookData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLookData});
            // 
            // gvLookData
            // 
            this.gvLookData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colpat_select,
            this.colpat_dep_name,
            this.colpat_bed_no,
            this.colpat_in_no,
            this.colpat_name,
            this.colpat_sex_name,
            this.colpat_age_exp,
            this.colpat_result,
            this.colpat_c_name,
            this.colpat_chk_name,
            this.colpat_chk_date,
            this.colmsg_type,
            this.colmsg_id,
            this.col_itr_name,
            this.col_pat_sid,
            this.col_pat_host_order,
            this.gridColumn10});
            this.gvLookData.GridControl = this.gcLookData;
            this.gvLookData.Name = "gvLookData";
            this.gvLookData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLookData.OptionsView.ColumnAutoWidth = false;
            this.gvLookData.OptionsView.RowAutoHeight = true;
            this.gvLookData.OptionsView.ShowGroupPanel = false;
            this.gvLookData.DoubleClick += new System.EventHandler(this.gvLookData_DoubleClick);
            // 
            // colpat_select
            // 
            this.colpat_select.ColumnEdit = this.repositoryItemCheckEdit1;
            this.colpat_select.FieldName = "pat_select";
            this.colpat_select.MinWidth = 30;
            this.colpat_select.Name = "colpat_select";
            this.colpat_select.OptionsColumn.AllowMove = false;
            this.colpat_select.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colpat_select.OptionsColumn.FixedWidth = true;
            this.colpat_select.OptionsFilter.AllowAutoFilter = false;
            this.colpat_select.OptionsFilter.AllowFilter = false;
            this.colpat_select.Width = 20;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // colpat_dep_name
            // 
            this.colpat_dep_name.Caption = "科室";
            this.colpat_dep_name.FieldName = "dep_name";
            this.colpat_dep_name.Name = "colpat_dep_name";
            this.colpat_dep_name.OptionsColumn.AllowEdit = false;
            this.colpat_dep_name.Visible = true;
            this.colpat_dep_name.VisibleIndex = 0;
            this.colpat_dep_name.Width = 90;
            // 
            // colpat_bed_no
            // 
            this.colpat_bed_no.Caption = "床号";
            this.colpat_bed_no.FieldName = "pat_bed_no";
            this.colpat_bed_no.Name = "colpat_bed_no";
            this.colpat_bed_no.OptionsColumn.AllowEdit = false;
            this.colpat_bed_no.Visible = true;
            this.colpat_bed_no.VisibleIndex = 1;
            this.colpat_bed_no.Width = 60;
            // 
            // colpat_in_no
            // 
            this.colpat_in_no.Caption = "病人ID";
            this.colpat_in_no.FieldName = "pat_in_no";
            this.colpat_in_no.Name = "colpat_in_no";
            this.colpat_in_no.OptionsColumn.AllowEdit = false;
            this.colpat_in_no.Visible = true;
            this.colpat_in_no.VisibleIndex = 2;
            this.colpat_in_no.Width = 85;
            // 
            // colpat_name
            // 
            this.colpat_name.Caption = "姓名";
            this.colpat_name.FieldName = "pat_name";
            this.colpat_name.Name = "colpat_name";
            this.colpat_name.OptionsColumn.AllowEdit = false;
            this.colpat_name.Visible = true;
            this.colpat_name.VisibleIndex = 3;
            // 
            // colpat_sex_name
            // 
            this.colpat_sex_name.Caption = "性别";
            this.colpat_sex_name.FieldName = "pat_sex";
            this.colpat_sex_name.Name = "colpat_sex_name";
            this.colpat_sex_name.OptionsColumn.AllowEdit = false;
            this.colpat_sex_name.Visible = true;
            this.colpat_sex_name.VisibleIndex = 4;
            this.colpat_sex_name.Width = 60;
            // 
            // colpat_age_exp
            // 
            this.colpat_age_exp.Caption = "年龄";
            this.colpat_age_exp.FieldName = "pat_age";
            this.colpat_age_exp.Name = "colpat_age_exp";
            this.colpat_age_exp.OptionsColumn.AllowEdit = false;
            this.colpat_age_exp.Visible = true;
            this.colpat_age_exp.VisibleIndex = 5;
            this.colpat_age_exp.Width = 70;
            // 
            // colpat_result
            // 
            this.colpat_result.Caption = "危急值结果";
            this.colpat_result.ColumnEdit = this.repositoryItemMemoEdit2;
            this.colpat_result.FieldName = "pat_result";
            this.colpat_result.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.colpat_result.Name = "colpat_result";
            this.colpat_result.OptionsColumn.AllowEdit = false;
            this.colpat_result.Visible = true;
            this.colpat_result.VisibleIndex = 6;
            this.colpat_result.Width = 150;
            // 
            // repositoryItemMemoEdit2
            // 
            this.repositoryItemMemoEdit2.Name = "repositoryItemMemoEdit2";
            this.repositoryItemMemoEdit2.ReadOnly = true;
            // 
            // colpat_c_name
            // 
            this.colpat_c_name.Caption = "组合";
            this.colpat_c_name.FieldName = "pat_c_name";
            this.colpat_c_name.Name = "colpat_c_name";
            this.colpat_c_name.OptionsColumn.AllowEdit = false;
            // 
            // colpat_chk_name
            // 
            this.colpat_chk_name.Caption = "审核人";
            this.colpat_chk_name.FieldName = "pat_chk_name";
            this.colpat_chk_name.Name = "colpat_chk_name";
            this.colpat_chk_name.OptionsColumn.AllowEdit = false;
            // 
            // colpat_chk_date
            // 
            this.colpat_chk_date.Caption = "发送时间";
            this.colpat_chk_date.DisplayFormat.FormatString = "yyyy-MM-dd hh:mm";
            this.colpat_chk_date.FieldName = "pat_chk_date";
            this.colpat_chk_date.Name = "colpat_chk_date";
            this.colpat_chk_date.OptionsColumn.AllowEdit = false;
            this.colpat_chk_date.Visible = true;
            this.colpat_chk_date.VisibleIndex = 8;
            this.colpat_chk_date.Width = 120;
            // 
            // colmsg_type
            // 
            this.colmsg_type.Caption = "危急值信息类型";
            this.colmsg_type.FieldName = "msg_type";
            this.colmsg_type.Name = "colmsg_type";
            this.colmsg_type.OptionsColumn.AllowEdit = false;
            // 
            // colmsg_id
            // 
            this.colmsg_id.Caption = "信息ID";
            this.colmsg_id.FieldName = "msg_id";
            this.colmsg_id.Name = "colmsg_id";
            this.colmsg_id.Width = 61;
            // 
            // col_itr_name
            // 
            this.col_itr_name.Caption = "仪器";
            this.col_itr_name.FieldName = "itr_name";
            this.col_itr_name.Name = "col_itr_name";
            // 
            // col_pat_sid
            // 
            this.col_pat_sid.Caption = "样本号";
            this.col_pat_sid.FieldName = "pat_sid";
            this.col_pat_sid.Name = "col_pat_sid";
            this.col_pat_sid.Width = 90;
            // 
            // col_pat_host_order
            // 
            this.col_pat_host_order.Caption = "序号";
            this.col_pat_host_order.FieldName = "pat_host_order";
            this.col_pat_host_order.Name = "col_pat_host_order";
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "开单医生";
            this.gridColumn10.FieldName = "doc_name";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 7;
            this.gridColumn10.Width = 90;
            // 
            // FrmShowInMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 494);
            this.Controls.Add(this.gcLookData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmShowInMsg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "自编危急值-内部提醒";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmShowMsg_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcLookData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLookData;
        public DevExpress.XtraGrid.Columns.GridColumn colpat_select;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_dep_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_bed_no;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_in_no;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_sex_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_age_exp;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_result;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_c_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_chk_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_chk_date;
        private DevExpress.XtraGrid.Columns.GridColumn colmsg_type;
        private DevExpress.XtraGrid.Columns.GridColumn colmsg_id;
        private DevExpress.XtraGrid.Columns.GridColumn col_itr_name;
        private DevExpress.XtraGrid.Columns.GridColumn col_pat_sid;
        private DevExpress.XtraGrid.Columns.GridColumn col_pat_host_order;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;

    }
}