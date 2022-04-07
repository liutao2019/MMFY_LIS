using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lis.dto;
using lis.dto.Entity;
using System.Data;
using dcl.common;
using Lib.LogManager;
using dcl.common;
using dcl.svr.interfaces;

namespace dcl.svr.sample
{
    public class NormalConnecter : IBCConnect
    {
        TestsMock testMock;

        public override System.Data.DataSet DownloadHisOrder()
        {
            BCHISInterfacesBIZ interfaceBiz = new BCHISInterfacesBIZ();
            interfaceBiz.DownLoadInfo = this.DownLoadInfo;
            // return interfaceBiz.ExecuteInterface("10003", this.DownLoadInfo.PatientID);
            string interfaceType = this.DownLoadInfo.GetDownloadTypeString();
            if (string.IsNullOrEmpty(interfaceType))
                return null;

#if DEBUG
            //模拟条码下载返回的字符串结果
            testMock = TestsMock.InitForNormal();
            if (testMock.ShouldTestDownLoadBarcode)
            {
                return testMock.MockDownloadBarcode(new MockInputInfo(DownLoadInfo));
            }
            else
#endif
                return interfaceBiz.ExecuteInterfaceBySql(string.Format("in_interface_type = '{0}'", interfaceType), new ParamAdapter(this.DownLoadInfo));
        }

        public override string GenerateOrderID(System.Data.DataRow dataRow, lis.dto.BarCodeEntity.BarcodeDownloadInfo downloadInfo)
        {

            //2014年2月21日16:57:30 ye
            //if (dataRow.Table.Columns.Contains(ConvertHelper.GetHISColumn("bc_order_id")) == false)
            //{
            //    Logger.LogInfo("对照表:" + string.Format("找不到HIS字段[{0}]", ConvertHelper.GetHISColumn("bc_order_id")));
            //    return "";
            //}
            //return dataRow[ConvertHelper.GetHISColumn("bc_order_id")].ToString();

            dcl.common.ConvertHelper.ColumnConvertHelper columnConvertHelper = new ConvertHelper.ColumnConvertHelper(downloadInfo);

            if (dataRow.Table.Columns.Contains(columnConvertHelper.GetHISColumn("bc_order_id")) == false)
            {
                Logger.LogInfo("对照表:" + string.Format("找不到HIS字段[{0}]", columnConvertHelper.GetHISColumn("bc_order_id")));
                return "";
            }
            return dataRow[columnConvertHelper.GetHISColumn("bc_order_id")].ToString();

           
        }

        public override string GetHisName(string hisCombineName, string showHisName, BarcodeCombines barcodeCombine)
        {
            if (barcodeCombine != null && !string.IsNullOrEmpty(barcodeCombine.PrintName))
                return barcodeCombine.PrintName;
            if (string.IsNullOrEmpty(showHisName))
                return hisCombineName;
            else
                return showHisName;
        }
    }
}
