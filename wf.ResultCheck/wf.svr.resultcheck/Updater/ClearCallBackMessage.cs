using System;

using System.Threading;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck
{
    public class ClearCallBackMessage : AbstractAuditClass, IAuditUpdater
    {
        public ClearCallBackMessage(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }


        public void Update(ref EntityOperationResult chkResult)
        {
            return;

            if (
                this.auditType == EnumOperationCode.Audit
                ||
                (this.auditType == EnumOperationCode.Report && config.bAllowStepAuditToReport)
                )
            {
                Thread t = new Thread(ThreadClearCallBackMessage);
                t.Start();
            }
        }


        private void ThreadClearCallBackMessage()
        {
            try
            {
                bool result = false;
                IDaoObrMessageContent dao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
                if (dao != null)
                {
                    result = dao.UpdateMessageDelFlag(pat_info.RepId, (int)EnumMessageType.LED1, (int)EnumMessageType.CALL_BACK_PATIENT);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("ThreadClearCallBackMessage", ex);
            }
        }
    }
}
