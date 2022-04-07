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
using dcl.common;
using dcl.client.wcf;

namespace wf.client.reagent
{
    public partial class FrmReagentLossReport : FrmCommon, IPatPanelConfig
    {
        List<EntityDCLPrintParameter> listPrintData_multithread;
        bool useMultiThread = false;
        /// <summary>
        /// 当前报损信息
        /// </summary>
        EntityReaLossReport CurrentLossReportInfo = new EntityReaLossReport();//病人消息
        /// <summary>
        /// 面板配置类
        /// </summary>
        PatInputRuntimeSetting UserCustomSetting = null;
        /// <summary>
        /// 新增时获得焦点的控件
        /// </summary>
        Control FocusOnAddNewControl = null;
        List<EntityReaLossReport> AllReaList = new List<EntityReaLossReport>();

        private String fpat_id = "";//主键

        List<EntityReaSetting> setList;
        private string CurrentReaID;
        bool isLoadData = false;
        bool isDataChaged = false;
        /// <summary>
        /// 试剂报损表
        /// </summary>
        List<EntityReaLossReportDetail> dtReaLossReportDetail = new List<EntityReaLossReportDetail>();
        public FrmReagentLossReport()
        {
            InitializeComponent();
            this.roundPanelGroup1.RoundLossReportGroupClick += this.roundPanelGroup1_Click;
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
            setList = CacheClient.GetCache<EntityReaSetting>();

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
            string prtTemplate = "ReagentLossReport";

            this.BeginLoading();
            List<string> listPatID = new List<string>();
            StringBuilder sbPatSidWhere2 = new StringBuilder();
            if (pat_id == null)
            {
                List<EntityReaLossReport> listPatient = GetCheckedReaLossReport();
                if (listPatient.Count > 0)
                {
                    foreach (EntityReaLossReport dr in listPatient)
                    {
                        if (isPreview)
                        {
                            listPatID.Add(dr.Rlr_no.ToString());
                        }
                        else
                        {
                            if (dr.Rlr_status.ToString() == LIS_Const.REAGENT_FLAG.Audited)
                            {
                                if (string.IsNullOrEmpty(dr.AuditorName))
                                {
                                    sbPatSidWhere2.Append(string.Format(",[{0}]", dr.Rlr_no));
                                    continue;
                                }
                                listPatID.Add(dr.Rlr_no.ToString());

                            }
                        }
                    }

                    if (sbPatSidWhere2.Length > 0)
                    {
                        MessageDialog.Show("单号为:" + sbPatSidWhere2.Remove(0, 1) + " 的报损单无审核者信息，需重新审核", "提示");
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
                printPara.CustomParameter.Add("LossReportId", patient_id);
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
                    this.SelectAllLossReportInGrid(false);
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
        public void SelectAllLossReportInGrid(bool selectAll)
        {
            //在全部勾选前把焦点行设置成-1解决：全选的时候焦点行会显示不了勾
            if (selectAll)
            {
                gvReaLossReport.SelectAll();
            }
            else
                gvReaLossReport.ClearSelection();
            this.gcReaLossReport.RefreshDataSource();
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
                new ProxyReaLossReport().Service.UpdatePrintState_whitOperator(listPatID, UserInfo.loginID, UserInfo.userName, strPlace);
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
            if (GetCheckedReaLossReport().Count <= 0)
            {
                return;
            }
            Print(true);
        }

        private void SysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (GetCheckedReaLossReport().Count <= 0)
            {
                return;
            }
            Print(false);
            RefreshPatients();
        }

        private void SysToolBar1_BtnAnswerClick(object sender, EventArgs e)
        {
            if (CurrentLossReportInfo == null || string.IsNullOrEmpty(CurrentLossReportInfo.Rlr_no))
            {
                MessageDialog.Show("请勾选你要撤销数据！", "提示");
                return;
            }
            //if (CurrentLossReportInfo.Rlr_status == "4")
            //{
            //    MessageDialog.Show("该记录已审核！", "提示");
            //    return;
            //}
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            FrmReaCheckPassword frmCheck = new FrmReaCheckPassword("身份验证 - 撤销", LIS_Const.ReagentPopedomCode.Audit, "", "");
            DialogResult dig = frmCheck.ShowDialog();

            if (dig == DialogResult.OK)
            {
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;
                Caller.UserID = frmCheck.OperatorID;
                CurrentLossReportInfo.Rlr_returnreason = frmCheck.ReturnReason;
                new ProxyReaLossReport().Service.ReturnReaData(Caller, CurrentLossReportInfo);

            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            RefreshPatients();
        }

        private void SysToolBar1_BtnUndoClick(object sender, EventArgs e)
        {
            if (CurrentLossReportInfo == null || string.IsNullOrEmpty(CurrentLossReportInfo.Rlr_no))
            {
                MessageDialog.Show("请勾选你要取消审核数据！", "提示");
                return;
            }
            if (CurrentLossReportInfo.Rlr_status != "4")
            {
                MessageDialog.Show("该记录未审核！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 取消审核", LIS_Const.ReagentPopedomCode.Audit, "", "");
            frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                             //验证窗口
            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;
                Caller.UserID = frmCheck.OperatorID;

                CurrentLossReportInfo.Rlr_auditor = string.Empty;
                CurrentLossReportInfo.Rlr_auditdate = null;
                CurrentLossReportInfo.ListReaLossReportDetail = dtReaLossReportDetail;
                CurrentLossReportInfo.Rlr_status = "1";

                var ret = new ProxyReaLossReport().Service.UpdateReaData(Caller, CurrentLossReportInfo);

            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            RefreshPatients();
        }

        private void SysToolBar1_OnAuditClicked(object sender, EventArgs e)
        {
            if (CurrentLossReportInfo == null || string.IsNullOrEmpty(CurrentLossReportInfo.Rlr_no))
            {
                MessageDialog.Show("请勾选你要审核数据！", "提示");
                return;
            }
            if (CurrentLossReportInfo.Rlr_status == "4")
            {
                MessageDialog.Show("该记录已审核！", "提示");
                return;
            }
            if (dtReaLossReportDetail.Count <= 0)
            {
                MessageDialog.Show($"没有报损明细！", "提示");
                return;
            }
            bool check = CheckItems(dtReaLossReportDetail);
            if (check)
            {
                EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = UserInfo.loginID;
                Caller.LoginName = UserInfo.userName;
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 审核", LIS_Const.ReagentPopedomCode.Audit, "", "");
                frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                 //验证窗口
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    Caller.IPAddress = UserInfo.ip;
                    Caller.LoginID = frmCheck.OperatorID;
                    Caller.LoginName = frmCheck.OperatorName;
                    Caller.UserID = frmCheck.OperatorID;

                    CurrentLossReportInfo.Rlr_auditor = Caller.UserID;
                    CurrentLossReportInfo.Rlr_auditdate = ServerDateTime.GetServerDateTime();
                    CurrentLossReportInfo.ListReaLossReportDetail = dtReaLossReportDetail;
                    CurrentLossReportInfo.Rlr_status = "4";
                    //CurrentLossReportInfo.Rlr_applyid = selectDictSysUser1.valueMember;
                    var ret = new ProxyReaLossReport().Service.AuditReaData(Caller, CurrentLossReportInfo);
                }
                else if (dig == DialogResult.No)
                {
                    MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }

            }

            RefreshPatients();
        }

        public List<EntityReaLossReport> GetCheckedReaLossReport()
        {
            gvReaLossReport.CloseEditor();
            this.bsReaLossReport.EndEdit();

            List<EntityReaLossReport> checkList = new List<EntityReaLossReport>();
            var selectIndex = gvReaLossReport.GetSelectedRows();
            foreach (int index in selectIndex)
            {
                checkList.Add(gvReaLossReport.GetRow(index) as EntityReaLossReport);
            }

            if (checkList.Count <= 0
                && CurrentLossReportInfo != null
                && !string.IsNullOrEmpty(CurrentLossReportInfo.Rlr_no))
            {
                checkList.Add(gvReaLossReport.GetFocusedRow() as EntityReaLossReport);
            }

            return checkList;
        }
        bool DeleteLossReport()
        {
            gvReaLossReport.CloseEditor();
            this.bsReaLossReport.EndEdit();

            StringBuilder logMsg = new StringBuilder();
            List<EntityReaLossReport> delReaLossReportList = new List<EntityReaLossReport>();

            int patCount = 0;
            bool delflag = false;

            if (UserInfo.GetSysConfigValue("BathOrSingleDelFlag").Equals("是"))
            {
                List<EntityReaLossReport> dtReaLossReport = GetCheckedReaLossReport();
                if (dtReaLossReport == null) return false;
                delflag = true;

                foreach (var dr in dtReaLossReport)
                {
                    patCount++;

                    if (dr.Rlr_status.ToString() == "1"
                        || dr.Rlr_status.ToString() == "2")
                    {
                        delReaLossReportList.Add(dr);
                    }
                }

            }
            else
            {
                delflag = false;
                EntityReaLossReport dr = this.gvReaLossReport.GetFocusedRow() as EntityReaLossReport;
                if (dr != null)
                {
                    patCount++;
                    if (dr.Rlr_status.ToString() == "1"
                        || dr.Rlr_status.ToString() == "2")
                    {
                        delReaLossReportList.Add(dr);
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

            if (delReaLossReportList.Count < 1)
            {
                lis.client.control.MessageDialog.Show(string.Format("所选数据已审核！"), "提示");
                return false;
            }
            if (delReaLossReportList.Count > 1)
            {
                if (MessageDialog.Show(string.Format("您将要删除 {0} 条病人记录，是否继续？", delReaLossReportList.Count), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }
            else
            {
                object name = delReaLossReportList[0].Rlr_no;
                if (MessageDialog.Show(string.Format("您将要删除 报损单号:{0} 的记录，是否继续？", name != null ? name.ToString() : string.Empty), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.BillPopedomCode.Delete, "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;

                var ret = new ProxyReaLossReport().Service.DeleteReaData(Caller, delReaLossReportList);

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
            DeleteLossReport();
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
            string sid = new ProxyReaLossReport().Service.GetReaSID_MaxPlusOne(dtPatDate, new LossReportStep().StepCode);
            return sid;
        }

        private void addNew()
        {
            CurrentLossReportInfo = new EntityReaLossReport();
            CurrentReaID = "";
            this.txtReaDate.Properties.ReadOnly = false;

            DateTime dtToday = ServerDateTime.GetServerDateTime();

            txtReaDate.EditValue = dtToday;
            //*********************************************
            txtReaSid.Text = getMaxSID();

            fpat_id = "";

            dtReaLossReportDetail = new List<EntityReaLossReportDetail>();

            this.gvReadetail.ClearSelection();

            gcReaDetail.DataSource = dtReaLossReportDetail;
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
            if (dtReaLossReportDetail.Count <= 0)
            {
                MessageDialog.Show($"没有报损明细！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            if (string.IsNullOrEmpty(fpat_id))
            {
                bool check = CheckItems(dtReaLossReportDetail);
                if (check)
                {
                    FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.Audit, "", "");
                    frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                     //验证窗口
                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig == DialogResult.OK)
                    {
                        Caller.IPAddress = UserInfo.ip;
                        Caller.LoginID = frmCheck.OperatorID;
                        Caller.LoginName = frmCheck.OperatorName;
                        Caller.UserID = frmCheck.OperatorID;

                        EntityReaLossReport apply = new EntityReaLossReport();
                        apply.Rlr_no = txtReaSid.Text;
                        apply.Rlr_date = txtReaDate.DateTime;
                        //apply.Rlr_srno = textEdit2.Text;
                        apply.ListReaLossReportDetail = dtReaLossReportDetail;
                        apply.Rlr_status = "1";
                        //apply.Rlr_applyid = selectDictSysUser1.valueMember;
                        var ret = new ProxyReaLossReport().Service.SaveReaData(Caller, apply);
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
                if (CurrentLossReportInfo.Rlr_status == "1" || CurrentLossReportInfo.Rlr_status == "2")
                {
                    bool check = CheckItems(dtReaLossReportDetail);
                    if (check)
                    {
                        FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.Audit, "", "");
                        frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                         //验证窗口
                        DialogResult dig = frmCheck.ShowDialog();
                        if (dig == DialogResult.OK)
                        {
                            Caller.IPAddress = UserInfo.ip;
                            Caller.LoginID = frmCheck.OperatorID;
                            Caller.LoginName = frmCheck.OperatorName;
                            Caller.UserID = frmCheck.OperatorID;

                            CurrentLossReportInfo.Rlr_date = txtReaDate.DateTime;
                            CurrentLossReportInfo.ListReaLossReportDetail = dtReaLossReportDetail;
                            CurrentLossReportInfo.Rlr_status = "1";
                            //CurrentLossReportInfo.Rlr_applyid = selectDictSysUser1.valueMember;

                            var ret = new ProxyReaLossReport().Service.UpdateReaData(Caller, CurrentLossReportInfo);
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

        public bool CheckItems(List<EntityReaLossReportDetail> list)
        {
            bool check = false;
            foreach (var item in dtReaLossReportDetail)
            {
                if (string.IsNullOrEmpty(item.Rld_no))
                {
                    MessageDialog.Show("报损单号不能为空！", "提示");
                    check = false;
                    break;
                }
                if (string.IsNullOrEmpty(item.Rld_reaid))
                {
                    MessageDialog.Show("试剂信息不能为空！", "提示");
                    check = false;
                    break;
                }

                if (string.IsNullOrEmpty(item.Rld_barcode))
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
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load("FrmReagenLossReport", string.Empty, UserInfo.loginID);
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
            drSearchBarDDL["name"] = "报损单号";
            drSearchBarDDL["value"] = "rea_sid";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            this.cmbBarSearchPatType.Properties.DropDownRows = dtSearchBarDDL.Rows.Count;
            this.cmbBarSearchPatType.Properties.DataSource = dtSearchBarDDL;
            this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            this.cmbBarSearchPatType.Properties.ValueMember = "value";



            #endregion

            #region 整行显示字体颜色 2018-05-11
            for (int i = 0; i < gvReaLossReport.Columns.Count; i++)
            {
                string columnName = gvReaLossReport.Columns[i].FieldName;
                if (columnName != "PatSelect" && columnName != "pat_icon")
                {
                    //未审核
                    FormatRowLossReportTo(gvReaLossReport, columnName, LIS_Const.REAGENT_FLAG.Natural, setting.PatListPanel.BackColorNormal, setting.PatListPanel.BackColorNormal, setting.PatListPanel.ForeColorNormal);
                    //已审核
                    FormatRowLossReportTo(gvReaLossReport, columnName, LIS_Const.REAGENT_FLAG.Audited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.ForeColorAudited);
                    //未通过
                    FormatRowLossReportTo(gvReaLossReport, columnName, LIS_Const.REAGENT_FLAG.Returned, setting.PatListPanel.BackColorReturn, setting.PatListPanel.BackColorReturn, setting.PatListPanel.ForeColorReturn);
                    //已完成
                    FormatRowLossReportTo(gvReaLossReport, columnName, LIS_Const.REAGENT_FLAG.Done, setting.PatListPanel.BackColorDone, setting.PatListPanel.BackColorDone, setting.PatListPanel.ForeColorDone);

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

        private void FormatRowLossReportTo(GridView gridView, string fieldName, string auditedValue, Color backColorAudited, Color backColor, Color foreColor)
        {
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridView.Columns["Rlr_status"];
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
            if (this.dtReaLossReportDetail != null)
            {
                this.dtReaLossReportDetail = this.dtReaLossReportDetail.OrderBy(i => i.ReagentName).ToList();

                CloseEditor();

                this.gvReadetail.ClearGrouping();
                this.gvReadetail.ClearSorting();
                this.gcReaDetail.DataSource = this.dtReaLossReportDetail;

                gvReadetail.RefreshData();
                gcReaDetail.RefreshDataSource();
            }
        }
        private void BindLookupData()
        {
            this.gcReaDetail.DataSource = this.dtReaLossReportDetail;
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
            var list = new ProxyReaLossReport().Service.ReaQuery(qc);

            EntityReaQC reaQc = new EntityReaQC();
            if (true)
            {
                qc.DateStart = dtBegin.DateTime.Date;
                qc.DateEnd = dtEnd.DateTime.Date.AddDays(1).AddSeconds(-1);
            }

            qc.ReaStatus = "4";

            AllReaList = list;
            List<EntityReaLossReport> dtOldRea = bsReaLossReport.DataSource as List<EntityReaLossReport>;

            bsReaLossReport.ResetBindings(false);

            bsReaLossReport.DataSource = list;

            gcReaLossReport.RefreshDataSource();
            gvReaLossReport.RefreshData();

            RefreshItemsCount();//更新记录
            this.fpat_id = "";
            RapitSearch();
            isLoadData = true;
        }
        private void RapitSearch()
        {
            List<EntityReaLossReport> ReaList = AllReaList;
            if (AllReaList.Count == 0) return;

            if (this.cmbBarSearchPatType.EditValue != null
                && this.cmbBarSearchPatType.EditValue.ToString().Trim(null) != string.Empty
                && this.txtBarSearchCondition.Text.Trim(null) != string.Empty)
            {
                string searchField = this.cmbBarSearchPatType.EditValue.ToString();
                string searchValue = this.txtBarSearchCondition.Text;


                //条码号
                if (searchField == "rea_sid")
                {
                    ReaList = ReaList.Where(i => i.Rlr_no.Contains(searchValue)).ToList();
                }

            }
            else
            {
                ReaList = AllReaList;

            }
            bsReaLossReport.DataSource = ReaList;

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
            if (bsReaLossReport.Current != null)
            {
                EntityReaLossReport drLst = (EntityReaLossReport)bsReaLossReport.Current;

                if (drLst != null && !string.IsNullOrEmpty(drLst.Rlr_no))
                {
                    string pid = drLst.Rlr_no;
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

            if (CurrentLossReportInfo == null || string.IsNullOrEmpty(CurrentLossReportInfo.Rlr_no))
                return;

            fpat_id = CurrentLossReportInfo.Rlr_no;
            var dr = CurrentLossReportInfo;
            txtReaSid.Text = CurrentLossReportInfo.Rlr_no;
            txtReaDate.DateTime = CurrentLossReportInfo.Rlr_date ?? DateTime.MinValue;
            //textEdit2.Text = CurrentLossReportInfo.Rlr_srno;
            //selectDictSysUser1.valueMember = CurrentLossReportInfo.Rlr_applyid;

            if (CurrentLossReportInfo.Rlr_status == "2" && !string.IsNullOrEmpty(CurrentLossReportInfo.Rlr_returnreason))
            {
                layoutControlItem4.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                txtReason.Visible = true;
                txtReason.Text = CurrentLossReportInfo.Rlr_returnreason;
            }
        }

        //根据ID得到数据
        private void GetReaDetailData(string _pat_id)
        {
            isDataChaged = false;

            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = _pat_id;
            var resList = new ProxyReaLossReportDetail().Service.GetDetail(qc);
            dtReaLossReportDetail = resList;
            CurrentLossReportInfo.ListReaLossReportDetail = dtReaLossReportDetail;
            BindGrid();
        }
        bool AnPatientChanging(string prev_patid, string pat_id, EntityReaLossReport drPat)
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

            if (this.bsReaLossReport.DataSource != null)
            {
                List<EntityReaLossReport> dtpat = this.bsReaLossReport.DataSource as List<EntityReaLossReport>;
                if (dtpat != null)
                {
                    countTotal = dtpat.Count;

                    foreach (var item in dtpat)
                    {
                        if (item.Rlr_status != null)
                        {
                            if (item.Rlr_status.ToString() == "4")
                            {
                                countAudited++;
                            }
                            else if (item.Rlr_status.ToString() == "0")
                            {
                                countUnAudited++;
                            }
                            else if (item.Rlr_status.ToString() == "2")
                            {
                                countReturned++;
                            }
                        }
                        if (item.Rlr_printflag.ToString() == "1")
                        {
                            countPrinted++;
                        }
                    }
                }
            }
            this.lbRecordCount.Text = string.Format("总数：{0} 已审核：{1} 未审核：{2} 已打印：{3} 未通过：{4}", countTotal, countAudited, countUnAudited, countPrinted, countReturned);
        }
        private void FrmReagenLossReport_Load(object sender, EventArgs e)
        {
            List<EntityDicPubEvaluate> listEvaluate = CacheClient.GetCache<EntityDicPubEvaluate>().
                FindAll(i => i.EvaFlag == "24").OrderBy(i => i.EvaSortNo).ToList();
            bsValue.DataSource = listEvaluate;
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
            sysToolBar1.BtnAnswer.Caption = "撤销报损";
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

        private void gvReaLossReport_Click(object sender, EventArgs e)
        {
            if (bsReaLossReport.Current != null)
            {
                EntityReaLossReport drLst = (EntityReaLossReport)bsReaLossReport.Current;

                CurrentLossReportInfo = drLst;
                CurrentReaID = drLst.Rlr_no;
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
            ProxyReaLossReport mainProxy = new ProxyReaLossReport();
            List<EntityReaLossReport> list = mainProxy.Service.ReaQuery(qc);

            if (list != null && list.Count > 0 && list[0].Rlr_status != "0" && list[0].Rlr_status != "2")
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
                List<EntityReaLossReportDetail> selectDataRows = new List<EntityReaLossReportDetail>();
                bool needComma = false;
                string tipItemsText = string.Empty;
                foreach (int rowHandler in selectedRowHandler)
                {
                    EntityReaLossReportDetail row = sourceGrid.GetRow(rowHandler) as EntityReaLossReportDetail;
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
        public void RemoveItem(List<EntityReaLossReportDetail> rowsPatResultItem, bool removeHasResult)
        {
            bool deleteInDatabase = false;
            if (UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除")
            {
                deleteInDatabase = true;
            }

            for (int i = 0; i < rowsPatResultItem.Count; i++)
            {
                EntityReaLossReportDetail drPatResultItem = rowsPatResultItem[i];
                //是否已录入结果
                bool hasResult = true;
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
                        if (!Compare.IsEmpty(drPatResultItem.Rld_no) && !Compare.IsEmpty(drPatResultItem.ObrSn))
                        {
                            if (deleteInDatabase)
                            {

                                EntityLogLogin logLogin = new EntityLogLogin();
                                logLogin.LogIP = UserInfo.ip;
                                logLogin.LogLoginID = UserInfo.loginID;

                                string rsdno = drPatResultItem.Rld_no.ToString();
                                string rea_name = drPatResultItem.ReagentName.TrimEnd().ToString();
                                string rea_itm_id = string.Empty;
                                if (!Compare.IsEmpty(drPatResultItem.Rld_reaid))
                                {
                                    rea_itm_id = drPatResultItem.Rld_reaid.ToString();
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
                                    opResult = new ProxyReaLossReportDetail().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), rsdno);
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    this.dtReaLossReportDetail.Remove(drPatResultItem);
                                }


                                if (!opResult)
                                {
                                    MessageDialog.Show(string.Format("数据库记录删除[{0}]失败！", rea_name), "错误");
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    int deleteIndex = dtReaLossReportDetail.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                    if (deleteIndex > -1)
                                        this.dtReaLossReportDetail.RemoveAt(deleteIndex);
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
                            this.dtReaLossReportDetail.Remove(drPatResultItem);
                            i--;
                        }
                    }
                    else
                    {
                        if (!recordInDataBase)
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                            this.dtReaLossReportDetail.Remove(drPatResultItem);
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(drPatResultItem.Rld_no) && drPatResultItem.ObrSn != 0)
                    {
                        if (deleteInDatabase)
                        {
                            EntityLogLogin logLogin = new EntityLogLogin();
                            logLogin.LogIP = UserInfo.ip;
                            logLogin.LogLoginID = UserInfo.loginID;

                            string resid = drPatResultItem.Rld_no.ToString();
                            string res_itm_ecd = drPatResultItem.ReagentName.TrimEnd().ToString();
                            string res_itm_id = string.Empty;
                            if (!Compare.IsEmpty(drPatResultItem.Rld_reaid))
                            {
                                res_itm_id = drPatResultItem.Rld_reaid.ToString();
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
                                opResult = new ProxyReaLossReportDetail().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
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

                                int deleteIndex = dtReaLossReportDetail.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                if (deleteIndex > -1)
                                    this.dtReaLossReportDetail.RemoveAt(deleteIndex);
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
                        this.dtReaLossReportDetail.Remove(drPatResultItem);
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
                if (dtReaLossReportDetail != null && dtReaLossReportDetail.Count > 0)
                {
                    foreach (var detail in dtReaLossReportDetail)
                    {
                        if (string.Equals(txtPatID.Text, detail.Rld_barcode))
                        {
                            lis.client.control.MessageDialog.Show("当前记录已被添加至报损列表", "提示");
                            txtPatID.Text = string.Empty;
                            return;
                        }
                    }
                }
                EntityReaQC qc = new EntityReaQC();
                qc.Barcode = txtPatID.Text;

                var stoItem = new ProxyReaStorageDetail().Service.GetDetail(qc);
                if (stoItem != null && stoItem.Count > 0)
                {
                    //if (stoItem[0].Rsd_status == "2")
                    //{
                    //    lis.client.control.MessageDialog.Show("当前记录已经报损", "提示");
                    //    txtPatID.Text = string.Empty;
                    //    return;
                    //}
                    if (stoItem[0].Rsd_count == 0)
                    {
                        lis.client.control.MessageDialog.Show("当前条码已无库存", "提示");
                        txtPatID.Text = string.Empty;
                        return;
                    }
                    //if (stoItem[0].Rsd_status == "1")
                    //{
                    //    lis.client.control.MessageDialog.Show("当前记录已经出库，请先撤销出库", "提示");
                    //    txtPatID.Text = string.Empty;
                    //    return;
                    //}
                    EntityReaLossReportDetail detail = new EntityReaLossReportDetail();
                    List<EntityReaSetting> setList = CacheClient.GetCache<EntityReaSetting>();
                    EntityReaSetting setting = setList.Find(m => m.Drea_id == stoItem[0].Rsd_reaid);
                    detail.del_flag = 0;
                    detail.IsNew = 1;

                    detail.Rld_no = txtReaSid.Text;

                    detail.package = stoItem[0].Rsd_package;
                    detail.con_id = stoItem[0].Rsd_conid;
                    detail.pos_id = stoItem[0].Rsd_posid;
                    detail.pdt_id = stoItem[0].Rsd_pdtid;
                    detail.unit_id = stoItem[0].Rsd_unitid;
                    detail.sup_id = stoItem[0].Rsd_supid;
                    detail.grp_id = stoItem[0].Rsd_groupid;

                    detail.Rld_reaid = stoItem[0].Rsd_reaid;
                    detail.ReagentName = stoItem[0].ReagentName;
                    detail.ReagentPackage = stoItem[0].Rsd_package;
                    detail.Rld_reacount = 1;
                    detail.StoreNo = stoItem[0].Rsd_no;
                    //detail.Rld_count = 1;
                    detail.Rld_barcode = txtPatID.Text;
                    if (stoItem[0].Rsd_status == "0")
                    {
                        detail.Rld_barstatus = 0;
                    }
                    else if (stoItem[0].Rsd_status == "1")
                    {
                        detail.Rld_barstatus = 1;
                    }
                    this.dtReaLossReportDetail.Add(detail);

                    BindLookupData();

                    txtPatID.Text = string.Empty;
                }
                else
                {
                    lis.client.control.MessageDialog.Show("找不到相应记录", "提示");
                    txtPatID.Text = string.Empty;
                    return;
                }
            }
        }
    }
}