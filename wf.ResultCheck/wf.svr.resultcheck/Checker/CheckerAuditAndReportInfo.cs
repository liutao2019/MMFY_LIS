using dcl.entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.resultcheck
{
    public class CheckerAuditAndReportInfo : AbstractAuditClass, IAuditChecker
    {
        public CheckerAuditAndReportInfo(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config, EntityRemoteCallClientInfo caller)
            : base(pat_info, null, null, auditType, config)
        {
            this.Caller = caller;
        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            switch (this.auditType)
            {
                case EnumOperationCode.Unspecified:
                    break;
                case EnumOperationCode.Audit:
                    if (string.IsNullOrEmpty(this.pat_info.RepAuditUserId))
                    {
                        string msg = string.Format("审核者或报告者为空,审核者{0},操作{1}", Caller.LoginID, this.auditType.ToString());
                        chkResult.AddCustomMessage("123343", "", "审核者或报告者为空", EnumOperationErrorLevel.Error);
                        dcl.root.logon.Logger.WriteBussError(this.GetType().Module.Name, "update前检查审核者信息错误,pat_id=" + pat_info.RepId, msg);

                    }
                    break;
                case EnumOperationCode.Report:
                    if (string.IsNullOrEmpty(this.pat_info.RepAuditUserId)
                        || string.IsNullOrEmpty(this.pat_info.RepReportUserId)
                        )
                    {
                        string msg = string.Format("审核者或报告者为空,审核者{0},操作{1}", Caller.LoginID, this.auditType.ToString());
                        chkResult.AddCustomMessage("123356", "", "审核者或报告者为空", EnumOperationErrorLevel.Error);
                        dcl.root.logon.Logger.WriteBussError(this.GetType().Module.Name, "update前检查审核者信息错误,pat_id=" + pat_info.RepId, msg);

                    }
                    break;
                case EnumOperationCode.UndoAudit:
                    break;
                case EnumOperationCode.UndoReport:
                    break;
                default:
                    break;
            }

            //chkResult.AddCustomMessage("123", "1", "测试2", EnumOperationErrorLevel.Message);
            //   chkResult.AddCustomMessage("1234", "1", "测试3", EnumOperationErrorLevel.Message);
        }

        #endregion
    }

}