namespace dcl.client.notifyclient
{
    partial class FrmBcSamplToReceiveTATNotify
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
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.gcLookData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcLookData.Location = new System.Drawing.Point(0, 0);
            this.gcLookData.MainView = this.gvLookData;
            this.gcLookData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gcLookData.Name = "gcLookData";
            this.gcLookData.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1,
            this.repositoryItemCheckEdit1});
            this.gcLookData.Size = new System.Drawing.Size(660, 285);
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
            this.gridColumn1,
            this.gridColumn2});
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
            // gridColumn1
            // 
            this.gridColumn1.Caption = "采集时间";
            this.gridColumn1.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn1.FieldName = "CollectionDate";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 3;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "超时(分钟)";
            this.gridColumn2.FieldName = "OverTat";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
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
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblShowCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 285);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(660, 29);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblShowCount
            // 
            this.lblShowCount.Name = "lblShowCount";
            this.lblShowCount.Size = new System.Drawing.Size(93, 24);
            this.lblShowCount.Text = "消息：0条";
            // 
            // timer1
            // 
            this.timer1.Interval = 30000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmBcSamplToReceiveTATNotify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 314);
            this.Controls.Add(this.gcLookData);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBcSamplToReceiveTATNotify";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TAT提示(采集-签收)";
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
        private DevExpress.XtraGrid.Columns.GridColumn colpat_name;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblShowCount;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.Columns.GridColumn col_barcode;
        private DevExpress.XtraGrid.Columns.GridColumn col_bc_com_name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}