namespace dcl.client.oa
{
    partial class FrmOfficeMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOfficeMessage));
            this.popupUsers = new DevExpress.XtraEditors.PopupContainerControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.tvUserInfo = new DevExpress.XtraTreeList.TreeList();
            this.colUserInfoName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colUserInfoId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.tvRoleUser = new DevExpress.XtraTreeList.TreeList();
            this.colRoleUserInfoId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colRoleUserName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.tvDepartUser = new DevExpress.XtraTreeList.TreeList();
            this.colDepartUserInfoId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDepartUserName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.tvMessage = new DevExpress.XtraTreeList.TreeList();
            this.colMessageId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colMessageTitle = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colCreateDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colReadDate = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colMessageType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.txtMessageContent = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.ckeSaveFrom = new DevExpress.XtraEditors.CheckEdit();
            this.txtMessageTo = new DevExpress.XtraEditors.PopupContainerEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtMessageFrom = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtMessageTitle = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panel10 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.popupUsers)).BeginInit();
            this.popupUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tvUserInfo)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tvRoleUser)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tvDepartUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tvMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageContent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckeSaveFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageTitle.Properties)).BeginInit();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // popupUsers
            // 
            this.popupUsers.Controls.Add(this.xtraTabControl1);
            this.popupUsers.Location = new System.Drawing.Point(171, 164);
            this.popupUsers.Name = "popupUsers";
            this.popupUsers.Size = new System.Drawing.Size(400, 374);
            this.popupUsers.TabIndex = 22;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(400, 374);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.tvUserInfo);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(388, 317);
            this.xtraTabPage1.Text = "物理组";
            // 
            // tvUserInfo
            // 
            this.tvUserInfo.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colUserInfoName,
            this.colUserInfoId});
            this.tvUserInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvUserInfo.KeyFieldName = "UserIDMess";
            this.tvUserInfo.Location = new System.Drawing.Point(0, 0);
            this.tvUserInfo.Name = "tvUserInfo";
            this.tvUserInfo.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvUserInfo.OptionsView.ShowCheckBoxes = true;
            this.tvUserInfo.ParentFieldName = "ProID";
            this.tvUserInfo.Size = new System.Drawing.Size(388, 317);
            this.tvUserInfo.TabIndex = 2;
            this.tvUserInfo.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tvUserInfo_AfterCheckNode);
            // 
            // colUserInfoName
            // 
            this.colUserInfoName.Caption = "物理组和用户";
            this.colUserInfoName.FieldName = "UserName";
            this.colUserInfoName.MinWidth = 47;
            this.colUserInfoName.Name = "colUserInfoName";
            this.colUserInfoName.OptionsColumn.AllowEdit = false;
            this.colUserInfoName.Visible = true;
            this.colUserInfoName.VisibleIndex = 0;
            // 
            // colUserInfoId
            // 
            this.colUserInfoId.Caption = "userInfoId";
            this.colUserInfoId.FieldName = "UserId";
            this.colUserInfoId.Name = "colUserInfoId";
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.tvRoleUser);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(388, 317);
            this.xtraTabPage2.Text = "角色";
            // 
            // tvRoleUser
            // 
            this.tvRoleUser.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colRoleUserInfoId,
            this.colRoleUserName});
            this.tvRoleUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRoleUser.KeyFieldName = "UserId";
            this.tvRoleUser.Location = new System.Drawing.Point(0, 0);
            this.tvRoleUser.Name = "tvRoleUser";
            this.tvRoleUser.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvRoleUser.OptionsView.ShowCheckBoxes = true;
            this.tvRoleUser.ParentFieldName = "RoleId";
            this.tvRoleUser.Size = new System.Drawing.Size(388, 317);
            this.tvRoleUser.TabIndex = 3;
            this.tvRoleUser.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tvRoleUser_AfterCheckNode);
            // 
            // colRoleUserInfoId
            // 
            this.colRoleUserInfoId.Caption = "userInfoId";
            this.colRoleUserInfoId.FieldName = "UserIdMess";
            this.colRoleUserInfoId.Name = "colRoleUserInfoId";
            this.colRoleUserInfoId.OptionsColumn.AllowEdit = false;
            // 
            // colRoleUserName
            // 
            this.colRoleUserName.Caption = "角色和用户";
            this.colRoleUserName.FieldName = "UserName";
            this.colRoleUserName.MinWidth = 47;
            this.colRoleUserName.Name = "colRoleUserName";
            this.colRoleUserName.OptionsColumn.AllowEdit = false;
            this.colRoleUserName.Visible = true;
            this.colRoleUserName.VisibleIndex = 0;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.tvDepartUser);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(388, 317);
            this.xtraTabPage3.Text = "科室";
            // 
            // tvDepartUser
            // 
            this.tvDepartUser.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colDepartUserInfoId,
            this.colDepartUserName});
            this.tvDepartUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDepartUser.KeyFieldName = "UserIdDep";
            this.tvDepartUser.Location = new System.Drawing.Point(0, 0);
            this.tvDepartUser.Name = "tvDepartUser";
            this.tvDepartUser.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvDepartUser.OptionsView.ShowCheckBoxes = true;
            this.tvDepartUser.ParentFieldName = "DeptId";
            this.tvDepartUser.Size = new System.Drawing.Size(388, 317);
            this.tvDepartUser.TabIndex = 3;
            this.tvDepartUser.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tvDepartUser_AfterCheckNode);
            // 
            // colDepartUserInfoId
            // 
            this.colDepartUserInfoId.Caption = "userInfoId";
            this.colDepartUserInfoId.FieldName = "UserId";
            this.colDepartUserInfoId.Name = "colDepartUserInfoId";
            this.colDepartUserInfoId.OptionsColumn.AllowEdit = false;
            // 
            // colDepartUserName
            // 
            this.colDepartUserName.Caption = "科室和用户";
            this.colDepartUserName.FieldName = "UserName";
            this.colDepartUserName.MinWidth = 47;
            this.colDepartUserName.Name = "colDepartUserName";
            this.colDepartUserName.OptionsColumn.AllowEdit = false;
            this.colDepartUserName.Visible = true;
            this.colDepartUserName.VisibleIndex = 0;
            // 
            // tvMessage
            // 
            this.tvMessage.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colMessageId,
            this.colMessageTitle,
            this.colCreateDate,
            this.colReadDate,
            this.colMessageType});
            this.tvMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMessage.KeyFieldName = "MessageId";
            this.tvMessage.Location = new System.Drawing.Point(13, 54);
            this.tvMessage.Name = "tvMessage";
            this.tvMessage.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvMessage.OptionsView.ShowCheckBoxes = true;
            this.tvMessage.ParentFieldName = "MessageOwerType";
            this.tvMessage.Size = new System.Drawing.Size(784, 587);
            this.tvMessage.TabIndex = 0;
            this.tvMessage.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tvMessage_AfterCheckNode);
            this.tvMessage.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tvMessage_FocusedNodeChanged);
            this.tvMessage.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.tvMessage_CustomDrawNodeCell);
            // 
            // colMessageId
            // 
            this.colMessageId.Caption = "MessageId";
            this.colMessageId.FieldName = "MessageId";
            this.colMessageId.Name = "colMessageId";
            this.colMessageId.OptionsColumn.AllowEdit = false;
            // 
            // colMessageTitle
            // 
            this.colMessageTitle.Caption = "消息标题";
            this.colMessageTitle.FieldName = "MessageTitle";
            this.colMessageTitle.MinWidth = 47;
            this.colMessageTitle.Name = "colMessageTitle";
            this.colMessageTitle.OptionsColumn.AllowEdit = false;
            this.colMessageTitle.Visible = true;
            this.colMessageTitle.VisibleIndex = 0;
            // 
            // colCreateDate
            // 
            this.colCreateDate.Caption = "发送时间";
            this.colCreateDate.FieldName = "CreateDate";
            this.colCreateDate.Name = "colCreateDate";
            this.colCreateDate.OptionsColumn.AllowEdit = false;
            this.colCreateDate.OptionsColumn.FixedWidth = true;
            this.colCreateDate.Visible = true;
            this.colCreateDate.VisibleIndex = 1;
            this.colCreateDate.Width = 110;
            // 
            // colReadDate
            // 
            this.colReadDate.Caption = "已读";
            this.colReadDate.FieldName = "ReadDateYN";
            this.colReadDate.Name = "colReadDate";
            this.colReadDate.OptionsColumn.AllowEdit = false;
            this.colReadDate.OptionsColumn.FixedWidth = true;
            this.colReadDate.Visible = true;
            this.colReadDate.VisibleIndex = 2;
            this.colReadDate.Width = 40;
            // 
            // colMessageType
            // 
            this.colMessageType.Caption = "MessageType";
            this.colMessageType.FieldName = "MessageType";
            this.colMessageType.Name = "colMessageType";
            // 
            // txtMessageContent
            // 
            this.txtMessageContent.Location = new System.Drawing.Point(81, 160);
            this.txtMessageContent.Name = "txtMessageContent";
            this.txtMessageContent.Size = new System.Drawing.Size(419, 486);
            this.txtMessageContent.TabIndex = 0;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(15, 161);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(96, 29);
            this.labelControl4.TabIndex = 21;
            this.labelControl4.Text = "消息正文";
            // 
            // ckeSaveFrom
            // 
            this.ckeSaveFrom.EditValue = true;
            this.ckeSaveFrom.Location = new System.Drawing.Point(15, 31);
            this.ckeSaveFrom.Name = "ckeSaveFrom";
            this.ckeSaveFrom.Properties.Caption = "发送消息时保存到发件箱";
            this.ckeSaveFrom.Size = new System.Drawing.Size(214, 34);
            this.ckeSaveFrom.TabIndex = 25;
            // 
            // txtMessageTo
            // 
            this.txtMessageTo.Location = new System.Drawing.Point(81, 129);
            this.txtMessageTo.Name = "txtMessageTo";
            this.txtMessageTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtMessageTo.Properties.PopupControl = this.popupUsers;
            this.txtMessageTo.Size = new System.Drawing.Size(419, 36);
            this.txtMessageTo.TabIndex = 24;
            this.txtMessageTo.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.txtMessageTo_Closed);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(30, 131);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 29);
            this.labelControl5.TabIndex = 23;
            this.labelControl5.Text = "收件人";
            // 
            // txtMessageFrom
            // 
            this.txtMessageFrom.EnterMoveNextControl = true;
            this.txtMessageFrom.Location = new System.Drawing.Point(81, 99);
            this.txtMessageFrom.Name = "txtMessageFrom";
            this.txtMessageFrom.Size = new System.Drawing.Size(419, 36);
            this.txtMessageFrom.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(30, 101);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 29);
            this.labelControl2.TabIndex = 21;
            this.labelControl2.Text = "发件人";
            // 
            // txtMessageTitle
            // 
            this.txtMessageTitle.EnterMoveNextControl = true;
            this.txtMessageTitle.Location = new System.Drawing.Point(81, 69);
            this.txtMessageTitle.Name = "txtMessageTitle";
            this.txtMessageTitle.Size = new System.Drawing.Size(419, 36);
            this.txtMessageTitle.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 71);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(96, 29);
            this.labelControl3.TabIndex = 19;
            this.labelControl3.Text = "消息标题";
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.sysToolBar1);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(1327, 81);
            this.panel10.TabIndex = 3;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1327, 81);
            this.sysToolBar1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 81);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1327, 660);
            this.panelControl1.TabIndex = 4;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.popupUsers);
            this.groupControl2.Controls.Add(this.tvMessage);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(3, 3);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Padding = new System.Windows.Forms.Padding(10);
            this.groupControl2.Size = new System.Drawing.Size(810, 654);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "消息记录";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtMessageContent);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.ckeSaveFrom);
            this.groupControl1.Controls.Add(this.txtMessageTo);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.txtMessageTitle);
            this.groupControl1.Controls.Add(this.txtMessageFrom);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupControl1.Location = new System.Drawing.Point(813, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(511, 654);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "消息发送";
            // 
            // FrmOfficeMessage
            // 
            this.ClientSize = new System.Drawing.Size(1327, 741);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel10);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "FrmOfficeMessage";
            this.Text = "科室通知管理";
            this.Load += new System.EventHandler(this.FrmOfficeMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.popupUsers)).EndInit();
            this.popupUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tvUserInfo)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tvRoleUser)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tvDepartUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tvMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageContent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckeSaveFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessageTitle.Properties)).EndInit();
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.TreeList tvMessage;
        private DevExpress.XtraEditors.MemoEdit txtMessageContent;
        private DevExpress.XtraEditors.TextEdit txtMessageTitle;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtMessageFrom;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colMessageId;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colMessageTitle;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colCreateDate;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colReadDate;
        private DevExpress.XtraEditors.PopupContainerControl popupUsers;
        private DevExpress.XtraEditors.PopupContainerEdit txtMessageTo;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colMessageType;
        private DevExpress.XtraEditors.CheckEdit ckeSaveFrom;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTreeList.TreeList tvUserInfo;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colUserInfoName;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colUserInfoId;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraTreeList.TreeList tvDepartUser;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartUserInfoId;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDepartUserName;
        private DevExpress.XtraTreeList.TreeList tvRoleUser;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colRoleUserInfoId;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colRoleUserName;
        private System.Windows.Forms.Panel panel10;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
    }
}