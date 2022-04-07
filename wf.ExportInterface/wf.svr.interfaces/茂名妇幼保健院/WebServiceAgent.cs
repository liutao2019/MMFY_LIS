using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml;

namespace dcl.svr.interfaces
{

    /// <summary<  
    /// WebService代理类  
    /// </summary<  
    public class WebServiceAgent
    {
        /// <summary>
        /// WebServiceCall
        /// </summary>
        /// <param name="URL">接口地址</param>
        /// <param name="MethodName">方法名</param>
        /// <param name="SoapXml">SoapXML</param>
        /// <param name="Response">返回内容</param>
        /// <param name="Errmsg">错误信息</param>
        /// <returns></returns>
        public bool Invoke(string URL, string MethodName, 
            string SoapXml, 
            out string Response, out string Errmsg, string ContentType = "text/xml", string SoapAction= "")
        {
            Response = "";
            Errmsg = "";
            bool Result = false;

            try
            {
                WebRequest webRequest = WebRequest.Create(URL);
                HttpWebRequest httpRequest = (HttpWebRequest)webRequest;
                httpRequest.Method = "POST";
                httpRequest.ContentType = ContentType;
                httpRequest.Headers.Add("SOAPAction:" + SoapAction);
                httpRequest.ProtocolVersion = HttpVersion.Version11;
                httpRequest.Credentials = CredentialCache.DefaultCredentials;
                Stream requestStream = httpRequest.GetRequestStream();
                StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.UTF8);

                streamWriter.Write(SoapXml);
                streamWriter.Close();
                HttpWebResponse wr = (HttpWebResponse)httpRequest.GetResponse();
                StreamReader srd = new StreamReader(wr.GetResponseStream());
                Response = srd.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(Response);
                Response = ReadXML(doc, "/Envelope/Body");
                Result = true;
            }
            catch (Exception ex)
            {
                Errmsg = "接口" + MethodName + "发生错误：" + ex.Message;
                Result = false;
            }
            return Result;
        }

        private string ReadXML(XmlDocument doc, string path)
        {
            try
            {
                if (doc.DocumentElement.Attributes != null && doc.DocumentElement.Attributes.Count > 0)
                {
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("ab", doc.DocumentElement.Attributes[0].Value);
                    return doc.SelectSingleNode(path.Replace("/", "/ab:"), nsmgr)?.InnerText;
                }
                else
                    return doc.SelectSingleNode(path)?.InnerText;

            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
