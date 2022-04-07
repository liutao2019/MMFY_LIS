using System;
using System.Collections.Generic;

using dcl.entity;
using dcl.svr.msg;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 查询此病人是否有召回信息
    /// </summary>
    public class CheckerCallBackPatient : AbstractAuditClass, IAuditChecker
    {
        public CheckerCallBackPatient(EntityPidReportMain pat_info, EnumOperationCode auditType,AuditConfig auditConfig)
            : base(pat_info, null, null, auditType, auditConfig)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Audit
                ||
                (this.auditType == EnumOperationCode.Report && config.bAllowStepAuditToReport)
                )
            {
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_Allow_CallBackPatient") != "是") return;
                List<EntityDicObrMessageContent> listContent = new List<EntityDicObrMessageContent>();
                EntityDicObrMessageContent content = new EntityDicObrMessageContent();
                content.ObrValueA = this.pat_info.RepId;
                content.ObrType = (int)EnumMessageType.CALL_BACK_PATIENT;
                content.DelFlag = false;
                listContent=new ObrMessageContentBIZ().GetMessageByCondition(content);
                if (listContent.Count > 0)
                {
                    object hasCallBack = listContent[0].ObrId;
                    if (hasCallBack != null && hasCallBack != DBNull.Value)
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.HasCallBackMessage, EnumOperationErrorLevel.Message);
                    }
                }
            }
        }

        #endregion
    }
}
