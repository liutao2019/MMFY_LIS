using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lis.dto;

namespace dcl.svr.sample
{
    public class SplitCodeCollection
    {
        public SplitCodeCollection(PrintType printType)
        {
            PrintType = printType;
        }
        public PrintType PrintType { get; set; }
        IList<SplitCodeInfo> SplitCodeInfos = new List<SplitCodeInfo>();
        internal void Add(SplitCodeInfo codeInfo)
        {
            SplitCodeInfos.Add(codeInfo);
        }

        /// <summary>
        /// 已经存在的条码中是否有可以合并的条码
        /// </summary>
        /// <param name="codeInfo"></param>
        /// <param name="findCodeInfo"></param>
        /// <returns></returns>
        internal bool Contains(SplitCodeInfo codeInfo, ref SplitCodeInfo findCodeInfo)
        {
            foreach (SplitCodeInfo item in SplitCodeInfos)
            {
                if (PrintType == PrintType.Inpatient)
                {
                    if (codeInfo.HisFeeCode != item.HisFeeCode
                         && (item.IncludeHisFeeCodes.Count > 0 && !item.IncludeHisFeeCodes.Contains(codeInfo.HisFeeCode)) 
                        && codeInfo.PatID == item.PatID
                        && codeInfo.PatName == item.PatName 
                        && codeInfo.SplitCode == item.SplitCode 
                        && SameOrderTime(codeInfo, item) 
                        && codeInfo.GroupID == item.GroupID
                            && codeInfo.OriID == item.OriID
                        )
                    {
                      //  codeInfo.IncludeHisFeeCodes.Add(item.HisFeeCode);
                        findCodeInfo = item;
                        return true;
                    }
                }
                else if (PrintType == PrintType.Outpatient)
                {
                    int count = string.IsNullOrEmpty(item.Count ) ? 1 : Convert.ToInt32(item.Count); //默认医嘱次数为1 edit by sink 2010-8-24
                    if (codeInfo.HisFeeCode != item.HisFeeCode
                        && (item.IncludeHisFeeCodes.Count > 0 && !item.IncludeHisFeeCodes.Contains(codeInfo.HisFeeCode)) 
                        && codeInfo.PatID == item.PatID
                        && codeInfo.PatName == item.PatName
                        && codeInfo.SplitCode == item.SplitCode
                            && codeInfo.OriID == item.OriID
                            && codeInfo.GroupID == item.GroupID
                        && SameOrderTime(codeInfo, item) && count > 0)
                    {
                        if (count > 1) //数量大于1的减1，多数量的处理
                            count--;

                     
                        item.Count = count.ToString();
                        findCodeInfo = item;
                        return true;
                    }
                }
                else if (PrintType == PrintType.TJ||PrintType==PrintType.TJSecond)
                {
                    if (codeInfo.HisFeeCode != item.HisFeeCode
                          && (item.IncludeHisFeeCodes.Count > 0 && !item.IncludeHisFeeCodes.Contains(codeInfo.HisFeeCode)) 
                        && codeInfo.PatID == item.PatID
                        && codeInfo.PatName == item.PatName
                        && codeInfo.SplitCode == item.SplitCode
                            && codeInfo.OriID == item.OriID
                        && SameOrderTime(codeInfo, item) 
                        //&& NotContain(SplitCodeInfos, codeInfo)
                        )
                    {
                     //   codeInfo.IncludeHisFeeCodes.Add(item.HisFeeCode);
                        findCodeInfo = item;
                        return true;
                    }
                }
            }

            findCodeInfo = null;
            return false;
        }

        private bool NotContain(IList<SplitCodeInfo> SplitCodeInfos, SplitCodeInfo codeInfo)
        {
            foreach (SplitCodeInfo item in SplitCodeInfos)
            {
                if (item.HisFeeCode == codeInfo.HisFeeCode)
                    return false;
            }

            return true;
        }

        private static bool SameOrderTime(SplitCodeInfo codeInfo, SplitCodeInfo item)
        {
            if (codeInfo == null || item == null)
                return false;

            //系统配置：[住院条码]不同开单日期允许合管
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_ZYAllowMergeDiffDate") == "是")
            {
                return true;
            }

            DateTime source = new DateTime();
            DateTime target = new DateTime();
            if (DateTime.TryParse(codeInfo.OrderTime, out source) && DateTime.TryParse(item.OrderTime, out target))
                return source.Date == target.Date; //只判断日期

            return false;
        }
    }
}
