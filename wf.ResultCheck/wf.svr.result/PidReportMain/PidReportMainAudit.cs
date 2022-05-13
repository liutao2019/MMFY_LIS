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
using dcl.servececontract;

namespace dcl.svr.resultcheck
{
    public class PidReportMainAudit : IPidReportMainAudit
    {
        AuditConfig config;

        public PidReportMainAudit()
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
            config.AuditTips_SampStatus = CacheSysConfig.Current.GetSystemConfig("AuditTips_SampStatus");
            config.Audit_UploadYss = CacheSysConfig.Current.GetSystemConfig("Audit_UploadYss") == "是";
            config.Audit_UploadAllPatTypeYss = CacheSysConfig.Current.GetSystemConfig("Audit_UploadAllPatTypeYss") == "是";
            config.Audit_YSSFilterDept = CacheSysConfig.Current.GetSystemConfig("Audit_YSSFilterDept");
            //
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
        /// 描述报告审核前检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType">检查方式 0:审核 1:报告 2:</param>
        /// <returns></returns>
        public EntityOperationResultList DesctAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList resultsList = new EntityOperationResultList();
            foreach (string pat_id in listPatientsID)
            {
                EntityOperationResult result = _DescAuditCheck(pat_id, checkType, caller);
                resultsList.Add(result);
            }
            return resultsList;
        }

        private EntityOperationResult _DescAuditCheck(string pat_id, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            EntityPidReportMain patinfo;

            if (config.CheckCurrentPatientInfo && pat_id.Split(':').Length > 0)
            {
                patinfo = GetPatInfo(pat_id.Split(':')[0]);
            }
            else
            {
                patinfo = GetPatInfo(pat_id);
            }

            //找不到病人资料
            if (patinfo == null)
            {
                EntityOperationResult chkResultErr = new EntityOperationResult();
                chkResultErr.Data.Patient.RepId = pat_id;
                chkResultErr.AddMessage(EnumOperationErrorCode.IDNotExist, EnumOperationErrorLevel.Error);
                return chkResultErr;
            }
            EntityOperationResult chkResult = CreateCheckResult(patinfo);
            if (!config.MergeCheckWithAction)
            {
                //当年龄或性别为空时，根据配置是否取默认的年龄和默认的性别
                patinfo.PidAge = Common.GetConfigAge(patinfo.PidAge);
                patinfo.PidSex = Common.GetConfigSex(patinfo.PidSex);

                //检查病人记录状态
                CheckerPatState checkerPatState = new CheckerPatState(patinfo, checkType, config);
                checkerPatState.Check(ref chkResult);
            }
            CheckerSpcialTimeInfo checkerSpcialTimeInfo = new CheckerSpcialTimeInfo(patinfo, checkType, config);
            checkerSpcialTimeInfo.Check(ref chkResult);
            return chkResult;
        }


        /// <summary>
        /// 审核前检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType">检查方式 0:审核 1:报告 2:</param>
        /// <returns></returns>
        public EntityOperationResultList CommonAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList resultsList = new EntityOperationResultList();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                List<EntityPidReportMain> listPatinfo = mainDao.GetPatientInfo(listPatientsID);

                foreach (EntityPidReportMain pat_id in listPatinfo)
                {
                    EntityOperationResult result = _CommonAuditCheck(pat_id, checkType, caller);
                    resultsList.Add(result);
                }
            }
            return resultsList;
        }


        private EntityOperationResult _CommonAuditCheck(EntityPidReportMain patinfo, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            if (enabledLog)
                Lib.LogManager.Logger.LogInfo("获取病人资料");
            patinfo.RepAuditUserId = caller.LoginID;
            //找不到病人资料
            if (patinfo == null)
            {
                EntityOperationResult chkResultErr = new EntityOperationResult();
                chkResultErr.Data.Patient.RepId = patinfo.RepId;
                chkResultErr.AddMessage(EnumOperationErrorCode.IDNotExist, EnumOperationErrorLevel.Error);
                return chkResultErr;
            }

            if (enabledLog)
                Lib.LogManager.Logger.LogInfo("CreateCheckResult");

            EntityOperationResult chkResult = CreateCheckResult(patinfo);

            if (!config.MergeCheckWithAction)
            {
                //当年龄或性别为空时，根据配置是否取默认的年龄和默认的性别
                patinfo.PidAge = Common.GetConfigAge(patinfo.PidAge);
                patinfo.PidSex = Common.GetConfigSex(patinfo.PidSex);

                //检查病人记录状态
                CheckerPatState checkerPatState = new CheckerPatState(patinfo, checkType, config);
                checkerPatState.Check(ref chkResult);

                if (enabledLog)
                    Lib.LogManager.Logger.LogInfo("检查病人记录状态");

                if (chkResult.Success)//上一步检查成功
                {
                    //获取病人组合
                    List<EntityPidReportDetail> patients_mi = GetPatientsMi(patinfo.RepId);

                    if (enabledLog)
                        Lib.LogManager.Logger.LogInfo("获取病人组合");

                    //获取病人结果
                    List<EntityObrResult> resulto = GetAuditResulto(patinfo.RepId);

                    if (enabledLog)
                        Lib.LogManager.Logger.LogInfo("获取病人结果");

                    //是否没有项目
                    CheckerZeroItem chkZeroItem = new CheckerZeroItem(resulto, checkType, config);
                    chkZeroItem.Check(ref chkResult);

                    if (enabledLog)
                        Lib.LogManager.Logger.LogInfo("是否没有项目");

                    if (chkResult.Success)//上一步检查成功
                    {
                        //超时检查
                        //**********************************************************************
                        //检查标本类别
                        CheckPatInfo checkPatInfo = new CheckPatInfo(patinfo, checkType, config);
                        checkPatInfo.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("检查标本类别");

                        //**********************************************************************

                        //检查标本类别是否正确
                        CheckerItemSample ckItemSam = new CheckerItemSample(patinfo, resulto, checkType, config);
                        ckItemSam.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("检查是否已查看报告单");

                        //质控时间检查
                        CheckerQCRule chkQC = new CheckerQCRule(patinfo, checkType);
                        chkQC.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("质控时间检查");

                        //仪器维护检测
                        CheckerItrMaintainance chkItr = new CheckerItrMaintainance(patinfo, checkType, config);
                        chkItr.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("仪器维护检测");

                        //必录项漏项检查
                        CheckerLostItem chkLostItem = new CheckerLostItem(patinfo, patients_mi, resulto, checkType, config);
                        chkLostItem.Check(ref chkResult);


                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("必录项漏项检查");

                        //系统配置：启用多余项目检查
                        if (CacheSysConfig.Current.GetSystemConfig("Audit_CheckerNotContaintItem") == "是")
                        {
                            //(不包含/多余)项目检查
                            CheckerNotContaintItem chkNotContaintItem = new CheckerNotContaintItem(patinfo, patients_mi, resulto, checkType, config);
                            chkNotContaintItem.Check(ref chkResult);


                            if (enabledLog)
                                Lib.LogManager.Logger.LogInfo("(不包含/多余)项目检查");
                        }

                        //参考值检查
                        CheckerRef chkRef = new CheckerRef(patinfo, resulto, checkType, config, caller);
                        chkRef.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("参考值检查");

                        //项目性别、结果数据类型检查
                        CheckerResultDataType chkDataType = new CheckerResultDataType(patinfo, resulto, checkType, config);
                        chkDataType.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("项目性别、结果数据类型检查");

                        //历史结果差异对比
                        //CheckerHistoryResultCompare chkHistory = new CheckerHistoryResultCompare(patinfo, resulto, checkType, config, caller);
                        //chkHistory.Check(ref chkResult);


                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("历史结果差异对比");

                        //检查是否有召回信息
                        CheckerCallBackPatient chkCallBack = new CheckerCallBackPatient(patinfo, checkType, config);
                        chkCallBack.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("检查是否有召回信息");

                        ////自定义审核脚本
                        CheckerClItem2 ClItem2 = new CheckerClItem2(patinfo, patients_mi, resulto, checkType, config);
                        ClItem2.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("CheckerClItem2");

                        ////自定义效验
                        CheckerEfficacy checkerEfficacy = new CheckerEfficacy(patinfo, patients_mi, resulto, checkType, config, caller);
                        checkerEfficacy.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("checkerEfficacy");

                        //检验复查信息
                        CheckerRecheckInfo recheck = new CheckerRecheckInfo(patinfo, checkType, config);
                        recheck.Check(ref chkResult);

                        CheckerPatInfo patcheck = new CheckerPatInfo(patinfo, checkType, config, patinfo.RepId, caller);
                        patcheck.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("检验复查信息");

                        //报告、审核时增加送检时间与采样时间的判断
                        CheckerReportTimeInfo checkerReportTimeInfo = new CheckerReportTimeInfo(patinfo, patients_mi, resulto, checkType, config);
                        checkerReportTimeInfo.Check(ref chkResult);

                        CheckerSpcialTimeInfo checkerSpcialTimeInfo = new CheckerSpcialTimeInfo(patinfo, checkType, config);
                        checkerSpcialTimeInfo.Check(ref chkResult);

                        if (enabledLog)
                            Lib.LogManager.Logger.LogInfo("报告、审核时增加送检时间与采样时间的判断");

                    }
                }
            }
            return chkResult;
        }

        /// <summary>
        /// 取消二审前检查(批)
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList CommonUndoReoprtCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList resultsList = new EntityOperationResultList();
            foreach (string pat_id in listPatientsID)
            {
                EntityPidReportMain patinfo;
                EntityOperationResult result = _CommonUndoReoprtCheck(pat_id, checkType, caller, out patinfo);
                resultsList.Add(result);
            }
            return resultsList;
        }



        /// <summary>
        /// 取消二审前检查(单)
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="checkType"></param>
        /// <param name="caller"></param>
        /// <param name="patinfo"></param>
        /// <returns></returns>
        private EntityOperationResult _CommonUndoReoprtCheck(string pat_id, EnumOperationCode checkType, EntityRemoteCallClientInfo caller, out EntityPidReportMain patinfo)
        {
            //获取病人资料
            patinfo = GetPatInfo(pat_id);

            if (enabledLog)
                Lib.LogManager.Logger.LogInfo("获取病人资料");

            //找不到病人资料
            if (patinfo == null)
            {
                EntityOperationResult chkResultErr = new EntityOperationResult();
                chkResultErr.Data.Patient.RepId = pat_id;
                chkResultErr.AddMessage(EnumOperationErrorCode.IDNotExist, EnumOperationErrorLevel.Error);
                return chkResultErr;
            }
            else
            {
                EntityOperationResult chkResult = CreateCheckResult(patinfo);

                CheckerPatState checker = new CheckerPatState(patinfo, checkType, config);//否则，只检查病人资料状态
                checker.Check(ref chkResult);

                if (!chkResult.Success)//如果不通过就返回
                {
                    return chkResult;
                }

                CheckerUndoAuditExpired checkerExpired = new CheckerUndoAuditExpired(patinfo, checkType, config, caller);
                checkerExpired.Caller = caller;
                checkerExpired.Check(ref chkResult);

                if (patinfo.RepStatus == (int)EnumPatFlag.Printed)//已打印
                {
                    if (checkType == EnumOperationCode.UndoReport)//取消报告时
                    {
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_Second_CancelPrintPower") != "是")
                            //病人报告已经打印，请收回病人原始报告
                            chkResult.AddCustomMessage("", "", "病人报告已经打印，请收回病人原始报告单", EnumOperationErrorLevel.Warn);
                    }
                }
                if (checkType == EnumOperationCode.UndoReport
                    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_ShowHasUrgentMsgWhenUndoReport") == "是")//取消报告时
                {
                    CheckerUrgentMsg checkerUrgentMsg = new CheckerUrgentMsg(patinfo, checkType, config);
                    checkerUrgentMsg.Check(ref chkResult);
                }

                return chkResult;
            }

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
            EntityPidReportMain patinfo = new EntityPidReportMain();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                patinfo = mainDao.GetPatientInfo(pat_id);
            }
            return patinfo;
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


            //string sql = "select * from patients_mi with(nolock) where pat_id = ?";
            //SqlHelper helper = new SqlHelper();
            //DbCommandEx cmd = helper.CreateCommandEx(sql);
            //cmd.AddParameterValue(pat_id, DbType.AnsiString);

            //DataTable table = helper.GetTable(cmd);

            //List<EntityPidReportDetail> list = Lib.EntityCore.EntityConverter.DataTableToEntityList<EntityPidReportDetail>(table);
            //table.Clear();
            //return list;
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

        /// <summary>
        /// 审核/报告
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        private EntityOperationResultList BatchAduitReport(IEnumerable<string> listPatientsID, EnumOperationCode auditType, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList listResult = new EntityOperationResultList();
            try
            {
                DateTime today = ServerDateTime.GetDatabaseServerDateTime();
                IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                List<string> listSuccessPatID = new List<string>();
                List<string> listOutPatientsID = new List<string>();
                List<string> listCovidPatientsID = new List<string>();
                if (mainDao != null)
                {
                    List<EntityPidReportMain> listPatinfo = mainDao.GetPatientInfo(listPatientsID);

                    foreach (EntityPidReportMain patinfo in listPatinfo)
                    {
                        EntityOperationResult result = AduitReport(patinfo, auditType, caller, today);
                        listResult.Add(result);
                        if (result.Success)
                        {

                            listSuccessPatID.Add(result.Data.Patient.RepId);

                            if (patinfo.PidSrcId == "110")
                            {
                                listOutPatientsID.Add(result.Data.Patient.RepId);
                            }
                            if (patinfo.PidComName.Contains("新冠") || patinfo.PidComName.Contains("新型冠状"))
                            {
                                if (string.IsNullOrEmpty(config.Audit_YSSFilterDept))
                                {
                                    listCovidPatientsID.Add(result.Data.Patient.RepId);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(patinfo.PidDeptCode) && config.Audit_YSSFilterDept.Contains(patinfo.PidDeptCode))
                                    {

                                    }
                                    else
                                    {
                                        listCovidPatientsID.Add(result.Data.Patient.RepId);
                                    }
                                }
                            }
                        }

                    }
                }
                new SendDataToMid().Run(listSuccessPatID, auditType);
                if (config.Audit_UploadYss)
                {
                    if (config.Audit_UploadAllPatTypeYss)
                    {
                        if (listCovidPatientsID != null && listCovidPatientsID.Count > 0)
                        {
                            new SendDataToMid().SendYssReport(listCovidPatientsID.ToList(), auditType);
                        }
                    }
                    else
                    {
                        if (listOutPatientsID != null && listOutPatientsID.Count > 0)
                        {
                            new SendDataToMid().SendYssReport(listOutPatientsID.ToList(), auditType);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listResult;
        }

        /// <summary>
        /// 报告与审核
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="auditType"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        private EntityOperationResult AduitReport(EntityPidReportMain patinfo, EnumOperationCode auditType, EntityRemoteCallClientInfo caller, DateTime today)
        {
            EntityOperationResult chkResult;

            if (config.MergeCheckWithAction)//如果审核与检查一起
            {
                chkResult = _CommonAuditCheck(patinfo, auditType, caller);
            }
            else
            {
                chkResult = CreateCheckResult(patinfo);
                CheckerPatState checker = new CheckerPatState(patinfo, auditType, config);//否则，只检查病人资料状态
                checker.Check(ref chkResult);

                CheckerUndoAuditExpired checkerExpired = new CheckerUndoAuditExpired(patinfo, auditType, config, caller);
                checkerExpired.Caller = caller;
                checkerExpired.Check(ref chkResult);
            }

            try
            {
                //检查:一审人员和二审人员是否一致
                //12-11-2 添加-检查:只有二审者可以取消二审
                CheckerAuditAndReportUserinfoConsistent checkerCon = new CheckerAuditAndReportUserinfoConsistent(patinfo, auditType, config, caller);
                checkerCon.Check(ref chkResult);



                if (chkResult.HasError)//检查成功再进行下一步处理
                {
                    return chkResult;
                }

                ClearPatSendFlag cpsf = new ClearPatSendFlag(patinfo, auditType);
                cpsf.Update(ref chkResult);

                //当年龄或性别为空时，根据配置是否取默认的年龄和默认的性别
                //patinfo.EnablePropertiesValueChangedLog = false;
                patinfo.PidAge = Common.GetConfigAge(patinfo.PidAge);
                patinfo.PidSex = Common.GetConfigSex(patinfo.PidSex);
                //一审后设置为未查看报告单
                if (auditType == EnumOperationCode.Audit)//一审后设置为未查看报告单
                    patinfo.RepReadUserId = null;

                //获取病人组合
                List<EntityPidReportDetail> patients_mi = GetPatientsMi(patinfo.RepId);

                //获取病人结果
                List<EntityObrResult> resulto = GetAuditResulto(patinfo.RepId);


                //更新结果表信息
                UpdateResulto urv = new UpdateResulto(patinfo, patients_mi, resulto, auditType, config);
                urv.Update(ref chkResult);

                //更新一二审人/时间
                UpdateOperatorNTime upOpDt = new UpdateOperatorNTime(patinfo, config, today, auditType, caller);
                upOpDt.Update(ref chkResult);
                if (chkResult.HasError)//检查成功再进行下一步处理
                {
                    return chkResult;
                }

                //生成特殊备注
                CreateSpecialComment csc = new CreateSpecialComment(patinfo, resulto, auditType, config);
                csc.Update(ref chkResult);

                //更新病人组合表
                UpdatePatientsMi update_mi = new UpdatePatientsMi(patinfo, patients_mi, config, auditType);
                update_mi.Update(ref chkResult);

                //更新病人其他信息
                UpdatePatientsInfo upi = new UpdatePatientsInfo(patinfo, patients_mi, resulto, auditType, config);
                upi.Update(ref chkResult);

                //检查审核/报告人与状态是否正确
                CheckerAuditAndReportInfo checkerAuditAndReportInfo = new CheckerAuditAndReportInfo(patinfo, auditType, config, caller);
                checkerAuditAndReportInfo.Check(ref chkResult);



                if (chkResult.HasError)//检查成功再进行下一步处理
                {
                    return chkResult;
                }

                string caSignatureMode = CacheSysConfig.Current.GetSystemConfig("CASignMode");
                if (auditType == EnumOperationCode.Report && caSignatureMode != "无")
                {
                    var checkCASign = new dcl.svr.ca.AuditCheckCASign(patinfo, resulto);

                    string caSignContent = checkCASign.CASignContentSplice();

                    chkResult.OperationResultData = caSignContent;
                }

                #region 旧CA
//                if (auditType == EnumOperationCode.Report)
//                {
//                    // 添加分支 GDNETCA
//                    if ((!string.IsNullOrEmpty(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode")) &&
//                        String.Equals(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode"), "GDNETCA", StringComparison.CurrentCultureIgnoreCase)
//                        ||
//                        (!string.IsNullOrEmpty(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode"))
//                        && String.Equals(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode"), "GDNETCA", StringComparison.CurrentCultureIgnoreCase))))
//                    {
//                        SqlHelper helper = new SqlHelper();
//                        if (chkResult.DclReturnResult == null)
//                        {
//                            chkResult.DclReturnResult = new Dictionary<string, object>();
//                        }
//                        EntityDicPidReportMainExt ext = new EntityDicPidReportMainExt();

//                        // chkResult.DclReturnResult.Add("patients_ext", ext);d
//                    }

//                    //拼接CA电子签名所需的报告单内容
//                    else if ((!string.IsNullOrEmpty(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode")) && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode") != "无") || (!string.IsNullOrEmpty(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode")) && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode") == "BJCATimestamp"))
//                    {
//                        Lib.biz.CASign.ICheckCASign CheckCASign = new Lib.biz.CASign.AuditCheckCASign(Lib.EntityCore.EntityConverter.EntityToDataTable<EntityPidReportMain>(patinfo), Lib.EntityCore.EntityConverter.EntityListToDataTable<EntityObrResult>(resulto));
//                        DataTable dtbCASignContent = CheckCASign.CASignContentSplice();
//                        //在原文追加upid
//                        if (dtbCASignContent != null && dtbCASignContent.Rows.Count > 0 && dtbCASignContent.Columns.Contains("SourceContent"))
//                        {
//                            string temp_scontent = dtbCASignContent.Rows[0]["SourceContent"].ToString();
//                            if (!string.IsNullOrEmpty(temp_scontent))
//                            {
//                                dtbCASignContent.Rows[0]["SourceContent"] = temp_scontent + patinfo.PidUniqueId + ",";
//                            }
//                        }

//                        //电子签章模式
//                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CASignatureMode") != "无")
//                        {
//                            if (chkResult.ReturnResult != null && chkResult.ReturnResult.Tables.Contains(dtbCASignContent.TableName))
//                            {
//                                chkResult.ReturnResult.Tables[dtbCASignContent.TableName].Merge(dtbCASignContent);
//                            }
//                            else
//                            {
//                                if (chkResult.ReturnResult == null)
//                                {
//                                    chkResult.ReturnResult = new DataSet();
//                                }
//                                chkResult.ReturnResult.Tables.Add(dtbCASignContent);
//                            }
//                        }


//                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode") == "BJCATimestamp")
//                        {
//                            Lib.biz.CASign.ICASignature Timestamp = Lib.biz.CASign.CASignatureFactory.CreateCASignature

//(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CATimestampMode"));
//                            DateTime dtTimestamp = DateTime.Now;
//                            DataTable dtbCATimestamp = dtbCASignContent.Copy();
//                            if (Timestamp.Sign(ref dtbCATimestamp, out dtTimestamp))
//                            {
//                                patinfo.RepReportDate = dtTimestamp;
//                                //2014-6-12用新方法，用新字段保存时间戳内容
//                                if (true)
//                                {
//                                    DataTable dttempCopy = dtbCATimestamp.DefaultView.ToTable();

//                                    //把SourceContent改为SourceTimestamp保存时间戳原文
//                                    if (dttempCopy != null && dttempCopy.Columns.Contains("SourceContent"))
//                                    {
//                                        dttempCopy.Columns["SourceContent"].ColumnName = "SourceTimestamp";
//                                    }
//                                    //把SignContent改为SignTimestamp保存时间戳密文
//                                    if (dttempCopy != null && dttempCopy.Columns.Contains("SignContent"))
//                                    {
//                                        dttempCopy.Columns["SignContent"].ColumnName = "SignTimestamp";
//                                    }

//                                    new CASignature().InsertReportCASignature(dttempCopy);
//                                }
//                                else
//                                {
//                                    new CASignature().InsertReportCASignature(dtbCATimestamp);
//                                }
//                            }
//                        }

//                    }
//                }
                #endregion

                try
                {
                    //更新数据到数据库
                    new DataUpdater(patinfo, patients_mi, resulto, auditType, config).Update(ref chkResult);
                    if (chkResult.HasError)
                    {
                        return chkResult;
                    }
                }
                catch (Exception ex)
                {
                    dcl.root.logon.Logger.WriteException(this.GetType().Name, "AduitReport", ex.ToString());

                    chkResult.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
                    return chkResult;
                }

                //更新条码信息
                UpdateBarcodeLog upBarcode = new UpdateBarcodeLog(patinfo, patients_mi, config, today, auditType, caller);
                upBarcode.Update(ref chkResult);

                //系统配置：二审时更新没有组合id的项目
                if (CacheSysConfig.Current.GetSystemConfig("SecondAudit_UpdateResNotComItem") == "是")
                {
                    //二审时更新所有没有组合id的项目
                    UpdateResNotComItemData upNotComItem = new UpdateResNotComItemData(patinfo, config, auditType);
                    upNotComItem.Update(ref chkResult);
                }

                //系统配置：二审时是否记录多参考值
                if (CacheSysConfig.Current.GetSystemConfig("Audit_Second_IsNoteRefExp") == "是")
                {
                    //写入分期多参考值
                    UpdateResultoRefExp upRefExp = new UpdateResultoRefExp(patinfo, resulto, auditType, config);
                    upRefExp.Update(ref chkResult);
                }

                if (caller.UnSendCriticalMessagePatIDs != null && caller.UnSendCriticalMessagePatIDs.Count > 0
                    && auditType == EnumOperationCode.Report
                    && caller.UnSendCriticalMessagePatIDs.Contains(patinfo.RepId))
                {
                    //不发送危急值消息
                }
                else
                {
                    //发送危急值消息
                    SendCriticalMessage scm = new SendCriticalMessage(patinfo, resulto, auditType, config);
                    scm.Update(ref chkResult);
                }

                //二审是否可以发送急查消息
                if (CacheSysConfig.Current.GetSystemConfig("Audit_Second_SendUrgentMessage") != "否")
                {
                    //发送急查消息
                    SendUrgentMessage sum = new SendUrgentMessage(patinfo, auditType, config);
                    sum.Update(ref chkResult);
                }

                //取消召回消息
                ClearCallBackMessage ccbm = new ClearCallBackMessage(patinfo, auditType, config);
                ccbm.Update(ref chkResult);
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "AduitReport", ex.ToString());
                chkResult.AddMessage(EnumOperationErrorCode.Exception, EnumOperationErrorLevel.Error);
            }

            return chkResult;
        }


        #region 审核/报告外部调用接口
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList Audit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return BatchAduitReport(listPatientsID.ToArray(), EnumOperationCode.Audit, caller);
        }

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList UndoAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return BatchAduitReport(listPatientsID.ToArray(), EnumOperationCode.UndoAudit, caller);
        }

        /// <summary>
        /// 预报告
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList PreAuditBatch(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return BatchAduitReport(listPatientsID.ToArray(), EnumOperationCode.PreAudit, caller);
        }

        /// <summary>
        /// 取消预报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList UndoPreAuditBatch(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return BatchAduitReport(listPatientsID.ToArray(), EnumOperationCode.UndoPreAudit, caller);
        }

        /// <summary>
        /// 报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList Report(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return BatchAduitReport(listPatientsID.ToArray(), EnumOperationCode.Report, caller);
        }


        /// <summary>
        /// 预览等特殊操作更新检验结果的参考值等
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <returns></returns>
        public bool UpdateObrResult(EntityPidReportMain patinfo)
        {
            if (patinfo == null)
                return true;

            List<EntityObrResult> resulto = GetAuditResulto(patinfo.RepId);
            UpdateResulto urv = new UpdateResulto(patinfo, null, resulto, EnumOperationCode.Unspecified, config);
            urv.Update(patinfo, resulto);
            new DataUpdater(patinfo, null, resulto, EnumOperationCode.Unspecified, config).Update(resulto);

            if (CacheSysConfig.Current.GetSystemConfig("Audit_Second_IsNoteRefExp") == "是")
            {
                UpdateResultoRefExp upRefExp = new UpdateResultoRefExp(patinfo, resulto, EnumOperationCode.Report, config);
                EntityOperationResult no = new EntityOperationResult();
                upRefExp.Update(ref no);
            }
            return true;
        }

        /// <summary>
        /// 取消报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList UndoReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return BatchAduitReport(listPatientsID.ToArray(), EnumOperationCode.UndoReport, caller);
        }

        #endregion

        private EntityOperationResult CreateCheckResult(EntityPidReportMain patinfo)
        {
            EntityOperationResult chkResult = new EntityOperationResult();
            chkResult.Data.Patient.RepId = patinfo.RepId;
            chkResult.Data.Patient.PidName = patinfo.PidName;
            chkResult.Data.Patient.PidSex = patinfo.PidSex;
            chkResult.Data.Patient.RepSid = patinfo.RepSid;
            chkResult.Data.Patient.PidAgeExp = patinfo.PidAgeExp;
            chkResult.Data.Patient.RepPrintFlag = patinfo.RepPrintFlag ?? 0;
            chkResult.Data.Patient.RepSerialNum = patinfo.RepSerialNum;
            chkResult.Data.Patient.RepBarCode = patinfo.RepBarCode;
            chkResult.Data.Patient.PidDeptName = patinfo.PidDeptName;
            chkResult.Data.Patient.PidDiag = patinfo.PidDiag;
            return chkResult;
        }


        /// <summary>
        /// 将报告单电子签章写入数据库
        /// </summary>
        /// <param name="dtCaSigin"></param>
        /// <returns></returns>
        public Boolean InsertReportCASignature(DataTable dtCaSigin)
        {
            return new CASignature().InsertReportCASignature(dtCaSigin);
        }

        /// <summary>
        /// 细菌检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="isAudit"></param>
        /// <returns></returns>
        public EntityOperationResultList BacAuditCheck(IEnumerable<string> listPatientsID, string isAudit)
        {
            return new result.BacAuditBll().BatchAuditCheck(listPatientsID, isAudit);
        }

        /// <summary>
        /// 细菌取消报告前检查 
        /// </summary>
        /// <param name="list_pat_id"></param>
        /// <param name="oper_type"></param>
        /// <returns></returns>
        public EntityOperationResultList BatchUndoReportCheck(IEnumerable<string> listPatientsID, string oper_type)
        {
            return new result.BacAuditBll().BatchUndoReportCheck(listPatientsID, oper_type);
        }

        /// <summary>
        /// 检查该用户是否可以审核
        /// </summary>
        /// <param name="itr_ID"></param>
        /// <param name="auditType"></param>
        /// <param name="loginID"></param>
        /// <returns></returns>
        public bool CheckCurUserCanAudit(string itr_ID, EnumOperationCode auditType, string loginID)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
            {
                return CacheUserInstrmtInfo.Current.CheckUserCanMgrIInstrmt2(loginID, itr_ID);

            }
            return true;
        }
    }
}
