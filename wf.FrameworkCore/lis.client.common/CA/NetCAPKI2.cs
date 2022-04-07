using System;
using System.Data;
using System.Configuration;

using System.Collections;
using System.Text;
using SecuInter;
using System.Web;
namespace NETCA
{
    /// <summary>NETCA PKI 中间件接口；
    /// 
    /// secuinter.dll v4.1.0.1请保障注册的是这个版本；
    /// 
    /// 需添加secuinter.dll的引用，本类即可使用；
    /// 
    /// 用户可以修改该文件，已达到自己需求，但须保障功能正确性；
    /// </summary>
    public class NetCAPKI2
    {
        public NetCAPKI2()
        {

            //
            // TODO: 在此处添加构造函数逻辑
            //
        }



        /// <summary>5.3.2 使用证书进行PKCS7签名 2011-12-19  
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="isNotHasSource"></param>
        /// <param name="pwd"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static string getTimeStamp(String sSource, String url)
        {
            SecuInter.Utilities util = new SecuInter.Utilities();
            byte[] text = (byte[])util.UTF8Encode(sSource);

            SecuInter.TimeStampService ts = new SecuInter.TimeStampService();
            ts.TSAURL = url;
            ts.Content = text;
            //ts.Options = 2;// SecuInter.s.SECUINTER_TSOPTION_DONT_VERIFY_TIMESTAMPCERT;
            ts.HashAlgorithm = SecuInter.SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM;
            object o1 = ts.GetTimeStamp();
            object o = ts.GetTimeStampToken();
           // SecuInter.TimeStampService ts = new SecuInter.tim.TimeStampService();

            return "";
        }


    }
}
