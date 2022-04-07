using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace dcl.client.ca
{
    public class WebApiAgent
    {
        /// <summary>
        /// HttpWebRequest实现post请求
        /// </summary>
        public static string PostHttp(string url, string body, string contentType = "application/json;charset=UTF-8")
        {
            string responseContent = string.Empty;
            try
            {
                Lib.LogManager.Logger.LogInfo(body);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                httpWebRequest.ContentType = contentType;
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 30 * 1000;

                byte[] btBodys = Encoding.UTF8.GetBytes(body);
                httpWebRequest.ContentLength = btBodys.Length;
                httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                responseContent = streamReader.ReadToEnd();

                httpWebResponse.Close();
                streamReader.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();
                Lib.LogManager.Logger.LogInfo(responseContent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            return responseContent;
        }
    }
}
