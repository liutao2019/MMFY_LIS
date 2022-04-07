using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using dcl.client.result.Interface;
using dcl.client.frame.runsetting;

using dcl.client.frame;
using dcl.client.wcf;
using dcl.root.logon;
using dcl.client.qc;
using lis.client.control;
using dcl.client.common;


using dcl.client.result.PatControl;
using dcl.entity;
using System.Linq;
using dcl.client.control;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmPatEnterNew : FrmPatInputBaseNew, IPatEnter
    {
        private static object objLock = new object();

        public FrmPatEnterNew() : base()
        {
            controlPatList.ItrDataType = ItrDataType;
            lock (objLock)
            {
                InitializeComponent();
            }
            this.PatEnter = this;
            this.patResult1.parentFormNew = this;
            this.patResult1.ClaItemInfo += new dcl.client.result.PatControl.ClaItemInfoEventHandler(patResult1_ClaItemInfo);
            patResult1.NullItemInfo += PatResult1_NullItemInfo;
            patResult1.LostItemInfo += PatResult1_LostItemInfo;
            this.Load += new System.EventHandler(this.FrmPatEnter_Load);

            this.txtPatSampleState.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicSState>.ValueChangedEventHandler(this.txtPatSampleState_ValueChanged);

            this.spbRelateRes.Click += new System.EventHandler(this.linkRelateRes_Click);
            this.spbMergerRes.Click += new System.EventHandler(this.linkMergerRes_Click);
            this.spbInfo.Click += new System.EventHandler(this.linkInfo_Click);
            this.spbHistory.Click += new System.EventHandler(this.linkHistory_Click);
            this.spbImage.Click += new System.EventHandler(this.linkImage_Click);
            this.rbResultType.SelectedIndexChanged += new System.EventHandler(this.rbResultType_SelectedIndexChanged);
        }

        private void PatResult1_LostItemInfo(object obj, CalInfoEventArgs args)
        {
            //lblLostItems.Visible = args.lostitem;
        }

        private void PatResult1_NullItemInfo(object obj, CalInfoEventArgs args)
        {
            lblNotEmptyItem.Text = args.TipText;
        }

        void patResult1_ClaItemInfo(object obj, dcl.client.result.PatControl.CalInfoEventArgs args)
        {
            args.SampName = this.txtPatSampleType.displayMember;
            args.SampRem = this.txtPatSamRem.popupContainerEdit1.Text;
            args.SampID = this.txtPatSampleType.valueMember;
            args.itm_itr_id = this.txtPatInstructment.valueMember;
            args.pat_weight = this.fpat_weight.Text;
            args.Sex = this.txtPatSex.valueMember;
            if (textAgeInput1.AgeYear != null || textAgeInput1.AgeMonth != null)
                args.Age = this.textAgeInput1.AgeYear == null ? ((textAgeInput1.AgeMonth ?? 0) / 12) : textAgeInput1.AgeYear;
        }

        public override bool showReport
        {
            get
            {
                string result = UserInfo.GetSysConfigValue("ShowReportUnderResult");

                if (result == "是")
                    return true;

                return false;
            }
        }


        #region IPatEnter 成员

        public string[] ToolBarStyle
        {
            get
            {
                string btnCalculationName = this.Lab_DisplaySamReturnButton ? sysToolBar1.btnCalculation.Name : "";
                string strFirstAudit = "BtnAudit";

                string prv = string.Empty;

                string btnAuditAndPrint = UserInfo.GetSysConfigValue("Lab_ReportAndPrintCusName");
                string BtnSinglePrint = "";
                string BtnUndoReport = string.Empty;
                if (!string.IsNullOrEmpty(btnAuditAndPrint))
                {
                    BtnSinglePrint = "BtnSinglePrint";
                    sysToolBar1.BtnSinglePrint.Caption = btnAuditAndPrint;
                }


                if (UserInfo.GetSysConfigValue("Lab_ShowPrintPreviewBtn") == "是")
                {
                    prv = "BtnPrintList";
                    sysToolBar1.BtnPrintList.Caption = "打印预览";
                }

                if (UserInfo.GetSysConfigValue("Lab_ShowFirstAuditButton") == "否")
                    strFirstAudit = string.Empty;
                string btnDeRef = string.Empty;
                if (UserInfo.GetSysConfigValue("Open_HisFeeView") == "是")
                {
                    btnDeRef = "BtnDeRef";
                    sysToolBar1.BtnDeRef.Caption = "费用清单";
                }

                string BtnQualityAudit = string.Empty;

                if (UserInfo.GetSysConfigValue("Lab_EnableNoBarCodeCheck") == "是")
                {
                    BtnQualityAudit = "BtnQualityAudit";
                    sysToolBar1.BtnQualityAudit.Caption = "打印确认";
                }
                string BtnUploadVersion = string.Empty;
                if (UserInfo.GetSysConfigValue("Lab_EnableUpload") == "是")
                {
                    BtnUploadVersion = "BtnUploadVersion";
                    sysToolBar1.BtnUploadVersion.Caption = "上传数据";
                }

                //FrmPatInputBaseNew  在此窗体把控制按钮visable为false
                sysToolBar1.BtnClear.Caption = "批量删除项目";
                sysToolBar1.BtnQuickEntry.Caption = "血型复核";

                if (UserInfo.GetSysConfigValue("lab_Undo_Report_button") == "是")
                {
                    BtnUndoReport = "BtnUndo";
                    string word = LocalSetting.Current.Setting.ReportWord == string.Empty ? "报告" : LocalSetting.Current.Setting.ReportWord;
                    sysToolBar1.BtnUndo.Caption = "取消" + word;
                    sysToolBar1.BtnUndo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    string word1 = LocalSetting.Current.Setting.ReportWord == string.Empty ? "审核" : LocalSetting.Current.Setting.AuditWord;
                    sysToolBar1.BtnUndo2.Caption = "取消" + word1;
                    sysToolBar1.BtnUndo2.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }

                //报告解读
                string BtnResultJudge = string.Empty;
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Interpretation_Report") == "是")
                {
                    BtnResultJudge = sysToolBar1.BtnResultJudge.Name;
                    sysToolBar1.BtnResultJudge.Caption = "报告解读";
                    sysToolBar1.OnBtnResultJudgeClicked += SysToolBar1_OnBtnResultJudgeClicked;
                }

                string BtnMZImport = "";//门诊导入按钮
                if (UserInfo.GetSysConfigValue("Lab_ShowMZImport") == "是")
                {
                    BtnMZImport = sysToolBar1.BtnSelectTemplate.Name;
                    sysToolBar1.BtnSelectTemplate.Caption = "门诊导入";
                    sysToolBar1.BtnSelectTemplateClick += SysToolBar1_BtnSelectTemplateClick;
                }

                string BtnQuickRegister = "";//门诊快速登记
                if (UserInfo.GetSysConfigValue("Lab_QuickRegister") == "是")//以后增加配置是否可见，门诊导入暂用于茂名妇幼
                {
                    BtnQuickRegister = sysToolBar1.BtnPageUp.Name;
                    sysToolBar1.BtnPageUp.Caption = "快速登记";
                    sysToolBar1.BtnPageUp.Tag = "F2";
                    sysToolBar1.OnBtnPageUpClicked += SysToolBar1_OnBtnPageUpClicked;
                }


                string BtnRematch = "";//快速匹配
                BtnRematch = sysToolBar1.BtnExport.Name;
                sysToolBar1.BtnExport.Caption = "重新匹配";
                sysToolBar1.OnBtnExportClicked += SysToolBar1_OnBtnExportClicked;

                sysToolBar1.btnAntibiotics.Caption = "追加条码";

                return new string[] {
                                    this.sysToolBar1.BtnAdd.Name,
                                    "BtnSave",
                                    "BtnDelete",
                                    "BtnRefresh",
                                    sysToolBar1.BtnAudit.Name,
                                    "BtnUndo2",
                                    "BtnReport",
                                     "BtnUndo",
                                    BtnSinglePrint,  //配置中设置(默认:审核并打印)
                                    "BtnSampleMonitor",
                                    "BtnResultView",
                                    BtnRematch,
                                    "BtnPrint",
                                    sysToolBar1.BtnPrintPreview2.Name,
                                    "BtnQualityImage", 
                                    "BtnCopy",
                                    sysToolBar1.BtnClose.Name,
                                    BtnResultJudge,
                                    sysToolBar1.btnAntibiotics.Name,
                                    BtnUploadVersion,
                                    BtnQualityAudit,  //新增打印确认按钮 
                                    BtnMZImport,
                                    BtnQuickRegister
                                    };
            }
        }


        /// <summary>
        /// 报告解读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SysToolBar1_OnBtnResultJudgeClicked(object sender, EventArgs e)
        {
            EntityPidReportMain drFoc = controlPatList.CurrentPatient as EntityPidReportMain;
            if (drFoc != null)
            {
                new FrmReportSumInfo(drFoc.RepId).ShowDialog();
            }
        }

        public void ApplyCustomSetting(PatInputRuntimeSetting UserCustomSetting)
        {
            this.patResult1.ApplyConfig(UserCustomSetting.PatResultPanel);
            tabControlResult.SelectedTabPageIndex = 0;
        }

        public string ItrDataType
        {
            get { return LIS_Const.InstmtDataType.Normal; }
        }

        public void TypeChanged(string typeID)
        {

        }

        public void InstructmentChanged(string itr_id)
        {
            //更改仪器数据类型
            patResult1.Itr_rep_flag = DictInstrmt.Instance.GetItrRepFlag(itr_id);
            patResult1.Itr_ptype = DictInstrmt.Instance.GetItrPTypeID(itr_id);
            patResult1.Pat_itr_id = itr_id;
            patResult1.SetPicVisable(itr_id);
            //更新样本类型
            this.patResult1.SamType_id = this.txtPatSampleType.valueMember;
        }

        public void Reset()
        {
            this.patResult1.Reset();
        }

        public void ResReset()
        {
            this.patResult1.ResetForBarcode();
        }

        public void AddNew()
        {
            Reset();
            this.patResult1.PatID = string.Empty;
            this.patResult1.Reset();

            this.patResult1.PatAge = this.textAgeInput1.AgeToMinute;
            this.patResult1.SamType_id = this.txtPatSampleType.valueMember;
            patResult1.Pat_dep_id = txtPatDeptId.valueMember;
            this.patResult1.PatSex = this.txtPatSex.valueMember == null ? string.Empty : this.txtPatSex.valueMember.ToString();
            this.patResult1.Samrem = this.txtPatSamRem.popupContainerEdit1.Text == null ? string.Empty : this.txtPatSamRem.popupContainerEdit1.Text;

            //新增时设计所有属性均可编辑
            setIsModify(true);
            patResult1.setResultCouldModify(false);
            this.combineEditor1.Enabled = true;
        }

        public void SetItrDefaultCombine(string itr_id)
        {
            this.patResult1.Reset();
        }

        public bool CheckResultBeforeAction(string pat_id, bool isAudit)
        {
            try
            {

                EntityPidReportMain patient = null;

                if (PatInfo != null && bsPat != null && bsPat.Current != null)
                {
                    bsPat.EndEdit();
                    patient = this.controlPatList.CurrentPatient;
                    FillUIValueToDataRow(patient);
                }

                //更新病人结果表
                List<EntityObrResult> listPatientResulto = new List<EntityObrResult>();
                listPatientResulto = this.patResult1.GetResultTable();


                List<EntityObrResult> listResultCopy = listPatientResulto;

                for (int i = listPatientResulto.Count - 1; i >= 0; i--)
                {
                    EntityObrResult result = listResultCopy[i];

                    if (!string.IsNullOrEmpty(result.ObrValue)
                        && result.ObrValue.Trim(null) != string.Empty)
                    {
                        if (string.IsNullOrEmpty(result.IsNew.ToString()))
                            result.IsNew = 1;

                        string res_chr = result.ObrValue.Trim();

                        decimal res_cast_chr;
                        if (decimal.TryParse(res_chr, out res_cast_chr))
                        {
                            result.ObrConvertValue = res_cast_chr;
                        }

                        if (Convert.ToBoolean(result.IsNew))
                        {
                            result.ObrItrId = this.txtPatInstructment.valueMember;
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(result.IsNew) == true) //如果是新增则空结果不保存
                        {
                            listResultCopy.Remove(result);
                        }
                    }
                }

                if (listResultCopy.Count == 0 || listResultCopy.Where(w => w.ObrId == pat_id).Count() == 0)
                    return true;
                bool isNeedWraning = false;
                EntityQcResultList resultList = new ProxyPatResult().Service.GetPatientCommonResult(pat_id, false);
                List<EntityObrResult> listPatResult = resultList.listResulto;
                foreach (EntityObrResult CurrResult in listPatResult)
                {
                    string itmecd = CurrResult.ItmId;
                    List<EntityObrResult> listOrigin = listResultCopy.Where(w => w.ItmId == itmecd).ToList();
                    if (listOrigin.Count == 0)
                    {
                        isNeedWraning = true;
                        break;
                    }
                    EntityObrResult OriginResult = listOrigin[0];
                    if (!ObjectEquals(OriginResult.ObrValue, CurrResult.ObrValue))
                    {
                        isNeedWraning = true;
                        break;
                    }
                }

                if (!isAudit && patient != null)
                {
                    if (string.IsNullOrEmpty(txtPatName.Text) && string.IsNullOrEmpty(txtPatID.Text))
                        return true;

                    EntityPidReportMain patData = resultList.patient;

                    if (patData != null)
                    {
                        EntityPidReportMain OriginPat = patData;

                        if (OriginPat.RepStatus != null && (OriginPat.RepStatus.ToString() == "1" || OriginPat.RepStatus.ToString() == "2" || OriginPat.RepStatus.ToString() == "4"))
                            return true;
                        if (!ObjectEquals(OriginPat.PidSex, patient.PidSex))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidName, patient.PidName))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.SampRemark, patient.SampRemark))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidAge, patient.PidAge))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidDeptCode, patient.PidDeptCode))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidDoctorCode, patient.PidDoctorCode))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidDiag, patient.PidDiag))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidRemark, patient.PidRemark))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidSamId, patient.PidSamId))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidBedNo, patient.PidBedNo))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidSrcId, patient.PidSrcId))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidPurpId, patient.PidPurpId))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidSrcId, patient.PidSrcId))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.RepCtype, patient.RepCtype))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.RepCheckUserId, patient.RepCheckUserId))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.RepCheckUserId, patient.RepCheckUserId))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.PidRemark, patient.PidRemark))
                        {
                            return false;
                        }
                        if (!ObjectEquals(OriginPat.RepRemark, patient.RepRemark))
                        {
                            return false;
                        }
                    }
                }

                if (!isAudit && listPatResult.Count > 0)
                {
                    foreach (EntityObrResult CurrResult in listResultCopy)
                    {
                        string itmecd = CurrResult.ItmId;
                        List<EntityObrResult> OriginResult = listPatResult.Where(w => w.ItmId == itmecd).ToList();
                        if (OriginResult.Count == 0)
                        {
                            isNeedWraning = true;
                            break;
                        }
                    }
                }

                if (isNeedWraning)
                {
                    if (isAudit)
                    {
                        MessageDialog.Show("审核失败,当前结果与数据库结果不一致，请刷新后再审核", "提示");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("操作失败" + ex.Message, 3m);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="patient">病人信息</param>
        /// <returns></returns>
        public string Save(EntityPidReportMain patient)
        {
            try
            {
                string pat_id = string.Empty;

                //更新病人结果表
                List<EntityObrResult> listObrResult = new List<EntityObrResult>();

                this.patResult1.ItemCalcNotSave();
                listObrResult = this.patResult1.GetResultTable();

                List<EntityObrResult> listResultCopy = EntityManager<EntityObrResult>.ListClone(listObrResult);
                for (int i = listObrResult.Count - 1; i >= 0; i--)
                {
                    EntityObrResult result = listObrResult[i];

                    if (!string.IsNullOrEmpty(result.ObrValue)
                        && result.ObrValue.ToString().Trim(null) != string.Empty)
                    {
                        if (string.IsNullOrEmpty(result.IsNew.ToString()))
                            result.IsNew = 1;

                        string res_chr = result.ObrValue.Trim();

                        double douTemp = 0;
                        decimal res_cast_chr;
                        if (decimal.TryParse((double.TryParse(res_chr, out douTemp) ? douTemp.ToString() : res_chr), out res_cast_chr))
                        {
                            result.ObrConvertValue = res_cast_chr;
                        }

                        if (Convert.ToBoolean(result.IsNew))
                        {
                            result.ObrItrId = this.txtPatInstructment.valueMember;
                        }
                    }
                    else
                    {
                        if (Convert.ToBoolean(result.IsNew) == true)//如果是新增则空结果不保存
                        {
                            listObrResult.Remove(result);
                        }
                    }
                }
                EntityQcResultList resultlist = new EntityQcResultList();
                resultlist.patient = patient;
                resultlist.listResulto = listObrResult;
                resultlist.listResultoNoFliter = listResultCopy;
                if (combineEditor1.listRepDetail != null && combineEditor1.listRepDetail.Count > 0)
                {
                    resultlist.listRepDetail = EntityManager<EntityPidReportDetail>.ListClone(combineEditor1.listRepDetail);
                    patient.ListPidReportDetail = EntityManager<EntityPidReportDetail>.ListClone(combineEditor1.listRepDetail);
                }
                //判断是否有历史结果差异保存当前结果的权限。
                bool blnOperat = UserInfo.HaveFunctionByCode("PatInput_SaveHistoryResult");

                EntityRemoteCallClientInfo remoteCaller = dcl.client.common.Util.ToCallerInfo();
                //remoteCaller.IPAddress = UserInfo.ip;
                //remoteCaller.LoginID = UserInfo.loginID;
                remoteCaller.OperationName = blnOperat.ToString();
                if (IsNew)
                {
                    //无条可录入
                    if (Lab_NoBarcodeNeedAuditCheek
                      && (string.IsNullOrEmpty(Lab_NoBarCodeAuditCheckItrExList) ||
                      !Lab_NoBarCodeAuditCheckItrExList.Contains(txtPatInstructment.valueMember))
                      && patient != null &&
                      (patient.RepBarCode == null
                       || string.IsNullOrEmpty(patient.RepBarCode)))
                    {
                        FrmCheckPassword frmCheck = new FrmCheckPassword("NoBarcode_CanInput");
                        frmCheck.Text = "无条码报告录入确认";
                        DialogResult dig = frmCheck.ShowDialog();
                        if (dig != DialogResult.OK)
                        {
                            return "";
                        }
                        remoteCaller.Remarks = string.Format("由{0}医生确认录入", frmCheck.OperatorName);
                        remoteCaller.LoginID = frmCheck.OperatorID;
                    }

                    EntityOperationResult opResult = new ProxyObrResult().Service.InsertPatCommonResult(remoteCaller, resultlist);
                    if (opResult.Success)
                    {
                        this.PatInfo = resultlist.patient;
                        pat_id = PatInfo.RepId;
                        string pat_sid = PatInfo.RepSid;

                        this.patResult1.UpdateSID(pat_id, pat_sid);

                        this.patResult1.SaveSuccess();

                        this.SearchPatients(false, pat_id);

                        //this.OnAddNew();

                        //焦点下移到最后一行新增的病人
                        //this.controlPatList1.MoveLast();
                    }
                    else
                    {
                        if (opResult.HasExcaptionError)
                        {
                            lis.client.control.MessageDialog.Show("保存失败", "提示");
                        }
                        else
                        {
                            lis.client.control.MessageDialog.Show(OperationMessageHelper.GetErrorMessage(opResult.Message), "提示");
                        }
                    }
                }
                else
                {
                    if (manualModityResultNeedAudit && ResultCache != null)
                    {
                        bool isNeedAudit = false;
                        foreach (EntityObrResult CurrResult in ResultCache)
                        {
                            string itmecd = CurrResult.ItmEname;
                            List<EntityObrResult> listOrigin = listResultCopy.Where(w => w.ItmEname == itmecd).ToList();
                            if (listOrigin.Count == 0)
                            {
                                isNeedAudit = true;
                                break;
                            }
                            EntityObrResult entityOrigin = listOrigin[0];
                            if (!ObjectEquals(entityOrigin.ObrValue, CurrResult.ObrValue))
                            {
                                isNeedAudit = true;
                                break;
                            }
                        }

                        if (isNeedAudit)
                        {
                            FrmCheckPassword frmCheck = new FrmCheckPassword();
                            DialogResult dig = frmCheck.ShowDialog();
                            if (dig != DialogResult.OK)
                            {
                                return pat_id;
                            }
                            remoteCaller.LoginID = frmCheck.OperatorID;
                        }


                    }

                    EntityOperationResult opResult = new ProxyObrResult().Service.UpdatePatCommonResult(remoteCaller, resultlist);
                    if (opResult.Success)
                    {
                        this.PatInfo = resultlist.patient;


                        EntityPidReportMain drCurrentpat = this.controlPatList.CurrentPatient;
                        if (drCurrentpat != null)
                        {
                            curr_patsid = drCurrentpat.RepId.ToString();
                        }
                        pat_id = PatInfo.RepId;
                        string pat_sid = PatInfo.RepSid;

                        this.patResult1.UpdateSID(pat_id, pat_sid);
                        this.patResult1.SaveSuccess();
                    }
                    else
                    {
                        if (opResult.HasExcaptionError)
                        {
                            lis.client.control.MessageDialog.Show("保存失败", "提示");
                        }
                        else
                        {
                            lis.client.control.MessageDialog.Show(OperationMessageHelper.GetErrorMessage(opResult.Message), "提示");
                        }
                    }
                }
                return pat_id;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "Save", ex.ToString());
                throw;
            }
        }

        bool ObjectEquals(object obj1, object obj2)
        {
            if (
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 == null || obj2 == DBNull.Value))
                ||
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 != null && obj2 != DBNull.Value && obj2.ToString() == string.Empty))
                ||
                ((obj2 == null || obj2 == DBNull.Value) && (obj1 != null && obj1 != DBNull.Value && obj1.ToString() == string.Empty))
            )
            {
                return true;
            }
            else
            {
                decimal v1, v2;

                if (decimal.TryParse(obj1.ToString(), out v1) && decimal.TryParse(obj2.ToString(), out v2) && v1 == v2)
                {
                    return true;
                }

                if (obj1.ToString() == obj2.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        /// <summary>
        /// 性别改变
        /// </summary>
        /// <param name="pat_sex"></param>
        public void SexChanged(string pat_sex)
        {
            SetPatResultProperty(true, false);
            this.patResult1.ItemsCountCheck();
        }
        #endregion


        /// <summary>
        /// 设置病人结果控件属性
        /// </summary>
        public void SetPatResultProperty(bool bCalItemRef, bool bGetItemSam)
        {
            this.patResult1.PatAge = this.textAgeInput1.AgeToMinute;
            this.patResult1.PatDiag = this.txtPatDiag.valueMember;
            this.patResult1.SamType_id = this.txtPatSampleType.valueMember;
            this.patResult1.PatSex = this.txtPatSex.valueMember == null ? string.Empty : this.txtPatSex.valueMember.ToString();
            this.patResult1.Samrem = this.txtPatSamRem.popupContainerEdit1.Text == null ? string.Empty : this.txtPatSamRem.popupContainerEdit1.Text;
            patResult1.Pat_dep_id = txtPatDeptId.valueMember;
            this.patResult1.CalAllRowsItemRef(bCalItemRef, bGetItemSam, false);

        }


        #region IPatEnter 成员

        /// <summary>
        /// ID改变
        /// </summary>
        /// <param name="PatIDType"></param>
        /// <param name="PatID"></param>
        public void PatIDChanged(string PatIDType, string PatID)
        {

        }

        /// <summary>
        /// ID类型改变
        /// </summary>
        /// <param name="PatIDType"></param>
        public void PatIDTypeChanged(string PatIDType)
        {

        }



        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="listPat"></param>
        public List<EntityOperationResult> DeleteBatch(EntityRemoteCallClientInfo caller, List<string> listPat)
        {
            DialogResult delDiaResult = lis.client.control.MessageDialog.Show("是否连同病人结果一起删除？", "确认", MessageBoxButtons.YesNoCancel);

            List<EntityOperationResult> listOpResult = new List<EntityOperationResult>();
            if (delDiaResult != DialogResult.Cancel)
            {
                bool bDelResult = (delDiaResult == DialogResult.Yes);
                foreach (string patId in listPat)
                {
                    EntityOperationResult result = new EntityOperationResult();
                    result = new ProxyObrResult().Service.DelPatCommonResult(caller, patId, bDelResult, false);
                    listOpResult.Add(result);

                }

            }
            else
            {
                listOpResult = new List<EntityOperationResult>();
            }
            return listOpResult;
        }

        #endregion


        #region IPatEnter 成员


        public void ResultView(DateTime date, string itr_id)
        {
            FrmRealTimeResultView frm = new FrmRealTimeResultView(date, itr_id);
            frm.Show();
        }

        #endregion

        /// <summary>
        /// 加载病人数据
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="PatInfo"></param>
        /// <param name="dtPatCombine"></param>
        public void LoadPatientData(string patID, ref EntityPidReportMain patient, ref List<EntityPidReportDetail> listPatCombine)
        {
            ProxyPatResult proxy = new ProxyPatResult();
            //获取病人资料（病人基本信息、病人检验组合、病人普通结果）
            EntityQcResultList resultList = proxy.Service.GetPatientCommonResult(patID, false);

            if (resultList.patient != null)
            {
                //病人资料表
                patient = resultList.patient;
            }
            else
            {
                patient = new EntityPidReportMain();
            }

            if (patient != null)
            {
                //病人组合表
                listPatCombine = patient.ListPidReportDetail;
            }
            else
            {
                listPatCombine = new List<EntityPidReportDetail>();
            }

            List<EntityObrResult> listPatResult = resultList.listResulto;

            if (manualModityResultNeedAudit)
            {
                ResultCache = listPatResult;
            }

            this.patResult1.bAllowCalItemRef = true;

            this.patResult1.LoadResult(patient, listPatResult, listPatCombine);
            this.patResult1.bAllowCalItemRef = false;

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
                            if (PatInfo != null && PatInfo.RepBarCode.ToString().Length > 0)
                            {
                                base.setIsModify(false);
                            }
                        }
                        else
                        {
                            this.patResult1.setResultCouldModify(true);
                            base.setIsModify(false);
                            this.combineEditor1.Enabled = false;
                        }
                    }
                }
                else
                {
                    this.patResult1.setResultCouldModify(false);
                    base.setIsModify(true);
                    this.combineEditor1.Enabled = true;
                }
            }
            else
            {
                this.patResult1.setResultCouldModify(false);
                base.setIsModify(true);
                this.combineEditor1.Enabled = true;
            }
            //*******************************************************************************************//
        }

        #region IPatEnter 成员


        public ICombineEditor CombineEditor
        {
            get { return this.combineEditor1; }
        }

        #endregion

        #region IPatEnter 成员


        public void PatAgeChanged(int ageMinute)
        {
            SetPatResultProperty(true, false);
        }
        public void PatDiagChanged(string patDiag)
        {
            SetPatResultProperty(true, false);
        }
        public void DepChanged(string depid)
        {
            patResult1.Pat_dep_id = depid;
            SetPatResultProperty(true, false);
        }
        public void SampleChanged(string sam_id)
        {
            SetPatResultProperty(true, true);
        }

        #endregion

        protected override void RefreshPatientDetails(string patID)
        {
            this.patResult1.bAllowCalItemRef = false;
            base.RefreshPatientDetails(patID);
            this.patResult1.bAllowCalItemRef = true;
            EntityPidReportMain _drPat = this.controlPatList.CurrentPatient;

            //*******************************************************************
            //判断是否有权修改检验报告管理信息
            if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManager") == "是"
                && ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManagerMode") != "gzzyy")
            {
                if (!isCanModify)
                {
                    if (!IsNew)
                    {
                        //系统配置：不能修改报告管理信息[模式]
                        if (ConfigHelper.GetSysConfigValueWithoutLogin("CanNotModifyReportManagerMode") == "gzzyy")
                        {
                        }
                        else
                        {
                            this.patResult1.setResultCouldModify(true);
                            this.combineEditor1.Enabled = false;
                        }
                    }
                }
                else
                {
                    this.patResult1.setResultCouldModify(false);
                    this.combineEditor1.Enabled = true;
                }
            }
            else
            {
                this.patResult1.setResultCouldModify(false);
                this.combineEditor1.Enabled = true;

            }

            //中山3院 特定仪器直接弹出图像浏览
            string strItr = ConfigHelper.GetSysConfigValueWithoutLogin("AutoShowPhoto_ItrId");
            string[] stItrs = strItr.Split(new char[] { ',' });
            List<string> listItr = new List<string>();
            foreach (string itr in stItrs)
            {
                listItr.Add(itr);
            }
            if (_drPat != null && listItr.Contains(_drPat.RepItrId.ToString()))
            {
                ProxyObrResultImage proxyImage = new ProxyObrResultImage();
                List<EntityObrResultImage> listObrResImage = new List<EntityObrResultImage>();
                listObrResImage = proxyImage.Service.GetObrResultImage(patID);//获取病人图像结果
                if (listObrResImage != null && listObrResImage.Count > 0)
                {
                    if (frmext == null || frmext.IsDisposed)
                    {
                        frmext = new FrmPatInfoExt();
                        int ScreenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                        int ScreenHeight = Screen.PrimaryScreen.WorkingArea.Height;
                        frmext.Width = ScreenWidth / 6;
                        frmext.Height = ScreenHeight - 20;
                        frmext.Tag = "图像浏览";
                        //计算窗体显示的坐标值，可以根据需要微调几个像素
                        int x = ScreenWidth - frmext.Width - 5;
                        int y = ScreenHeight - frmext.Height;

                        frmext.Location = new Point(x, y);
                        frmext.TopMost = true;
                        frmext.Show();
                    }
                    frmext.LoadImage(listObrResImage);
                }
            }
            else
            {
                if (frmext != null)
                    frmext.Close();
            }
        }

        #region IPatEnter 成员


        public void QualityImageView(DateTime date, string itr_id, string itr_mid)
        {
            FrmChart frmChart = new FrmChart(itr_id, itr_mid);
            frmChart.Show();
        }

        #endregion

        #region IPatEnter 成员


        //public void SamRemChanged(string sam_rem)
        //{
        //    SetPatResultProperty(true, true);
        //    if (!isDataFill)
        //    {
        //        this.patResult1.ItemCalc(false);
        //    }
        //}

        public void SetColumnFocus()
        {
            patResult1.SetColumnFocus();
        }

        #endregion

        #region IPatEnter 成员


        public void PatDateChanged(DateTime dt)
        {
            //if (this.controlResultMerge1 != null)
            //{
            //    this.controlResultMerge1.PatDate = dt;
            //}
        }

        #endregion

        #region  Load


        public bool HasNotManualResult()
        {
            List<EntityObrResult> listResultTableTemp = patResult1.GetResultTable();
            return (listResultTableTemp == null || listResultTableTemp.Count <= 0);
        }


        public bool ShouldCheckWhenPatSidLeave
        {
            get { return true; }
        }


        public void PatSIDChanged(string pat_id, bool merge)
        {
            if (merge)
                this.patResult1.MergeResult(pat_id);
            else
            {
                ProxyPatResult proxy = new ProxyPatResult();
                //获取病人资料（病人基本信息、病人检验组合、病人普通结果）
                EntityQcResultList resultList = proxy.Service.GetPatientCommonResult(pat_id, false);

                //病人资料表
                EntityPidReportMain patient = resultList.patient;

                List<EntityPidReportDetail> listPatCombine = new List<EntityPidReportDetail>();
                //病人组合表
                if (patient != null && patient.ListPidReportDetail.Count > 0)
                {
                    listPatCombine = patient.ListPidReportDetail;
                }
                List<EntityObrResult> listPatResult = resultList.listResulto;

                if (PatEnter.CombineEditor.listRepDetail != null)
                {
                    PatEnter.CombineEditor.listRepDetail.Clear();
                }
                PatEnter.CombineEditor.listRepDetail = listPatCombine;

                this.patResult1.LoadResult(patient, listPatResult, listPatCombine);
            }
        }

        #endregion

        private void FrmPatEnter_Load(object sender, EventArgs e)
        {
            this.sysToolBar1.BtnSearch.Caption = "镜检";

            this.sysToolBar1.BtnAntibioticsClick += btnRecheck_Click;

            this.ActiveControl = this.txtPatInstructment;

            if (ConfigHelper.GetSysConfigValueWithoutLogin("PatResultColumnSort") != string.Empty)
            {
                patResult1.setColumnSort();
            }
        }


        private void btnRecheck_Click(object sender, EventArgs e)
        {
            if (this.controlPatList.CurrentPatient == null)
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
                    CombineEditor.AddCombine(com.ComId, com.OrderSn, Convert.ToDecimal(com.OrderPrice), com.RepBarCode);
                }
                Save();
            }
        }


        private void linkHistory_Click(object sender, EventArgs e)
        {
            if (this.controlPatList.CurrentPatient != null)
            {
                if (frmext != null)
                    frmext.Close();
                frmext = new FrmPatInfoExt();
                frmext.Tag = "历史结果";
                frmext.LoadHistory(this.controlPatList.CurrentPatient.RepId.ToString(), this.patResult1.GetResultShowData());
                frmext.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
            }
        }

        private void linkImage_Click(object sender, EventArgs e)
        {
            if (this.controlPatList.CurrentPatient != null)
            {
                if (frmext != null)
                    frmext.Close();
                frmext = new FrmPatInfoExt();
                frmext.Tag = "图像浏览";
                frmext.LoadImage(this.controlPatList.CurrentPatient.RepId.ToString());
                frmext.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
            }
        }

        private void linkInfo_Click(object sender, EventArgs e)
        {
            if (this.controlPatList.CurrentPatient != null)
            {
                if (frmext != null)
                    frmext.Close();
                frmext = new FrmPatInfoExt();
                frmext.Tag = "标本信息";
                frmext.LoadInfo(this.controlPatList.CurrentPatient.RepId.ToString(), listPatCombine
                    , this.controlPatList.CurrentPatient.RepBarCode.ToString());
                frmext.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
            }
        }


        FrmPatInfoExt frmext = null;
        private void linkRelateRes_Click(object sender, EventArgs e)
        {
            if (this.controlPatList.CurrentPatient != null)
            {
                if (frmext != null)
                    frmext.Close();
                frmext = new FrmPatInfoExt();
                frmext.Tag = "相关结果";
                EntityPidReportMain patient = new EntityPidReportMain();
                patient.RepId = controlPatList.CurrentPatient.RepId.ToString();
                patient.PidIdtId = this.txtPatIdType.valueMember;
                patient.PidInNo = this.txtPatID.Text;
                patient.RepStatus = 0;
                frmext.LoadRelateRes(patient);
                frmext.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择报告信息");
            }
        }

        private void linkMergerRes_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(controlPatList.ItrID))
            {
                if (frmext != null)
                    frmext.Close();
                frmext = new FrmPatInfoExt();
                frmext.Tag = "结果合并";
                frmext.LoadRelateMergeRes(txtPatDate.Text
                    , controlPatList.TypeName, controlPatList.TypeID, controlPatList.ItrName, controlPatList.ItrID, (DateTime)this.txtPatDate.EditValue);
                frmext.Width = frmext.Width + 100;
                frmext.Show();
            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("请选择仪器");
            }
        }

        private void txtPatSampleState_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            string sampleStatus = txtPatSampleState.valueMember;
            if (!string.IsNullOrEmpty(sampleStatus))
            {
                if (!sampleStatus.Equals("合格"))
                {
                    this.txtPatSampleState.PFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
                    this.txtPatSampleState.popupContainerEdit1.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    this.txtPatSampleState.PFont = new System.Drawing.Font("Tahoma", 9F);
                    this.txtPatSampleState.popupContainerEdit1.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void rbResultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            patResult1.radioGroup1.SelectedIndex = rbResultType.SelectedIndex;
        }


        /// <summary>
        /// 门诊导入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SysToolBar1_BtnSelectTemplateClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(controlPatList.ItrID))
            {
                lis.client.control.MessageDialog.Show("请选择仪器", "提示");
                return;
            }
            FrmMZImport Frm = new FrmMZImport(controlPatList.ItrID, controlPatList.ItrName);
            Frm.ShowDialog();
        }

        /// <summary>
        /// 快速登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SysToolBar1_OnBtnPageUpClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(controlPatList.ItrID))
            {
                lis.client.control.MessageDialog.Show("请选择仪器", "提示");
                return;
            }

            FrmSampleRegisterNew frmRegister = new FrmSampleRegisterNew(true, controlPatList.ItrID);
            frmRegister.FormClosed += FrmRegister_FormClosed;
            frmRegister.ShowDialog();
        }

        private void FrmRegister_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.RefreahData();
        }

        /// <summary>
        /// 重新匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            if (this.controlPatList.CurrentPatient == null
                || string.IsNullOrEmpty(this.controlPatList.CurrentPatient.RepId))
            {
                lis.client.control.MessageDialog.Show("请需要匹配的标本记录！", "提示");
                return;
            }

            if (this.controlPatList.CurrentPatient.RepStatus != 0)
            {
                lis.client.control.MessageDialog.Show("该标本不是录入状态，不能进行匹配！", "提示");
                return;
            }

            bool result = new ProxyObrResult().Service.SaveOrignObrResult(this.controlPatList.CurrentPatient);
            if (!result)
            {
                string message;
                if (this.controlPatList.CurrentPatient.PidSrcId == "107")
                {
                    message = string.Format("   门诊登记标本时，匹配结果源数据失败!,录入日期为： {0}, 仪器代码为：{1},   样本号为：{2}", this.controlPatList.CurrentPatient.RepInDate, this.controlPatList.CurrentPatient.RepItrId, this.controlPatList.CurrentPatient.RepSid);
                }
                else if (this.controlPatList.CurrentPatient.PidSrcId == "108")
                {
                    message = string.Format("   住院登记标本时，匹配结果源数据失败!,录入日期为： {0}, 仪器代码为：{1},   样本号为：{2}", this.controlPatList.CurrentPatient.RepInDate, this.controlPatList.CurrentPatient.RepItrId, this.controlPatList.CurrentPatient.RepSid);
                }
                else
                {
                    message = string.Format("   登记标本时，病人类型不存在!,录入日期为： {0}, 仪器代码为：{1},   样本号为：{2}", this.controlPatList.CurrentPatient.RepInDate, this.controlPatList.CurrentPatient.RepItrId, this.controlPatList.CurrentPatient.RepSid);
                }
                lis.client.control.MessageDialog.Show(message, "提示");
            }
            else
            {
                lis.client.control.MessageDialog.ShowAutoCloseDialog("匹配成功！");
            }
        }
    }
}
