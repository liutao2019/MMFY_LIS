using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using lis.dto;
using lis.dto.Entity;
using dcl.common;

namespace dcl.svr.sample
{
    public class OutlinkConnecter : IBCConnect
    {
        /// <summary>
        /// 生成医嘱ID
        /// </summary>
        /// <param name="printType"></param>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public override string GenerateOrderID(System.Data.DataRow dataRow, lis.dto.BarCodeEntity.BarcodeDownloadInfo downloadInfo)
        {
            if (this.DownLoadInfo.DownloadType == PrintType.Inpatient)//住院
                return dataRow["OrdID"].ToString();
            else
                return string.Format("{0}|{1}|{2}", dataRow["ID"].ToString(), dataRow["SeqID"].ToString(), dataRow["RpID"].ToString());
        }

        public override System.Data.DataSet DownloadHisOrder()
        {
            string input = string.Empty;// Outlink.GenerateDownloadInfoString(this.DownLoadInfo);
            if (String.IsNullOrEmpty(input))
                return null;
            //获取HIS结果字符串
            Outlink outlink = new Outlink();

            string result = outlink.HISDownloadBarcode(input);
            outlink.Dispose();
            ConvertHelper convertHelper = new ConvertHelper();

            //HIS项目转成DataTable
            System.Data.DataSet dsHISData = convertHelper.ConvertToDataSet(result, SplitType.MzInfo);
            return dsHISData;
        }

        public override string GetHisCode(string hisCombineID, string showHisCode)
        {
            if (string.IsNullOrEmpty(showHisCode))
                return hisCombineID;
            else
                return showHisCode;
        }

        public override string GetHisName(string hisCombineName, string showHisName, BarcodeCombines barcodeCombine)
        {
            if (string.IsNullOrEmpty(showHisName))
                return hisCombineName;
            else
                return showHisName;
        }

        ///// <summary>
        ///// 特殊项目处理
        ///// </summary>
        ///// <param name="hisCombineName"></param>
        ///// <param name="cnNameRow"></param>
        ///// <param name="combine"></param>
        //public override DataRow SpecialItem(string hisCombineName, DataRow cnNameRow, Combines combine, BarcodeCombines barcodeCombines)
        //{
        //    //特殊组合 静脉血分：血清 血浆 全血,项目是否在以上三个之中，有就取，没有就按传来的标本
        //    if (hisCombineName.Contains("静脉血"))
        //    {
        //        if (combine != null)
        //        {
        //            if (combine.Name.Contains("血清") || combine.Name.Contains("血浆") || combine.Name.Contains("全血"))
        //            {
        //                cnNameRow[BarcodeTable.CName.CombineName] = combine.Name;
        //            }
        //            else
        //            {
        //                cnNameRow[BarcodeTable.CName.CombineName] = hisCombineName;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //条码组合有打印名称就取，没有则取检验组合的名称
        //        if (barcodeCombines != null && !string.IsNullOrEmpty(barcodeCombines.PrintName))
        //        {
        //            cnNameRow[BarcodeTable.CName.CombineName] = barcodeCombines.PrintName;
        //        }
        //        else if (combine != null)
        //            cnNameRow[BarcodeTable.CName.CombineName] = combine.Name;
        //        else
        //            cnNameRow[BarcodeTable.CName.CombineName] = hisCombineName;
        //    }


        //    return cnNameRow;
        //}

        public override void SpecialPatient(ref DataRow newBCPatientRow, string hisCombineName)
        {
            if (hisCombineName.Contains("产检男性"))
            {
                newBCPatientRow[BarcodeTable.Patient.Name] = newBCPatientRow[BarcodeTable.Patient.Name].ToString() + "之夫";
                newBCPatientRow[BarcodeTable.Patient.Sex] = "男";
            }
        }
    }
}
