using System;
using System.Collections.Generic;
using System.Text;
using GDCAHeper;
using Lib.LogManager;

namespace dcl.client.common
{
    public class GDCAReader
    {
        public const string CA_FLAG = "GDCA";
        GDCAHeper.GDCAReader reader=new GDCAHeper.GDCAReader();
        public int Init()
        {
           return reader.Init();
        }


        public bool NoneUsbKey()
        {
            return reader.NoneUsbKey();
        }


        public void AGWComInit(string agwurl)
        {
            reader.AGWComInit(agwurl);
        }

        /// <summary>
        /// 输入PIN码，登录USBKEY
        /// </summary>
        /// <param name="psw"></param>
        /// <returns></returns>
        public bool GDCALoginKey(string psw)
        {
            return reader.GDCALoginKey(psw);
        }

        /// <summary>
        /// 读取证书用户名（用在GetCertKey之后）
        /// </summary>
        /// <returns></returns>
        public string GetCertName()
        {
            return reader.GetCertName();
        }

        /// <summary>
        /// 读取证书唯一key(使用前请调用登录方法)
        /// </summary>
        /// <returns></returns>
        public string GetCertKey()
        {
            return reader.GetCertKey();
        }


        /// <summary>
        /// 读取证书唯一key
        /// </summary>
        /// <returns></returns>
        public string GetCertKeyWithLogin(string pin)
        {
            return reader.GetCertKeyWithLogin(pin);
        }


        /// <summary>
        /// 读取用户签名证书
        /// </summary>
        /// <returns></returns>
        public string GetGDCAUserCert()
        {
            return reader.GetGDCAUserCert();
        }


        public void Release()
        {
             reader.Release();
        }

        /**************************身份认证登录*******************************************************
*入参：pin  用户输入的USBKEY的PIN码与验证网关地址
*返回：trust_id   信任服务号，即证书的唯一标示符，用来关联绑定应用的用户角色权限信息
*********************************************************************************************/
        public string GDCARemoteLogin(string pin, string agwurl)
        {
            return reader.GDCARemoteLogin(pin, agwurl);
        }


        /**************************用户对数据签名******************************************************
*入参：pin  用户输入的USBKEY的PIN码
	   org_data   需要签名的原文数据
*返回：sign_data  签名产生的签名值
*********************************************************************************************/
        public string GDCASign(string pin, string org_data,ref string userCert)
        {
            return reader.GDCASign(pin, org_data, ref userCert);
        }



        /**************************调用电子认证网关服务验证签名***************************************
*入参：urladd  电子认证网关的URL地址，例如：http://192.168.2.121:8080/AGW/services/AGWService
	   user_cert  用户签名证书，用于验证签名
	   sign_data  签名值
*返回：verify_result   验签通过将返回原文数据
*********************************************************************************************/
        public string GDCAVerify(string urladd, string user_cert, string sign_data)
        {
            return reader.GDCAVerify(urladd, user_cert,  sign_data);
        }





    }
}
