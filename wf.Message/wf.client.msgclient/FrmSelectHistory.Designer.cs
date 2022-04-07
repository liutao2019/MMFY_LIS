namespace dcl.client.msgclient
{
    partial class FrmSelectHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectHistory));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpLookEnd = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSel = new System.Windows.Forms.Button();
            this.dtpLookStart = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDepCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gcExportData = new DevExpress.XtraGrid.GridControl();
            this.gvExportData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcLookData = new DevExpress.XtraGrid.GridControl();
            this.gvLookData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colpat_dep_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_bar_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_bed_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_in_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_sex_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_age_exp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_tel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_result = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.colpat_chk_number = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_chk_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_chk_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_look_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_look_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_datediff = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colaffirm_mode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_rem = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.colpat_rem2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.colDotname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcExportData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExportData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit3)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpLookEnd);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnSel);
            this.panel1.Controls.Add(this.dtpLookStart);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblDepCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1241, 74);
            this.panel1.TabIndex = 0;
            // 
            // dtpLookEnd
            // 
            this.dtpLookEnd.CustomFormat = "yyyy-MM-dd";
            this.dtpLookEnd.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpLookEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLookEnd.Location = new System.Drawing.Point(301, 35);
            this.dtpLookEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpLookEnd.Name = "dtpLookEnd";
            this.dtpLookEnd.Size = new System.Drawing.Size(135, 27);
            this.dtpLookEnd.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "至";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(611, 31);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(104, 32);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "导 出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSel
            // 
            this.btnSel.Location = new System.Drawing.Point(465, 31);
            this.btnSel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSel.Name = "btnSel";
            this.btnSel.Size = new System.Drawing.Size(104, 32);
            this.btnSel.TabIndex = 4;
            this.btnSel.Text = "查 看";
            this.btnSel.UseVisualStyleBackColor = true;
            this.btnSel.Click += new System.EventHandler(this.btnSel_Click);
            // 
            // dtpLookStart
            // 
            this.dtpLookStart.CustomFormat = "yyyy-MM-dd";
            this.dtpLookStart.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dtpLookStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpLookStart.Location = new System.Drawing.Point(128, 35);
            this.dtpLookStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpLookStart.Name = "dtpLookStart";
            this.dtpLookStart.Size = new System.Drawing.Size(135, 27);
            this.dtpLookStart.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(29, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "查看日期:";
            // 
            // lblDepCode
            // 
            this.lblDepCode.AutoSize = true;
            this.lblDepCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDepCode.Location = new System.Drawing.Point(133, 11);
            this.lblDepCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDepCode.Name = "lblDepCode";
            this.lblDepCode.Size = new System.Drawing.Size(53, 18);
            this.lblDepCode.TabIndex = 1;
            this.lblDepCode.Text = "00000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(29, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前科室:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gcExportData);
            this.panel2.Controls.Add(this.gcLookData);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 74);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1241, 421);
            this.panel2.TabIndex = 1;
            // 
            // gcExportData
            // 
            this.gcExportData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcExportData.Location = new System.Drawing.Point(72, 128);
            this.gcExportData.MainView = this.gvExportData;
            this.gcExportData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcExportData.Name = "gcExportData";
            this.gcExportData.Size = new System.Drawing.Size(533, 250);
            this.gcExportData.TabIndex = 5;
            this.gcExportData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExportData});
            this.gcExportData.Visible = false;
            // 
            // gvExportData
            // 
            this.gvExportData.GridControl = this.gcExportData;
            this.gvExportData.Name = "gvExportData";
            // 
            // gcLookData
            // 
            this.gcLookData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLookData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcLookData.Location = new System.Drawing.Point(0, 0);
            this.gcLookData.MainView = this.gvLookData;
            this.gcLookData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcLookData.Name = "gcLookData";
            this.gcLookData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemMemoEdit2,
            this.repositoryItemMemoEdit3});
            this.gcLookData.Size = new System.Drawing.Size(1241, 421);
            this.gcLookData.TabIndex = 4;
            this.gcLookData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLookData});
            // 
            // gvLookData
            // 
            this.gvLookData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colpat_dep_name,
            this.colpat_bar_code,
            this.gridColumn1,
            this.colpat_bed_no,
            this.colpat_in_no,
            this.colpat_name,
            this.colpat_sex_name,
            this.colpat_age_exp,
            this.colpat_tel,
            this.colpat_result,
            this.colpat_chk_number,
            this.colpat_chk_name,
            this.colpat_chk_date,
            this.colpat_look_name,
            this.colpat_look_date,
            this.colpat_datediff,
            this.colaffirm_mode,
            this.colpat_rem,
            this.colpat_rem2,
            this.colDotname});
            this.gvLookData.GridControl = this.gcLookData;
            this.gvLookData.Name = "gvLookData";
            this.gvLookData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLookData.OptionsView.ColumnAutoWidth = false;
            this.gvLookData.OptionsView.RowAutoHeight = true;
            this.gvLookData.OptionsView.ShowGroupPanel = false;
            this.gvLookData.DoubleClick += new System.EventHandler(this.gvLookData_DoubleClick);
            // 
            // colpat_dep_name
            // 
            this.colpat_dep_name.Caption = "科室";
            this.colpat_dep_name.FieldName = "DeptName";
            this.colpat_dep_name.Name = "colpat_dep_name";
            this.colpat_dep_name.OptionsColumn.AllowEdit = false;
            this.colpat_dep_name.Visible = true;
            this.colpat_dep_name.VisibleIndex = 0;
            this.colpat_dep_name.Width = 77;
            // 
            // colpat_bar_code
            // 
            this.colpat_bar_code.Caption = "条码";
            this.colpat_bar_code.FieldName = "RepBarCode";
            this.colpat_bar_code.Name = "colpat_bar_code";
            this.colpat_bar_code.OptionsColumn.ReadOnly = true;
            this.colpat_bar_code.Visible = true;
            this.colpat_bar_code.VisibleIndex = 1;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "标本";
            this.gridColumn1.FieldName = "SamName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            // 
            // colpat_bed_no
            // 
            this.colpat_bed_no.Caption = "床号";
            this.colpat_bed_no.FieldName = "PidBedNo";
            this.colpat_bed_no.Name = "colpat_bed_no";
            this.colpat_bed_no.OptionsColumn.AllowEdit = false;
            this.colpat_bed_no.Visible = true;
            this.colpat_bed_no.VisibleIndex = 3;
            this.colpat_bed_no.Width = 45;
            // 
            // colpat_in_no
            // 
            this.colpat_in_no.Caption = "病人ID";
            this.colpat_in_no.FieldName = "PidInNo";
            this.colpat_in_no.Name = "colpat_in_no";
            this.colpat_in_no.OptionsColumn.AllowEdit = false;
            this.colpat_in_no.Visible = true;
            this.colpat_in_no.VisibleIndex = 4;
            this.colpat_in_no.Width = 79;
            // 
            // colpat_name
            // 
            this.colpat_name.Caption = "姓名";
            this.colpat_name.FieldName = "PidName";
            this.colpat_name.Name = "colpat_name";
            this.colpat_name.OptionsColumn.AllowEdit = false;
            this.colpat_name.Visible = true;
            this.colpat_name.VisibleIndex = 5;
            this.colpat_name.Width = 85;
            // 
            // colpat_sex_name
            // 
            this.colpat_sex_name.Caption = "性别";
            this.colpat_sex_name.FieldName = "PidSex";
            this.colpat_sex_name.Name = "colpat_sex_name";
            this.colpat_sex_name.OptionsColumn.AllowEdit = false;
            this.colpat_sex_name.Visible = true;
            this.colpat_sex_name.VisibleIndex = 6;
            this.colpat_sex_name.Width = 39;
            // 
            // colpat_age_exp
            // 
            this.colpat_age_exp.Caption = "年龄";
            this.colpat_age_exp.FieldName = "PidAgeStr";
            this.colpat_age_exp.Name = "colpat_age_exp";
            this.colpat_age_exp.OptionsColumn.AllowEdit = false;
            this.colpat_age_exp.Visible = true;
            this.colpat_age_exp.VisibleIndex = 7;
            this.colpat_age_exp.Width = 39;
            // 
            // colpat_tel
            // 
            this.colpat_tel.Caption = "电话";
            this.colpat_tel.FieldName = "PidTel";
            this.colpat_tel.Name = "colpat_tel";
            this.colpat_tel.Visible = true;
            this.colpat_tel.VisibleIndex = 8;
            // 
            // colpat_result
            // 
            this.colpat_result.Caption = "危急值结果";
            this.colpat_result.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colpat_result.FieldName = "PidResult";
            this.colpat_result.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
            this.colpat_result.Name = "colpat_result";
            this.colpat_result.OptionsColumn.AllowEdit = false;
            this.colpat_result.Visible = true;
            this.colpat_result.VisibleIndex = 9;
            this.colpat_result.Width = 194;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.repositoryItemMemoEdit1.ReadOnly = true;
            // 
            // colpat_chk_number
            // 
            this.colpat_chk_number.Caption = "审核人工号";
            this.colpat_chk_number.FieldName = "PidChkNumber";
            this.colpat_chk_number.Name = "colpat_chk_number";
            this.colpat_chk_number.OptionsColumn.AllowEdit = false;
            this.colpat_chk_number.Visible = true;
            this.colpat_chk_number.VisibleIndex = 10;
            // 
            // colpat_chk_name
            // 
            this.colpat_chk_name.Caption = "审核人";
            this.colpat_chk_name.FieldName = "PidChkName";
            this.colpat_chk_name.Name = "colpat_chk_name";
            this.colpat_chk_name.OptionsColumn.AllowEdit = false;
            this.colpat_chk_name.Visible = true;
            this.colpat_chk_name.VisibleIndex = 11;
            this.colpat_chk_name.Width = 68;
            // 
            // colpat_chk_date
            // 
            this.colpat_chk_date.Caption = "审核时间";
            this.colpat_chk_date.DisplayFormat.FormatString = "yyyy-MM-dd hh:mm";
            this.colpat_chk_date.FieldName = "RepReportDate";
            this.colpat_chk_date.Name = "colpat_chk_date";
            this.colpat_chk_date.OptionsColumn.AllowEdit = false;
            this.colpat_chk_date.Visible = true;
            this.colpat_chk_date.VisibleIndex = 12;
            this.colpat_chk_date.Width = 98;
            // 
            // colpat_look_name
            // 
            this.colpat_look_name.Caption = "确认人";
            this.colpat_look_name.FieldName = "PatLookName";
            this.colpat_look_name.Name = "colpat_look_name";
            this.colpat_look_name.OptionsColumn.AllowEdit = false;
            this.colpat_look_name.Visible = true;
            this.colpat_look_name.VisibleIndex = 13;
            this.colpat_look_name.Width = 62;
            // 
            // colpat_look_date
            // 
            this.colpat_look_date.Caption = "确认时间";
            this.colpat_look_date.DisplayFormat.FormatString = "yyyy-MM-dd hh:mm";
            this.colpat_look_date.FieldName = "RepReadDate";
            this.colpat_look_date.Name = "colpat_look_date";
            this.colpat_look_date.OptionsColumn.AllowEdit = false;
            this.colpat_look_date.Visible = true;
            this.colpat_look_date.VisibleIndex = 14;
            this.colpat_look_date.Width = 82;
            // 
            // colpat_datediff
            // 
            this.colpat_datediff.Caption = "确认时间差";
            this.colpat_datediff.FieldName = "PidDatediff";
            this.colpat_datediff.Name = "colpat_datediff";
            this.colpat_datediff.OptionsColumn.AllowEdit = false;
            this.colpat_datediff.Visible = true;
            this.colpat_datediff.VisibleIndex = 15;
            // 
            // colaffirm_mode
            // 
            this.colaffirm_mode.Caption = "确认方式";
            this.colaffirm_mode.FieldName = "AffirmMode";
            this.colaffirm_mode.Name = "colaffirm_mode";
            this.colaffirm_mode.OptionsColumn.AllowEdit = false;
            this.colaffirm_mode.Visible = true;
            this.colaffirm_mode.VisibleIndex = 16;
            this.colaffirm_mode.Width = 67;
            // 
            // colpat_rem
            // 
            this.colpat_rem.Caption = "备注";
            this.colpat_rem.ColumnEdit = this.repositoryItemMemoEdit2;
            this.colpat_rem.FieldName = "PidRes";
            this.colpat_rem.Name = "colpat_rem";
            this.colpat_rem.Visible = true;
            this.colpat_rem.VisibleIndex = 18;
            this.colpat_rem.Width = 177;
            // 
            // repositoryItemMemoEdit2
            // 
            this.repositoryItemMemoEdit2.Name = "repositoryItemMemoEdit2";
            this.repositoryItemMemoEdit2.ReadOnly = true;
            // 
            // colpat_rem2
            // 
            this.colpat_rem2.Caption = "备注2";
            this.colpat_rem2.ColumnEdit = this.repositoryItemMemoEdit3;
            this.colpat_rem2.FieldName = "PidRes2";
            this.colpat_rem2.Name = "colpat_rem2";
            this.colpat_rem2.Visible = true;
            this.colpat_rem2.VisibleIndex = 19;
            this.colpat_rem2.Width = 177;
            // 
            // repositoryItemMemoEdit3
            // 
            this.repositoryItemMemoEdit3.Name = "repositoryItemMemoEdit3";
            // 
            // colDotname
            // 
            this.colDotname.Caption = "医生";
            this.colDotname.FieldName = "DoctorName";
            this.colDotname.Name = "colDotname";
            this.colDotname.OptionsColumn.AllowEdit = false;
            this.colDotname.Visible = true;
            this.colDotname.VisibleIndex = 17;
            this.colDotname.Width = 89;
            // 
            // FrmSelectHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 495);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimizeBox = false;
            this.Name = "FrmSelectHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查看历史数据";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmSelectHistory_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcExportData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExportData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDepCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpLookStart;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSel;
        private DevExpress.XtraGrid.GridControl gcLookData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLookData;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_dep_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_sex_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_age_exp;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_result;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_chk_date;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_bed_no;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_chk_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_look_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_look_date;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_datediff;
        private DevExpress.XtraGrid.Columns.GridColumn colaffirm_mode;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_rem;
        private System.Windows.Forms.DateTimePicker dtpLookEnd;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_in_no;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn colDotname;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_bar_code;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_rem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit3;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_chk_number;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_tel;
        private DevExpress.XtraGrid.GridControl gcExportData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvExportData;
    }
}