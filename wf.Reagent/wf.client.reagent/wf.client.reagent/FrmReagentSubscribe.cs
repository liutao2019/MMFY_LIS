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
    public partial class FrmReagentSubscribe : FrmCommon, IPatPanelConfig
    {
        List<EntityDCLPrintParameter> listPrintData_multithread;
        bool useMultiThread = false;
        /// <summary>
        /// 当前申购信息
        /// </summary>
        EntityReaSubscribe CurrentInfo = new EntityReaSubscribe();//病人消息
        /// <summary>
        /// 面板配置类
        /// </summary>
        PatInputRuntimeSetting UserCustomSetting = null;
        /// <summary>
        /// 新增时获得焦点的控件
        /// </summary>
        Control FocusOnAddNewControl = null;
        List<EntityReaSubscribe> AllReaList = new List<EntityReaSubscribe>();
        private String fpat_id = "";//主键
        private string CurrentReaID;
        bool isLoadData = false;
        bool isDataChaged = false;
        /// <summary>
        /// 试剂申购表
        /// </summary>
        List<EntityReaSubscribeDetail> dtReaSubscribeDetail = new List<EntityReaSubscribeDetail>();
        public FrmReagentSubscribe()
        {
            InitializeComponent();
            this.roundPanelGroup1.RoundPanelGroupClick += this.roundPanelGroup1_Click;
            this.reagentEditor1.ReagentAdded += ReagentEditor1_ReagentAdded;
            this.reagentEditor1.ReagentRemoved += ReagentEditor1_ReagentRemoved;
            this.sysToolBar1.OnCloseClicked += SysToolBar1_OnCloseClicked;
            this.sysToolBar1.OnBtnSaveClicked += SysToolBar1_OnBtnSaveClicked;
            this.sysToolBar1.OnBtnAddClicked += SysToolBar1_OnBtnAddClicked;
            this.sysToolBar1.OnBtnRefreshClicked += SysToolBar1_OnBtnRefreshClicked;
            this.sysToolBar1.OnBtnDeleteClicked += SysToolBar1_OnBtnDeleteClicked;
            this.sysToolBar1.OnAuditClicked += SysToolBar1_OnAuditClicked;
            this.sysToolBar1.BtnUndoClick += SysToolBar1_BtnUndoClick;
            this.sysToolBar1.BtnAnswerClick += SysToolBar1_BtnAnswerClick;
            this.sysToolBar1.OnBtnPrintClicked += SysToolBar1_OnBtnPrintClicked;
            this.sysToolBar1.OnPrintPreviewClicked += SysToolBar1_OnPrintPreviewClicked;

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
            string prtTemplate = "ReagentSubscribe";

            this.BeginLoading();
            List<string> listPatID = new List<string>();
            StringBuilder sbPatSidWhere2 = new StringBuilder();
            if (pat_id == null)
            {
                List<EntityReaSubscribe> listPatient = GetCheckedReaSubscribe();
                if (listPatient.Count > 0)
                {
                    foreach (EntityReaSubscribe dr in listPatient)
                    {
                        if (isPreview)
                        {
                            listPatID.Add(dr.Rsb_no.ToString());
                        }
                        else
                        {
                            if (dr.Rsb_status.ToString() == LIS_Const.REAGENT_FLAG.Audited)
                            {
                                if (string.IsNullOrEmpty(dr.AuditorName))
                                {
                                    sbPatSidWhere2.Append(string.Format(",[{0}]", dr.Rsb_no));
                                    continue;
                                }
                                listPatID.Add(dr.Rsb_no.ToString());

                            }
                        }
                    }

                    if (sbPatSidWhere2.Length > 0)
                    {
                        MessageDialog.Show("单号为:" + sbPatSidWhere2.Remove(0, 1) + " 的申购单无审核者信息，需重新审核", "提示");
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
                printPara.CustomParameter.Add("SubscribeId", patient_id);
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
                    this.SelectAllSubscribeInGrid(false);
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
        public void SelectAllSubscribeInGrid(bool selectAll)
        {
            //在全部勾选前把焦点行设置成-1解决：全选的时候焦点行会显示不了勾
            if (selectAll)
            {
                gvReaInfo.SelectAll();
            }
            else
                gvReaInfo.ClearSelection();
            this.gcReaInfo.RefreshDataSource();
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
                new ProxyReaSubscribe().Service.UpdatePrintState_whitOperator(listPatID, UserInfo.loginID, UserInfo.userName, strPlace);
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
            if (GetCheckedReaSubscribe().Count <= 0)
            {
                return;
            }
            Print(true);
        }

        private void SysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (GetCheckedReaSubscribe().Count <= 0)
            {
                return;
            }
            Print(false);
            RefreshPatients();
        }

        private void SysToolBar1_BtnAnswerClick(object sender, EventArgs e)
        {
            if (CurrentInfo == null || string.IsNullOrEmpty(CurrentInfo.Rsb_no))
            {
                MessageDialog.Show("请勾选你要回退数据！", "提示");
                return;
            }
            if (CurrentInfo.Rsb_status == "4")
            {
                MessageDialog.Show("该记录已审核！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            FrmReaCheckPassword frmCheck = new FrmReaCheckPassword("身份验证 - 回退", LIS_Const.ReagentPopedomCode.SubscribeReturn, "", "");
            DialogResult dig = frmCheck.ShowDialog();

            if (dig == DialogResult.OK)
            {
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;
                Caller.UserID = frmCheck.OperatorID;
                CurrentInfo.Rsb_returnreason = frmCheck.ReturnReason;
                new ProxyReaSubscribe().Service.ReturnReaData(Caller, CurrentInfo);

            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            RefreshPatients();
        }

        private void SysToolBar1_BtnUndoClick(object sender, EventArgs e)
        {
            if (CurrentInfo == null || string.IsNullOrEmpty(CurrentInfo.Rsb_no))
            {
                MessageDialog.Show("请勾选你要取消审核数据！", "提示");
                return;
            }
            if (CurrentInfo.Rsb_status != "4")
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

                CurrentInfo.Rsb_auditor = string.Empty;
                CurrentInfo.Rsb_auditdate = null;
                CurrentInfo.ListReaSubscribeDetail = dtReaSubscribeDetail;
                CurrentInfo.Rsb_status = "1";

                var ret = new ProxyReaSubscribe().Service.UpdateReaData(Caller, CurrentInfo);

            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            RefreshPatients();
        }

        private void SysToolBar1_OnAuditClicked(object sender, EventArgs e)
        {
            if (CurrentInfo == null || string.IsNullOrEmpty(CurrentInfo.Rsb_no))
            {
                MessageDialog.Show("请勾选你要审核数据！", "提示");
                return;
            }
            if (CurrentInfo.Rsb_status == "4")
            {
                MessageDialog.Show("该记录已审核！", "提示");
                return;
            }
            if (dtReaSubscribeDetail.Count <= 0)
            {
                MessageDialog.Show($"没有申购明细！", "提示");
                return;
            }

            bool checkcount = false;
            foreach (var item in dtReaSubscribeDetail)
            {
                if (item.Rsbd_reacount <= 0)
                {
                    checkcount = false;
                    MessageDialog.Show($"{item.ReagentName}申购数量不为正，不能进行当前操作！", "提示");
                    return;
                }
                else
                {
                    checkcount = true;
                }
            }
            if (checkcount)
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

                    CurrentInfo.Rsb_auditor = Caller.UserID;
                    CurrentInfo.Rsb_auditdate = ServerDateTime.GetServerDateTime();
                    CurrentInfo.ListReaSubscribeDetail = dtReaSubscribeDetail;
                    CurrentInfo.Rsb_status = "4";
                    var ret = new ProxyReaSubscribe().Service.AuditReaData(Caller, CurrentInfo);

                }
                else if (dig == DialogResult.No)
                {
                    MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }
            }

            RefreshPatients();
        }

        public List<EntityReaSubscribe> GetCheckedReaSubscribe()
        {
            gvReaInfo.CloseEditor();
            this.bsReaInfo.EndEdit();

            List<EntityReaSubscribe> checkList = new List<EntityReaSubscribe>();
            var selectIndex = gvReaInfo.GetSelectedRows();
            foreach (int index in selectIndex)
            {
                checkList.Add(gvReaInfo.GetRow(index) as EntityReaSubscribe);
            }

            if (checkList.Count <= 0
                && CurrentInfo != null
                && !string.IsNullOrEmpty(CurrentInfo.Rsb_no))
            {
                checkList.Add(gvReaInfo.GetFocusedRow() as EntityReaSubscribe);
            }

            return checkList;
        }
        bool DeleteSubscribe()
        {
            gvReaInfo.CloseEditor();
            this.bsReaInfo.EndEdit();

            StringBuilder logMsg = new StringBuilder();
            List<EntityReaSubscribe> delReaSubscribeList = new List<EntityReaSubscribe>();

            int patCount = 0;
            bool delflag = false;

            if (UserInfo.GetSysConfigValue("BathOrSingleDelFlag").Equals("是"))
            {
                List<EntityReaSubscribe> dtReaSubscribe = GetCheckedReaSubscribe();
                if (dtReaSubscribe == null) return false;
                delflag = true;

                foreach (var dr in dtReaSubscribe)
                {
                    patCount++;

                    if (dr.Rsb_status.ToString() == "1"
                        || dr.Rsb_status.ToString() == "2")
                    {
                        delReaSubscribeList.Add(dr);
                    }
                }

            }
            else
            {
                delflag = false;
                EntityReaSubscribe dr = this.gvReaInfo.GetFocusedRow() as EntityReaSubscribe;
                if (dr != null)
                {
                    patCount++;
                    if (dr.Rsb_status.ToString() == "1"
                        || dr.Rsb_status.ToString() == "2")
                    {
                        delReaSubscribeList.Add(dr);
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

            if (delReaSubscribeList.Count < 1)
            {
                lis.client.control.MessageDialog.Show(string.Format("所选数据已审核！"), "提示");
                return false;
            }
            if (delReaSubscribeList.Count > 1)
            {
                if (MessageDialog.Show(string.Format("您将要删除 {0} 条病人记录，是否继续？", delReaSubscribeList.Count), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }
            else
            {
                object name = delReaSubscribeList[0].Rsb_no;
                if (MessageDialog.Show(string.Format("您将要删除 申购单号:{0} 的记录，是否继续？", name != null ? name.ToString() : string.Empty), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.ReagentPopedomCode.SubscribeDelete, "", "");

            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {
                EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                Caller.IPAddress = UserInfo.ip;
                Caller.LoginID = frmCheck.OperatorID;
                Caller.LoginName = frmCheck.OperatorName;

                var ret = new ProxyReaSubscribe().Service.DeleteReaData(Caller, delReaSubscribeList);

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
            DeleteSubscribe();
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
            string sid = new ProxyReaSubscribe().Service.GetReaSID_MaxPlusOne(dtPatDate, new SubscribeStep().StepCode);
            return sid;
        }

        private void addNew()
        {
            CurrentInfo = new EntityReaSubscribe();
            CurrentReaID = "";
            this.txtReaDate.Properties.ReadOnly = false;

            DateTime dtToday = ServerDateTime.GetServerDateTime();

            txtReaDate.EditValue = dtToday;
            //*********************************************
            txtReaSid.Text = getMaxSID();

            fpat_id = "";
            dtReaSubscribeDetail = new List<EntityReaSubscribeDetail>();

            if (reagentEditor1.listReaDetail != null)
                reagentEditor1.listReaDetail.Clear();//清空原来的
            reagentEditor1.RefreshEditBoxText();

            this.gvReadetail.ClearSelection();

            gcReaDetail.DataSource = dtReaSubscribeDetail;
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
            if (dtReaSubscribeDetail.Count <= 0)
            {
                MessageDialog.Show($"没有申购明细！", "提示");
                return;
            }
            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;
            if (string.IsNullOrEmpty(fpat_id))
            {
                bool checkcount = false;
                foreach (var item in dtReaSubscribeDetail)
                {
                    if (item.Rsbd_reacount <= 0)
                    {
                        checkcount = false;
                        MessageDialog.Show($"{item.ReagentName}申购数量不为正，不能进行当前操作！", "提示");
                        return;
                    }
                    else
                    {
                        checkcount = true;
                    }
                }
                if (checkcount)
                {
                    FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.SubscribeSave, "", "");
                    frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                     //验证窗口
                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig == DialogResult.OK)
                    {
                        Caller.IPAddress = UserInfo.ip;
                        Caller.LoginID = frmCheck.OperatorID;
                        Caller.LoginName = frmCheck.OperatorName;
                        Caller.UserID = frmCheck.OperatorID;

                        EntityReaSubscribe Subscribe = new EntityReaSubscribe();
                        Subscribe.Rsb_no = txtReaSid.Text;
                        Subscribe.Rsb_date = txtReaDate.DateTime;
                        Subscribe.ListReaSubscribeDetail = dtReaSubscribeDetail;
                        Subscribe.Rsb_status = "1";
                        var ret = new ProxyReaSubscribe().Service.SaveReaData(Caller, Subscribe);
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
                if (CurrentInfo.Rsb_status == "1" || CurrentInfo.Rsb_status == "2")
                {
                    bool checkcount = false;
                    foreach (var item in dtReaSubscribeDetail)
                    {
                        if (item.Rsbd_reacount <= 0)
                        {
                            checkcount = false;
                            MessageDialog.Show($"{item.ReagentName}申购数量不为正，不能进行当前操作！", "提示");
                            return;
                        }
                        else
                        {
                            checkcount = true;
                        }
                    }
                    if (checkcount)
                    {
                        FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 保存", LIS_Const.ReagentPopedomCode.SubscribeSave, "", "");
                        frmCheck.operationCode = EnumOperationCode.Audit;//保存用以取消时统一身份验证
                                                                         //验证窗口
                        DialogResult dig = frmCheck.ShowDialog();
                        if (dig == DialogResult.OK)
                        {
                            Caller.IPAddress = UserInfo.ip;
                            Caller.LoginID = frmCheck.OperatorID;
                            Caller.LoginName = frmCheck.OperatorName;
                            Caller.UserID = frmCheck.OperatorID;

                            CurrentInfo.Rsb_date = txtReaDate.DateTime;
                            CurrentInfo.ListReaSubscribeDetail = dtReaSubscribeDetail;
                            CurrentInfo.Rsb_status = "1";
                            var ret = new ProxyReaSubscribe().Service.UpdateReaData(Caller, CurrentInfo);

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

        private void SysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReagentEditor1_ReagentRemoved(object sender, string com_id)
        {
            bool deleteRes = false;

            if (UserInfo.GetSysConfigValue("Lab_ShowDeleteReaItem") == "是")
            {
                deleteRes = MessageDialog.Show(string.Format("您确定要删除该试剂信息？"), "提示",
                                                MessageBoxButtons.YesNo) == DialogResult.Yes;
            }
            RemoveCombineItems(com_id, true);
        }

        /// <summary>
        /// 关闭编辑单元格并更新数据
        /// </summary>
        private void CloseEditor()
        {
            this.gvReadetail.CloseEditor();
        }
        /// <summary>
        /// 添加试剂信息
        /// </summary>
        /// <param name="com_id">试剂ID</param>
        private void AddReaItems(string com_id, int com_seq)
        {
            CloseEditor();

            //根据组合ID获取组合信息
            EntityReaSetting drRea = DictReagent.Instance.GetReaByID(com_id);

            if (drRea != null)
            {
                EntityReaSubscribeDetail drReaSubscribeDetail = new EntityReaSubscribeDetail();
                drReaSubscribeDetail.Rsbd_no = txtReaSid.Text;
                drReaSubscribeDetail.ReagentName = drRea.Drea_name;
                drReaSubscribeDetail.ReagentPackage = drRea.Drea_package;
                drReaSubscribeDetail.Rsbd_reaid = drRea.Drea_id;
                drReaSubscribeDetail.Rsbd_status = "0";
                drReaSubscribeDetail.package = drRea.Drea_package;
                drReaSubscribeDetail.con_id = drRea.Drea_condition;
                drReaSubscribeDetail.grp_id = drRea.Drea_group;
                drReaSubscribeDetail.pdt_id = drRea.Drea_product;
                drReaSubscribeDetail.pos_id = drRea.Drea_position;
                drReaSubscribeDetail.sup_id = drRea.Drea_supplier;
                drReaSubscribeDetail.unit_id = drRea.Drea_unit;
                drReaSubscribeDetail.IsNew = 1;
                dtReaSubscribeDetail.Add(drReaSubscribeDetail);
            }
        }
        /// <summary>
        /// 加载用户式样配置
        /// </summary>
        private void LoadUserSetting()
        {
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load("FrmReagentSubscribe", string.Empty, UserInfo.loginID);
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
            drSearchBarDDL["name"] = "申购单号";
            drSearchBarDDL["value"] = "rea_sid";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            this.cmbBarSearchPatType.Properties.DropDownRows = dtSearchBarDDL.Rows.Count;
            this.cmbBarSearchPatType.Properties.DataSource = dtSearchBarDDL;
            this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            this.cmbBarSearchPatType.Properties.ValueMember = "value";



            #endregion

            #region 整行显示字体颜色 2018-05-11
            for (int i = 0; i < gvReaInfo.Columns.Count; i++)
            {
                string columnName = gvReaInfo.Columns[i].FieldName;
                if (columnName != "PatSelect" && columnName != "pat_icon")
                {
                    //未审核
                    FormatRowSubscribeTo(gvReaInfo, columnName, LIS_Const.REAGENT_FLAG.Natural, setting.PatListPanel.BackColorNormal, setting.PatListPanel.BackColorNormal, setting.PatListPanel.ForeColorNormal);
                    //已审核
                    FormatRowSubscribeTo(gvReaInfo, columnName, LIS_Const.REAGENT_FLAG.Audited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.BackColorAudited, setting.PatListPanel.ForeColorAudited);
                    //未通过
                    FormatRowSubscribeTo(gvReaInfo, columnName, LIS_Const.REAGENT_FLAG.Returned, setting.PatListPanel.BackColorReturn, setting.PatListPanel.BackColorReturn, setting.PatListPanel.ForeColorReturn);
                    //已完成
                    FormatRowSubscribeTo(gvReaInfo, columnName, LIS_Const.REAGENT_FLAG.Done, setting.PatListPanel.BackColorDone, setting.PatListPanel.BackColorDone, setting.PatListPanel.ForeColorDone);

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

        private void FormatRowSubscribeTo(GridView gridView, string fieldName, string auditedValue, Color backColorAudited, Color backColor, Color foreColor)
        {
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridView.Columns["Rsb_status"];
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
        /// 移除组合
        /// </summary>
        /// <param name="com_id"></param>
        public void RemoveCombineItems(string com_id, bool removeHasResult)
        {
            CloseEditor();
            EntityLogLogin logLogin = new EntityLogLogin();
            logLogin.LogIP = UserInfo.ip;
            logLogin.LogLoginID = UserInfo.loginID;

            string Rsbno = CurrentInfo.Rsb_no;

            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = Rsbno;
            qc.ReaId = com_id;

            bool opResult = false;
            opResult = new ProxyReaSubscribeDetail().Service.DeleteCommonDetail(logLogin, qc);
            if (!opResult)
            {
                MessageDialog.Show(string.Format("删除[{0}]失败！", com_id), "错误");
            }
            int deleteIndex = dtReaSubscribeDetail.FindIndex(w => w.Rsbd_reaid == com_id);
            if (deleteIndex > -1)
                this.dtReaSubscribeDetail.RemoveAt(deleteIndex);
            //重新绑定
            BindGrid();
        }
        private void ReagentEditor1_ReagentAdded(object sender, string com_id, int com_seq)
        {
            if (string.IsNullOrEmpty(txtReaSid.Text))
            {
                lis.client.control.MessageDialog.Show("申购单号不能为空！", "提示");
                txtReaSid.Focus();
                return;
            }
            if (new ProxyReaSubscribe().Service.ExsitSid(txtReaSid.Text, txtReaDate.DateTime))
            {
                lis.client.control.MessageDialog.Show("已存在该申购单号，请重新修改！", "提示");
                txtReaSid.Focus();
                return;
            }
            CloseEditor();
            AddReaItems(com_id, com_seq);
            BindGrid();
        }

        /// <summary>
        /// 绑定
        /// </summary>
        private void BindGrid()
        {
            if (this.dtReaSubscribeDetail != null)
            {
                this.dtReaSubscribeDetail = this.dtReaSubscribeDetail.OrderBy(i => i.ReagentName).ToList();

                CloseEditor();

                this.gvReadetail.ClearGrouping();
                this.gvReadetail.ClearSorting();
                this.gcReaDetail.DataSource = this.dtReaSubscribeDetail;

                gvReadetail.RefreshData();
                gcReaDetail.RefreshDataSource();
            }
        }
        private void BindLookupData()
        {
            this.gcReaDetail.DataSource = this.dtReaSubscribeDetail;
            gvReadetail.RefreshData();
            gcReaDetail.RefreshDataSource();
        }
        private void SearchPatientsAndAddNew()
        {
            GetReaList(false);

            if (reagentEditor1.listReaDetail != null)
                reagentEditor1.listReaDetail.Clear();

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
            else if (strPatFlag == "9")//已完成
            {
                qc.ReaStatus = "9";
            }
            var list = new ProxyReaSubscribe().Service.ReaQuery(qc);

            AllReaList = list;

            List<EntityReaSubscribe> dtOldRea = bsReaInfo.DataSource as List<EntityReaSubscribe>;

            bsReaInfo.ResetBindings(false);

            bsReaInfo.DataSource = list;

            gcReaInfo.RefreshDataSource();
            gvReaInfo.RefreshData();

            RefreshItemsCount();//更新记录
            this.fpat_id = "";
            RapitSearch();
            isLoadData = true;
        }
        private void RapitSearch()
        {
            List<EntityReaSubscribe> ReaList = AllReaList;
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
                    ReaList = ReaList.Where(i => i.Rsb_no.Contains(searchValue)).ToList();
                }
            }
            else
            {
                ReaList = AllReaList;
            }
            bsReaInfo.DataSource = ReaList;
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
            if (bsReaInfo.Current != null)
            {
                EntityReaSubscribe drLst = (EntityReaSubscribe)bsReaInfo.Current;

                if (drLst != null && !string.IsNullOrEmpty(drLst.Rsb_no))
                {
                    string pid = drLst.Rsb_no;
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

            if (CurrentInfo == null || string.IsNullOrEmpty(CurrentInfo.Rsb_no))
                return;

            fpat_id = CurrentInfo.Rsb_no;
            var dr = CurrentInfo;
            txtReaSid.Text = CurrentInfo.Rsb_no;
            txtReaDate.DateTime = CurrentInfo.Rsb_date ?? DateTime.MinValue;

            if (CurrentInfo.Rsb_status == "2" && !string.IsNullOrEmpty(CurrentInfo.Rsb_returnreason))
            {
                textEdit1.Visible = true;
                textEdit1.Text = CurrentInfo.Rsb_returnreason;
            }
        }
        //根据ID得到数据
        private void GetReaDetailData(string _pat_id)
        {
            isDataChaged = false;

            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = _pat_id;
            var resList = new ProxyReaSubscribeDetail().Service.GetDetail(qc);
            if (reagentEditor1.listReaDetail != null)
                reagentEditor1.listReaDetail.Clear();
            dtReaSubscribeDetail = resList;
            reagentEditor1.listReaDetail = resList;
            BindGrid();
        }
        bool AnPatientChanging(string prev_patid, string pat_id, EntityReaSubscribe drPat)
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

            if (this.bsReaInfo.DataSource != null)
            {
                List<EntityReaSubscribe> dtpat = this.bsReaInfo.DataSource as List<EntityReaSubscribe>;
                if (dtpat != null)
                {
                    countTotal = dtpat.Count;

                    foreach (var item in dtpat)
                    {
                        if (item.Rsb_status != null)
                        {
                            if (item.Rsb_status.ToString() == "4")
                            {
                                countAudited++;
                            }
                            else if (item.Rsb_status.ToString() == "0")
                            {
                                countUnAudited++;
                            }
                            else if (item.Rsb_status.ToString() == "2")
                            {
                                countReturned++;
                            }
                        }
                        if (item.Rsb_printflag.ToString() == "1")
                        {
                            countPrinted++;
                        }
                    }
                }
            }
            this.lbRecordCount.Text = string.Format("总数：{0} 已审核：{1} 未审核：{2} 已打印：{3} 未通过：{4}", countTotal, countAudited, countUnAudited, countPrinted, countReturned);
        }
        private void FrmReagentSubscribe_Load(object sender, EventArgs e)
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
            this.selectDicReaGroup1.ValueChanged += SelectDicReaGroup1_ValueChanged;
            sysToolBar1.BtnDelete.Caption = "撤销申购";
            sysToolBar1.BtnUndo.Caption = "取消审核";
            sysToolBar1.BtnAnswer.Caption = "退回";
            sysToolBar1.SetToolButtonStyle(new string[] {sysToolBar1.BtnAdd.Name,
                                                sysToolBar1.BtnSave.Name,
                                                sysToolBar1.BtnDelete.Name,
                                                sysToolBar1.BtnRefresh.Name,
                                                sysToolBar1.BtnAudit.Name,
                                                sysToolBar1.BtnUndo.Name,
                                                sysToolBar1.BtnAnswer.Name,
                                                sysToolBar1.BtnPrint.Name,
                                                sysToolBar1.BtnPrintPreview2.Name,
                                                sysToolBar1.BtnClose.Name});
            BindLookupData();
        }

        private void SelectDicReaGroup1_ValueChanged(object sender, dcl.client.control.ValueChangeEventArgs args)
        {
            var drRes = selectDicReaGroup1.selectRow;
            if (drRes != null)
            {
                reagentEditor1.GroupID = drRes.Rea_group_id;
            }
            else
            {
                reagentEditor1.GroupID = string.Empty;
            }
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

        private void gvReaInfo_Click(object sender, EventArgs e)
        {
            if (bsReaInfo.Current != null)
            {
                EntityReaSubscribe drLst = (EntityReaSubscribe)bsReaInfo.Current;

                textEdit1.Visible = false;
                CurrentInfo = drLst;
                CurrentReaID = drLst.Rsb_no;
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
    }
}
