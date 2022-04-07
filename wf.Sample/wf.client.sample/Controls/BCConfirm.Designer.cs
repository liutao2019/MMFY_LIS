namespace dcl.client.sample
{
    partial class BCConfirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BCConfirm));
            dcl.client.sample.PrintStep printStep1 = new dcl.client.sample.PrintStep();
            dcl.client.sample.CoolStepController coolStepController1 = new dcl.client.sample.CoolStepController();
            this.panelTo = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBcfrequency = new DevExpress.XtraEditors.TextEdit();
            this.lbBarCode = new System.Windows.Forms.Label();
            this.txtBarcode = new DevExpress.XtraEditors.TextEdit();
            this.lblOp = new System.Windows.Forms.Label();
            this.lblOP2 = new System.Windows.Forms.Label();
            this.lbPackCount = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.chkPrint = new System.Windows.Forms.CheckBox();
            this.rgType = new DevExpress.XtraEditors.RadioGroup();
            this.labelUrgentFlag = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkRegFlag = new System.Windows.Forms.CheckBox();
            this.lblMes2 = new System.Windows.Forms.Label();
            this.lblMes1 = new System.Windows.Forms.Label();
            this.cmbFowardMinutes = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ctlTimeCountDown1 = new lis.client.control.ctlTimeCountDown();
            this.cbStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lueTypes = new lis.client.control.SelectDict_TypeChecks();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.gcTitle = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.patientControl = new dcl.client.sample.PatientControlForMed();
            ((System.ComponentModel.ISupportInitialize)(this.dtSub)).BeginInit();
            this.panelTo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBcfrequency.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFowardMinutes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTitle)).BeginInit();
            this.gcTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTo
            // 
            this.panelTo.Controls.Add(this.label14);
            this.panelTo.Controls.Add(this.txtBcfrequency);
            this.panelTo.Location = new System.Drawing.Point(680, 7);
            this.panelTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelTo.Name = "panelTo";
            this.panelTo.Size = new System.Drawing.Size(175, 32);
            this.panelTo.TabIndex = 29;
            this.panelTo.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 7);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 18);
            this.label14.TabIndex = 5;
            this.label14.Text = "条码批号:";
            this.label14.Visible = false;
            // 
            // txtBcfrequency
            // 
            this.txtBcfrequency.EditValue = "";
            this.txtBcfrequency.Location = new System.Drawing.Point(82, 4);
            this.txtBcfrequency.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBcfrequency.Name = "txtBcfrequency";
            this.txtBcfrequency.Properties.NullText = "请输入条码并回车";
            this.txtBcfrequency.Size = new System.Drawing.Size(90, 24);
            this.txtBcfrequency.TabIndex = 4;
            this.txtBcfrequency.ToolTip = "请输入条码并回车";
            this.txtBcfrequency.Visible = false;
            // 
            // lbBarCode
            // 
            this.lbBarCode.AutoSize = true;
            this.lbBarCode.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBarCode.ForeColor = System.Drawing.Color.Black;
            this.lbBarCode.Location = new System.Drawing.Point(7, 15);
            this.lbBarCode.Name = "lbBarCode";
            this.lbBarCode.Size = new System.Drawing.Size(114, 28);
            this.lbBarCode.TabIndex = 28;
            this.lbBarCode.Text = "条码号:";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(127, 16);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtBarcode.Properties.Appearance.Options.UseFont = true;
            this.txtBarcode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtBarcode.Properties.NullText = "请输入条码并回车";
            this.txtBarcode.Size = new System.Drawing.Size(183, 28);
            this.txtBarcode.TabIndex = 0;
            this.txtBarcode.ToolTip = "请输入条码并回车";
            this.txtBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBarcode_KeyPress);
            // 
            // lblOp
            // 
            this.lblOp.AutoSize = true;
            this.lblOp.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOp.ForeColor = System.Drawing.Color.Red;
            this.lblOp.Location = new System.Drawing.Point(519, 50);
            this.lblOp.Name = "lblOp";
            this.lblOp.Size = new System.Drawing.Size(27, 27);
            this.lblOp.TabIndex = 8;
            this.lblOp.Text = ".";
            // 
            // lblOP2
            // 
            this.lblOP2.AutoSize = true;
            this.lblOP2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOP2.ForeColor = System.Drawing.Color.Black;
            this.lblOP2.Location = new System.Drawing.Point(438, 57);
            this.lblOP2.Name = "lblOP2";
            this.lblOP2.Size = new System.Drawing.Size(75, 18);
            this.lblOP2.TabIndex = 7;
            this.lblOP2.Text = "操作人:";
            // 
            // lbPackCount
            // 
            this.lbPackCount.AutoSize = true;
            this.lbPackCount.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPackCount.Location = new System.Drawing.Point(8, 58);
            this.lbPackCount.Name = "lbPackCount";
            this.lbPackCount.Size = new System.Drawing.Size(289, 20);
            this.lbPackCount.TabIndex = 24;
            this.lbPackCount.Text = "已收取包数：0  应收取包数：0";
            this.lbPackCount.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 25F);
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(14, 31);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(35, 51);
            this.label15.TabIndex = 23;
            this.label15.Text = ".";
            this.label15.Visible = false;
            // 
            // chkPrint
            // 
            this.chkPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPrint.AutoSize = true;
            this.chkPrint.Location = new System.Drawing.Point(861, 6);
            this.chkPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkPrint.Name = "chkPrint";
            this.chkPrint.Size = new System.Drawing.Size(90, 22);
            this.chkPrint.TabIndex = 25;
            this.chkPrint.Text = "自动打印";
            this.chkPrint.UseVisualStyleBackColor = true;
            this.chkPrint.Visible = false;
            // 
            // rgType
            // 
            this.rgType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rgType.EditValue = "样本号";
            this.rgType.Location = new System.Drawing.Point(858, 32);
            this.rgType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rgType.Name = "rgType";
            this.rgType.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rgType.Properties.Appearance.Options.UseBackColor = true;
            this.rgType.Properties.Columns = 2;
            this.rgType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("样本号", "按样本号"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("序号", "按序号")});
            this.rgType.Size = new System.Drawing.Size(210, 23);
            this.rgType.TabIndex = 24;
            this.rgType.Visible = false;
            // 
            // labelUrgentFlag
            // 
            this.labelUrgentFlag.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelUrgentFlag.ForeColor = System.Drawing.Color.Red;
            this.labelUrgentFlag.Location = new System.Drawing.Point(380, 15);
            this.labelUrgentFlag.Name = "labelUrgentFlag";
            this.labelUrgentFlag.Size = new System.Drawing.Size(33, 27);
            this.labelUrgentFlag.TabIndex = 6;
            this.labelUrgentFlag.Text = "急";
            this.labelUrgentFlag.Visible = false;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTo.ForeColor = System.Drawing.Color.Red;
            this.lblTo.Location = new System.Drawing.Point(519, 12);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 27);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = ".";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(419, 19);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 18);
            this.label10.TabIndex = 3;
            this.label10.Text = "目标科室:";
            // 
            // chkRegFlag
            // 
            this.chkRegFlag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRegFlag.AutoSize = true;
            this.chkRegFlag.Location = new System.Drawing.Point(962, 6);
            this.chkRegFlag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkRegFlag.Name = "chkRegFlag";
            this.chkRegFlag.Size = new System.Drawing.Size(120, 22);
            this.chkRegFlag.TabIndex = 22;
            this.chkRegFlag.Text = "是否同步登记";
            this.chkRegFlag.UseVisualStyleBackColor = true;
            this.chkRegFlag.Visible = false;
            // 
            // lblMes2
            // 
            this.lblMes2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMes2.AutoSize = true;
            this.lblMes2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMes2.Location = new System.Drawing.Point(951, 65);
            this.lblMes2.Name = "lblMes2";
            this.lblMes2.Size = new System.Drawing.Size(127, 15);
            this.lblMes2.TabIndex = 19;
            this.lblMes2.Text = "分钟作为采集时间";
            this.lblMes2.Visible = false;
            // 
            // lblMes1
            // 
            this.lblMes1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMes1.AutoSize = true;
            this.lblMes1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMes1.Location = new System.Drawing.Point(809, 65);
            this.lblMes1.Name = "lblMes1";
            this.lblMes1.Size = new System.Drawing.Size(67, 15);
            this.lblMes1.TabIndex = 18;
            this.lblMes1.Text = "往前调整";
            this.lblMes1.Visible = false;
            // 
            // cmbFowardMinutes
            // 
            this.cmbFowardMinutes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFowardMinutes.EditValue = "0";
            this.cmbFowardMinutes.EnterMoveNextControl = true;
            this.cmbFowardMinutes.Location = new System.Drawing.Point(882, 61);
            this.cmbFowardMinutes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbFowardMinutes.Name = "cmbFowardMinutes";
            this.cmbFowardMinutes.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cmbFowardMinutes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFowardMinutes.Properties.Items.AddRange(new object[] {
            "0",
            "5",
            "10",
            "15",
            "30",
            "45",
            "60",
            "90",
            "120",
            "150",
            "180",
            "240"});
            this.cmbFowardMinutes.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbFowardMinutes.Size = new System.Drawing.Size(64, 24);
            this.cmbFowardMinutes.TabIndex = 17;
            this.cmbFowardMinutes.Visible = false;
            // 
            // ctlTimeCountDown1
            // 
            this.ctlTimeCountDown1.BackColor = System.Drawing.Color.Transparent;
            this.ctlTimeCountDown1.Location = new System.Drawing.Point(318, 21);
            this.ctlTimeCountDown1.Margin = new System.Windows.Forms.Padding(5);
            this.ctlTimeCountDown1.Name = "ctlTimeCountDown1";
            this.ctlTimeCountDown1.Size = new System.Drawing.Size(46, 22);
            this.ctlTimeCountDown1.TabIndex = 16;
            this.ctlTimeCountDown1.TimeSeconds = 120;
            this.ctlTimeCountDown1.TimeOut += new lis.client.control.ctlTimeCountDown.TimeOutEventHandler(this.ctlTimeCountDown1_TimeOut);
            // 
            // cbStatus
            // 
            this.cbStatus.EditValue = "送达";
            this.cbStatus.EnterMoveNextControl = true;
            this.cbStatus.Location = new System.Drawing.Point(134, 33);
            this.cbStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cbStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbStatus.Properties.Items.AddRange(new object[] {
            "送达",
            "收取",
            "二次送检"});
            this.cbStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbStatus.Size = new System.Drawing.Size(132, 24);
            this.cbStatus.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(4, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(124, 22);
            this.label12.TabIndex = 27;
            this.label12.Text = "签收目标科室:";
            this.label12.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(83, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 15);
            this.label13.TabIndex = 10;
            this.label13.Text = "状态:";
            // 
            // lueTypes
            // 
            this.lueTypes.displayMember = "";
            this.lueTypes.Location = new System.Drawing.Point(134, 6);
            this.lueTypes.Margin = new System.Windows.Forms.Padding(5);
            this.lueTypes.Name = "lueTypes";
            this.lueTypes.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.lueTypes.Size = new System.Drawing.Size(132, 26);
            this.lueTypes.TabIndex = 26;
            this.lueTypes.valueMember = null;
            this.lueTypes.Visible = false;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = false;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1370, 75);
            this.sysToolBar1.TabIndex = 8;
            this.sysToolBar1.OnBtnBCPrintReturnClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintReturnClicked);
            this.sysToolBar1.OnBtnPrintListClicked += new System.EventHandler(this.sysToolBar1_OnBtnPrintListClicked);
            this.sysToolBar1.OnBtnPrintListGermClicked += new System.EventHandler(this.sysToolBar1_OnBtnPrintListGermClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.sysToolBar1_OnBtnConfirmClicked);
            this.sysToolBar1.BtnClearClick += new System.EventHandler(this.sysToolBar1_BtnClearClick);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
            this.sysToolBar1.OnBtnQualityTestClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityTestClicked);
            this.sysToolBar1.BtnDeSpeClick += new System.EventHandler(this.sysToolBar1_BtnDeSpeClick);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // gcTitle
            // 
            this.gcTitle.Controls.Add(this.panelControl1);
            this.gcTitle.Controls.Add(this.panelControl2);
            this.gcTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.gcTitle.Location = new System.Drawing.Point(0, 75);
            this.gcTitle.Name = "gcTitle";
            this.gcTitle.Size = new System.Drawing.Size(1370, 121);
            this.gcTitle.TabIndex = 30;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelTo);
            this.panelControl1.Controls.Add(this.lbBarCode);
            this.panelControl1.Controls.Add(this.ctlTimeCountDown1);
            this.panelControl1.Controls.Add(this.txtBarcode);
            this.panelControl1.Controls.Add(this.cmbFowardMinutes);
            this.panelControl1.Controls.Add(this.lblOp);
            this.panelControl1.Controls.Add(this.lblMes1);
            this.panelControl1.Controls.Add(this.lblOP2);
            this.panelControl1.Controls.Add(this.lblMes2);
            this.panelControl1.Controls.Add(this.lbPackCount);
            this.panelControl1.Controls.Add(this.chkRegFlag);
            this.panelControl1.Controls.Add(this.label15);
            this.panelControl1.Controls.Add(this.label10);
            this.panelControl1.Controls.Add(this.chkPrint);
            this.panelControl1.Controls.Add(this.lblTo);
            this.panelControl1.Controls.Add(this.rgType);
            this.panelControl1.Controls.Add(this.labelUrgentFlag);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1091, 92);
            this.panelControl1.TabIndex = 2;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.label12);
            this.panelControl2.Controls.Add(this.cbStatus);
            this.panelControl2.Controls.Add(this.lueTypes);
            this.panelControl2.Controls.Add(this.label13);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl2.Location = new System.Drawing.Point(1093, 27);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(275, 92);
            this.panelControl2.TabIndex = 1;
            // 
            // patientControl
            // 
            this.patientControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientControl.Location = new System.Drawing.Point(0, 196);
            this.patientControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.patientControl.Name = "patientControl";
            this.patientControl.SelectType = dcl.client.sample.SelectType.Create;
            this.patientControl.SelectWhenNotPrint = false;
            this.patientControl.ShouldMultiSelect = true;
            this.patientControl.ShowCollectNotice = false;
            this.patientControl.Size = new System.Drawing.Size(1370, 798);
            printStep1.BaseSampMain = null;
            printStep1.Bcfrequency = null;
            printStep1.EnabledFowardMinutes = false;
            printStep1.FowardMinutes = 0;
            printStep1.MustFinishPreviousAction = true;
            printStep1.Printer = null;
            printStep1.ShouldDoAction = true;
            printStep1.ShouldEnabledBarcodeInput = true;
            printStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController1.MustFinishPreviousAction = true;
            coolStepController1.ShouldDoAction = true;
            printStep1.StepController = coolStepController1;
            printStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.patientControl.Step = printStep1;
            this.patientControl.StepType = dcl.client.sample.StepType.Print;
            this.patientControl.TabIndex = 31;
            // 
            // BCConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.patientControl);
            this.Controls.Add(this.gcTitle);
            this.Controls.Add(this.sysToolBar1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "BCConfirm";
            this.Size = new System.Drawing.Size(1370, 994);
            this.Load += new System.EventHandler(this.BCConfirm_Load);
            this.VisibleChanged += new System.EventHandler(this.BCConfirm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dtSub)).EndInit();
            this.panelTo.ResumeLayout(false);
            this.panelTo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBcfrequency.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFowardMinutes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTitle)).EndInit();
            this.gcTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTo;
        private DevExpress.XtraEditors.ComboBoxEdit cbStatus;
        private System.Windows.Forms.Label label13;
        private lis.client.control.ctlTimeCountDown ctlTimeCountDown1;
        public DevExpress.XtraEditors.TextEdit txtBarcode;
        private DevExpress.XtraEditors.ComboBoxEdit cmbFowardMinutes;
        private System.Windows.Forms.Label lblMes2;
        private System.Windows.Forms.Label lblMes1;
        private System.Windows.Forms.Label labelUrgentFlag;
        private System.Windows.Forms.Label lblOp;
        private System.Windows.Forms.Label lblOP2;
        private System.Windows.Forms.CheckBox chkRegFlag;
        private System.Windows.Forms.Label label15;

        private DevExpress.XtraEditors.RadioGroup rgType;

        private System.Windows.Forms.Label lbPackCount;
        private System.Windows.Forms.CheckBox chkPrint;
        private lis.client.control.SelectDict_TypeChecks lueTypes;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbBarCode;
        public DevExpress.XtraEditors.TextEdit txtBcfrequency;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panelTo;
        private DevExpress.XtraEditors.GroupControl gcTitle;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        public PatientControlForMed patientControl;
    }
}
