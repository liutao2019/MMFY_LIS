using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.ca
{
    public class GDCARequest
    {
        //渠道编码
        public string appId { get; set; }
        //交易流水
        public string bussNo { get; set; }
        //签名信息
        public string signInfo { get; set; }
        //时间戳
        public string timestamp { get;set; }
    }

    public class GDCACertQueryRequest: GDCARequest
    {
        public GDCACertQueryData data { get; set; }
    }

    public class GDCACertQueryData
    {
        //工号 String 是
        public string bizNum { get; set; }
    }

    public class GDCASignRequest: GDCARequest
    {
        public GDCASignData data { get; set; }
    }

    public class GDCASignData
    {
        //工号 String 是
        public string bizNum { get; set; }
        //外部系统签署号 String 是
        public string signNo { get; set; }
        //渠道号 String 是
        public string channelCode { get; set; }
        //待签署数据（base64） String 是
        public string signString { get; set; }
        //标题 String 否
        public string title { get; set; }
        //文件名 String 否
        public string fileName { get; set; }
        //文件路径 String 否
        public string filePath { get; set; }
        //外部系统 id String 是
        public string outSysId { get; set; }
    }

    public class GDCALoginRequest:GDCARequest
    {
        public GDCALoginData data { get; set; }
    }
    public class GDCALoginData
    {
        //工号
        public string bizNum { get; set; }
    }

}
