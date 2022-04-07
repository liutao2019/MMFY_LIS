namespace dcl.client.users
{
    partial class FrmSystemConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSystemConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtSearch = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.gdSystemConfig = new DevExpress.XtraGrid.GridControl();
            this.gvSystemConfig = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colConfigId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConfigItemName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConfigItemValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colConfigGroup = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSystemConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSystemConfig)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(287, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 160;
            this.label1.Text = "分组";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(331, 41);
            this.comboBoxEdit1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.DropDownRows = 20;
            this.comboBoxEdit1.Size = new System.Drawing.Size(211, 24);
            this.comboBoxEdit1.TabIndex = 159;
            this.comboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit1_SelectedIndexChanged);
            this.comboBoxEdit1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxEdit1_KeyDown);
            // 
            // txtSearch
            // 
            this.txtSearch.EditValue = "";
            this.txtSearch.EnterMoveNextControl = true;
            this.txtSearch.Location = new System.Drawing.Point(82, 41);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F);
            this.txtSearch.Properties.Appearance.Options.UseFont = true;
            this.txtSearch.Size = new System.Drawing.Size(182, 22);
            this.txtSearch.TabIndex = 158;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 157;
            this.label3.Text = "搜索";
            // 
            // gdSystemConfig
            // 
            this.gdSystemConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdSystemConfig.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gdSystemConfig.Location = new System.Drawing.Point(2, 27);
            this.gdSystemConfig.MainView = this.gvSystemConfig;
            this.gdSystemConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gdSystemConfig.Name = "gdSystemConfig";
            this.gdSystemConfig.Size = new System.Drawing.Size(1031, 614);
            this.gdSystemConfig.TabIndex = 4;
            this.gdSystemConfig.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvSystemConfig});
            // 
            // gvSystemConfig
            // 
            this.gvSystemConfig.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colConfigId,
            this.colConfigItemName,
            this.colConfigItemValue,
            this.colConfigGroup});
            this.gvSystemConfig.GridControl = this.gdSystemConfig;
            this.gvSystemConfig.GroupCount = 1;
            this.gvSystemConfig.Name = "gvSystemConfig";
            this.gvSystemConfig.OptionsView.ShowGroupPanel = false;
            this.gvSystemConfig.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colConfigGroup, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvSystemConfig.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvSystemConfig_CustomRowCellEdit);
            // 
            // colConfigId
            // 
            this.colConfigId.Caption = "Id";
            this.colConfigId.FieldName = "Id";
            this.colConfigId.Name = "colConfigId";
            this.colConfigId.OptionsColumn.AllowEdit = false;
            // 
            // colConfigItemName
            // 
            this.colConfigItemName.Caption = "参数名称";
            this.colConfigItemName.FieldName = "Name";
            this.colConfigItemName.Name = "colConfigItemName";
            this.colConfigItemName.OptionsColumn.AllowEdit = false;
            this.colConfigItemName.Visible = true;
            this.colConfigItemName.VisibleIndex = 0;
            // 
            // colConfigItemValue
            // 
            this.colConfigItemValue.Caption = "参数值";
            this.colConfigItemValue.FieldName = "Value";
            this.colConfigItemValue.Name = "colConfigItemValue";
            this.colConfigItemValue.Visible = true;
            this.colConfigItemValue.VisibleIndex = 1;
            // 
            // colConfigGroup
            // 
            this.colConfigGroup.Caption = "分组";
            this.colConfigGroup.FieldName = "Group";
            this.colConfigGroup.Name = "colConfigGroup";
            this.colConfigGroup.OptionsColumn.AllowEdit = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.sysToolBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1035, 81);
            this.panel3.TabIndex = 6;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1035, 81);
            this.sysToolBar1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.comboBoxEdit1);
            this.groupControl1.Controls.Add(this.txtSearch);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 81);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1035, 79);
            this.groupControl1.TabIndex = 7;
            this.groupControl1.Text = "过滤";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.gdSystemConfig);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 160);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1035, 643);
            this.groupControl2.TabIndex = 8;
            this.groupControl2.Text = "参数列表";
            // 
            // FrmSystemConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1035, 803);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panel3);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmSystemConfig";
            this.Text = "系统配置";
            this.Load += new System.EventHandler(this.FrmSystemConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gdSystemConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvSystemConfig)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gdSystemConfig;
        private DevExpress.XtraGrid.Views.Grid.GridView gvSystemConfig;
        private DevExpress.XtraGrid.Columns.GridColumn colConfigId;
        private DevExpress.XtraGrid.Columns.GridColumn colConfigItemName;
        private DevExpress.XtraGrid.Columns.GridColumn colConfigItemValue;
        private DevExpress.XtraGrid.Columns.GridColumn colConfigGroup;
        private System.Windows.Forms.Label label3;
        protected DevExpress.XtraEditors.TextEdit txtSearch;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private System.Windows.Forms.Panel panel3;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}