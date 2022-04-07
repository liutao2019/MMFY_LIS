using dcl.entity;
using dcl.svr.sample;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace dcl.pub.wcf.WebService
{
    /// <summary>
    /// GetTubeInfoByCombine 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class GetTubeInfoByCombine : System.Web.Services.WebService
    {
        string strTestInput = @"<?xml version='1.0' encoding='utf-8'?>
                                <Request>
                                  <CombineInfo>
                                    <SampleID>77</SampleID> --标本类型ID
                                    <HisCode>Y</HisCode><!--His组合收费编码-->
                                    <Name>测试</Name><!--组合名称-->
                                  </CombineInfo>
                                  <CombineInfo>
                                    <SampleID>77</SampleID> --标本类型ID
                                    <HisCode>X</HisCode>
                                    <Name>测试</Name>
                                  </CombineInfo>
                                </Request>";


        [WebMethod(Description = "根据组合信息获取合管信息")]
        public string Invoke(string pInput)
        {
            try
            {
                if (string.IsNullOrEmpty(pInput))
                    return ErroMessage("传入参数不能为空！");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(pInput);

                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);
                xmlReader.Close();

                DataTable dtPat = ds.Tables["CombineInfo"];
                if (ds == null || ds.Tables.Count == 0)
                    return ErroMessage("无组合信息！");

                List<EntityDicCombine> cache = dcl.svr.cache.DictCombineCache.Current.DclCache;
                List<EntityDicCombine> lst = new List<EntityDicCombine>();
                foreach (DataRow item in dtPat.Rows)
                {
                    string strSampleId = item["SampleID"].ToString();
                    string strItrId = item["HisCode"].ToString();
                    string strName = item["Name"].ToString();
                    if (!string.IsNullOrEmpty(strItrId))
                    {
                        var i = cache.FindAll(w => w.ComHisCode == strItrId);

                        if (i.Count() > 0)
                        {
                            i[0].ComSamId = strSampleId;
                            lst.Add(i[0]);
                        }
                        else
                        {
                            return ErroMessage(string.Format("不存在收费编码为【{0}】的组合【{1}】", strItrId, strName));
                        }
                    }
                }


                GetTubeInfoByCombineBIZ Biz = new GetTubeInfoByCombineBIZ();
                List<EntityDicTestTube> Tubes = Biz.GetTubes(lst);

                if (Tubes.Count == 0)
                    return ErroMessage("无法找到组合对应的试管信息！");

                StringBuilder sb = new StringBuilder();
                string format = "<{0}>{1}</{0}>";
                sb.Append(@"<?xml version='1.0' encoding='utf-8'?>");
                sb.Append("<Response>");
                sb.Append("<ResultCode>0</ResultCode>");
                sb.Append("<ResultContent></ResultContent>");
                sb.Append("<Result>");
                foreach (EntityDicTestTube t in Tubes)
                {
                    sb.Append("<Tube>");
                    sb.Append(string.Format(format, "ID", t.TubCode));
                    sb.Append(string.Format(format, "Name", t.TubName));
                    sb.Append(string.Format(format, "HisCode", t.TubChargeCode));
                    sb.Append("</Tube>");
                }
                sb.Append("</Result>");
                sb.Append("</Response>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return ErroMessage("接口错误:" + ex.Message);
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
    }
}
