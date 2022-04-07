using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace dcl.outside.service
{
    /// <summary>
    /// OutSendService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class OutSendService : System.Web.Services.WebService
    {

        [WebMethod]
        public string UploadLisRepData(string reportResult)
        {
            return OutSendServiceBIZ.UploadLisRepData(reportResult);
        }

        [WebMethod]
        public string GetLisItems()
        {
            StringBuilder builder = OutSendServiceBIZ.GetLisItems();
            if (builder != null)
                return builder.ToString();
            else
                return null;
        }


        [WebMethod]
        public string GetLisSubItems()
        {
            StringBuilder builder = OutSendServiceBIZ.GetLisSubItems();
            if (builder != null)
                return builder.ToString();
            else
                return null;
        }

        /// <summary>
        /// 返回此编码相关的数据
        /// </summary>
        /// <param name="hospSampleID"></param>
        /// <returns>如果参数为空，返回结果也为空。</returns>
        [WebMethod]
        public string GetLisRequest(string hospSampleID)
        {
            StringBuilder builder = OutSendServiceBIZ.GetLisRequest(hospSampleID);
            if (builder != null)
                return builder.ToString();
            else
                return "";
        }

        /// <summary>
        /// 确认获取标本信息成功
        /// </summary>
        /// <param name="hospSampleID"></param>
        /// <returns></returns>
        [WebMethod]
        public string AffirmRequest(string hospSampleID)
        {
            return OutSendServiceBIZ.AffirmRequest(hospSampleID);
        }

        [WebMethod]
        public string RecordSampleStatus(string hospSampleID, string statusCode)
        {
            return OutSendServiceBIZ.RecordSampleStatus(hospSampleID, statusCode);
        }
    }
}
