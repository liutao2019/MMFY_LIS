using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class CheckerRecheckInfo : AbstractAuditClass, IAuditChecker
    {
        public CheckerRecheckInfo(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
            {
                if (pat_info != null && pat_info.RepRecheckFlag == 1)
                {
                    chkResult.AddMessage(EnumOperationErrorCode.PatinetRecheck, EnumOperationErrorLevel.Warn);
                }
            }
        }

        #endregion
    }
}
