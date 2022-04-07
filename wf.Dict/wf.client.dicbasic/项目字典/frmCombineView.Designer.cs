namespace dcl.client.dicbasic
{
    partial class frmCombineView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCombineView));
            this.gridControlSingle = new DevExpress.XtraGrid.GridControl();
            this.bsCombine = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewSingle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colComID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHisName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colHisCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPrintName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSplitCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTubName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSamName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSplitSeq = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBloodNotice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaveNotice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOriName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRepTimes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemGridLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.leditResType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtNameFilter = new System.Windows.Forms.TextBox();
            this.txtCodeFilter = new System.Windows.Forms.TextBox();
            this.txtSplit_codeFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.txtCType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSingle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCombine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSingle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leditResType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControlSingle
            // 
            this.gridControlSingle.DataSource = this.bsCombine;
            this.gridControlSingle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlSingle.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControlSingle.Location = new System.Drawing.Point(6, 35);
            this.gridControlSingle.MainView = this.gridViewSingle;
            this.gridControlSingle.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gridControlSingle.Name = "gridControlSingle";
            this.gridControlSingle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemTextEdit4,
            this.repositoryItemGridLookUpEdit1,
            this.repositoryItemLookUpEdit2,
            this.leditResType,
            this.repositoryItemTextEdit3});
            this.gridControlSingle.Size = new System.Drawing.Size(1962, 1233);
            this.gridControlSingle.TabIndex = 3;
            this.gridControlSingle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSingle});
            // 
            // bsCombine
            // 
            this.bsCombine.DataSource = typeof(dcl.entity.EntitySampMergeRule);
            // 
            // gridViewSingle
            // 
            this.gridViewSingle.ColumnPanelRowHeight = 40;
            this.gridViewSingle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colComID,
            this.colComName,
            this.colHisName,
            this.colHisCode,
            this.colPrintName,
            this.colSplitCode,
            this.colTypeName,
            this.colTubName,
            this.colSamName,
            this.colSplitSeq,
            this.colBloodNotice,
            this.colSaveNotice,
            this.colOriName,
            this.colRepTimes});
            this.gridViewSingle.CustomizationFormBounds = new System.Drawing.Rectangle(375, 197, 208, 308);
            this.gridViewSingle.GridControl = this.gridControlSingle;
            this.gridViewSingle.Name = "gridViewSingle";
            this.gridViewSingle.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridViewSingle.OptionsNavigation.AutoFocusNewRow = true;
            this.gridViewSingle.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridViewSingle.OptionsView.ShowGroupPanel = false;
            this.gridViewSingle.RowHeight = 20;
            // 
            // colComID
            // 
            this.colComID.Caption = "组合ID";
            this.colComID.FieldName = "ComId";
            this.colComID.Name = "colComID";
            this.colComID.OptionsColumn.AllowEdit = false;
            this.colComID.Visible = true;
            this.colComID.VisibleIndex = 0;
            this.colComID.Width = 74;
            // 
            // colComName
            // 
            this.colComName.Caption = "组合名称";
            this.colComName.FieldName = "ComName";
            this.colComName.Name = "colComName";
            this.colComName.OptionsColumn.AllowEdit = false;
            this.colComName.Visible = true;
            this.colComName.VisibleIndex = 1;
            this.colComName.Width = 74;
            // 
            // colHisName
            // 
            this.colHisName.Caption = "收费名称";
            this.colHisName.FieldName = "ComHisName";
            this.colHisName.Name = "colHisName";
            this.colHisName.OptionsColumn.AllowEdit = false;
            this.colHisName.Visible = true;
            this.colHisName.VisibleIndex = 2;
            this.colHisName.Width = 74;
            // 
            // colHisCode
            // 
            this.colHisCode.Caption = "收费代码";
            this.colHisCode.FieldName = "ComHisFeeCode";
            this.colHisCode.Name = "colHisCode";
            this.colHisCode.OptionsColumn.AllowEdit = false;
            this.colHisCode.Visible = true;
            this.colHisCode.VisibleIndex = 3;
            this.colHisCode.Width = 74;
            // 
            // colPrintName
            // 
            this.colPrintName.Caption = "打印名称";
            this.colPrintName.FieldName = "ComPrintName";
            this.colPrintName.Name = "colPrintName";
            this.colPrintName.OptionsColumn.AllowEdit = false;
            this.colPrintName.Visible = true;
            this.colPrintName.VisibleIndex = 4;
            this.colPrintName.Width = 74;
            // 
            // colSplitCode
            // 
            this.colSplitCode.Caption = "拆分码";
            this.colSplitCode.FieldName = "ComSplitCode";
            this.colSplitCode.Name = "colSplitCode";
            this.colSplitCode.Visible = true;
            this.colSplitCode.VisibleIndex = 5;
            this.colSplitCode.Width = 120;
            // 
            // colTypeName
            // 
            this.colTypeName.Caption = "物理组别";
            this.colTypeName.FieldName = "ProName";
            this.colTypeName.Name = "colTypeName";
            this.colTypeName.OptionsColumn.AllowEdit = false;
            this.colTypeName.Visible = true;
            this.colTypeName.VisibleIndex = 6;
            this.colTypeName.Width = 68;
            // 
            // colTubName
            // 
            this.colTubName.Caption = "试管";
            this.colTubName.FieldName = "TubName";
            this.colTubName.Name = "colTubName";
            this.colTubName.OptionsColumn.AllowEdit = false;
            this.colTubName.Visible = true;
            this.colTubName.VisibleIndex = 7;
            this.colTubName.Width = 68;
            // 
            // colSamName
            // 
            this.colSamName.Caption = "标本类别";
            this.colSamName.FieldName = "SamName";
            this.colSamName.Name = "colSamName";
            this.colSamName.OptionsColumn.AllowEdit = false;
            this.colSamName.Visible = true;
            this.colSamName.VisibleIndex = 8;
            this.colSamName.Width = 68;
            // 
            // colSplitSeq
            // 
            this.colSplitSeq.Caption = "序号";
            this.colSplitSeq.FieldName = "ComSortNo";
            this.colSplitSeq.Name = "colSplitSeq";
            this.colSplitSeq.OptionsColumn.AllowEdit = false;
            this.colSplitSeq.Visible = true;
            this.colSplitSeq.VisibleIndex = 9;
            this.colSplitSeq.Width = 68;
            // 
            // colBloodNotice
            // 
            this.colBloodNotice.Caption = "采样注意事项";
            this.colBloodNotice.FieldName = "ComSamNotice";
            this.colBloodNotice.Name = "colBloodNotice";
            this.colBloodNotice.OptionsColumn.AllowEdit = false;
            this.colBloodNotice.Visible = true;
            this.colBloodNotice.VisibleIndex = 10;
            this.colBloodNotice.Width = 68;
            // 
            // colSaveNotice
            // 
            this.colSaveNotice.Caption = "保存注意事项";
            this.colSaveNotice.FieldName = "ComSaveNotice";
            this.colSaveNotice.Name = "colSaveNotice";
            this.colSaveNotice.OptionsColumn.AllowEdit = false;
            this.colSaveNotice.Visible = true;
            this.colSaveNotice.VisibleIndex = 11;
            this.colSaveNotice.Width = 68;
            // 
            // colOriName
            // 
            this.colOriName.Caption = "组合来源";
            this.colOriName.FieldName = "SrcName";
            this.colOriName.Name = "colOriName";
            this.colOriName.OptionsColumn.AllowEdit = false;
            this.colOriName.Visible = true;
            this.colOriName.VisibleIndex = 12;
            this.colOriName.Width = 68;
            // 
            // colRepTimes
            // 
            this.colRepTimes.Caption = "出报告时间";
            this.colRepTimes.FieldName = "ComRepTimes";
            this.colRepTimes.Name = "colRepTimes";
            this.colRepTimes.OptionsColumn.AllowEdit = false;
            this.colRepTimes.Visible = true;
            this.colRepTimes.VisibleIndex = 13;
            this.colRepTimes.Width = 73;
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // repositoryItemTextEdit4
            // 
            this.repositoryItemTextEdit4.AutoHeight = false;
            this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
            // 
            // repositoryItemGridLookUpEdit1
            // 
            this.repositoryItemGridLookUpEdit1.AutoHeight = false;
            this.repositoryItemGridLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemGridLookUpEdit1.Name = "repositoryItemGridLookUpEdit1";
            this.repositoryItemGridLookUpEdit1.View = this.gridView1;
            // 
            // gridView1
            // 
            this.gridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.DisplayMember = "value";
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.NullText = "";
            this.repositoryItemLookUpEdit2.ValueMember = "id";
            // 
            // leditResType
            // 
            this.leditResType.AutoHeight = false;
            this.leditResType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.leditResType.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("value", "value")});
            this.leditResType.DisplayMember = "name";
            this.leditResType.Name = "leditResType";
            this.leditResType.ValueMember = "value";
            // 
            // repositoryItemTextEdit3
            // 
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gridControlSingle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 85);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Size = new System.Drawing.Size(1974, 1274);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // txtNameFilter
            // 
            this.txtNameFilter.Location = new System.Drawing.Point(247, 17);
            this.txtNameFilter.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtNameFilter.Name = "txtNameFilter";
            this.txtNameFilter.Size = new System.Drawing.Size(234, 36);
            this.txtNameFilter.TabIndex = 0;
            // 
            // txtCodeFilter
            // 
            this.txtCodeFilter.Location = new System.Drawing.Point(602, 17);
            this.txtCodeFilter.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCodeFilter.Name = "txtCodeFilter";
            this.txtCodeFilter.Size = new System.Drawing.Size(234, 36);
            this.txtCodeFilter.TabIndex = 1;
            // 
            // txtSplit_codeFilter
            // 
            this.txtSplit_codeFilter.Location = new System.Drawing.Point(949, 17);
            this.txtSplit_codeFilter.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSplit_codeFilter.Name = "txtSplit_codeFilter";
            this.txtSplit_codeFilter.Size = new System.Drawing.Size(329, 36);
            this.txtSplit_codeFilter.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(498, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 29);
            this.label2.TabIndex = 7;
            this.label2.Text = "HIS代码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(858, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 29);
            this.label3.TabIndex = 7;
            this.label3.Text = "合并码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 29);
            this.label1.TabIndex = 9;
            this.label1.Text = "收费名称/组合名称";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.txtCType);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSplit_codeFilter);
            this.panel1.Controls.Add(this.txtCodeFilter);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtNameFilter);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1974, 85);
            this.panel1.TabIndex = 9;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(1696, 14);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(139, 56);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "导出";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtCType
            // 
            this.txtCType.Location = new System.Drawing.Point(1400, 19);
            this.txtCType.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtCType.Name = "txtCType";
            this.txtCType.Size = new System.Drawing.Size(255, 36);
            this.txtCType.TabIndex = 10;
            this.txtCType.TextChanged += new System.EventHandler(this.txtCType_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(1306, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 29);
            this.label4.TabIndex = 11;
            this.label4.Text = "物理组";
            // 
            // frmCombineView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1974, 1359);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "frmCombineView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "组合信息";
            this.Load += new System.EventHandler(this.frmCombineView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSingle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCombine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSingle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leditResType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlSingle;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewSingle;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit leditResType;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private System.Windows.Forms.BindingSource bsCombine;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtNameFilter;
        private System.Windows.Forms.TextBox txtCodeFilter;
        private System.Windows.Forms.TextBox txtSplit_codeFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtCType;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraGrid.Columns.GridColumn colComID;
        private DevExpress.XtraGrid.Columns.GridColumn colComName;
        private DevExpress.XtraGrid.Columns.GridColumn colHisName;
        private DevExpress.XtraGrid.Columns.GridColumn colHisCode;
        private DevExpress.XtraGrid.Columns.GridColumn colPrintName;
        private DevExpress.XtraGrid.Columns.GridColumn colSplitCode;
        private DevExpress.XtraGrid.Columns.GridColumn colTypeName;
        private DevExpress.XtraGrid.Columns.GridColumn colTubName;
        private DevExpress.XtraGrid.Columns.GridColumn colSamName;
        private DevExpress.XtraGrid.Columns.GridColumn colSplitSeq;
        private DevExpress.XtraGrid.Columns.GridColumn colBloodNotice;
        private DevExpress.XtraGrid.Columns.GridColumn colSaveNotice;
        private DevExpress.XtraGrid.Columns.GridColumn colOriName;
        private DevExpress.XtraGrid.Columns.GridColumn colRepTimes;
    }
}