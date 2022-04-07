using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using dcl.root.logon;

using dcl.svr.cache;
using dcl.entity;
using dcl.common;

namespace dcl.svr.resultcheck.Updater
{
    /// <summary>
    /// 更新结果表信息
    /// </summary>
   public class UpdateResulto : AbstractAuditClass, IAuditUpdater
    {
        public UpdateResulto(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {
        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Audit || this.auditType == EnumOperationCode.PreAudit
                ||
                (this.auditType == EnumOperationCode.Report && (config.bAllowStepAuditToReport || (config.Audit_SendTjCriticalToMsg && pat_info.PidSrcId == "109"))
                ))
            {
                try
                {
                    //外部报告单审核时无需计算参考范围
                    if (pat_info.PidOrgId == "outreport")
                    {
                        return;
                    }
                    EntityDicItmRefdetail refValue = null;
                    EntityDicItemSample referenceSam = null;
                    EntityDicItmItem refItem = null;

                    foreach (EntityObrResult res in this.resulto)
                    {
                        if (string.IsNullOrEmpty(res.ObrValue)
                            && string.IsNullOrEmpty(res.ObrValue2))//如果结果为空则删除当前结果
                        {
                            res.NeedDelete = true;
                        }
                        else
                        {
                            //更新没有组合id的结果
                            if (string.IsNullOrEmpty(res.ItmComId))//如果组合id为空
                            {
                                foreach (EntityPidReportDetail patmi in patients_mi)//循环病人组合表
                                {
                                    List<EntityDicCombineDetail> listComMi = DictCombineMiCache2.Current.GetComMi(patmi.ComId);

                                    if (listComMi.Any(i => i.ComItmId == res.ItmId))//在组合包含项目中找到此项目
                                    {
                                        res.ItmComId = patmi.ComId;//更新结果表的项目组合id
                                    }
                                    else
                                    {
                                        //找不到
                                        res.ItmComId = "-1";
                                    }
                                }
                            }

                            //防止pat_age=-1,但pat_age_exp正常，根据pat_age计算参考值无结果的情况
                            int pat_age = pat_info.PidAge == null ? -1 : Convert.ToInt32(pat_info.PidAge.Value);
                            if (pat_age == -1
                                && !string.IsNullOrEmpty(pat_info.PidAgeExp))
                            {
                                pat_age = dcl.common.AgeConverter.AgeValueTextToMinute(pat_info.PidAgeExp);
                                pat_info.PidAge = pat_age;
                            }


                            //更新结果参考值与偏高偏低提示
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

                            EnumResRefFlag res_ref_flag = RefGetter.GetRefResult(res.ItmId, reschr, pat_info.PidSex, pat_info.PidSamId, pat_age, pat_info.SampRemark, pat_info.RepItrId, pat_info.PidDeptId, 
                                out refValue, out referenceSam, out refItem, 
                                ref refResult, ref lineResult, ref reportResult);

                            if (res.RefFlag != ((int)res_ref_flag).ToString())//如果参考值偏高偏低提示与之前的不符
                            {
                                res.RefFlag = ((int)res_ref_flag).ToString();
                            }
                            if (res.ItmId == "2082")
                            {
                                if (Convert.ToDouble(res.ObrValue) < 0.1)
                                {
                                    res.RefFlag = "0";
                                }
                                else if (Convert.ToDouble(res.ObrValue) > 0.1 && Convert.ToDouble(res.ObrValue) < 50)
                                {
                                    res.RefFlag = "8";
                                }
                            }

                            #region 更新参考值
                            if (refValue != null)
                            {
                                res.RefUpperLimit = refValue.ItmUpperLimitValue;
                                res.RefLowerLimit = refValue.ItmLowerLimitValue;
                                res.RefType = ValueConvertHelper.IntParse(refValue.ItmRefFlag, 0);
                            }
                            else if(res.ItmId == "2082")   //茂名妇幼PCT特殊处理
                            {
                                res.RefUpperLimit = null;
                                res.RefLowerLimit = null;
                                res.RefType = 1;
                            }
                            else
                            {
                                res.RefUpperLimit = null;
                                res.RefLowerLimit = null;
                                res.RefType = 0;                              
                            }
                            #endregion

                            #region 更新项目标本中的信息
                            if (referenceSam != null)
                            {
                                res.ObrUnit = referenceSam.ItmUnit;
                                res.ItmPrice = referenceSam.ItmPrice;
                                res.ObrItmMethod = referenceSam.ItmMethod;

                                decimal decResChr;
                                //裁剪小数位 增加判断结果类型不为数值是不裁剪小数位
                                if (referenceSam.ItmResType == "数值"
                                    && decimal.TryParse(res.ObrValue, out decResChr))//为数字型
                                {
                                    if (referenceSam.ItmMaxDigit > 0)
                                    {
                                        string formatString = "{0:f" + referenceSam.ItmMaxDigit.ToString() + "}";

                                        res.ObrValue = string.Format(formatString, decResChr);
                                    }
                                    else if (referenceSam.ItmMaxDigit == -1)//字典设置，当小数点位设为-1则为取整
                                    {
                                        res.ObrValue = string.Format("{0:f0}", decResChr);
                                    }
                                   
                                }
                            }
                            else
                            {
                                if (refItem != null)
                                {
                                    res.ItmPrice = refItem.ItmPrice;
                                }
                                else
                                {
                                    res.ItmPrice = null;
                                }
                                res.ObrUnit = null;
                                res.ObrItmMethod = null;
                            }
                            #endregion

                            #region 更新项目中的信息
                            if (refItem != null)
                            {
                                res.ItmReportCode = refItem.ItmRepCode;
                                res.ItmEname = refItem.ItmEcode;
                            }
                            else
                            {
                                res.ItmReportCode = res.ItmEname;
                            }
                            #endregion

                            #region 转换数字结果
                            //更新res_cast_chr，数字结果

                            res.ObrConvertValue = null;
                            if (!string.IsNullOrEmpty(res.ObrValue))
                            {
                                string res_chr_temp = res.ObrValue; ;
                                if (res.ObrValue.Contains(">")
                                        || res.ObrValue.Contains("<")
                                        || res.ObrValue.Contains("=")
                                        )
                                {
                                    res_chr_temp = res.ObrValue.Replace(">", "").Replace("<", "").Replace("=", "");
                                }
                                double douTemp = 0;
                                decimal decResCastChr;
                                if (decimal.TryParse((double.TryParse(res_chr_temp, out douTemp) ? douTemp.ToString() : res_chr_temp), out decResCastChr))
                                {
                                    res.ObrConvertValue = decResCastChr;
                                }
                                else if (res_chr_temp.Contains('e') || res_chr_temp.Contains('E'))
                                {
                                    double d;
                                    if (double.TryParse(res_chr_temp.Replace(" ", ""), out d))
                                    {
                                        res.ObrConvertValue = Convert.ToDecimal(d);
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "UpdateResulto", ex.ToString());
                    chkResult.AddMessage(EnumOperationErrorCode.Exception, ex.StackTrace, EnumOperationErrorLevel.Error);
                }
            }
        }

        public void Update(EntityPidReportMain pat_info, List<EntityObrResult> obrResultList)
        {
            EntityDicItmRefdetail refValue = null;
            EntityDicItemSample referenceSam = null;
            EntityDicItmItem refItem = null;

            try
            {
                foreach (EntityObrResult res in obrResultList)
                {
                    if (string.IsNullOrEmpty(res.ObrValue)
                        && string.IsNullOrEmpty(res.ObrValue2))//如果结果为空则删除当前结果
                    {
                        res.NeedDelete = true;
                    }
                    else
                    {
                        //防止pat_age=-1,但pat_age_exp正常，根据pat_age计算参考值无结果的情况
                        int pat_age = pat_info.PidAge == null ? -1 : Convert.ToInt32(pat_info.PidAge.Value);
                        if (pat_age == -1
                            && !string.IsNullOrEmpty(pat_info.PidAgeExp))
                        {
                            pat_age = dcl.common.AgeConverter.AgeValueTextToMinute(pat_info.PidAgeExp);
                            pat_info.PidAge = pat_age;
                        }


                        //更新结果参考值与偏高偏低提示
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

                        EnumResRefFlag res_ref_flag = RefGetter.GetRefResult(res.ItmId, reschr, pat_info.PidSex, pat_info.PidSamId, pat_age, pat_info.SampRemark, pat_info.RepItrId, pat_info.PidDeptId, 
                            out refValue, out referenceSam, out refItem, 
                            ref refResult, ref lineResult, ref reportResult);
                        if (res.RefFlag != ((int)res_ref_flag).ToString())//如果参考值偏高偏低提示与之前的不符
                        {
                            res.RefFlag = ((int)res_ref_flag).ToString();
                        }

                        if (res.ItmId == "2082")
                        {
                            if (Convert.ToDouble(res.ObrValue) < 0.1)
                            {
                                res.RefFlag = "0";
                            }
                            else if (Convert.ToDouble(res.ObrValue) > 0.1 && Convert.ToDouble(res.ObrValue) < 50)
                            {
                                res.RefFlag = "8";
                            }
                        }

                        #region 更新参考值
                        if (refValue != null)
                        {
                            res.RefUpperLimit = refValue.ItmUpperLimitValue;
                            res.RefLowerLimit = refValue.ItmLowerLimitValue;
                            res.RefType = ValueConvertHelper.IntParse(refValue.ItmRefFlag, 0);
                        }
                        else if (res.ItmId == "2082")   //茂名妇幼PCT特殊处理
                        {
                            res.RefUpperLimit = null;
                            res.RefLowerLimit = null;
                            res.RefType = 1;
                        }
                        else
                        {
                            res.RefUpperLimit = null;
                            res.RefLowerLimit = null;
                            res.RefType = 0;
                        }
                        #endregion

                        #region 更新项目标本中的信息
                        if (referenceSam != null)
                        {
                            res.ObrUnit = referenceSam.ItmUnit;
                            res.ItmPrice = referenceSam.ItmPrice;
                            res.ObrItmMethod = referenceSam.ItmMethod;

                            decimal decResChr;
                            //裁剪小数位 增加判断结果类型不为数值是不裁剪小数位
                            if (referenceSam.ItmResType == "数值"
                                && decimal.TryParse(res.ObrValue, out decResChr))//为数字型
                            {
                                if (referenceSam.ItmMaxDigit > 0)
                                {
                                    string formatString = "{0:f" + referenceSam.ItmMaxDigit.ToString() + "}";

                                    res.ObrValue = string.Format(formatString, decResChr);
                                }
                                else if (referenceSam.ItmMaxDigit == -1)//字典设置，当小数点位设为-1则为取整
                                {
                                    res.ObrValue = string.Format("{0:f0}", decResChr);
                                }
                            }
                        }
                        else
                        {
                            if (refItem != null)
                            {
                                res.ItmPrice = refItem.ItmPrice;
                            }
                            else
                            {
                                res.ItmPrice = null;
                            }
                            res.ObrUnit = null;
                            res.ObrItmMethod = null;
                        }
                        #endregion

                        #region 更新项目中的信息

                        if (refItem != null)
                        {
                            res.ItmReportCode = refItem.ItmRepCode;
                            res.ItmEname = refItem.ItmEcode;
                        }
                        else
                        {
                            res.ItmReportCode = res.ItmEname;
                        }
                        #endregion

                        #region 转换数字结果
                        //更新res_cast_chr，数字结果

                        res.ObrConvertValue = null;
                        if (!string.IsNullOrEmpty(res.ObrValue))
                        {
                            string res_chr_temp = res.ObrValue; ;
                            if (res.ObrValue.Contains(">")
                                    || res.ObrValue.Contains("<")
                                    || res.ObrValue.Contains("=")
                                    )
                            {
                                res_chr_temp = res.ObrValue.Replace(">", "").Replace("<", "").Replace("=", "");
                            }
                            double douTemp = 0;
                            decimal decResCastChr;
                            if (decimal.TryParse((double.TryParse(res_chr_temp, out douTemp) ? douTemp.ToString() : res_chr_temp), out decResCastChr))
                            {
                                res.ObrConvertValue = decResCastChr;
                            }
                            else if (res_chr_temp.Contains('e') || res_chr_temp.Contains('E'))
                            {
                                double d;
                                if (double.TryParse(res_chr_temp.Replace(" ", ""), out d))
                                {
                                    res.ObrConvertValue = Convert.ToDecimal(d);
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "预览更新参考值", ex.ToString());
            }
        }

        #endregion

    }
}
