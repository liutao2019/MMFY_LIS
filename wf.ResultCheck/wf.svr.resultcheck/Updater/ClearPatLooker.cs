using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using System.Threading;
using dcl.root.logon;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    /// <summary>
    /// 一审时清除查看者
    /// </summary>
    class ClearPatLooker : AbstractAuditClass, IAuditUpdater
    {
        public ClearPatLooker(EntityPidReportMain pat_info, EnumOperationCode auditType)
            : base(pat_info, null, null, auditType, null)
        { }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (
                this.auditType == EnumOperationCode.Audit//一审后设置为未查看报告单
                )
            {
                pat_info.RepReadUserId = null;
            }
        }

        #endregion
    }
}
