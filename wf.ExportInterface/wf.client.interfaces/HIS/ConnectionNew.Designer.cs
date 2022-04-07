namespace dcl.client.interfaces
{
    partial class ConnectionNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionNew));
            this.bsDataInterConn = new System.Windows.Forms.BindingSource(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAddress = new System.Windows.Forms.RichTextBox();
            this.txtCatelog = new DevExpress.XtraEditors.TextEdit();
            this.lblCatelog = new System.Windows.Forms.Label();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSql = new DevExpress.XtraEditors.MemoEdit();
            this.btnTestConnenct = new DevExpress.XtraEditors.SimpleButton();
            this.txtDriver = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtDialet = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbConnectType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLogin = new DevExpress.XtraEditors.TextEdit();
            this.txtLoginPass = new DevExpress.XtraEditors.TextEdit();
            this.lblRemark = new System.Windows.Forms.Label();
            this.lblDriver = new System.Windows.Forms.Label();
            this.lblDialet = new System.Windows.Forms.Label();
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.lblLoginPass = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.bsDataInterConn)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCatelog.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSql.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriver.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDialet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConnectType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bsDataInterConn
            // 
            this.bsDataInterConn.DataSource = typeof(dcl.entity.EntityDicDataInterfaceConnection);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1107, 803);
            this.panel3.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridControl1);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1107, 735);
            this.panel1.TabIndex = 8;
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsDataInterConn;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(360, 0);
            this.gridControl1.MainView = this.gridView3;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.gridControl1.Size = new System.Drawing.Size(747, 735);
            this.gridControl1.TabIndex = 51;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3,
            this.gridView1});
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn1,
            this.gridColumn9,
            this.gridColumn10});
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.GridControl = this.gridControl1;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsBehavior.Editable = false;
            this.gridView3.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.OptionsView.ShowIndicator = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "ID";
            this.gridColumn2.FieldName = "ConnId";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 61;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "名称";
            this.gridColumn3.FieldName = "ConnName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 79;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "编码";
            this.gridColumn1.FieldName = "ConnCode";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            this.gridColumn1.Width = 104;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "连接类型";
            this.gridColumn9.FieldName = "ConnType";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn9.OptionsColumn.FixedWidth = true;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 3;
            this.gridColumn9.Width = 91;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "地址";
            this.gridColumn10.FieldName = "ConnAddress";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.FixedWidth = true;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 4;
            this.gridColumn10.Width = 188;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.LinesCount = 100;
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            this.repositoryItemMemoEdit1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(360, 735);
            this.panel4.TabIndex = 50;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAddress);
            this.groupBox1.Controls.Add(this.txtCatelog);
            this.groupBox1.Controls.Add(this.lblCatelog);
            this.groupBox1.Controls.Add(this.comboBoxEdit1);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textEdit1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtSql);
            this.groupBox1.Controls.Add(this.btnTestConnenct);
            this.groupBox1.Controls.Add(this.txtDriver);
            this.groupBox1.Controls.Add(this.txtDialet);
            this.groupBox1.Controls.Add(this.cbConnectType);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtLogin);
            this.groupBox1.Controls.Add(this.txtLoginPass);
            this.groupBox1.Controls.Add(this.lblRemark);
            this.groupBox1.Controls.Add(this.lblDriver);
            this.groupBox1.Controls.Add(this.lblDialet);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.lblLoginPass);
            this.groupBox1.Controls.Add(this.lblLogin);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblAddress);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 735);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本信息";
            // 
            // txtAddress
            // 
            this.txtAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsDataInterConn, "ConnAddress", true));
            this.txtAddress.Location = new System.Drawing.Point(105, 207);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(240, 96);
            this.txtAddress.TabIndex = 97;
            this.txtAddress.Text = "";
            // 
            // txtCatelog
            // 
            this.txtCatelog.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnDbCatelog", true));
            this.txtCatelog.EnterMoveNextControl = true;
            this.txtCatelog.Location = new System.Drawing.Point(105, 319);
            this.txtCatelog.Name = "txtCatelog";
            this.txtCatelog.Size = new System.Drawing.Size(240, 20);
            this.txtCatelog.TabIndex = 96;
            // 
            // lblCatelog
            // 
            this.lblCatelog.AutoSize = true;
            this.lblCatelog.Location = new System.Drawing.Point(12, 323);
            this.lblCatelog.Name = "lblCatelog";
            this.lblCatelog.Size = new System.Drawing.Size(71, 14);
            this.lblCatelog.TabIndex = 95;
            this.lblCatelog.Text = "数   据   库:";
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnRunningSide", true));
            this.comboBoxEdit1.EditValue = "";
            this.comboBoxEdit1.EnterMoveNextControl = true;
            this.comboBoxEdit1.Location = new System.Drawing.Point(105, 133);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Size = new System.Drawing.Size(240, 20);
            this.comboBoxEdit1.TabIndex = 93;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 136);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 14);
            this.label12.TabIndex = 94;
            this.label12.Text = "运   行   端:";
            // 
            // textEdit1
            // 
            this.textEdit1.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnName", true));
            this.textEdit1.EnterMoveNextControl = true;
            this.textEdit1.Location = new System.Drawing.Point(105, 100);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Size = new System.Drawing.Size(240, 20);
            this.textEdit1.TabIndex = 91;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 103);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 14);
            this.label11.TabIndex = 92;
            this.label11.Text = "名         称:";
            // 
            // txtSql
            // 
            this.txtSql.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnDesc", true));
            this.txtSql.EnterMoveNextControl = true;
            this.txtSql.Location = new System.Drawing.Point(105, 498);
            this.txtSql.Name = "txtSql";
            this.txtSql.Size = new System.Drawing.Size(240, 127);
            this.txtSql.TabIndex = 7;
            // 
            // btnTestConnenct
            // 
            this.btnTestConnenct.Location = new System.Drawing.Point(15, 638);
            this.btnTestConnenct.Name = "btnTestConnenct";
            this.btnTestConnenct.Size = new System.Drawing.Size(330, 37);
            this.btnTestConnenct.TabIndex = 9;
            this.btnTestConnenct.Text = "测试连接";
            this.btnTestConnenct.Click += new System.EventHandler(this.btnTestConnenct_Click);
            // 
            // txtDriver
            // 
            this.txtDriver.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsDataInterConn, "ConnDbDriver", true));
            this.txtDriver.EditValue = "院网";
            this.txtDriver.EnterMoveNextControl = true;
            this.txtDriver.Location = new System.Drawing.Point(105, 354);
            this.txtDriver.Name = "txtDriver";
            this.txtDriver.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDriver.Properties.Items.AddRange(new object[] {
            "院网",
            "住院条码",
            "门诊条码",
            "体检条码",
            "第二体检条码",
            "体检检查条码",
            "医生资料",
            "科室资料",
            "用户资料"});
            this.txtDriver.Size = new System.Drawing.Size(240, 20);
            this.txtDriver.TabIndex = 6;
            this.txtDriver.SelectedIndexChanged += new System.EventHandler(this.cbInterfaceType_SelectedIndexChanged);
            // 
            // txtDialet
            // 
            this.txtDialet.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsDataInterConn, "ConnDbDialet", true));
            this.txtDialet.EditValue = "SQL";
            this.txtDialet.EnterMoveNextControl = true;
            this.txtDialet.Location = new System.Drawing.Point(105, 388);
            this.txtDialet.Name = "txtDialet";
            this.txtDialet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDialet.Properties.Items.AddRange(new object[] {
            "SQL",
            "存储过程"});
            this.txtDialet.Size = new System.Drawing.Size(240, 20);
            this.txtDialet.TabIndex = 6;
            this.txtDialet.SelectedIndexChanged += new System.EventHandler(this.cbInterfaceType_SelectedIndexChanged);
            // 
            // cbConnectType
            // 
            this.cbConnectType.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnType", true));
            this.cbConnectType.EditValue = "";
            this.cbConnectType.EnterMoveNextControl = true;
            this.cbConnectType.Location = new System.Drawing.Point(105, 169);
            this.cbConnectType.Name = "cbConnectType";
            this.cbConnectType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbConnectType.Size = new System.Drawing.Size(240, 20);
            this.cbConnectType.TabIndex = 2;
            this.cbConnectType.SelectedIndexChanged += new System.EventHandler(this.cbConnectType_SelectedIndexChanged);
            // 
            // txtName
            // 
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "SysModule", true));
            this.txtName.EnterMoveNextControl = true;
            this.txtName.Location = new System.Drawing.Point(105, 29);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(240, 20);
            this.txtName.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 33);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 14);
            this.label8.TabIndex = 5;
            this.label8.Text = "模         块:";
            // 
            // txtLogin
            // 
            this.txtLogin.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnLogin", true));
            this.txtLogin.EnterMoveNextControl = true;
            this.txtLogin.Location = new System.Drawing.Point(105, 422);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(240, 20);
            this.txtLogin.TabIndex = 4;
            // 
            // txtLoginPass
            // 
            this.txtLoginPass.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnPass", true));
            this.txtLoginPass.EnterMoveNextControl = true;
            this.txtLoginPass.Location = new System.Drawing.Point(105, 460);
            this.txtLoginPass.Name = "txtLoginPass";
            this.txtLoginPass.Properties.PasswordChar = '*';
            this.txtLoginPass.Size = new System.Drawing.Size(240, 20);
            this.txtLoginPass.TabIndex = 5;
            // 
            // lblRemark
            // 
            this.lblRemark.AutoSize = true;
            this.lblRemark.Location = new System.Drawing.Point(12, 540);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(71, 14);
            this.lblRemark.TabIndex = 3;
            this.lblRemark.Text = "备         注:";
            // 
            // lblDriver
            // 
            this.lblDriver.AutoSize = true;
            this.lblDriver.Location = new System.Drawing.Point(12, 357);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(71, 14);
            this.lblDriver.TabIndex = 3;
            this.lblDriver.Text = "数 据 驱 动:";
            // 
            // lblDialet
            // 
            this.lblDialet.AutoSize = true;
            this.lblDialet.Location = new System.Drawing.Point(12, 391);
            this.lblDialet.Name = "lblDialet";
            this.lblDialet.Size = new System.Drawing.Size(71, 14);
            this.lblDialet.TabIndex = 3;
            this.lblDialet.Text = "数据库类别:";
            // 
            // txtCode
            // 
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsDataInterConn, "ConnCode", true));
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(105, 65);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(240, 20);
            this.txtCode.TabIndex = 1;
            // 
            // lblLoginPass
            // 
            this.lblLoginPass.AutoSize = true;
            this.lblLoginPass.Location = new System.Drawing.Point(12, 463);
            this.lblLoginPass.Name = "lblLoginPass";
            this.lblLoginPass.Size = new System.Drawing.Size(71, 14);
            this.lblLoginPass.TabIndex = 2;
            this.lblLoginPass.Text = "密         码:";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(12, 425);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(71, 14);
            this.lblLogin.TabIndex = 3;
            this.lblLogin.Text = "登   录   名:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "类         型:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "编         码:";
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Location = new System.Drawing.Point(12, 219);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(71, 14);
            this.lblAddress.TabIndex = 2;
            this.lblAddress.Text = "数据库地址:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sysToolBar1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1107, 68);
            this.panel2.TabIndex = 7;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1107, 67);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.sysToolBar1_OnBtnAddClicked);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar1_OnBtnCancelClicked);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.sysToolBar1_OnBtnRefreshClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar1_OnBtnExportClicked);
            this.sysToolBar1.OnBtnImportClicked += new System.EventHandler(this.sysToolBar1_OnBtnImportClicked);
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);
            this.sysToolBar1.BtnDeRefClick += new System.EventHandler(this.sysToolBar1_BtnDeRefClick);
            // 
            // ConnectionNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConnectionNew";
            this.Size = new System.Drawing.Size(1107, 803);
            this.Load += new System.EventHandler(this.ConnectionNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bsDataInterConn)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCatelog.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSql.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDriver.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDialet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConnectType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource bsDataInterConn;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.TextEdit txtLogin;
        private DevExpress.XtraEditors.TextEdit txtCode;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblAddress;
        private DevExpress.XtraEditors.TextEdit txtLoginPass;
        private System.Windows.Forms.Label lblDialet;
        private System.Windows.Forms.Label lblLoginPass;
        private DevExpress.XtraEditors.TextEdit txtName;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ComboBoxEdit cbConnectType;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnTestConnenct;
        private DevExpress.XtraEditors.MemoEdit txtSql;
        private System.Windows.Forms.Label lblRemark;
        private DevExpress.XtraEditors.ComboBoxEdit txtDriver;
        private System.Windows.Forms.Label lblDriver;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.ComboBoxEdit txtDialet;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.TextEdit txtCatelog;
        private System.Windows.Forms.Label lblCatelog;
        private System.Windows.Forms.RichTextBox txtAddress;
        private common.SysToolBar sysToolBar1;
    }
}
