using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.svr.cache;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 取参考值
    /// </summary>
    class RefGetter
    {
        public static EnumResRefFlag GetRefResult(string res_itm_id, string res_chr, string sex, string pat_sam_id, int age_int, string sam_remark, string itr_id, string dep_code,
            out EntityDicItmRefdetail referenceValue, out EntityDicItemSample referenceSam,
            out EntityDicItmItem refItem, ref EnumResRefFlag extResRefFlag, 
            ref EnumResRefFlag extLineFlag, ref EnumResRefFlag extReportFlag, string pid_diag = "")
        {
            referenceValue = null;
            referenceSam = null;
            refItem = null;
            EnumResRefFlag refResult = EnumResRefFlag.Normal;

            //查找项目信息
            refItem = DictItemCache.Current.DclCache.Find(i => i.ItmId == res_itm_id && i.ItmDelFlag != "1");
            if (refItem == null)
            {
                return EnumResRefFlag.Unknow;
            }

            //找出对应的标本类型dict_item_sam
            var queryItemSam = from itm_sam in DictItemSamCache.Current.DclCache
                               where itm_sam.ItmId == res_itm_id
                               && (itm_sam.ItmSamId == pat_sam_id || itm_sam.ItmSamId == "-1")
                               && itm_sam.ItmItrId == itr_id
                               orderby itm_sam.ItmSamId descending, itm_sam.ItmItrId descending
                               select itm_sam;

            List<EntityDicItemSample> listItemSam = new List<EntityDicItemSample>(queryItemSam);
            if (listItemSam.Count == 0)
            {
                queryItemSam = from itm_sam in DictItemSamCache.Current.DclCache
                               where itm_sam.ItmId == res_itm_id
                               && (itm_sam.ItmSamId == pat_sam_id || itm_sam.ItmSamId == "-1")
                               && (itm_sam.ItmItrId == itr_id || itm_sam.ItmItrId == "-1")
                               orderby itm_sam.ItmSamId descending, itm_sam.ItmItrId descending
                               select itm_sam;

                listItemSam = new List<EntityDicItemSample>(queryItemSam);
            }


            //获取危急值信息
            EntityDicUtgentValue urgentValue = DictItemUrgentValueCache.Current.GetValue(res_itm_id, pat_sam_id, dep_code, sex, age_int,pid_diag);

            if (listItemSam.Count == 0 && urgentValue == null)
            {
                return EnumResRefFlag.Unknow;
            }
            if (listItemSam.Count == 0)
            {
                return EnumResRefFlag.Unknow;
            }
            EntityDicItemSample refSam = listItemSam[0];
            referenceSam = refSam;
            var quertItemMi = from itm_mi in DictItemMiCache.Current.DclCache
                              where itm_mi.ItmId == res_itm_id && (itm_mi.ItmRefFlag == 0 || itm_mi.ItmId == "2082")
                               && (itm_mi.ItmSex == sex || itm_mi.ItmSex == "0" || string.IsNullOrEmpty(itm_mi.ItmSex))
                               && itm_mi.ItmItrId == itr_id
                               && itm_mi.ItmAgeUpperMinute >= age_int && itm_mi.ItmAgeLowerMinute <= age_int && (itm_mi.ItmSamId == refSam.ItmSamId)
                               && (itm_mi.ItmSamRemark == sam_remark || string.IsNullOrEmpty(itm_mi.ItmSamRemark))
                              orderby itm_mi.ItmSamRemark descending
                              select itm_mi;

            List<EntityDicItmRefdetail> listItemMi = new List<EntityDicItmRefdetail>(quertItemMi);

            if (listItemMi.Count == 0 && refSam.ItmItrId == "-1")
            {
                quertItemMi = from itm_mi in DictItemMiCache.Current.DclCache
                              where itm_mi.ItmId == res_itm_id && (itm_mi.ItmRefFlag == 0 || itm_mi.ItmId == "2082")
                               && (itm_mi.ItmSex == sex || itm_mi.ItmSex == "0" || string.IsNullOrEmpty(itm_mi.ItmSex))
                               && (itm_mi.ItmItrId == itr_id || itm_mi.ItmItrId == "-1")
                               && itm_mi.ItmAgeUpperMinute >= age_int && itm_mi.ItmAgeLowerMinute <= age_int && (itm_mi.ItmSamId == refSam.ItmSamId)
                               && (itm_mi.ItmSamRemark == sam_remark || string.IsNullOrEmpty(itm_mi.ItmSamRemark))
                              orderby itm_mi.ItmSamRemark descending
                              select itm_mi;

                listItemMi = new List<EntityDicItmRefdetail>(quertItemMi);
            }

            //使用新版的危急值填充项目参考值信息的 危急值与阈值
            if (listItemMi.Count > 0)
            {
                referenceValue = listItemMi[0];

                if (urgentValue != null)
                {
                    //此时需要克隆该对象，更改此对象不会造成引用影响
                    referenceValue = referenceValue.Clone() as EntityDicItmRefdetail;

                    referenceValue.ItmDangerUpperLimit = urgentValue.UgtPanH;
                    referenceValue.ItmDangerLowerLimit = urgentValue.UgtPanL;

                    referenceValue.ItmMaxValue = urgentValue.UgtMax;
                    referenceValue.ItmMinValue = urgentValue.UgtMin;

                    referenceValue.ext_itm_max = urgentValue.ExtUgtMax;
                    referenceValue.ext_itm_min = urgentValue.ExtUgtMin;
                }
            }
            else if (urgentValue != null)//如果危急值信息不为空则需要创建一个新的参考值信息
            {
                referenceValue = new EntityDicItmRefdetail();
                referenceValue.ItmId = res_itm_id;
                referenceValue.ItmSamId = urgentValue.UgtSamId;

                referenceValue.ItmDangerUpperLimit = urgentValue.UgtPanH;
                referenceValue.ItmDangerLowerLimit = urgentValue.UgtPanL;
                referenceValue.ItmMaxValue = urgentValue.UgtMax;
                referenceValue.ItmMinValue = urgentValue.UgtMin;

                referenceValue.ext_itm_max = urgentValue.ExtUgtMax;
                referenceValue.ext_itm_min = urgentValue.ExtUgtMin;
            }


            if (string.IsNullOrEmpty(res_chr))
            {
                return EnumResRefFlag.Unknow;
            }

            //允许的结果
            if (referenceValue != null && !string.IsNullOrEmpty(referenceValue.ItmResultAllow))
            {
                bool existedNotAllowValue = true;
                foreach (string item in referenceValue.ListAllowResult)
                {
                    if (res_chr == item)
                    {
                        existedNotAllowValue = false;
                        break;

                    }
                }

                if (existedNotAllowValue)
                    return EnumResRefFlag.ExistedNotAllowValues;
            }
            // 科室危急值增加描述性危急值判断
            if (urgentValue != null && !string.IsNullOrEmpty(urgentValue.UgtDesc))
            {
                if (urgentValue.UgtDesc.Contains(res_chr))
                {
                    return EnumResRefFlag.CustomCriticalValue;
                }
            }
            //自定义危急值
            if (referenceSam != null && referenceSam.ListUrgentResult.Count > 0)
            {
                if (referenceSam.ListUrgentResult.Contains(res_chr))
                {
                    return EnumResRefFlag.CustomCriticalValue;
                }
            }

            if (referenceSam != null && referenceSam.ListPositiveResult.Count == 0)//阳性结果定义
            {
                if (
                        (res_chr.Contains("阳性")
                        || res_chr.StartsWith("+")
                        || res_chr.EndsWith("+")
                        || res_chr.ToLower() == "pos"
                        )
                        && !res_chr.Contains("弱阳性")
                        && !res_chr.Contains("±")
                       && !(res_chr.Length > 1 && res_chr.Replace("+", "").Trim() == string.Empty)
                    )
                {
                    return EnumResRefFlag.Positive;
                }
                else if (res_chr.Contains("弱阳性") || res_chr.Trim() == "±")
                {
                    return EnumResRefFlag.WeaklyPositive;
                }
                else
                {
                    //return EnumResRefFlag.Unknow;
                }
            }
            else
            {
                if (referenceSam != null && referenceSam.ListPositiveResult.Contains(res_chr))
                {
                    return EnumResRefFlag.Positive;
                }
            }

            if (referenceValue == null)
            {
                return EnumResRefFlag.Unknow;
            }

            string res_chrbackup = res_chr;

            decimal decResChr = 0;decimal decResChr1 = 0;
            string part0;string part1;
            bool isSplitRange = ResultRemoveSymbolRange(res_chr, out part0, out part1);//拆分并判断是否有符号"-"
            if (isSplitRange)//有符号"-"拆分
            {
                if (!decimal.TryParse(part0, out decResChr) || !decimal.TryParse(part1, out decResChr1))
                {
                    return EnumResRefFlag.Unknow;
                }
            }
            
            bool isSplitColon = false; ;//拆分并判断是否有符号":"
            isSplitColon = ResultRemoveSymbolColon(res_chr, out part0, out part1);
            if (isSplitColon)//有符号":"拆分
            {
                if (!decimal.TryParse(part0, out decResChr) || !decimal.TryParse(part1, out decResChr1))
                {
                    return EnumResRefFlag.Unknow;
                }
            }

            if (!decimal.TryParse(res_chr, out decResChr))
            {
                return EnumResRefFlag.Unknow;
            }

            //判断结果偏高偏低的时候是否包含设定值在内(参考值)
            bool Ref_CheckValueInclude = CacheSysConfig.Current.GetSystemConfig("Ref_CheckValueInclude") == "是";

            //判断结果偏高偏低的时候是否包含设定值在内(危急值)
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

            //阈值上下限
            decimal decMax;
            decimal decMin;

            //扩展值上下限
            decimal decExtMax;
            decimal decExtMin;

            //参考值上限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmUpperLimitValue), out decRefH))
            {
                //判断结果和参考值是否符合 比值（:）
                if (ResultSymbolCheckColon(res_chrbackup, referenceValue.ItmUpperLimitValue) 
                    && isSplitColon
                    && Ref_CheckValueIsGradeScore)
                {
                    if (decResChr1 < decRefH)
                    {
                        refResult = refResult | EnumResRefFlag.Greater1;
                    }
                }
                //结果存在"-"符号，存在拆分
                else if (!string.IsNullOrEmpty(res_chrbackup) &&
                    !ResultSymbolCheckRange(res_chrbackup, referenceValue.ItmUpperLimitValue) 
                    && isSplitRange)
                {
                    if(decResChr> decRefH)//如果结果最小值比参考值最大值还要大
                    {
                        refResult = refResult | EnumResRefFlag.Greater1;
                    }
                }
                ////结果存在":"符号，存在拆分
                //else if (!string.IsNullOrEmpty(res_chrbackup) &&
                //    !ResultSymbolCheckColon(res_chrbackup, referenceValue.ItmUpperLimitValue)
                //    && isSplitColon)
                //{
                //    if (decResChr < decRefH)//如果结果最小值比参考值最大值还要大
                //    {
                //        refResult = refResult | EnumResRefFlag.Greater1;
                //    }
                //}
                else
                {
                    if (!string.IsNullOrEmpty(res_chrbackup) && res_chrbackup.StartsWith(">") && res_chrbackup.IndexOf("=") < 0 && !Ref_CheckValueIsNegbigOrLittleSymbol) //edit by zheng
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

            //参考值下限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmLowerLimitValue), out decRefL))
            {
                //判断结果和参考值是否符合 比值（:）
                if (ResultSymbolCheckColon(res_chrbackup, referenceValue.ItmLowerLimitValue)
                    && isSplitColon
                    && Ref_CheckValueIsGradeScore)
                {
                    if (decResChr1 > decRefL)
                    {
                        refResult = refResult | EnumResRefFlag.Lower1;
                    }
                    //if (referenceValue.ItmLowerLimitValue.IndexOf('<') >= 0)
                    //{
                    //    if (decResChr < decRefL)
                    //    {
                    //        refResult = refResult | EnumResRefFlag.Greater1;
                    //    }
                    //}
                    //else
                    //{
                    //    if (decResChr > decRefL)
                    //    {
                    //        refResult = refResult | EnumResRefFlag.Lower1;
                    //    }
                    //}
                }
                //结果存在"-"符号，存在拆分
                else if (!string.IsNullOrEmpty(res_chrbackup) &&
                    !ResultSymbolCheckRange(res_chrbackup, referenceValue.ItmUpperLimitValue)
                    && isSplitRange)
                {
                    if (decResChr1 < decRefL)//如果结果最大值比参考值最小值还要小
                    {
                        refResult = refResult | EnumResRefFlag.Lower1;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(res_chrbackup) && res_chrbackup.StartsWith("<") && res_chrbackup.IndexOf("=") < 0 && !Ref_CheckValueIsNegbigOrLittleSymbol) //edit by zheng
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

            //危急值上限
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

            //危急值下限
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


            //阈值上限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmMaxValue), out decMax))
            {
                if (Pan_CheckValueInclude)
                {
                    if (decResChr >= decMax)
                    {
                        refResult = refResult | EnumResRefFlag.Greater3;
                    }
                }
                else
                {
                    if (referenceValue.ItmMaxValue.IndexOf('<') >= 0 && referenceValue.ItmMaxValue.IndexOf('=') < 0)
                    {
                        if (decResChr >= decMax)
                        {
                            refResult = refResult | EnumResRefFlag.Greater3;
                        }
                    }
                    else
                    {
                        if (decResChr > decMax)
                        {
                            refResult = refResult | EnumResRefFlag.Greater3;
                        }
                    }
                }
            }

            //阈值下限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmMinValue), out decMin))
            {
                if (Pan_CheckValueInclude)
                {
                    if (decResChr <= decMin)
                    {
                        refResult = refResult | EnumResRefFlag.Lower3;
                    }
                }
                else
                {
                    if (referenceValue.ItmMinValue.IndexOf('>') >= 0 && referenceValue.ItmMinValue.IndexOf('=') < 0)
                    {
                        if (decResChr <= decMin)
                        {
                            refResult = refResult | EnumResRefFlag.Lower3;
                        }
                    }
                    else
                    {
                        if (decResChr < decMin)
                        {
                            refResult = refResult | EnumResRefFlag.Lower3;
                        }
                    }
                }

            }



            //扩展值上限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ext_itm_max), out decExtMax))
            {
                if (Pan_CheckValueInclude)
                {
                    if (decResChr >= decExtMax)
                    {
                        extResRefFlag = extResRefFlag | EnumResRefFlag.Greater2;
                    }
                }
                else
                {
                    if (referenceValue.ext_itm_max.IndexOf('<') >= 0 && referenceValue.ext_itm_max.IndexOf('=') < 0)
                    {
                        if (decResChr >= decExtMax)
                        {
                            extResRefFlag = extResRefFlag | EnumResRefFlag.Greater2;

                        }
                    }
                    else
                    {
                        if (decResChr > decExtMax)
                        {
                            extResRefFlag = extResRefFlag | EnumResRefFlag.Greater2;
                        }
                    }
                }
            }


            //扩展值下限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ext_itm_min), out decExtMin))
            {
                if (Pan_CheckValueInclude)
                {
                    if (decResChr <= decExtMin)
                    {
                        extResRefFlag = extResRefFlag | EnumResRefFlag.Lower2;
                    }
                }
                else
                {
                    if (referenceValue.ext_itm_min.IndexOf('>') >= 0 && referenceValue.ext_itm_min.IndexOf('=') < 0)
                    {
                        if (decResChr <= decExtMin)
                        {
                            extResRefFlag = extResRefFlag | EnumResRefFlag.Lower2;
                        }
                    }
                    else
                    {
                        if (decResChr < decExtMin)
                        {
                            extResRefFlag = extResRefFlag | EnumResRefFlag.Lower2;
                        }
                    }
                }

            }


            //线性值上限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmLineUpperLimit), out decExtMax))
            {
                if (referenceValue.ItmLineUpperLimit.IndexOf('<') >= 0 && referenceValue.ItmLineUpperLimit.IndexOf('=') < 0)
                {
                    if (decResChr >= decExtMax)
                    {
                        extLineFlag = extLineFlag | EnumResRefFlag.Greater2;

                    }
                }
                else
                {
                    if (decResChr > decExtMax)
                    {
                        extLineFlag = extLineFlag | EnumResRefFlag.Greater2;
                    }
                }
            }

            //线性值下限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmLineLowerLimit), out decExtMin))
            {

                if (referenceValue.ItmLineLowerLimit.IndexOf('>') >= 0 && referenceValue.ItmLineLowerLimit.IndexOf('=') < 0)
                {
                    if (decResChr <= decExtMin)
                    {
                        extLineFlag = extLineFlag | EnumResRefFlag.Lower2;
                    }
                }
                else
                {
                    if (decResChr < decExtMin)
                    {
                        extLineFlag = extLineFlag | EnumResRefFlag.Lower2;
                    }
                }

            }

            //可报告上限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmReportUpperLimit), out decExtMax))
            {
                if (referenceValue.ItmReportUpperLimit.IndexOf('<') >= 0 && referenceValue.ItmReportUpperLimit.IndexOf('=') < 0)
                {
                    if (decResChr >= decExtMax)
                    {
                        extReportFlag = extReportFlag | EnumResRefFlag.Greater2;

                    }
                }
                else
                {
                    if (decResChr > decExtMax)
                    {
                        extReportFlag = extReportFlag | EnumResRefFlag.Greater2;
                    }
                }

            }

            //可报告下限
            if (decimal.TryParse(ResultRemoveSymbol(referenceValue.ItmReportLowerLimit), out decExtMin))
            {

                if (referenceValue.ItmReportLowerLimit.IndexOf('>') >= 0 && referenceValue.ItmReportLowerLimit.IndexOf('=') < 0)
                {
                    if (decResChr <= decExtMin)
                    {
                        extReportFlag = extReportFlag | EnumResRefFlag.Lower2;
                    }
                }
                else
                {
                    if (decResChr < decExtMin)
                    {
                        extReportFlag = extReportFlag | EnumResRefFlag.Lower2;
                    }
                }

            }
            return refResult;

        }



        /// <summary>
        /// 去掉指定的符号
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string ResultRemoveSymbol(string strValue)
        {
            double douTemp = 0;
            strValue = (strValue ?? "").TrimStart(new char[] { '=', '>', '<' , '≥','≤' });
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


        /// <summary>
        /// 判断结果和参考值是否符合 比值（:）(是否同时判断结果和参考值)
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="refvalue"></param>
        /// <param name="bothValid">是否同时判断结果和参考值</param>
        /// <returns></returns>
        private static bool ResultSymbolCheckColon(string strValue, string refvalue,bool bothValid = true)
        {
            try
            {
                double douTemp = 0;
                if (string.IsNullOrEmpty(strValue) || string.IsNullOrEmpty(refvalue))
                    return false;

                strValue = strValue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                refvalue = refvalue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                if (bothValid)
                {
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
                else
                {
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
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            { }

            return false;

        }

        /// <summary>
        /// 去掉指定的符号":"
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool ResultRemoveSymbolColon(string strValue,out string part0,out string part1)
        {
            bool isSplit = false;//是否含有需要拆分的符号
            part0 = "0";
            part1 = "0";

            double douTemp = 0;
            strValue = (strValue ?? "").TrimStart(new char[] { '=', '>', '<', '≥', '≤' });
            if (strValue.Contains(":"))
            {
                string[] splited = strValue.Split(':');
                if (splited.Length == 2)
                {
                    decimal decLeft = 0;
                    decimal decRight = 0;
                    if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                        && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
                    {
                        part0 = decLeft.ToString();
                        part1 = decRight.ToString();
                    }
                }
                isSplit = true;
            }
            else if (strValue.Contains("："))
            {
                string[] splited = strValue.Split('：');
                if (splited.Length == 2)
                {
                    decimal decLeft = 0;
                    decimal decRight = 0;
                    if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                        && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
                    {
                        part0 = decLeft.ToString();
                        part1 = decRight.ToString();
                    }
                }
                isSplit = true;
            }
            else if (!string.IsNullOrEmpty(strValue) && double.TryParse(strValue, out douTemp))
            {
                part0 = Convert.ToDecimal(douTemp).ToString();
                part1 = "NULL";
                isSplit = false;
            }
            else
            {
                isSplit = false;
            }
            return isSplit;
        }





        /// <summary>
        /// 判断结果和参考值是否符合 范围值（-）
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="refvalue"></param>
        /// <returns></returns>
        private static bool ResultSymbolCheckRange(string strValue, string refvalue)
        {
            try
            {
                double douTemp = 0;
                if (string.IsNullOrEmpty(strValue) || string.IsNullOrEmpty(refvalue))
                    return false;

                strValue = strValue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                refvalue = refvalue.TrimStart(new char[] { '=', '>', '<', (char)30 });
                if (strValue.Contains("-") && refvalue.Contains("-"))
                {
                    string[] splited = strValue.Split('-');
                    string[] splitedRef = strValue.Split('-');
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

                if (strValue.Contains("-"))
                {
                    string[] splited = strValue.Split('-');
                    if (splited.Length == 2)
                    {
                        decimal decLeft;
                        decimal decRight;
                        if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                            && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
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
        /// 去掉指定的符号-
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool ResultRemoveSymbolRange(string strValue, out string part0, out string part1)
        {
            bool isSplitRange = false;//是否含有需要拆分的符号
            part0 = "0";
            part1 = "0";

            double douTemp = 0;
            strValue = (strValue ?? "").TrimStart(new char[] { '=', '>', '<', '≥', '≤' });
            if (strValue.Contains("-"))
            {
                string[] splited = strValue.Split('-');
                if (splited.Length == 2)
                {
                    decimal decLeft = 0;
                    decimal decRight = 0;
                    if (decimal.TryParse((double.TryParse(splited[0], out douTemp) ? douTemp.ToString() : splited[0]), out decLeft)
                        && decimal.TryParse((double.TryParse(splited[1], out douTemp) ? douTemp.ToString() : splited[1]), out decRight))
                    {
                        part0 = decLeft.ToString();
                        part1 = decRight.ToString();
                    }
                }
                isSplitRange = true;
            }
            else if (!string.IsNullOrEmpty(strValue) && double.TryParse(strValue, out douTemp))
            {
                part0 = Convert.ToDecimal(douTemp).ToString();
                part1 = "NULL";
                isSplitRange = false;
            }
            else
            {
                isSplitRange = false;
            }
            return isSplitRange;
        }
    }
}
