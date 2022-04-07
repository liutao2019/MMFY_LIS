namespace dcl.client.users
{
    partial class FrmRoleManagePro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRoleManagePro));
            this.tvRole = new DevExpress.XtraTreeList.TreeList();
            this.colRoleInfoId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colRoleName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colRoleDesc = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.bsRole = new System.Windows.Forms.BindingSource();
            this.bsFunc = new System.Windows.Forms.BindingSource();
            this.bsUser = new System.Windows.Forms.BindingSource();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.tvPower = new DevExpress.XtraTreeList.TreeList();
            this.colFuncName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.cbxRoleUser = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRoleName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtRoleDesc = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tvRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFunc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tvPower)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxRoleUser)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleDesc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tvRole
            // 
            this.tvRole.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colRoleInfoId,
            this.colRoleName,
            this.colRoleDesc});
            this.tvRole.DataSource = this.bsRole;
            this.tvRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRole.KeyFieldName = "RoleId";
            this.tvRole.Location = new System.Drawing.Point(3, 33);
            this.tvRole.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tvRole.Name = "tvRole";
            this.tvRole.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvRole.OptionsView.ShowRoot = false;
            this.tvRole.ParentFieldName = "";
            this.tvRole.Size = new System.Drawing.Size(620, 573);
            this.tvRole.TabIndex = 2;
            this.tvRole.TreeLevelWidth = 12;
            this.tvRole.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tvRole.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tvRole_FocusedNodeChanged);
            // 
            // colRoleInfoId
            // 
            this.colRoleInfoId.Caption = "角色编号";
            this.colRoleInfoId.FieldName = "RoleId";
            this.colRoleInfoId.Name = "colRoleInfoId";
            // 
            // colRoleName
            // 
            this.colRoleName.Caption = "角色名称";
            this.colRoleName.FieldName = "RoleName";
            this.colRoleName.Name = "colRoleName";
            this.colRoleName.OptionsColumn.AllowEdit = false;
            this.colRoleName.OptionsColumn.FixedWidth = true;
            this.colRoleName.Visible = true;
            this.colRoleName.VisibleIndex = 0;
            this.colRoleName.Width = 70;
            // 
            // colRoleDesc
            // 
            this.colRoleDesc.Caption = "角色描述";
            this.colRoleDesc.FieldName = "RoleRemark";
            this.colRoleDesc.Name = "colRoleDesc";
            this.colRoleDesc.OptionsColumn.AllowEdit = false;
            this.colRoleDesc.OptionsColumn.FixedWidth = true;
            this.colRoleDesc.Visible = true;
            this.colRoleDesc.VisibleIndex = 1;
            this.colRoleDesc.Width = 120;
            // 
            // bsRole
            // 
            this.bsRole.DataSource = typeof(dcl.entity.EntitySysRole);
            this.bsRole.CurrentChanged += new System.EventHandler(this.bsRole_CurrentChanged);
            // 
            // bsFunc
            // 
            this.bsFunc.DataSource = typeof(dcl.entity.EntitySysFunction);
            // 
            // bsUser
            // 
            this.bsUser.DataSource = typeof(dcl.entity.EntitySysUser);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.xtraTabControl1.Location = new System.Drawing.Point(3, 3);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(965, 689);
            this.xtraTabControl1.TabIndex = 20;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.tvPower);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(955, 644);
            this.xtraTabPage1.Text = "角色列表";
            // 
            // tvPower
            // 
            this.tvPower.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colFuncName});
            this.tvPower.DataSource = this.bsFunc;
            this.tvPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvPower.KeyFieldName = "FuncId";
            this.tvPower.Location = new System.Drawing.Point(0, 0);
            this.tvPower.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tvPower.Name = "tvPower";
            this.tvPower.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvPower.OptionsView.ShowCheckBoxes = true;
            this.tvPower.ParentFieldName = "FuncParentId";
            this.tvPower.Size = new System.Drawing.Size(955, 644);
            this.tvPower.TabIndex = 3;
            // 
            // colFuncName
            // 
            this.colFuncName.Caption = "功能名称";
            this.colFuncName.FieldName = "FuncName";
            this.colFuncName.MinWidth = 32;
            this.colFuncName.Name = "colFuncName";
            this.colFuncName.OptionsColumn.AllowEdit = false;
            this.colFuncName.OptionsColumn.AllowSort = false;
            this.colFuncName.Visible = true;
            this.colFuncName.VisibleIndex = 0;
            this.colFuncName.Width = 200;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.cbxRoleUser);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(955, 644);
            this.xtraTabPage2.Text = "用户列表";
            // 
            // cbxRoleUser
            // 
            this.cbxRoleUser.CheckOnClick = true;
            this.cbxRoleUser.ColumnWidth = 140;
            this.cbxRoleUser.DataSource = this.bsUser;
            this.cbxRoleUser.DisplayMember = "UserName";
            this.cbxRoleUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbxRoleUser.Location = new System.Drawing.Point(0, 0);
            this.cbxRoleUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxRoleUser.MultiColumn = true;
            this.cbxRoleUser.Name = "cbxRoleUser";
            this.cbxRoleUser.Size = new System.Drawing.Size(955, 644);
            this.cbxRoleUser.TabIndex = 4;
            this.cbxRoleUser.ValueMember = "UserLoginid";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.sysToolBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1603, 99);
            this.panel3.TabIndex = 21;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1603, 99);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.btnAddRole_Click);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.btnModifyRole_Click);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.btnDelRole_Click);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.btnSaveRole_Click);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.btnCancelRole_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 99);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1603, 701);
            this.panelControl1.TabIndex = 22;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.xtraTabControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(629, 3);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(971, 695);
            this.panelControl2.TabIndex = 21;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.tvRole);
            this.groupControl1.Controls.Add(this.panel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(626, 695);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "角色列表";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtRoleName);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.txtRoleDesc);
            this.panel1.Controls.Add(this.labelControl3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 606);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(620, 86);
            this.panel1.TabIndex = 1;
            // 
            // txtRoleName
            // 
            this.txtRoleName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsRole, "RoleName", true));
            this.txtRoleName.EnterMoveNextControl = true;
            this.txtRoleName.Location = new System.Drawing.Point(113, 11);
            this.txtRoleName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(333, 28);
            this.txtRoleName.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(30, 14);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 22);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "角色名称";
            // 
            // txtRoleDesc
            // 
            this.txtRoleDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsRole, "RoleRemark", true));
            this.txtRoleDesc.EnterMoveNextControl = true;
            this.txtRoleDesc.Location = new System.Drawing.Point(113, 50);
            this.txtRoleDesc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRoleDesc.Name = "txtRoleDesc";
            this.txtRoleDesc.Size = new System.Drawing.Size(333, 28);
            this.txtRoleDesc.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(30, 53);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 22);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "角色描述";
            // 
            // FrmRoleManagePro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1603, 800);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel3);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmRoleManagePro";
            this.Text = "角色管理";
            this.Load += new System.EventHandler(this.FrmRoleManagePro_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tvRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFunc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tvPower)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbxRoleUser)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoleDesc.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.TreeList tvRole;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colRoleName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colRoleDesc;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colRoleInfoId;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Windows.Forms.BindingSource bsRole;
        private System.Windows.Forms.BindingSource bsUser;
        private System.Windows.Forms.BindingSource bsFunc;
        private System.Windows.Forms.Panel panel3;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtRoleDesc;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtRoleName;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFuncName;
        private DevExpress.XtraTreeList.TreeList tvPower;
        private DevExpress.XtraEditors.CheckedListBoxControl cbxRoleUser;
    }
}