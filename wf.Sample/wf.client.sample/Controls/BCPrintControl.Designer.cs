using lis.client.control;
namespace dcl.client.sample
{
    partial class BCPrintControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BCPrintControl));
            dcl.client.sample.PrintStep printStep1 = new dcl.client.sample.PrintStep();
            dcl.client.sample.CoolStepController coolStepController1 = new dcl.client.sample.CoolStepController();
            this.pnlTop = new DevExpress.XtraEditors.PanelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmBloodStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.cmStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panelInpatient = new System.Windows.Forms.Panel();
            this.selectDict_Depart1 = new dcl.client.control.SelectDicPubDept();
            this.lblBedNum = new System.Windows.Forms.Label();
            this.lblDept = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.txtBedNum = new DevExpress.XtraEditors.TextEdit();
            this.txtInpatientID = new DevExpress.XtraEditors.TextEdit();
            this.panelOutpatient = new System.Windows.Forms.Panel();
            this.txtOutPatientsEnd = new DevExpress.XtraEditors.TextEdit();
            this.txtEmpCompanyDept = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblOutPatients = new System.Windows.Forms.Label();
            this.lblEmpCompanyName = new System.Windows.Forms.Label();
            this.cbTypeID = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtOutPatients = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ckPrePlaceBarcode = new DevExpress.XtraEditors.CheckEdit();
            this.adviceTime1 = new dcl.client.sample.AdviceTime();
            this.lblTitle = new System.Windows.Forms.Label();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.chkAutoPrintBarcode = new DevExpress.XtraEditors.CheckEdit();
            this.chkAutoPrintReturnBarcode = new DevExpress.XtraEditors.CheckEdit();
            this.lblPrePlaceBarcode = new System.Windows.Forms.Label();
            this.txtPrePlaceBarcode = new DevExpress.XtraEditors.TextEdit();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.scrollingText1 = new lis.client.control.ScrollingTextControl.ScrollingText();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.patientControl = new dcl.client.sample.PatientControlForMed();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.xtraGridBlending1 = new DevExpress.XtraGrid.Blending.XtraGridBlending();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::dcl.client.sample.Controls.WaitForm1), true, true, typeof(System.Windows.Forms.UserControl));
            ((System.ComponentModel.ISupportInitialize)(this.dtSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmBloodStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmStatus.Properties)).BeginInit();
            this.panelInpatient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBedNum.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInpatientID.Properties)).BeginInit();
            this.panelOutpatient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutPatientsEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpCompanyDept.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTypeID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutPatients.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckPrePlaceBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoPrintBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoPrintReturnBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrePlaceBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.panel2);
            this.pnlTop.Controls.Add(this.ckPrePlaceBarcode);
            this.pnlTop.Controls.Add(this.adviceTime1);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.sysToolBar1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1714, 127);
            this.pnlTop.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panelInpatient);
            this.panel2.Controls.Add(this.panelOutpatient);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(335, 84);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1377, 41);
            this.panel2.TabIndex = 12;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmBloodStatus);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.cmStatus);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(1116, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(261, 41);
            this.panel3.TabIndex = 13;
            // 
            // cmBloodStatus
            // 
            this.cmBloodStatus.EditValue = "全部";
            this.cmBloodStatus.Location = new System.Drawing.Point(170, 6);
            this.cmBloodStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmBloodStatus.Name = "cmBloodStatus";
            this.cmBloodStatus.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmBloodStatus.Properties.Appearance.Options.UseFont = true;
            this.cmBloodStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmBloodStatus.Properties.Items.AddRange(new object[] {
            "全部",
            "抽血项目",
            "非抽血项目"});
            this.cmBloodStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmBloodStatus.Size = new System.Drawing.Size(79, 28);
            this.cmBloodStatus.TabIndex = 110;
            this.cmBloodStatus.SelectedIndexChanged += new System.EventHandler(this.cmStatus2_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(2, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 25;
            this.label1.Text = "状态:";
            // 
            // cmStatus
            // 
            this.cmStatus.EditValue = "全部";
            this.cmStatus.Location = new System.Drawing.Point(61, 6);
            this.cmStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmStatus.Name = "cmStatus";
            this.cmStatus.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmStatus.Properties.Appearance.Options.UseFont = true;
            this.cmStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmStatus.Properties.Items.AddRange(new object[] {
            "全部",
            "未打印",
            "未采集",
            "已打印",
            "已收取",
            "已送达",
            "已签收",
            "已报告"});
            this.cmStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmStatus.Size = new System.Drawing.Size(103, 28);
            this.cmStatus.TabIndex = 24;
            this.cmStatus.SelectedIndexChanged += new System.EventHandler(this.Filter_CheckedChanged);
            // 
            // panelInpatient
            // 
            this.panelInpatient.Controls.Add(this.selectDict_Depart1);
            this.panelInpatient.Controls.Add(this.lblBedNum);
            this.panelInpatient.Controls.Add(this.lblDept);
            this.panelInpatient.Controls.Add(this.lblID);
            this.panelInpatient.Controls.Add(this.txtBedNum);
            this.panelInpatient.Controls.Add(this.txtInpatientID);
            this.panelInpatient.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelInpatient.Location = new System.Drawing.Point(631, 0);
            this.panelInpatient.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelInpatient.Name = "panelInpatient";
            this.panelInpatient.Size = new System.Drawing.Size(485, 41);
            this.panelInpatient.TabIndex = 2;
            this.panelInpatient.Visible = false;
            // 
            // selectDict_Depart1
            // 
            this.selectDict_Depart1.AddEmptyRow = true;
            this.selectDict_Depart1.AutoScroll = true;
            this.selectDict_Depart1.BindByValue = false;
            this.selectDict_Depart1.colDisplay = "";
            this.selectDict_Depart1.colExtend1 = "DeptCode";
            this.selectDict_Depart1.colInCode = "";
            this.selectDict_Depart1.colPY = "";
            this.selectDict_Depart1.colSeq = "";
            this.selectDict_Depart1.colValue = "";
            this.selectDict_Depart1.colWB = "";
            this.selectDict_Depart1.displayMember = null;
            this.selectDict_Depart1.EnterMoveNext = true;
            this.selectDict_Depart1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDict_Depart1.KeyUpDownMoveNext = false;
            this.selectDict_Depart1.LoadDataOnDesignMode = true;
            this.selectDict_Depart1.Location = new System.Drawing.Point(53, 6);
            this.selectDict_Depart1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectDict_Depart1.MaximumSize = new System.Drawing.Size(571, 27);
            this.selectDict_Depart1.MinimumSize = new System.Drawing.Size(57, 27);
            this.selectDict_Depart1.Name = "selectDict_Depart1";
            this.selectDict_Depart1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDict_Depart1.PFont = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectDict_Depart1.Readonly = false;
            this.selectDict_Depart1.SaveSourceID = false;
            this.selectDict_Depart1.SelectFilter = null;
            this.selectDict_Depart1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDict_Depart1.SelectOnly = true;
            this.selectDict_Depart1.Size = new System.Drawing.Size(118, 27);
            this.selectDict_Depart1.TabIndex = 5;
            this.selectDict_Depart1.UseExtend = false;
            this.selectDict_Depart1.valueMember = null;
            this.selectDict_Depart1.Visible = false;
            // 
            // lblBedNum
            // 
            this.lblBedNum.AutoSize = true;
            this.lblBedNum.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBedNum.Location = new System.Drawing.Point(177, 12);
            this.lblBedNum.Name = "lblBedNum";
            this.lblBedNum.Size = new System.Drawing.Size(44, 18);
            this.lblBedNum.TabIndex = 2;
            this.lblBedNum.Text = "床号";
            this.lblBedNum.Visible = false;
            // 
            // lblDept
            // 
            this.lblDept.AutoSize = true;
            this.lblDept.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDept.Location = new System.Drawing.Point(3, 12);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(44, 18);
            this.lblDept.TabIndex = 2;
            this.lblDept.Text = "科室";
            this.lblDept.Visible = false;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblID.Location = new System.Drawing.Point(306, 12);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(62, 18);
            this.lblID.TabIndex = 2;
            this.lblID.Text = "住院号";
            // 
            // txtBedNum
            // 
            this.txtBedNum.Location = new System.Drawing.Point(227, 6);
            this.txtBedNum.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBedNum.Name = "txtBedNum";
            this.txtBedNum.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBedNum.Properties.Appearance.Options.UseFont = true;
            this.txtBedNum.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtBedNum.Size = new System.Drawing.Size(73, 28);
            this.txtBedNum.TabIndex = 0;
            this.txtBedNum.Visible = false;
            this.txtBedNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInpatientID_KeyPress);
            // 
            // txtInpatientID
            // 
            this.txtInpatientID.Location = new System.Drawing.Point(374, 6);
            this.txtInpatientID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInpatientID.Name = "txtInpatientID";
            this.txtInpatientID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInpatientID.Properties.Appearance.Options.UseFont = true;
            this.txtInpatientID.Size = new System.Drawing.Size(93, 28);
            this.txtInpatientID.TabIndex = 0;
            this.txtInpatientID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInpatientID_KeyPress);
            // 
            // panelOutpatient
            // 
            this.panelOutpatient.Controls.Add(this.txtOutPatientsEnd);
            this.panelOutpatient.Controls.Add(this.txtEmpCompanyDept);
            this.panelOutpatient.Controls.Add(this.lblOutPatients);
            this.panelOutpatient.Controls.Add(this.lblEmpCompanyName);
            this.panelOutpatient.Controls.Add(this.cbTypeID);
            this.panelOutpatient.Controls.Add(this.txtOutPatients);
            this.panelOutpatient.Controls.Add(this.label5);
            this.panelOutpatient.Controls.Add(this.label4);
            this.panelOutpatient.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelOutpatient.Location = new System.Drawing.Point(0, 0);
            this.panelOutpatient.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelOutpatient.Name = "panelOutpatient";
            this.panelOutpatient.Size = new System.Drawing.Size(631, 41);
            this.panelOutpatient.TabIndex = 1;
            this.panelOutpatient.Visible = false;
            // 
            // txtOutPatientsEnd
            // 
            this.txtOutPatientsEnd.Location = new System.Drawing.Point(331, 7);
            this.txtOutPatientsEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOutPatientsEnd.Name = "txtOutPatientsEnd";
            this.txtOutPatientsEnd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutPatientsEnd.Properties.Appearance.Options.UseFont = true;
            this.txtOutPatientsEnd.Size = new System.Drawing.Size(96, 28);
            this.txtOutPatientsEnd.TabIndex = 109;
            this.txtOutPatientsEnd.Visible = false;
            this.txtOutPatientsEnd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEdit1_KeyPress);
            // 
            // txtEmpCompanyDept
            // 
            this.txtEmpCompanyDept.EditValue = "";
            this.txtEmpCompanyDept.Location = new System.Drawing.Point(518, 7);
            this.txtEmpCompanyDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmpCompanyDept.Name = "txtEmpCompanyDept";
            this.txtEmpCompanyDept.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmpCompanyDept.Properties.Appearance.Options.UseFont = true;
            this.txtEmpCompanyDept.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtEmpCompanyDept.Size = new System.Drawing.Size(107, 28);
            this.txtEmpCompanyDept.TabIndex = 104;
            this.txtEmpCompanyDept.Visible = false;
            this.txtEmpCompanyDept.SelectedIndexChanged += new System.EventHandler(this.txtEmpCompanyDept_SelectedIndexChanged);
            // 
            // lblOutPatients
            // 
            this.lblOutPatients.AutoSize = true;
            this.lblOutPatients.Location = new System.Drawing.Point(310, 12);
            this.lblOutPatients.Name = "lblOutPatients";
            this.lblOutPatients.Size = new System.Drawing.Size(19, 18);
            this.lblOutPatients.TabIndex = 107;
            this.lblOutPatients.Text = "~";
            this.lblOutPatients.Visible = false;
            // 
            // lblEmpCompanyName
            // 
            this.lblEmpCompanyName.AutoSize = true;
            this.lblEmpCompanyName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblEmpCompanyName.ForeColor = System.Drawing.Color.Black;
            this.lblEmpCompanyName.Location = new System.Drawing.Point(436, 9);
            this.lblEmpCompanyName.Name = "lblEmpCompanyName";
            this.lblEmpCompanyName.Size = new System.Drawing.Size(80, 18);
            this.lblEmpCompanyName.TabIndex = 105;
            this.lblEmpCompanyName.Text = "单位部门";
            this.lblEmpCompanyName.Visible = false;
            // 
            // cbTypeID
            // 
            this.cbTypeID.EditValue = "门诊ID";
            this.cbTypeID.EnterMoveNextControl = true;
            this.cbTypeID.Location = new System.Drawing.Point(87, 7);
            this.cbTypeID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbTypeID.Name = "cbTypeID";
            this.cbTypeID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTypeID.Properties.Appearance.Options.UseFont = true;
            this.cbTypeID.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbTypeID.Properties.Items.AddRange(new object[] {
            "门诊ID",
            "发票号",
            "姓名"});
            this.cbTypeID.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbTypeID.Size = new System.Drawing.Size(97, 28);
            this.cbTypeID.TabIndex = 0;
            // 
            // txtOutPatients
            // 
            this.txtOutPatients.Location = new System.Drawing.Point(189, 7);
            this.txtOutPatients.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOutPatients.Name = "txtOutPatients";
            this.txtOutPatients.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutPatients.Properties.Appearance.Options.UseFont = true;
            this.txtOutPatients.Size = new System.Drawing.Size(120, 28);
            this.txtOutPatients.TabIndex = 1;
            this.txtOutPatients.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textEdit1_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(291, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(1, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "读取类型";
            // 
            // ckPrePlaceBarcode
            // 
            this.ckPrePlaceBarcode.EditValue = true;
            this.ckPrePlaceBarcode.Location = new System.Drawing.Point(225, 135);
            this.ckPrePlaceBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckPrePlaceBarcode.Name = "ckPrePlaceBarcode";
            this.ckPrePlaceBarcode.Properties.Caption = "启用预置";
            this.ckPrePlaceBarcode.Size = new System.Drawing.Size(99, 22);
            this.ckPrePlaceBarcode.TabIndex = 14;
            this.ckPrePlaceBarcode.Visible = false;
            this.ckPrePlaceBarcode.CheckedChanged += new System.EventHandler(this.ckPrePlaceBarcode_CheckedChanged);
            // 
            // adviceTime1
            // 
            this.adviceTime1.Dock = System.Windows.Forms.DockStyle.Left;
            this.adviceTime1.EndText = "";
            this.adviceTime1.EndTime = new System.DateTime(2007, 5, 1, 23, 59, 0, 0);
            this.adviceTime1.Location = new System.Drawing.Point(2, 84);
            this.adviceTime1.Margin = new System.Windows.Forms.Padding(5);
            this.adviceTime1.Name = "adviceTime1";
            this.adviceTime1.ShowTime = false;
            this.adviceTime1.Size = new System.Drawing.Size(333, 41);
            this.adviceTime1.StartText = "日期:";
            this.adviceTime1.StartTime = new System.DateTime(2007, 1, 1, 0, 0, 0, 0);
            this.adviceTime1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 10F);
            this.lblTitle.ForeColor = System.Drawing.Color.Blue;
            this.lblTitle.Location = new System.Drawing.Point(464, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 17);
            this.lblTitle.TabIndex = 4;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = false;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sysToolBar1.Location = new System.Drawing.Point(2, 2);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1710, 82);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.sysToolBar1_OnBtnAddClicked);
            this.sysToolBar1.OnBtnBCPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintClicked);
            this.sysToolBar1.OnBtnBCPrintReturnClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintReturnClicked);
            this.sysToolBar1.OnBtnPrintListClicked += new System.EventHandler(this.sysToolBar1_OnBtnPrintListClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.BtnDeleteSubClick += new System.EventHandler(this.sysToolBar1_BtnDeleteSubClick);
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.sysToolBar1_OnBtnConfirmClicked);
            this.sysToolBar1.BtnSperateBarcodeClick += new System.EventHandler(this.sysToolBar1_BtnSperateBarcodeClick);
            this.sysToolBar1.BtnUndoClick += new System.EventHandler(this.sysToolBar1_BtnUndoClick);
            this.sysToolBar1.OnResultViewClicked += new System.EventHandler(this.sysToolBar1_OnResultViewClicked);
            this.sysToolBar1.OnBtnQualityOutClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityOutClicked);
            this.sysToolBar1.BtnPrintSetClick += new System.EventHandler(this.sysToolBar1_BtnPrintSetClick);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // chkAutoPrintBarcode
            // 
            this.chkAutoPrintBarcode.Location = new System.Drawing.Point(1167, 8);
            this.chkAutoPrintBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAutoPrintBarcode.Name = "chkAutoPrintBarcode";
            this.chkAutoPrintBarcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoPrintBarcode.Properties.Appearance.Options.UseFont = true;
            this.chkAutoPrintBarcode.Properties.Caption = "自动打条码";
            this.chkAutoPrintBarcode.Size = new System.Drawing.Size(113, 26);
            this.chkAutoPrintBarcode.TabIndex = 109;
            // 
            // chkAutoPrintReturnBarcode
            // 
            this.chkAutoPrintReturnBarcode.Location = new System.Drawing.Point(857, 8);
            this.chkAutoPrintReturnBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAutoPrintReturnBarcode.Name = "chkAutoPrintReturnBarcode";
            this.chkAutoPrintReturnBarcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.chkAutoPrintReturnBarcode.Properties.Appearance.Options.UseFont = true;
            this.chkAutoPrintReturnBarcode.Properties.Caption = "自动打回执";
            this.chkAutoPrintReturnBarcode.Size = new System.Drawing.Size(109, 26);
            this.chkAutoPrintReturnBarcode.TabIndex = 108;
            this.chkAutoPrintReturnBarcode.CheckedChanged += new System.EventHandler(this.chkAutoPrintReturnBarcode_CheckedChanged);
            // 
            // lblPrePlaceBarcode
            // 
            this.lblPrePlaceBarcode.AutoSize = true;
            this.lblPrePlaceBarcode.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.lblPrePlaceBarcode.ForeColor = System.Drawing.Color.Black;
            this.lblPrePlaceBarcode.Location = new System.Drawing.Point(15, 12);
            this.lblPrePlaceBarcode.Name = "lblPrePlaceBarcode";
            this.lblPrePlaceBarcode.Size = new System.Drawing.Size(88, 22);
            this.lblPrePlaceBarcode.TabIndex = 13;
            this.lblPrePlaceBarcode.Text = "预置条码:";
            this.lblPrePlaceBarcode.Visible = false;
            // 
            // txtPrePlaceBarcode
            // 
            this.txtPrePlaceBarcode.Location = new System.Drawing.Point(105, 6);
            this.txtPrePlaceBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPrePlaceBarcode.Name = "txtPrePlaceBarcode";
            this.txtPrePlaceBarcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.txtPrePlaceBarcode.Properties.Appearance.Options.UseFont = true;
            this.txtPrePlaceBarcode.Size = new System.Drawing.Size(186, 28);
            this.txtPrePlaceBarcode.TabIndex = 12;
            this.txtPrePlaceBarcode.Visible = false;
            this.txtPrePlaceBarcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrePlaceBarcode_KeyPress);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.scrollingText1);
            this.panelControl3.Controls.Add(this.lblPrePlaceBarcode);
            this.panelControl3.Controls.Add(this.chkAutoPrintBarcode);
            this.panelControl3.Controls.Add(this.chkAutoPrintReturnBarcode);
            this.panelControl3.Controls.Add(this.txtPrePlaceBarcode);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 127);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1714, 44);
            this.panelControl3.TabIndex = 1;
            // 
            // scrollingText1
            // 
            this.scrollingText1.BorderColor = System.Drawing.Color.Black;
            this.scrollingText1.Cursor = System.Windows.Forms.Cursors.Default;
            this.scrollingText1.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scrollingText1.ForeColor = System.Drawing.Color.Red;
            this.scrollingText1.ForegroundBrush = null;
            this.scrollingText1.Location = new System.Drawing.Point(312, 5);
            this.scrollingText1.Name = "scrollingText1";
            this.scrollingText1.ScrollDirection = lis.client.control.ScrollingTextControl.ScrollDirection.RightToLeft;
            this.scrollingText1.ScrollText = "暂无回退条码";
            this.scrollingText1.ShowBorder = false;
            this.scrollingText1.Size = new System.Drawing.Size(518, 35);
            this.scrollingText1.StopScrollOnMouseOver = false;
            this.scrollingText1.TabIndex = 111;
            this.scrollingText1.Text = "暂无回退条码";
            this.scrollingText1.TextScrollDistance = 2;
            this.scrollingText1.TextScrollSpeed = 25;
            this.scrollingText1.VerticleTextPosition = lis.client.control.ScrollingTextControl.VerticleTextPosition.Center;
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.patientControl);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(0, 171);
            this.panelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1714, 823);
            this.panelControl4.TabIndex = 13;
            // 
            // patientControl
            // 
            this.patientControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientControl.Location = new System.Drawing.Point(2, 2);
            this.patientControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.patientControl.Name = "patientControl";
            this.patientControl.SelectType = dcl.client.sample.SelectType.Create;
            this.patientControl.SelectWhenNotPrint = false;
            this.patientControl.ShouldMultiSelect = true;
            this.patientControl.ShowCollectNotice = false;
            this.patientControl.Size = new System.Drawing.Size(1710, 819);
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
            this.patientControl.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            // 
            // timer2
            // 
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // BCPrintControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.pnlTop);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "BCPrintControl";
            this.Size = new System.Drawing.Size(1714, 994);
            ((System.ComponentModel.ISupportInitialize)(this.dtSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmBloodStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmStatus.Properties)).EndInit();
            this.panelInpatient.ResumeLayout(false);
            this.panelInpatient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBedNum.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInpatientID.Properties)).EndInit();
            this.panelOutpatient.ResumeLayout(false);
            this.panelOutpatient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutPatientsEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpCompanyDept.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTypeID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOutPatients.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckPrePlaceBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoPrintBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoPrintReturnBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrePlaceBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlTop;
        private AdviceTime adviceTime1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.TextEdit txtInpatientID;
        private System.Windows.Forms.Label lblID;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private System.Windows.Forms.Panel panelInpatient;
        private System.Windows.Forms.Panel panelOutpatient;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        public DevExpress.XtraEditors.ComboBoxEdit cbTypeID;
        private DevExpress.XtraEditors.TextEdit txtOutPatients;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.Blending.XtraGridBlending xtraGridBlending1;
        private System.Windows.Forms.Label lblDept;
        private dcl.client.control.SelectDicPubDept selectDict_Depart1;
        private System.Windows.Forms.Label lblBedNum;
        private DevExpress.XtraEditors.TextEdit txtBedNum;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.TextEdit txtPrePlaceBarcode;
        private System.Windows.Forms.Label lblPrePlaceBarcode;
        private DevExpress.XtraEditors.CheckEdit ckPrePlaceBarcode;
        private DevExpress.XtraEditors.ComboBoxEdit txtEmpCompanyDept;
        public System.Windows.Forms.Label lblOutPatients;
        private DevExpress.XtraEditors.CheckEdit chkAutoPrintReturnBarcode;
        private DevExpress.XtraEditors.CheckEdit chkAutoPrintBarcode;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.ComboBoxEdit cmStatus;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        public common.SysToolBar sysToolBar1;
        public DevExpress.XtraEditors.TextEdit txtOutPatientsEnd;
        private DevExpress.XtraEditors.ComboBoxEdit cmBloodStatus;
        public PatientControlForMed patientControl;
        private lis.client.control.ScrollingTextControl.ScrollingText scrollingText1;
        private System.Windows.Forms.Label lblEmpCompanyName;
    }
}
