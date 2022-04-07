using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Lib.DAC;


using dcl.svr.cache;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 参考值检查
    /// </summary>
    public class CheckerRef : AbstractAuditClass, IAuditChecker
    {
        EntityRemoteCallClientInfo Caller = new EntityRemoteCallClientInfo();
        public CheckerRef(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode auditType,
                          AuditConfig config, EntityRemoteCallClientInfo p_Caller)
            : base(pat_info, null, resulto, auditType, config)
        {
            Caller = p_Caller;
        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report || this.auditType == EnumOperationCode.PreAudit)
            {

                //无历史数据，结果超出参考区间范围上下限的5%（超出范围可设置）； 
                List<EntityObrResult> tbHistoryRes = null; //历史结果

                double douTemp = 0;

                foreach (EntityObrResult res in resulto)
                {

                    if (!string.IsNullOrEmpty(res.ObrValue)
                        && config.Audit_NegativeResultCheck != EnumOperationErrorLevel.None)
                    {
                        string res_chr = res.ObrValue.TrimStart(new char[] { '=', '>', '<' });
                        res_chr = res_chr.Replace(" ", "");
                        decimal decResChr;

                        if (decimal.TryParse((double.TryParse(res_chr, out douTemp) ? douTemp.ToString() : res_chr), out decResChr))
                        {
                            if (decResChr < 0 && !config.Audit_AllowNegativeResult.Contains(res.ItmId))
                            {
                                chkResult.AddCustomMessage("4324", "不允许负值结果", string.Format("项目{0}不允许负值结果", res.ItmEname), config.Audit_NegativeResultCheck);

                            }

                            if (decResChr == 0 && config.Audit_IncludeItrZeroCheck.Contains(pat_info.RepItrId))
                            {
                                chkResult.AddCustomMessage("4324", "不允许为零结果", string.Format("项目{0}不允许为零结果", res.ItmEname), config.Audit_ItrZeroCheck);

                            }
                        }
                    }


                    EntityDicItmRefdetail refValue;
                    EntityDicItemSample referenceSam;
                    EntityDicItmItem refItem;
                    EnumResRefFlag refResult = EnumResRefFlag.Normal;
                    EnumResRefFlag lineResult = EnumResRefFlag.Normal;
                    EnumResRefFlag reportResult = EnumResRefFlag.Normal;

                    string reschr = res.ObrValue;

                    if (config.Lab_EiasaCheckItmResUseOdValue && !string.IsNullOrEmpty(res.ObrValue2))
                    {
                        EntityDicInstrument itrentity = DictInstrmtCache.Current.GetInstructmentByID(pat_info.RepItrId);

                        if (itrentity != null && itrentity.ItrReportType == "2")
                            reschr = res.ObrValue2;
                    }
                    EnumResRefFlag res_flag = RefGetter.GetRefResult(res.ItmId, reschr, pat_info.PidSex, pat_info.PidSamId, Convert.ToInt32(pat_info.PidAge.Value), pat_info.SampRemark, pat_info.RepItrId, pat_info.PidDeptId,
                        out refValue, out referenceSam, out refItem, ref refResult, 
                        ref lineResult, ref reportResult, pat_info.PidDiag);

                    //将项目字典中的 【阳性结果】标题改为【结果提示】，内容可以设置多个字符串，现调整参数格式，结果关键字 + | + 类型，多个以逗号(,)连接,
                    //如：结果字符1|阳性,结果字符2|阳性,结果字符3|提示
                    //为了不影响其他医院已设置的参数，无  |  时默认为阳性结果提示。
                    //格式  结果1|提示1,结果2|提示2,结果3,结果4
                    //有|符号时,满足 结果1的就 提示1,满足 结果2的就 提示2。
                    if (referenceSam != null && referenceSam.ListPositiveResult != null && referenceSam.ListPositiveResult.Count > 0
                        && !string.IsNullOrEmpty(reschr))
                    {
                        foreach (string srPositiveResult in referenceSam.ListPositiveResult)
                        {
                            if (!string.IsNullOrEmpty(srPositiveResult)
                                && srPositiveResult.Contains("|")
                                && srPositiveResult.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Length >= 2
                                && reschr == srPositiveResult.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[0])
                            {
                                chkResult.AddCustomMessage("", "", string.Format("项目{0}结果({1})", res.ItmEname, srPositiveResult.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)[1]), EnumOperationErrorLevel.Warn);
                                break;
                            }
                        }
                    }

                    if (res_flag == EnumResRefFlag.Unknow)
                    {
                        continue;
                    }

                    if (config.Audit_EnableLineRangeWarning && auditType == EnumOperationCode.Report &&
                         (((lineResult & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2)
                        ||
                        ((lineResult & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2)))
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.overLineRange, res.ItmEname, EnumOperationErrorLevel.Warn);
                    }

                    if (config.Audit_EnableReportRangeWarning && auditType == EnumOperationCode.Report &&
                       (((reportResult & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2)
                       ||
                       ((reportResult & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2)))
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.overReportRange, res.ItmEname, EnumOperationErrorLevel.Warn);
                    }


                    if (res_flag == EnumResRefFlag.Positive)//阳性结果提示
                    {
                        if (!config.Audit_AllowPosResult.Contains(res.ItmId))
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.PositiveResult, res.ItmEname
                           , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_PositiveResult : config.Audit_Second_ErrorLevel_PositiveResult
                           );
                        }

                    }

                    if (res_flag == EnumResRefFlag.ExistedNotAllowValues)//出现异常结果提示
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.ExistedNotAllowValue, res.ItmEname
                            , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_ExistNotAllowValues : config.Audit_Second_ErrorLevel_ExistNotAllowValues
                            );
                    }


                    if (
                        ((res_flag & EnumResRefFlag.Greater1) == EnumResRefFlag.Greater1)
                        ||
                        ((res_flag & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
                      )
                    {
                        if (!config.Audit_AllowOverRefResult.Contains(res.ItmId))
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.OverRef, res.ItmEname, auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverRef : config.Audit_Second_ErrorLevel_OverRef);
                        }
                    }

                    if ((res_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue)
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.CustomCriticalValue, res.ItmEname, auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverCritical : config.Audit_Second_ErrorLevel_OverCritical);
                    }

                    if (
                        ((res_flag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2)
                        ||
                        ((res_flag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2)
                      )
                    {

                        //系统配置：有超出危急值的项目时必须复查后才能报告
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_OverCritical_AndCallback") == "是"
                            && pat_info.RepRecheckFlag != 2)
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.OverCritical, "[须复查]" + res.ItmEname + "(" + res.ObrValue + ")", EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            chkResult.AddMessage(EnumOperationErrorCode.OverCritical, res.ItmEname + "(" + res.ObrValue + ")", auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverCritical : config.Audit_Second_ErrorLevel_OverCritical);
                        }
                    }

                    if (
                        ((res_flag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3)
                        ||
                        ((res_flag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3)
                      )
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.OverThreshold, res.ItmEname, auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverThreshold : config.Audit_Second_ErrorLevel_OverThreshold);
                    }
                }
            }
        }

        //public void CheckForBabyFilter(ref EntityOperationResult chkResult)
        //{
        //    if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
        //    {
        //        //无历史数据，结果超出参考区间范围上下限的5%（超出范围可设置）； 

        //        double douTemp = 0;
        //        foreach (EntityObrResult res in resulto)
        //        {


        //            if (!string.IsNullOrEmpty(res.ObrValue)
        //                && config.Audit_NegativeResultCheck != EnumOperationErrorLevel.None)
        //            {
        //                string res_chr = res.ObrValue.TrimStart(new char[] { '=', '>', '<' });
        //                res_chr = res_chr.Replace(" ", "");
        //                decimal decResChr;

        //                if (decimal.TryParse((double.TryParse(res_chr, out douTemp) ? douTemp.ToString() : res_chr), out decResChr))
        //                {
        //                    if (decResChr < 0 && !config.Audit_AllowNegativeResult.Contains(res.ItmId))
        //                    {
        //                        chkResult.AddCustomMessage("4324", "不允许负值结果", string.Format("项目{0}不允许负值结果", res.ItmEname), config.Audit_NegativeResultCheck);

        //                    }

        //                    if (decResChr == 0 && config.Audit_IncludeItrZeroCheck.Contains(pat_info.RepItrId))
        //                    {
        //                        chkResult.AddCustomMessage("4324", "不允许为零结果", string.Format("项目{0}不允许为零结果", res.ItmEname), config.Audit_ItrZeroCheck);

        //                    }
        //                }
        //            }


        //            EntityDicItmRefdetail refValue;
        //            EntityDictItemSam referenceSam;
        //            EntityDicItmItem refItem;
        //            EnumResRefFlag refResult = EnumResRefFlag.Normal;
        //            EnumResRefFlag lineResult = EnumResRefFlag.Normal;
        //            EnumResRefFlag reportResult = EnumResRefFlag.Normal;
        //            EnumResRefFlag res_flag = RefGetter.GetRefResult(res.ItmId, res.ObrValue, pat_info.PidSex, pat_info.PidSamId, Convert.ToInt32(pat_info.PidAge.Value), pat_info.SampRemark, pat_info.RepItrId, pat_info.PidDeptId, 
        //                out refValue, out referenceSam, out refItem,
        //                ref refResult, ref lineResult, ref reportResult);

        //            if (res_flag == EnumResRefFlag.Unknow)
        //            {
        //                continue;
        //            }

        //            if (res_flag == EnumResRefFlag.Positive) //阳性结果提示
        //            {
        //                if (pat_info.RepPrintFlag == 1)
        //                {
        //                    chkResult.AddMessage(EnumOperationErrorCode.PositiveResultForBabyFilter, res.ItmEname,
        //                                     EnumOperationErrorLevel.Warn);
        //                }
        //                if (pat_info.RepPrintFlag == 2)
        //                {
        //                    chkResult.AddMessage(EnumOperationErrorCode.PositiveResultForBabyFilter2, res.ItmEname,
        //                                     EnumOperationErrorLevel.Warn);
        //                }
        //                if (pat_info.RepPrintFlag != 1 && pat_info.RepPrintFlag != 2)
        //                {
        //                    chkResult.AddMessage(EnumOperationErrorCode.PositiveResult, res.ItmEname,
        //                                     EnumOperationErrorLevel.Warn);
        //                }
        //            }
        //            if (res_flag == EnumResRefFlag.ExistedNotAllowValues)//出现异常结果提示
        //            {
        //                chkResult.AddMessage(EnumOperationErrorCode.ExistedNotAllowValue, res.ItmEname
        //                    , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_ExistNotAllowValues : config.Audit_Second_ErrorLevel_ExistNotAllowValues
        //                    );
        //            }

        //            //if (res_flag == EnumResRefFlag.Negative)//出现负值结果
        //            //{
        //            //    chkResult.AddMessage(EnumOperationErrorCode.ExistedNotAllowValue, res.ItmEname, EnumOperationErrorLevel.Warn);
        //            //}
        //            //启用审核规则

        //            if (
        //                ((res_flag & EnumResRefFlag.Greater1) == EnumResRefFlag.Greater1)
        //                ||
        //                ((res_flag & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1)
        //              )
        //            {
        //                if (!config.Audit_AllowOverRefResult.Contains(res.ItmId))
        //                {
        //                    chkResult.AddMessage(EnumOperationErrorCode.OverRef, res.ItmEname, auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverRef : config.Audit_Second_ErrorLevel_OverRef);
        //                }

        //            }

        //            if ((res_flag & EnumResRefFlag.CustomCriticalValue) == EnumResRefFlag.CustomCriticalValue)
        //            {
        //                chkResult.AddMessage(EnumOperationErrorCode.CustomCriticalValue, res.ItmEname, auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverCritical : config.Audit_Second_ErrorLevel_OverCritical);
        //            }

        //            if (
        //                ((res_flag & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2)
        //                ||
        //                ((res_flag & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2)
        //              )
        //            {
        //                //系统配置：有超出危急值的项目时必须复查后才能报告  2014-1-13
        //                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_OverCritical_AndCallback") == "是"
        //                    && pat_info.RepRecheckFlag != 2)
        //                {
        //                    chkResult.AddMessage(EnumOperationErrorCode.OverCritical, "[须复查]" + res.ItmEname + "(" + res.ObrValue + ")", EnumOperationErrorLevel.Error);
        //                }
        //                else
        //                {
        //                    chkResult.AddMessage(EnumOperationErrorCode.OverCritical, res.ItmEname + "(" + res.ObrValue + ")", auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverCritical : config.Audit_Second_ErrorLevel_OverCritical);
        //                }
        //            }

        //            if (
        //                ((res_flag & EnumResRefFlag.Greater3) == EnumResRefFlag.Greater3)
        //                ||
        //                ((res_flag & EnumResRefFlag.Lower3) == EnumResRefFlag.Lower3)
        //              )
        //            {
        //                chkResult.AddMessage(EnumOperationErrorCode.OverThreshold, res.ItmEname, auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_OverThreshold : config.Audit_Second_ErrorLevel_OverThreshold);
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
