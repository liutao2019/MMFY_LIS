namespace dcl.client.notifyclient
{
    partial class FrmUrgentNotify
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUrgentNotify));
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.gcLookData = new DevExpress.XtraGrid.GridControl();
            this.gvLookData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colpat_select = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colpat_dep_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coldep_tel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_bed_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_in_no = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_sex_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_age_exp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_result = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.colpat_c_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_chk_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_chk_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colmsg_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colmsg_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itr_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_host_order = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.chkSendMsg = new System.Windows.Forms.CheckBox();
            this.BtnUndo = new System.Windows.Forms.Button();
            this.BtnAudit = new System.Windows.Forms.Button();
            this.BtnSAudit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblShowCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblShowCount2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkEdit1);
            this.panel1.Controls.Add(this.gcLookData);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(814, 308);
            this.panel1.TabIndex = 0;
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(24, 3);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "";
            this.checkEdit1.Size = new System.Drawing.Size(18, 19);
            this.checkEdit1.TabIndex = 160;
            this.checkEdit1.Visible = false;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // gcLookData
            // 
            this.gcLookData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcLookData.Location = new System.Drawing.Point(0, 0);
            this.gcLookData.MainView = this.gvLookData;
            this.gcLookData.Name = "gcLookData";
            this.gcLookData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemCheckEdit1});
            this.gcLookData.Size = new System.Drawing.Size(814, 268);
            this.gcLookData.TabIndex = 6;
            this.gcLookData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLookData});
            // 
            // gvLookData
            // 
            this.gvLookData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colpat_select,
            this.colpat_dep_name,
            this.coldep_tel,
            this.colpat_bed_no,
            this.colpat_in_no,
            this.colpat_name,
            this.gridColumn2,
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
            this.gridColumn1,
            this.gridColumn3});
            this.gvLookData.GridControl = this.gcLookData;
            this.gvLookData.Name = "gvLookData";
            this.gvLookData.OptionsDetail.ShowDetailTabs = false;
            this.gvLookData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLookData.OptionsView.ColumnAutoWidth = false;
            this.gvLookData.OptionsView.RowAutoHeight = true;
            this.gvLookData.OptionsView.ShowDetailButtons = false;
            this.gvLookData.OptionsView.ShowGroupPanel = false;
            this.gvLookData.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvLookData_RowStyle);
            this.gvLookData.DoubleClick += new System.EventHandler(this.gvLookData_DoubleClick);
            // 
            // colpat_select
            // 
            this.colpat_select.Caption = " ";
            this.colpat_select.ColumnEdit = this.repositoryItemCheckEdit1;
            this.colpat_select.FieldName = "PatSelect";
            this.colpat_select.MinWidth = 30;
            this.colpat_select.Name = "colpat_select";
            this.colpat_select.OptionsColumn.AllowMove = false;
            this.colpat_select.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colpat_select.OptionsColumn.FixedWidth = true;
            this.colpat_select.OptionsFilter.AllowAutoFilter = false;
            this.colpat_select.OptionsFilter.AllowFilter = false;
            this.colpat_select.Visible = true;
            this.colpat_select.VisibleIndex = 0;
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
            this.colpat_dep_name.FieldName = "DeptName";
            this.colpat_dep_name.Name = "colpat_dep_name";
            this.colpat_dep_name.OptionsColumn.AllowEdit = false;
            this.colpat_dep_name.Visible = true;
            this.colpat_dep_name.VisibleIndex = 1;
            this.colpat_dep_name.Width = 60;
            // 
            // coldep_tel
            // 
            this.coldep_tel.Caption = "科室电话";
            this.coldep_tel.FieldName = "DeptTel";
            this.coldep_tel.Name = "coldep_tel";
            this.coldep_tel.OptionsColumn.AllowEdit = false;
            this.coldep_tel.Visible = true;
            this.coldep_tel.VisibleIndex = 2;
            // 
            // colpat_bed_no
            // 
            this.colpat_bed_no.Caption = "床号";
            this.colpat_bed_no.FieldName = "PidBedNo";
            this.colpat_bed_no.Name = "colpat_bed_no";
            this.colpat_bed_no.OptionsColumn.AllowEdit = false;
            this.colpat_bed_no.Visible = true;
            this.colpat_bed_no.VisibleIndex = 3;
            this.colpat_bed_no.Width = 52;
            // 
            // colpat_in_no
            // 
            this.colpat_in_no.Caption = "病人ID";
            this.colpat_in_no.FieldName = "PidInNo";
            this.colpat_in_no.Name = "colpat_in_no";
            this.colpat_in_no.OptionsColumn.AllowEdit = false;
            this.colpat_in_no.Visible = true;
            this.colpat_in_no.VisibleIndex = 4;
            this.colpat_in_no.Width = 67;
            // 
            // colpat_name
            // 
            this.colpat_name.Caption = "姓名";
            this.colpat_name.FieldName = "PidName";
            this.colpat_name.Name = "colpat_name";
            this.colpat_name.OptionsColumn.AllowEdit = false;
            this.colpat_name.Visible = true;
            this.colpat_name.VisibleIndex = 5;
            this.colpat_name.Width = 65;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "病人电话";
            this.gridColumn2.FieldName = "PidTel";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 6;
            this.gridColumn2.Width = 84;
            // 
            // colpat_sex_name
            // 
            this.colpat_sex_name.Caption = "性别";
            this.colpat_sex_name.FieldName = "PidSex";
            this.colpat_sex_name.Name = "colpat_sex_name";
            this.colpat_sex_name.OptionsColumn.AllowEdit = false;
            this.colpat_sex_name.Visible = true;
            this.colpat_sex_name.VisibleIndex = 7;
            this.colpat_sex_name.Width = 45;
            // 
            // colpat_age_exp
            // 
            this.colpat_age_exp.Caption = "年龄";
            this.colpat_age_exp.FieldName = "PidAgeStr";
            this.colpat_age_exp.Name = "colpat_age_exp";
            this.colpat_age_exp.OptionsColumn.AllowEdit = false;
            this.colpat_age_exp.Visible = true;
            this.colpat_age_exp.VisibleIndex = 8;
            this.colpat_age_exp.Width = 45;
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
            this.colpat_result.Width = 131;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.repositoryItemMemoEdit1.ReadOnly = true;
            // 
            // colpat_c_name
            // 
            this.colpat_c_name.Caption = "组合";
            this.colpat_c_name.FieldName = "PidComName";
            this.colpat_c_name.Name = "colpat_c_name";
            this.colpat_c_name.OptionsColumn.AllowEdit = false;
            this.colpat_c_name.Visible = true;
            this.colpat_c_name.VisibleIndex = 12;
            this.colpat_c_name.Width = 89;
            // 
            // colpat_chk_name
            // 
            this.colpat_chk_name.Caption = "审核人";
            this.colpat_chk_name.FieldName = "PidChkName";
            this.colpat_chk_name.Name = "colpat_chk_name";
            this.colpat_chk_name.OptionsColumn.AllowEdit = false;
            this.colpat_chk_name.Visible = true;
            this.colpat_chk_name.VisibleIndex = 13;
            this.colpat_chk_name.Width = 74;
            // 
            // colpat_chk_date
            // 
            this.colpat_chk_date.Caption = "审核时间";
            this.colpat_chk_date.DisplayFormat.FormatString = "yyyy-MM-dd hh:mm";
            this.colpat_chk_date.FieldName = "RepAuditDate";
            this.colpat_chk_date.Name = "colpat_chk_date";
            this.colpat_chk_date.OptionsColumn.AllowEdit = false;
            this.colpat_chk_date.Visible = true;
            this.colpat_chk_date.VisibleIndex = 14;
            this.colpat_chk_date.Width = 137;
            // 
            // colmsg_type
            // 
            this.colmsg_type.Caption = "危急值信息类型";
            this.colmsg_type.FieldName = "ObrType";
            this.colmsg_type.Name = "colmsg_type";
            this.colmsg_type.OptionsColumn.AllowEdit = false;
            this.colmsg_type.Width = 67;
            // 
            // colmsg_id
            // 
            this.colmsg_id.Caption = "信息ID";
            this.colmsg_id.FieldName = "ObrIdMsg";
            this.colmsg_id.Name = "colmsg_id";
            this.colmsg_id.Width = 61;
            // 
            // col_itr_name
            // 
            this.col_itr_name.Caption = "仪器";
            this.col_itr_name.FieldName = "ItrName";
            this.col_itr_name.Name = "col_itr_name";
            this.col_itr_name.Visible = true;
            this.col_itr_name.VisibleIndex = 15;
            this.col_itr_name.Width = 130;
            // 
            // col_pat_sid
            // 
            this.col_pat_sid.Caption = "样本号";
            this.col_pat_sid.FieldName = "RepSid";
            this.col_pat_sid.Name = "col_pat_sid";
            this.col_pat_sid.Visible = true;
            this.col_pat_sid.VisibleIndex = 16;
            this.col_pat_sid.Width = 72;
            // 
            // col_pat_host_order
            // 
            this.col_pat_host_order.Caption = "序号";
            this.col_pat_host_order.FieldName = "RepSerialNum";
            this.col_pat_host_order.Name = "col_pat_host_order";
            this.col_pat_host_order.Visible = true;
            this.col_pat_host_order.VisibleIndex = 17;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "开单医生";
            this.gridColumn1.FieldName = "DoctorName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 10;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "医生电话";
            this.gridColumn3.FieldName = "DoctorTel";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Info;
            this.panel2.ContextMenuStrip = this.contextMenuStrip1;
            this.panel2.Controls.Add(this.chkSendMsg);
            this.panel2.Controls.Add(this.BtnUndo);
            this.panel2.Controls.Add(this.BtnAudit);
            this.panel2.Controls.Add(this.BtnSAudit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 268);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(814, 40);
            this.panel2.TabIndex = 161;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 33);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "无",
            "主机发音",
            "音响发音"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            // 
            // chkSendMsg
            // 
            this.chkSendMsg.AutoSize = true;
            this.chkSendMsg.Location = new System.Drawing.Point(618, 10);
            this.chkSendMsg.Name = "chkSendMsg";
            this.chkSendMsg.Size = new System.Drawing.Size(168, 16);
            this.chkSendMsg.TabIndex = 3;
            this.chkSendMsg.Text = "是否向医生发送危急值短信";
            this.chkSendMsg.UseVisualStyleBackColor = true;
            this.chkSendMsg.Visible = false;
            // 
            // BtnUndo
            // 
            this.BtnUndo.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnUndo.Location = new System.Drawing.Point(236, 10);
            this.BtnUndo.Name = "BtnUndo";
            this.BtnUndo.Size = new System.Drawing.Size(96, 23);
            this.BtnUndo.TabIndex = 2;
            this.BtnUndo.Text = "放弃批量确认";
            this.BtnUndo.UseVisualStyleBackColor = false;
            this.BtnUndo.Click += new System.EventHandler(this.BtnUndo_Click);
            // 
            // BtnAudit
            // 
            this.BtnAudit.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnAudit.Location = new System.Drawing.Point(141, 10);
            this.BtnAudit.Name = "BtnAudit";
            this.BtnAudit.Size = new System.Drawing.Size(75, 23);
            this.BtnAudit.TabIndex = 1;
            this.BtnAudit.Text = "通知确认";
            this.BtnAudit.UseVisualStyleBackColor = false;
            this.BtnAudit.Click += new System.EventHandler(this.BtnAudit_Click);
            // 
            // BtnSAudit
            // 
            this.BtnSAudit.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BtnSAudit.Location = new System.Drawing.Point(26, 10);
            this.BtnSAudit.Name = "BtnSAudit";
            this.BtnSAudit.Size = new System.Drawing.Size(91, 23);
            this.BtnSAudit.TabIndex = 0;
            this.BtnSAudit.Text = "批量通知确认";
            this.BtnSAudit.UseVisualStyleBackColor = false;
            this.BtnSAudit.Click += new System.EventHandler(this.BtnSAudit_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblShowCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 340);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(820, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblShowCount
            // 
            this.lblShowCount.Name = "lblShowCount";
            this.lblShowCount.Size = new System.Drawing.Size(63, 17);
            this.lblShowCount.Text = "消息：0条";
            // 
            // lblShowCount2
            // 
            this.lblShowCount2.Name = "lblShowCount2";
            this.lblShowCount2.Size = new System.Drawing.Size(63, 17);
            this.lblShowCount2.Text = "消息：0条";
            // 
            // timer1
            // 
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.groupBox1.Size = new System.Drawing.Size(820, 335);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "危急值消息";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.BackgroundImage = global::dcl.client.notifyclient.Properties.Resources.stop;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(787, -2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(30, 30);
            this.button1.TabIndex = 1;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmUrgentNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 362);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUrgentNotify";
            this.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmUrgentNotify_FormClosing);
            this.DoubleClick += new System.EventHandler(this.FrmUrgentNotify_DoubleClick);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private DevExpress.XtraGrid.GridControl gcLookData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLookData;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_dep_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_bed_no;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_in_no;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_sex_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_age_exp;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_result;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_c_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_chk_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_chk_date;
        private DevExpress.XtraGrid.Columns.GridColumn colmsg_type;
        private DevExpress.XtraGrid.Columns.GridColumn colmsg_id;
        private System.Windows.Forms.ToolStripStatusLabel lblShowCount2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        public DevExpress.XtraGrid.Columns.GridColumn colpat_select;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private System.Windows.Forms.ToolStripStatusLabel lblShowCount;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnUndo;
        private System.Windows.Forms.Button BtnAudit;
        private System.Windows.Forms.Button BtnSAudit;
        private DevExpress.XtraGrid.Columns.GridColumn col_itr_name;
        private DevExpress.XtraGrid.Columns.GridColumn col_pat_sid;
        private DevExpress.XtraGrid.Columns.GridColumn col_pat_host_order;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.CheckBox chkSendMsg;
        private DevExpress.XtraGrid.Columns.GridColumn coldep_tel;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}

