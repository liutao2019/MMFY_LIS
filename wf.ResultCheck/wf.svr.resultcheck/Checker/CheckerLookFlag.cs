using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using System.Data;
using dcl.svr.interfaces;
using dcl.entity;

namespace dcl.svr.resultcheck.Checker
{
    class CheckerTJFlag : AbstractAuditClass, IAuditChecker
    {
        //检查体检总检标识
        public CheckerTJFlag(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        { }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.UndoReport && pat_info.PidSrcId == "109" && pat_info.PidExamNo != string.Empty)
            {  
                BCHISInterfacesBIZ interfaceBiz = new BCHISInterfacesBIZ();

                DataSet dsResult = interfaceBiz.ExecuteInterfaceBySql("in_interface_type = '体检总检'", pat_info.PidExamNo);
                if (dsResult != null && dsResult.Tables.Count > 0)
                {
                    DataTable dt = dsResult.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string flag = dt.Rows[0][0].ToString();

                        if (flag == "1")
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.CustomMessage, "该标本已总检，要缓审请先通知体检中心", EnumOperationErrorLevel.Error);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
