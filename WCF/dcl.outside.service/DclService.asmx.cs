using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace dcl.outside.service
{
    /// <summary>
    /// DclService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DCLService : System.Web.Services.WebService
    {
        //<?xml version = "1.0" encoding="utf-8"?>
        //<Request>

        //</Request>


        [WebMethod]
        public string DCLInterface(string MethodName, string XMLData)
        {
            DCLServiceBIZ biz = new DCLServiceBIZ();
            return biz.DCLInterface(MethodName, XMLData);
        }



    }
}
