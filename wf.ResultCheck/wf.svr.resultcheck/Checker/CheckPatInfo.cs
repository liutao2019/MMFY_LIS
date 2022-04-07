using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class CheckPatInfo : AbstractAuditClass, IAuditChecker
    {
        public CheckPatInfo(EntityPidReportMain patinfo, EnumOperationCode auditType, AuditConfig config)
            : base(patinfo, null, null, auditType, config)
        {

        }

        #region IAuditChecker 成员

        /// <summary>
        /// 判断是否有标本类别
        /// </summary>
        /// <param name="chkResult"></param>
        public void Check(ref EntityOperationResult chkResult)
        {
            
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
            {
                if (string.IsNullOrEmpty(this.pat_info.PidSamId))
                {
                    chkResult.AddCustomMessage("4654897", "没有标本类别", string.Format("样本号：[{0}]缺少标本类别",this.pat_info.RepSid), EnumOperationErrorLevel.Error);

                }
            }

            if (auditType == EnumOperationCode.Audit || (config.bAllowStepAuditToReport && (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)))
            {
                if (string.IsNullOrEmpty(this.pat_info.RepCheckUserId))
                {
                    EnumOperationErrorLevel lvError = EnumOperationErrorLevel.None;

                    //1审错误等级:未填录入者
                    string Audit_First_ErrorLevel_PatIcode = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_First_ErrorLevel_PatIcode");

                    if (!string.IsNullOrEmpty(Audit_First_ErrorLevel_PatIcode))
                    {
                        switch (Audit_First_ErrorLevel_PatIcode)
                        {
                            case "提示": lvError = EnumOperationErrorLevel.Message; break;
                            case "警告": lvError = EnumOperationErrorLevel.Warn; break;
                            case "错误": lvError = EnumOperationErrorLevel.Error; break;
                            default: lvError = EnumOperationErrorLevel.None; break;
                        }
                    }

                    chkResult.AddCustomMessage("", "", string.Format("样本号【{0}】未填录入者", this.pat_info.RepSid), lvError);
                }
            }
        }

        #endregion
    }
}
