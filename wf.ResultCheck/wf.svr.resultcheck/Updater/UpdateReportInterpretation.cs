using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using dcl.root.logon;
using System.Threading;
using Lib.DAC;
using System.Data;
using dcl.svr.cache;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck.Updater
{
    /// <summary>
    /// 报告解读功能
    /// </summary>
    public class UpdateReportInterpretation : AbstractAuditClass, IAuditUpdater
    {
        public UpdateReportInterpretation(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi,  List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {
        }

        public void Update(ref EntityOperationResult chkResult)
        {
            if (config.Interpretation_Report&&(
                auditType == EnumOperationCode.Audit 
                || (config.bAllowStepAuditToReport 
                && (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report))))
            {
                Thread t = new Thread(ThreadUpdateInterpretation);
                t.Start();
            }
           
        }

        /// <summary>
        /// 报告解读功能
        /// </summary>
        public void ThreadUpdateInterpretation()
        {
            try
            {
                //解读内容决定性结果，诊断意义
                StringBuilder decisiveInfo = new StringBuilder();

                //参考范围
                StringBuilder refInfo = new StringBuilder();

                //危急值
                StringBuilder dangerInfo = new StringBuilder();


                //水平临床意义
                StringBuilder lvInfo = new StringBuilder();


                bool Ref_CheckValueInclude = CacheSysConfig.Current.GetSystemConfig("Ref_CheckValueInclude") == "是";
                bool Pan_CheckValueInclude = CacheSysConfig.Current.GetSystemConfig("Pan_CheckValueInclude") == "是";

                //系统配置：比值结果按照分数判断参考值
                bool Ref_CheckValueIsGradeScore = CacheSysConfig.Current.GetSystemConfig("Ref_CheckValueIsGradeScore") != "否";
                //系统配置：参考值忽略结果的大于小于号直接判断
                bool Ref_CheckValueIsNegbigOrLittleSymbol = CacheSysConfig.Current.GetSystemConfig("Ref_CheckValueIsNegbigOrLittleSymbol") == "是";

                //参考值上下限
                decimal decRefH;
                decimal decRefL;

                //危急值上下限
                decimal decPanH;
                decimal decPanL;


                //判断是否有分期标志
                if (resulto != null && resulto.Count > 0)
                {
                    string pat_sam_id = pat_info.PidSamId;//标本ID
                    string pat_sex = pat_info.PidSex;//性别
                    string itr_id = pat_info.RepItrId;//仪器
                    string sam_remark = pat_info.SampRemark;//仪器

                    foreach (EntityObrResult res in resulto)
                    {
                        bool ismatch = false;
                        //找出对应的标本类型dict_item_sam
                        var queryItemSam = from itm_sam in DictItemSamCache.Current.DclCache
                                           where itm_sam.ItmId == res.ItmId
                                           && (itm_sam.ItmSamId == pat_sam_id || itm_sam.ItmSamId == "-1")
                                           && itm_sam.ItmItrId == itr_id
                                           orderby itm_sam.ItmSamId descending, itm_sam.ItmItrId descending
                                           select itm_sam;

                        List<EntityDicItemSample> listItemSam = new List<EntityDicItemSample>(queryItemSam);
                        if (listItemSam.Count == 0)
                        {
                            queryItemSam = from itm_sam in DictItemSamCache.Current.DclCache
                                           where itm_sam.ItmId == res.ItmId
                                           && (itm_sam.ItmSamId == pat_sam_id || itm_sam.ItmSamId == "-1")
                                           && (itm_sam.ItmItrId == itr_id || itm_sam.ItmItrId == "-1")
                                           orderby itm_sam.ItmSamId descending, itm_sam.ItmItrId descending
                                           select itm_sam;

                            listItemSam = new List<EntityDicItemSample>(queryItemSam);
                        }

                        if (listItemSam.Count == 0)
                        {
                            continue;
                        }
                        EntityDicItemSample refSam = listItemSam[0];
                        //针对决定性试验，可以对疾病的本质做出诊断的高度特异性的试验。
                        //当出现阳性结果时即可诊断为该病，而非其它病。如尿道分泌物培养淋球菌阳性时，即可诊断为淋病。
                        if (!string.IsNullOrEmpty(refSam.DecisiveResult) && !string.IsNullOrEmpty(res.ObrValue)
                            && (refSam.DecisiveResult == res.ObrValue || res.ObrValue.Contains(refSam.DecisiveResult)))
                        {
                            decisiveInfo.AppendLine(string.Format("  {0} 结果为:{1} 时  {2}; ", res.ItmName, res.ObrValue, refSam.DecisiveResult));
                            continue;
                        }

                        if (!string.IsNullOrEmpty(refSam.DangerResult) && !string.IsNullOrEmpty(res.ObrValue)
                            && (refSam.DangerResult == res.ObrValue || res.ObrValue.Contains(refSam.DangerResult)))
                        {
                            dangerInfo.AppendLine(string.Format(" {0}结果:{1} 出现危急值; ", res.ItmName, res.ObrValue));
                            ismatch = true;
                        }

                        //List<EntityDicItmRefdetail> ListdictItmMi =DictItemMiCache.Current.DclCache.FindAll(item => (item.ItmId == res.ItmId && item.ItmSamId == sam_id
                        //    && (item.ItmSex == pat_sex || (item.ItmSex != "1" && item.ItmSex != "2"))));

                        var quertItemMi = from itm_mi in DictItemMiCache.Current.DclCache
                                          where itm_mi.ItmId == res.ItmId && itm_mi.ItmRefFlag == 0
                                           && (itm_mi.ItmSex == pat_sex || itm_mi.ItmSex == "0" || string.IsNullOrEmpty(itm_mi.ItmSex))
                                           && itm_mi.ItmItrId == itr_id
                                           && itm_mi.ItmAgeUpperMinute >= Convert.ToInt32(pat_info.PidAge.Value) && itm_mi.ItmAgeLowerMinute <= Convert.ToInt32(pat_info.PidAge.Value) && (itm_mi.ItmSamId == refSam.ItmSamId)
                                           && (itm_mi.ItmSamRemark == sam_remark || string.IsNullOrEmpty(itm_mi.ItmSamRemark))
                                          orderby itm_mi.ItmSamRemark descending
                                          select itm_mi;

                        List<EntityDicItmRefdetail> listItemMi = new List<EntityDicItmRefdetail>(quertItemMi);

                        if (listItemMi.Count == 0 && refSam.ItmItrId == "-1")
                        {
                            quertItemMi = from itm_mi in DictItemMiCache.Current.DclCache
                                          where itm_mi.ItmId == res.ItmId && itm_mi.ItmRefFlag == 0
                                           && (itm_mi.ItmSex == pat_sex || itm_mi.ItmSex == "0" || string.IsNullOrEmpty(itm_mi.ItmSex))
                                           && (itm_mi.ItmItrId == itr_id || itm_mi.ItmItrId == "-1")
                                           && itm_mi.ItmAgeUpperMinute >= Convert.ToInt32(pat_info.PidAge.Value) && itm_mi.ItmAgeLowerMinute <= Convert.ToInt32(pat_info.PidAge.Value) && (itm_mi.ItmSamId == refSam.ItmSamId)
                                           && (itm_mi.ItmSamRemark == sam_remark || string.IsNullOrEmpty(itm_mi.ItmSamRemark))
                                          orderby itm_mi.ItmSamRemark descending
                                          select itm_mi;

                            listItemMi = new List<EntityDicItmRefdetail>(quertItemMi);
                        }

                        if (listItemMi.Count > 0)
                        {
                            //参考范围与危急值
                            EnumResRefFlag refResult = EnumResRefFlag.Normal;
                            string res_chr = ResultRemoveSymbol(res.ObrValue);
                            var referenceValue = listItemMi[0];

                            decimal decResChr;

                            bool ismatchRef = false;

                            #region 水平范围(优先判断水平，如果命中水平范围，则不判断参考值与危急值)

                            if (decimal.TryParse(res_chr, out decResChr))
                            {
                                if (!string.IsNullOrEmpty(referenceValue.DecisiveLeve1) &&
                                !string.IsNullOrEmpty(referenceValue.DecisiveLeve1Mean))
                                {
                                    ismatchRef = GenLevelInfo(lvInfo, res, referenceValue.DecisiveLeve1, referenceValue.DecisiveLeve1Mean, decResChr);
                                }

                                if (!string.IsNullOrEmpty(referenceValue.DecisiveLeve2) &&
                                !string.IsNullOrEmpty(referenceValue.DecisiveLeve2Mean))
                                {
                                    ismatchRef = GenLevelInfo(lvInfo, res, referenceValue.DecisiveLeve2, referenceValue.DecisiveLeve2Mean, decResChr);
                                }

                                if (!string.IsNullOrEmpty(referenceValue.DecisiveLeve3) &&
                                !string.IsNullOrEmpty(referenceValue.DecisiveLeve3Mean))
                                {
                                    ismatchRef = GenLevelInfo(lvInfo, res, referenceValue.DecisiveLeve3, referenceValue.DecisiveLeve3Mean, decResChr);
                                }

                                if (!string.IsNullOrEmpty(referenceValue.DecisiveLeve4) &&
                                !string.IsNullOrEmpty(referenceValue.DecisiveLeve4Mean))
                                {
                                    ismatchRef = GenLevelInfo(lvInfo, res, referenceValue.DecisiveLeve4, referenceValue.DecisiveLeve4Mean, decResChr);
                                }
                            }
                            #endregion

                            #region 判断参考值危急值 返回refResult 

                            if (decimal.TryParse(res_chr, out decResChr)&&!ismatchRef)
                            {
                                if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmUpperLimitValue), out decRefH))
                                {
                                    if (ResultSymbolCheck(res.ObrValue, referenceValue.ItmUpperLimitValue) && Ref_CheckValueIsGradeScore)
                                    {

                                        if (decResChr < decRefH)
                                        {
                                            refResult = refResult | EnumResRefFlag.Greater1;
                                        }

                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(res.ObrValue) && res.ObrValue.StartsWith(">") && res.ObrValue.IndexOf("=") < 0 && !Ref_CheckValueIsNegbigOrLittleSymbol) //edit by zheng
                                        {
                                            if (decResChr >= decRefH)
                                            {
                                                refResult = refResult | EnumResRefFlag.Greater1;
                                            }
                                        }
                                        else if (Ref_CheckValueInclude)
                                        {
                                            if (decResChr >= decRefH)
                                            {
                                                refResult = refResult | EnumResRefFlag.Greater1;
                                            }
                                        }
                                        else
                                        {
                                            if (referenceValue.ItmUpperLimitValue.IndexOf('<') >= 0 && referenceValue.ItmUpperLimitValue.IndexOf('=') >= 0)
                                            {
                                                if (decResChr > decRefH)
                                                {
                                                    refResult = refResult | EnumResRefFlag.Greater1;

                                                }
                                            }
                                            else if (referenceValue.ItmUpperLimitValue.IndexOf('<') >= 0 && referenceValue.ItmUpperLimitValue.IndexOf('=') < 0)
                                            {
                                                if (decResChr > decRefH)
                                                {
                                                    refResult = refResult | EnumResRefFlag.Greater1;

                                                }
                                            }
                                            else
                                            {
                                                if (decResChr > decRefH)
                                                {
                                                    refResult = refResult | EnumResRefFlag.Greater1;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmLowerLimitValue), out decRefL))
                                {
                                    if (ResultSymbolCheck(res.ObrValue, referenceValue.ItmLowerLimitValue) && Ref_CheckValueIsGradeScore)
                                    {
                                        if (referenceValue.ItmLowerLimitValue.IndexOf('<') >= 0)
                                        {
                                            if (decResChr < decRefL)
                                            {
                                                refResult = refResult | EnumResRefFlag.Greater1;
                                            }
                                        }
                                        else
                                        {
                                            if (decResChr > decRefL)
                                            {
                                                refResult = refResult | EnumResRefFlag.Lower1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(res.ObrValue) && res.ObrValue.StartsWith("<") && res.ObrValue.IndexOf("=") < 0 && !Ref_CheckValueIsNegbigOrLittleSymbol) //edit by zheng
                                        {
                                            if (decResChr <= decRefL)
                                            {
                                                refResult = refResult | EnumResRefFlag.Lower1;
                                            }
                                        }
                                        else if (Ref_CheckValueInclude)
                                        {
                                            if (decResChr <= decRefL)
                                            {
                                                refResult = refResult | EnumResRefFlag.Lower1;
                                            }
                                        }
                                        else
                                        {
                                            if (referenceValue.ItmLowerLimitValue.IndexOf('>') >= 0 && referenceValue.ItmLowerLimitValue.IndexOf('=') >= 0)
                                            {
                                                if (decResChr < decRefL)
                                                {
                                                    refResult = refResult | EnumResRefFlag.Lower1;
                                                }
                                            }
                                            else if (referenceValue.ItmLowerLimitValue.IndexOf('>') >= 0 && referenceValue.ItmLowerLimitValue.IndexOf('=') < 0)
                                            {
                                                if (decResChr < decRefL)
                                                {
                                                    refResult = refResult | EnumResRefFlag.Lower1;

                                                }
                                            }
                                            else
                                            {
                                                if (decResChr < decRefL)
                                                {
                                                    refResult = refResult | EnumResRefFlag.Lower1;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmDangerUpperLimit), out decPanH))
                                {
                                    if (Pan_CheckValueInclude)
                                    {
                                        if (decResChr >= decPanH)
                                        {
                                            refResult = refResult | EnumResRefFlag.Greater2;
                                        }
                                    }
                                    else
                                    {
                                        if (referenceValue.ItmDangerUpperLimit.IndexOf('<') >= 0 && referenceValue.ItmDangerUpperLimit.IndexOf('=') < 0)
                                        {
                                            if (decResChr >= decPanH)
                                            {
                                                refResult = refResult | EnumResRefFlag.Greater2;
                                            }
                                        }
                                        else
                                        {
                                            if (decResChr > decPanH)
                                            {
                                                refResult = refResult | EnumResRefFlag.Greater2;
                                            }
                                        }
                                    }
                                }

                                if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmDangerLowerLimit), out decPanL))
                                {
                                    if (Pan_CheckValueInclude)
                                    {
                                        if (decResChr <= decPanL)
                                        {
                                            refResult = refResult | EnumResRefFlag.Lower2;
                                        }
                                    }
                                    else
                                    {
                                        if (referenceValue.ItmDangerLowerLimit.IndexOf('>') >= 0 && referenceValue.ItmDangerLowerLimit.IndexOf('=') < 0)
                                        {
                                            if (decResChr <= decPanL)
                                            {
                                                refResult = refResult | EnumResRefFlag.Lower2;
                                            }
                                        }
                                        else
                                        {
                                            if (decResChr < decPanL)
                                            {
                                                refResult = refResult | EnumResRefFlag.Lower2;
                                            }
                                        }
                                    }

                                }
                            }

                            if (((refResult & EnumResRefFlag.Greater1) == EnumResRefFlag.Greater1) ||
                               ((refResult & EnumResRefFlag.Lower1) == EnumResRefFlag.Lower1))
                            {
                                ismatch = true;
                                refInfo.AppendLine(string.Format("  {0} 结果:{1} 超出参考值; {2} {3}"
                                    , res.ItmName, res.ObrValue
                                    , string.IsNullOrEmpty(refSam.ItmLowerTips) ? "" : "\r\n  (1)" + refSam.ItmLowerTips
                                    , string.IsNullOrEmpty(refSam.ItmUpperTips) ? "" : "\r\n  (2)" + refSam.ItmUpperTips
                                   ));
                            }

                            if ((refResult & EnumResRefFlag.Greater2) == EnumResRefFlag.Greater2)
                            {
                                ismatch = true;
                                dangerInfo.AppendLine(string.Format("  {0} 结果:{1} 超出危急值上限; {2} ",
                                    res.ItmName, res.ObrValue
                                    , string.IsNullOrEmpty(referenceValue.DangerUpperMean) ? "" : "\r\n  " + referenceValue.DangerUpperMean
                                    ));
                            }
                            if ((refResult & EnumResRefFlag.Lower2) == EnumResRefFlag.Lower2)
                            {
                                ismatch = true;
                                dangerInfo.AppendLine(string.Format("  {0} 结果:{1} 超出危急值下限; {2} ",
                                  res.ItmName, res.ObrValue
                                  , string.IsNullOrEmpty(referenceValue.DangerUpperMean) ? "" : "\r\n  " + referenceValue.DangerUpperMean
                                  ));
                            }
                            #endregion

                          

                            //结果影响因素查询
                            if (ismatch && !string.IsNullOrEmpty(refSam.ResultInfluence) && !refInfo.ToString().Contains(refSam.ResultInfluence))
                            {
                                refInfo.AppendLine(string.Format("  项目结果影响因素:{0};", refSam.ResultInfluence));
                            }

                        }
                    }
                }
                StringBuilder sumInfo = new StringBuilder();
                sumInfo.AppendLine("智能报告解读仅供参考,如有与临床不符,请慎重!");
                if (decisiveInfo.Length > 0)
                {
                    sumInfo.AppendLine(" ");
                    sumInfo.AppendLine("★ ★ ★");
                    sumInfo.Append(decisiveInfo);
                }
                if (lvInfo.Length > 0)
                {
                    sumInfo.AppendLine(" ");
                    sumInfo.AppendLine("★ ★");
                    sumInfo.Append(lvInfo);
                }
                if (dangerInfo.Length > 0)
                {
                    sumInfo.AppendLine(" ");
                    sumInfo.AppendLine(" ★");
                    sumInfo.Append(dangerInfo);
                }
                if (refInfo.Length > 0)
                {
                    sumInfo.AppendLine(" ");
                    sumInfo.AppendLine(" ☆");
                    sumInfo.Append(refInfo);
                }
                

                if(decisiveInfo.Length==0&& lvInfo.Length == 0 &&
                    dangerInfo.Length == 0 && refInfo.Length == 0 )
                {
                    sumInfo = new StringBuilder();
                    sumInfo.Append("报告正常");
                }
                IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                if (dao != null)
                {
                    dao.UpdateReportSumInfo(pat_info.RepId, sumInfo.ToString());//执行更新
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("报告解读ThreadUpdateInterpretation", ex);
            }
        }

        private static bool GenLevelInfo(StringBuilder lvInfo, EntityObrResult res, string lev,string levelMean, decimal decResChr)
        {
            decimal decLow;
            decimal decHigh;
            string[] splitedRef = lev.Split('-');
            if (splitedRef.Length == 2)
            {
               
                if (decimal.TryParse(splitedRef[0], out decLow)
                    && decimal.TryParse(splitedRef[1], out decHigh)
                    && decResChr > decLow && decResChr < decHigh)
                {
                    lvInfo.Append(string.Format("  {2}结果:{3}  ;{0} {1} ",
                       lev, levelMean, res.ItmName,res.ObrValue));
                    return true;
                }
            }
            else
            {
                if(lev.Contains("<"))
                {
                    if (decimal.TryParse(ResultRemoveSymbol(lev), out decLow)&& decResChr< decLow)
                    {
                        lvInfo.Append(string.Format("  {2}结果:{3}  ;{0} {1} ",
                       lev, levelMean, res.ItmName, res.ObrValue));
                        return true;
                    }
                }
                if (lev.Contains(">"))
                {
                    if (decimal.TryParse(ResultRemoveSymbol(lev), out decHigh) && decResChr > decHigh)
                    {
                        lvInfo.Append(string.Format("  {2}结果:{3}  ;{0} {1} ",
                         lev, levelMean, res.ItmName, res.ObrValue));
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool ResultSymbolCheck(string strValue, string refvalue)
        {
            try
            {
                double douTemp = 0;
                if (string.IsNullOrEmpty(strValue) || string.IsNullOrEmpty(refvalue)) return false;

                strValue = strValue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                refvalue = refvalue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                if (strValue.Contains(":") && refvalue.Contains(":"))
                {
                    string[] splited = strValue.Split(':');
                    string[] splitedRef = strValue.Split(':');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        decimal decLeftRef;
                        decimal decRightRef;
                        if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                            && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight)
                            && decimal.TryParse((double.TryParse(splitedRef[0], out douTemp) ? douTemp.ToString() : splitedRef[0]), out decLeftRef)
                            && decimal.TryParse((double.TryParse(splitedRef[1], out douTemp) ? douTemp.ToString() : splitedRef[1]), out decRightRef))
                        {

                            return true;
                        }
                    }
                }
            }
            catch
            { }

            return false;

        }

        /// <summary>
        /// 去掉指定的符号
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string ResultRemoveSymbol(string strValue)
        {
            double douTemp = 0;
            strValue = (strValue ?? "").TrimStart(new char[] { '=', '>', '<', '≥' });
            if (strValue.Contains(":"))
            {
                string[] splited = strValue.Split(':');
                if (splited.Length == 2)
                {
                    decimal decLeft;
                    decimal decRight;
                    if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                        && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
                    {

                        return Convert.ToDecimal(Convert.ToDouble(splited[1])).ToString();
                    }
                }
            }
            else if (strValue.Contains("："))
            {
                string[] splited = strValue.Split('：');
                if (splited.Length == 2)
                {
                    decimal decLeft;
                    decimal decRight;
                    if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                        && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
                    {

                        return Convert.ToDecimal(Convert.ToDouble(splited[1])).ToString();
                    }
                }
            }

            if (!string.IsNullOrEmpty(strValue) && double.TryParse(strValue, out douTemp))
            {
                strValue = Convert.ToDecimal(douTemp).ToString();
            }

            return strValue;
        }

    }
}
