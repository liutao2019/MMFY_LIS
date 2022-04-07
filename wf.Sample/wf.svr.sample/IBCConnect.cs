using dcl.root.dac;
using lis.dto;
using IBatisNet.DataMapper;
using lis.dto.Entity;
using lis.dto.BarCodeEntity;
using System.Collections;
using dcl.common.extensions;
using dcl.common;
using System.Data;
using System.Collections.Generic;
using dcl.common;

namespace dcl.svr.sample
{
    /// <summary>
    /// HIS取条码方式
    /// </summary>
    public abstract class IBCConnect
    {
        /// <summary>
        /// 从HIS下载条码
        /// </summary>
        /// <returns></returns>
        public abstract System.Data.DataSet DownloadHisOrder();


        /// <summary>
        /// 生成医嘱ID
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public abstract string GenerateOrderID(System.Data.DataRow dataRow, lis.dto.BarCodeEntity.BarcodeDownloadInfo downloadInfo);


        public BarcodeDownloadInfo DownLoadInfo { get; set; }

        /// <summary>
        /// 合并条码后,返回条码号
        /// </summary>
        /// <param name="barcodeCombine">组合</param>
        /// <param name="newBarcode">原条码</param>
        /// <returns></returns>
        public virtual string MergeBarcode(BarcodeCombines barcodeCombine, string newBarcode, IDictionary<string, string> splitCodes)
        {
            if (barcodeCombine == null) //没有对应的项目
            {
                newBarcode = CreateBarcodeNumber();
            }
            else if (string.IsNullOrEmpty(barcodeCombine.ComSplitCode) || !splitCodes.ContainsKey(barcodeCombine.ComSplitCode)) //如果合并代码不一样则生成不同的项目和条码
            {
                newBarcode = CreateBarcodeNumber();
                if (!string.IsNullOrEmpty(barcodeCombine.ComSplitCode)) //没有合并代码则肯定不合并
                    splitCodes.Add(barcodeCombine.ComSplitCode, newBarcode);
            }
            else
            {
                newBarcode = splitCodes[barcodeCombine.ComSplitCode];
            }
            return newBarcode;
        }


        /// <summary>
        /// 生成条码号
        /// </summary>
        /// <returns></returns>
        protected string CreateBarcodeNumber()
        {
            //获取最大条码号
            BCBarcodeBIZ barcodeBIZ = new BCBarcodeBIZ();
            string barcode = barcodeBIZ.GetNewBarcode();
            //不同医院的条码规则不同
            IBarcodeGenerateRule rule = new DefaultGenerateRule();
            string chkcodetype = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeUseCheckCode");
            //条码自定义前缀
            rule.barcodeCustomHead = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_BarcodeCustomHead");
            if (chkcodetype == "不使用")
                rule.CheckCodeType = 0;
            else if (chkcodetype == "2位后缀")
                rule.CheckCodeType = 2;
            else if (chkcodetype == "自定义前缀")
                rule.CheckCodeType = 3;
            else
                rule.CheckCodeType = 1;
            return rule.GenerateBarcode(barcode);
        }

        /// <summary>
        /// 特殊项目
        /// </summary>
        /// <param name="hisCombineName"></param>
        /// <param name="cnNameRow"></param>
        /// <param name="combine"></param>
        public DataRow SpecialItem(string hisCombineName, DataRow cnNameRow, Combines combine, BarcodeCombines barcodeCombines)
        {
            //特殊组合 静脉血分：血清 血浆 全血,项目是否在以上三个之中，有就取，没有就按传来的标本
            if (hisCombineName.Contains("静脉血"))
            {
                if (combine != null)
                {
                    if (combine.Name.Contains("血清") || combine.Name.Contains("血浆") || combine.Name.Contains("全血"))
                    {
                        cnNameRow[BarcodeTable.CName.CombineName] = combine.Name;
                    }
                    else
                    {
                        cnNameRow[BarcodeTable.CName.CombineName] = hisCombineName;
                    }
                }
            }
            else
            {
                //条码组合有打印名称就取，没有则取检验组合的名称
                if (barcodeCombines != null && !string.IsNullOrEmpty(barcodeCombines.PrintName))
                {
                    cnNameRow[BarcodeTable.CName.CombineName] = barcodeCombines.PrintName;
                }
                else if (combine != null)
                    cnNameRow[BarcodeTable.CName.CombineName] = combine.Name;
                else
                    cnNameRow[BarcodeTable.CName.CombineName] = hisCombineName;
            }


            return cnNameRow;
        }

        public virtual string GetHisCode(string hisCombineID, string showHisCode)
        {
            return hisCombineID;
        }

        public virtual string GetHisName(string hisCombineName, string showHisName, BarcodeCombines barcodeCombine)
        {
            return hisCombineName;
        }

        public virtual void SpecialPatient(ref DataRow newBCPatientRow, string hisCombineName)
        {

        }
    }
}
