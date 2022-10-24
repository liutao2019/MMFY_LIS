using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.result.Interface;
using dcl.client.result.CommonPatientInput;
using dcl.client.frame.runsetting;
using dcl.root.logon;
using lis.client.control;
using dcl.client.result.UILogic;
using dcl.common;
using System.Collections;
using dcl.client.report;
using dcl.client.wcf;
using dcl.client.result.DictToolkit;
using dcl.client.common;
using System.Threading;
using dcl.interfaces;
using dcl.client.result.PatControl;
using DevExpress.XtraBars;
using lis.client.result;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result
{
    /// <summary>
    /// 检验报告基类,包含：病人列表子控件，病人基本信息控件，右侧
    /// </summary>
    public partial class FrmPatInputBaseNew : FrmCommon, IPatPanelConfig, IPatientList
    {
        string AuditWord = "审核";

        private bool isNewOrModify;

        #region Fields & Properties
        bool PatDateChanging = false;

        private bool _isnew = true;

        /// <summary>
        /// 用户是否有权修改病人信息
        /// </summary>
        public bool isCanModify = false;

        /// 病历号查询前面自动补零位数
        /// </summary>
        private string patInNoAutoAddZeroNum;

        /// <summary>
        /// 当前状态是否为新增
        /// </summary>
        public bool IsNew
        {
            get
            {
                return _isnew;
            }
            set
            {
                _isnew = value;
            }
        }

        private bool isNewSave = false;
        private bool isReLoadData = false;
        string currPatidForSave = string.Empty;
        /// <summary>
        /// 新增时获得焦点的控件
        /// </summary>
        Control FocusOnAddNewControl = null;

        /// <summary>
        /// 面板配置类
        /// </summary>
        PatInputRuntimeSetting UserCustomSetting = null;

        /// <summary>
        /// 临时变量：日期改变时用于记录之前的日期
        /// </summary>
        DateTime dtPrevPatDate;

        protected string curr_patsid = string.Empty;
        #endregion

        /// <summary>
        /// 实现IPatEnter接口的检验报告子类
        /// </summary>
        protected IPatEnter PatEnter;
        protected bool Lab_DisplaySamReturnButton = false;
        private bool checkSaveBeforeLeave = false;
        private bool CheckReceiveTimeAndPatdate = false;
        /// <summary>
        /// 病人信息表
        /// </summary>
        protected EntityPidReportMain PatInfo;

        /// <summary>
        /// 病人检验组合
        /// </summary>
        protected List<EntityPidReportDetail> listPatCombine;

        /// <summary>
        /// 具有此仪器权限的用户ID(随机)
        /// </summary>
        protected string strCanMsgItrUserId = "";

        /// <summary>
        /// 具有此仪器权限的用户姓名(随机)
        /// </summary>
        protected string strCanMsgItrUsername = "";

        /// <summary>
        /// 手工修改病人结果保存时需验证
        /// </summary>
        public bool manualModityResultNeedAudit = false;

        /// <summary>
        /// 开启当前显示病人资料和数据库是否一致检查
        /// </summary>
        public bool checkCurrentPatientInfo = false;

        private bool Lab_EnableNoBarCodeCheck = false;
        private string Lab_NoBarCodeCheckItrExpectList = "";

        /// <summary>
        /// 审核者或者报告者为空时不允许打印报告
        /// </summary>
        bool Lab_ReportCodeIsNullNotAllowPrint = false;

        /// <summary>
        /// 结果缓存，用于对比
        /// </summary>
        public List<EntityObrResult> ResultCache = null;

        public virtual bool showReport { get { return false; } }

        private string patidentity = string.Empty;
        public bool Lab_NoBarcodeNeedAuditCheek = false;
        public string Lab_NoBarCodeAuditCheckItrExList = "";


        private System.Windows.Forms.Timer clearPassWordTimer;

        /// <summary>
        /// 需第三次审核的仪器ID
        /// </summary>
        public string Lab_ThreeAuditItrIDs = string.Empty;

        string defaultSamId = string.Empty; //默认标本(标本类别)

        /// <summary>
        /// .ctor
        /// </summary>
        public FrmPatInputBaseNew()
        {
            try
            {
                InitializeComponent();
                if (DesignMode)
                    return;

                this.Load += new System.EventHandler(this.FrmPatInputBase_Load);
                this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPatInputBase_FormClosing);

                this.controlPatList.ParentFormType = this.GetType().Name;
                this.IsNew = true;
                InitBtnEvent();

                this.sysToolBar1.NotWriteLogButtonNameList.AddRange(new string[] { this.sysToolBar1.BtnSave.Name, this.sysToolBar1.BtnAudit.Name, this.sysToolBar1.BtnDelete.Name, this.sysToolBar1.BtnReport.Name, this.sysToolBar1.BtnUndoAudit2.Name, this.sysToolBar1.BtnUndoReport2.Name, this.sysToolBar1.BtnAdd.Name, this.sysToolBar1.BtnCopy.Name, this.sysToolBar1.BtnPrint.Name });
                this.Lab_DisplaySamReturnButton = UserInfo.GetSysConfigValue("Lab_DisplaySamReturnButton") == "是";
                Lab_EnableNoBarCodeCheck = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_EnableNoBarCodeCheck") == "是";
                Lab_NoBarCodeCheckItrExpectList =
                    ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeCheckItrExpectList");
                // 手工修改病人结果保存时需验证
                manualModityResultNeedAudit = UserInfo.GetSysConfigValue("ManualModityResult_NeedAudit") == "是";

                Lab_ThreeAuditItrIDs = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ThreeAuditItrIDs");

                Lab_ReportCodeIsNullNotAllowPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ReportCodeIsNullNotAllowPrint") == "是";
                CheckReceiveTimeAndPatdate = UserInfo.GetSysConfigValue("Other_CheckReceiveTimeAndPatdate") == "是";

                //开启当前显示病人资料和数据库是否一致检查
                checkCurrentPatientInfo = UserInfo.GetSysConfigValue("Audit_CheckCurrentPatientInfo") == "是";
                //开启修改病人的结果数据后，离开当前病人资料时提示保存修改
                checkSaveBeforeLeave = UserInfo.GetSysConfigValue("Lab_CheckSaveBeforeLeave") == "是";

                //获取用户是否有权修改病人信息的权限
                FrmCheckPassword check = new FrmCheckPassword();
                check.OperatorID = UserInfo.loginID;
                isCanModify = new PatEnterUILogic().canReportOnNoBarCode(check, "ModifyReportManagerInfo");
                //系统配置：不能修改报告管理信息[模式]
                if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManagerMode") == "gzzyy")
                {
                    isCanModify = new PatEnterUILogic().canReportOnNoBarCode(check, "ModifyReportManagerInfo");
                }
                patInNoAutoAddZeroNum = ConfigHelper.GetSysConfigValueWithoutLogin("LabQuery_PatInNoAutoAddZeroNum");

                controlPatList.ParentForm = this;
                controlPatList.SetToolEvent();
                //隐藏非报告仪器
                bool Lab_HideNotReportInstrmt = UserInfo.GetSysConfigValue("Lab_HideNotReportInstrmt") == "是";
                bool BC_EnableOvertimeMessage = UserInfo.GetSysConfigValue("BC_EnableOvertimeMessage") == "是";
                if (Lab_HideNotReportInstrmt)
                {
                    this.txtPatInstructment.SelectFilter = "isnull(itr_rep_ins,'1')<>'0'";
                }

                this.txtPatDate.Leave += new System.EventHandler(this.txtPatDate_MouseLeave);
                txtPatSampleType_onBeforeFilter();

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "FrmPatInputBase", ex.ToString());
                lis.client.control.MessageDialog.Show("加载界面失败", "错误");
            }
        }


        /// <summary>
        /// OnLoad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmPatInputBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            int minute = ConvertHelper.IntParse(LocalSetting.Current.Setting.CachePwTime, 0);
            if (minute > 0)
            {
                clearPassWordTimer = new System.Windows.Forms.Timer();
                clearPassWordTimer.Interval = minute * 60 * 1000;
                clearPassWordTimer.Tick += ClearPassWordTimer_Tick;
                clearPassWordTimer.Start();
            }

            this.txtPatInspetor.Load += new System.EventHandler(this.txtPatInspetor_Load);
            this.btnHistoryEXP.Click += new System.EventHandler(this.btnHistoryEXP_Click);
            this.txtPatInstructment.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.txtPatInstructment_ValueChanged);
            this.btnSelectPatExp.Click += new System.EventHandler(this.btnSelectPatExp_Click);
            this.fpat_exp2.EditValueChanged += new System.EventHandler(this.fpat_exp2_EditValueChanged);

            this.tabPatientInfo.SelectedTabPageIndex = 2;
            this.tabPatientInfo.SelectedTabPageIndex = 1;
            this.tabPatientInfo.SelectedTabPageIndex = 0;

            LoadUserSetting();

            //配置按钮
            sysToolBar1.SetToolButtonStyle(PatEnter.ToolBarStyle);

            sysToolBar1.BtnQuickEntry.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;


            sysToolBar1.BtnAnswer.Caption = "病历浏览";
            sysToolBar1.BtnCopy.Name = "资料复制";
            sysToolBar1.OnBtnPrintListClicked += new EventHandler(sysToolBar1_OnPrintPreviewClicked);
            sysToolBar1.OnBtnSinglePrintClicked += new EventHandler(sysToolBar1_OnBtnSinglePrintClicked);

            //控件数据初始化
            InitControlData();

            Lab_NoBarcodeNeedAuditCheek = UserInfo.GetSysConfigValue("Lab_NoBarcodeNeedAuditCheek") == "是";
            if (Lab_NoBarcodeNeedAuditCheek)
                Lab_NoBarCodeAuditCheckItrExList = UserInfo.GetSysConfigValue("Lab_NoBarCodeAuditCheckItrExList");

            if (this.txtPatInstructment.valueMember != null)
            {
                this.controlPatList.CanChangeItrHostFlag = false;
                this.controlPatList.CanChangeItrHostFlag = true;
            }
            //角色权限：检验者录入许可
            if (!UserInfo.HaveFunction(243))
            {
                txtPatInspetor.Readonly = true;
            }

            if (UserInfo.GetSysConfigValue("HostOrder_Editable") == "是")
            {
                txt_pat_host_order.Properties.ReadOnly = false;
            }

            //是否显示一审按钮
            if (UserInfo.GetSysConfigValue("Lab_ShowFirstAuditButton") == "否")
            {
                sysToolBar1.BtnAudit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                sysToolBar1.BtnUndo2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            //自定义审核显示语
            string auditWord = LocalSetting.Current.Setting.AuditWord;
            string reportWord = LocalSetting.Current.Setting.ReportWord;
            if (!string.IsNullOrEmpty(auditWord))
            {
                AuditWord = auditWord;
                ReplaceText(sysToolBar1.BtnAudit, "审核", AuditWord);
                ReplaceText(sysToolBar1.BtnUndoAudit, "审核", AuditWord);
                ReplaceText(sysToolBar1.BtnReport, "报告", reportWord);
                ReplaceText(sysToolBar1.BtnUndoReport, "报告", reportWord);
                labelControl591.Text = LocalSetting.Current.Setting.AuditWord + "者";
                labelControl601.Text = LocalSetting.Current.Setting.ReportWord + "者";
            }

            if (PatEnter != null && PatEnter.CombineEditor != null)
            {
                PatEnter.CombineEditor.CombineAdded += new CombineAddedEventHandler(CombineEditor_CombineAdded);
            }

            this.controlPatList.AutoScaleMode = AutoScaleMode.None;
        }

        /// <summary>
        /// 自动清除缓存审核人的密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPassWordTimer_Tick(object sender, EventArgs e)
        {
            LastReportOperationPw = string.Empty;
        }

        void InitBtnEvent()
        {
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.sysToolBar1_OnBtnAddClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnDeleteBatchClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteBatchClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.sysToolBar1_OnBtnRefreshClicked);
            this.sysToolBar1.OnBtnPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnPrintClicked);
            this.sysToolBar1.OnResultViewClicked += new System.EventHandler(this.sysToolBar1_OnResultViewClicked);
            this.sysToolBar1.OnSampleMonitorClicked += new System.EventHandler(this.sysToolBar1_OnSampleMonitorClicked);
            this.sysToolBar1.OnReportClicked += new System.EventHandler(this.sysToolBar1_OnReportClicked);
            this.sysToolBar1.OnUndoReportClicked += new System.EventHandler(this.sysToolBar1_OnUndoReportClicked);
            this.sysToolBar1.OnAuditClicked += new System.EventHandler(this.sysToolBar1_OnAuditClicked);
            this.sysToolBar1.OnUndoAuditClicked += new System.EventHandler(this.sysToolBar1_OnUndoAuditClicked);
            this.sysToolBar1.OnPrintPreviewClicked += new System.EventHandler(this.sysToolBar1_OnPrintPreviewClicked);
            this.sysToolBar1.OnBtnQualityImageClicked += new System.EventHandler(this.sysToolBar1_OnBtnQualityImageClicked);
            this.sysToolBar1.BtnUndoClick += new System.EventHandler(this.sysToolBar1_BtnUndoClick);
            this.sysToolBar1.BtnUndo2Click += new System.EventHandler(this.sysToolBar1_BtnUndo2Click);
            this.sysToolBar1.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);
            this.sysToolBar1.BtnAnswerClick += new System.EventHandler(this.sysToolBar1_BtnAnswerClick);
            this.sysToolBar1.BtnUploadVersionClick += new System.EventHandler(this.sysToolBar1_BtnUploadVersionClick);
        }

        void CombineEditor_CombineAdded(object sender, string com_id, int com_seq)
        {
            IsDataChange = false;
            List<EntityDicCombine> listCombine = CacheClient.GetCache<EntityDicCombine>();
            string strSamId = listCombine.Where(w => w.ComId == com_id).ToList()[0].ComSamId;
            if (strSamId != null && strSamId != string.Empty)
            {
                EntityPidReportMain pat = bsPat.DataSource as EntityPidReportMain;
                pat.PidSamId = strSamId;
                txtPatSampleType.SelectByID(strSamId);
            }
        }

        /// <summary>
        /// 更新Text
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="oldText">旧Text</param>
        /// <param name="newText">新Text</param>
        private void ReplaceText(BarItem control, string oldText, string newText)
        {
            control.Caption = control.Caption.Replace(oldText, newText);
        }

        /// <summary>
        /// 初始化控件数据
        /// </summary>
        public void InitControlData()
        {
            //设置默认检验人
            SetDefaultInspector();

            #region 设置默认组别和默认仪器


            //设置默认物理组(根据权限)
            if (UserInfo.isAdmin)
            {
                //如果是管理员：直接设置默认物理组
                this.txtPatType.valueMember = UserInfo.defaultType;
                this.txtPatType.displayMember = UserInfo.defaultTypeName;
                PatEnter.CombineEditor.CTypeID = UserInfo.defaultType;
            }
            else
            {
                //用户配置中的默认物理组
                string defTypeID = UserInfo.defaultType;
                string defTypeName = UserInfo.defaultTypeName;

                if (string.IsNullOrEmpty(defTypeID))
                {
                    defTypeID = LocalSetting.Current.Setting.CType_id;
                    defTypeName = LocalSetting.Current.Setting.CType_name;
                }

                //不是管理员：查找当前用户是否有默认物理组的权限
                if (!string.IsNullOrEmpty(defTypeID))
                {
                    List<EntityUserLab> drs = UserInfo.listUserLab.Where(w => w.LabId == defTypeID).ToList();
                    if (drs.Count > 0)
                    {
                        this.txtPatType.valueMember = defTypeID;
                        this.txtPatType.displayMember = defTypeName;
                        PatEnter.CombineEditor.CTypeID = defTypeID;
                    }
                    else
                    {
                    }
                }
            }

            #endregion

            //设置录入日期为当天
            DateTime dtToday = ServerDateTime.GetServerDateTime();

            this.txtPatDate.EditValue = dtToday;
            this.dtPrevPatDate = dtToday;

            this.txtPatSampleDate.EditValue = dtToday;
            this.txtPatSDate.EditValue = dtToday;
            this.txtPatReachDate.EditValue = dtToday;
            this.txtPatApplyTime.EditValue = dtToday;
            this.txtPatRecDate.EditValue = dtToday;
            this.txtPatReceiveTime.EditValue = dtToday;
            this.txtPatReportDate.EditValue = dtToday;
            this.txtBirthday.EditValue = string.Empty;
        }

        /// <summary>
        /// 设置仪器的默认样本类型
        /// </summary>
        /// <param name="itrID"></param>
        protected virtual void SetItrDefalutSample()
        {
            List<EntityDicInstrument> list = CacheClient.GetCache<EntityDicInstrument>().Where(w => w.ItrId == this.txtPatInstructment.valueMember).ToList();
            if (list.Count > 0)
            {
                if (!string.IsNullOrEmpty(list[0].ItrSamId))
                {
                    string sam_id = list[0].ItrSamId;

                    List<EntityDicSample> listSampleType = this.txtPatSampleType.dtSource.Where(w => w.SamId == sam_id).ToList();

                    if (listSampleType.Count > 0 && !string.IsNullOrEmpty(listSampleType[0].SamName))
                    {
                        string sam_name = listSampleType[0].SamName;

                        this.txtPatSampleType.displayMember = sam_name;
                        this.txtPatSampleType.valueMember = sam_id;
                        defaultSamId = sam_id; //赋值给默认标本全局变量
                    }
                    else
                    {
                        this.txtPatSampleType.valueMember = null;
                        this.txtPatSampleType.displayMember = null;
                        defaultSamId = "";
                    }
                }
                else
                {
                    this.txtPatSampleType.valueMember = null;
                    this.txtPatSampleType.displayMember = null;
                    defaultSamId = null;
                }
            }
            else
            {
                this.txtPatSampleType.valueMember = null;
                this.txtPatSampleType.displayMember = null;
                defaultSamId = null;
            }
        }

        /// <summary>
        /// 设置仪器默认组合
        /// </summary>
        protected virtual void SetItrDefaultCombine()
        {
            string itr_id = this.txtPatInstructment.valueMember;
            if (!string.IsNullOrEmpty(itr_id))
            {
                string itrID = this.txtPatInstructment.valueMember;

                //获取仪器默认组合
                string itr_com_id = DictInstrmt.Instance.GetItrComID(itrID);

                //如果仪器默认组合为空
                if (string.IsNullOrEmpty(itr_com_id))
                {
                    if (PatEnter.CombineEditor != null)
                    {
                        //判断面板设置中套餐是否勾选自动记忆
                        if (UserCustomSetting != null && !UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtCombine"))
                        {
                            PatEnter.CombineEditor.Reset();
                            PatEnter.CombineEditor.RefreshEditBoxText();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(PatEnter.CombineEditor.Text))
                            {
                                string[] combinename = PatEnter.CombineEditor.Text.Split('+');
                                List<string> combines = new List<string>();
                                foreach (string comb in combinename)
                                {
                                    combines.Add(comb.Trim());
                                }
                                List<EntityDicCombine> cacheCombine = CacheClient.GetCache<EntityDicCombine>();
                                List<EntityDicCombine> combine = (from x in cacheCombine where combines.Contains(x.ComName) select x).ToList();
                                if (combine != null && combine.Count > 0)
                                {
                                    foreach (EntityDicCombine com in combine)
                                    {
                                        PatEnter.CombineEditor.AddCombine(com.ComId);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //新增时先清除之前的结果
                    if (this.IsNew && PatEnter.CombineEditor != null)
                        PatEnter.CombineEditor.Reset();

                    PatEnter.CombineEditor.AddCombine(itr_com_id);
                }
            }
            else
            {
                if (PatEnter.CombineEditor != null)
                {
                    PatEnter.CombineEditor.Reset();
                    PatEnter.CombineEditor.RefreshEditBoxText();
                }
            }
        }

        /// <summary>
        /// 加载用户式样配置
        /// </summary>
        private void LoadUserSetting()
        {
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load(this.GetType().Name, string.Empty, UserInfo.loginID);
            ApplySetting(setting);
        }

        public void ApplySetting(PatInputRuntimeSetting setting)
        {
            this.UserCustomSetting = setting;
            ApplySetting();

        }

        /// <summary>
        /// 应用用户式样配置
        /// </summary>
        /// <param name="UserCustomSetting"></param>
        private void ApplySetting()
        {
            this.controlPatList.ApplySetting(this.UserCustomSetting.PatListPanel);

            bool bAllowCustomizePanel = (UserInfo.GetSysConfigValue(LIS_Const.SystemConfigurationCode.AllowCustomizePanel) == "是");

            #region 加载layoutcontrol式样

            //设置新增时获得焦点的控件
            if (FocusOnAddNewControl == null)
            {
                FocusOnAddNewControl = this.txtPatSid;
            }

            if (FocusOnAddNewControl != null)
            {
                this.ActiveControl = FocusOnAddNewControl;
                FocusOnAddNewControl.Focus();
            }

            #endregion

            foreach (Control control in this.pcBaseInfo.Controls)
            {
                if (control != null && control.Visible == true)
                {
                    if (control.Name == this.UserCustomSetting.PatInfoPanel.FindFocusOnAddNewControlName())
                    {
                        FocusOnAddNewControl = control;
                    }
                }
            }
            PatEnter.ApplyCustomSetting(this.UserCustomSetting);
        }

        /// <summary>
        /// 设置默认检验人
        /// </summary>
        public void SetDefaultInspector()
        {
            //设置默认检验人
            List<EntitySysUser> drPat_chks = this.txtPatInspetor.dtSource.Where(w => w.LoginId == UserInfo.loginID).ToList();
            if (drPat_chks.Count > 0)
            {
                this.txtPatInspetor.valueMember = UserInfo.loginID;
            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        private void ResetAll()
        {
            isDataFill = true;
            this.controlPatList.Reset();
            ucTimeLine1.Reset();
            if (this.PatInfo != null)
            {
                this.PatInfo = new EntityPidReportMain();
            }

            if (this.listPatCombine != null)
            {
                this.listPatCombine.Clear();
            }

            PatEnter.Reset();

            DateTime dtToday = ServerDateTime.GetServerDateTime();
            this.txtPatSampleDate.EditValue = dtToday;
            if (UserInfo.GetSysConfigValue("AlwaysKeepSendDate") != "是") //如果不强制保存送检时间
                this.txtPatSDate.EditValue = dtToday;
            this.txtPatReachDate.EditValue = dtToday;
            this.txtPatApplyTime.EditValue = dtToday;
            this.txtPatRecDate.EditValue = dtToday;
            this.txtPatReportDate.EditValue = dtToday;
            SetDefaultInspector();
            isDataFill = false;
        }

        /// <summary>
        /// 新增时专用-查找病人
        /// </summary>
        /// <param name="addnewAfterSearch"></param>
        /// <param name="patID"></param>
        protected virtual void SearchPatients(bool addnewAfterSearch, string patID)
        {
            try
            {
                isNewSave = true;
                currPatidForSave = patID;
                SearchPatients(addnewAfterSearch);
            }
            finally
            {
                currPatidForSave = string.Empty;
                isNewSave = false;
            }
        }

        /// <summary>
        /// 查找病人
        /// </summary>
        protected virtual void SearchPatients(bool addnewAfterSearch)
        {
            try
            {
                isReLoadData = true;

                if (this.txtPatDate.EditValue != null)
                {
                    //查找病人
                    this.controlPatList.RefreshPatients();
                    if (addnewAfterSearch && !Compare.IsEmpty(this.txtPatInstructment.valueMember))
                    {
                        this.OnAddNew();
                        SIDChanged();
                    }
                }
                else
                {
                }

                //******************************************************************************//
                //设置焦点行问题；以下不应该这样写，不应该通过这种方式传入系统配置项，
                //实现(改变仪器后是否在首行、审核后是否在首行)，应该传入1个bool类型的参数，在调用前读取系统配置传入
                if (!isNewSave)//新增保存时不处理
                {
                    setControlPatListFocus("ChangeItrIsFocusOnTheFirstRow");
                }
                //******************************************************************************//
            }

            finally
            {
                isReLoadData = false;
            }

        }

        /// <summary>
        /// 上下键,转跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtPatSid)
            {
                try
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        if (this.IsNew || UserInfo.GetSysConfigValue("Lab_AllsampleLocatePatient") == "是")
                        {
                            string pat_sid = this.txtPatSid.Text;
                            if (UserInfo.GetSysConfigValue("Lab_AllsampleLocatePatient") == "是")
                            {
                                this.controlPatList.RefreshPatients();
                                bool blnRes = this.controlPatList.LocatePatient(pat_sid);

                                //如果没有数据，直接新增事件
                                if (!blnRes)
                                {
                                    OnAddNew();
                                    this.txtPatSid.Text = pat_sid;
                                }

                            }

                        }
                        else
                        {
                            if (this.txtPatSid.Text != this.curr_patsid)
                            {
                                //判断新修改的样本号是否在病人列表中存在
                                if (this.controlPatList.ExistSID(this.txtPatSid.Text))//存在
                                {
                                    lis.client.control.MessageDialog.Show("样本号已存在", "提示");
                                    this.txtPatSid.Focus();
                                    this.txtPatSid.EditValue = curr_patsid;
                                }
                                else//不存在
                                {
                                    if (lis.client.control.MessageDialog.Show(string.Format("是否将原样本号[{0}]修改为新样本[{1}]？", curr_patsid, this.txtPatSid.Text), "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        this.txtPatSid.Focus();
                                        this.txtPatSid.EditValue = curr_patsid;
                                    }
                                    else
                                    {
                                        curr_patsid = this.txtPatSid.Text;
                                        sampleIdChange = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "txtPatSid_Leave", ex.ToString());
                }
            }


            KeysHelper.Jump(e.KeyCode);
        }

        /// <summary>
        /// 新增
        /// </summary>
        public void OnAddNew()
        {
            OnAddNew(true, true);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="IsMoveLast"></param>
        public void OnAddNew(bool IsMoveLast, bool isWarning)
        {
            if (controlPatList.ItrID == null || controlPatList.ItrID.Trim(null) == string.Empty)
            {
                if (UserInfo.GetSysConfigValue("Lab_TypeSelect") != "是")
                {
                    this.controlPatList.FocusItr();
                    if (isWarning)
                        lis.client.control.MessageDialog.Show("请选择仪器", "提示");
                    return;
                }
            }
            patidentity = null;


            if (this.listPatCombine != null)
            {
                this.listPatCombine.Clear();
            }
            isDataFill = true;
            this.IsNew = true;

            EntityPidReportMain patient = new EntityPidReportMain();

            if (UserCustomSetting == null)
                return;
            //根据设置保留还是记忆界面原有值
            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("textAgeInput1"))
            {
                patient.PidAgeExp = this.textAgeInput1.AgeValueText;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatName"))
            {
                patient.PidName = this.txtPatName.Text;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatID"))
            {
                patient.PidInNo = this.txtPatID.Text;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatBedNo"))
            {
                patient.PidBedNo = this.txtPatBedNo.Text;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDiag"))
            {
                patient.PidDiag = this.txtPatDiag.valueMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSource"))
            {
                patient.PidSrcId = this.txtPatSource.valueMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSampleState"))
            {
                patient.PidRemark = this.txtPatSampleState.displayMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDoc"))
            {
                patient.PidDoctorCode = this.txtPatDoc2.displayMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPat_social_no"))
            {
                patient.PidSocialNo = this.txtPat_social_no.Text;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPat_chk_purpose"))
            {
                patient.PidPurpId = this.txtPat_chk_purpose.valueMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSex"))
            {
                patient.PidSex = this.txtPatSex.valueMember;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatIdType")) //如果不自动记忆ID类型 
            {
                if (UserInfo.GetSysConfigValue("UseBarcode") == "是")//如果启用条码,就默认跳到条码号
                {
                    this.txtPatIdType.SelectByDispaly("条码号");
                    patient.PidIdtId = txtPatIdType.valueMember;
                }
            }
            else
            {
                patient.PidIdtId = txtPatIdType.valueMember;

                string ori_id = DictPatNumberType.Instance.GetOriID_byNoType(this.txtPatIdType.valueMember);

                patient.PidSrcId = ori_id;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatChkType"))
            {
                patient.RepCtype = txtPatChkType.valueMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDeptId"))
            {
                patient.PidDeptId = txtPatDeptId.valueMember;
                patient.PidDeptName = txtPatDeptId.displayMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatFeeType"))
            {
                patient.PidInsuId = txtPatFeeType.valueMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSamRem"))
            {
                patient.SampRemark = txtPatSamRem.valueMember;
            }

            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatInspetor") || string.IsNullOrEmpty(txtPatInspetor.valueMember))
            {
                patient.RepCheckUserId = txtPatInspetor.valueMember;
            }

            DateTime dtToday = ServerDateTime.GetServerDateTime();
            patient.RepInDate = dtToday;
            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDate"))
            {
                patient.RepInDate = Convert.ToDateTime(txtPatDate.EditValue);
            }
            //防止检验时间出现0001/01/01
            if (patient.RepInDate <= Convert.ToDateTime("1900-01-01 0:00:00") || Convert.ToDateTime(this.txtPatDate.EditValue) <= Convert.ToDateTime("1900-01-01 0:00:00"))
            {
                patient.RepInDate = dtToday;
                this.txtPatDate.EditValue = dtToday;
            }
            this.controlPatList.AddNew();
            PatEnter.AddNew();
            SetItrDefaultCombine();

            string strNextID = "";
            //系统配置：细菌报告采用当前标本号加1
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_BtUseCurrentSamAddOne") == "是")
            {
                List<EntityPidReportMain> drpat = this.controlPatList.GetALLPatients();
                if (drpat != null && drpat.Count > 0 && !string.IsNullOrEmpty(txtPatSid.Text))
                {
                    List<EntityPidReportMain> dtpat = new List<EntityPidReportMain>();
                    foreach (EntityPidReportMain item in drpat)
                    {
                        dtpat.Add(item);
                    }
                    List<EntityPidReportMain> drtempSel = dtpat.Where(i => i.PatSidInt == Convert.ToInt32(txtPatSid.Text)).ToList();
                    if (drtempSel.Count > 0)
                    {
                        if (dtpat.Where(i => i.PatSidInt == (Convert.ToInt64(drtempSel[0].PatSidInt.ToString()) + 1)).ToList().Count <= 0)
                        {
                            patient.RepSid = (Convert.ToInt64(drtempSel[0].PatSidInt.ToString()) + 1).ToString();
                        }
                        else
                        {
                            strNextID = this.GetMaxSID();
                            patient.RepSid = strNextID;
                        }
                    }
                    else
                    {
                        strNextID = this.GetMaxSID();
                        patient.RepSid = strNextID;
                    }
                }
                else
                {
                    strNextID = this.GetMaxSID();
                    patient.RepSid = strNextID;
                }
            }
            else
            {
                strNextID = this.GetMaxSID();
                patient.RepSid = strNextID;
            }

            if (UserInfo.GetSysConfigValue("AlwaysKeepSendDate") != "是") //如果不强制保存送检时间
            {
                patient.SampSendDate = dtToday;
            }

            patient.SampCheckDate = dtToday.AddMinutes(1);//检验时间为当前时间+1分钟
            patient.SampApplyDate = dtToday;
            patient.SampCollectionDate = dtToday;
            patient.SampReceiveDate = dtToday;
            patient.SampReachDate = dtToday;
            patient.PidAddmissTimes = 0;

            string defaultSampleState = UserInfo.GetSysConfigValue("DefaultSampleState");

            this.txtPatSampleState.SelectByDispaly(defaultSampleState);
            patient.PidRemark = txtPatSampleState.displayMember;
            this.txtPatChkType.SelectByID("1");
            patient.RepCtype = txtPatChkType.valueMember;
            if (FocusOnAddNewControl != null)
            {
                PatDateChanging = true;
                this.ActiveControl = FocusOnAddNewControl;
                FocusOnAddNewControl.Focus();
                PatDateChanging = false;
            }

            if (IsMoveLast)
            {
                this.controlPatList.ScrollToBottom();
            }

            isDataFill = false;

            //新增 添加默认仪器标本 2018-03-22
            if (!string.IsNullOrEmpty(defaultSamId) && string.IsNullOrEmpty(patient.PidSamId))
            {
                patient.PidSamId = defaultSamId;
            }
            else {
                if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSampleType"))
                {
                    patient.PidSamId = txtPatSampleType.valueMember;
                }
            }
            bsPat.DataSource = patient;
        }
        bool isChanging = false;
        private void controlPatList1_PatientChanging(object sender, ControlPatListNew.PatientChangingEventArgs args)
        {
            //当前是否修改过结果信息
            bool temp_currentIsPatResultVChange = PatResult.IsPatResultVChange || PatResultDoubleColumn.IsPatResultDoubleColVChange;

            if (checkSaveBeforeLeave && (IsDataChange || temp_currentIsPatResultVChange) && controlPatList.CurrentPatient != null && !controlPatList.IsReLoadData && !isNewSave && !isReLoadData)
            {
                string pat_id = args.Prev_PatId;
                if (args.Prev_PatId != args.Pat_ID && !PatEnter.CheckResultBeforeAction(pat_id, false))
                {
                    if (MessageDialog.Show("当前资料或结果未保存，是否保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        isChanging = true;
                        Save();
                        isChanging = false;
                        args.Cancel = false;
                    }
                }
            }
        }

        /// <summary>
        /// 病人索引中选择的病人改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void controlPatList1_PatientChanged(object sender, dcl.client.result.PatControl.ControlPatListNew.PatientChangedEventArgs args)
        {
            this.curr_patsid = args.Pat_Data.RepSid.ToString();
            patidentity = null;
            //优化保存相关
            if (!isNewSave || string.IsNullOrEmpty(currPatidForSave) || (isNewSave && args.Pat_ID == currPatidForSave))
            {
                this.RefreshPatientDetails(args.Pat_ID);
                currPatidForSave = string.Empty;
            }

            //****************************************************
            //加载报告者、审核者、审核时间等控件数据
            this.txtAuditName.Text = args.Pat_Data.PidChkName.ToString();
            this.txtAuditTime.Text = args.Pat_Data.RepAuditDate?.ToString();
            this.txtReportName.Text = args.Pat_Data.BgName.ToString();
            //this.txt_dept_tel.Text = args.Pat_Data.DeptTel;
            //****************************************************

            //*******************************************************************
            //判断是否有权修改检验报告管理信息
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManager") == "是")
            {
                if (!isCanModify)
                {
                    if (!IsNew)
                    {
                        //系统配置：不能修改报告管理信息[模式]
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManagerMode") == "gzzyy")
                        {
                            if (!string.IsNullOrEmpty(this.txtPatBarCode.Text))
                            {
                                setIsModify(false);
                            }
                        }
                        else
                        {
                            setIsModify(false);
                        }
                    }
                }
                else
                {
                    setIsModify(true);
                }
            }
            else
            {
                setIsModify(true);
            }
            IsDataChange = false;
        }

        /// <summary>
        /// 刷新病人信息
        /// </summary>
        /// <param name="patID"></param>
        protected virtual void RefreshPatientDetails(string patID)
        {
            try
            {
                IsDataChange = false;
                //子类加载病人数据
                PatEnter.LoadPatientData(patID, ref this.PatInfo, ref this.listPatCombine);

                if (frmHistory != null && frmHistory.Visible)
                    ShowHistoryExp();

                bDataIsBinding = true;
                bAllowFirePatDept_ValueChanged = false;

                //绑定病人基本资料
                this.bsPat.DataSource = this.PatInfo;

                //设置体检ID号为只读
                if(this.PatInfo.PidSrcId == "109" && UserInfo.GetSysConfigValue("IsSetPidReadOnly") == "是")
                {
                    this.txtPatID.Properties.ReadOnly = true;
                    this.txtPatIdType.Readonly = true;
                }
                else
                {
                    this.txtPatIdType.Readonly = false;
                    this.txtPatID.Properties.ReadOnly = false;
                }

                try
                {
                    this.txtPatFeeType.SelectByID(PatInfo.PidInsuId);
                    if (string.IsNullOrEmpty(txtPatDoc2.displayMember))
                    {
                        this.txtPatDoc2.displayMember = PatInfo.PidDocName;
                    }
                    txtPatDate.EditValue = Convert.ToDateTime(PatInfo.RepInDate).Date;
                }
                catch { }

                try
                {
                    FillTimeLine(PatInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                #region 填充--病人扩展资料信息

                this.txtPat_BCRQ.EditValue = null;//B超时间
                this.txtBirthday.EditValue = null;//出生日期
                this.txtPat_race.EditValue = string.Empty;
                this.txtPat_birth_number.EditValue = string.Empty;
                this.txtPat_diabetes_mellitus.EditValue = string.Empty;
                this.txtPat_smoke.EditValue = string.Empty;
                this.txtPat_last_menstrual_period.EditValue = string.Empty;
                this.txtPat_YCQML.EditValue = string.Empty;
                this.txtPat_lmp_weeks.EditValue = string.Empty;
                this.txtPatInformDate.EditValue = null;//通知日期
                this.txt_dept_tel.Text = string.Empty;//科室电话
                this.txtBirthday.EditValue = PatInfo.PidBirthday; //listPatExt[0].PidBirthday.Value; 出生日期              

                //系统配置:是否使用病人扩展表资料
                if (UserInfo.GetSysConfigValue("IsUse_patients_ext") == "是")
                {
                    //获取病人扩展资料信息--辅助信息
                    List<EntityDicPidReportMainExt> listPatExt = new ProxyPidReportMainExt().Service.GetPatientExtDataByPatID(patID);
                    if (listPatExt != null && listPatExt.Count > 0)
                    {
                        //通知医生时间
                        if (listPatExt[0].PatInformDate != null && !string.IsNullOrEmpty(listPatExt[0].PatInformDate.ToString()))
                        {
                            this.txtPat_BCRQ.EditValue = listPatExt[0].PatInformDate.ToString();
                        }

                        if (!string.IsNullOrEmpty(listPatExt[0].PatRace))
                        {
                            this.txtPat_race.EditValue = listPatExt[0].PatRace;
                        }

                        if (!string.IsNullOrEmpty(listPatExt[0].PatBirthNumber))
                        {
                            this.txtPat_birth_number.EditValue = listPatExt[0].PatBirthNumber;
                        }

                        if (string.IsNullOrEmpty(listPatExt[0].PatDiabetesMellitus))
                        {
                            this.txtPat_diabetes_mellitus.EditValue = listPatExt[0].PatDiabetesMellitus;
                        }
                        if (!string.IsNullOrEmpty(listPatExt[0].PatSmoke))
                        {
                            this.txtPat_smoke.EditValue = listPatExt[0].PatSmoke;
                        }
                        if (!string.IsNullOrEmpty(listPatExt[0].PatLastMenstrualPeriod))
                        {
                            this.txtPat_last_menstrual_period.EditValue = listPatExt[0].PatLastMenstrualPeriod;
                        }
                        if (!string.IsNullOrEmpty(listPatExt[0].PatBCRQ))
                        {
                            this.txtPat_BCRQ.EditValue = listPatExt[0].PatBCRQ;
                        }
                        if (!string.IsNullOrEmpty(listPatExt[0].PatYCQML))
                        {
                            this.txtPat_YCQML.EditValue = listPatExt[0].PatYCQML;
                        }
                        if (!string.IsNullOrEmpty(listPatExt[0].PatlmpWeeks))
                        {
                            this.txtPat_lmp_weeks.EditValue = listPatExt[0].PatlmpWeeks;
                        }
                        if (!string.IsNullOrEmpty(listPatExt[0].MsgDepTel))
                        {
                            this.txt_dept_tel.Text = listPatExt[0].MsgDepTel;
                        }
                        if (listPatExt[0].PatNoticeDate.HasValue)
                        {
                            this.txtPatInformDate.EditValue = listPatExt[0].PatNoticeDate.Value;
                        }
                    }
                }

                #endregion

                //注意事项--填充
                this.txtPatNotice.Text = "";
                if (txtPatNotice.Visible)
                {
                    EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(patID, false);
                    string StrTempNotice = new ProxySampMain().Service.SampMainQueryByBarId(patient.RepBarCode).SampRemark;
                    if (!string.IsNullOrEmpty(StrTempNotice)) this.txtPatNotice.Text = StrTempNotice;
                }
                if (!string.IsNullOrEmpty(this.txtPatDoc2.valueMember)
                    && string.IsNullOrEmpty(this.txtPatDoc2.displayMember))
                {
                    this.txtPatDoc2.SelectByID(this.txtPatDoc2.valueMember);
                }
                bDataIsBinding = false;
                bAllowFirePatDept_ValueChanged = true;

                //组合编辑框绑定病人检验组合
                PatEnter.CombineEditor.listRepDetail = this.listPatCombine;

                if (this.PatInfo != null)
                {
                    this.IsNew = false;
                    this.controlPatList.CancelAddNew();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, string.Format("RefreshPatientDetails({0})", patID), ex.ToString());
                lis.client.control.MessageDialog.Show("获取病人信息出错", "提示");
            }
            finally
            {
                IsDataChange = false;
            }
        }

        string DateMins(string dtstr1, string dtstr2)
        {
            DateTime dt1 = Convert.ToDateTime(dtstr1);
            DateTime dt2 = Convert.ToDateTime(dtstr2);

            var ts = dt1 - dt2;

            if (ts.Hours == 0 && ts.Days == 0)
            {
                return Math.Round(ts.TotalMinutes, 1).ToString() + "分";
            }
            else
            {
                if (ts.TotalDays > 2)
                {
                    return Math.Round(ts.TotalDays, 1).ToString() + "天";
                }

                if (ts.TotalHours > 24)
                {
                    return Math.Round(ts.TotalHours, 1).ToString() + "时";
                }

                return ts.Hours + "时 " + ts.Minutes + "分";

            }
        }

        private void FillTimeLine(EntityPidReportMain PatInfo)
        {
            if (PatInfo == null) return;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ProxyPidReportMain proxy = new ProxyPidReportMain();
            List<EntityTatOverTime> listOverTime = proxy.Service.GetPatTatOverTime(PatInfo.RepBarCode);
            List<EntityTimeLineParameters> listPar = new List<EntityTimeLineParameters>();
            string LASTDATE = "";
            if (!string.IsNullOrEmpty(PatInfo.SampReceiveDate.ToString())
                && !dic.ContainsKey(PatInfo.SampReceiveDate.ToString()))
            {
                LASTDATE = PatInfo.SampReceiveDate.ToString();
                EntityTimeLineParameters par = new EntityTimeLineParameters();
                par.DateName = "申请时间";
                par.DateTime = PatInfo.SampReceiveDate.ToString();
                par.OverTime = false;
                listPar.Add(par);
                dic.Add(PatInfo.SampReceiveDate.ToString(), "申请时间");
            }
            if (!string.IsNullOrEmpty(PatInfo.SampCollectionDate.ToString())
                 && !dic.ContainsKey(PatInfo.SampCollectionDate.ToString()))
            {
                EntityTimeLineParameters par = new EntityTimeLineParameters();
                par.DateTime = PatInfo.SampCollectionDate.ToString();
                par.OverTime = false;
                par.Status = "2";
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampCollectionDate.ToString(),
                        LASTDATE);
                    par.DateName = "采集时间" + "|" + mins;
                    dic.Add(PatInfo.SampCollectionDate.ToString(), "采集时间" + "|" + mins);
                }
                else
                {
                    par.DateName = "采集时间";
                    dic.Add(PatInfo.SampCollectionDate.ToString(), "采集时间");
                }
                listPar.Add(par);
                LASTDATE = PatInfo.SampCollectionDate.ToString();
            }

            if (!string.IsNullOrEmpty(PatInfo.SampSendDate.ToString())
                 && !dic.ContainsKey(PatInfo.SampSendDate.ToString()))
            {
                EntityTimeLineParameters par = new EntityTimeLineParameters();
                par.DateTime = PatInfo.SampSendDate.ToString();
                par.OverTime = false;
                par.Status = "3";
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampSendDate.ToString(),
                        LASTDATE);
                    par.DateName = "收取时间" + "|" + mins;
                    dic.Add(PatInfo.SampSendDate.ToString(), "收取时间" + "|" + mins);
                }
                else
                {
                    par.DateName = "收取时间";
                    dic.Add(PatInfo.SampSendDate.ToString(), "收取时间");
                }
                listPar.Add(par);
                LASTDATE = PatInfo.SampSendDate.ToString();
            }
            if (!string.IsNullOrEmpty(PatInfo.SampReachDate.ToString())
               && !dic.ContainsKey(PatInfo.SampReachDate.ToString()))
            {
                EntityTimeLineParameters par = new EntityTimeLineParameters();
                par.DateTime = PatInfo.SampReachDate.ToString();
                par.OverTime = false;
                par.Status = "4";
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampReachDate.ToString(),
                        LASTDATE);
                    par.DateName = "送达时间" + "|" + mins;
                    dic.Add(PatInfo.SampReachDate.ToString(), "送达时间" + "|" + mins);
                }
                else
                {
                    par.DateName = "送达时间";
                    dic.Add(PatInfo.SampReachDate.ToString(), "送达时间");
                }
                listPar.Add(par);
                LASTDATE = PatInfo.SampReachDate.ToString();
            }
            if (!string.IsNullOrEmpty(PatInfo.SampApplyDate.ToString())
               && !dic.ContainsKey(PatInfo.SampApplyDate.ToString()))
            {
                EntityTimeLineParameters par = new EntityTimeLineParameters();
                par.DateTime = PatInfo.SampApplyDate.ToString();
                par.OverTime = false;
                par.Status = "5";
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampApplyDate.ToString(),
                        LASTDATE);
                    par.DateName = "签收时间" + "|" + mins;
                    dic.Add(PatInfo.SampApplyDate.ToString(), "签收时间" + "|" + mins);
                }
                else
                {
                    par.DateName = "签收时间";
                    dic.Add(PatInfo.SampApplyDate.ToString(), "签收时间");
                }
                LASTDATE = PatInfo.SampApplyDate.ToString();
                listPar.Add(par);
            }
            if (!string.IsNullOrEmpty(PatInfo.SampCheckDate.ToString())
                && !dic.ContainsKey(PatInfo.SampCheckDate.ToString()))
            {
                EntityTimeLineParameters par = new EntityTimeLineParameters();
                par.DateTime = PatInfo.SampCheckDate.ToString();
                par.OverTime = false;
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampCheckDate.ToString(),
                        LASTDATE);
                    par.DateName = "检验时间" + "|" + mins;
                    dic.Add(PatInfo.SampCheckDate.ToString(), "检验时间" + "|" + mins);
                }
                else
                {
                    par.DateName = "检验时间";
                    dic.Add(PatInfo.SampCheckDate.ToString(), "检验时间");
                }
                LASTDATE = PatInfo.SampCheckDate.ToString();
                listPar.Add(par);
            }

            if (!string.IsNullOrEmpty(PatInfo.RepAuditDate.ToString())
              && !dic.ContainsKey(PatInfo.RepAuditDate.ToString()))
            {
                bool isskip = false;
                if (!string.IsNullOrEmpty(PatInfo.RepReportDate.ToString()))
                {
                    if (DateMins(PatInfo.RepReportDate.ToString(),
                        PatInfo.RepAuditDate.ToString()) == "0分")
                    {
                        isskip = true;
                    }
                }
                if (!isskip)
                {
                    EntityTimeLineParameters par = new EntityTimeLineParameters();
                    par.DateTime = PatInfo.RepAuditDate.ToString();
                    par.OverTime = false;
                    par.Status = "40";
                    if (!string.IsNullOrEmpty(LASTDATE))
                    {
                        string mins = DateMins(PatInfo.RepAuditDate.ToString(),
                            LASTDATE);
                        par.DateName = "审核时间" + "|" + mins;
                        dic.Add(PatInfo.RepAuditDate.ToString(), "审核时间" + "|" + mins);
                    }
                    else
                    {
                        par.DateName = "审核时间";
                        dic.Add(PatInfo.RepAuditDate.ToString(), "审核时间");
                    }
                    listPar.Add(par);
                    LASTDATE = PatInfo.RepAuditDate.ToString();
                }
                else {
                    EntityTimeLineParameters par = new EntityTimeLineParameters();
                    par.DateTime = PatInfo.RepAuditDate.ToString();
                    par.OverTime = false;
                    par.Status = "60";
                    if (!string.IsNullOrEmpty(LASTDATE))
                    {
                        string mins = DateMins(PatInfo.RepAuditDate.ToString(),
                            LASTDATE);
                        par.DateName = "审核时间" + "|" + mins;
                        dic.Add(PatInfo.RepAuditDate.ToString(), "审核时间" + "|" + mins);
                    }
                    else
                    {
                        par.DateName = "审核时间";
                        dic.Add(PatInfo.RepAuditDate.ToString(), "审核时间");
                    }
                    listPar.Add(par);
                    LASTDATE = PatInfo.RepAuditDate.ToString();
                }
            }
            if (!string.IsNullOrEmpty(PatInfo.RepReportDate.ToString())
             && !dic.ContainsKey(PatInfo.RepReportDate.ToString()))
            {
                EntityTimeLineParameters par = new EntityTimeLineParameters();
                par.DateTime = PatInfo.RepReportDate.ToString();
                par.OverTime = false;
                par.Status = "60";
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.RepReportDate.ToString(),
                        LASTDATE);
                    par.DateName = "报告时间" + "|" + mins;
                    dic.Add(PatInfo.RepReportDate.ToString(), "报告时间" + "|" + mins);
                }
                else
                {
                    par.DateName = "报告时间";
                    dic.Add(PatInfo.RepReportDate.ToString(), "报告时间");
                }
                listPar.Add(par);
                LASTDATE = PatInfo.RepReportDate.ToString();
            }

            //判断该节点是否超时
            foreach (EntityTimeLineParameters par in listPar)
            {
                List<EntityTatOverTime> list = listOverTime.FindAll(w => w.TatEndType.ToString() == par.Status);
                if (list.Count > 0)
                    par.OverTime = true;
            }

            ucTimeLine1.LoadData(listPar);
        }


        /// <summary>
        /// 随机获取此仪器权限的用户
        /// </summary>
        private void getCanMrgItrUser()
        {
            List<EntityUserInstrmt> listUserItr = new ProxyUserManage().Service.GetUserCanMgrIInstrmt(this.txtPatInstructment.valueMember);
            if (listUserItr != null && listUserItr.Count > 0)
            {
                Random rd = new Random();
                int intIndex = rd.Next(listUserItr.Count - 1);
                if (listUserItr[intIndex].UserLoginid == UserInfo.loginID)
                {
                    intIndex = rd.Next(listUserItr.Count - 1);

                }
                strCanMsgItrUserId = listUserItr[intIndex].UserLoginid;
                strCanMsgItrUsername = listUserItr[intIndex].UserName;
            }
            else
            {
                strCanMsgItrUserId = UserInfo.loginID;
                strCanMsgItrUsername = UserInfo.userName;
            }

        }

        string strPatInstructmentName = string.Empty;
        string strPatInstructmentValue = string.Empty;
        /// <summary>
        /// 仪器改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtPatInstructment_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            strPatInstructmentName = this.txtPatInstructment.displayMember;
            strPatInstructmentValue = this.txtPatInstructment.valueMember;
        }


        //仪器的通讯方式1=单向,2=双向
        int itrHostFlag = 1;

        /// <summary>
        /// 获取仪器下一个样本号
        /// </summary>
        /// <returns></returns>
        protected string GetMaxSID()
        {
            try
            {
                if (txtPatDate.EditValue == null) return "1";
                DateTime dtPatDate = (DateTime)this.txtPatDate.EditValue;
                string strInstructID = this.txtPatInstructment.valueMember;
                if (this.controlPatList.gridControl1.Focused)
                {
                    string currentSID = string.Empty;
                    if (!string.IsNullOrEmpty(this.txtPatSid.Text) && strInstructID != null && strInstructID.Trim(null) != string.Empty)
                    {
                        currentSID = (Convert.ToInt64(this.txtPatSid.Text) + 1).ToString();
                        string RepId = strInstructID + dtPatDate.ToString("yyyyMMdd") + currentSID;
                        EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(RepId, false);
                        if (patient == null)
                        {
                            return currentSID;
                        }
                    }

                }
                if (strInstructID != null && strInstructID.Trim(null) != string.Empty)
                {
                    string sid = new ProxyPidReportMain().Service.GetItrSID_MaxPlusOne(dtPatDate, strInstructID, true);
                    return sid;
                }
            }
            catch
            { }

            return "1";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            try
            {
                this.sysToolBar1.Focus();
                string pat_id = string.Empty;

                //获取当前病人记录的状态
                string currPatFlag = GetCurrentPatFlat();

                if (currPatFlag != LIS_Const.PATIENT_FLAG.Natural && currPatFlag != string.Empty)
                {
                    lis.client.control.MessageDialog.Show(string.Format("当前记录已{1}或已{0}，不能修改", AuditWord, LocalSetting.Current.Setting.ReportWord), "提示");
                }
                else
                {
                    if (BeforeSaveCheck())//保存前检查
                    {
                        //更新病人信息表
                        EntityPidReportMain patient;

                        if (IsNew)
                        {//新增
                            patient = new EntityPidReportMain();
                            patient = bsPat.DataSource as EntityPidReportMain;
                        }
                        else
                        {
                            if (bsPat == null || bsPat.Current == null)
                                return "";

                            patient = bsPat.Current as EntityPidReportMain;
                        }

                        if (patient == null)
                            return "";

                        FillUIValueToDataRow(patient);

                        //非检验者能否修改结果？
                        if (UserInfo.GetSysConfigValue("IsAllowNonCheckerChangeResult") == "否") 
                        {
                            if (patient.RepCheckUserId != UserInfo.loginID && !string.IsNullOrEmpty(patient.RepCheckUserId) && this.txtPatType.valueMember != "10047")
                            {
                                lis.client.control.MessageDialog.Show("当前登录用户非检验者，不允许修改结果", "提示");
                                return "";
                            }
                        }

                        if (!string.IsNullOrEmpty(patient.PidSex))
                        {
                            if (patient.PidSex == "-1")
                            {
                                patient.PidSex = null;
                            }
                        }

                        //*****判断是新增还是修改***********************************
                        //true为新增，false为修改
                        if (this.IsNew)
                        {
                            isNewOrModify = true;
                        }
                        else
                            isNewOrModify = false;

                        //************************************************************

                        //调用子类的保存方法

                        //if (patient.RepBarCode == patient.PidName)//粤核总码使用条码做完姓名生成
                        //{
                        //    EntitySampQC sampQC1 = new EntitySampQC();
                        //    sampQC1.SampYhsBarCode = patient.RepBarCode;
                        //    List<EntitySampMain> patientList = new ProxySampMain().Service.SampMainQuery(sampQC1);
                        //    EntityPatientQC qc = new EntityPatientQC();

                        //    if (patientList != null && patientList.Count > 0)
                        //    {
                        //        foreach (var patSqmpMain in patientList)
                        //        {
                        //            List<EntityPidReportMain> listPid = new List<EntityPidReportMain>();
                        //            qc.RepBarCode = patSqmpMain.SampBarCode;
                        //            listPid = new ProxyPidReportMain().Service.PatientQuery(qc);

                        //            if (listPid != null && listPid.Count > 0)
                        //            {
                        //                foreach (var pat in listPid)
                        //                {
                        //                    pat_id = PatEnter.Save(pat);
                        //                }
                                        
                        //            }
                        //        }
                        //    }
                        //}

                        pat_id = PatEnter.Save(patient);
                        if (!string.IsNullOrEmpty(pat_id))
                        {
                            #region 同时保存病人扩展表资料

                            //系统配置:是否使用病人扩展表资料
                            if (UserInfo.GetSysConfigValue("IsUse_patients_ext") == "是")
                            {
                                List<string> patExtColName = new List<string>();//列名
                                List<string> patExtColValue = new List<string>();//列值
                                EntityDicPidReportMainExt ext = new EntityDicPidReportMainExt();
                                ext.RepId = pat_id;
                                if (!string.IsNullOrEmpty(this.txtPat_BCRQ.EditValue?.ToString()))
                                {
                                    ext.PatInformDate = Convert.ToDateTime(this.txtPat_BCRQ.EditValue);
                                }
                                if (!string.IsNullOrEmpty(this.txtBirthday.EditValue?.ToString()))
                                {
                                    ext.PidBirthday = Convert.ToDateTime(this.txtBirthday.EditValue);
                                }
                                //List<EntityDicPidReportMainExt> listPatExt = new ProxyPidReportMainExt().Service.GetPatientExtDataByPatID(pat_id);
                                //if (listPatExt != null)
                                {
                                    ext.PatRace = this.txtPat_race.Text.Trim();
                                    ext.PatBirthNumber = this.txtPat_birth_number.Text.Trim();
                                    ext.MsgDepTel = this.txt_dept_tel.Text.Trim();
                                    ext.PatDiabetesMellitus = this.txtPat_diabetes_mellitus.Text.Trim();
                                    ext.PatSmoke = this.txtPat_smoke.Text.Trim();
                                    ext.PatLastMenstrualPeriod = this.txtPat_last_menstrual_period.Text.Trim();
                                    ext.PatBCRQ = this.txtPat_BCRQ.Text.Trim();
                                    ext.PatYCQML = this.txtPat_YCQML.Text.Trim();
                                    ext.PatlmpWeeks = this.txtPat_lmp_weeks.Text.Trim();
                                    if (!string.IsNullOrEmpty(this.txtPatInformDate.EditValue?.ToString()))
                                    {
                                        ext.PatNoticeDate = Convert.ToDateTime(this.txtPatInformDate.Text);
                                    }
                                }

                                //保存病人扩展资料
                                bool eoPatExt = new ProxyPidReportMainExt().Service.AddOrUpdatePatientExt(ext);
                                List<object> list = new List<object>();
                                list.Add(patExtColName);
                                list.Add(patExtColValue);
                                list.Add(pat_id);
                            }
                            #endregion

                        }

                        //*****************************************************************************//
                        if (pat_id != string.Empty && !this.IsNew && this.controlPatList.CurrentPatient != null && !isChanging)
                        {

                            this.controlPatList.CurrentPatient.PidName = this.PatInfo.PidName;
                            this.controlPatList.CurrentPatient.PidSex = this.PatInfo.PidSex;

                            string pat_age_exp = AgeConverter.TrimZeroValue(this.PatInfo.PidAgeExp);
                            pat_age_exp = AgeConverter.ValueToText(pat_age_exp);

                            this.controlPatList.CurrentPatient.PidAgeExp = pat_age_exp;

                            this.controlPatList.CurrentPatient.PidBedNo = this.PatInfo.PidBedNo;
                            this.controlPatList.CurrentPatient.SamName = this.PatInfo.SamName;

                            this.controlPatList.CurrentPatient.RepSid = this.PatInfo.RepSid;
                            this.controlPatList.CurrentPatient.PatSidInt = Convert.ToInt64(this.PatInfo.PatSidInt);

                            this.controlPatList.CurrentPatient.RepCtype = this.PatInfo.RepCtype;
                            this.controlPatList.CurrentPatient.PidComName = this.PatInfo.PidComName;

                            if (DictInstrmt.Instance.GetItrHostFlag(this.txtPatInstructment.valueMember) == 2)
                            {
                                this.controlPatList.CurrentPatient.RepSerialNum = this.PatInfo.RepSerialNum;
                            }

                            if (sampleIdChange) //更改了样本号
                            {
                                this.PatInfo.RepId = pat_id;
                                this.controlPatList.CurrentPatient.RepId = pat_id;
                                curr_patsid = txtPatSid.Text; //同步样本号
                                RefreshPatientDetails(pat_id);
                                sampleIdChange = false;
                            }
                        }
                    }
                }
                //单机版保存后自动新增 并将焦点至于ID号上
                if (UserInfo.GetSysConfigValue("Lab_EnableUpload") == "是")
                {
                    OnAddNew();
                    txtPatID.Focus();
                }

                //lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功");
                return pat_id;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "Save", ex.ToString());
                lis.client.control.MessageDialog.Show("保存失败", "错误");
                return null;
            }
        }

        /// <summary>
        /// 保存后设置下一个需要修改或新增的样本号
        /// </summary>
        /// <returns></returns>
        internal void SetSID_AfterSaved(string prev_sid)
        {
            SetSID_AfterSaved(prev_sid, false);
        }

        /// <summary>
        /// 保存后设置下一个需要修改或新增的样本号
        /// </summary>
        /// <returns></returns>
        internal void SetSID_AfterSaved(string prev_sid, bool SerialNumber)
        {
            try
            {

                isNewSave = true;
                IsDataChange = false;
                string current_sid = prev_sid;

                if (!string.IsNullOrEmpty(current_sid))
                {
                    Int64 currSid = 1;
                    Int64.TryParse(current_sid, out currSid);

                    Int64 nextSid = currSid + 1;

                    bool patientExist = this.controlPatList.LocatePatient(nextSid.ToString(), SerialNumber);

                    if (!patientExist)
                    {
                        bool blnSucceedHandle = this.controlPatList.LocatePatient(prev_sid, SerialNumber);//定位到最新样本号,并记忆此部分信息 12-7-31 add
                        this.OnAddNew(false, true);
                        this.txtPatSid.Text = nextSid.ToString();

                        this.SIDChanged();
                    }
                }
            }
            finally
            {
                IsDataChange = false;
                isNewSave = false;
            }
        }

        /// <summary>
        /// 填充界面数据到datarow
        /// </summary>
        /// <param name="drPat"></param>
        protected void FillUIValueToDataRow(EntityPidReportMain patient)
        {
            patient.RepSid = this.txtPatSid.Text;
            if (patient.RepInDate.Value.Date != Convert.ToDateTime(this.txtPatDate.EditValue))
            {
                patient.RepInDate = Convert.ToDateTime(this.txtPatDate.EditValue);
            }

            if (!(!IsNew && patient.RepItrId != string.Empty &&
                UserInfo.GetSysConfigValue("Lab_TypeSelect") == "是"))
                patient.RepItrId = this.txtPatInstructment.valueMember;

            patient.ItrName = this.txtPatInstructment.displayMember;

            patient.PidName = this.txtPatName.Text;
            patient.PidSex = this.txtPatSex.valueMember;

            patient.PidInsuId = this.txtPatFeeType.valueMember;//费用类别
            patient.PidDiag = this.txtPatDiag.displayMember;
            patient.SampRemark = this.txtPatSamRem.popupContainerEdit1.Text;//标本备注

            if (this.textAgeInput1.AgeToMinute < 0)
            {
                patient.PidAge = null;
            }
            else
            {
                patient.PidAge = this.textAgeInput1.AgeToMinute;
            }

            if (!string.IsNullOrEmpty(patidentity))
            {
                patient.PidIdentity = Convert.ToInt32(patidentity);
                patidentity = null;
            }
            patient.PidAgeExp = this.textAgeInput1.AgeValueText;

            #region  科室ID 保存时防止丢失操作
            string displayDeptName = this.txtPatDeptId.displayMember;
            this.txtPatDeptId.SelectByDispaly(this.txtPatDeptId.displayMember);
            patient.PidDeptId = this.txtPatDeptId.valueMember;
            if (!string.IsNullOrEmpty(displayDeptName) && string.IsNullOrEmpty(this.txtPatDeptId.displayMember))
                this.txtPatDeptId.displayMember = displayDeptName;
            patient.PidDeptName = this.txtPatDeptId.displayMember;
            #endregion

            patient.PidWardId = this.txt_pat_ward_id.Text;
            patient.PidWardName = this.txt_pat_ward_name.Text;

            patient.PidIdtId = this.txtPatIdType.valueMember;
            if (this.txtPatID.EditValue != null && !string.IsNullOrEmpty(this.txtPatID.EditValue.ToString()))
            {
                patient.PidInNo = this.txtPatID.EditValue.ToString();
            }
            if (this.txtPatBedNo.EditValue != null)
            {
                patient.PidBedNo = this.txtPatBedNo.EditValue.ToString();
            }
            //*********************************************************
            //让用户输入标本状态时按照自定义输入显示保存
            patient.PidRemark = this.txtPatSampleState.displayMember;
            //*********************************************************

            //辅助信息
            if (this.fpat_work.EditValue != null)
            {
                patient.PidWork = this.fpat_work.EditValue.ToString();
            }

            if (this.fpat_tel.EditValue != null)
            {
                patient.PidTel = this.fpat_tel.EditValue.ToString();
            }
            if (this.fpat_email.EditValue != null)
            {
                patient.PidEmail = this.fpat_email.EditValue.ToString();
            }
            if (this.fpat_unit.EditValue != null)
            {
                patient.PidUnit = this.fpat_unit.EditValue.ToString();
            }
            if (this.fpat_address.EditValue != null)
            {
                patient.PidAddress = this.fpat_address.EditValue.ToString();
            }
            if (this.fpat_pre_week.EditValue != null)
            {
                patient.PidPreWeek = this.fpat_pre_week.EditValue.ToString();
            }
            if (this.fpat_height.EditValue != null)
            {
                patient.PidHeight = this.fpat_height.EditValue.ToString();
            }
            if (this.fpat_weight.EditValue != null)
            {
                patient.PidWeight = this.fpat_weight.EditValue.ToString();
            }
            if (this.fpat_comment.EditValue != null)
            {
                patient.RepComment = this.fpat_comment.EditValue.ToString();
            }
            patient.PidSamId = this.txtPatSampleType.valueMember;//样本类别
            patient.PidPurpId = this.txtPat_chk_purpose.valueMember;//检验目的

            patient.PidDoctorCode = this.txtPatDoc2.valueMember;
            patient.PidDocName = this.txtPatDoc2.displayMember;


            //如果检验者为空，则默认为登录者
            if (string.IsNullOrEmpty(this.txtPatInspetor.valueMember))
            {
                patient.RepCheckUserId = UserInfo.loginID;
            }
            else
            {
                patient.RepCheckUserId = this.txtPatInspetor.valueMember;
            }

            patient.RepCtype = this.txtPatChkType.valueMember;

            patient.PidComName = this.PatEnter.CombineEditor.Text;

            if (this.txtBirthday.EditValue != null
                && Convert.ToString(this.txtBirthday.EditValue) != "")
            {
                patient.PidBirthday = Convert.ToDateTime(this.txtBirthday.EditValue);
            }

            if (this.txtPatSDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatSDate.EditValue.ToString()))
            {
                patient.SampSendDate = Convert.ToDateTime(this.txtPatSDate.EditValue.ToString());
            }

            if (this.txtPatReachDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatReachDate.EditValue.ToString()))
            {
                patient.SampReachDate = Convert.ToDateTime(this.txtPatReachDate.EditValue);
            }

            if (this.txtPatSampleDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatSampleDate.EditValue.ToString()))
            {
                patient.SampCollectionDate = Convert.ToDateTime(this.txtPatSampleDate.EditValue);
            }

            if (this.txtPatApplyTime.EditValue != null && !string.IsNullOrEmpty(this.txtPatApplyTime.EditValue.ToString()))
            {
                patient.SampApplyDate = Convert.ToDateTime(this.txtPatApplyTime.EditValue);
            }

            if (this.txtPatRecDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatRecDate.EditValue.ToString()))
            {
                patient.RepPrintDate = Convert.ToDateTime(this.txtPatRecDate.EditValue);
            }
            if (this.fpat_exp2.EditValue != null && !string.IsNullOrEmpty(this.fpat_exp2.EditValue.ToString()))
            {
                patient.RepRemark = this.fpat_exp2.EditValue.ToString();
            }

            if (this.txtPat_social_no.EditValue != null && !string.IsNullOrEmpty(txtPat_social_no.Text))
                patient.PidSocialNo = this.txtPat_social_no.EditValue.ToString();
            if (!string.IsNullOrEmpty(this.fpat_sample_part.EditValue.ToString()))
            {
                patient.CollectionPart = this.fpat_sample_part.EditValue.ToString();
            }
            patient.PidApplyNo = this.txtPatAplyNo.Text;


            patient.RepBarCode = this.txtPatBarCode.Text;

            if (this.txtPatReceiveTime.EditValue != null && !string.IsNullOrEmpty(this.txtPatReceiveTime.EditValue.ToString()))
            {
                patient.SampReceiveDate = Convert.ToDateTime(this.txtPatReceiveTime.EditValue);
            }

            patient.PidSrcId = this.txtPatSource.valueMember;

            //体检id
            patient.PidExamNo = this.txt_pat_emp_id.Text;

            //就诊次数
            int iAdmissTimes = 0;
            if (int.TryParse(this.txtPatAdmissTime.Text, out iAdmissTimes))
            {
                patient.PidAddmissTimes = iAdmissTimes;
            }
            else
            {
                patient.PidAddmissTimes = 0;
            }

            patient.PidExamCompany = this.txtPatEmpCompanyName.Text;

            if (DictInstrmt.Instance.GetItrHostFlag(this.txtPatInstructment.valueMember) == 2)
            {
                if (this.txt_pat_host_order.Text.Trim() == string.Empty)
                {
                    patient.RepSerialNum = null;
                }
                else
                {
                    patient.RepSerialNum = this.txt_pat_host_order.Text;
                }
            }

            //自定义号  2010-6-22
            patient.RepInputId = this.txtPatPid.Text;
        }


        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <returns></returns>
        protected bool BeforeSaveCheck()
        {
            bool b = true;

            if (!(!IsNew && UserInfo.GetSysConfigValue("Lab_TypeSelect") == "是"))
            {
                if (this.controlPatList.ItrID == null
                    || controlPatList.ItrID.ToString().Trim(null) == string.Empty)
                {
                    lis.client.control.MessageDialog.Show("请选择[仪器]", "提示");
                    this.ActiveControl = this.txtPatInstructment;
                    this.controlPatList.FocusItr();
                    return false;
                }
            }
            if (this.txtPatSid.EditValue == null || this.txtPatSid.EditValue.ToString().Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请输入[标本号]", "提示");
                
                this.ActiveControl = this.txtPatSid;
                this.txtPatSid.Focus();
                return false;
            }

            //判断是否为有效的样本号
            Int64 iSid = -1;
            if (!Int64.TryParse(this.txtPatSid.EditValue.ToString(), out iSid))
            {
                lis.client.control.MessageDialog.Show("请输入正确的[标本号]", "提示");
                
                this.ActiveControl = this.txtPatSid;
                this.txtPatSid.Focus();
                return false;
            }


            //检查当前的[id类型]对应的[病人ID]是否为必录
            if (this.txtPatIdType.selectRow != null)

            {
                if (!string.IsNullOrEmpty(this.txtPatIdType.selectRow.IdtPatinnoNotnull.ToString()))
                {
                    if (Convert.ToBoolean(this.txtPatIdType.selectRow.IdtPatinnoNotnull) == true
                        && this.txtPatID.Text.Trim() == string.Empty
                        )
                    {
                        lis.client.control.MessageDialog.Show(string.Format("当前设定的[ID类型]为[{0}]的[病人ID]为必录，请输入[病人ID]", this.txtPatIdType.displayMember), "提示");
                        
                        this.ActiveControl = this.txtPatID;
                        this.txtPatID.Focus();
                        return false;
                    }
                }
            }

            if (this.txtPatSampleType.valueMember == null
                || this.txtPatSampleType.displayMember == null
                || this.txtPatSampleType.displayMember.ToString().Trim(null) == string.Empty)
            {
                lis.client.control.MessageDialog.Show("请输入[标本类别]", "提示");
                
                this.ActiveControl = this.txtPatSampleType;
                this.txtPatSampleType.Focus();
                return false;
            }

            if ((this.txtPatSex.valueMember == null
                || this.txtPatSex.displayMember == null
                || this.txtPatSex.displayMember.ToString().Trim(null) == string.Empty) && UserInfo.GetSysConfigValue("Lab_NotNull_Sex") == "是")
            {
                lis.client.control.MessageDialog.Show("请输入[性别]", "提示");
                
                this.ActiveControl = this.txtPatSex;
                this.txtPatSex.Focus();
                return false;
            }

            if ((this.txtPatName.Text == null || this.txtPatName.Text.Trim() == string.Empty)
                 && UserInfo.GetSysConfigValue("Lab_NotNull_Name") == "是")
            {
                lis.client.control.MessageDialog.Show("请输入[姓名]", "提示");
                
                this.ActiveControl = this.txtPatName;
                this.txtPatName.Focus();
                return false;
            }


            if ((this.txtPatInspetor.valueMember == null
                || this.txtPatInspetor.displayMember == null
                || this.txtPatInspetor.displayMember.ToString().Trim(null) == string.Empty
                )
                && UserInfo.GetSysConfigValue("Lab_NotNull_Inspector") == "是")
            {
                lis.client.control.MessageDialog.Show("请输入[检验者]", "提示");
                
                this.ActiveControl = this.txtPatInspetor;
                this.txtPatInspetor.Focus();
                return false;
            }

            if (
                    //未输入组合时是否提示
                    UserInfo.GetSysConfigValue("Lab_AlertOnEmptyCombine") == "是"
                &&
                    (this.listPatCombine == null
                    ||
                    this.listPatCombine.Count == 0))
            {
                if (MessageDialog.Show("当前资料未选组合，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }

            if (
                 //未输入组合时是否提示
                 UserInfo.GetSysConfigValue("Lab_AlertOnEmptySendDepart") == "是"
             &&
                 (
                       string.IsNullOrEmpty(this.txtPatDeptId.valueMember)
                    || string.IsNullOrEmpty(this.txtPatDeptId.displayMember)
                 )
                )
            {
                if (MessageDialog.Show("当前资料未选送检科室，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }
            //未输入年龄时是否提示
            if (this.textAgeInput1.AgeToMinute <= 0)
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("NoTipsInZeroOld") == "是")
                {
                }
                else
                {
                    if (MessageDialog.Show("当前资料未输入年龄，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return false;
                    }
                }
            }

            if (!Compare.IsNullOrDBNull(this.txtPatSDate.EditValue)//送检时间
                  && !Compare.IsNullOrDBNull(this.txtPatRecDate.EditValue))//检验时间
            {
                DateTime sample_send_date = (DateTime)this.txtPatSDate.EditValue;
                DateTime pat_jy_date = (DateTime)this.txtPatRecDate.EditValue;

                if (sample_send_date > pat_jy_date)
                {
                    lis.client.control.MessageDialog.Show(string.Format("[送检时间]不能大于[检验时间]"));
                    return false;
                }
            }
            return b;
        }

        /// <summary>
        /// 获取当前病人记录的审核状态
        /// </summary>
        /// <returns></returns>
        private string GetCurrentPatFlat()
        {
            if (this.IsNew)
            {

            }
            else
            {
                if (this.controlPatList.CurrentPatient != null)
                {
                    string pat_id = this.controlPatList.CurrentPatient.RepId.ToString();
                    string pat_flag = new ProxyPidReportMain().Service.GetPatientState(pat_id);

                    if (string.IsNullOrEmpty(pat_flag))
                    {
                        return LIS_Const.PATIENT_FLAG.Natural;
                    }
                    else
                    {
                        return pat_flag;
                    }
                }
            }
            return LIS_Const.PATIENT_FLAG.Natural;
        }

        /// <summary>
        /// 样本号失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPatSid_Leave(object sender, EventArgs e)
        {
            //如果没有手工结果并有选择仪器与录入日期
            if (this.txtPatSid.Text.Trim() == string.Empty)
            {
            }
            else
            {
                long testSampleID;
                if (!long.TryParse(this.txtPatSid.Text, out testSampleID))
                {
                    MessageDialog.Show("输入的样本号不正确，请确保为半角数字");
                    txtPatSid.Text = string.Empty;
                    this.ActiveControl = txtPatSid;
                    txtPatSid.Focus();
                    return;
                }
                char[] c = txtPatSid.Text.ToCharArray();
                for (int i = 0; i < c.Length; i++)
                {
                    if (IsCharDBC(c[i]))
                    {
                        MessageDialog.Show("输入的样本号不正确，请确保为半角数字");
                        txtPatSid.Text = string.Empty;
                        this.ActiveControl = txtPatSid;
                        txtPatSid.Focus();
                        return;
                    }
                }
            }

            SIDChanged();
        }
        public bool IsCharDBC(char c)
        {
            if (c > 65280 && c < 65375)
                return true;
            else
                return false;
        }

        #region   2010-6-8 如果没有手工结果并有选择仪器与录入日期 2010-6-29修改

        /// <summary>
        /// 样本号变化
        /// </summary>
        private void SIDChanged()
        {
            if (PatEnter.ShouldCheckWhenPatSidLeave && HasChooseDateAndInstructment())//&& HasDefaultCombines())
            {
                PatEnter.PatSIDChanged(ResultoHelper.GenerateResID(txtPatInstructment.valueMember, txtPatDate.DateTime, txtPatSid.Text), HasDefaultCombines());
            }
        }

        private bool HasNotManualResultOrHasDefaultCombines()
        {
            return PatEnter.HasNotManualResult() || HasDefaultCombines();
        }

        private bool HasDefaultCombines()
        {
            if (UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtCombine"))
            {
                return true;
            }
            return !string.IsNullOrEmpty(DictInstrmt.Instance.GetItrComID(this.txtPatInstructment.valueMember));
        }

        private bool HasChooseDateAndInstructment()
        {
            return txtPatDate.EditValue != null && !string.IsNullOrEmpty(txtPatInstructment.valueMember);
        }

        #endregion

        bool sampleIdChange = false;

        /// <summary>
        /// 性别改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtPatSex_ValueChanged(object sender, lis.client.control.ValueChangeEventArgs args)
        {
            IsDataChange = true;
            PatEnter.SexChanged(this.txtPatSex.valueMember);
        }

        #region 评价\意见范文选择
        /// <summary>
        /// 选择评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPatExp_Click(object sender, EventArgs e)
        {
            bool showCurrentExp = false;
            //当前有备注时 选择可显示当前备注
            if (!string.IsNullOrEmpty(fpat_exp2.Text))
            {
                showCurrentExp = true;
            }
            FrmBscripeSelectV2 fb = new FrmBscripeSelectV2(this, "0", showCurrentExp, fpat_exp2.Text);
            fb.GetCurInstrmtID += new FrmBscripeSelectV2.GetCurInstrmtIDEventHandler(fb_GetCurInstrmtID);
            fb.ShowDialog();
        }

        string fb_GetCurInstrmtID()
        {
            return this.txtPatInstructment.valueMember;
        }

        /// <summary>
        /// 选择意见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPatComment_Click(object sender, EventArgs e)
        {
            FrmBscripeSelectV2 fb = new FrmBscripeSelectV2(this, "1");
            fb.ShowDialog();
        }

        public void getBscripe(String str, string type)
        {
            if (type == "0")
            {
                fpat_exp2.Text += str;
            }
            if (type == "1")
            {
                fpat_comment.Text += str;
            }
            if (type == "3")
            {

                fpat_exp2.Text = str;
            }
        }

        #endregion

        /// <summary>
        /// 病人ID改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtPatID_TextLeaveChanged(object sender, lis.client.control.LISEditor.LISTextEdit.TextChangedEventArgs args)
        {
            if (UserInfo.GetSysConfigValue("EnterOrLeaveTextToLoadPatientInfo") == "离开文本框")
                LoadPatInfo();
        }

        private void txtPatID_EnterKeyDown(object sender, EventArgs args)
        {
            if (UserInfo.GetSysConfigValue("EnterOrLeaveTextToLoadPatientInfo") != "离开文本框")
                LoadPatInfo();
        }

        private void LoadPatInfo()
        {
            // 2010-6-29 修改状态也可调用病人资料
            string currPatFlag = GetCurrentPatFlat();

            if (currPatFlag != LIS_Const.PATIENT_FLAG.Natural && currPatFlag != string.Empty)
            {
                lis.client.control.MessageDialog.Show(string.Format("当前记录已{1}或已{0}，不能修改", AuditWord, LocalSetting.Current.Setting.ReportWord), "提示");
            }
            else//edit end
            {
                string typeid;
                string noteTempPatID = this.txtPatID.Text;//加载前-记录病人ID-用于筛查号
                bool success = LoadPatInfoByID(out typeid, this.txtPatID.Text);

                //获取病人信息成功则继续
                if (!success)
                    return;

                EntityDicPubIdent entityNoType = DictPatNumberType.Instance.GetNoType(typeid);

                //接口类型
                string notype = string.Empty;

                //接口代码
                string nocode = string.Empty;

                if (entityNoType != null)
                {
                    notype = entityNoType.IdtInterfaceType;
                    nocode = entityNoType.IdtCode;
                }

                PatEnter.PatIDChanged(notype, this.txtPatID.Text);

                if (nocode.ToLower() == "physicalexamination" ||
                   (success && notype.ToLower() == "barcode" && UserInfo.GetSysConfigValue("BarcodeAutoSave") == "是")
                    || (notype.ToLower() == "interface" && UserInfo.GetSysConfigValue("Lab_PatientsAutoSave") == "是"))
                {
                    string prev_sid = this.txtPatSid.Text;

                    string pat_id = this.Save();

                    if (!string.IsNullOrEmpty(pat_id))
                    {
                        if (!UserCustomSetting.PatResultPanel.SavePatInfoNoNext)
                        {
                            SetSID_AfterSaved(prev_sid);

                            this.txtPatIdType.SelectByID(typeid);
                            this.txtPatID.Text = string.Empty;
                            this.ActiveControl = this.txtPatID;
                            this.txtPatID.Focus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据ID类型和ID获取病人信息
        /// </summary>
        private bool LoadPatInfoByID(out string originidtype, string id_code)
        {
            bool ret = false;

            originidtype = this.txtPatIdType.valueMember;

            if (this.txtPatIdType.valueMember != null
                        && this.txtPatIdType.valueMember.Trim(null) != string.Empty
                        && this.txtPatID.Text.Trim(null) != string.Empty)
            {
                try
                {
                    string typeName = this.txtPatIdType.displayMember;

                    string idtype = this.txtPatIdType.valueMember;
                    EntityDicPubIdent entityNoType = DictPatNumberType.Instance.GetNoType(idtype);

                    if (entityNoType != null
                        && !Compare.IsEmpty(entityNoType.IdtInterfaceType)
                        && entityNoType.IdtInterfaceType.Trim() != string.Empty)
                    {
                        //接口类型
                        string notype = string.Empty;

                        //接口代码
                        string nocode = string.Empty;

                        if (entityNoType != null)
                        {
                            notype = entityNoType.IdtInterfaceType;
                            nocode = entityNoType.IdtCode;
                        }

                        string interfaceID = string.Empty;

                        if (!Compare.IsNullOrDBNull(entityNoType.IdtInterfaceId) && entityNoType.IdtInterfaceId.Trim() != string.Empty)
                        {
                            interfaceID = entityNoType.IdtInterfaceId;
                        }

                        ////当接口类型为院网接口并且接口ID部位空
                        ////并且接口类型不为None是才获取数据
                        //if ((type == NetInterfaceType.Interface && interfaceID != string.Empty && type != NetInterfaceType.None) || type == NetInterfaceType.Barcode)
                        //{
                        string interfaceFrom = "通用";
                        interfaceFrom = UserInfo.GetSysConfigValue("GetPatientsInfoType").ToLower().Trim();
                        //------------ 2010-5-10 
                        NetInterfaceType type = (NetInterfaceType)Enum.Parse(typeof(NetInterfaceType), entityNoType.IdtInterfaceType);

                        if (interfaceFrom == "通用" && type == NetInterfaceType.None)
                            return false;

                        string patid = this.txtPatID.Text;
                        if (nocode.Trim().ToLower() == "outpatient" || nocode.Trim().ToLower() == "inpatient")
                        {
                            int AddZeroNum = 0;
                            int.TryParse(patInNoAutoAddZeroNum, out AddZeroNum);
                            if (AddZeroNum > 0)
                            {
                                patid = patid.PadLeft(AddZeroNum, '0');
                            }
                        }
                        PatientInterfaceInfo patInterface = new PatientInterfaceInfo(nocode.Trim().ToLower(), patid, interfaceID, type);
                        EntityPidReportMain PatInfo = null;
                        IPatients patientsInterface = null;
                        if (interfaceFrom == "通用")
                            patientsInterface = new NormalPatients();

                        else if (interfaceFrom == "outlink")
                            patientsInterface = new OutlinkPatinets();
                        EntityInterfaceExtParameter parameter = new EntityInterfaceExtParameter();
                        parameter.PatientID = patid;
                        if (entityNoType.IdtName == "住院号")
                        {
                            parameter.DownloadType = InterfaceType.ZYPatient;
                        }
                        else if (entityNoType.IdtName == "门诊号")
                        {
                            parameter.DownloadType = InterfaceType.MZPatient;
                        }
                        else if (entityNoType.IdtName == "体检号")
                        {
                            parameter.DownloadType = InterfaceType.TJPatient;
                        }
                        else if (entityNoType.IdtName == "条码号")
                        {
                            parameter.DownloadType = InterfaceType.BarcodePatient;
                        }
                        ProxyPidReportMainInterface proxy = new ProxyPidReportMainInterface();
                        //广州12医院单机版 扫条码号之后只记录录入时间、样本号、条码号
                        if (UserInfo.GetSysConfigValue("Lab_EnableUpload") == "是")
                        {
                            PatInfo = new EntityPidReportMain();
                            PatInfo.RepInDate = txtPatDate.DateTime;
                            PatInfo.RepSid = txtPatSid.Text.ToString();
                            PatInfo.RepBarCode = txtPatID.Text.ToString();
                            FillInterfacePatToControl(PatInfo);
                            return true;
                        }
                        else
                        {
                            PatInfo = proxy.Service.GetPatientFromInterface(parameter);
                        }

                        if (PatInfo == null)
                        {
                            lis.client.control.MessageDialog.Show(string.Format("{0}不存在", typeName), "提示");
                            this.txtPatID.EditValue = string.Empty;
                            this.txtPatID.Focus();
                            return false;
                        }
                        else
                        {
                            if (nocode == "barcode")// NetInterfaceType.Barcode)
                            {

                                if (PatInfo.BcStatus == "9")
                                {
                                    /**********peng:提示信息增加操作者和时间*******************************************/
                                    string name = string.Empty;
                                    string time = string.Empty;
                                    string remark = string.Empty;
                                    EntitySampProcessDetail processDetail = new ProxySampProcessDetail().Service.GetLastSampProcessDetail(PatInfo.RepBarCode);
                                    MessageDialog.Show(string.Format("此条码已回退，不允许录入！\r\n{2}\r\n 操作者：{0},时间：{1}", processDetail.ProcUsername, processDetail.ProcDate, processDetail.ProcContent), "提示");
                                    /***************************************************************/
                                    return false;

                                }
                                if (
                                     PatInfo.BcStatus == "0"
                                     || PatInfo.BcStatus == "1"
                                     || PatInfo.BcStatus == "2"
                                     || PatInfo.BcStatus == "3"
                                     || PatInfo.BcStatus == "4"
                                     || PatInfo.BcStatus == "8"
                                     || PatInfo.BcStatus == "9"
                                     || PatInfo.BcStatus == "500"
                                     || PatInfo.BcStatus == "510")//条码未签收
                                {
                                    if (PatInfo.PidSrcId == "108" && UserInfo.GetSysConfigValue("Barcode_ZYShouldReceiveConfirm") == "是")//住院
                                    {
                                        MessageDialog.Show("当前条码未签收！\r\n当前设置为：[住院条码]必须进行[签收]确认", "提示");
                                        return false;
                                    }

                                    if (PatInfo.PidSrcId == "109" && UserInfo.GetSysConfigValue("Barcode_TJShouldReceiveConfirm") == "是")//体检
                                    {
                                        MessageDialog.Show("当前条码未签收！\r\n当前设置为：[体检条码]必须进行[签收]确认", "提示");
                                        return false;
                                    }

                                    if (PatInfo.PidSrcId == "107" && UserInfo.GetSysConfigValue("Barcode_MZShouldReceiveConfirm") == "是")//门诊
                                    {
                                        MessageDialog.Show("当前条码未签收！\r\n当前设置为：[门诊条码]必须进行[签收]确认", "提示");
                                        return false;
                                    }
                                }
                                if (CheckReceiveTimeAndPatdate)
                                {
                                    DateTime jyTime = ServerDateTime.GetServerDateTime(); ;
                                    if (PatInfo.SampApplyDate != null &&
                                        PatInfo.SampApplyDate.Value > jyTime)
                                    {
                                        MessageDialog.Show("签收时间大于当前检验时间，请检查！", "提示");
                                        return false;
                                    }
                                }
                            }

                            if (nocode.Trim().ToLower() == "barcode")
                            {
                                FillInterfacePatToControl(PatInfo);
                                if (PatInfo.ListPidReportDetail != null && PatInfo.ListPidReportDetail.Count > 0)
                                {
                                    if (chkCustomSelectCombine.Checked)//弹出选择框让用户选择
                                    {
                                        #region 弹出选择框让用户选择
                                        frmBarcodeCombineSelect frmCombineSelect = new frmBarcodeCombineSelect(this.txtPatInstructment.valueMember, PatInfo.ListPidReportDetail);

                                        if (frmCombineSelect.ShowDialog() == DialogResult.OK)
                                        {
                                            foreach (EntityPidReportDetail com_selected in frmCombineSelect.listCombineSelected)
                                            {
                                                PatEnter.CombineEditor.AddCombine(com_selected.ComId, com_selected.OrderSn, Convert.ToDecimal(com_selected.OrderPrice), com_selected.RepBarCode);
                                            }

                                            if (frmCombineSelect.listCombineSelected.Count > 0)
                                            {
                                                ret = true;
                                            }
                                            else
                                            {
                                                ret = false;
                                            }
                                        }
                                        else
                                        {
                                            ret = false;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        if (PatEnter.CombineEditor.listRepDetail != null)
                                        {
                                            PatEnter.CombineEditor.listRepDetail.Clear();
                                            PatEnter.ResReset();
                                        }
                                        PatEnter.CombineEditor.RefreshEditBoxText();

                                        //可以在此仪器登记的组合
                                        List<EntityPidReportDetail> combineCanRegistedInThisItr;// = new List<EntityPatientsMi_4Barcode>();

                                        //可以在此仪器登记,但未登记的组合
                                        List<EntityPidReportDetail> combineCanRegistedInThisItrWithoutReg;// = new List<EntityPatientsMi_4Barcode>();

                                        //可以在此仪器登记,但已经登记的组合
                                        List<EntityPidReportDetail> combineCanRegistedInThisItrReged;// = new List<EntityPatientsMi_4Barcode>();

                                        //不能此仪器登记的组合
                                        List<EntityPidReportDetail> combineCannotRegistedInThisItr;// = new List<EntityPatientsMi_4Barcode>();


                                        RegisterCombine2(PatInfo.ListPidReportDetail
                                            , out combineCanRegistedInThisItr
                                            , out combineCanRegistedInThisItrWithoutReg
                                            , out combineCanRegistedInThisItrReged
                                            , out combineCannotRegistedInThisItr);

                                        if (combineCanRegistedInThisItrWithoutReg.Count > 0)//可以登记但未登记的组合
                                        {
                                            foreach (EntityPidReportDetail com_selected in combineCanRegistedInThisItrWithoutReg)
                                            {
                                                PatEnter.CombineEditor.AddCombine(com_selected.ComId, com_selected.OrderSn, Convert.ToDecimal(com_selected.OrderPrice), com_selected.RepBarCode);
                                            }
                                            ret = true;
                                        }
                                        else if (combineCanRegistedInThisItrWithoutReg.Count == 0 && combineCanRegistedInThisItrReged.Count == 0 && combineCannotRegistedInThisItr.Count > 0)
                                        {
                                            string msg = "以下组合不能在当前仪器中登记，请检查当前仪器是否已经包含有以下组合：\r\n\r\n";
                                            foreach (EntityPidReportDetail combine in combineCannotRegistedInThisItr)
                                            {
                                                msg += string.Format("[{0}]{1}\r\n", combine.ComId, combine.PatComName);
                                            }

                                            msg += "\r\n\r\n注：仪器组合可以在 [项目字典]->[仪器组合]中设置";

                                            MessageDialog.Show(msg, "提示");
                                            ret = false;
                                        }
                                        else if (combineCanRegistedInThisItrReged.Count > 0)
                                        {
                                            EntityPatientQC patientQc = new EntityPatientQC();
                                            patientQc.RepBarCode = SQLFormater.FormatSQL(id_code);
                                            List<EntityPidReportMain> listPatCount = new ProxyPidReportMain().Service.PatientQuery(patientQc);
                                            if (listPatCount != null && listPatCount.Count > 0)
                                            {
                                                string mes = "";
                                                foreach (EntityPidReportMain dr in listPatCount)
                                                {
                                                    mes += string.Format("该条码已在 {1} 时 在仪器 {0} 录入，样本号:{2}，组合:{3} \r\n", dr.ItrName, dr.RepInDate, dr.RepSid, dr.PidComName);
                                                }
                                                mes += "\r\n是否继续登记？";
                                                if (lis.client.control.MessageDialog.Show(mes, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                                                {
                                                    foreach (EntityPidReportDetail com_selected in combineCanRegistedInThisItrReged)
                                                    {
                                                        PatEnter.CombineEditor.AddCombine(com_selected.ComId, com_selected.OrderSn, Convert.ToDecimal(com_selected.OrderPrice), com_selected.RepBarCode);
                                                    }
                                                    ret = true;
                                                    if (combineCannotRegistedInThisItr.Count > 0)
                                                    {
                                                        string msg = "以下组合不能在当前仪器中登记，请检查当前仪器是否已经包含有以下组合：\r\n\r\n";
                                                        foreach (EntityPidReportDetail combine in combineCannotRegistedInThisItr)
                                                        {
                                                            msg += string.Format("[{0}]{1}\r\n", combine.ComId, combine.PatComName);
                                                        }

                                                        msg += "\r\n\r\n注：仪器组合可以在 [项目字典]->[仪器组合]中设置";

                                                        MessageDialog.Show(msg, "提示");
                                                        ret = false;
                                                    }
                                                }
                                                else
                                                {
                                                    ret = false;
                                                }
                                            }
                                            else
                                            {
                                                MessageDialog.Show("当前条码没有可登记的组合！", "提示");
                                                ret = false;
                                            }
                                        }

                                    }


                                }
                                else
                                {
                                    MessageDialog.Show("当前条码没有可登记的组合！", "提示");
                                    ret = false;
                                }
                            }
                            else
                            {
                                FillInterfacePatToControl(PatInfo);
                                txtPatID.Focus();

                                //院网医嘱组合的his编码转换为lis的组合编码
                                //病人来源
                                string ori_id = DictPatNumberType.Instance.GetOriID_byNoType(this.txtPatIdType.valueMember);
                                string loginID = UserInfo.loginID;
                                string loginName = UserInfo.userName;
                                //系统配置：登记病人号时提醒已登记过的
                                if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_PatInNoSignWarnExists") == "是")
                                {
                                    if (controlPatList.ExistPatientsID(PatInfo.PidInNo, ori_id))
                                    {
                                        lis.client.control.MessageDialog.ShowAutoCloseDialog("当前病人号[" + PatInfo.PidInNo + "]已经登记过", 2);
                                    }
                                }

                                #region 院网接口调组合  2010-7-5
                                if (PatInfo.ListPidReportDetail != null && PatInfo.ListPidReportDetail.Count > 0)
                                {
                                    if (PatEnter.CombineEditor.listRepDetail != null)
                                    {
                                        PatEnter.CombineEditor.listRepDetail.Clear();

                                    }
                                    PatEnter.CombineEditor.Reset();
                                    PatEnter.CombineEditor.RefreshEditBoxText();
                                    List<EntityPidReportDetail> hisCodeNotExists = new List<EntityPidReportDetail>();
                                    if (!string.IsNullOrEmpty(ori_id))
                                    {
                                        List<EntityPidReportDetail> comIds = GetComIdWithHISCode(PatInfo.ListPidReportDetail,
                                                                                                     ref
                                                                                                         hisCodeNotExists,
                                                                                                     ori_id);
                                        string message = CheckComIdFromPatinetMi(hisCodeNotExists);

                                        if (!string.IsNullOrEmpty(message))
                                        {
                                            MessageDialog.Show(message, "提示");
                                        }

                                        if (comIds.Count > 0)
                                        {
                                            if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("GetHISCombinePattern"))
                                                && UserInfo.GetSysConfigValue("GetHISCombinePattern") != "简单")
                                            //取组合完整模式，弹出框确认// 2010-8-24
                                            {

                                                List<EntityDicItrCombine> listItrCombine = CacheClient.GetCache<EntityDicItrCombine>();

                                                foreach (var item in comIds)
                                                {
                                                    if (
                                                    listItrCombine.FindAll(w => w.ComId == item.ComId && w.ItrId == txtPatInstructment.valueMember).Count >
                                                    0)
                                                        item.AllowSelect = true;
                                                    else
                                                        item.AllowSelect = false;
                                                }

                                                frmAdviceSelect frmCombineSelect = new frmAdviceSelect(comIds);

                                                if (frmCombineSelect.ShowDialog() == DialogResult.OK)
                                                {
                                                    comIds = frmCombineSelect.listCombineConfirmed;
                                                }
                                                else
                                                {
                                                    comIds.Clear();
                                                }
                                            }

                                            if (comIds != null && comIds.Count > 0)
                                            {
                                                if (!string.IsNullOrEmpty(comIds[0].PidDoctorCode))
                                                {
                                                    this.txtPatDoc2.SelectByValue(comIds[0].PidDoctorCode);
                                                }
                                                else if (!string.IsNullOrEmpty(comIds[0].PidDoctorName))
                                                {
                                                    this.txtPatDoc2.SelectByDispaly(comIds[0].PidDoctorName);
                                                }
                                                else
                                                {
                                                    txtPatDoc2.valueMember = null;
                                                    txtPatDoc2.displayMember = null;
                                                }

                                                if (!string.IsNullOrEmpty(comIds[0].PidDeptCode))
                                                {
                                                    txtPatDeptId.SelectByValue(comIds[0].PidDeptCode);
                                                }
                                                else if (!string.IsNullOrEmpty(comIds[0].PidDeptName))
                                                {
                                                    txtPatDeptId.SelectByDispaly(comIds[0].PidDeptName);
                                                }
                                                else
                                                {
                                                    txtPatDeptId.valueMember = null;
                                                    txtPatDeptId.displayMember = null;
                                                }

                                                txtPatDiag.popupContainerEdit1.Text = comIds[0].PidDiag;

                                                //可以登记的组合
                                                List<EntityPidReportDetail> combineCanRegisted =
                                                    new List<EntityPidReportDetail>();

                                                //不能登记的组合
                                                List<EntityPidReportDetail> combineCannotRegisted =
                                                    new List<EntityPidReportDetail>();

                                                ret = RegisterCombine(comIds, out combineCanRegisted,
                                                                      out combineCannotRegisted);
                                            }
                                        }
                                        else
                                        {
                                            ret = true;
                                        }
                                    }
                                    else
                                    {

                                    }
                                    #endregion

                                    if (txtPatSampleDate.EditValue == null)
                                        txtPatSampleDate.EditValue = txtPatRecDate.EditValue;

                                    if (txtPatSDate.EditValue == null)
                                        txtPatSDate.EditValue = txtPatRecDate.EditValue;

                                    if (txtPatReachDate.EditValue == null)
                                        txtPatReachDate.EditValue = txtPatRecDate.EditValue;

                                    if (txtPatApplyTime.EditValue == null)
                                        txtPatApplyTime.EditValue = txtPatRecDate.EditValue;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lis.client.control.MessageDialog.Show("获取病人信息失败", "提示");
                    Logger.WriteException(this.GetType().Name, "LoadPatInfoByID", ex.ToString());
                }
            }

            return ret;
        }

        /// <summary>
        /// 通过院网接口登记组合到病人资料中
        /// </summary>
        /// <param name="combineToFilter"></param>
        /// <param name="listCombineCanRegister"></param>
        /// <param name="listCombineCannotRegister"></param>
        /// <returns></returns>
        private bool RegisterCombine(List<EntityPidReportDetail> combineToFilter, out List<EntityPidReportDetail> listCombineCanRegister, out List<EntityPidReportDetail> listCombineCannotRegister)
        {
            //可以登记的组合
            listCombineCanRegister = new List<EntityPidReportDetail>();

            //不能登记的组合
            listCombineCannotRegister = new List<EntityPidReportDetail>();

            //查找当前仪器的组合
            List<string> currentItrComIDs = null;
            if (!string.IsNullOrEmpty(this.txtPatInstructment.valueMember))//没有选择仪器
            {
                currentItrComIDs = DictInstrmt.Instance.GetItrCombineID(this.txtPatInstructment.valueMember, true);
            }

            foreach (EntityPidReportDetail comId in combineToFilter)
            {
                string com_id = comId.ComId;

                if (currentItrComIDs == null)//如果没有选择仪器
                {
                    PatEnter.CombineEditor.AddCombine(comId.ComId, comId.OrderSn, Convert.ToDecimal(comId.OrderPrice), comId.RepBarCode);
                    listCombineCanRegister.Add(comId);
                }
                else
                {
                    if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("GetHISCombinePattern"))
                                             && UserInfo.GetSysConfigValue("GetHISCombinePattern") != "简单")//取组合完整模式，弹出框确认
                    {
                        if (currentItrComIDs.Contains(com_id))//当前仪器包含此组合 // 2010-8-24
                        {
                            PatEnter.CombineEditor.AddCombine(comId.ComId, comId.OrderSn, Convert.ToDecimal(comId.OrderPrice), comId.RepBarCode);
                            listCombineCanRegister.Add(comId);
                        }
                        else//不包含
                        {
                            listCombineCannotRegister.Add(comId);
                        }
                    }
                    else//取组合简单模式，不弹出框，有则显示组合，没有则提示
                    {
                        PatEnter.CombineEditor.AddCombine(comId.ComId, comId.OrderSn, Convert.ToDecimal(comId.OrderPrice), comId.RepBarCode);
                        listCombineCanRegister.Add(comId);
                    }
                }
            }

            //根据组合的默认标本设置标本
            if (listCombineCanRegister.Count > 0 && string.IsNullOrEmpty(txtPatSampleType.valueMember))
            {
                string comSampleID = "";
                foreach (EntityPidReportDetail comID in listCombineCanRegister)
                {
                    EntityDicCombine combineRow = DictCombine.Instance.GetCombinebyID(comID.ComId);
                    if (string.IsNullOrEmpty(comSampleID))
                    {
                        comSampleID = combineRow.ComSamId;
                    }
                }

                if (!string.IsNullOrEmpty(comSampleID))
                {
                    txtPatSampleType.SelectByID(comSampleID);
                }
            }

            if (listCombineCannotRegister.Count == 0)
            {
                return true;
            }
            else
            {
                string msg = "以下组合不能在当前仪器中登记，请检查当前仪器是否已经包含有以下组合：\r\n\r\n";
                foreach (EntityPidReportDetail combine in listCombineCannotRegister)
                {
                    msg += string.Format("[{0}]{1}\r\n", combine.ComId, combine.PatComName);
                }

                msg += "\r\n\r\n注：仪器组合可以在 [项目字典]->[仪器组合]中设置";

                MessageDialog.Show(msg, "提示");
                return false;
            }
        }

        private void RegisterCombine2(List<EntityPidReportDetail> combineToReg
                                     , out List<EntityPidReportDetail> combineCanRegistedInThisItr
                                     , out List<EntityPidReportDetail> combineCanRegistedInThisItrWithoutReg
                                     , out List<EntityPidReportDetail> combineCanRegistedInThisItrReged
                                     , out List<EntityPidReportDetail> combineCannotRegistedInThisItr)
        {
            combineCanRegistedInThisItr = new List<EntityPidReportDetail>(); //可以在此仪器登记的组合
            combineCanRegistedInThisItrWithoutReg = new List<EntityPidReportDetail>();//可以在此仪器登记,但未登记的组合
            combineCannotRegistedInThisItr = new List<EntityPidReportDetail>();//不能此仪器登记的组合
            combineCanRegistedInThisItrReged = new List<EntityPidReportDetail>();//可以在此仪器登记,但已经登记的组合

            //查找当前仪器的组合
            List<string> currentItrComIDs = null;
            if (!string.IsNullOrEmpty(this.txtPatInstructment.valueMember))//没有选择仪器
            {
                currentItrComIDs = DictInstrmt.Instance.GetItrCombineID(this.txtPatInstructment.valueMember, true);
            }

            //遍历每一个需要登记的组合
            foreach (EntityPidReportDetail combine in combineToReg)
            {
                string com_id = combine.ComId;

                if (currentItrComIDs == null)//如果没有选择仪器
                {
                    combineCanRegistedInThisItr.Add(combine);
                    if (combine.SampFlag == 1)
                    {
                        combineCanRegistedInThisItrReged.Add(combine);
                    }
                    else
                    {
                        combineCanRegistedInThisItrWithoutReg.Add(combine);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("GetHISCombinePattern"))
                                             && UserInfo.GetSysConfigValue("GetHISCombinePattern") != "简单")//取组合完整模式，弹出框确认
                    {
                        if (currentItrComIDs.Contains(com_id))//当前仪器包含此组合 // 2010-8-24
                        {
                            combineCanRegistedInThisItr.Add(combine);
                            if (combine.SampFlag == 1)
                            {
                                combineCanRegistedInThisItrReged.Add(combine);
                            }
                            else
                            {
                                combineCanRegistedInThisItrWithoutReg.Add(combine);
                            }
                        }
                        else//不包含
                        {
                            combineCannotRegistedInThisItr.Add(combine);
                        }
                    }
                    else//取组合简单模式，不弹出框，有则显示组合，没有则提示
                    {
                        combineCanRegistedInThisItr.Add(combine);
                        if (combine.SampFlag == 1)
                        {
                            combineCanRegistedInThisItrReged.Add(combine);
                        }
                        else
                        {
                            combineCanRegistedInThisItrWithoutReg.Add(combine);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查没有对应LIS组合的HIS代码
        /// </summary>
        /// <param name="hisCodeNotExists"></param>
        /// <returns></returns>
        private string CheckComIdFromPatinetMi(List<EntityPidReportDetail> hisCodeNotExists)
        {
            bool onlyPatientInfo = true; //是否只有病人资料
            string message = "";
            if (hisCodeNotExists.Count > 0)
            {
                message += "HIS代码：";

                bool needComma = false;

                foreach (EntityPidReportDetail item in hisCodeNotExists)
                {
                    if (needComma)
                    {
                        message += "，";
                    }

                    if (!string.IsNullOrEmpty(item.OrderCode))
                    {
                        onlyPatientInfo = false;
                    }
                    message += string.Format("[{0}]", item.OrderCode);

                    needComma = true;
                }
                if (onlyPatientInfo)
                {
                    message = "该病人没有医嘱！";
                }
                else
                    message += "\r\n\r\n在系统中找不到相应的组合!\r\n请在[字典管理]->[项目字典]->[项目组合]->[条码信息]中添加！";
            }
            return message;
        }

        /// <summary>
        /// 通过组合HIS码获取组合ID
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<EntityPidReportDetail> GetComIdWithHISCode(List<EntityPidReportDetail> list, ref List<EntityPidReportDetail> hisCodeNotExists, string ori_id)
        {
            List<EntityPidReportDetail> ret = new List<EntityPidReportDetail>();

            //在组合条码表中查找当前病人来源,指定his组合收费码对应的lis组合id
            if (string.IsNullOrEmpty(ori_id))
                return null;

            List<string> hisCodes = new List<string>();
            foreach (EntityPidReportDetail mi in list)
            {
                hisCodes.Add(mi.OrderCode);
            }

            if (hisCodes == null || hisCodes.Count <= 0)
                return ret;

            //根据his收费码查找lis组合id(所有)
            List<EntitySampMergeRule> listRule = new ProxyPatResult().Service.GetRuleByHisCode(hisCodes, ori_id);

            if (listRule == null && listRule.Count <= 0)//找不到
            {
                hisCodeNotExists.AddRange(list);
                return ret;
            }

            foreach (EntityPidReportDetail entityMi in list)
            {

                EntitySampMergeRule bcCombine = listRule.Find(m => m.ComHisFeeCode == entityMi.OrderCode
                                                                     && !string.IsNullOrEmpty(m.ComHisFeeCode));
                if (bcCombine != null)
                {
                    entityMi.ComId = bcCombine.ComId;
                    entityMi.PatComName = bcCombine.ComName;
                    entityMi.OrderPrice = bcCombine.ComPrice.ToString();
                    ret.Add(entityMi);
                }
                else
                {
                    hisCodeNotExists.Add(entityMi);
                }
            }

            return ret;
        }

        /// <summary>
        /// 填充数据到控件
        /// </summary>
        /// <param name="drSourceDataRow"></param>
        private void FillInterfacePatToControl(EntityPidReportMain PatInfo)
        {
            isDataFill = true;
            if (bsPat.DataSource == null) return;
            EntityPidReportMain pat = (bsPat.DataSource as EntityPidReportMain);//new EntityPidReportMain();

            DateTime dtServerTime = ServerDateTime.GetServerDateTime();

            //姓名
            pat.PidName = PatInfo.PidName;

            pat.PidSex = PatInfo.PidSex; //性别

            pat.PidAgeExp = PatInfo.PidAgeExp;

            if (!string.IsNullOrEmpty(PatInfo.PidSamId))
            {
                pat.PidSamId = PatInfo.PidSamId;
                this.txtPatSampleType.SelectByID(PatInfo.PidSamId);
            }
            else if (!string.IsNullOrEmpty(PatInfo.SamName))
            {
                this.txtPatSampleType.SelectByDispaly(PatInfo.SamName);
                pat.PidSamId = txtPatSampleType.valueMember;
            }

            pat.PidBedNo = PatInfo.PidBedNo; //床号

            #region 病人辅助信息
            //职业
            pat.PidWork = PatInfo.PidWork;

            //联系电话
            pat.PidTel = PatInfo.PidTel;

            //联系邮件
            pat.PidEmail = PatInfo.PidEmail;

            //工作单位
            pat.PidUnit = PatInfo.PidUnit;

            //联系地址
            pat.PidAddress = PatInfo.PidAddress;

            //身高
            pat.PidHeight = PatInfo.PidHeight;

            //体重
            pat.PidWeight = PatInfo.PidWeight;

            //出生日期
            //pat.PidBirthday = PatInfo.birthday;

            #endregion

            if (!string.IsNullOrEmpty(PatInfo.PidDoctorCode))
            {
                //如果送检者工号保存在表的ID列上，（不推荐）// 2010-8-24
                if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("GetSendingDoctorType"))
                    && UserInfo.GetSysConfigValue("GetSendingDoctorType") == "LIS编码关联")
                {
                    this.txtPatDoc2.SelectByID(PatInfo.PidDoctorCode);
                }
                else//如果送检者工号保存在表的Code列上（推荐）//157医院中大六院 用HISCODE
                {
                    this.txtPatDoc2.SelectByValue(PatInfo.PidDoctorCode);
                    if (string.IsNullOrEmpty(this.txtPatDoc2.valueMember)
                      && PatInfo.PidDocName != null)
                    {
                        this.txtPatDoc2.SelectByDispaly(PatInfo.PidDocName);

                        if (string.IsNullOrEmpty(this.txtPatDoc2.valueMember))
                        {
                            this.txtPatDoc2.displayMember = PatInfo.PidDocName;
                        }
                    }
                }
            }
            else
            {
                this.txtPatDoc2.SelectByDispaly(PatInfo.PidDocName);

                if (!string.IsNullOrEmpty(PatInfo.PidDocName) && string.IsNullOrEmpty(this.txtPatDoc2.displayMember))
                {
                    this.txtPatDoc2.displayMember = PatInfo.PidDocName;
                }
            }

            pat.PidDoctorCode = txtPatDoc2.valueMember;
            //送检科室
            bAllowFirePatDept_ValueChanged = false;

            if (!string.IsNullOrEmpty(PatInfo.PidDeptCode))
            {
                //送检科室
                this.txtPatDeptId.SelectByValue(PatInfo.PidDeptCode);

                if (string.IsNullOrEmpty(this.txtPatDeptId.valueMember)
                    && !string.IsNullOrEmpty(PatInfo.PidDeptName))
                {
                    this.txtPatDeptId.SelectByDispaly(PatInfo.PidDeptName);
                }
            }
            else
            {
                this.txtPatDeptId.SelectByDispaly(PatInfo.PidDeptName);
                txtPatDeptId.displayMember = PatInfo.PidDeptName;
                pat.PidDeptName = PatInfo.PidDeptName;
            }

            pat.PidDeptCode = txtPatDeptId.valueMember;
            bAllowFirePatDept_ValueChanged = true;

            //病区
            pat.PidWardId = PatInfo.PidWardId;
            pat.PidWardName = PatInfo.PidWardName;
            this.txtPatDiag.displayMember = PatInfo.PidDiag;
            pat.PidDiag = txtPatDiag.displayMember;
            pat.PidPurpId = txtPat_chk_purpose.valueMember;

            patidentity = PatInfo.PidIdentity.ToString();
            //身份证
            pat.PidIdentityCard = PatInfo.PidIdentityCard;
            //条码号
            pat.RepBarCode = PatInfo.RepBarCode;

            //注意事项 
            this.txtPatNotice.Text = "";
            if (txtPatNotice.Visible && (!string.IsNullOrEmpty(PatInfo.RepBarCode)))
            {
                string StrTempNotice = new ProxySampMain().Service.SampMainQueryByBarId(PatInfo.RepBarCode).SampRemark;
                if (!string.IsNullOrEmpty(StrTempNotice))
                    this.txtPatNotice.Text = StrTempNotice;
            }

            if (PatInfo.SampReceiveDate != null)
            {
                pat.SampReceiveDate = PatInfo.SampReceiveDate;
            }
            else
            {
                pat.SampReceiveDate = null;
            }

            //采样时间
            if (PatInfo.SampCollectionDate != null)
            {
                pat.SampCollectionDate = PatInfo.SampCollectionDate.Value;
            }
            else
            {
                pat.SampCollectionDate = null;
            }

            //送达时间
            if (PatInfo.SampSendDate != null)
            {
                pat.SampSendDate = PatInfo.SampSendDate.Value;
            }
            else
            {
                pat.SampSendDate = null;
            }
            //送检时间
            if (PatInfo.SampReachDate != null)
            {
                pat.SampReachDate = PatInfo.SampReachDate.Value;
            }
            else
            {
                pat.SampReachDate = null;
            }
            //接收时间
            if (PatInfo.SampApplyDate != null)
            {
                pat.SampApplyDate = PatInfo.SampApplyDate.Value;
            }
            else
            {
                pat.SampApplyDate = null;
            }
            //出生日期
            if (PatInfo.PidBirthday != null)
            {
                pat.PidBirthday = PatInfo.PidBirthday;
            }
            else
            {
                pat.PidBirthday = null;
            }

            if (PatInfo.SampSendDate == null)
            {
                pat.SampCheckDate = dtServerTime;
            }
            else
            {
                if (dtServerTime > PatInfo.SampSendDate.Value)
                {
                    pat.SampCheckDate = dtServerTime;
                }
                else
                {
                    pat.SampCheckDate = PatInfo.SampSendDate.Value.AddMinutes(1);
                }
            }

            if (Compare.IsEmpty(this.txtPatInspetor.valueMember))
                pat.RepCheckUserId = UserInfo.loginID;

            //费用类别
            pat.PidInsuId = PatInfo.PidInsuId;
            this.txtPatFeeType.valueMember = PatInfo.PidInsuId;
            //标本备注
            pat.SampRemark = PatInfo.SampRemark;

            //病人ID类型
            if (!string.IsNullOrEmpty(PatInfo.PidIdtId))
            {
                pat.PidIdtId = PatInfo.PidIdtId;
            }

            string ori_id = DictPatNumberType.Instance.GetOriID_byNoType(this.txtPatIdType.valueMember);
            //病人来源
            if (!string.IsNullOrEmpty(PatInfo.PidSrcId))
            {
                pat.PidSrcId = PatInfo.PidSrcId;
                this.txtPatSource.SelectByID(PatInfo.PidSrcId);
            }
            else if (!string.IsNullOrEmpty(PatInfo.SrcName))
            {
                this.txtPatSource.SelectByDispaly(PatInfo.SrcName);
                pat.PidSrcId = txtPatSource.valueMember;
            }
            else if (!string.IsNullOrEmpty(ori_id))
            {
                this.txtPatSource.SelectByID(ori_id);
                pat.PidSrcId = txtPatSource.valueMember;
            }


            //病人ID
            pat.PidInNo = PatInfo.PidInNo;
            pat.RepCtype = PatInfo.RepCtype;

            //体检id
            pat.PidExamNo = PatInfo.PidExamNo;

            //就诊次数
            pat.PidAddmissTimes = PatInfo.PidAddmissTimes;

            pat.PidExamCompany = PatInfo.PidExamCompany;

            //门诊卡号，病历号
            pat.PidSocialNo = PatInfo.PidSocialNo;

            pat.RepInputId = PatInfo.RepInputId;//  2010-6-17 +++++++

            pat.PidUniqueId = PatInfo.PidUniqueId;// edit by 2013-12-11 ++++ 滨海病人唯一号
            pat.DeptTel = PatInfo.DeptTel;//科室电话

            pat.PidApplyNo = PatInfo.PidApplyNo;
            pat.PidOrgId = PatInfo.PidOrgId;

            pat.HISPatientID = PatInfo.HISPatientID;
            pat.HISSerialnum = PatInfo.HISSerialnum;

            bsPat.ResetBindings(true);
            isDataFill = false;
        }

        protected bool isDataFill = false;


        /// <summary>
        /// 病人索引点击"面板设置"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void controlPatList1_PanelConfig(object sender, EventArgs args)
        {
            ShowConfigPanel();
        }

        /// <summary>
        /// 显示配置面板
        /// </summary>
        private void ShowConfigPanel()
        {
            FrmPatPanelConfig f = new FrmPatPanelConfig(this);
            f.Show();
        }

        protected override void afterUpdate(DataSet returnData)
        {
        }

        protected override void afterNew(DataSet returnData)
        {
        }

        protected override void afterDel(DataSet returnData)
        {
        }

        protected override void afterNew(DataSet returnData, object userObj)
        {
        }

        /// <summary>
        /// 上一次操作ID
        /// </summary>
        public string LastOperationID = string.Empty;

        /// <summary>
        /// 上一次报告操作密码
        /// </summary>
        public string LastReportOperationPw = string.Empty;
        protected bool IsDataChange = false;

        #region 工具栏事件

        /// <summary>
        /// 工具栏：新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnAddClicked(object sender, EventArgs e)
        {
            try
            {
                if (PatInfo == null)
                {
                    RefreshPatientDetails("-1");
                }
                
                IsDataChange = false;
                isNewSave = true;
                if (checkSaveBeforeLeave && controlPatList.CurrentPatient != null
                && !string.IsNullOrEmpty(controlPatList.CurrentPatient.RepId.ToString()))
                {
                    string pat_id = controlPatList.CurrentPatient.RepId.ToString();
                    if (!PatEnter.CheckResultBeforeAction(pat_id, false))
                    {
                        if (MessageDialog.Show("当前资料或结果未保存，是否保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            sysToolBar1_OnBtnSaveClicked(null, null);
                            return;
                        }
                    }
                }
                this.OnAddNew();

                string sid = txtPatSid.Text;
                SIDChanged();

                if (string.IsNullOrEmpty(txtPatSid.Text))
                {
                    txtPatSid.EditValue = sid;
                    txtPatSid.Text = sid;
                }
            }
            finally
            {
                IsDataChange = false;
                isNewSave = false;
            }

        }

        /// <summary>
        /// 工具栏：保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void sysToolBar1_OnBtnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                if (PatInfo == null)
                {
                    RefreshPatientDetails("-1");
                }
                isReLoadData = true;
                IsDataChange = false;
                int focusedRowHandle = controlPatList.gridViewPatientList.FocusedRowHandle; //保存时不跳到第一个　　2010-5-12

                string prev_sid = this.txtPatSid.Text;

                string patid = Save();

                EntityPidReportMain preCur = new EntityPidReportMain();
                preCur = controlPatList.CurrentPatient;
                controlPatList.listCheck = controlPatList.GetCheckedPatients();
                controlPatList.RefreshPatients();
                //保存刷新后记录上次勾选
                //controlPatList.SelectLastCheckPatients();//保存后不自动勾选，防止选择别的病人审核时连这个病人一同审核

                if (UserCustomSetting.PatResultPanel.SavePatInfoNoNext
                    && preCur != null
                    && !string.IsNullOrEmpty(preCur.RepSid))
                {
                    controlPatList.LocatePatient(preCur.RepSid);
                }

                if (patid != string.Empty)
                {
                    if (!UserCustomSetting.PatResultPanel.SavePatInfoNoNext)
                    {
                        EntityPidReportMain eyPatient = controlPatList.gridViewPatientList.GetFocusedRow() as EntityPidReportMain;
                        if (eyPatient != null)
                        {
                            itrHostFlag = DictInstrmt.Instance.GetItrHostFlag(eyPatient.RepItrId); //根据仪器代码ID获取通讯类型
                            //itrHostFlag仪器的通讯方式1=单向,2=双向
                            // 如果为双向仪器,并且序号不为空,则利用序号递增下一个样本号
                            if (itrHostFlag == 2 && (!string.IsNullOrEmpty(eyPatient.RepSerialNum)))   //if (itrHostFlag == 2 && (!string.IsNullOrEmpty(txt_pat_host_order.Text)))
                            {
                                controlPatList.gridViewPatientList.FocusedRowHandle = focusedRowHandle + 1; //原本没有加1
                                                                                                            //SetSID_AfterSaved(txt_pat_host_order.Text, true);
                                SetSID_AfterSaved(eyPatient.RepSerialNum, true);
                                FocusOnAddNewControl.Focus();
                            }
                            else
                            {
                                controlPatList.gridViewPatientList.FocusedRowHandle = focusedRowHandle;//保存时不跳到第一个　　2010-5-12
                                SetSID_AfterSaved(prev_sid);
                                FocusOnAddNewControl.Focus(); //保存后，焦点选中'ID类型' edit  2012-6-19
                                                              //this.OnAddNew();
                            }
                        }
                    }
                    if (UserCustomSetting.PatResultPanel.SaveFocusResultColumn)
                    {
                        PatEnter.SetColumnFocus();
                    }
                }


                controlPatList.gridViewPatientList.HorzScrollStep = controlPatList.gridViewPatientList.HorzScrollStep;//保存时不跳到第一个　　2010-5-12
            }
            finally
            {
                isReLoadData = false;
                IsDataChange = false;
            }

        }


        /// <summary>
        /// 工具栏：批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void sysToolBar1_OnBtnDeleteBatchClicked(object sender, EventArgs e)
        {
            OnDeleteBatch();
            this.RefreshPatients();
        }

        public bool OnDeleteBatch()
        {
            bool result = false;
            //获取勾选的病人
            bool delflag = false;
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();

            if (dcl.client.frame.UserInfo.GetSysConfigValue("BathOrSingleDelFlag").Equals("是"))
            {
                listPats = this.controlPatList.GetCheckedPatients();
                delflag = true;
            }
            else
            {
                delflag = false;
                if (this.controlPatList.CurrentPatient != null)
                {
                    listPats.Add(this.controlPatList.CurrentPatient);
                }
            }

            if (listPats.Count > 0)
            {

                List<string> listPatID = new List<string>();
                foreach (EntityPidReportMain dr in listPats)
                {
                    string pat_id = dr.RepId.ToString();
                    listPatID.Add(pat_id);
                }
                if (listPatID.Count >= 20)
                {
                    if (lis.client.control.MessageDialog.Show(string.Format("您将要删除 {0} 条病人记录，是否继续？", listPatID.Count), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return false;
                    }
                }
                else
                {
                    string show = string.Empty;
                    foreach (EntityPidReportMain dr in listPats)
                    {
                        if (!string.IsNullOrEmpty(dr.RepSerialNum))
                            show += string.Format("姓名:{0},流水号: {1} \r\n", dr.PidName != null ? dr.PidName.ToString() : " ", dr.RepSerialNum);
                        else
                            show += string.Format("姓名:{0},样本号: {1} \r\n", !string.IsNullOrEmpty(dr.PidName) ? dr.PidName : " ", !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty);
                    }
                    if (lis.client.control.MessageDialog.Show(string.Format("您将要删除\r\n{0}的记录，是否继续？", show), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return false;
                    }
                }


                //身份验证
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.BillPopedomCode.Delete, "", "");
                frmCheck.txtLoginid.Text = LastOperationID;
                frmCheck.ActiveControl = frmCheck.txtPassword;
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    EntityRemoteCallClientInfo caller = dcl.client.common.Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName, frmCheck.OperatorSftId);
                    //调用子类删除
                    List<EntityOperationResult> listOpResult = PatEnter.DeleteBatch(caller, listPatID);

                    if (listOpResult.Count > 0)
                    {
                        StringBuilder msg = new StringBuilder();
                        List<string> listIDtoDelete = new List<string>();
                        //删除成功的从左侧列表中移除
                        foreach (EntityOperationResult opResult in listOpResult)
                        {
                            string patid = opResult.Data.Patient.RepId;
                            EntityPidReportMain drpat = GetRowByPatId(listPats, patid);

                            opResult.Data.Patient.PidName = drpat.PidName;
                            opResult.Data.Patient.RepSid = drpat.RepSid;

                            if (opResult.Success)
                            {
                                listIDtoDelete.Add(patid);
                            }
                        }


                        if (msg.Length > 0)
                        {
                            ProxyLogin proxy = new ProxyLogin();
                            bool userInfo = proxy.Service.InsertSystemLog("删除",this.Text, frmCheck.OperatorID, UserInfo.ip, UserInfo.mac, msg.ToString());
                        }

                        this.controlPatList.Remove(listIDtoDelete);

                        //刷新
                        this.controlPatList.RefreshCurrent();

                        if (AnySuccess(listOpResult, false))//存在删除失败的记录则显示出来
                        {
                            FrmOpResultViwer opViewer = new FrmOpResultViwer(listOpResult);
                            opViewer.ShowDialog();
                        }
                        else
                        {
                            if (this.controlPatList.PatientsCount == 0)
                            {
                                this.OnAddNew();
                                SIDChanged();
                            }
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("删除成功");
                            result = true;
                        }
                    }
                    LastOperationID = frmCheck.OperatorID;
                }
            }
            else
            {
                if (delflag == true)
                    lis.client.control.MessageDialog.Show("请在病人索引中勾选需要删除的记录", "提示");
                else
                    lis.client.control.MessageDialog.Show("请在病人索引中选中需要删除的记录", "提示");
            }
            return result;
        }

        /// <summary>
        /// 返回是否存在指定success
        /// </summary>
        /// <param name="listOpResult"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        private bool AnySuccess(List<EntityOperationResult> listOpResult, bool success)
        {
            foreach (EntityOperationResult opResult in listOpResult)
            {
                if (opResult.Success == success)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 工具栏：删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            OnDeleteBatch();

            this.RefreshPatients();
        }

        /// <summary>
        /// 工具栏"刷新"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            //刷新列表
            this.controlPatList.RefreshPatients();
            this.controlPatList.listCheck = new List<EntityPidReportMain>();
            IsDataChange = false;
        }

        public void RefreahData()
        {
            this.controlPatList.RefreshPatients();
            this.controlPatList.listCheck = new List<EntityPidReportMain>();
            IsDataChange = false;
        }

        #region 审核相关
        /// <summary>
        /// 工具栏：审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnAuditClicked(object sender, EventArgs e)
        {
            if (controlPatList.PatientsCount <= 0)
                return;

            OnAuditBatch();//OnAudit();
            //******************************************************************************//
            //设置焦点行问题
            setControlPatListFocus("report_isFocusOnTheFirstRow");

            //******************************************************************************//
        }

        /// <summary>
        /// 工具栏：反审
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnUndoAuditClicked(object sender, EventArgs e)
        {
            if (controlPatList.PatientsCount <= 0)
                return;

            //获取勾选的病人
            List<EntityPidReportMain> drs = this.controlPatList.GetCheckedPatients();

            if (drs.Count > 0)
            {
                PatEnterUILogic uil = new PatEnterUILogic(this, PatEnter.ItrDataType);
                uil.strLastOperationID = LastOperationID;
                uil.ItrName = this.txtPatInstructment.displayMember;
                uil.UndoAuditBatch(drs, null, null);
                LastOperationID = uil.strLastOperationID;
            }
            else
            {
                lis.client.control.MessageDialog.Show(string.Format("请在病人列表中勾选需要取消{0}的记录", AuditWord), "提示");
            }
        }


        /// <summary>
        /// 批量审核
        /// </summary>
        public void OnAuditBatch()
        {
            this.controlPatList.CloseEditor();

            List<EntityPidReportMain> drs = this.controlPatList.GetCheckedPatients();

            if (drs.Count > 0)
            {
                PatEnterUILogic uil = new PatEnterUILogic(this, PatEnter.ItrDataType);
                uil.strLastOperationID = LastOperationID;
                if (checkCurrentPatientInfo)
                {
                    if (CheckCurrentPatientResInfo(uil)) return;
                }
                //调用逻辑层批量审核方法
                uil.ItrName = this.txtPatInstructment.displayMember;
                uil.Itr_ID = this.txtPatInstructment.valueMember;
                uil.AuditBatch(drs);
                LastOperationID = uil.strLastOperationID;
            }
            else
            {
                lis.client.control.MessageDialog.Show(string.Format("请在病人列表中勾选需要{0}的记录", AuditWord), "提示");
            }
        }



        private bool CheckCurrentPatientResInfo(PatEnterUILogic uil)
        {
            EntityPidReportMain row = controlPatList.CurrentPatient;
            if (row != null && !string.IsNullOrEmpty(row.RepId))
            {
                uil.PatIdAndInNoAndName = string.Format("{0}:{1}:{2}", row.RepId,
                                                        txtPatID.Text.Trim(), txtPatName.Text.Trim());

                if (!PatEnter.CheckResultBeforeAction(row.RepId.ToString(), true))
                {
                    return true;
                }
            }
            return false;
        }


        #endregion

        #region 报告相关
        /// <summary>
        /// 报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnReportClicked(object sender, EventArgs e)
        {
            if (controlPatList.PatientsCount <= 0)
                return;

            int tempFocusedRowHandle = controlPatList.gridViewPatientList.FocusedRowHandle;
            //当前行
            EntityPidReportMain pat = controlPatList.gridViewPatientList.GetRow(tempFocusedRowHandle) as EntityPidReportMain;
            string nextPatID = "";//记录下一个id号
            if (controlPatList.gridViewPatientList.RowCount > 0)
            {
                if (tempFocusedRowHandle < (controlPatList.gridViewPatientList.RowCount - 1))
                {
                    EntityPidReportMain patient = controlPatList.gridViewPatientList.GetRow(tempFocusedRowHandle + 1) as EntityPidReportMain;
                    if (patient != null && patient.RepId.Length > 0)
                    {
                        nextPatID = patient.RepId.ToString();
                    }
                }
            }


            OnReportBatch(false);
            //******************************************************************************//
            //设置焦点行问题
            setControlPatListFocus("report_isFocusOnTheFirstRow");

            //******************************************************************************//

            //系统配置：二审后焦点跳到下一行
            if (ConfigHelper.GetSysConfigValueWithoutLogin("report_isFocusOnTheNextRow") == "是")
            {
                if (controlPatList.gridViewPatientList.RowCount > 0)
                {
                    //当审核到最后一条的时候，不跳转到下一条
                    //如果没有下一个patID号,则跳到第一行
                    if (string.IsNullOrEmpty(nextPatID))
                    {
                        if (pat != null)
                            controlPatList.LocatePatientByPatID(pat.RepId, true);

                        //controlPatList.gridViewPatientList.FocusedRowHandle = 0;
                        //EntityPidReportMain dr = controlPatList.CurrentPatient;
                        //if (dr != null)
                        //{
                        //    controlPatList.LocatePatientByPatID(dr.RepId.ToString(), true);
                        //}
                    }
                    else
                    {
                        controlPatList.LocatePatientByPatID(nextPatID, true);
                    }
                }
            }//否则 则不跳转 就在当前行 不然会跳到最后一行
            else
            {
                controlPatList.LocatePatientByPatID(pat.RepId, true);
            }
        }

        /// <summary>
        /// 工具栏：取消报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnUndoReportClicked(object sender, EventArgs e)
        {
            if (controlPatList.PatientsCount <= 0)
                return;

            OnUndoReportBacth();
        }

        /// <summary>
        /// 工具栏：批量报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void sysToolBar1_OnBatchReportClicked(object sender, EventArgs e)
        {
            OnReportBatch(false);
            //******************************************************************************//
            //设置焦点行问题
            setControlPatListFocus("report_isFocusOnTheFirstRow");
            //******************************************************************************//
        }

        private void sysToolBar1_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            OnReportBatch(true);
            //******************************************************************************//
            //设置焦点行问题
            setControlPatListFocus("report_isFocusOnTheFirstRow");
        }


        /// <summary>
        /// 工具栏：批量取消报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void sysToolBar1_OnBatchUndoReportClicked(object sender, EventArgs e)
        {
            int tempFocusedRowHandle = controlPatList.gridViewPatientList.FocusedRowHandle;
            //当前行
            EntityPidReportMain pat = controlPatList.gridViewPatientList.GetRow(tempFocusedRowHandle) as EntityPidReportMain;
            OnUndoReportBacth();
            controlPatList.LocatePatientByPatID(pat.RepId, true);
        }


        /// <summary>
        /// 批量报告
        /// </summary>
        protected virtual void OnReportBatch(bool isprint)
        {
            List<EntityPidReportMain> drs = this.controlPatList.GetCheckedPatients();
            if (drs.Count > 0)
            {
                PatEnterUILogic uil = new PatEnterUILogic(this, PatEnter.ItrDataType);
                uil.strLastOperationID = LastOperationID;
                uil.strLastOperationPw = LastReportOperationPw;
                if (checkCurrentPatientInfo)
                {
                    if (CheckCurrentPatientResInfo(uil))
                        return;
                }
                uil.ItrName = this.txtPatInstructment.displayMember;
                uil.Itr_ID = this.txtPatInstructment.valueMember;
                bool success = uil.ReoprtBatch(drs);

                if (success)
                {
                    LastReportOperationPw = uil.strLastOperationPw;

                    string printOnReport = UserInfo.GetSysConfigValue("PrintOnReport");
                    if (printOnReport == "是" || controlPatList.IsAtuoPrint || isprint)
                    {
                        MessageDialog.ShowAutoCloseDialog("审核成功，正在打印......", 2m);
                        this.OnPatPrint(null, false);
                    }

                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
                        this.RefreshPatients();
                        this.SelectAllPatient(false);
                    }
                }
                LastOperationID = uil.strLastOperationID;
            }
            else
            {
                lis.client.control.MessageDialog.Show("请在病人列表中勾选需要" + LocalSetting.Current.Setting.ReportWord + "的记录", "提示");
            }
        }

        /// <summary>
        /// 批量取消报告
        /// </summary>
        protected virtual void OnUndoReportBacth()
        {
            List<EntityPidReportMain> drs = this.controlPatList.GetCheckedPatients();
            if (drs.Count > 0)
            {
                PatEnterUILogic uil = new PatEnterUILogic(this, PatEnter.ItrDataType);
                uil.strLastOperationID = LastOperationID;
                uil.ItrName = this.txtPatInstructment.displayMember;
                uil.UndoReoprtBatch(drs);
                LastOperationID = uil.strLastOperationID;
            }
            else
            {
                lis.client.control.MessageDialog.Show("请在病人列表中勾选需要取消" + LocalSetting.Current.Setting.ReportWord + "的记录", "取消" + LocalSetting.Current.Setting.ReportWord);
            }
        }
        #endregion

        /// <summary>
        /// 工具栏 结果视窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnResultViewClicked(object sender, EventArgs e)
        {
            string itr_id = this.txtPatInstructment.valueMember;//仪器ID
            DateTime dtPat_date = (DateTime)this.txtPatDate.EditValue;//日期
            PatEnter.ResultView(dtPat_date, itr_id);
        }

        /// <summary>
        /// 查看质控图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnQualityImageClicked(object sender, EventArgs e)
        {
            if (this.txtPatInstructment.valueMember != null
            && this.txtPatInstructment.valueMember != string.Empty
            && this.txtPatDate.EditValue != null
            && this.txtPatDate.EditValue != DBNull.Value
            )
            {
                string itr_id = this.txtPatInstructment.valueMember;//仪器ID
                DateTime dtPat_date = (DateTime)this.txtPatDate.EditValue;//日期

                PatEnter.QualityImageView(dtPat_date, this.txtPatInstructment.valueMember, this.txtPatInstructment.displayMember);
            }
            else
            {
                lis.client.control.MessageDialog.Show("请选择仪器", "提示");
            }
        }

        /// <summary>
        /// 工具栏 样本进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnSampleMonitorClicked(object sender, EventArgs e)
        {
            string itr_id = this.txtPatInstructment.valueMember;//仪器ID
            string itr_name = this.txtPatInstructment.displayMember;//仪器名称

            string type_id = controlPatList.TypeID;//物理组ID
            string type_name = controlPatList.TypeName;//物理组名称

            this.sysToolBar1.Focus();

            if (!string.IsNullOrEmpty(type_id))
            {
                FrmMonitor frm = new FrmMonitor(itr_id, itr_name, type_id, type_name, Convert.ToDateTime(this.txtPatDate.EditValue));
                frm.ShowDialog();
            }
        }


        /// <summary>
        /// 工具栏：打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            if (controlPatList.PatientsCount <= 0)
                return;

            Print(true);
        }

        /// <summary>
        /// 工具栏：打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void sysToolBar1_OnBtnPrintClicked(object sender, EventArgs e)
        {
            if (Lab_ReportCodeIsNullNotAllowPrint)
            {
                //打印前,刷新当前列表信息.令其状态保持跟数据库一致
                string prtTemplate = DictInstrmt.Instance.GetItrPrtTemplate(this.txtPatInstructment.valueMember);
                if (prtTemplate == string.Empty)
                {

                }
                else
                {
                    controlPatList.listCheck = controlPatList.GetCheckedPatients();
                    if (controlPatList.listCheck.Count > 0)
                    {
                        controlPatList.RefreshPatients();
                        //保存刷新后记录上次勾选
                        controlPatList.SelectLastCheckPatients();
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("请勾选需要打印的数据", "提示");
                        return;
                    }

                    //List<EntityPidReportMain> listPatient = this.controlPatList.GetCheckedPatients();
                    //if (listPatient.Count > 0)
                    //{
                    //    List<string> tempPatIDs = new List<string>();//记录勾选中的pat_id
                    //    foreach (EntityPidReportMain dr in listPatient)
                    //    {
                    //        tempPatIDs.Add(dr.RepId.ToString());
                    //    }
                    //    //刷新列表
                    //    this.controlPatList.RefreshPatients();

                    //    //刷新当前病人信息
                    //    if (this.controlPatList.CurrentPatient != null)
                    //    {
                    //        this.RefreshPatientDetails(this.controlPatList.CurrentPatient.RepId.ToString());
                    //    }

                    //    listPatient = this.controlPatList.GetALLPatients();
                    //    if (listPatient.Count > 0)
                    //    {
                    //        foreach (EntityPidReportMain dr in listPatient)
                    //        {
                    //            if (tempPatIDs.Contains(dr.RepId.ToString()))
                    //            {
                    //                dr.PatSelect = true;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        lis.client.control.MessageDialog.Show("请勾选需要打印的数据", "提示");
                    //        return;
                    //    }
                    //}
                }
            }

            Print(false);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="isPreview"></param>
        private void Print(bool isPreview)
        {
            OnPatPrint(null, isPreview);
        }

        //删除危急值信息patId集合
        private List<string> listDeleteMsgPatId = null;
        /// <summary>
        /// 打印/预览
        /// <param name="pat_id">病人ID，如果传null值则打印当前勾选中的病人</param>
        /// <param name="isPreview">当前打印是否为打印预览</param>
        /// </summary>
        private void OnPatPrint(string pat_id, bool isPreview)
        {
            PrintNoUpdateStartPatIDs = new List<string>();//打印前,清空此数据
            string prtTemplate = DictInstrmt.Instance.GetItrPrtTemplate(this.txtPatInstructment.valueMember);
            if (prtTemplate == string.Empty)
            {
                lis.client.control.MessageDialog.Show("找不到当前仪器的打印模版", "提示");
                return;
            }


            this.BeginLoading();
            List<string> listPatID = new List<string>();
            StringBuilder sbPatSidWhere = new StringBuilder();
            StringBuilder sbPatSidWhere2 = new StringBuilder();
            if (pat_id == null)
            {
                List<EntityPidReportMain> listPatient = this.controlPatList.GetCheckedPatients();
                if (listPatient.Count > 0)
                {
                    foreach (EntityPidReportMain dr in listPatient)
                    {
                        if (isPreview)
                        {
                            //初始状态标本在预览前更新相应的参考值
                            if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Natural)
                            {
                                //Thread combineItemThread = new Thread(new ThreadStart(delegate ()
                                //{

                                //}));
                                //combineItemThread.IsBackground = true;
                                //combineItemThread.Start();
                                new ProxyPidReportMainAudit().Service.UpdateObrResult(dr);
                            }
                            listPatID.Add(dr.RepId.ToString());
                        }
                        else
                        {
                            if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed || dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                            {
                                if (Lab_EnableNoBarCodeCheck
                                    && (dr.RepPrintFlag == null)
                                    && (string.IsNullOrEmpty(dr.RepBarCode.ToString())))
                                {
                                    string currentitrID = txtPatInstructment.valueMember;

                                    if (Lab_NoBarCodeCheckItrExpectList.Contains(currentitrID))
                                    {
                                        listPatID.Add(dr.RepId.ToString());
                                    }
                                    else
                                    {
                                        sbPatSidWhere.Append(string.Format(",[{0}]", dr.RepSid));
                                    }
                                }
                                else
                                {
                                    if (Lab_ReportCodeIsNullNotAllowPrint &&
                                        ((string.IsNullOrEmpty(dr.PidChkName) ||
                                          string.IsNullOrEmpty(dr.BgName))))
                                    {
                                        sbPatSidWhere2.Append(string.Format(",[{0}]", dr.RepSid));
                                        continue;
                                    }
                                    listPatID.Add(dr.RepId.ToString());
                                }
                                //报告打印后确认内部危急值
                                if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_PrintReportConfirmMsg") == "是"
                                      && dr.RepUrgentFlag == 1
                                      && dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                                {
                                    if (listDeleteMsgPatId == null)
                                        listDeleteMsgPatId = new List<string>();
                                    listDeleteMsgPatId.Add(dr.RepId);
                                }
                            }
                            else if (dr.RepTempFlag.ToString() == "1")
                            {
                                PrintNoUpdateStartPatIDs.Add(dr.RepId.ToString());//打印后,不更新打印状态的
                                listPatID.Add(dr.RepId.ToString());
                            }
                        }
                    }

                    if (sbPatSidWhere.Length > 0)
                    {
                        MessageDialog.Show("样本号为:" + sbPatSidWhere.Remove(0, 1) + " 的报告单无条码信息，需要打印确认", "提示");
                        return;
                    }
                    if (sbPatSidWhere2.Length > 0)
                    {
                        MessageDialog.Show("样本号为:" + sbPatSidWhere2.Remove(0, 1) + " 的报告单无审核者信息，需重新审核", "提示");
                        this.CloseLoading();
                        return;
                    }
                    if (listPatID.Count == 0)
                    {
                        lis.client.control.MessageDialog.Show("没有符合打印/预览要求的记录，请检查选中的记录是否已" + LocalSetting.Current.Setting.ReportWord, "提示");
                        this.CloseLoading();
                        return;
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.Show("在请左侧勾选需要预览/打印的病人", "提示");
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
                printPara.RepId = patient_id;
                printPara.ReportCode = prtTemplate;
                printPara.Sequence = sequence;
                if (PatEnter.ItrDataType == LIS_Const.InstmtDataType.Description)
                {
                    printPara.CustomParameter.Add("ItrRepFlag", "4");
                }
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
                        lis.client.control.MessageDialog.Show(ex1.MSG);
                    }
                    catch (Exception ex2)
                    {
                        dcl.root.logon.Logger.WriteException(this.GetType().ToString(), "print", ex2.Message);
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
                    this.SelectAllPatient(false);
                }
                catch (ReportNotFoundException ex1)
                {
                    lis.client.control.MessageDialog.Show(ex1.MSG);
                }
                catch (Exception ex2)
                {
                    dcl.root.logon.Logger.WriteException(this.GetType().ToString(), "print", ex2.Message);
                }
            }

            //tip.Close();

            #endregion
            this.CloseLoading();
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
                lis.client.control.MessageDialog.Show(ex1.MSG);
            }
            catch (Exception ex2)
            {

            }
        }

        Hashtable htPrint2;
        List<EntityDCLPrintParameter> listPrintData_multithread;

        bool useMultiThread = true;

        /// <summary>
        /// 打印后不更新打印状态的patID
        /// </summary>
        private List<string> PrintNoUpdateStartPatIDs = new List<string>();
        void pForm_PrintStart2(List<EntityDCLPrintParameter> listPara)
        {
            List<string> listPatID = new List<string>();

            foreach (EntityDCLPrintParameter item in listPara)
            {
                if (!PrintNoUpdateStartPatIDs.Contains(item.RepId))
                {
                    listPatID.Add(item.RepId);
                }
            }

            if (listPatID != null && listPatID.Count > 0)
            {
                UpdatePrintState(listPatID);
            }
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_PrintReportConfirmMsg") == "是"
              && listDeleteMsgPatId != null && listDeleteMsgPatId.Count > 0)
            {
                EntityAuditInfo audit = new EntityAuditInfo();
                audit.UserId = UserInfo.loginID;
                audit.UserName = UserInfo.userName;
                audit.MsgAffirmType = "3";//1-自动确认 2-手工确认 2-报告单打印确认
                audit.IsOnlyInsgin = true;
                ProxyUserMessage proxyUserMsg = new ProxyUserMessage();
                List<EntityDicObrMessageContent> messageList = new List<EntityDicObrMessageContent>();
                ProxyObrMessageContent proxyMsg = new ProxyObrMessageContent();
                messageList = proxyMsg.Service.GetAllMessage(false, false, true);
                foreach (string id in listDeleteMsgPatId)
                {
                    ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();
                    EntityDicObrMessageContent message = messageList.Find(i => i.ObrValueA == id);
                    if (message != null)
                        proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(audit, message.ObrId, id);
                }
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
                //如果有登陆用户,则用登陆用户作为操作者记录报告打印流程
                if (UserInfo.userName != null && UserInfo.loginID != null && UserInfo.userName.Length > 0 && UserInfo.loginID.Length > 0)
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
                        strPlace = dcl.common.IPUtility.GetIP();
                    }
                    string repStatus = LIS_Const.PATIENT_FLAG.Printed;
                    new ProxyPidReportMain().Service.UpdatePrintState_whitOperator(listPatID, repStatus, UserInfo.loginID, UserInfo.userName, strPlace);
                }
                else
                {
                    string repStatus = LIS_Const.PATIENT_FLAG.Printed;
                    new ProxyPidReportMain().Service.UpdatePrintState(listPatID, repStatus);
                }

                foreach (string pat_id in listPatID)
                {
                    EntityPidReportMain drPat = this.controlPatList.GetPatient(pat_id);
                    if (drPat != null)
                    {
                        drPat.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Printed);
                        drPat.RepStatusName = "已打印";
                    }
                }
            }
        }

        #endregion


        /// <summary>
        /// 根据PatID返回DataRow
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="patId"></param>
        /// <returns></returns>
        private EntityPidReportMain GetRowByPatId(List<EntityPidReportMain> drs, string patId)
        {
            foreach (EntityPidReportMain dr in drs)
            {
                if (dr.RepId.ToString() == patId)
                {
                    return dr;
                }
            }
            return null;
        }

        /// <summary>
        /// 窗体正在关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void FrmPatInputBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall && lis.client.control.MessageDialog.Show("您确定要关闭当前窗口吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void txtPatSampleType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            IsDataChange = true;
            EntityPidReportMain patient = (EntityPidReportMain)bsPat.Current;
            if (patient != null && !string.IsNullOrEmpty(txtPatSampleType.popupContainerEdit1.Text.ToString()))
            {
                patient.SamName = txtPatSampleType.popupContainerEdit1.Text;
            }
            PatEnter.SampleChanged(this.txtPatSampleType.valueMember);
        }

        private void textAgeInput1_Leave(object sender, EventArgs e)
        {
            IsDataChange = true;
            PatEnter.PatAgeChanged(this.textAgeInput1.AgeToMinute);
            //年龄判断
            if (textAgeInput1.AgeYear > 100 && UserInfo.GetSysConfigValue("UseAgeNotice") == "是")
                lis.client.control.MessageDialog.Show("年龄超过100岁，请确认输入是否正确！", "提示");
        }

        private void txtPatBarCode_TextChanged(object sender, EventArgs e)
        {
            if (this.txtPatBarCode.Text.Trim() == string.Empty)
            {
                this.txtPatSampleDate.Properties.ReadOnly = false;
                this.txtPatSDate.Properties.ReadOnly = false;
                this.txtPatReachDate.Properties.ReadOnly = false;
                this.txtPatApplyTime.Properties.ReadOnly = false;
                this.txtPatReceiveTime.Properties.ReadOnly = false;
            }
            else if (UserInfo.GetSysConfigValue("UseBarcode") == "是") //启用条码的才只读
            {
                this.txtPatSampleDate.Properties.ReadOnly = true;
                this.txtPatSDate.Properties.ReadOnly = true;
                this.txtPatReachDate.Properties.ReadOnly = true;
                this.txtPatApplyTime.Properties.ReadOnly = true;
                this.txtPatReceiveTime.Properties.ReadOnly = true;
            }
        }

        string prevPatIdType = null;
        private void txtPatIdType_onAfterChange(EntityDicPubIdent oldRow)
        {
            prevPatIdType = this.txtPatIdType.valueMember;
            string ori_id = DictPatNumberType.Instance.GetOriID_byNoType(this.txtPatIdType.valueMember);
            this.txtPatSource.SelectByID(ori_id);
        }

        private void txtPatSampleType_onBeforeFilter()
        {
            List<EntityDicSample> listSamp = new List<EntityDicSample>();
            listSamp = txtPatSampleType.getDataSource().Where(w => w.SamId != "-1").ToList();
            txtPatSampleType.SetFilter(listSamp);
        }

        /// <summary>
        /// 标本备注改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtPatSamRem_DisplayTextChanged(object sender, control.ValueChangeEventArgs args)
        {
            IsDataChange = true;
            PatEnter.PatDiagChanged(txtPatDiag.displayMember);
        }


        protected bool bDataIsBinding = false;

        private void fpat_exp2_EditValueChanged(object sender, EventArgs e)
        {
            IsDataChange = true;

        }

        private void fpat_comment_EditValueChanged(object sender, EventArgs e)
        {
            IsDataChange = true;

        }

        #region IPatPrint 成员



        #endregion

        #region IPatientList 成员

        /// <summary>
        /// 选择/取消选择所有病人
        /// </summary>
        /// <param name="selectAll"></param>
        public void SelectAllPatient(bool selectAll)
        {
            this.controlPatList.SelectAllPatientInGrid(selectAll);
        }

        #endregion

        bool bAllowFirePatDept_ValueChanged = true;

        /// <summary>
        /// 送检科室改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtPatDeptId_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            IsDataChange = true;
            if (bAllowFirePatDept_ValueChanged)
            {
                string dep_code = this.txtPatDeptId.valueMember;
                PatEnter.DepChanged(dep_code);
                if (string.IsNullOrEmpty(dep_code))
                {
                    this.txt_pat_ward_id.Text = string.Empty;
                    this.txt_pat_ward_name.Text = string.Empty;
                }
                else
                {
                    this.txt_pat_ward_id.Text = DictDepartment.Instance.GetWardCode(dep_code);
                }
            }
        }

        private void controlPatList1_AddNewDemand(object sender, EventArgs e)
        {
            OnAddNew(false, false);
            SIDChanged();
        }

        #region IPatientList 成员


        public void RefreshPatients()
        {
            this.controlPatList.RefreshPatients();
            this.OnAddNew();
            SIDChanged();
        }

        #endregion

        private void txtPatID_EditValueChanged(object sender, EventArgs e)
        {
            IsDataChange = true;
            if (txtPatID.Text != null && txtPatID.Text.StartsWith("#"))
                txtPatID.Text = txtPatID.Text.Replace("#", "").Trim();
        }

        /// <summary>
        /// 资料复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            this.controlPatList.CloseEditor();

            //获取勾选的病人信息
            List<EntityPidReportMain> listPats = this.controlPatList.GetCheckedPatients();

            if (listPats.Count > 0)
            {
                FrmPatInfoCopy frmCopy = new FrmPatInfoCopy(listPats);
                frmCopy.ShowDialog();
            }
            else
                lis.client.control.MessageDialog.Show("请勾选要复制的数据");
        }

        FrmPatHistoryEXP frmHistory = null;

        private void btnHistoryEXP_Click(object sender, EventArgs e)
        {
            ShowHistoryExp();
        }

        private void ShowHistoryExp()
        {
            if (this.controlPatList.CurrentPatient != null)
            {
                if (frmHistory == null)
                {
                    frmHistory = new FrmPatHistoryEXP();
                    //***************************************************************************
                    //窗体显示在屏幕中间
                    frmHistory.StartPosition = FormStartPosition.CenterScreen;
                    frmHistory.ExpDouclick += new FrmPatHistoryEXP.ExpDouclickEventHandler(frmHistory_ExpDouclick);
                    //***************************************************************************
                    //frmHistory.Left = Control.MousePosition.X;
                    //frmHistory.Top = Control.MousePosition.Y - frmHistory.Height - 10;
                }
                frmHistory.PatInNo = this.controlPatList.CurrentPatient.PidInNo.ToString();
                frmHistory.Visible = true;
                frmHistory.GetHistoryExp();
                //frmHistory.Left = Control.MousePosition.X;
                //frmHistory.Top = Control.MousePosition.Y - frmHistory.Height - 10;
                frmHistory.Show();
            }
        }

        void frmHistory_ExpDouclick(object sender, ExpDouclickEventArgs args)
        {
            fpat_exp2.Text += args.Pat_Exp;
        }

        /// <summary>
        /// 根据系统配置的代码设置是否将报告模块中的获焦行在首行还是尾行
        /// </summary>
        /// <param name="configCode"></param>
        public void setControlPatListFocus(string configCode)
        {
            if (ConfigHelper.GetSysConfigValueWithoutLogin(configCode) == "是")
            {
                controlPatList.gridViewPatientList.FocusedRowHandle = 0;
                int i = controlPatList.gridViewPatientList.RowCount;

                EntityPidReportMain dr = controlPatList.CurrentPatient;
                if (dr != null)
                {
                    controlPatList.LocatePatientByPatID(dr.RepId.ToString(), true);
                }
            }

        }

        /// <summary>
        /// 设置病人基本信息可修改性
        /// </summary>
        /// <param name="Modify"></param>
        public void setIsModify(bool Modify)
        {
            //系统配置：不能修改报告管理信息[模式]
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManagerMode") == "gzzyy")
            {
                #region 非通用
                this.txtPatIdType.Readonly = !Modify;
                this.txtPatID.Properties.ReadOnly = !Modify;
                this.txtPatDeptId.Readonly = !Modify;
                this.txtPatBedNo.Properties.ReadOnly = !Modify;
                this.txtPatName.Properties.ReadOnly = !Modify;
                this.txtPatSex.Readonly = !Modify;
                this.textAgeInput1.Properties.ReadOnly = !Modify;
                this.txtPatSampleType.Readonly = !Modify;
                this.txtPatDiag.Readonly = !Modify;
                this.txtPatSource.Readonly = !Modify;
                this.txtPatDoc2.Readonly = !Modify;
                this.txtPat_social_no.Properties.ReadOnly = !Modify;

                #endregion
            }
            else
            {
                #region 通用

                this.txtPatDeptId.Readonly = !Modify;
                this.txtPatBedNo.Properties.ReadOnly = !Modify;
                this.txtPatName.Properties.ReadOnly = !Modify;
                this.txtPatSex.Readonly = !Modify;
                this.textAgeInput1.Properties.ReadOnly = !Modify;
                this.txtPatDiag.Readonly = !Modify;
                this.txtPatSource.Readonly = !Modify;
                this.txtPatDoc2.Readonly = !Modify;
                this.txtPat_social_no.Properties.ReadOnly = !Modify;
                //角色权限：检验者录入许可
                if (!UserInfo.HaveFunction(243))
                {
                    txtPatInspetor.Readonly = true;
                }
                else
                {
                    this.txtPatInspetor.Readonly = !Modify;
                }

                this.txtPat_chk_purpose.Readonly = !Modify;
                this.txtPatReceiveTime.Properties.ReadOnly = !Modify;
                this.txtPatSampleDate.Properties.ReadOnly = !Modify;
                this.txtPatSDate.Properties.ReadOnly = !Modify;
                this.txtPatReachDate.Properties.ReadOnly = !Modify;
                this.txtPatApplyTime.Properties.ReadOnly = !Modify;
                this.txtPatRecDate.Properties.ReadOnly = !Modify;
                this.txtPatReportDate.Properties.ReadOnly = !Modify;
                #endregion
            }
        }

        private void sysToolBar1_BtnUploadVersionClick(object sender, EventArgs e)
        {
            List<EntityPidReportMain> drs = this.controlPatList.GetCheckedPatients();
            if (drs.Count > 0)
            {
                ProxyPidReportMain proxyPatient = new ProxyPidReportMain();
                string result = proxyPatient.Service.BatchUpload(drs);
                if (string.IsNullOrEmpty(result))
                {
                    MessageDialog.ShowAutoCloseDialog("上传成功!");
                }
                else {
                    lis.client.control.MessageDialog.Show("上传失败！" + result);
                }
            }
            else {
                lis.client.control.MessageDialog.Show("请勾选需要上传的报告！");
            }
        }
        void PatDateChange()
        {
            if (!PatDateChanging)
            {

                if (txtPatDate.EditValue == null)
                {
                    PatEnter.Reset();
                    return;
                }
                //跟原值不同->日期改变
                if (dtPrevPatDate.Date != ((DateTime)this.txtPatDate.EditValue).Date)
                {
                    try
                    {
                        PatDateChanging = true;
                        //控件数据重置
                        isDataFill = true;

                        if (this.PatInfo != null)
                        {
                            this.PatInfo = new EntityPidReportMain();
                        }

                        if (this.listPatCombine != null)
                        {
                            this.listPatCombine.Clear();
                        }

                        PatEnter.Reset();

                        DateTime dtToday = ServerDateTime.GetServerDateTime();
                        this.txtPatSampleDate.EditValue = dtToday;
                        if (UserInfo.GetSysConfigValue("AlwaysKeepSendDate") != "是") //如果不强制保存送检时间
                            this.txtPatSDate.EditValue = dtToday;
                        this.txtPatReachDate.EditValue = dtToday;
                        this.txtPatApplyTime.EditValue = dtToday;
                        this.txtPatRecDate.EditValue = dtToday;
                        this.txtPatReportDate.EditValue = dtToday;
                        SetDefaultInspector();
                        isDataFill = false;

                        if (PatEnter.CombineEditor.listRepDetail != null)
                        {
                            PatEnter.CombineEditor.listRepDetail.Clear();
                        }


                        PatEnter.PatDateChanged(((DateTime)this.txtPatDate.EditValue).Date);

                        dtPrevPatDate = ((DateTime)this.txtPatDate.EditValue).Date;


                        if (dtPrevPatDate.Date != dtToday.Date)
                        {
                            txtPatDate.BackColor = Color.Pink;
                        }
                        else
                        {
                            txtPatDate.BackColor = Color.White;
                        }

                        this.OnAddNew();
                        SIDChanged();
                    }
                    finally
                    {
                        PatDateChanging = false;
                    }
                }
            }
        }

        private void txtPatDate_MouseLeave(object sender, EventArgs e)
        {
            PatDateChange();
        }

        private void sysToolBar1_BtnUndoClick(object sender, EventArgs e)
        {
            sysToolBar1_OnBatchUndoReportClicked(sender, e);
        }
        private void sysToolBar1_BtnUndo2Click(object sender, EventArgs e)
        {
            sysToolBar1_OnUndoAuditClicked(sender, e);
        }

        private void txtPatName_EditValueChanged(object sender, EventArgs e)
        {
            IsDataChange = true;
        }

        private void txtPatBedNo_EditValueChanged(object sender, EventArgs e)
        {
            IsDataChange = true;
        }

        private void textAgeInput1_EditValueChanged(object sender, EventArgs e)
        {
            IsDataChange = true;
        }

        private void txtPatSampleState_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            IsDataChange = true;
        }

        private void sysToolBar1_BtnAnswerClick(object sender, EventArgs e)
        {

            EntityPidReportMain drtemp = this.controlPatList.CurrentPatient;
            if (drtemp != null)
            {
                if (drtemp.PidSrcId.ToString() == "108")
                {
                    if (drtemp.PidInNo.ToString().Length <= 0)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("当前没有住院号信息不能病历浏览");
                        return;
                    }

                    string temp_url = @"http://172.17.250.10:82/show.aspx?EncryptImf=strPatientCode&ZYHM=strPatientCode";

                    temp_url = temp_url.Replace("strPatientCode", drtemp.PidInNo.ToString());

                    //调用系统默认的浏览器   
                    //System.Diagnostics.Process.Start("explorer.exe", temp_url);
                    System.Diagnostics.Process.Start(temp_url);
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("当前不是住院病人不能病历浏览");
                }
            }

            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选中一行需要病历浏览的病人信息");
            }
        }

        private void controlPatList1_OnItrChanged(object sender, EventArgs e)
        {
            this.txtPatInstructment.displayMember = controlPatList.ItrName;
            this.txtPatInstructment.valueMember = controlPatList.ItrID;

            //选择仪器后如果物理组为空则填充当前仪器的物理组
            if (string.IsNullOrEmpty(this.txtPatType.valueMember))
            {
                string ctype_id = DictInstrmt.Instance.GetItrCTypeID(this.txtPatInstructment.valueMember);

                if (!string.IsNullOrEmpty(ctype_id))
                {
                    EntityDicPubProfession entityCType = DictType.Instance.GetCType(ctype_id);

                    if (entityCType != null)
                    {
                        this.txtPatType.valueMember = ctype_id;
                        this.txtPatType.displayMember = entityCType.ProTypeName;
                    }
                }
            }

            getCanMrgItrUser();
            ResetAll();
            //设置仪器默认样本类型
            SetItrDefalutSample();

            PatEnter.InstructmentChanged(this.txtPatInstructment.valueMember);
            PatEnter.CombineEditor.Reset();
            PatEnter.CombineEditor.RefreshEditBoxText();
            //设置仪器默认组合
            SetItrDefaultCombine();

            PatEnter.CombineEditor.ItrID = this.txtPatInstructment.valueMember;
        }

        private void controlPatList1_OnTypeChanged(object sender, EventArgs e)
        {
            //清空仪器
            this.txtPatInstructment.valueMember = string.Empty;
            this.txtPatInstructment.displayMember = string.Empty;
            txtPatType.displayMember = controlPatList.TypeName;
            txtPatType.valueMember = controlPatList.TypeID;
            //txtPatIdType.valueMember = controlPatList.TypeID;
            PatEnter.TypeChanged(this.txtPatType.valueMember);
            PatEnter.CombineEditor.CTypeID = controlPatList.TypeID;
        }

        private void txtEditDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDateType.Text) && txtPatReceiveTime.Properties.ReadOnly == false)
            {
                switch (txtDateType.Text)
                {
                    case "申请时间":
                        txtPatReceiveTime.EditValue = txtEditDate.EditValue;
                        break;
                    case "采样时间":
                        txtPatSampleDate.EditValue = txtEditDate.EditValue;
                        break;
                    case "送达时间":
                        txtPatReachDate.EditValue = txtEditDate.EditValue;
                        break;
                    case "接收时间":
                        txtPatApplyTime.EditValue = txtEditDate.EditValue;
                        break;
                    case "检验时间":
                        txtPatRecDate.EditValue = txtEditDate.EditValue;
                        break;
                    case "送检时间":
                        txtPatSDate.EditValue = txtEditDate.EditValue;
                        break;
                }
                try
                {
                    bsPat.EndEdit();
                    FillTimeLine((EntityPidReportMain)bsPat.DataSource);
                }
                catch
                { }
            }
        }

        private void txtPatInspetor_Load(object sender, EventArgs e)
        {
            if (DesignMode) return;
            this.txtPatInspetor.SetFilter(this.txtPatInspetor.getDataSource().FindAll(w => w.UserType == "检验组"));
        }

        private void txtDateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDateType.Text) && txtPatReceiveTime.Properties.ReadOnly == false)
            {
                switch (txtDateType.Text)
                {
                    case "申请时间":
                        txtEditDate.EditValue = txtPatReceiveTime.EditValue;
                        break;
                    case "采样时间":
                        txtEditDate.EditValue = txtPatSampleDate.EditValue;
                        break;
                    case "送达时间":
                        txtEditDate.EditValue = txtPatReachDate.EditValue;
                        break;
                    case "接收时间":
                        txtEditDate.EditValue = txtPatApplyTime.EditValue;
                        break;
                    case "检验时间":
                        txtEditDate.EditValue = txtPatRecDate.EditValue;
                        break;
                    case "送检时间":
                        txtEditDate.EditValue = txtPatSDate.EditValue;
                        break;
                }
            }
        }
        private void controlPatList1_UpdateCRBClick(object sender, EventArgs e)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();

            listPats = this.controlPatList.GetCheckedPatients();
            List<EntityPidReportMain> listUpdate = new List<EntityPidReportMain>();
            if (listPats.Count < 1)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择要更新为【传染病】的数据！");
                return;
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder sbother = new StringBuilder();
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendLine("以下患者报告已审核，无法更新为【传染病】类型报告：");
            sbMsg.Append("【{0}】");
            foreach (EntityPidReportMain dr in listPats)
            {
                if (dr.RepStatus == 0)
                {
                    listUpdate.Add(dr);
                }
                else
                {
                    sbother.Append("、" + dr.PidName + "");
                }
            }

            if (listUpdate.Count > 0)
            {
                foreach (EntityPidReportMain reportMain in listUpdate)
                {
                    reportMain.SampRemark = "传染病";
                }
                bool result = new ProxyPidReportMain().Service.UpdatePatientData(listUpdate);
                if (result)
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("更新成功！", 1m);

                    if (sbother != null && sbother.Length > 0)
                    {
                        lis.client.control.MessageDialog.Show(string.Format(sbMsg.ToString() + "\r\n 剩余的更新成功。", sbother.ToString().Substring(1)), "提示");
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("更新失败！");
                    return;
                }
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("无可更新的数据！", 1m);
                return;
            }
        }

        #region 中期报告(失效)

        /// <summary>
        /// 点击右键菜单--允许发送中期报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlPatList1_OnMenuAllowMidReportClicked(object sender, EventArgs e)
        {
            OnMidReportBatch();
        }

        /// <summary>
        /// 批量发送中期报告
        /// </summary>
        public void OnMidReportBatch()
        {
            this.controlPatList.CloseEditor();

            List<EntityPidReportMain> drs = this.controlPatList.GetCheckedPatients();
            if (drs.Count > 0)
            {
                PatEnterUILogic uil = new PatEnterUILogic(this, PatEnter.ItrDataType);

                if (checkCurrentPatientInfo)
                {
                    if (CheckCurrentPatientResInfo(uil)) return;
                }
                //调用逻辑层批量审核方法
                uil.ItrName = this.txtPatInstructment.displayMember;
                uil.Itr_ID = this.txtPatInstructment.valueMember;
                uil.MidReportBatch(drs);
            }
            else
            {
                lis.client.control.MessageDialog.Show("请在病人列表中勾选需要【发送中期报告】的记录", "提示");
            }
        }

        #endregion
    }
}
