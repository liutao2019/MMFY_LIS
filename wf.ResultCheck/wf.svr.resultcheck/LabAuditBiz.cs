using System;
using System.Collections.Generic;
using System.Linq;

using dcl.svr.cache;
using Lib.DAC;
using System.Data;
using dcl.common;
using dcl.svr.resultcheck.Checker;
using dcl.svr.resultcheck.Updater;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.resultcheck
{
    public class LabAuditBiz
    {
        AuditConfig config;
        public LabAuditBiz()
        {
            InitSysConfig();
        }

        bool enabledLog = false;

        /// <summary>
        /// 加载系统配置
        /// </summary>
        private void InitSysConfig()
        {
            config = new AuditConfig();
            config.MergeCheckWithAction = false;
            config.bAllowCallBackPatient = CacheSysConfig.Current.GetSystemConfig("Lab_Allow_CallBackPatient") == "是" ? true : false;
            config.Audit_First_ErrorLevel_PositiveResult = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_PositiveResult"));
            config.Audit_First_ErrorLevel_ResDataTypeError = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_ResDataTypeError"));
            config.Audit_First_ErrorLevel_OverThreshold = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_OverThreshold"));
            config.Audit_First_ErrorLevel_OverCritical = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_OverCritical"));
            config.Audit_First_ErrorLevel_OverRef = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_OverRef"));
            config.Audit_First_ErrorLevel_ItrFalut = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_ItrFalut"));
            config.Audit_First_ErrorLevel_ExistNotAllowValues = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_ExistNotAllowValues"));
            config.Audit_First_ErrorLevel_NotViewReportBeforeAudit = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_NotViewReportBeforeAudit"));
            config.Audit_Second_ErrorLevel_PositiveResult = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_PositiveResult"));
            config.Audit_Second_ErrorLevel_ResDataTypeError = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_ResDataTypeError"));
            config.Audit_Second_ErrorLevel_OverThreshold = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_OverThreshold"));
            config.Audit_Second_ErrorLevel_OverCritical = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_OverCritical"));
            config.Audit_Second_ErrorLevel_OverRef = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_OverRef"));
            config.Audit_Second_ErrorLevel_ItrFalut = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_ItrFalut"));
            config.Audit_Second_ErrorLevel_ExistNotAllowValues = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_ExistNotAllowValues"));
            config.Audit_Second_ErrorLevel_NotViewReportBeforeAudit = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_NotViewReportBeforeAudit"));
            config.UndoAudit_Second_ErrorLevel_DateExpired = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("UndoAudit_Second_ErrorLevel_DateExpired"));
            config.Audit_ReportTimeCheck = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_ReportTimeCheck"));
            config.Audit_PatSpcialTimeCheck = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_PatSpcialTimeCheck"));
            config.ForceAuditerAndReporterMustDiff = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_ForceAuditerAndReporterMustDiff"));
            config.InputerAndReporterMustDiff = CacheSysConfig.Current.GetSystemConfig("Audit_InputerAndReporterMustDiff") == "是" ? true : false;
            config.bAllowStepAuditToReport = CacheSysConfig.Current.GetSystemConfig("AllowStepAuditToReport") == "是" ? true : false;
            config.bCancelCallBackPatientOnAudit = CacheSysConfig.Current.GetSystemConfig("Audit_CancelCallBackPatient") == "否" ? false : true;
            config.bCanInsertDefultItemResult = CacheSysConfig.Current.GetSystemConfig("Audit_CanInsertDefultItemResult") == "是" ? true : false;
            config.bHistoryResultOnlySelectWithName = CacheSysConfig.Current.GetSystemConfig("Lab_ResultHistoryContrainName") == "是" ? true : false;
            config.strHistoryResultSelectField = CacheSysConfig.Current.GetSystemConfig("Lab_HistoryResultSelectField");
            config.OneStepCancelReport = CacheSysConfig.Current.GetSystemConfig("OneStepCancelReport") == "是" ? true : false;
            config.strSameItemResultContrastId = CacheSysConfig.Current.GetSystemConfig("Audit_SameItemResultContrastId");
            config.Lab_ThreeAuditItrIDs = CacheSysConfig.Current.GetSystemConfig("Lab_ThreeAuditItrIDs");
            int iSecondAuditExpDays = 0;
            int.TryParse(CacheSysConfig.Current.GetSystemConfig("UndoAudit_Second_UndoExpiredHours"), out iSecondAuditExpDays);
            config.SecondAuditUndoExpiredDays = iSecondAuditExpDays;
            config.bSecondAuditUndoOnlySelf = CacheSysConfig.Current.GetSystemConfig("UndoAudit_Second_UndoOnlySelf") == "是" ? true : false;
            config.bUndoAuditSecondCheckLookcode = CacheSysConfig.Current.GetSystemConfig("UndoAudit_Second_CheckLookcode") == "是" ? true : false;
            config.bSecondAuditUrgentContainCom = CacheSysConfig.Current.GetSystemConfig("Audit_Sec_UrgentContainCom") == "是" ? true : false;
            config.Audit_NegativeResultCheck = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_NegativeResultCheck"));
            config.Lab_EiasaCheckItmResUseOdValue = CacheSysConfig.Current.GetSystemConfig("Lab_EiasaCheckItmResUseOdValue") == "是";
            config.Audit_First_ErrorLevel_NotAllowSample = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_NotAllowSample"));
            config.IllReportNotAllowPrintMz = CacheSysConfig.Current.GetSystemConfig("Lab_illReportNotAllowPrintMZ") == "是";
            config.CheckCurrentPatientInfo = CacheSysConfig.Current.GetSystemConfig("Audit_CheckCurrentPatientInfo") == "是";
            config.CheckerClItemDealNumOnly = CacheSysConfig.Current.GetSystemConfig("CheckerClItem_DealNumOnly") == "是";
            config.Audit_EnableLineRangeWarning = CacheSysConfig.Current.GetSystemConfig("Audit_EnableLineRangeWarning") == "是";
            config.Audit_EnableReportRangeWarning = CacheSysConfig.Current.GetSystemConfig("Audit_EnableReportRangeWarning") == "是";
            config.Audit_AlloweditPat_i_code = CacheSysConfig.Current.GetSystemConfig("Audit_AlloweditPat_i_code") == "是";
            config.Audit_SendTjCriticalToMsg = CacheSysConfig.Current.GetSystemConfig("Audit_SendTjCriticalToMsg") == "是";
            config.Barcode_CheckCombineAllAudit = CacheSysConfig.Current.GetSystemConfig("Barcode_CheckCombineAllAudit") == "是";

            //报告解读
            config.Interpretation_Report = CacheSysConfig.Current.GetSystemConfig("Interpretation_Report") == "是";

            //不进行负数结果检查的项目(项目id,项目id2)
            string audit_AllowNegativeResult = CacheSysConfig.Current.GetSystemConfig("Audit_AllowNegativeResult");
            if (!string.IsNullOrEmpty(audit_AllowNegativeResult))
            {

                string[] array = audit_AllowNegativeResult.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length > 0)
                {
                    config.Audit_AllowNegativeResult = new List<string>(array);
                }
            }

            config.Audit_ItrZeroCheck = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_ItrZeroCheck"));

            //进行为零结果检查的项目(项目id,项目id2)
            string Audit_IncludeItrZeroCheck = CacheSysConfig.Current.GetSystemConfig("Audit_IncludeItrZeroCheck");
            if (!string.IsNullOrEmpty(Audit_IncludeItrZeroCheck))
            {

                string[] array = Audit_IncludeItrZeroCheck.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length > 0)
                {
                    config.Audit_IncludeItrZeroCheck = new List<string>(array);
                }
            }

            //进行参考值范围结果检查的项目(项目id,项目id2)
            string Audit_AllowOverRefResult = CacheSysConfig.Current.GetSystemConfig("Audit_AllowOverRefResult");
            if (!string.IsNullOrEmpty(Audit_AllowOverRefResult))
            {

                string[] array = Audit_AllowOverRefResult.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length > 0)
                {
                    config.Audit_AllowOverRefResult = new List<string>(array);
                }
            }

            //不进行参考值范围结果检查的项目(项目id,项目id2)
            string Audit_AllowPosResult = CacheSysConfig.Current.GetSystemConfig("Audit_AllowPosResult");
            if (!string.IsNullOrEmpty(Audit_AllowPosResult))
            {

                string[] array = Audit_AllowPosResult.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length > 0)
                {
                    config.Audit_AllowPosResult = new List<string>(array);
                }
            }

            //历史结果差异对比错误提示等级配置
            config.Audit_First_ErrorLevel_HistoryResultCompare = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_HistoryResultCompare"));
            config.Audit_Second_ErrorLevel_HistoryResultCompare = AuditConfig.GetOpErrLv(CacheSysConfig.Current.GetSystemConfig("Audit_Second_ErrorLevel_HistoryResultCompare"));
            config.OneStepAuditTimeZone = CacheSysConfig.Current.GetSystemConfig("Audit_SetOneStepAuditTimeZone");

            if (!string.IsNullOrEmpty(config.OneStepAuditTimeZone) && config.OneStepAuditTimeZone.Split(',').Length == 2)
            {
                DateTime dtStart = Convert.ToDateTime(config.OneStepAuditTimeZone.Split(',')[0]);
                DateTime dtEnd = Convert.ToDateTime(config.OneStepAuditTimeZone.Split(',')[1]);

                if (dtStart <= DateTime.Now && dtStart > dtEnd)
                {
                    dtEnd = dtEnd.AddDays(1);
                }

                if (dtStart > DateTime.Now)
                {
                    if (dtStart < dtEnd)
                    {
                        dtEnd = dtEnd.AddDays(-1);
                    }
                    dtStart = dtStart.AddDays(-1);
                }
                DateTime dtNow = DateTime.Now;

                if (dtNow > dtStart && dtNow < dtEnd)
                {
                    config.bAllowStepAuditToReport = true;
                }
            }
        }

        /// <summary>
        /// 报告解读
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityPidReportMain GetReportInterpretation(string pat_id)
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                var patinfo = mainDao.GetPatientInfo(pat_id);
                if (patinfo == null) return null;

                //如果已经存在报告解读信息，直接返回
                if (!string.IsNullOrEmpty(patinfo.RepSumInfo) && patinfo.RepStatus >= 1) return patinfo;

                patinfo.PidAge = Common.GetConfigAge(patinfo.PidAge);
                patinfo.PidSex = Common.GetConfigSex(patinfo.PidSex);
                //获取病人组合
                List<EntityPidReportDetail> patients_mi = GetPatientsMi(patinfo.RepId);

                //获取病人结果
                List<EntityObrResult> resulto = GetAuditResulto(patinfo.RepId);

                //生成报告解读信息
                new UpdateReportInterpretation(patinfo, patients_mi, resulto,
                    EnumOperationCode.Audit, config).ThreadUpdateInterpretation();

                return mainDao.GetPatientInfo(pat_id);//返回整个病人信息，方便以后扩展
            }

            return null;
        }

        #region 获取病人信息
        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        internal EntityPidReportMain GetPatInfo(string pat_id)
        {
            EntityPatientQC qc = new EntityPatientQC();
            qc.RepId = pat_id;

            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                listPat = mainDao.PatientQuery(qc);
            }

            if (listPat.Count > 0)
            {
                EntityPidReportMain patinfo = listPat[0];
                return patinfo;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取病人检验组合
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        List<EntityPidReportDetail> GetPatientsMi(string pat_id)
        {
            List<EntityPidReportDetail> dtDetail = new List<EntityPidReportDetail>();
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
            if (detailDao != null)
            {
                dtDetail = detailDao.GetPidReportDetailByRepId(pat_id);
            }
            return dtDetail;
        }


        internal List<EntityObrResult> GetAuditResulto(string pat_id)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();

            EntityResultQC resultQc = new EntityResultQC();
            resultQc.ListObrId.Add(pat_id);
            resultQc.ObrFlag = "1";

            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                listResult = resultDao.ObrResultQuery(resultQc);
            }

            return listResult;
        }

        #endregion

    }
}
