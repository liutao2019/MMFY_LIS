using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.entity;
using dcl.client.common;
using dcl.client.wcf;
using dcl.client.frame;
using dcl.common;
using lis.client.control;
using System.Text.RegularExpressions;
using dcl.root.logon;
using dcl.client.cache;

namespace dcl.client.sample
{
    public partial class QueueNumberControl : UserControl
    {
        public QueueNumberControl()
        {
            InitializeComponent();
        }
        public IPrint Printer { get; set; }
        private string OperatorID = string.Empty;
        private string OperatorName = string.Empty;
        private string OperatorStfId = string.Empty;
        private bool preBarCodeCheckCuvType = false;
        private string StartDate = DateTime.Now.ToString("yyyy-MM-dd 0:00:00");
        private string EndDate = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
        public IStep Step
        {
            get;
            set;
        }
        private StepType stepType;
        public StepType StepType
        {
            get { return stepType; }
            set
            {
                patientControl.StepType = stepType = value;
                Step = StepFactory.CreateStep(value);

            }
        }
        private string window = string.Empty;

        /// <summary>
        /// 定时器
        /// </summary>
        Timer refreshTimer = new Timer();
        List<EntityQueueNumber> listNotCollect= new List<EntityQueueNumber>();
        List<EntityQueueNumber> listOverNum = new List<EntityQueueNumber>();
        //重叫语音
        private string heavyCallVoice = string.Empty;
        //重叫pid_in_no
        private string heavyCallNo = string.Empty;
        private void QueueNumberControl_Load(object sender, EventArgs e)
        {
            Color colPrinted = IStep.GetBarcodeConfigColor("Barcode_Color_Printed");
            Color colReceived = IStep.GetBarcodeConfigColor("Barcode_Color_Received");
            Color colBlooded = IStep.GetBarcodeConfigColor("Barcode_Color_Blooded");
            Color Barcode_Color_Sended = IStep.GetBarcodeConfigColor("Barcode_Color_Sended");
            Color Barcode_Color_Reach = IStep.GetBarcodeConfigColor("Barcode_Color_Reach");
            Color Barcode_Color_Reported = IStep.GetBarcodeConfigColor("Barcode_Color_Reported");
            preBarCodeCheckCuvType = ConfigHelper.GetSysConfigValueWithoutLogin("BarCode_PreBarCodeCheckCuvType") == "是";
            window = LocalSetting.Current.Setting.BloodWindow;
            //使用的条码类型：自动条码、预置条码
            string barcode_type = ConfigHelper.GetSysConfigValueWithoutLogin("7");
            if (barcode_type == "预置条码")
            {
                lblPrePlaceBarcode.Visible = txtPrePlaceBarcode.Visible  = true;
            }
            this.btnPrint.Appearance.BackColor = colPrinted;
            this.btnPrint.Appearance.BorderColor = colPrinted;
            this.btnReceived.Appearance.BackColor = colReceived;
            this.btnReceived.Appearance.BorderColor = colReceived;
            this.btnBlooded.Appearance.BackColor = colBlooded;
            this.btnBlooded.Appearance.BorderColor = colBlooded;
            this.btnCollected.Appearance.BackColor = Barcode_Color_Sended;
            this.btnReported.Appearance.BackColor = Barcode_Color_Reported;
            this.btnReported.Appearance.BorderColor = Barcode_Color_Reported;

            string[] strBtnName;
            string[] strBtntool;
            sysToolBar1.BtnBCPrint.Caption = "打印采集确认";
            sysToolBar1.BtnPageDown.Caption = "下一个";
            sysToolBar1.BtnUndo.Caption = "重置条码";
            sysToolBar1.BtnConfirm.Caption = "叫号";
            sysToolBar1.BtnAdd.Caption = "生成条码";
            strBtnName = new string[] {
                sysToolBar1.BtnBCPrint.Name,
                sysToolBar1.BtnPageDown.Name,
                sysToolBar1.BtnOverNumber.Name,
                sysToolBar1.BtnHeavyCall.Name,
                sysToolBar1.BtnRefresh.Name,
                sysToolBar1.BtnConfirm.Name,
                sysToolBar1.BtnBCPrintReturn.Name,
                sysToolBar1.BtnUndo.Name,
                sysToolBar1.BtnAdd.Name,
                sysToolBar1.BtnClose.Name
                };

            strBtntool = new string[]{"", "","","","","","F3","","",""
                };
            sysToolBar1.SetToolButtonStyle(strBtnName, strBtntool);
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Enable_CreateBarCodeButton") == "是")
            {
                sysToolBar1.BtnAdd.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            Init();
            stepType = StepType.Sampling;
            Step = StepFactory.CreateStep(stepType);
            refreshTimer.Interval = 300000;
            refreshTimer.Tick += new EventHandler(refreshTimer_Tick);
            refreshTimer.Start();
            sysToolBar1_OnBtnRefreshClicked(null,null);
            LoadCustomerReadType();
        }
        /// <summary>
        /// 初始化医嘱时间
        /// </summary>
        private void Init()
        {
            adviceTime1.Value= new DateTimeRange(DateTime.Now, DateTime.Now);
        }
        /// <summary>
        /// 加载自定义读取类型,比如门诊下载有：门诊ID、姓名等等
        /// </summary>
        private void LoadCustomerReadType()
        {

            List<EntitySysItfInterface> listInterface = CacheClient.GetCache<EntitySysItfInterface>();
            List<EntitySysItfContrast> listContrast = CacheClient.GetCache<EntitySysItfContrast>();

            if (listInterface == null || listInterface.Count == 0 ||
                listContrast == null || listContrast.Count == 0)
            {
                return;
            }

            int interfaceIndex = listInterface.FindIndex(w => w.ItfaceInterfaceType == (Printer.Name + "条码"));

            if (interfaceIndex < 0)
                return;

            listContrast = listContrast.FindAll(w => w.ContItfaceId == listInterface[interfaceIndex].ItfaceId && w.ContSearchSeq > 0).OrderBy(o => o.ContSearchSeq).ToList();

            if (listContrast.Count > 0)
            {
                cbTypeID.Text = "";
                cbTypeID.Properties.Items.Clear();

                foreach (EntitySysItfContrast row in listContrast)
                {
                    cbTypeID.Properties.Items.Add(row.ContRemark);
                }

                if (cbTypeID.Properties.Items.Count > 0)
                    cbTypeID.SelectedIndex = 0;

            }
        }
        /// <summary>
        /// 打印采集确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintClicked(object sender, EventArgs e)
        {
            //打印条码
            patientControl.PrintBarcode(this.Printer, false);
         
            bool result = true;
            //判断该病人的条码是否全部成功打印
            if (patientControl.ListSampMain != null && patientControl.ListSampMain.Count > 0)
            {
                foreach (EntitySampMain samp in patientControl.ListSampMain)
                {
                    if (samp.SampPrintFlag == 0)
                        result = false;
                }
            }
            else {
                result = false;
            }
            if (result)
            {
                //执行院网接口  以及更新条码采集状态
                barConfirm();
                //更新排队状态为已采集
                UpdateQueueStatus("3");
                sysToolBar1_OnBtnRefreshClicked(null, null);
                patientControl.Selection.ClearSelection();
            }
        }
        /// <summary>
        /// 更新病人的排队状态
        /// </summary>
        /// <param name="status"></param>
        private void UpdateQueueStatus(string status)
        {
            EntityQueueNumber queue = null;
            if (gvNotCollected.SelectedRowsCount > 0)
            {
                queue = gvNotCollected.GetFocusedRow() as EntityQueueNumber;
            }
            else if (gvOverNumber.SelectedRowsCount > 0)
            {
                queue = gvOverNumber.GetFocusedRow() as EntityQueueNumber;
            }
            if (queue != null && !string.IsNullOrEmpty(queue.QueueNo))
            {
                new ProxyQueueNumber().Service.UpdateQueueStatus(queue.PidInNo,queue.QueueNo, status);
            }
        }
        /// <summary>
        /// 条码确认
        /// </summary>
        /// <param name="isSelect"></param>
        /// <param name="isDoubleReceive"></param>
        /// <param name="isNeedChildConfirm"></param>
        private void barConfirm()
        {
            List<EntitySampMain> listSamp = patientControl.MainTable;
            if (listSamp != null && listSamp.Count > 0)
            {
                OperatorID = UserInfo.loginID;
                OperatorName = UserInfo.userName;
                //    powerConfirm();
                if (OperatorID != string.Empty && OperatorName != string.Empty)
                {
                    foreach (EntitySampMain samp in listSamp)
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
                        string strResultMsg = proxy.Service.ConfirmBeforeCheck(sign, samp);

                        if (strResultMsg.Trim() != string.Empty)
                        {
                            MessageDialog.Show(strResultMsg + Environment.NewLine + "请回退该条码。");

                            if (patientControl.HasData())
                                patientControl.Reset();
                            //ClearBaseInfo();
                            //ClearAndFocusBarcode();
                            return;
                        }
                        #endregion
                    }

                    string strMess = string.Empty;
                    if (patientControl.ComfirmAll(OperatorID, OperatorName, false, OperatorStfId))
                    {
                        // ClearAndFocusBarcode();
                        strMess = "操作成功!";
                        foreach (EntitySampMain drBac in patientControl.MainTable)
                        {
                            if (drBac.SampStatusId != Step.StepCode)
                                drBac.SampLastactionDate = ServerDateTime.GetServerDateTime();
                            drBac.SampStatusId = Step.StepCode;
                        }
                    }
                    else
                        strMess = "操作失败!";
                }
            }
        }

        /// <summary>
        /// 权限确定
        /// </summary>
        private void powerConfirm()
        {
            int count = patientControl.GetAllUsedBarcodesCount();
            if (count <= 0)
            {

                lis.client.control.MessageDialog.Show("无可确认条码,请确保条码流程正确!");

                return;
            }
            if (OperatorID == string.Empty || OperatorName == string.Empty)//扫第一条条码时需预确认 by li:2010-6-30
            {
                FrmHISCheckPassword frm = new FrmHISCheckPassword(Step.Audit);
                frm.Text = Step.StepName + " 待确认条码数:" + count.ToString();
                if ((frm.ShowDialog() != DialogResult.OK))
                {
                    //bacReset();
                    return;
                }

                //录入时再次检查权限
                string funcInfoID = string.Empty;
                if (frm.OperatorID != "admin" &&
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

                listUserRole = listUserRole.FindAll(w => w.UserLoginId == frm.OperatorID && w.RoleRemark.IndexOf("护工组") >= 0);
                OperatorID = frm.OperatorID;
                OperatorName = frm.OperatorName;
                OperatorStfId = frm.OperatorSftId;


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
                                    //  bacReset();
                                    return;
                                }
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
        /// 打印回执
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnBCPrintReturnClicked(object sender, EventArgs e)
        {
            patientControl.PrintBarcodeReturn(this.Printer);

            //   ClearAndFocusBarcode(true);
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            try
            {
                string dateStart = adviceTime1.Value.Start.ToString("yyyy-MM-dd 0:00:00");
                string dateEnd = adviceTime1.Value.End.ToString("yyyy-MM-dd 23:59:59");
                string windowName = string.Empty;
                string windowArea = string.Empty;
                List<EntityQueueNumber> listQueue = new ProxyQueueNumber().Service.GetQueueNumber(dateStart, dateEnd, windowName, windowArea);
                if (listQueue.Count > 0)
                {
                    this.listCollected.DataSource = listQueue.FindAll(w => w.QueueStatus == "3");
                    this.listOverNumber.DataSource =listOverNum= listQueue.FindAll(w => w.QueueStatus == "2");
                    this.listNotCollected.DataSource = listNotCollect = listQueue.FindAll(w => w.QueueStatus == "0" || w.QueueStatus=="1");
                }
                if (listNotCollected.Count > 0)
                {
                    gvNotCollected.FocusedRowHandle = 0;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }
        /// <summary>
        /// 下一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnPageDownClicked(object sender, EventArgs e)
        {
            gvNotCollected.MoveFirst();
            for (int i = 0; i < this.listNotCollected.Count; i++)
            {
                EntityQueueNumber queue = gvNotCollected.GetFocusedRow() as EntityQueueNumber;
                if (queue.QueueStatus == "1" || !string.IsNullOrEmpty(queue.QueueWindowsName))
                    gvNotCollected.MoveNext();
                else break;
            }
            sysToolBar1_OnBtnConfirmClicked(null, null); 
        }
        /// <summary>
        /// 过号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnOverNumberClicked(object sender, EventArgs e)
        {
            UpdateQueueStatus("2");
            sysToolBar1_OnBtnRefreshClicked(null, null);
        }
        /// <summary>
        /// 重叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnHeavyCallClicked(object sender, EventArgs e)
        {
     
            EntityQueueNumber queue = new ProxyQueueNumber().Service.GetQueueNumberByNo(heavyCallNo, "", StartDate, EndDate);
            //判断重叫病人是否已采集
            if (queue != null && queue.QueueStatus != "3")
            {
                if (!string.IsNullOrEmpty(heavyCallVoice))
                {
                    SaveMessageSpeech(heavyCallVoice);
                }
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("重叫病人已采集！");
            }
        }
        /// <summary>
        /// 叫号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            EntityQueueNumber queue = null;
            //是否刷新  
            bool refresh = false;
            string pidInNo = string.Empty;
            if (gvNotCollected.SelectedRowsCount > 0)
            {
                queue = gvNotCollected.GetFocusedRow() as EntityQueueNumber;
            }
            else if (gvOverNumber.SelectedRowsCount > 0)
            {
                queue = gvOverNumber.GetFocusedRow() as EntityQueueNumber;
                pidInNo = queue.PidInNo;
                refresh = true;
            }
            if (queue != null && !string.IsNullOrEmpty(queue.QueueNo))
            {
                //判断当前叫号病人是否已被叫号
                EntityQueueNumber quetemp = new ProxyQueueNumber().Service.GetQueueNumberByNo(queue.PidInNo, "", StartDate, EndDate);
                if (!string.IsNullOrEmpty(quetemp.QueueWindowsName) && quetemp.QueueWindowsName != window && quetemp.QueueStatus=="1")
                {
                    if (
                   MessageDialog.Show(string.Format("{0}已在{1}号窗口采血，是否继续", quetemp.PidInNo, quetemp.QueueWindowsName),
                                      MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                string voice_txt = string.Format("请{0}号,{1},到{2}窗口", queue.QueueNo,queue.PidName, window);
                //更新采血窗口号
                new ProxyQueueNumber().Service.UpdateQueueWindow(queue.PidInNo, queue.QueueNo, window);
                //更新排队状态为正在叫号
                new ProxyQueueNumber().Service.UpdateQueueStatus(queue.PidInNo, queue.QueueNo, "1");
                //记录重新叫号信息
                heavyCallNo = queue.PidInNo;
                heavyCallVoice = voice_txt;
                SaveMessageSpeech(voice_txt);
            }
            //已过号病人再次叫号需要刷新列表 并将光标至于当前叫号病人
            if (refresh)
            {
                sysToolBar1_OnBtnRefreshClicked(null, null);
                for (int i = 0; i < listNotCollected.Count; i++)
                {
                    EntityQueueNumber dr = gvNotCollected.GetRow(i) as EntityQueueNumber;
                    if (dr.PidInNo == pidInNo)
                    { //在列表中找到了
                        gvNotCollected.ClearSelection();
                        this.ActiveControl = gcNotCollected;
                        gvNotCollected.FocusedRowHandle = i;
                        gvNotCollected.InvertRowSelection(i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 保存叫号信息
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
       private  bool SaveMessageSpeech(string voice)
        {
            EntitySysMessageSpeech speech = new EntitySysMessageSpeech();
            speech.SpeechText = voice;
            speech.Status = 0;
            speech.CreateDate = ServerDateTime.GetServerDateTime();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
            bool result= new ProxyQueueNumber().Service.SaveMessageSpeech(speech);
            return result;
        }
        /// <summary>
        /// 重置条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnUndoClick(object sender, EventArgs e)
        {
            EntitySampMain sampMain = patientControl.CurrentSampMain;
            if (sampMain == null)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无重置条码！");
            }
            else
            {
                string bc_bar_no = sampMain.SampBarCode;

                if (string.IsNullOrEmpty(bc_bar_no))
                {
                    MessageDialog.ShowAutoCloseDialog("条码无需重置！");
                    return;
                }
                if (sampMain.SampBarType == 0)
                {
                    MessageDialog.ShowAutoCloseDialog("非预制条码无法重置！");
                    return;
                }

                EntitySampProcessDetail proDetail = new ProxySampProcessDetail().Service.GetLastSampProcessDetail(bc_bar_no);

                string strStatus = proDetail.ProcStatus;

                //只有：打印条码、回退标本、生成条码、重置条码的状态后才能再次重置条码
                if (
                    strStatus == EnumBarcodeOperationCode.BarcodePrint.ToString()
                    || strStatus == EnumBarcodeOperationCode.SampleReturn.ToString()
                    || strStatus == EnumBarcodeOperationCode.BarcodeGenerate.ToString()
                    || strStatus == EnumBarcodeOperationCode.ResetPrePlaceBarcode.ToString()
                    )
                {
                    string strBarCode = sampMain.SampBarCode;

                    if (strBarCode.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("此条码未预置条码，无需重置。");
                        return;
                    }

                    if (lis.client.control.MessageDialog.Show("是否要重置 " + strBarCode + " 的预置条码？", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EntitySampOperation operation = new EntitySampOperation(UserInfo.loginID, UserInfo.userName);
                        operation.OperationStatus = "570";
                        operation.OperationStatusName = "重置预置条码";
                        operation.OperationPlace = LocalSetting.Current.Setting.CType_name;
                        operation.Remark = string.Format("IP地址:{0}", IPUtility.GetIP());
                        operation.OperationTime = IStep.GetServerTime();
                        operation.OperationWorkId = UserInfo.loginID;

                        string newBarCode = new ProxySampMain().Service.UndoSampMain(operation, sampMain);
                        if (!string.IsNullOrEmpty(newBarCode))
                        {
                            patientControl.BaseSampMain.SampBarCode = string.Empty;
                            patientControl.BaseSampMain.SampBarId = newBarCode;
                            patientControl.BaseSampMain.SampPrintFlag = 0;
                            patientControl.BaseSampMain.SampStatusId = "0";
                            patientControl.MainGrid.RefreshDataSource();
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("重置成功！");
                            patientControl.isResetBarcode = true;
                        }
                        else
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("重置错误！");
                    }
                }
                else
                    lis.client.control.MessageDialog.Show("此试管已采集,不能重置,如需重置请回退标本！");
            }
        }
        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            EntitySampMain dr = patientControl.CurrentSampMain;
            if (dr == null)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无数据！");
            }
            else
            {
                int bc_bar_type = dr.SampBarType;
                string strBarId = dr.SampBarId;

                if (bc_bar_type == 1 && string.IsNullOrEmpty(dr.SampBarCode)) //判断是否预制条码
                {
                    if (lis.client.control.MessageDialog.Show("是否要对病人:" + dr.PidName + "、组合为\"" + dr.SampComName + "\"生成条码？", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        String newBarCode = new ProxySampMain().Service.CancelUndoSampMain(strBarId);
                        if (!string.IsNullOrEmpty(newBarCode))
                        {
                            patientControl.BaseSampMain.SampBarCode = newBarCode;
                            patientControl.BaseSampMain.SampBarId = strBarId;
                            patientControl.BaseSampMain.SampPrintFlag = 0;
                            patientControl.BaseSampMain.SampStatusId = "0";
                            patientControl.BaseSampMain.SampBarType = 0; //条码类型(0-打印条码 1-预制条码)
                            patientControl.MainGrid.RefreshDataSource();
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("生成成功！");
                            patientControl.isResetBarcode = true;
                        }
                        else
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("生成错误！");
                    }
                }
                else
                    lis.client.control.MessageDialog.Show("此条码是非预制条码，不能生成条码！");
            }

        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                this.ParentForm.Close();
                refreshTimer.Stop();
            }
        }

        private void gvNotCollected_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gvCollected.ClearSelection();
            gvOverNumber.ClearSelection();
            EntityQueueNumber queue = listNotCollected.Current as EntityQueueNumber;
            LoadData(queue,"0");
            patientControl.SelectAllNoPrintRow();
            ShowBloodNotice();
        }

        private void gvOverNumber_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityQueueNumber queue = listOverNumber.Current as EntityQueueNumber;
            LoadData(queue,"0");
            patientControl.SelectAllNoPrintRow();
            ShowBloodNotice();
        }

        private void gvCollected_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EntityQueueNumber queue = listCollected.Current as EntityQueueNumber;
            LoadData(queue);
            ShowBloodNotice();
        }
        private void LoadData(EntityQueueNumber queue,string status="")
        {
            if (queue != null && !string.IsNullOrEmpty(queue.PidInNo))
            {
                List<EntitySampMain> listSamp = SearchSamp(queue.PidInNo,true);
                //过滤抽血项目且没删除的条码
                listSamp = listSamp.FindAll(w => w.SampSamName.Contains("血") ||w.SamName.Contains("血") && w.DelFlag == "0");
                if (!string.IsNullOrEmpty(status))
                {
                    listSamp = listSamp.FindAll(w => w.SampStatusId == "0" || w.SampStatusId == "1");
                }
                listSamp = listSamp.OrderByDescending(w => w.SampBarType).ToList();
                patientControl.BindingPatientsSource(listSamp);
                patientControl.RefreshCurrentBarcode();
            }
        }
        private void gvNotCollected_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gvNotCollected.FocusedRowHandle)
            {
               //.Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;
            }
        }

        private void gvOverNumber_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gvOverNumber.FocusedRowHandle)
            {
                //Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;
            }
        }

        private void gvCollected_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gvCollected.FocusedRowHandle)
            {
               //.Appearance.BackColor = System.Drawing.Color.LightBlue;
                e.Appearance.Options.UseBackColor = true;
            }
        }

        private void txtPrePlaceBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && txtPrePlaceBarcode.Text.Trim() != string.Empty)
            {
                bool result = patientControl.PrintPrePlaceBarcode(txtPrePlaceBarcode.Text.Trim(), this.Printer);
                if (result)
                {
                    int focusedRowHandle = patientControl.MainGridView.FocusedRowHandle;
                    bool allPrintResult = true;
                    //判断该病人的条码是否全部成功打印
                    if (patientControl.ListSampMain != null && patientControl.ListSampMain.Count > 0)
                    {
                        foreach (EntitySampMain samp in patientControl.ListSampMain)
                        {
                            if (samp.SampPrintFlag == 0)
                                allPrintResult = false;
                        }
                    }
                    else
                    {
                        allPrintResult = false;
                    }
                    if (allPrintResult)
                    {
                        //执行院网接口  以及更新条码采集状态
                        barConfirm();
                        //更新排队状态为已采集
                        UpdateQueueStatus("3");
                        sysToolBar1_OnBtnRefreshClicked(null, null);
                        patientControl.Selection.ClearSelection();
                    }
                    patientControl.MoveNext(focusedRowHandle);
                }
                txtPrePlaceBarcode.Text = "";
            }
            patientControl.SelectAllNoPrintRow();
        }
        /// <summary>
        /// 查询病人条码
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="onlyPidInNo">是否只查pidInNo</param>
        /// <returns></returns>
        private List<EntitySampMain> SearchSamp(string searchValue,bool onlyPidInNo)
        {
            EntitySampQC sampQC = new EntitySampQC();
            if (onlyPidInNo)
            {
                sampQC.PidInNo = searchValue;
            }
            else {
                if (cbTypeID.SelectedIndex == 1)
                    sampQC.PidInNo = searchValue;
                else if (cbTypeID.SelectedIndex == 0)
                    sampQC.PidSocialNo = searchValue;
                else if (cbTypeID.SelectedIndex == 2)
                    sampQC.PidName = searchValue;
            }
            sampQC.PidSrcId = "107";
            sampQC.StartDate = adviceTime1.Value.Start.ToString("yyyy-MM-dd 00:00:00");
            sampQC.EndDate = adviceTime1.Value.End.ToString("yyyy-MM-dd 23:59:59");
            sampQC.SearchValue = txtOutPatients.Text.Trim();
            List<EntitySampMain> listSamp = new ProxySampMain().Service.SampMainQuery(sampQC);
            return listSamp;
        }
        /// <summary>
        /// 门诊条码下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOutPatients_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(txtOutPatients.Text)) 
            {
                List<EntitySampMain> listSamp = SearchSamp(txtOutPatients.Text.Trim(),false);
                listSamp = listSamp.FindAll(w => w.SampSamName.Contains("血") && w.DelFlag == "0");
                listSamp = listSamp.FindAll(w => w.SampStatusId == "0");
                if (listSamp != null && listSamp.Count > 0)
                {
                    bool findResult = false;
                    for (int i = 0; i < listNotCollected.Count; i++)
                    {
                        EntityQueueNumber dr = gvNotCollected.GetRow(i) as EntityQueueNumber;
                        if (dr.PidInNo == txtOutPatients.Text.Trim())
                        { //在列表中找到了
                            gvNotCollected.ClearSelection();
                            gvNotCollected.FocusedRowHandle = i;
                            gvNotCollected.InvertRowSelection(i);
                            findResult = true;
                            break;
                        }
                    }
                    //在未采集列表中没找到则继续在过号列表中找
                    if (!findResult)
                    {
                        for (int i = 0; i < listOverNumber.Count; i++)
                        {
                            EntityQueueNumber dr = gvOverNumber.GetRow(i) as EntityQueueNumber;
                            if (dr.PidInNo == txtOutPatients.Text.Trim())
                            { //在列表中找到了
                                gvOverNumber.FocusedRowHandle = i;
                                gvOverNumber.InvertRowSelection(i);
                                findResult = true;
                                break;
                            }
                        }
                    }
                    listSamp = listSamp.OrderByDescending(w => w.SampBarType).ToList();
                    patientControl.BindingPatientsSource(listSamp);
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("当前时间无医嘱!");
                }
                txtOutPatients.Text = "";
              //  txtOutPatients.Focus();
            }
        }

        #region 不需要从接口下载数据  先注释
        //private List<EntitySampMain> DownLoadData()
        //{
        //    DownLoadFromInterface();
        //    //检索数据
        //    EntitySampQC sampQC = new EntitySampQC();
        //    sampQC.PidSrcId = "107";
        //    sampQC.StartDate = adviceTime1.Value.Start.ToString("yyyy-MM-dd 00:00:00");
        //    sampQC.EndDate = adviceTime1.Value.End.ToString("yyyy-MM-dd 23:59:59");
        //    sampQC.SearchType = "病人ID";
        //    sampQC.SearchValue = txtOutPatients.Text.Trim();
        //    List<EntitySampMain> listSamp = new ProxySampMain().Service.SampMainQuery(sampQC);
        //    return listSamp;
        //}
        ///// <summary>
        ///// 从接口下载数据
        ///// </summary>
        //private void DownLoadFromInterface()
        //{
        //    EntityInterfaceExtParameter downLoadInfo = new EntityInterfaceExtParameter();
        //    downLoadInfo.DownloadType = InterfaceType.MZDownload;
        //    downLoadInfo.PatientID = txtOutPatients.Text.Trim();
        //    downLoadInfo.StartTime =Convert.ToDateTime( adviceTime1.Value.Start.ToString("yyyy-MM-dd 00:00:00"));
        //    downLoadInfo.EndTime = Convert.ToDateTime(adviceTime1.Value.End.ToString("yyyy-MM-dd 23:59:59"));
        //    try
        //    {
        //        ProxySampMainDownload download = new ProxySampMainDownload();
        //        download.Service.DownloadBarcode(downLoadInfo);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteException(this.GetType().Name, "下载条码", ex.ToString() + ex.StackTrace);
        //    }
        //}
        #endregion

        private void gvNotCollected_MouseDown(object sender, MouseEventArgs e)
        {
            gvCollected.ClearSelection();
            gvOverNumber.ClearSelection();
            if(gvNotCollected.FocusedRowHandle==0)
            {
                EntityQueueNumber queue = listNotCollected.Current as EntityQueueNumber;
                LoadData(queue, "0");
                patientControl.SelectAllNoPrintRow();
                ShowBloodNotice();
            }
        }

        private void gvOverNumber_MouseDown(object sender, MouseEventArgs e)
        {
            gvCollected.ClearSelection();
            gvNotCollected.ClearSelection();
            if (gvOverNumber.FocusedRowHandle == 0)
            {
                EntityQueueNumber queue = listOverNumber.Current as EntityQueueNumber;
                LoadData(queue, "0");
                patientControl.SelectAllNoPrintRow();
                ShowBloodNotice();
            }
        }
        private void gvCollected_MouseDown(object sender, MouseEventArgs e)
        {
            gvOverNumber.ClearSelection();
            gvNotCollected.ClearSelection();
            if (gvCollected.FocusedRowHandle == 0)
            {
                EntityQueueNumber queue = listCollected.Current as EntityQueueNumber;
                LoadData(queue);
                ShowBloodNotice();
            }
        }
        /// <summary>
        /// 显示采血注意事项
        /// </summary>
        private void ShowBloodNotice()
        {
            lblBloodNotice.Text = "";
            if (patientControl.ListSampMain != null && patientControl.ListSampMain.Count>0)
            {
                List<string> listBarcode = new List<string>();
                foreach (EntitySampMain samp in patientControl.ListSampMain)
                {
                    listBarcode.Add(samp.SampBarId);
                }
                List<EntitySampDetail>listSampDetail = new ProxySampDetail().Service.GetSampDetailByListBarId(listBarcode);
                if (listSampDetail.Count > 0)
                {
                    foreach (EntitySampDetail detail in listSampDetail)
                    {
                        if (!lblBloodNotice.Text.ToString().Contains(detail.BloodNotice))
                        {
                            lblBloodNotice.Text += "+"+detail.BloodNotice;
                        }
                    }
                    if(!string.IsNullOrEmpty(lblBloodNotice.Text))
                        lblBloodNotice.Text=lblBloodNotice.Text.Remove(0, 1);
                }
            }
        }
        /// <summary>
        /// 定时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            //当未采集和已过号列表都为空时 刷新
            if (listOverNum.Count == 0 && listNotCollect.Count == 0)
            {
                sysToolBar1_OnBtnRefreshClicked(null, null);
            }
        }

    }
}
