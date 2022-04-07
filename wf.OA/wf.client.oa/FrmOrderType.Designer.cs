namespace dcl.client.oa
{
    partial class FrmOrderType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOrderType));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.tvOrderType = new DevExpress.XtraTreeList.TreeList();
            this.colOrderTypeCode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrderTypeName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtOrderTypeName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.barOrderType = new dcl.client.common.SysToolBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ckeShowInList = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtDictFilter = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cboOrderItemType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtDictName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtOrderItemName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.tvOrderItem = new DevExpress.XtraTreeList.TreeList();
            this.colOrderItemCode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrderItemName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrderItemType = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDictName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colDictFilter = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colShowInList = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colInlist = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colOrderItemIndex = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.barOrderItem = new dcl.client.common.SysToolBar();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderType)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrderTypeName.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckeShowInList.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDictFilter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOrderItemType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDictName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrderItemName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderItem)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupControl1);
            this.panel1.Controls.Add(this.barOrderType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 830);
            this.panel1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.tvOrderType);
            this.groupControl1.Controls.Add(this.panel3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 80);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Padding = new System.Windows.Forms.Padding(10);
            this.groupControl1.Size = new System.Drawing.Size(302, 750);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "单证类型";
            // 
            // tvOrderType
            // 
            this.tvOrderType.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colOrderTypeCode,
            this.colOrderTypeName});
            this.tvOrderType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrderType.KeyFieldName = "OrderTypeCode";
            this.tvOrderType.Location = new System.Drawing.Point(12, 37);
            this.tvOrderType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tvOrderType.Name = "tvOrderType";
            this.tvOrderType.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvOrderType.OptionsView.ShowRoot = false;
            this.tvOrderType.ParentFieldName = "";
            this.tvOrderType.Size = new System.Drawing.Size(278, 664);
            this.tvOrderType.TabIndex = 5;
            this.tvOrderType.TreeLevelWidth = 12;
            this.tvOrderType.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tvOrderType.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tvOrderType_FocusedNodeChanged);
            // 
            // colOrderTypeCode
            // 
            this.colOrderTypeCode.Caption = "代码";
            this.colOrderTypeCode.FieldName = "TabCode";
            this.colOrderTypeCode.Name = "colOrderTypeCode";
            this.colOrderTypeCode.OptionsColumn.AllowEdit = false;
            this.colOrderTypeCode.OptionsColumn.FixedWidth = true;
            this.colOrderTypeCode.Visible = true;
            this.colOrderTypeCode.VisibleIndex = 0;
            this.colOrderTypeCode.Width = 46;
            // 
            // colOrderTypeName
            // 
            this.colOrderTypeName.Caption = "单证名称";
            this.colOrderTypeName.FieldName = "TabName";
            this.colOrderTypeName.Name = "colOrderTypeName";
            this.colOrderTypeName.OptionsColumn.AllowEdit = false;
            this.colOrderTypeName.Visible = true;
            this.colOrderTypeName.VisibleIndex = 1;
            this.colOrderTypeName.Width = 127;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtOrderTypeName);
            this.panel3.Controls.Add(this.labelControl2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(12, 701);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(278, 37);
            this.panel3.TabIndex = 5;
            // 
            // txtOrderTypeName
            // 
            this.txtOrderTypeName.EnterMoveNextControl = true;
            this.txtOrderTypeName.Location = new System.Drawing.Point(77, 6);
            this.txtOrderTypeName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOrderTypeName.Name = "txtOrderTypeName";
            this.txtOrderTypeName.Size = new System.Drawing.Size(178, 24);
            this.txtOrderTypeName.TabIndex = 14;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(11, 9);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 15;
            this.labelControl2.Text = "单证名称";
            // 
            // barOrderType
            // 
            this.barOrderType.AutoCloseButton = true;
            this.barOrderType.AutoEnableButtons = false;
            this.barOrderType.Dock = System.Windows.Forms.DockStyle.Top;
            this.barOrderType.Location = new System.Drawing.Point(0, 0);
            this.barOrderType.Margin = new System.Windows.Forms.Padding(5);
            this.barOrderType.Name = "barOrderType";
            this.barOrderType.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("barOrderType.NotWriteLogButtonNameList")));
            this.barOrderType.QuickOption = false;
            this.barOrderType.ShowItemToolTips = false;
            this.barOrderType.Size = new System.Drawing.Size(302, 80);
            this.barOrderType.TabIndex = 0;
            this.barOrderType.OnBtnAddClicked += new System.EventHandler(this.barOrderType_OnBtnAddClicked);
            this.barOrderType.OnBtnDeleteClicked += new System.EventHandler(this.barOrderType_OnBtnDeleteClicked);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupControl2);
            this.panel2.Controls.Add(this.barOrderItem);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(302, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1052, 830);
            this.panel2.TabIndex = 1;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.tvOrderItem);
            this.groupControl2.Controls.Add(this.panel4);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 81);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Padding = new System.Windows.Forms.Padding(10);
            this.groupControl2.Size = new System.Drawing.Size(1052, 749);
            this.groupControl2.TabIndex = 3;
            this.groupControl2.Text = "单证字段";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ckeShowInList);
            this.panel4.Controls.Add(this.labelControl6);
            this.panel4.Controls.Add(this.txtDictFilter);
            this.panel4.Controls.Add(this.labelControl4);
            this.panel4.Controls.Add(this.cboOrderItemType);
            this.panel4.Controls.Add(this.txtDictName);
            this.panel4.Controls.Add(this.labelControl5);
            this.panel4.Controls.Add(this.txtOrderItemName);
            this.panel4.Controls.Add(this.labelControl3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(12, 698);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1028, 39);
            this.panel4.TabIndex = 6;
            // 
            // ckeShowInList
            // 
            this.ckeShowInList.Location = new System.Drawing.Point(914, 8);
            this.ckeShowInList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckeShowInList.Name = "ckeShowInList";
            this.ckeShowInList.Properties.Caption = "列表显示";
            this.ckeShowInList.Size = new System.Drawing.Size(100, 22);
            this.ckeShowInList.TabIndex = 26;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(700, 10);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 18);
            this.labelControl6.TabIndex = 25;
            this.labelControl6.Text = "字典过滤";
            // 
            // txtDictFilter
            // 
            this.txtDictFilter.EnterMoveNextControl = true;
            this.txtDictFilter.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtDictFilter.Location = new System.Drawing.Point(766, 7);
            this.txtDictFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDictFilter.Name = "txtDictFilter";
            this.txtDictFilter.Size = new System.Drawing.Size(142, 24);
            this.txtDictFilter.TabIndex = 24;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(469, 10);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 18);
            this.labelControl4.TabIndex = 23;
            this.labelControl4.Text = "数据字典";
            // 
            // cboOrderItemType
            // 
            this.cboOrderItemType.EnterMoveNextControl = true;
            this.cboOrderItemType.ImeMode = System.Windows.Forms.ImeMode.On;
            this.cboOrderItemType.Location = new System.Drawing.Point(302, 7);
            this.cboOrderItemType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboOrderItemType.Name = "cboOrderItemType";
            this.cboOrderItemType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboOrderItemType.Properties.Items.AddRange(new object[] {
            "字符串",
            "数字",
            "枚举",
            "日期",
            "时间",
            "文本",
            "文件"});
            this.cboOrderItemType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboOrderItemType.Size = new System.Drawing.Size(142, 24);
            this.cboOrderItemType.TabIndex = 22;
            // 
            // txtDictName
            // 
            this.txtDictName.EnterMoveNextControl = true;
            this.txtDictName.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtDictName.Location = new System.Drawing.Point(535, 7);
            this.txtDictName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDictName.Name = "txtDictName";
            this.txtDictName.Size = new System.Drawing.Size(142, 24);
            this.txtDictName.TabIndex = 20;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl5.Location = new System.Drawing.Point(236, 10);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 18);
            this.labelControl5.TabIndex = 21;
            this.labelControl5.Text = "数据类型";
            // 
            // txtOrderItemName
            // 
            this.txtOrderItemName.EnterMoveNextControl = true;
            this.txtOrderItemName.ImeMode = System.Windows.Forms.ImeMode.On;
            this.txtOrderItemName.Location = new System.Drawing.Point(74, 7);
            this.txtOrderItemName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOrderItemName.Name = "txtOrderItemName";
            this.txtOrderItemName.Size = new System.Drawing.Size(142, 24);
            this.txtOrderItemName.TabIndex = 17;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Location = new System.Drawing.Point(8, 10);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 18);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "字段名称";
            // 
            // tvOrderItem
            // 
            this.tvOrderItem.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colOrderItemCode,
            this.colOrderItemName,
            this.colOrderItemType,
            this.colDictName,
            this.colDictFilter,
            this.colShowInList,
            this.colInlist,
            this.colOrderItemIndex});
            this.tvOrderItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvOrderItem.KeyFieldName = "OrderItemCode";
            this.tvOrderItem.Location = new System.Drawing.Point(12, 37);
            this.tvOrderItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tvOrderItem.Name = "tvOrderItem";
            this.tvOrderItem.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tvOrderItem.OptionsView.ShowRoot = false;
            this.tvOrderItem.ParentFieldName = "";
            this.tvOrderItem.Size = new System.Drawing.Size(1028, 661);
            this.tvOrderItem.TabIndex = 6;
            this.tvOrderItem.TreeLevelWidth = 12;
            this.tvOrderItem.TreeLineStyle = DevExpress.XtraTreeList.LineStyle.None;
            this.tvOrderItem.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tvOrderItem_FocusedNodeChanged);
            // 
            // colOrderItemCode
            // 
            this.colOrderItemCode.Caption = "字段代码";
            this.colOrderItemCode.FieldName = "FieldCode";
            this.colOrderItemCode.Name = "colOrderItemCode";
            this.colOrderItemCode.Width = 163;
            // 
            // colOrderItemName
            // 
            this.colOrderItemName.Caption = "字段名称";
            this.colOrderItemName.FieldName = "FieldName";
            this.colOrderItemName.Name = "colOrderItemName";
            this.colOrderItemName.OptionsColumn.AllowEdit = false;
            this.colOrderItemName.OptionsColumn.AllowSort = false;
            this.colOrderItemName.Visible = true;
            this.colOrderItemName.VisibleIndex = 0;
            this.colOrderItemName.Width = 159;
            // 
            // colOrderItemType
            // 
            this.colOrderItemType.Caption = "数据类型";
            this.colOrderItemType.FieldName = "FieldType";
            this.colOrderItemType.Name = "colOrderItemType";
            this.colOrderItemType.OptionsColumn.AllowEdit = false;
            this.colOrderItemType.OptionsColumn.AllowSort = false;
            this.colOrderItemType.OptionsColumn.FixedWidth = true;
            this.colOrderItemType.Visible = true;
            this.colOrderItemType.VisibleIndex = 1;
            this.colOrderItemType.Width = 80;
            // 
            // colDictName
            // 
            this.colDictName.Caption = "数据字典";
            this.colDictName.FieldName = "FieldDictList";
            this.colDictName.Name = "colDictName";
            this.colDictName.OptionsColumn.AllowEdit = false;
            this.colDictName.OptionsColumn.AllowSort = false;
            this.colDictName.Visible = true;
            this.colDictName.VisibleIndex = 2;
            this.colDictName.Width = 164;
            // 
            // colDictFilter
            // 
            this.colDictFilter.Caption = "字典过滤";
            this.colDictFilter.FieldName = "FieldDictSql";
            this.colDictFilter.Name = "colDictFilter";
            this.colDictFilter.OptionsColumn.AllowEdit = false;
            this.colDictFilter.OptionsColumn.AllowSort = false;
            this.colDictFilter.Visible = true;
            this.colDictFilter.VisibleIndex = 3;
            this.colDictFilter.Width = 163;
            // 
            // colShowInList
            // 
            this.colShowInList.Caption = "列表显示";
            this.colShowInList.FieldName = "ShowInList";
            this.colShowInList.Name = "colShowInList";
            this.colShowInList.OptionsColumn.AllowEdit = false;
            this.colShowInList.OptionsColumn.AllowSort = false;
            this.colShowInList.OptionsColumn.FixedWidth = true;
            this.colShowInList.Width = 60;
            // 
            // colInlist
            // 
            this.colInlist.Caption = "列表显示";
            this.colInlist.FieldName = "DetCharH";
            this.colInlist.Name = "colInlist";
            this.colInlist.OptionsColumn.AllowEdit = false;
            this.colInlist.OptionsColumn.AllowSort = false;
            this.colInlist.OptionsColumn.FixedWidth = true;
            this.colInlist.Visible = true;
            this.colInlist.VisibleIndex = 4;
            this.colInlist.Width = 60;
            // 
            // colOrderItemIndex
            // 
            this.colOrderItemIndex.Caption = "顺序";
            this.colOrderItemIndex.FieldName = "FieldIndex";
            this.colOrderItemIndex.Name = "colOrderItemIndex";
            // 
            // barOrderItem
            // 
            this.barOrderItem.AutoCloseButton = true;
            this.barOrderItem.Dock = System.Windows.Forms.DockStyle.Top;
            this.barOrderItem.Location = new System.Drawing.Point(0, 0);
            this.barOrderItem.Margin = new System.Windows.Forms.Padding(5);
            this.barOrderItem.Name = "barOrderItem";
            this.barOrderItem.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("barOrderItem.NotWriteLogButtonNameList")));
            this.barOrderItem.QuickOption = false;
            this.barOrderItem.ShowItemToolTips = false;
            this.barOrderItem.Size = new System.Drawing.Size(1052, 81);
            this.barOrderItem.TabIndex = 1;
            this.barOrderItem.OnBtnAddClicked += new System.EventHandler(this.barOrderItem_OnBtnAddClicked);
            this.barOrderItem.OnBtnModifyClicked += new System.EventHandler(this.barOrderItem_OnBtnModifyClicked);
            this.barOrderItem.OnBtnDeleteClicked += new System.EventHandler(this.barOrderItem_OnBtnDeleteClicked);
            this.barOrderItem.OnBtnSaveClicked += new System.EventHandler(this.barOrderItem_OnBtnSaveClicked);
            this.barOrderItem.OnBtnCancelClicked += new System.EventHandler(this.barOrderItem_OnBtnCancelClicked);
            this.barOrderItem.OnBtnPageUpClicked += new System.EventHandler(this.barOrderItem_OnBtnPageUpClicked);
            this.barOrderItem.OnBtnPageDownClicked += new System.EventHandler(this.barOrderItem_OnBtnPageDownClicked);
            // 
            // FrmOrderType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1354, 830);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmOrderType";
            this.Text = "单证格式";
            this.Load += new System.EventHandler(this.FrmOrderType_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderType)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrderTypeName.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckeShowInList.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDictFilter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboOrderItemType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDictName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrderItemName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tvOrderItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private dcl.client.common.SysToolBar barOrderType;
        private System.Windows.Forms.Panel panel2;
        private dcl.client.common.SysToolBar barOrderItem;
        private DevExpress.XtraTreeList.TreeList tvOrderType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderTypeName;
        private DevExpress.XtraTreeList.TreeList tvOrderItem;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderItemName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderItemType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDictName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colDictFilter;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colShowInList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderTypeCode;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colInlist;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderItemCode;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colOrderItemIndex;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtOrderTypeName;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtOrderItemName;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtDictName;
        private DevExpress.XtraEditors.ComboBoxEdit cboOrderItemType;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtDictFilter;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.CheckEdit ckeShowInList;
        private System.Windows.Forms.Panel panel4;
    }
}