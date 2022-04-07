namespace dcl.client.oa
{
    partial class FrmHandOverInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHandOverInput));
            this.panel2 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridControlItr = new DevExpress.XtraGrid.GridControl();
            this.bsHo = new System.Windows.Forms.BindingSource(this.components);
            this.gvItr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colitr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhr_hand_time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhr_totalRecv_count = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhr_noqutity_count = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhr_report_count = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhr_unreport_count = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhr_hand_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colhr_recvconfirm_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlItr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsHo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItr)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sysToolBar1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1203, 81);
            this.panel2.TabIndex = 1;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1203, 81);
            this.sysToolBar1.TabIndex = 1;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.sysToolBar1_OnBtnAddClicked);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.sysToolBar1_OnBtnRefreshClicked);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 81);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1203, 760);
            this.panel3.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridControlItr);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1203, 760);
            this.splitContainer1.SplitterDistance = 235;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // gridControlItr
            // 
            this.gridControlItr.DataSource = this.bsHo;
            this.gridControlItr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlItr.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControlItr.Location = new System.Drawing.Point(0, 0);
            this.gridControlItr.MainView = this.gvItr;
            this.gridControlItr.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControlItr.Name = "gridControlItr";
            this.gridControlItr.Size = new System.Drawing.Size(1203, 760);
            this.gridControlItr.TabIndex = 48;
            this.gridControlItr.TabStop = false;
            this.gridControlItr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItr});
            // 
            // bsHo
            // 
            this.bsHo.DataSource = typeof(dcl.entity.EntityHoRecord);
            // 
            // gvItr
            // 
            this.gvItr.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colitr_id,
            this.colitr_name,
            this.colhr_hand_time,
            this.colhr_totalRecv_count,
            this.colhr_noqutity_count,
            this.colhr_report_count,
            this.colhr_unreport_count,
            this.colhr_hand_name,
            this.colhr_recvconfirm_name});
            this.gvItr.GridControl = this.gridControlItr;
            this.gvItr.Name = "gvItr";
            this.gvItr.OptionsBehavior.Editable = false;
            this.gvItr.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvItr.OptionsView.ColumnAutoWidth = false;
            this.gvItr.OptionsView.ShowGroupPanel = false;
            this.gvItr.DoubleClick += new System.EventHandler(this.gvItr_DoubleClick);
            // 
            // colitr_id
            // 
            this.colitr_id.Caption = "实验组编码";
            this.colitr_id.FieldName = "HrTypeId";
            this.colitr_id.Name = "colitr_id";
            this.colitr_id.Visible = true;
            this.colitr_id.VisibleIndex = 0;
            this.colitr_id.Width = 135;
            // 
            // colitr_name
            // 
            this.colitr_name.Caption = "实验组名称";
            this.colitr_name.FieldName = "HrTypeName";
            this.colitr_name.Name = "colitr_name";
            this.colitr_name.Visible = true;
            this.colitr_name.VisibleIndex = 1;
            this.colitr_name.Width = 126;
            // 
            // colhr_hand_time
            // 
            this.colhr_hand_time.Caption = "交班时间";
            this.colhr_hand_time.FieldName = "HrHandTime";
            this.colhr_hand_time.Name = "colhr_hand_time";
            this.colhr_hand_time.Visible = true;
            this.colhr_hand_time.VisibleIndex = 2;
            this.colhr_hand_time.Width = 149;
            // 
            // colhr_totalRecv_count
            // 
            this.colhr_totalRecv_count.Caption = "总签收标本";
            this.colhr_totalRecv_count.FieldName = "HrTotalRecvCount";
            this.colhr_totalRecv_count.Name = "colhr_totalRecv_count";
            this.colhr_totalRecv_count.Visible = true;
            this.colhr_totalRecv_count.VisibleIndex = 5;
            // 
            // colhr_noqutity_count
            // 
            this.colhr_noqutity_count.Caption = "无效标本";
            this.colhr_noqutity_count.FieldName = "HrNoqutityCount";
            this.colhr_noqutity_count.Name = "colhr_noqutity_count";
            this.colhr_noqutity_count.Visible = true;
            this.colhr_noqutity_count.VisibleIndex = 6;
            this.colhr_noqutity_count.Width = 82;
            // 
            // colhr_report_count
            // 
            this.colhr_report_count.Caption = "发出报告数";
            this.colhr_report_count.FieldName = "HrReportCount";
            this.colhr_report_count.Name = "colhr_report_count";
            this.colhr_report_count.Visible = true;
            this.colhr_report_count.VisibleIndex = 7;
            this.colhr_report_count.Width = 84;
            // 
            // colhr_unreport_count
            // 
            this.colhr_unreport_count.Caption = "延时检验数";
            this.colhr_unreport_count.FieldName = "HrUnreportCount";
            this.colhr_unreport_count.Name = "colhr_unreport_count";
            this.colhr_unreport_count.Visible = true;
            this.colhr_unreport_count.VisibleIndex = 8;
            this.colhr_unreport_count.Width = 86;
            // 
            // colhr_hand_name
            // 
            this.colhr_hand_name.Caption = "交班人";
            this.colhr_hand_name.FieldName = "HrHandName";
            this.colhr_hand_name.Name = "colhr_hand_name";
            this.colhr_hand_name.Visible = true;
            this.colhr_hand_name.VisibleIndex = 3;
            // 
            // colhr_recvconfirm_name
            // 
            this.colhr_recvconfirm_name.Caption = "接班确认人";
            this.colhr_recvconfirm_name.FieldName = "HrRecvConFirmName";
            this.colhr_recvconfirm_name.Name = "colhr_recvconfirm_name";
            this.colhr_recvconfirm_name.Visible = true;
            this.colhr_recvconfirm_name.VisibleIndex = 4;
            // 
            // FrmHandOverInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 841);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmHandOverInput";
            this.Text = "交班管理";
            this.Load += new System.EventHandler(this.FrmHandOverMgr_Load);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlItr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsHo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraGrid.GridControl gridControlItr;
        private DevExpress.XtraGrid.Views.Grid.GridView gvItr;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_id;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_name;
        private System.Windows.Forms.BindingSource bsHo;
        private DevExpress.XtraGrid.Columns.GridColumn colhr_hand_time;
        private DevExpress.XtraGrid.Columns.GridColumn colhr_totalRecv_count;
        private DevExpress.XtraGrid.Columns.GridColumn colhr_noqutity_count;
        private DevExpress.XtraGrid.Columns.GridColumn colhr_report_count;
        private DevExpress.XtraGrid.Columns.GridColumn colhr_unreport_count;
        private DevExpress.XtraGrid.Columns.GridColumn colhr_hand_name;
        private DevExpress.XtraGrid.Columns.GridColumn colhr_recvconfirm_name;
    }
}