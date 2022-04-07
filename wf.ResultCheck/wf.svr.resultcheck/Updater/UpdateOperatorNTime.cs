using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.DAC;


using dcl.root.logon;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 更新审核/报告 状态、时间、操作人
    /// </summary>
    public class UpdateOperatorNTime : AbstractAuditClass, IAuditUpdater
    {
        DateTime today;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pat_info">病人基本信息</param>
        /// <param name="config">审核配置</param>
        /// <param name="auditType">审核类型（审核/报告/取消审核/取消报告）</param>
        /// <param name="caller">操作者基本信息</param>
        public UpdateOperatorNTime(EntityPidReportMain pat_info, AuditConfig config, DateTime dtToday, EnumOperationCode auditType, EntityRemoteCallClientInfo caller)
            : base(pat_info, null, null, auditType, config)
        {
            this.Caller = caller;
            this.today = dtToday;
        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)//, Lib.DAC.SqlHelper transHelper)
        {
            try
            {
                if (auditType == EnumOperationCode.Audit)//操作类型为：一审
                {
                    #region 一审
                    pat_info.RepStatus = 1;
                    pat_info.RepAuditUserId = Caller.LoginID;
                    pat_info.RepAuditDate = this.today;
                    #endregion

                    if (config.Audit_AlloweditPat_i_code && !string.IsNullOrEmpty(Caller.UserID))
                    {
                        pat_info.RepCheckUserId = Caller.UserID;
                    }
                }
                else if (auditType == EnumOperationCode.Report)//二审
                {
                    #region 二审
                    if (this.pat_info.RepStatus == 1)//如果为已一审
                    {
                        //一审时间与二审时间必须相隔多少秒
                        string str_DiffSecond = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_Second_ChkAndReport_DiffDatetime");
                        int int_DiffSecond = 0;//默认为零

                        if (!string.IsNullOrEmpty(str_DiffSecond))
                        {
                            if (int.TryParse(str_DiffSecond, out int_DiffSecond))
                            {
                                if (int_DiffSecond < 0)
                                {
                                    int_DiffSecond = 0;//如果小于等于0,则默认为0
                                }
                            }
                        }

                        if (int_DiffSecond > 0)
                        {
                            //如果一审时间与二审时间小于相隔时间,则不能二审
                            if (pat_info.RepAuditDate > (this.today.AddSeconds(-int_DiffSecond)))
                            {
                                chkResult.AddCustomMessage("", "", "一审时间与二审时间必须相隔" + int_DiffSecond.ToString() + "秒", EnumOperationErrorLevel.Error);
                                return;
                            }
                        }

                        pat_info.RepStatus = 2;
                        pat_info.RepReportUserId = Caller.LoginID;
                        pat_info.RepReportDate = this.today;



                    }
                    else if (this.pat_info.RepStatus == 0 || this.pat_info.RepStatus == null)//如果还没审核(直接二审)
                    {
                        if (!config.bAllowStepAuditToReport)
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.NotAudit, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            //一审二审为当前操作人
                            //一审为当前操作人/二审为当前操作人所属组别随机一个
                            //一审为当前操作人所属组别随机一个/二审为当前操作人
                            //一审为当前操作人/二审为仪器默认审核人
                            //一审为仪器默认审核人/二审为当前操作人

                            //判断当前审核人是否为空
                            if (string.IsNullOrEmpty(Caller.LoginID) || Caller.LoginID.Trim() == string.Empty)
                            {
                                throw new Exception("操作人为空");
                            }

                            //默认一二审者为当前操作人
                            string firstAuditer = pat_info.RepCheckUserId;
                            string secondAuditer = Caller.LoginID;
                            if (!string.IsNullOrEmpty(Caller.UserID))
                            {
                                firstAuditer = Caller.UserID;
                            }
                            //根据配置生成操作人
                            string checkerConfig = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_SpetTo2ndAdutiUserMode");

                            if (checkerConfig == "一审二审为当前操作人")//跳过
                            {
                            }
                            else if (checkerConfig == "一审为当前操作人/二审为当前操作人所属组别随机一个")
                            {
                                secondAuditer = dcl.svr.cache.CacheUserInfo.Current.GetRandomUserCodeWithSamGroup(firstAuditer);
                                if (secondAuditer == null)//找不到同组别用户
                                {
                                    secondAuditer = firstAuditer;//一审二审为当前操作人
                                }
                            }
                            else if (checkerConfig == "一审为当前操作人所属组别随机一个/二审为当前操作人")
                            {
                                firstAuditer = dcl.svr.cache.CacheUserInfo.Current.GetRandomUserCodeWithSamGroup(secondAuditer);
                                if (firstAuditer == null)//找不到同组别用户
                                {
                                    firstAuditer = secondAuditer;//一审二审为当前操作人
                                }
                            }
                            else if (checkerConfig == "一审为当前操作人/二审为仪器默认审核人")
                            {
                                secondAuditer = dcl.svr.cache.DictInstrmtCache.Current.GetItrDefaultAuditerCode(pat_info.RepItrId);
                                if (secondAuditer == null)
                                {
                                    secondAuditer = firstAuditer;
                                }
                            }
                            else if (checkerConfig == "一审为仪器默认审核人/二审为当前操作人")
                            {
                                firstAuditer = dcl.svr.cache.DictInstrmtCache.Current.GetItrDefaultAuditerCode(pat_info.RepItrId);
                                if (firstAuditer == null)
                                {
                                    firstAuditer = secondAuditer;
                                }
                            }

                            pat_info.RepStatus = 2;
                            pat_info.RepAuditUserId = firstAuditer;
                            pat_info.RepAuditDate = this.today;
                            pat_info.RepReportUserId = secondAuditer;
                            pat_info.RepReportDate = this.today;

                            //跟踪审核后，审核状态为2，但审核者与报告者为空的错误
                            if (string.IsNullOrEmpty(pat_info.RepReportUserId) || string.IsNullOrEmpty(pat_info.RepAuditUserId))
                            {
                                dcl.root.logon.Logger.WriteBussError(this.GetType().Module.Name, "二审", string.Format("审核者或报告者为空,审核者{0}", Caller.LoginID));
                                //throw new Exception("审核信息错误！请从新审核。");
                                chkResult.AddCustomMessage("87654", "审核信息错误", "审核信息错误！请从新审核。", EnumOperationErrorLevel.Error);

                            }

                            if (string.IsNullOrEmpty(pat_info.RepReportUserId) || string.IsNullOrEmpty(pat_info.RepAuditUserId))
                            {
                                dcl.root.logon.Logger.WriteException(this.GetType().Name, "Update", "操作人为空");
                            }
                        }
                    }
                    #endregion

                    if (config.Audit_AlloweditPat_i_code && !string.IsNullOrEmpty(Caller.UserID))
                    {
                        //pat_info.RepCheckUserId = Caller.UserID;
                    }
                }
                else if (auditType == EnumOperationCode.UndoAudit)//取消一审
                {
                    #region 取消一审
                    pat_info.RepStatus = 0;
                    pat_info.RepAuditUserId = null;
                    pat_info.RepAuditDate = null;
                    #endregion

                    if (pat_info.RepAuditWay == 3 )
                    {
                        pat_info.RepAuditWay = 0;
                    }
                }
                else if (auditType == EnumOperationCode.UndoReport)//取消二审
                {
                    #region 取消二审
                    if (config.OneStepCancelReport)//如果一步取消所有审核
                    {
                        pat_info.RepStatus = 0;
                        pat_info.RepAuditUserId = null;
                        pat_info.RepAuditDate = null;
                        pat_info.RepReportUserId = null;
                        pat_info.RepReportDate = null;
                        pat_info.RepReadUserId = null;
                        pat_info.RepReadDate = null;
                    }
                    else
                    {
                        pat_info.RepStatus = 1;
                        pat_info.RepReportUserId = null;
                        pat_info.RepReportDate = null;
                        pat_info.RepReadUserId = null;
                        pat_info.RepReadDate = null;
                    }
                    #endregion

                    if (pat_info.RepAuditWay == 3 )
                    {
                        pat_info.RepAuditWay = 0;
                    }
                }
                else if (auditType == EnumOperationCode.PreAudit)//预审
                {
                    pat_info.RepInitialFlag= 1;
                    pat_info.RepInitialUserId = Caller.LoginID;
                    pat_info.RepInitialDate = this.today;
                }
                else if (auditType == EnumOperationCode.UndoPreAudit)//取消预审
                {
                    pat_info.RepInitialFlag = 0;
                    pat_info.RepInitialUserId = null;
                    pat_info.RepInitialDate = null;
                }
                else
                {
                    chkResult.AddCustomMessage("err1", "", "系统异常，未知的操作", EnumOperationErrorLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "UpdateOperatorNTime", ex.ToString());
                chkResult.AddMessage(EnumOperationErrorCode.Exception, ex.StackTrace, EnumOperationErrorLevel.Error);
            }
        }
        #endregion
    }
}
