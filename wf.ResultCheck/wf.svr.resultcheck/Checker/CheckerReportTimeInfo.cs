using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lib.DAC;
using System.Data;
using dcl.entity;

namespace dcl.svr.resultcheck.Checker
{
    public class CheckerReportTimeInfo : AbstractAuditClass, IAuditChecker
    {
        public CheckerReportTimeInfo(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            try
            {

                if (this.config.Audit_ReportTimeCheck != EnumOperationErrorLevel.None
                    && auditType == EnumOperationCode.Report)
                {
                    //报告时需要检查时间规则：采样时间《收取时间《送达时间《签收时间《检验时间《报告时间《审核时间
                    if (this.pat_info.SampCollectionDate.HasValue
                        && this.pat_info.SampSendDate.HasValue
                        && this.pat_info.SampCollectionDate > this.pat_info.SampSendDate)
                    {
                        chkResult.AddCustomMessage("23454676", "", "采样时间大于送检时间！", this.config.Audit_ReportTimeCheck);

                    }

                    if (this.pat_info.SampSendDate.HasValue
                       && this.pat_info.SampApplyDate.HasValue
                       && this.pat_info.SampSendDate > this.pat_info.SampApplyDate)
                    {
                        chkResult.AddCustomMessage("2345465", "", "送检时间大于接收时间！", this.config.Audit_ReportTimeCheck);

                    }

                    if (this.pat_info.SampApplyDate.HasValue
                       && this.pat_info.SampCheckDate.HasValue
                       && this.pat_info.SampApplyDate > this.pat_info.SampCheckDate)
                    {
                        chkResult.AddCustomMessage("2345469", "", "接收时间大于检验时间！", this.config.Audit_ReportTimeCheck);

                    }

                    if (this.pat_info.SampCheckDate.HasValue
                        && this.pat_info.RepAuditDate.HasValue
                        && this.pat_info.SampCheckDate > this.pat_info.RepAuditDate)
                    {
                        chkResult.AddCustomMessage("2345469", "", "检验时间大于审核时间！", this.config.Audit_ReportTimeCheck);

                    }

                    if (this.pat_info.RepAuditDate.HasValue
                        && this.pat_info.RepReportDate.HasValue
                        && this.pat_info.RepAuditDate > this.pat_info.RepReportDate)
                    {
                        chkResult.AddCustomMessage("234542", "", "审核时间大于报告时间！", this.config.Audit_ReportTimeCheck);

                    }
                }

            }
            catch (Exception ex)
            {

                dcl.root.logon.Logger.WriteException(this.GetType().Name, "CheckerReportTimeInfo", ex.ToString());

            }

        }

        #endregion
    }
}