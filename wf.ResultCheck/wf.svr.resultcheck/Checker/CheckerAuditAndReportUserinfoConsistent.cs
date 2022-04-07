using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class CheckerAuditAndReportUserinfoConsistent : AbstractAuditClass, IAuditChecker
    {
        public CheckerAuditAndReportUserinfoConsistent(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config, EntityRemoteCallClientInfo caller)
            : base(pat_info, null, null, auditType, config)
        {
            Caller = caller;
        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Report && !config.bAllowStepAuditToReport)
            {
                if (pat_info.RepAuditUserId == Caller.LoginID)
                {
                    chkResult.AddMessage(EnumOperationErrorCode.AuditerAndReporterMustDiff, config.ForceAuditerAndReporterMustDiff);
                }
            }
            if (this.auditType == EnumOperationCode.Report  && config.InputerAndReporterMustDiff)
            {
                if (pat_info.RepCheckUserId == Caller.LoginID)
                {
                   // chkResult.AddMessage(EnumOperationErrorCode.InputerAndReporterMustDiff, EnumOperationErrorLevel.Error);
                    chkResult.AddCustomMessage("", "", "审核者不能与检验录入者相同！", EnumOperationErrorLevel.Error);
                    
                }
            }
            //取消二审时，是否只有二审者才可以取消二审
            if (this.auditType == EnumOperationCode.UndoReport && config.bSecondAuditUndoOnlySelf)
            {
                if (pat_info.RepReportUserId != Caller.LoginID)//[取消二审者]是否不等于[二审者]
                {
                    //检查是否有特殊权限,可以跳过限制
                    bool hasFunc = dcl.svr.cache.CacheUserInfo.Current.HasFunctionByLoginID(Caller.LoginID, "PatInput_UndoSecondAuditOnlySelf");

                    if (!hasFunc)
                    {
                        //chkResult.AddMessage(EnumOperationErrorCode.UndoSecondAuditOnlySelf, "非二审者本人不可取消二审", EnumOperationErrorLevel.Error);
                        chkResult.AddMessage(EnumOperationErrorCode.UndoSecondAuditOnlySelf, pat_info.RepReportUserId, EnumOperationErrorLevel.Error);
                    }
                }
            }
        }
        #endregion
    }
}
