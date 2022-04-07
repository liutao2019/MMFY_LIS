using System;
using System.Collections.Generic;
using System.Text;
using NETCA;
using SecuInter;

namespace dcl.client.common
{
    public class GDNetCA
    {
        public const string CA_FLAG = "GDNETCA";
        public static string GetUserID()
        {
            //NETCA：证书唯一标识/证件号码信息扩展域值/证书旧姆印,首选证书唯一标识（参见本表值23->36->31）
            SecuInter.X509Certificate oCert = NetCAPKI.getX509Certificate(
                NetCAPKI.SECUINTER_CURRENT_USER_STORE, NetCAPKI.SECUINTER_MY_STORE,
                1, 1);
            if (oCert != null)
            //?//
            {
                String bs64String = NetCAPKI.getX509CertificateInfo(oCert, 9);
                string[] strings = bs64String.Split('@');
                if (strings.Length == 1)
                {
                    return null;
                }
                SecuInter.Utilities oUtil = new SecuInter.Utilities();
                return oUtil.UTF8Decode(oUtil.Base64Decode(strings[1].Substring(7)));
            }
            //return "28276CD9DDD1F8AD812A2821C6DDAD001AC4F55D";

            return null;
        }

        public static string GetThumbPrint()
        {
            try
            {
                //NETCA模式
                string strNETCA_ModeType = ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType");
                if (strNETCA_ModeType == "深圳新安")
                {
                    NetcaPkiLib.Certificate prCert = new NetcaSign().getCertForSignature();
                    if (prCert != null)
                    {
                        /***获取证书微缩图 *****/

                        //sha1
                        object SHA1 = prCert.get_ThumbPrint(8192);
                        //string str_SHA1 = System.Text.Encoding.UTF8.GetString(((byte[])SHA1));
                        string str_SHA1 = BitConverter.ToString(((byte[])SHA1)).Replace("-", string.Empty);

                        return str_SHA1;
                    }
                    return "";
                }
            }
            catch
            {
            }


            //NETCA：证书唯一标识/证件号码信息扩展域值/证书旧姆印,首选证书唯一标识（参见本表值23->36->31）
            SecuInter.X509Certificate oCert = NetCAPKI.getX509Certificate(
                NetCAPKI.SECUINTER_CURRENT_USER_STORE, NetCAPKI.SECUINTER_MY_STORE,
                1, 1);
            if (oCert != null)
                return NetCAPKI.getX509CertificateThumbprint(oCert);
            return null;
        }

        /*
         *  证书响应码
         *  0	响应成功，当响应码不为0时，“验证证书姆印” 和 “证书状态码”无效
            1	请求数据包有误
            2	电子验证服务器内部出错
            3	电子验证服务器压力过重，请以后再试
            4	未用（在此作为客户端签名有误）
            5	请求数据需要签名
            6	请求未被授权
         * 
         * 
            证书状态码
            0	证书有效
            1	证书被注销
            2	状态未知
            3	证书格式有误
            4	证书已过有效期
            5	不是NETCA颁发的证书
            8	其他错误
        */
        public static string CheckCert(string url)
        {
            //return "";
            X509Certificate oCert = NetCAPKI.getX509Certificate(
               NetCAPKI.SECUINTER_CURRENT_USER_STORE, NetCAPKI.SECUINTER_MY_STORE,
               1, 1);
            CertAuthClient oCertAuthClient = new CertAuthClient();
            int[] iRT = new int[2];
            if (!String.IsNullOrEmpty(url))
            {
                try
                {
                    oCertAuthClient.s_chkUrl = url;
                    iRT = oCertAuthClient.CheckCert(oCert);
                }
                catch (Exception ee)
                {
                    return "证书验证出现错误！";
                }
            }

            string result = "";
            switch (iRT[0])
            {
                case 0:
                    break;
                case 1:
                    result = "请求数据包有误";
                    break;
                case 2:
                    result = "电子验证服务器内部出错";
                    break;
                case 3:
                    result = "电子验证服务器压力过重，请以后再试";
                    break;
                case 4:
                    result = "未用（在此作为客户端签名有误）";
                    break;
                case 5:
                    result = "请求数据需要签名";
                    break;
                case 6:
                    result = "请求未被授权";
                    break;
                default:
                    break;
            }
            string result2 = "";
            switch (iRT[1])
            {
                case 0:
                    break;
                case 1:
                    result2 = "证书被注销";
                    // Lib.LogManager.Logger.WriteLine("证书被注销");
                    break;
                case 2:
                    result2 = "证书状态未知";
                    //Lib.LogManager.Logger.WriteLine("状态未知");
                    break;
                case 3:
                    result2 = "证书格式有误";
                    // Lib.LogManager.Logger.WriteLine("证书格式有误");
                    break;
                case 4:
                    result2 = "证书已过有效期";
                    //Lib.LogManager.Logger.WriteLine("证书已过有效期");
                    break;
                case 5:
                    result2 = "不是NETCA颁发的证书";
                    // Lib.LogManager.Logger.WriteLine("不是NETCA颁发的证书");
                    break;
                case 8:
                    result2 = "证书出现错误！";
                    //Lib.LogManager.Logger.WriteLine("其他错误");
                    break;
                default:
                    break;
            }

            if (!String.IsNullOrEmpty(result))
                result = "证书响应：" + result;
            if (!string.IsNullOrEmpty(result2))
                result2 = " 证书状态:" + result2;
            result = result + result2;

            Lib.LogManager.Logger.WriteLine(result);
            return result;
        }

        public static string CheckCert(string url, string GWCert)
        {
            int[] iRT = new int[2];
            try
            {
                string random = NetCAPKI.getRandom(8);
                string RandSignedData = NetCAPKI.signNETCA2(random, false, "");

                NetcaPkiLib.Certificate oCert = NetCAPKI.verifySignedData(random, RandSignedData);

                //string sUsrCertId = NetCAPKI.getX509CertificateInfo(oCert, 9);

                ///验证证书的有效性
                ///验证方法一、有网关验证
                ///验证方法二、OCSP验证
                ///验证方法三、本地离线验证
                NetcaPkiLib.Certificate oSerCert = NetCAPKI.getX509CertificateNew(GWCert);

                if (oSerCert == null)
                {
                    return "网关证书内容解析错误";
                }
                DateTime dtBegin = DateTime.Now;
                CertAuthClient oCertAuthClient = new CertAuthClient();

                //网关接口验证方式二，广州市第十二人民医院项目调用该服务接口进行验证，医院正式的url地址和服务器证书信息请详见<<广州市第十二人民医院电子认证网关信息.txt>>。
                iRT = oCertAuthClient.checkCert(oCert, url, GWCert);
            }
            catch (Exception ex)
            {
                return ex.ToString();
                //MessageBox.Show(ex.Message);
            }
            string result = "";
            switch (iRT[0])
            {
                case 0:
                    break;
                case 1:
                    result = "请求数据包有误";
                    break;
                case 2:
                    result = "电子验证服务器内部出错";
                    break;
                case 3:
                    result = "电子验证服务器压力过重，请以后再试";
                    break;
                case 4:
                    result = "未用（在此作为客户端签名有误）";
                    break;
                case 5:
                    result = "请求数据需要签名";
                    break;
                case 6:
                    result = "请求未被授权";
                    break;
                default:
                    break;
            }
            string result2 = "";
            result2 = parseCertStatus(iRT[1]);
            if (!String.IsNullOrEmpty(result))
                result = "证书响应：" + result;
            if (!string.IsNullOrEmpty(result2))
                result2 = " 证书状态:" + result2;
            result = result + result2;
            Lib.LogManager.Logger.WriteLine(result);
            return result;
        }
        /// <summary>6.1.3解析证书状态码
        /// 
        /// </summary>
        /// <param name="certCode"></param>
        /// <returns></returns>
        public static string parseCertStatus(int certCode)
        {
            string[] sParse = {
                    "证书有效","验证处理失败","证书格式有误",
                    "证书不在有效期内","证书不能用于电子签名",
                    "证书名字不合","证书策略不合",
                    "证书扩展不合","不受证书链信任",
                    "证书被注销","注销状态未知",
                    "用户证书未授权","用户状态被锁定"
                    };

            if (certCode >= 0 && certCode < 13)
            {
                return "证书状态码: " + certCode + " (" + sParse[certCode] + ")";
            }
            return "证书状态码:" + certCode + " " + "(其他错误)";
        }
        public static bool SignData(string text, ref string signData)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }



            try
            {
                //string strNETCA_ModeType = ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType");
                //if (strNETCA_ModeType == "深圳新安")
                //{
                //    return new NetcaSign().sign(text,ref signData);
                //}



                string ctmodel = ConfigHelper.GetSysConfigValueWithoutLogin("CATimestampMode");
                string url = ConfigHelper.GetSysConfigValueWithoutLogin("CA_NETCAURL");
                if (string.IsNullOrEmpty(url) || ctmodel != "GDNETCA")
                {
                    signData = NetCAPKI.signPKCS7ByPwd(text, false, "");
                }
                else
                {
                    signData = NetCAPKI.signPKCS7WithTSA(text, url, false);
                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
            return false;
        }

        /// <summary>
        /// 上传ca数据到netca
        /// </summary>
        /// <param name="buss_code">业务类型编码 001登陆，004检验，999其他</param>
        /// <param name="Op_code">操作类型编码 01查看，02修改，03增加，04删除，99其他</param>
        /// <param name="op_date_time">操作时间 yyyyMMddHHmmss</param>
        /// <param name="hispital_code">认证机构批复码</param>
        /// <param name="ca_code">证书编号</param>
        /// <param name="ca_dept">所属科室（汉字科室）</param>
        /// <param name="ca_name">使用者姓名（汉字名称）</param>
        /// <param name="ca_type">认证类型编码 01登陆认证，02电子签名，03电子签章，04电子签名+时间戳，05电子签章+时间戳，06时间戳</param>
        /// <param name="ca_userid">使用者身份证号</param>
        /// <returns></returns>
        public static bool LabUploadStringToCa(string buss_code, string op_code, string op_date_time, string hispital_code, string ca_code, string ca_dept, string ca_name, string ca_type, string ca_userid)
        {
            try
            {
                NetcaUploadLink.LabUploadMsg labToca = new NetcaUploadLink.LabUploadMsg();
                return labToca.LabUploadString(buss_code, op_code, op_date_time, hispital_code, ca_code, ca_dept, ca_name, ca_type, ca_userid);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return false;
        }
    }
}
