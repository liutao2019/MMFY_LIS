using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

using dcl.client.common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using lis.client.control;
using dcl.client.report;
using dcl.client.result.CommonPatientInput;
using dcl.client.result.Interface;
using dcl.client.frame.runsetting;
using DevExpress.XtraEditors;
using dcl.common;
using dcl.client.result.DictToolkit;

using dcl.client.wcf;
using dcl.root.logon;
using dcl.client.result.PatControl;
using System.Threading;
using DevExpress.XtraBars;
using dcl.entity;
using System.Linq;
using Lis.CustomControls;
using dcl.client.cache;
using dcl.interfaces;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace dcl.client.result
{
    public partial class FrmBacterialInputNew : FrmCommon, IPatPanelConfig
    {
        #region 全局变量
        /// <summary>
        /// 面板配置类
        /// </summary>
        PatInputRuntimeSetting UserCustomSetting = null;

        /// <summary>
        /// 新增时获得焦点的控件
        /// </summary>
        Control FocusOnAddNewControl = null;

        /// <summary>
        /// 当前病人信息
        /// </summary>
        EntityPidReportMain CurrentPatInfo = new EntityPidReportMain();//病人消息


        List<EntityPidReportMain> AllPatList = new List<EntityPidReportMain>();

        /// <summary>
        /// 细菌字典
        /// </summary>
        List<EntityDicMicBacteria> BacteriList = new List<EntityDicMicBacteria>();//菌株

        DataTable dtAntibio = new DataTable();//药敏

        /// <summary>
        /// 细菌列表
        /// </summary>
        List<EntityObrResultBact> BactList = new List<EntityObrResultBact>();

        /// <summary>
        /// 药敏列表
        /// </summary>
        List<EntityObrResultAnti> AntiList = new List<EntityObrResultAnti>();

        /// <summary>
        /// 描述涂片结果列表
        /// </summary>
        List<EntityObrResultDesc> DescList = new List<EntityObrResultDesc>();

        /// <summary>
        /// 细菌无菌涂片字典列表
        /// </summary>
        List<EntityDicMicSmear> smearList = new List<EntityDicMicSmear>();

        private string ordate = "";
        String userTypes = "";
        string AuditWord = "审核";
        bool clearCheckType = true;
        /// <summary>
        /// 危急报告特定标本配置
        /// </summary>
        List<string> ListConfigCriticalSample = new List<string>();
        bool Lab_DisplaySamReturnButton = false;
        bool radioGroupEventEnable = true;
        /// <summary>
        /// 细菌报告界面样本号自动定位病人资料
        /// </summary>
        bool IsBacLab_sampleLocatePatient = false;
        private string patidentity = string.Empty;
        //用于判断病人ID是否进行了统一设置
        string PatientIDNameConfirm;
        private bool Lab_EnableNoBarCodeCheck = false;
        private string Lab_NoBarCodeCheckItrExpectList = "";
        bool Lab_ReportCodeIsNullNotAllowPrint = false;
        bool Lab_ShowNewSid = false;
        private bool CheckReceiveTimeAndPatdate = false;
        string Lab_NewSidCheckNullItrIDs = string.Empty;
        bool Lab_NoCheckSelectCurRow = false;
        /// <summary>
        /// [无菌和涂片]关联组合
        /// </summary>
        bool BacLab_DictNobactByComID = false;


        private bool checkSaveBeforeLeave = false;

        /// <summary>
        /// 需第三次审核的仪器ID
        /// </summary>
        public string Lab_ThreeAuditItrIDs = string.Empty;

        /// <summary>
        /// 用户是否有权修改病人信息
        /// </summary>
        public bool isCanModify = false;

        /// <summary>
        /// 用户是否修改病人信息
        /// </summary>
        bool isDataChaged = false;
        bool isLoadData = false;
        private string CurrentPatID;


        public bool Lab_NoBarcodeNeedAuditCheek = false;
        public string Lab_NoBarCodeAuditCheckItrExList = "";

        /// <summary>
        /// 审核、删除默认前一个人工号
        /// </summary>
        private bool IsRecordLastOperationID = false;
        /// <summary>
        /// 报告(二审)是否记录前一个人的密码
        /// </summary>
        private bool IsRecordLastReportOperationPw = false;


        /// <summary>
        /// 上一次操作ID
        /// </summary>
        public string strLastOperationID = string.Empty;

        /// <summary>
        /// 上一次操作密码
        /// </summary>
        public string strLastOperationPw = string.Empty;//密码

        private System.Windows.Forms.Timer clearPassWordTimer;
        
        private EntityPidReportMain NewPatientForSave { get; set; }
        
        #endregion

        #region 窗体构造与Load函数

        public FrmBacterialInputNew()
        {
            InitializeComponent();

            this.txtPatSid.Font = new Font("宋体", 12f, FontStyle.Bold);
            txtPatID.Font = new Font("宋体", 12f, FontStyle.Bold);
            txtPatName.Font = new Font("宋体", 12f, FontStyle.Bold);
            txtPatDeptId.Font = new Font("宋体", 9f, FontStyle.Bold);
            txtPatSampleType.Font = new Font("宋体", 9f, FontStyle.Bold);
            base.ShowSucessMessage = false;
            this.repositoryItemComboBox2.FormatEditValue += new DevExpress.XtraEditors.Controls.ConvertEditValueEventHandler(repositoryItemComboBox2_FormatEditValue);
            this.Lab_DisplaySamReturnButton = UserInfo.GetSysConfigValue("Lab_DisplaySamReturnButton") == "是";
            sy.OrderCustomer = true;//20120920

            this.ceCombine.CombineAdded += new CombineAddedEventHandler(ceCombine_CombineAdded);
            this.ceCombine.CombineRemoved += new CombineRemovedEventHandler(ceCombine_CombineRemoved);

            //开启修改病人的结果数据后，离开当前病人资料时提示保存修改
            checkSaveBeforeLeave = UserInfo.GetSysConfigValue("Lab_CheckSaveBeforeLeave") == "是";

            //获取用户是否有权修改病人信息的权限
            FrmCheckPassword check = new FrmCheckPassword();
            check.OperatorID = UserInfo.loginID;
            isCanModify = new UILogic.PatEnterUILogic().canReportOnNoBarCode(check, "ModifyReportManagerInfo");
            //系统配置：不能修改报告管理信息[模式]
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManagerMode") == "gzzyy")
            {
                isCanModify = new UILogic.PatEnterUILogic().canReportOnNoBarCode(check, "ModifyReportManagerInfo");
            }

            Lab_NoCheckSelectCurRow = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoCheckSelectCurRow") == "是";
            //系统配置：细菌报告界面样本号自动定位病人资料
            IsBacLab_sampleLocatePatient = UserInfo.GetSysConfigValue("BacLab_sampleLocatePatient") == "是";
            Lab_EnableNoBarCodeCheck = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_EnableNoBarCodeCheck") == "是";
            Lab_NoBarCodeCheckItrExpectList =
                ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeCheckItrExpectList");
            CheckReceiveTimeAndPatdate = UserInfo.GetSysConfigValue("Other_CheckReceiveTimeAndPatdate") == "是";

            Lab_ReportCodeIsNullNotAllowPrint =
              ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ReportCodeIsNullNotAllowPrint") == "是";

            Lab_ThreeAuditItrIDs = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ThreeAuditItrIDs");

            BacLab_DictNobactByComID = ConfigHelper.GetSysConfigValueWithoutLogin("BacLab_DictNobactByComID") == "是";

            CurrentPatID = "-1";

            this.sy.BtnUndoClick += new System.EventHandler(this.sy_BtnUndoClick);
            this.sy.BtnUndo2Click += new System.EventHandler(this.sy_BtnUndo2Click);
            if (ConfigHelper.GetSysConfigValueWithoutLogin("IsUseUpdateInfectiousDisease") == "是")
            {
                更新为传染病ToolStripMenuItem.Visible = true;
            }
            else
            {
                更新为传染病ToolStripMenuItem.Visible = false;
            }
        }


        private void FrmBacterialInput_Load(object sender, EventArgs e)
        {
            IsRecordLastOperationID = UserInfo.GetSysConfigValue("lab_default_id") == "是";
            IsRecordLastReportOperationPw = UserInfo.GetSysConfigValue("Lab_report_DefaultPw") == "是";

            int minute = ConvertHelper.IntParse(LocalSetting.Current.Setting.CachePwTime, 0);
            if (minute > 0)
            {
                clearPassWordTimer = new System.Windows.Forms.Timer();
                clearPassWordTimer.Interval = minute * 60 * 1000;
                clearPassWordTimer.Tick += ClearPassWordTimer_Tick;
                clearPassWordTimer.Start();
            }


            InitEvent();
            PatientIDNameConfirm = ConfigHelper.GetSysConfigValueWithoutLogin("PatientIDNameConfirm");
            LoadUserSetting();
            string btnDeRef = string.Empty;
            string btnreturn = string.Empty;
            if (UserInfo.GetSysConfigValue("Open_HisFeeView") == "是")
            {
                btnDeRef = "BtnDeRef";
                sy.BtnDeRef.Caption = "费用清单";
                sy.BtnDeRefClick += new EventHandler(sy_BtnDeRefClick);
            }

            string prv = string.Empty;
            if (UserInfo.GetSysConfigValue("Lab_ShowPrintPreviewBtn") == "是")
            {
                prv = "BtnPrintList";
                sy.BtnPrintList.Caption = "打印预览";
                sy.OnBtnPrintListClicked += sy_OnPrintPreviewClicked;
            }
            string srBtnDeBaResult = string.Empty;
            //系统配置：细菌报告管理显示[默认阴性结果]按钮
            if (UserInfo.GetSysConfigValue("BacLab_ShowBtnDeBaResult") == "是")
            {
                srBtnDeBaResult = "BtnDeSpe";
                string strBtnName = UserInfo.GetSysConfigValue("BacLab_DefaultNegativeBtnName");
                if (!string.IsNullOrEmpty(strBtnName))
                    sy.BtnDeSpe.Caption = strBtnName;
                else
                    sy.BtnDeSpe.Caption = "默认阴性结果";
                sy.BtnDeSpeClick += new EventHandler(sy_BtnDeSpeClick);
            }
            string btnAuditAndPrint = UserInfo.GetSysConfigValue("Lab_ReportAndPrintCusName");
            string BtnSinglePrint = "";
            if (!string.IsNullOrEmpty(btnAuditAndPrint))
            {
                BtnSinglePrint = "BtnSinglePrint";
                sy.BtnSinglePrint.Caption = btnAuditAndPrint;
                sy.OnBtnSinglePrintClicked += new EventHandler(sy_OnBtnSinglePrintClicked);
            }

            //系统配置：细菌药敏[危急报告复选框]名称
            string BtchkPatCriticaltext = UserInfo.GetSysConfigValue("BtchkPatCriticaltext");

            if (!string.IsNullOrEmpty(BtchkPatCriticaltext))
            {
                chk_pat_critical.Text = BtchkPatCriticaltext;
            }

            //系统配置：细菌药敏显示[列名]危急类型
            if (UserInfo.GetSysConfigValue("BtShowColBarWjtext") == "是")
            {
                colbar_wjtext.Visible = true;
            }
            //系统配置：细菌药敏允许相同细菌
            if (UserInfo.GetSysConfigValue("BtAllowAddSameBarbid") != "是")
            {
                gridColumn26.GroupIndex = -1;
                gridColumn5.GroupIndex = 0;
                gridColumn26.Visible = false;//隐藏,以anr_seq分组药敏结果
            }
            if (UserInfo.GetSysConfigValue("BacLab_AntiGroup") == "是")
            {
                gridColumn28.GroupIndex = 2;
                gridColumn28.Visible = true;
            }
            else
            {
                gridColumn28.GroupIndex = -1;
                gridColumn28.Visible = false;
            }
            string BtnQualityAudit = string.Empty;


            int days;
            string iterdays = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_BacHistoryDataQueryDays");
            if (!int.TryParse(iterdays, out days))
            {
                days = 30;
            }

            dtBegin.EditValue = DateTime.Now;
            dtEnd.EditValue = DateTime.Now;

            Lab_ShowNewSid = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ShowNewSid") == "是";
            Lab_NewSidCheckNullItrIDs = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NewSidCheckNullItrIDs");

            //是否独立显示取消检验和报告按钮
            if (UserInfo.GetSysConfigValue("lab_Undo_Report_button") == "是")
            {
                string word = LocalSetting.Current.Setting.ReportWord == string.Empty ? "报告" : LocalSetting.Current.Setting.ReportWord;
                sy.BtnUndo.Caption = "取消" + word;
                sy.BtnUndo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                string word1 = LocalSetting.Current.Setting.ReportWord == string.Empty ? "审核" : LocalSetting.Current.Setting.AuditWord;
                sy.BtnUndo2.Caption = "取消" + word1;
                sy.BtnUndo2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            sy.BtnDesign.Caption = "追加条码";
            string strFirstAudit = "BtnAudit";


            sy.BtnSelectTemplate.Caption = "临时报告";
            sy.BtnSelectTemplateClick += Sy_BtnSelectTemplateClick;

            string BtnQuickRegister = "";//门诊快速登记
            if (UserInfo.GetSysConfigValue("Lab_QuickRegister") == "是")//以后增加配置是否可见，门诊导入暂用于茂名妇幼
            {
                BtnQuickRegister = sy.BtnPageUp.Name;
                sy.BtnPageUp.Caption = "快速登记";
                sy.OnBtnPageUpClicked += SysToolBar1_OnBtnPageUpClicked;
            }

            string returnSamButName = this.Lab_DisplaySamReturnButton ? sy.btnCalculation.Name : string.Empty;
            sy.SetToolButtonStyle(new string[] {sy.BtnAdd.Name,
                                                sy.BtnSave.Name,
                                                sy.BtnDelete.Name,
                                                sy.BtnRefresh.Name,
                                                strFirstAudit,
                                                sy.BtnUndoAudit2.Name,
                                                 sy.BtnUndo2.Name,
                                                sy.BtnReport.Name,
                                                sy.BtnUndoReport2.Name,
                                                sy.BtnUndo.Name,
                                               BtnSinglePrint,
                                                //sy.BtnPageUp.Name,
                                                //sy.BtnPageDown.Name,
                                                sy.BtnResultView.Name,
                                                //btnDeRef,btnreturn,
                                                sy.btnAntibiotics.Name,
                                                sy.BtnPrint.Name,
                                                sy.BtnPrintPreview2.Name,
                                                sy.BtnDesign.Name,
                                                sy.BtnSelectTemplate.Name,
                                                BtnQuickRegister,
                                                sy.BtnExport.Name,
                                                sy.BtnCopy.Name,
                                                sy.BtnClose.Name});

            this.sy.OnBtnExportClicked += Sy_OnBtnExportClicked;

            sy.BtnCopy.Name = "资料复制";
            this.sy.BtnCopyClick += new System.EventHandler(this.sysToolBar1_BtnCopyClick);

            sy.btnCalculation.Caption = "标本回退";
            sy.btnBrowse.Caption = "病历浏览";

            if (UserInfo.GetSysConfigValue("Lab_ReportBrowse") != "是")
            {
                sy.btnBrowse.Visibility = BarItemVisibility.Never;
            }
            if (UserInfo.GetSysConfigValue("Lab_ShowFirstAuditButton") == "否")
            {
                sy.BtnAudit.Visibility = BarItemVisibility.Never;
                sy.BtnUndo2.Visibility = BarItemVisibility.Never;
            }

            string strExpName = UserInfo.GetSysConfigValue("BacLab_ExpName").Trim() == string.Empty ? "备注" : UserInfo.GetSysConfigValue("BacLab_ExpName");
            string strCommentName = UserInfo.GetSysConfigValue("BacLab_CommentName").Trim() == string.Empty ? "处理意见" : UserInfo.GetSysConfigValue("BacLab_CommentName");

            if (UserInfo.GetSysConfigValue("BacLab_Institute") == "是")
            {
                labelControl13.Text = "阴性结果";
                labelControl14.Text = "涂片结果";

                this.btnBacRemarks.Text = "选择\r\n备注范本";
                this.btnRemarks.Text = "选择";
                this.btnPatComment.Text = "选择";
                if (xtabExperiment.SelectedTabPageIndex == 1)
                    this.btnSelectNoBat.Visible = true;
            }
            else
            {
                labelControl13.Text = strExpName;
                labelControl14.Text = strCommentName;
                this.btnBacRemarks.Text = "选择备\r\n注范本";
                this.btnRemarks.Text = "选择\r\n" + strExpName;
                this.btnPatComment.Text = "选择\r\n" + strCommentName;
            }


            corUrgent = GetBarcodeConfigColor("New_Barcode_Color_Urgent");
            corBD = GetBarcodeConfigColor("New_Barcode_Color_BD");

            this.gvPatList.IndicatorWidth = 20;
            this.gvPatList.CustomDrawRowIndicator -= this.gridView1_CustomDrawRowIndicator;
            initScheml();
            userTypes = "";

            foreach (EntityUserLab dr in UserInfo.listUserLab)
            {
                userTypes += "'" + dr.LabId + "',";
            }
            if (userTypes != "")
            {
                userTypes = userTypes.TrimEnd(',');
                userTypes = " (" + userTypes + ") ";
            }

            string range = UserInfo.GetSysConfigValue("BacLab_DateRange");

            DateTime dtServer = ServerDateTime.GetServerDateTime();
            txtPatDate.DateTime = dtServer;
            if (!string.IsNullOrEmpty(range))
            {
                dtBegin.EditValue = dtServer.AddDays(-(Convert.ToInt32(range) - 1));
            }

            //默认检验人
            var drPat_chks = this.txtPatInspetor.dtSource.Find(a => a.UserLoginid == UserInfo.loginID);
            if (drPat_chks != null)
            {
                this.txtPatInspetor.selectRow = drPat_chks;
                this.txtPatInspetor.displayMember = UserInfo.userName;
                this.txtPatInspetor.valueMember = UserInfo.loginID;
            }
            //默认物理组
            var drPat_Types = this.txtPatType.dtSource.Find(a => a.ProId == UserInfo.defaultType);
            if (drPat_Types != null)
            {
                this.txtPatType.selectRow = drPat_Types;
                this.txtPatType.displayMember = drPat_Types.ProName;
                this.txtPatType.valueMember = UserInfo.defaultType;
            }
            string strDefaultItr = string.Empty;

            if (string.IsNullOrEmpty(LocalSetting.Current.Setting.LocalItrID))
                strDefaultItr = UserInfo.defaultItr;
            else
                strDefaultItr = LocalSetting.Current.Setting.LocalItrID;

            //默认仪器 
            var drPat_itrs = this.txtPatInstructment.dtSource.Find(a => a.ItrId == strDefaultItr);
            if (drPat_itrs != null && !string.IsNullOrEmpty(drPat_itrs.ItrId))
            {
                this.txtPatInstructment.selectRow = drPat_itrs;
                this.txtPatInstructment.displayMember = drPat_itrs.ItrEname;
                this.txtPatInstructment.valueMember = strDefaultItr;// UserInfo.defaultItr;
                ceCombine.ItrID = strDefaultItr;// UserInfo.defaultItr;
                txtItr1_onAfterChange(null);
                //默认标本
                this.getItrDefaultSample();
                if (!string.IsNullOrEmpty(drPat_itrs.ItrComId))
                {
                    ceCombine.AddCombine(drPat_itrs.ItrComId);
                }
            }
            else
            {
                List<EntityDicInstrument> itrList = CacheClient.GetCache<EntityDicInstrument>();
                var query = itrList.Where(w => w.ItrReportType == "3").ToList();
                txtPatInstructment.SetFilter(query);
            }

            BindLookupData();

            this.bsAnti.DataSource = AntiList;

            //角色权限：检验者录入许可
            if (!UserInfo.HaveFunction(243))
            {
                txtPatInspetor.Readonly = true;
            }

            btnCopy.Visible = UserInfo.GetSysConfigValue("ShowBacteriCopyBtn") == "是";

            //自定义审核显示语
            string auditWord = LocalSetting.Current.Setting.AuditWord;
            if (!string.IsNullOrEmpty(auditWord))
            {
                AuditWord = auditWord;
                ReplaceText(sy.BtnAudit, "审核", AuditWord);
                ReplaceText(sy.BtnUndoAudit, "审核", AuditWord);
                ReplaceText(sy.BtnReport, "报告", LocalSetting.Current.Setting.ReportWord);
                ReplaceText(sy.BtnUndoReport, "报告", LocalSetting.Current.Setting.ReportWord);
                labelControl591.Text = LocalSetting.Current.Setting.AuditWord + "者";
                labelControl601.Text = LocalSetting.Current.Setting.ReportWord + "者";
            }

            if (UserInfo.GetSysConfigValue("Lab_EableCustomeShortCut") == "是")
            { }

            //repositoryItemComboBox1 菌落计数下拉框
            //repositoryItemComboBox4 危急类型下拉框
            var dtBscripe = CacheClient.GetCache<EntityDicPubEvaluate>();

            string strInstrmt = string.Empty;
            if (UserInfo.GetSysConfigValue("BacLab_Institute") == "是")
            {
                List<EntityDicInstrument> ins = CacheClient.GetCache<EntityDicInstrument>();

                if (ins != null && ins.Count > 0)
                {
                    foreach (var dr in ins)
                    {
                        strInstrmt += "," + dr.ItrId;
                    }
                    if (!string.IsNullOrEmpty(strInstrmt))
                    {
                        strInstrmt = strInstrmt.Remove(0, 1);
                    }
                }
            }
            foreach (var drBscripe in dtBscripe)
            {
                if (drBscripe.EvaFlag == "4")
                {
                    if (UserInfo.GetSysConfigValue("BacLab_Institute") == "是")
                    {
                        string br_InstrmtInfo = drBscripe.EvaItrId;
                        string[] instrmtArray = br_InstrmtInfo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in instrmtArray)
                        {
                            if (strInstrmt.Contains(s))
                            {
                                repositoryItemComboBox1.Items.Add(drBscripe.EvaContent.Trim());
                                break;
                            }
                        }
                    }
                    else
                        repositoryItemComboBox1.Items.Add(drBscripe.EvaContent.Trim());
                }
                else if (drBscripe.EvaFlag == "7")
                {
                    if (UserInfo.GetSysConfigValue("BacLab_Institute") == "是")
                    {
                        string br_InstrmtInfo = drBscripe.EvaItrId;
                        string[] instrmtArray = br_InstrmtInfo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string s in instrmtArray)
                        {
                            if (strInstrmt.Contains(s))
                            {
                                repositoryItemComboBox1.Items.Add(drBscripe.EvaContent.Trim());
                                break;
                            }
                        }
                    }
                    else
                        repositoryItemComboBox4.Items.Add(drBscripe.EvaContent.Trim());
                }

            }

            LoadSystemConfig();

            Lab_NoBarcodeNeedAuditCheek = UserInfo.GetSysConfigValue("Lab_NoBarcodeNeedAuditCheek") == "是";
            if (Lab_NoBarcodeNeedAuditCheek)
                Lab_NoBarCodeAuditCheckItrExList = UserInfo.GetSysConfigValue("Lab_NoBarCodeAuditCheckItrExList");
            //初始化打印预览
            pForm = GetReportPrintFormInstance();


            if (cmbBarSearchPatType.Properties.DropDownRows > 2)
                cmbBarSearchPatType.Text = "样本号(模糊)";

            if (UserInfo.GetSysConfigValue("CmAntiVisible") == "是")
            {
                cmAnti.Items["tsmAddAnti"].Visible = false;
                cmAnti.Items["tsmAddAntiAll"].Visible = false;
            }

            sy.btnCalculation.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            sy.BtnDesignClick += (btnRecheck_Click);

            this.txtEditDate.EditValueChanged += new System.EventHandler(this.txtEditDate_EditValueChanged);
        }


        #endregion

        #region 初始化方法

        private void InitEvent()
        {
            roundPanelGroupBac.RoundPanelGroupClick += RoundPanelGroupBac_RoundPanelGroupClick;
            roundPanelResult.RoundPanelGroupClick += RoundPanelResult_RoundPanelGroupClick;

            this.dtBegin.EditValueChanged += new System.EventHandler(this.dtBegin_EditValueChanged);
            this.dtEnd.EditValueChanged += new System.EventHandler(this.dtEnd_EditValueChanged);

            this.txtPatType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.txtSelType1_ValueChanged);
            this.txtPatType.Load += new System.EventHandler(this.txtSelType1_Load);

            this.txtPatInstructment.Load += new System.EventHandler(this.txtItr1_Load);
            this.txtPatInstructment.onAfterChange += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.afterChange(this.txtItr1_onAfterChange);
            this.txtPatInstructment.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.txtItr1_ValueChanged);

            this.txtBarSearchCondition.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtBarSearchCondition_EditValueChanging);
            this.txtBarSearchCondition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarSearchCondition_KeyDown);

            this.linkHistory.Click += new System.EventHandler(this.linkHistory_Click);
            this.linkImage.Click += new System.EventHandler(this.linkImage_Click);
            this.linkInfo.Click += new System.EventHandler(this.linkInfo_Click);
            sbtnCheckObj.Click += SbtnCheckObj_Click;
                    
            this.xtabExperiment.SelectedPageChanged += XtabExperiment_SelectedPageChanged;
            this.radioGroup2.SelectedIndexChanged += new System.EventHandler(this.radioGroup2_SelectedIndexChanged);

            this.gcPatList.MouseDown += new MouseEventHandler(this.gridControl1_MouseDown);
            //this.gvPatList.CustomDrawColumnHeader += new ColumnHeaderCustomDrawEventHandler(this.gridView1_CustomDrawColumnHeader);
            this.gvPatList.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(this.gridView1_CustomDrawRowIndicator);
            this.gvPatList.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
            this.gvPatList.RowCellStyle += new RowCellStyleEventHandler(this.gridView1_RowCellStyle);
            this.gvPatList.RowStyle += new RowStyleEventHandler(this.gridView1_RowStyle);
            this.gvPatList.Click += new EventHandler(this.gridView1_Click);


            gcAnti.ProcessGridKey += new KeyEventHandler(gcAnti_ProcessGridKey);
            this.gvAnti.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView4_CustomDrawCell);
            this.gvAnti.Click += new System.EventHandler(this.gvAnti_Click);
            this.txtSimc.EditValueChanged += new System.EventHandler(this.txtSimc_EditValueChanged);
            this.repositoryItemLookUpEdit4.EditValueChanged += new System.EventHandler(this.repositoryItemLookUpEdit4_EditValueChanged);
            this.repositoryItemLookUpEdit6.EditValueChanged += new System.EventHandler(this.repositoryItemLookUpEdit4_EditValueChanged);
            this.repositoryItemLookUpEdit5.EditValueChanged += new System.EventHandler(this.repositoryItemLookUpEdit4_EditValueChanged);


            this.txtRes.EditValueChanged += new System.EventHandler(this.txtRes_Leave);
            this.txtRes.Enter += new System.EventHandler(this.txtRes_Enter);
            this.txtSimc.Enter += new System.EventHandler(this.txtSimc_Enter);
            this.repositoryItemComboBox3.EditValueChanged += new System.EventHandler(this.repositoryItemComboBox3_EditValueChanged);

            this.gvDesc.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView3_RowStyle);
            this.gvDesc.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridView3_CellValueChanged);

            this.repositoryItemComboBox2.EditValueChanged += new System.EventHandler(this.repositoryItemComboBox1_EditValueChanged);

            this.repositoryItemCheckEdit2.CheckedChanged += new System.EventHandler(this.repositoryItemCheckEdit2_CheckedChanged);

            this.gvBac.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridView2_RowCellStyle);
            this.gvBac.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView2_FocusedRowChanged);

            this.repositoryItemLookUpEdit1.EditValueChanged += new System.EventHandler(this.repositoryItemLookUpEdit1_EditValueChanged);
            this.repositoryItemLookUpEdit1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.repositoryItemLookUpEdit1_KeyUp);
            this.repositoryItemLookUpEdit1.Leave += new System.EventHandler(this.repositoryItemLookUpEdit1_Leave);

            this.repositoryItemLookUpEdit2.EditValueChanged += new System.EventHandler(this.repositoryItemLookUpEdit2_EditValueChanged);
            this.repositoryItemLookUpEdit2.Enter += new System.EventHandler(this.repositoryItemLookUpEdit2_Enter);
            this.repositoryItemLookUpEdit2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.repositoryItemLookUpEdit2_KeyUp);

            this.txtYingYang.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtYingYang_KeyDown);

            this.repositoryItemComboBox1.EditValueChanged += new System.EventHandler(this.repositoryItemComboBox1_EditValueChanged);
            this.repositoryItemComboBox1.DoubleClick += new System.EventHandler(this.repositoryItemComboBox1_DoubleClick);
            this.repositoryItemComboBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.repositoryItemTextEdit4_KeyUp);


            this.repositoryItemComboBox4.EditValueChanged += new System.EventHandler(this.repositoryItemComboBox1_EditValueChanged);
            this.repositoryItemTextEdit4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.repositoryItemTextEdit4_KeyUp);

            this.btnSelectNoBat.Click += new System.EventHandler(this.btnSelectNoBat_Click);
            this.btnHistoryEXP.Click += new System.EventHandler(this.btnHistoryEXP_Click);
            this.chk_pat_critical.CheckedChanged += new System.EventHandler(this.chk_pat_critical_CheckedChanged);
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);

            this.memoEdit1.EditValueChanged += new System.EventHandler(this.textEdit1_EditValueChanged);
            this.memoEdit1.DoubleClick += new System.EventHandler(this.memoEdit1_DoubleClick);

            this.btnBacRemarks.Click += new System.EventHandler(this.simpleButton3_Click);
            this.btnPatComment.Click += new System.EventHandler(this.simpleButton2_Click);
            this.btnRemarks.Click += new System.EventHandler(this.simpleButton1_Click);
        }


        /// <summary>
        /// 初始化涂片结果
        /// </summary>
        /// <param name="drIns"></param>
        private void InitNobact(EntityDicInstrument drIns)
        {
            if (drIns != null && smearList != null && smearList.Count > 0)
            {
                repositoryItemComboBox2.Items.Clear();
                foreach (var dr in smearList)
                {

                    if (drIns.ItrMicType.ToString() != dr.SmeClass.ToString())
                    {
                        continue;
                    }
                    string inCode = string.Empty;
                    if (!string.IsNullOrEmpty(dr.SmeCCode.ToString()))
                    {
                        inCode = dr.SmeCCode + "&";
                    }
                    repositoryItemComboBox2.Items.Add(inCode + dr.SmeName.ToString());
                }
            }
        }


        /// <summary>
        /// 初始化表结构
        /// </summary>
        private void initScheml()
        {


            bsPat.DataSource = new List<EntityPidReportMain>();
            //PatMiList = new List<EntityPidReportDetail>();

            this.ceCombine.listRepDetail = new List<EntityPidReportDetail>();

            if (this.ceCombine.listRepDetail != null)
            {
                PatientsMi_RowChanged(null, null);
            }

            BactList = new List<EntityObrResultBact>();
            AntiList = new List<EntityObrResultAnti>();

            smearList = CacheClient.GetCache<EntityDicMicSmear>();
            if (smearList.Count > 0)
            {
                repositoryItemComboBox2.Items.Clear();
                foreach (EntityDicMicSmear dr in smearList)
                {
                    string inCode = string.Empty;
                    if (!string.IsNullOrEmpty(dr.SmeCCode))
                    {
                        inCode = dr.SmeCCode + "&";
                    }
                    repositoryItemComboBox2.Items.Add(inCode + dr.SmeName);
                }
            }
        }


        /// <summary>
        /// 加载此界面所有需要用的参数
        /// </summary>
        private void LoadSystemConfig()
        {
            //加细菌危急值特定标本报告参数
            string ConfigCriticalSample = UserInfo.GetSysConfigValue("BacterialCriticalSample");
            string[] configCriticalSampleArr = ConfigCriticalSample.Split(';');
            foreach (string sample in configCriticalSampleArr)
            {
                ListConfigCriticalSample.Add(sample);
            }
        }
        private void defaultInspector()
        {
            var drPat_chks = this.txtPatInspetor.dtSource.Find(a => a.UserLoginid == UserInfo.loginID);
            if (drPat_chks != null)
            {
                this.txtPatInspetor.selectRow = drPat_chks;
                this.txtPatInspetor.displayMember = UserInfo.userName;
                this.txtPatInspetor.valueMember = UserInfo.loginID;
            }

        }


        /// <summary>
        /// 加载用户式样配置
        /// </summary>
        private void LoadUserSetting()
        {
            PatInputRuntimeSetting setting = PatInputRuntimeSetting.Load("FrmBacterialInput", string.Empty, UserInfo.loginID);
            ApplySetting(setting);

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
        /// 绑定数据
        /// </summary>
        private void BindLookupData()
        {
            for (int i = 0; i < 10; i++)
            {
                EntityObrResultDesc dr = new EntityObrResultDesc();
                dr.ObrPositiveFlag = 0;
                DescList.Add(dr);
            }
            this.gcDesc.DataSource = DescList;

            for (int j = 0; j < 6; j++)
            {
                BactList.Add(new EntityObrResultBact());
            }
            this.bs_rlts.DataSource = BactList;
            this.bsAntibio.DataSource = CacheClient.GetCache<EntityDicMicAntibio>();

            var btcope = CacheClient.GetCache<EntityDicMicBacttype>();
            this.bsBtype.DataSource = btcope;
            BacteriList = CacheClient.GetCache<EntityDicMicBacteria>();

            //按排序号排列，在按编号
            if (BacteriList != null && BacteriList.Count > 0)
                BacteriList = BacteriList.OrderBy(a => a.BacSortNo).OrderBy(a => a.BacId).ToList();

            this.bsbacteri.DataSource = BacteriList;
            this.bsbacteri1.DataSource = BacteriList;

            this.bsSex.DataSource = CommonValue.GetSex();
        }

        private String getMaxSID()
        {
            DateTime dtPatDate = this.txtPatDate.DateTime;
            string strInstructID = this.txtPatInstructment.valueMember;
            if (strInstructID != null && strInstructID.Trim(null) != string.Empty)
            {
                string sid = new ProxyPidReportMain().Service.GetItrSID_MaxPlusOne(dtPatDate, strInstructID, true);
                return sid;
            }
            return "1";
        }

        #endregion

        #region 病人列表相关操作

        protected virtual void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = this.gvPatList.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gvPatList.CalcHitInfo(pt);

            //判断是否有权修改检验报告管理信息
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManager") == "是")
            {
                if (!isCanModify)
                {
                    if (labelControl2.Text.Trim() == "编辑")
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
        }


        private void gridView1_Click(object sender, EventArgs e)
        {
            if (bsPatLst.Current != null)
            {
                if (this.labelControl2.Text == "新增")
                {
                    this.labelControl2.Text = "编辑";
                    if (bsPatLst.Current != null)
                    {
                        EntityPidReportMain drLst = (EntityPidReportMain)bsPatLst.Current;
                        CurrentPatID = drLst.RepId;
                        this.GetPatDetailData(CurrentPatID);
                        this.FillUiFromEntity();
                        isDataChaged = false;
                    }

                    //*******************************************************************
                    //判断是否有权修改检验报告管理信息
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManager") == "是")
                    {
                        if (!isCanModify)
                        {
                            if (labelControl2.Text.Trim() == "编辑")
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
                }
            }

        }


        /// <summary>
        /// 改变旗子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            Rectangle r = e.Bounds;

            if (e.Column.Name == "col_icon")
            {
                var dr = this.gvPatList.GetRow(e.RowHandle) as EntityPidReportMain;

                //有无菌涂片结果
                if (dr.HasResult == "1" && dr.HasResult2 == 0)
                {
                    int x = r.X + 2;
                    int y = r.Y + (r.Height - imageList1.ImageSize.Height) / 2;
                    e.Graphics.DrawImageUnscaled(imageList1.Images[1], x, y);
                }
                else if (dr.HasResult == "1" && dr.HasResult2 == 1)
                {
                    //有药敏细菌信息,但是没有药敏结果
                    int x = r.X + 2;
                    int y = r.Y + (r.Height - imageList1.ImageSize.Height) / 2;
                    e.Graphics.DrawImageUnscaled(imageList1.Images[2], x, y);
                }
                else if (dr.HasResult == "1" && dr.HasResult2 == 2)
                {
                    //有药敏细菌信息,并且有药敏结果
                    int x = r.X + 2;
                    int y = r.Y + (r.Height - imageList1.ImageSize.Height) / 2;
                    e.Graphics.DrawImageUnscaled(imageList1.Images[1], x, y);
                }
                e.Handled = true;
            }
        }


        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView grid = sender as GridView;
            var row = grid.GetRow(e.RowHandle) as EntityPidReportMain;
            if (row == null)
                return;

            if (e.Column.FieldName == "PidName")
            {
                if (row.RepUrgentFlag != null && row.RepUrgentFlag != 0)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 187, 255);
                }
                else if (row.RepCtype == "2")
                {
                    e.Appearance.BackColor = Color.FromArgb(64, 224, 208);
                }
            }


            #region 颜色区分门诊住院
            if (UserInfo.GetSysConfigValue("Lab_ColorWithInOutPatient") == "是")
            {
                if (e.Column.FieldName == "RepSid")
                {
                    string pat_ori_id = row.PidSrcId.ToString();
                    string pat_bed_no = row.PidBedNo.ToString().Trim();

                    if (pat_ori_id == "107")//门诊(可以与下面的代码合并到一起)
                    {
                        e.Appearance.BackColor = Color.LightGreen;
                    }
                    else if (pat_ori_id == "108")//住院
                    {
                        e.Appearance.BackColor = Color.Moccasin;

                    }
                    else
                    {
                    }
                }
            }
            #endregion


            //危急值显示颜色
            if (e.Column.FieldName == "PidName")
            {
                if (row.RepUrgentFlag.ToString() == "1")//未查看危急值
                {
                    if (UserCustomSetting.PatListPanel.BackColorNUrgent == Color.Empty)
                        e.Appearance.BackColor = Color.FromArgb(255, 0, 255);
                    else
                        e.Appearance.BackColor = UserCustomSetting.PatListPanel.BackColorNUrgent;
                }
                else if (row.RepUrgentFlag.ToString() == "2")//已查看危急值
                {
                    if (UserCustomSetting.PatListPanel.BackColorUrgent == Color.Empty)
                        e.Appearance.BackColor = Color.FromArgb(255, 0, 255);
                    else
                        e.Appearance.BackColor = UserCustomSetting.PatListPanel.BackColorUrgent;
                }
                else
                {
                    e.Appearance.BackColor = Color.Moccasin;
                }
            }
        }


        /// <summary>
        /// 更新记录数量信息
        /// </summary>
        public void RefreshItemsCount()
        {
            int countUnAudited = 0;
            int countAudited = 0;
            int countReported = 0;
            int countPrinted = 0;
            int countTotal = 0;

            if (this.bsPatLst.DataSource != null)
            {
                List<EntityPidReportMain> dtpat = this.bsPatLst.DataSource as List<EntityPidReportMain>;
                if (dtpat != null)
                {
                    countTotal = dtpat.Count;

                    foreach (var item in dtpat)
                    {
                        if (item.RepStatus != null)
                        {
                            if (item.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Audited
                                || item.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed
                                || item.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported
                                )
                            {
                                countAudited++;

                                if (item.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed
                                    || item.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                                {
                                    countReported++;
                                }

                                if (item.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed)
                                {
                                    countPrinted++;
                                }
                            }
                            else
                            {
                                countUnAudited++;
                            }
                        }
                    }
                }
            }
            this.lbRecordCount.Text = string.Format("总数：{0} 已{4}：{1} 未{4}：{2} 已{5}：{3}", countTotal, countAudited, countUnAudited, countReported, LocalSetting.Current.Setting.AuditWord, LocalSetting.Current.Setting.ReportWord);
        }


        void PatientsMi_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            //通过组合过滤
            if (BacLab_DictNobactByComID)
            {
                bool isSelByComIDs = false;//是否通过组合过滤
                List<string> tempNobIDs = new List<string>();
                string strComIDs = "";
                if (ceCombine.listRepDetail != null && ceCombine.listRepDetail.Count > 0)
                {
                    foreach (var item in this.ceCombine.listRepDetail)
                    {
                        if (string.IsNullOrEmpty(strComIDs))
                        {
                            strComIDs = item.ComId;
                        }
                        else
                        {
                            strComIDs += "," + item.ComId;
                        }
                    }
                }
                //如果当前有组合信息,则过滤组合包含的无菌信息
                if (!string.IsNullOrEmpty(strComIDs))
                {
                    isSelByComIDs = true;//标记要过滤
                    var dtTempDictNobact = new ProxyMicEnter().Service.GetDicMicSmearByComID(strComIDs);
                    if (dtTempDictNobact != null && dtTempDictNobact.Count > 0)
                    {
                        for (int i = 0; i < dtTempDictNobact.Count; i++)
                        {
                            tempNobIDs.Add(dtTempDictNobact[i].SmeId);
                        }
                    }
                }
                repositoryItemComboBox2.Items.Clear();
                foreach (var dr in smearList)
                {
                    if (isSelByComIDs)
                    {
                        //如果不包含，不显示此无菌信息
                        if (!tempNobIDs.Contains(dr.SmeId))
                        {
                            continue;
                        }
                    }

                    string inCode = string.Empty;
                    if (!string.IsNullOrEmpty(dr.SmeCCode))
                    {
                        inCode = dr.SmeCCode + "&";
                    }
                    repositoryItemComboBox2.Items.Add(inCode + dr.SmeName);
                }
            }
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle == this.gvPatList.FocusedRowHandle)
            {
                e.Appearance.BackColor = Color.Lavender;
                if (Lab_PatientsFocusColor == "是")
                {
                    e.Appearance.BackColor = Color.LightSalmon;
                }
            }
            var row = this.gvPatList.GetRow(e.RowHandle) as EntityPidReportMain;
            if (row != null)
            {
                if (row.RepCtype == "2")
                {
                    e.Appearance.BackColor = Color.FromArgb(64, 224, 208); ;
                }
                if (row.RepCtype == "4")
                {
                    e.Appearance.BackColor = corBD;
                }
            }
        }



        #endregion


        ProxyPidReportMainAudit proxy = new ProxyPidReportMainAudit();

        void gcAnti_ProcessGridKey(object sender, KeyEventArgs e)
        {
            if (this.gvAnti.FocusedRowHandle >= 0)
            {
                if (e.KeyCode == Keys.Right)
                {
                    GridColumn focusColumn = this.gvAnti.FocusedColumn;
                    if (focusColumn == this.gridColumn9)//药敏性
                    {

                        this.gvAnti.FocusedColumn = this.gridColumn12;
                        this.gvAnti.FocusedRowHandle = this.gvAnti.FocusedRowHandle;
                        e.SuppressKeyPress = true;
                    }

                }
                else if (e.KeyCode == Keys.Left)
                {
                    GridColumn focusColumn = this.gvAnti.FocusedColumn;
                    if (focusColumn == this.gridColumn12)//Mic
                    {
                        this.gvAnti.FocusedColumn = this.gridColumn9;
                        this.gvAnti.FocusedRowHandle = this.gvAnti.FocusedRowHandle;

                        e.SuppressKeyPress = true;
                    }

                }
            }
        }

        void repositoryItemComboBox2_FormatEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        {
            if (e.Value != null)
            {
                string[] array = e.Value.ToString().Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length == 2)
                {
                    e.Value = array[1];
                    strBsrCname = array[1];
                }
            }
        }

        //鉴定类别改变不同界面
        private void XtabExperiment_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtabExperiment.SelectedTabPageIndex == 0)
            {
                btnSelectNoBat.Visible = false;
                this.radioGroup2.Enabled = true;
                this.memoEdit1.Enabled = true;
                this.btnBacRemarks.Enabled = true;
                panelControl6.Visible = true;

            }
            if (xtabExperiment.SelectedTabPageIndex == 1)
            {
                btnSelectNoBat.Visible = true;
                this.radioGroup2.Enabled = false;
                this.memoEdit1.Enabled = false;
                this.btnBacRemarks.Enabled = false;
                panelControl6.Visible = false;
            }
        }


        /// <summary>
        /// 获取仪器下一个样本号
        /// </summary>
        /// <returns></returns>
        protected string GetMaxCxhNewSID()
        {
            DateTime dtPatDate = this.txtPatDate.DateTime;
            string strInstructID = this.txtPatInstructment.valueMember;
            if (strInstructID != null && strInstructID.Trim(null) != string.Empty && Lab_ShowNewSid)
            {
            }

            return string.Empty;
        }


        /// <summary>
        /// 根据所选菌株绑定相对应的药敏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            if (this.gvBac.FocusedRowHandle > -1 && this.gvBac.EditingValue != null)
            {
                string strBacId = txtBac.Text.ToString().Trim();
                string bac_id = this.gvBac.EditingValue.ToString();
                if (strBacId != "")
                {
                    List<EntityDicMicBacteria> dicBacList = bsbacteri.DataSource as List<EntityDicMicBacteria>;

                    if (dicBacList == null)
                    {
                        MessageDialog.Show("细菌字典绑定出错!");
                        return;
                    }
                    var bac1 = dicBacList.Find(a => a.BacId == strBacId);
                    var bac2 = dicBacList.Find(a => a.BacId == bac_id);

                    EntityObrResultBact dvr = (EntityObrResultBact)(gvBac.GetFocusedRow());
                    dvr.BtypeId = bac2?.BacBtId;
                    gvBac.SetFocusedRowCellValue(gvBac.VisibleColumns[gvBac.FocusedColumn.VisibleIndex - 1], bac2?.BacBtId);

                    List <EntityObrResultAnti> dt_Bac = (List<EntityObrResultAnti>)this.bsAnti.DataSource;
                    if (bac1 != null && bac2 != null && bac1.BacBtId == bac2.BacBtId && dt_Bac != null && dt_Bac.Count > 0)
                    {
                        if (dt_Bac != null)
                        {
                            var dr_Bac = dt_Bac.FindAll(a => a.ObrBacId == strBacId);//("bt_id='" + strBacId + "'");
                            for (int i = 0; i < dr_Bac.Count; i++)
                            {
                                foreach (var dtRow in dt_Bac)
                                {
                                    if (dtRow.ObrBacId == strBacId)
                                    {
                                        dtRow.ObrBacId = bac_id;
                                        break;
                                    }
                                }
                            }
                        }
                        gvAnti.ExpandAllGroups();
                        this.txtBac.Text = bac_id;
                        return;
                    }


                    removeAnti(txtBac.Text.ToString());//抗生素删除
                }

                EntityObrResultBact dr = this.gvBac.GetFocusedRow() as EntityObrResultBact;
                if (dr != null && this.gvBac.EditingValue != null)
                {
                    this.txtBac.Text = bac_id;
                    if (bac_id != "")
                    {
                        var anDetailList = new ProxyMicEnter().Service.GetMicAntidetailList(bac_id);
                        if (anDetailList.Count > 0)
                        {
                            dr.BtypeId = anDetailList[0].BtypeId;
                            dr.BacCname = bac_id;
                        }


                        List<EntityObrResultAnti> Add_ti = new List<EntityObrResultAnti>();
                        foreach (var dRow in anDetailList)
                        {
                            EntityObrResultAnti newRow = new EntityObrResultAnti();
                            newRow.ObrBacId = bac_id;
                            newRow.AntId = dRow.AntId;
                            newRow.ObrAntId = dRow.AntId;
                            newRow.ObrAtypeId = dRow.AnsDefId;
                            newRow.SsHstd = dRow.AnsStdUpperLimit;
                            newRow.SsMstd = dRow.AnsStdMiddleLimit;
                            newRow.SsLstd = dRow.AnsStdLowerLimit;
                            newRow.SsRzone = dRow.AnsZoneDurgfast;
                            newRow.SsIzone = dRow.AnsZoneIntermed;
                            newRow.SsSzone = dRow.AnsZoneSensitive;
                            newRow.Sszone = dRow.Sszone;
                            newRow.Ssaen = dRow.AnsAntiShortName;
                            newRow.ObrRef = radioGroup2.SelectedIndex == 0 ? "MIC" : "KB";
                            Add_ti.Add(newRow);
                        }
                        setDt_Anti(Add_ti, false, bac_id);
                    }
                }
                this.gvAnti.ExpandAllGroups();

            }
            gvBac.RefreshData();
        }

        /// <summary>
        /// 删除抗生素
        /// </summary>
        /// <param name="bacId">菌株ID</param>
        private void removeAnti(string bacId)
        {
            List<EntityObrResultAnti> dt_Bac = (List<EntityObrResultAnti>)this.bsAnti.DataSource;
            if (dt_Bac == null) return;
            var dr_Bac = dt_Bac.FindAll(a => a.ObrBacId == bacId);//.Select("bt_id='" + bacId + "'");
            for (int i = 0; i < dr_Bac.Count; i++)
            {
                foreach (var dtRow in dt_Bac)
                {
                    if (dtRow.ObrBacId == bacId)
                    {
                        dt_Bac.Remove(dtRow);
                        break;
                    }
                }
            }
            gvAnti.RefreshData();
        }

        /// <summary>
        /// 根据菌类的改变，改变菌株下拉列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            gvBac.SetFocusedRowCellValue(gvBac.VisibleColumns[gvBac.FocusedColumn.VisibleIndex + 1], "");//清空细菌
            if (this.txtBac.Text.ToString().Trim() != "")
            {
                removeAnti(txtBac.Text.ToString());//抗生素删除
            }
        }

        #region 改变焦点
        private void repositoryItemTextEdit4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.gvAnti.Focus();
                ColumnView View = (ColumnView)gcAnti.FocusedView;
                GridColumn column = View.Columns["ObrValue"];
                if (column != null)
                {
                    View.FocusedColumn = column;
                }
            }
        }

        private void repositoryItemLookUpEdit2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ColumnView View = (ColumnView)gcBac.FocusedView;
                GridColumn column = new GridColumn();
                if (colbt_scripe.Visible)
                    column = View.Columns["ObrRemark"];
                else
                    column = View.Columns["ObrColonyCount"];
                if (column != null)
                {
                    View.FocusedColumn = column;
                }
            }
        }

        private void repositoryItemLookUpEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ColumnView View = (ColumnView)gcBac.FocusedView;
                GridColumn column = View.Columns["BacCname"];
                if (column != null)
                {
                    View.FocusedColumn = column;
                }
            }
        }

        private void radioGroup2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!radioGroupEventEnable) return;
            gvAnti.CloseEditor();
            foreach (var dr in AntiList)
            {
                if (!string.IsNullOrEmpty(txtBac.Text)
                    && !string.IsNullOrEmpty(dr.ObrBacId)
                    && dr.ObrBacId != txtBac.Text)
                    continue;
                dr.Sszone = radioGroup2.SelectedIndex == 0 ? dr.SsMstd : dr.SsIzone;
                dr.ObrRef = radioGroup2.SelectedIndex == 0 ? "MIC" : "KB";
            }
            gcAnti.RefreshDataSource();
        }

        #endregion

        //药敏输入快速更换
        //药敏输入快速更换
        private void txtRes_Leave(object sender, EventArgs e)
        {
            if (this.gvAnti.EditingValue != null)
            {
                bool blnIsAutoFull = false;//是否自动填充结果
                //系统配置：输入抗生素的药敏性时不自动填充结果
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Bacterial_MicNoAutoFullRes") != "是")
                {
                    blnIsAutoFull = true;
                }

                bool micType = UserInfo.GetSysConfigValue("Bacterial_mic_type") == "敏感、中介、耐药";
                EntityObrResultAnti dr = this.gvAnti.GetFocusedRow() as EntityObrResultAnti;

                if (dr == null) return;

                if (this.gvAnti.EditingValue.ToString() == "1")
                {
                    if (micType)
                        this.gvAnti.EditingValue = "耐药";
                    else
                        this.gvAnti.EditingValue = "R";

                    if (string.IsNullOrEmpty(dr.ObrValue2))
                    {
                        if (blnIsAutoFull)//是否自动填充结果
                        {
                            if (dr.ObrRef == "MIC")
                                dr.ObrValue2 = dr.SsLstd;
                            else
                                dr.ObrValue2 = dr.SsRzone;
                        }
                    }


                }
                else if (this.gvAnti.EditingValue.ToString() == "2")
                {
                    if (micType)
                        this.gvAnti.EditingValue = "中介";
                    else
                        this.gvAnti.EditingValue = "I";
                    if (string.IsNullOrEmpty(dr.ObrValue2))
                    {
                        if (blnIsAutoFull)//是否自动填充结果
                        {
                            if (dr.ObrRef == "MIC")
                                dr.ObrValue2 = dr.SsMstd;
                            else
                                dr.ObrValue2 = dr.SsIzone;
                        }
                    }
                }
                else if (this.gvAnti.EditingValue.ToString() == "3")
                {
                    if (micType)
                        this.gvAnti.EditingValue = "敏感";
                    else
                        this.gvAnti.EditingValue = "S";
                    if (string.IsNullOrEmpty(dr.ObrValue2))
                    {
                        if (blnIsAutoFull)//是否自动填充结果
                        {
                            if (dr.ObrRef == "MIC")
                                dr.ObrValue2 = dr.SsHstd;
                            else
                                dr.ObrValue2 = dr.SsSzone;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// MIC改变判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSimc_Leave(object sender, EventArgs e)
        {
            if (strMic != string.Empty && drMic != null)
            {
                var dr = drMic;

                if (strMic == "阳性" || strMic == "+")
                {
                    dr.ObrValue = "阳性";
                    return;
                }
                if (strMic == "阴性" || strMic == "-")
                {
                    dr.ObrValue = "阴性";
                    return;
                }
                double count = 0;
                try
                {
                    count = Convert.ToDouble(strMic);
                }
                catch (Exception)
                {
                    disposal(strMic);
                    return;
                }
                if (count >= 0)
                {
                    bool isTrue = false;
                    string micl = "";
                    string micm = "";
                    string mich = "";
                    if (dr.ObrRef == "MIC")
                    {
                        micl = dr.SsLstd;
                        micm = dr.SsMstd;
                        mich = dr.SsHstd;
                    }
                    else
                    {
                        micl = dr.SsRzone;
                        micm = dr.SsIzone;
                        mich = dr.SsSzone;
                    }

                    int x = count.ToString().Length - 1;

                    bool micType = UserInfo.GetSysConfigValue("Bacterial_mic_type") == "敏感、中介、耐药";
                    if (micl != "")
                    {
                        isTrue = GetStd(micl, count);
                        if (isTrue)
                        {
                            if (micType)
                                dr.ObrValue = "耐药";
                            else
                                dr.ObrValue = "R";



                            return;
                        }
                    }
                    if (micm != "" && isTrue == false)
                    {
                        isTrue = GetStd(micm, count);
                        if (isTrue)
                        {
                            if (micType)
                                dr.ObrValue = "中介";
                            else
                                dr.ObrValue = "I";
                            return;
                        }
                    }
                    if (mich != "" && isTrue == false)
                    {
                        isTrue = GetStd(mich, count);
                        if (isTrue)
                        {
                            if (micType)
                                dr.ObrValue = "敏感";
                            else
                                dr.ObrValue = "S";

                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 处理不合法数据
        /// </summary>
        /// <param name="tem"></param>
        private void disposal(string tem)
        {
            for (int i = 0; i <= 9; i++)
            {
                tem = tem.Replace(i.ToString(), "");
            }
            tem = tem.Trim();
            switch (tem)
            {
                case ">=":
                    break;
                case ">":
                    break;
                case "<=":
                    break;
                case "<":
                    break;
                case "-":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 根据时间仪器获得列表
        /// </summary>
        /// <param name="isTrue"></param>
        private void GetPatientList(bool isTrue)
        {
            if (txtPatInstructment.displayMember == null || txtPatInstructment.displayMember == "")
            {
                return;
            }
            EntityPatientQC qc = new EntityPatientQC();
            qc.DateStart = dtBegin.DateTime.Date;
            qc.DateEnd = dtEnd.DateTime.Date.AddDays(1).AddSeconds(-1);
            qc.ListItrId = new List<string> { txtPatInstructment.valueMember };

            string strPatFlag = cbeFlag.EditValue.ToString();
            if (strPatFlag == "1")//未报告
            {
                qc.RepStatus = "1";
            }
            else if (strPatFlag == "2")//未审核
            {
                qc.RepStatus = "0";
            }
            else if (strPatFlag == "3")//已报告
            {
                qc.RepStatus = "2";
            }
            else if (strPatFlag == "4")//未打印
            {
                qc.RepStatus = "2";
            }
            else if (strPatFlag == "6")//危急值
            {
                qc.RepUrgent = true;
            }
            var list = new ProxyMicEnter().Service.MicPatientQuery(qc);

            AllPatList = list;

            List<EntityPidReportMain> dtOldPat = bsPatLst.DataSource as List<EntityPidReportMain>;

            bsPatLst.ResetBindings(false);

            //if (clearCheckType)
            //{
            //    foreach (var item in list)
            //    {
            //        if (dtOldPat != null && dtOldPat.Count > 0)
            //        {
            //            if (dtOldPat.Any(a => a.PatSelect && a.RepId == item.RepId))
            //                item.PatSelect = true;
            //        }
            //    }
            //}

            if (UserInfo.GetSysConfigValue("BacteriAuditFilterNoResults") == "是")
            {
                if (cbeFlag.EditValue.ToString() == "未报告")
                {
                    List<EntityPidReportMain> dtTemp = new List<EntityPidReportMain>();
                    var drTemp = list.FindAll(a => a.HasResult != "0");
                    foreach (var rows in drTemp)
                    {
                        dtTemp.Add(rows);
                    }
                    bsPatLst.DataSource = dtTemp;
                }
                else
                {
                    bsPatLst.DataSource = list;
                }
            }
            else
            {
                bsPatLst.DataSource = list;
            }

            RefreshItemsCount();//更新记录
            this.fpat_id = "";
            RapitSearch();
            isLoadData = true;
        }
        private String fpat_id = "";//主键
        private String oldPat_sid = "";


        private void SearchPatientsAndAddNew()
        {
            GetPatientList(false);
            var drItr = txtPatInstructment.selectRow;
            if (drItr != null)
            {
                if (!string.IsNullOrEmpty(drItr.ItrReportId) && drItr.ItrReportId.IndexOf("bacilli") == 0)
                {
                    colbt_scripe.Visible = false;
                }
                else
                {
                    colbt_scripe.Visible = true;
                    colbt_scripe.VisibleIndex = 2;
                }
            }
            if (ceCombine.listRepDetail != null)
                ceCombine.listRepDetail.Clear();

            addNew();
            getItrDefaultSample();


            this.txtPatSid.Text = getMaxSID();
            txtNewSid.EditValue = GetMaxCxhNewSID();
        }

        //样本号失去交点时
        private void txtSid_Leave(object sender, EventArgs e)
        {
            if (!isDataChaged)
                return;

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

            if (this.txtPatSid.Text != this.fpat_sid_old.Text)
            {
                if (this.fpat_id == "")
                { //新增状态
                    this.LocatePatients();
                }
                else if (IsBacLab_sampleLocatePatient)//细菌报告界面样本号自动定位病人资料
                {
                    DialogResult diaRv = lis.client.control.MessageDialog.Show("是否跳转样本号？否则修改样本号", "提示",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                    if (diaRv == DialogResult.Yes)
                    {
                        if (this.txtPatDate.EditValue == null || txtPatInstructment.displayMember == null || txtPatInstructment.displayMember == "")
                            return;

                        //根据当前仪器和样本号、年份获取满足条件的日期
                        string temp_Set_Strdate = new ProxyMicEnter().Service.GetPatDate_ByItrSID(this.txtPatDate.DateTime, txtPatInstructment.valueMember, this.txtPatSid.Text);

                        DateTime temp_Set_date = DateTime.Now;

                        if ((!string.IsNullOrEmpty(temp_Set_Strdate)) && DateTime.TryParse(temp_Set_Strdate, out temp_Set_date))
                        {
                            txtPatDate.EditValue = temp_Set_date;
                            GetPatientList(false);//刷新病人列表
                            ordate = this.txtPatDate.Text.ToString();

                        }

                        this.LocatePatients();
                    }
                    else if (diaRv == DialogResult.No)
                    {
                        oldPat_sid = this.txtPatSid.Text;
                    }
                    else
                    {
                        this.txtPatSid.Text = this.fpat_sid_old.Text;
                    }

                }
                else
                {
                    if (lis.client.control.MessageDialog.Show("是否修改样本号？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        oldPat_sid = this.txtPatSid.Text;
                    }
                    else
                    {
                        this.txtPatSid.Text = this.fpat_sid_old.Text;
                    }
                }
            }
        }
        public bool IsCharDBC(char c)
        {
            if (c > 65280 && c < 65375)
                return true;
            else
                return false;
        }

        //定位
        private void LocatePatients()
        {
            if (this.txtPatDate.Text == "" || this.txtPatInstructment.valueMember == ""
        || this.txtPatInstructment.displayMember == null || (this.fpat_id != "" && (!IsBacLab_sampleLocatePatient))) return;//新增状态下才是定位
            bool isFound = false;
            int oldPos = bsPatLst.Position;
            string patSid = this.txtPatSid.Text.Trim();
            bsPatLst.PositionChanged -= this.bsPatLst_PositionChanged;
            int focusedRowHandle = gvPatList.FocusedRowHandle;
            int rowHand = -1;
            try
            {
                bsPatLst.MoveFirst();
                for (int i = 0; i < bsPatLst.Count; i++)
                {
                    rowHand = -1;
                    EntityPidReportMain dr = (EntityPidReportMain)bsPatLst.Current;
                    if (dr.RepSid == patSid)
                    { //在列表中找到了
                        this.GetPatDetailData(dr.RepId);
                        this.FillUiFromEntity();

                        oldPat_sid = this.txtPatSid.Text;
                        this.fpat_sid_old.Text = this.txtPatSid.Text;
                        this.labelControl2.Text = "编辑";
                        rowHand = i;
                        isFound = true;
                        break;
                    }
                    gvPatList.FocusedRowHandle = focusedRowHandle + 1;
                }
            }
            finally
            {
                bsPatLst.PositionChanged += this.bsPatLst_PositionChanged;
                this.gvPatList.ClearSelection();
                this.gvPatList.SelectRow(rowHand);
            }
            if (!isFound)
            {
                addNew();
                this.txtPatSid.Text = patSid;
                if (txtPatInstructment.valueMember != null && txtPatInstructment.valueMember.Trim() != "" && txtPatDate.EditValue != null && txtPatSid.Text.Trim() != "")
                {
                    string pat_id = txtPatInstructment.valueMember + String.Format("{0:yyyyMMdd}", txtPatDate.EditValue)
                       + txtPatSid.Text.Trim();
                    GetPatDetailData(pat_id);
                    setResulto();
                }
            }
        }

        //给控件赋值
        private void FillUiFromEntity()
        {
            isDataChaged = false;

            if (CurrentPatInfo == null || string.IsNullOrEmpty(CurrentPatInfo.RepId))
                return;

            var dr = CurrentPatInfo;
            PatInfo = null;

            txtPatDate.EditValue = dr.RepInDate;

            fpat_id = dr.RepId;
            CurrentPatID = dr.RepId;
            txtPatSid.Text = dr.RepSid;
            txtPatName.Text = dr.PidName;

            bAllowFirePatDept_ValueChanged = false;
            txtPatDeptId.displayMember = dr.PidDeptName;
            txtPatDeptId.valueMember = dr.PidDeptId;
            bAllowFirePatDept_ValueChanged = true;

            this.txt_pat_ward_id.Text = dr.PidWardId;//病区ID
            this.txt_pat_ward_name.Text = dr.PidWardName;//病区名称
            this.txt_pat_emp_id.Text = dr.PidExamNo;//体检ID

            //就诊次数
            this.txtPatAdmissTime.Text = dr.PidAddmissTimes.ToString();

            this.txtPatSocialNo.Text = dr.PidSocialNo;//social_no
            this.txtPatEmpCompanyName.Text = dr.PidExamCompany;

            this.txtPatAppNo.Text = dr.PidApplyNo;


            txtPatBedNo.Text = dr.PidBedNo;
            txtPatSex.valueMember = dr.PidSex;
            txtPatSex.displayMember = dr.PidSexName;
            txtPatBarCode.Text = dr.RepBarCode;

            this.textAgeInput1.AgeValueText = dr.PidAgeExp;

            txtPatID.Text = dr.PidInNo;
            txtPatPid.Text = dr.RepInputId;


            txtPatUpid.Text = dr.PidUniqueId;
            txtPatUpid2.Text = dr.PidUniqueId;
            txtNewSid.Text = dr.PidUniqueId;

            this.txtPatNotice.Text = string.Empty;

            //序号
            txt_pat_host_order.Text = dr.RepSerialNum;

            txtPatSampleType.displayMember = dr.SamName;
            txtPatSampleType.valueMember = dr.PidSamId;

            txtPatDoc.displayMember = dr.PidDocName;
            txtPatDoc.valueMember = dr.PidDoctorCode;

            if (!string.IsNullOrEmpty(this.txtPatDoc.valueMember)
                && string.IsNullOrEmpty(this.txtPatDoc.displayMember))
            {
                this.txtPatDoc.SelectByID(this.txtPatDoc.valueMember);
            }

            txtPatIdType.displayMember = dr.IdtName;
            txtPatIdType.valueMember = dr.PidIdtId;
            txtPatDiag.displayMember = dr.PidDiag;
            txtPatDiag.valueMember = dr.PidDiag;
            txtPatSDate.EditValue = dr.SampSendDate;
            txtPatReachDate.EditValue = dr.RepReadDate;
            txtFlag.Text = dr.RepStatus == null ? "0" : dr.RepStatus.ToString();
            string cheid = dr.RepCheckUserId;
            fpat_sid_old.Text = dr.RepSid;
            fpat_work.Text = dr.PidWork;
            fpat_tel.Text = dr.PidTel;
            fpat_email.Text = dr.PidEmail;
            fpat_address.Text = dr.PidAddress;
            fpat_height.Text = dr.PidHeight;
            fpat_weight.Text = dr.PidWeight;
            fpat_pre_week.Text = dr.PidPreWeek;
            fpat_sample_part.Text = dr.CollectionPart;
            fpat_exp2.Text = dr.RepRemark;
            fpat_comment2.Text = dr.RepComment;
            luePatCheckType.valueMember = dr.RepCtype;
            txtPatSampleDate.EditValue = dr.SampCollectionDate;
            txtPatApplyDate.EditValue = dr.SampApplyDate;
            txtPatSamRem.displayMember = dr.SampRemark;
            txtPatReceiveDate.EditValue = dr.SampReceiveDate;

            //出生日期
            this.txtBirthday.EditValue = dr.PidBirthday;

            txtPatRecDate.EditValue = dr.SampCheckDate;
            txtPatSampleState.displayMember = txtPatSampleState.valueMember = dr.PidRemark;
            fpat_unit.EditValue = dr.PidUnit;
            if (dr.PidInsuId != null && dr.PidInsuId != "")
            {
                txtPatFeeType.displayMember = dr.PidInsuId;
                txtPatFeeType.valueMember = dr.PidInsuId;
            }
            else
            {
                txtPatFeeType.displayMember = txtPatFeeType.valueMember = "";
                txtPatFeeType.valueMember = txtPatFeeType.valueMember = "";
            }

            this.txtPatSource.SelectByID(dr.PidSrcId);

            if (dr.RepCtype != null && dr.RepCtype != "")
                luePatCheckType.displayMember = CommonValue.GetPatCtype().Select("id='" + dr.RepCtype + "'")[0]["value"].ToString();
            else
                luePatCheckType.displayMember = "";
            if (dr.PidPurpId != null && dr.PidPurpId != "")
            {
                txtPat_chk_purpose.SelectByID(dr.PidPurpId);
            }
            else
            {
                txtPat_chk_purpose.displayMember = "";
                txtPat_chk_purpose.valueMember = "";
            }

            if (dr.RepCheckUserId != null && dr.RepCheckUserId != "")
            {
                var dre = txtPatInspetor.dtSource.Find(a => a.UserLoginid == dr.RepCheckUserId);
                if (dre != null)
                {
                    txtPatInspetor.displayMember = dre.UserName;
                    txtPatInspetor.valueMember = dre.UserLoginid;
                }
            }
            else
            {
                txtPatInspetor.displayMember = "";
                txtPatInspetor.valueMember = "";
            }

            if (dr.RepUrgentFlag == 1 || dr.RepUrgentFlag == 2)
            {
                this.chk_pat_critical.Checked = true;
            }
            else
            {
                if (xtabExperiment.SelectedTabPageIndex == 0)
                {
                    if (!string.IsNullOrEmpty(txtPatSampleType.valueMember) && ListConfigCriticalSample.Contains(txtPatSampleType.valueMember))
                    {
                        this.chk_pat_critical.Checked = true;
                    }
                    else
                    {
                        this.chk_pat_critical.Checked = false;
                    }
                }
                else
                {
                    this.chk_pat_critical.Checked = false;
                }


            }
            this.txtAuditName.Text = dr.PidChkName.ToString();
            this.txtAuditTime.Text = dr.RepAuditDate?.ToString();
            this.txtReportName.Text = dr.BgName.ToString();


            if (dr.SampReceiveDate != null)
                lblReceive.Text = dr.SampApplyDate?.ToString("yyyy/MM/dd HH:mm");
            else
                lblReceive.Text = string.Empty;

            if (dr.SampCollectionDate != null)
                lblSam.Text = dr.SampCollectionDate?.ToString("yyyy/MM/dd HH:mm");
            else
                lblSam.Text = string.Empty;

            if (dr.SampSendDate != null)
                lblSdate.Text = dr.SampSendDate?.ToString("yyyy/MM/dd HH:mm");
            else
                lblSdate.Text = string.Empty;

            if (dr.SampApplyDate != null)
                lblApply.Text = dr.SampReceiveDate?.ToString("yyyy/MM/dd HH:mm"); 
            else
                lblApply.Text = string.Empty;

            if (dr.RepReportDate != null)
                lblReport.Text = dr.RepReportDate?.ToString("yyyy/MM/dd HH:mm");
            else
                lblReport.Text = string.Empty;


            setResulto();

            if (UserInfo.GetUserConfigValue("BacteriDefault") == "是")
            {
                var drItr = txtPatInstructment.selectRow;
                if (drItr != null)
                {
                    if (drItr.ItrReportId != null && drItr.ItrReportId.ToString() != "bacilli" && BactList[3].BtypeId == "")
                    {
                        BactList[3].BtypeId = "10007";
                    }
                }
            }
            isDataChaged = false;
            try
            {

                FillTimeLine(CurrentPatInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 显示细菌结果
        /// </summary>
        private void setResulto()
        {
            ClearDt_Byte();

            ClearDT_Cs();
            if (DescList != null && DescList.Count > 0)
            {
                //系统配置：细菌管理同时有药敏与无菌结果时优先药敏
                if (UserInfo.GetSysConfigValue("BacLab_ExistsAnAndCs_SelAn") == "是"
                    && BactList != null && BactList.Count > 0)
                {
                    xtabExperiment.SelectedTabPageIndex = 0;
                }
                else
                {
                    xtabExperiment.SelectedTabPageIndex = 1;
                }
            }
            else if (BactList != null && BactList.Count > 0)
            {
                xtabExperiment.SelectedTabPageIndex = 0;
            }
            else
            {
                if (UserInfo.GetSysConfigValue("BacteriDefaultType") == "鉴定药敏")
                    xtabExperiment.SelectedTabPageIndex = 0;
                else
                    xtabExperiment.SelectedTabPageIndex = 1;
            }


            int cnt = DescList.Count;
            if (cnt > 0)
            {
                if (cnt > 10)
                    cnt = 10;
                var list = gcDesc.DataSource as List<EntityObrResultDesc>;
                for (int j = 0; j < cnt; j++)
                {
                    list[j].ObrValue = DescList[j].ObrValue;
                    list[j].ObrCheckObj = DescList[j].ObrCheckObj;
                    list[j].ObrPositiveFlag = DescList[j].ObrPositiveFlag;
                }
                gcDesc.RefreshDataSource();
            }

            if (BactList != null && BactList.Count > 0)
            {
                List<EntityObrResultBact> nullList = bs_rlts.DataSource as List<EntityObrResultBact>;
                int count = BactList.Count;
                if (count > 6)
                    count = 6;
                for (int i = 0; i < count; i++)
                {
                    var drbid = ((List<EntityDicMicBacteria>)bsbacteri1.DataSource).Find(a => a.BacId == BactList[i].ObrBacId.ToString());//.Select("bac_id='" + dt_btype.Rows[i]["bar_bid"].ToString() + "'");
                    if (drbid != null)
                    {
                        BactList[i].BtypeId = drbid.BacBtId;
                    }
                    nullList[i].ObrRemark = BactList[i].ObrRemark;
                    nullList[i].ObrBacId = BactList[i].ObrBacId;
                    nullList[i].ObrSterile = BactList[i].ObrSterile;
                    nullList[i].ObrColonyCount = BactList[i].ObrColonyCount;
                    nullList[i].BtypeId = BactList[i].BtypeId;
                    nullList[i].SortNo = BactList[i].SortNo;
                    nullList[i].ObrId = BactList[i].ObrId;
                    nullList[i].ObrSid = BactList[i].ObrSid;
                    nullList[i].ObrDate = BactList[i].ObrDate;
                    nullList[i].ObrItrId = BactList[i].ObrItrId;
                    nullList[i].BacCname = BactList[i].BacCname;
                    nullList[i].ObrMrbFlag = BactList[i].ObrMrbFlag;
                    nullList[i].ObrIddFlag = BactList[i].ObrIddFlag;
                    if (i == 0)
                    {
                        this.txtBac.Text = BactList[i].ObrBacId.ToString();
                    }
                }
                gcBac.RefreshDataSource();
            }
            else
            {
                setDefaultType();
            }

            if (AntiList != null)
            {
                List<EntityObrResultAnti> dt_anti3 = new List<EntityObrResultAnti>();
                foreach (var dr_btype in BactList)
                {
                    //系统配置：细菌药敏允许相同细菌
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("BtAllowAddSameBarbid") == "是")
                    {
                        var drs = AntiList.FindAll(a => a.ObrBacId == dr_btype.ObrBacId.ToString() && a.SortNo == dr_btype.SortNo);//.Select("bt_id='" + dr_btype["bar_bid"] + "' and anr_seq=" + dr_btype["bar_seq"]);
                        for (int i = 0; i < drs.Count; i++)
                        {
                            drs[i].Sstype = (i + 1).ToString();
                            dt_anti3.Add((EntityObrResultAnti)drs[i].Clone());
                        }
                    }
                    else
                    {
                        var drs = AntiList.FindAll(a => a.ObrBacId == dr_btype.ObrBacId.ToString());
                        for (int i = 0; i < drs.Count; i++)
                        {
                            drs[i].Sstype = (i + 1).ToString();
                            dt_anti3.Add((EntityObrResultAnti)drs[i].Clone());
                        }
                    }
                }
                AntiList = dt_anti3;
                this.bsAnti.DataSource = AntiList;
                this.gvAnti.ExpandAllGroups();
                if (AntiList.Count > 0)
                {
                    try
                    {
                        radioGroupEventEnable = false;
                        string stMic = AntiList[0].ObrRef;
                        if (stMic == "MIC")
                        {
                            this.radioGroup2.SelectedIndex = 0;
                        }
                        if (stMic == "KB")
                        {
                            this.radioGroup2.SelectedIndex = 1;
                        }
                    }
                    finally
                    {
                        radioGroupEventEnable = true;
                    }

                }
            }

            if (this.ceCombine.listRepDetail != null)
            {
                PatientsMi_RowChanged(null, null);
            }

            if (xtabExperiment.SelectedTabPageIndex == 1 && (DescList == null || DescList.Count == 0))
            {
                try
                {
                    List<EntityObrResultDesc> table = this.gcDesc.DataSource as List<EntityObrResultDesc>;
                    if (table == null)
                        return;
                    foreach (var item in table)
                    {
                        item.ObrDescribe = string.Empty;
                    }
                    foreach (var item in this.ceCombine.listRepDetail)
                    {
                        string comID = item.ComId;
                        List<string> defDataList = new ProxyObrResult().Service.GetCombineDefData(txtPatInstructment.valueMember, comID);
                        for (int i = 0; i < defDataList.Count; i++)
                        {
                            if (table.Count > i)
                            {
                                table[i].ObrValue = defDataList[i];
                            }
                        }

                    }

                    this.gcDesc.Refresh();
                }
                catch
                {

                }
            }
        }


        //根据ID得到数据
        private void GetPatDetailData(string _pat_id)
        {
            isDataChaged = false;

            var resList = new ProxyMicEnter().Service.GetMicPatinentData(_pat_id);
            if (ceCombine.listRepDetail != null)
                ceCombine.listRepDetail.Clear();
            if (resList == null || resList.patient == null)
            {
                this.CurrentPatInfo = new EntityPidReportMain();
                BactList.Clear();
                AntiList.Clear();
                DescList.Clear();
                return;
            }
            ceCombine.listRepDetail = resList.patient.ListPidReportDetail;
            this.CurrentPatInfo = resList.patient;
            this.BactList = resList.listBact;
            this.AntiList = resList.listAnti;
            this.DescList = resList.listDesc;
        }

        /// <summary>
        /// 根据索引得到数据
        /// </summary>       
        private void getIndexValue()
        {
            if (bsPatLst.Current != null)
            {
                EntityPidReportMain dr = (EntityPidReportMain)bsPatLst.Current;

                if (dr != null && !string.IsNullOrEmpty(dr.RepId))
                {
                    if (dr.RepId != CurrentPatID)
                    {
                        if (AnPatientChanging(CurrentPatID, dr.RepId, dr))
                        {
                            CurrentPatID = dr.RepId;
                            this.GetPatDetailData(dr.RepId);
                            this.FillUiFromEntity();
                            isDataChaged = false;
                            this.fpat_sid_old.Text = this.txtPatSid.Text;
                            this.labelControl2.Text = "编辑";
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 获取当前选中仪器的默认标本,并显示到界面
        /// </summary>
        /// <param name="_itr_id"></param>
        private void getItrDefaultSample()
        {
            if (txtPatInstructment.displayMember != "" && txtPatInstructment.valueMember != null)
            {
                String _sam_id = this.txtPatInstructment.selectRow.ItrSamId;
                var drSam_ids = this.txtPatSampleType.dtSource.Find(a => a.SamId == _sam_id);//.Select(" sam_id ='" + _sam_id + "'");
                if (drSam_ids != null)
                {
                    this.txtPatSampleType.selectRow = drSam_ids;
                    this.txtPatSampleType.displayMember = drSam_ids.SamName;
                    this.txtPatSampleType.valueMember = _sam_id;
                }
            }
        }

        private void addNew()
        {
            addNew(true);
        }


        //增加、刷新
        private void addNew(bool MaxSid)
        {
            this.labelControl2.Text = "新增";
            this.txtPatDate.Properties.ReadOnly = false;
            patidentity = null;
            //新增时设计所有属性均可编辑
            setIsModify(true);
            //*********************************************

            if (MaxSid)
            {
                //系统配置：细菌报告采用当前标本号加1
                if (ConfigHelper.GetSysConfigValueWithoutLogin("BtUseCurrentSamAddOne") == "是")
                {
                    if (this.bsPatLst.DataSource != null)
                    {
                        List<EntityPidReportMain> dtpat = this.bsPatLst.DataSource as List<EntityPidReportMain>;
                        if (dtpat != null && dtpat.Count > 0 && !string.IsNullOrEmpty(txtPatSid.Text))
                        {
                            var drtempSel = dtpat.Find(a => a.RepSid == txtPatSid.Text && a.RepInDate.Value.Date == txtPatDate.DateTime.Date);//("pat_sid_int='" + txtPatSid.Text + "'");
                            if (drtempSel != null)
                            {
                                if (dtpat.Find(a => a.RepSid == (drtempSel.PatSidInt + 1).ToString() && a.RepInDate.Value.Date == txtPatDate.DateTime.Date) == null)
                                {
                                    txtPatSid.Text = (drtempSel.PatSidInt + 1).ToString();
                                }
                                else
                                {
                                    txtPatSid.Text = getMaxSID();
                                }
                            }
                            else
                            {
                                txtPatSid.Text = getMaxSID();
                            }
                        }
                        else
                        {
                            txtPatSid.Text = getMaxSID();
                        }
                    }
                    else
                    {
                        txtPatSid.Text = getMaxSID();
                    }
                }
                else
                {
                    txtPatSid.Text = getMaxSID();
                }
                txtNewSid.EditValue = GetMaxCxhNewSID();
            }

            //根据设置保留还是记忆界面原有值

            fpat_id = "";

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatName"))
            {
                this.txtPatName.EditValue = DBNull.Value;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDeptId"))
            {
                this.txtPatDeptId.valueMember = null;
                this.txtPatDeptId.displayMember = null;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatBedNo"))
            {
                this.txtPatBedNo.EditValue = null;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSex"))
            {
                this.txtPatSex.valueMember = null;
                this.txtPatSex.displayMember = null;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSampleState"))
            {
                this.txtPatSampleState.valueMember = null;
                this.txtPatSampleState.displayMember = null;
            }

            DateTime dtToday = ServerDateTime.GetServerDateTime();
            fpat_unit.EditValue = null;
            txtPatSDate.EditValue = dtToday;
            txtPatReachDate.EditValue = dtToday;
            txtPatRecDate.EditValue = dtToday;
            txtPatSampleDate.EditValue = dtToday;
            txtPatApplyDate.EditValue = dtToday;
            this.txtBirthday.EditValue = string.Empty;

            //********************************************************************************
            //增加申请时间为当前申请时间
            txtPatReceiveDate.EditValue = dtToday;

            //********************************************************************************

            //录入日期--记忆,获取最新的
            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDate"))
            {
                this.txtPatDate.EditValue = dtToday;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatID"))
            {
                this.txtPatID.EditValue = null;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatIdType"))
            {
                var drPatId = txtPatIdType.dtSource.Find(a => a.IdtCode == "barcode");//.Select("no_code='barcode'");
                if (drPatId != null)
                {
                    txtPatIdType.displayMember = drPatId.IdtName;
                    txtPatIdType.valueMember = drPatId.IdtId;
                }
                else
                {
                    if (UserInfo.GetSysConfigValue("UseBarcode") == "是")  //如果启用条码,就默认跳到条码号  2010-5-4 
                        this.txtPatIdType.SelectByDispaly("条码号");
                    else
                    {
                        txtPatIdType.displayMember = "";
                        txtPatIdType.valueMember = "";
                    }
                }
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDoc"))
            {
                this.txtPatDoc.valueMember = null;
                this.txtPatDoc.displayMember = null;
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPat_chk_purpose"))
            {
                txtPat_chk_purpose.displayMember = ""; txtPat_chk_purpose.valueMember = "";
            }

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatDiag"))
            {
                this.txtPatDiag.valueMember = null;
                this.txtPatDiag.displayMember = null;
            }
            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatFeeType"))
            {
                txtPatFeeType.displayMember = "";
            }
            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatChkType"))
            {
                luePatCheckType.valueMember = "";
                luePatCheckType.displayMember = "";
            }
            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSamRem"))
            {
                txtPatSamRem.displayMember = "";
            }


            fpat_sid_old.Text = "";
            this.txtFlag.Text = "";
            txtPatBarCode.Text = "";
            if (UserInfo.GetSysConfigValue("BacteriDefaultType") == "鉴定药敏")
                xtabExperiment.SelectedTabPageIndex = 0;
            else
                xtabExperiment.SelectedTabPageIndex = 1;
            ClearDt_Byte();
            ClearDT_Anti();
            ClearDT_Cs();
            setDefaultType();

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("textAgeInput1"))
            {
                this.textAgeInput1.AgeValueText = string.Empty;
            }
            if (ceCombine.listRepDetail != null)
                ceCombine.listRepDetail.Clear();//清空原来的
            ceCombine.RefreshEditBoxText();

            #region 默认仪器组合
            var drPat_itrs = txtPatInstructment.selectRow;

            if (drPat_itrs != null && txtPatInstructment.valueMember != null && txtPatInstructment.valueMember.Trim() != "")
            {
                if (!string.IsNullOrEmpty(drPat_itrs.ItrComId))
                {
                    ceCombine.AddCombine(drPat_itrs.ItrComId);
                }
            }
            #endregion

            fpat_work.Text = "";
            fpat_tel.Text = "";
            fpat_email.Text = "";
            fpat_address.Text = "";
            fpat_height.Text = "";
            fpat_weight.Text = "";
            fpat_pre_week.Text = "";
            fpat_sample_part.Text = "";
            fpat_exp2.Text = "";
            fpat_comment2.Text = "";
            this.txtPatAppNo.Text = string.Empty;
            this.txtPatPid.Text = string.Empty;
            this.txtPatUpid.Text = string.Empty;//滨海唯一号
            this.txtPatUpid2.Text = string.Empty;//滨海唯一号
            this.txtPatNotice.Text = string.Empty;//注意事项 by 2014-03-12 add
            this.txt_pat_host_order.Text = string.Empty;

            this.txtPatSocialNo.Text = string.Empty;
            this.txtPatEmpCompanyName.Text = string.Empty;
            this.txt_pat_emp_id.Text = string.Empty;
            this.txtPatAdmissTime.Text = "0";
            this.chk_pat_critical.Checked = false;

            txtAuditName.Text = string.Empty;
            txtAuditTime.Text = string.Empty;
            txtReportName.Text = string.Empty;
            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatInspetor") || string.IsNullOrEmpty(txtPatInspetor.valueMember))
            {
                this.txtPatInspetor.valueMember = null;
                this.txtPatInspetor.displayMember = null;
                defaultInspector();
            }
            this.gvPatList.ClearSelection();

        }

        //设置默认菌株
        private void setDefaultType()
        {
            if (UserInfo.GetUserConfigValue("BacteriDefault") == "是")
            {
                var drItr = txtPatInstructment.selectRow;//txtItr
                if (drItr != null && BactList != null && BactList.Count > 3)
                {
                    if (!string.IsNullOrEmpty(drItr.ItrReportId) && drItr.ItrReportId != "bacilli")
                    {
                        BactList[0].ObrBacId = 10006;
                        BactList[0].BacCname = "30011";
                        BactList[1].ObrBacId = 10006;
                        BactList[1].BacCname = "30012";
                        BactList[2].ObrBacId = 10006;
                        BactList[2].BacCname = "30016";
                        BactList[3].ObrBacId = 10007;
                    }
                }
            }
        }

        /// <summary>
        /// 清空菌株
        /// </summary>
        private void ClearDt_Byte()
        {
            List<EntityObrResultBact> list = new List<EntityObrResultBact>();

            for (int i = 0; i < 6; i++)
            {
                list.Add(new EntityObrResultBact());
            }

            bs_rlts.DataSource = list;
        }

        /// <summary>
        /// 清空药敏
        /// </summary>
        private void ClearDT_Anti()
        {
            AntiList.Clear();
        }

        /// <summary>
        /// 清空涂片结果
        /// </summary>
        private void ClearDT_Cs()
        {
            List<EntityObrResultDesc> list = new List<EntityObrResultDesc>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new EntityObrResultDesc { ObrPositiveFlag = 0 });
            }
            this.gcDesc.DataSource = list;
        }


        private bool bSave = true;

        private void GetBactResult(EntityQcResultList saveData, DateTime dtNow)
        {
            List<EntityObrResultBact> baclist = bs_rlts.DataSource as List<EntityObrResultBact>;

            foreach (EntityObrResultBact bacInfo in baclist)
            {
                if (bacInfo.ObrBacId != 0)
                {
                    bacInfo.ObrItrId = txtPatInstructment.valueMember;
                    bacInfo.ObrSid = Convert.ToInt64(txtPatSid.Text);
                    bacInfo.ObrDate = dtNow;
                    saveData.listBact.Add(bacInfo);
                }
            }
        }

        private bool GetAntiResult(EntityQcResultList saveData, DateTime dtNow)
        {
            List<EntityObrResultAnti> anlist = bsAnti.DataSource as List<EntityObrResultAnti>;

            foreach (EntityObrResultAnti antiInfo in anlist)
            {
                if (!string.IsNullOrEmpty(antiInfo.ObrValue) || !string.IsNullOrEmpty(antiInfo.ObrValue2))
                {
                    if (this.radioGroup2.SelectedIndex == 1 && antiInfo.ObrRef == "KB")
                    {
                        if (UserInfo.GetSysConfigValue("AntiKBResultCheck") == "是")
                        {
                            string[] strRangeList = UserInfo.GetSysConfigValue("AntiKBResultCheckRange").Split(',');
                            double strRes = 0;
                            double strLower = Convert.ToDouble(strRangeList[0]);
                            double strHigh = Convert.ToDouble(strRangeList[1]);
                            if (double.TryParse(antiInfo.ObrValue, out strRes))
                            {
                                if (strRes > strHigh || strRes < strLower)
                                {
                                    MessageDialog.Show(string.Format("[{3}]，输入的结果[{0}] >{1} 或 <{2}，请重新输入", strRes, strHigh, strLower, antiInfo.BacName), "提示");
                                    gcAnti.Focus();
                                    return false;
                                }
                            }

                        }
                    }
                    antiInfo.ObrItrId = txtPatInstructment.valueMember;
                    antiInfo.ObrSid = Convert.ToInt64(txtPatSid.Text);
                    antiInfo.ObrDate = dtNow;
                    saveData.listAnti.Add(antiInfo);
                }
            }
            return true;
        }

        private void GetDescResult(EntityQcResultList saveData, DateTime dtNow)
        {
            List<EntityObrResultDesc> descs = gcDesc.DataSource as List<EntityObrResultDesc>;

            int rowIndex = 1;
            foreach (var drCs in descs)
            {
                if (!string.IsNullOrEmpty(drCs.ObrValue))
                {
                    drCs.ObrItrId = txtPatInstructment.valueMember;
                    drCs.ObrDate = dtNow;
                    drCs.ObrCheckObj = drCs.ObrCheckObj;
                    drCs.ObrSid = Convert.ToInt64(txtPatSid.Text);
                    drCs.SortNo = rowIndex;

                    string bsr_cname = drCs.ObrValue;

                    //去掉录入结果前面的序号。
                    string[] bsr_cnames = bsr_cname.Split('&');
                    if (bsr_cnames.Length > 1)
                    {
                        bsr_cname = string.Empty;
                        for (int i = 1; i < bsr_cnames.Length; i++)
                        {
                            bsr_cname += bsr_cnames[i];
                        }
                    }
                    drCs.ObrValue = bsr_cname;

                    rowIndex++;
                    saveData.listDesc.Add(drCs);
                }
            }
        }

        private void SaveOrUpdate()
        {
            gvBac.CloseEditor();
            gvDesc.CloseEditor();
            gvAnti.CloseEditor();

            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;

            DateTime dtNow = ServerDateTime.GetServerDateTime();
            EntityQcResultList saveData = new EntityQcResultList();
            EntityPidReportMain dr = new EntityPidReportMain();
            this.FillEntityFromUI(dr);

            if (!string.IsNullOrEmpty(fpat_id))
            {
                EntityPidReportMain current = this.gvPatList.GetFocusedRow() as EntityPidReportMain;
                dr.HISPatientID = current?.HISPatientID;
                dr.HISSerialnum = current?.HISSerialnum;
                dr.MicReportDate = current?.MicReportDate;
                dr.MicReportFlag = current == null?0: current.MicReportFlag;
                dr.MicReportChkUserID = current.MicReportChkUserID;
                dr.MicReportSendUserID = current.MicReportSendUserID;
            }
            else
            {
                //新增记录的情况
                if(NewPatientForSave!=null && NewPatientForSave.RepBarCode == dr.RepBarCode)
                {
                    dr.HISPatientID = NewPatientForSave.HISPatientID;
                    dr.HISSerialnum = NewPatientForSave.HISSerialnum;
                }

            }
            
            String _pat_id = this.fpat_id;
            saveData.patient = dr;
            if (ceCombine.listRepDetail != null)
                saveData.listRepDetail = EntityManager<EntityPidReportDetail>.ListClone(ceCombine.listRepDetail);

            if (xtabExperiment.SelectedTabPageIndex == 0)
            {
                List<EntityObrResultBact> baclist = bs_rlts.DataSource as List<EntityObrResultBact>;
                List<EntityObrResultAnti> anlist = bsAnti.DataSource as List<EntityObrResultAnti>;
                if (baclist == null || anlist == null)
                {
                    MessageDialog.ShowAutoCloseDialog("数据异常,请刷新");
                    return;
                }
                GetBactResult(saveData, dtNow);
                if (!GetAntiResult(saveData, dtNow))
                    return;
            }
            else
            {
                List<EntityObrResultDesc> descs = gcDesc.DataSource as List<EntityObrResultDesc>;
                if (descs == null)
                {
                    MessageDialog.ShowAutoCloseDialog("数据异常,请刷新");
                    return;
                }
                GetDescResult(saveData, dtNow);
            }

            EntityPatientQC qc = new EntityPatientQC();
            qc.ListItrId = new List<string> { txtPatInstructment.valueMember };
            qc.DateStart = txtPatDate.DateTime.Date;
            qc.DateEnd = txtPatDate.DateTime.Date.AddDays(1).AddSeconds(-1);
            qc.RepSid = txtPatSid.Text.ToString();
            List<EntityPidReportMain> plist = new ProxyMicEnter().Service.MicPatientQuery(qc);
            if (_pat_id == "")
            {
                if (plist.Count > 0)
                {
                    MessageDialog.Show("已存在该样本，请勿重复添加", "提示");
                    return;
                }

                //无条可录入
                if (Lab_NoBarcodeNeedAuditCheek
                    && (string.IsNullOrEmpty(Lab_NoBarCodeAuditCheckItrExList) ||
                    !Lab_NoBarCodeAuditCheckItrExList.Contains(txtPatInstructment.valueMember))
                    && string.IsNullOrEmpty(dr.RepBarCode)
                    )
                {

                    FrmCheckPassword frmCheck = new FrmCheckPassword("NoBarcode_CanInput");
                    frmCheck.Text = "无条码报告录入确认";
                    DialogResult dig = frmCheck.ShowDialog();
                    if (dig != DialogResult.OK)
                    {
                        return;
                    }
                }

                var ret = new ProxyMicEnter().Service.InsertMicPatResult(Caller, saveData);
                StringBuilder msg = new StringBuilder();

                this.bsPatLst.PositionChanged -= this.bsPatLst_PositionChanged;
                try
                {
                    EntityPidReportMain _dr = (EntityPidReportMain)bsPatLst.AddNew();
                    _dr = (EntityPidReportMain)dr.Clone();
                    _dr.RepId = txtPatInstructment.valueMember.ToString() + String.Format("{0:yyyyMMdd}", txtPatDate.EditValue) + txtPatSid.Text.ToString();
                    _dr.RepStatusName = "未" + AuditWord;
                    bsPatLst.EndEdit();
                }
                finally
                {
                    this.bsPatLst.PositionChanged += this.bsPatLst_PositionChanged;
                }
            }
            else
            {
                if (Convert.ToInt64(txtPatSid.Text) != Convert.ToInt64(fpat_sid_old.Text) && plist.Count > 0)
                {
                    lis.client.control.MessageDialog.Show("已存在该样本号，请重新修改！", "提示");
                    txtPatSid.Focus();
                    return;
                }

                var ret = new ProxyMicEnter().Service.UpdateMicPatResult(Caller, saveData);
                StringBuilder msg = new StringBuilder();

                bsPatLst.EndEdit();
            }
            txtPatID.Focus();
        }


        /// <summary>
        /// 把界面值放入DataRow
        /// </summary>
        /// <param name="dr"></param>
        private void FillEntityFromUI(EntityPidReportMain patient)
        {
            patient.RepSid = this.txtPatSid.Text;
            patient.RepInDate = Convert.ToDateTime(this.txtPatDate.EditValue);


            patient.RepItrId = this.txtPatInstructment.valueMember;

            patient.ItrName = this.txtPatInstructment.displayMember;

            patient.PidName = this.txtPatName.Text;
            patient.PidSex = this.txtPatSex.valueMember;

            patient.PidInsuId = this.txtPatFeeType.displayMember;//费用类别
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


            patient.PidDeptId = this.txtPatDeptId.valueMember;
            patient.PidDeptName = this.txtPatDeptId.displayMember;

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
            //drPat["pat_rem"] = this.txtPatSampleState.valueMember;
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
            if (this.fpat_comment2.EditValue != null)
            {
                patient.RepComment = this.fpat_comment2.EditValue.ToString();
            }
            patient.PidSamId = this.txtPatSampleType.valueMember;//样本类别
            patient.PidPurpId = this.txtPat_chk_purpose.valueMember;//检验目的

            patient.PidDoctorCode = this.txtPatDoc.valueMember;
            patient.PidDocName = this.txtPatDoc.displayMember;

            //如果检验者为空，则默认为登录者
            if (string.IsNullOrEmpty(this.txtPatInspetor.valueMember))
            {
                patient.RepCheckUserId = UserInfo.loginID;
            }
            else
            {
                patient.RepCheckUserId = this.txtPatInspetor.valueMember;
            }
            patient.SampCheckDate = Convert.ToDateTime(txtPatRecDate.EditValue);


            patient.RepCtype = luePatCheckType.valueMember;

            patient.PidComName = ceCombine.Text;

            if (this.txtBirthday.EditValue != null && Convert.ToString(this.txtBirthday.EditValue) != "")
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

            if (this.txtPatApplyDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatApplyDate.EditValue.ToString()))
            {
                patient.SampApplyDate = Convert.ToDateTime(this.txtPatApplyDate.EditValue);
            }

            if (this.txtPatRecDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatRecDate.EditValue.ToString()))
            {
                patient.RepPrintDate = Convert.ToDateTime(this.txtPatRecDate.EditValue);
            }
            if (this.fpat_exp2.EditValue != null && !string.IsNullOrEmpty(this.fpat_exp2.EditValue.ToString()))
            {
                patient.RepRemark = this.fpat_exp2.EditValue.ToString();
            }

            patient.PidSocialNo = txtPatSocialNo.Text;
            if (!string.IsNullOrEmpty(this.fpat_sample_part.EditValue.ToString()))
            {
                patient.CollectionPart = this.fpat_sample_part.EditValue.ToString();
            }
            patient.PidApplyNo = this.txtPatAppNo.Text;

            patient.RepBarCode = this.txtPatBarCode.Text;

            if (this.txtPatReceiveDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatReceiveDate.EditValue.ToString()))
            {
                patient.SampReceiveDate = Convert.ToDateTime(this.txtPatReceiveDate.EditValue);
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

            patient.RepId = fpat_id;
            patient.PidUniqueId = txtPatUpid.Text;



            patient.RepCheckUserId = txtPatInspetor.valueMember;
            patient.RepStatus = txtFlag.Text == "" ? 0 : Convert.ToInt32(txtFlag.Text);
            patient.RepRemark = fpat_exp2.Text;
            patient.PidRemark = this.txtPatSampleState.displayMember;

            if (this.chk_pat_critical.Checked)
            {
                patient.RepUrgentFlag = 1;
            }
            List<EntityObrResultBact> list = (List<EntityObrResultBact>)this.bs_rlts.DataSource;
            if (list != null && list.Count > 0)
            {
                list = list.FindAll(w => w.ObrMrbFlag == 1);
                if (list.Count > 0)
                {
                    patient.RepDrugfastFlag = 1;
                }
            }
            if (PatInfo != null && patient.RepBarCode == PatInfo.BarCode)
            {
                if (string.IsNullOrEmpty(patient.PidDoctorCode))
                {
                    patient.PidDoctorCode = PatInfo.SenderID;
                }
                if (string.IsNullOrEmpty(patient.PidSrcId))
                {
                    patient.PidSrcId = PatInfo.Ori_id;
                }
                if (string.IsNullOrEmpty(patient.PidDeptId))
                {
                    patient.PidDeptId = PatInfo.SenderDeptCode;
                }
                if (string.IsNullOrEmpty(patient.PidDeptName))
                {
                    patient.PidDeptName = PatInfo.SenderDeptName;
                }
            }

        }



        /// <summary>
        /// 自定义事件
        /// </summary>
        /// <param name="c"></param>
        public void SetTable(ClickEventArgs c)
        {
            var Anti = ConvertToAnti(c.Antibio2);

            EntityObrResultBact dr = this.gvBac.GetFocusedRow() as EntityObrResultBact;
            if (dr.ObrBacId == 0)
            {
                lis.client.control.MessageDialog.Show("请先选择菌株！", "提示");
                return;
            }
            setDt_Anti(Anti, true, dr.ObrBacId.ToString());
        }

        private List<EntityObrResultAnti> ConvertToAnti(List<EntityDicMicAntidetail> antibio2)
        {
            List<EntityObrResultAnti> anits = new List<EntityObrResultAnti>();
            foreach (var dt in antibio2)
            {
                EntityObrResultAnti anti = new EntityObrResultAnti();
                anti.AntId = dt.AnsAntiCode;
                anti.ObrAntId = dt.AnsAntiCode;
                anti.ObrAtypeId = dt.AnsDefId;
                anti.SsHstd = dt.AnsStdUpperLimit;
                anti.SsIzone = dt.AnsZoneIntermed;
                anti.SsLstd = dt.AnsStdLowerLimit;
                anti.SsMstd = dt.AnsStdMiddleLimit;
                anti.SsRzone = dt.AnsZoneDurgfast;
                anti.SsSzone = dt.AnsZoneSensitive;
                anti.ObrRef = "KB";
                anits.Add(anti);
            }

            return anits;
        }

        /// <summary>
        /// 添加抗生素
        /// </summary>
        /// <param name="Anti"></param>
        private void setDt_Anti(List<EntityObrResultAnti> Anti, bool judge, string type)
        {
            int i = 1;
            //if (judge)
            //{
            //    i = AntiList.Where(a => a.ObrBacId == type).Count() + 1;//AntiList.Select("bt_id='" + type + "'").Length + 1;
            //}
            if (judge)
            {
                if (Anti != null && Anti.Count > 0)
                {
                    int temp_anr_seq = 1;//默认序号1
                    EntityObrResultBact dr = this.gvBac.GetFocusedRow() as EntityObrResultBact;
                    temp_anr_seq = dr.SortNo;
                    for (int j = 0; j < Anti.Count; j++)
                    {
                        Anti[j].SortNo = temp_anr_seq;//
                    }
                    i = AntiList.Where(a => a.ObrBacId == type && a.SortNo == temp_anr_seq).Count() + 1;
                }
            }
            else
            {
                if (Anti != null && Anti.Count > 0)
                {
                    int temp_anr_seq = 1;//默认序号1
                    if (AntiList != null && AntiList.Count > 0)
                    {
                        if (AntiList[0].SortNo == 0)
                        {
                            temp_anr_seq = 2;
                        }
                        else
                        {
                            temp_anr_seq = AntiList[0].SortNo + 1;
                        }
                    }

                    for (int j = 0; j < Anti.Count; j++)
                    {
                        Anti[j].SortNo = temp_anr_seq;//
                    }
                }
            }
            foreach (var dtRow in Anti)
            {
                if (AntiList.Count == 0)
                {
                    EntityObrResultBact dr = this.gvBac.GetFocusedRow() as EntityObrResultBact;
                    if (dr != null)
                    {
                        if (this.txtBac.Text.ToString() != "")
                        {
                            dr.ObrBacId = Convert.ToDecimal(this.txtBac.Text);
                        }
                    }
                    dtRow.Sstype = i++.ToString();

                    var rowTemp = (EntityObrResultAnti)dtRow.Clone();
                    rowTemp.Sszone = (rowTemp.ObrRef == "MIC" ? rowTemp.SsMstd : rowTemp.SsIzone);
                    AntiList.Add(rowTemp);
                }
                else
                {
                    bool isTrue = true;
                    foreach (var anrow in AntiList)
                    {
                        if (!string.IsNullOrEmpty(anrow.AntId)
                            && !string.IsNullOrEmpty(anrow.ObrBacId))
                        {
                            try
                            {
                                if (dtRow.AntId == anrow.AntId && anrow.ObrBacId == type)//
                                {
                                    //系统配置：细菌药敏允许相同细菌
                                    if (ConfigHelper.GetSysConfigValueWithoutLogin("BtAllowAddSameBarbid") == "是"
                                        && dtRow.SortNo != anrow.SortNo)
                                    {
                                    }
                                    else
                                    {
                                        isTrue = false;
                                        break;
                                    }
                                }
                            }
                            catch (Exception)
                            { }
                        }
                    }
                    if (isTrue)
                    {
                        EntityObrResultBact dr = this.gvBac.GetFocusedRow() as EntityObrResultBact;
                        if (dr != null)
                        {
                            if (string.IsNullOrEmpty(dtRow.ObrBacId))
                            {
                                if (dr.ObrBacId != 0)
                                {
                                    dtRow.ObrBacId = dr.ObrBacId.ToString();
                                }
                            }
                            if (dr.SortNo == 0 && dtRow.SortNo != 0)
                            {
                                dr.SortNo = dtRow.SortNo;
                            }
                        }
                        dtRow.Sstype = i++.ToString();

                        var rowTemp = (EntityObrResultAnti)dtRow.Clone();
                        rowTemp.ObrRef = radioGroup2.SelectedIndex == 0 ? "MIC" : "KB";
                        rowTemp.Sszone = (rowTemp.ObrRef == "MIC" ? rowTemp.SsMstd : rowTemp.SsIzone);
                        AntiList.Add(rowTemp);
                    }
                }
            }
            bsAnti.DataSource = AntiList;
            gcAnti.RefreshDataSource();
        }

        /// <summary>
        /// 样本状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bsPatLst_PositionChanged(object sender, EventArgs e)
        {
            if (bsPatLst.Current != null)
            {

                this.labelControl2.Text = "编辑";
                EntityPidReportMain drLst = (EntityPidReportMain)bsPatLst.Current;

                if (drLst != null && !string.IsNullOrEmpty(drLst.RepId))
                {
                    string pid = drLst.RepId;
                    if (pid != CurrentPatID)
                    {
                        if (AnPatientChanging(CurrentPatID, pid, drLst))
                        {
                            CurrentPatID = pid;
                            this.GetPatDetailData(pid);
                            this.FillUiFromEntity();

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断在哪个区间
        /// </summary>
        /// <param name="std">条件</param>
        /// <param name="balance">对比值</param>
        /// <returns></returns>
        private bool GetStd(string std, double balance)
        {
            if (std == "" || std == null)
            {
                return false;
            }
            std = std.Trim();
            string tem = std;
            for (int i = 0; i <= 9; i++)
            {
                tem = tem.Replace(i.ToString(), "");
            }
            tem = tem.Replace(".", "");

            tem = tem.Trim();
            switch (tem)
            {
                case ">=":
                    try
                    {
                        double count = Convert.ToDouble(std.Replace(">=", ""));
                        if (balance >= count)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    break;
                case ">":
                    try
                    {
                        double count = Convert.ToDouble(std.Replace(">", ""));
                        if (balance > count)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    break;
                case "<=":
                    try
                    {
                        double count = Convert.ToDouble(std.Replace("<=", ""));
                        if (balance <= count)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    break;
                case "<":
                    try
                    {
                        double count = Convert.ToDouble(std.Replace("<", ""));
                        if (balance < count)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    break;
                case "-":
                    try
                    {
                        string[] a = std.Split('-');
                        double count = Convert.ToDouble(a[0]);
                        double count2 = Convert.ToDouble(a[1]);
                        if (count <= balance && balance <= count2)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        break;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        private void txtRes_Enter(object sender, EventArgs e)
        {
            if (gridColumn9.OptionsColumn.AllowFocus == false || this.gridColumn12.OptionsColumn.AllowFocus == true)
            {
                this.gridColumn9.OptionsColumn.AllowFocus = true;
                this.gridColumn12.OptionsColumn.AllowFocus = false;
                gridColumn23.OptionsColumn.AllowFocus = false;
            }
        }

        private void txtSimc_Enter(object sender, EventArgs e)
        {
            txtSimc_Leave(sender, e);
            if (gridColumn9.OptionsColumn.AllowFocus == true || this.gridColumn12.OptionsColumn.AllowFocus == false)
            {
                this.gridColumn9.OptionsColumn.AllowFocus = false;
                this.gridColumn12.OptionsColumn.AllowFocus = true;
                gridColumn23.OptionsColumn.AllowFocus = false;

            }
            strMic = string.Empty;
            drMic = null;
        }

        private void gvAnti_Click(object sender, EventArgs e)
        {
            this.gridColumn9.OptionsColumn.AllowFocus = true;
            this.gridColumn12.OptionsColumn.AllowFocus = true;
            this.gridColumn23.OptionsColumn.AllowFocus = true;
        }

        /// <summary>
        /// 新增单个抗生素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmAddAnti_Click(object sender, EventArgs e)
        {
            EntityObrResultAnti dr = this.gvAnti.GetFocusedRow() as EntityObrResultAnti;
            if (dr != null)
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {
                    string bac_id = dr.ObrBacId.ToString();


                    var btypeList = CacheClient.GetCache<EntityDicMicBacttype>();
                    var bacterialist = CacheClient.GetCache<EntityDicMicBacteria>();

                    var query = from b in btypeList
                                join a in bacterialist on b.BtypeId equals a.BacBtId
                                where a.BacId == bac_id
                                select b.BtypeAtypeId;

                    if (query == null || query.Count() == 0)
                    {
                        MessageDialog.Show("改菌株无药敏卡编码，无法添加！", "提示");
                        return;
                    }

                    var anDetailList = new ProxyMicEnter().Service.GetMicAntidetailList(bac_id);

                    var antiInfo = anDetailList.Find(a => a.AntId == dr.ObrAntId);
                    if (antiInfo != null)
                    {
                        antiInfo = new EntityDicMicAntidetail();
                        antiInfo.AntId = dr.ObrAntId;
                        antiInfo.AnsDefId = query.ToList()[0];
                        antiInfo.AnsDelFlag = "0";
                        antiInfo.AnsDefFlag = "0";
                    }
                    else
                    {
                        MessageDialog.Show("该药敏卡已存在该抗生素！", "提示");
                        return;
                    }
                    List<EntityDicMicAntidetail> saveList = new List<EntityDicMicAntidetail>();

                    saveList.Add(antiInfo);

                    new ProxyMicEnter().Service.SaveMicAntidetailList(saveList);

                    MessageDialog.ShowAutoCloseDialog("操作成功");

                }
                else if (dig == DialogResult.No)
                {
                    MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }
            }
        }

        /// <summary>
        /// 选择范本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FrmBscripe fb = new FrmBscripe(this, "0");
            fb.sam_id = txtPatSampleType.valueMember;
            fb.IsSortSam = true;//过滤标本
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
                fpat_comment2.Text += str;

            }
            if (type == "2")
            {
                memoEdit1.Text += str;
            }
            if (type == "11")
            {
                //txtConclusion.Text += str;
            }
        }

        /// <summary>
        /// 新增所有抗生素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmAddAntiAll_Click(object sender, EventArgs e)
        {
            EntityObrResultAnti dr = this.gvAnti.GetFocusedRow() as EntityObrResultAnti;
            if (dr != null)
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword();
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK)
                {

                    string bac_id = dr.ObrBacId.ToString();


                    var btypeList = CacheClient.GetCache<EntityDicMicBacttype>();
                    var bacterialist = CacheClient.GetCache<EntityDicMicBacteria>();

                    var query = from b in btypeList
                                join a in bacterialist on b.BtypeId equals a.BacBtId
                                where a.BacId == bac_id
                                select b.BtypeAtypeId;

                    if (query == null || query.Count() == 0)
                    {
                        MessageDialog.Show("改菌株无药敏卡编码，无法添加！", "提示");
                        return;
                    }

                    List<EntityObrResultAnti> dt = (List<EntityObrResultAnti>)bsAnti.DataSource;


                    var anDetailList = new ProxyMicEnter().Service.GetMicAntidetailList(bac_id);

                    var drAn_Rlts = dt.FindAll(A => A.ObrBacId == bac_id);

                    List<EntityDicMicAntidetail> saveList = new List<EntityDicMicAntidetail>();

                    foreach (var info in drAn_Rlts)
                    {
                        if (anDetailList.Find(a => a.AntId == info.ObrAntId) == null)
                        {
                            var antiInfo = new EntityDicMicAntidetail();
                            antiInfo.AntId = dr.ObrAntId;
                            antiInfo.AnsDefId = query.ToList()[0];
                            antiInfo.AnsDelFlag = "0";
                            antiInfo.AnsDefFlag = "0";
                            saveList.Add(antiInfo);
                        }
                    }

                    new ProxyMicEnter().Service.SaveMicAntidetailList(saveList);
                    MessageDialog.ShowAutoCloseDialog("操作成功");
                }
                else if (dig == DialogResult.No)
                {
                    lis.client.control.MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }
            }
        }


        /// <summary>
        /// 备注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
            EntityObrResultBact dr = this.gvBac.GetFocusedRow() as EntityObrResultBact;

            if (dr != null)
            {
                dr.ObrRemark = this.memoEdit1.Text;
            }
        }

        /// <summary>
        /// 选择意见范本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            FrmBscripe fb = new FrmBscripe(this, "1");
            fb.ShowDialog();
        }

        /// <summary> 
        /// 选择描述范本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            FrmBscripe fb = new FrmBscripe(this, "2");
            fb.sam_id = txtPatSampleType.valueMember;
            fb.IsSortSam = true;
            fb.ShowDialog();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {

                EntityPidReportMain drRow = (EntityPidReportMain)gvPatList.GetRow(e.RowHandle);
                e.Info.DisplayText = drRow.RepId;
            }
            e.Info.ImageIndex = -1;
        }

        private void cbeAge_EditValueChanged(object sender, EventArgs e)
        {
            GetPatientList(false);
            addNew();
        }


        List<EntityDCLPrintParameter> listPrintData_multithread;
        //删除危急值信息patId集合
        private List<string> listDeleteMsgPatId = null;
        /// <summary>
        /// 打印预览方法
        /// </summary>
        /// <param name="isTrue">true 为打印,false 为预览</param>
        private void printPreview(bool isTrue)
        {
            List<EntityPidReportMain> dt = GetCheckedPatients();
            if (dt.Count <= 0 || gvPatList.RowCount == 0)
            {
                lis.client.control.MessageDialog.Show("未选择数据！", "提示");
                return;
            }

            List<EntityPidReportMain> drPat = new List<EntityPidReportMain>();

            //细菌报告管理未一审的报告单是否可打印预览
            if (!isTrue && UserInfo.GetSysConfigValue("BacterialInput_NotAuditedPrintPreview") == "否")
            {
                drPat = dt.Where(a => a.RepStatus == 1 || a.RepStatus == 2 || a.RepStatus == 4).ToList();
            }
            //细菌报告管理一审的报告单是否可打印预览
            else if (UserInfo.GetSysConfigValue("BacterialInput_AuditedPrintPreview") == "否")
            {
                drPat = dt.Where(a => a.RepStatus == 2 || a.RepStatus == 4).ToList();
            }
            else
            {
                drPat = dt.Where(a => a.RepStatus == 0 || a.RepStatus == 2 || a.RepStatus == 1 || a.RepStatus == 4).ToList();
            }

            if (drPat.Count > 0)
            {
                #region 新打印方式，传多个打印指令打印多个病人，每条指令一个病人
                string prtTemplate = DictInstrmt.Instance.GetItrPrtTemplate(this.txtPatInstructment.valueMember);
                if (prtTemplate == string.Empty)
                {
                    lis.client.control.MessageDialog.Show("找不到当前仪器的打印模版", "提示");
                    return;
                }
                List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
                int sequence = 0;
                foreach (var patient in drPat)
                {
                    //报告打印后确认内部危急值
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_PrintReportConfirmMsg") == "是"
                        && patient.RepUrgentFlag == 1
                        && patient.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        if (listDeleteMsgPatId == null)
                            listDeleteMsgPatId = new List<string>();
                        listDeleteMsgPatId.Add(patient.RepId);
                    }
                    EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                    printPara.RepId = patient.RepId;
                    printPara.ReportCode = prtTemplate;
                    printPara.Sequence = sequence;

                    listPara.Add(printPara);
                    sequence++;
                }

                listPrintData_multithread = EntityManager<EntityDCLPrintParameter>.ListClone(listPara);
                pForm_PrintStart2(listPara, isTrue);
                if (!isTrue)//是否为打印
                {
                    if (useMultiThread)
                    {
                        if (!pForm.HasShowPreview)
                        {
                            Thread thread = new Thread(new ThreadStart(StartPreviewReports));
                            thread.IsBackground = true;
                            thread.Start();
                        }
                        else
                        {
                            MessageDialog.Show("打印预览已在后台运行...");
                        }
                    }
                    else
                    {
                        pForm.ShowPrint = false;
                        try
                        {
                            StartPreviewReports();
                            //pForm.PrintPreview2(htPrint);
                        }
                        catch (ReportNotFoundException ex1)
                        {
                            lis.client.control.MessageDialog.Show(ex1.MSG);
                        }
                        catch (Exception ex2)
                        {

                        }
                    }
                }
                else
                {
                    try
                    {
                        DCLReportPrint.BatchPrint(listPara, ReportSetting.PrintName);
                        this.SelectAllPatient(false);
                    }
                    catch (ReportNotFoundException ex1)
                    {
                        lis.client.control.MessageDialog.Show(ex1.MSG);
                    }
                    catch (Exception ex2)
                    {
                        lis.client.control.MessageDialog.Show(ex2.Message);
                    }
                }


                #endregion
            }
            else
            {
                lis.client.control.MessageDialog.Show("所选数据未" + LocalSetting.Current.Setting.ReportWord + "！", "提示");
                return;
            }
        }

        /// <summary>
        /// 打印后不更新打印状态的patID
        /// </summary>
        private List<string> PrintNoUpdateStartPatIDs = new List<string>();
        void pForm_PrintStart2(List<EntityDCLPrintParameter> listPara, bool isPrint)
        {
            List<string> listPatID = new List<string>();

            foreach (EntityDCLPrintParameter item in listPara)
            {
                if (!PrintNoUpdateStartPatIDs.Contains(item.RepId))
                {
                    listPatID.Add(item.RepId);
                }
            }

            if (listPatID != null && listPatID.Count > 0 && isPrint)
            {
                UpdatePrintState(listPatID);
            }
        }
        /// <summary>
        /// 打印窗口的唯一实例
        /// </summary>       
        private FrmReportPrint GetReportPrintFormInstance()
        {
            lock (this)
            {
                if (pForm == null)
                {
                    pForm = new FrmReportPrint();
                }
            }

            return pForm;
        }

        //**************************
        //另一线程的方法
        FrmReportPrint pForm;

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
        }


        bool useMultiThread = true;
        //**************************

        /// <summary>
        /// 打印预览方法
        /// </summary>
        /// <param name="isTrue">true 为打印,false 为预览</param>
        private void printPreview(List<string> patIds, bool isTrue)
        {
            StringBuilder stAll = new StringBuilder();
            for (int i = 0; i < patIds.Count; i++)
            {
                stAll.Append(",'" + patIds[i] + "'");
            }
            stAll.Remove(0, 1);

            string prtTemplate = DictInstrmt.Instance.GetItrPrtTemplate(this.txtPatInstructment.valueMember);
            if (prtTemplate == string.Empty)
            {
                lis.client.control.MessageDialog.Show("找不到当前仪器的打印模版", "提示");
                return;
            }
            List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
            int sequence = 0;
            foreach (string patient_id in patIds)
            {
                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                printPara.RepId = patient_id;
                printPara.ReportCode = prtTemplate;
                printPara.Sequence = sequence;
                listPara.Add(printPara);
                sequence++;
            }
            listPrintData_multithread = EntityManager<EntityDCLPrintParameter>.ListClone(listPara);

            try
            {
                pForm_PrintStart2(listPara, isTrue);
                DCLReportPrint.BatchPrint(listPara);
                this.SelectAllPatient(false);
            }
            catch (ReportNotFoundException ex1)
            {
                lis.client.control.MessageDialog.Show(ex1.MSG);
            }
            catch (Exception)
            {

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

                List<EntityPidReportMain> dt = (List<EntityPidReportMain>)bsPatLst.DataSource;
                foreach (var dr in dt)
                {
                    if (listPatID.Contains(dr.RepId))
                    {
                        dr.RepStatus = 4;
                        dr.RepStatusName = "已打印";
                    }
                }
                gvPatList.ClearSelection();

                if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_PrintReportConfirmMsg") == "是"
                    && listDeleteMsgPatId != null && listDeleteMsgPatId.Count > 0)
                {
                    EntityAuditInfo audit = new EntityAuditInfo();
                    audit.UserId = UserInfo.loginID;
                    audit.UserName = UserInfo.userName;
                    audit.MsgAffirmType = "3";//1-自动确认 2-手工确认 2-报告单打印确认
                    audit.IsOnlyInsgin = true;
                    ProxyObrMessageContent proxyMsg = new ProxyObrMessageContent();
                    List<EntityDicObrMessageContent> messageList = new List<EntityDicObrMessageContent>();
                    messageList = proxyMsg.Service.GetAllMessage(false, false, true);
                    foreach (string id in listDeleteMsgPatId)
                    {
                        ProxyObrMessage proxyDeptMsg = new ProxyObrMessage();
                        EntityDicObrMessageContent message = messageList.Find(i => i.ObrValueA == id);
                        if (message != null)
                            proxyDeptMsg.Service.DeleteMessageByIDAndUpdateCriticalChecker(audit, message.ObrId, id);
                    }
                }
                this.gcPatList.RefreshDataSource();
            }
        }
        bool AnPatientChanging(string prev_patid, string pat_id, EntityPidReportMain drPat)
        {
            if (checkSaveBeforeLeave && isDataChaged && isLoadData && (!string.IsNullOrEmpty(txtPatID.Text) || !string.IsNullOrEmpty(txtPatName.Text)))
            {
                if (MessageDialog.Show("当前资料或结果未保存，是否保存？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    sy_OnBtnSaveClicked(null, null);
                }
            }
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnBtnSaveClicked(object sender, EventArgs e)
        {
            sy.Focus();
            var _drPat = this.gvPatList.GetFocusedRow() as EntityPidReportMain;
            if (_drPat != null && (this.fpat_id != "" && (_drPat.RepStatus.ToString() != "0" || (_drPat.RepInitialFlag.ToString() != "0"))))
            {
                lis.client.control.MessageDialog.Show(string.Format("该记录已{0}或已{1}，无法保存！", AuditWord,
                                                                    LocalSetting.Current.Setting.ReportWord));
                return;
            }
            else if (_drPat != null)
            {
                this.txtFlag.Text = _drPat.RepStatus.ToString(); //获取当前状态
            }
            if (this.txtPatDate.EditValue == null)
            {
                MessageDialog.Show("请输入录入日期！", "提示");
                txtPatDate.Focus();
                return;
            }
            if (this.txtPatInstructment.valueMember == "" || this.txtPatInstructment.valueMember == null)
            {
                MessageDialog.Show("请选择仪器！", "提示");
                txtPatInstructment.Focus();
                return;
            }
            try
            {
                Convert.ToInt64(txtPatSid.Text);
            }
            catch (Exception)
            {
                lis.client.control.MessageDialog.Show("样本号类型错误或为空！", "提示");
                txtPatSid.Focus();
                return;
            }


            if (this.txtPatSampleType.valueMember == "" || this.txtPatSampleType.valueMember == null)
            {
                lis.client.control.MessageDialog.Show("请输入标本类别！", "提示");
                txtPatSampleType.Focus();
                return;
            }

            if (!Compare.IsNullOrDBNull(this.txtPatSDate.EditValue) //送检时间
                && !Compare.IsNullOrDBNull(this.txtPatRecDate.EditValue)) //检验时间
            {
                DateTime sample_send_date = (DateTime)this.txtPatSDate.EditValue;
                DateTime pat_jy_date = (DateTime)this.txtPatRecDate.EditValue;

                if (sample_send_date > pat_jy_date)
                {
                    lis.client.control.MessageDialog.Show("[送检时间]不能大于[检验时间]");
                    return;
                }
            }

            if ((this.txtPatSex.valueMember == null
                 || this.txtPatSex.displayMember == null
                 || this.txtPatSex.displayMember.ToString().Trim(null) == string.Empty) &&
                UserInfo.GetSysConfigValue("Lab_NotNull_Sex") == "是")
            {
                lis.client.control.MessageDialog.Show("请输入[性别]", "提示");
                this.ActiveControl = this.txtPatSex;
                this.txtPatSex.Focus();
                return;
            }

            if ((this.txtPatName.Text == null || this.txtPatName.Text.Trim() == string.Empty)
                && UserInfo.GetSysConfigValue("Lab_NotNull_Name") == "是")
            {
                lis.client.control.MessageDialog.Show("请输入[姓名]", "提示");
                this.ActiveControl = this.txtPatName;
                this.txtPatName.Focus();
                return;
            }

            if ((this.txtPatInspetor.valueMember == null
                 || this.txtPatInspetor.displayMember == null
                 || this.txtPatInspetor.displayMember.ToString().Trim(null) == string.Empty
                )
                && UserInfo.GetSysConfigValue("Lab_NotNull_Inspector") == "是")
            {
                //if ((this.txtPatInspetor.Text == null || this.txtPatName.Text.Trim() == string.Empty)
                //    && UserInfo.GetSysConfigValue("Lab_NotNull_Inspector") == "是")
                {
                    lis.client.control.MessageDialog.Show("请输入[检验者]", "提示");
                    this.ActiveControl = this.txtPatInspetor;
                    this.txtPatInspetor.Focus();
                    return;
                }
            }

            if (ceCombine.listRepDetail == null || ceCombine.listRepDetail.Count == 0)
            {
                MessageDialog.Show("请输入组合项目！", "提示");
                ceCombine.Focus();
                return;
            }
            //未输入年龄时是否提示
            if (this.textAgeInput1.AgeToMinute <= 0)
            {
                if (MessageDialog.Show("当前资料未输入年龄，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }

            SaveOrUpdate();
            if (!bSave)
            {
                return;
            }
            sy_OnBtnRefreshClicked(null, null);
            if (!UserCustomSetting.PatResultPanel.SavePatInfoNoNext)
            {
                long sid = Convert.ToInt64(txtPatSid.Text) + 1;
                txtPatSid.EditValue = sid;
                fpat_id = "";
                txtSid_Leave(null, null);

                if (this.fpat_id == "") //没有下一个样本号则新增
                {
                    this.addNew(false);
                }
                else //有下一个样本号则修改
                {
                    if (xtabExperiment.SelectedTabPageIndex  == 1)
                    {
                        this.gvDesc.Focus();
                    }
                }
            }
            else
                gridView1_Click(sender, e);
        }

        /// <summary>
        /// 默认阴性结果(批量添加菌株)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void sy_BtnDeSpeClick(object sender, EventArgs e)
        {
            if (bsPatLst == null)
                return;

            try
            {
                List<EntityPidReportMain> dt = GetCheckedPatients();//得到所有数据
                if (dt.Count <= 0 || gvPatList.RowCount == 0)
                {
                    lis.client.control.MessageDialog.Show("未选择数据！", "提示");
                    return;
                }

                var drPat = dt.Where(a =>a.RepStatus == 0).ToList();//能添加默认结果的病人信息
                var drPatNot = dt.Where(a => a.RepStatus != 0).ToList();//不能添加默认结果的病人信息
                if (drPatNot.Count > 0)
                {
                    string strnotSidno = "";//不能添加默认结果的样本号
                    for (int j = 0; j < drPatNot.Count; j++)
                    {
                        strnotSidno += "[" + drPatNot[j].RepSid + "] " + drPatNot[j].RepStatusName + "\r\n";
                    }
                    lis.client.control.MessageDialog.Show("如下样本号不能添加默认结果\r\n" + strnotSidno);
                    return;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                MessageDialog.Show("遇到错误:" + ex.Message, "错误提示");
            }
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            DeletePat();

        }
        bool DeletePat()
        {
            gvPatList.CloseEditor();
            this.bsPat.EndEdit();

            DataTable _dtPat = CommonClient.CreateDT(
              new string[] { "pat_id", "pat_bar_code", "pat_name", "pat_sid" }, "patients");
            String pat_chk_date = ServerDateTime.GetServerDateTime().ToLongTimeString(); //DateTime.Now.ToLongTimeString();

            StringBuilder logMsg = new StringBuilder();
            List<EntityPidReportMain> delPatList = new List<EntityPidReportMain>();

            int patCount = 0;
            bool delflag = false;

            if (UserInfo.GetSysConfigValue("BathOrSingleDelFlag").Equals("是"))
            {
                List<EntityPidReportMain> dtPat = GetCheckedPatients();
                if (dtPat == null) return false;
                delflag = true;

                foreach (var dr in dtPat)
                {
                    patCount++;

                    if (dr.RepStatus.ToString() == "0"
                        && dr.RepInitialFlag.ToString() == "0")
                    {
                        delPatList.Add(dr);
                    }
                }

            }
            else
            {
                delflag = false;
                EntityPidReportMain dr = this.gvPatList.GetFocusedRow() as EntityPidReportMain;
                if (dr != null)
                {
                    patCount++;
                    if (dr.RepStatus.ToString() == "0"
                        && dr.RepInitialFlag.ToString() == "0")
                    {
                        delPatList.Add(dr);
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

            if (delPatList.Count < 1)
            {
                lis.client.control.MessageDialog.Show(string.Format("所选数据已{0}/{1}！", AuditWord, LocalSetting.Current.Setting.ReportWord), "提示");
                return false;
            }
            if (delPatList.Count > 1)
            {
                if (MessageDialog.Show(string.Format("您将要删除 {0} 条病人记录，是否继续？", delPatList.Count), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return false;
                }
            }
            else
            {
                object name = delPatList[0].PidName;
                object sid = delPatList[0].RepSid;
                if (MessageDialog.Show(string.Format("您将要删除 姓名:{0},样本号:{1} 的记录，是否继续？", name != null ? name.ToString() : string.Empty, sid != null ? sid.ToString() : string.Empty), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
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

                bool del_flag = false;
                if (LisSysParam.lab_del_flag == LIS_Const.STRUCT_LAB_DEL_FLAG.DEL_ALTER)
                {
                    DialogResult r = MessageDialog.Show("是否连结果一起删除？", "提示", MessageBoxButtons.YesNo);
                    if (r == DialogResult.Yes) { del_flag = true; };
                    if (r == DialogResult.No) { del_flag = false; };
                }
                var ret = new ProxyMicEnter().Service.DeleteMicPatResult(Caller, delPatList, del_flag);

                addNew();
                this.txtPatID.Focus();
                GetPatientList(false);
                return true;
            }
            else if (dig == DialogResult.No)
            {
                MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
            return false;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_BtnResetClick(object sender, EventArgs e)
        {
            GetPatientList(false);
            addNew();
            this.txtPatID.Focus();
        }

        /// <summary>
        /// 临时报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sy_BtnSelectTemplateClick(object sender, EventArgs e)
        {
            this.bsPat.EndEdit();
            gvPatList.CloseEditor();

            #region 记录下一个id号

            int tempFocusedRowHandle = gvPatList.FocusedRowHandle;
            string nextPatID = "";//记录下一个id号
            if (gvPatList.RowCount > 0)
            {
                if (tempFocusedRowHandle < (gvPatList.RowCount - 1))
                {
                    EntityPidReportMain dr = gvPatList.GetRow(tempFocusedRowHandle + 1) as EntityPidReportMain;
                    if (dr != null && dr.RepId.Length > 0)
                    {
                        nextPatID = dr.RepId;
                    }
                }
            }

            #endregion

            string pat_report_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");
            List<string> list_pat_id = new List<string>();
            StringBuilder msg = new StringBuilder();

            //*******************************************************************
            //判断是否开启没有条码号的的检验报告不能审核报告打印
            List<string> list_pat_id_no_bar = new List<string>();

            List<EntityPidReportMain> patLis = GetCheckedPatients();
            if (patLis == null || patLis.Count <= 0)
            {
                MessageDialog.Show("未选择" + LocalSetting.Current.Setting.ReportWord + "数据！", "提示");
                return;
            }

            foreach (var dr in patLis)
            {
                if (string.IsNullOrEmpty(dr.RepBarCode))
                {
                    list_pat_id_no_bar.Add(dr.RepId);
                }
                list_pat_id.Add(dr.RepId);
            }


            EntityOperationResultList result_message = new ProxyPidReportMainAudit().Service.BacAuditCheck(list_pat_id, "0");
            List<string> patlist = new List<string>();
            DialogResult drReturn = new DialogResult();
            if (result_message.FailedCount > 0)
            {
                //显示审核检查提示窗口
                AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(result_message, EnumOperationCode.MidReport);
                drReturn = resultviwer.ShowDialog();
                patlist = resultviwer.GetSelectedID();//获取需要继续操作的病人ID集合
            }
            else//全部检查通过
                patlist = list_pat_id;

            if ((result_message.FailedCount == 0) || (result_message.FailedCount > 0 && drReturn == DialogResult.OK))//点击了"继续"
            {
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - " + "中期报告", LIS_Const.BillPopedomCode.Audit, "", "");
                frmCheck.operationCode = EnumOperationCode.Report;

                //验证窗口
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK && patlist.Count > 0)
                {
                    if (!UserInfo.isAdmin)
                    {
                        bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(this.txtPatInstructment.valueMember, EnumOperationCode.Audit, frmCheck.OperatorID);
                        if (!ok)
                        {
                            lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                            return;
                        }
                    }

                    //判断是否开启没有条码号的的检验报告不能审核报告打印
                    bool isNeedCheckNoBarcode = false;
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarcodeNeedAuditCheek") == "是")
                    {
                        string itrExList =
                            ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeAuditCheckItrExList");

                        if (string.IsNullOrEmpty(itrExList) || !itrExList.Contains(txtPatInstructment.valueMember))
                        {
                            isNeedCheckNoBarcode = true;
                        }

                    }

                    if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotAuditReportPrintOnNoBC") == "是" || isNeedCheckNoBarcode)
                    {
                        if (!new UILogic.PatEnterUILogic().canReportOnNoBarCode(frmCheck, isNeedCheckNoBarcode ? "NoBarcode_CanAudit" : "CanNotAuditReportPrintInNoBC"))
                        {
                            for (int i = patlist.Count - 1; i >= 0; i--)
                            {
                                if (list_pat_id_no_bar.Contains(patlist[i]))
                                {
                                    patlist.Remove(patlist[i]);
                                }
                            }
                            lis.client.control.MessageDialog.ShowAutoCloseDialog(string.Format("共有{1}条记录没有条码号，用户名：[{0}]无权报告没有条码号的报告！", frmCheck.OperatorID, 3));
                        }
                    }
                    if (patlist.Count <= 0)
                    {
                        return;
                    }
                    //*******************************************************************

                    EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                    caller.IPAddress = UserInfo.ip;
                    caller.UserID = frmCheck.Pat_i_code;

                    var resresult = new ProxyMicEnter().Service.MicMidReport(patlist, caller);

                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
                        this.SelectAllPatient(false);
                    }
                    MessageDialog.ShowAutoCloseDialog("操作成功!");
                    if (UserInfo.GetSysConfigValue("PrintOnReport") == "是")
                        printPreview(patlist, true);
                    sy_OnBtnRefreshClicked(null, null);

                    this.LocatePatientByPatID(nextPatID, true);

                }
                else if (dig == DialogResult.No)
                {
                    lis.client.control.MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                    return;
                }
            }
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnAuditClicked(object sender, EventArgs e)
        {
            gvPatList.CloseEditor();
            this.bsPat.EndEdit();

            List<EntityPidReportMain> list = this.bsPatLst.DataSource as List<EntityPidReportMain>;
            if (list == null || list.Count == 0) return;

            String pat_chk_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            List<string> list_pat_id = new List<string>();
            List<EntityPidReportMain> patLis = GetCheckedPatients();
            if (patLis == null || patLis.Count <= 0)
            {
                MessageDialog.Show("未选择" + LocalSetting.Current.Setting.ReportWord + "数据！", "提示");
                return;
            }

            foreach (var dr in patLis)
            {
                list_pat_id.Add(dr.RepId.ToString());
            }

            EntityOperationResultList result_message = new ProxyPidReportMainAudit().Service.BacAuditCheck(list_pat_id, "1");

            List<string> patlist = new List<string>();
            DialogResult drReturn = new DialogResult();

            if (result_message.FailedCount > 0)
            {
                //显示审核检查提示窗口
                AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(result_message, EnumOperationCode.Audit);
                drReturn = resultviwer.ShowDialog();
                patlist = resultviwer.GetSelectedID();//获取需要继续操作的病人ID集合
            }
            else//全部检查通过
                patlist = list_pat_id;

            if ((result_message.FailedCount == 0) || (result_message.FailedCount > 0 && drReturn == DialogResult.OK))//点击了"继续"
            {
                AuditWord = "检验";
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - " + AuditWord, LIS_Const.BillPopedomCode.Audit, "", "");
                frmCheck.operationCode = EnumOperationCode.Audit;

                //验证窗口
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK && patlist.Count > 0)
                {
                    if (!UserInfo.isAdmin)
                    {
                        bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(this.txtPatInstructment.valueMember, EnumOperationCode.Audit, frmCheck.OperatorID);
                        if (!ok)
                        {
                            lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                            return;
                        }

                    }
                    EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                    Caller.IPAddress = UserInfo.ip;
                    Caller.LoginID = frmCheck.OperatorID;
                    Caller.LoginName = frmCheck.OperatorName;
                    Caller.UserID = frmCheck.Pat_i_code;


                    new ProxyMicEnter().Service.MicAudit(patlist, Caller);
                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
                        this.SelectAllPatient(false);
                    }

                    sy_OnBtnRefreshClicked(null, null);
                }
                else if (dig == DialogResult.No)
                {
                    MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }
            }
        }

        /// <summary>
        /// 批量反审
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnUndoAuditClicked(object sender, EventArgs e)
        {
            gvPatList.CloseEditor();
            this.bsPat.EndEdit();

            List<EntityPidReportMain> list = GetCheckedPatients();
            if (list == null || list.Count <= 0)
            {
                MessageDialog.Show(string.Format("未选择{0}数据！", AuditWord), "提示");
                return;
            }
            String pat_report_date = ServerDateTime.GetServerDateTime().ToLongTimeString();// DateTime.Now.ToLongTimeString();

            List<string> lisPatId = new List<string>();
            foreach (var dr in list)
            {
                if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Audited)
                    lisPatId.Add(dr.RepId);
            }

            if (lisPatId.Count < 1)
            {
                MessageDialog.Show(string.Format("所选记录不是{0}状态！", AuditWord), "提示");
                return;
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 取消" + AuditWord, LIS_Const.BillPopedomCode.UndoAudit, "", "");
            frmCheck.operationCode = EnumOperationCode.UndoAudit;
            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {

                EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                caller.IPAddress = UserInfo.ip;
                caller.UserID = frmCheck.Pat_i_code;

                var ret = new ProxyMicEnter().Service.UndoMicAudit(lisPatId, caller);
                for (int i = 0; i < this.gvPatList.RowCount; i++)
                {
                    EntityPidReportMain dr = this.gvPatList.GetRow(i) as EntityPidReportMain;
                    if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        dr.RepStatus = 0;
                        dr.RepStatusName = "未" + AuditWord;
                        dr.RepAuditUserId = "";
                        dr.RepAuditUserName = "";
                    }
                }

                if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                {
                    this.SelectAllPatient(false);
                }

                MessageDialog.ShowAutoCloseDialog("操作成功!");
            }
            else if (dig == DialogResult.No)
            {
                lis.client.control.MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
        }

        /// <summary>
        /// 批量报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnReportClicked(object sender, EventArgs e)
        {
            this.bsPat.EndEdit();
            gvPatList.CloseEditor();

            #region 记录下一个id号

            int tempFocusedRowHandle = gvPatList.FocusedRowHandle;
            string nextPatID = "";//记录下一个id号
            if (gvPatList.RowCount > 0)
            {
                if (tempFocusedRowHandle < (gvPatList.RowCount - 1))
                {
                    EntityPidReportMain dr = gvPatList.GetRow(tempFocusedRowHandle + 1) as EntityPidReportMain;
                    if (dr != null && dr.RepId.Length > 0)
                    {
                        nextPatID = dr.RepId;
                    }
                }
            }


            #endregion

            string pat_report_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");

            List<string> list_pat_id = new List<string>();
            StringBuilder msg = new StringBuilder();

            //判断是否开启没有条码号的的检验报告不能审核报告打印
            List<string> list_pat_id_no_bar = new List<string>();

            List<EntityPidReportMain> list = GetCheckedPatients();
            if (list.Count <= 0)
            {
                MessageDialog.Show("未选择" + LocalSetting.Current.Setting.ReportWord + "数据！", "提示");
                return;
            }

            foreach (var dr in list)
            {
                if (string.IsNullOrEmpty(dr.RepBarCode))
                {
                    list_pat_id_no_bar.Add(dr.RepId);
                }
                list_pat_id.Add(dr.RepId);
            }

            EntityOperationResultList result_message = new ProxyPidReportMainAudit().Service.BacAuditCheck(list_pat_id, "2");

            List<string> patlist = new List<string>();
            DialogResult drReturn = new DialogResult();

            if (result_message.FailedCount > 0)
            {
                //显示审核检查提示窗口
                AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(result_message, EnumOperationCode.Report);
                drReturn = resultviwer.ShowDialog();
                patlist = resultviwer.GetSelectedID();//获取需要继续操作的病人ID集合
            }
            else//全部检查通过
                patlist = list_pat_id;


            if ((result_message.FailedCount == 0) || (result_message.FailedCount > 0 && drReturn == DialogResult.OK))//点击了"继续"
            {
                AuditWord = "审核";
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - " + AuditWord, LIS_Const.BillPopedomCode.Audit, "", "");
                frmCheck.operationCode = EnumOperationCode.Report;


                DialogResult dig = DialogResult.Cancel;
                if (IsRecordLastOperationID
                    && IsRecordLastReportOperationPw
                    && strLastOperationID != string.Empty
                    && strLastOperationPw != string.Empty)
                {
                    bool flag = frmCheck.Valid(strLastOperationID, strLastOperationPw);
                    if (!flag)
                        dig = frmCheck.ShowDialog();
                    else
                        dig = DialogResult.OK;
                }
                else
                {
                    frmCheck.txtLoginid.Text = strLastOperationID;
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                    dig = frmCheck.ShowDialog();
                }

                if (dig == DialogResult.OK && patlist.Count > 0)
                {
                    if (!UserInfo.isAdmin)
                    {
                        bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(this.txtPatInstructment.valueMember, EnumOperationCode.Audit, frmCheck.OperatorID);
                        if (!ok)
                        {
                            lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                            return;
                        }
                    }


                    if (IsRecordLastOperationID)
                    {
                        strLastOperationID = frmCheck.OperatorID;
                    }

                    if (IsRecordLastReportOperationPw)
                    {
                        strLastOperationPw = frmCheck.PassWord;
                    }

                    //判断是否开启没有条码号的的检验报告不能审核报告打印
                    bool isNeedCheckNoBarcode = false;
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarcodeNeedAuditCheek") == "是")
                    {
                        string itrExList =
                            ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeAuditCheckItrExList");

                        if (string.IsNullOrEmpty(itrExList) || !itrExList.Contains(txtPatInstructment.valueMember))
                        {
                            isNeedCheckNoBarcode = true;
                        }

                    }

                    if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotAuditReportPrintOnNoBC") == "是" || isNeedCheckNoBarcode)
                    {
                        if (!new UILogic.PatEnterUILogic().canReportOnNoBarCode(frmCheck, isNeedCheckNoBarcode ? "NoBarcode_CanAudit" : "CanNotAuditReportPrintInNoBC"))
                        {
                            for (int i = patlist.Count - 1; i >= 0; i--)
                            {
                                if (list_pat_id_no_bar.Contains(patlist[i]))
                                {
                                    patlist.Remove(patlist[i]);
                                }
                            }
                            lis.client.control.MessageDialog.ShowAutoCloseDialog(string.Format("共有{1}条记录没有条码号，用户名：[{0}]无权报告没有条码号的报告！", frmCheck.OperatorID, 3));
                        }
                    }
                    if (patlist.Count <= 0)
                    {
                        return;
                    }
                    //*******************************************************************

                    EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                    caller.IPAddress = UserInfo.ip;
                    caller.UserID = frmCheck.Pat_i_code;

                    //报告(二审)时允许修改一审人
                    if (string.IsNullOrEmpty(frmCheck.Pat_i_code)
                    || ConfigHelper.GetSysConfigValueWithoutLogin("report_Allowedit_auditercode") != "是")
                    {
                        caller.UserID = string.Empty;
                    }

                    var resresult = new ProxyMicEnter().Service.MicReport(patlist, caller);

                    this.SaveCASignInfo(frmCheck, resresult);

                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
                        this.SelectAllPatient(false);
                    }
                    MessageDialog.ShowAutoCloseDialog("操作成功!");
                    if (UserInfo.GetSysConfigValue("PrintOnReport") == "是")
                        printPreview(patlist, true);
                    sy_OnBtnRefreshClicked(null, null);

                    #region 二审后焦点跳到下一行

                    //系统配置：二审后焦点跳到下一行
                    if (ConfigHelper.GetSysConfigValueWithoutLogin("report_isFocusOnTheNextRow") == "是")
                    {
                        if (gvPatList.RowCount > 0)
                        {
                            //当审核到最后一条的时候，不跳转到下一条
                            //如果没有下一个patID号,则跳到第一行
                            if (string.IsNullOrEmpty(nextPatID))
                            {
                                EntityPidReportMain dr = gvPatList.GetFocusedRow() as EntityPidReportMain;
                                if (dr != null)
                                    this.LocatePatientByPatID(dr.RepId, true);

                                //gvPatList.FocusedRowHandle = 0;
                                //EntityPidReportMain dr = gvPatList.GetRow(0) as EntityPidReportMain;
                                //if (dr != null)
                                //{
                                //    this.LocatePatientByPatID(dr.RepId, true);
                                //}
                            }
                            else
                            {
                                this.LocatePatientByPatID(nextPatID, true);
                            }
                        }
                    }

                    #endregion

                }
                else if (dig == DialogResult.No)
                {
                    lis.client.control.MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                    return;
                }
            }
        }

        /// <summary>
        /// 取消报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnUndoReportClicked(object sender, EventArgs e)
        {
            gvPatList.CloseEditor();
            this.bsPat.EndEdit();

            List<EntityPidReportMain> list = GetCheckedPatients();
            if (list == null || list.Count <= 0)
            {
                lis.client.control.MessageDialog.Show("未选择" + LocalSetting.Current.Setting.ReportWord + "数据！", "提示");
                return;
            }

            String pat_report_date = ServerDateTime.GetServerDateTime().ToLongTimeString();

            bool CancelPrintReportPower = UserInfo.GetSysConfigValue("Audit_Second_CancelPrintPower") == "是";

            string powerName = CancelPrintReportPower ? "FrmPatEnter_CancelPrintReportPower" : string.Empty;


            List<string> idList = new List<string>();
            List<string> onlyReportidList = new List<string>();

            foreach (var dr in list)
            {
                if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported
                    || dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed)
                {
                    idList.Add(dr.RepId);
                }
                if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported)
                {
                    onlyReportidList.Add(dr.RepId);
                }
            }

            if (idList.Count < 1)
            {
                lis.client.control.MessageDialog.Show("所选记录未" + LocalSetting.Current.Setting.ReportWord + "！", "提示");
                return;
            }


            #region 取消报告前检查
            List<string> patlist = new List<string>();
            if (idList.Count > 0)
            {
                //取消报告前检查
                EntityOperationResultList result_message = new ProxyPidReportMainAudit().Service.BatchUndoReportCheck(idList, "UndoReport");

                if (result_message.FailedCount > 0)
                {

                    //显示审核检查提示窗口
                    AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(result_message, EnumOperationCode.UndoReport);
                    DialogResult drReturn = new DialogResult();
                    drReturn = resultviwer.ShowDialog();
                    patlist = resultviwer.GetSelectedID();//获取需要继续操作的病人ID集合

                    //筛选可以继续操作的病人ID
                    if (patlist != null && patlist.Count > 0)
                    {
                    }
                    else
                    {
                        return;
                    }
                }
                else//全部检查通过
                {
                    patlist = idList;
                }
            }
            #endregion

            string undoReportRemark = string.Empty;
            if (UserInfo.GetSysConfigValue("Lab_UndoReportRemark") == "是")
            {
                FrmUndoRepotReson frmReson = new FrmUndoRepotReson();
                if (frmReson.ShowDialog() == DialogResult.OK)
                {
                    undoReportRemark = frmReson.strRemark;
                }
                else
                    return;
            }

            FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 取消" + LocalSetting.Current.Setting.ReportWord, LIS_Const.BillPopedomCode.UndoReport, "", "", powerName);
            frmCheck.operationCode = EnumOperationCode.UndoReport;
            DialogResult dig = frmCheck.ShowDialog();
            if (dig == DialogResult.OK)
            {

                if (CancelPrintReportPower && !frmCheck.Power)
                {
                    if (onlyReportidList.Count == 0)
                    {
                        MessageDialog.Show("您没有权限反审已打印的标本！");
                        return;
                    }
                    if (onlyReportidList.Count < idList.Count)
                    {
                        MessageDialog.Show("您没有权限反审已打印的标本！将过滤掉已打印过的标本！");
                    }
                    idList = onlyReportidList;
                }


                EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                caller.IPAddress = UserInfo.ip;
                caller.UserID = frmCheck.Pat_i_code;

                var ret = new ProxyMicEnter().Service.UndoMicReport(patlist, caller);

                this.SaveCASignInfo(frmCheck, ret);
                bool blnUnAudit = UserInfo.GetSysConfigValue("OneStepCancelReport") == "是" ? true : false;

                StringBuilder msg = new StringBuilder();
                for (int i = 0; i < this.gvPatList.RowCount; i++)
                {
                    EntityPidReportMain dr = this.gvPatList.GetRow(i) as EntityPidReportMain;
                    if (dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Reported
                        || dr.RepStatus.ToString() == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        if (blnUnAudit)
                        {
                            dr.RepStatus = 0;
                            dr.RepStatusName = "未" + AuditWord;
                            dr.RepAuditUserId = "";
                            dr.RepAuditUserName = "";
                        }
                        else
                        {
                            dr.RepStatus = 1;
                            dr.RepStatusName = "已" + AuditWord;
                        }

                        dr.RepReportUserId = "";
                        dr.RepReportUserName = "";

                    }
                }

                if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                {
                    this.SelectAllPatient(false);
                }

                MessageDialog.ShowAutoCloseDialog("操作成功!");
            }
            else if (dig == DialogResult.No)
            {
                lis.client.control.MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
            }
        }

        /// <summary>
        /// 报告并打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnBtnSinglePrintClicked(object sender, EventArgs e)
        {
            gvPatList.CloseEditor();
            this.bsPat.EndEdit();

            List<EntityPidReportMain> list = this.bsPatLst.DataSource as List<EntityPidReportMain>;
            if (list == null || list.Count == 0) return;

            String pat_chk_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            List<string> list_pat_id = new List<string>();

            List<EntityPidReportMain> patLis = GetCheckedPatients();
            if (patLis == null || patLis.Count <= 0)
            {
                MessageDialog.Show("未选择" + LocalSetting.Current.Setting.ReportWord + "数据！", "提示");
                return;
            }

            foreach (var dr in patLis)
            {
                list_pat_id.Add(dr.RepId.ToString());
            }

            EntityOperationResultList result_message = proxy.Service.BacAuditCheck(list_pat_id, "2");
            List<string> patlist = new List<string>();
            DialogResult drReturn = new DialogResult();

            if (result_message.FailedCount > 0)
            {
                //显示审核检查提示窗口
                AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(result_message, EnumOperationCode.Report);
                drReturn = resultviwer.ShowDialog();
                patlist = resultviwer.GetSelectedID();//获取需要继续操作的病人ID集合
            }
            else//全部检查通过
                patlist = list_pat_id;

            if ((result_message.FailedCount == 0) || (result_message.FailedCount > 0 && drReturn == DialogResult.OK))//点击了"继续"
            {
                AuditWord = "报告";
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - " + AuditWord, LIS_Const.BillPopedomCode.Audit, "", "");
                frmCheck.operationCode = EnumOperationCode.Report;

                //验证窗口
                DialogResult dig = frmCheck.ShowDialog();
                if (dig == DialogResult.OK && patlist.Count > 0)
                {
                    if (!UserInfo.isAdmin)
                    {
                        bool ok = new ProxyPidReportMainAudit().Service.CheckCurUserCanAudit(this.txtPatInstructment.valueMember, EnumOperationCode.Audit, frmCheck.OperatorID);
                        if (!ok)
                        {
                            lis.client.control.MessageDialog.Show("没有权限对当前仪器的报告单进行此操作！", "提示");
                            return;
                        }
                    }
                    EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
                    Caller.IPAddress = UserInfo.ip;
                    Caller.LoginID = frmCheck.OperatorID;
                    Caller.LoginName = frmCheck.OperatorName;
                    Caller.UserID = frmCheck.Pat_i_code;

                    //报告(二审)时允许修改一审人
                    if (string.IsNullOrEmpty(frmCheck.Pat_i_code)
                    || ConfigHelper.GetSysConfigValueWithoutLogin("report_Allowedit_auditercode") != "是")
                    {
                        Caller.UserID = string.Empty;
                    }

                    var result = new ProxyMicEnter().Service.MicReport(patlist, Caller);

                    this.SaveCASignInfo(frmCheck, result);
                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
                        this.SelectAllPatient(false);
                    }
                    MessageDialog.ShowAutoCloseDialog("审核成功，正在打印......", 2m);
                    printPreview(patlist, true);
                    sy_OnBtnRefreshClicked(null, null);
                }
                else if (dig == DialogResult.No)
                {
                    MessageDialog.Show("身份验证失败，不能进行当前操作！", "提示");
                }
            }

        }

        /// <summary>
        /// 快速登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SysToolBar1_OnBtnPageUpClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.valueMember))
            {
                lis.client.control.MessageDialog.Show("请选择仪器", "提示");
                return;
            }

            FrmSampleRegisterNew frmRegister = new FrmSampleRegisterNew(true, txtPatInstructment.valueMember);
            frmRegister.FormClosed += FrmRegister_FormClosed;
            frmRegister.ShowDialog();
        }

        private void FrmRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            sy_OnBtnRefreshClicked(null, null);
        }

        /// <summary>
        /// 资料复制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_BtnCopyClick(object sender, EventArgs e)
        {
            gvPatList.CloseEditor();
            this.bsPat.EndEdit();
            List<EntityPidReportMain> list = this.bsPatLst.DataSource as List<EntityPidReportMain>;
            if (list == null || list.Count == 0)
                return;

            //获取勾选的病人信息
            List<EntityPidReportMain> listPats = this.GetCheckedPatients();

            if (listPats.Count > 0)
            {
                FrmPatInfoCopy frmCopy = new FrmPatInfoCopy(listPats);
                frmCopy.ShowDialog();
            }
            else
                lis.client.control.MessageDialog.Show("请勾选要复制的数据");
        }


        /// <summary>
        /// 药敏结果导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sy_OnBtnExportClicked(object sender, EventArgs e)
        {
            List<EntityPidReportMain> listRepMain = GetCheckedPatients();

            var tbXml = new ProxyMicEnter().Service.GetAntiResult(listRepMain.Select(o => o.RepId).ToList());

            byte[] byteArray = Encoding.UTF8.GetBytes(tbXml);

            MemoryStream stream = new MemoryStream(byteArray);

            DataSet ds = new DataSet();

            ds.ReadXml(stream);

            GridControl control = new GridControl();
            GridView view = new GridView();
            control.ViewCollection.Add(view);
            control.MainView = view;
            foreach (DataColumn tCol in ds.Tables[0].Columns)
            {
                GridColumn gCol = new GridColumn();
                gCol.Name = "col" + tCol.ColumnName;
                gCol.FieldName = tCol.ColumnName;
                gCol.UnboundType = DevExpress.Data.UnboundColumnType.Bound;
                view.Columns.Add(gCol);
                gCol.Visible = true;
            }
            control.DataSource = ds.Tables[0];
            control.Visible = false;

            this.groupControl1.Controls.Add(control);
            control.ForceInitialize();

            SaveFileDialog open = new SaveFileDialog();
            open.Filter = "(*.xlsx)|*.xlsx";
            open.RestoreDirectory = true;
            if (open.ShowDialog() == DialogResult.OK)
            {
                view.ExportToXlsx(open.FileName);
            }
            this.groupControl1.Controls.Remove(control);
        }

        private void sy_BtnDeRefClick(object sender, EventArgs e)
        {
            try
            {
                MessageDialog.ShowAutoCloseDialog("特殊功能,,暂时不开放");
                return;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void sy_BtnUndoClick(object sender, EventArgs e)
        {
            sy_OnUndoReportClicked(sender, e);
        }
        private void sy_BtnUndo2Click(object sender, EventArgs e)
        {
            sy_OnUndoAuditClicked(sender, e);
        }
        public bool LocatePatientByPatID(string pat_id, bool autoSelect)
        {
            //this.gvPatList.SelectAll();
            bool founded = false;
            for (int i = 0; i < this.gvPatList.RowCount; i++)
            {
                EntityPidReportMain dr = this.gvPatList.GetRow(i) as EntityPidReportMain;

                if (!string.IsNullOrEmpty(dr.RepId))
                {
                    if (pat_id == dr.RepId)
                    {
                        if (autoSelect)
                        {
                            this.gvPatList.FocusedRowHandle = i;
                            getIndexValue();
                        }
                        founded = true;
                    }
                    //else
                    //{
                    //    this.gvPatList.UnselectRow(i);
                    //}
                }
            }
            return founded;
        }

        /// <summary>
        /// 选择/取消选择所有病人
        /// </summary>
        /// <param name="selectAll"></param>
        public void SelectAllPatient(bool selectAll)
        {
            //foreach (var item in this.bsPatLst)
            //{
            //    (item as EntityPidReportMain).PatSelect = selectAll;
            //}
            if (selectAll)
                gvPatList.SelectAll();
            else
                gvPatList.ClearSelection();
            gcPatList.RefreshDataSource();

        }

        private void sy_OnBtnPageUpClicked(object sender, EventArgs e)
        {
            this.gvPatList.MovePrev();
        }

        private void sy_OnBtnPageDownClicked(object sender, EventArgs e)
        {
            this.gvPatList.MoveNext();
        }

        private void sy_OnResultViewClicked(object sender, EventArgs e)
        {
            if (this.txtPatDate.EditValue == null)
            {
                lis.client.control.MessageDialog.Show("请输入时间！", "提示");
                return;
            }
            if (this.txtPatInstructment.valueMember == null)
            {
                lis.client.control.MessageDialog.Show("请输入仪器", "提示");
                return;
            }
            FrmView fv = new FrmView(this.txtPatDate.EditValue.ToString(), this.txtPatInstructment.valueMember);
            fv.ShowDialog();
        }

        /// <summary>
        /// 选择抗生素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_BtnAntibioticsClick(object sender, EventArgs e)
        {
            FrmDict_An_Sstd fd = new FrmDict_An_Sstd(this);
            fd.ShowDialog();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnBtnPrintClicked(object sender, EventArgs e)
        {
            printPreview(true);
        }

        private void sy_OnPrintPreviewClicked(object sender, EventArgs e)
        {
            printPreview(false);
        }

        private void sy_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnBtnAddClicked(object sender, EventArgs e)
        {
            addNew();
            this.txtPatID.Focus();
            isDataChaged = false;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sy_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            string type = this.labelControl2.Text;
            GetPatientList(false);//刷新病人列表
            if (bsPatLst.Current != null && type != "新增")//是编辑状态刷新列表框
            {
                this.labelControl2.Text = "编辑";
                EntityPidReportMain drLst = (EntityPidReportMain)bsPatLst.Current;
                this.GetPatDetailData(drLst.RepId);
                this.FillUiFromEntity();
                isDataChaged = false;
            }
        }

        private void sy_BtnBrowseClick(object sender, EventArgs e)
        {

            if (bsPatLst.DataSource != null
                && gvPatList.GetFocusedRow() != null)
            {
                EntityPidReportMain drtemp = gvPatList.GetFocusedRow() as EntityPidReportMain;

                if (drtemp.PidSrcId == "108")
                {
                    if (drtemp.PidInNo.Length <= 0)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("当前没有住院号信息不能病历浏览");
                        return;
                    }

                    string temp_url = @"http://172.17.250.10:82/show.aspx?EncryptImf=strPatientCode&ZYHM=strPatientCode";

                    temp_url = temp_url.Replace("strPatientCode", drtemp.PidInNo);

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

        /// <summary>
        /// 点击面板设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPanelConfig_Click(object sender, EventArgs e)
        {
            FrmPatPanelConfig frm = new FrmPatPanelConfig(this);
            frm.Show();
        }

        string Lab_PatientsFocusColor = UserInfo.GetSysConfigValue("Lab_PatientsFocusColor");

        #region IPatPanelConfig 成员

        public void ApplySetting(PatInputRuntimeSetting setting)
        {
            this.UserCustomSetting = setting;
            ApplySetting();
        }

        public void ApplySetting()
        {
            PatInputRuntimeSetting setting = this.UserCustomSetting;

            #region 左侧式样
            setting.PatListPanel.GridColSetting.DefaultView.Sort = "VisibleIndex";

            foreach (DataRow dr in setting.PatListPanel.GridColSetting.Rows)
            {
                GridColumn col = this.gvPatList.Columns[dr["FieldName"].ToString()];
                this.gvPatList.Columns.Remove(col);
            }

            DataTable dtSearchBarDDL = new DataTable();
            dtSearchBarDDL.Columns.Add("name");
            dtSearchBarDDL.Columns.Add("value");

            DataRow drSearchBarDDL = dtSearchBarDDL.NewRow();

            drSearchBarDDL["name"] = string.Empty;
            drSearchBarDDL["value"] = string.Empty;
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "样本号";
            drSearchBarDDL["value"] = "pat_sid";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "样本号(模糊)";
            drSearchBarDDL["value"] = "pat_sid_like";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);


            drSearchBarDDL = dtSearchBarDDL.NewRow();
            drSearchBarDDL["name"] = "条码号";
            drSearchBarDDL["value"] = "pat_bar_code";
            dtSearchBarDDL.Rows.Add(drSearchBarDDL);

            foreach (DataRow dr in setting.PatListPanel.GridColSetting.Rows)
            {
                bool visible = Convert.ToBoolean(dr["Visible"]);

                if (visible)
                {
                    GridColumn col = new GridColumn();
                    col.Name = "col_" + dr["FieldName"].ToString();
                    col.Width = Convert.ToInt32(dr["ColumnWidth"]);
                    col.FieldName = dr["FieldName"].ToString();
                    col.Caption = dr["HeaderText"].ToString();
                    col.OptionsColumn.AllowEdit = false;
                    col.OptionsColumn.AllowMove = false;
                    col.OptionsFilter.AllowFilter = false;
                    col.Visible = true;
                    col.VisibleIndex = 3 + (int)dr["VisibleIndex"];
                    this.gvPatList.Columns.Add(col);

                    if (dr["HeaderText"].ToString() == "序号")
                    {

                    }

                    drSearchBarDDL = dtSearchBarDDL.NewRow();
                    drSearchBarDDL["name"] = dr["HeaderText"].ToString();
                    drSearchBarDDL["value"] = dr["FieldName"].ToString();

                    dtSearchBarDDL.Rows.Add(drSearchBarDDL);

                    if (col.FieldName == "pat_sex_name")
                    {
                        col.Width = 36;
                    }

                    if (dr["FieldName"].ToString() == "pat_check_name")
                    {
                        col.Caption = LocalSetting.Current.Setting.AuditWord + "人";
                    }

                    if (dr["FieldName"].ToString() == "pat_report_name")
                    {
                        col.Caption = LocalSetting.Current.Setting.ReportWord + "人";
                    }

                    if (dr["FieldName"].ToString() == "pat_in_no")
                    {
                        if (!string.IsNullOrEmpty(PatientIDNameConfirm) && PatientIDNameConfirm != "不统一设置")
                        {
                            col.Caption = PatientIDNameConfirm;
                        }
                    }

                }
            }

            this.cmbBarSearchPatType.Properties.DropDownRows = dtSearchBarDDL.Rows.Count;
            this.cmbBarSearchPatType.Properties.DataSource = dtSearchBarDDL;
            this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            this.cmbBarSearchPatType.Properties.ValueMember = "value";

            //已审核
            GridFormatRule rule = new GridFormatRule();
            rule.Column = gvPatList.Columns["RepStatus"];
            rule.ColumnApplyTo = gvPatList.Columns["PidName"];
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = LIS_Const.PATIENT_FLAG.Audited;
            if (setting.PatListPanel.BackColorAudited != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = setting.PatListPanel.BackColorAudited;
            }
            formatConditionRuleValue1.Appearance.ForeColor = setting.PatListPanel.ForeColorAudited;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gvPatList.FormatRules.Add(rule);

            //已中期报告
            rule = new GridFormatRule();
            rule.Column = gvPatList.Columns["MicReportFlag"];
            rule.ColumnApplyTo = gvPatList.Columns["RepSid"];
            formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = LIS_Const.MicReportFlag.Yes;
            if (setting.PatListPanel.BackColorPreReported != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = setting.PatListPanel.BackColorPreReported;
            }
            formatConditionRuleValue1.Appearance.ForeColor = setting.PatListPanel.ForeColorPreReported;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gvPatList.FormatRules.Add(rule);

            //已报告
            rule = new GridFormatRule();
            rule.Column = gvPatList.Columns["RepStatus"];
            rule.ColumnApplyTo = gvPatList.Columns["PidName"];
            formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = LIS_Const.PATIENT_FLAG.Reported;
            if (setting.PatListPanel.BackColorReported != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = setting.PatListPanel.BackColorReported;
            }
            formatConditionRuleValue1.Appearance.ForeColor = setting.PatListPanel.ForeColorReported;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gvPatList.FormatRules.Add(rule);

            //已打印
            rule = new GridFormatRule();
            rule.Column = gvPatList.Columns["RepStatus"];
            rule.ColumnApplyTo = gvPatList.Columns["PidName"];
            formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Equal;
            formatConditionRuleValue1.Value1 = LIS_Const.PATIENT_FLAG.Printed;
            if (setting.PatListPanel.BackColorPrinted != Color.Transparent)
            {
                formatConditionRuleValue1.Appearance.BackColor = setting.PatListPanel.BackColorPrinted;
            }
            formatConditionRuleValue1.Appearance.ForeColor = setting.PatListPanel.ForeColorPrinted;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            rule.Rule = formatConditionRuleValue1;
            gvPatList.FormatRules.Add(rule);

            #endregion

            #region 中间式样
            bool bAllowCustomizePanel = (UserInfo.GetSysConfigValue(LIS_Const.SystemConfigurationCode.AllowCustomizePanel) == "是");

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
        }

        #endregion

        private void txtBarSearchCondition_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            RapitSearch();
        }

        private void txtBarSearchCondition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                RapitSearch();
            }
        }

        private void RapitSearch()
        {
            List<EntityPidReportMain> PatList = AllPatList;
            if (AllPatList.Count == 0) return;

            if (this.cmbBarSearchPatType.EditValue != null
                && this.cmbBarSearchPatType.EditValue.ToString().Trim(null) != string.Empty
                && this.txtBarSearchCondition.Text.Trim(null) != string.Empty)
            {
                string searchField = this.cmbBarSearchPatType.EditValue.ToString();
                string searchValue = this.txtBarSearchCondition.Text;

                if (searchField == "pat_sid")
                {
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
                                    PatList = AllPatList.Where(i => i.PatSidInt >= sidFrom && i.PatSidInt <= sidTo).ToList();
                                }
                                else
                                {
                                    PatList = AllPatList.Where(i => i.PatSidInt < 0).ToList();
                                }
                            }
                            else
                            {
                                string sid2 = sids.Trim(null);

                                Int64 i = -1;

                                Int64.TryParse(sid2, out i);

                                PatList = AllPatList.Where(r => r.PatSidInt == i).ToList();
                            }
                        }
                    }
                    else
                    {
                        PatList = new List<EntityPidReportMain>();
                    }
                }
                //条码号
                else if (searchField == "pat_bar_code")
                {
                    PatList = PatList.Where(i => i.RepBarCode.Contains(searchValue)).ToList();
                }
                else if (searchField == "pat_sid_like")
                {
                    PatList = AllPatList.Where(i => i.RepSid.Contains(SQLFormater.Format(searchValue))).ToList();
                }
                else if (searchField == "pat_sex_name")
                {
                    if (searchValue == "1")
                    {
                        searchValue = "男";
                    }
                    else if (searchValue == "2")
                    {
                        searchValue = "女";
                    }
                    PatList = AllPatList.Where(i => i.PidSexName.Contains(searchValue)).ToList();
                }
                else
                {
                    //姓名
                    if (searchField == "PidName")
                    {
                        PatList = AllPatList.Where(i => i.PidName.Contains(searchValue)).ToList();
                    }
                    //检验组合
                    else if (searchField == "PidComName")
                    {
                        PatList = AllPatList.Where(i => i.PidComName.Contains(searchValue)).ToList();
                    }
                    //年龄
                    else if (searchField == "PidAgeExp")
                    {
                        PatList = AllPatList.Where(i => i.PidAgeExp.Contains(searchValue)).ToList();
                    }
                    //状态
                    else if (searchField == "RepStatusName")
                    {
                        PatList = AllPatList.Where(i => i.RepStatusName.Contains(searchValue)).ToList();
                    }
                    //病人ID
                    else if (searchField == "PidInNo")
                    {
                        PatList = AllPatList.Where(i => i.PidInNo.Contains(searchValue)).ToList();
                    }
                    //标本
                    else if (searchField == "SamName")
                    {
                        PatList = AllPatList.Where(i => i.SamName.Contains(searchValue)).ToList();
                    }
                    //病床号
                    else if (searchField == "PidDeptName")
                    {
                        PatList = AllPatList.Where(i => i.PidDeptName.Contains(searchValue)).ToList();
                    }
                    //科室
                    else if (searchField == "PidDeptName")
                    {
                        PatList = AllPatList.Where(i => i.PidDeptName.Contains(searchValue)).ToList();
                    }
                    //录入人
                    else if (searchField == "LrName")
                    {
                        PatList = AllPatList.Where(i => i.LrName.Contains(searchValue)).ToList();
                    }
                    //审核人
                    else if (searchField == "PidChkName")
                    {
                        PatList = AllPatList.Where(i => i.PidChkName.Contains(searchValue)).ToList();
                    }
                    //报告人
                    else if (searchField == "BgName")
                    {
                        PatList = AllPatList.Where(i => i.BgName.Contains(searchValue)).ToList();
                    }
                    //查看人
                    else if (searchField == "PatLookName")
                    {
                        PatList = AllPatList.Where(i => i.PatLookName.Contains(searchValue)).ToList();
                    }
                    //查看时间
                    else if (searchField == "RepReadDate")
                    {
                        PatList = AllPatList.Where(i => i.RepReadDate.ToString().Contains(searchValue)).ToList();
                    }
                    //接收时间
                    else if (searchField == "SampApplyDate")
                    {
                        PatList = AllPatList.Where(i => i.LrName.Contains(searchValue)).ToList();
                    }
                    //仪器代码
                    else if (searchField == "ItrEname")
                    {
                        PatList = AllPatList.Where(i => i.ItrEname.Contains(searchValue)).ToList();
                    }
                }
            }
            else
            {
                PatList = AllPatList;
            }

            if (roundPanelResult.GetCurRoundPanel().Tag?.ToString() == "0")
            {
            }
            else if (roundPanelResult.GetCurRoundPanel().Tag?.ToString() == "1")
            {
                PatList = PatList.Where(i => i.HasResult == "1").ToList();
            }
            else if (roundPanelResult.GetCurRoundPanel().Tag?.ToString() == "2")
            {
                PatList = PatList.Where(i => i.HasResult == "0").ToList();
            }
            else if (roundPanelResult.GetCurRoundPanel().Tag?.ToString() == "3")
            {
                PatList = PatList.Where(i => !string.IsNullOrEmpty(i.RepReadUserId)).ToList();
            }
            bsPatLst.DataSource = PatList;
            RefreshItemsCount();//更新记录数量信息
            bsPatLst_PositionChanged(null, null);
        }

        /// <summary>
        /// 选择菌类后更新数据,以保证选择菌类bt_id不为空_放到EditValueChanged中会导致值丢失
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemLookUpEdit1_Leave(object sender, EventArgs e)
        {
            bsBtype.EndEdit();
            bsbacteri.EndEdit();
        }

        /// <summary>
        /// 设置菌类数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemLookUpEdit2_Enter(object sender, EventArgs e)
        {
            EntityObrResultBact dvr = (EntityObrResultBact)(gvBac.GetFocusedRow());
            LookUpEdit thisEdit = (LookUpEdit)sender;

            string filter = dvr.BtypeId;
            List<EntityDicMicBacteria> newList = new List<EntityDicMicBacteria>();
            if (string.IsNullOrEmpty(filter))
            {
                newList = BacteriList;
            }
            else
            {
                newList = BacteriList.FindAll(a => a.BacBtId == filter);
            }
            thisEdit.Properties.DataSource = newList;
        }


        /// <summary>
        /// 删除抗生素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmDeleAnti_Click(object sender, EventArgs e)
        {
            List<EntityObrResultAnti> dt_Bac = (List<EntityObrResultAnti>)this.bsAnti.DataSource;
            if (bsAnti.Current != null)
            {
                EntityObrResultAnti drBac = (EntityObrResultAnti)bsAnti.Current;
                dt_Bac.Remove(drBac);
                gcAnti.RefreshDataSource();
            }
        }

        /// <summary>
        /// 病人ID改变时，带出原来的资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPatID_Leave(object sender, EventArgs e)
        {
            //LoadPatInfoByID();
        }

        private void txtPatID_EnterKeyDown(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.valueMember))
            {
                MessageDialog.Show("请选择仪器", "提示");
                return;
            }

            string typeid;
            bool success = LoadPatInfoByID(out typeid, this.txtPatID.Text);

            EntityDicPubIdent drNoType = DictPatNumberType.Instance.GetNoType(typeid);

            string notype = string.Empty;

            if (drNoType != null)
            {
                notype = drNoType.IdtInterfaceType;
            }

            if (success)
            {
                if ((notype.ToLower() == "barcode" && UserInfo.GetSysConfigValue("BarcodeAutoSave") == "是")
                    || (notype.ToLower() == "interface" && UserInfo.GetSysConfigValue("Lab_PatientsAutoSave") == "是"))
                {
                    SaveOrUpdate();
                    sy_OnBtnRefreshClicked(null, null);
                    if (!UserCustomSetting.PatResultPanel.SavePatInfoNoNext)
                        addNew();
                    this.ActiveControl = this.txtPatID;
                }
            }
            else
            {
                //addNew();
            }
        }

        InterfacePatientInfo PatInfo = null;

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
                    EntityDicPubIdent drNoType = DictPatNumberType.Instance.GetNoType(idtype);

                    if (drNoType != null
                        && !Compare.IsNullOrDBNull(drNoType.IdtInterfaceType)
                        && drNoType.IdtInterfaceType.Trim() != string.Empty)
                    {

                        //接口类型
                        string notype = string.Empty;

                        //接口代码
                        string nocode = string.Empty;

                        if (drNoType != null)
                        {
                            notype = drNoType.IdtInterfaceType;
                            nocode = drNoType.IdtCode;
                        }

                        //------------ 2010-5-10 
                        NetInterfaceType type = (NetInterfaceType)Enum.Parse(typeof(NetInterfaceType), drNoType.IdtInterfaceType);
                        string interfaceID = string.Empty;

                        if (!Compare.IsNullOrDBNull(drNoType.IdtInterfaceId) && drNoType.IdtInterfaceId.Trim() != string.Empty)
                        {
                            interfaceID = drNoType.IdtInterfaceId;
                        }

                        PatientInterfaceInfo patInterface = new PatientInterfaceInfo(nocode.Trim().ToLower(), this.txtPatID.Text, interfaceID, type);
                        string interfaceFrom = "通用";
                        interfaceFrom = UserInfo.GetSysConfigValue("GetPatientsInfoType").ToLower().Trim();
                        IPatients patientsInterface = null;
                        if (interfaceFrom == "通用")
                            patientsInterface = new NormalPatients();

                        else if (interfaceFrom == "outlink")
                            patientsInterface = new OutlinkPatinets();

                        EntityInterfaceExtParameter parameter = new EntityInterfaceExtParameter();
                        parameter.PatientID = this.txtPatID.Text;
                        if (drNoType.IdtId == "106")
                        {
                            parameter.DownloadType = InterfaceType.ZYPatient;
                        }
                        else if (drNoType.IdtId == "107")
                        {
                            parameter.DownloadType = InterfaceType.MZPatient;
                        }
                        else if (drNoType.IdtId == "110")
                        {
                            parameter.DownloadType = InterfaceType.TJPatient;
                        }
                        else if (drNoType.IdtId == "111")
                        {
                            parameter.DownloadType = InterfaceType.BarcodePatient;
                        }
                        ProxyPidReportMainInterface proxy = new ProxyPidReportMainInterface();
                        EntityPidReportMain PatInfo = proxy.Service.GetPatientFromInterface(parameter);

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
                                {               /**********peng:提示信息增加操作者和时间*******************************************/
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

                                    if (PatInfo.PidSrcId == "109" && UserInfo.GetSysConfigValue("Barcode_TJShouldReceiveConfirm") == "是")//住院
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
                                                ceCombine.AddCombine(com_selected.ComId, com_selected.OrderSn, Convert.ToDecimal(com_selected.OrderPrice), com_selected.RepBarCode);
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
                                        if (ceCombine.listRepDetail != null)
                                        {
                                            ceCombine.listRepDetail.Clear();
                                        }
                                        ceCombine.RefreshEditBoxText();

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
                                                ceCombine.AddCombine(com_selected.ComId, com_selected.OrderSn, Convert.ToDecimal(com_selected.OrderPrice), com_selected.RepBarCode);
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
                                                if (lis.client.control.MessageDialog.Show(mes, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                                {
                                                    foreach (EntityPidReportDetail com_selected in combineCanRegistedInThisItrReged)
                                                    {
                                                        ceCombine.AddCombine(com_selected.ComId, com_selected.OrderSn, Convert.ToDecimal(com_selected.OrderPrice), com_selected.RepBarCode);
                                                    }
                                                    ret = true;
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
                                //病人来源
                                string ori_id = DictPatNumberType.Instance.GetOriID_byNoType(this.txtPatIdType.valueMember);
                                string loginID = UserInfo.loginID;
                                string loginName = UserInfo.userName;

                                #region 院网接口调组合  2010-8-3
                                if (PatInfo.ListPidReportDetail != null && PatInfo.ListPidReportDetail.Count > 0)
                                {
                                    if (ceCombine.listRepDetail != null)
                                        ceCombine.listRepDetail.Clear();
                                    ceCombine.RefreshEditBoxText();
                                    List<EntityPidReportDetail> hisCodeNotExists = new List<EntityPidReportDetail>();

                                    List<EntityPidReportDetail> comIds = GetComIdWithHISCode(PatInfo.ListPidReportDetail, ref hisCodeNotExists, ori_id);
                                    string message = CheckComIdFromPatinetMi(hisCodeNotExists);
                                    if (!string.IsNullOrEmpty(message))
                                    {
                                        MessageDialog.Show(message, "提示");
                                    }

                                    if (comIds != null)
                                    {
                                        if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("GetHISCombinePattern"))
                                               && UserInfo.GetSysConfigValue("GetHISCombinePattern") != "简单")//取组合完整模式，弹出框确认// 2010-8-24
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
                                                this.txtPatDoc.SelectByValue(comIds[0].PidDoctorCode);
                                            }
                                            else if (!string.IsNullOrEmpty(comIds[0].PidDoctorName))
                                            {
                                                this.txtPatDoc.SelectByDispaly(comIds[0].PidDoctorName);
                                            }
                                            else
                                            {
                                                txtPatDoc.valueMember = null;
                                                txtPatDoc.displayMember = null;
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
                                            List<EntityPidReportDetail> combineCanRegisted = new List<EntityPidReportDetail>();

                                            //不能登记的组合
                                            List<EntityPidReportDetail> combineCannotRegisted = new List<EntityPidReportDetail>();

                                            ret = RegisterCombine(comIds, out combineCanRegisted, out combineCannotRegisted);
                                        }

                                        //根据组合的默认标本设置标本
                                        if (comIds.Count > 0 && string.IsNullOrEmpty(txtPatSampleType.valueMember))
                                        {
                                            string comSampleID = "";
                                            foreach (EntityPidReportDetail comID in comIds)
                                            {
                                                EntityDicCombine combineRow = DictCombine.Instance.GetCombinebyID(comID.ComId);
                                                if (string.IsNullOrEmpty(comSampleID))
                                                    comSampleID = combineRow.ComSamId;
                                            }
                                            if (!string.IsNullOrEmpty(comSampleID))
                                                txtPatSampleType.SelectByID(comSampleID);
                                        }
                                    }
                                }
                                else
                                {
                                    ret = true;
                                }

                                #endregion

                                if (txtPatSampleDate.EditValue == null)
                                    txtPatSampleDate.EditValue = txtPatRecDate.EditValue;

                                if (txtPatSDate.EditValue == null)
                                    txtPatSDate.EditValue = txtPatRecDate.EditValue;

                                if (txtPatReachDate.EditValue == null)
                                    txtPatReachDate.EditValue = txtPatRecDate.EditValue;

                                if (txtPatApplyDate.EditValue == null)
                                    txtPatApplyDate.EditValue = txtPatRecDate.EditValue;
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
                    ceCombine.AddCombine(com_id);
                    listCombineCanRegister.Add(comId);
                }
                else
                {
                    if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("GetHISCombinePattern"))
                                             && UserInfo.GetSysConfigValue("GetHISCombinePattern") != "简单")//取组合完整模式，弹出框确认
                    {
                        if (currentItrComIDs.Contains(com_id))//当前仪器包含此组合 // 2010-8-24
                        {
                            ceCombine.AddCombine(com_id);
                            listCombineCanRegister.Add(comId);
                        }
                        else//不包含
                        {
                            listCombineCannotRegister.Add(comId);
                        }
                    }
                    else//取组合简单模式，不弹出框，有则显示组合，没有则提示
                    {
                        ceCombine.AddCombine(com_id);
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

        #region 院网接口调组合  2010-8-3
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

        #endregion

        /// <summary>
        /// 填充数据到控件
        /// </summary>
        /// <param name="drSourceDataRow"></param>
        private void FillInterfacePatToControl(EntityPidReportMain PatInfo)
        {

            NewPatientForSave = PatInfo;

            //姓名
            this.txtPatName.EditValue = PatInfo.PidName;

            //性别
            this.txtPatSex.valueMember = PatInfo.PidSex;
            this.txtPatSex.displayMember = PatInfo.PidSexExp;

            //年龄
            this.textAgeInput1.AgeValueText = PatInfo.PidAgeExp;

            //标本类别
            if (!string.IsNullOrEmpty(PatInfo.PidSamId))
            {
                this.txtPatSampleType.SelectByID(PatInfo.PidSamId);
            }
            else
            {
                //佛山市一都去取标本，没有就返回空；其他医院不同
                if (UserInfo.GetSysConfigValue("GetPatientsInfoType") == "outlink") //佛山市一
                    this.txtPatSampleType.SelectByDispaly(PatInfo.SamName); //不自动记忆标本 2010-8-24 
                else //其他医院
                    if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSampleType"))
                {
                    this.txtPatSampleType.SelectByDispaly(PatInfo.SamName);
                }
            }

            //床号
            this.txtPatBedNo.EditValue = PatInfo.PidBedNo;

            #region 病人辅助信息
            //职业
            this.fpat_work.EditValue = PatInfo.PidWork;

            //联系电话
            this.fpat_tel.EditValue = PatInfo.PidTel;

            //联系邮件
            this.fpat_email.EditValue = PatInfo.PidEmail;

            //工作单位
            this.fpat_unit.EditValue = PatInfo.PidUnit;

            //联系地址
            this.fpat_address.EditValue = PatInfo.PidAddress;

            //身高
            this.fpat_height.EditValue = PatInfo.PidHeight;

            //体重
            this.fpat_weight.EditValue = PatInfo.PidWeight;




            #endregion

            patidentity = PatInfo.PidIdentity.ToString();
            if (!string.IsNullOrEmpty(PatInfo.PidDoctorCode))
            {
                //如果送检者工号保存在表的ID列上，（不推荐）// 2010-8-24
                if (!string.IsNullOrEmpty(UserInfo.GetSysConfigValue("GetSendingDoctorType"))
                    && UserInfo.GetSysConfigValue("GetSendingDoctorType") == "LIS编码关联")
                {
                    this.txtPatDoc.SelectByID(PatInfo.PidDoctorCode);
                }
                else//如果送检者工号保存在表的Code列上（推荐）//157医院中大六院 用HISCODE
                {
                    this.txtPatDoc.SelectByValue(PatInfo.PidDoctorCode);
                    if (string.IsNullOrEmpty(this.txtPatDoc.valueMember)
                        && PatInfo.PidDocName != null
                        )
                    {
                        this.txtPatDoc.SelectByDispaly(PatInfo.PidDocName);
                        //Logger.WriteException(this.GetType().Name, "", string.Format("找不到doc_id={0}的信息，使用名称匹配，姓名={1}，匹配后txtPatDoc.valueMember={2}", PatInfo.SenderID, PatInfo.SenderName, this.txtPatDoc.valueMember));
                    }
                }
            }
            else
            {
                this.txtPatDoc.SelectByDispaly(PatInfo.PidDocName);

                if (!string.IsNullOrEmpty(PatInfo.PidDocName) && string.IsNullOrEmpty(this.txtPatDoc.displayMember))
                {
                    this.txtPatDoc.displayMember = PatInfo.PidDocName;
                }
                //Logger.WriteException(this.GetType().Name, "", string.Format("送检医生id为空 医生Name = {0}", PatInfo.SenderName));
            }

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
            }


            bAllowFirePatDept_ValueChanged = true;

            //病区
            this.txt_pat_ward_id.Text = PatInfo.PidDeptCode;
            this.txt_pat_ward_name.Text = PatInfo.PidDeptName;

            this.txtPatDiag.displayMember = PatInfo.PidDiag;

            //条码号
            this.txtPatBarCode.EditValue = PatInfo.RepBarCode;

            if (PatInfo.SampReceiveDate != null)
            {
                this.txtPatReceiveDate.EditValue = PatInfo.SampReceiveDate;
            }
            else
            {
                this.txtPatReceiveDate.EditValue = null;
            }

            //采样时间
            if (PatInfo.SampCollectionDate != null)
            {
                this.txtPatSampleDate.EditValue = PatInfo.SampCollectionDate.Value;
            }
            else
            {
                this.txtPatSampleDate.EditValue = null;
            }

            //出生日期
            if (PatInfo.PidBirthday != null)
            {
                this.txtBirthday.EditValue = PatInfo.PidBirthday.Value;
            }
            else
            {
                this.txtBirthday.EditValue = null;
            }

            //送检时间
            if (PatInfo.SampSendDate != null)// && UserInfo.GetSysConfigValue("AlwaysKeepSendDate") != "是")//如果不强制保存送检时间
            {
                this.txtPatSDate.EditValue = PatInfo.SampSendDate.Value;
            }
            else
            {
                this.txtPatSDate.EditValue = null;
            }
            //送达时间
            if (PatInfo.SampReachDate != null)// && UserInfo.GetSysConfigValue("AlwaysKeepSendDate") != "是")//如果不强制保存送检时间
            {
                this.txtPatReachDate.EditValue = PatInfo.SampReachDate.Value;
            }
            else
            {
                this.txtPatReachDate.EditValue = null;
            }
            //接收时间
            if (PatInfo.SampApplyDate != null)
            {
                this.txtPatApplyDate.EditValue = PatInfo.SampApplyDate.Value;
            }
            else
            {
                this.txtPatApplyDate.EditValue = null;
            }

            //检验时间
            this.txtPatRecDate.EditValue = ServerDateTime.GetServerDateTime();

            if (!string.IsNullOrEmpty(this.txtPatInspetor.valueMember))
            {
                this.txtPatInspetor.SelectByID(UserInfo.loginID);
            }

            //费用类别
            txtPatFeeType.valueMember = PatInfo.PidInsuId;

            //标本备注
            txtPatSamRem.displayMember = PatInfo.SampRemark;

            //病人ID类型
            if (!string.IsNullOrEmpty(PatInfo.PidIdtId))
            {
                this.txtPatIdType.SelectByID(PatInfo.PidIdtId);
            }

            //地址
            if (!string.IsNullOrEmpty(PatInfo.PidAddress))
                fpat_address.Text = PatInfo.PidAddress;

            //联系电话
            if (!string.IsNullOrEmpty(PatInfo.PidTel))
                fpat_tel.Text = PatInfo.PidTel;

            //病人来源
            if (!string.IsNullOrEmpty(PatInfo.PidSrcId))
            {
                this.txtPatSource.SelectByID(PatInfo.PidSrcId);
            }
            else if (!string.IsNullOrEmpty(PatInfo.SrcName))
            {
                this.txtPatSource.SelectByDispaly(PatInfo.SrcName);
            }

            //病人ID
            this.txtPatID.Text = PatInfo.PidInNo;

            //体检id
            this.txt_pat_emp_id.Text = PatInfo.PidExamNo;
            this.txtPatAdmissTime.Text = PatInfo.PidAddmissTimes.ToString();

            this.txtPatEmpCompanyName.Text = PatInfo.PidExamCompany;
            this.txtPatSocialNo.Text = PatInfo.PidSocialNo;

            //申请单号
            this.txtPatAppNo.Text = PatInfo.PidApplyNo;

            this.txtPatPid.Text = PatInfo.RepInputId;

            this.txtPatUpid.Text = PatInfo.PidUniqueId;//滨海唯一号

            this.txtPatNotice.Text = string.Empty;//注意事项
            if (txtPatNotice.Visible && (!string.IsNullOrEmpty(PatInfo.RepBarCode)))
            {
                string StrTempNotice = new ProxySampMain().Service.SampMainQueryByBarId(PatInfo.RepBarCode).SampRemark;
                if (!string.IsNullOrEmpty(StrTempNotice)) this.txtPatNotice.Text = StrTempNotice;
            }
        }


        //展开细菌对应的抗生素
        private void gridView2_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            EntityObrResultBact dr = this.gvBac.GetFocusedRow() as EntityObrResultBact;

            if (dr != null)
            {
                this.txtBac.Text = dr.ObrBacId.ToString();

                if (this.txtBac.Text.ToString().Trim() != "")
                {
                    List<EntityObrResultAnti> dt = (List<EntityObrResultAnti>)bsAnti.DataSource;
                    if (dt == null)
                        return;
                    var drPat = dt.FindAll(a => a.ObrBacId == txtBac.Text && a.SortNo == dr.SortNo);//.Select("bt_id='" + txtBac.Text.ToString() + "'");//查询出是否有该细菌的抗生素
                    if (drPat.Count > 0)
                    {
                        gvAnti.FocusedRowHandle = 0;
                        bsAnti.MoveFirst();
                        for (int i = 0; i < bsAnti.Count; i++)
                        {
                            EntityObrResultAnti dtRow = (EntityObrResultAnti)bsAnti.Current;
                            if (dtRow.ObrBacId == this.txtBac.Text)//如果抗生素属于该菌
                            {
                                this.gvAnti.CollapseAllGroups();//闭合所有分组
                                // GridView view = gridView4;
                                int m = gvAnti.GetParentRowHandle(gvAnti.FocusedRowHandle);
                                if (UserInfo.GetSysConfigValue("BtAllowAddSameBarbid") == "是")
                                {
                                    this.gvAnti.ExpandGroupRow(-dr.SortNo * 2 + 1);//展开分组
                                    this.gvAnti.ExpandGroupRow(-dr.SortNo * 2);
                                }
                                else {
                                    this.gvAnti.ExpandGroupRow(m);
                                }
                                return;
                            }
                            bsAnti.MoveNext();
                        }
                    }
                    else
                        this.gvAnti.CollapseAllGroups();//闭合所有分组
                }
                else
                    this.gvAnti.ExpandAllGroups();//展开所有
            }
        }

        //注意：选择用哪个事件 2010-5-21
        private void txtYingYang_KeyDown(object sender, KeyEventArgs e)
        {
            TextEdit thisEdit = (TextEdit)sender;

            if (e.KeyCode == Keys.Enter)
            {
                string inputStr = thisEdit.EditValue.ToString().Trim();
                if (inputStr == "-")
                {
                    gvBac.SetFocusedValue("阴性(-)");
                }
                else if (inputStr == "+")
                {
                    gvBac.SetFocusedValue("阳性(+)");
                }
                else if (inputStr == "*")
                {
                    gvBac.SetFocusedValue("弱阳性(±)");
                }
                if (gvBac.FocusedRowHandle < gvBac.RowCount - 1)
                {
                    gvBac.MoveNext();
                }
            }
        }

        private void gridView2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            EntityObrResultBact dr = gvBac.GetRow(e.RowHandle) as EntityObrResultBact;
            if (dr != null)
            {
                string strRes = dr.ObrRemark;
                if (strRes == "阳性(+)" || strRes == "阳性")
                {
                    if (e.Column.FieldName == "ObrRemark")
                        e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        private void repositoryItemComboBox1_DoubleClick(object sender, EventArgs e)
        {
            gvBac.EditingValue = "≥10E4";
        }

        /// <summary>
        /// 复制抗生素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopy_Click(object sender, EventArgs e)
        {
            List<EntityObrResultBact> dtBac = (List<EntityObrResultBact>)bs_rlts.DataSource;
            EntityObrResultBact dr = (EntityObrResultBact)bs_rlts.Current;

            List<EntityObrResultAnti> anlist = (List<EntityObrResultAnti>)bsAnti.DataSource;
            if (!anlist.Any(a => a.ObrBacId == dr.ObrBacId.ToString()))//.Select("bt_id='" + dr["bac_cname"] + "'").Length == 0)
            {
                MessageDialog.Show("该菌无抗生素！", "提示");
                return;
            }

            List<EntityObrResultBact> dtBacCopy = new List<EntityObrResultBact>();
            foreach (var drBac in dtBac)
            {
                if (drBac.ObrBacId != dr.ObrBacId)
                {
                    dtBacCopy.Add((EntityObrResultBact)drBac.Clone());
                }
            }
        }

        /// <summary>
        /// 上下箭头跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            KeysHelper.Jump(e.KeyCode);
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBacterialInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall && MessageDialog.Show("您确定要关闭当前窗口吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        bool bAllowFirePatDept_ValueChanged = true;
        private void txtPatDeptId_ValueChanged(object sender, ValueChangeEventArgs args)
        {
            if (bAllowFirePatDept_ValueChanged)
            {
                string dep_code = this.txtPatDeptId.valueMember;

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

        private void SelectedPageChanged(int index)
        {
            switch (index)
            {
                case 0:
                    cbeFlag.EditValue = "-1";
                    break;
                case 1:
                    cbeFlag.EditValue = "2";
                    break;
                case 2:
                    cbeFlag.EditValue = "2";
                    break;
                case 3:
                    if (UserInfo.GetSysConfigValue("Lab_ShowFirstAuditButton") == "是")
                        cbeFlag.EditValue = "1";
                    else
                        cbeFlag.EditValue = "2";
                    break;
                case 4:
                    cbeFlag.EditValue = "6";
                    break;
                case 5:
                    cbeFlag.EditValue = "3";
                    break;
                default:
                    break;
            }
        }


        private void RoundPanelGroupBac_RoundPanelGroupClick(object sender, EventArgs e)
        {
            RoundPanel curRp = roundPanelGroupBac.GetCurRoundPanel();
            if (curRp.Tag == null)
                return;
            int pageIndex = ConvertHelper.IntParse(curRp.Tag?.ToString(), 0);
            SelectedPageChanged(pageIndex);
        }


        private void txtPatID_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
            if (txtPatID.Text != null && txtPatID.Text.StartsWith("#"))
                txtPatID.Text = txtPatID.Text.Replace("#", "").Trim();
        }

        private void gridView4_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "ObrValue")
            {
                if (e.DisplayText.Replace(" ", "") == "敏感"
                    || e.DisplayText.Replace(" ", "").ToLower() == "s"
                    )
                {
                    e.Appearance.ForeColor = Color.Blue;
                }
                else if (e.DisplayText.Replace(" ", "") == "中介"
                    || e.DisplayText.Replace(" ", "").ToLower() == "i"
                    )
                {
                }
                else if (e.DisplayText.Replace(" ", "") == "阳性"
                    )
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (e.DisplayText.Replace(" ", "") == "耐药"
                    || e.DisplayText.Replace(" ", "").ToLower() == "r"
                    )
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (e.DisplayText.Replace(" ", "") == "BLAC"
                    || e.DisplayText.Replace(" ", "").ToLower() == "BLAC"
                    )
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (e.DisplayText.Replace(" ", "") == "MRS"
                    || e.DisplayText.Replace(" ", "").ToLower() == "MRS"
                    )
                {
                    e.Appearance.ForeColor = Color.Red;
                }
                else if (e.DisplayText.Replace(" ", "") == "ESBL"
                    || e.DisplayText.Replace(" ", "").ToLower() == "ESBL"
                    )
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }

        private void gridView3_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "ObrValue")
            {
                if (e.Value == null)
                {
                }
                else
                {
                    string bsr_cname = e.Value.ToString().Trim();



                    bsr_cname = strBsrCname;
                    var drArr = smearList.FindAll(a => a.SmeName == bsr_cname);//.Select(" nob_cname = '" + strBsrCname + "'");
                    if (drArr.Count > 0)
                    {
                        if (drArr[0].SmePositiveFlag.ToString() == "1")
                        {
                            if (ListConfigCriticalSample.Contains(txtPatSampleType.valueMember))
                            {
                                ((EntityObrResultDesc)this.gvDesc.GetRow(e.RowHandle)).ObrPositiveFlag = 1;
                                this.chk_pat_critical.Checked = true;
                            }
                        }
                    }
                }
            }
        }

        private void gridView3_RowStyle(object sender, RowStyleEventArgs e)
        {
            var dr = this.gvDesc.GetRow(e.RowHandle) as EntityObrResultDesc;
            if (
                dr != null
                && dr.ObrValue != null)
            {
                if (dr.ObrValue.ToString().Trim().IndexOf("找到") > -1 && dr.ObrValue.Trim().IndexOf("未") < 0)
                {
                    e.Appearance.ForeColor = Color.Red;
                }
            }
        }


        private string strBsrCname = "";


        private void repositoryItemComboBox3_EditValueChanged(object sender, EventArgs e)
        {
            var dr = gvAnti.GetFocusedRow() as EntityObrResultAnti;
            if (dr != null)
            {
                isDataChaged = true;
                string str = this.gvAnti.EditingValue.ToString();
                dr.Sszone = str == "MIC" ? dr.SsMstd : dr.SsIzone;
                dr.ObrRef = str == "MIC" ? "MIC" : "KB";
            }
        }

        #region 无菌或涂片默认值设置


        void ceCombine_CombineRemoved(object sender, string com_id)
        {
            isDataChaged = true;

            if (xtabExperiment.SelectedTabPageIndex == 1)
            {
                SetDefaultData();
            }
        }

        void ceCombine_CombineAdded(object sender, string com_id, int com_seq)
        {
            isDataChaged = true;

            if (xtabExperiment.SelectedTabPageIndex == 1)
            {
                SetDefaultData();
            }
        }

        void SetDefaultData()
        {
            try
            {
                PatientsMi_RowChanged(null, null);

                if (this.ceCombine.listRepDetail != null && this.ceCombine.listRepDetail.Count > 0)
                {

                    if (this.fpat_id == "" || this.DescList.Count == 0)
                    {
                        List<EntityObrResultDesc> deList = this.gcDesc.DataSource as List<EntityObrResultDesc>;
                        foreach (var item in deList)
                        {
                            item.ObrValue = string.Empty;
                        }
                        foreach (var item in this.ceCombine.listRepDetail)
                        {
                            string comID = item.ComId;
                            List<string> defDataList = new ProxyObrResult().Service.GetCombineDefData(txtPatInstructment.valueMember, comID);
                            for (int i = 0; i < defDataList.Count; i++)
                            {
                                if (deList.Count > i)
                                {
                                    deList[i].ObrValue = defDataList[i];
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Logger.WriteException("FrmBacterialInput", "无菌或涂片默认值设置", ex.Message);
            }

            gcDesc.RefreshDataSource();
        }

        #endregion


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
                this.txtPatDoc.Readonly = !Modify;
                #endregion
            }
            else
            {
                #region 通用

                this.txtPatDate.Properties.ReadOnly = !Modify;
                this.txtPatSid.Properties.ReadOnly = !Modify;
                this.txtPatIdType.Readonly = !Modify;
                this.txtPatID.Properties.ReadOnly = !Modify;
                this.txtPatFeeType.Readonly = !Modify;
                this.txtPatDeptId.Readonly = !Modify;
                this.txtPatBedNo.Properties.ReadOnly = !Modify;
                this.txtPatName.Properties.ReadOnly = !Modify;
                this.txtPatSex.Readonly = !Modify;
                this.textAgeInput1.Properties.ReadOnly = !Modify;
                this.txtPatSampleType.Readonly = !Modify;
                this.txtPatSampleState.Readonly = !Modify;
                this.txtPatDiag.Readonly = !Modify;
                this.txtPatSource.Readonly = !Modify;
                this.txtPatDoc.Readonly = !Modify;
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
                this.txtPatReceiveDate.Properties.ReadOnly = !Modify;
                this.txtPatSampleDate.Properties.ReadOnly = !Modify;
                this.txtPatSDate.Properties.ReadOnly = !Modify;
                this.txtPatReachDate.Properties.ReadOnly = !Modify;
                this.txtPatApplyDate.Properties.ReadOnly = !Modify;
                this.txtPatRecDate.Properties.ReadOnly = !Modify;
                this.luePatCheckType.Readonly = !Modify;
                #endregion
            }
        }

        /// <summary>
        /// 双击细菌备注框弹出大窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memoEdit1_DoubleClick(object sender, EventArgs e)
        {
            FrmShowText st = new FrmShowText(this.memoEdit1, "细菌备注");
            st.ShowDialog();
            this.memoEdit1.Focus();
            this.memoEdit1.Select(memoEdit1.Text.Length, 0);
        }


        FrmPatHistoryEXP frmHistory = null;
        /// <summary>
        /// 查看历史评价
        /// </summary>
        private void ShowHistoryExp()
        {
            if (bsPatLst.Current != null)
            {
                EntityPidReportMain drLst = (EntityPidReportMain)bsPatLst.Current;

                if (drLst != null)
                {
                    if (frmHistory == null)
                    {
                        frmHistory = new FrmPatHistoryEXP();
                        //***************************************************************************
                        //窗体显示在屏幕中间
                        frmHistory.StartPosition = FormStartPosition.CenterScreen;

                        //***************************************************************************
                    }
                    frmHistory.PatInNo = drLst.PidInNo;
                    frmHistory.PatID = drLst.RepId;
                    frmHistory.Visible = true;
                    frmHistory.GetHistoryExp();
                    frmHistory.Show();
                }
            }

        }

        private void btnHistoryEXP_Click(object sender, EventArgs e)
        {
            ShowHistoryExp();//查看历史评价
        }

        /// <summary>
        /// 追加条码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecheck_Click(object sender, EventArgs e)
        {
            if (this.gvPatList.GetFocusedRow() == null)
            {
                lis.client.control.MessageDialog.Show("请在病人列表中选中需要追加条码的记录", "提示");
                return;
            }

            FrmAdditionalBarcode frmAddBarcode = new FrmAdditionalBarcode();
            frmAddBarcode.DestPatID = this.txtPatID.Text;
            if (frmAddBarcode.ShowDialog() == DialogResult.OK)
            {
                ProxyPatEnterNew proxyEnter = new ProxyPatEnterNew();
                bool result = proxyEnter.Service.PatientAdditionalBarcode(txtPatInstructment.valueMember + Convert.ToDateTime(this.txtPatDate.EditValue).ToString("yyyyMMdd") + txtPatSid.Text.Trim()
                    , Convert.ToDateTime(this.txtPatDate.EditValue).Date
                    , this.txtPatBarCode.Text
                    , frmAddBarcode.PatientMi[0].RepBarCode
                    , txtPatInstructment.displayMember
                    , txtPatSid.Text.Trim()
                    , UserInfo.loginID
                    , UserInfo.userName);
                foreach (var com in frmAddBarcode.PatientMi)
                {
                    ceCombine.AddCombine(com.ComId, com.OrderSn, Convert.ToDecimal(com.OrderPrice), com.RepBarCode);
                }
            }

        }


        string strMic = string.Empty;
        EntityObrResultAnti drMic = null;

        private void txtSimc_EditValueChanged(object sender, EventArgs e)
        {
            if (gvAnti.EditingValue != null)
            {
                strMic = gvAnti.EditingValue.ToString();
                drMic = gvAnti.GetFocusedRow() as EntityObrResultAnti;
            }
        }

        /// <summary>
        /// 复制历史结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmenuCopyRlts_Click(object sender, EventArgs e)
        {
        }

        private void 危急值记录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentPatInfo == null || CurrentPatInfo.RepId == null)
            {
                return;
            }
            var drTempPat = CurrentPatInfo;

            bool isRAV = UserInfo.GetSysConfigValue("Lab_ReportCriticalValueInfo") == "是";
            if (isRAV)
            {
                FrmPatientsExt frmPat = new FrmPatientsExt();
                frmPat.PatId = drTempPat.RepId;
                frmPat.IsBacterial = true;
                frmPat.selectMode = !(drTempPat.RepStatus.ToString() == "2" || drTempPat.RepStatus.ToString() == "4");
                frmPat.ShowDialog();

                return;
            }
            if (drTempPat != null && (drTempPat.RepStatus.ToString() == "2" || drTempPat.RepStatus.ToString() == "4"))
            {
                FrmPatientsExt frmPat = new FrmPatientsExt();
                frmPat.PatId = drTempPat.RepId;
                frmPat.IsBacterial = true;//标记为微生物调用
                if (UserInfo.GetSysConfigValue("AntiLab_RecordCriticalValueInfo") == "是")
                    frmPat.selectMode = false;
                else
                    frmPat.selectMode = true;
                frmPat.ShowDialog();
            }
            else if (drTempPat != null
                && (!string.IsNullOrEmpty(drTempPat.RepUrgentFlag.ToString())
                && drTempPat.RepUrgentFlag.ToString() != "0"))
            {
                FrmPatientsExt frmPat = new FrmPatientsExt();
                frmPat.PatId = drTempPat.RepId;
                frmPat.IsBacterial = true;//标记为微生物调用
                frmPat.selectMode = false;
                frmPat.ShowDialog();
            }
        }


        private void 更新为传染病ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<EntityPidReportMain> listMain = bsPatLst.DataSource as List<EntityPidReportMain>;
            List<EntityPidReportMain> listMainFilter = GetCheckedPatients();
            List<EntityPidReportMain> listUpdate = new List<EntityPidReportMain>();
            if (listMainFilter.Count < 1)
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("请选择要更新为【传染病】的数据！");
                return;
            }
            StringBuilder sbother = new StringBuilder();
            StringBuilder sbMsg = new StringBuilder();
            sbMsg.AppendLine("以下患者报告已审核，无法更新为【传染病】类型报告：");
            sbMsg.Append("【{0}】");
            foreach (EntityPidReportMain dr in listMainFilter)
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

        private void RoundPanelResult_RoundPanelGroupClick(object sender, EventArgs e)
        {
            RapitSearch();
        }


        #region  isDataChaged
        private void repositoryItemComboBox1_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void repositoryItemLookUpEdit4_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void repositoryItemCheckEdit2_CheckedChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void textAgeInput1_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void txtPatSex_onAfterChange(DataRow oldRow)
        {
            isDataChaged = true;
        }

        private void txtPatName_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void txtPatBedNo_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void txtPatSid_EditValueChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void chk_pat_critical_CheckedChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }

        private void txtPatSex_ValueChanged(object sender, ValueChangeEventArgs args)
        {
            isDataChaged = true;
        }

        private void textAgeInput1_TextChanged(object sender, EventArgs e)
        {
            isDataChaged = true;
        }
        #endregion


        /// <summary>
        /// 急查颜色
        /// </summary>
        Color corUrgent = Color.White;
        Color corBD = Color.White;
        public static Color GetBarcodeConfigColor(string configCode)
        {

            string cfgValue = ConfigHelper.GetSysConfigValueWithoutLogin(configCode);

            Color col;
            //黑色,红色,灰色,蓝色,绿色,紫色
            switch (cfgValue)
            {
                case "黑色":
                    col = Color.Black;
                    break;

                case "红色":
                    col = Color.Red;
                    break;

                case "灰色":
                    col = Color.DarkGray;
                    break;
                case "粉红色":
                    col = Color.Pink;
                    break;
                case "黄色":
                    col = Color.Yellow;
                    break;
                case "蓝色":
                    col = Color.Blue;
                    break;

                case "绿色":
                    col = Color.Green;
                    break;

                case "紫色":
                    col = Color.Purple;
                    break;
                case "白色":
                    col = Color.White;
                    break;
                case "棕色":
                    col = Color.Brown;
                    break;

                default:
                    col = Color.Black;
                    break;
            }
            return col;
        }

        private void btnSelectNoBat_Click(object sender, EventArgs e)
        {
            try
            {
                EntityPidReportMain row = gvPatList.GetFocusedRow() as EntityPidReportMain;
                if (row == null) return;

                if (row.RepStatus != null && (row.RepStatus.ToString() == "2" || row.RepStatus.ToString() == "4"))
                {
                    MessageDialog.ShowAutoCloseDialog("该报告已审核");
                    return;
                }

                List<string> listComId = new List<string>();
                if (ceCombine.listRepDetail != null && ceCombine.listRepDetail.Count > 0)
                {
                    foreach (EntityPidReportDetail detail in ceCombine.listRepDetail)
                    {
                        listComId.Add(detail.ComId);
                    }
                }
                List<EntityDicNobactCom> NobactCache = CacheClient.GetCache<EntityDicNobactCom>();
                List<EntityDicNobactCom> listNobact = new List<EntityDicNobactCom>();
                listNobact = (from x in NobactCache where listComId.Contains(x.ComId) select x).ToList();
                List<string> listNobId = new List<string>();
                if (listNobact != null && listNobact.Count > 0)
                {
                    foreach (EntityDicNobactCom nobact in listNobact)
                    {
                        listNobId.Add(nobact.NobId);
                    }
                }
                List<EntityDicMicSmear> listSmearFilter = new List<EntityDicMicSmear>();
                listSmearFilter = (from x in smearList where listNobId.Contains(x.SmeId) select x).ToList();
                FrmManuaSelectNobat selector = new FrmManuaSelectNobat();
                selector.LoadData(listSmearFilter);

                if (selector.ShowDialog() == DialogResult.Yes)
                {

                    List<string> nobat = selector.NobatList;
                    List<EntityObrResultDesc> tabel = gcDesc.DataSource as List<EntityObrResultDesc>;

                    if (tabel != null && tabel.Count > 0)
                    {
                        for (int i = 0; i < tabel.Count; i++)
                        {
                            if (nobat.Count > 0 && (tabel[i].ObrValue == null
                           || string.IsNullOrEmpty(tabel[i].ObrValue.ToString())))
                            {
                                tabel[i].ObrValue = nobat[0];
                                nobat.RemoveAt(0);
                            }
                        }
                        QuickUpdate();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);
            }
        }

        private void QuickUpdate()
        {
            gvBac.CloseEditor();
            gvDesc.CloseEditor();
            gvAnti.CloseEditor();

            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;

            DateTime dtNow = ServerDateTime.GetServerDateTime();
            EntityQcResultList saveData = new EntityQcResultList();
            EntityPidReportMain dr = new EntityPidReportMain();
            this.FillEntityFromUI(dr);
            String _pat_id = this.fpat_id;
            saveData.patient = dr;
            if (ceCombine.listRepDetail != null)
                saveData.listRepDetail = EntityManager<EntityPidReportDetail>.ListClone(ceCombine.listRepDetail);

            if (xtabExperiment.SelectedTabPageIndex == 0)
            {
                List<EntityObrResultBact> baclist = bs_rlts.DataSource as List<EntityObrResultBact>;
                List<EntityObrResultAnti> anlist = bsAnti.DataSource as List<EntityObrResultAnti>;
                if (baclist == null || anlist == null)
                {
                    MessageDialog.ShowAutoCloseDialog("数据异常,请刷新");
                    return;
                }

                GetBactResult(saveData, dtNow);
                if (GetAntiResult(saveData, dtNow))
                    return;
            }
            else
            {
                List<EntityObrResultDesc> descs = gcDesc.DataSource as List<EntityObrResultDesc>;
                if (descs == null)
                {
                    MessageDialog.ShowAutoCloseDialog("数据异常,请刷新");
                    return;
                }
                GetDescResult(saveData, dtNow);
            }

            var ret = new ProxyMicEnter().Service.UpdateMicPatResult(Caller, saveData);

            bsPatLst.EndEdit();
            gcDesc.RefreshDataSource();
        }

        private void linkHistory_Click(object sender, EventArgs e)
        {
            if (CurrentPatInfo == null || string.IsNullOrEmpty(CurrentPatInfo.RepId))
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
                return;
            }
            var dr = CurrentPatInfo;
            FrmPatInfoExt frm = new FrmPatInfoExt();
            frm.LoadBacHistory(dr);
            Rectangle area = new Rectangle();
            area = Screen.GetWorkingArea(this);
            frm.Left = area.Width - frm.Width;
            frm.Top = area.Height - frm.Height;
            frm.Show();
        }

        private void linkImage_Click(object sender, EventArgs e)
        {
            EntityPidReportMain dr = gvPatList.GetFocusedRow() as EntityPidReportMain;
            if (dr != null)
            {

                string pat_id = dr.RepId;
                FrmPatInfoExt frm = new FrmPatInfoExt();
                frm.LoadImage(pat_id);
                Rectangle area = new Rectangle();
                area = Screen.GetWorkingArea(this);
                frm.Left = area.Width - frm.Width;
                frm.Top = area.Height - frm.Height;
                frm.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
            }
        }

        private void linkInfo_Click(object sender, EventArgs e)
        {
            if (CurrentPatInfo == null)
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
                return;
            }

            EntityPidReportMain dr = gvPatList.GetFocusedRow() as EntityPidReportMain;
            if (dr != null)
            {

                string pat_id = dr.RepId;
                FrmPatInfoExt frm = new FrmPatInfoExt();
                frm.LoadInfo(pat_id, ceCombine.listRepDetail, dr.RepBarCode);
                Rectangle area = new Rectangle();
                area = Screen.GetWorkingArea(this);
                frm.Left = area.Width - frm.Width;
                frm.Top = area.Height - frm.Height;
                frm.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
            }

        }

        /// <summary>
        /// 获取检测对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SbtnCheckObj_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatName.Text))
                return;

            string[] checkObj = txtPatName.Text.Split('＋');
            if (checkObj == null || checkObj.Count() <= 0)
                return;

            var list = gcDesc.DataSource as List<EntityObrResultDesc>;

            for (int j = 0; j < list.Count(); j++)
            {
                if (j > checkObj.Count()-1)
                    continue;

                list[j].ObrCheckObj = checkObj[j];
            }
            gcDesc.RefreshDataSource();
        }

        private void dtBegin_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.valueMember) || dtEnd.EditValue == null)
                return;
            SearchPatientsAndAddNew();
        }

        private void dtEnd_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPatInstructment.valueMember) || dtBegin.EditValue == null)
                return;
            SearchPatientsAndAddNew();
        }

        private void txtEditDate_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDateType.Text) && txtPatReceiveDate.Properties.ReadOnly == false)
            {
                switch (txtDateType.Text)
                {
                    case "申请时间":
                        txtPatReceiveDate.EditValue = txtEditDate.EditValue;
                        break;
                    case "采样时间":
                        txtPatSampleDate.EditValue = txtEditDate.EditValue;
                        break;
                    case "送达时间":
                        txtPatReachDate.EditValue = txtEditDate.EditValue;
                        break;
                    case "接收时间":
                        txtPatApplyDate.EditValue = txtEditDate.EditValue;
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
                    FillTimeLine((EntityPidReportMain)bsPat.Current);
                }
                catch
                { }
            }
        }

        private void FillTimeLine(EntityPidReportMain PatInfo)
        {
            if (PatInfo == null) return;
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string LASTDATE = "";

            if (!string.IsNullOrEmpty(PatInfo.SampReceiveDate.ToString())
                && !dic.ContainsKey(PatInfo.SampReceiveDate.ToString()))
            {
                LASTDATE = PatInfo.SampReceiveDate.ToString();
                dic.Add(PatInfo.SampReceiveDate.ToString(), "申请时间");
            }
            if (!string.IsNullOrEmpty(PatInfo.SampCollectionDate.ToString())
                 && !dic.ContainsKey(PatInfo.SampCollectionDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampCollectionDate.ToString(),
                        LASTDATE);

                    dic.Add(PatInfo.SampCollectionDate.ToString(), "采集时间" + "|" + mins);
                }
                else
                {
                    dic.Add(PatInfo.SampCollectionDate.ToString(), "采集时间");
                }
                LASTDATE = PatInfo.SampCollectionDate.ToString();
            }

            if (!string.IsNullOrEmpty(PatInfo.SampSendDate.ToString())
                 && !dic.ContainsKey(PatInfo.SampSendDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampSendDate.ToString(),
                        LASTDATE);

                    dic.Add(PatInfo.SampSendDate.ToString(), "收取时间" + "|" + mins);
                }
                else
                {
                    dic.Add(PatInfo.SampSendDate.ToString(), "收取时间");
                }
                LASTDATE = PatInfo.SampSendDate.ToString();

            }

            if (!string.IsNullOrEmpty(PatInfo.SampReachDate.ToString())
               && !dic.ContainsKey(PatInfo.SampReachDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampReachDate.ToString(),
                        LASTDATE);

                    dic.Add(PatInfo.SampReachDate.ToString(), "送达时间" + "|" + mins);
                }
                else
                {
                    dic.Add(PatInfo.SampReachDate.ToString(), "送达时间");
                }
                LASTDATE = PatInfo.SampReachDate.ToString();
            }


            if (!string.IsNullOrEmpty(PatInfo.SampApplyDate.ToString())
               && !dic.ContainsKey(PatInfo.SampApplyDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampApplyDate.ToString(),
                        LASTDATE);

                    dic.Add(PatInfo.SampApplyDate.ToString(), "签收时间" + "|" + mins);
                }
                else
                {
                    dic.Add(PatInfo.SampApplyDate.ToString(), "签收时间");
                }
                LASTDATE = PatInfo.SampApplyDate.ToString();
            }

            if (!string.IsNullOrEmpty(PatInfo.SampCheckDate.ToString())
                && !dic.ContainsKey(PatInfo.SampCheckDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.SampCheckDate.ToString(),
                        LASTDATE);

                    dic.Add(PatInfo.SampCheckDate.ToString(), "检验时间" + "|" + mins);
                }
                else
                {
                    dic.Add(PatInfo.SampCheckDate.ToString(), "检验时间");
                }
                LASTDATE = PatInfo.SampCheckDate.ToString();

                //dic.Add(PatInfo.Rows[0]["pat_jy_date"].ToString(), "检验时间");
            }


            if (!string.IsNullOrEmpty(PatInfo.RepAuditDate.ToString())
              && !dic.ContainsKey(PatInfo.RepAuditDate.ToString()))
            {
                bool isskip = false;
                if (!string.IsNullOrEmpty(PatInfo.RepReportDate.ToString()))
                {
                    if (DateMins(PatInfo.RepReportDate.ToString(),
                        PatInfo.RepAuditDate.ToString()) == "0")
                    {
                        isskip = true;
                    }
                }
                if (!isskip)
                {
                    if (!string.IsNullOrEmpty(LASTDATE))
                    {
                        string mins = DateMins(PatInfo.RepAuditDate.ToString(),
                            LASTDATE);

                        dic.Add(PatInfo.RepAuditDate.ToString(), "审核时间" + "|" + mins);
                    }
                    else
                    {
                        dic.Add(PatInfo.RepAuditDate.ToString(), "审核时间");
                    }
                    LASTDATE = PatInfo.RepAuditDate.ToString();
                }
            }


            if (!string.IsNullOrEmpty(PatInfo.RepReportDate.ToString())
             && !dic.ContainsKey(PatInfo.RepReportDate.ToString()))
            {
                if (!string.IsNullOrEmpty(LASTDATE))
                {
                    string mins = DateMins(PatInfo.RepReportDate.ToString(),
                        LASTDATE);

                    dic.Add(PatInfo.RepReportDate.ToString(), "报告时间" + "|" + mins);
                }
                else
                {
                    dic.Add(PatInfo.RepReportDate.ToString(), "报告时间");
                }
                LASTDATE = PatInfo.RepReportDate.ToString();
            }





            ucTimeLine1.LoadData(dic);
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

        private void txtPatInspetor_onAfterChange(EntitySysUser oldRow)
        {
            isDataChaged = true;
        }

        private void txtPatInspetor_Load(object sender, EventArgs e)
        {
            this.txtPatInspetor.SetFilter(this.txtPatInspetor.getDataSource().FindAll(w => w.UserType == "检验组"));
        }

        private void txtPatIdType_onAfterChange(EntityDicPubIdent oldRow)
        {
            string ori_id = DictPatNumberType.Instance.GetOriID_byNoType(this.txtPatIdType.valueMember);

            this.txtPatSource.SelectByID(ori_id);
        }

        private void txtPatDeptId_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (bAllowFirePatDept_ValueChanged)
            {
                string dep_code = this.txtPatDeptId.valueMember;

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

        private void txtSelType1_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (txtPatType.valueMember != null && txtPatType.valueMember != "")
                ceCombine.CTypeID = txtPatType.valueMember;
            else
                ceCombine.CTypeID = "";

            txtPatInstructment_onBeforeFilter();
        }

        private string ProId = string.Empty;
        private void txtPatInstructment_onBeforeFilter()
        {
            //当前选中的物理组ID
            string currentSelectType = this.txtPatType.valueMember;
            List<EntityDicInstrument> itrList = CacheClient.GetCache<EntityDicInstrument>();
            //是否有物理组
            if (currentSelectType != null && currentSelectType.Trim(null) != string.Empty)
            {
                itrList = itrList.Where(i => i.ItrLabId == currentSelectType && i.ItrReportType == "3").ToList();
            }
            else//没有：列出所有仪器(非细菌的)
            {
                itrList = itrList.Where(i => i.ItrReportType == "3").ToList();
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

            this.txtPatInstructment.SetFilter(itrList);
        }

        private void txtSelType1_Load(object sender, EventArgs e)
        {
            List<EntityDicInstrument> itrList = CacheClient.GetCache<EntityDicInstrument>();
            List<EntityDicPubProfession> typeList = CacheClient.GetCache<EntityDicPubProfession>();

            var query = (from t in typeList join i in itrList on t.ProId equals i.ItrLabId where i.ItrReportType == "3" select t).Distinct().ToList();

            txtPatType.SetFilter(query);
        }

        private void txtItr1_onAfterChange(EntityDicInstrument oldRow)
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
                    }
                }
            }
            SearchPatientsAndAddNew();
        }

        private void txtItr1_Load(object sender, EventArgs e)
        {
            List<EntityDicInstrument> itrList = CacheClient.GetCache<EntityDicInstrument>();
            var query = itrList.Where(w => w.ItrReportType == "3").ToList();
            txtPatInstructment.SetFilter(query);
        }

        private void txtItr1_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            var drIns = txtPatInstructment.selectRow;
            if (drIns != null)
            {
                ceCombine.ItrID = drIns.ItrId;
                InitNobact(drIns);
            }
            else
            {
                ceCombine.ItrID = string.Empty;
                ceCombine.PTypeID = "";
            }
        }

        /// <summary>
        /// 自动清除缓存审核人的密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearPassWordTimer_Tick(object sender, EventArgs e)
        {
            strLastOperationPw = string.Empty;
        }

        public List<EntityPidReportMain> GetCheckedPatients()
        {
            gvPatList.CloseEditor();
            this.bsPat.EndEdit();

            List<EntityPidReportMain> checkList = new List<EntityPidReportMain>();
            var selectIndex = gvPatList.GetSelectedRows();
            foreach (int index in selectIndex)
            {
                checkList.Add(gvPatList.GetRow(index) as EntityPidReportMain);
            }

            if (checkList.Count <= 0 
                && Lab_NoCheckSelectCurRow
                && CurrentPatInfo != null
                && !string.IsNullOrEmpty(CurrentPatInfo.RepId))
            {
                checkList.Add(gvPatList.GetFocusedRow() as EntityPidReportMain);
            }

            return checkList;
        }

        /// <summary>
        /// 保存CA签名
        /// </summary>
        /// <param name="frmCheck"></param>
        /// <param name="result"></param>
        private void SaveCASignInfo(FrmCheckPassword frmCheck, EntityOperationResultList result)
        {
            if (frmCheck.caPKI != null)
            {
                //wf.client.frame.PubWaitForm waitForm = new PubWaitForm();
                //waitForm.StartPosition = FormStartPosition.CenterScreen;
                //waitForm.SetCaption("正在签名");
                //waitForm.Show();
                Stopwatch s = new Stopwatch();
                s.Start();

                ProxyUserManage proxyUserManage = new ProxyUserManage();

                List<EntityCaSign> caSignList = new List<EntityCaSign>();

                string plainSignData = string.Empty;
                string signRes = string.Empty;
                string plainStampData = string.Empty;
                string timeStampRes = string.Empty;

                bool caRes = true;

                foreach (var res in result)
                {
                    if (frmCheck.operationCode == EnumOperationCode.Report)
                    {
                        //签名原文
                        plainSignData = res.OperationResultData.ToString();

                        //数字签名结果
                        signRes = frmCheck.caPKI.CASignature(plainSignData);

                        if (string.IsNullOrEmpty(signRes))
                        {
                            MessageDialog.Show("数字签名出错：" + frmCheck.caPKI.ErrorInfo);
                            caRes = false;
                        }

                        //时间戳原文
                        plainStampData = "Ukey证书签名原文：" + plainSignData + "Ukey证书签名结果：" + signRes;
                        //时间戳结果
                        timeStampRes = frmCheck.caPKI.CATimeStamp(plainStampData);

                        if (string.IsNullOrEmpty(timeStampRes))
                        {
                            MessageDialog.Show("打时间戳出错：" + frmCheck.caPKI.ErrorInfo);
                            caRes = false;
                        }
                    }

                    EntityCaSign caSign = new EntityCaSign();

                    caSign.CaDate = ServerDateTime.GetServerDateTime();
                    caSign.CaLoginId = frmCheck.OperatorID;
                    caSign.CaName = frmCheck.OperatorName;
                    caSign.CaEvent = frmCheck.operationCode == EnumOperationCode.Report ? "发布报告" : "取消报告";
                    caSign.CaRemark = string.Format("{0}[{1}]{2}", caSign.CaEvent, res.Data.Patient.RepId, (caRes ? "成功" : "失败:" + frmCheck.caPKI.ErrorInfo));
                    //caSign.CaEntityId = frmCheck.caPKI.GetIdentityID();

                    caSign.CaSourceContent = plainSignData;
                    caSign.CaSignContent = signRes;
                    caSign.CaSourceTimestamp = plainStampData;
                    caSign.CaSignTimestamp = timeStampRes;
                    caSignList.Add(caSign);
                }

                proxyUserManage.Service.InsertCaSign(caSignList);

                s.Stop();
                Lib.LogManager.Logger.LogInfo("签名完成，用时：" + s.Elapsed);

                //waitForm.SetCaption("签名完成，用时" + s.Elapsed);
                //waitForm.Close();
            }
        }


    }
}

