using System;
using System.Collections;
using System.Media;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using lis.client.control;
using dcl.common.extensions;
using dcl.common;
using dcl.client.wcf;
using System.Drawing;
using System.Data;
using System.IO;
using System.Collections.Generic;
using dcl.entity;
using System.Linq;
using dcl.client.cache;
using System.Threading;

namespace dcl.client.sample
{
    public partial class BCConfirm : ClientBaseControl, IStepable
    {
        public IStep Step
        {
            get;
            set;
        }
        private string OperatorID = string.Empty;
        private string OperatorName = string.Empty;
        private string OperatorStfId = string.Empty;
        //条码送达与签收界面强制限制目的地
        private bool BC_ForceSendDestFlag = false;

        /// <summary>
        /// 采集时提示相同发票号未采集的
        /// </summary>
        private bool IsSamePid_readySam = false;

        /// <summary>
        /// 医生姓名
        /// </summary>
        public string DoctorName { get; set; }

        /// <summary>
        /// 医生工号
        /// </summary>
        public string DoctorCode { get; set; }

        public BCConfirm()
        {
            InitializeComponent();
            patientControl.ShouldShowIndicator = true;
        }

        private System.Windows.Forms.Form thisForm;

        public System.Windows.Forms.Form ThisForm
        {
            get { return thisForm; }
            set { thisForm = value; }
        }

        private StepType stepType;
        public StepType StepType
        {
            get { return stepType; }
            set
            {
                if (DesignMode)
                    return;

                patientControl.StepType = stepType = value;
                Step = StepFactory.CreateStep(value);

            }
        }

        public override void InitData()
        {
        }

        string strDep = string.Empty;

        private void BCConfirm_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            //系统配置：[标本确认]提示相同发票号未采集的
            IsSamePid_readySam = UserInfo.GetSysConfigValue("BCConfirm_samePid_readySam") == "是";

            lblOp.Visible = true;
            lblOP2.Visible = true;

            UserInfo.SkipPower = true;

            sysToolBar1.BtnSave.Caption = "回退标本";
            sysToolBar1.BtnDeSpe.Caption = "记录";
            sysToolBar1.BtnBCPrint.Caption = "复制标本信息";
            string strPrintBarcodeInfo = string.Empty;

            BC_ForceSendDestFlag = ConfigHelper.GetSysConfigValueWithoutLogin("BC_ForceSendDestFlag") == "是";
            strPrintBarcodeInfo = sysToolBar1.BtnQualityTest.Name;

            sysToolBar1.btnBrowse.Caption = "查询申请单号";

            if (this.StepType == StepType.Sampling || this.stepType == StepType.Confirm)//如果为采集确认则显示打印条码回执
            {
                string btnAnswer = string.Empty;
                string btnCopy = string.Empty;
                if (StepType == StepType.Sampling && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EablePackBarcode") == "是")
                {
                    btnAnswer = "BtnAnswer";
                    sysToolBar1.BtnAnswer.Caption = "标本打包";
                    sysToolBar1.BtnAnswerClick += new EventHandler(sysToolBar1_BtnAnswerClick);
                }
                if (StepType == StepType.Confirm)
                {
                    btnCopy = "BtnQualityOut";
                    sysToolBar1.BtnQualityOut.Caption = "复制标本信息";
                    sysToolBar1.OnBtnQualityOutClicked += new EventHandler(sysToolBar1_BtnCopyClick);
                }

                //启用签收时同步资料登记录入功能
                if (StepType == StepType.Confirm && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EnableConfirmwithSignIn") == "是")
                {
                    chkRegFlag.Visible = true;
                    rgType.Visible = true;
                }
                else
                {
                    chkRegFlag.Visible = false;
                    rgType.Visible = false;
                }

                string btnResultView = string.Empty;
                if (this.stepType == StepType.Confirm && UserInfo.GetSysConfigValue("Barcode_ExportConfirmwithSignIn") == "是")
                {
                    sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnExport.Name });
                    sysToolBar1.OnBtnExportClicked += new EventHandler(sysToolBar1_OnBtnExportClicked);
                }

                sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnConfirm.Name,
                sysToolBar1.BtnClear.Name,
                sysToolBar1.BtnReset.Name,
                sysToolBar1.BtnBCPrintReturn.Name,
                sysToolBar1.BtnPrintList.Name,
                sysToolBar1.BtnSave.Name,
                StepType == StepType.Sampling ? string.Empty : strPrintBarcodeInfo,
                sysToolBar1.btnBrowse.Name,
                btnAnswer,
                btnResultView,
                sysToolBar1.BtnDeSpe.Name,
                sysToolBar1.BtnSearch.Name,
                sysToolBar1.BtnStat.Name,
                sysToolBar1.BtnClose.Name,
                btnCopy}, new string[] { "F2", "F5", "F4", "F3", "F6", "F7", "F12" });
                sysToolBar1.BtnBCPrintReturn.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                sysToolBar1.SetToolButtonStyle(new string[] {
                sysToolBar1.BtnConfirm.Name,
                sysToolBar1.BtnClear.Name,
                sysToolBar1.BtnReset.Name,
                sysToolBar1.BtnPrintList.Name,
                sysToolBar1.BtnSave.Name,
                sysToolBar1.btnBrowse.Name,
                sysToolBar1.BtnDeSpe.Name,
                sysToolBar1.BtnSearch.Name,
                sysToolBar1.BtnStat.Name,
                sysToolBar1.BtnClose.Name }, new string[] { "F2", "F3", "F4", "F5", "F6", "F7", "F12" });
            }
            sysToolBar1.btnBrowse.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sysToolBar1.OnBtnSearchClicked += SysToolBar1_OnBtnSearchClicked;

            sysToolBar1.OnBtnStatClicked += sysToolBar1_OnBtnStatClicked;

            sysToolBar1.BtnPrintListGerm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;//显示打印细菌清单按钮

            sysToolBar1.BtnQualityTest.Caption = "打印条码信息";

            UserInfo.SkipPower = false;


            InitTitle();
            patientControl.ShowNotice(false);
            patientControl.ClearAllClick += new EventHandler(patientControl1_ClearAllClick);
            patientControl.DoShowBaseInfo += new EventHandler(patientControl1_ShowBaseInfo);
            panelControl2.Visible = Step.ShouldShowSimpleSearchPanel;

            panelControl2.Enabled = Step.ShouldEnlableSimpleSearchPanel;
            txtBarcode.Enabled = Step.ShouldEnabledBarcodeInput;
            Step.enabledBarcodeInput += new IStep.EnabledBarcodeInput(Step_enabledBarcodeInput);
            Step.enlableSimpleSearchPanel += new IStep.EnlableSimpleSearchPanel(Step_enlableSimpleSearchPanel);


            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_AllowSaveUrgent") == "否")
            {
                this.patientControl.gridColumn15.OptionsColumn.AllowEdit = false;
            }

            if (this.StepType == StepType.Confirm || this.StepType == StepType.Reach)
            {
                label12.Visible = true;
                lueTypes.Visible = true;
            }

            if (StepType == StepType.Confirm && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SignEnableBitchModifySamp") == "是")
            {
                patientControl.ShouldMultiSelect = true;
            }

            //读取默认配置物理组
            if (LocalSetting.Current.Setting.IDTypeFlag != null && LocalSetting.Current.Setting.IDTypeFlag.Trim() != "")
            {
                string[] ids = LocalSetting.Current.Setting.IDTypeFlag.Split('|')[0].Split(',');

                ArrayList alType = new ArrayList();
                foreach (string id in ids)
                {
                    alType.Add(id);
                }

                lueTypes.valueMember = alType;
                lueTypes.displayMember = LocalSetting.Current.Setting.IDTypeFlag.Split('|')[1];
            }
            else
            {
                if (LocalSetting.Current.Setting.CType_id != null && LocalSetting.Current.Setting.CType_id.Trim() != "")
                {
                    ArrayList alType = new ArrayList();
                    alType.Add(LocalSetting.Current.Setting.CType_id);
                    lueTypes.valueMember = alType;
                    lueTypes.displayMember = LocalSetting.Current.Setting.CType_name;
                }
            }


            #region 控制查询状态
            cbStatus.Properties.Items.Clear();
            switch (Step.StepName)
            {
                case "采集":
                    cbStatus.Properties.Items.Add("采集");
                    break;
                case "收取":
                    cbStatus.Properties.Items.Add("采集");
                    cbStatus.Properties.Items.Add("二次送检");
                    break;
                case "送达":
                    cbStatus.Properties.Items.Add("收取");
                    cbStatus.Properties.Items.Add("二次送检");
                    break;
                case "签收":
                    cbStatus.Properties.Items.Add("送达");
                    cbStatus.Properties.Items.Add("收取");
                    cbStatus.Properties.Items.Add("二次送检");
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "二次送检":
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "离心":
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "标本上机":
                    cbStatus.Properties.Items.Add("离心");
                    cbStatus.Properties.Items.Add("签收");
                    break;
                case "耗材领取":
                    cbStatus.Properties.Items.Add("条码打印");
                    break;
                default:
                    break;
            }
            cbStatus.SelectedIndex = 0;

            #endregion

            int timeSeconds = 120;
            string timeoutStr = string.Empty;
            //设置不同的底色以区分不同的步骤
            switch (this.StepType)
            {
                case StepType.Sampling:
                    Color colBlooded = IStep.GetBarcodeConfigColor("Barcode_Color_Blooded");
                    SetPanelColor(colBlooded);
                    timeoutStr = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_New_Sampling_TimeOut");
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EablePackBarcode") == "是")
                        label15.Visible = true;
                    break;

                case StepType.Send://硬编码颜色
                    SetPanelColor(Color.Blue);
                    timeoutStr = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_New_Send_TimeOut");

                    break;

                case StepType.Reach:
                    Color Barcode_Color_Reach = IStep.GetBarcodeConfigColor("Barcode_Color_Reach");
                    SetPanelColor(Barcode_Color_Reach);
                    timeoutStr = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_New_Reach_TimeOut");

                    break;

                case StepType.Confirm:
                    Color colReceived = IStep.GetBarcodeConfigColor("Barcode_Color_Received");
                    SetPanelColor(colReceived);
                    timeoutStr = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_New_Sign_TimeOut");

                    break;

                case StepType.SecondSend://硬编码颜色
                    SetPanelColor(Color.RosyBrown);
                    timeoutStr = ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_New_SecondSend_TimeOut");
                    break;
            }

            if (!string.IsNullOrEmpty(timeoutStr))
            {
                int.TryParse(timeoutStr, out timeSeconds);
            }

            ctlTimeCountDown1.TimeSeconds = timeSeconds;
            if (Step.StepName == UserInfo.GetSysConfigValue("barcode_confirmBatchNumber"))
            {
                this.txtBcfrequency.Visible = true;
                this.label14.Visible = true;
            }

            this.ActiveControl = txtBarcode;

            patientControl.SetInfoDisVisableForConfirm();
            if (StepType == StepType.SecondSend && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EnableSecondSendPrint") == "是")
            {
                chkPrint.Visible = true;
            }
            else
            {
                chkPrint.Visible = false;
            }
        }

        /// <summary>
        /// 简单统计工作量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnStatClicked(object sender, EventArgs e)
        {
            FrmSimpleStatistics sta = new FrmSimpleStatistics();
            sta.Show();
        }

        /// <summary>
        /// 复制标本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            string text = patientControl.CopyBarcodeInfo();
            FrmCopyBarcode copy = new FrmCopyBarcode(text);
            copy.Show();
        }

        private void SysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            FrmBCQuery frm = new FrmBCQuery();
            frm.Step = Step;
            frm.StepType = stepType;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                patientControl.LoadData(frm.SampQueryCondition);
                if (!patientControl.HasData())
                {
                    lis.client.control.MessageDialog.Show("无返回数据!");
                    return;
                }
            }
            else
            {
                return;
            }

            if (this.stepType == StepType.Confirm)
            {
                //在签收时,查询签收条码,则不能确认
                if (cbStatus.Visible && cbStatus.Text.Trim() == "签收")
                {
                    sysToolBar1.BtnConfirm.Enabled = false;
                    sysToolBar1.BtnClear.Enabled = false;
                }
            }

            if (patientControl.Selection != null)
                patientControl.Selection.SelectAll();
            Step.ShouldEnabledBarcodeInput = false;
            sysToolBar1.BtnReset.Caption = "重置";

            if (this.ctlTimeCountDown1.Visible)
            {
                this.ctlTimeCountDown1.ReCount();
            }

        }

        void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            patientControl.ExportToExcel();
        }


        void Step_enlableSimpleSearchPanel()
        {
            setBtnEnabled(Step.ShouldEnlableSimpleSearchPanel);
        }

        private void setBtnEnabled(bool isEnabled)
        {
            cbStatus.Enabled = isEnabled;
            sysToolBar1.BtnSearch.Enabled = isEnabled;
        }


        void Step_enabledBarcodeInput()
        {
            txtBarcode.Enabled = Step.ShouldEnabledBarcodeInput;
        }

        void patientControl1_ShowBaseInfo(object sender, EventArgs e)
        {
            if (sender is EntitySampMain)
                ShowTo((sender as EntitySampMain));
        }

        void patientControl1_ClearAllClick(object sender, EventArgs e)
        {
            ClearBaseInfo();
        }

        /// <summary>
        /// 初始化标题
        /// </summary>
        private void InitTitle()
        {
            if (Step != null)
            {
                if (Step.StepName == "二次送检")
                    gcTitle.Text = Step.StepName;
                else
                    gcTitle.Text = "标本" + Step.StepName;
            }
        }

        private void ShowTo(EntitySampMain sampMain)
        {
            lblTo.Text = sampMain.ProName;
            label15.Text = sampMain.SampInfo;
            if (sampMain.SampUrgentFlag && stepType == StepType.Confirm)
            {
                labelUrgentFlag.Visible = true;
            }
            else
            {
                labelUrgentFlag.Visible = false;
            }
        }
        /// <summary>
        /// 清除基本信息
        /// </summary>
        private void ClearBaseInfo()
        {
            labelUrgentFlag.Visible = false;
        }

        bool isNotClearOp = false;
        int packCountTr = 0;

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
                return;

            if (this.chkHsBarCode.Checked && Extensions.IsNotEmpty(txtBarcode.Text) && Step.StepName == "签收")
            {
                List<string> BarCodeList = new List<string>();
                BarCodeList.Add(txtBarcode.Text.Trim());

                ProxySampMain proxySampMain = new ProxySampMain();

                List<EntitySampMain> listPatient = proxySampMain.Service.GetSampleInfo(BarCodeList);
                MessageDialog.ShowAutoCloseDialog("患者数量:" + listPatient.Count.ToString() , 2m);
                bool res = false;
                bool result = false;
                string SampBarCode = string.Empty;

                if (listPatient != null && listPatient.Count > 0)
                {
                    foreach (var patient in listPatient)
                    {
                        SampBarCode += patient.SampBarCode + ",";

                        result = patientControl.AddBarcode(patient.SampBarCode, lueTypes.valueMember);

                        if (!result)
                        {
                            ClearAndFocusBarcode();
                            return;
                        }
                    }
                }

                ClearAndFocusBarcode();
                return;
            }
            else
            {
                try
                {
                   
                    isNotClearOp = false;

                    if (txtBarcode.Text.Trim().Length > 12
                        && StepType != StepType.Sampling
                        && ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_EablePackBarcode") == "是")
                    {
                        EntitySampQC sampQC = new EntitySampQC();
                        sampQC.PidUniqueId = txtBarcode.Text.Trim();
                        patientControl.LoadDataForPack(sampQC);
                        if (patientControl.HasData())
                        {
                            isNotClearOp = true;

                            if (patientControl.Selection != null)
                                patientControl.Selection.SelectAll();
                            sysToolBar1.BtnReset.Caption = "重置";

                            barConfirm(false, false, false);
                            lbPackCount.Visible = true;
                            string strStep = string.Empty;
                            switch (this.StepType)
                            {
                                case StepType.Send:
                                    strStep = "3";
                                    break;
                                case StepType.Reach:
                                    strStep = "4";
                                    break;
                                case StepType.Confirm:
                                    strStep = "5";
                                    break;
                                default:
                                    break;
                            }
                            if (!string.IsNullOrEmpty(strStep))
                            {
                                List<EntitySampMain> dtSource = patientControl.ListSampMain;
                                if (dtSource != null && dtSource.Count > 0)
                                {
                                    int count = dtSource.Count - 1;
                                    string bc_d_code = dtSource[count].PidDeptCode;
                                    ProxySampMain proxy = new ProxySampMain();
                                    List<string> list = proxy.Service.GetPackCount(strStep, bc_d_code);
                                    if (list.Count > 1)
                                    {
                                        int packCount = Convert.ToInt16(list[0].ToString());
                                        if (dtSource[count].SampStatusId != strStep)
                                        {
                                            if (packCountTr == 0)
                                                packCountTr = packCount + 1;
                                            else
                                                packCountTr++;
                                        }
                                        else
                                            packCountTr = packCount;
                                        lbPackCount.Text = string.Format(@"已收取包数：{0}  应收取包数：{1}", packCountTr.ToString(), list[1].ToString());
                                    }

                                }
                            }

                        }
                        ClearAndFocusBarcode();
                        return;
                    }

                    bool isNeedChildConfirm = false;
                    if (Extensions.IsNotEmpty(txtBarcode.Text))
                    {
                        bool result = false;
                        patientControl.Step.Bcfrequency = this.txtBcfrequency.Text.Trim();

                        //系统配置--条码确认时[回退条码]加急提醒
                        if (UserInfo.GetSysConfigValue("IsReturnBarCodeClew") == "是")
                        {
                            if (this.StepType == StepType.Sampling || this.StepType == StepType.Send || this.StepType == StepType.Reach || this.StepType == StepType.Confirm)
                            {
                                patientControl.ReturnBarCodeClew(txtBarcode.Text);
                            }
                        }

                        if ((Step.StepName == "签收" || (BC_ForceSendDestFlag && Step.StepName == "送达"))
                            && lueTypes.valueMember != null
                            && lueTypes.valueMember.Count > 0)
                            result = patientControl.AddBarcode(txtBarcode.Text, lueTypes.valueMember);
                        else
                            result = patientControl.AddBarcode(txtBarcode.Text);

                        if (!result)
                        {
                            ClearAndFocusBarcode();
                            return;
                        }

                        Step.ShouldEnlableSimpleSearchPanel = false;

                        sysToolBar1.BtnClear.Enabled = false;
                        sysToolBar1.BtnReset.Caption = "完成";

                        if (patientControl.HasData())
                        {
                            ShowTo(patientControl.BaseSampMain);
                            //检验科标本收集时，扫描条码时，碰到急诊标本请用不同声音提示操作者
                            if (StepType == StepType.Confirm && patientControl.BaseSampMain.SampUrgentFlag && File.Exists(Application.StartupPath + @"\Urgent.wav"))
                            {
                                SoundPlayer soundPlayer = new SoundPlayer();
                                soundPlayer.SoundLocation = Application.StartupPath + @"\Urgent.wav";
                                soundPlayer.Load();
                                soundPlayer.Play();
                                soundPlayer.Dispose();
                            }
                        }

                        if (this.ctlTimeCountDown1.Visible)
                        {
                            this.ctlTimeCountDown1.ReCount();
                        }

                        sysToolBar1.BtnConfirm.Enabled = false;
                        barConfirm(false, false, isNeedChildConfirm);
                        ClearAndFocusBarcode();
                    }
                }
                catch (Exception ex)
                {
                    ClearAndFocusBarcode();
                    Lib.LogManager.Logger.LogException(ex);
                    MessageDialog.ShowAutoCloseDialog(ex.Message);
                }
            }
            
        }

        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                ParentForm.Close();
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        private void sysToolBar1_BtnClearClick(object sender, EventArgs e)
        {
            if (patientControl.ClearSelected())
                ClearAndFocusBarcode();

            if (!patientControl.HasData())
                patientControl.Reset();
        }


        /// <summary>
        /// 确认
        /// </summary>
        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            //不允许批量确认的流转操作(0-采集，1-收取，2-送达，3-签收)
            string opers = ConfigHelper.GetSysConfigValueWithoutLogin("NotAllowBatchConfirmOperations");

            if (!string.IsNullOrEmpty(opers))
            {
                List<string> operList = opers.Split(',').ToList<string>();
                
                foreach(string s in operList)
                {
                    if(Convert.ToInt32(s) == (int)this.StepType)
                    {
                        MessageDialog.ShowAutoCloseDialog("当前操作不允许批量确认！", 3m);
                        return;
                    }
                }
            }

            barConfirm(true, false, false);
        }

        /// <summary>
        /// 条码确认
        /// </summary>
        /// <param name="isSelect"></param>
        /// <param name="isDoubleReceive"></param>
        /// <param name="isNeedChildConfirm"></param>
        private void barConfirm(bool isSelect, bool isDoubleReceive, bool isNeedChildConfirm)
        {
            IStep StepBack = Step;

            if (isDoubleReceive)
            {
                Step = new ReceiveStep();
                patientControl.Step = Step;
            }

            if (isSelect)
                ClearOperatorMes();
            Step.BaseSampMain = patientControl.BaseSampMain;
            if (string.IsNullOrEmpty(Step.BaseSampMain.SampBarId))
            {
                lis.client.control.MessageDialog.Show("无法获取条码信息,请选择一条码!");
                return;
            }

            if (cmbFowardMinutes.Visible)
                patientControl.Step.FowardMinutes = Convert.ToInt32(cmbFowardMinutes.Text);

            //系统配置：[采集]收取[送达]默认当前登录者
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_SamplingORSendORReachUseloginID") == "是"
                && Step != null 
                && !string.IsNullOrEmpty(Step.StepCode)
                && (Step.StepCode == "2" || Step.StepCode == "3" || Step.StepCode == "4")
                && !string.IsNullOrEmpty(UserInfo.loginID) 
                && !string.IsNullOrEmpty(UserInfo.userName))
            {
                OperatorID = UserInfo.loginID;
                OperatorName = UserInfo.userName;
            }

            powerConfirm();

            if (OperatorID != string.Empty && OperatorName != string.Empty)
            {
                #region 操作前确认费用，确认失败去不允许继续操作。
                EntitySampOperation sign = new EntitySampOperation(OperatorID, OperatorName);
                sign.OperationStatus = Step.StepCode;
                sign.OperationStatusName = Step.StepName;
                sign.OperationPlace = LocalSetting.Current.Setting.CType_name;
                sign.Remark = string.Format("IP地址:{0}", IPUtility.GetIP());
                sign.OperationTime = IStep.GetServerTime();
                sign.OperationWorkId = OperatorStfId;

                ProxySampMain proxy = new ProxySampMain();
                string strResultMsg = proxy.Service.ConfirmBeforeCheck(sign, Step.BaseSampMain);

                if (strResultMsg.Trim() != string.Empty)
                {
                    MessageDialog.Show(strResultMsg + Environment.NewLine + "请回退该条码。");

                    if (patientControl.HasData())
                        patientControl.Reset();
                    ClearBaseInfo();
                    ClearAndFocusBarcode();
                    return;
                }
                #endregion

                string strMess = string.Empty;
                if (patientControl.ComfirmAll(OperatorID, OperatorName, isSelect, OperatorStfId))
                {

                    strMess = "操作成功!";

                    if (StepType == StepType.SecondSend && chkPrint.Checked == true)
                    {
                        patientControl.isNotUpdateFlag = true;
                        patientControl.PrintBarcodeAuto(1);
                    }
                    foreach (EntitySampMain drBac in patientControl.MainTable)
                    {
                        if (drBac.SampStatusId != Step.StepCode)
                            drBac.SampLastactionDate = ServerDateTime.GetServerDateTime();
                        drBac.SampStatusId = Step.StepCode;
                    }
                    if (chkRegFlag.Visible && chkRegFlag.Checked)
                        ConfirmWithSignIn();
                    ClearAndFocusBarcode();
                }
                else
                    strMess = "操作失败!";

                if (isSelect) //扫条码不弹出提示，查询确认才提示
                    lis.client.control.MessageDialog.Show(strMess);

                //结果在这里又把焦点设置到第一条
                if (patientControl.MainGridView.RowCount > 0)
                {
                    //标本流转的时候,是否跳转到最后一行
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ActionMoveLast") == "是")
                    {
                        patientControl.MoveLast();
                        patientControl.RefreshCurrentBarcodeInfo();
                    }
                    else
                        patientControl.MainGridView.FocusedRowHandle = 0;
                }
            }

            if (isDoubleReceive)
            {
                Step = StepBack;
                patientControl.Step = StepBack;
            }
        }


        string ItrId = string.Empty;
        string ItrName = string.Empty;
        /// <summary>
        /// 签收同步登记
        /// </summary>
        private void ConfirmWithSignIn()
        {
            string strBarcode = txtBarcode.Text;
            ProxyPidReportMain proxyPatient = new ProxyPidReportMain();
            EntityPidReportMain patInfo = proxyPatient.Service.GetPatientsByBarCode(strBarcode);
            if (patInfo != null && !string.IsNullOrEmpty(patInfo.RepBarCode))
            {
                patInfo.RepCheckUserId = OperatorID;
                #region 根据组合所在的仪器自动分配仪器
                //根据组合所在的仪器自动分配仪器
                IList<string> combineIDs = new List<string>();
                foreach (EntityPidReportDetail row in patInfo.ListPidReportDetail)
                {
                    if (!combineIDs.Contains(row.ComId))
                        combineIDs.Add(row.ComId);
                }

                if (combineIDs.Count == 0)
                {
                    lis.client.control.MessageDialog.Show("组合找不到仪器！", "提示");
                    return;
                }

                List<EntityDicItrCombine> listInsCom = CacheClient.GetCache<EntityDicItrCombine>();

                List<EntityDicInstrument> listIns = CacheClient.GetCache<EntityDicInstrument>();

                List<EntityDicInstrument> dsInstrments = listIns.FindAll(z => listInsCom.FindIndex(
                                                                         w => combineIDs.Contains(w.ComId) &&
                                                                         w.ItrId == z.ItrId) >= 0);

                if (dsInstrments != null && dsInstrments.Count > 0)
                {
                    List<EntityDicInstrument> dtInstrumentType = new List<EntityDicInstrument>();//用于存放过滤物理组的仪器
                    string typeId = patInfo.ItrLabId;//物理组别
                    foreach (EntityDicInstrument drIns in dsInstrments)
                    {
                        dtInstrumentType.Add((EntityDicInstrument)drIns.Clone());
                    }
                    if (dtInstrumentType.Count == 0)//没有此物理组的仪器
                    {
                        lis.client.control.MessageDialog.Show("当前条码组合不能在此物理组登记!");
                        return;
                    }
                    else if (dtInstrumentType.Count == 1)//属于此物理组的仪器只有一台
                    {
                        patInfo.RepItrId = dtInstrumentType[0].ItrId;
                        patInfo.ItrName = dtInstrumentType[0].ItrEname;
                    }
                    else//列出列表给予用户选择
                    {
                        int itrCount = 0;
                        bool isItrAll = false;
                        string itrComId = "";
                        string itrComName = "";

                        #region 处理组合是否全部在某个仪器当中，这样的仪器有几台
                        StringBuilder comId = new StringBuilder();
                        bool isFrist = false;
                        for (int i = 0; i < combineIDs.Count; i++)
                        {
                            if (isFrist)
                                comId.Append(",");
                            comId.Append(string.Format("'{0}'", combineIDs[i]));
                            isFrist = true;
                        }
                        List<string> listItrIds = new List<string>();
                        foreach (EntityDicInstrument drInsType in dtInstrumentType)
                        {
                            listItrIds.Add(drInsType.ItrId);
                            string insId = drInsType.ItrId;
                            int insComLength = listInsCom.FindAll(w => w.ItrId == insId && combineIDs.Contains(w.ComId)).Count;
                            if (insComLength >= combineIDs.Count)
                            {
                                itrComId = insId;
                                itrComName = drInsType.ItrEname;
                                itrCount++;
                                isItrAll = true;
                            }

                        }
                        #endregion

                        if (!isItrAll || (isItrAll == true && itrCount > 1))
                        {
                            if (!listItrIds.Contains(ItrId) || string.IsNullOrEmpty(ItrId))
                            {
                                FrmSelectInstrument frmSelectInstrument = new FrmSelectInstrument();
                                frmSelectInstrument.Instrments = dtInstrumentType;
                                frmSelectInstrument.ShowDialogByData();
                                if (string.IsNullOrEmpty(frmSelectInstrument.SelectInstrumentID))
                                {
                                    lis.client.control.MessageDialog.Show("无效仪器信息!");
                                    return;
                                }
                                ItrId = frmSelectInstrument.SelectInstrumentID;
                                ItrName = frmSelectInstrument.SelectInstrumentName;
                            }
                            patInfo.RepItrId = ItrId;
                            patInfo.ItrName = ItrName;
                        }
                        else
                        {
                            patInfo.RepItrId = itrComId;
                            patInfo.ItrName = itrComName;
                        }
                    }
                    bool Lab_CheckInstrmtAudit = UserInfo.GetSysConfigValue("Lab_CheckInstrmtAudit") == "是";
                    if (Lab_CheckInstrmtAudit && patInfo.RepItrId != null && !UserInfo.isAdmin &&
                        !string.IsNullOrEmpty(patInfo.RepItrId.ToString()))
                    {
                        List<EntityUserInstrmt> listInstrmt = new ProxyUserManage().Service.GetUserCanMgrIInstrmt(
                                patInfo.RepItrId.ToString());
                        if (listInstrmt.Count > 0)
                        {
                            if (listInstrmt.Where(w => w.UserLoginid == UserInfo.loginID).Count() == 0)
                            {
                                lis.client.control.MessageDialog.Show(string.Format("您无操作仪器[{0}]的权限!", patInfo.RepItrId));
                                return;
                            }
                        }
                    }

                }
                else
                {
                    lis.client.control.MessageDialog.Show("无法获取仪器信息!");
                    return;
                }

                #endregion

                DateTime date = ServerDateTime.GetServerDateTime();
                patInfo.RepInDate = date;

                //按样本号或序号
                if (rgType.SelectedIndex == 0) //按样本号
                {
                    patInfo.RepSid = proxyPatient.Service.GetItrSID_MaxPlusOne(date, patInfo.RepItrId.ToString(), true);
                }
                else//如果是序号,条码号当成样本号保存
                {
                    if (rgType.SelectedIndex == 1)
                    {

                        patInfo.RepSerialNum = proxyPatient.Service.GetItrHostOrder_MaxPlusOne(date, patInfo.RepItrId.ToString());
                    }
                    patInfo.RepSid = strBarcode; //条码号当成样本号保存
                }

                List<EntityPidReportDetail> dtBCCombineCopy = new List<EntityPidReportDetail>();
                StringBuilder sbComIn = new StringBuilder();
                StringBuilder sbComNotIn = new StringBuilder();
                #region 判断组合是否能在此仪器内做
                bool isFist = false;
                bool isNotInFist = false;
                List<string> comid = DictInstrmt.Instance.GetItrCombineID(patInfo.RepItrId.ToString(), true);

                foreach (EntityPidReportDetail patCom in patInfo.ListPidReportDetail)
                {
                    if (comid.Exists(i => i == patCom.ComId))
                    {
                        dtBCCombineCopy.Add(patCom);
                        if (isFist)
                            sbComIn.Append(",");

                        sbComIn.Append(patCom.PatComName);
                        isFist = true;
                    }
                    else
                    {
                        if (isNotInFist)
                            sbComNotIn.Append(",");

                        sbComNotIn.Append(patCom.PatComName);
                        isNotInFist = true;
                    }
                }
                #endregion

                if (dtBCCombineCopy.Count == 0)
                {
                    patientControl.RemoveRow(strBarcode);
                    MessageDialog.Show("当前条码组合不能在此仪器登记！", "提示");
                    ClearAndFocusBarcode();
                    return;
                }

                if (dtBCCombineCopy.Count != patInfo.ListPidReportDetail.Count)
                {
                    string strmes = sbComIn.ToString() + "可以在此仪器登记！\r\n" + sbComNotIn.ToString() + "组合不能在此仪器登记！";
                    if (MessageDialog.Show(strmes, "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        patientControl.RemoveRow(strBarcode);
                        cacheItrID = string.Empty;
                        ClearAndFocusBarcode();
                        return;
                    }
                }
                string patItrId = patInfo.RepItrId;
                string pat_com_id = dtBCCombineCopy[0].ComId.ToString();

                EntityRemoteCallClientInfo caller = dcl.client.common.Util.ToCallerInfo();
                caller.Remarks = LocalSetting.Current.Setting.Description;

                EntityOperateResult opResult = proxyPatient.Service.SavePatient(caller, patInfo);

                ClearAndFocusBarcode();
                if (opResult.Success)
                {
                    foreach (EntitySampMain samp in patientControl.MainTable)
                    {
                        if (samp.SampLastactionDate != null && !string.IsNullOrEmpty(samp.SampLastactionDate.ToString()) &&
                            samp.SampStatusId != "20")
                            samp.SampLastactionDate = ServerDateTime.GetServerDateTime();
                        samp.SampStatusId = "20";
                    }


                    if (patientControl.MainGridView.RowCount > 0)
                        patientControl.MainGridView.FocusedRowHandle = 0;
                }
                else
                {
                    if (opResult.HasExcaptionError)
                    {
                        lis.client.control.MessageDialog.Show("同步登记失败，请在标本登记界面登记", "提示");
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show(
                            OperationMessageHelper.GetErrorMessage(opResult.Message), "提示");
                    }
                }
            }

            else
            {
                lis.client.control.MessageDialog.Show("无此条形码的信息", "提示");
                ClearAndFocusBarcode();
                return;
            }
        }

        string cacheItrID = string.Empty;

        /// <summary>
        /// 权限确定
        /// </summary>
        private void powerConfirm()
        {
            int count = patientControl.GetAllUsedBarcodesCount();
            if (count <= 0)
            {
                if (isNotClearOp)
                    lis.client.control.MessageDialog.Show("此条码已经采集!");
                else
                    lis.client.control.MessageDialog.Show("无可确认条码,请确保条码流程正确!");

                return;
            }

            //如果HIS那边传入了病人工号和名字，删除、采集的时候不需要弹出验证。直接过。
            bool skipPower = !string.IsNullOrEmpty(DoctorName) && !string.IsNullOrEmpty(DoctorCode);
            bool notNeedSign = (Step.StepName == "送达" && ConfigHelper.GetSysConfigValueWithoutLogin("ReachStepNeedSign") == "否") || skipPower;
            if (OperatorID == string.Empty || OperatorName == string.Empty)//扫第一条条码时需预确认
            {
                lblOp.Text = string.Empty;
                FrmHISCheckPassword frm = new FrmHISCheckPassword(Step.Audit);
                frm.Text = Step.StepName + " 待确认条码数:" + count.ToString();
                if (!notNeedSign && (frm.ShowDialog() != DialogResult.OK))
                {
                    bacReset();
                    return;
                }

                //录入时再次检查权限
                string funcInfoID = string.Empty;
                if (!notNeedSign && frm.OperatorID != "admin" &&
                    ConfigHelper.GetSysConfigValueWithoutLogin("BCConfirm_ReCheckSignFunc") == "是")
                {
                    bool isOk = ReCheckPowerFunc(frm, funcInfoID);
                    if (!isOk)
                    {
                        OperatorID = string.Empty;
                        OperatorName = string.Empty;
                        MessageDialog.Show("该账号无此权限！");
                        powerConfirm();
                        return;
                    }
                }

                List<dcl.entity.EntityUserRole> listUserRole = new List<entity.EntityUserRole>();
                listUserRole = listUserRole.FindAll(w => w.UserLoginId == frm.OperatorID && w.RoleRemark.IndexOf("护工组") >= 0 );
                lblOp.Text = frm.OperatorName;

                if (Step.StepName == "签收" || Step.StepName == "二次送检")
                {
                    if (listUserRole.Count > 0)
                    {
                        lis.client.control.MessageDialog.Show("护工组无此权限！");
                        powerConfirm();
                    }
                    else
                    {
                        OperatorID = frm.OperatorID;
                        OperatorName = frm.OperatorName;
                        OperatorStfId = frm.OperatorSftId;
                        lblOp.Text = frm.OperatorName;
                    }
                }
                else
                {
                    if (notNeedSign)//不需要签名
                    {
                        OperatorID = UserInfo.loginID;
                        OperatorName = UserInfo.userName;
                        if (skipPower)
                        {
                            OperatorID = DoctorCode;
                            OperatorName = DoctorName;
                        }
                    }
                    else
                    {
                        lblOp.Text = frm.OperatorName;
                        OperatorID = frm.OperatorID;
                        OperatorName = frm.OperatorName;
                        OperatorStfId = frm.OperatorSftId;
                    }

                    if (Step.StepName == "采集")
                    {
                        #region 检查是否护工组的用户
                        //检查是否护工组的用户,如果是,则需要重新验证。
                        bool blnNowhile = false;//是否跳出循环
                        while (!blnNowhile)
                        {
                            if (frm.OperatorID != "admin")
                            {
                                if (listUserRole.Count > 0)
                                {
                                    lis.client.control.MessageDialog.Show("护工组无此权限！请换用户重新验证");
                                    string strTempfrmText = frm.Text;

                                    frm = new FrmHISCheckPassword(Step.Audit);
                                    frm.Text = strTempfrmText;
                                    if (frm.ShowDialog() != DialogResult.OK)
                                    {
                                        bacReset();
                                        return;
                                    }
                                    lblOp.Text = frm.OperatorName;
                                    OperatorID = frm.OperatorID;
                                    OperatorName = frm.OperatorName;
                                    OperatorStfId = frm.OperatorSftId;
                                    continue;
                                }
                            }
                            blnNowhile = true;//跳出循环
                        }
                        #endregion
                    }
                }

                frm.Dispose();
            }
            else
            {
                patientControl.Step.Bcfrequency = Step.Bcfrequency;
            }
        }

        private bool ReCheckPowerFunc(FrmHISCheckPassword frm, string funcInfoID)
        {
            ProxySysUserInfo proxyUser = new ProxySysUserInfo();
            EntityUserQc otherUserQc = new EntityUserQc();
            otherUserQc.LoginId = frm.OperatorID;

            switch (StepType)
            {
                case StepType.Sampling:
                    funcInfoID = "219";
                    break;
                case StepType.Send:
                    funcInfoID = "220";
                    break;
                case StepType.Reach:
                    funcInfoID = "221";
                    break;
                case StepType.Confirm:
                    funcInfoID = "222";
                    break;
                case StepType.SecondSend:
                    funcInfoID = "247";
                    break;
            }

            if (!string.IsNullOrEmpty(funcInfoID))
            {
                otherUserQc.FuncId = funcInfoID;
            }

            List<EntitySysUser> listSysUser = proxyUser.Service.SysUserQuery(otherUserQc);
            if (listSysUser.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清除条码输入框并设置焦点
        /// </summary>
        public void ClearAndFocusBarcode()
        {
            txtBarcode.Text = "";
            txtBarcode.Focus();
            this.ActiveControl = txtBarcode;
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void sysToolBar1_BtnResetClick(object sender, EventArgs e)
        {
            if (Step is ReachStep 
                && UserInfo.GetSysConfigValue("DoubleReceive") == "是"
                && patientControl.HasData()
                && !sysToolBar1.BtnConfirm.Enabled)
            {
                barConfirm(true, true, false);
                ClearCacheOperator();
                ClearOperatorMes();
            }
            patientControl.blReset = true;
            bacReset();

        }

        /// <summary>
        /// 重置调用方法
        /// </summary>
        private void bacReset()
        {
            if (patientControl.HasData())
                patientControl.Reset();
            ClearBaseInfo();
            ClearAndFocusBarcode();
            ClearOperatorMes();
            ClearCacheOperator();
        }

        /// <summary>
        /// 重置限制
        /// </summary>
        private void ClearCacheOperator()
        {
            Step.ShouldEnabledBarcodeInput = true;
            Step.ShouldEnlableSimpleSearchPanel = true;
            sysToolBar1.BtnConfirm.Enabled = true;
            sysToolBar1.BtnClear.Enabled = true;
            ctlTimeCountDown1.Reset();
        }


        /// <summary>
        /// 控件显示时设置光标停在条码框里
        /// </summary>
        private void BCConfirm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                ClearAndFocusBarcode();
        }

        /// <summary>
        /// 打印清单
        /// </summary>
        private void sysToolBar1_OnBtnPrintListClicked(object sender, EventArgs e)
        {
            patientControl.PrintList();
        }


        /// <summary>
        /// 回退标本
        /// </summary>
        private void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            //标本确认各个界面只能回退对应流程的条码
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Enable_ReturnBarcodeCheck") == "是")
            {
                FrmReturnBarcodeV2 frm = new FrmReturnBarcodeV2(stepType);
                frm.ShowDialog();
            }
            else
            {
                FrmReturnBarcodeV2 frm = new FrmReturnBarcodeV2();
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 设置不同的底色以区分不同的步骤
        /// </summary>
        /// <param name="color"></param>
        private void SetPanelColor(Color color)
        {
            lbBarCode.ForeColor = color;
        }

        /// <summary>
        /// 清除操作信息 
        /// </summary>
        public void ClearOperatorMes()
        {
            if (isNotClearOp) return;
            OperatorID = string.Empty;
            OperatorName = string.Empty;
            lblOp.Text = "";
        }

        private void ctlTimeCountDown1_TimeOut(object sender, EventArgs args)
        {
            if (this.ctlTimeCountDown1.Visible)
            {
                bacReset();
            }
        }

        /// <summary>
        /// 打印条码回执
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintReturnClicked(object sender, EventArgs e)
        {
            IPrintMachine machine = new ZeBraPrinter();

            string printMachineName = GetReturnPrintMachineName();

            List<String> printIDForMZReturn = this.patientControl.GetAllBarcodesByMZ();
            string reutrnTemplate = UserInfo.GetSysConfigValue("MZBarcodeReturnReport");
            machine.PrintInfo = new PrintInfo(printIDForMZReturn);
            bool result = machine.Print(printMachineName, reutrnTemplate, "bc_bar_code", string.Empty);

            if (result)
            {
                if (patientControl.HasData())
                    patientControl.Reset();
                ClearBaseInfo();
                ClearAndFocusBarcode();
            }
        }
        string xmlFile = PathManager.SettingLisPath + @"\printXml\barcodePrintConfig.xml";

        private string GetReturnPrintMachineName()
        {
            if (File.Exists(xmlFile))
            {
                DataSet dsPrint = new DataSet();
                dsPrint.ReadXml(xmlFile);
                if (dsPrint.Tables.Count > 0)
                {
                    DataTable dt = dsPrint.Tables[0];
                    if (dt != null && dt.Columns.Contains("ReturnPrintName")
                        && !string.IsNullOrEmpty(dt.Rows[0]["ReturnPrintName"].ToString()))
                    {
                        return dt.Rows[0]["ReturnPrintName"].ToString();
                    }
                    if (dt != null)
                    {
                        return dt.Rows[0]["printName"].ToString();
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// 打印条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnQualityTestClicked(object sender, EventArgs e)
        {
            patientControl.PrintBarCodeInfo();
        }

        /// <summary>
        /// 打印细菌清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPrintListGermClicked(object sender, EventArgs e)
        {
            patientControl.PrintListGerm();
        }

        /// <summary>
        /// 标本打包
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sysToolBar1_BtnAnswerClick(object sender, EventArgs e)
        {
            patientControl.PackBarcode();
        }

        /// <summary>
        /// 记录条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnDeSpeClick(object sender, EventArgs e)
        {
            FrmRecordBarcode frmRecord = new FrmRecordBarcode();
            frmRecord.ShowDialog();
        }

        public void TimeCountDown1ResetWhenZero()
        {
            try
            {
                if (ctlTimeCountDown1.LblTimeText == "0")
                    ctlTimeCountDown1_TimeOut(null, null);
            }
            catch
            { }
        }
    }
}
