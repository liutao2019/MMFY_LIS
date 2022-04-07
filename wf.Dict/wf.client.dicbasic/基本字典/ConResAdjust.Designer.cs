namespace dcl.client.dicbasic
{
    partial class ConResAdjust
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
            this.components = new System.ComponentModel.Container();
            this.bsBscript = new System.Windows.Forms.BindingSource(this.components);
            this.bsItem = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colItrId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMitCno = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSrcRes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSrcSid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colResMultiple = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colResDeciPlace = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDstRes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDstSid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.repositoryItemLookUpEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.bsInstrmt = new System.Windows.Forms.BindingSource(this.components);
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colitr_Id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colItrEname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSortNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemGridLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemGridLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.btnCopy = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.bsBscript)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInstrmt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bsBscript
            // 
            this.bsBscript.DataSource = typeof(dcl.entity.EntityDicResAdjust);
            // 
            // bsItem
            // 
            this.bsItem.DataMember = "dict_item";
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colItrId,
            this.colMitCno,
            this.colSrcRes,
            this.colSrcSid,
            this.colResMultiple,
            this.colResDeciPlace,
            this.colDstRes,
            this.colDstSid});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // colItrId
            // 
            this.colItrId.Caption = "仪器代码";
            this.colItrId.FieldName = "ItrId";
            this.colItrId.Name = "colItrId";
            this.colItrId.OptionsColumn.AllowEdit = false;
            this.colItrId.Visible = true;
            this.colItrId.VisibleIndex = 0;
            // 
            // colMitCno
            // 
            this.colMitCno.Caption = "通道码";
            this.colMitCno.FieldName = "MitCno";
            this.colMitCno.Name = "colMitCno";
            this.colMitCno.Visible = true;
            this.colMitCno.VisibleIndex = 1;
            // 
            // colSrcRes
            // 
            this.colSrcRes.Caption = "原结果";
            this.colSrcRes.FieldName = "SrcRes";
            this.colSrcRes.Name = "colSrcRes";
            this.colSrcRes.Visible = true;
            this.colSrcRes.VisibleIndex = 2;
            // 
            // colSrcSid
            // 
            this.colSrcSid.Caption = "原样本号";
            this.colSrcSid.FieldName = "SrcSid";
            this.colSrcSid.Name = "colSrcSid";
            this.colSrcSid.Visible = true;
            this.colSrcSid.VisibleIndex = 3;
            // 
            // colResMultiple
            // 
            this.colResMultiple.Caption = "调整因子";
            this.colResMultiple.FieldName = "ResMultiple";
            this.colResMultiple.Name = "colResMultiple";
            this.colResMultiple.Visible = true;
            this.colResMultiple.VisibleIndex = 4;
            // 
            // colResDeciPlace
            // 
            this.colResDeciPlace.Caption = "小数位数";
            this.colResDeciPlace.FieldName = "ResDecPlace";
            this.colResDeciPlace.Name = "colResDeciPlace";
            this.colResDeciPlace.Visible = true;
            this.colResDeciPlace.VisibleIndex = 5;
            // 
            // colDstRes
            // 
            this.colDstRes.Caption = "替换结果";
            this.colDstRes.FieldName = "DstRes";
            this.colDstRes.Name = "colDstRes";
            this.colDstRes.Visible = true;
            this.colDstRes.VisibleIndex = 6;
            // 
            // colDstSid
            // 
            this.colDstSid.Caption = "替换样本号";
            this.colDstSid.FieldName = "DstSid";
            this.colDstSid.Name = "colDstSid";
            this.colDstSid.Visible = true;
            this.colDstSid.VisibleIndex = 7;
            // 
            // gridControl1
            // 
            this.gridControl1.CausesValidation = false;
            this.gridControl1.DataSource = this.bsBscript;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit3});
            this.gridControl1.Size = new System.Drawing.Size(639, 615);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // repositoryItemLookUpEdit3
            // 
            this.repositoryItemLookUpEdit3.AutoHeight = false;
            this.repositoryItemLookUpEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit3.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itr_id", "编号", 43, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itr_mid", "仪器代码", 38, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.repositoryItemLookUpEdit3.DataSource = this.bsInstrmt;
            this.repositoryItemLookUpEdit3.DisplayMember = "itr_mid";
            this.repositoryItemLookUpEdit3.Name = "repositoryItemLookUpEdit3";
            this.repositoryItemLookUpEdit3.ValueMember = "itr_id";
            // 
            // bsInstrmt
            // 
            this.bsInstrmt.DataSource = typeof(dcl.entity.EntityDicInstrument);
            this.bsInstrmt.CurrentChanged += new System.EventHandler(this.bsInstrmt_CurrentChanged);
            // 
            // gridControl2
            // 
            this.gridControl2.AllowRestoreSelectionAndFocusedRow = DevExpress.Utils.DefaultBoolean.False;
            this.gridControl2.CausesValidation = false;
            this.gridControl2.DataMember = null;
            this.gridControl2.DataSource = this.bsInstrmt;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemGridLookUpEdit1,
            this.repositoryItemGridLookUpEdit2,
            this.repositoryItemLookUpEdit2});
            this.gridControl2.Size = new System.Drawing.Size(314, 575);
            this.gridControl2.TabIndex = 47;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colitr_Id,
            this.colItrEname,
            this.colSortNo});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsFind.AlwaysVisible = true;
            this.gridView2.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView2.OptionsView.ShowFooter = true;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // colitr_Id
            // 
            this.colitr_Id.Caption = "编码";
            this.colitr_Id.FieldName = "ItrId";
            this.colitr_Id.Name = "colitr_Id";
            this.colitr_Id.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ItrId", "记录总数:{0:0.##}")});
            this.colitr_Id.Visible = true;
            this.colitr_Id.VisibleIndex = 0;
            this.colitr_Id.Width = 56;
            // 
            // colItrEname
            // 
            this.colItrEname.Caption = "仪器代码";
            this.colItrEname.FieldName = "ItrEname";
            this.colItrEname.Name = "colItrEname";
            this.colItrEname.Visible = true;
            this.colItrEname.VisibleIndex = 1;
            this.colItrEname.Width = 111;
            // 
            // colSortNo
            // 
            this.colSortNo.Caption = "序号";
            this.colSortNo.FieldName = "SortNo";
            this.colSortNo.Name = "colSortNo";
            // 
            // repositoryItemGridLookUpEdit1
            // 
            this.repositoryItemGridLookUpEdit1.AutoHeight = false;
            this.repositoryItemGridLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemGridLookUpEdit1.Name = "repositoryItemGridLookUpEdit1";
            this.repositoryItemGridLookUpEdit1.View = this.gridView3;
            // 
            // gridView3
            // 
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemGridLookUpEdit2
            // 
            this.repositoryItemGridLookUpEdit2.AutoHeight = false;
            this.repositoryItemGridLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemGridLookUpEdit2.DataSource = this.bsInstrmt;
            this.repositoryItemGridLookUpEdit2.Name = "repositoryItemGridLookUpEdit2";
            this.repositoryItemGridLookUpEdit2.View = this.gridView4;
            // 
            // gridView4
            // 
            this.gridView4.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView4.Name = "gridView4";
            this.gridView4.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView4.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itr_id", "编码", 43, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itr_mid", "仪器代码", 38, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.repositoryItemLookUpEdit2.DataSource = this.bsInstrmt;
            this.repositoryItemLookUpEdit2.DisplayMember = "itr_mid";
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.ValueMember = "itr_id";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl3);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(958, 615);
            this.splitContainerControl1.SplitterPosition = 449;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.btnCopy);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(0, 575);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(314, 40);
            this.panelControl3.TabIndex = 49;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(6, 6);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(274, 29);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.Text = "复制仪器结果调整";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(962, 619);
            this.panelControl2.TabIndex = 2;
            // 
            // ConResAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "ConResAdjust";
            this.Size = new System.Drawing.Size(962, 619);
            this.Load += new System.EventHandler(this.ConResAdjust_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsBscript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsInstrmt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemGridLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsBscript;
        private System.Windows.Forms.BindingSource bsItem;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit3;
        private System.Windows.Forms.BindingSource bsInstrmt;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_Id;
        private DevExpress.XtraGrid.Columns.GridColumn colItrEname;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit repositoryItemGridLookUpEdit2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.Columns.GridColumn colItrId;
        private DevExpress.XtraGrid.Columns.GridColumn colMitCno;
        private DevExpress.XtraGrid.Columns.GridColumn colSrcRes;
        private DevExpress.XtraGrid.Columns.GridColumn colSrcSid;
        private DevExpress.XtraGrid.Columns.GridColumn colResMultiple;
        private DevExpress.XtraGrid.Columns.GridColumn colResDeciPlace;
        private DevExpress.XtraGrid.Columns.GridColumn colDstRes;
        private DevExpress.XtraGrid.Columns.GridColumn colDstSid;
        private DevExpress.XtraGrid.Columns.GridColumn colSortNo;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btnCopy;
    }
}
