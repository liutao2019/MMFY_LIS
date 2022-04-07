using dcl.client.common;
using dcl.client.frame;
using dcl.entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.ca
{
    public class GDCAPKI : ICaPKI
    {
        private static string _caServiceUrl = string.Empty;
        private static string _appId = string.Empty;
        private static string _channelSecret = string.Empty;
        private static string _signNo = string.Empty;
        private static string _channelCode = string.Empty;
        private static string _outSysId = string.Empty;
        private static string _callBackUrl = string.Empty;

        static GDCAPKI()
        {
            _caServiceUrl = ConfigHelper.GetSysConfigValueWithoutLogin("CAUrl");
            _appId = ConfigHelper.GetSysConfigValueWithoutLogin("CAAppId");
            _channelSecret = ConfigHelper.GetSysConfigValueWithoutLogin("CAChannelSecret");
            _signNo = ConfigHelper.GetSysConfigValueWithoutLogin("CASignNo");
            _channelCode = ConfigHelper.GetSysConfigValueWithoutLogin("CAChannelCode");
            _outSysId = ConfigHelper.GetSysConfigValueWithoutLogin("CAOutSysId");
            _callBackUrl = ConfigHelper.GetSysConfigValueWithoutLogin("CACallBackUrl");
        }
        public string CAMode => "GDCA";

        public string ErrorInfo { get; set; }
        public string CurrentKey { get; set; }
        public string UserId { get; set; }

        /// <summary>
        /// 数字签名
        /// </summary>
        /// <param name="plainData"></param>
        /// <returns></returns>
        public string CASignature(string plainData)
        {
            string res = string.Empty;
            try
            {
                string url = _caServiceUrl + @"/getway/signservice/api/v2/sign/signText";

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;

                var data = new GDCASignData()
                {
                    bizNum = UserId,
                    signNo = _signNo,
                    channelCode = _channelCode,
                    outSysId = _outSysId,
                    signString = plainData
                };
                var postData = new GDCASignRequest()
                {
                    appId = _appId,
                    bussNo = System.Guid.NewGuid().ToString("N"),
                    data = data,
                    timestamp = t.ToString()
                };
                string sText = string.Format("appId={0}&bizNum={1}&bussNo={2}&channelCode={3}&outSysId={4}&signNo={5}&signString={6}&timestamp={7}",
                    _appId, data.bizNum, postData.bussNo, postData.data.channelCode, postData.data.outSysId,
                    postData.data.signNo, postData.data.signString, postData.timestamp);
                sText += @"&APP=" + _channelSecret;
                string signInfo = GetSignInfo(sText);
                postData.signInfo = signInfo;

                string jsonData = WebApiAgent.PostHttp(url, SerializeObject(postData));

                GDCASignResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<GDCASignResponse>(jsonData);

                if (string.Equals("0",response.code))
                {
                    res = response.data.signedValue;
                }
                else
                {
                    throw new Exception(response.message);
                }
                
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                ErrorInfo = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// 时间戳签名
        /// </summary>
        /// <param name="plainData"></param>
        /// <returns></returns>
        public string CATimeStamp(string plainData)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;
            return t.ToString();
        }

        /// <summary>
        /// 获取用户证书绑定值
        /// </summary>
        /// <returns></returns>
        public string GetIdentityID()
        {
            string res = string.Empty;
            try
            {
                string url = _caServiceUrl + @"/getway/signservice/api/v2/stamp/cert/query";

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;
                var data = new GDCACertQueryData()
                {
                    bizNum = UserId
                };
                var postData = new GDCACertQueryRequest()
                {
                    appId = _appId,
                    bussNo = System.Guid.NewGuid().ToString("N"),
                    timestamp = t.ToString()
                };
                postData.data = data;
                string sText = string.Format("appId={0}&bizNum={1}&bussNo={2}&timestamp={3}", _appId, data.bizNum, postData.bussNo, postData.timestamp);
                sText += @"&APP=" + _channelSecret;
                string signInfo = GetSignInfo(sText);
                postData.signInfo = signInfo;

                string jsonData = WebApiAgent.PostHttp(url, SerializeObject(postData));

                GDCACertQueryResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<GDCACertQueryResponse>(jsonData);

                if (string.Equals("0", response.code))
                {
                    res = response.data.certBase64+":" + response.data.stampBase64;
                }
                else
                {
                    throw new Exception(response.message);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                ErrorInfo = ex.Message;
            }
            return res;
        }

        /// <summary>
        /// CA认证登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool LoginWithCA(EntityLogin login)
        {
            try
            {
                //string usrCertId = GetIdentityID();
                //if (usrCertId != login.CAEntityId)
                //{
                //    ErrorInfo = "数字证书不匹配";
                //    return false;
                //}

                string url = _caServiceUrl + @"/getway/signservice/api/v2/auth/getAuthEabTime";

                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
                long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000;
                var data = new GDCALoginData()
                {
                    bizNum = UserId
                };
                var postData = new GDCALoginRequest()
                {
                    appId = _appId,
                    bussNo = System.Guid.NewGuid().ToString("N"),
                    data = data,
                    timestamp = t.ToString()
                };

                string sText = string.Format("appId={0}&bizNum={1}&bussNo={2}&timestamp={3}", _appId, data.bizNum, postData.bussNo,postData.timestamp);
                sText += @"&APP=" + _channelSecret;
                string signInfo = GetSignInfo(sText);
                postData.signInfo = signInfo;

                string jsonData = WebApiAgent.PostHttp(url, SerializeObject(postData));

                GDCALoginResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<GDCALoginResponse>(jsonData);

                if (string.Equals("0", response.code))
                {
                    if (-1 == response.data.status)
                    {
                        throw new Exception("用户授权失败");
                    }
                    if (response.data.availableTime <= 0)
                    {
                        throw new Exception("用户授权时间已过，需重新授权");
                    }
                    return true;
                }
                else
                {
                    throw new Exception(response.message);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                ErrorInfo = ex.Message;
                return false;
            }
        }

        private static string GetSignInfo(string signData)
        {
            System.Security.Cryptography.SHA256Managed sha256 = new System.Security.Cryptography.SHA256Managed();
            byte[] sData = Encoding.UTF8.GetBytes(signData);
            signData = Convert.ToBase64String(sData);
            byte[] data = Encoding.Default.GetBytes(signData);
            byte[] hash = sha256.ComputeHash(data);
            string sHash = ByteToStringUnicode(hash);
            return sHash;
        }

        private static string ByteToStringUnicode(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                string str = Convert.ToString(data[i], 16);
                if (str.Length < 2) { str = "0" + str; }
                sb.Append(str);
            }
            return sb.ToString();
        }

        private static string SerializeObject(object o)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(o, Formatting.Indented, setting);
            return json;
        }

    }
}
