using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.ca
{
    public class GDCAResponse
    {
        //数字，信息码，如果是 0，说明是正常，其他说明错误
        public string code { get; set; }
        //字符串，具体信息。
        public string message { get; set; }
        //交易流水号
        public string transactionNo { get; set; }
    }

    public class GDCACertQueryResponse: GDCAResponse
    {
        public GDCACertQueryContent data { get; set; }
    }
    public class GDCACertQueryContent
    {
        //外 观 图 片BASE64
        public string stampBase64 { get; set; }
        //证书 BASE64
        public string certBase64 { get; set; }
    }
    public class GDCASignResponse : GDCAResponse
    {
        public GDCASignContent data { get; set; }
    }
    public class GDCASignContent
    {
        //签署结果（P1）
        public string signedValue { get; set; }
        //签署序列号
        public string signSerialNum { get; set; }
    }
    public class GDCALoginResponse : GDCAResponse
    {
        public GDCALoginContent data { get; set; }
    }
    public class GDCALoginContent
    {
        //剩余授权时间
        public long? availableTime { get; set; }
        //授权状态
        public int? status { get; set; }
        //授权时间
        public DateTime? authTime { get; set; }
        //姓名
        public string name { get; set; }
        //工号
        public string bizNum { get; set; }
    }
}
