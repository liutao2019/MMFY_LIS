using dcl.entity;
using dcl.hl7;
using dcl.svr.report;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace dcl.pub.wcf
{
    /// <summary>
    /// DCLReport 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DCLReport : System.Web.Services.WebService
    {
        string strTestInput = @"<?xml version='1.0' encoding='utf-8'?>
                                <Report>
                                  <Patient>
                                    <RepId>10216201804160186160959</RepId>
                                    <RepItrId>10216</RepItrId>
                                    <PidName>蓝文谦</PidName>
                                  </Patient>
                                  <Patient>
                                    <RepId>10216201804160187104450</RepId>
                                    <RepItrId>10216</RepItrId>
                                    <PidName>蓝文谦</PidName>
                                  </Patient>
                                </Report>";

        //入参
        //<?xml version = "1.0" encoding="utf-8"?>
        //<Report>
        //  <Patient>
        //    <RepId>标识ID</RepId>
        //    <RepItrId>仪器ID</RepItrId>
        //    <PidName>姓名</PidName>
        //  </Patient>
        //  <Patient>
        //    <RepId>标识ID</RepId>
        //    <RepItrId>仪器ID</RepItrId>
        //    <PidName>姓名</PidName>
        //  </Patient>
        //</Report>
        [WebMethod(Description = "获取A4连续打印报告")]
        public string GetA4ContinuousReport(string pInput)
        {
            try
            {
                //pInput = strTestInput;

                if (string.IsNullOrEmpty(pInput))
                    return ErroMessage("入参不能为空");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pInput);//testXml
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();

                DataTable dtPat = ds.Tables["Patient"];
                if (dtPat == null || dtPat.Rows.Count == 0)
                    return ErroMessage("未传入报告信息");

                List<EntityDicInstrument> listInstrmt = dcl.svr.cache.DictInstrmtCache.Current.DclCache;

                List<EntityDCLPrintParameter> listParameter = new List<EntityDCLPrintParameter>();

                foreach (DataRow item in dtPat.Rows)
                {
                    string strItrId = item["RepItrId"].ToString();
                    if (!string.IsNullOrEmpty(strItrId))
                    {
                        int i = listInstrmt.FindIndex(w => w.ItrId == strItrId);
                        if (i >= 0)
                        {
                            EntityDCLPrintParameter par = new EntityDCLPrintParameter();
                            par.RepId = item["RepId"].ToString();
                            par.ReportCode = listInstrmt[i].ItrReportId;
                            par.PatName = item["PidName"].ToString();

                            listParameter.Add(par);
                        }
                    }
                }

                if (listParameter.Count == 0)
                    return ErroMessage("未找到报告信息");

                DCLReportPrintBIZ reportBiz = new DCLReportPrintBIZ();
                string strPDF = reportBiz.GetA4ContinuousReportPDF(listParameter);

                if (string.IsNullOrEmpty(strPDF))
                    return ErroMessage("报表生成异常，请联系管理员");

                XmlDocument docResult = new XmlDocument();

                XmlDeclaration dec = docResult.CreateXmlDeclaration("1.0", "UTF-8", null);
                docResult.AppendChild(dec);

                XmlElement root = docResult.CreateElement("Response");
                docResult.AppendChild(root);

                XmlElement resultCode = docResult.CreateElement("ResultCode");
                resultCode.InnerText = "0";
                root.AppendChild(resultCode);

                XmlElement resultContent = docResult.CreateElement("ResultContent");
                resultContent.InnerText = "";
                root.AppendChild(resultContent);

                XmlElement result = docResult.CreateElement("Result");
                result.InnerText = strPDF;
                root.AppendChild(result);

                return ConvertXmlToString(docResult);
            }
            catch (Exception ex)
            {
                return ErroMessage(ex.ToString());
                Logger.LogInfo(ex.ToString());
            }
        }

        private string ErroMessage(string erroInfo)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("Response");
            doc.AppendChild(root);

            XmlElement resultCode = doc.CreateElement("ResultCode");
            resultCode.InnerText = "1";
            root.AppendChild(resultCode);

            XmlElement resultContent = doc.CreateElement("ResultContent");
            resultContent.InnerText = erroInfo;
            root.AppendChild(resultContent);

            return ConvertXmlToString(doc);
        }

        private string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        }

        //[WebMethod(Description = "HL7测试方法")]
        //public string HL7Test(string pInput)
        //{
            //HL7MessageBIZ biz = new HL7MessageBIZ();
            //return biz.SaveOrderInfo("", "POOR_IN200901UV21").ToString();
        //}

        //[WebMethod(Description = "HL7测试方法")]
        //public string HL7GetReportInfoTest(string pInput)
        //{
            //HL7MessageBIZ biz = new HL7MessageBIZ();
            //return biz.GetReportInfo(pInput, "POOR_IN200901UV21").ToString();
        //}
    }
}
