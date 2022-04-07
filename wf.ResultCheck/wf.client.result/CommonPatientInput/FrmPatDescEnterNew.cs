using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.result.Interface;
using dcl.client.frame.runsetting;
using dcl.client.frame;
using dcl.client.wcf;
using lis.client.control;
using dcl.client.common;

using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmPatDescEnterNew : FrmPatInputBaseNew, IPatEnter
    {
        public FrmPatDescEnterNew()
        {
            controlPatList.ItrDataType = ItrDataType;
            InitializeComponent();
            this.PatEnter = this;

            this.Load += new System.EventHandler(this.FrmPatDescEnterNew_Load);

            this.combineEditor1.CombineAdded += new CombineAddedEventHandler(ceCombine_CombineAdded);
            this.combineEditor1.CombineRemoved += new CombineRemovedEventHandler(ceCombine_CombineRemoved);
            this.controlPatDescResult1.GetCurInstrmtID += new FrmBscripeSelectV2.GetCurInstrmtIDEventHandler(controlPatDescResult1_GetCurInstrmtID);
            this.txtPatSampleState.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicSState>.ValueChangedEventHandler(this.txtPatSampleState_ValueChanged);

            controlPatList.ShowUpdateMenu();
        }

        string controlPatDescResult1_GetCurInstrmtID()
        {
            return this.txtPatInstructment.valueMember;
        }

        void ceCombine_CombineRemoved(object sender, string com_id)
        {
        }

        void ceCombine_CombineAdded(object sender, string com_id, int com_seq)
        {
        }

        #region IPatEnter 成员

        public string[] ToolBarStyle
        {
            get
            {
                string btnCalculationName = this.Lab_DisplaySamReturnButton ? sysToolBar1.btnCalculation.Name : "";

                string btnAuditAndPrint = UserInfo.GetSysConfigValue("Lab_ReportAndPrintCusName");
                string BtnSinglePrint = "";
                if (!string.IsNullOrEmpty(btnAuditAndPrint))
                {
                    BtnSinglePrint = "BtnSinglePrint";
                    sysToolBar1.BtnSinglePrint.Caption = btnAuditAndPrint;
                }

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
                string BtnUndoReport = string.Empty;
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
                return
                    new string[] {
                                    "BtnAdd",
                                    "BtnSave",
                                    "BtnDelete",
                                    "BtnRefresh",
                                    "BtnAudit",
                                    "BtnUndo2",
                                    "BtnReport",
                                     "BtnUndo",
                                    "BtnPrint",
                                    this.sysToolBar1.BtnPrintPreview2.Name,
                                    "BtnClose",
                                };
            }
        }


        public void ApplyCustomSetting(PatInputRuntimeSetting UserCustomSetting)
        {

        }

        public string ItrDataType
        {
            get { return LIS_Const.InstmtDataType.Description; }
        }

        public void TypeChanged(string typeID)
        {

        }

        public void InstructmentChanged(string itr_id)
        {

        }

        public void Reset()
        {
            this.controlPatDescResult1.Reset();
        }

        public void ResReset()
        {
        }

        public void AddNew()
        {
            this.controlPatDescResult1.Reset();

            //新增时设计所有属性均可编辑
            setIsModify(true);
            this.combineEditor1.Enabled = true;
        }

        public void SetItrDefaultCombine(string itr_id)
        {

        }

        /// <summary>
        /// 性别改变
        /// </summary>
        /// <param name="pat_sex"></param>
        public void SexChanged(string pat_sex)
        {

        }
        #endregion

        #region IPatEnter 成员


        public void PatIDChanged(string PatIDType, string PatID)
        {

        }

        public void PatIDTypeChanged(string PatIDType)
        {

        }

        public void DepChanged(string depid)
        {
        }

     
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="listPat"></param>
        public List<EntityOperationResult> DeleteBatch(EntityRemoteCallClientInfo caller, List<string> listPat)
        {
            DialogResult delDiaResult = lis.client.control.MessageDialog.Show("是否连同病人结果一起删除？", "确认", MessageBoxButtons.YesNoCancel);

            List<EntityOperationResult> listOpResult = null;
            if (delDiaResult != DialogResult.Cancel)
            {
                bool bDelResult = (delDiaResult == DialogResult.Yes);

                ProxyObrResultDesc proxy = new ProxyObrResultDesc();
                listOpResult = proxy.Service.BatchDelPatDescResult(caller, listPat, bDelResult);
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
           
        }

        #endregion

        #region IPatEnter 成员


        public ICombineEditor CombineEditor
        {
            get { return this.combineEditor1; }
        }

        #endregion

        #region IPatEnter 成员


        public void PatAgeChanged(int ageMinute)
        {

        }

        public void SampleChanged(string sam_id)
        {

        }

        #endregion

        #region IPatEnter 成员


        public void QualityImageView(DateTime date, string itr_id, string itr_mid)
        {
        }

        #endregion

        #region IPatEnter 成员


        public void SamRemChanged(string sam_rem)
        {
        }

        public void SetColumnFocus()
        {
        }

        #endregion

        #region IPatEnter 成员


        public void PatDateChanged(DateTime dt)
        {

        }

        #endregion

        #region IPatEnter 成员


        public bool HasNotManualResult()
        {
            return false;
        }

        public bool ShouldCheckWhenPatSidLeave
        {
            get { return false; }
        }


        public void PatSIDChanged(string pat_id, bool merge)
        {
            // IsDataChange = true; 
        }

        public bool CheckResultBeforeAction(string pat_id, bool isAudit)
        {
            try
            {
                ProxyPatResult proxy = new ProxyPatResult();
                //获取病人资料（病人基本信息、病人检验组合、病人普通结果）
                EntityQcResultList resultList = proxy.Service.GetPatientCommonResult(pat_id, false);

                //病人资料表
                EntityPidReportMain patient = resultList.patient;

                //病人组合表
                listPatCombine = patient.ListPidReportDetail;

                ProxyObrResultDesc proxyResultDesc = new ProxyObrResultDesc();
                List<EntityObrResultDesc> dtParDescResult = proxyResultDesc.Service.GetDescResultById(pat_id);//描述结果

                if (patient != null)
                {
                    if (patient.RepStatus != null && (patient.RepStatus.ToString() == "1" || patient.RepStatus.ToString() == "2" || patient.RepStatus.ToString() == "4"))
                        return true;
                }
                if (dtParDescResult != null && dtParDescResult.Count > 0 && !string.IsNullOrEmpty(dtParDescResult[0].ObrDescribe))
                {
                    if (dtParDescResult[0].ObrDescribe != controlPatDescResult1.GetBsr_describe())
                    {
                        return false;
                    }
                }

                if (string.IsNullOrEmpty(txtPatName.Text) && string.IsNullOrEmpty(txtPatID.Text)
                    && string.IsNullOrEmpty(controlPatDescResult1.GetBsr_describe())) return true;

                return !IsDataChange;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowAutoCloseDialog("操作失败" + ex.Message, 3m);
                return false;
            }
        }

        #endregion


        private void FrmPatDescEnterNew_Load(object sender, EventArgs e)
        {
            controlPatList.ItrDataType = ItrDataType;
        }

        private void txtPatSampleState_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            //样本状态字体颜色大小变化
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

        public void PatDiagChanged(string patDiag)
        {
        }

        public string Save(EntityPidReportMain patient)
        {
            EntityQcResultList resultlist = new EntityQcResultList();

            string pat_id = string.Empty;

            List<EntityObrResultDesc> dtDescResult = controlPatDescResult1.GetResult();
            dtDescResult[0].ObrItrId = this.txtPatInstructment.valueMember;

            List<EntityObrResultDesc> list = new List<EntityObrResultDesc>();

            foreach (EntityObrResultDesc item in dtDescResult)
            {
                EntityObrResultDesc desc = item;
                desc.ObrDate = ServerDateTime.GetServerDateTime();
                desc.ObrId = patient.RepId;
                desc.ObrItrId = patient.RepItrId;
                desc.ObrSid = Convert.ToDecimal(patient.RepSid);
                list.Add(desc);
            }

            EntityRemoteCallClientInfo remoteCaller = new EntityRemoteCallClientInfo();
            remoteCaller.IPAddress = UserInfo.ip;
            remoteCaller.LoginID = UserInfo.loginID;

            resultlist.patient = patient;
            resultlist.patient.ListPidReportDetail = PatEnter.CombineEditor.listRepDetail;
            resultlist.listRepDetail = PatEnter.CombineEditor.listRepDetail;
            resultlist.listDesc = list;

            if (IsNew)
            {
                //无条可录入
                if (Lab_NoBarcodeNeedAuditCheek
                    && (string.IsNullOrEmpty(Lab_NoBarCodeAuditCheckItrExList) ||
                    !Lab_NoBarCodeAuditCheckItrExList.Contains(txtPatInstructment.valueMember))
                     &&
                    (string.IsNullOrEmpty(patient.RepBarCode)))
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
                    remoteCaller.OperationName = frmCheck.OperatorName;
                }

                EntityOperationResult opResult = new ProxyObrResult().Service.InsertPatCommonResult(remoteCaller, resultlist);

                if (opResult.Success)
                {
                    this.SearchPatients(false, pat_id);
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
                EntityOperationResult opResult = new ProxyObrResult().Service.UpdatePatCommonResult(remoteCaller, resultlist);

                if (opResult.Success)
                {
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

            ProxyObrResultDesc proxyResultDesc = new ProxyObrResultDesc();
            List<EntityObrResultDesc> dtParDescResult = proxyResultDesc.Service.GetDescResultById(patID);//描述结果
            this.controlPatDescResult1.LoadDescResult(dtParDescResult);

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
                                base.setIsModify(false);
                            }
                        }
                        else
                        {
                            base.setIsModify(false);
                            this.combineEditor1.Enabled = false;
                        }
                    }
                    else
                    {
                        base.setIsModify(true);
                        this.combineEditor1.Enabled = true;
                    }
                }
                else
                {
                    base.setIsModify(true);
                    this.combineEditor1.Enabled = true;
                }
            }
            else
            {
                base.setIsModify(true);
                this.combineEditor1.Enabled = true;
            }
        }
    }
}
