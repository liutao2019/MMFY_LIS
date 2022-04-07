using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.IO;
using System.Text;

namespace NETCA
{
    class Constants
    {
        public const int NETCAPKI_ALGORITHM_3DES_CBC = 16777216;
        public const int NETCAPKI_ALGORITHM_3DES_ECB = 12582912;
        public const int NETCAPKI_ALGORITHM_AES_CBC = 41943040;
        public const int NETCAPKI_ALGORITHM_AES_ECB = 37748736;
        public const int NETCAPKI_ALGORITHM_DES_CBC = 8388608;
        public const int NETCAPKI_ALGORITHM_DES_ECB = 4194304;
        public const int NETCAPKI_ALGORITHM_DH = 3;
        public const int NETCAPKI_ALGORITHM_DSA = 2;
        public const int NETCAPKI_ALGORITHM_ECC = 4;
        public const int NETCAPKI_ALGORITHM_HMAC_MD5 = 131072;
        public const int NETCAPKI_ALGORITHM_HMAC_SHA1 = 262144;
        public const int NETCAPKI_ALGORITHM_HMAC_SHA224 = 393216;
        public const int NETCAPKI_ALGORITHM_HMAC_SHA256 = 524288;
        public const int NETCAPKI_ALGORITHM_HMAC_SHA384 = 655360;
        public const int NETCAPKI_ALGORITHM_HMAC_SHA512 = 786432;
        public const int NETCAPKI_ALGORITHM_HMAC_SM3 = 1048576;
        public const int NETCAPKI_ALGORITHM_MD5 = 4096;
        public const int NETCAPKI_ALGORITHM_MD5WITHRSA = 1;
        public const int NETCAPKI_ALGORITHM_RC4 = 25165824;
        public const int NETCAPKI_ALGORITHM_RSA = 1;
        public const int NETCAPKI_ALGORITHM_RSA_PKCS1_V1_5_ENC = 16;
        public const int NETCAPKI_ALGORITHM_RSA_RAW_ENC = 48;
        public const int NETCAPKI_ALGORITHM_SHA1 = 8192;
        public const int NETCAPKI_ALGORITHM_SHA1WITHRSA = 2;
        public const int NETCAPKI_ALGORITHM_SHA224 = 12288;
        public const int NETCAPKI_ALGORITHM_SHA224WITHRSA = 3;
        public const int NETCAPKI_ALGORITHM_SHA256 = 16384;
        public const int NETCAPKI_ALGORITHM_SHA256WITHRSA = 4;
        public const int NETCAPKI_ALGORITHM_SHA384 = 20480;
        public const int NETCAPKI_ALGORITHM_SHA384WITHRSA = 5;
        public const int NETCAPKI_ALGORITHM_SHA512 = 24576;
        public const int NETCAPKI_ALGORITHM_SHA512WITHRSA = 6;
        public const int NETCAPKI_ALGORITHM_SM1_CBC = 67108864;
        public const int NETCAPKI_ALGORITHM_SM1_ECB = 62914560;
        public const int NETCAPKI_ALGORITHM_SM2_ENC = 64;
        public const int NETCAPKI_ALGORITHM_SM3 = 28672;
        public const int NETCAPKI_ALGORITHM_SM3WITHRSA = 31;
        public const int NETCAPKI_ALGORITHM_SM3WITHSM2 = 25;
        public const int NETCAPKI_ALGORITHM_SM4_CBC = 79691776;
        public const int NETCAPKI_ALGORITHM_SM4_ECB = 75497472;
        public const int NETCAPKI_ALGORITHM_SMS4_CBC = 79691776;
        public const int NETCAPKI_ALGORITHM_SMS4_ECB = 75497472;
        public const int NETCAPKI_ALGORITHM_SSF33_CBC = 54525952;
        public const int NETCAPKI_ALGORITHM_SSF33_ECB = 50331648;
        public const int NETCAPKI_BASE64_DECODE_STRICT = 2;
        public const int NETCAPKI_BASE64_ENCODE_NO_NL = 1;
        public const int NETCAPKI_BMPSTRING = 5;
        public const int NETCAPKI_CERT_ATTRIBUTE_CERT_POLICY_OID = 61;
        public const int NETCAPKI_CERT_ATTRIBUTE_CHECK_PRIVKEY = 54;
        public const int NETCAPKI_CERT_ATTRIBUTE_CRL_URL = 56;
        public const int NETCAPKI_CERT_ATTRIBUTE_CSP_NAME = 55;
        public const int NETCAPKI_CERT_ATTRIBUTE_DELTA_CRL_URL = 57;
        public const int NETCAPKI_CERT_ATTRIBUTE_DNS_NAME = 60;
        public const int NETCAPKI_CERT_ATTRIBUTE_EX_DEPARTMENT = 25;
        public const int NETCAPKI_CERT_ATTRIBUTE_EX_DEVICE_SN = 42;
        public const int NETCAPKI_CERT_ATTRIBUTE_EX_DEVICE_TYPE = 41;
        public const int NETCAPKI_CERT_ATTRIBUTE_EX_EMAIL = 26;
        public const int NETCAPKI_CERT_ATTRIBUTE_EX_FRIENDLY_NAME = 22;
        public const int NETCAPKI_CERT_ATTRIBUTE_EX_NAME = 23;
        public const int NETCAPKI_CERT_ATTRIBUTE_EX_ORGANIZATION = 24;
        public const int NETCAPKI_CERT_ATTRIBUTE_EXTENDED_KEY_USAGE = 62;
        public const int NETCAPKI_CERT_ATTRIBUTE_IP = 59;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_C = 10;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_CN = 13;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_DISPLAY_NAME = 9;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_EMAIL = 14;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_HEX_ENCODE = 53;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_L = 38;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_LDAP_NAME = 48;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_O = 11;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_OU = 12;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_ST = 37;
        public const int NETCAPKI_CERT_ATTRIBUTE_ISSUER_XMLSIG_NAME = 49;
        public const int NETCAPKI_CERT_ATTRIBUTE_OCSP_URL = 58;
        public const int NETCAPKI_CERT_ATTRIBUTE_PREVCERT_THUMBPRINT = 29;
        public const int NETCAPKI_CERT_ATTRIBUTE_PUBKEY_ECC_CURVE = 43;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_C = 17;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_CN = 20;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_DISPLAY_NAME = 16;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_EMAIL = 21;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_HEX_ENCODE = 52;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_L = 40;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_LDAP_NAME = 50;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_O = 18;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_OU = 19;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_ST = 39;
        public const int NETCAPKI_CERT_ATTRIBUTE_SUBJECT_XMLSIG_NAME = 51;
        public const int NETCAPKI_CERT_ATTRIBUTE_UPN = 36;
        public const int NETCAPKI_CERT_ENCODE_BASE64 = 2;
        public const int NETCAPKI_CERT_ENCODE_BASE64_NO_NL = 3;
        public const int NETCAPKI_CERT_ENCODE_DER = 1;
        public const int NETCAPKI_CERT_PURPOSE_ENCRYPT = 1;
        public const int NETCAPKI_CERT_PURPOSE_SIGN = 2;
        public const int NETCAPKI_CERT_PURPOSE_VERIFY_OLD_DATA = 268435458;
        public const int NETCAPKI_CERT_STATUS_CA_REVOKED = -2;
        public const int NETCAPKI_CERT_STATUS_REVOKED = 0;
        public const int NETCAPKI_CERT_STATUS_UNDETERMINED = -1;
        public const int NETCAPKI_CERT_STATUS_UNREVOKED = 1;
        public const string NETCAPKI_CERT_STORE_NAME_CA = "ca";
        public const string NETCAPKI_CERT_STORE_NAME_MY = "my";
        public const string NETCAPKI_CERT_STORE_NAME_OTHERS = "others";
        public const string NETCAPKI_CERT_STORE_NAME_ROOT = "root";
        public const int NETCAPKI_CERT_STORE_TYPE_CURRENT_USER = 0;
        public const int NETCAPKI_CERT_STORE_TYPE_LOCAL_MACHINE = 1;
        public const int NETCAPKI_CERT_STORE_TYPE_MEMORY = 2;
        public const int NETCAPKI_CERT_TYPE_LOCAL_MACHINE = 32768;
        public const int NETCAPKI_CERT_VERIFY_FLAG_ONLINE = 2;
        public const int NETCAPKI_CERT_VERIFY_FLAG_VERIFY_CACERT_REVOKE = 4;
        public const int NETCAPKI_CERT_VERIFY_FLAG_VERIFY_CRL = 32;
        public const int NETCAPKI_CERT_VERIFY_FLAG_VERIFY_OCSP = 16;
        public const int NETCAPKI_CERT_VERIFY_FLAG_VERIFY_REVOKE = 1;
        public const int NETCAPKI_CERTVERIFY_OPTION_IGNOREALLEXT = 4;
        public const int NETCAPKI_CERTVERIFY_OPTION_ONLINE_GETCACERT = 32;
        public const int NETCAPKI_CERTVERIFY_OPTION_ONLINE_GETREVOKEINFO = 64;
        public const int NETCAPKI_CERTVERIFY_OPTION_USEALLCACERT = 2;
        public const int NETCAPKI_CERTVERIFY_OPTION_USEV1CERT = 1;
        public const int NETCAPKI_CERTVERIFY_OPTION_VERIFYCAREVOKE = 16;
        public const int NETCAPKI_CERTVERIFY_OPTION_VERIFYEEREVOKE = 8;
        public const int NETCAPKI_CMS_ENCODE_BASE64 = 2;
        public const int NETCAPKI_CMS_ENCODE_BASE64_MULTILINE = 3;
        public const int NETCAPKI_CMS_ENCODE_DER = 1;
        public const int NETCAPKI_CP_ACP = 0;
        public const int NETCAPKI_CP_BIG5 = 950;
        public const int NETCAPKI_CP_GB18030 = 54936;
        public const int NETCAPKI_CP_GBK = 936;
        public const int NETCAPKI_CP_UTF16LE = 1200;
        public const int NETCAPKI_CP_UTF8 = 65001;
        public const int NETCAPKI_CRL_REASON_AACOMPROMISE = 10;
        public const int NETCAPKI_CRL_REASON_AFFILIATION_CHANGED = 3;
        public const int NETCAPKI_CRL_REASON_CACOMPROMISE = 2;
        public const int NETCAPKI_CRL_REASON_CERTIFATE_HOLD = 6;
        public const int NETCAPKI_CRL_REASON_CESSATION_OF_OPERATION = 5;
        public const int NETCAPKI_CRL_REASON_KEYCOMPROMISE = 1;
        public const int NETCAPKI_CRL_REASON_PRIVILEGE_WITHDRAWN = 9;
        public const int NETCAPKI_CRL_REASON_REMOVE_FROM_CRL = 8;
        public const int NETCAPKI_CRL_REASON_SUPERSEDED = 4;
        public const int NETCAPKI_CRL_REASON_UNSPECIFIED = 0;
        public const int NETCAPKI_DEVICEFLAG_CACHE_PIN_IN_HANDLE = 2;
        public const int NETCAPKI_DEVICEFLAG_CACHE_PIN_IN_PROCESS = 0;
        public const int NETCAPKI_DEVICEFLAG_NOT_CACHE_PIN = 4;
        public const int NETCAPKI_DEVICEFLAG_SILENT = 1;
        public const int NETCAPKI_DEVICETYPE_ANY = -1;
        public const int NETCAPKI_DEVICETYPE_EKEY_EKPK = 1;
        public const int NETCAPKI_DEVICETYPE_EKEY_EKPKXC_V2 = 2;
        public const int NETCAPKI_DEVICETYPE_EKEY_EKPKXC_V3 = 6;
        public const int NETCAPKI_DEVICETYPE_EPASS3000 = 3;
        public const int NETCAPKI_DEVICETYPE_EPASS3003 = 5;
        public const int NETCAPKI_DEVICETYPE_ETAX = 30;
        public const int NETCAPKI_DEVICETYPE_HAIKEY = 4;
        public const int NETCAPKI_DEVICETYPE_HAIKEY_SM2 = 33;
        public const int NETCAPKI_DEVICETYPE_HWETAX = 31;
        public const int NETCAPKI_DEVICETYPE_SJY03B = 101;
        public const int NETCAPKI_DEVICETYPE_SJY05B = 100;
        public const int NETCAPKI_DEVICETYPE_SOFTWARE = 0;
        public const int NETCAPKI_ECC_CURVE_P192 = 1;
        public const int NETCAPKI_ECC_CURVE_P224 = 2;
        public const int NETCAPKI_ECC_CURVE_P256 = 3;
        public const int NETCAPKI_ECC_CURVE_P384 = 4;
        public const int NETCAPKI_ECC_CURVE_P521 = 5;
        public const int NETCAPKI_ECC_CURVE_SM2 = 7;
        public const int NETCAPKI_ECC_CURVE_WAPI = 6;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_AES128CBC = 4;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_AES192CBC = 5;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_AES256CBC = 6;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_SM1CBC = 8;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_SM4CBC = 9;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_SMS4CBC = 9;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_SSF33CBC = 7;
        public const int NETCAPKI_ENVELOPEDDATA_ALGORITHM_TDESCBC = 3;
        public const int NETCAPKI_IA5STRING = 3;
        public const int NETCAPKI_KEYPAIR_ENCRYPT = 1;
        public const int NETCAPKI_KEYPAIR_SIGNATURE = 2;
        public const int NETCAPKI_KEYUSAGE_CONTENTCOMMITMENT = 2;
        public const int NETCAPKI_KEYUSAGE_CRLSIGN = 64;
        public const int NETCAPKI_KEYUSAGE_DATAENCIPHERMENT = 8;
        public const int NETCAPKI_KEYUSAGE_DECIPHERONLY = 256;
        public const int NETCAPKI_KEYUSAGE_DIGITALSIGNATURE = 1;
        public const int NETCAPKI_KEYUSAGE_ENCIPHERONLY = 128;
        public const int NETCAPKI_KEYUSAGE_KEYAGRESSMENT = 16;
        public const int NETCAPKI_KEYUSAGE_KEYCERTSIGN = 32;
        public const int NETCAPKI_KEYUSAGE_KEYENCIPHERMENT = 4;
        public const int NETCAPKI_KEYUSAGE_NONREPUDIATION = 2;
        public const int NETCAPKI_MAJOR_VERSION = 1;
        public const int NETCAPKI_MIME_TYPE_BASIC = 0;
        public const int NETCAPKI_MIME_TYPE_MESSAGE = 2;
        public const int NETCAPKI_MIME_TYPE_MULTIPART = 1;
        public const int NETCAPKI_MINOR_VERSION = 0;
        public const int NETCAPKI_OID = 6;
        public const int NETCAPKI_PADDING_NONE = 1;
        public const int NETCAPKI_PADDING_PKCS5 = 2;
        public const int NETCAPKI_PRINTABLESTRING = 2;
        public const int NETCAPKI_REGCERT_IN_MS_CERTSTORE = 2;
        public const int NETCAPKI_REGCERT_IN_NETCA_CERTSTORE = 1;
        public const int NETCAPKI_SEARCH_KEYPAIR_FLAG_CURRENT_USER = 1073741824;
        public const int NETCAPKI_SEARCH_KEYPAIR_FLAG_DEVICE = 268435456;
        public const int NETCAPKI_SEARCH_KEYPAIR_FLAG_LOCAL_MACHINE = 536870912;
        public const int NETCAPKI_SIGNEDDATA_INCLUDE_CERT_OPTION_CERTPATH = 3;
        public const int NETCAPKI_SIGNEDDATA_INCLUDE_CERT_OPTION_CERTPATHWITHROOT = 4;
        public const int NETCAPKI_SIGNEDDATA_INCLUDE_CERT_OPTION_NONE = 1;
        public const int NETCAPKI_SIGNEDDATA_INCLUDE_CERT_OPTION_SELF = 2;
        public const int NETCAPKI_SIGNEDDATA_VERIFY_LEVEL_NO_VERIFY = 0;
        public const int NETCAPKI_SIGNEDDATA_VERIFY_LEVEL_VERIFY_CERT = 2;
        public const int NETCAPKI_SIGNEDDATA_VERIFY_LEVEL_VERIFY_CERT_REVOKE = 3;
        public const int NETCAPKI_SIGNEDDATA_VERIFY_LEVEL_VERIFY_CERTPATH_REVOKE = 4;
        public const int NETCAPKI_SIGNEDDATA_VERIFY_LEVEL_VERIFY_SIGNATURE_ONLY = 1;
        public const int NETCAPKI_SO_PWD = 2;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_BADRESP = -1;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_BADTSACERT = -2;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_GRANTED = 0;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_GRANTEDWITHMODS = 1;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_REJECTION = 2;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_REVOCATIONNOTIFICATION = 5;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_REVOCATIONWARNING = 4;
        public const int NETCAPKI_TIMESTAMP_RESP_STATUS_WAITING = 3;
        public const int NETCAPKI_UIFLAG_ALWAYS_UI = 3;
        public const int NETCAPKI_UIFLAG_DEFAULT_UI = 1;
        public const int NETCAPKI_UIFLAG_NO_UI = 2;
        public const int NETCAPKI_USER_PWD = 1;
        public const int NETCAPKI_UTF8STRING = 1;
        public const int NETCAPKI_VISIBLESTRING = 4;
    }
    /// <summary>CertAuthClient 的摘要说明
    /// 验证证书有效性代码
    /// 调用该类，需修改网关的IP；
    /// 调用该类，网关的签名证书一般不修改，如有变化，NETCA会通知客户；
    /// 
    /// 
    ///      2011-12-26  整理该类,使之更容易读懂 （luhanmin修订）
    /// </summary>
    /// 
    public class CertAuthClient
    {
        //证书验证服务器的签名证书
        private static String s_certServer =
    "MIID2DCCAsCgAwIBAgIQQsFlLJb3B7Z/h0QYzgo/TzANBgkqhkiG9w0BAQUFADBv" +
    "MQswCQYDVQQGEwJDTjEkMCIGA1UEChMbTkVUQ0EgQ2VydGlmaWNhdGUgQXV0aG9y" +
    "aXR5MRkwFwYDVQQLExBTZXJ2ZXIgQ2xhc3NBIENBMR8wHQYDVQQDExZORVRDQSBT" +
    "ZXJ2ZXIgQ2xhc3NBIENBMB4XDTA4MDgxODE2MDAwMFoXDTA5MDIxOTE1NTk1OVow" +
    "VjELMAkGA1UEBhMCQ04xEjAQBgNVBAgTCUd1YW5nZG9uZzEQMA4GA1UEChMHVW5r" +
    "bm93bjEQMA4GA1UECxMHVW5rbm93bjEPMA0GA1UEAxMGdGVzdDEzMIGfMA0GCSqG" +
    "SIb3DQEBAQUAA4GNADCBiQKBgQCr9N2Xj0Q1OAourygo4cVm3ekzJBXr1LCPLgMC" +
    "pFP8M8RHPurk2rQr21LQ8dkEM3/FGU2mLyUoz22IdcJASrvfckfDOQyMD9sX6UQM" +
    "gm2wZbWwYvUMTDfHk+7OkYPaqaCCyiIACcyg67KtAhFGcr3GZIiUCjMnduZZjemd" +
    "0AsmPQIDAQABo4IBCzCCAQcwHwYDVR0jBBgwFoAUuvNKBSTm+CTI5lfaeI0MWeRD" +
    "ZMowHQYDVR0OBBYEFJ+q4Tl8GnjttO9ZV45I47tEKr97MFcGA1UdIARQME4wTAYK" +
    "KwYBBAGBkkgBCjA+MDwGCCsGAQUFBwIBFjBodHRwOi8vd3d3LmNuY2EubmV0L2Nz" +
    "L2tub3dsZWRnZS93aGl0ZXBhcGVyL2Nwcy8wEQYDVR0RBAowCIIGdGVzdDEzMAwG" +
    "A1UdEwEB/wQCMAAwDgYDVR0PAQH/BAQDAgSwMDsGA1UdHwQ0MDIwMKAuoCyGKmh0" +
    "dHA6Ly9jbGFzc2FjYTEuY25jYS5uZXQvY3JsL1NlcnZlckNBLmNybDANBgkqhkiG" +
    "9w0BAQUFAAOCAQEAO5Jy2xP2I/IJuQLnHH78BPmuC7n79FeKMleNpre/wUlSzIO1" +
    "rGFkp2gU6/s4aRY8Y9Wz5wEXrQcjLHRp/DzIqAr1Lxdm5q5wwTrlNAIaEMYwI9c0" +
    "5IQTMZdFpuidx7e6PVABQRyQ4vm90xII1JYyOyCE9Xe/vRLCE1UmeVKwJdxIcv5O" +
    "/5M/rhmS3HEGKVOElyzzjup8+WWRJJxuzLrBJsXaWfli8gEJazVWjejVuc2WHHKw" +
    "7JCGTmnvg/SWvFKXObY9wEAVXDqkQuXPldQpLubr5bDmneWjaRJHoSXtbnmQ+E1d" +
    "WdrYB6ddhK0Pc71Ei8I3gf2tKPp8TVUZrbnlzA==";

        //请根据实际的IP地址修改
        //"http://61.140.20.131:7002/certauth/chkonecert"//网证通外网网关测试地址
        //"http://192.168.0.77:7002/certauth/chkonecert" //网证通内部网关测试地址
        public String s_chkUrl = "http://192.168.0.77:7002/certauth/chkonecert";
        private int m_iseq;
        //HASH算法
        public static int NETCAPKI_ALGORITHM_HASH = Constants.NETCAPKI_ALGORITHM_SHA1;
        //RSA P1签名算法，一般无需定制
        public static int NETCAPKI_ALGORITHM_RSASIGN = Constants.NETCAPKI_ALGORITHM_SHA1WITHRSA;
        //SM2 P1签名算法，一般无需定制
        public static int NETCAPKI_ALGORITHM_SM2SIGN = Constants.NETCAPKI_ALGORITHM_SM3WITHSM2;
        public CertAuthClient()
        {
            m_iseq = 1;
        }

        /// <summary>
        /// 得到请求序列码
        /// </summary>
        /// <returns></returns>
        private /*synchronized*/ int GetSeq()
        {
            m_iseq++;
            return m_iseq;
        }
        /// <summary>
        /// PKCS1验证,微软方式实现,目前未使用了,用NETCAPKI.VerifyPKCS1替换  陆汉民 2011-12-06
        /// </summary>
        /// <param name="sContent"></param>
        /// <param name="sSignatureBase64"></param>
        /// <param name="sX509CertificateBase64"></param>
        /// <returns></returns>
        private bool VerifyData(String sContent, String sSignatureBase64, String sX509CertificateBase64)
        {
            byte[] bContent = Encoding.Default.GetBytes(sContent);
            byte[] bSignature = Convert.FromBase64String(sSignatureBase64);
            byte[] bX509CertificateDer = Convert.FromBase64String(sX509CertificateBase64);
            X509Certificate2 certSev = new X509Certificate2();
            certSev.Import(bX509CertificateDer);
            RSACryptoServiceProvider m_rsaSign = (RSACryptoServiceProvider)certSev.PublicKey.Key;
            SHA1 m_sha = SHA1.Create();
            return m_rsaSign.VerifyData(bContent, m_sha, bSignature);
        }
        /// <summary>
        /// 证书验证函数
        /// </summary>
        /// <param name="certBeChk">证书</param>
        /// <returns>返回int[2],第一个为响应码,第二个为证书状态码</returns>
        public int[] CheckCert(SecuInter.X509Certificate certBeChk)//X509Certificate2 certBeChk
        {
            SecuInter.Utilities oUtil = new SecuInter.Utilities();
            //封装请求码,http发送到电子认证网关
            int iSeq = GetSeq();
            string hexdigest = oUtil.BinaryToHex(certBeChk.get_Thumbprint(SecuInter.SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM));
            string sReqParam = Convert.ToString(iSeq) + "|" + oUtil.Base64Encode(certBeChk.get_Encoded(SecuInter.SECUINTER_CERT_ENCODE_TYPE.SECUINTER_CERT_ENCODE_DER));
            byte[] bReqParam = Encoding.Default.GetBytes(sReqParam);
            HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(s_chkUrl);
            Req.ContentType = "text/plain";
            Req.ContentLength = bReqParam.Length;
            Req.Method = "POST";
            Stream ReqStream = Req.GetRequestStream();
            ReqStream.Write(bReqParam, 0, bReqParam.Length);
            ReqStream.Close();
            HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
            StreamReader RespStream = new StreamReader(Resp.GetResponseStream(), Encoding.Default);
            string sResponse = RespStream.ReadToEnd();
            RespStream.Close();
            Resp.Close();




            //获取电子认证网关响应码    请求流水号 | 响应码 | 验证证书姆印Hex编码 | 证书状态码 | 对前面内容的签名 Base64编码
            int pos = sResponse.LastIndexOf('|');
            if (pos < 0)
                throw new Exception("服务端返回数据包有误1");
            string content = sResponse.Substring(0, pos);
            string signature = sResponse.Substring(pos + 1);
            //验证签名
            //bool isPass = VerifyData(content, signature, s_certServer);
            bool isPass = NetCAPKI.verifyPKCS1(content, signature, s_certServer);
            if (!isPass)
                throw new Exception("服务端签名无效");
            //定义返回2位数组, 响应码及证书状态码
            int[] iReturns = new int[2];
            string[] sTmps = content.Split('|');
            if (sTmps.Length < 2)
                throw new Exception("服务端返回数据包有误2");
            //请求流水号,比对验证
            string sSeq = sTmps[0];
            if (Convert.ToInt32(sSeq) != iSeq)
                throw new Exception("交易流水号不匹配,可能遭到恶意攻击");
            //响应码
            iReturns[0] = Convert.ToInt32(sTmps[1]);
            if (iReturns[0] == 0)
            {
                if (sTmps.Length != 4)
                    throw new Exception("服务端返回数据包有误3");
                //证书姆印,比对验证 
                string digest = sTmps[2].ToUpper();
                if (!digest.Equals(hexdigest))
                    throw new Exception("被验证的证书摘要不匹配,可能遭到恶意攻击");
                iReturns[1] = Convert.ToInt32(sTmps[3]);
            }
            return iReturns;
        }

        /// <summary>网关数字证书验证函数
        /// 6.1.2 a网关验证证书方式二：网关HTTP接口
        /// </summary>
        /// <param name="certBeChk">证书</param>
        /// <returns>返回int[2],第一个为响应码,第二个为证书状态码</returns>
        public int[] checkCert(NetcaPkiLib.Certificate certBeChk, string sUrl, string sGWSeverCer = "")
        {
            int[] iReturns = new int[2];
            try
            {
                if (String.IsNullOrEmpty(sGWSeverCer))
                {
                    //   sGWSeverCer = sDefaultServerCert;
                }
                //封装请求码,http发送到电子认证网关
                int iSeq = GetSeq();
                string hexdigest = NetCAPKI.convertHex(certBeChk.get_ThumbPrint(NETCAPKI_ALGORITHM_HASH)).ToUpper(); //NetcaPKI.getX509CertificateInfo(certBeChk, 1);
                string sReqParam = Convert.ToString(iSeq) + "|" + NetCAPKI.getX509CertificateInfo(certBeChk, 0);
                byte[] bReqParam = Encoding.Default.GetBytes(sReqParam);

                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(sUrl);
                Req.ContentType = "text/plain";
                Req.ContentLength = bReqParam.Length;
                Req.Method = "POST";
                Stream ReqStream = Req.GetRequestStream();
                ReqStream.Write(bReqParam, 0, bReqParam.Length);
                ReqStream.Close();

                HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                StreamReader RespStream = new StreamReader(Resp.GetResponseStream(), Encoding.Default);
                string sResponse = RespStream.ReadToEnd();
                RespStream.Close();
                Resp.Close();

                //获取电子认证网关响应码    请求流水号 | 响应码 | 验证证书姆印Hex编码 | 证书状态码 | 对前面内容的签名 Base64编码

                int pos = sResponse.LastIndexOf('|');
                if (pos < 0)
                    throw new Exception("服务端返回数据包有误1");
                string content = sResponse.Substring(0, pos);
                string signature = sResponse.Substring(pos + 1);
                //验证签名
                //网关肯定返回UTF8编码,此处不能定制，需特殊处理;
                NetcaPkiLib.Utilities initialObj = new NetcaPkiLib.Utilities();
                byte[] bContent = (byte[])initialObj.Encode(content, Constants.NETCAPKI_CP_UTF8);
                NetcaPkiLib.Certificate oWGCert = NetCAPKI.getX509CertificateNew(sGWSeverCer);
                bool isPass = verifyWGSign(oWGCert, bContent, signature);
                if (!isPass)
                    throw new Exception("服务端签名无效");
                //定义返回2位数组, 响应码及证书状态码
   
                string[] sTmps = content.Split('|');
                if (sTmps.Length < 2)
                    throw new Exception("服务端返回数据包有误2");
                //请求流水号,比对验证
                string sSeq = sTmps[0];
                if (Convert.ToInt32(sSeq) != iSeq)
                    throw new Exception("交易流水号不匹配,可能遭到恶意攻击");
                //响应码
                iReturns[0] = Convert.ToInt32(sTmps[1]);
                if (iReturns[0] == 0)
                {
                    if (sTmps.Length != 4)
                        throw new Exception("服务端返回数据包有误3");
                    //证书姆印,比对验证 
                    string digest = sTmps[2].ToUpper();
                    if (!digest.Equals(hexdigest))
                        throw new Exception("被验证的证书摘要不匹配,可能遭到恶意攻击");
                    iReturns[1] = Convert.ToInt32(sTmps[3]);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        
            return iReturns;
        }
        /// <summary>
        /// 解析响应码
        /// </summary>
        /// <param name="echoCode"></param>
        /// <returns></returns>
        public static string printEchoCode(int echoCode)
        {
            String msg = "响应码: " + echoCode;
            switch (echoCode)
            {
                case 0:
                    msg += " (响应成功)";
                    break;
                case 1:
                    msg += " (请求数据包有误)";
                    break;
                case 2:
                    msg += " (证书验证服务器内部出错)";
                    break;
                case 3:
                    msg += " (证书验证服务器压力过重，请以后再试)";
                    break;
                case 4:
                    msg += " (客户端签名有误)";
                    break;
                case 5:
                    msg += " (请求数据需要签名)";
                    break;
                case 6:
                    msg += " (请求未被授权)";
                    break;
            }
            return msg;
        }
        /// <summary>
        /// 解析证书状态码
        /// </summary>
        /// <param name="certCode"></param>
        /// <returns></returns>
        public static string printCertCode(int certCode)
        {
            String msg = "证书状态码: " + certCode;
            switch (certCode)
            {
                case 0:
                    msg += " (证书有效)";
                    break;
                case 1:
                    msg += " (证书被注销)";
                    break;
                case 2:
                    msg += " (状态未知)";
                    break;
                case 3:
                    msg += " (证书格式有误)";
                    break;
                case 4:
                    msg += " (证书已过有效期)";
                    break;
                case 5:
                    msg += " (不是NETCA颁发的证书)";
                    break;
                case 6:
                    msg += " (该用户未注册)";
                    break;
                case 7:
                    msg += " (该用户被冻结)";
                    break;
                default:
                    msg += " (其他错误)";
                    break;
            }
            return msg;
        }

        /// <summary>验证网关签名
        /// </summary>
        /// <param name="oCert"></param>
        /// <param name="sSource"></param>
        /// <param name="sSignData"></param>
        /// <returns></returns>
        public static Boolean verifyWGSign(NetcaPkiLib.Certificate oCert, byte[] bSource, string sSignature)
        {
            try
            {
                byte[] bSignature = NetCAPKI.base64Decode(sSignature);
                return oCert.VerifyEx(NetCAPKI.getX509CertificateInfo(oCert, 8).Equals(Constants.NETCAPKI_ALGORITHM_RSA.ToString()) ? NETCAPKI_ALGORITHM_RSASIGN : NETCAPKI_ALGORITHM_SM2SIGN, bSource, bSignature, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}