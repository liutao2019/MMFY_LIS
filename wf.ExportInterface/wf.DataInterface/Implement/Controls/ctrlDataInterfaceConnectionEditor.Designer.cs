namespace Lib.DataInterface.Implement
{
    partial class ctrlDataInterfaceConnectionEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlDataInterfaceConnectionEditor));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddNew = new System.Windows.Forms.ToolStripButton();
            this.btnModify = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnCancel = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnTestConnection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCopy = new System.Windows.Forms.ToolStripButton();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.btnImport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridList = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsList = new System.Windows.Forms.BindingSource(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtDesc = new System.Windows.Forms.RichTextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtLoginPass = new System.Windows.Forms.TextBox();
            this.txtType = new System.Windows.Forms.ComboBox();
            this.txtAddress = new System.Windows.Forms.RichTextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtDriver = new System.Windows.Forms.ComboBox();
            this.txtDialet = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblLoginPass = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblDriver = new System.Windows.Forms.Label();
            this.txtCatelog = new System.Windows.Forms.TextBox();
            this.lblCatelog = new System.Windows.Forms.Label();
            this.lblDialet = new System.Windows.Forms.Label();
            this.pnlModuleName = new System.Windows.Forms.Panel();
            this.txtModule = new System.Windows.Forms.TextBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.txtRunningSide = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.bsItem = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsList)).BeginInit();
            this.panel2.SuspendLayout();
            this.pnlModuleName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 12F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddNew,
            this.btnModify,
            this.btnDelete,
            this.btnSave,
            this.btnCancel,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnTestConnection,
            this.toolStripSeparator2,
            this.btnCopy,
            this.btnExport,
            this.btnImport,
            this.toolStripSeparator3,
            this.btnHelp,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(812, 39);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddNew
            // 
            this.btnAddNew.Image = global::Lib.DataInterface.Properties.Resources.新增;
            this.btnAddNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(44, 36);
            this.btnAddNew.Text = "新增";
            this.btnAddNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnModify
            // 
            this.btnModify.Image = global::Lib.DataInterface.Properties.Resources.修改;
            this.btnModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(44, 36);
            this.btnModify.Text = "修改";
            this.btnModify.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Lib.DataInterface.Properties.Resources.删除;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(44, 36);
            this.btnDelete.Text = "删除";
            this.btnDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::Lib.DataInterface.Properties.Resources.保存;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(44, 36);
            this.btnSave.Text = "保存";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Lib.DataInterface.Properties.Resources.取消;
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(44, 36);
            this.btnCancel.Text = "放弃";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(44, 36);
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Image = global::Lib.DataInterface.Properties.Resources.连接;
            this.btnTestConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(76, 36);
            this.btnTestConnection.Text = "测试连接";
            this.btnTestConnection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // btnCopy
            // 
            this.btnCopy.Image = ((System.Drawing.Image)(resources.GetObject("btnCopy.Image")));
            this.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(44, 36);
            this.btnCopy.Text = "复制";
            this.btnCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(44, 36);
            this.btnExport.Text = "导出";
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(44, 36);
            this.btnImport.Text = "导入";
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // btnHelp
            // 
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(44, 36);
            this.btnHelp.Text = "帮助";
            this.btnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::Lib.DataInterface.Properties.Resources.关闭;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(44, 36);
            this.btnClose.Text = "关闭";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnClose.ToolTipText = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(162)))), ((int)(((byte)(218)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 39);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.pnlModuleName);
            this.splitContainer1.Size = new System.Drawing.Size(812, 544);
            this.splitContainer1.SplitterDistance = 554;
            this.splitContainer1.TabIndex = 2;
            // 
            // gridList
            // 
            this.gridList.AllowUserToAddRows = false;
            this.gridList.AllowUserToDeleteRows = false;
            this.gridList.AutoGenerateColumns = false;
            this.gridList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column1,
            this.Column4,
            this.Column2,
            this.Column3});
            this.gridList.DataSource = this.bsList;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridList.Location = new System.Drawing.Point(0, 0);
            this.gridList.Name = "gridList";
            this.gridList.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridList.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridList.RowHeadersWidth = 15;
            this.gridList.RowTemplate.Height = 23;
            this.gridList.Size = new System.Drawing.Size(554, 544);
            this.gridList.TabIndex = 0;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "conn_id";
            this.Column5.HeaderText = "ID";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 60;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "conn_name";
            this.Column1.HeaderText = "名称";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 120;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "conn_code";
            this.Column4.HeaderText = "编码";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 90;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "conn_type";
            this.Column2.HeaderText = "连接类型";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "conn_address";
            this.Column3.HeaderText = "地址";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 200;
            // 
            // bsList
            // 
            this.bsList.CurrentChanged += new System.EventHandler(this.bsList_CurrentChanged);
            this.bsList.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.bsList_AddingNew);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtRunningSide);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtCode);
            this.panel2.Controls.Add(this.txtDesc);
            this.panel2.Controls.Add(this.txtName);
            this.panel2.Controls.Add(this.txtLoginPass);
            this.panel2.Controls.Add(this.txtType);
            this.panel2.Controls.Add(this.txtAddress);
            this.panel2.Controls.Add(this.txtLogin);
            this.panel2.Controls.Add(this.txtDriver);
            this.panel2.Controls.Add(this.txtDialet);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.lblLoginPass);
            this.panel2.Controls.Add(this.lblAddress);
            this.panel2.Controls.Add(this.lblLogin);
            this.panel2.Controls.Add(this.lblDriver);
            this.panel2.Controls.Add(this.txtCatelog);
            this.panel2.Controls.Add(this.lblCatelog);
            this.panel2.Controls.Add(this.lblDialet);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(254, 514);
            this.panel2.TabIndex = 40;
            // 
            // txtCode
            // 
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "conn_code", true));
            this.txtCode.Location = new System.Drawing.Point(91, 3);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(156, 26);
            this.txtCode.TabIndex = 0;
            // 
            // txtDesc
            // 
            this.txtDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "conn_desc", true));
            this.txtDesc.Location = new System.Drawing.Point(91, 353);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(155, 115);
            this.txtDesc.TabIndex = 10;
            this.txtDesc.Text = "";
            // 
            // txtName
            // 
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "conn_name", true));
            this.txtName.Location = new System.Drawing.Point(91, 34);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(156, 26);
            this.txtName.TabIndex = 1;
            // 
            // txtLoginPass
            // 
            this.txtLoginPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLoginPass.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "conn_pass", true));
            this.txtLoginPass.Location = new System.Drawing.Point(91, 322);
            this.txtLoginPass.Name = "txtLoginPass";
            this.txtLoginPass.PasswordChar = '*';
            this.txtLoginPass.Size = new System.Drawing.Size(156, 26);
            this.txtLoginPass.TabIndex = 9;
            // 
            // txtType
            // 
            this.txtType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsItem, "conn_type", true));
            this.txtType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtType.FormattingEnabled = true;
            this.txtType.Location = new System.Drawing.Point(91, 95);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(156, 24);
            this.txtType.TabIndex = 3;
            this.txtType.SelectedIndexChanged += new System.EventHandler(this.txtType_SelectedIndexChanged);
            // 
            // txtAddress
            // 
            this.txtAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "conn_address", true));
            this.txtAddress.Location = new System.Drawing.Point(91, 124);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(156, 73);
            this.txtAddress.TabIndex = 4;
            this.txtAddress.Text = "";
            // 
            // txtLogin
            // 
            this.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLogin.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "conn_login", true));
            this.txtLogin.Location = new System.Drawing.Point(91, 291);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(156, 26);
            this.txtLogin.TabIndex = 8;
            // 
            // txtDriver
            // 
            this.txtDriver.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsItem, "conn_db_driver", true));
            this.txtDriver.FormattingEnabled = true;
            this.txtDriver.Location = new System.Drawing.Point(91, 233);
            this.txtDriver.Name = "txtDriver";
            this.txtDriver.Size = new System.Drawing.Size(156, 24);
            this.txtDriver.TabIndex = 6;
            // 
            // txtDialet
            // 
            this.txtDialet.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsItem, "conn_db_dialet", true));
            this.txtDialet.FormattingEnabled = true;
            this.txtDialet.Location = new System.Drawing.Point(91, 262);
            this.txtDialet.Name = "txtDialet";
            this.txtDialet.Size = new System.Drawing.Size(156, 24);
            this.txtDialet.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 38;
            this.label4.Text = "编  码：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "名  称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "类  型：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 352);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "备  注：";
            // 
            // lblLoginPass
            // 
            this.lblLoginPass.AutoSize = true;
            this.lblLoginPass.Location = new System.Drawing.Point(5, 323);
            this.lblLoginPass.Name = "lblLoginPass";
            this.lblLoginPass.Size = new System.Drawing.Size(88, 16);
            this.lblLoginPass.TabIndex = 34;
            this.lblLoginPass.Text = "登录密码：";
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(5, 124);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(84, 70);
            this.lblAddress.TabIndex = 25;
            this.lblAddress.Text = "地址：";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(5, 294);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(72, 16);
            this.lblLogin.TabIndex = 32;
            this.lblLogin.Text = "登录名：";
            // 
            // lblDriver
            // 
            this.lblDriver.AutoSize = true;
            this.lblDriver.Location = new System.Drawing.Point(5, 236);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(88, 16);
            this.lblDriver.TabIndex = 26;
            this.lblDriver.Text = "数据驱动：";
            // 
            // txtCatelog
            // 
            this.txtCatelog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCatelog.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "conn_db_catelog", true));
            this.txtCatelog.Location = new System.Drawing.Point(91, 202);
            this.txtCatelog.Name = "txtCatelog";
            this.txtCatelog.Size = new System.Drawing.Size(156, 26);
            this.txtCatelog.TabIndex = 5;
            // 
            // lblCatelog
            // 
            this.lblCatelog.AutoSize = true;
            this.lblCatelog.Location = new System.Drawing.Point(5, 207);
            this.lblCatelog.Name = "lblCatelog";
            this.lblCatelog.Size = new System.Drawing.Size(88, 16);
            this.lblCatelog.TabIndex = 30;
            this.lblCatelog.Text = "数据名称：";
            // 
            // lblDialet
            // 
            this.lblDialet.AutoSize = true;
            this.lblDialet.Location = new System.Drawing.Point(5, 265);
            this.lblDialet.Name = "lblDialet";
            this.lblDialet.Size = new System.Drawing.Size(88, 16);
            this.lblDialet.TabIndex = 28;
            this.lblDialet.Text = "数据库类别";
            // 
            // pnlModuleName
            // 
            this.pnlModuleName.Controls.Add(this.txtModule);
            this.pnlModuleName.Controls.Add(this.lblModule);
            this.pnlModuleName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlModuleName.Location = new System.Drawing.Point(0, 0);
            this.pnlModuleName.Name = "pnlModuleName";
            this.pnlModuleName.Size = new System.Drawing.Size(254, 30);
            this.pnlModuleName.TabIndex = 39;
            // 
            // txtModule
            // 
            this.txtModule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtModule.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsItem, "sys_module", true));
            this.txtModule.Location = new System.Drawing.Point(91, 4);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(156, 26);
            this.txtModule.TabIndex = 0;
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(4, 10);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(72, 16);
            this.lblModule.TabIndex = 23;
            this.lblModule.Text = "模  块：";
            // 
            // txtRunningSide
            // 
            this.txtRunningSide.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.bsItem, "conn_running_side", true));
            this.txtRunningSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtRunningSide.FormattingEnabled = true;
            this.txtRunningSide.Location = new System.Drawing.Point(91, 66);
            this.txtRunningSide.Name = "txtRunningSide";
            this.txtRunningSide.Size = new System.Drawing.Size(156, 24);
            this.txtRunningSide.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 39;
            this.label5.Text = "运行端：";
            // 
            // bsItem
            // 
            this.bsItem.DataSource = typeof(Lib.DataInterface.Implement.EntityDictDataInterfaceConnection);
            // 
            // ctrlDataInterfaceConnectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 12F);
            this.Name = "ctrlDataInterfaceConnectionEditor";
            this.Size = new System.Drawing.Size(812, 583);
            this.Load += new System.EventHandler(this.ctrlDataInterfaceConnectionEditor_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnlModuleName.ResumeLayout(false);
            this.pnlModuleName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddNew;
        private System.Windows.Forms.ToolStripButton btnModify;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnCancel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnTestConnection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnCopy;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.ToolStripButton btnImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnHelp;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridList;
        private System.Windows.Forms.RichTextBox txtDesc;
        private System.Windows.Forms.TextBox txtLoginPass;
        private System.Windows.Forms.Label lblLoginPass;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.TextBox txtCatelog;
        private System.Windows.Forms.Label lblCatelog;
        private System.Windows.Forms.ComboBox txtDialet;
        private System.Windows.Forms.Label lblDialet;
        private System.Windows.Forms.ComboBox txtDriver;
        private System.Windows.Forms.Label lblDriver;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.RichTextBox txtAddress;
        private System.Windows.Forms.TextBox txtModule;
        private System.Windows.Forms.Label lblModule;
        private System.Windows.Forms.ComboBox txtType;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource bsList;
        private System.Windows.Forms.BindingSource bsItem;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.ComboBox txtRunningSide;
        private System.Windows.Forms.Label label5;
    }
}
