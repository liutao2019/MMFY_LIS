using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.IO;

namespace dcl.svr.interfaces
{
    public class WebApiAgent
    {
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {  // 总是接受  
            return true;
        }
        internal YssResponse Invoke(string url, string postData, bool auth, out string ticket, bool getTicket, string postTicket)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();//开始计时

            ticket = string.Empty;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 30 * 1000;
            request.Method = "post";
            if (auth)
            {
                request.Headers.Add(HttpRequestHeader.Authorization, postTicket);//头文件增加Authorization
            }
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.118 Safari/537.36";
            request.ContentType = "application/json";
            if (!string.IsNullOrWhiteSpace(postData))
            {
                Stream strStream = request.GetRequestStream();

                byte[] data = Encoding.UTF8.GetBytes(postData);
                strStream.Write(data, 0, data.Length);
                strStream.Close();
            }

            YssResponse result = null;
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<YssResponse>(reader.ReadToEnd());
                    if ("000000".Equals(result.statusCode) && getTicket)
                    {
                        ticket = res.GetResponseHeader("Authorization");
                    }
                }
            }

            sw.Stop();//结束计时
            Lib.LogManager.Logger.LogInfo(result + "\n耗时：" + sw.Elapsed.ToString());

            return result;
        }

        /// <summary>
        /// 采样数据上传接口
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Data"></param>
        /// <param name="Referer"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        internal YssResponse sendDatePost(string Url, string Data, string Referer, string authorization)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.Referer = Referer;
            request.Headers.Add("Authorization", authorization);
            byte[] bytes = Encoding.UTF8.GetBytes(Data);
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            Stream myResponseStream = request.GetRequestStream();
            myResponseStream.Write(bytes, 0, bytes.Length);
            Lib.LogManager.Logger.LogInfo("\n采样数据上传接口入参：" + Data);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            YssResponse result = null;
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<YssResponse>(reader.ReadToEnd());
                    reader.Close();
                }
            }

            myResponseStream.Close();

            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }
            Lib.LogManager.Logger.LogInfo("\n采样数据上传接口出参：" + result.resultData);
            return result;
        }

        /// <summary>
        /// 采样数据信息返还接口
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="Data"></param>
        /// <param name="Referer"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        internal YssResponse GetYssPratintInfo(string Url, string Data, string Referer, string authorization)
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();//开始计时
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.Referer = Referer;
            request.Headers.Add("Authorization", authorization);
            byte[] bytes = Encoding.UTF8.GetBytes(Data);
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            Stream myResponseStream = request.GetRequestStream();
            myResponseStream.Write(bytes, 0, bytes.Length);
            Lib.LogManager.Logger.LogInfo("\n采样数据信息返还接口入参：" + Data);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            YssResponse result = null;
            using (var res = request.GetResponse() as HttpWebResponse)
            {
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader reader = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<YssResponse>(reader.ReadToEnd());
                    reader.Close();
                }
            }

            myResponseStream.Close();

            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }

            sw.Stop();//结束

            Lib.LogManager.Logger.LogInfo("\n采样数据信息返还接口出参：" + result + "\n耗时：" + sw.Elapsed.ToString());
            return result;
        }
    }
}
