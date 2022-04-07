using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Lib.DAC;

using dcl.entity;

namespace dcl.svr.resultcheck.Checker
{
    public class CheckerSpcialTimeInfo : AbstractAuditClass, IAuditChecker
    {
        public CheckerSpcialTimeInfo(EntityPidReportMain pat_info,  EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {


            try
            {
                if (this.config.Audit_PatSpcialTimeCheck != EnumOperationErrorLevel.None
                    && (auditType == EnumOperationCode.Report || auditType == EnumOperationCode.Audit))
                {

                    if (!this.pat_info.SampSendDate.HasValue)
                    {
                        chkResult.AddCustomMessage("223454676", "", "检验时间为空！", this.config.Audit_PatSpcialTimeCheck);
                    }
                    if (!this.pat_info.SampCollectionDate.HasValue)
                    {
                        chkResult.AddCustomMessage("233454676", "", "标本采集时间为空！", this.config.Audit_PatSpcialTimeCheck);
                    }
                    if (!this.pat_info.SampApplyDate.HasValue)
                    {
                        chkResult.AddCustomMessage("243454676", "", "标本接收时间为空！", this.config.Audit_PatSpcialTimeCheck);
                    }
                }

            }
            catch (Exception ex)
            {

                dcl.root.logon.Logger.WriteException(this.GetType().Name, "Audit_PatSpcialTimeCheck", ex.ToString());

            }

        }

        #endregion
    }
}
