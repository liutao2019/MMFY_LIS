namespace dcl.client.notifyclient
{
    partial class FrmComTATNotify
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
            this.gcLookData = new DevExpress.XtraGrid.GridControl();
            this.gvLookData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_barcode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_bc_com_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_type_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itr_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_host_order = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colpat_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblShowCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
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
            this.gcLookData.Size = new System.Drawing.Size(440, 187);
            this.gcLookData.TabIndex = 9;
            this.gcLookData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvLookData});
            // 
            // gvLookData
            // 
            this.gvLookData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_barcode,
            this.colpat_name,
            this.col_bc_com_name,
            this.colitr_type_name,
            this.colpat_date,
            this.col_itr_name,
            this.col_pat_sid,
            this.col_pat_host_order,
            this.colpat_id});
            this.gvLookData.GridControl = this.gcLookData;
            this.gvLookData.Name = "gvLookData";
            this.gvLookData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvLookData.OptionsView.ColumnAutoWidth = false;
            this.gvLookData.OptionsView.RowAutoHeight = true;
            this.gvLookData.OptionsView.ShowGroupPanel = false;
            this.gvLookData.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gvLookData_RowStyle);
            // 
            // col_barcode
            // 
            this.col_barcode.Caption = "条码";
            this.col_barcode.FieldName = "SampBarCode";
            this.col_barcode.Name = "col_barcode";
            this.col_barcode.OptionsColumn.ReadOnly = true;
            this.col_barcode.Visible = true;
            this.col_barcode.VisibleIndex = 0;
            this.col_barcode.Width = 100;
            // 
            // colpat_name
            // 
            this.colpat_name.Caption = "姓名";
            this.colpat_name.FieldName = "PidName";
            this.colpat_name.Name = "colpat_name";
            this.colpat_name.OptionsColumn.ReadOnly = true;
            this.colpat_name.Visible = true;
            this.colpat_name.VisibleIndex = 1;
            this.colpat_name.Width = 82;
            // 
            // col_bc_com_name
            // 
            this.col_bc_com_name.Caption = "组合";
            this.col_bc_com_name.FieldName = "BcComName";
            this.col_bc_com_name.Name = "col_bc_com_name";
            this.col_bc_com_name.OptionsColumn.ReadOnly = true;
            this.col_bc_com_name.Visible = true;
            this.col_bc_com_name.VisibleIndex = 2;
            this.col_bc_com_name.Width = 105;
            // 
            // colitr_type_name
            // 
            this.colitr_type_name.Caption = "物理组";
            this.colitr_type_name.FieldName = "ProName";
            this.colitr_type_name.Name = "colitr_type_name";
            this.colitr_type_name.OptionsColumn.ReadOnly = true;
            this.colitr_type_name.Visible = true;
            this.colitr_type_name.VisibleIndex = 3;
            this.colitr_type_name.Width = 100;
            // 
            // colpat_date
            // 
            this.colpat_date.Caption = "日期";
            this.colpat_date.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.colpat_date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colpat_date.FieldName = "RepInDate";
            this.colpat_date.Name = "colpat_date";
            this.colpat_date.OptionsColumn.AllowEdit = false;
            this.colpat_date.Visible = true;
            this.colpat_date.VisibleIndex = 4;
            this.colpat_date.Width = 96;
            // 
            // col_itr_name
            // 
            this.col_itr_name.Caption = "仪器";
            this.col_itr_name.FieldName = "ItrEname";
            this.col_itr_name.Name = "col_itr_name";
            this.col_itr_name.OptionsColumn.AllowEdit = false;
            this.col_itr_name.Visible = true;
            this.col_itr_name.VisibleIndex = 5;
            this.col_itr_name.Width = 98;
            // 
            // col_pat_sid
            // 
            this.col_pat_sid.Caption = "样本号";
            this.col_pat_sid.FieldName = "RepSid";
            this.col_pat_sid.Name = "col_pat_sid";
            this.col_pat_sid.OptionsColumn.ReadOnly = true;
            this.col_pat_sid.Visible = true;
            this.col_pat_sid.VisibleIndex = 6;
            this.col_pat_sid.Width = 95;
            // 
            // col_pat_host_order
            // 
            this.col_pat_host_order.Caption = "序号";
            this.col_pat_host_order.FieldName = "RepSerialNum";
            this.col_pat_host_order.Name = "col_pat_host_order";
            this.col_pat_host_order.OptionsColumn.ReadOnly = true;
            this.col_pat_host_order.Visible = true;
            this.col_pat_host_order.VisibleIndex = 7;
            this.col_pat_host_order.Width = 68;
            // 
            // colpat_id
            // 
            this.colpat_id.Caption = "信息ID";
            this.colpat_id.FieldName = "RepId";
            this.colpat_id.Name = "colpat_id";
            this.colpat_id.Width = 61;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.repositoryItemMemoEdit1.ReadOnly = true;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblShowCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 187);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(440, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblShowCount
            // 
            this.lblShowCount.Name = "lblShowCount";
            this.lblShowCount.Size = new System.Drawing.Size(63, 17);
            this.lblShowCount.Text = "消息：0条";
            // 
            // timer1
            // 
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmComTATNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 209);
            this.Controls.Add(this.gcLookData);
            this.Controls.Add(this.statusStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmComTATNotify";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "组合TAT提示";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmComTATNotify_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gcLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvLookData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcLookData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvLookData;
        private DevExpress.XtraGrid.Columns.GridColumn col_itr_name;
        private DevExpress.XtraGrid.Columns.GridColumn col_pat_sid;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_date;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_name;
        private DevExpress.XtraGrid.Columns.GridColumn col_pat_host_order;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_type_name;
        private DevExpress.XtraGrid.Columns.GridColumn colpat_id;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblShowCount;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.Columns.GridColumn col_barcode;
        private DevExpress.XtraGrid.Columns.GridColumn col_bc_com_name;
    }
}