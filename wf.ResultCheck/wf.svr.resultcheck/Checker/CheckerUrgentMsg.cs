using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lib.DAC;
using System.Data;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using dcl.svr.msg;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 缓审时对危急值报告进行提示
    /// </summary>
    public class CheckerUrgentMsg : AbstractAuditClass, IAuditChecker
    {
        public CheckerUrgentMsg(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.UndoReport)
            {
                List<EntityDicObrMessageContent> listContent = new List<EntityDicObrMessageContent>();
                EntityDicObrMessageContent content = new EntityDicObrMessageContent();
                content.ObrValueA = this.pat_info.RepId;
                content.ObrType =1024;

                listContent =new ObrMessageContentBIZ().GetMessageByCondition(content);
                
                if (listContent.Count > 0)
                {
                    chkResult.AddCustomMessage("", "", "危急值报告，请确认！", EnumOperationErrorLevel.Warn);
                }
            }
        }


        #endregion
    }
}
