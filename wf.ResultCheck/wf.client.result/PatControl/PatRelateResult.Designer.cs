namespace dcl.client.result.PatControl
{
    partial class PatRelateResult
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatRelateResult));
            this.gridControlRelateResult = new DevExpress.XtraGrid.GridControl();
            this.gridViewRelateResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_instrument = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_comname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_ecd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colres_chr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colres_ref_range = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colres_ref_flag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colres_date = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colres_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.pnlDate = new DevExpress.XtraEditors.PanelControl();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.deEnd = new DevExpress.XtraEditors.DateEdit();
            this.deBegin = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRelateResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRelateResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlDate)).BeginInit();
            this.pnlDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deBegin.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlRelateResult
            // 
            this.gridControlRelateResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlRelateResult.Location = new System.Drawing.Point(0, 45);
            this.gridControlRelateResult.MainView = this.gridViewRelateResult;
            this.gridControlRelateResult.Name = "gridControlRelateResult";
            this.gridControlRelateResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemDateEdit1,
            this.repositoryItemTextEdit4});
            this.gridControlRelateResult.Size = new System.Drawing.Size(842, 588);
            this.gridControlRelateResult.TabIndex = 3;
            this.gridControlRelateResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewRelateResult});
            // 
            // gridViewRelateResult
            // 
            this.gridViewRelateResult.ColumnPanelRowHeight = 26;
            this.gridViewRelateResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_instrument,
            this.col_comname,
            this.colitm_name,
            this.colitm_ecd,
            this.colres_chr,
            this.colres_ref_range,
            this.colres_ref_flag,
            this.colres_date,
            this.colres_sid});
            this.gridViewRelateResult.CustomizationFormBounds = new System.Drawing.Rectangle(375, 197, 208, 308);
            this.gridViewRelateResult.GridControl = this.gridControlRelateResult;
            this.gridViewRelateResult.Name = "gridViewRelateResult";
            this.gridViewRelateResult.OptionsBehavior.AllowIncrementalSearch = true;
            this.gridViewRelateResult.OptionsBehavior.AutoPopulateColumns = false;
            this.gridViewRelateResult.OptionsNavigation.AutoFocusNewRow = true;
            this.gridViewRelateResult.OptionsNavigation.EnterMoveNextColumn = true;
            this.gridViewRelateResult.OptionsView.ColumnAutoWidth = false;
            this.gridViewRelateResult.OptionsView.ShowGroupPanel = false;
            this.gridViewRelateResult.RowHeight = 25;
            this.gridViewRelateResult.CustomDrawGroupRow += new DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventHandler(this.gridViewRelateResult_CustomDrawGroupRow);
            this.gridViewRelateResult.CustomDrawEmptyForeground += new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler(this.gridViewRelateResult_CustomDrawEmptyForeground);
            // 
            // col_instrument
            // 
            this.col_instrument.Caption = "仪器";
            this.col_instrument.FieldName = "ObrSourceItrName";
            this.col_instrument.Name = "col_instrument";
            this.col_instrument.OptionsColumn.AllowEdit = false;
            this.col_instrument.OptionsColumn.AllowFocus = false;
            this.col_instrument.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.col_instrument.OptionsFilter.AllowAutoFilter = false;
            this.col_instrument.OptionsFilter.AllowFilter = false;
            this.col_instrument.Visible = true;
            this.col_instrument.VisibleIndex = 1;
            this.col_instrument.Width = 100;
            // 
            // col_comname
            // 
            this.col_comname.Caption = "组合";
            this.col_comname.FieldName = "ComName";
            this.col_comname.Name = "col_comname";
            this.col_comname.OptionsColumn.AllowEdit = false;
            this.col_comname.OptionsColumn.AllowFocus = false;
            this.col_comname.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.col_comname.OptionsFilter.AllowAutoFilter = false;
            this.col_comname.OptionsFilter.AllowFilter = false;
            this.col_comname.Visible = true;
            this.col_comname.VisibleIndex = 2;
            this.col_comname.Width = 142;
            // 
            // colitm_name
            // 
            this.colitm_name.Caption = "名称";
            this.colitm_name.FieldName = "ItmName";
            this.colitm_name.Name = "colitm_name";
            this.colitm_name.Visible = true;
            this.colitm_name.VisibleIndex = 3;
            this.colitm_name.Width = 161;
            // 
            // colitm_ecd
            // 
            this.colitm_ecd.Caption = "项目";
            this.colitm_ecd.FieldName = "ItmEname";
            this.colitm_ecd.Name = "colitm_ecd";
            this.colitm_ecd.OptionsColumn.AllowEdit = false;
            this.colitm_ecd.OptionsColumn.AllowFocus = false;
            this.colitm_ecd.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colitm_ecd.OptionsFilter.AllowAutoFilter = false;
            this.colitm_ecd.OptionsFilter.AllowFilter = false;
            this.colitm_ecd.Visible = true;
            this.colitm_ecd.VisibleIndex = 4;
            this.colitm_ecd.Width = 112;
            // 
            // colres_chr
            // 
            this.colres_chr.AppearanceCell.Options.UseTextOptions = true;
            this.colres_chr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colres_chr.Caption = "结果";
            this.colres_chr.FieldName = "ObrValue";
            this.colres_chr.Name = "colres_chr";
            this.colres_chr.OptionsColumn.AllowEdit = false;
            this.colres_chr.OptionsColumn.AllowFocus = false;
            this.colres_chr.OptionsColumn.AllowIncrementalSearch = false;
            this.colres_chr.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colres_chr.OptionsFilter.AllowAutoFilter = false;
            this.colres_chr.OptionsFilter.AllowFilter = false;
            this.colres_chr.Visible = true;
            this.colres_chr.VisibleIndex = 5;
            this.colres_chr.Width = 78;
            // 
            // colres_ref_range
            // 
            this.colres_ref_range.AppearanceCell.Options.UseTextOptions = true;
            this.colres_ref_range.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colres_ref_range.Caption = "参考范围";
            this.colres_ref_range.FieldName = "ResRefRange";
            this.colres_ref_range.Name = "colres_ref_range";
            this.colres_ref_range.OptionsColumn.AllowEdit = false;
            this.colres_ref_range.OptionsColumn.AllowFocus = false;
            this.colres_ref_range.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colres_ref_range.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colres_ref_range.OptionsFilter.AllowAutoFilter = false;
            this.colres_ref_range.OptionsFilter.AllowFilter = false;
            this.colres_ref_range.Visible = true;
            this.colres_ref_range.VisibleIndex = 6;
            this.colres_ref_range.Width = 126;
            // 
            // colres_ref_flag
            // 
            this.colres_ref_flag.Caption = "提示";
            this.colres_ref_flag.FieldName = "ResTips";
            this.colres_ref_flag.Name = "colres_ref_flag";
            this.colres_ref_flag.OptionsColumn.AllowEdit = false;
            this.colres_ref_flag.OptionsColumn.AllowFocus = false;
            this.colres_ref_flag.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colres_ref_flag.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colres_ref_flag.OptionsFilter.AllowAutoFilter = false;
            this.colres_ref_flag.OptionsFilter.AllowFilter = false;
            this.colres_ref_flag.Visible = true;
            this.colres_ref_flag.VisibleIndex = 7;
            this.colres_ref_flag.Width = 48;
            // 
            // colres_date
            // 
            this.colres_date.Caption = "报告时间";
            this.colres_date.ColumnEdit = this.repositoryItemDateEdit1;
            this.colres_date.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colres_date.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colres_date.FieldName = "RepReportDate";
            this.colres_date.Name = "colres_date";
            this.colres_date.OptionsColumn.AllowEdit = false;
            this.colres_date.OptionsColumn.AllowFocus = false;
            this.colres_date.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colres_date.OptionsFilter.AllowAutoFilter = false;
            this.colres_date.OptionsFilter.AllowFilter = false;
            this.colres_date.Visible = true;
            this.colres_date.VisibleIndex = 8;
            this.colres_date.Width = 167;
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
            // colres_sid
            // 
            this.colres_sid.Caption = "标本号";
            this.colres_sid.FieldName = "ObrSid";
            this.colres_sid.Name = "colres_sid";
            this.colres_sid.OptionsColumn.AllowEdit = false;
            this.colres_sid.OptionsColumn.AllowFocus = false;
            this.colres_sid.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colres_sid.OptionsFilter.AllowAutoFilter = false;
            this.colres_sid.OptionsFilter.AllowFilter = false;
            this.colres_sid.Visible = true;
            this.colres_sid.VisibleIndex = 0;
            this.colres_sid.Width = 109;
            // 
            // repositoryItemTextEdit4
            // 
            this.repositoryItemTextEdit4.AutoHeight = false;
            this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.Font = new System.Drawing.Font("宋体", 9F);
            this.panelControl1.Appearance.Options.UseFont = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.pnlDate);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(842, 45);
            this.panelControl1.TabIndex = 4;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.radioGroup1);
            this.panelControl2.Location = new System.Drawing.Point(82, 10);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(255, 26);
            this.panelControl2.TabIndex = 4;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(2, 1);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup1.Properties.Columns = 4;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(1)), "半年"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(2)), "一年"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(((short)(3)), "自定义")});
            this.radioGroup1.Size = new System.Drawing.Size(260, 23);
            this.radioGroup1.TabIndex = 1;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // pnlDate
            // 
            this.pnlDate.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlDate.Controls.Add(this.btnSearch);
            this.pnlDate.Controls.Add(this.deEnd);
            this.pnlDate.Controls.Add(this.deBegin);
            this.pnlDate.Controls.Add(this.labelControl2);
            this.pnlDate.Location = new System.Drawing.Point(342, 8);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(302, 31);
            this.pnlDate.TabIndex = 3;
            this.pnlDate.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnSearch.Location = new System.Drawing.Point(240, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(40, 24);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // deEnd
            // 
            this.deEnd.EditValue = null;
            this.deEnd.EnterMoveNextControl = true;
            this.deEnd.Location = new System.Drawing.Point(128, 5);
            this.deEnd.Name = "deEnd";
            this.deEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Size = new System.Drawing.Size(107, 20);
            this.deEnd.TabIndex = 5;
            // 
            // deBegin
            // 
            this.deBegin.EditValue = null;
            this.deBegin.EnterMoveNextControl = true;
            this.deBegin.Location = new System.Drawing.Point(3, 5);
            this.deBegin.Name = "deBegin";
            this.deBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deBegin.Size = new System.Drawing.Size(107, 20);
            this.deBegin.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(114, 7);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(9, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "~";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(23, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "报告时间:";
            // 
            // PatRelateResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlRelateResult);
            this.Controls.Add(this.panelControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Name = "PatRelateResult";
            this.Size = new System.Drawing.Size(842, 633);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlRelateResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRelateResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlDate)).EndInit();
            this.pnlDate.ResumeLayout(false);
            this.pnlDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deBegin.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlRelateResult;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewRelateResult;
        private DevExpress.XtraGrid.Columns.GridColumn colitm_ecd;
        private DevExpress.XtraGrid.Columns.GridColumn colres_chr;
        private DevExpress.XtraGrid.Columns.GridColumn colres_date;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraGrid.Columns.GridColumn col_comname;
        private DevExpress.XtraGrid.Columns.GridColumn colres_sid;
        private DevExpress.XtraGrid.Columns.GridColumn colres_ref_range;
        private DevExpress.XtraGrid.Columns.GridColumn colres_ref_flag;
        private DevExpress.XtraGrid.Columns.GridColumn colitm_name;
        private DevExpress.XtraGrid.Columns.GridColumn col_instrument;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl pnlDate;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.DateEdit deEnd;
        private DevExpress.XtraEditors.DateEdit deBegin;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}
