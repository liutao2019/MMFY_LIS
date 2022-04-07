namespace dcl.client.qc
{
    partial class FrmAddQcItemNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddQcItemNew));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtSearch = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.sysAddItem = new dcl.client.common.SysToolBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.gcItem = new DevExpress.XtraGrid.GridControl();
            this.gvItem = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.itm_select = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDelQcItem = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddQcItem = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gcQcItem = new DevExpress.XtraGrid.GridControl();
            this.gvQcItem = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtSd = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtCV = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.cklRules = new DevExpress.XtraEditors.CheckedListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcQcItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQcItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cklRules)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.txtSearch);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 80);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(935, 53);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(537, 15);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(169, 22);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "可双击添加/删除项目";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(86, 10);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtSearch.Properties.Appearance.Options.UseFont = true;
            this.txtSearch.Size = new System.Drawing.Size(235, 28);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.EditValueChanged += new System.EventHandler(this.txtSearch_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl1.Location = new System.Drawing.Point(30, 15);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(54, 22);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "检索：";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.sysAddItem);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(935, 80);
            this.panelControl2.TabIndex = 1;
            // 
            // sysAddItem
            // 
            this.sysAddItem.AutoCloseButton = true;
            this.sysAddItem.AutoEnableButtons = false;
            this.sysAddItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysAddItem.Location = new System.Drawing.Point(2, 2);
            this.sysAddItem.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.sysAddItem.Name = "sysAddItem";
            this.sysAddItem.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysAddItem.NotWriteLogButtonNameList")));
            this.sysAddItem.ShowItemToolTips = false;
            this.sysAddItem.Size = new System.Drawing.Size(931, 76);
            this.sysAddItem.TabIndex = 0;
            this.sysAddItem.OnBtnSaveClicked += new System.EventHandler(this.sysAddItem_OnBtnSaveClicked);
            this.sysAddItem.BtnResetClick += new System.EventHandler(this.sysAddItem_BtnResetClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 133);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(935, 676);
            this.panel1.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gcItem);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(336, 676);
            this.panel4.TabIndex = 2;
            // 
            // gcItem
            // 
            this.gcItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcItem.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcItem.Location = new System.Drawing.Point(0, 0);
            this.gcItem.MainView = this.gvItem;
            this.gcItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcItem.Name = "gcItem";
            this.gcItem.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gcItem.Size = new System.Drawing.Size(336, 676);
            this.gcItem.TabIndex = 0;
            this.gcItem.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItem});
            this.gcItem.DoubleClick += new System.EventHandler(this.gcItem_DoubleClick);
            this.gcItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridControl1_MouseDown);
            // 
            // gvItem
            // 
            this.gvItem.ColumnPanelRowHeight = 23;
            this.gvItem.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.itm_select,
            this.gridColumn2,
            this.gridColumn3});
            this.gvItem.GridControl = this.gcItem;
            this.gvItem.Name = "gvItem";
            this.gvItem.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvItem.OptionsView.ShowGroupPanel = false;
            this.gvItem.CustomDrawColumnHeader += new DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventHandler(this.gvItem_CustomDrawColumnHeader);
            // 
            // itm_select
            // 
            this.itm_select.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.itm_select.AppearanceCell.Options.UseFont = true;
            this.itm_select.AppearanceCell.Options.UseTextOptions = true;
            this.itm_select.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.itm_select.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.itm_select.AppearanceHeader.Options.UseFont = true;
            this.itm_select.AppearanceHeader.Options.UseTextOptions = true;
            this.itm_select.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.itm_select.ColumnEdit = this.repositoryItemCheckEdit1;
            this.itm_select.FieldName = "ItmSelect";
            this.itm_select.Name = "itm_select";
            this.itm_select.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.itm_select.Visible = true;
            this.itm_select.VisibleIndex = 0;
            this.itm_select.Width = 20;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.repositoryItemCheckEdit1.ValueChecked = 1;
            this.repositoryItemCheckEdit1.ValueUnchecked = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "项目名称";
            this.gridColumn2.FieldName = "ItmName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 115;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "项目代码";
            this.gridColumn3.FieldName = "ItmEcode";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 92;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDelQcItem);
            this.panel3.Controls.Add(this.btnAddQcItem);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(336, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(96, 676);
            this.panel3.TabIndex = 1;
            // 
            // btnDelQcItem
            // 
            this.btnDelQcItem.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.btnDelQcItem.Appearance.Options.UseFont = true;
            this.btnDelQcItem.Location = new System.Drawing.Point(13, 332);
            this.btnDelQcItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelQcItem.Name = "btnDelQcItem";
            this.btnDelQcItem.Size = new System.Drawing.Size(66, 35);
            this.btnDelQcItem.TabIndex = 1;
            this.btnDelQcItem.Text = "<";
            this.btnDelQcItem.Click += new System.EventHandler(this.btnDelQcItem_Click);
            // 
            // btnAddQcItem
            // 
            this.btnAddQcItem.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.btnAddQcItem.Appearance.Options.UseFont = true;
            this.btnAddQcItem.Location = new System.Drawing.Point(13, 255);
            this.btnAddQcItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddQcItem.Name = "btnAddQcItem";
            this.btnAddQcItem.Size = new System.Drawing.Size(66, 35);
            this.btnAddQcItem.TabIndex = 0;
            this.btnAddQcItem.Text = ">";
            this.btnAddQcItem.Click += new System.EventHandler(this.btnAddQcItem_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gcQcItem);
            this.panel2.Controls.Add(this.cklRules);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(432, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(503, 676);
            this.panel2.TabIndex = 0;
            // 
            // gcQcItem
            // 
            this.gcQcItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcQcItem.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcQcItem.Location = new System.Drawing.Point(0, 0);
            this.gcQcItem.MainView = this.gvQcItem;
            this.gcQcItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcQcItem.Name = "gcQcItem";
            this.gcQcItem.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.txtSd,
            this.txtCV});
            this.gcQcItem.Size = new System.Drawing.Size(503, 603);
            this.gcQcItem.TabIndex = 1;
            this.gcQcItem.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvQcItem});
            this.gcQcItem.DoubleClick += new System.EventHandler(this.gcQcItem_DoubleClick);
            // 
            // gvQcItem
            // 
            this.gvQcItem.ColumnPanelRowHeight = 23;
            this.gvQcItem.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn1,
            this.gridColumn7});
            this.gvQcItem.GridControl = this.gcQcItem;
            this.gvQcItem.Name = "gvQcItem";
            this.gvQcItem.OptionsNavigation.EnterMoveNextColumn = true;
            this.gvQcItem.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "项目名称";
            this.gridColumn4.FieldName = "QcrItmName";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "靶值";
            this.gridColumn5.FieldName = "MatItmX";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 1;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "标准差";
            this.gridColumn6.ColumnEdit = this.txtSd;
            this.gridColumn6.FieldName = "MatItmSd";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            // 
            // txtSd
            // 
            this.txtSd.AutoHeight = false;
            this.txtSd.Name = "txtSd";
            this.txtSd.EditValueChanged += new System.EventHandler(this.txtSd_EditValueChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "CCV";
            this.gridColumn1.FieldName = "MatItmCcv";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 4;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn7.AppearanceCell.Options.UseFont = true;
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "CV";
            this.gridColumn7.ColumnEdit = this.txtCV;
            this.gridColumn7.FieldName = "MatItmCv";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 3;
            // 
            // txtCV
            // 
            this.txtCV.AutoHeight = false;
            this.txtCV.Name = "txtCV";
            this.txtCV.EditValueChanged += new System.EventHandler(this.txtCV_EditValueChanged);
            // 
            // cklRules
            // 
            this.cklRules.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.cklRules.Appearance.Options.UseFont = true;
            this.cklRules.CheckOnClick = true;
            this.cklRules.ColumnWidth = 65;
            this.cklRules.DisplayMember = "RulName";
            this.cklRules.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cklRules.ItemHeight = 15;
            this.cklRules.Location = new System.Drawing.Point(0, 603);
            this.cklRules.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cklRules.MultiColumn = true;
            this.cklRules.Name = "cklRules";
            this.cklRules.Size = new System.Drawing.Size(503, 73);
            this.cklRules.TabIndex = 15;
            this.cklRules.ValueMember = "RulId";
            // 
            // FrmAddQcItemNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 809);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddQcItemNew";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "质控项目快速录入";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAddQcItem_FormClosed);
            this.Load += new System.EventHandler(this.FrmQcCopy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcQcItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQcItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cklRules)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private dcl.client.common.SysToolBar sysAddItem;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.TextEdit txtSearch;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private DevExpress.XtraGrid.GridControl gcItem;
        private DevExpress.XtraGrid.Views.Grid.GridView gvItem;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton btnDelQcItem;
        private DevExpress.XtraEditors.SimpleButton btnAddQcItem;
        private DevExpress.XtraGrid.GridControl gcQcItem;
        private DevExpress.XtraGrid.Views.Grid.GridView gvQcItem;
        private DevExpress.XtraGrid.Columns.GridColumn itm_select;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckedListBoxControl cklRules;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtSd;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtCV;

    }
}