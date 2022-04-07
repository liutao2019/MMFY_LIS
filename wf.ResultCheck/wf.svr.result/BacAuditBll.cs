using System;
using System.Collections.Generic;
using dcl.svr.resultcheck;
using dcl.svr.cache;
using dcl.entity;

namespace dcl.svr.result
{
    public class BacAuditBll
    {
        public EntityOperationResult SingleAuditCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;


            string Lab_ThreeAuditItrIDs = CacheSysConfig.Current.GetSystemConfig("Lab_ThreeAuditItrIDs");

            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);
            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;
                switch (strFlag)
                {
                    case "0":

                        if (strpat_pre_flag == "0" && !string.IsNullOrEmpty(Lab_ThreeAuditItrIDs)
                            && Lab_ThreeAuditItrIDs.Contains(pat_itr_id))
                        {
                            result.AddMessage(EnumOperationErrorCode.NotPreAudit, EnumOperationErrorLevel.Error);
                            break;
                        }

                        var listBact = new ObrResultBactBIZ().GetBactResultById(pat_id);
                        var listDesc = new ObrResultDescBIZ().GetDescResultById(pat_id);

                        if ((listBact == null || listBact.Count == 0) && (listDesc == null || listDesc.Count == 0))
                        {
                            result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                        }

                        string strMes = string.Empty;
                        foreach (var item in listDesc)
                        {
                            if (item.PositiveFlag.ToString() == "1")
                            {
                                strMes += "," + item.ObrValue;
                            }
                        }

                        if (strMes.Length > 0)
                        {
                            strMes = strMes.Remove(0, 1);
                            result.AddMessage(EnumOperationErrorCode.PositiveResult, strMes, EnumOperationErrorLevel.Warn);
                        }

                        break;
                    case "1":
                        result.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                        break;
                    case "2":
                        result.AddMessage(EnumOperationErrorCode.Reported, EnumOperationErrorLevel.Error);
                        break;
                    case "4":
                        result.AddMessage(EnumOperationErrorCode.Printed, EnumOperationErrorLevel.Error);
                        break;
                    default:
                        break;
                }
            }
            //判断当检查目的为空时的错误等级
            EnumOperationErrorLevel strNoInstention = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CanNotAuditWhileNoCheckIntention"));
            EnumOperationErrorLevel Audit_PatSpcialTimeCheck = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_PatSpcialTimeCheck"));

            string PidPurpId = drPat.PidPurpId;
            result.Data.Patient.PidName = drPat.PidName;
            result.Data.Patient.RepSid = drPat.RepSid;

            if (string.IsNullOrEmpty(PidPurpId))
            {
                result.AddCustomMessage("54t4", "检查目的", string.Format("病人姓名：{1}，样本号：{0},没有检查目的", drPat.RepSid, drPat.PidName), strNoInstention);
            }

            if (Audit_PatSpcialTimeCheck != EnumOperationErrorLevel.None)
            {

                if (drPat.SampSendDate == null)
                {
                    result.AddCustomMessage("223454676", "", "检验时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampCollectionDate == null)
                {
                    result.AddCustomMessage("233454676", "", "标本采集时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampApplyDate == null)
                {
                    result.AddCustomMessage("243454676", "", "标本接收时间为空！", Audit_PatSpcialTimeCheck);
                }

            }
            return result;
        }

        public EntityOperationResult SinglePreAuditCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;

            string Lab_ThreeAuditItrIDs = CacheSysConfig.Current.GetSystemConfig("Lab_ThreeAuditItrIDs");

            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);

            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;
                switch (strFlag)
                {
                    case "0":

                        if (strpat_pre_flag == "1")
                        {
                            result.AddMessage(EnumOperationErrorCode.PreAudited, EnumOperationErrorLevel.Error);
                            break;
                        }
                        var listBact = new ObrResultBactBIZ().GetBactResultById(pat_id);
                        var listDesc = new ObrResultDescBIZ().GetDescResultById(pat_id);
                        if ((listBact == null || listBact.Count == 0) && (listDesc == null || listDesc.Count == 0))
                        {
                            result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                        }
                        string strMes = string.Empty;
                        foreach (var item in listDesc)
                        {
                            if (item.PositiveFlag.ToString() == "1")
                            {
                                strMes += "," + item.ObrValue;
                            }
                        }

                        if (strMes.Length > 0)
                        {
                            strMes = strMes.Remove(0, 1);
                            result.AddMessage(EnumOperationErrorCode.PositiveResult, strMes, EnumOperationErrorLevel.Warn);
                        }

                        break;
                    case "1":
                        result.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                        break;
                    case "2":
                        result.AddMessage(EnumOperationErrorCode.Reported, EnumOperationErrorLevel.Error);
                        break;
                    case "4":
                        result.AddMessage(EnumOperationErrorCode.Printed, EnumOperationErrorLevel.Error);
                        break;
                    default:
                        break;
                }
            }
            //判断当检查目的为空时的错误等级
            EnumOperationErrorLevel strNoInstention = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CanNotAuditWhileNoCheckIntention"));
            EnumOperationErrorLevel Audit_PatSpcialTimeCheck = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_PatSpcialTimeCheck"));
            string PidPurpId = drPat.PidPurpId;
            result.Data.Patient.PidName = drPat.PidName;
            result.Data.Patient.RepSid = drPat.RepSid;

            if (string.IsNullOrEmpty(PidPurpId))
            {
                result.AddCustomMessage("54t4", "检查目的", string.Format("病人姓名：{1}，样本号：{0},没有检查目的", drPat.RepSid, drPat.PidName), strNoInstention);
            }

            if (Audit_PatSpcialTimeCheck != EnumOperationErrorLevel.None)
            {

                if (drPat.SampSendDate == null)
                {
                    result.AddCustomMessage("223454676", "", "检验时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampCollectionDate == null)
                {
                    result.AddCustomMessage("233454676", "", "标本采集时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampApplyDate == null)
                {
                    result.AddCustomMessage("243454676", "", "标本接收时间为空！", Audit_PatSpcialTimeCheck);
                }

            }
            return result;
        }

        /// <summary>
        /// 微生物取消报告前检查
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult SingleUndoReportCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;

            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);
            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;

                //取消二审时检查是否已阅读
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("UndoAudit_Second_CheckLookcode") == "是")
                {
                    if (!string.IsNullOrEmpty(drPat.RepReadUserId))
                    {
                        result.AddCustomMessage("", "", string.Format("此报告【{0}】已阅读过不能取消二审", drPat.RepReadUserId), EnumOperationErrorLevel.Error);
                    }
                }

                if (!result.Success)//如果不通过就返回
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 批量取消报告前检查
        /// </summary>
        /// <param name="list_pat_id"></param>
        /// <param name="oper_type">预留-暂未用</param>
        /// <returns></returns>
        public EntityOperationResultList BatchUndoReportCheck(IEnumerable<string> list_pat_id, string oper_type)
        {
            EntityOperationResultList resultsList = new EntityOperationResultList();
            foreach (string pat_id in list_pat_id)
            {
                EntityOperationResult result = new EntityOperationResult();
                result.Data.Patient.RepId = pat_id;
                result = SingleUndoReportCheck(pat_id);
                resultsList.Add(result);
            }
            return resultsList;
        }

        public EntityOperationResultList BatchAuditCheck(IEnumerable<string> list_pat_id, string isAudit)
        {
            EntityOperationResultList resultsList = new EntityOperationResultList();
            foreach (string pat_id in list_pat_id)
            {
                EntityOperationResult result = new EntityOperationResult();
                result.Data.Patient.RepId = pat_id;
                if (isAudit == "1")
                    result = SingleAuditCheck(pat_id);
                else if (isAudit == "2")
                    result = SingleReportCheck(pat_id);
                else if (isAudit == "0")
                    result = SinglePreAuditCheck(pat_id);
                resultsList.Add(result);
            }
            return resultsList;
        }



        public EntityOperationResult SingleReportCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;

            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);

            string strProp = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AllowStepAuditToReport");

            string oneStepAuditTimeZone = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_SetOneStepAuditTimeZone");
            if (!string.IsNullOrEmpty(oneStepAuditTimeZone) && oneStepAuditTimeZone.Split(',').Length == 2)
            {
                DateTime dtStart = Convert.ToDateTime(oneStepAuditTimeZone.Split(',')[0]);
                DateTime dtEnd = Convert.ToDateTime(oneStepAuditTimeZone.Split(',')[1]);

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
                    strProp = "是";
                }
            }

            //判断当检查目的为空时的错误等级
            EnumOperationErrorLevel strNoInstention = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CanNotAuditWhileNoCheckIntention"));
            EnumOperationErrorLevel Audit_PatSpcialTimeCheck = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_PatSpcialTimeCheck"));
            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;
                switch (strFlag)
                {
                    case "0":
                        var listBact = new ObrResultBactBIZ().GetBactResultById(pat_id);
                        var listDesc = new ObrResultDescBIZ().GetDescResultById(pat_id);
                        if ((listBact == null || listBact.Count == 0) && (listDesc == null || listDesc.Count == 0))
                        {
                            result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                        }
                        if (strProp == "否")
                            result.AddMessage(EnumOperationErrorCode.NotAudit, EnumOperationErrorLevel.Error);
                        break;
                    case "2":
                        result.AddMessage(EnumOperationErrorCode.Reported, EnumOperationErrorLevel.Error);
                        break;
                    case "4":
                        result.AddMessage(EnumOperationErrorCode.Printed, EnumOperationErrorLevel.Error);
                        break;
                    default:
                        break;
                }
            }

            string PidPurpId = drPat.PidPurpId;
            result.Data.Patient.PidName = drPat.PidName;
            result.Data.Patient.RepSid = drPat.RepSid;

            if (string.IsNullOrEmpty(PidPurpId))
            {
                result.AddCustomMessage("54t4", "检查目的", string.Format("病人姓名：{1}，样本号：{0},没有检查目的", drPat.RepSid, drPat.PidName), strNoInstention);
            }

            if (Audit_PatSpcialTimeCheck != EnumOperationErrorLevel.None)
            {

                if (drPat.SampSendDate == null)
                {
                    result.AddCustomMessage("223454676", "", "检验时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampCollectionDate == null)
                {
                    result.AddCustomMessage("233454676", "", "标本采集时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampApplyDate == null)
                {
                    result.AddCustomMessage("243454676", "", "标本接收时间为空！", Audit_PatSpcialTimeCheck);
                }

            }
            return result;
        }
    }
}
