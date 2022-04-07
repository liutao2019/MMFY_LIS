namespace dcl.client.result
{
    partial class FrmBakItmForResulto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBakItmForResulto));
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.groupAddInfo = new DevExpress.XtraEditors.GroupControl();
            this.gcResInfo = new DevExpress.XtraGrid.GridControl();
            this.bsResInfo = new System.Windows.Forms.BindingSource(this.components);
            this.gvResInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemCheckedComboBoxEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblMsgCount = new System.Windows.Forms.Label();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupSelRv = new DevExpress.XtraEditors.GroupControl();
            this.gcDateInfo = new DevExpress.XtraGrid.GridControl();
            this.bsDateInfo = new System.Windows.Forms.BindingSource(this.components);
            this.gvDateInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.itemSampleState = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.groupAddInfo)).BeginInit();
            this.groupAddInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcResInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsResInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupSelRv)).BeginInit();
            this.groupSelRv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDateInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDateInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDateInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemSampleState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.AutoShortCuts = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 564);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1067, 90);
            this.sysToolBar1.TabIndex = 10;
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            // 
            // groupAddInfo
            // 
            this.groupAddInfo.Controls.Add(this.gcResInfo);
            this.groupAddInfo.Controls.Add(this.panel1);
            this.groupAddInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupAddInfo.Location = new System.Drawing.Point(0, 0);
            this.groupAddInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupAddInfo.Name = "groupAddInfo";
            this.groupAddInfo.Size = new System.Drawing.Size(674, 564);
            this.groupAddInfo.TabIndex = 2;
            this.groupAddInfo.Text = "新增资料";
            // 
            // gcResInfo
            // 
            this.gcResInfo.DataSource = this.bsResInfo;
            this.gcResInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcResInfo.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcResInfo.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcResInfo.Location = new System.Drawing.Point(2, 27);
            this.gcResInfo.MainView = this.gvResInfo;
            this.gcResInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcResInfo.Name = "gcResInfo";
            this.gcResInfo.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemCheckedComboBoxEdit1});
            this.gcResInfo.Size = new System.Drawing.Size(670, 472);
            this.gcResInfo.TabIndex = 5;
            this.gcResInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvResInfo});
            // 
            // bsResInfo
            // 
            this.bsResInfo.DataSource = typeof(dcl.entity.EntityObrResultBakItm);
            this.bsResInfo.DataSourceChanged += new System.EventHandler(this.bsResInfo_DataSourceChanged);
            // 
            // gvResInfo
            // 
            this.gvResInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.gvResInfo.GridControl = this.gcResInfo;
            this.gvResInfo.Name = "gvResInfo";
            this.gvResInfo.OptionsCustomization.AllowFilter = false;
            this.gvResInfo.OptionsCustomization.AllowGroup = false;
            this.gvResInfo.OptionsNavigation.EnterMoveNextColumn = true;
            this.gvResInfo.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gvResInfo.OptionsSelection.MultiSelect = true;
            this.gvResInfo.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gvResInfo.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "项目名称";
            this.gridColumn2.FieldName = "ItmName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "项目代码";
            this.gridColumn3.FieldName = "ResItmEcd";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "结果";
            this.gridColumn4.FieldName = "ResChr";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "单位";
            this.gridColumn5.FieldName = "ResUnit";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "参考值";
            this.gridColumn6.FieldName = "ResRefType";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("st_id", "编号"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("st_stat", 50, "标本状态")});
            this.repositoryItemLookUpEdit1.DisplayMember = "st_stat";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "";
            this.repositoryItemLookUpEdit1.ValueMember = "st_stat";
            // 
            // repositoryItemCheckedComboBoxEdit1
            // 
            this.repositoryItemCheckedComboBoxEdit1.AutoHeight = false;
            this.repositoryItemCheckedComboBoxEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCheckedComboBoxEdit1.Name = "repositoryItemCheckedComboBoxEdit1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblMsgCount);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 499);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(670, 63);
            this.panel1.TabIndex = 6;
            // 
            // lblMsgCount
            // 
            this.lblMsgCount.AutoSize = true;
            this.lblMsgCount.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsgCount.Location = new System.Drawing.Point(26, 19);
            this.lblMsgCount.Name = "lblMsgCount";
            this.lblMsgCount.Size = new System.Drawing.Size(122, 22);
            this.lblMsgCount.TabIndex = 0;
            this.lblMsgCount.Text = "当前数据 0 条";
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupSelRv);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupAddInfo);
            this.splitContainer1.Size = new System.Drawing.Size(1067, 564);
            this.splitContainer1.SplitterDistance = 387;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 11;
            // 
            // groupSelRv
            // 
            this.groupSelRv.Controls.Add(this.gcDateInfo);
            this.groupSelRv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupSelRv.Location = new System.Drawing.Point(0, 0);
            this.groupSelRv.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupSelRv.Name = "groupSelRv";
            this.groupSelRv.Size = new System.Drawing.Size(387, 564);
            this.groupSelRv.TabIndex = 2;
            this.groupSelRv.Text = "查询结果";
            // 
            // gcDateInfo
            // 
            this.gcDateInfo.DataSource = this.bsDateInfo;
            this.gcDateInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDateInfo.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDateInfo.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gcDateInfo.Location = new System.Drawing.Point(2, 27);
            this.gcDateInfo.MainView = this.gvDateInfo;
            this.gcDateInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcDateInfo.Name = "gcDateInfo";
            this.gcDateInfo.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.itemSampleState});
            this.gcDateInfo.Size = new System.Drawing.Size(383, 535);
            this.gcDateInfo.TabIndex = 2;
            this.gcDateInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDateInfo,
            this.gridView2});
            // 
            // bsDateInfo
            // 
            this.bsDateInfo.DataSource = typeof(dcl.entity.EntityObrResultBakItm);
            this.bsDateInfo.PositionChanged += new System.EventHandler(this.bsDateInfo_PositionChanged);
            // 
            // gvDateInfo
            // 
            this.gvDateInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.gvDateInfo.GridControl = this.gcDateInfo;
            this.gvDateInfo.Name = "gvDateInfo";
            this.gvDateInfo.OptionsNavigation.EnterMoveNextColumn = true;
            this.gvDateInfo.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.Caption = "日期";
            this.gridColumn1.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn1.FieldName = "BakDate";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // itemSampleState
            // 
            this.itemSampleState.AutoHeight = false;
            this.itemSampleState.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.itemSampleState.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("st_id", "编号"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("st_stat", 50, "标本状态")});
            this.itemSampleState.DisplayMember = "st_stat";
            this.itemSampleState.Name = "itemSampleState";
            this.itemSampleState.NullText = "";
            this.itemSampleState.ValueMember = "st_stat";
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gcDateInfo;
            this.gridView2.Name = "gridView2";
            // 
            // FrmBakItmForResulto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 654);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.sysToolBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBakItmForResulto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "还原项目结果";
            this.Load += new System.EventHandler(this.FrmBakItmForResulto_Load);
            this.Shown += new System.EventHandler(this.FrmBakItmForResulto_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.groupAddInfo)).EndInit();
            this.groupAddInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcResInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsResInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvResInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupSelRv)).EndInit();
            this.groupSelRv.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDateInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDateInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDateInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemSampleState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.GroupControl groupAddInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.GroupControl groupSelRv;
        private DevExpress.XtraGrid.GridControl gcDateInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDateInfo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit itemSampleState;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.GridControl gcResInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvResInfo;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMsgCount;
        private System.Windows.Forms.BindingSource bsDateInfo;
        private System.Windows.Forms.BindingSource bsResInfo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
    }
}