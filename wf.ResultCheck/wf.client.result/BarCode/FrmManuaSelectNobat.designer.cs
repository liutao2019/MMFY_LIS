namespace dcl.client.result
{
    partial class FrmManuaSelectNobat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmManuaSelectNobat));
            DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
            this.barSave = new dcl.client.common.SysToolBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSort = new DevExpress.XtraEditors.TextEdit();
            this.labelControl42 = new DevExpress.XtraEditors.LabelControl();
            this.gdSysLog = new DevExpress.XtraGrid.GridControl();
            this.bsCombine = new System.Windows.Forms.BindingSource();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colEcd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCombine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // barSave
            // 
            this.barSave.AutoCloseButton = true;
            this.barSave.AutoEnableButtons = false;
            this.barSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barSave.Location = new System.Drawing.Point(0, 836);
            this.barSave.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.barSave.Name = "barSave";
            this.barSave.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("barSave.NotWriteLogButtonNameList")));
            this.barSave.QuickOption = false;
            this.barSave.ShowItemToolTips = false;
            this.barSave.Size = new System.Drawing.Size(1310, 131);
            this.barSave.TabIndex = 0;
            this.barSave.OnCloseClicked += new System.EventHandler(this.barSave_OnCloseClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSort);
            this.panel1.Controls.Add(this.labelControl42);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1310, 100);
            this.panel1.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(484, 29);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(223, 29);
            this.label9.TabIndex = 72;
            this.label9.Text = "(按回车键进行过滤)";
            // 
            // txtSort
            // 
            this.txtSort.EnterMoveNextControl = true;
            this.txtSort.Location = new System.Drawing.Point(200, 21);
            this.txtSort.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(271, 36);
            this.txtSort.TabIndex = 70;
            this.txtSort.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSort_KeyDown);
            // 
            // labelControl42
            // 
            this.labelControl42.Location = new System.Drawing.Point(83, 29);
            this.labelControl42.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.labelControl42.Name = "labelControl42";
            this.labelControl42.Size = new System.Drawing.Size(96, 29);
            this.labelControl42.TabIndex = 71;
            this.labelControl42.Text = "数据检索";
            // 
            // gdSysLog
            // 
            this.gdSysLog.DataSource = this.bsCombine;
            this.gdSysLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSysLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gdSysLog.Location = new System.Drawing.Point(0, 100);
            this.gdSysLog.MainView = this.gridView1;
            this.gdSysLog.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.gdSysLog.Name = "gdSysLog";
            this.gdSysLog.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gdSysLog.Size = new System.Drawing.Size(1310, 736);
            this.gdSysLog.TabIndex = 4;
            this.gdSysLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colEcd,
            this.gridColumn2});
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.Red;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
            styleFormatCondition1.Value1 = "未扣费";
            this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
            this.gridView1.GridControl = this.gdSysLog;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colEcd
            // 
            this.colEcd.Caption = "无菌涂片描述结果";
            this.colEcd.FieldName = "SmeName";
            this.colEcd.Name = "colEcd";
            this.colEcd.OptionsColumn.AllowEdit = false;
            this.colEcd.OptionsColumn.FixedWidth = true;
            this.colEcd.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "HisPrice", "{0}")});
            this.colEcd.Visible = true;
            this.colEcd.VisibleIndex = 1;
            this.colEcd.Width = 530;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = " ";
            this.gridColumn2.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn2.FieldName = "isselected";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 52;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.repositoryItemCheckEdit1.ValueChecked = 1;
            this.repositoryItemCheckEdit1.ValueUnchecked = 0;
            // 
            // FrmManuaSelectNobat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1310, 967);
            this.Controls.Add(this.gdSysLog);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.barSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmManuaSelectNobat";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择结果";
            this.Load += new System.EventHandler(this.FrmResultView_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSysLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCombine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private dcl.client.common.SysToolBar barSave;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gdSysLog;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colEcd;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.TextEdit txtSort;
        private DevExpress.XtraEditors.LabelControl labelControl42;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.BindingSource bsCombine;

    }
}