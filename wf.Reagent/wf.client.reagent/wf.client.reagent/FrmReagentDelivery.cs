using DevExpress.XtraGrid;
using Lis.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.Threading;
using dcl.client.frame;
using dcl.client.result.Interface;
using dcl.entity;
using dcl.client.frame.runsetting;
using dcl.client.cache;
using lis.client.control;
using dcl.client.report;
using dcl.root.logon;
using dcl.client.common;
using dcl.client.wcf;
using dcl.common;

namespace wf.client.reagent
{
    public partial class FrmReagentDelivery : FrmCommon, IPatPanelConfig
    {
        List<EntityDCLPrintParameter> listPrintData_multithread;
        bool useMultiThread = false;
        /// <summary>
        /// 当前出库信息
        /// </summary>
        EntityReaDelivery CurrentDeliveryInfo = new EntityReaDelivery();//病人消息
        /// <summary>
        /// 面板配置类
        /// </summary>
        PatInputRuntimeSetting UserCustomSetting = null;
        /// <summary>
        /// 新增时获得焦点的控件
        /// </summary>
        Control FocusOnAddNewControl = null;
        List<EntityReaDelivery> AllReaList = new List<EntityReaDelivery>();
        List<EntityReaApply> AllApplyList = new List<EntityReaApply>();
        private String fpat_id = "";//主键
        private String apply_id = "";//主键
        List<EntityReaSetting> setList;
        private string CurrentReaID;
        bool isLoadData = false;
        bool isDataChaged = false;
        /// <summary>
        /// 试剂出库表
        /// </summary>
        List<EntityReaDeliveryDetail> dtReaDeliveryDetail = new List<EntityReaDeliveryDetail>();

        private bool ReagentOutFromApply = false;
        public FrmReagentDelivery()
        {
            InitializeComponent();
            this.roundPanelGroup1.RoundDeliveryGroupClick += this.roundPanelGroup1_Click;
            this.sysToolBar1.OnCloseClicked += SysToolBar1_OnCloseClicked;
            this.sysToolBar1.OnBtnSaveClicked += SysToolBar1_OnBtnSaveClicked;
            this.sysToolBar1.OnBtnAddClicked += SysToolBar1_OnBtnAddClicked;
            this.sysToolBar1.OnBtnRefreshClicked += SysToolBar1_OnBtnRefreshClicked;
            this.sysToolBar1.OnBtnDeleteClicked += SysToolBar1_OnBtnDeleteClicked;
            this.sysToolBar1.OnAuditClicked += SysToolBar1_OnAuditClicked;
            //this.sysToolBar1.BtnUndoClick += SysToolBar1_BtnUndoClick;
            this.sysToolBar1.BtnAnswerClick += SysToolBar1_BtnAnswerClick;
            this.sysToolBar1.OnBtnPrintClicked += SysToolBar1_OnBtnPrintClicked;
            this.sysToolBar1.OnPrintPreviewClicked += SysToolBar1_OnPrintPreviewClicked;
            this.selectDicReaSetting1.ValueChanged += SelectDicReaSetting1_ValueChanged;
            setList = CacheClient.GetCache<EntityReaSetting>();
            ReagentOutFromApply =
                ConfigHelper.GetSysConfigValueWithoutLogin("ReagentOutFromApply") == "是";
            if (!ReagentOutFromApply)
            {
                gcReaApply.Visible = false;
                gcReaApply.Height = 0;
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="isPreview"></param>
        private void Print(bool isPreview)
        {
            OnPatPrint(null, isPreview);
        }
        /// <summary>
        /// 打印/预览
        /// <param name="pat_id">病人ID，如果传null值则打印当前勾选中的病人</param>
        /// <param name="isPreview">当前打印是否为打印预览</param>
        /// </summary>
        private void OnPatPrint(string pat_id, bool isPreview)
        {
            string prtTemplate = "ReagentDelivery";

            this.BeginLoading();
            List<string> listPatID = new List<string>();
            StringBuilder sbPatSidWhere2 = new StringBuilder();
            if (pat_id == null)
            {
                List<EntityReaDelivery> listPatient = GetCheckedReaDelivery();
                if (listPatient.Count > 0)
                {
                    foreach (EntityReaDelivery dr in listPatient)
                    {
                        if (isPreview)
                        {
                            listPatID.Add(dr.Rdl_no.ToString());
                        }
                        else
                        {
                            if (dr.Rdl_status.ToString() == LIS_Const.REAGENT_FLAG.Audited)
                            {
                                if (string.IsNullOrEmpty(dr.AuditorName))
                                {
                                    sbPatSidWhere2.Append(string.Format(",[{0}]", dr.Rdl_no));
                                    continue;
                                }
                                listPatID.Add(dr.Rdl_no.ToString());

                            }
                        }
                    }

                    if (sbPatSidWhere2.Length > 0)
                    {
                        MessageDialog.Show("单号为:" + sbPatSidWhere2.Remove(0, 1) + " 的出库单无审核者信息，需重新审核", "提示");
                        this.CloseLoading();
                        return;
                    }
                    if (listPatID.Count == 0)
                    {
                        MessageDialog.Show("没有符合打印/预览要求的记录，请检查选中的记录是否已审核", "提示");
                        this.CloseLoading();
                        return;
                    }
                }
                else
                {
                    MessageDialog.Show("在请左侧勾选需要预览/打印的记录", "提示");
                    this.CloseLoading();
                    return;
                }
            }
            else//存打
            {
                listPatID.Add(pat_id);
            }

            #region 新打印方式，传多个打印指令打印多个病人，每条指令一个病人

            List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
            int sequence = 0;

            //ShowDialogForm tip = new ShowDialogForm("提示", "正在打开......", "请耐心等候！", listPatID.Count);
            foreach (string patient_id in listPatID)
            {
                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                //printPara.RepId = patient_id;
                printPara.CustomParameter.Add("DeliveryId", patient_id);
                printPara.ReportCode = prtTemplate;
                printPara.Sequence = sequence;
                listPara.Add(printPara);
                sequence++;

                //tip.SetProgress();
            }
            listPrintData_multithread = EntityManager<EntityDCLPrintParameter>.ListClone(listPara);

            if (isPreview)//是否为打印预览
            {
                if (useMultiThread)
                {
                    Thread thread = new Thread(new ThreadStart(StartPreviewReports));
                    thread.IsBackground = true;
                    thread.Start();
                }
                else
                {
                    try
                    {
                        StartPreviewReports();
                    }
                    catch (ReportNotFoundException ex1)
                    {
                        MessageDialog.Show(ex1.MSG);
                    }
                    catch (Exception ex2)
                    {
                        Logger.WriteException(this.GetType().ToString(), "print", ex2.Message);
                    }
                }
            }
            else
            {
                try
                {
                    //不是打印预览才需要更新打印标志
                    pForm_PrintStart2(listPara);
                    DCLReportPrint.BatchPrint(listPara, ReportSetting.PrintName);
                    this.RefreshPatients();
                    this.SelectAllDeliveryInGrid(false);
                }
                catch (ReportNotFoundException ex1)
                {
                    MessageDialog.Show(ex1.MSG);
                }
                catch (Exception ex2)
                {
                    Logger.WriteException(this.GetType().ToString(), "print", ex2.Message);
                }
            }

            //tip.Close();

            #endregion
            this.CloseLoading();
        }
        public void SelectAllDeliveryInGrid(bool selectAll)
        {
            //在全部勾选前把焦点行设置成-1解决：全选的时候焦点行会显示不了勾
            if (selectAll)
            {
                gvReaDelivery.SelectAll();
            }
            else
                gvReaDelivery.ClearSelection();
            this.gcReaDelivery.RefreshDataSource();
        }
        public void RefreshPatients()
        {
            GetReaList(false);
            this.addNew();
        }
        void pForm_PrintStart2(List<EntityDCLPrintParameter> listPara)
        {
            List<string> listPatID = new List<string>();

            foreach (EntityDCLPrintParameter item in listPara)
            {
                listPatID.Add(item.RepId);
            }

            if (listPatID != null && listPatID.Count > 0)
            {
                UpdatePrintState(listPatID);
            }
        }
        /// <summary>
        /// 更新打印状态
        /// </summary>
        /// <param name="listPatID"></param>
        public void UpdatePrintState(IEnumerable<string> listPatID)
        {
            if (listPatID != null)
            {
                string strPlace = null;
                try
                {
                    strPlace = LocalSetting.Current.Setting.Description;//描述信息
                }
                catch (Exception)
                {
                }
                if (!ConfigHelper.IsNotOutlink())
                {
                    strPlace = IPUtility.GetIP();
                }
                new ProxyReaDelivery().Service.UpdatePrintState_whitOperator(listPatID, UserInfo.loginID, UserInfo.userName, strPlace);
            }
        }

        //**************************
        //另一线程的方法
        void StartPreviewReports()
        {
            try
            {
                DCLReportPrint.BatchPrintPreview(listPrintData_multithread);
            }
            catch (ReportNotFoundException ex1)
            {
                MessageDialog.Show(ex1.MSG);
            }
            catch (Exception ex2)
            {

            }
        }
        private void SysToolBar1_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            if (GetCheckedReaDelivery().Count <= 0)
            {
                return;
            }
            Print(true);
        }

        private void SysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (GetCheckedReaDelivery().Count <= 0)
            {
                return;
            }
            Print(false);
            RefreshPatients();
        }

        private void SysToolBar1_BtnAnswerClick(object sender, EventArgs e)
        {
            if (CurrentDeliveryInfo == null || string.IsNullOrEmpty(CurrentDeliveryInfo.Rdl_no))
            {
                MessageDialog.Show("请勾选你要撤销数据！", "提示");
                return;
            }
            if (CurrentDeliveryInfo.Rdl_status != "4")
            {
                MessageDialog.Show("该记录未审核！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            FrmReaCheckPassword frmCheck = new FrmReaCheckPassword("身份验证 - 撤销", LIS_Const.ReagentPopedomCode.DeliveryReturn, "", "");
            DialogResult dig = frmCheck.ShowDialog();

            if (dig == DialogResult.OK)
            {
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;
                Caller.UserID = frmCheck.OperatorID;
                CurrentDeliveryInfo.Rdl_returnreason = frmCheck.ReturnReason;
                new ProxyReaDelivery().Service.ReturnReaData(Caller, CurrentDeliveryInfo);

            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            RefreshPatients();
        }

        private void SysToolBar1_BtnUndoClick(object sender, EventArgs e)
        {
            if (CurrentDeliveryInfo == null || string.IsNullOrEmpty(CurrentDeliveryInfo.Rdl_no))
            {
                MessageDialog.Show("请勾选你要取消审核数据！", "提示");
                return;
            }
            if (CurrentDeliveryInfo.Rdl_status != "4")
            {
                MessageDialog.Show("该记录未审核！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 取消审核", LIS_Const.ReagentPopedomCode.DeliveryAudit, "", "");
            frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                             //验证窗口
            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;
                Caller.UserID = frmCheck.OperatorID;

                CurrentDeliveryInfo.Rdl_auditor = string.Empty;
                CurrentDeliveryInfo.Rdl_auditdate = null;
                CurrentDeliveryInfo.ListReaDeliveryDetail = dtReaDeliveryDetail;
                CurrentDeliveryInfo.Rdl_status = "1";

                var ret = new ProxyReaDelivery().Service.UpdateReaData(Caller, CurrentDeliveryInfo);

            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            RefreshPatients();
        }

        private void SysToolBar1_OnAuditClicked(object sender, EventArgs e)
        {
            if (CurrentDeliveryInfo == null || string.IsNullOrEmpty(CurrentDeliveryInfo.Rdl_no))
            {
                MessageDialog.Show("请勾选你要审核数据！", "提示");
                return;
            }
            if (CurrentDeliveryInfo.Rdl_status == "4")
            {
                MessageDialog.Show("该记录已审核！", "提示");
                return;
            }
            if (dtReaDeliveryDetail.Count <= 0)
            {
                MessageDialog.Show($"没有出库明细！", "提示");
                return;
            }
            bool check = CheckItems(dtReaDeliveryDetail);
            if (check)
            {
                EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = UserInfo.loginID;
                Caller.LoginName = UserInfo.userName;
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 审核", LIS_Const.ReagentPopedomCode.DeliveryAudit, "", "");
                frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                 //验证窗口
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    Caller.IPAddress = UserInfo.ip;
                    Caller.LoginID = frmCheck.OperatorID;
                    Caller.LoginName = frmCheck.OperatorName;
                    Caller.UserID = frmCheck.OperatorID;

                    CurrentDeliveryInfo.Rdl_auditor = Caller.UserID;
                    CurrentDeliveryInfo.Rdl_auditdate = ServerDateTime.GetServerDateTime();
                    CurrentDeliveryInfo.ListReaDeliveryDetail = dtReaDeliveryDetail;
                    CurrentDeliveryInfo.Rdl_status = "4";
                    CurrentDeliveryInfo.Rdl_applyid = selectDictSysUser1.valueMember;
                    CurrentDeliveryInfo.Rdl_deptid = selectDicReaDept1.valueMember;
                    CurrentDeliveryInfo.Rdl_claimid = selectDicReaClaimant1.valueMember;
                    var ret = new ProxyReaDelivery().Service.AuditReaData(Caller, CurrentDeliveryInfo);
                }
                else if (dig == DialogResult.No)
                {
                    MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }
            }

            RefreshPatients();
        }

        public List<EntityReaDelivery> GetCheckedReaDelivery()
        {
            gvReaDelivery.CloseEditor();
            this.bsReaDelivery.EndEdit();

            List<EntityReaDelivery> checkList = new List<EntityReaDelivery>();
            var selectIndex = gvReaDelivery.GetSelectedRows();
            foreach (int index in selectIndex)
            {
                checkList.Add(gvReaDelivery.GetRow(index) as EntityReaDelivery);
            }

            if (checkList.Count <= 0
                && CurrentDeliveryInfo != null
                && !string.IsNullOrEmpty(CurrentDeliveryInfo.Rdl_no))
            {
                checkList.Add(gvReaDelivery.GetFocusedRow() as EntityReaDelivery);
            }

            return checkList;
        }
        bool DeleteDelivery()
        {
            gvReaDelivery.CloseEditor();
            this.bsReaDelivery.EndEdit();

            StringBuilder logMsg = new StringBuilder();
            List<EntityReaDelivery> delReaDeliveryList = new List<EntityReaDelivery>();

            int patCount = 0;
            bool delflag = false;

            if (UserInfo.GetSysConfigValue("BathOrSingleDelFlag").Equals("是"))
            {
                List<EntityReaDelivery> dtReaDelivery = GetCheckedReaDelivery();
                if (dtReaDelivery == null) return false;
                delflag = true;

                foreach (var dr in dtReaDelivery)
                {
                    patCount++;

                    if (dr.Rdl_status.ToString() == "1"
                        || dr.Rdl_status.ToString() == "2")
                    {
                        delReaDeliveryList.Add(dr);
                    }
                }

            }
            else
            {
                delflag = false;
                EntityReaDelivery dr = this.gvReaDelivery.GetFocusedRow() as EntityReaDelivery;
                if (dr != null)
                {
                    patCount++;
                    if (dr.Rdl_status.ToString() == "1"
                        || dr.Rdl_status.ToString() == "2")
                    {
                        delReaDeliveryList.Add(dr);
                    }
                }
            }

            if (patCount == 0)
            {
                if (delflag == false)
                    MessageDialog.Show("请选中你要删除数据！", "提示");
                else
                    MessageDialog.Show("请勾选你要删除数据！", "提示");

                return false;
            }

            if (delReaDeliveryList.Count < 1)
            {
                lis.client.control.MessageDialog.Show(string.Format("所选数据已审核！"), "提示");
                return false;
            }
            if (delReaDeliveryList.Count > 1)
            {
                if (MessageDialog.Show(string.Format("您将要删除 {0} 条病人记录，是否继续？", delReaDeliveryList.Count), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }
            else
            {
                object name = delReaDeliveryList[0].Rdl_no;
                if (MessageDialog.Show(string.Format("您将要删除 出库单号:{0} 的记录，是否继续？", name != null ? name.ToString() : string.Empty), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.ReagentPopedomCode.DeliveryDelete, "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;

                var ret = new ProxyReaDelivery().Service.DeleteReaData(Caller, delReaDeliveryList);

                addNew();
                GetReaList(false);
                return true;
            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            return false;
        }
        private void SysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            DeleteDelivery();
        }

        private void SysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            GetReaList(false);
            addNew();
        }

        private void SysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            addNew();
        }
        private String getMaxSID()
        {
            DateTime dtPatDate = this.txtReaDate.DateTime;
            string sid = new ProxyReaDelivery().Service.GetReaSID_MaxPlusOne(dtPatDate, new DeliveryStep().StepCode);
            return sid;
        }
        private void gvReaPurchase_Click(object sender, EventArgs e)
        {
            if (bsReaApplication.Current != null)
            {
                EntityReaApply drLst = (EntityReaApply)bsReaApplication.Current;

                CurrentReaID = drLst.Ray_no;
                this.GetPurDetailData(CurrentReaID);
            }
        }
        private void addNew()
        {
            CurrentDeliveryInfo = new EntityReaDelivery();
            CurrentReaID = "";

            this.txtReaDate.Properties.ReadOnly = false;

            DateTime dtToday = ServerDateTime.GetServerDateTime();

            txtReaDate.EditValue = dtToday;
            //*********************************************
            txtReaSid.Text = getMaxSID();

            fpat_id = "";
            apply_id = "";
            textEdit2.Text = "";
            dtReaDeliveryDetail = new List<EntityReaDeliveryDetail>();

            this.gvReadetail.ClearSelection();

            gcReaDetail.DataSource = dtReaDeliveryDetail;
            gcReaDetail.RefreshDataSource();
            gvReadetail.RefreshData();
        }



        private void SysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            sysToolBar1.Focus();
            SaveOrUpdate();

        }
        public void SaveOrUpdate()
        {
            if (dtReaDeliveryDetail.Count <= 0)
            {
                MessageDialog.Show($"没有出库明细！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            if (string.IsNullOrEmpty(fpat_id))
            {

                bool check = CheckItems(dtReaDeliveryDetail);
                if (check)
                {
                    FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.DeliverySave, "", "");
                    frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                     //验证窗口
                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig == DialogResult.OK)
                    {
                        Caller.IPAddress = UserInfo.ip;
                        Caller.LoginID = frmCheck.OperatorID;
                        Caller.LoginName = frmCheck.OperatorName;
                        Caller.UserID = frmCheck.OperatorID;

                        EntityReaDelivery apply = new EntityReaDelivery();
                        apply.Rdl_no = txtReaSid.Text;
                        apply.Rdl_date = txtReaDate.DateTime;
                        apply.Rdl_srno = textEdit2.Text;
                        apply.ListReaDeliveryDetail = dtReaDeliveryDetail;
                        apply.Rdl_status = "1";
                        apply.Rdl_applyid = selectDictSysUser1.valueMember;
                        apply.Rdl_deptid = selectDicReaDept1.valueMember;
                        apply.Rdl_claimid = selectDicReaClaimant1.valueMember;

                        var ret = new ProxyReaDelivery().Service.SaveReaData(Caller, apply);
                    }
                    else if (dig == DialogResult.No)
                    {
                        MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (CurrentDeliveryInfo.Rdl_status == "1" || CurrentDeliveryInfo.Rdl_status == "2")
                {
                    bool check = CheckItems(dtReaDeliveryDetail);
                    if (check)
                    {
                        FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.DeliverySave, "", "");
                        frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                         //验证窗口
                        DialogResult dig = frmCheck.ShowDialog();
                        if (dig == DialogResult.OK)
                        {
                            Caller.IPAddress = UserInfo.ip;
                            Caller.LoginID = frmCheck.OperatorID;
                            Caller.LoginName = frmCheck.OperatorName;
                            Caller.UserID = frmCheck.OperatorID;

                            CurrentDeliveryInfo.Rdl_date = txtReaDate.DateTime;
                            CurrentDeliveryInfo.ListReaDeliveryDetail = dtReaDeliveryDetail;
                            CurrentDeliveryInfo.Rdl_status = "1";
                            CurrentDeliveryInfo.Rdl_applyid = selectDictSysUser1.valueMember;
                            CurrentDeliveryInfo.Rdl_deptid = selectDicReaDept1.valueMember;
                            CurrentDeliveryInfo.Rdl_claimid = selectDicReaClaimant1.valueMember;

                            var ret = new ProxyReaDelivery().Service.UpdateReaData(Caller, CurrentDeliveryInfo);
                        }
                        else if (dig == DialogResult.No)
                        {
                            MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                            return;
                        }
                    }
                }
                else
                {
                    MessageDialog.Show("当前记录已审核，不能进行当前操作！", "提示");
                }
            }
            RefreshPatients();
        }

        public bool CheckItems(List<EntityReaDeliveryDetail> list)
        {
            bool check = false;
            foreach (var item in dtReaDeliveryDetail)
            {
                EntityReaQC qc = new EntityReaQC();
                qc.Barcode = item.Rdvd_barcode;
                qc.ReaNo = item.StoreNo;
                List<EntityReaStorageDetail> inDetail = new ProxyReaStorageDetail().Service.GetDetail(qc);
                if (item.Rdvd_reacount > inDetail[0].Rsd_count)
                {
                    MessageDialog.Show("出库数目不能大于当前库存！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rdvd_no))
                {
                    MessageDialog.Show("出库单号不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rdvd_reaid))
                {
                    MessageDialog.Show("试剂信息不能为空！", "提示");
                    check = false;
                    break;
                }

                if (string.IsNullOrEmpty(item.Rdvd_barcode))
                {
                    MessageDialog.Show("条码号不能为空！", "提示");
                    check = false;
                    break;
                }

                check = true;
            }
            return check;
        }

        private void SysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 关闭编辑单元格并更新数据
        /// </summary>
        private void CloseEditor()
        {
            this.gvReadetail.CloseEditor();
        }
        /// <summary>
        /// 加载用户式样配置
        /// </summary>
        private void LoadUserSetting()
        {
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load("FrmReagenDelivery", string.Empty, UserInfo.loginID);
            if (setting != null)
            {
                ApplySetting(setting);

            }

        }
        public void ApplySetting(PatInputRuntimeSetting setting)
        {
            this.UserCustomSetting = setting;
            ApplySetting();
        }
        public void ApplySetting()
        {
            PatInputRuntimeSetting setting = this.UserCustomSetting;

            #region 左侧式样

            DataTable dtSearchBarDDL = new DataTable();
            dtSearchBarDDL.Columns.Add("name");
            dtSearchBarDDL.Columns.Add("value");

            DataRow drSearchBarDDL = dtSearchBarDDL.NewRow();

            drSearchBarDDL["name"] = string.Empty;
            drSearchBarDDL["value"] = string.Empty;
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "出库单号";
            drSearchBarDDL["value"] = "rea_sid";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            //this.cmbBarSearchPatType.Properties.DropDownRows = dtSearchBarDDL.Rows.Count;
            //this.cmbBarSearchPatType.Properties.DataSource = dtSearchBarDDL;
            //this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            //this.cmbBarSearchPatType.Properties.ValueMember = "value";



            #endregion

            #region 整行显示字体颜色 2018-05-11
            for (int i = 0; i < gvReaDelivery.Columns.Count; i++)
            {
                string columnName = gvReaDelivery.Columns[i].FieldName;
                if (columnName != "PatSelect" && columnName != "pat_icon")
                {
                    //未审核
                    FormatRowDeliveryTo(gvReaDelivery, columnName, LIS_Const.REAGENT_FLAG.Natural, setting.PatListPanel.BackColorNormal, setting.PatListPanel.BackColorNormal, setting.PatListPanel.ForeColorNormal);
                    //已审核
                    FormatRowDeliveryTo(gvReaDelivery, columnName, LIS_Const.REAGENT_FLAG.Audited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.ForeColorAudited);
                    //未通过
                    FormatRowDeliveryTo(gvReaDelivery, columnName, LIS_Const.REAGENT_FLAG.Returned, setting.PatListPanel.BackColorReturn, setting.PatListPanel.BackColorReturn, setting.PatListPanel.ForeColorReturn);
                    //已完成
                    FormatRowDeliveryTo(gvReaDelivery, columnName, LIS_Const.REAGENT_FLAG.Done, setting.PatListPanel.BackColorDone, setting.PatListPanel.BackColorDone, setting.PatListPanel.ForeColorDone);

                }
            }
            #endregion

            #region 中间式样

            //设置新增时获得焦点的控件
            if (FocusOnAddNewControl == null)
            {
                FocusOnAddNewControl = this.txtReaSid;
            }

            if (FocusOnAddNewControl != null)
            {
                this.ActiveControl = FocusOnAddNewControl;
                FocusOnAddNewControl.Focus();
            }
            #endregion
        }

        private void FormatRowDeliveryTo(GridView gridView, string fieldName, string auditedValue, Color backColorAudited, Color backColor, Color foreColor)
        {
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridView.Columns["Rdl_status"];
            rule.ColumnApplyTo = gridView.Columns[fieldName];
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = auditedValue;
            if (backColorAudited != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = backColor;//已审核
            }
            formatConditionRuleValue1.Appearance.ForeColor = foreColor;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridView.FormatRules.Add(rule);
        }

        /// <summary>
        /// 绑定
        /// </summary>
        private void BindGrid()
        {
            if (this.dtReaDeliveryDetail != null)
            {
                this.dtReaDeliveryDetail = this.dtReaDeliveryDetail.OrderBy(i => i.ReagentName).ToList();

                CloseEditor();

                this.gvReadetail.ClearGrouping();
                this.gvReadetail.ClearSorting();
                this.gcReaDetail.DataSource = this.dtReaDeliveryDetail;

                gvReadetail.RefreshData();
                gcReaDetail.RefreshDataSource();
            }
        }
        private void BindLookupData()
        {
            this.gcReaDetail.DataSource = this.dtReaDeliveryDetail;
            gvReadetail.RefreshData();
            gcReaDetail.RefreshDataSource();
        }
        private void SearchPatientsAndAddNew()
        {
            GetReaList(false);

            addNew();

            this.txtReaSid.Text = getMaxSID();
        }

        private void SelectedPageChanged(int index)
        {
            switch (index)
            {
                case 0:
                    cbeFlag.EditValue = "-1";//全部
                    break;
                case 1:
                    cbeFlag.EditValue = "1";//未审核
                    break;
                case 2:
                    cbeFlag.EditValue = "2";//审核未通过
                    break;
                case 4:
                    cbeFlag.EditValue = "4";//已审核
                    break;
                case 8:
                    cbeFlag.EditValue = "8";//未打印
                    break;
                case 9:
                    cbeFlag.EditValue = "9";//已完成
                    break;
                default:
                    break;
            }
        }
        private void cbeAge_EditValueChanged(object sender, EventArgs e)
        {
            GetReaList(false);
            addNew();
        }
        /// <summary>
        /// 根据时间获得列表
        /// </summary>
        /// <param name="isTrue"></param>
        private void GetReaList(bool isTrue)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.DateStart = dtBegin.DateTime.Date;
            qc.DateEnd = dtEnd.DateTime.Date.AddDays(1).AddSeconds(-1);

            string strPatFlag = cbeFlag.EditValue.ToString();
            if (strPatFlag == "1")//未审核
            {
                qc.ReaStatus = "1";
            }
            else if (strPatFlag == "2")//审核不通过
            {
                qc.ReaStatus = "2";
            }
            else if (strPatFlag == "4")//已审核
            {
                qc.ReaStatus = "4";
            }
            else if (strPatFlag == "8")//未打印
            {
                qc.ReaPrintFlag = "0";
            }
            var list = new ProxyReaDelivery().Service.ReaQuery(qc);

            EntityReaQC reaQc = new EntityReaQC();
            if (true)
            {
                qc.DateStart = dtBegin.DateTime.Date;
                qc.DateEnd = dtEnd.DateTime.Date.AddDays(1).AddSeconds(-1);
            }

            qc.ReaStatus = "4";
            var applyList = new ProxyReaApply().Service.ReaQuery(qc);

            AllReaList = list;
            AllApplyList = applyList;
            List<EntityReaDelivery> dtOldRea = bsReaDelivery.DataSource as List<EntityReaDelivery>;

            bsReaDelivery.ResetBindings(false);

            bsReaDelivery.DataSource = list;

            gcReaDelivery.RefreshDataSource();
            gvReaDelivery.RefreshData();

            bsReaApplication.ResetBindings(false);

            bsReaApplication.DataSource = applyList;

            gcReaApply.RefreshDataSource();
            gvReaApply.RefreshData();

            RefreshItemsCount();//更新记录
            this.fpat_id = "";
            this.apply_id = "";
            this.textEdit2.Text = "";
            RapitSearch();
            isLoadData = true;
        }
        private void RapitSearch()
        {
            List<EntityReaDelivery> ReaList = AllReaList;
            List<EntityReaApply> ApplyList = AllApplyList;
            if (AllReaList.Count == 0) return;

            //if (this.cmbBarSearchPatType.EditValue != null
            //    && this.cmbBarSearchPatType.EditValue.ToString().Trim(null) != string.Empty
            //    && this.txtBarSearchCondition.Text.Trim(null) != string.Empty)
            //{
            //    string searchField = this.cmbBarSearchPatType.EditValue.ToString();
            //    string searchValue = this.txtBarSearchCondition.Text;


            //    //条码号
            //    if (searchField == "rea_sid")
            //    {
            //        ReaList = ReaList.Where(i => i.Rdl_no.Contains(searchValue)).ToList();
            //    }
            //    //申领单号
            //    if (searchField == "ray_no")
            //    {
            //        if (ApplyList != null&& ApplyList.Count >0)
            //        {
            //            ApplyList = ApplyList.Where(i => i.Ray_no.Contains(searchValue)).ToList();

            //        }
            //    }
            //}
            //else
            //{
            //    ReaList = AllReaList;

            //}
            bsReaDelivery.DataSource = ReaList;
            bsReaApplication.DataSource = ApplyList;
            RefreshItemsCount();//更新记录数量信息
            bsReaLst_PositionChanged(null, null);
        }
        /// <summary>
        /// 样本状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsReaLst_PositionChanged(object sender, EventArgs e)
        {
            if (bsReaDelivery.Current != null)
            {
                EntityReaDelivery drLst = (EntityReaDelivery)bsReaDelivery.Current;

                if (drLst != null && !string.IsNullOrEmpty(drLst.Rdl_no))
                {
                    string pid = drLst.Rdl_no;
                    if (pid != CurrentReaID)
                    {
                        if (AnPatientChanging(CurrentReaID, pid, drLst))
                        {
                            CurrentReaID = pid;
                            this.GetReaDetailData(pid);
                            this.FillUiFromEntity();

                        }
                    }
                }
            }
        }
        public void FillUiFromEntity()
        {
            isDataChaged = false;

            if (CurrentDeliveryInfo == null || string.IsNullOrEmpty(CurrentDeliveryInfo.Rdl_no))
                return;

            fpat_id = CurrentDeliveryInfo.Rdl_no;
            var dr = CurrentDeliveryInfo;
            txtReaSid.Text = CurrentDeliveryInfo.Rdl_no;
            txtReaDate.DateTime = CurrentDeliveryInfo.Rdl_date ?? DateTime.MinValue;
            textEdit2.Text = CurrentDeliveryInfo.Rdl_srno;
            selectDictSysUser1.valueMember = CurrentDeliveryInfo.Rdl_applyid;
            selectDicReaClaimant1.valueMember = CurrentDeliveryInfo.Rdl_claimid;
            selectDicReaDept1.valueMember = CurrentDeliveryInfo.Rdl_deptid;

            if (CurrentDeliveryInfo.Rdl_status == "2" && !string.IsNullOrEmpty(CurrentDeliveryInfo.Rdl_returnreason))
            {
                txtReason.Visible = true;
                txtReason.Text = CurrentDeliveryInfo.Rdl_returnreason;
            }
        }
        //根据ID得到数据
        private void GetPurDetailData(string _pat_id)
        {
            isDataChaged = false;
            if (dtReaDeliveryDetail.Count > 0)
            {
                lis.client.control.MessageDialog.Show("已有申领订单被添加！", "提示");
                return;
            }
            apply_id = _pat_id;


            var resList = new ProxyReaApplyDetail().Service.GetReaApplyDetailByReaId(_pat_id);
            List<EntityReaSetting> setList = CacheClient.GetCache<EntityReaSetting>();
            var invetoryList = new ProxyReaDelivery().Service.SearchAllReaStoreCount();
            if (resList != null && resList.Count > 0)
            {
                textEdit2.Text = resList[0].Rdet_no;
                DateTime dtToday = ServerDateTime.GetServerDateTime();

                txtReaDate.EditValue = dtToday;

                selectDictSysUser1.valueMember = resList[0].Rdet_applier;

                foreach (var item in resList)
                {
                    EntityReaStoreCount entity = invetoryList.Find(m => string.Equals(item.Rdet_reaid, m.Rri_Drea_id));
                    if (item.Rdet_reacount > entity.Rri_Count)
                    {
                        lis.client.control.MessageDialog.Show($"当前试剂{item.ReagentName}的申领数目为{item.Rdet_reacount}，超出库存{entity.Rri_Count}！", "提示");
                        return;
                    }
                    List<EntityReaStorageDetail> storeList = new ProxyReaStorageDetail().Service.getNotdeliveredById(item.Rdet_reaid, item.Rdet_reacount);
                    if (storeList == null || storeList.Count < item.Rdet_reacount)
                    {
                        lis.client.control.MessageDialog.Show($"当前试剂{item.ReagentName}的出库记录数不足以出库！", "提示");
                        return;
                    }
                    for (int i = 0; i < item.Rdet_reacount; i++)
                    {
                        EntityReaDeliveryDetail detail = new EntityReaDeliveryDetail();
                        EntityReaSetting setting = setList.Find(m => m.Drea_id == item.Rdet_reaid);
                        detail.del_flag = 0;
                        detail.IsNew = 1;

                        detail.Rdvd_no = txtReaSid.Text;

                        detail.Rdvd_reaid = item.Rdet_reaid;
                        detail.ReagentName = item.ReagentName;
                        detail.ReagentPackage = item.ReagentPackage;
                        detail.ApplyCount = item.Rdet_reacount;
                        detail.ApplyNo = item.Rdet_no;
                        detail.PdtName = setting.Rpdt_name;
                        detail.Rdvd_reacount = 1;
                        detail.Rdvd_count = item.Rdet_reacount;
                        detail.Rdvd_barcode = storeList[i].Rsd_barcode;
                        detail.pdt_id = storeList[i].Rsd_pdtid;
                        detail.grp_id = storeList[i].Rsd_groupid;
                        detail.pos_id = storeList[i].Rsd_posid;
                        detail.con_id = storeList[i].Rsd_conid;
                        detail.sup_id = storeList[i].Rsd_supid;
                        detail.unit_id = storeList[i].Rsd_unitid;
                        detail.package = storeList[i].Rsd_package;
                        this.dtReaDeliveryDetail.Add(detail);
                    }

                }
            }
            BindLookupData();

        }
        //根据ID得到数据
        private void GetReaDetailData(string _pat_id)
        {
            isDataChaged = false;

            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = _pat_id;
            var resList = new ProxyReaDeliveryDetail().Service.GetDetail(qc);
            dtReaDeliveryDetail = resList;
            CurrentDeliveryInfo.ListReaDeliveryDetail = dtReaDeliveryDetail;
            BindGrid();
        }
        bool AnPatientChanging(string prev_patid, string pat_id, EntityReaDelivery drPat)
        {
            if (isDataChaged && isLoadData && (!string.IsNullOrEmpty(txtReaSid.Text)))
            {
                if (MessageDialog.Show("当前资料或结果未保存，是否保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SysToolBar1_OnBtnSaveClicked(null, null);
                }
            }
            return true;
        }
        /// <summary>
        /// 更新记录数量信息
        /// </summary>
        public void RefreshItemsCount()
        {
            int countUnAudited = 0;
            int countAudited = 0;
            int countPrinted = 0;
            int countReturned = 0;
            int countTotal = 0;

            if (this.bsReaDelivery.DataSource != null)
            {
                List<EntityReaDelivery> dtpat = this.bsReaDelivery.DataSource as List<EntityReaDelivery>;
                if (dtpat != null)
                {
                    countTotal = dtpat.Count;

                    foreach (var item in dtpat)
                    {
                        if (item.Rdl_status != null)
                        {
                            if (item.Rdl_status.ToString() == "4")
                            {
                                countAudited++;
                            }
                            else if (item.Rdl_status.ToString() == "0")
                            {
                                countUnAudited++;
                            }
                            else if (item.Rdl_status.ToString() == "2")
                            {
                                countReturned++;
                            }
                        }
                        if (item.Rdl_printflag.ToString() == "1")
                        {
                            countPrinted++;
                        }
                    }
                }
            }
            this.lbRecordCount.Text = string.Format("总数：{0} 已审核：{1} 未审核：{2} 已打印：{3} 未通过：{4}", countTotal, countAudited, countUnAudited, countPrinted, countReturned);
        }
        private void FrmReagenDelivery_Load(object sender, EventArgs e)
        {
            LoadUserSetting();
            ApplySetting();
            dtBegin.EditValue = DateTime.Now;
            dtEnd.EditValue = DateTime.Now;
            string range = UserInfo.GetSysConfigValue("Rea_DateRange");

            DateTime dtServer = ServerDateTime.GetServerDateTime();
            txtReaDate.DateTime = dtServer;
            if (!string.IsNullOrEmpty(range))
            {
                dtBegin.EditValue = dtServer.AddDays(-(Convert.ToInt32(range) - 1));
            }
            sysToolBar1.OrderCustomer = true;
            sysToolBar1.BtnDelete.Caption = "删除";
            sysToolBar1.BtnUndo.Caption = "取消审核";
            sysToolBar1.BtnAnswer.Caption = "撤销出库";
            sysToolBar1.SetToolButtonStyle(new string[] {sysToolBar1.BtnAdd.Name,
                                                sysToolBar1.BtnSave.Name,
                                                sysToolBar1.BtnDelete.Name,
                                                sysToolBar1.BtnAnswer.Name,
                                                sysToolBar1.BtnRefresh.Name,
                                                sysToolBar1.BtnAudit.Name,
                                                sysToolBar1.BtnPrint.Name,
                                                sysToolBar1.BtnPrintPreview2.Name,
                                                sysToolBar1.BtnClose.Name});
            BindLookupData();
        }



        private void roundPanelGroup1_Click(object sender, EventArgs e)
        {
            RoundPanel curRp = roundPanelGroup1.GetCurRoundPanel();
            if (curRp.Tag == null)
                return;
            int pageIndex = ConvertHelper.IntParse(curRp.Tag?.ToString(), 0);
            SelectedPageChanged(pageIndex);
        }

        private void dtBegin_EditValueChanged(object sender, EventArgs e)
        {
            if (dtEnd.EditValue == null)
                return;
            SearchPatientsAndAddNew();
        }

        private void dtEnd_EditValueChanged(object sender, EventArgs e)
        {
            if (dtBegin.EditValue == null)
                return;
            SearchPatientsAndAddNew();
        }

        private void gvReaDelivery_Click(object sender, EventArgs e)
        {
            if (bsReaDelivery.Current != null)
            {
                EntityReaDelivery drLst = (EntityReaDelivery)bsReaDelivery.Current;

                CurrentDeliveryInfo = drLst;
                CurrentReaID = drLst.Rdl_no;
                this.GetReaDetailData(CurrentReaID);
                this.FillUiFromEntity();
                isDataChaged = false;
            }
        }

        private void 面板设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmReaPanelConfig frm = new FrmReaPanelConfig(this);
            frm.Show();
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.CloseEditor();

            ContextMenuStrip menu = (sender as ContextMenuStrip);

            menu.Visible = false;
            switch (e.ClickedItem.Name)
            {
                case "MenuDelItem":
                    MenuDelItemClick(gvReadetail, true);
                    break;
            }
        }
        #region 删除项目
        /// <summary>
        /// 菜单删除项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        /// <param name="bNeedConfirm">是否需要提示确认</param>
        private void MenuDelItemClick(GridView sourceGrid, bool bNeedConfirm)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = txtReaSid.Text;
            //获取当前记录的审核状态
            ProxyReaDelivery mainProxy = new ProxyReaDelivery();
            List<EntityReaDelivery> list = mainProxy.Service.ReaQuery(qc);

            if (list != null && list.Count > 0 && list[0].Rdl_status != "0" && list[0].Rdl_status != "2")
            {
                lis.client.control.MessageDialog.Show("当前记录已审核，不能删除", "提示");
            }
            else
            {
                int[] selectedRowHandler = sourceGrid.GetSelectedRows();

                if (selectedRowHandler.Length == 0)
                {
                    return;
                }
                List<EntityReaDeliveryDetail> selectDataRows = new List<EntityReaDeliveryDetail>();
                bool needComma = false;
                string tipItemsText = string.Empty;
                foreach (int rowHandler in selectedRowHandler)
                {
                    EntityReaDeliveryDetail row = sourceGrid.GetRow(rowHandler) as EntityReaDeliveryDetail;
                    if (row != null)
                    {
                        selectDataRows.Add(row);
                    }

                    if (needComma)
                    {
                        tipItemsText += "，";
                    }

                    tipItemsText += string.Format("[{0}]", row.ReagentName.TrimEnd().ToString());

                    needComma = true;
                }

                string messagetips = "是否要移除项目{0}？\r\n{1}";

                if (UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除") //是否立刻从数据库删除
                {
                    messagetips = string.Format(messagetips, tipItemsText, "当前的配置为：[确定]后立刻从数据库删除");
                }
                else
                {
                    messagetips = string.Format(messagetips, tipItemsText, "当前的配置为：[确定]后需要点击[保存]才从数据库中删除");
                }

                if (!bNeedConfirm
                    || lis.client.control.MessageDialog.Show(messagetips, "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RemoveItem(selectDataRows, true);
                }
            }
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        public void RemoveItem(List<EntityReaDeliveryDetail> rowsPatResultItem, bool removeHasResult)
        {
            bool deleteInDatabase = false;
            if (UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除")
            {
                deleteInDatabase = true;
            }

            for (int i = 0; i < rowsPatResultItem.Count; i++)
            {
                EntityReaDeliveryDetail drPatResultItem = rowsPatResultItem[i];
                //是否已录入结果
                bool hasResult = false;
                if (drPatResultItem.Rdvd_reacount > 0)
                {
                    hasResult = true;
                }
                //数据库是否存在结果
                bool recordInDataBase = false;
                if (drPatResultItem.ObrSn != 0)
                {
                    recordInDataBase = true;
                }
                if (hasResult)
                {
                    if (removeHasResult && recordInDataBase)
                    {
                        if (!Compare.IsEmpty(drPatResultItem.Rdvd_no) && !Compare.IsEmpty(drPatResultItem.ObrSn))
                        {
                            if (deleteInDatabase)
                            {

                                EntityLogLogin logLogin = new EntityLogLogin();
                                logLogin.LogIP = UserInfo.ip;
                                logLogin.LogLoginID = UserInfo.loginID;

                                string rsdno = drPatResultItem.Rdvd_no.ToString();
                                string rea_name = drPatResultItem.ReagentName.TrimEnd().ToString();
                                string rea_itm_id = string.Empty;
                                if (!Compare.IsEmpty(drPatResultItem.Rdvd_reaid))
                                {
                                    rea_itm_id = drPatResultItem.Rdvd_reaid.ToString();
                                }

                                long reskey = -1;

                                bool opResult = false;
                                if (!Compare.IsEmpty(drPatResultItem.ObrSn))
                                {
                                    reskey = Convert.ToInt64(drPatResultItem.ObrSn);
                                }

                                if (rea_itm_id == string.Empty)
                                {

                                }
                                else if (reskey != -1)
                                {
                                    opResult = new ProxyReaDeliveryDetail().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), rsdno);
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    this.dtReaDeliveryDetail.Remove(drPatResultItem);
                                }


                                if (!opResult)
                                {
                                    MessageDialog.Show(string.Format("删除[{0}]失败！", rea_name), "错误");
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    int deleteIndex = dtReaDeliveryDetail.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                    if (deleteIndex > -1)
                                        this.dtReaDeliveryDetail.RemoveAt(deleteIndex);
                                }
                            }
                            else
                            {
                                drPatResultItem.IsNew = 2;
                            }
                        }
                        else
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                            this.dtReaDeliveryDetail.Remove(drPatResultItem);
                            i--;
                        }
                    }
                    else
                    {
                        if (!recordInDataBase)
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                            this.dtReaDeliveryDetail.Remove(drPatResultItem);
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(drPatResultItem.Rdvd_no) && drPatResultItem.ObrSn != 0)
                    {
                        if (deleteInDatabase)
                        {
                            EntityLogLogin logLogin = new EntityLogLogin();
                            logLogin.LogIP = UserInfo.ip;
                            logLogin.LogLoginID = UserInfo.loginID;

                            string resid = drPatResultItem.Rdvd_no.ToString();
                            string res_itm_ecd = drPatResultItem.ReagentName.TrimEnd().ToString();
                            string res_itm_id = string.Empty;
                            if (!Compare.IsEmpty(drPatResultItem.Rdvd_reaid))
                            {
                                res_itm_id = drPatResultItem.Rdvd_reaid.ToString();
                            }

                            long reskey = -1;

                            bool opResult = false;
                            if (!Compare.IsEmpty(drPatResultItem.ObrSn))
                            {
                                reskey = Convert.ToInt64(drPatResultItem.ObrSn);
                            }

                            if (res_itm_id == string.Empty)
                            {

                            }
                            else if (reskey != 0)
                            {
                                opResult = new ProxyReaDeliveryDetail().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
                            }
                            else
                            {
                                rowsPatResultItem.Remove(drPatResultItem);
                            }


                            if (!opResult)
                            {
                                MessageDialog.Show(string.Format("删除[{0}]失败！", res_itm_ecd), "错误");
                            }
                            else
                            {
                                rowsPatResultItem.Remove(drPatResultItem);

                                int deleteIndex = dtReaDeliveryDetail.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                if (deleteIndex > -1)
                                    this.dtReaDeliveryDetail.RemoveAt(deleteIndex);
                            }

                        }
                        else
                        {
                            drPatResultItem.IsNew = 2;
                        }
                    }
                    else
                    {
                        rowsPatResultItem.Remove(drPatResultItem);
                        i--;
                        this.dtReaDeliveryDetail.Remove(drPatResultItem);
                    }
                }
            }
            this.gcReaDetail.RefreshDataSource();
            this.BindGrid();

        }
        #endregion

        private void txtPatID_EnterKeyDown(object sender, EventArgs args)
        {
            if (!string.IsNullOrEmpty(txtPatID.Text))
            {
                if (dtReaDeliveryDetail!=null && dtReaDeliveryDetail.Count > 0)
                {
                    foreach (var detail in dtReaDeliveryDetail)
                    {
                        if (string.Equals(txtPatID.Text,detail.Rdvd_barcode))
                        {
                            lis.client.control.MessageDialog.Show("当前记录已被添加至出库列表", "提示");
                            txtPatID.Text = string.Empty;
                            return;
                        }
                    }
                }

                EntityReaQC qc = new EntityReaQC();
                qc.Barcode = txtPatID.Text;

                var item = new ProxyReaStorageDetail().Service.GetDetail(qc);
                if (item != null && item.Count > 0)
                {
                    if (item[0].Rsd_status != "0" && item[0].Rsd_count == 0)
                    {
                        lis.client.control.MessageDialog.Show("当前记录已经出库或报损", "提示");
                        txtPatID.Text = string.Empty;
                        return;
                    }
                    var detailList = new ProxyReaStorageDetail().Service.getNotdeliveredById(item[0].Rsd_reaid, 1);
                    if (Convert.ToInt32(detailList[0].sort_no) < Convert.ToInt32(item[0].sort_no))
                    {
                        if (MessageDialog.Show($"当前批次有效日期{item[0].Rsd_validdate.Date},最邻近失效批次为{detailList[0].Rsd_batchno}，条码为{detailList[0].Rsd_barcode}，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            EntityReaDeliveryDetail detail = new EntityReaDeliveryDetail();
                            List<EntityReaSetting> setList = CacheClient.GetCache<EntityReaSetting>();
                            EntityReaSetting setting = setList.Find(m => m.Drea_id == item[0].Rsd_reaid);
                            detail.del_flag = 0;
                            detail.IsNew = 1;

                            detail.Rdvd_no = txtReaSid.Text;

                            detail.Rdvd_reaid = item[0].Rsd_reaid;
                            detail.ReagentName = item[0].ReagentName;
                            detail.ReagentPackage = item[0].Rsd_package;

                            detail.pdt_id = item[0].Rsd_pdtid;
                            detail.grp_id = item[0].Rsd_groupid;
                            detail.pos_id = item[0].Rsd_posid;
                            detail.con_id = item[0].Rsd_conid;
                            detail.sup_id = item[0].Rsd_supid;
                            detail.unit_id = item[0].Rsd_unitid;
                            detail.package = item[0].Rsd_package;
                            detail.StoreNo = item[0].Rsd_no;

                            detail.PdtName = setting.Rpdt_name;
                            detail.Rdvd_reacount = 1;
                            detail.Rdvd_count = 1;
                            detail.Rdvd_barcode = txtPatID.Text;
                            this.dtReaDeliveryDetail.Add(detail);

                            BindLookupData();

                            txtPatID.Text = string.Empty;
                        }
                    }
                    else
                    {
                        EntityReaDeliveryDetail detail = new EntityReaDeliveryDetail();
                        List<EntityReaSetting> setList = CacheClient.GetCache<EntityReaSetting>();
                        EntityReaSetting setting = setList.Find(m => m.Drea_id == item[0].Rsd_reaid);
                        detail.del_flag = 0;
                        detail.IsNew = 1;

                        detail.Rdvd_no = txtReaSid.Text;

                        detail.Rdvd_reaid = item[0].Rsd_reaid;
                        detail.ReagentName = item[0].ReagentName;
                        detail.ReagentPackage = item[0].Rsd_package;

                        detail.pdt_id = item[0].Rsd_pdtid;
                        detail.grp_id = item[0].Rsd_groupid;
                        detail.pos_id = item[0].Rsd_posid;
                        detail.con_id = item[0].Rsd_conid;
                        detail.sup_id = item[0].Rsd_supid;
                        detail.unit_id = item[0].Rsd_unitid;
                        detail.package = item[0].Rsd_package;
                        detail.StoreNo = item[0].Rsd_no;

                        detail.PdtName = setting.Rpdt_name;
                        detail.Rdvd_reacount = 1;
                        detail.Rdvd_count = 1;
                        detail.Rdvd_barcode = txtPatID.Text;
                        this.dtReaDeliveryDetail.Add(detail);

                        BindLookupData();

                        txtPatID.Text = string.Empty;
                    }

                }
                else
                {
                    lis.client.control.MessageDialog.Show("没有相应条码记录", "提示");
                    txtPatID.Text = string.Empty;
                    return;
                }
            }
        }

        private void txtBatchNo_EnterKeyDown(object sender, EventArgs args)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.JudgeCount = true;
            qc.JudgeValidtime = true;
            qc.BatchNo = txtBatchNo.Text;
            qc.ReaId = selectDicReaSetting1.valueMember;
            var storeList = new ProxyReaStorageDetail().Service.GetDetail(qc).OrderBy(m => m.sort_no).ToList<EntityReaStorageDetail>();


            if (storeList != null && storeList.Count > 0)
            {
                bsReaStoreDetail.DataSource = storeList;
                gridControl1.RefreshDataSource();
            }
            else
            {
                lis.client.control.MessageDialog.Show("没有相应入库记录", "提示");
                txtPatID.Text = string.Empty;
                return;
            }
        }

        private void SelectDicReaSetting1_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.JudgeCount = true;
            qc.JudgeValidtime = true;
            qc.BatchNo = txtBatchNo.Text;
            qc.ReaId = selectDicReaSetting1.valueMember;
            var storeList = new ProxyReaStorageDetail().Service.GetDetail(qc).OrderBy(m => m.sort_no).ToList<EntityReaStorageDetail>();
            if (storeList != null && storeList.Count > 0)
            {
                bsReaStoreDetail.DataSource = storeList;
                gridControl1.RefreshDataSource();
            }
            else
            {
                lis.client.control.MessageDialog.Show("没有相应入库记录", "提示");
                txtPatID.Text = string.Empty;
                return;
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            if (bsReaStoreDetail.Current != null)
            {
                EntityReaStorageDetail entity = bsReaStoreDetail.Current as EntityReaStorageDetail;
                if (dtReaDeliveryDetail != null && dtReaDeliveryDetail.Count > 0)
                {
                    foreach (var item in dtReaDeliveryDetail)
                    {
                        if (string.Equals(entity.Rsd_barcode, item.Rdvd_barcode))
                        {
                            lis.client.control.MessageDialog.Show("当前记录已被添加至出库单，请勿重复添加", "提示");
                            txtPatID.Text = string.Empty;
                            return;
                        }
                    }
                }
                EntityReaDeliveryDetail detail = new EntityReaDeliveryDetail();
                EntityReaSetting setting = setList.Find(m => m.Drea_id == entity.Rsd_reaid);
                detail.del_flag = 0;
                detail.IsNew = 1;

                detail.Rdvd_no = txtReaSid.Text;

                detail.Rdvd_reaid = entity.Rsd_reaid;
                detail.ReagentName = entity.ReagentName;
                detail.ReagentPackage = entity.Rsd_package;
                detail.PdtName = setting.Rpdt_name;
                detail.Rdvd_reacount = 1;
                detail.Rdvd_barcode = entity.Rsd_barcode;
                detail.pdt_id = entity.Rsd_pdtid;
                detail.grp_id = entity.Rsd_groupid;
                detail.pos_id = entity.Rsd_posid;
                detail.con_id = entity.Rsd_conid;
                detail.sup_id = entity.Rsd_supid;
                detail.unit_id = entity.Rsd_unitid;
                detail.package = entity.Rsd_package;
                detail.StoreNo = entity.Rsd_no;
                this.dtReaDeliveryDetail.Add(detail);
                BindLookupData();
            }
        }
    }
}
