using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace dcl.common
{
    public class WebRequestManager
    {
        #region 全局cookie 用于记录会话
        private static CookieContainer ConnectionCookieContainer = null;

        public static CookieContainer GetCookieContainer()
        {
            if (ConnectionCookieContainer == null)
            {
                ConnectionCookieContainer = new CookieContainer();
            }
            return ConnectionCookieContainer;
        }
       
        #endregion

        #region 共用私有函数
       

        /// <summary>
        /// 预处理Key/value请求,把一个Dictionary对象转换成Json字符串请求
        /// </summary>
        /// <param name="PostData"></param>
        /// <returns></returns>
        private static string PrepareRequestData(Dictionary<string, string> PostData)
        {
            string Result = "";
            foreach (string s in PostData.Keys)
            {
                Result += String.Format("{0}={1}&", s, PostData[s]);
            }
            if (Result != "")
            {
                Result = Result.Substring(0, Result.Length - 1);
            }

            return Result;
        }

        #endregion
        
        #region 实现体
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="SendStream"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UploadFile(string FileName, Stream SendStream, String url,string dir)
        {
            CookieContainer ConnectionCookieContainer = GetCookieContainer();

            //请求对象
            HttpWebRequest wRequest =
                    GetRequest(url, ConnectionCookieContainer);
            wRequest.Headers.Add("filename", FileName);
            wRequest.Headers.Add("dirname", dir);
            using (Stream requestStream = wRequest.GetRequestStream())
            {
                byte[] paramByte = new byte[SendStream.Length]; ;
                SendStream.Read(paramByte, 0, paramByte.Length);
                requestStream.Write(paramByte, 0, paramByte.Length);
            }
            //接收编码对象
            Encoding RecieveDecode = Encoding.GetEncoding("UTF-8");

            return GetResponse(wRequest, RecieveDecode);
        }

        static private HttpWebRequest GetRequest(string url, CookieContainer CookieContainer)
        {
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            wRequest.Method = "POST";
            wRequest.Headers.Add(HttpRequestHeader.Pragma.ToString(), "no-cache");
            wRequest.CookieContainer = CookieContainer;
            return wRequest;
        }


        private static HttpWebRequest GetPostNormalRequest(string url,
            CookieContainer CookieContainer)
        {
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            wRequest.Method = "POST";
            wRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            wRequest.Headers.Add(HttpRequestHeader.Pragma.ToString(), "no-cache");
            wRequest.CookieContainer = CookieContainer;
            return wRequest;

        }

        static private string GetResponse(HttpWebRequest wRequest, Encoding enCoding)
        {
            using (HttpWebResponse WResponse = (HttpWebResponse)wRequest.GetResponse())
            {
                using (StreamReader recieve = new StreamReader(WResponse.GetResponseStream(), enCoding))
                {
                    return recieve.ReadToEnd().ToString();
                }
            }
        }


        public static string SendPostNormalRequest(
            string PostUrl, Dictionary<string,string> PostValues)
        {
            string Result = null;
            try
            {

                HttpWebRequest wRequest =
                GetPostNormalRequest(PostUrl, ConnectionCookieContainer);

                using (Stream writer = wRequest.GetRequestStream())
                {
                    string send = PrepareRequestData(PostValues);
                    byte[] byteArray = Encoding.UTF8.GetBytes(send); // 转化 UTF8
                    writer.Write(byteArray, 0, byteArray.Length);
                }
                //接收编码对象
                Encoding RecieveDecode = Encoding.UTF8;
                Result = GetResponse(wRequest, RecieveDecode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public static string SendPostNormalRequest(
                     string PostUrl, string sendString)
        {
            string Result = null;
            try
            {
                if (ConnectionCookieContainer == null)
                    ConnectionCookieContainer = new CookieContainer();
                HttpWebRequest wRequest =
                         GetNormalRequest(PostUrl, ConnectionCookieContainer);

                using (Stream writer = wRequest.GetRequestStream())
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(sendString); // 转化 UTF8
                    writer.Write(byteArray, 0, byteArray.Length);
                }
                //接收编码对象
                Encoding RecieveDecode = Encoding.UTF8;
                Result = GetResponse(wRequest, RecieveDecode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }


        public static byte[] SendPostNormalRequestByteRes(string URL, 
            Dictionary<string,string> PostValues, string FailFlag)
        {
            byte[] Result = null;
            string url = URL.Trim();
            try
            {
                //请求对象
                HttpWebRequest wRequest =
                    GetPostNormalRequest(url, GetCookieContainer());
                
                //发送编码对象 UTF8
                Encoding SendEnCode = Encoding.UTF8;

                using (Stream requestStream = wRequest.GetRequestStream())
                {
                    string send = PrepareRequestData(PostValues);
                    byte[] paramByte = SendEnCode.GetBytes(send);
                    requestStream.Write(paramByte, 0, paramByte.Length);
                }
                Result = GetByteResponse(wRequest, FailFlag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }


        public static byte[] SendGetNormalRequestByteRes(
            string URL,string FailFlag)
        {
            byte[] Result = null;
            string url = URL.Trim();
            try
            {
                //请求对象
                HttpWebRequest wRequest =
                    GetNormalRequestByGetWay(url, GetCookieContainer());
                Result = GetByteResponse(wRequest, FailFlag);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public static HttpWebRequest GetNormalRequestByGetWay(
            string url, CookieContainer CookieContainer)
        {
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            wRequest.Method = "GET";
            wRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            wRequest.Headers.Add(HttpRequestHeader.Pragma.ToString(), "no-cache");
            wRequest.CookieContainer = CookieContainer;
            return wRequest;
        }


        public static byte[] GetByteResponse(
            HttpWebRequest wRequest,string FailFlag)
        {
            byte[] Result = null;
            int bufferSize = 1024;
            MemoryStream ms = new MemoryStream();
            try
            {
                using (HttpWebResponse WResponse =
                    (HttpWebResponse)wRequest.GetResponse())
                {
                    using (Stream ResultStream = WResponse.GetResponseStream())
                    {
                        byte[] bytes = new byte[bufferSize];
                        int length = ResultStream.Read(bytes, 0, bufferSize);

                        //若第一个byte[]是输出“FailFlag”，则表示没有文件流数据返回
                        string FirstString = Encoding.UTF8.GetString(bytes);
                        FirstString = (FirstString.Length > FailFlag.Length)
                            ? FirstString.Substring(0, FailFlag.Length) 
                            : FirstString;
                        if (FirstString == FailFlag)
                            return Result;

                        while (length > 0)
                        {
                            ms.Write(bytes, 0, length);
                            length = ResultStream.Read(bytes, 0, bufferSize);
                        }
                    }
                }
                if (ms.Length > 0)
                {
                    Result = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);// 设置当前流的位置为流的开始
                    ms.Read(Result, 0, Result.Length);
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse res = (HttpWebResponse)ex.Response;
                StreamReader sr = new StreamReader(res.GetResponseStream());
                string strHtml = sr.ReadToEnd();
            }
            finally
            {
                ms.Close();
            }
            return Result;
        }

        #endregion

        public static HttpWebRequest GetJsonRequest(string url, CookieContainer CookieContainer)
        {
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            wRequest.Method = "POST";
            //wRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            wRequest.Headers.Add(HttpRequestHeader.Pragma.ToString(), "no-cache");
            wRequest.CookieContainer = CookieContainer;
            return wRequest;

        }

        public static string SendNormalRequest(string URL, string RequestString)
        {
            string Result = null;
            string url = URL.Trim();

            try
            {
                //请求对象
                HttpWebRequest wRequest =
                    GetNormalRequest(url, GetCookieContainer());
                wRequest.Timeout = 15000;

                //发送编码对象 UTF8
                Encoding SendEnCode = Encoding.UTF8;
                //接收编码对象 UTF8
                Encoding RecieveDecode = Encoding.UTF8;

                using (Stream requestStream = wRequest.GetRequestStream())
                {
                    byte[] paramByte = SendEnCode.GetBytes(RequestString);
                    requestStream.Write(paramByte, 0, paramByte.Length);
                }
                Result = GetResponse(wRequest, RecieveDecode);
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            return Result;
        }

        public static HttpWebRequest GetNormalRequest(string url,
           CookieContainer CookieContainer)
        {
            HttpWebRequest wRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            wRequest.Method = "POST";
            wRequest.Headers.Add(HttpRequestHeader.Pragma.ToString(), "no-cache");
            wRequest.Headers.Add("accept-encoding", "no");
            wRequest.ContentType = "";
            wRequest.CookieContainer = CookieContainer;
            return wRequest;

        }
    }
}
