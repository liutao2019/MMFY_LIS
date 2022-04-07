namespace dcl.client.sample
{
    partial class BCManualYG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BCManualYG));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip5 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip6 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem6 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip7 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem7 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip8 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem8 = new DevExpress.Utils.ToolTipTitleItem();
            dcl.client.sample.PrintStep printStep2 = new dcl.client.sample.PrintStep();
            dcl.client.sample.CoolStepController coolStepController2 = new dcl.client.sample.CoolStepController();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.ckbNotAge = new System.Windows.Forms.CheckBox();
            this.ckbNotSex = new System.Windows.Forms.CheckBox();
            this.txtAge = new lis.client.control.TextAgeInput();
            this.cbSex = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.selectDic_Pub_Evalute = new dcl.client.control.SelectDicPubEvaluate();
            this.checkItem = new System.Windows.Forms.CheckBox();
            this.checkPatName = new System.Windows.Forms.CheckBox();
            this.checkDoctor = new System.Windows.Forms.CheckBox();
            this.checkDepart = new System.Windows.Forms.CheckBox();
            this.chkAutoPrint = new System.Windows.Forms.CheckBox();
            this.txtDoctor = new dcl.client.control.SelectDicPubDoctor();
            this.selectDict_Origin1 = new dcl.client.control.SelectDicPubSource();
            this.dateTimeControl1 = new DevExpress.XtraEditors.DateEdit();
            this.txtDeptId = new dcl.client.control.SelectDicPubDept();
            this.txtCombineEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblPatName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.patientControl1 = new dcl.client.sample.PatientControlForMed();
            this.lbEvaluteMore = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAge.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeControl1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCombineEdit.Properties)).BeginInit();
            this.SuspendLayout();
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
            this.sysToolBar1.Size = new System.Drawing.Size(1352, 72);
            this.sysToolBar1.TabIndex = 19;
            this.sysToolBar1.OnBtnBCPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnBCPrintClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.BtnResetClick += new System.EventHandler(this.sysToolBar1_BtnResetClick);
            this.sysToolBar1.BtnPrintSetClick += new System.EventHandler(this.sysToolBar1_BtnPrintSetClick);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lbEvaluteMore);
            this.groupControl1.Controls.Add(this.ckbNotAge);
            this.groupControl1.Controls.Add(this.ckbNotSex);
            this.groupControl1.Controls.Add(this.txtAge);
            this.groupControl1.Controls.Add(this.cbSex);
            this.groupControl1.Controls.Add(this.label8);
            this.groupControl1.Controls.Add(this.label7);
            this.groupControl1.Controls.Add(this.selectDic_Pub_Evalute);
            this.groupControl1.Controls.Add(this.checkItem);
            this.groupControl1.Controls.Add(this.checkPatName);
            this.groupControl1.Controls.Add(this.checkDoctor);
            this.groupControl1.Controls.Add(this.checkDepart);
            this.groupControl1.Controls.Add(this.chkAutoPrint);
            this.groupControl1.Controls.Add(this.txtDoctor);
            this.groupControl1.Controls.Add(this.selectDict_Origin1);
            this.groupControl1.Controls.Add(this.dateTimeControl1);
            this.groupControl1.Controls.Add(this.txtDeptId);
            this.groupControl1.Controls.Add(this.txtCombineEdit);
            this.groupControl1.Controls.Add(this.label14);
            this.groupControl1.Controls.Add(this.label9);
            this.groupControl1.Controls.Add(this.lblPatName);
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 72);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1352, 133);
            this.groupControl1.TabIndex = 20;
            this.groupControl1.Text = "环境监测查询";
            // 
            // ckbNotAge
            // 
            this.ckbNotAge.AutoSize = true;
            this.ckbNotAge.Checked = true;
            this.ckbNotAge.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNotAge.ForeColor = System.Drawing.Color.Blue;
            this.ckbNotAge.Location = new System.Drawing.Point(929, 94);
            this.ckbNotAge.Margin = new System.Windows.Forms.Padding(4);
            this.ckbNotAge.Name = "ckbNotAge";
            this.ckbNotAge.Size = new System.Drawing.Size(60, 22);
            this.ckbNotAge.TabIndex = 938;
            this.ckbNotAge.Text = "忽略";
            this.ckbNotAge.UseVisualStyleBackColor = true;
            // 
            // ckbNotSex
            // 
            this.ckbNotSex.AutoSize = true;
            this.ckbNotSex.Checked = true;
            this.ckbNotSex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbNotSex.ForeColor = System.Drawing.Color.Blue;
            this.ckbNotSex.Location = new System.Drawing.Point(929, 63);
            this.ckbNotSex.Margin = new System.Windows.Forms.Padding(4);
            this.ckbNotSex.Name = "ckbNotSex";
            this.ckbNotSex.Size = new System.Drawing.Size(60, 22);
            this.ckbNotSex.TabIndex = 937;
            this.ckbNotSex.Text = "忽略";
            this.ckbNotSex.UseVisualStyleBackColor = true;
            // 
            // txtAge
            // 
            this.txtAge.AgeValueText = "";
            this.txtAge.EditValue = "   岁  月  天  时  分";
            this.txtAge.EnterMoveNextControl = true;
            this.txtAge.Location = new System.Drawing.Point(1042, 96);
            this.txtAge.Margin = new System.Windows.Forms.Padding(4);
            this.txtAge.Name = "txtAge";
            this.txtAge.Properties.Mask.EditMask = "\\d?\\d?\\d?岁\\d?\\d?\\d?月\\d?\\d?\\d?天\\d?\\d?\\d?时\\d?\\d?\\d?分";
            this.txtAge.Properties.Mask.IgnoreMaskBlank = false;
            this.txtAge.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
            this.txtAge.Properties.Mask.PlaceHolder = ' ';
            this.txtAge.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtAge.Properties.NullText = "   岁  月  天  时  分";
            this.txtAge.Size = new System.Drawing.Size(175, 24);
            this.txtAge.TabIndex = 936;
            // 
            // cbSex
            // 
            this.cbSex.EditValue = "未知";
            this.cbSex.EnterMoveNextControl = true;
            this.cbSex.Location = new System.Drawing.Point(1042, 64);
            this.cbSex.Margin = new System.Windows.Forms.Padding(4);
            this.cbSex.Name = "cbSex";
            this.cbSex.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbSex.Properties.Items.AddRange(new object[] {
            "男",
            "女",
            "未知"});
            this.cbSex.Size = new System.Drawing.Size(175, 24);
            this.cbSex.TabIndex = 935;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(997, 102);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 15);
            this.label8.TabIndex = 933;
            this.label8.Text = "年龄";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(997, 67);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 15);
            this.label7.TabIndex = 934;
            this.label7.Text = "性别";
            // 
            // selectDic_Pub_Evalute
            // 
            this.selectDic_Pub_Evalute.AddEmptyRow = true;
            this.selectDic_Pub_Evalute.AutoScroll = true;
            this.selectDic_Pub_Evalute.BindByValue = false;
            this.selectDic_Pub_Evalute.colDisplay = "";
            this.selectDic_Pub_Evalute.colExtend1 = "";
            this.selectDic_Pub_Evalute.colInCode = "";
            this.selectDic_Pub_Evalute.colPY = "";
            this.selectDic_Pub_Evalute.colSeq = "";
            this.selectDic_Pub_Evalute.colValue = "";
            this.selectDic_Pub_Evalute.colWB = "";
            this.selectDic_Pub_Evalute.displayMember = null;
            this.selectDic_Pub_Evalute.EnterMoveNext = true;
            this.selectDic_Pub_Evalute.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDic_Pub_Evalute.KeyUpDownMoveNext = true;
            this.selectDic_Pub_Evalute.LoadDataOnDesignMode = true;
            this.selectDic_Pub_Evalute.Location = new System.Drawing.Point(559, 65);
            this.selectDic_Pub_Evalute.Margin = new System.Windows.Forms.Padding(4);
            this.selectDic_Pub_Evalute.MaximumSize = new System.Drawing.Size(667, 26);
            this.selectDic_Pub_Evalute.MinimumSize = new System.Drawing.Size(67, 26);
            this.selectDic_Pub_Evalute.Name = "selectDic_Pub_Evalute";
            this.selectDic_Pub_Evalute.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDic_Pub_Evalute.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDic_Pub_Evalute.Readonly = false;
            this.selectDic_Pub_Evalute.SaveSourceID = false;
            this.selectDic_Pub_Evalute.SelectFilter = null;
            this.selectDic_Pub_Evalute.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDic_Pub_Evalute.SelectOnly = false;
            this.selectDic_Pub_Evalute.Size = new System.Drawing.Size(252, 26);
            this.selectDic_Pub_Evalute.TabIndex = 932;
            this.selectDic_Pub_Evalute.UseExtend = false;
            this.selectDic_Pub_Evalute.valueMember = null;
            // 
            // checkItem
            // 
            this.checkItem.AutoSize = true;
            this.checkItem.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkItem.Location = new System.Drawing.Point(425, 94);
            this.checkItem.Margin = new System.Windows.Forms.Padding(4);
            this.checkItem.Name = "checkItem";
            this.checkItem.Size = new System.Drawing.Size(60, 22);
            this.checkItem.TabIndex = 931;
            this.checkItem.Text = "锁定";
            this.checkItem.UseVisualStyleBackColor = true;
            // 
            // checkPatName
            // 
            this.checkPatName.AutoSize = true;
            this.checkPatName.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkPatName.Location = new System.Drawing.Point(425, 63);
            this.checkPatName.Margin = new System.Windows.Forms.Padding(4);
            this.checkPatName.Name = "checkPatName";
            this.checkPatName.Size = new System.Drawing.Size(60, 22);
            this.checkPatName.TabIndex = 930;
            this.checkPatName.Text = "锁定";
            this.checkPatName.UseVisualStyleBackColor = true;
            // 
            // checkDoctor
            // 
            this.checkDoctor.AutoSize = true;
            this.checkDoctor.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkDoctor.Location = new System.Drawing.Point(27, 94);
            this.checkDoctor.Margin = new System.Windows.Forms.Padding(4);
            this.checkDoctor.Name = "checkDoctor";
            this.checkDoctor.Size = new System.Drawing.Size(60, 22);
            this.checkDoctor.TabIndex = 929;
            this.checkDoctor.Text = "锁定";
            this.checkDoctor.UseVisualStyleBackColor = true;
            // 
            // checkDepart
            // 
            this.checkDepart.AutoSize = true;
            this.checkDepart.ForeColor = System.Drawing.Color.OrangeRed;
            this.checkDepart.Location = new System.Drawing.Point(27, 63);
            this.checkDepart.Margin = new System.Windows.Forms.Padding(4);
            this.checkDepart.Name = "checkDepart";
            this.checkDepart.Size = new System.Drawing.Size(60, 22);
            this.checkDepart.TabIndex = 928;
            this.checkDepart.Text = "锁定";
            this.checkDepart.UseVisualStyleBackColor = true;
            // 
            // chkAutoPrint
            // 
            this.chkAutoPrint.AutoSize = true;
            this.chkAutoPrint.Checked = true;
            this.chkAutoPrint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAutoPrint.Location = new System.Drawing.Point(929, 33);
            this.chkAutoPrint.Margin = new System.Windows.Forms.Padding(4);
            this.chkAutoPrint.Name = "chkAutoPrint";
            this.chkAutoPrint.Size = new System.Drawing.Size(86, 22);
            this.chkAutoPrint.TabIndex = 927;
            this.chkAutoPrint.Text = "自动打印";
            this.chkAutoPrint.UseVisualStyleBackColor = true;
            // 
            // txtDoctor
            // 
            this.txtDoctor.AddEmptyRow = true;
            this.txtDoctor.AutoScroll = true;
            this.txtDoctor.BindByValue = false;
            this.txtDoctor.colDisplay = "";
            this.txtDoctor.colExtend1 = "doc_code";
            this.txtDoctor.colInCode = "";
            this.txtDoctor.colPY = "";
            this.txtDoctor.colSeq = "";
            this.txtDoctor.colValue = "";
            this.txtDoctor.colWB = "";
            this.txtDoctor.displayMember = null;
            this.txtDoctor.EnterMoveNext = true;
            this.txtDoctor.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtDoctor.KeyUpDownMoveNext = true;
            this.txtDoctor.LoadDataOnDesignMode = true;
            this.txtDoctor.Location = new System.Drawing.Point(162, 97);
            this.txtDoctor.Margin = new System.Windows.Forms.Padding(4);
            this.txtDoctor.MaximumSize = new System.Drawing.Size(667, 26);
            this.txtDoctor.MinimumSize = new System.Drawing.Size(67, 26);
            this.txtDoctor.Name = "txtDoctor";
            this.txtDoctor.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtDoctor.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtDoctor.Readonly = false;
            this.txtDoctor.SaveSourceID = false;
            this.txtDoctor.SelectFilter = null;
            this.txtDoctor.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtDoctor.SelectOnly = false;
            this.txtDoctor.Size = new System.Drawing.Size(157, 26);
            this.txtDoctor.TabIndex = 925;
            this.txtDoctor.UseExtend = false;
            this.txtDoctor.valueMember = null;
            // 
            // selectDict_Origin1
            // 
            this.selectDict_Origin1.AddEmptyRow = true;
            this.selectDict_Origin1.AutoScroll = true;
            this.selectDict_Origin1.BindByValue = false;
            this.selectDict_Origin1.colDisplay = "";
            this.selectDict_Origin1.colExtend1 = null;
            this.selectDict_Origin1.colInCode = "";
            this.selectDict_Origin1.colPY = "";
            this.selectDict_Origin1.colSeq = "";
            this.selectDict_Origin1.colValue = "";
            this.selectDict_Origin1.colWB = "";
            this.selectDict_Origin1.displayMember = null;
            this.selectDict_Origin1.EnterMoveNext = true;
            this.selectDict_Origin1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDict_Origin1.KeyUpDownMoveNext = true;
            this.selectDict_Origin1.LoadDataOnDesignMode = true;
            this.selectDict_Origin1.Location = new System.Drawing.Point(162, 33);
            this.selectDict_Origin1.Margin = new System.Windows.Forms.Padding(4);
            this.selectDict_Origin1.MaximumSize = new System.Drawing.Size(667, 26);
            this.selectDict_Origin1.MinimumSize = new System.Drawing.Size(67, 26);
            this.selectDict_Origin1.Name = "selectDict_Origin1";
            this.selectDict_Origin1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDict_Origin1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDict_Origin1.Readonly = false;
            this.selectDict_Origin1.SaveSourceID = false;
            this.selectDict_Origin1.SelectFilter = null;
            this.selectDict_Origin1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDict_Origin1.SelectOnly = true;
            this.selectDict_Origin1.Size = new System.Drawing.Size(157, 26);
            this.selectDict_Origin1.TabIndex = 916;
            this.selectDict_Origin1.UseExtend = false;
            this.selectDict_Origin1.valueMember = null;
            // 
            // dateTimeControl1
            // 
            this.dateTimeControl1.EditValue = null;
            this.dateTimeControl1.EnterMoveNextControl = true;
            this.dateTimeControl1.Location = new System.Drawing.Point(559, 33);
            this.dateTimeControl1.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimeControl1.Name = "dateTimeControl1";
            this.dateTimeControl1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateTimeControl1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateTimeControl1.Properties.DisplayFormat.FormatString = "u";
            this.dateTimeControl1.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateTimeControl1.Properties.EditFormat.FormatString = "u";
            this.dateTimeControl1.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateTimeControl1.Properties.Mask.EditMask = "u";
            this.dateTimeControl1.Size = new System.Drawing.Size(252, 24);
            this.dateTimeControl1.TabIndex = 917;
            // 
            // txtDeptId
            // 
            this.txtDeptId.AddEmptyRow = true;
            this.txtDeptId.AutoScroll = true;
            this.txtDeptId.BindByValue = false;
            this.txtDeptId.colDisplay = "";
            this.txtDeptId.colExtend1 = null;
            this.txtDeptId.colInCode = "";
            this.txtDeptId.colPY = "";
            this.txtDeptId.colSeq = "";
            this.txtDeptId.colValue = "";
            this.txtDeptId.colWB = "";
            this.txtDeptId.displayMember = null;
            this.txtDeptId.EnterMoveNext = true;
            this.txtDeptId.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtDeptId.KeyUpDownMoveNext = false;
            this.txtDeptId.LoadDataOnDesignMode = true;
            this.txtDeptId.Location = new System.Drawing.Point(162, 65);
            this.txtDeptId.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeptId.MaximumSize = new System.Drawing.Size(667, 26);
            this.txtDeptId.MinimumSize = new System.Drawing.Size(67, 26);
            this.txtDeptId.Name = "txtDeptId";
            this.txtDeptId.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtDeptId.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtDeptId.Readonly = false;
            this.txtDeptId.SaveSourceID = false;
            this.txtDeptId.SelectFilter = null;
            this.txtDeptId.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtDeptId.SelectOnly = true;
            this.txtDeptId.Size = new System.Drawing.Size(157, 26);
            this.txtDeptId.TabIndex = 924;
            this.txtDeptId.UseExtend = false;
            this.txtDeptId.valueMember = null;
            // 
            // txtCombineEdit
            // 
            this.txtCombineEdit.EnterMoveNextControl = true;
            this.txtCombineEdit.Location = new System.Drawing.Point(559, 97);
            this.txtCombineEdit.Margin = new System.Windows.Forms.Padding(4);
            this.txtCombineEdit.Name = "txtCombineEdit";
            this.txtCombineEdit.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCombineEdit.Properties.Appearance.Options.UseFont = true;
            this.txtCombineEdit.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtCombineEdit.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            toolTipTitleItem5.Text = "刷新当前组合对应的项目";
            superToolTip5.Items.Add(toolTipTitleItem5);
            toolTipTitleItem6.Text = "移除没有结果的项目";
            superToolTip6.Items.Add(toolTipTitleItem6);
            serializableAppearanceObject7.BorderColor = System.Drawing.Color.Gray;
            serializableAppearanceObject7.Options.UseBorderColor = true;
            toolTipTitleItem7.Text = "添加组合";
            superToolTip7.Items.Add(toolTipTitleItem7);
            serializableAppearanceObject8.BorderColor = System.Drawing.Color.Gray;
            serializableAppearanceObject8.Options.UseBorderColor = true;
            toolTipTitleItem8.Text = "移除组合";
            superToolTip8.Items.Add(toolTipTitleItem8);
            this.txtCombineEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Up, "刷新当前组合对应的项目", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, superToolTip5, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject6, "", null, superToolTip6, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "添加", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject7, "", null, superToolTip7, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Minus, "移除", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject8, "", null, superToolTip8, false)});
            this.txtCombineEdit.Size = new System.Drawing.Size(252, 22);
            this.txtCombineEdit.TabIndex = 926;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(484, 101);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 15);
            this.label14.TabIndex = 922;
            this.label14.Text = "监测项目";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(87, 101);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 15);
            this.label9.TabIndex = 921;
            this.label9.Text = "采样人员";
            // 
            // lblPatName
            // 
            this.lblPatName.AutoSize = true;
            this.lblPatName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPatName.ForeColor = System.Drawing.Color.Black;
            this.lblPatName.Location = new System.Drawing.Point(484, 67);
            this.lblPatName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPatName.Name = "lblPatName";
            this.lblPatName.Size = new System.Drawing.Size(67, 15);
            this.lblPatName.TabIndex = 920;
            this.lblPatName.Text = "监测对象";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(87, 67);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 919;
            this.label4.Text = "送检科室";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(117, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 923;
            this.label1.Text = "来源";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(484, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 918;
            this.label3.Text = "申请时间";
            // 
            // patientControl1
            // 
            this.patientControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientControl1.Location = new System.Drawing.Point(0, 205);
            this.patientControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.patientControl1.Name = "patientControl1";
            this.patientControl1.SelectType = dcl.client.sample.SelectType.Create;
            this.patientControl1.SelectWhenNotPrint = false;
            this.patientControl1.ShouldMultiSelect = true;
            this.patientControl1.ShowCollectNotice = false;
            this.patientControl1.Size = new System.Drawing.Size(1352, 623);
            printStep2.BaseSampMain = null;
            printStep2.Bcfrequency = null;
            printStep2.EnabledFowardMinutes = false;
            printStep2.FowardMinutes = 0;
            printStep2.MustFinishPreviousAction = true;
            printStep2.Printer = null;
            printStep2.ShouldDoAction = true;
            printStep2.ShouldEnabledBarcodeInput = true;
            printStep2.ShouldEnlableSimpleSearchPanel = true;
            coolStepController2.MustFinishPreviousAction = true;
            coolStepController2.ShouldDoAction = true;
            printStep2.StepController = coolStepController2;
            printStep2.TimeOutValue = new System.DateTime(((long)(0)));
            this.patientControl1.Step = printStep2;
            this.patientControl1.StepType = dcl.client.sample.StepType.Print;
            this.patientControl1.TabIndex = 21;
            // 
            // lbEvaluteMore
            // 
            this.lbEvaluteMore.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lbEvaluteMore.Location = new System.Drawing.Point(818, 70);
            this.lbEvaluteMore.Name = "lbEvaluteMore";
            this.lbEvaluteMore.Size = new System.Drawing.Size(30, 18);
            this.lbEvaluteMore.TabIndex = 939;
            this.lbEvaluteMore.Text = "多选";
            // 
            // BCManualYG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.patientControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.sysToolBar1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BCManualYG";
            this.Size = new System.Drawing.Size(1352, 828);
            this.Load += new System.EventHandler(this.BCManualYG_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAge.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeControl1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateTimeControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCombineEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private dcl.client.control.SelectDicSample lueSample;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPatName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label14;
        private DevExpress.XtraEditors.ButtonEdit txtCombineEdit;
        protected control.SelectDicPubDept txtDeptId;
        private DevExpress.XtraEditors.DateEdit dateTimeControl1;
        private control.SelectDicPubSource selectDict_Origin1;
        private control.SelectDicPubDoctor txtDoctor;
        private System.Windows.Forms.CheckBox chkAutoPrint;
        private System.Windows.Forms.CheckBox checkDepart;
        private System.Windows.Forms.CheckBox checkDoctor;
        private System.Windows.Forms.CheckBox checkPatName;
        private System.Windows.Forms.CheckBox checkItem;
        private control.SelectDicPubEvaluate selectDic_Pub_Evalute;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.ComboBoxEdit cbSex;
        private lis.client.control.TextAgeInput txtAge;
        private System.Windows.Forms.CheckBox ckbNotSex;
        private System.Windows.Forms.CheckBox ckbNotAge;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private PatientControlForMed patientControl1;
        private DevExpress.XtraEditors.LabelControl lbEvaluteMore;
    }
}
