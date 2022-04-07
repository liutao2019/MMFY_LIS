using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 零项目检查
    /// </summary>
    public class CheckerZeroItem : AbstractAuditClass, IAuditChecker
    {
        public CheckerZeroItem(List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig auditConfig)
            : base(null, null, resulto, auditType, auditConfig)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Audit || this.auditType == EnumOperationCode.PreAudit
                 ||
                 (this.auditType == EnumOperationCode.Report && config.bAllowStepAuditToReport)
                 )
            {
                if (resulto == null || resulto.Count == 0)
                {
                    chkResult.AddMessage(EnumOperationErrorCode.ZeroItem, EnumOperationErrorLevel.Error);
                }
            }
        }

        #endregion
    }
}
