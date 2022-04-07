namespace dcl.client.notifyclient
{
    partial class FrmQCNotify
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
            this.gcQcData = new DevExpress.XtraGrid.GridControl();
            this.gvQcData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_itr_mid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_itm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_qcm_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colqrm_start_time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_type_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblShowCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gcQcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcQcData
            // 
            this.gcQcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcQcData.Location = new System.Drawing.Point(0, 0);
            this.gcQcData.MainView = this.gvQcData;
            this.gcQcData.Name = "gcQcData";
            this.gcQcData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemCheckEdit1});
            this.gcQcData.Size = new System.Drawing.Size(440, 187);
            this.gcQcData.TabIndex = 9;
            this.gcQcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvQcData});
            // 
            // gvQcData
            // 
            this.gvQcData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_itr_mid,
            this.col_itm_ecd,
            this.col_qcm_type,
            this.colqrm_start_time,
            this.colitr_type_name});
            this.gvQcData.GridControl = this.gcQcData;
            this.gvQcData.Name = "gvQcData";
            this.gvQcData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvQcData.OptionsView.ColumnAutoWidth = false;
            this.gvQcData.OptionsView.RowAutoHeight = true;
            this.gvQcData.OptionsView.ShowGroupPanel = false;
            // 
            // col_itr_mid
            // 
            this.col_itr_mid.Caption = "仪器";
            this.col_itr_mid.FieldName = "ItrEname";
            this.col_itr_mid.Name = "col_itr_mid";
            this.col_itr_mid.OptionsColumn.AllowEdit = false;
            this.col_itr_mid.Visible = true;
            this.col_itr_mid.VisibleIndex = 0;
            this.col_itr_mid.Width = 98;
            // 
            // col_itm_ecd
            // 
            this.col_itm_ecd.Caption = "项目";
            this.col_itm_ecd.FieldName = "ItmEcode";
            this.col_itm_ecd.Name = "col_itm_ecd";
            this.col_itm_ecd.OptionsColumn.AllowEdit = false;
            this.col_itm_ecd.Visible = true;
            this.col_itm_ecd.VisibleIndex = 1;
            this.col_itm_ecd.Width = 85;
            // 
            // col_qcm_type
            // 
            this.col_qcm_type.Caption = "类型";
            this.col_qcm_type.FieldName = "QcmType";
            this.col_qcm_type.Name = "col_qcm_type";
            this.col_qcm_type.OptionsColumn.AllowEdit = false;
            this.col_qcm_type.Visible = true;
            this.col_qcm_type.VisibleIndex = 2;
            // 
            // colqrm_start_time
            // 
            this.colqrm_start_time.Caption = "日期";
            this.colqrm_start_time.DisplayFormat.FormatString = "yyyy-MM-dd";
            this.colqrm_start_time.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colqrm_start_time.FieldName = "QrmStartTime";
            this.colqrm_start_time.Name = "colqrm_start_time";
            this.colqrm_start_time.OptionsColumn.AllowEdit = false;
            this.colqrm_start_time.Visible = true;
            this.colqrm_start_time.VisibleIndex = 3;
            this.colqrm_start_time.Width = 96;
            // 
            // colitr_type_name
            // 
            this.colitr_type_name.Caption = "物理组";
            this.colitr_type_name.FieldName = "ProName";
            this.colitr_type_name.Name = "colitr_type_name";
            this.colitr_type_name.OptionsColumn.AllowEdit = false;
            this.colitr_type_name.Visible = true;
            this.colitr_type_name.VisibleIndex = 4;
            this.colitr_type_name.Width = 52;
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
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmQCNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 209);
            this.Controls.Add(this.gcQcData);
            this.Controls.Add(this.statusStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmQCNotify";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "仪器质控提示";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmQCNotify_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gcQcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gcQcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gvQcData;
        private DevExpress.XtraGrid.Columns.GridColumn col_itr_mid;
        private DevExpress.XtraGrid.Columns.GridColumn col_itm_ecd;
        private DevExpress.XtraGrid.Columns.GridColumn colqrm_start_time;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_type_name;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblShowCount;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.Columns.GridColumn col_qcm_type;
    }
}