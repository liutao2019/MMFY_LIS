using dcl.entity;
using dcl.svr.sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace dcl.pub.wcf
{
    /// <summary>
    /// DownLoadData 的摘要说明 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DownLoadData : System.Web.Services.WebService
    {

        [WebMethod(Description = "下载医嘱")]
        public void DownLoad(string DeptID,string UserName,string StartTime,string EndTime)
        {
            EntityInterfaceExtParameter downLoadInfo = new EntityInterfaceExtParameter();
            downLoadInfo.DownloadType = InterfaceType.ZYDownload;
            downLoadInfo.DeptID = DeptID;
            downLoadInfo.OperationName = UserName;
            downLoadInfo.StartTime = Convert.ToDateTime(StartTime);
            downLoadInfo.EndTime = Convert.ToDateTime(EndTime);
            try
            {
                SampMainDownloadBIZ DownloadBIZ = new SampMainDownloadBIZ();
                DownloadBIZ.DownloadBarcode(downLoadInfo);
            }
            catch (Exception ex)
            {
            }
        }

        [WebMethod(Description = "删除医嘱")]
        public void Delete(string barcode, string userID, string userName)
        {
            EntitySampOperation oper = new EntitySampOperation();
            oper.OperationID = userID;
            oper.OperationName = userName;
            oper.OperationStatus = "510";
            oper.OperationStatusName = "删除条码";
            oper.OperationTime = DateTime.Now;
            oper.Remark = string.Format("删除条码：" + barcode);

            EntitySampMain SampMain = new EntitySampMain();
            SampMain.SampBarId = barcode;
            SampMain.SampBarCode = barcode;
            SampMainBIZ sampMainBIZ = new SampMainBIZ();
            sampMainBIZ.DeleteSampMain(oper, SampMain);
        }

    }
}
