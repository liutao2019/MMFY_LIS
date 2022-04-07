using dcl.svr.cache;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class CheckerPatState : AbstractAuditClass, IAuditChecker
    {
        public CheckerPatState(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, null, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            //病人资料状态为未审核
            if (pat_info.RepStatus == (int)EnumPatFlag.Natural)
            {
                if (!string.IsNullOrEmpty(config.Lab_ThreeAuditItrIDs) && config.Lab_ThreeAuditItrIDs.Contains(pat_info.RepItrId))
                {
                    if (pat_info.RepInitialFlag == 0)
                    {
                        if (auditType == EnumOperationCode.UndoPreAudit || auditType == EnumOperationCode.Audit)
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.NotPreAudit, EnumOperationErrorLevel.Error);
                        }
                    }

                    if (pat_info.RepInitialFlag == 1)
                    {
                        if (auditType == EnumOperationCode.PreAudit)
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.PreAudited, EnumOperationErrorLevel.Error);
                        }
                    }

                }
                if (auditType == EnumOperationCode.UndoAudit)//取消审核
                {
                    chkResult.AddMessage(EnumOperationErrorCode.NotAudit, EnumOperationErrorLevel.Error);
                }
                else if (auditType == EnumOperationCode.Report)//报告
                {
                    if (!config.bAllowStepAuditToReport)
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.NotAudit, EnumOperationErrorLevel.Error);
                    }
                }
                else if (auditType == EnumOperationCode.UndoReport)//取消报告
                {
                    chkResult.AddMessage(EnumOperationErrorCode.NotAudit, string.Empty, EnumOperationErrorLevel.Error);
                }
            }
            else if (pat_info.RepStatus == (int)EnumPatFlag.Audited)//已审核
            {
                if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.PreAudit || auditType == EnumOperationCode.UndoPreAudit)//审核
                {
                    chkResult.AddMessage(EnumOperationErrorCode.Audited, string.Empty, EnumOperationErrorLevel.Error);
                }
                else if (auditType == EnumOperationCode.Report)
                {

                }
                else if (auditType == EnumOperationCode.UndoReport)//取消报告
                {
                    chkResult.AddMessage(EnumOperationErrorCode.NotReport, string.Empty, EnumOperationErrorLevel.Error);
                }
                else if (auditType == EnumOperationCode.MidReport)//中期报告
                {
                    chkResult.AddCustomMessage("", "", "此报告已一审不能再发中期报告", EnumOperationErrorLevel.Error);
                }
            }
            else if (pat_info.RepStatus == (int)EnumPatFlag.Reported)//已报告
            {
                if (auditType != EnumOperationCode.UndoReport)//不为取消报告
                {
                    chkResult.AddMessage(EnumOperationErrorCode.Reported, string.Empty, EnumOperationErrorLevel.Error);
                }
                else if (auditType == EnumOperationCode.UndoReport)//取消报告时,判断是否已经阅读过
                {
                    if (!string.IsNullOrEmpty(pat_info.RepReadUserId) && config.bUndoAuditSecondCheckLookcode)
                    {
                        chkResult.AddCustomMessage("", "", string.Format("此报告【{1}({0})】已阅读过", pat_info.RepReadUserId, CacheUserInfo.Current.GetUserNameByLoginID(pat_info.RepReadUserId)), EnumOperationErrorLevel.Warn);
                    }
                }
                else if (auditType == EnumOperationCode.MidReport)//中期报告
                {
                    chkResult.AddCustomMessage("", "", "此报告已二审不能再发中期报告", EnumOperationErrorLevel.Error);
                }
            }
            else if (pat_info.RepStatus == (int)EnumPatFlag.Printed)//已打印
            {
                if (auditType != EnumOperationCode.UndoReport)//不为取消报告
                {
                    chkResult.AddMessage(EnumOperationErrorCode.Reported, string.Empty, EnumOperationErrorLevel.Error);
                }
                else if (auditType == EnumOperationCode.UndoReport)//取消报告时,判断是否已经阅读过
                {
                    if (!string.IsNullOrEmpty(pat_info.RepReadUserId) && config.bUndoAuditSecondCheckLookcode)
                    {
                        chkResult.AddCustomMessage("", "", string.Format("此报告【{1}({0})】已阅读过", pat_info.RepReadUserId, CacheUserInfo.Current.GetUserNameByLoginID(pat_info.RepReadUserId)), EnumOperationErrorLevel.Warn);
                    }
                }
                else if (auditType == EnumOperationCode.MidReport)//中期报告
                {
                    chkResult.AddCustomMessage("", "", "此报告已打印不能再发中期报告", EnumOperationErrorLevel.Error);
                }
            }
        }

        #endregion
    }
}
