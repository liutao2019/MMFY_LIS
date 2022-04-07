using dcl.entity;
//using dcl.svr.micManageMent;
using dcl.svr.report;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace dcl.outside.service
{
    /// <summary>
    /// 检验平板服务接口(暂时失效，因为删除了微生物模块)
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PadService : System.Web.Services.WebService
    {

        //#region 微生物相关Pad服务方法

        //[WebMethod(Description = "新增与修改涂片数据")]
        //public string SaveOrUpdateMicSmear(string jsonMicPat, string jsonMicSmear, string jsonMicSmearImage)
        //{
        //    MicPadBIZ biz = new MicPadBIZ();
        //    string ret= biz.SaveOrUpdateMicSmear(jsonMicPat, jsonMicSmear, jsonMicSmearImage);

        //    return RetMessage(string.IsNullOrEmpty(ret), ret);
        //}

        //[WebMethod(Description = "删除涂片数据")]
        //public string DeleteMicSmear(string smearID)
        //{
        //    MicPadBIZ biz = new MicPadBIZ();
        //    string ret = biz.DeleteMicSmear(smearID);

        //    return RetMessage(string.IsNullOrEmpty(ret), ret);
        //}


        //[WebMethod(Description = "新增与修改培养基信息")]
        //public string SaveOrUpdateMicIno(string jsonMicIno)
        //{
        //    MicPadBIZ biz = new MicPadBIZ();
        //    string ret = biz.SaveOrUpdateMicIno(jsonMicIno);

        //    return RetMessage(string.IsNullOrEmpty(ret), ret);
        //}

        //[WebMethod(Description = "删除培养基数据")]
        //public string DeleteMicIno(string inoId)
        //{
        //    MicPadBIZ biz = new MicPadBIZ();
        //    string ret = biz.DeleteMicIno(inoId);

        //    return RetMessage(string.IsNullOrEmpty(ret), ret);
        //}

        //[WebMethod(Description = "新增与修改观察结果")]
        //public string SaveOrUpdateMicObserve(string repPatKey,string jsonMicObserve)
        //{
        //    MicPadBIZ biz = new MicPadBIZ();
        //    string ret = biz.SaveOrUpdateMicObserve(repPatKey,jsonMicObserve);

        //    return RetMessage(string.IsNullOrEmpty(ret), ret);
        //}



        //[WebMethod(Description = "删除观察结果")]
        //public string DeleteMicObserve(string obsId)
        //{
        //    MicPadBIZ biz = new MicPadBIZ();
        //    string ret = biz.DeleteMicObserve(obsId);

        //    return RetMessage(string.IsNullOrEmpty(ret), ret);
        //}

        //[WebMethod(Description = "返回base64格式PDF文件")]
        //public string GetReportPDF(string jsonPrintParameter)
        //{
        //    try
        //    {
        //        Lib.LogManager.Logger.LogInfo(jsonPrintParameter);

        //        EntityDCLPrintParameter parameter = JsonConvert.DeserializeObject<EntityDCLPrintParameter>(jsonPrintParameter);

        //        if(parameter==null||string.IsNullOrEmpty(parameter.ReportCode))
        //        {
        //            throw new Exception("ReportCode 为空");
        //        }

                DCLReportPrintBIZ biz = new DCLReportPrintBIZ();
        //        string ret = biz.GetReportPDF(parameter);

        //        return RetMessage(true, ret);
        //    }
        //    catch(Exception ex)
        //    {
        //        return RetMessage(false, ex.Message);
        //    }
            
        //}

        //#endregion

        //#region 公共方法
        ///// <summary>
        ///// 获取错误消息格式
        ///// </summary>
        ///// <param name="Info"></param>
        ///// <returns></returns>
        //private string RetMessage(bool success,string msgInfo)
        //{
        //    XmlDocument doc = new XmlDocument();

        //    XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        //    doc.AppendChild(dec);

        //    XmlElement root = doc.CreateElement("Response");
        //    doc.AppendChild(root);

        //    XmlElement resultCode = doc.CreateElement("ResultCode");
        //    resultCode.InnerText = success ? "0": "1";
        //    root.AppendChild(resultCode);

        //    XmlElement resultContent = doc.CreateElement("ResultContent");
        //    resultContent.InnerText = msgInfo;
        //    root.AppendChild(resultContent);

        //    return ConvertXmlToString(doc);
        //}

        ///// <summary>
        ///// XML转换为文本数据
        ///// </summary>
        ///// <param name="xmlDoc"></param>
        ///// <returns></returns>
        //private string ConvertXmlToString(XmlDocument xmlDoc)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    XmlTextWriter writer = new XmlTextWriter(stream, null);
        //    writer.Formatting = System.Xml.Formatting.Indented;
        //    xmlDoc.Save(writer);
        //    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
        //    stream.Position = 0;
        //    string xmlString = sr.ReadToEnd();
        //    sr.Close();
        //    stream.Close();
        //    stream.Dispose();
        //    return xmlString;
        //}

        //#endregion

    }
}
