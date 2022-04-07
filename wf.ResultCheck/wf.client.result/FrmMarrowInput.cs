using dcl.client.control;
using lis.client.control;
using dcl.client.frame;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.entity;
using dcl.client.frame.runsetting;
using dcl.common;
using dcl.client.wcf;

using System.IO;
using dcl.client.result.CommonPatientInput;
using dcl.client.result.Interface;
using DevExpress.XtraEditors;
using lis.client.result;
using System.Threading;
using dcl.client.report;
using dcl.root.logon;
using dcl.client.result.UILogic;
using dcl.client.result.PatControl;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.client.result.DictToolkit;
using dcl.interfaces;
using DevExpress.XtraBars;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmMarrowInput : BaseButtonFormEx, IPatPanelConfig
    {
        /// <summary>
        /// 新增时获得焦点的控件
        /// </summary>
        Control FocusOnAddNewControl = null;

        /// <summary>
        /// 结果表
        /// </summary>
        List<EntityObrResult> dtPatientResulto
        {
            get;
            set;
        }

        /// <summary>
        /// 结果表
        /// </summary>
        List<EntityObrResultImage> imageList
        {
            get;
            set;
        }

        /// <summary>
        /// 面板配置类
        /// </summary>
        PatInputRuntimeSetting UserCustomSetting = null;

        /// <summary>
        /// 是新增还是修改，true：新增 false：修改
        /// </summary>
        private bool isNewOrModify = false;

        string AuditWord = "审核";
        string ReportWord = "报告";

        // 上次操作人员ID
        string LastOperationID = string.Empty;
        /// <summary>
        /// 实现IPatEnter接口的检验报告子类
        /// </summary>
        protected IPatEnter PatEnter;
        /// <summary>
        /// 手工修改病人结果保存时需验证
        /// </summary>
        public bool manualModityResultNeedAudit = false;


        /// <summary>
        /// 开启当前显示病人资料和数据库是否一致检查
        /// </summary>
        public bool checkCurrentPatientInfo = false;

        private bool Lab_EnableNoBarCodeCheck = false;

        private bool Lab_ReportCodeIsNullNotAllowPrint = false;

        private string Lab_NoBarCodeCheckItrExpectList = "";

        private List<string> listDeleteMsgPatId = null;

        protected List<EntityPidReportDetail> listPatCombine;

        //bool Lab_NoCheckSelectCurRow = false;

        bool useMultiThread = true;

        List<EntityDCLPrintParameter> listPrintData_multithread;

        bool IsBtnClose = false; // 是否点击关机按钮关闭窗口

        public FrmMarrowInput()
        {
            InitializeComponent();

            AuditWord = LocalSetting.Current.Setting.AuditWord;
            ReportWord = LocalSetting.Current.Setting.ReportWord;

            GenComandButton();


            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            this.btnPatComment.Click += BtnPatComment_Click;
            this.btnBarInfo.Click += new System.EventHandler(this.btnBarInfo_Click);

            memoEditDescribe.DoubleClick += MemoEditDescribe_DoubleClick;
            memoEditOpinion.DoubleClick += MemoEditOpinion_DoubleClick;


            this.gridViewResult.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gridViewResult_CellValueChanged);
            this.gridViewResult.Click += new System.EventHandler(this.gridViewResult_Click);

            Lab_EnableNoBarCodeCheck = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_EnableNoBarCodeCheck") == "是";
            Lab_ReportCodeIsNullNotAllowPrint = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_ReportCodeIsNullNotAllowPrint") == "是";
            Lab_NoBarCodeCheckItrExpectList = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoBarCodeCheckItrExpectList");
            //Lab_NoCheckSelectCurRow = ConfigHelper.GetSysConfigValueWithoutLogin("Lab_NoCheckSelectCurRow") == "是";

            this.dtPatientResulto = new List<EntityObrResult>();
            this.imageList = new List<EntityObrResultImage>();
            this.controlPatListNew.ItrDataType = this.ItrDataType;


            ceCombine.CombineAdded += new CombineAddedEventHandler(CombineEditor_CombineAdded);
            ceCombine.CombineRemoved += new CombineRemovedEventHandler(CombineEditor_CombineRemoved);
            ceCombine.Reseted += new EventHandler(CombineEditor_Reseted);
        }

        private void GenComandButton()
        {
            try
            {
                AddComandButton("新增(F1)", dcl.client.common.Properties.Resources._32_新增, Add_Click, (int)Keys.F1);
                AddComandButton("镜检(F2)", dcl.client.common.Properties.Resources._32_打印预览, Mirror_Click, (int)Keys.F2);
                AddComandButton("保存(F3)", dcl.client.common.Properties.Resources._32_记录, Save_Click, (int)Keys.F3);
                AddComandButton("删除", dcl.client.common.Properties.Resources._32_删除, Delete_Click, 0);
                AddComandButton("刷新", dcl.client.common.Properties.Resources._32_刷新, Refresh_Click, 0);


                //是否显示一审按钮
                if (UserInfo.GetSysConfigValue("Lab_ShowFirstAuditButton") == "是")
                {
                    string first_audit = this.AuditWord;
                    AddComandButton(first_audit, dcl.client.common.Properties.Resources._32_审核, Audit_Click, 0);
                    string cancel_audit = "取消" + this.AuditWord;
                    AddComandButton(cancel_audit, dcl.client.common.Properties.Resources._32_取消审核, UndoAudit_Click, 0);
                }

                string report = this.ReportWord;
                AddComandButton(report, dcl.client.common.Properties.Resources._32_二次审核, Report_Click, 0);
                string cancel_report = "取消" + this.ReportWord;
                AddComandButton(cancel_report, dcl.client.common.Properties.Resources._32_取消二次审核, UndoReport_Click, 0);


                AddComandButton("审核并打印", dcl.client.common.Properties.Resources._32_打印, AuditPrint_Click, 0);
                AddComandButton("标本进程", dcl.client.common.Properties.Resources._32_模版调用, SpeciPro_Click, 0);
                //AddComandButtonNew("仪器数据", dcl.client.common.Properties.Resources._32_测定数据, Apparatus_Click, 0);

                BarLargeButtonItem printParent = AddComandButton("打印", common.Properties.Resources._32_打印, null, 0);
                BarLargeButtonItem printPreview = GenComandButton("打印预览", common.Properties.Resources._32_打印, PrePrint_Click, 0);
                BarLargeButtonItem printDirect = GenComandButton("直接打印", common.Properties.Resources._32_打印, Print_Click, 0);
                GenDropDownButton(GetBarManagerInstance(), printParent, new BarLargeButtonItem[] { printPreview, printDirect });
                AddComandButton("关闭", dcl.client.common.Properties.Resources._32_关闭, Close_Click, 0);
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// 机器数据类型，此处为骨髓数据
        /// </summary>
        public string ItrDataType
        {
            get { return LIS_Const.InstmtDataType.Marrow; }
        }

        private void FrmMarrowInput_Load(object sender, EventArgs e)
        {
            this.AutoTabKeyPress = false;

            // 获取一审二审名称
            if (!string.IsNullOrEmpty(LocalSetting.Current.Setting.AuditWord))
            {
                this.AuditWord = LocalSetting.Current.Setting.AuditWord;
            }

            if (!string.IsNullOrEmpty(LocalSetting.Current.Setting.ReportWord))
            {
                this.ReportWord = LocalSetting.Current.Setting.ReportWord;
            }
            LoadUserSetting();

            this.controlPatListNew.SetRoundPanelVisble("rpUrgent", false);
            this.controlPatListNew.SetRoundPanelVisble("rpReCheck", false);
            this.controlPatListNew.危急值记录ToolStripMenuItem.Visible = false;
            this.controlPatListNew.批量添加组合ToolStripMenuItem.Visible = false;
            this.controlPatListNew.更新为传染病toolStripMenuItem.Visible = false;

            this.tabPatientInfo.SelectedTabPageIndex = 2;
            this.tabPatientInfo.SelectedTabPageIndex = 1;
            this.tabPatientInfo.SelectedTabPageIndex = 0;
            this.ceCombine.listRepDetail = new List<EntityPidReportDetail>(); // 初始化组合内容
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
            this.controlPatListNew.ApplySetting(this.UserCustomSetting.PatListPanel);

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
            foreach (Control control in this.panelPatInfo.Controls)
            {
                if (control != null && control.Visible == true)
                {
                    if (control.Name == this.UserCustomSetting.PatInfoPanel.FindFocusOnAddNewControlName())
                    {
                        FocusOnAddNewControl = control;
                    }
                }
            }

        }

        /// <summary>
        /// 新增检验记录
        /// </summary>
        private void Add_Click(object sender, EventArgs e)
        {
            string strInstructID = this.controlPatListNew.ItrID;
            if (string.IsNullOrEmpty(strInstructID))
            {
                MessageDialog.Show("请选择仪器");
                this.controlPatListNew.txtPatInstructment.Focus();
                return;
            }
            //this.CommandtoolStrip.Focus();
            this.controlPatListNew.AddNew();
            AddNew();
            txtPatSid.Text = GetMaxSID();
            txtPatIdType.valueMember = txtPatIdType.dtSource.Find(x => x.IdtName == "条码号")?.IdtId;
            this.isNewOrModify = true;
            this.dtPatientResulto = new List<EntityObrResult>();
            BindResults();
            this.imageList = new List<EntityObrResultImage>();
            UpdatePictures(null);

            EntityPidReportMain CurrentPatInfo = new EntityPidReportMain();
            this.FillEntityFromUI(CurrentPatInfo);
            bsPat.DataSource = CurrentPatInfo;
            if (FocusOnAddNewControl != null)
            {
                this.ActiveControl = FocusOnAddNewControl;
                FocusOnAddNewControl.Focus();
            }
            DateTime dtServer = ServerDateTime.GetServerDateTime();
            txtPatDate.DateTime = dtServer;
        }

        /// <summary>
        /// 新增记录
        /// </summary>
        private void AddNew()
        {
            //根据设置保留还是记忆界面原有值

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
            txtPatSDate.EditValue = dtToday;// DateTime.Now;
            txtPatReachDate.EditValue = dtToday;// DateTime.Now;
            txtPatRecDate.EditValue = dtToday;// DateTime.Now;
            txtPatSampleDate.EditValue = dtToday;// DateTime.Now;
            txtPatReceiveDate.EditValue = dtToday;// DateTime.Now;
            this.txtBirthday.EditValue = string.Empty;

            //增加申请时间为当前申请时间
            txtPatApplyDate.EditValue = dtToday;// DateTime.Now;
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
            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSamRem"))
            {
                txtPatSamRem.displayMember = "";
            }

            txtPatBarCode.Text = "";

            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("textAgeInput1"))
            {
                this.textAgeInput1.AgeValueText = string.Empty;
            }
            if (ceCombine.listRepDetail != null)
                ceCombine.listRepDetail.Clear();//清空原来的
            ceCombine.RefreshEditBoxText();

            fpat_work.Text = "";
            fpat_tel.Text = "";
            fpat_email.Text = "";
            fpat_address.Text = "";
            fpat_height.Text = "";
            fpat_weight.Text = "";
            fpat_pre_week.Text = "";
            fpat_sample_part.Text = "";
            fpat_exp2.Text = "";
            this.txtPatAplyNo.Text = string.Empty;
            this.txtPatPid.Text = string.Empty;
            this.txtPatUpid.Text = string.Empty;
            this.txtPatNotice.Text = string.Empty;
            this.txt_pat_host_order.Text = string.Empty;

            this.txtPatEmpCompanyName.Text = string.Empty;
            this.txt_pat_emp_id.Text = string.Empty;
            this.txtPatAdmissTime.Text = "0";
            txtAuditName.Text = string.Empty;
            txtAuditTime.Text = string.Empty;
            txtReportName.Text = string.Empty;
            if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatInspetor") || string.IsNullOrEmpty(txtPatInspetor.valueMember))
            {
                this.txtPatInspetor.valueMember = null;
                this.txtPatInspetor.displayMember = null;
                defaultInspector();
            }

            memoEditDescribe.Text = string.Empty;
            memoEditOpinion.Text = string.Empty;
        }

        /// <summary>
        /// 设置默认检验者信息
        /// </summary>
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
        /// 获取仪器下一个样本号
        /// </summary>
        /// <returns></returns>
        protected string GetMaxSID()
        {
            try
            {
                if (txtPatDate.EditValue == null) return "1";
                DateTime dtPatDate = (DateTime)this.txtPatDate.EditValue;
                string strInstructID = this.controlPatListNew.ItrID;
                if (this.controlPatListNew.gridControl1.Focused)
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
        /// 点击镜检
        /// </summary>
        private void Mirror_Click(object sender, EventArgs e)
        {
            EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
            if (CurrentPatInfo == null)
            {
                MessageDialog.Show("请选择需要镜检的记录");
                return;
            }
            if (CurrentPatInfo.RepStatus.ToString() != "0" || CurrentPatInfo.RepInitialFlag.ToString() != "0")
            {
                MessageDialog.Show(string.Format("该记录已{0}或已{1}，无法镜检！", this.AuditWord, this.ReportWord));
                return;
            }
            if (this.txtPatSampleType.valueMember == "" || this.txtPatSampleType.valueMember == null)
            {
                MessageDialog.Show("请输入标本类别！", "提示");
                txtPatSampleType.Focus();
                return;
            }
            FrmMarrowMark mark = new FrmMarrowMark();
            mark.SampleType = this.txtPatSampleType.displayMember;
            mark.RepId = CurrentPatInfo.RepId;
            if (this.dtPatientResulto != null && this.dtPatientResulto.Count > 0)
            {

                mark.dtPatientResulto = this.dtPatientResulto;
                if (this.imageList != null && this.imageList.Count > 0)
                {
                    mark.result_images = this.imageList;
                }
            }
            mark.ShowDialog();
            if (mark.IsSave && mark.cellMarkList != null && mark.cellMarkList.Count > 0)
            {
                List<EntityObrResultImage> result_images = mark.resultImageList;
                List<EntityObrResult> obr_results = SetBsResult(mark.cellMarkList);
                UpdateResults(obr_results, result_images);
                Save_Click(null, null);
            }
        }

        /// <summary>
        /// 将镜检界面结果返回给检验项目
        /// </summary>
        /// <param name="obr_results"></param>
        /// <param name="result_images"></param>
        private void UpdateResults(List<EntityObrResult> obr_results, List<EntityObrResultImage> result_images)
        {
            if (this.dtPatientResulto != null)
            {
                foreach (EntityObrResult result in obr_results)
                {
                    EntityObrResult cur_result = this.dtPatientResulto.FirstOrDefault(a => a.ItmId == result.ItmId);
                    if (cur_result != null)
                    {
                        cur_result.ObrBldValue = result.ObrBldValue;
                        cur_result.ObrBoneValue = result.ObrBoneValue;
                    }
                }
            }

            this.imageList = result_images;
            UpdatePictures(this.imageList);
            BindResults();

        }

        /// <summary>
        /// 镜检后传出数据
        /// </summary>
        /// <param name="itm_id"></param>
        private List<EntityObrResult> SetBsResult(List<EntityObrCellsMark> cellmarklist)
        {
            List<EntityObrResult> entityObrResult = new List<EntityObrResult>();
            List<string> ItemId = new List<string>();
            foreach (EntityObrCellsMark cellmark in cellmarklist)
            {
                ItemId.Add(cellmark.ItemId);
            }
            HashSet<string> hs = new HashSet<string>(ItemId);
            foreach (string Id in hs)
            {
                EntityObrResult obrResult = new EntityObrResult();
                var celllist = cellmarklist.FindAll(x => x.ItemId == Id);
                if (celllist != null && celllist.Count > 0)
                {
                    var cellmarkBone = cellmarklist.FindAll(x => x.ItemId == Id);
                    if (cellmarkBone != null && cellmarkBone.Count > 0 && cellmarkBone.Find(x => !string.IsNullOrEmpty(x.ObrBoneValue)) != null)
                    {
                        string ObrBoneValue = cellmarkBone.Find(x => !string.IsNullOrEmpty(x.ObrBoneValue)).ObrBoneValue.ToString();
                        if (!string.IsNullOrEmpty(ObrBoneValue))
                            obrResult.ObrBoneValue = ObrBoneValue;
                    }
                    var cellmarkBld = cellmarklist.FindAll(x => x.ItemId == Id);
                    if (cellmarkBld != null && cellmarkBld.Count > 0 && cellmarkBld.Find(x => !string.IsNullOrEmpty(x.ObrBldValue)) != null)
                    {
                        string ObrBldValue = cellmarkBld.Find(x => !string.IsNullOrEmpty(x.ObrBldValue)).ObrBldValue.ToString();
                        if (!string.IsNullOrEmpty(ObrBldValue))
                            obrResult.ObrBldValue = ObrBldValue;
                    }
                    obrResult.ItmId = Id;
                    entityObrResult.Add(obrResult);
                }
            }
            return entityObrResult;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save_Click(object sender, EventArgs e)
        {
            //this.CommandtoolStrip.Focus();
            bsPat.EndEdit();
            gridViewResult.CloseEditor();
            BsResult.EndEdit();

            if (!CheckCanSaveReport())
            {
                return;
            }
            if (SaveOrUpdateReport())
            {
                MessageDialog.ShowAutoCloseDialog("保存成功！");
                this.controlPatListNew.RefreshPatients();
                this.isNewOrModify = false;
            }

        }

        /// <summary>
        /// 判断是否可以保存
        /// </summary>
        /// <returns></returns>
        private bool CheckCanSaveReport()
        {
            if (this.isNewOrModify) // 新增
            {

            }
            else // 更新
            {
                if (bsPat.Current != null)
                {
                    EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                    if (CurrentPatInfo.RepStatus.ToString() != "0" || CurrentPatInfo.RepInitialFlag.ToString() != "0")
                    {
                        MessageDialog.Show(string.Format("该记录已{0}或已{1}，无法保存！", this.AuditWord, this.ReportWord));
                        return false;
                    }
                    else
                    {
                        // 显示报告状态
                    }
                }
            }
            if (this.txtPatDate.EditValue == null)
            {
                MessageDialog.Show("请输入录入日期！", "提示");
                txtPatDate.Focus();
                return false;
            }
            if (this.controlPatListNew.ItrID == null || string.IsNullOrWhiteSpace(this.controlPatListNew.ItrID))
            {
                if (UserInfo.GetSysConfigValue("Lab_TypeSelect") != "是")
                {
                    this.controlPatListNew.FocusItr();
                    MessageDialog.Show("请选择仪器！", "提示");
                    return false;
                }
            }
            try
            {
                Convert.ToInt64(txtPatSid.Text);
            }
            catch (Exception)
            {
                MessageDialog.Show("样本号类型错误或为空！", "提示");
                txtPatSid.Focus();
                return false;
            }


            if (this.txtPatSampleType.valueMember == "" || this.txtPatSampleType.valueMember == null)
            {
                MessageDialog.Show("请输入标本类别！", "提示");
                txtPatSampleType.Focus();
                return false;
            }

            if (!Compare.IsNullOrDBNull(this.txtPatSDate.EditValue) //送检时间
                && !Compare.IsNullOrDBNull(this.txtPatRecDate.EditValue)) //检验时间
            {
                DateTime sample_send_date = (DateTime)this.txtPatSDate.EditValue;
                DateTime pat_jy_date = (DateTime)this.txtPatRecDate.EditValue;

                if (sample_send_date > pat_jy_date)
                {
                    MessageDialog.Show("[送检时间]不能大于[检验时间]");
                    return false;
                }
            }

            if ((this.txtPatSex.valueMember == null
                 || this.txtPatSex.displayMember == null
                 || this.txtPatSex.displayMember.ToString().Trim(null) == string.Empty) &&
                UserInfo.GetSysConfigValue("Lab_NotNull_Sex") == "是")
            {
                MessageDialog.Show("请输入[性别]", "提示");
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
                MessageDialog.Show("请输入[检验者]", "提示");
                this.ActiveControl = this.txtPatInspetor;
                this.txtPatInspetor.Focus();
                return false;
            }

            if (ceCombine.listRepDetail == null || ceCombine.listRepDetail.Count == 0)
            {
                MessageDialog.Show("请输入组合项目！", "提示");
                ceCombine.Focus();
                return false;
            }
            //未输入年龄时是否提示
            if (this.textAgeInput1.AgeToMinute <= 0)
            {
                if (MessageDialog.Show("当前资料未输入年龄，是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }

            if (this.isNewOrModify)
            {
                EntityPatientQC qc = new EntityPatientQC();
                qc.ListItrId = new List<string> { this.controlPatListNew.ItrID };
                qc.DateStart = txtPatDate.DateTime.Date;
                qc.DateEnd = txtPatDate.DateTime.Date.AddDays(1).AddSeconds(-1);
                qc.RepSid = txtPatSid.Text.ToString();
                qc.RepBarCode = txtPatID.Text.ToString();
                List<EntityPidReportMain> plist = new ProxyPidReportMain().Service.PatientQuery(qc);
                if (plist.Count > 0)
                {
                    MessageDialog.Show("已存在该样本，请勿重复添加", "提示");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 保存或者更新报告
        /// </summary>
        /// <returns></returns>
        private bool SaveOrUpdateReport()
        {
            bsPat.EndEdit();
            gridViewResult.CloseEditor();
            BsResult.EndEdit();

            EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
            Caller.IPAddress = UserInfo.ip;
            Caller.LoginID = UserInfo.loginID;
            Caller.LoginName = UserInfo.userName;

            // 获取报告信息信息
            EntityPidReportMain report_main = GetReportMain();

            // 保存细胞数信息
            List<EntityObrResult> obr_results = GetObrResults(report_main);

            // 保存图片结果
            List<EntityObrResultImage> result_images = GetResultImages(report_main);

            try
            {
                if (isNewOrModify)
                {
                    bool result = new ProxyMarrowEnter().Service.InsertMarrowPatResult(Caller, report_main, obr_results, result_images);
                    return result;
                }
                else
                {
                    bool result = new ProxyMarrowEnter().Service.UpdateMarrowPatResult(Caller, report_main, obr_results, result_images);
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show("保存报告失败！", ex.Message);
                Lib.LogManager.Logger.LogException("SaveOrUpdateReport", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取主报告单
        /// </summary>
        /// <returns></returns>
        private EntityPidReportMain GetReportMain()
        {
            EntityPidReportMain report_main = null;
            if (isNewOrModify)
            {
                if (bsPat.Current != null)
                {
                    EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                    report_main = CurrentPatInfo;
                    report_main.RepSid = txtPatSid.Text.ToString();
                    report_main.RepItrId = this.controlPatListNew.ItrID;
                    if (!string.IsNullOrEmpty(memoEditDescribe.Text))
                    {
                        report_main.RepDiscribe = memoEditDescribe.Text;
                    }
                    if (!string.IsNullOrEmpty(memoEditOpinion.Text))
                    {
                        report_main.RepComment = memoEditOpinion.Text;
                    }
                    if (this.ceCombine.listRepDetail != null && this.ceCombine.listRepDetail.Count > 0)
                    {
                        report_main.ListPidReportDetail = EntityManager<EntityPidReportDetail>.ListClone(this.ceCombine.listRepDetail);
                    }
                    report_main.RepInDate = Convert.ToDateTime(this.txtPatDate.EditValue);
                    report_main.PidComName = ceCombine.Text;
                }
            }
            else
            {
                if (bsPat.Current != null)
                {
                    EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                    report_main = CurrentPatInfo;
                    report_main.RepSid = txtPatSid.Text.ToString();
                    report_main.RepDiscribe = memoEditDescribe.Text;
                    report_main.RepComment = memoEditOpinion.Text;
                    if (this.ceCombine.listRepDetail != null && this.ceCombine.listRepDetail.Count > 0)
                    {
                        report_main.ListPidReportDetail = EntityManager<EntityPidReportDetail>.ListClone(this.ceCombine.listRepDetail);
                    }
                    report_main.PidComName = ceCombine.Text;
                }
            }

            return report_main;
        }


        /// <summary>
        /// 把界面值放入DataRow
        /// </summary>
        /// <param name="dr"></param>
        private void FillEntityFromUI(EntityPidReportMain patient)
        {
            patient.RepSid = this.txtPatSid.Text;
            patient.RepInDate = Convert.ToDateTime(this.txtPatDate.EditValue);
            patient.RepItrId = this.controlPatListNew.ItrID;
            patient.ItrName = this.controlPatListNew.ItrName;

            if (!string.IsNullOrEmpty(memoEditDescribe.Text))
            {
                patient.RepDiscribe = memoEditDescribe.Text;
            }
            if (!string.IsNullOrEmpty(memoEditOpinion.Text))
            {
                patient.RepComment = memoEditOpinion.Text;
            }
            if (this.ceCombine.listRepDetail != null && this.ceCombine.listRepDetail.Count > 0)
            {
                patient.ListPidReportDetail = EntityManager<EntityPidReportDetail>.ListClone(this.ceCombine.listRepDetail);
            }
            patient.RepRemark = fpat_exp2.Text;

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

            if (this.txtPatReceiveDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatReceiveDate.EditValue.ToString()))
            {
                patient.SampApplyDate = Convert.ToDateTime(this.txtPatReceiveDate.EditValue);
            }

            if (this.txtPatRecDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatRecDate.EditValue.ToString()))
            {
                patient.RepPrintDate = Convert.ToDateTime(this.txtPatRecDate.EditValue);
            }
            if (this.fpat_exp2.EditValue != null && !string.IsNullOrEmpty(this.fpat_exp2.EditValue.ToString()))
            {
                patient.RepRemark = this.fpat_exp2.EditValue.ToString();
            }

            if (!string.IsNullOrEmpty(this.fpat_sample_part.EditValue.ToString()))
            {
                patient.CollectionPart = this.fpat_sample_part.EditValue.ToString();
            }
            patient.PidApplyNo = this.txtPatAplyNo.Text;

            patient.RepBarCode = this.txtPatBarCode.Text;

            if (this.txtPatApplyDate.EditValue != null && !string.IsNullOrEmpty(this.txtPatApplyDate.EditValue.ToString()))
            {
                patient.SampReceiveDate = Convert.ToDateTime(this.txtPatApplyDate.EditValue);
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

            if (DictInstrmt.Instance.GetItrHostFlag(this.controlPatListNew.ItrID) == 2)
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
            patient.PidUniqueId = txtPatUpid.Text;

            patient.RepCheckUserId = txtPatInspetor.valueMember;
            patient.RepRemark = fpat_exp2.Text;
            patient.PidRemark = this.txtPatSampleState.displayMember;


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
        /// 获取图片结果
        /// </summary>
        /// <returns></returns>
        private List<EntityObrResultImage> GetResultImages(EntityPidReportMain report_main)
        {
            if (report_main == null)
            {
                return null;
            }

            if (this.imageList == null || this.imageList.Count == 0)
            {
                return null;
            }
            foreach (EntityObrResultImage img in this.imageList)
            {
                img.ObrFlag = 1;
            }
            return this.imageList;
        }

        /// <summary>
        /// 获取细胞数信息
        /// </summary>
        /// <returns></returns>
        private List<EntityObrResult> GetObrResults(EntityPidReportMain report_main)
        {
            if (report_main == null)
            {
                return null;
            }
            if (this.dtPatientResulto == null || this.dtPatientResulto.Count == 0)
            {
                return null;
            }

            if (this.dtPatientResulto != null && this.dtPatientResulto.Count > 0)
            {
                foreach (EntityObrResult result in this.dtPatientResulto)
                {
                    result.ObrItrId = report_main.RepItrId;
                    result.ObrSid = report_main.RepSid;
                }
            }
            return this.dtPatientResulto;
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                OnDeleteBatch();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("删除失败！", ex.Message);
            }
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <returns></returns>
        public bool OnDeleteBatch()
        {
            bool result = false;
            //获取勾选的病人
            bool delflag = false;
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();

            if (UserInfo.GetSysConfigValue("BathOrSingleDelFlag").Equals("是"))
            {
                listPats = this.controlPatListNew.GetCheckedPatients();
                delflag = true;
            }
            else
            {
                delflag = false;
                if (this.controlPatListNew.CurrentPatient != null)
                {
                    listPats.Add(this.controlPatListNew.CurrentPatient);
                }
            }

            if (listPats.Count > 0)
            {
                if (listPats.Count >= 20)
                {
                    if (MessageDialog.Show(string.Format("您将要删除 {0} 条病人记录，是否继续？", listPats.Count), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return false;
                    }
                }
                else
                {
                    string show = string.Empty;
                    foreach (EntityPidReportMain dr in listPats)
                    {
                        if (dr.RepStatus != null && dr.RepStatus != 0)
                        {
                            show = string.Format("姓名:{0}, 样本号: {1}, 已{2}，不能删除",
                                dr.PidName != null ? dr.PidName.ToString() : " ",
                                !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty,
                                this.AuditWord
                                );
                            MessageDialog.Show(show);
                            return false;
                        }
                        show += string.Format("姓名:{0},样本号: {1} \r\n", !string.IsNullOrEmpty(dr.PidName) ? dr.PidName : " ", !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty);
                    }
                    if (MessageDialog.Show(string.Format("您将要删除\r\n{0}的记录，是否继续？", show), "确认", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return false;
                    }
                }

                //身份验证
                FrmCheckPassword frmCheck = new FrmCheckPassword("身份验证 - 删除", LIS_Const.BillPopedomCode.Delete, "", "");
                frmCheck.txtLoginid.Text = LastOperationID;
                if (!string.IsNullOrEmpty(LastOperationID)) // 删除已经删除过
                {
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                }
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                    bool del_result_flag = false;
                    if (LisSysParam.lab_del_flag == LIS_Const.STRUCT_LAB_DEL_FLAG.DEL_ALTER)
                    {
                        DialogResult r = MessageDialog.Show("是否连结果一起删除？", "提示", MessageBoxButtons.YesNo);
                        if (r == DialogResult.Yes) { del_result_flag = true; };
                        if (r == DialogResult.No) { del_result_flag = false; };
                    }
                    //调用子类删除
                    bool del_flag = new ProxyMarrowEnter().Service.DeleteMarrowPatResult(caller, listPats, del_result_flag);

                    if (del_flag)
                    {
                        this.controlPatListNew.RefreshPatients();
                    }
                    LastOperationID = frmCheck.OperatorID;
                    MessageDialog.ShowAutoCloseDialog("报告记录删除成功！");
                }

                return true;
            }
            else
            {
                if (delflag == true)
                {
                    MessageDialog.Show("请在病人索引中勾选需要删除的记录", "提示");
                }
                else
                {
                    MessageDialog.Show("请在病人索引中选中需要删除的记录", "提示");
                }
            }
            return result;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh_Click(object sender, EventArgs e)
        {
            this.controlPatListNew.RefreshPatients();
        }


        /// <summary>
        /// 审核（一审）
        /// </summary>
        private void Audit_Click(object sender, EventArgs e)
        {
            gridViewResult.CloseEditor();
            this.bsPat.EndEdit();
            try
            {
                OnAudit();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(string.Format("{0}失败！", this.AuditWord), ex.Message);
            }
        }

        /// <summary>
        /// （一审）逻辑
        /// </summary>
        /// <returns></returns>
        public bool OnAudit()
        {
            bool result = false;
            //获取勾选的病人
            bool delflag = false;
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            listPats = this.controlPatListNew.GetCheckedPatients();

            if (listPats.Count > 0)
            {
                List<string> listPatIds = new List<string>();
                foreach (EntityPidReportMain dr in listPats)
                {
                    if (dr.RepStatus != null && dr.RepStatus != 0)
                    {
                        string show = string.Format("姓名:{0}, 样本号: {1}, 非初始状态，不能{2}",
                            dr.PidName != null ? dr.PidName.ToString() : " ", !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty, this.AuditWord);
                        MessageDialog.Show(show);
                        return false;
                    }
                    listPatIds.Add(dr.RepId);
                }

                String pat_chk_date = ServerDateTime.GetServerDateTime().ToString("yyyy-MM-dd HH:mm:ss");// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (listPatIds.Count == 0)
                {
                    MessageDialog.Show(string.Format("未选择{0}数据！", AuditWord), "提示");
                    return false;
                }

                EntityOperationResultList result_message = new ProxyMarrowEnter().Service.BatchAuditCheck(listPatIds, "1");

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
                    patlist = listPatIds;

                if (patlist == null || patlist.Count == 0)
                {
                    return false;
                }

                //身份验证
                string audit_str = "身份验证 - " + this.AuditWord;
                FrmCheckPassword frmCheck = new FrmCheckPassword(audit_str, LIS_Const.BillPopedomCode.Audit, "", "");
                frmCheck.txtLoginid.Text = LastOperationID;
                if (!string.IsNullOrEmpty(LastOperationID))
                {
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                }
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                    //调用子类删除
                    bool audit_flag = new ProxyMarrowEnter().Service.MarrowAudit(listPatIds, caller);

                    if (audit_flag)
                    {
                        this.controlPatListNew.RefreshPatients();
                    }
                    LastOperationID = frmCheck.OperatorID;
                    MessageDialog.ShowAutoCloseDialog(string.Format("{0}成功！", this.AuditWord));
                }
                return true;
            }
            else
            {
                if (delflag == true)
                {
                    MessageDialog.Show(string.Format("请在病人索引中勾选需要{0}的记录", this.AuditWord), "提示");
                }
                else
                {
                    MessageDialog.Show(string.Format("请在病人索引中选中需要{0}的记录", this.AuditWord), "提示");
                }
            }
            return result;
        }



        /// <summary>
        /// 取消审核(一审、检验)
        /// </summary>
        private void UndoAudit_Click(object sender, EventArgs e)
        {
            gridViewResult.CloseEditor();
            this.bsPat.EndEdit();
            try
            {
                UndoAudit();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(string.Format("取消{0}失败！", this.AuditWord), ex.Message);
            }
        }

        /// <summary>
        /// 批量取消审核(一审、检验)
        /// </summary>
        /// <returns></returns>
        public bool UndoAudit()
        {
            bool result = false;
            //获取勾选的病人
            bool delflag = false;
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            listPats = this.controlPatListNew.GetCheckedPatients();

            if (listPats.Count > 0)
            {
                List<string> listPatIds = new List<string>();
                foreach (EntityPidReportMain dr in listPats)
                {
                    if (dr.RepStatus != 1)
                    {
                        string show = string.Format("姓名:{0}, 样本号: {1}, 非{2}状态，不能取消{2}",
                            dr.PidName != null ? dr.PidName.ToString() : " ",
                            !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty,
                            this.AuditWord
                            );
                        MessageDialog.Show(show);
                        return false;
                    }
                    listPatIds.Add(dr.RepId);
                }

                //身份验证
                string cancel_audit = "身份验证 - 取消" + this.AuditWord;
                FrmCheckPassword frmCheck = new FrmCheckPassword(cancel_audit, LIS_Const.BillPopedomCode.UndoAudit, "", "");
                frmCheck.txtLoginid.Text = LastOperationID;
                if (!string.IsNullOrEmpty(LastOperationID))
                {
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                }
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                    //调用子类删除
                    bool audit_flag = new ProxyMarrowEnter().Service.UndoMarrowAudit(listPatIds, caller);

                    if (audit_flag)
                    {
                        this.controlPatListNew.RefreshPatients();
                    }
                    LastOperationID = frmCheck.OperatorID;
                    MessageDialog.ShowAutoCloseDialog(string.Format("取消{0}成功！", this.AuditWord));
                }
                return true;
            }
            else
            {
                if (delflag == true)
                {
                    MessageDialog.Show(string.Format("请在病人索引中勾选需要取消{0}的记录", this.AuditWord), "提示");
                }
                else
                {
                    MessageDialog.Show(string.Format("请在病人索引中选中需要取消{0}的记录", this.AuditWord), "提示");
                }
            }
            return result;
        }




        /// <summary>
        /// 报告
        /// </summary>
        private void Report_Click(object sender, EventArgs e)
        {
            gridViewResult.CloseEditor();
            this.bsPat.EndEdit();
            try
            {
                Report();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(string.Format("{0}失败！", this.ReportWord), ex.Message);
            }
        }

        /// <summary>
        /// 报告逻辑
        /// </summary>
        /// <returns></returns>
        public bool Report()
        {
            bool result = false;
            //获取勾选的病人
            bool delflag = false;
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            listPats = this.controlPatListNew.GetCheckedPatients();

            if (listPats.Count > 0)
            {
                List<string> listPatIds = new List<string>();
                foreach (EntityPidReportMain dr in listPats)
                {
                    //if (dr.RepStatus != 1)
                    //{
                    //    string show = string.Format("姓名:{0}, 样本号: {1}, 非{2}状态，不能{3}",
                    //        dr.PidName != null ? dr.PidName.ToString() : " ",
                    //        !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty,
                    //        this.AuditWord, this.ReportWord
                    //        );
                    //    MessageDialog.Show(show);
                    //    return false;
                    //}
                    listPatIds.Add(dr.RepId);
                }


                EntityOperationResultList result_message = new ProxyMarrowEnter().Service.BatchAuditCheck(listPatIds, "2");

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
                    patlist = listPatIds;

                if (patlist == null || patlist.Count == 0)
                {
                    return false;
                }

                //身份验证
                string report_str = "身份验证 - " + this.ReportWord;
                FrmCheckPassword frmCheck = new FrmCheckPassword(report_str, LIS_Const.BillPopedomCode.Report, "", "");
                frmCheck.txtLoginid.Text = LastOperationID;
                if (!string.IsNullOrEmpty(LastOperationID))
                {
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                }
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                    //调用子类删除
                    bool audit_flag = new ProxyMarrowEnter().Service.MarrowReport(listPatIds, caller);

                    if (audit_flag)
                        this.controlPatListNew.RefreshPatients();
                    LastOperationID = frmCheck.OperatorID;
                    MessageDialog.ShowAutoCloseDialog(string.Format("{0}成功！", this.ReportWord));
                }
                return true;
            }
            else
            {
                if (delflag == true)
                {
                    MessageDialog.Show(string.Format("请在病人索引中勾选需要{0}的记录", this.ReportWord), "提示");
                }
                else
                {
                    MessageDialog.Show(string.Format("请在病人索引中选中需要{0}的记录", this.ReportWord), "提示");
                }
            }
            return result;
        }




        /// <summary>
        /// 取消报告
        /// </summary>
        private void UndoReport_Click(object sender, EventArgs e)
        {
            gridViewResult.CloseEditor();
            this.bsPat.EndEdit();
            try
            {
                UndoReport();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(string.Format("取消{0}失败！", this.ReportWord), ex.Message);
            }
        }

        /// <summary>
        /// 取消报告逻辑
        /// </summary>
        /// <returns></returns>
        public bool UndoReport()
        {
            bool result = false;
            //获取勾选的病人
            bool delflag = false;
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            listPats = this.controlPatListNew.GetCheckedPatients();

            if (listPats.Count > 0)
            {
                List<string> listPatIds = new List<string>();
                foreach (EntityPidReportMain dr in listPats)
                {
                    if (dr.RepStatus != 2 && dr.RepStatus != 4) // 不是已报告和已打印状态
                    {
                        string show = string.Format("姓名:{0}, 样本号: {1}, 非{2}状态，不能取消{2}",
                            dr.PidName != null ? dr.PidName.ToString() : " ",
                            !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty,
                            this.ReportWord
                            );
                        MessageDialog.Show(show);
                        return false;
                    }
                    listPatIds.Add(dr.RepId);
                }

                EntityOperationResultList result_message = new ProxyMarrowEnter().Service.BatchUndoReportCheck(listPatIds, null);

                List<string> patlist = new List<string>();
                DialogResult drReturn = new DialogResult();

                if (result_message.FailedCount > 0)
                {
                    //显示审核检查提示窗口
                    AuditCheckResultViwer resultviwer = new AuditCheckResultViwer(result_message, EnumOperationCode.UndoReport);
                    drReturn = resultviwer.ShowDialog();
                    patlist = resultviwer.GetSelectedID();//获取需要继续操作的病人ID集合
                }
                else//全部检查通过
                    patlist = listPatIds;

                //身份验证
                string cancel_report_str = "身份验证 - 取消" + this.ReportWord;
                FrmCheckPassword frmCheck = new FrmCheckPassword(cancel_report_str, LIS_Const.BillPopedomCode.UndoReport, "", "");
                frmCheck.txtLoginid.Text = LastOperationID;
                if (!string.IsNullOrEmpty(LastOperationID))
                {
                    frmCheck.ActiveControl = frmCheck.txtPassword;
                }
                DialogResult dig = frmCheck.ShowDialog();

                if (dig == DialogResult.OK)
                {
                    EntityRemoteCallClientInfo caller = Util.ToCallerInfo(frmCheck.OperatorID, string.Empty, frmCheck.OperatorName);
                    //调用子类删除
                    bool audit_flag = new ProxyMarrowEnter().Service.UndoMarrowReport(patlist, caller);

                    if (audit_flag)
                    {
                        this.controlPatListNew.RefreshPatients();
                    }
                    LastOperationID = frmCheck.OperatorID;
                    MessageDialog.ShowAutoCloseDialog(string.Format("取消{0}成功！", this.ReportWord));
                }

                return true;
            }
            else
            {
                if (delflag == true)
                {
                    MessageDialog.Show(string.Format("请在病人索引中勾选需要取消{0}的记录", this.ReportWord), "提示");
                }
                else
                {
                    MessageDialog.Show(string.Format("请在病人索引中选中需要取消{0}的记录", this.ReportWord), "提示");
                }
            }
            return result;
        }


        /// <summary>
        /// 审核打印
        /// </summary>
        private void AuditPrint_Click(object sender, EventArgs e)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            listPats = this.controlPatListNew.GetCheckedPatients();

            if (listPats.Count > 0)
            {
                foreach (EntityPidReportMain dr in listPats)
                {
                    if (dr.RepStatus != 1)
                    {
                        string show = string.Format("姓名:{0}, 样本号: {1}, 非{2}状态，不能审核并打印",
                            dr.PidName != null ? dr.PidName.ToString() : " ",
                            !string.IsNullOrEmpty(dr.RepSid) ? dr.RepSid : string.Empty,
                            this.AuditWord
                            );
                        MessageDialog.Show(show);
                        return;
                    }
                }
            }


            //OnReportBatch(true);

            //设置焦点行问题
            setControlPatListFocus("report_isFocusOnTheFirstRow");
        }


        /// <summary>
        /// 批量报告
        /// </summary>
        protected void OnReportBatch(bool isprint)
        {
            List<EntityPidReportMain> drs = this.controlPatListNew.GetCheckedPatients();
            if (drs.Count > 0)
            {
                PatEnterUILogic uil = new PatEnterUILogic();//this, PatEnter.ItrDataType
                uil.strLastOperationID = LastOperationID;
                if (checkCurrentPatientInfo)
                {
                    if (CheckCurrentPatientResInfo(uil)) return;
                }
                uil.ItrName = controlPatListNew.txtPatInstructment.displayMember;
                uil.Itr_ID = controlPatListNew.txtPatInstructment.valueMember;
                bool success = uil.ReoprtBatch(drs);

                if (success)
                {
                    string printOnReport = UserInfo.GetSysConfigValue("PrintOnReport");

                    if (printOnReport == "是" || controlPatListNew.IsAtuoPrint || isprint)
                    {
                        MessageDialog.ShowAutoCloseDialog("审核成功，正在打印......", 2m);
                        this.OnPatPrint(null, false);
                    }

                    if (UserInfo.GetSysConfigValue("Lab_ClearCheckedAfterOperation") == "是")
                    {
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

        private bool CheckCurrentPatientResInfo(PatEnterUILogic uil)
        {
            EntityPidReportMain row = controlPatListNew.CurrentPatient;
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

        /// <summary>
        /// 根据系统配置的代码设置是否将报告模块中的获焦行在首行还是尾行
        /// </summary>
        /// <param name="configCode"></param>
        public void setControlPatListFocus(string configCode)
        {
            if (ConfigHelper.GetSysConfigValueWithoutLogin(configCode) == "是")
            {
                controlPatListNew.gridViewPatientList.FocusedRowHandle = 0;
                int i = controlPatListNew.gridViewPatientList.RowCount;

                EntityPidReportMain dr = controlPatListNew.CurrentPatient;
                if (dr != null)
                {
                    controlPatListNew.LocatePatientByPatID(dr.RepId.ToString(), true);
                }
            }

        }

        /// <summary>
        /// 显示标本进程
        /// </summary>
        private void SpeciPro_Click(object sender, EventArgs e)
        {
            string itr_id = controlPatListNew.txtPatInstructment.valueMember;
            string itr_name = controlPatListNew.txtPatInstructment.displayMember;

            string type_id = controlPatListNew.TypeID;//物理组ID
            string type_name = controlPatListNew.TypeName;//物理组名称

            if (!string.IsNullOrEmpty(type_id))
            {
                FrmMonitor frm = new FrmMonitor(itr_id, itr_name, type_id, type_name, Convert.ToDateTime(this.txtPatDate.EditValue));
                frm.ShowDialog();
            }
        }


        /// <summary>
        /// 打印
        /// </summary>
        private void Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lab_ReportCodeIsNullNotAllowPrint)
                {
                    //打印前,刷新当前列表信息.令其状态保持跟数据库一致
                    string prtTemplate = DictInstrmt.Instance.GetItrPrtTemplate(controlPatListNew.txtPatInstructment.valueMember);
                    if (prtTemplate == string.Empty)
                    {

                    }
                    else
                    {
                        controlPatListNew.listCheck = controlPatListNew.GetCheckedPatients();
                        if (controlPatListNew.listCheck.Count > 0)
                        {
                            controlPatListNew.RefreshPatients();
                            //保存刷新后记录上次勾选
                            controlPatListNew.SelectLastCheckPatients();
                        }
                        else
                        {
                            lis.client.control.MessageDialog.Show("请勾选需要打印的数据", "提示");
                            return;
                        }


                        //List<EntityPidReportMain> listPatient = this.controlPatListNew.GetCheckedPatients();
                        //if (listPatient.Count > 0)
                        //{
                        //    List<string> tempPatIDs = new List<string>();//记录勾选中的pat_id
                        //    foreach (EntityPidReportMain dr in listPatient)
                        //    {
                        //        tempPatIDs.Add(dr.RepId.ToString());
                        //    }
                        //    //刷新列表
                        //    this.controlPatListNew.RefreshPatients();

                        //    listPatient = this.controlPatListNew.GetALLPatients();
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
            catch (Exception ex)
            {
                MessageDialog.Show("打印失败！", ex.Message);
                Lib.LogManager.Logger.LogException("Print_Click", ex);
            }

        }

        /// <summary>
        /// 打印预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrePrint_Click(object sender, EventArgs e)
        {
            try
            {
                Print(true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show("预览失败！", ex.Message);
                Lib.LogManager.Logger.LogException("PrePrint_Click", ex);
            }

        }

        private void Print(bool isPreview)
        {
            OnPatPrint(null, isPreview);
        }

        private void OnPatPrint(string pat_id, bool isPreview)
        {
            List<string> PrintNoUpdateStartPatIDs = new List<string>();
            PrintNoUpdateStartPatIDs = new List<string>();//打印前,清空此数据
            string prtTemplate = DictInstrmt.Instance.GetItrPrtTemplate(controlPatListNew.txtPatInstructment.valueMember);
            if (prtTemplate == string.Empty)
            {
                MessageDialog.Show("找不到当前仪器的打印模版", "提示");
                return;
            }

            List<string> listPatID = new List<string>();
            StringBuilder sbPatWhere = new StringBuilder();

            StringBuilder sbPatSidWhere = new StringBuilder();
            StringBuilder sbPatSidWhere2 = new StringBuilder();
            if (pat_id == null)
            {
                List<EntityPidReportMain> listPatient = this.controlPatListNew.GetCheckedPatients();

                //if (Lab_NoCheckSelectCurRow)
                //{
                //    if (listPatient.Count == 0 && controlPatListNew.CurrentPatient != null
                //     && controlPatListNew.CurrentPatient.RepId.ToString().Trim(null) != string.Empty)
                //    {
                //        listPatient.Add(controlPatListNew.CurrentPatient);
                //    }
                //}

                if (listPatient.Count > 0)
                {
                    foreach (EntityPidReportMain dr in listPatient)
                    {
                        if (isPreview)
                        {
                            sbPatWhere.Append(string.Format(",'{0}'", dr.RepId.ToString()));
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
                                    string currentitrID = controlPatListNew.txtPatInstructment.valueMember;

                                    if (Lab_NoBarCodeCheckItrExpectList.Contains(currentitrID))
                                    {
                                        sbPatWhere.Append(string.Format(",'{0}'", dr.RepId.ToString()));
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
                                    sbPatWhere.Append(string.Format(",'{0}'", dr.RepId.ToString()));
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
                                sbPatWhere.Append(string.Format(",'{0}'", dr.RepId.ToString()));
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
                        return;
                    }
                    if (listPatID.Count == 0)
                    {
                        MessageDialog.Show("没有符合打印/预览要求的记录，请检查选中的记录是否已" + LocalSetting.Current.Setting.ReportWord, "提示");
                        return;
                    }

                    sbPatWhere.Remove(0, 1);
                }
                else
                {
                    MessageDialog.Show("在请左侧勾选需要预览/打印的病人", "提示");
                    return;
                }
            }
            else//存打
            {
                listPatID.Add(pat_id);
                sbPatWhere.Append(string.Format("'{0}'", pat_id));
            }


            #region 新打印方式，传多个打印指令打印多个病人，每条指令一个病人

            List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
            int sequence = 0;
            foreach (string patient_id in listPatID)
            {
                EntityDCLPrintParameter printPara = new EntityDCLPrintParameter();
                printPara.RepId = patient_id;
                printPara.ReportCode = prtTemplate;
                printPara.Sequence = sequence;
                if (this.controlPatListNew.ItrDataType == LIS_Const.InstmtDataType.Description)
                {
                    printPara.CustomParameter.Add("ItrRepFlag", "4");
                }
                listPara.Add(printPara);
                sequence++;
            }
            listPrintData_multithread = EntityManager<EntityDCLPrintParameter>.ListClone(listPara);
            if (isPreview)//是否为打印预览
            {
                if (useMultiThread)
                {
                    Thread thread = new Thread(new ThreadStart(StartPreviewReports));
                    thread.Start();
                }
            }
            else
            {
                try
                {
                    //不是打印预览才需要更新打印标志
                    pForm_PrintStart2(listPara);
                    DCLReportPrint.BatchPrint(listPara);
                    this.SelectAllPatient(false);
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
            #endregion
        }

        /// <summary>
        /// 选择/取消选择所有病人
        /// </summary>
        /// <param name="selectAll"></param>
        public void SelectAllPatient(bool selectAll)
        {
            this.controlPatListNew.SelectAllPatientInGrid(selectAll);
        }

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
                    EntityPidReportMain drPat = this.controlPatListNew.GetPatient(pat_id);
                    if (drPat != null)
                    {
                        drPat.RepStatus = Convert.ToInt32(LIS_Const.PATIENT_FLAG.Printed);
                        drPat.RepStatusName = "已打印";
                    }
                }
            }
        }

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

        /// <summary>
        /// 关闭
        /// </summary>
        private void Close_Click(object sender, EventArgs e)
        {
            if (MessageDialog.Show("您确定要关闭当前窗口吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.IsBtnClose = true;
                this.Close();
            }
        }

        /// <summary>
        /// 当前病人信息发生修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void controlPatListNew1_PatientChanged(object sender, PatControl.ControlPatListNew.PatientChangedEventArgs args)
        {
            this.isNewOrModify = false;
            bsPat.DataSource = args.Pat_Data;
            if (args.Pat_Data == null)
            {
                memoEditDescribe.Text = "";
                memoEditOpinion.Text = "";
                List<EntityObrResult> results = new List<EntityObrResult>();
                this.dtPatientResulto = results;
                BindResults();
                this.imageList = new List<EntityObrResultImage>();
                UpdatePictures(null);
                List<EntityPidReportDetail> report_details = new List<EntityPidReportDetail>();
                this.ceCombine.listRepDetail = report_details;
                this.ceCombine.RefreshEditBoxText();
                this.listPatCombine = report_details;
                return;
            }
            try
            {
                EntityPidReportMain report = args.Pat_Data;

                ShowReportMain(args.Pat_Data);

                List<EntityPidReportDetail> report_details = new ProxyReportDetail().Service.GetPidReportDetailByRepId(report.RepId);
                this.ceCombine.listRepDetail = report_details;
                this.ceCombine.RefreshEditBoxText();
                this.listPatCombine = report_details;

                EntityResultQC qc = new EntityResultQC();
                qc.ListObrId.Add(report.RepId);
                List<EntityObrResult> results = new ProxyObrResult().Service.GetObrResultQuery(qc);
                if (results == null)
                {
                    results = new List<EntityObrResult>();
                }

                this.dtPatientResulto = results;
                this.FullFillItems();
                BindResults();

                List<EntityObrResultImage> img_results = new ProxyObrResultImage().Service.GetObrResultImage(report.RepId);
                this.imageList = img_results;
                UpdatePictures(img_results);

                if (bsPat.Current != null)
                {
                    EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                    FillTimeLine(CurrentPatInfo);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show("获取报告信息失败" + ex.Message);
            }
        }

        /// <summary>
        /// 补充已有结果外的检验项目，保存的时候只获取有值的记录进行获取
        /// </summary>
        private void FullFillItems()
        {
            if (this.listPatCombine != null && this.listPatCombine.Count > 0)
            {
                foreach (EntityPidReportDetail report_detail in this.listPatCombine)
                {
                    if (report_detail != null)
                    {
                        string com_id = report_detail.ComId;
                        AddCombineItems(com_id, 0);
                    }
                }
            }
        }


        /// <summary>
        ///  更新图片显示
        /// </summary>
        /// <param name="list_result_img"></param>
        private void UpdatePictures(List<EntityObrResultImage> list_result_img)
        {
            flowLayoutPanel1.Controls.Clear();
            if (list_result_img == null || list_result_img.Count == 0)
            {
                return;
            }
            foreach (EntityObrResultImage img in list_result_img)
            {
                MemoryStream ms = new MemoryStream(img.ObrImage);
                Image img_show = Image.FromStream(ms);
                PictureEdit pic = new PictureEdit();
                ContextMenu emptyMenu = new ContextMenu();
                pic.Properties.ContextMenu = emptyMenu;
                pic.MouseClick += new MouseEventHandler(ImageOperation);
                pic.Size = new Size(81, 62);
                pic.Image = img_show;
                pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                flowLayoutPanel1.Controls.Add(pic);
            }
        }
        /// <summary>
        /// 图像预览、图像删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageOperation(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureEdit pic = (PictureEdit)sender;
                FrmImagView imagview = new FrmImagView();
                imagview.imagelist = GetAllImage(this.flowLayoutPanel1);
                imagview.currentImage = pic.Image;
                imagview.ShowDialog();
            }
            else if (e.Button == MouseButtons.Right)
            {
                List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
                listPats = this.controlPatListNew.GetCheckedPatients();
                EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                if (CurrentPatInfo.RepStatus != 0)
                {
                    string show = string.Format("该记录已{0}或{1}，不能删除", this.AuditWord, this.ReportWord);
                    MessageDialog.Show(show);
                    return;
                }

                if (lis.client.control.MessageDialog.Show("确定删除该图像吗？", "确认", MessageBoxButtons.YesNo) ==
            DialogResult.Yes)
                {
                    PictureEdit pic = (PictureEdit)sender;
                    int index = flowLayoutPanel1.Controls.IndexOf(pic);
                    this.flowLayoutPanel1.Controls.Remove(pic);
                    if (this.imageList != null && this.imageList.Count > 0)
                    {
                        this.imageList.RemoveAt(index);
                        Save_Click(null, null);
                    }
                }
                else
                    return;

            }
        }

        /// <summary>
        /// 绑定结果表数据
        /// </summary>
        private void BindResults()
        {
            BsResult.DataSource = this.dtPatientResulto;
            this.gridControlResult.RefreshDataSource();
        }

        /// <summary>
        /// 显示主报告信息
        /// </summary>
        /// <param name="report"></param>
        private void ShowReportMain(EntityPidReportMain report)
        {
            #region 显示主界面病人信息
            if (report.RepInDate != null)//report.SampReceiveDate
                txtPatDate.EditValue = report.RepInDate;

            txtPatSid.Text = report.RepSid;
            memoEditDescribe.Text = report.RepDiscribe;
            memoEditOpinion.Text = report.RepComment;

            #endregion

            #region 加载报告者、审核者、审核时间等控件数据
            this.txtAuditName.Text = report.PidChkName.ToString();
            this.txtAuditTime.Text = report.RepAuditDate?.ToString();
            this.txtReportName.Text = report.BgName.ToString();
            this.txt_dept_tel.Text = report.DeptTel;
            #endregion

            #region 显示申请、采样、送达、收取、检验、审核时间
            if (report.SampReceiveDate != null)
                lblReceive.Text = report.SampReceiveDate.Value.ToString("yyyy/MM/dd HH:mm");
            else
                lblReceive.Text = string.Empty;

            if (report.SampCollectionDate != null)
                lblSam.Text = report.SampCollectionDate.Value.ToString("yyyy/MM/dd HH:mm");
            else
                lblSam.Text = string.Empty;

            if (report.SampSendDate != null)
                lblSdate.Text = report.SampSendDate.Value.ToString("yyyy/MM/dd HH:mm");
            else
                lblSdate.Text = string.Empty;

            if (report.SampApplyDate != null)
                lblApply.Text = report.SampApplyDate.Value.ToString("yyyy/MM/dd HH:mm");
            else
                lblApply.Text = string.Empty;

            if (report.SampReachDate != null)
                lblreachDate.Text = report.SampReachDate.Value.ToString("yyyy/MM/dd HH:mm");
            else
                lblreachDate.Text = string.Empty;

            if (report.RepReportDate != null)
                lblReport.Text = report.RepReportDate.Value.ToString("yyyy/MM/dd HH:mm");
            else
                lblReport.Text = string.Empty;
            #endregion
        }

        /// <summary>
        /// 显示TAT
        /// </summary>
        /// <param name="PatInfo"></param>
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

        /// <summary>
        /// 返回时间差
        /// </summary>
        /// <param name="dtstr1">起始时间</param>
        /// <param name="dtstr2">结束时间</param>
        /// <returns></returns>
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

        private List<Image> GetAllImage(Control control)
        {
            List<Image> imaglist = new List<Image>();
            foreach (Control con in control.Controls)
            {
                if (con is PictureEdit)
                {
                    PictureEdit im = con as PictureEdit;
                    imaglist.Add(im.Image);
                }
            }
            return imaglist;
        }

        /// <summary>
        /// 样本号失去交点时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPatSid_Leave(object sender, EventArgs e)
        {
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

                if (IsNumStr(txtPatSid.Text))
                {
                    MessageDialog.Show("输入的样本号不正确，请确保为半角数字");
                    txtPatSid.Text = string.Empty;
                    this.ActiveControl = txtPatSid;
                    txtPatSid.Focus();
                    return;
                }
            }

        }

        /// <summary>
        /// 判断是否是全角
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsNumStr(string str)
        {
            char[] c = str.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] > 65280 && c[i] < 65375)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 双击特征描述
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoEditDescribe_DoubleClick(object sender, EventArgs e)
        {
            FrmMarrowDescribe fb = new FrmMarrowDescribe(this, "22");
            fb.sam_id = this.txtPatSampleType.valueMember;
            fb.IsSortSam = true;//过滤标本
            fb.ShowDialog();
        }


        /// <summary>
        /// 双击意见描述
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemoEditOpinion_DoubleClick(object sender, EventArgs e)
        {
            FrmMarrowDescribe fb = new FrmMarrowDescribe(this, "1");
            fb.sam_id = this.txtPatSampleType.valueMember;
            fb.IsSortSam = true;//过滤标本
            fb.ShowDialog();
        }

        /// <summary>
        /// 选中备注评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnComment_Click(object sender, EventArgs e)
        {
            FrmMarrowDescribe fb = new FrmMarrowDescribe(this, "0");
            fb.sam_id = this.txtPatSampleType.valueMember;
            fb.IsSortSam = true;//过滤标本
            fb.ShowDialog();
        }

        private void BtnPatComment_Click(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 根据类型获取描述模板信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        public void getBscripe(String str, string type)
        {
            if (type == "0")
            {
                fpat_exp2.Text += str;
            }
            else if (type == "1")
            {
                memoEditOpinion.Text = str;
            }
            else if (type == "22")
            {
                memoEditDescribe.Text = str;
            }

        }

        /// <summary>
        /// 右键点击添加项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bsPat.Current == null)
            {
                return;
            }
            MenuAddItem();
        }

        #region 添加项目
        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        private void MenuAddItem()
        {
            //获取病人记录状态
            ProxyPidReportMain mainProxy = new ProxyPidReportMain();
            if (bsPat.Current != null)
            {
                EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                string strPatState = mainProxy.Service.GetPatientState(CurrentPatInfo.RepId);

                if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
                {
                    FrmItemSelect frm = new FrmItemSelect();
                    frm.itm_ptype = CurrentPatInfo.ItrProId;
                    frm.itr_id = CurrentPatInfo.RepItrId;
                    frm.ResetFilter();
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();

                    if (frm.DialogResult == DialogResult.OK)
                    {
                        string itm_id = frm.ReturnItemID;
                        string itm_ecd = frm.ReturnItemECD;
                        AddNewItem(itm_id);//添加项目
                    }
                }
                else//已审核、报告、打印
                {
                    MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能再添加项目", "提示");
                }

            }
        }

        private void AddNewItem(string itm_id)
        {
            //查找当前病人结果表中的项目是否已存在
            if (this.dtPatientResulto != null && this.dtPatientResulto.Count > 0)
            {
                List<EntityObrResult> drsResultItems = this.dtPatientResulto.FindAll(i => i.ItmId == itm_id);
                if (drsResultItems.Count > 0)
                {
                    MessageDialog.Show("已包含该项目！");
                    return;
                }
            }

            try
            {
                AddNewResult(itm_id);
                BindResults();
                NotNullItemCheck();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("新增项目失败：" + ex.Message);
            }

        }

        /// <summary>
        /// 根据项目id获取临时结果项目
        /// </summary>
        /// <param name="itm_id"></param>
        /// <returns></returns>
        private void AddNewResult(string itm_id)
        {
            EntityDicItmItem dict_item = CacheClient.GetCache<EntityDicItmItem>().FirstOrDefault(a => a.ItmId == itm_id);
            if (dict_item == null)
            {
                return;
            }
            string Pat_itr_id = this.controlPatListNew.ItrID;
            string samtypeid = this.txtPatSampleType.valueMember;
            int age = -1;
            if (textAgeInput1.AgeYear != null)
            {
                age = textAgeInput1.AgeYear.Value;
            }
            string sex = txtPatSex.valueMember;

            if (bsPat.Current != null)
            {
                EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;

                EntityObrResult result = new EntityObrResult();

                string defValue = null;

                ////获取项目：项目信息、项目样本信息、参考值
                List<EntityItmRefInfo> listItmRefInfo = new ProxyPatResult().Service.GetItemRefInfo(new List<string> { itm_id }, samtypeid, GetConfigOnNullAge(age), GetConfigOnNullSex(sex), "", Pat_itr_id, "", "");
                if (listItmRefInfo.Count > 0)
                {
                    EntityItmRefInfo drItem = listItmRefInfo[0];

                    List<EntityPidReportDetail> patients_mi = EntityManager<EntityPidReportDetail>.ListClone(this.ceCombine.listRepDetail);

                    if (patients_mi == null || patients_mi.Count == 0)
                        return;

                    List<EntityDicCombineDetail> drsCombineMi = null;
                    EntityDicCombine drCombine = null;
                    int com_seq = 0;
                    //遍历当前病人检验组合
                    foreach (EntityPidReportDetail drPatComMi in patients_mi)
                    {
                        com_seq++;
                        string com_id = drPatComMi.ComId.ToString();

                        //查找组合所有检验项目
                        List<EntityDicCombineDetail> drsCombine = DictCombineMi.Instance.GetCombineMi(com_id, sex);
                        if (drsCombine == null || drsCombine.Count == 0)
                        {
                            continue;
                        }
                        if (drsCombine.FindIndex(a => a.ComItmId == itm_id) >= 0)
                        {
                            drsCombineMi = drsCombine;
                            drCombine = DictCombine.Instance.GetCombinebyID(com_id);
                        }
                    }

                    if (drCombine == null)
                    {
                        return;
                    }

                    if (!Compare.IsNullOrDBNull(drItem.ItmDefault))
                    {
                        defValue = drItem.ItmDefault;
                    }

                    EntityDicCombineDetail drCombineMi = null;
                    if (drsCombineMi.Count > 0)
                    {
                        drCombineMi = drsCombineMi[0];
                    }
                    AddItem(drItem, drCombineMi, drCombine.ComId, drCombine.ComName, com_seq, defValue, null);
                }
            }
        }

        private int GetConfigOnNullAge(int age)
        {
            if (age < 0)
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalAge = UserInfo.GetSysConfigValue("GetRefOnNullAge");

                int calage = -1;

                if (!string.IsNullOrEmpty(configCalAge)
                    && configCalAge != "不计算参考值")
                {
                    calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
                    if (age >= 0)
                    {
                        calage = age;
                    }
                }
                return calage;
            }
            else
            {
                return age;
            }
        }

        private string GetConfigOnNullSex(string sex)
        {
            if (string.IsNullOrEmpty(sex)

                || (sex != "1"
                && sex != "2"
                && sex != "0"))
            {
                //获取参考值：年龄/性别为空时的计算规则
                string configCalSex = UserInfo.GetSysConfigValue("GetRefOnNullSex");

                if (configCalSex.Contains("男"))
                {
                    return "1";
                }
                else if (configCalSex.Contains("女"))
                {
                    return "2";
                }

                return "0";
            }
            else
            {
                return sex;
            }
        }
        #endregion

        /// <summary>
        /// 右键弹窗删除项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bsPat.Current == null)
            {
                return;
            }
            MenuDelItem();
        }


        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="sourceGrid"></param>
        private void MenuDelItem()
        {
            //获取病人记录状态
            ProxyPidReportMain mainProxy = new ProxyPidReportMain();
            if (bsPat.Current != null)
            {
                EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
                string strPatState = mainProxy.Service.GetPatientState(CurrentPatInfo.RepId);

                if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
                {
                    EntityObrResult result = gridViewResult.GetFocusedRow() as EntityObrResult;
                    if (result == null)
                    {
                        return;
                    }
                    else
                    {
                        this.dtPatientResulto.Remove(result);
                        BindResults();
                        NotNullItemCheck();
                    }
                }
                else//已审核、报告、打印
                {
                    MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能再添加项目", "提示");
                }
            }
        }

        /// <summary>
        /// 仪器改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlPatListNew1_OnItrChanged(object sender, EventArgs e)
        {
            this.ceCombine.ItrID = this.controlPatListNew.ItrID;
            ResetAll();
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        private void ResetAll()
        {
            this.isNewOrModify = false;
            this.controlPatListNew.Reset();
            ucTimeLine1.Reset();

            bsPat.DataSource = new EntityPidReportMain();
            this.imageList = null;
            this.dtPatientResulto = new List<EntityObrResult>();
            UpdatePictures(null);
            BsResult.DataSource = this.dtPatientResulto;

            if (this.listPatCombine != null)
            {
                this.listPatCombine.Clear();
            }

            DateTime dtToday = ServerDateTime.GetServerDateTime();
            this.txtPatSampleDate.EditValue = dtToday;
            if (UserInfo.GetSysConfigValue("AlwaysKeepSendDate") != "是") //如果不强制保存送检时间
                this.txtPatSDate.EditValue = dtToday;
            this.txtPatReachDate.EditValue = dtToday;
            this.txtPatReceiveDate.EditValue = dtToday;
            this.txtPatRecDate.EditValue = dtToday;
            this.txtPatReportDate.EditValue = dtToday;
            NotNullItemCheck();

        }

        /// <summary>
        /// 组合改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void controlPatListNew1_OnTypeChanged(object sender, EventArgs e)
        {
            ResetAll();
        }


        #region 组合发生改变

        /// <summary>
        /// 组合编辑框添加组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="com_id"></param>
        void CombineEditor_CombineAdded(object sender, string com_id, int com_seq)
        {
            AddCombineItems(com_id, com_seq);
            BindResults();
        }


        /// <summary>
        /// 添加组合对应的项目
        /// </summary>
        /// <param name="com_id">组合ID</param>
        private void AddCombineItems(string com_id, int com_seq)
        {
            this.gridViewResult.CloseEditor();

            //根据组合ID获取组合信息
            EntityDicCombine drCombine = DictCombine.Instance.GetCombinebyID(com_id);

            if (drCombine != null)
            {
                string samtypeid = this.txtPatSampleType.valueMember;
                int age = -1;
                if (textAgeInput1.AgeYear != null)
                {
                    age = textAgeInput1.AgeYear.Value;
                }
                string sex = txtPatSex.valueMember;
                string Pat_itr_id = this.controlPatListNew.ItrID;
                string com_name = drCombine.ComName;

                //获取组合包含的项目
                List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, sex);
                if (dtComItems.Count > 0)
                {
                    List<string> itemsID = new List<string>();

                    //遍历
                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {

                        if (!string.IsNullOrEmpty(drComItem.ComItmId))
                        {
                            string itm_id = drComItem.ComItmId;
                            if (this.dtPatientResulto != null && this.dtPatientResulto.FindAll(i => i.ItmId == itm_id).Count == 0)
                                itemsID.Add(itm_id);//添加到ID集合
                        }
                    }
                    if (itemsID.Count == 0)
                    {
                        return;
                    }


                    ////获取项目：项目信息、项目样本信息、参考值
                    List<EntityItmRefInfo> listItemRefInfo = new ProxyPatResult().Service.GetItemRefInfo(itemsID,
                                                                samtypeid,
                                                                GetConfigOnNullAge(age),
                                                                GetConfigOnNullSex(sex),
                                                                "", Pat_itr_id, "", "");
                    if (listItemRefInfo == null)
                    {
                        return;
                    }

                    foreach (string itm_id in itemsID)
                    {
                        List<EntityItmRefInfo> drItems = listItemRefInfo.FindAll(i => i.ItmId == itm_id);

                        if (drItems.Count > 0)
                        {
                            EntityItmRefInfo drItem = drItems[0];

                            string defValue = null;

                            List<EntityDicCombineDetail> drsCombineMi = dtComItems.FindAll(i => i.ComItmId == itm_id);

                            if (!Compare.IsNullOrDBNull(drItem.ItmDefault))
                            {
                                defValue = drItem.ItmDefault;
                            }

                            EntityDicCombineDetail drCombineMi = null;
                            if (drsCombineMi.Count > 0)
                            {
                                drCombineMi = drsCombineMi[0];
                            }
                            AddItem(drItem, drCombineMi, com_id, com_name, com_seq, defValue, null);
                        }
                    }

                }
            }

            NotNullItemCheck();
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="drItem"></param>
        public EntityObrResult AddItem(EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, int com_seq, string res_chr, string res_od_chr)
        {
            if (drItem != null)
            {
                string itm_id = drItem.ItmId;

                //项目编号
                string itm_ecd = string.Empty;
                if (!string.IsNullOrEmpty(drItem.ItmEcode))
                {
                    itm_ecd = drItem.ItmEcode;
                }

                string strEcd = SQLFormater.Format(itm_ecd.Trim());

                //查找当前病人结果表中的项目是否已存在
                List<EntityObrResult> drsResultItems = this.dtPatientResulto.FindAll(i => i.ItmId == itm_id || i.ItmEname == strEcd);

                EntityObrResult drResultItem = null;
                if (drsResultItems.Count == 0)
                {
                    drResultItem = new EntityObrResult();
                    FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                    drResultItem.IsNew = 1;
                    drResultItem.ResComSeq = com_seq;
                    this.dtPatientResulto.Add(drResultItem);
                }
                else
                {
                    EntityObrResult drResultExistItem = drsResultItems[0];
                    int row_state = drResultExistItem.IsNew;


                    if (row_state == 2)//需要添加的项目为已被删除的项目
                    {
                        this.dtPatientResulto.Remove(drResultExistItem);//先把被删除(隐藏)的项目移除，再添加

                        drResultItem = new EntityObrResult();
                        FillItemToResult(drResultItem, drItem, drComMi, com_id, com_name, itm_id, itm_ecd, res_chr, res_od_chr);
                        drResultItem.ResComSeq = com_seq;
                        drResultItem.IsNew = 0;
                        this.dtPatientResulto.Add(drResultItem);
                    }
                    else
                    {
                        drResultItem = drsResultItems[0];
                        if (drResultItem.ObrSn == 0)
                        {
                            drResultItem.IsNew = 1;
                        }
                        else
                        {
                            drResultItem.IsNew = 0;
                        }
                    }
                }

                return drResultItem;
            }
            return null;
        }

        /// <summary>
        /// 将参考值，组合信息等放入结果数据中
        /// </summary>
        /// <param name="drResultItem">待完善的结果数据</param>
        /// <param name="drItem">参考值对象</param>
        /// <param name="drComMi">组合明细</param>
        /// <param name="com_id">组合ID</param>
        /// <param name="com_name">组合名</param>
        /// <param name="itm_id">项目ID</param>
        /// <param name="itm_ecd"></param>
        /// <param name="res_chr"></param>
        /// <param name="res_od_chr"></param>
        private void FillItemToResult(EntityObrResult drResultItem, EntityItmRefInfo drItem, EntityDicCombineDetail drComMi, string com_id, string com_name, string itm_id, string itm_ecd, string res_chr, string res_od_chr)
        {
            if (bsPat.Current == null)
            {
                return;
            }

            EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;

            drResultItem.ObrFlag = 1;

            //项目名称
            string itm_name = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmName))
            {
                itm_name = drItem.ItmName;
            }

            //单位
            string itm_unit = string.Empty;
            if (!string.IsNullOrEmpty(drItem.ItmUnit))
            {
                itm_unit = drItem.ItmUnit;
            }

            string itm_rep_ecd = string.Empty;
            if (!Compare.IsNullOrDBNull(drItem.ItmRepCode) && drItem.ItmRepCode.Trim() != string.Empty)
            {
                itm_rep_ecd = drItem.ItmRepCode;
            }
            else
            {
                itm_rep_ecd = itm_ecd;
            }
            drResultItem.ObrDate = ServerDateTime.GetServerDateTime();
            drResultItem.ObrId = CurrentPatInfo.RepId;
            drResultItem.ItmId = itm_id;
            drResultItem.ObrItmMethod = drItem.ItmMethod;
            drResultItem.ItmEname = itm_ecd.Trim();
            drResultItem.ItmReportCode = itm_rep_ecd;
            drResultItem.ItmName = itm_name;
            drResultItem.ObrUnit = itm_unit;
            drResultItem.ResComName = com_name;
            //drResultItem["pat_com_seq"] = com_seq;
            drResultItem.ItmDtype = drItem.ItmResType;
            drResultItem.ItmMaxDigit = drItem.ItmMaxDigit;

            if (drItem.ItmCaluFlag == 1)
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Cal);
            }
            else if (!string.IsNullOrEmpty(res_chr))
            {
                drResultItem.ObrType = 3;
            }
            else
            {
                drResultItem.ObrType = Convert.ToInt32(LIS_Const.PatResultType.Normal);
            }

            drResultItem.ItmSeq = drItem.ItmSortNo;
            drResultItem.ItmPrice = drItem.ItmPrice;
            drResultItem.ItmComId = com_id;

            if (!string.IsNullOrEmpty(drItem.ItmPositiveRes))
                drResultItem.ResPositiveResult = drItem.ItmPositiveRes;

            if (!string.IsNullOrEmpty(drItem.ItmUrgentRes))
                drResultItem.ResCustomCriticalResult = drItem.ItmUrgentRes;

            if (!string.IsNullOrEmpty(drItem.ItmResultAllow))
                drResultItem.ResAllowValues = drItem.ItmResultAllow;

            drResultItem.ResMax = drItem.ItmMaxValue;
            drResultItem.ResMin = drItem.ItmMinValue;

            drResultItem.ResPanH = drItem.ItmDangerUpperLimit;
            drResultItem.ResPanL = drItem.ItmDangerLowerLimit;

            drResultItem.RefUpperLimit = drItem.ItmUpperLimitValue;
            drResultItem.RefLowerLimit = drItem.ItmLowerLimitValue;

            drResultItem.ResMaxCal = drItem.ItmMaxValueCal;
            drResultItem.ResMinCal = drItem.ItmMinValueCal;

            drResultItem.ResPanHCal = drItem.ItmDangerUpperLimitCal;
            drResultItem.ResPanLCal = drItem.ItmDangerLowerLimitCal;

            drResultItem.ResRefHCal = drItem.ItmUpperLimitValueCal;
            if (drResultItem.ResRefHCal == null) drResultItem.ResRefHCal = string.Empty;

            drResultItem.ResRefLCal = drItem.ItmLowerLimitValueCal;
            if (drResultItem.ResRefLCal == null) drResultItem.ResRefLCal = string.Empty;

            if (drResultItem.ResRefLCal.Trim() != string.Empty
                && drResultItem.ResRefHCal.Trim() != string.Empty)
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().Trim() + " - " + drResultItem.ResRefHCal.ToString().Trim();
            }
            else
            {
                drResultItem.ResRefRange = drResultItem.ResRefLCal.ToString().Trim() + drResultItem.ResRefHCal.ToString().Trim();
            }

            if (drComMi != null)
            {
                drResultItem.IsNotEmpty = drComMi.ComMustItem;
                drResultItem.ComMiSort = drComMi.ComSortNo;
            }
            drResultItem.ObrValue = res_chr;

        }

        /// <summary>
        /// 组合编辑框移除组合
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="com_id"></param>
        void CombineEditor_CombineRemoved(object sender, string com_id)
        {
            this.gridViewResult.CloseEditor();

            bool deleteRes = false;

            if (UserInfo.GetSysConfigValue("Lab_ShowDeleteCombineItem") == "是")
            {
                deleteRes = MessageDialog.Show(string.Format("您确定要删除该组合的所有项目结果？"), "提示",
                                                MessageBoxButtons.YesNo) == DialogResult.Yes;
            }
            RemoveCombineItems(com_id, deleteRes);
        }


        /// <summary>
        /// 移除组合
        /// </summary>
        /// <param name="com_id"></param>
        public void RemoveCombineItems(string com_id, bool removeHasResult)
        {
            EntityPidReportMain CurrentPatInfo = bsPat.Current as EntityPidReportMain;
            if (CurrentPatInfo == null)
            {
                MessageDialog.Show("请选择相应记录");
                return;
            }
            EntityPidReportMain patient = new ProxyPidReportMain().Service.GetPatientByPatId(CurrentPatInfo.RepId, false);
            if (patient != null)
            {
                if (patient.RepStatus.ToString() != "0" || patient.RepInitialFlag.ToString() != "0")
                {
                    MessageDialog.Show(string.Format("当前记录已{1}或已{0}，不能删除组合", "审核", LocalSetting.Current.Setting.ReportWord), "提示");
                    return;
                }
            }

            try
            {
                List<EntityObrResult> drs = new List<EntityObrResult>();
                if (this.dtPatientResulto != null)
                    drs = this.dtPatientResulto.FindAll(i => i.ItmComId == com_id);

                RemoveItem(drs, removeHasResult); // 删除组合中的项目
                BindResults();
            }
            catch (Exception ex)
            {
                MessageDialog.Show("删除组合项目失败" + ex.Message);
            }

            NotNullItemCheck();
        }


        /// <summary>
        /// 删除项目
        /// </summary>
        public void RemoveItem(List<EntityObrResult> rowsPatResultItem, bool removeHasResult)
        {
            bool deleteInDatabase = false;
            if (UserInfo.GetSysConfigValue("PatEnterItemDeleteMode") == "立刻从数据库删除")
            {
                deleteInDatabase = true;
            }
            for (int i = 0; i < rowsPatResultItem.Count; i++)
            {
                EntityObrResult drPatResultItem = rowsPatResultItem[i];
                //是否已录入结果
                bool hasResult = false;
                if (!string.IsNullOrEmpty(drPatResultItem.ObrValue)
                    && drPatResultItem.ObrValue.Trim(null) != string.Empty)
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
                        if (!Compare.IsEmpty(drPatResultItem.ObrId) && !Compare.IsEmpty(drPatResultItem.ObrSn))
                        {
                            if (deleteInDatabase)
                            {
                                EntityLogLogin logLogin = new EntityLogLogin();
                                logLogin.LogIP = UserInfo.ip;
                                logLogin.LogLoginID = UserInfo.loginID;

                                string resid = drPatResultItem.ObrId.ToString();
                                string res_itm_ecd = drPatResultItem.ItmEname.ToString();
                                string res_itm_id = string.Empty;
                                if (!Compare.IsEmpty(drPatResultItem.ItmId))
                                {
                                    res_itm_id = drPatResultItem.ItmId.ToString();
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
                                else if (reskey != -1)
                                {
                                    opResult = new ProxyPatResult().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                }


                                if (!opResult)
                                {
                                    MessageDialog.Show(string.Format("删除[{0}]失败！", res_itm_ecd), "错误");
                                }
                                else
                                {
                                    rowsPatResultItem.Remove(drPatResultItem);
                                    i--;
                                    int deleteIndex = dtPatientResulto.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                    if (deleteIndex > -1)
                                        this.dtPatientResulto.RemoveAt(deleteIndex);
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
                        }
                    }
                    else
                    {
                        if (!recordInDataBase)
                        {
                            rowsPatResultItem.Remove(drPatResultItem);
                        }

                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(drPatResultItem.ObrId) && drPatResultItem.ObrSn != 0)
                    {
                        if (deleteInDatabase)
                        {
                            EntityLogLogin logLogin = new EntityLogLogin();
                            logLogin.LogIP = UserInfo.ip;
                            logLogin.LogLoginID = UserInfo.loginID;

                            string resid = drPatResultItem.ObrId.ToString();
                            string res_itm_ecd = drPatResultItem.ItmEname.ToString();
                            string res_itm_id = string.Empty;
                            if (!Compare.IsEmpty(drPatResultItem.ItmId))
                            {
                                res_itm_id = drPatResultItem.ItmId.ToString();
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
                                opResult = new ProxyPatResult().Service.DeleteCommonResultItemByObrSn(logLogin, reskey.ToString(), resid);
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

                                int deleteIndex = dtPatientResulto.FindIndex(w => w.ObrSn == drPatResultItem.ObrSn);
                                if (deleteIndex > -1)
                                    this.dtPatientResulto.RemoveAt(deleteIndex);
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
                        this.dtPatientResulto.Remove(drPatResultItem);
                    }
                }
            }
        }

        /// <summary>
        /// 组合编辑框重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CombineEditor_Reseted(object sender, EventArgs e)
        {
            //this.Reset();
        }


        /// <summary>
        /// 判断必须和非空项目数目
        /// </summary>
        public void NotNullItemCheck()
        {
            List<EntityPidReportDetail> patients_mi = null;
            if (this.ceCombine.listRepDetail != null && this.ceCombine.listRepDetail.Count > 0)
            {
                patients_mi = EntityManager<EntityPidReportDetail>.ListClone(this.ceCombine.listRepDetail);
            }
            if (patients_mi != null && patients_mi.Count > 0)
            {
                string sex = txtPatSex.valueMember;
                //必录项目
                List<string> listNotNullItem = new List<string>();

                List<string> listNotNullItemHasResult = new List<string>();

                //遍历当前病人检验组合
                foreach (EntityPidReportDetail drPatComMi in patients_mi)
                {
                    string com_id = drPatComMi.ComId.ToString();

                    //查找组合所有检验项目
                    List<EntityDicCombineDetail> dtComItems = DictCombineMi.Instance.GetCombineMi(com_id, sex);

                    foreach (EntityDicCombineDetail drComItem in dtComItems)
                    {
                        string com_itm_id = drComItem.ComItmId;
                        string com_itm_ecd = SQLFormater.Format(drComItem.ComItmEname);

                        if (!listNotNullItem.Exists(i => i == com_itm_id))
                        {
                            if (!string.IsNullOrEmpty(drComItem.ComMustItem))
                            {
                                if (Convert.ToInt32(drComItem.ComMustItem) == 1)
                                {
                                    listNotNullItem.Add(com_itm_id);

                                    if (this.dtPatientResulto != null &&
                                        this.dtPatientResulto.FindAll(i => i.ItmId == com_itm_id &&
                                        (!string.IsNullOrEmpty(i.ObrBldValue) || !string.IsNullOrEmpty(i.ObrBoneValue))).Count > 0)
                                    {
                                        listNotNullItemHasResult.Add(com_itm_id);
                                    }
                                }
                            }
                        }
                    }
                }

                this.lblNotEmptyItem.Text = string.Format("项目数：{0}/{1}", listNotNullItemHasResult.Count, listNotNullItem.Count);
            }
            else
            {
                this.lblNotEmptyItem.Text = string.Format("项目数：{0}/{0}", 0);
            }
        }

        #endregion

        /// <summary>
        /// 实验项目结果发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewResult_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            NotNullItemCheck();
        }


        /// <summary>
        /// 显示配置面板
        /// </summary>
        private void ShowConfigPanel()
        {
            FrmPatPanelConfig f = new FrmPatPanelConfig(this);
            f.Show();
        }

        // 点击显示配置面板
        private void controlPatListNew1_PanelConfig(object sender, EventArgs args)
        {
            ShowConfigPanel();
        }


        FrmPatInfoExt frmext = null; // 标本显示窗口
        /// <summary>
        /// 显示标本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBarInfo_Click(object sender, EventArgs e)
        {
            if (this.controlPatListNew.CurrentPatient != null)
            {
                if (frmext != null)
                    frmext.Close();
                frmext = new FrmPatInfoExt();
                frmext.Tag = "标本信息";
                frmext.LoadInfo(this.controlPatListNew.CurrentPatient.RepId.ToString(), listPatCombine
                    , this.controlPatListNew.CurrentPatient.RepBarCode.ToString());
                frmext.StartPosition = FormStartPosition.CenterScreen;
                frmext.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
            }
        }

        /// <summary>
        /// 点击gridview，若选中骨髓和血片列则进入编辑模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewResult_Click(object sender, EventArgs e)
        {
            GridView gridView = sender as GridView;
            if (gridView == null)
                return;

            GridHitInfo info;
            Point pt = gridView.GridControl.PointToClient(Control.MousePosition);
            if (pt == null)
                return;
            info = gridView.CalcHitInfo(pt);
            if (info == null)
                return;

            if (info.InRowCell)
            {
                if (info.Column.FieldName == "ObrBldValue" || info.Column.FieldName == "ObrBoneValue")
                {
                    gridViewResult.ShowEditor();
                }
            }
            else
            {
                gridViewResult.ClearSelection();
            }
        }

        #region ID号查询院网逻辑
        private void txtPatID_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPatID.Text != null && txtPatID.Text.StartsWith("#"))
                txtPatID.Text = txtPatID.Text.Replace("#", "").Trim();
        }

        private void txtPatID_EnterKeyDown(object sender, EventArgs args)
        {
            if (!this.isNewOrModify)
            {
                return;
            }
            string itr_id = this.controlPatListNew.ItrID;
            if (string.IsNullOrEmpty(itr_id))
            {
                MessageDialog.Show("请选择仪器", "提示");
                return;
            }

            string typeid;
            bool success = LoadPatInfoByID(out typeid, this.txtPatID.Text);
            bsPat.EndEdit();
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
                    if (SaveOrUpdateReport()) // 保存
                    {
                        if (!UserCustomSetting.PatResultPanel.SavePatInfoNoNext)
                        {
                            Add_Click(null, null);
                        }
                        this.Refresh_Click(null, null);
                    }
                    this.ActiveControl = this.txtPatID;
                }
            }
            else
            {
            }
        }

        private void txtPatID_KeyDown(object sender, KeyEventArgs e)
        {
            KeysHelper.Jump(e.KeyCode);
        }

        InterfacePatientInfo PatInfo = null;

        /// <summary>
        /// 根据ID类型和ID获取病人信息
        /// </summary>
        private bool LoadPatInfoByID(out string originidtype, string id_code)
        {
            bool ret = false;

            string itr_id = this.controlPatListNew.ItrID;  // 仪器编码

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
                        bool retBool = true;
                        if (PatInfo == null)
                        {
                            MessageDialog.Show(string.Format("{0}不存在", typeName), "提示");
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
                                    MessageDialog.Show(
                                        string.Format("此条码已回退，不允许录入！\r\n{2}\r\n 操作者：{0},时间：{1}",
                                        processDetail.ProcUsername, processDetail.ProcDate, processDetail.ProcContent), "提示");
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
                                        frmBarcodeCombineSelect frmCombineSelect = new frmBarcodeCombineSelect(itr_id, PatInfo.ListPidReportDetail);

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
                                                if (MessageDialog.Show(mes, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
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
                            else // 非条码号
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
                                                    listItrCombine.FindAll(w => w.ComId == item.ComId && w.ItrId == itr_id).Count >
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

                                if (txtPatReceiveDate.EditValue == null)
                                    txtPatReceiveDate.EditValue = txtPatRecDate.EditValue;
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
        /// 填充数据到控件
        /// </summary>
        /// <param name="PatInfo"></param>
        private void FillInterfacePatToControl(EntityPidReportMain PatInfo)
        {
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
                else if (!UserCustomSetting.PatInfoPanel.IsPreserveOnNext("txtPatSampleType"))//其他医院
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
                        && PatInfo.PidDocName != null)
                    {
                        this.txtPatDoc.SelectByDispaly(PatInfo.PidDocName);
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
            }

            this.txtPatDeptId.valueMember = PatInfo.PidDeptCode;
            this.txtPatDeptId.displayMember = PatInfo.PidDeptName;

            //病区
            this.txt_pat_ward_id.Text = PatInfo.PidDeptCode;
            this.txt_pat_ward_name.Text = PatInfo.PidDeptName;

            this.txtPatDiag.displayMember = PatInfo.PidDiag;

            //条码号
            this.txtPatBarCode.EditValue = PatInfo.RepBarCode;

            if (PatInfo.SampReceiveDate != null)
            {
                this.txtPatApplyDate.EditValue = PatInfo.SampReceiveDate;
            }
            else
            {
                this.txtPatApplyDate.EditValue = null;
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
            if (PatInfo.SampSendDate != null)
            {
                this.txtPatSDate.EditValue = PatInfo.SampSendDate.Value;
            }
            else
            {
                this.txtPatSDate.EditValue = null;
            }
            //送达时间
            if (PatInfo.SampReachDate != null)
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
                this.txtPatReceiveDate.EditValue = PatInfo.SampApplyDate.Value;
            }
            else
            {
                this.txtPatReceiveDate.EditValue = null;
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

            //申请单号
            this.txtPatAplyNo.Text = PatInfo.PidApplyNo;

            this.txtPatPid.Text = PatInfo.RepInputId;

            this.txtPatUpid.Text = PatInfo.PidUniqueId;
            this.txtPatNotice.Text = string.Empty;//注意事项
            if (txtPatNotice.Visible && (!string.IsNullOrEmpty(PatInfo.RepBarCode)))
            {
                string StrTempNotice = new ProxySampMain().Service.SampMainQueryByBarId(PatInfo.RepBarCode).SampRemark;
                if (!string.IsNullOrEmpty(StrTempNotice)) this.txtPatNotice.Text = StrTempNotice;
            }

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
            string Itr_id = this.controlPatListNew.ItrID;
            if (!string.IsNullOrEmpty(Itr_id))//没有选择仪器
            {
                currentItrComIDs = DictInstrmt.Instance.GetItrCombineID(Itr_id, true);
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
            string Itr_id = this.controlPatListNew.ItrID;
            if (!string.IsNullOrEmpty(Itr_id))//没有选择仪器
            {
                currentItrComIDs = DictInstrmt.Instance.GetItrCombineID(Itr_id, true);
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


        #endregion


        /// <summary>
        /// ID类型发生修改，病人来源也相应的进行修改
        /// </summary>
        /// <param name="oldRow"></param>
        private void txtPatIdType_onAfterChange(EntityDicPubIdent oldRow)
        {
            string ori_id = DictPatNumberType.Instance.GetOriID_byNoType(this.txtPatIdType.valueMember);
            this.txtPatSource.SelectByID(ori_id);
            bsPat.EndEdit();
        }

        private void FrmMarrowInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.IsBtnClose) // 是按钮关闭
            {
                return;
            }
            if (e.CloseReason != CloseReason.ApplicationExitCall && MessageDialog.Show("您确定要关闭当前窗口吗？", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}