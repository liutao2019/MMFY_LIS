namespace dcl.client.result.PatControl
{
    partial class frmItemModifyHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemModifyHistory));
            this.gdItem = new DevExpress.XtraGrid.GridControl();
            this.bsOperationLog = new System.Windows.Forms.BindingSource();
            this.gvItem = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gdItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperationLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gdItem
            // 
            this.gdItem.DataSource = this.bsOperationLog;
            this.gdItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdItem.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gdItem.Location = new System.Drawing.Point(0, 87);
            this.gdItem.MainView = this.gvItem;
            this.gdItem.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gdItem.Name = "gdItem";
            this.gdItem.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.repositoryItemMemoEdit1});
            this.gdItem.Size = new System.Drawing.Size(1029, 536);
            this.gdItem.TabIndex = 60;
            this.gdItem.TabStop = false;
            this.gdItem.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItem});
            // 
            // bsOperationLog
            // 
            this.bsOperationLog.DataSource = typeof(dcl.entity.EntitySysOperationLog);
            // 
            // gvItem
            // 
            this.gvItem.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9});
            this.gvItem.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvItem.GridControl = this.gdItem;
            this.gvItem.Name = "gvItem";
            this.gvItem.OptionsBehavior.Editable = false;
            this.gvItem.OptionsFilter.ShowAllTableValuesInFilterPopup = true;
            this.gvItem.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvItem.OptionsView.ColumnAutoWidth = false;
            this.gvItem.OptionsView.ShowGroupPanel = false;
            this.gvItem.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "操作时间";
            this.gridColumn1.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn1.FieldName = "OperatDate";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 132;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "项目ID";
            this.gridColumn2.FieldName = "OperatKey";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Width = 57;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "项目编码";
            this.gridColumn3.FieldName = "OperatObject";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 69;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "操作人ID";
            this.gridColumn4.FieldName = "OperatUserId";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 71;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "操作人";
            this.gridColumn5.FieldName = "OperatUserName";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Width = 67;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "操作类型";
            this.gridColumn6.FieldName = "OperatAction";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Width = 57;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "操作内容";
            this.gridColumn7.FieldName = "OperatGroup";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Width = 93;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "操作内容";
            this.gridColumn8.FieldName = "OperatObject";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Width = 128;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "修改内容";
            this.gridColumn9.ColumnEdit = this.repositoryItemMemoEdit1;
            this.gridColumn9.FieldName = "OperatContent";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 3;
            this.gridColumn9.Width = 199;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.repositoryItemMemoEdit1.WordWrap = false;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1029, 87);
            this.panel1.TabIndex = 61;
            this.panel1.Visible = false;
            // 
            // frmItemModifyHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 623);
            this.Controls.Add(this.gdItem);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmItemModifyHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "项目结果修改日志";
            this.Load += new System.EventHandler(this.frmItemModifyHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsOperationLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gdItem;
        private DevExpress.XtraGrid.Views.Grid.GridView gvItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private System.Windows.Forms.BindingSource bsOperationLog;
    }
}