namespace dcl.client.sample
{
    partial class BCSearchControl
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
            dcl.client.sample.PrintStep printStep1 = new dcl.client.sample.PrintStep();
            dcl.client.sample.CoolStepController coolStepController1 = new dcl.client.sample.CoolStepController();
            this.pnlTop = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.selectDict_Instrmt1 = new dcl.client.control.SelectDicInstrument();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.selectAuditOper = new dcl.client.control.SelectDictSysUser();
            this.OperType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbTimeType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrintTime = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPt = new DevExpress.XtraEditors.LabelControl();
            this.lueType = new dcl.client.control.SelectDicLabProfession();
            this.lueDepart = new dcl.client.control.SelectDicPubDept();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBarcode = new DevExpress.XtraEditors.TextEdit();
            this.txtPlace = new DevExpress.XtraEditors.TextEdit();
            this.txtInNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPatName = new DevExpress.XtraEditors.TextEdit();
            this.txtRackNo = new DevExpress.XtraEditors.TextEdit();
            this.lueItem = new dcl.client.control.SelectDicCombine();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.deStart = new DevExpress.XtraEditors.DateEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.deEnd = new DevExpress.XtraEditors.DateEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.cepatmain = new DevExpress.XtraEditors.CheckEdit();
            this.cePrint = new DevExpress.XtraEditors.CheckEdit();
            this.ceContinuitySearch = new DevExpress.XtraEditors.CheckEdit();
            this.lblTitle = new System.Windows.Forms.Label();
            this.bindingSourceItr = new System.Windows.Forms.BindingSource(this.components);
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.label3 = new System.Windows.Forms.Label();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.patientControl = new dcl.client.sample.PatientControlForMed();
            ((System.ComponentModel.ISupportInitialize)(this.dtSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrintTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlace.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRackNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cepatmain.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePrint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceContinuitySearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceItr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.groupControl1);
            this.pnlTop.Controls.Add(this.groupControl2);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTop.Location = new System.Drawing.Point(0, 81);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(275, 913);
            this.pnlTop.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.selectDict_Instrmt1);
            this.groupControl1.Controls.Add(this.labelControl10);
            this.groupControl1.Controls.Add(this.selectAuditOper);
            this.groupControl1.Controls.Add(this.OperType);
            this.groupControl1.Controls.Add(this.label8);
            this.groupControl1.Controls.Add(this.cmbTimeType);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.label5);
            this.groupControl1.Controls.Add(this.txtPrintTime);
            this.groupControl1.Controls.Add(this.label6);
            this.groupControl1.Controls.Add(this.lblPt);
            this.groupControl1.Controls.Add(this.lueType);
            this.groupControl1.Controls.Add(this.lueDepart);
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.txtBarcode);
            this.groupControl1.Controls.Add(this.txtPlace);
            this.groupControl1.Controls.Add(this.txtInNo);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txtPatName);
            this.groupControl1.Controls.Add(this.txtRackNo);
            this.groupControl1.Controls.Add(this.lueItem);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.label7);
            this.groupControl1.Controls.Add(this.label12);
            this.groupControl1.Controls.Add(this.deStart);
            this.groupControl1.Controls.Add(this.label11);
            this.groupControl1.Controls.Add(this.deEnd);
            this.groupControl1.Controls.Add(this.label10);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(2, 103);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(271, 556);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "查询条件";
            // 
            // selectDict_Instrmt1
            // 
            this.selectDict_Instrmt1.AddEmptyRow = true;
            this.selectDict_Instrmt1.AutoScroll = true;
            this.selectDict_Instrmt1.BindByValue = false;
            this.selectDict_Instrmt1.colDisplay = "";
            this.selectDict_Instrmt1.colExtend1 = null;
            this.selectDict_Instrmt1.colInCode = "";
            this.selectDict_Instrmt1.colPY = "";
            this.selectDict_Instrmt1.colSeq = "";
            this.selectDict_Instrmt1.colValue = "";
            this.selectDict_Instrmt1.colWB = "";
            this.selectDict_Instrmt1.displayMember = null;
            this.selectDict_Instrmt1.Enabled = false;
            this.selectDict_Instrmt1.EnterMoveNext = true;
            this.selectDict_Instrmt1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDict_Instrmt1.KeyUpDownMoveNext = false;
            this.selectDict_Instrmt1.LoadDataOnDesignMode = true;
            this.selectDict_Instrmt1.Location = new System.Drawing.Point(92, 357);
            this.selectDict_Instrmt1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectDict_Instrmt1.MaximumSize = new System.Drawing.Size(571, 26);
            this.selectDict_Instrmt1.MinimumSize = new System.Drawing.Size(57, 26);
            this.selectDict_Instrmt1.Name = "selectDict_Instrmt1";
            this.selectDict_Instrmt1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDict_Instrmt1.PFont = new System.Drawing.Font("Tahoma", 10.5F);
            this.selectDict_Instrmt1.Readonly = false;
            this.selectDict_Instrmt1.SaveSourceID = false;
            this.selectDict_Instrmt1.SelectFilter = null;
            this.selectDict_Instrmt1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDict_Instrmt1.SelectOnly = true;
            this.selectDict_Instrmt1.ShowAllInstrmt = false;
            this.selectDict_Instrmt1.Size = new System.Drawing.Size(170, 26);
            this.selectDict_Instrmt1.TabIndex = 169;
            this.selectDict_Instrmt1.UseExtend = false;
            this.selectDict_Instrmt1.valueMember = null;
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl10.Location = new System.Drawing.Point(47, 355);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(36, 22);
            this.labelControl10.TabIndex = 168;
            this.labelControl10.Text = "仪器";
            // 
            // selectAuditOper
            // 
            this.selectAuditOper.AddEmptyRow = true;
            this.selectAuditOper.BindByValue = false;
            this.selectAuditOper.colDisplay = "";
            this.selectAuditOper.colExtend1 = null;
            this.selectAuditOper.colInCode = "";
            this.selectAuditOper.colPY = "";
            this.selectAuditOper.colSeq = "";
            this.selectAuditOper.colValue = "";
            this.selectAuditOper.colWB = "";
            this.selectAuditOper.displayMember = null;
            this.selectAuditOper.EnterMoveNext = true;
            this.selectAuditOper.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectAuditOper.KeyUpDownMoveNext = false;
            this.selectAuditOper.LoadDataOnDesignMode = true;
            this.selectAuditOper.Location = new System.Drawing.Point(92, 459);
            this.selectAuditOper.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectAuditOper.MaximumSize = new System.Drawing.Size(571, 27);
            this.selectAuditOper.MinimumSize = new System.Drawing.Size(57, 27);
            this.selectAuditOper.Name = "selectAuditOper";
            this.selectAuditOper.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectAuditOper.PFont = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectAuditOper.Readonly = false;
            this.selectAuditOper.SaveSourceID = false;
            this.selectAuditOper.SelectFilter = null;
            this.selectAuditOper.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectAuditOper.SelectOnly = true;
            this.selectAuditOper.Size = new System.Drawing.Size(170, 27);
            this.selectAuditOper.TabIndex = 36;
            this.selectAuditOper.UseExtend = false;
            this.selectAuditOper.valueMember = null;
            // 
            // OperType
            // 
            this.OperType.EditValue = "采集者";
            this.OperType.Location = new System.Drawing.Point(7, 460);
            this.OperType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OperType.Name = "OperType";
            this.OperType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OperType.Properties.Appearance.Options.UseFont = true;
            this.OperType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.OperType.Properties.Items.AddRange(new object[] {
            "采集者",
            "送达者",
            "送检者",
            "签收者"});
            this.OperType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.OperType.Size = new System.Drawing.Size(79, 28);
            this.OperType.TabIndex = 35;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(8, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 18);
            this.label8.TabIndex = 34;
            this.label8.Text = "开始时间";
            // 
            // cmbTimeType
            // 
            this.cmbTimeType.EditValue = "医嘱下载";
            this.cmbTimeType.Location = new System.Drawing.Point(92, 37);
            this.cmbTimeType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbTimeType.Name = "cmbTimeType";
            this.cmbTimeType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTimeType.Properties.Appearance.Options.UseFont = true;
            this.cmbTimeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTimeType.Properties.Items.AddRange(new object[] {
            "医嘱下载",
            "条码打印",
            "条码采集",
            "条码收取",
            "条码送达",
            "条码签收",
            "二次送检"});
            this.cmbTimeType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbTimeType.Size = new System.Drawing.Size(170, 28);
            this.cmbTimeType.TabIndex = 0;
            this.cmbTimeType.SelectedIndexChanged += new System.EventHandler(this.cmbTimeType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 33;
            this.label1.Text = "时间类型";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(26, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 18);
            this.label5.TabIndex = 2;
            this.label5.Text = "条码号";
            // 
            // txtPrintTime
            // 
            this.txtPrintTime.EditValue = "1";
            this.txtPrintTime.Location = new System.Drawing.Point(92, 493);
            this.txtPrintTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPrintTime.Name = "txtPrintTime";
            this.txtPrintTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrintTime.Properties.Appearance.Options.UseFont = true;
            this.txtPrintTime.Size = new System.Drawing.Size(170, 28);
            this.txtPrintTime.TabIndex = 31;
            this.txtPrintTime.Visible = false;
            this.txtPrintTime.EditValueChanged += new System.EventHandler(this.txtPrintTime_EditValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(25, 220);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 18);
            this.label6.TabIndex = 2;
            this.label6.Text = "病人ID";
            // 
            // lblPt
            // 
            this.lblPt.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPt.Location = new System.Drawing.Point(12, 496);
            this.lblPt.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lblPt.Name = "lblPt";
            this.lblPt.Size = new System.Drawing.Size(72, 18);
            this.lblPt.TabIndex = 32;
            this.lblPt.Text = "打印次数";
            this.lblPt.Visible = false;
            // 
            // lueType
            // 
            this.lueType.AddEmptyRow = true;
            this.lueType.AutoScroll = true;
            this.lueType.BindByValue = false;
            this.lueType.colDisplay = "";
            this.lueType.colExtend1 = null;
            this.lueType.colInCode = "";
            this.lueType.colPY = "";
            this.lueType.colSeq = "";
            this.lueType.colValue = "";
            this.lueType.colWB = "";
            this.lueType.displayMember = null;
            this.lueType.EnterMoveNext = true;
            this.lueType.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueType.KeyUpDownMoveNext = false;
            this.lueType.LoadDataOnDesignMode = true;
            this.lueType.Location = new System.Drawing.Point(92, 288);
            this.lueType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueType.MaximumSize = new System.Drawing.Size(571, 27);
            this.lueType.MinimumSize = new System.Drawing.Size(57, 27);
            this.lueType.Name = "lueType";
            this.lueType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueType.PFont = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueType.Readonly = false;
            this.lueType.SaveSourceID = false;
            this.lueType.SelectFilter = null;
            this.lueType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueType.SelectOnly = false;
            this.lueType.Size = new System.Drawing.Size(170, 27);
            this.lueType.TabIndex = 4;
            this.lueType.UseExtend = false;
            this.lueType.valueMember = null;
            this.lueType.onAfterSelected += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.afterSelected(this.lueType_onAfterSelected);
            // 
            // lueDepart
            // 
            this.lueDepart.AddEmptyRow = true;
            this.lueDepart.AutoScroll = true;
            this.lueDepart.BindByValue = false;
            this.lueDepart.colDisplay = "";
            this.lueDepart.colExtend1 = null;
            this.lueDepart.colInCode = "";
            this.lueDepart.colPY = "";
            this.lueDepart.colSeq = "";
            this.lueDepart.colValue = "";
            this.lueDepart.colWB = "";
            this.lueDepart.displayMember = null;
            this.lueDepart.EnterMoveNext = true;
            this.lueDepart.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueDepart.KeyUpDownMoveNext = false;
            this.lueDepart.LoadDataOnDesignMode = true;
            this.lueDepart.Location = new System.Drawing.Point(92, 253);
            this.lueDepart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueDepart.MaximumSize = new System.Drawing.Size(571, 27);
            this.lueDepart.MinimumSize = new System.Drawing.Size(57, 27);
            this.lueDepart.Name = "lueDepart";
            this.lueDepart.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueDepart.PFont = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueDepart.Readonly = false;
            this.lueDepart.SaveSourceID = false;
            this.lueDepart.SelectFilter = null;
            this.lueDepart.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueDepart.SelectOnly = false;
            this.lueDepart.Size = new System.Drawing.Size(170, 27);
            this.lueDepart.TabIndex = 3;
            this.lueDepart.UseExtend = false;
            this.lueDepart.valueMember = null;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(8, 393);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 29;
            this.label4.Text = "操作地点";
            // 
            // txtBarcode
            // 
            this.txtBarcode.EditValue = "";
            this.txtBarcode.Location = new System.Drawing.Point(92, 181);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcode.Properties.Appearance.Options.UseFont = true;
            this.txtBarcode.Size = new System.Drawing.Size(170, 28);
            this.txtBarcode.TabIndex = 0;
            this.txtBarcode.Enter += new System.EventHandler(this.txtBarcode_Enter);
            this.txtBarcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarcode_KeyDown);
            // 
            // txtPlace
            // 
            this.txtPlace.Location = new System.Drawing.Point(92, 390);
            this.txtPlace.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPlace.Name = "txtPlace";
            this.txtPlace.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlace.Properties.Appearance.Options.UseFont = true;
            this.txtPlace.Size = new System.Drawing.Size(170, 28);
            this.txtPlace.TabIndex = 28;
            // 
            // txtInNo
            // 
            this.txtInNo.EditValue = "";
            this.txtInNo.Location = new System.Drawing.Point(92, 217);
            this.txtInNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInNo.Name = "txtInNo";
            this.txtInNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInNo.Properties.Appearance.Options.UseFont = true;
            this.txtInNo.Size = new System.Drawing.Size(170, 28);
            this.txtInNo.TabIndex = 8;
            this.txtInNo.Enter += new System.EventHandler(this.txtInNo_Enter);
            this.txtInNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInNo_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl1.Location = new System.Drawing.Point(11, 149);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 18);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "病人姓名";
            // 
            // txtPatName
            // 
            this.txtPatName.Location = new System.Drawing.Point(92, 145);
            this.txtPatName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPatName.Name = "txtPatName";
            this.txtPatName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatName.Properties.Appearance.Options.UseFont = true;
            this.txtPatName.Size = new System.Drawing.Size(170, 28);
            this.txtPatName.TabIndex = 7;
            this.txtPatName.Enter += new System.EventHandler(this.txtPatName_Enter);
            this.txtPatName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPatName_KeyDown);
            // 
            // txtRackNo
            // 
            this.txtRackNo.EditValue = "";
            this.txtRackNo.Location = new System.Drawing.Point(92, 426);
            this.txtRackNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRackNo.Name = "txtRackNo";
            this.txtRackNo.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRackNo.Properties.Appearance.Options.UseFont = true;
            this.txtRackNo.Size = new System.Drawing.Size(170, 28);
            this.txtRackNo.TabIndex = 25;
            // 
            // lueItem
            // 
            this.lueItem.AddEmptyRow = true;
            this.lueItem.AutoScroll = true;
            this.lueItem.BindByValue = false;
            this.lueItem.colDisplay = "";
            this.lueItem.colExtend1 = null;
            this.lueItem.colInCode = "";
            this.lueItem.colPY = "";
            this.lueItem.colSeq = "";
            this.lueItem.colValue = "";
            this.lueItem.colWB = "";
            this.lueItem.displayMember = null;
            this.lueItem.EnterMoveNext = true;
            this.lueItem.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueItem.KeyUpDownMoveNext = false;
            this.lueItem.LoadDataOnDesignMode = true;
            this.lueItem.Location = new System.Drawing.Point(92, 323);
            this.lueItem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lueItem.MaximumSize = new System.Drawing.Size(571, 27);
            this.lueItem.MinimumSize = new System.Drawing.Size(57, 27);
            this.lueItem.Name = "lueItem";
            this.lueItem.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueItem.PFont = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lueItem.Readonly = false;
            this.lueItem.SaveSourceID = false;
            this.lueItem.SelectFilter = null;
            this.lueItem.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueItem.SelectOnly = true;
            this.lueItem.Size = new System.Drawing.Size(170, 27);
            this.lueItem.TabIndex = 10;
            this.lueItem.UseExtend = false;
            this.lueItem.valueMember = null;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(26, 429);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 24;
            this.label2.Text = "架子号";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(8, 326);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 12;
            this.label7.Text = "包含项目";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(26, 291);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 18);
            this.label12.TabIndex = 21;
            this.label12.Text = "实验组";
            // 
            // deStart
            // 
            this.deStart.EditValue = null;
            this.deStart.EnterMoveNextControl = true;
            this.deStart.Location = new System.Drawing.Point(92, 73);
            this.deStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deStart.Name = "deStart";
            this.deStart.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deStart.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deStart.Properties.Appearance.Options.UseFont = true;
            this.deStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStart.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.deStart.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.deStart.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deStart.Properties.Mask.EditMask = "";
            this.deStart.Size = new System.Drawing.Size(170, 28);
            this.deStart.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(44, 256);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 18);
            this.label11.TabIndex = 20;
            this.label11.Text = "科室";
            // 
            // deEnd
            // 
            this.deEnd.EditValue = null;
            this.deEnd.EnterMoveNextControl = true;
            this.deEnd.Location = new System.Drawing.Point(92, 109);
            this.deEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.deEnd.Name = "deEnd";
            this.deEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.deEnd.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deEnd.Properties.Appearance.Options.UseFont = true;
            this.deEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.deEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm";
            this.deEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.deEnd.Properties.Mask.EditMask = "";
            this.deEnd.Size = new System.Drawing.Size(170, 28);
            this.deEnd.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(8, 112);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 18);
            this.label10.TabIndex = 18;
            this.label10.Text = "结束时间";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.cepatmain);
            this.groupControl2.Controls.Add(this.cePrint);
            this.groupControl2.Controls.Add(this.ceContinuitySearch);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(2, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(271, 101);
            this.groupControl2.TabIndex = 6;
            this.groupControl2.Text = "查询设置";
            // 
            // cepatmain
            // 
            this.cepatmain.Location = new System.Drawing.Point(19, 67);
            this.cepatmain.Name = "cepatmain";
            this.cepatmain.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cepatmain.Properties.Appearance.Options.UseFont = true;
            this.cepatmain.Properties.Caption = "仪器组合";
            this.cepatmain.Size = new System.Drawing.Size(105, 28);
            this.cepatmain.TabIndex = 33;
            // 
            // cePrint
            // 
            this.cePrint.Location = new System.Drawing.Point(163, 33);
            this.cePrint.Name = "cePrint";
            this.cePrint.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cePrint.Properties.Appearance.Options.UseFont = true;
            this.cePrint.Properties.Caption = "打印";
            this.cePrint.Size = new System.Drawing.Size(64, 28);
            this.cePrint.TabIndex = 32;
            // 
            // ceContinuitySearch
            // 
            this.ceContinuitySearch.Location = new System.Drawing.Point(19, 33);
            this.ceContinuitySearch.Name = "ceContinuitySearch";
            this.ceContinuitySearch.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ceContinuitySearch.Properties.Appearance.Options.UseFont = true;
            this.ceContinuitySearch.Properties.Caption = "连查";
            this.ceContinuitySearch.Size = new System.Drawing.Size(64, 28);
            this.ceContinuitySearch.TabIndex = 31;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 10F);
            this.lblTitle.ForeColor = System.Drawing.Color.Blue;
            this.lblTitle.Location = new System.Drawing.Point(483, 24);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 17);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Visible = false;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.label3);
            this.panelControl3.Controls.Add(this.radioGroup1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(275, 81);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1352, 49);
            this.panelControl3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.label3.ForeColor = System.Drawing.Color.Goldenrod;
            this.label3.Location = new System.Drawing.Point(1470, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 22);
            this.label3.TabIndex = 8;
            this.label3.Text = "已删除";
            this.label3.Visible = false;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(9, 6);
            this.radioGroup1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.Appearance.Options.UseFont = true;
            this.radioGroup1.Properties.Columns = 12;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "全部"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "打印"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "采集"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "收取"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "送检"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "签收"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "登记"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "一审"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "二审"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "追加"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "删除"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "回退")});
            this.radioGroup1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radioGroup1.Size = new System.Drawing.Size(916, 36);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.sysToolBar1);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1627, 81);
            this.panelControl4.TabIndex = 13;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(2, 2);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = null;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1623, 77);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnBCPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintClicked);
            this.sysToolBar1.OnBtnBCPrintReturnClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintReturnClicked);
            this.sysToolBar1.OnBtnPrintListClicked += new System.EventHandler(this.sysToolBar1_OnBtnPrintListClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.BtnDeleteSubClick += new System.EventHandler(this.sysToolBar1_BtnDeleteSubClick);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar1_OnBtnCancelClicked);
            this.sysToolBar1.BtnSperateBarcodeClick += new System.EventHandler(this.sysToolBar1_BtnSperateBarcodeClick);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
            this.sysToolBar1.OnBtnSinglePrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnSinglePrintClicked);
            this.sysToolBar1.OnBtnQualityTestClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityTestClicked);
            this.sysToolBar1.BtnDeSpeClick += new System.EventHandler(this.sysToolBar1_BtnDeSpeClick);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // patientControl
            // 
            this.patientControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientControl.Location = new System.Drawing.Point(275, 130);
            this.patientControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.patientControl.Name = "patientControl";
            this.patientControl.SelectType = dcl.client.sample.SelectType.Create;
            this.patientControl.SelectWhenNotPrint = false;
            this.patientControl.ShouldMultiSelect = true;
            this.patientControl.ShowCollectNotice = false;
            this.patientControl.Size = new System.Drawing.Size(1352, 864);
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
            this.patientControl.Load += new System.EventHandler(this.patientControl_Load);
            // 
            // BCSearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.patientControl);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.panelControl4);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "BCSearchControl";
            this.Size = new System.Drawing.Size(1627, 994);
            ((System.ComponentModel.ISupportInitialize)(this.dtSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTop)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTimeType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrintTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlace.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRackNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cepatmain.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cePrint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceContinuitySearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceItr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl pnlTop;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
     
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private dcl.client.common.SysToolBar sysToolBar1;
        private PatientControlForMed patientControl;
        private System.Windows.Forms.Label lblTitle;
        private dcl.client.control.SelectDicPubDept lueDepart;
        private dcl.client.control.SelectDicLabProfession lueType;
        private DevExpress.XtraEditors.TextEdit txtBarcode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtInNo;
        private DevExpress.XtraEditors.TextEdit txtPatName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private dcl.client.control.SelectDicCombine lueItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.DateEdit deEnd;
        private DevExpress.XtraEditors.DateEdit deStart;
        private DevExpress.XtraEditors.ComboBoxEdit cmbTimeType;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.TextEdit txtRackNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtPlace;
        private DevExpress.XtraEditors.TextEdit txtPrintTime;
        private DevExpress.XtraEditors.LabelControl lblPt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        //private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private System.Windows.Forms.BindingSource bindingSourceItr;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit ceContinuitySearch;
        private DevExpress.XtraEditors.CheckEdit cePrint;
        private DevExpress.XtraEditors.ComboBoxEdit OperType;
        private control.SelectDictSysUser selectAuditOper;
        private DevExpress.XtraEditors.CheckEdit cepatmain;
        private control.SelectDicInstrument selectDict_Instrmt1;
        private DevExpress.XtraEditors.LabelControl labelControl10;
    }
}
