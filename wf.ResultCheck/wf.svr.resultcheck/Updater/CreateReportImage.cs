using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.resultcheck.Updater
{
    /// <summary>
    /// .ctor报告单生成图像
    /// </summary>
    class CreateReportImage : AbstractAuditClass, IAuditUpdater
    {
        public CreateReportImage(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Report)//只有报告时才操作
            {
            }
        }

        #endregion
    }
}
