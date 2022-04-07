using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Data;
using dcl.svr.cache;
using System.Collections;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Lib.LogManager;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 效验规则检查
    /// </summary>
    public class CheckerUndoAuditExpired : AbstractAuditClass, IAuditChecker
    {
        public CheckerUndoAuditExpired(EntityPidReportMain pat_info, EnumOperationCode auditType, AuditConfig config, EntityRemoteCallClientInfo caller)
            : base(pat_info, null, null, auditType, config)
        {
            if (caller != null)
            {
                Caller = caller;
            }
        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {


            if (auditType == EnumOperationCode.UndoReport//如果为取消二审
                && pat_info != null && pat_info.RepReportDate != null
                && config.SecondAuditUndoExpiredDays != 0//设定为零则不限制
                )
            {


                int hours = Convert.ToInt32((DateTime.Now - pat_info.RepReportDate.Value).TotalHours);


                if (
                    hours > config.SecondAuditUndoExpiredDays
                    )//超出取消二审的时间限制(小时)
                {
                    string errMsg = string.Format("二次审核时间为：{0}，\r\n        设定超时时间为：{1}小时，当前报告为：{2}小时，超出：{3}小时"
                                              , pat_info.RepReportDate.Value.ToString("yyyy-MM-dd HH:mm")
                                              , config.SecondAuditUndoExpiredDays
                                              , hours
                                              , hours - config.SecondAuditUndoExpiredDays
                                              );

                    if (config.UndoAudit_Second_ErrorLevel_DateExpired == EnumOperationErrorLevel.Message)
                    {
                        errMsg += "\r\n        由于当前的错误级别为【提示】，将不限制超时取消审核";
                    }
                    else if (config.UndoAudit_Second_ErrorLevel_DateExpired == EnumOperationErrorLevel.Warn)
                    {
                        errMsg += "\r\n        由于当前的错误级别为【警告】，将不限制超时取消审核";
                    }


                    if (Caller != null)
                    {
                        //如果有传入调用者
                        //并且有特殊权限也可以取消2审
                        bool hasFunc = CacheUserInfo.Current.HasFunctionByLoginID(Caller.LoginID, "PatInput_UndoSecondAuditExpired");

                        if (!hasFunc)
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.AuditDayExpire,
                                               errMsg,
                                               config.UndoAudit_Second_ErrorLevel_DateExpired);
                        }
                    }
                    else
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.AuditDayExpire,
                                            errMsg,
                                            config.UndoAudit_Second_ErrorLevel_DateExpired);
                    }
                }
            }
            else
            {
            }
        }

        #endregion
    }
}
