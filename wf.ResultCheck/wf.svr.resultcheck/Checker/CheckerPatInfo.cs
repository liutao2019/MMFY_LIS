using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using Lib.DAC;
using System.Data;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class CheckerPatInfo : AbstractAuditClass, IAuditChecker
    {
        EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
        private string patID = string.Empty;
        public CheckerPatInfo(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config, string pat_id, EntityRemoteCallClientInfo caller)
            : base(pat_info, null, null, auditType, config)
        {
            patID = pat_id;
            Caller = caller;
        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit && config.CheckCurrentPatientInfo) //不为取消报告
            {
                string[] patArry = patID.Split(':');

                if (patArry.Length>2&&patArry[2] != pat_info.PidName)
                {
                    chkResult.AddCustomMessage("", "", string.Format("姓名不匹配,当前显示病人姓名:{0}，数据库病人姓名:{1}", patArry[2], pat_info.PidName), EnumOperationErrorLevel.Error);
                    return;
                }
                if (patArry.Length > 2 && patArry[1] != pat_info.PidInNo)
                {
                    chkResult.AddCustomMessage("", "", string.Format("病人ID不匹配,当前显示病人ID:{0}，数据库病人ID:{1}", patArry[1], pat_info.PidInNo), EnumOperationErrorLevel.Error);
                    return;
                }
            }

             if ((auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report) && Caller.UseAuditRule)
             {
                 if (!string.IsNullOrEmpty(pat_info.PidSamId))
                 {
                    EntityDicSample dictSample = dcl.svr.cache.DictSampleCache.Current.DclCache.Find(z => z.SamId == pat_info.PidSamId);

                     if (dictSample != null && dictSample.SamCustomType == "9999")
                     {
                         chkResult.AddCustomMessage("", "", string.Format("该标本已配置为不能自动审核,样本名称:{0}", dictSample.SamName), EnumOperationErrorLevel.Message);
                         return;
                     }
                 }
             }
             //标本状态包含某些字样时  审核提示（中山三院）
            if ((auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report) &&!string.IsNullOrEmpty(config.AuditTips_SampStatus))
            {
                if (!string.IsNullOrEmpty(pat_info.PidRemark))
                {
                    bool boolTips = false;
                    string[] strStatus = config.AuditTips_SampStatus.Split(',');
                    if (strStatus != null && strStatus.Length > 0)
                    {
                        foreach (string status in strStatus)
                        {
                            if (!string.IsNullOrEmpty(status) && pat_info.PidRemark.Contains(status))
                                boolTips = true;
                        }
                    }
                    if (boolTips)
                    {
                        chkResult.AddCustomMessage("", "", string.Format("标本状态:{0}", pat_info.PidRemark), EnumOperationErrorLevel.Message);
                        return;
                    }
                }
            }
        }

        #endregion
    }
}
