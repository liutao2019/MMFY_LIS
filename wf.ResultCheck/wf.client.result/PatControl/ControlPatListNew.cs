using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.frame.runsetting;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using dcl.client.common;
using dcl.common;
using DevExpress.XtraGrid;
using dcl.client.frame;
using dcl.root.logon;
using dcl.client.wcf;
using lis.client.control;
using System.Threading;
using dcl.entity;
using System.Linq;
using dcl.client.control;
using Lis.CustomControls;
using dcl.client.cache;

namespace dcl.client.result.PatControl
{
    public partial class ControlPatListNew : UserControl
    {
        internal FrmPatInputBaseNew ParentForm = null;

        //厦门 界面需配置
        bool IsNew;
        public string ItrDataType { get; set; }

        #region props & fields

        #region 录入日期
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime PatDate
        {
            get
            {
                return (DateTime)this.dtBegin.EditValue;
            }
            set
            {
                dtBegin.EditValue = value;
            }
        }

        public DateTime PatDateEnd
        {
            get
            {
                return (DateTime)this.dtEnd.EditValue;
            }
            set
            {
                dtEnd.EditValue = value;
            }
        }
        #endregion

        #region 物理组别ID

        /// <summary>
        /// 物理组别ID
        /// </summary>
        public string TypeID
        {
            get
            {
                return this.txtPatType.valueMember;
            }
            set
            {
                this.txtPatType.valueMember = value;
            }
        }


        #endregion

        #region 物理组名称

        public string TypeName
        {
            get
            {
                return this.txtPatType.displayMember;
            }
            set
            {
                this.txtPatType.displayMember = value;
            }
        }
        #endregion

        #region 仪器ID


        /// <summary>
        /// 仪器ID
        /// </summary>
        public string ItrID
        {
            get
            {
                return this.txtPatInstructment.valueMember;
            }
            set
            {
                if (txtPatInstructment.valueMember != value)
                {
                    txtPatInstructment.valueMember = value;


                }
            }
        }


        public string ItrName
        {
            get
            {
                return this.txtPatInstructment.displayMember;
            }
        }
        #endregion

        public bool CanChangeItrHostFlag = true;

        public bool IsReLoadData = false;

        private string ProId = string.Empty;

        #region 仪器通讯方式
        private int itr_host_flag = -1;

        public int ItrHostFlag
        {
            get
            {
                return itr_host_flag;
            }
            set
            {
                if (itr_host_flag != value)
                {
                    itr_host_flag = value;

                    ItrHostFlagChanged(itr_host_flag);

                }
            }
        }

        public bool IsAtuoPrint
        {
            get
            {
                return false;
            }
        }


        #endregion

        /// <summary>
        /// 仪器通讯方式改变
        /// </summary>
        /// <param name="itr_host_flag"></param>
        public void ItrHostFlagChanged(int itr_host_flag)
        {
            if (this.currentSetting != null)
            {
                ApplySetting(this.currentSetting);
            }
        }

        public string CurrentPatID { get; set; }

        private List<EntityPidReportMain> PatList;

        /// <summary>
        /// 用于存放总的数据源用于过滤
        /// </summary>
        private List<EntityPidReportMain> PatFilterList;


        public int PatientsCount
        {
            get
            {
                if (this.PatList != null)
                {
                    return this.PatList.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 父窗体类型
        /// </summary>
        public string ParentFormType { get; set; }
        #endregion

        #region ctor

        /// <summary>
        /// 构造函数
        /// </summary>
        public ControlPatListNew()
        {
            try
            {
                InitializeComponent();

                this.gridControl1.KeyDown += new KeyEventHandler(this.gridControl1_KeyDown);
                this.gridControl1.MouseDoubleClick += new MouseEventHandler(this.gridControl1_MouseDoubleClick);
                this.gridControl1.MouseUp += new MouseEventHandler(this.gridControl1_MouseUp);
                this.gridViewPatientList.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewPatientList_CustomDrawCell);
                this.gridViewPatientList.RowCellStyle += new RowCellStyleEventHandler(this.gridViewPatientList_RowCellStyle);
                this.gridViewPatientList.RowStyle += new RowStyleEventHandler(this.gridViewPatientList_RowStyle);
                this.gridViewPatientList.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewPatientList_FocusedRowChanged);
                this.gridViewPatientList.CustomColumnSort += new CustomColumnSortEventHandler(this.gridViewPatientList_CustomColumnSort);
                this.gridViewPatientList.MouseUp += new MouseEventHandler(this.gridViewPatientList_MouseUp);

                roundPanelGroup.RoundPanelGroupClick += RoundPanelGroup_RoundPanelGroupClick;
                rptOrigin.RoundPanelGroupClick += RptOrigin_RoundPanelGroupClick;

                this.dtBegin.EditValue = DateTime.Now;
                this.dtEnd.EditValue = DateTime.Now;

                ParentFormType = string.Empty;
                CurrentPatID = string.Empty;
                //隐藏非报告仪器
                bool Lab_HideNotReportInstrmt = UserInfo.GetSysConfigValue("Lab_HideNotReportInstrmt") == "是";
                if (Lab_HideNotReportInstrmt)
                {
                    this.txtPatInstructment.dtSource = this.txtPatInstructment.dtSource.Where(i => i.ItrReportIns != 0).ToList();
                }
                if (UserInfo.GetSysConfigValue("lab_patients_list") == "是")
                {
                    审核ToolStripMenuItem.Visible = true;
                    取消审核ToolStripMenuItem.Visible = true;
                    打印报告ToolStripMenuItem.Visible = true;
                    删除标本ToolStripMenuItem.Visible = true;
                    保存信息ToolStripMenuItem.Visible = true;
                }
            }
            catch (Exception ex)
            {

                Logger.WriteException(this.GetType().Name, "ctor", ex.ToString());
            }

        }

        #endregion

        /// <summary>
        /// 定位指定的样本ID
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public bool LocatePatient(string pat_sid)
        {
            if (string.IsNullOrEmpty(pat_sid))
                return false;
            return LocatePatient(pat_sid, false);
        }
        /// <summary>
        /// 急查颜色
        /// </summary>
        Color corUrgent = Color.White;
        Color corBD = Color.White;


        public void SetToolEvent()
        {
            审核ToolStripMenuItem.Click += new EventHandler(ParentForm.sysToolBar1_OnBatchReportClicked);
            取消审核ToolStripMenuItem.Click += new EventHandler(ParentForm.sysToolBar1_OnBatchUndoReportClicked);
            打印报告ToolStripMenuItem.Click += new EventHandler(ParentForm.sysToolBar1_OnBtnPrintClicked);
            删除标本ToolStripMenuItem.Click += new EventHandler(ParentForm.sysToolBar1_OnBtnDeleteBatchClicked);
            保存信息ToolStripMenuItem.Click += new EventHandler(ParentForm.sysToolBar1_OnBtnSaveClicked);
        }
        public void ShowUpdateMenu()
        {
            if (ConfigHelper.GetSysConfigValueWithoutLogin("IsUseUpdateInfectiousDisease") == "是")
            {
                更新为传染病toolStripMenuItem.Visible = true;
            }
            else
            {
                更新为传染病toolStripMenuItem.Visible = false;
            }
        }
        public event EventHandler UpdateCRBClick;

        private void 更新为传染病ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UpdateCRBClick != null)
            {
                UpdateCRBClick(sender, e);
            }
        }
        /// <summary>
        /// 定位指定的样本ID
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="SerialNumber">是否根据序列号定位</param>
        /// <returns></returns>
        public bool LocatePatient(string pat_sid, bool SerialNumber)
        {
            //this.gridViewPatientList.SelectAll();
            bool founded = false;
            List<EntityPidReportMain> patientsList = this.bsPatList.DataSource as List<EntityPidReportMain>;
            for (int i = 0; i < patientsList.Count; i++)
            {
                EntityPidReportMain patient = patientsList[i];
                //双向仪器是定位序列号而不是样本号的处理方法
                if (!SerialNumber)
                {
                    if (!string.IsNullOrEmpty(patient.RepSid))
                    {
                        if (pat_sid == patient.RepSid.ToString())
                        {
                            this._currpat = patient;
                            this.gridViewPatientList.FocusedRowHandle = i;
                            this.OnPatientChanged(patient.RepId.ToString(), patient);
                            founded = true;
                        }
                        else
                        {
                            this.gridViewPatientList.UnselectRow(i);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(patient.RepSerialNum))
                    {
                        if (pat_sid == patient.RepSerialNum.ToString())
                        {
                            this._currpat = patient;
                            this.OnPatientChanged(patient.RepId.ToString(), patient);
                            founded = true;
                        }
                        else
                        {
                        }
                    }
                }
            }
            return founded;
        }

        public bool LocatePatientByPatID(string pat_id, bool autoSelect)
        {
            //this.gridViewPatientList.SelectAll();
            bool founded = false;
            List<EntityPidReportMain> patientsList = this.bsPatList.DataSource as List<EntityPidReportMain>;
            for (int i = 0; i < patientsList.Count; i++)
            {
                EntityPidReportMain patient = patientsList[i];
                //patient.PatSelect = true;
                if (!string.IsNullOrEmpty(patient.RepId))
                {
                    if (pat_id == patient.RepId.ToString())
                    {
                        this._currpat = patient;
                        if (autoSelect)
                        {
                            this.gridViewPatientList.FocusedRowHandle = i;
                            this.OnPatientChanged(patient.RepId.ToString(), patient);
                        }
                        founded = true;
                    }
                    //else
                    //{
                    //    this.gridViewPatientList.UnselectRow(i);
                    //}
                    //patient.PatSelect = false;
                }
            }
            return founded;
        }

        public bool LocatePatientByPatID(string pat_id)
        {
            this.gridViewPatientList.SelectAll();
            bool founded = false;
            List<EntityPidReportMain> patientsList = this.bsPatList.DataSource as List<EntityPidReportMain>;
            foreach (EntityPidReportMain patient in patientsList)
            {
                if (!string.IsNullOrEmpty(patient.RepId))
                {
                    if (pat_id == patient.RepId.ToString())
                    {
                        this._currpat = patient;

                        //this.gridViewPatientList.FocusedRowHandle = i;

                        founded = true;
                    }
                    else
                    {
                        //this.gridViewPatientList.UnselectRow(i);
                    }
                }
            }
            return founded;
        }

        /// <summary>
        /// 取消选择所有
        /// </summary>
        public void UnSelectAll()
        {
            this.gridViewPatientList.ClearSelection();
        }


        /// <summary>
        /// 列表中是否存在指定的样本号
        /// </summary>
        /// <param name="pat_sid"></param>
        /// <returns></returns>
        public bool ExistSID(string pat_sid)
        {
            if (this.PatList != null)
            {
                foreach (EntityPidReportMain drPat in this.PatList)
                {
                    if (!string.IsNullOrEmpty(drPat.RepSid))
                    {
                        if (pat_sid == drPat.RepSid.ToString())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 列表是否存在相同的病人号在指定来源中
        /// </summary>
        /// <param name="str_ID">病人号</param>
        /// <param name="str_ori_id">来源ID</param>
        /// <returns></returns>
        public bool ExistPatientsID(string str_ID, string str_ori_id)
        {
            if (this.PatList != null && (!string.IsNullOrEmpty(str_ID)) && (!string.IsNullOrEmpty(str_ori_id)))
            {
                foreach (EntityPidReportMain drPat in this.PatList)
                {
                    if (!string.IsNullOrEmpty(drPat.PidInNo))
                    {
                        if (str_ID == drPat.PidInNo.ToString() && str_ori_id == drPat.PidInNo.ToString())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public void RefreshPatients()
        {
            //检验报告管理界面启用物理组查询
            if (UserInfo.GetSysConfigValue("Lab_TypeSelect") == "是")
                RefreshPatients(true);
            else
                RefreshPatients(false);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshPatients(bool typeSelect)
        {
            try
            {
                this.gridViewPatientList.FocusedRowChanged -= new FocusedRowChangedEventHandler(this.gridViewPatientList_FocusedRowChanged);
                string prevPatID = CurrentPatID;
                CurrentPatID = string.Empty;

                SearchPatients(typeSelect);

                this.UnSelectAll();
                this.gridViewPatientList.ClearSorting();

                this.LocatePatientByPatID(prevPatID, true);
                this.SelectAllPatientInGrid(false);
                OnAddNewDemand();

                Thread t = new Thread(ThreadLoadPatientStatus);
                t.Start();
            }
            catch (Exception EX)
            {
                MessageDialog.ShowAutoCloseDialog(EX.Message);
            }
            finally
            {
                this.gridViewPatientList.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewPatientList_FocusedRowChanged);

            }
        }

        #region 线程加载病人结果状态
        private void ThreadLoadPatientStatus()
        {
            if (!string.IsNullOrEmpty(ItrID))
            {
                ProxyPidReportMain proxy = new ProxyPidReportMain();
                List<EntityPidReportMain> table = proxy.Service.GetPatientStatus(this.PatDate, this.PatDateEnd, this.ItrID);
                //Thread.Sleep(2000);
                SetPatStatus(this.gridControl1, table);
            }
        }

        private delegate void DelegateSetPatStatus(GridControl grid, List<EntityPidReportMain> data);

        private void SetPatStatus(GridControl grid, List<EntityPidReportMain> data)
        {
            if (grid.InvokeRequired)
            {
                DelegateSetPatStatus del = new DelegateSetPatStatus(SetPatStatus);

                //if (!this.IsDisposed)
                this.Invoke(del, new object[] { grid, data });
            }
            else
            {
                try
                {
                    if (grid.DataSource != null && grid.DataSource is BindingSource)
                    {
                        BindingSource bs = grid.DataSource as BindingSource;
                        if (bs != null)
                        {
                            bAllowFirePatientChange = false;
                            List<EntityPidReportMain> source = bs.DataSource as List<EntityPidReportMain>;
                            if (source != null)
                            {
                                foreach (EntityPidReportMain rowSource in source)
                                {
                                    string pat_id = rowSource.RepId.ToString();
                                    if (rowSource.RepStatus != null
                                          && (rowSource.RepStatus.ToString() == "1" ||
                                          rowSource.RepStatus.ToString() == "2" ||
                                              rowSource.RepStatus.ToString() == "4"))
                                    {
                                        rowSource.HasResult = "1";
                                    }

                                    List<EntityPidReportMain> patList = data.Where(i => i.RepId == pat_id).ToList();
                                    if (patList.Count > 0)
                                    {
                                        if (patList[0].Status.ToString() == "0")
                                        {
                                            rowSource.HasResult = "10";
                                        }
                                        else
                                        {
                                            rowSource.HasResult = patList[0].Status.ToString();
                                        }
                                        if (UserInfo.GetSysConfigValue("Lab_RecheckFlag") == "是")
                                        {
                                            if (patList[0].RepRecheckFlag.ToString() == "1")
                                            {
                                                rowSource.HasResult = "-1";
                                            }
                                        }
                                        rowSource.HasResult2 = Convert.ToInt32(patList[0].ResStatus);
                                        rowSource.UrgentCount = Convert.ToInt32(patList[0].UrgStatus);
                                        rowSource.ModifyFlag = Convert.ToString(patList[0].ModifyFlag);
                                        if (!string.IsNullOrEmpty(patList[0].MsgContent))
                                        {
                                            rowSource.UrgentMsgHandle = 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    //throw;
                }
                finally
                {
                    this.gridControl1.RefreshDataSource();

                    bAllowFirePatientChange = true;
                }

            }
        }
        #endregion

        /// <summary>
        /// 移除指定的病人ID
        /// </summary>
        /// <param name="listPatID"></param>
        public void Remove(List<string> listPatID)
        {
            if (this.PatList != null && listPatID.Count > 0)
            {
                if (listPatID.Count == 1)
                {
                    Remove(listPatID[0]);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string pat_id in listPatID)
                    {
                        sb.Append(string.Format(",'{0}'", pat_id));
                    }
                    sb.Remove(0, 1);
                    List<EntityPidReportMain> patientList = this.PatList.Where(i => sb.ToString().Contains(i.RepId)).ToList();
                    List<EntityPidReportMain> ListPatient = patientList.FindAll(i => i.RepId != null);
                    foreach (EntityPidReportMain patient in ListPatient)
                    {
                        patientList.Remove(patient);
                    }
                }
            }
            RefreshCurrent();
        }

        public void Remove(string pat_id)
        {
            List<EntityPidReportMain> patientList = this.PatList.Where(i => i.RepId == pat_id).ToList();
            if (patientList.Count > 0)
            {
                patientList.Remove(patientList[0]);
            }
            RefreshItemsCount();
        }

        public void RefreshCurrent()
        {
            EntityPidReportMain patient = this.bsPatList.Current as EntityPidReportMain;

            if (patient != null)
            {
                this.gridViewPatientList.SelectRow(this.gridViewPatientList.FocusedRowHandle);
                OnPatientChanged(patient.RepId.ToString(), patient);
            }
            RefreshItemsCount();
        }


        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="type">物理组ID</param>
        /// <param name="itrid">仪器ID</param>
        private void SearchPatients(bool typeSelect)
        {
            try
            {
                if (dtBegin.DateTime != null && dtBegin.DateTime > DateTime.Parse("1900-01-01"))
                {
                    List<EntityPidReportMain> patientsList = new List<EntityPidReportMain>();

                    if (dtEnd.DateTime != null && dtBegin.DateTime.AddMonths(1) < dtEnd.DateTime)
                    {
                        MessageDialog.ShowAutoCloseDialog("时间跨度不能超过1个月");
                        return;
                    }

                    EntityRemoteCallClientInfo remoteCaller = new EntityRemoteCallClientInfo();
                    remoteCaller.IPAddress = UserInfo.ip;
                    remoteCaller.LoginID = UserInfo.loginID;

                    ProxyPidReportMain proxy = new ProxyPidReportMain();


                    EntityPatientQC PatientQC = new EntityPatientQC();
                    //根据实验组仪器查询
                    if (typeSelect)
                    {
                        string stringTypeId = string.IsNullOrEmpty(TypeID) ? "-1" : TypeID;
                        List<EntityDicInstrument> listInst = CacheClient.GetCache<EntityDicInstrument>();
                        //选择了仪器则按仪器查询  没选仪器选了实验组则按实验组的仪器来查询
                        if (!string.IsNullOrEmpty(ItrID))
                        {
                            PatientQC.ListItrId.Add(ItrID);
                        }
                        else {
                            foreach (EntityDicInstrument item in listInst)
                            {
                                if (item.ItrLabId == stringTypeId)
                                {
                                    PatientQC.ListItrId.Add(item.ItrId);
                                }
                            }
                        }
                    }
                    else
                    {
                        PatientQC.ListItrId.Add(string.IsNullOrEmpty(ItrID) ? "-1" : ItrID);
                    }

                    string auditWord = UserInfo.GetSysConfigValue("AuditWord");
                    if (auditWord == string.Empty)
                        auditWord = "审核";

                    string reportWord = UserInfo.GetSysConfigValue("ReportWord");
                    if (reportWord == string.Empty)
                        reportWord = "报告";


                    string strPatFlag = this.cmbGridFilterPatientState.EditValue.ToString();

                    if (strPatFlag == "1")//未报告
                    {
                        PatientQC.RepStatus = "1";
                    }
                    else if (strPatFlag == "2")//未审核
                    {
                        PatientQC.RepStatus = "0";
                    }
                    else if (strPatFlag == "3")//已报告
                    {
                        PatientQC.RepStatus = "2";
                    }
                    else if (strPatFlag == "4")//未打印
                    {
                        PatientQC.RepStatus = "2";
                    }
                    else if (strPatFlag == "5")//需复查和已复查
                    {
                        PatientQC.RepRecheck = true;
                    }
                    else if (strPatFlag == "6")//危急值
                    {
                        PatientQC.RepUrgent = true;
                    }

                    if (this.rptOrigin.GetCurRoundPanel().Tag?.ToString() == "107")
                    {
                        PatientQC.RepSrcId = "107";
                    }
                    else if (this.rptOrigin.GetCurRoundPanel().Tag?.ToString() == "108")
                    {
                        PatientQC.RepSrcId = "108";
                    }
                    else if (this.rptOrigin.GetCurRoundPanel().Tag?.ToString() == "109")
                    {
                        PatientQC.RepSrcId = "109";
                    }

                    PatientQC.auditWord = auditWord;
                    PatientQC.reportWord = reportWord;
                    PatientQC.DateStart = PatDate.Date;

                    DateTime end = PatDateEnd.Date.AddDays(1).AddMilliseconds(-1);
                    PatientQC.DateEnd = end;
                    //勾选了Tat显示  
                    if (visibileTat)
                    {
                        PatientQC.QueryTaT = true;
                    }
                    patientsList = proxy.Service.PatientQuery(PatientQC);
                    Lib.LogManager.Logger.LogInfo("病人个数:", patientsList.Count.ToString());
                    this.PatList = patientsList;
                    this.bsPatList.DataSource = this.PatList;

                    this.PatFilterList = EntityManager<EntityPidReportMain>.ListClone(PatList);

                    FilterPatients();
                    bsPatList.SuspendBinding();
                    bsPatList.ResumeBinding();
                    if (this.PatList.Count > 0)
                    {
                        this.IsNew = false;
                    }
                    else
                    {
                        this.IsNew = true;
                    }
                    this.gridViewPatientList.FocusedRowHandle = 0;

                    RefreshItemsCount();
                }
            }
            catch //(Exception ex)
            {
                throw;
            }
        }

        public void RefreshItemsCount()
        {
            this.lbRecordCount.Text = "记录数：" + this.gridViewPatientList.RowCount.ToString();

            int countUnAudited = 0;
            int countAudited = 0;
            int countReported = 0;
            int countPrinted = 0;
            int countTotal = 0;


            int countPass = 0;
            int countUnPass = 0;
            int countChecking = 0;
            int countNoChecking = 0;


            if (this.bsPatList.DataSource != null)
            {
                List<EntityPidReportMain> dtpat = this.bsPatList.DataSource as List<EntityPidReportMain>;
                if (dtpat != null && dtpat.Count > 0)
                {
                    countTotal = dtpat.Count;

                    foreach (EntityPidReportMain item in dtpat)
                    {
                        if (item.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Audited)
                            || item.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Printed)
                            || item.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported)
                            )
                        {
                            countAudited++;

                            if (item.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Printed)
                                || item.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported))
                            {
                                countReported++;
                            }

                            if (item.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Printed))
                            {
                                countPrinted++;
                            }
                        }
                        else if (item.RepStatus == null || item.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Natural))
                        {
                            countUnAudited++;
                        }
                        if (Lab_ShowAlarmColumn)
                        {
                            if (!string.IsNullOrEmpty(item.RepAuditWay.ToString()))
                            {
                                if (item.RepAuditWay.ToString() == "2")
                                {
                                    countPass++;
                                }
                                else if (item.RepAuditWay.ToString() == "1")
                                {
                                    countUnPass++;
                                }
                                else
                                {
                                    if (item.HasResult2.ToString() == "2")
                                    {
                                        countChecking++;
                                    }
                                    if (item.HasResult2.ToString() == "0")
                                    {
                                        countNoChecking++;
                                    }
                                }
                            }
                            else
                            {
                                if (item.HasResult2.ToString() == "2")
                                {
                                    countChecking++;
                                }
                                if (item.HasResult2.ToString() == "0")
                                {
                                    countNoChecking++;
                                }
                            }

                        }
                    }
                }
            }
            this.lbRecordCount.Text = string.Format("总数：{0} 已{4}：{1} 未{4}：{2} 已{5}：{3}", countTotal, countAudited, countUnAudited, countReported, LocalSetting.Current.Setting.AuditWord, LocalSetting.Current.Setting.ReportWord);
        }
        bool Lab_ShowAlarmColumn = false;

        string LabID = string.Empty;//存放默认实验组的ID
        string strDefaultItr = string.Empty; //存放默认仪器ID

        //没有勾选时默认选中当前行
        bool Lab_NoCheckSelectCurRow = false;

        /// <summary>
        /// 控件加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ControlPatList_Load(object sender, EventArgs e)
        {
            //防止设计器错误，后面还是会取服务器时间
            this.dtBegin.EditValue = this.dtEnd.EditValue = DateTime.Now;

            #region 实验组权限过滤 
            List<EntityDicPubProfession> listLueType = new List<EntityDicPubProfession>();//存放实验组过滤之后的数据
            if (!UserInfo.isAdmin)
            {
                if (UserInfo.listUserLab != null && UserInfo.listUserLab.Count > 0)
                {
                    listLueType = this.txtPatType.getDataSource().FindAll(w => UserInfo.listUserLab.FindIndex(i => i.LabId == w.ProId) > -1);
                }
                else
                    this.txtPatType.SetFilter(new List<EntityDicPubProfession>());
            }
            else
            {
                listLueType = this.txtPatType.getDataSource();
            }
            this.txtPatType.SetFilter(listLueType);
            #endregion

            //仪器权限过滤 
            txtPatInstructment_onBeforeFilter();
            if (DesignMode)
                return;

            //没有勾选时默认选中当前行
            Lab_NoCheckSelectCurRow = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoCheckSelectCurRow") == "是";
            this.menuConfig.Visible = (UserInfo.GetSysConfigValue(LIS_Const.SystemConfigurationCode.AllowCustomizePanel) == "是" || UserInfo.isAdmin);

            gridViewPatientList.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridViewPatientList.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewPatientList.FocusRectStyle = DrawFocusRectStyle.RowFocus;

            gridViewPatientList.MouseWheel += gridViewPatientList_MouseWheel;
            SearchPatients(false);

            InitControl();

            IsNew = true;

            Lab_ShowAlarmColumn = UserInfo.GetSysConfigValue("Lab_ShowAlarmColumn") == "是";


            this.dtBegin.EditValue = ServerDateTime.GetServerDateTime();
            this.dtEnd.EditValue = ServerDateTime.GetServerDateTime();

            corUrgent = ConfigHelper.GetBarcodeConfigColor("New_Barcode_Color_Urgent");
            corBD = ConfigHelper.GetBarcodeConfigColor("New_Barcode_Color_BD");

            报告解读ToolStripMenuItem.Visible = ConfigHelper.GetSysConfigValueWithoutLogin("Interpretation_Report") == "是";
            this.AutoScaleMode = AutoScaleMode.None;

            txtPatInstructment.popupContainerEdit1.BackColor = Color.LightBlue;

            #region 默认物理组和默认仪器
            //默认实验组
            LabID = LocalSetting.Current.Setting.CType_id;
            if (!string.IsNullOrEmpty(LabID))
            {
                var drPat_Types = listLueType.Find(a => a.ProId == LabID);
                if (drPat_Types != null)
                {
                    this.isMRZ = true;
                    this.txtPatType.selectRow = drPat_Types;
                    this.txtPatType.valueMember = LabID;
                    this.txtPatType.displayMember = drPat_Types.ProName;
                    this.isMRZ = false;

                    this.txtPatInstructment.SetFilter(this.txtPatInstructment.dtSource.FindAll(i => i.ItrLabId == LabID));//过滤仪器
                }
            }

            //默认仪器
            strDefaultItr = string.Empty;
            if (!string.IsNullOrEmpty(LocalSetting.Current.Setting.LocalItrID))
                strDefaultItr = LocalSetting.Current.Setting.LocalItrID;

            //默认仪器 
            var drPat_itrs = this.txtPatInstructment.dtSource.Find(i => i.ItrId == strDefaultItr);
            if (drPat_itrs != null)
            {
                this.txtPatInstructment.selectRow = drPat_itrs;
                this.txtPatInstructment.displayMember = drPat_itrs.ItrEname;
                this.txtPatInstructment.valueMember = strDefaultItr;
            }
            #endregion
        }

        void gridViewPatientList_MouseWheel(object sender, MouseEventArgs e)
        {
            gridViewPatientList.CloseEditor();
            gridViewPatientList.FocusedRowHandle = gridViewPatientList.FocusedRowHandle;
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            this.cmbGridFilterPatCType.Properties.DataSource = CommonValue.GetPatCtype();

            this.cmbGridFilterPatientState.Properties.DataSource = CommonValue.GetPatFlag();
        }


        PatListPanelSettingItem currentSetting = null;
        bool visibileTat = false;
        /// <summary>
        /// 应用式样设置
        /// </summary>
        public void ApplySetting(PatListPanelSettingItem setting)
        {
            currentSetting = setting;


            foreach (DataRow dr in setting.GridColSetting.Rows)
            {
                GridColumn col = this.gridViewPatientList.Columns[dr["FieldName"].ToString()];
                this.gridViewPatientList.Columns.Remove(col);
            }
            setting.GridColSetting.DefaultView.Sort = "VisibleIndex";
            DataTable dtSearchBarDDL = new DataTable();
            dtSearchBarDDL.Columns.Add("name");
            dtSearchBarDDL.Columns.Add("value");

            DataRow drSearchBarDDL = dtSearchBarDDL.NewRow();

            drSearchBarDDL["name"] = string.Empty;
            drSearchBarDDL["value"] = string.Empty;
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "样本号";
            drSearchBarDDL["value"] = "pat_sid_int";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "样本号(模糊)";
            drSearchBarDDL["value"] = "pat_sid_like";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "条码号";
            drSearchBarDDL["value"] = "pat_bar_code";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            // 2010-7-22
            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "ID类型";
            drSearchBarDDL["value"] = "pat_no_id_name";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "医院名称";
            drSearchBarDDL["value"] = "hos_name";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "诊断";
            drSearchBarDDL["value"] = "pat_diag";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            #region 生成列
            int visibleIndex = 0; ;
            setting.GridColSetting.DefaultView.Sort = "VisibleIndex asc";
            foreach (DataRow dr in setting.GridColSetting.Rows)
            {
                bool visible = Convert.ToBoolean(dr["Visible"]);
                //是否开启配置显示病人身份
                if (ConfigHelper.GetSysConfigValueWithoutLogin("ShowIdentityName") == "是")
                {
                    if (dr["FieldName"].ToString() == "PidIdentityName")
                    {
                        dr["Visible"] = true;
                    }
                }
                if (dr["FieldName"].ToString() == "TatTime")
                {
                    visibileTat = visible;
                }

                if (visible)
                {
                    if (dr["FieldName"].ToString() == "RepSerialNum" && this.ItrHostFlag != 2)
                    {
                        continue;
                    }
                    GridColumn col = new GridColumn();
                    col.Name = "col_" + dr["FieldName"].ToString();
                    col.Width = Convert.ToInt32(dr["ColumnWidth"]);
                    col.FieldName = dr["FieldName"].ToString();
                    col.Caption = dr["HeaderText"].ToString();
                    if (col.Caption == "序号")
                    {
                        col.Caption = "流水号";
                        col.Width = col.Width + 10;
                    }

                    col.OptionsColumn.AllowFocus = true;
                    col.OptionsColumn.AllowEdit = false;
                    col.OptionsColumn.AllowMove = false;
                    col.OptionsFilter.AllowFilter = false;
                    col.Visible = true;

                    col.VisibleIndex = 3 + visibleIndex;// + (int)dr["VisibleIndex"];
                    this.gridViewPatientList.Columns.Add(col);
                    if (CanChangeItrHostFlag)
                    {
                        if (dr["FieldName"].ToString() == "RepSerialNum")
                        {
                            col.VisibleIndex = 2;
                            //设置序号的排序方式为自定义排序
                            col.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
                        }

                    }
                    if (dr["FieldName"].ToString() == "PidIdentityName")
                    { col.VisibleIndex = 6; }
                    drSearchBarDDL = dtSearchBarDDL.NewRow();

                    drSearchBarDDL["name"] = dr["HeaderText"].ToString();
                    drSearchBarDDL["value"] = dr["FieldName"].ToString();

                    dtSearchBarDDL.Rows.Add(drSearchBarDDL);

                    if (dr["FieldName"].ToString() == "RepSerialNum")
                    {
                        drSearchBarDDL = dtSearchBarDDL.NewRow();
                        drSearchBarDDL["name"] = "流水号(模糊)";
                        drSearchBarDDL["value"] = "RepSerialNumLike";
                        dtSearchBarDDL.Rows.Add(drSearchBarDDL);

                        //pat_host_created = false;
                    }

                    if (dr["FieldName"].ToString() == "PidChkName")
                    {
                        col.Caption = LocalSetting.Current.Setting.AuditWord + "人";
                    }

                    if (dr["FieldName"].ToString() == "BgName")
                    {
                        col.Caption = LocalSetting.Current.Setting.ReportWord + "人";
                    }

                    if (col.FieldName == "PidSexName")
                    {
                        col.Width = 36;
                    }
                    if (col.FieldName == "SampApplyDate")
                    {
                        col.OptionsColumn.AllowSort = DefaultBoolean.True;
                        col.DisplayFormat.FormatType = FormatType.DateTime;
                        col.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                        col.Width = 180;
                    }
                    if (col.FieldName == "RepReportDate")
                    {
                        col.OptionsColumn.AllowSort = DefaultBoolean.True;
                        col.DisplayFormat.FormatType = FormatType.DateTime;
                        col.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                        col.Width = 180;
                    }
                    if (col.FieldName == "PidComName")
                    {
                        col.Width = 136;
                    }
                    if (col.FieldName == "PidName")
                    {
                        col.Width = 70;
                    }
                    if (col.FieldName == "PatLookName")
                    {
                        col.Width = 100;
                    }
                    if (col.FieldName == "RepReadDate")
                    {
                        col.Width = 180;
                    }
                    if (col.FieldName == "TatTime")
                    {
                        col.Width = 70;
                    }
                    visibleIndex++;

                }
            }
            #endregion

            //生成快速搜索栏的下拉列表
            this.cmbBarSearchPatType.Properties.DropDownRows = dtSearchBarDDL.Rows.Count;

            this.cmbBarSearchPatType.Properties.DataSource = dtSearchBarDDL;
            this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            this.cmbBarSearchPatType.Properties.ValueMember = "value";

            //生成危急值下拉框
            #region 生成危急值下拉框

            DataTable dtUrgentTab = new DataTable();
            dtUrgentTab.Columns.Add("uType");
            dtUrgentTab.Columns.Add("uValue");

            DataRow drUrgentTab = dtUrgentTab.NewRow();

            drUrgentTab["uType"] = "危急值";
            drUrgentTab["uValue"] = "0";
            dtUrgentTab.Rows.Add(drUrgentTab);

            drUrgentTab = dtUrgentTab.NewRow();
            drUrgentTab["uType"] = "已确认";
            drUrgentTab["uValue"] = "1";
            dtUrgentTab.Rows.Add(drUrgentTab);


            drUrgentTab = dtUrgentTab.NewRow();
            drUrgentTab["uType"] = "未确认";
            drUrgentTab["uValue"] = "2";
            dtUrgentTab.Rows.Add(drUrgentTab);

            #endregion

            //生成危急值下拉框
            #region 生成危急值下拉框

            DataTable dtSortTab = new DataTable();
            dtSortTab.Columns.Add("sType");
            dtSortTab.Columns.Add("sValue");

            DataRow drSortTab = dtSortTab.NewRow();

            drSortTab["sType"] = "默认";
            drSortTab["sValue"] = "0";
            dtSortTab.Rows.Add(drSortTab);

            drSortTab = dtSortTab.NewRow();
            drSortTab["sType"] = "样本号";
            drSortTab["sValue"] = "1";
            dtSortTab.Rows.Add(drSortTab);


            drSortTab = dtSortTab.NewRow();
            drSortTab["sType"] = "序号";
            drSortTab["sValue"] = "2";
            dtSortTab.Rows.Add(drSortTab);

            drSortTab = dtSortTab.NewRow();
            drSortTab["sType"] = "标本状态";
            drSortTab["sValue"] = "3";
            dtSortTab.Rows.Add(drSortTab);

            drSortTab = dtSortTab.NewRow();
            drSortTab["sType"] = "科室";
            drSortTab["sValue"] = "4";
            dtSortTab.Rows.Add(drSortTab);

            #endregion

            #region 整行显示字体颜色 2018-05-11
            for (int i = 0; i < gridViewPatientList.Columns.Count; i++) 
            {
                string columnName = gridViewPatientList.Columns[i].FieldName;
                if (columnName != "PatSelect" && columnName != "pat_icon")
                {
                    //已审核
                    FormatRowApplyTo(gridViewPatientList, columnName, LIS_Const.PATIENT_FLAG.Audited, setting.BackColorAudited, setting.BackColorAudited, setting.ForeColorAudited);
                    //已报告
                    FormatRowApplyTo(gridViewPatientList, columnName, LIS_Const.PATIENT_FLAG.Reported, setting.BackColorAudited, setting.BackColorReported, setting.ForeColorReported);
                    //已打印
                    FormatRowApplyTo(gridViewPatientList, columnName, LIS_Const.PATIENT_FLAG.Printed, setting.BackColorAudited, setting.BackColorPrinted, setting.ForeColorPrinted);
                }
            }
            #endregion

            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridViewPatientList.Columns["RepStatus"];
            rule.ColumnApplyTo = gridViewPatientList.Columns["PidName"];
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = LIS_Const.PATIENT_FLAG.Audited;
            if (setting.BackColorAudited != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = setting.BackColorAudited;//已审核
            }
            formatConditionRuleValue1.Appearance.ForeColor = setting.ForeColorAudited;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridViewPatientList.FormatRules.Add(rule);


            rule = new GridFormatRule();

            rule.Column = gridViewPatientList.Columns["RepStatus"];
            rule.ColumnApplyTo = gridViewPatientList.Columns["PidName"];
            //rule.Name = "Format0";
            formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = LIS_Const.PATIENT_FLAG.Reported;
            if (setting.BackColorAudited != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = setting.BackColorReported;//已报告
            }
            formatConditionRuleValue1.Appearance.ForeColor = setting.ForeColorReported;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridViewPatientList.FormatRules.Add(rule);

            rule = new GridFormatRule();

            rule.Column = gridViewPatientList.Columns["RepStatus"];
            rule.ColumnApplyTo = gridViewPatientList.Columns["PidName"];
            //rule.Name = "Format0";
            formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = LIS_Const.PATIENT_FLAG.Printed;
            if (setting.BackColorAudited != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = setting.BackColorPrinted;//已打印
            }
            formatConditionRuleValue1.Appearance.ForeColor = setting.ForeColorPrinted;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridViewPatientList.FormatRules.Add(rule);

            rule = new GridFormatRule();

            rule.Column = gridViewPatientList.Columns["PidIdentityName"];
            rule.ColumnApplyTo = gridViewPatientList.Columns["PidIdentityName"];
            formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = "临床路径";
            formatConditionRuleValue1.Appearance.BackColor = Color.FromArgb(0, 255, 0);//病人身份颜色
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gridViewPatientList.FormatRules.Add(rule);
        }

        private void FormatRowApplyTo(GridView gridView, string fieldName, string auditedValue, Color backColorAudited, Color backColor, Color foreColor)
        {
            GridFormatRule rule = new GridFormatRule();

            rule.Column = gridView.Columns["RepStatus"];
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

        #region Event 事件

        #region patchanged
        public delegate void PatientChangedEventHandler(object sender, PatientChangedEventArgs args);

        /// <summary>
        /// 病人选择改变后
        /// </summary>
        public event PatientChangedEventHandler PatientChanged;

        public void OnPatientChanged(string pat_id, EntityPidReportMain drPat)
        {
            if (PatientChanged != null && bAllowFirePatientChange)
            {
                PatientChanged(this, new PatientChangedEventArgs(pat_id, drPat));
            }
        }
        private bool bAllowFirePatientChange = true;

        public class PatientChangedEventArgs : EventArgs
        {
            public string Pat_ID { get; private set; }
            public EntityPidReportMain Pat_Data { get; private set; }

            public PatientChangedEventArgs(string pat_id, EntityPidReportMain drPat)
            {
                this.Pat_ID = pat_id;
                this.Pat_Data = drPat;
            }
        }
        #endregion

        #region patchanging
        public delegate void PatientChangingEventHandler(object sender, PatientChangingEventArgs args);

        /// <summary>
        /// 病人选择改变时
        /// </summary>
        public event PatientChangingEventHandler PatientChanging;

        public bool OnPatientChanging(string prev_patid, string pat_id, EntityPidReportMain drPat)
        {
            PatientChangingEventArgs args = new PatientChangingEventArgs(prev_patid, pat_id, drPat);
            if (PatientChanging != null)
            {
                PatientChanging(this, args);
            }
            return args.Cancel;
        }

        public class PatientChangingEventArgs : EventArgs
        {
            public string Prev_PatId { get; set; }
            public string Pat_ID { get; private set; }
            public EntityPidReportMain Pat_Data { get; private set; }

            public PatientChangingEventArgs(string prev_patid, string pat_id, EntityPidReportMain drPat)
            {
                this.Prev_PatId = prev_patid;
                this.Pat_ID = pat_id;
                this.Pat_Data = drPat;
                Cancel = false;
            }

            public bool Cancel { get; set; }
        }
        #endregion

        public delegate void PanelConfigEventHandler(object sender, EventArgs args);
        public event PanelConfigEventHandler PanelConfig;
        public void OnPanelConfig()
        {
            if (PanelConfig != null)
            {
                PanelConfig(this, EventArgs.Empty);
            }
        }

        #endregion

        #region CheckBoxOnGridHeader

        public void SelectAllPatientInGrid(bool selectAll)
        {
            //在全部勾选前把焦点行设置成-1解决：全选的时候焦点行会显示不了勾
            if (selectAll)
            {
                gridViewPatientList.SelectAll();
            }
            else
                gridViewPatientList.ClearSelection();
            this.gridControl1.RefreshDataSource();
        }
        #endregion

        #region CustomDrawCell
        /// <summary>
        /// draw icon on patlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPatientList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;
            if (e.Column.Name == "col_icon")
            {
                EntityPidReportMain dr = this.gridViewPatientList.GetRow(e.RowHandle) as EntityPidReportMain;
                if (dr != null)
                {
                    if (dr.HasResult == "1")
                    {
                        int x = r.X + 2;
                        int y = r.Y + (r.Height - imageList1.ImageSize.Height) / 2;
                        //绿色旗子
                        e.Graphics.DrawImageUnscaled(imageList1.Images[1], x, y);
                    }
                    else if (dr.HasResult == "2")
                    {
                        int x = r.X + 2;
                        int y = r.Y + (r.Height - imageList1.ImageSize.Height) / 2;
                        //黄色旗子
                        e.Graphics.DrawImageUnscaled(imageList1.Images[2], x, y);
                    }
                    else if (dr.HasResult == "3")
                    {
                        int x = r.X + 2;
                        int y = r.Y + (r.Height - imageList1.ImageSize.Height) / 2;
                        //红色旗子
                        e.Graphics.DrawImageUnscaled(imageList1.Images[3], x, y);
                    }
                    if (UserInfo.GetSysConfigValue("Lab_RecheckFlag") == "是")
                    {
                        if (dr.HasResult == "-1")
                        {
                            int x = r.X + 2;
                            int y = r.Y + (r.Height - imageList1.ImageSize.Height) / 2;
                            e.Graphics.DrawImageUnscaled(imageList1.Images[6], x, y);
                        }
                    }
                }
                e.Handled = true;
            }
        }

        internal void FocusItr()
        {
            txtPatInstructment.Focus();
        }
        #endregion

        #region 过滤条件相关

        /// <summary>
        /// 在病人列表中过滤病人
        /// </summary>
        private void FilterPatients()
        {
            string strPatCType = this.cmbGridFilterPatCType.EditValue.ToString();
            string strPatFlag = this.cmbGridFilterPatientState.EditValue.ToString();

            #region 病人状态过滤方式
            int itrHostFlag = DictInstrmt.Instance.GetItrHostFlag(ItrID);
            if (strPatFlag == "-1")//未报告
            {
            }

            if (strPatFlag == "1")//未报告
            {
                PatList = PatFilterList.Where(i => i.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Audited)).ToList();
            }
            else if (strPatFlag == "2")//未审核
            {
                PatList = PatFilterList.Where(i => ((i.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Natural)) || i.RepStatus == null)).ToList();
                if (!ConfigHelper.IsNotOutlink())
                {
                    PatList = PatFilterList.OrderBy(i => i.HasResult).OrderBy(w => w.PatSidInt).OrderBy(r => r.RepSerialNum).ToList();
                }
            }
            else if (strPatFlag == "3")//已报告
            {
                PatList = PatFilterList.Where(i => i.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported)).ToList();
            }
            else if (strPatFlag == "4")//未打印
            {
                PatList = PatFilterList.Where(i => i.RepStatus == Convert.ToInt32(LIS_Const.PATIENT_FLAG.Reported)).ToList();
            }
            else if (strPatFlag == "5")//需复查和已复查
            {
                PatList = PatFilterList.Where(i => (i.RepRecheckFlag == 1 || i.RepRecheckFlag == 2)).ToList();
            }
            else if (strPatFlag == "6")//危急值
            {
                if (this.bsPatList.DataSource != null)
                {
                    PatList = PatFilterList.Where(i => (i.UrgentCount > 0 || i.RepUrgentFlag >= 1)).ToList();
                }
            }
            #endregion

            List<EntityPidReportMain> listPatTemp = new List<EntityPidReportMain>();
            listPatTemp = EntityManager<EntityPidReportMain>.ListClone(PatList);
            string origin = rptOrigin.GetCurRoundPanel().Tag?.ToString();
            if (origin == "0")
            {
            }
            else if (origin == "107")
            {
                PatList = listPatTemp.Where(i => i.PidSrcId == "107").ToList();
            }
            else if (origin == "108")
            {
                PatList = listPatTemp.Where(i => i.PidSrcId == "108").ToList();
            }
            else if (origin == "109")
            {
                PatList = listPatTemp.Where(i => i.PidSrcId == "109").ToList();
            }
            else if (origin == "110")
            {
                PatList = listPatTemp.Where(i => i.RepCtype == "2").ToList();
            }
            else if (origin == "111")
            {
                PatList = listPatTemp.Where(i => (!string.IsNullOrEmpty(i.PatLookName) || i.RepReadDate != null)).ToList();
            }
            if (strPatCType != "-1" && !string.IsNullOrEmpty(strPatCType))
            {
                PatList = listPatTemp.Where(i => i.RepCtype == strPatCType).ToList();
            }
            else
            {

            }

            //Lib.LogManager.Logger.LogInfo("过滤后病人个数:", PatList.Count.ToString());
            this.bsPatList.DataSource = PatList;

            gridViewPatientList.ClearSelection();
        }

        private void cmbGridFilterPatientState_EditValueChanged(object sender, EventArgs e)
        {
            RefreshPatients();
        }

        private void cmbGridFilterPatCType_EditValueChanged(object sender, EventArgs e)
        {

            RefreshPatients();

            this.SelectAllPatientInGrid(false);
            OnAddNewDemand();
        }

        private void OnAddNewDemand()
        {
            if (this.AddNewDemand != null)
            {
                this.AddNewDemand(this, EventArgs.Empty);
            }
        }

        private void RapitSearch()
        {
            gridViewPatientList.FocusedRowChanged -= gridViewPatientList_FocusedRowChanged;

            isChanging = true;
            FilterPatients();
            if (this.cmbBarSearchPatType.EditValue != null
                && this.cmbBarSearchPatType.EditValue.ToString().Trim(null) != string.Empty
                && this.txtBarSearchCondition.Text.Trim(null) != string.Empty)
            {
                string searchField = this.cmbBarSearchPatType.EditValue.ToString();
                string searchValue = this.txtBarSearchCondition.Text.TrimEnd();
                #region 查询条件LINQ
                if (searchField == "pat_sid_int")
                {
                    #region 样本号区间查询
                    string[] sidList = searchValue.Split(',');
                    if (sidList.Length > 0)
                    {
                        foreach (string sids in sidList)
                        {
                            if (sids.IndexOf('-') != -1)
                            {
                                string[] sid2 = sids.Split('-');


                                if (sid2.Length == 2)
                                {
                                    Int64 sidFrom = 0;
                                    Int64 sidTo = 0;

                                    Int64.TryParse(sid2[0], out sidFrom);
                                    Int64.TryParse(sid2[1], out sidTo);
                                    PatList = PatFilterList.Where(i => i.PatSidInt >= sidFrom && i.PatSidInt <= sidTo).ToList();
                                    //sidFilter += string.Format(" or (pat_sid_int >={0} and pat_sid_int <={1}) ", sidFrom, sidTo);
                                }
                                else
                                {
                                    PatList = PatFilterList.Where(i => i.PatSidInt < 0).ToList();
                                    //sidFilter += " and pat_sid_int < 0";
                                }
                            }
                            else
                            {
                                string sid2 = sids.Trim(null);

                                Int64 i = -1;

                                Int64.TryParse(sid2, out i);

                                PatList = PatFilterList.Where(r => r.PatSidInt == i).ToList();
                                //sidFilter += " or pat_sid_int = " + i;
                            }
                        }

                        //this.bsPatList.Filter += " and (" + sidFilter + ") ";
                    }
                    else
                    {
                        PatList = new List<EntityPidReportMain>();
                    }
                    #endregion
                }
                else if (searchField == "RepSerialNum")
                {
                    #region 样本号区间查询
                    string[] sidList = searchValue.Split(',');
                    if (sidList.Length > 0)
                    {
                        //string sidFilter = " 1=2 ";
                        foreach (string sids in sidList)
                        {
                            if (sids.IndexOf('-') != -1)
                            {
                                string[] sid2 = sids.Split('-');


                                if (sid2.Length == 2)
                                {
                                    int sidFrom = 0;
                                    int sidTo = 0;

                                    int.TryParse(sid2[0], out sidFrom);
                                    int.TryParse(sid2[1], out sidTo);

                                    PatList = PatFilterList.Where(i => !string.IsNullOrEmpty(i.RepSerialNum) && Convert.ToInt32(i.RepSerialNum) >= sidFrom && Convert.ToInt32(i.RepSerialNum) <= sidTo).ToList();

                                    //sidFilter += string.Format(" or (pat_host_order_int >={0} and pat_host_order_int <={1}) ", sidFrom, sidTo);
                                }
                                else
                                {
                                    PatList = PatFilterList.Where(i => Convert.ToInt32(i.RepSerialNum) < 0).ToList();
                                    //sidFilter += " and pat_host_order_int < 0";
                                }
                            }
                            else
                            {
                                string sid2 = sids.Trim(null);

                                int i = -1;

                                int.TryParse(sid2, out i);

                                PatList = PatFilterList.Where(r => r.RepSerialNum == i.ToString()).ToList();
                                //sidFilter += " or pat_host_order_int = " + i;
                            }
                        }

                        //this.bsPatList.Filter += " and (" + sidFilter + ") ";
                    }
                    else
                    {
                        PatList = new List<EntityPidReportMain>();
                        this.bsPatList.Filter += " and 1=2 ";
                    }
                    #endregion
                }
                //条码号
                else if (searchField == "pat_bar_code")
                {
                    PatList = PatList.Where(i => i.RepBarCode.Contains(searchValue)).ToList();
                }
                //样本号（模糊）
                else if (searchField == "pat_sid_like")
                {
                    PatList = PatFilterList.Where(i => i.RepSid.Contains(searchValue)).ToList();
                }
                //ID类型
                else if (searchField == "pat_no_id_name")// 2010-7-22 增加ID类型的查询
                {
                    PatList = PatFilterList.Where(i => i.IdtName.Contains(searchValue)).ToList();
                }
                //医院名称
                else if (searchField == "hos_name")// 2016-1-25 增加分院的查询
                {
                    PatList = PatFilterList.Where(i => i.HosName.Contains(searchValue)).ToList();
                }
                //诊断
                else if (searchField == "pat_diag")//pat_diag
                {
                    PatList = PatFilterList.Where(i => i.PidDiag.Contains(searchValue)).ToList();
                }
                //序号（模糊）
                else if (searchField == "RepSerialNumLike")
                {
                    PatList = PatFilterList.Where(i => i.RepSerialNum.Contains(searchValue)).ToList();
                }
                //性别
                else if (searchField == "PidSexName")
                {
                    if (searchValue == "1")
                    {
                        searchValue = "男";
                    }
                    else if (searchValue == "2")
                    {
                        searchValue = "女";
                    }
                    PatList = PatFilterList.Where(i => i.PidSexName.Contains(searchValue)).ToList();
                }
                //姓名
                else if (searchField == "PidName")
                {
                    PatList = PatFilterList.Where(i => i.PidName.Contains(searchValue)).ToList();
                }
                //检验组合
                else if (searchField == "PidComName")
                {
                    PatList = PatFilterList.Where(i => i.PidComName.Contains(searchValue)).ToList();
                }
                //年龄
                else if (searchField == "PidAgeExp")
                {
                    PatList = PatFilterList.Where(i => i.PidAgeExp.Contains(searchValue)).ToList();
                }
                //状态
                else if (searchField == "RepStatusName")
                {
                    PatList = PatFilterList.Where(i => i.RepStatusName.Contains(searchValue)).ToList();
                }
                //病人ID
                else if (searchField == "PidInNo")
                {
                    PatList = PatFilterList.Where(i => i.PidInNo.Contains(searchValue) ||
                                                  i.PidSocialNo.Contains(searchValue) ||
                                                 (!string.IsNullOrEmpty(i.PidExamCompany) && i.PidExamCompany.Contains(searchValue))).ToList();
                }
                //标本
                else if (searchField == "SamName")
                {
                    PatList = PatFilterList.Where(i => i.SamName.Contains(searchValue)).ToList();
                }
                //病床号
                else if (searchField == "PidDeptName")
                {
                    PatList = PatFilterList.Where(i => i.PidDeptName.Contains(searchValue)).ToList();
                }
                //科室
                else if (searchField == "PidDeptName")
                {
                    PatList = PatFilterList.Where(i => i.PidDeptName.Contains(searchValue)).ToList();
                }
                //录入人
                else if (searchField == "LrName")
                {
                    PatList = PatFilterList.Where(i => i.LrName.Contains(searchValue)).ToList();
                }
                //审核人
                else if (searchField == "PidChkName")
                {
                    PatList = PatFilterList.Where(i => i.PidChkName.Contains(searchValue)).ToList();
                }
                //报告人
                else if (searchField == "BgName")
                {
                    PatList = PatFilterList.Where(i => i.BgName.Contains(searchValue)).ToList();
                }
                //查看人
                else if (searchField == "PatLookName")
                {
                    PatList = PatFilterList.Where(i => i.PatLookName.Contains(searchValue)).ToList();
                }
                //查看时间
                else if (searchField == "RepReadDate")
                {
                    PatList = PatFilterList.Where(i => i.RepReadDate.ToString().Contains(searchValue)).ToList();
                }
                //接收时间
                else if (searchField == "SampApplyDate")
                {
                    PatList = PatFilterList.Where(i => i.LrName.Contains(searchValue)).ToList();
                }
                //仪器代码
                else if (searchField == "ItrEname")
                {
                    PatList = PatFilterList.Where(i => i.ItrEname.Contains(searchValue)).ToList();
                }
                #endregion
            }
            else
                PatList = PatFilterList;

            bsPatList.DataSource = PatList;
            //this.gridControl1.DataSource = patientList;
            isChanging = false;
            RefreshItemsCount();
            gridViewPatientList_FocusedRowChanged(null, null);
            gridViewPatientList.FocusedRowChanged += gridViewPatientList_FocusedRowChanged;
        }

        bool isChanging = false;
        private void txtBarSearchCondition_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //现修改为，清空才检索，其他需要回车才检索。
            if (txtBarSearchCondition.Text.Trim() == string.Empty)
            {
                barSearch();
            }
        }

        private void txtBarSearchCondition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                barSearch();
            }
        }


        private void barSearch()
        {
            try
            {
                isChanging = true;
                this.SelectAllPatientInGrid(false);
                RapitSearch();
            }
            finally
            {
                isChanging = false;
            }
        }


        #endregion

        public void AddNew()
        {
            this.UnSelectAll();
            this.gridViewPatientList.FocusedRowHandle = -1;
            this._currpat = null;
            IsNew = true;

        }

        public void CancelAddNew()
        {
            IsNew = false;
        }

        EntityPidReportMain _currpat = null;
        /// <summary>
        /// 当前病人
        /// </summary>
        public EntityPidReportMain CurrentPatient
        {
            set
            {
                _currpat = value;
            }
            get
            {
                return _currpat;
            }
        }


        //已勾选的病人
        public List<EntityPidReportMain> listCheck = new List<EntityPidReportMain>();

        /// <summary>
        /// 获取已勾选的病人
        /// </summary>
        /// <returns></returns>
        public List<EntityPidReportMain> GetCheckedPatients()
        {
            this.CloseEditor();

            List<EntityPidReportMain> checkList = new List<EntityPidReportMain>();
            var selectIndex = this.gridViewPatientList.GetSelectedRows();
            foreach (int index in selectIndex)
            {
                checkList.Add(gridViewPatientList.GetRow(index) as EntityPidReportMain);
            }

            if (checkList.Count <= 0
                && Lab_NoCheckSelectCurRow
                && CurrentPatient != null
                && !string.IsNullOrEmpty(CurrentPatient.RepId))
            {
                checkList.Add(gridViewPatientList.GetFocusedRow() as EntityPidReportMain);
            }

            listCheck = checkList;

            return checkList;
        }


        /// <summary>
        /// 记录保存前勾选的病人
        /// </summary>
        public void SelectLastCheckPatients()
        {
            if (listCheck == null || listCheck.Count <= 0)
                return;

            //EntityPidReportMain drtemp = gridViewPatientList.GetFocusedRow() as EntityPidReportMain;
            //gridViewPatientList.FocusedRowHandle
            // int[] rownumber = this.gridView1.GetSelectedRows();//获取选中行号；
            //DataRow row = this.gridView1.GetDataRow(rownumber[0]);//根据行号获取相应行的数据；
            //this.bsPatList.DataSource as List<EntityPidReportMain>

            foreach (EntityPidReportMain check in listCheck)
            {
                int dataSourceIndex = 0;
                foreach (EntityPidReportMain report in this.bsPatList)
                {
                    if (check.RepId == report.RepId)
                    {
                        int rowHandle = gridViewPatientList.GetRowHandle(dataSourceIndex);
                        gridViewPatientList.SelectRow(rowHandle);
                        dataSourceIndex++;
                        break;
                    }
                    dataSourceIndex++;
                }
            }
            gridViewPatientList.RefreshData();
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<EntityPidReportMain> GetALLPatients()
        {
            this.CloseEditor();
            List<EntityPidReportMain> list = new List<EntityPidReportMain>();
            foreach (EntityPidReportMain dr in this.PatList)
            {
                if (!string.IsNullOrEmpty(dr.RepId.ToString()))
                {
                    list.Add(dr);
                }
            }
            return list;
        }

        /// <summary>
        /// 鼠标点击病人列表网格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPatientList_MouseUp(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.gridViewPatientList.CalcHitInfo(e.X, e.Y);
            if (hi.InRowCell && hi.InRow)
            {
                if (IsNew)
                {
                    EntityPidReportMain dr = this.bsPatList.Current as EntityPidReportMain;
                    if (dr != null && !string.IsNullOrEmpty(dr.RepId))
                    {
                        string pid = dr.RepId.ToString();

                        if (!OnPatientChanging(CurrentPatID, pid, dr))
                        {
                            CurrentPatID = pid;
                            OnPatientChanged(pid, dr);
                        }
                    }
                    this.IsNew = false;
                }
            }

        }


        /// <summary>
        /// 设置、获取搜索栏，过滤栏是否可见
        /// </summary>
        public bool FilterSearchBarVisible
        {
            get
            {
                return this.pnlFilterBar.Visible;
            }
            set
            {

            }
        }


        public void CloseEditor()
        {
            try
            {
                gridViewPatientList.CloseEditor();
            }
            catch
            {
            }
        }

        private void gridViewPatientList_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (isChanging) return;
            CloseEditor();
            this._currpat = this.gridViewPatientList.GetFocusedRow() as EntityPidReportMain;
            if (this._currpat != null && !string.IsNullOrEmpty(this._currpat.RepId))
            {
                string pid = this._currpat.RepId.ToString();
                if (pid != CurrentPatID)
                {
                    if (!OnPatientChanging(CurrentPatID, pid, this._currpat))
                    {
                        CurrentPatID = pid;
                        OnPatientChanged(pid, this._currpat);
                    }
                }
            }
            this.IsNew = false;
            this.gridViewPatientList.Focus(); //获取焦点行
        }

        /// <summary>
        /// 刷新焦点行数据
        /// </summary>
        public void RefreshFocusedRowData()
        {
            CurrentPatID = "-1";
            gridViewPatientList_FocusedRowChanged(null, null);
        }

        private void menuConfig_Click(object sender, EventArgs e)
        {
            OnPanelConfig();
        }

        public void Reset()
        {
            this.gridViewPatientList.FocusedRowChanged -= new FocusedRowChangedEventHandler(this.gridViewPatientList_FocusedRowChanged);
            this.PatList.Clear();
            this.gridViewPatientList.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewPatientList_FocusedRowChanged);

        }

        /// <summary>
        /// 根据病人ID获取病人信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityPidReportMain GetPatient(string pat_id)
        {
            if (this.PatList != null)
            {
                foreach (EntityPidReportMain dr in this.PatList)
                {
                    if (dr.RepId.ToString() == pat_id)
                    {
                        return dr;
                    }
                }
            }
            return null;
        }

        public bool ResultImageVisible
        {
            get
            {
                return this.gridViewPatientList.Columns["pat_icon"].Visible;
            }
            set
            {
                this.gridViewPatientList.Columns["pat_icon"].Visible = value;
            }
        }

        private void gridControl1_MouseUp(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = this.gridViewPatientList.CalcHitInfo(e.X, e.Y);

            if (hi.InRowCell == true)
            {
                this.gridViewPatientList.CloseEditor();
                EntityPidReportMain dr = this.gridViewPatientList.GetFocusedRow() as EntityPidReportMain;

                if (dr != null && !string.IsNullOrEmpty(dr.RepId))
                {
                    string pid = dr.RepId.ToString();

                    this._currpat = dr;

                    if (this.gridViewPatientList.FocusedRowHandle == 0)
                    {
                        OnPatientChanged(pid, dr);
                    }
                }
            }
        }

        public event EventHandler AddNewDemand;

        public event EventHandler OnItrChanged;
        public event EventHandler OnTypeChanged;

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //系统配置：允许发送中期报告的仪器集
            string tempItrIDs = UserInfo.GetSysConfigValue("Lab_AllowSendMidRepItrIDs");
            if (!string.IsNullOrEmpty(tempItrIDs) && !string.IsNullOrEmpty(ItrID)
                && tempItrIDs == ItrID || (tempItrIDs.StartsWith(ItrID + ",") || tempItrIDs.Contains("," + ItrID + ",") || tempItrIDs.EndsWith("," + ItrID)))
            {
                menuAllowMidReport.Visible = true;
            }
            else
            {
                menuAllowMidReport.Visible = false;
            }
        }


        public void ScrollToBottom()
        {
            bAllowFirePatientChange = false;

            this.gridViewPatientList.MoveLastVisible();

            bAllowFirePatientChange = true;
        }



        private void gridViewPatientList_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gridViewPatientList.FocusedRowHandle)
            {
                if (this.IsNew)
                {
                    e.Appearance.BackColor = Color.Transparent;
                }
                else
                {
                    e.Appearance.BackColor = Color.LightBlue;
                }
            }
            else
            {
                EntityPidReportMain row = this.gridViewPatientList.GetRow(e.RowHandle) as EntityPidReportMain;
                if (row != null)
                {
                    if (!string.IsNullOrEmpty(row.HasResult) && row.HasResult.ToString() == "4")
                    {
                        e.Appearance.BackColor = Color.Yellow;
                    }
                    if (!string.IsNullOrEmpty(row.HasResult) && row.HasResult.ToString() == "5")
                    {
                        e.Appearance.BackColor = Color.Green;
                    }
                }
            }
        }

        private void gridViewPatientList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //if (e.RowHandle == this.gridViewPatientList.FocusedRowHandle)
            //{
            //    if (this.IsNew)
            //    {
            //        e.Appearance.BackColor = Color.Transparent;
            //    }
            //    else
            //    {
            //        e.Appearance.BackColor = Color.LightBlue;
            //    }
            //}
            //else
            //{
            //    EntityPidReportMain row = this.gridViewPatientList.GetRow(e.RowHandle) as EntityPidReportMain;
            //    if (row != null)
            //    {
            //        if (!string.IsNullOrEmpty(row.HasResult) && row.HasResult.ToString() == "4")
            //        {
            //            e.Appearance.BackColor = Color.Yellow;
            //        }
            //        if (!string.IsNullOrEmpty(row.HasResult) && row.HasResult.ToString() == "5")
            //        {
            //            e.Appearance.BackColor = Color.Green;
            //        }
            //    }
            //}

            GridView grid = sender as GridView;
            EntityPidReportMain row = grid.GetRow(e.RowHandle) as EntityPidReportMain;

            #region 颜色区分门诊住院
            if (UserInfo.GetSysConfigValue("Lab_ColorWithInOutPatient") == "是")
            {
                if (e.Column.FieldName == "RepSid")
                {
                    if (row != null)
                    {
                        string pat_ori_id = row.PidSrcId?.ToString();
                        string pat_bed_no = row.PidBedNo?.ToString().Trim();

                        if (pat_ori_id == "107")//门诊(可以与下面的代码合并到一起)
                        {
                            e.Appearance.BackColor = Color.LightGreen;
                        }
                        else if (pat_ori_id == "108")//住院
                        {
                            e.Appearance.BackColor = Color.Moccasin;
                        }
                    }
                }
            }
            #endregion

            if (e.Column.FieldName == "PidName" && row != null)
            {
                if (row.RepUrgentFlag.ToString() == "1")//未查看危急值
                {
                    if (currentSetting.BackColorNUrgent == Color.Empty)
                        e.Appearance.BackColor = Color.FromArgb(192, 255, 255);
                    else
                        e.Appearance.BackColor = currentSetting.BackColorNUrgent;
                }
                else if (row.RepUrgentFlag.ToString() == "2" && row.UrgentCount.ToString() != "0")//已查看危急值
                {
                    if (currentSetting.BackColorUrgent == Color.Empty)
                        e.Appearance.BackColor = Color.FromArgb(192, 255, 255);
                    else
                        e.Appearance.BackColor = currentSetting.BackColorUrgent;
                }
                else if (row.RepCtype.ToString() == "2")
                {
                    e.Appearance.BackColor = Color.FromArgb(64, 224, 208);
                }
            }

            if (e.Column.FieldName == "RepSid" && row != null)
            {
                if (row.RepRecheckFlag == 1)
                    e.Appearance.BackColor = Color.FromArgb(255, 218, 185);
                else if (row.RepRecheckFlag == 2)
                    e.Appearance.BackColor = Color.FromArgb(133, 133, 253);
            }

            if (e.Column.FieldName == "RepSid" 
                && row != null 
                && row.RepStatus != null)
            {
                if (row.RepStatus == 1) //已审核
                    e.Appearance.ForeColor =  Color.Green;
                else if (row.RepStatus == 2) //已报告
                    e.Appearance.ForeColor =  Color.Blue;
                else if (row.RepStatus == 4) //已打印
                    e.Appearance.ForeColor =  Color.Red;
            }

            if (e.Column.FieldName.Contains("TatTime") 
                && e.Column.FieldName == "TatTime"
                && row != null 
                && !string.IsNullOrEmpty(row.TatTime) 
                && !string.IsNullOrEmpty(row.TatComTime))
            {
                if (row.RepStatus == 0)
                {
                    if ((Convert.ToInt32(row.TatComTime) - Convert.ToInt32(row.TatTime)) < 10)
                    {
                        e.Appearance.BackColor = Color.Yellow;
                    }
                    if (Convert.ToInt32(row.TatTime) > Convert.ToInt32(row.TatComTime))
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.Black;
                    }
                }
                if (row.RepStatus == 2 || row.RepStatus == 4)
                {
                    if (Convert.ToInt32(row.TatTime) > Convert.ToInt32(row.TatComTime))
                    {
                        e.Appearance.BackColor = Color.Red;
                        e.Appearance.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Green;
                    }
                }
            }
        }


        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            //屏蔽键盘上下页事件，变成上下翻病人资料前自动保存数据
            if (e.KeyCode == Keys.PageDown || e.KeyCode == Keys.PageUp)
            {
                e.Handled = true;
                ParentForm.Save();
                bool blnIsHostOrder = false;
                string pat_sid = "";
                if (this.CurrentPatient != null)
                {

                    pat_sid = this.CurrentPatient.RepSid.ToString();
                    //判断是否双向序号就取作为定位号
                    if (!string.IsNullOrEmpty(this.CurrentPatient.RepSerialNum))
                    {
                        blnIsHostOrder = true;
                        pat_sid = this.CurrentPatient.RepSerialNum.ToString();

                    }
                }
                if (e.KeyCode == Keys.PageDown)
                {
                    ParentForm.SetSID_AfterSaved(pat_sid, blnIsHostOrder);
                }
                else if (e.KeyCode == Keys.PageUp)
                {
                    ParentForm.SetSID_AfterSaved((int.Parse(pat_sid) - 2).ToString(), blnIsHostOrder);
                }
            }
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && CurrentPatient != null)
            {
                //EntityPidReportMain drtemp = gridViewPatientList.GetFocusedRow() as EntityPidReportMain;
                //gridViewPatientList.FocusedRowHandle
                // int[] rownumber = this.gridView1.GetSelectedRows();//获取选中行号；
                //DataRow row = this.gridView1.GetDataRow(rownumber[0]);//根据行号获取相应行的数据；

                if (!gridViewPatientList.IsRowSelected(gridViewPatientList.FocusedRowHandle))
                {
                    gridViewPatientList.SelectRow(gridViewPatientList.FocusedRowHandle);
                }
                else
                {
                    gridViewPatientList.UnselectRow(gridViewPatientList.FocusedRowHandle);
                }
            }
        }

        private void 危急值记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EntityPidReportMain drFoc = this.bsPatList.Current as EntityPidReportMain;

            if (drFoc != null &&
                    (
                        drFoc.RepUrgentFlag.ToString() != "0" ||
                        (drFoc.UrgentCount.ToString() != "0")
                    )
                )
            {
                bool isRAV = UserInfo.GetSysConfigValue("Lab_ReportCriticalValueInfo") == "是";
                if (isRAV)
                {
                    FrmPatientsExt frmPat = new FrmPatientsExt();
                    frmPat.PatId = drFoc.RepId.ToString();
                    frmPat.selectMode = !(drFoc.RepStatus.ToString() == "2" || drFoc.RepStatus.ToString() == "4");
                    frmPat.ShowDialog();

                    return;
                }
                bool isRCV = UserInfo.GetSysConfigValue("Lab_RecordCriticalValueInfo") != "是";
                if (isRCV && (drFoc.RepStatus.ToString() == "2" || drFoc.RepStatus.ToString() == "4"))
                {
                    FrmPatientsExt frmPat = new FrmPatientsExt();
                    frmPat.PatId = drFoc.RepId.ToString();
                    frmPat.selectMode = true;
                    frmPat.ShowDialog();
                }
                else
                {
                    FrmPatientsExt frmPat = new FrmPatientsExt();
                    frmPat.PatId = drFoc.RepId.ToString();
                    frmPat.selectMode = false;
                    frmPat.ShowDialog();
                }
            }
            else
                MessageDialog.ShowAutoCloseDialog("该标本无危急值结果");
        }


        /// <summary>
        /// 菜单事件-允许发送中期报告
        /// </summary>
        public event EventHandler OnMenuAllowMidReportClicked;
        private void menuAllowMidReport_Click(object sender, EventArgs e)
        {
            if (OnMenuAllowMidReportClicked != null)
            {
                this.OnMenuAllowMidReportClicked(sender, e);
            }
        }


        private void 批量添加组合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EntityPidReportMain dr = this.bsPatList.Current as EntityPidReportMain;
            if (dr != null)
            {
                FrmBatchAddCombine frmBAC = new FrmBatchAddCombine();
                List<string> listPat = new List<string>();
                listPat.Add(dr.RepItrId.ToString());
                listPat.Add(dr.RepInDate.ToString());
                listPat.Add(TypeID);
                frmBAC.list = listPat;
                frmBAC.ShowDialog();
            }
        }

        //过滤条码状态
        private void RoundPanelGroup_RoundPanelGroupClick(object sender, EventArgs e)
        {
            RoundPanel curRp = roundPanelGroup.GetCurRoundPanel();
            if (curRp.Tag == null)
                return;

            string value = curRp.Tag?.ToString();
            int pageIndex = ValueConvertHelper.IntParse(value, 0);
            SelectedPageChanged(pageIndex);
        }

        private void SelectedPageChanged(int pageIndex)
        {
            try
            {
                IsReLoadData = true;
                switch (pageIndex)
                {
                    case 0:
                        cmbGridFilterPatientState.EditValue = "-1";
                        break;
                    case 1:
                        cmbGridFilterPatientState.EditValue = "2";
                        break;
                    case 2:
                        cmbGridFilterPatientState.EditValue = "2";
                        break;
                    case 3:
                        if (UserInfo.GetSysConfigValue("Lab_ShowFirstAuditButton") == "是")
                            cmbGridFilterPatientState.EditValue = "1";
                        else
                            cmbGridFilterPatientState.EditValue = "2";
                        break;
                    case 4:
                        cmbGridFilterPatientState.EditValue = "3";
                        break;
                    case 5:
                        cmbGridFilterPatientState.EditValue = "5";
                        break;
                    case 9:
                        cmbGridFilterPatientState.EditValue = "4";
                        break;
                    case 7:
                        cmbGridFilterPatientState.EditValue = "6";
                        break;
                    case 8:
                        cmbGridFilterPatientState.EditValue = "7";
                        break;
                    default:
                        break;
                }
                //切换状态后设置焦点行
                if (ConfigHelper.GetSysConfigValueWithoutLogin("ChangeItrIsFocusOnTheFirstRow") == "是")
                {
                    this.gridViewPatientList.FocusedRowHandle = 0;
                    int i = this.gridViewPatientList.RowCount;

                    EntityPidReportMain dr = this.CurrentPatient;
                    if (dr != null)
                    {
                        this.LocatePatientByPatID(dr.RepId.ToString(), true);
                    }
                }
            }
            finally
            {
                IsReLoadData = false;
            }
        }

        private void txtPatInstructment_onBeforeFilter()
        {
            //当前选中的物理组ID
            string currentSelectType = this.txtPatType.valueMember;
            List<EntityDicInstrument> itrList = CacheClient.GetCache<EntityDicInstrument>();
            //是否有物理组
            if (currentSelectType != null && currentSelectType.Trim(null) != string.Empty)
            {
                //根据仪器数据类型过滤出当前物理组的仪器
                if (ItrDataType == LIS_Const.InstmtDataType.Normal || ItrDataType == LIS_Const.InstmtDataType.Eiasa
                    || ItrDataType == LIS_Const.InstmtDataType.Rapid)
                {
                    if (UserInfo.GetSysConfigValue("HospitalName") == "汕头中心医院")
                        itrList = itrList.Where(i => i.ItrLabId == currentSelectType && (i.ItrReportType == LIS_Const.InstmtDataType.Normal || i.ItrReportType == LIS_Const.InstmtDataType.Eiasa || i.ItrReportType == LIS_Const.InstmtDataType.Rapid || i.ItrReportType == LIS_Const.InstmtDataType.Description)).ToList();
                    else
                        itrList = itrList.Where(i => i.ItrLabId == currentSelectType && (i.ItrReportType == LIS_Const.InstmtDataType.Normal || i.ItrReportType == LIS_Const.InstmtDataType.Eiasa || i.ItrReportType == LIS_Const.InstmtDataType.Rapid)).ToList();
                }
                else
                {
                    itrList = itrList.Where(i => i.ItrLabId == currentSelectType && i.ItrReportType == ItrDataType).ToList();
                }
            }
            else//没有：列出所有仪器(非细菌的)
            {
                //strFilter += " and 1=2";
                //根据仪器数据类型过滤出当前物理组的仪器
                if (ItrDataType == LIS_Const.InstmtDataType.Normal || ItrDataType == LIS_Const.InstmtDataType.Eiasa
                    || ItrDataType == LIS_Const.InstmtDataType.Rapid)
                {
                    if (UserInfo.GetSysConfigValue("HospitalName") == "汕头中心医院")
                        itrList = itrList.Where(i => i.ItrReportType == LIS_Const.InstmtDataType.Normal || i.ItrReportType == LIS_Const.InstmtDataType.Eiasa || i.ItrReportType == LIS_Const.InstmtDataType.Rapid || i.ItrReportType == LIS_Const.InstmtDataType.Description).ToList();
                    else
                        itrList = itrList.Where(i => i.ItrReportType == LIS_Const.InstmtDataType.Normal || i.ItrReportType == LIS_Const.InstmtDataType.Eiasa || i.ItrReportType == LIS_Const.InstmtDataType.Rapid).ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(ItrDataType))
                    {
                        itrList = itrList.Where(i => i.ItrReportType == ItrDataType).ToList();
                    }
                }
            }
            //当切换和不是当前仪器的实验组时 将仪器也置为空  防止出现仪器丢失问题
            if (currentSelectType != ProId)
            {
                this.txtPatInstructment.displayMember = "";
                this.txtPatInstructment.valueMember = "";
            }
            if (!UserInfo.isAdmin)
            {
                //非管理员：列出有权限的仪器
                if (UserInfo.sqlUserTypesFilter != string.Empty)
                {
                    itrList = itrList.Where(i => UserInfo.sqlUserTypesFilter.Contains(i.ItrLabId) && UserInfo.sqlUserItrs.Contains(i.ItrId)).ToList();
                }
                else
                {
                    itrList = new List<EntityDicInstrument>();
                }
                itrList = itrList.Where(i => string.IsNullOrEmpty(i.ItrStatus) || i.ItrStatus == "正常").ToList();
            }
            //防止多空白行出现
            List<EntityDicInstrument> listFilter = new List<EntityDicInstrument>();
            if (itrList.Count > 0)
            {
                listFilter = EntityManager<EntityDicInstrument>.ListClone(itrList);
            }
            this.txtPatInstructment.SetFilter(listFilter);
        }

        private void dtEnd_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.valueMember) ||
                string.IsNullOrEmpty(txtPatType.valueMember) || dtBegin.EditValue == null)
                return;

            RefreshPatients(false);
        }

        private void dtBegin_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.valueMember) ||
               string.IsNullOrEmpty(txtPatType.valueMember) || dtEnd.EditValue == null)
                return;

            RefreshPatients(false);
        }


        private void RptOrigin_RoundPanelGroupClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.valueMember)
              || string.IsNullOrEmpty(txtPatType.valueMember)
              || dtBegin.EditValue == null
              || dtEnd.EditValue == null)
                return;
            RefreshPatients(false);
        }


        private void txtPatInstructment_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            //选择仪器后如果物理组为空则填充当前仪器的物理组
            if (string.IsNullOrEmpty(this.txtPatType.valueMember))
            {
                string ctype_id = DictInstrmt.Instance.GetItrCTypeID(this.txtPatInstructment.valueMember);

                if (!string.IsNullOrEmpty(ctype_id))
                {
                    EntityDicPubProfession rowCType = DictType.Instance.GetCType(ctype_id);
                    ProId = ctype_id;
                    if (rowCType != null)
                    {
                        this.txtPatType.valueMember = ctype_id;
                        this.txtPatType.displayMember = rowCType.ProName;
                        txtPatType_ValueChanged(null, null);
                    }
                }
            }
            if (OnItrChanged != null)
            {
                OnItrChanged(sender, args);
            }
            int hostflag = DictInstrmt.Instance.GetItrHostFlag(txtPatInstructment.valueMember);
            this.ItrHostFlag = hostflag;
            RefreshPatients(false);
        }

        public bool isMRZ = false; //新增，默认值时不启动事件
        private void txtPatType_DisplayTextChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (!isMRZ)
                txtPatType_ValueChanged(null, null);
        }

        private void txtPatType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (OnTypeChanged != null)
            {
                OnTypeChanged(sender, args);

                txtPatInstructment_onBeforeFilter();
            }
        }


        /// <summary>
        /// 点击序号和样本号排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewPatientList_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column != null && (e.Column.FieldName == "RepSerialNum" || e.Column.FieldName == "RepSid") && e.Value1 != null && e.Value2 != null)
            {
                e.Handled = true;
                string s1 = e.Value1.ToString();
                string s2 = e.Value2.ToString();
                if (s1.Length > s2.Length)
                {
                    e.Result = 1;
                }
                else
                    if (s1.Length == s2.Length)
                {
                    e.Result = System.Collections.Comparer.Default.Compare(s1, s2);
                }
                else
                    e.Result = -1;
            }
        }

        private void 报告解读ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EntityPidReportMain drFoc = this.bsPatList.Current as EntityPidReportMain;
            if (drFoc != null)
            {
                new FrmReportSumInfo(drFoc.RepId).ShowDialog();
            }
        }

        /// <summary>
        /// rpAll,rpNonAudit,rpNonReport,rpUrgent,rpNunPrint,rpReCheck
        /// </summary>
        /// <param name="name"></param>
        /// <param name="visable"></param>
        public void SetRoundPanelVisble(string name, bool visable)
        {
            roundPanelGroup.SetRoundPanelVisble(name, visable);
        }
    }
}
