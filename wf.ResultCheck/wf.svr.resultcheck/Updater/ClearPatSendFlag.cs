using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class ClearPatSendFlag : AbstractAuditClass, IAuditUpdater
    {
        public ClearPatSendFlag(EntityPidReportMain pat_info, EnumOperationCode auditType)
            : base(pat_info, null, null, auditType, null)
        { }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.UndoAudit || this.auditType == EnumOperationCode.UndoReport || this.auditType == EnumOperationCode.UndoPreAudit)
            {
                //Thread t = new Thread(ThreadClearLooker);
                //t.Start();
                pat_info.RepSendFlag = 0;
            }
        }

        #endregion
    }
}
