using System;
using System.Data;
using System.Configuration;

using System.Collections;
using System.Text;
using SecuInter;
using System.Web;
using System.IO;


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
    public class NetCAPKI
    {
        public NetCAPKI()
        {

            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 5.1	常量定义
        public const SECUINTER_STORE_LOCATION SECUINTER_LOCAL_MACHINE_STORE = SECUINTER_STORE_LOCATION.SECUINTER_LOCAL_MACHINE_STORE;
        public const SECUINTER_STORE_LOCATION SECUINTER_CURRENT_USER_STORE = SECUINTER_STORE_LOCATION.SECUINTER_CURRENT_USER_STORE;

        public const SECUINTER_STORE_NAME SECUINTER_MY_STORE = SECUINTER_STORE_NAME.SECUINTER_MY_STORE;
        public const SECUINTER_STORE_NAME SECUINTER_OTHER_STORE = SECUINTER_STORE_NAME.SECUINTER_OTHER_STORE;
        public const SECUINTER_STORE_NAME SECUINTER_CA_STORE = SECUINTER_STORE_NAME.SECUINTER_CA_STORE;
        public const SECUINTER_STORE_NAME SECUINTER_ROOT_STORE = SECUINTER_STORE_NAME.SECUINTER_ROOT_STORE;

        public const int SECUINTER_CERTTYPE_ALL = 0;
        public const int SECUINTER_CERTTYPE_SIGN = 1;
        public const int SECUINTER_CERTTYPE_ENV = 2;

        public const int CATITLE = 21;
        public const int SECUINTER_NETCA_ALL = 0;
        public const int SECUINTER_NETCA_YES = 1;
        public const int SECUINTER_NETCA_NO = 2;//非网证通证书
                                                //定制2：证书获取方式，特定项目需定制
                                                //Device：从设备获取证书,速度更快【推荐】；
                                                //CSP：从获取证书，多CA支持时需采用此方式；
        public static string NETCAPKI_CERTFROM = "Device";
        /*NETCA和其他CA*/
        public const int SECUINTER_NETCA_OTHER = 3;//网证通证书+其他CA证书
        //定制3：默认的证书筛选条件，特定项目需定制
        //多CA支持时需定制，如 { "NETCA", "GDCA", "SZCA","BJCA" }
        public static string[] NETCAPKI_SUPPORTCA = { "NETCA", "GDCA", "SZCA", "BJCA" };

        //定制4：NETCA证书实体唯一标识，特定项目需定制
        //NETCA证书唯一实体标识OID：1.3.6.1.4.1.18760.1.12.11 NETCA证书绑定值OID：1.3.6.1.4.1.18760.1.12.14；
        public static string NETCAPKI_UUID = "1.3.6.1.4.1.18760.1.12.11";

        //定制5：HASH算法，一般无需定制 2017-3-7定制由SHA1改为SHA256
        public static int NETCAPKI_ALGORITHM_HASH = Constants.NETCAPKI_ALGORITHM_SHA256;
        //定制6：RSA签名算法，一般无需定制 2017-3-7定制由SHA1改为SHA256
        public static int NETCAPKI_ALGORITHM_RSASIGN = Constants.NETCAPKI_ALGORITHM_SHA256WITHRSA;
        //定制7：SM2签名算法，一般无需定制
        public static int NETCAPKI_ALGORITHM_SM2SIGN = Constants.NETCAPKI_ALGORITHM_SM3WITHSM2;
        //定制8：RSA加密算法，一般无需定制
        public static int NETCAPKI_ENVELOPEDDATA_ALGORITHM_RSAENV = Constants.NETCAPKI_ENVELOPEDDATA_ALGORITHM_AES256CBC;
        //定制9：SM2加密算法，一般无需定制
        public static int NETCAPKI_ENVELOPEDDATA_ALGORITHM_SM2ENV = Constants.NETCAPKI_ENVELOPEDDATA_ALGORITHM_SM4CBC;
        //定制10：RSA对称加密算法，一般无需定制
        public static int NETCAPKI_ALGORITHM_SYMENV = Constants.NETCAPKI_ALGORITHM_AES_ECB;
        //定制11：签名包含证书的选项，一般无需定制
        public static int NETCAPKI_SIGNEDDATA_INCLUDE_CERT_OPTION = Constants.NETCAPKI_SIGNEDDATA_INCLUDE_CERT_OPTION_SELF;
        //定制12：非对称加密算法 RSA PKCS#1 V1.5加密
        public static int NETCAPKI_ALGORITHM_RSA_PKCS1_V1_5_ENC = Constants.NETCAPKI_ALGORITHM_RSA_PKCS1_V1_5_ENC;
        //定制13：BASE64编码选项定制，设置该项，则编码时不换行
        public static int NETCAPKI_BASE64_ENCODE_NO_NL = Constants.NETCAPKI_BASE64_ENCODE_NO_NL;
        //证书筛选
        public static String[] CASTR = new String[3] { "CN=NETCA", "CN=GDCA", "CN=WGCA" };//根据实际情况修订
        public static String[] CERTVALUEPARSE = new String[61] {
            "证书PEM编码","证书姆印", "证书序列号", "证书主题","证书颁发者主题", "证书有效期起",
            "证书有效期止", "密钥用法", "","用户证书绑定值", "旧的用户证书绑定值",//0~9
            "证书主题名称", "Subject的人名(CN)", "Subject中的单位(O)","Subject中的地址项(L)", "Subject中的Email(E)",
            "Subject中的部门名(OU)", "国家名(C)", "省州名(S)","", "",//10~19
            "CA ID", "证书类型", "证书唯一标识","", "",
            "", "", "","", "",//20~29
            "NETCA:旧姆印信息", "纳税人编码", "企业法人代码","税务登记号", "证书来源地",
            "证件号码信息", "", "","", "",//30~39
            "", "", "","", "",
            "", "", "","", "",//40~49
            "GDCA:信任号TrustID", "", "","", "",
            "", "", "","", "",//50~59
        };//根据实际情况修订



        public const int SECUINTER_SHA1_ALGORITHM = 1;
        public const int SECUINTER_ALGORITHM_RC2 = 0;
        public const int SECUINTER_ALGORITHM_DES = 6;
        public const int SECUINTER_SHA1WithRSA_ALGORITHM = 2;
        public static int NETCAPKI_CP = NetcaPkiLib.Constants.NETCAPKI_CP_UTF8;
        #endregion
        //证书筛选
        static string getCAFilter()
        {
            //(IssuerCN~'NETCA' || IssuerCN~'CFCA' || IssuerCN~'GDCA')
            string filter = "";
            if (NETCAPKI_SUPPORTCA.Length > 0)
            {
                filter += "(";

                for (int j = 0; j < NETCAPKI_SUPPORTCA.Length; j++)
                {
                    if (j == 0)
                    {
                        filter += "IssuerCN~'" + NETCAPKI_SUPPORTCA[j] + "'";
                    }
                    else
                    {
                        filter += "||IssuerCN~'" + NETCAPKI_SUPPORTCA[j] + "'";
                    }
                }
                filter += ")";
            }
            return filter;
        }
        #region 5.2	证书类

        /// <summary>5.2.1 获取证书集 2011-12-19
        /// </summary>
        /// <param name="StoreLocation">SECUINTER_LOCAL_MACHINE_STORE = 0;SECUINTER_CURRENT_USER_STORE= 1;</param>
        /// <param name="StoreName">SECUINTER_MY_STORE=0(个人);SECUINTER_OTHER_STORE:1(其他人);SECUINTER_CA_STORE= 2;SECUINTER_ROOT_STORE= 3;</param>
        /// <param name="certType">SECUINTER_CERTTYPE_ALL= 0;SECUINTER_CERTTYPE_SIGN= 1;SECUINTER_CERTTYPE_ENV= 2;</param>
        /// <param name="netcaType">SECUINTER_NETCA_ALL= 0;SECUINTER_NETCA_YES= 1;SECUINTER_NETCA_NO= 2;SECUINTER_NETCA_OTHER=3</param>
        /// <returns></returns>
        public static SecuInter.X509Certificates getX509Certificates(
            SECUINTER_STORE_LOCATION StoreLocation, SECUINTER_STORE_NAME StoreName, int certType, int netcaType)
        {
            SecuInter.Store oMyStore = new SecuInter.Store();
            SecuInter.X509Certificates oMyCerts = new SecuInter.X509Certificates();
            SecuInter.Utilities oUtil = new Utilities();
            SecuInter.Store oMyStore2 = oUtil.CreateStoreObject();

            try
            {
                oMyStore.Open(StoreLocation, StoreName);
            }
            catch (Exception)
            {
                throw new Exception("打开证书库失败");
            }
            SecuInter.X509Certificates certs = (SecuInter.X509Certificates)oMyStore.X509Certificates;
            oMyStore.Close();
            oMyStore = null;


            IEnumerator oEnum = certs.GetEnumerator();
            while (oEnum.MoveNext())
            {
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)oEnum.Current;

                String issuer = oCert.get_Issuer(SECUINTER_NAMESTRING_TYPE.SECUINTER_X500_NAMESTRING);
                if (certType == SECUINTER_CERTTYPE_ALL)
                {

                    if (netcaType == SECUINTER_NETCA_ALL)
                    {
                        oMyCerts.Add(oCert);
                    }
                    else if (netcaType == SECUINTER_NETCA_YES)
                    {
                        if (issuer.IndexOf("CN=NETCA") >= 0)
                        {
                            oMyCerts.Add(oCert);
                        }
                    }
                    else if (netcaType == SECUINTER_NETCA_NO)
                    {
                        if (issuer.IndexOf("CN=NETCA") < 0)
                        {
                            oMyCerts.Add(oCert);
                        }
                    }
                    //限制可以使用NETCA证书和其他CA证书
                    else if (netcaType == SECUINTER_NETCA_OTHER)
                    {
                        for (int j = 0; j < CASTR.Length; j++)
                        {
                            if (issuer.IndexOf(CASTR[j]) >= 0)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }
                }
                else if (certType == SECUINTER_CERTTYPE_SIGN)
                {

                    if (netcaType == SECUINTER_NETCA_ALL)
                    {
                        if (oCert.KeyUsage == 3)
                        {
                            oMyCerts.Add(oCert);
                        }
                        if (oCert.KeyUsage == -1)
                        {
                            oMyCerts.Add(oCert);
                        }

                    }
                    else if (netcaType == SECUINTER_NETCA_YES)
                    {
                        if (issuer.IndexOf("CN=NETCA") >= 0)
                        {
                            if (oCert.KeyUsage == 3)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }
                    else if (netcaType == SECUINTER_NETCA_NO)
                    {
                        if (issuer.IndexOf("CN=NETCA") < 0)
                        {
                            if (oCert.KeyUsage == 3)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }

                    //限制可以使用NETCA证书和其他CA证书
                    else if (netcaType == SECUINTER_NETCA_OTHER)
                    {
                        for (int j = 0; j < CASTR.Length; j++)
                        {
                            if (issuer.IndexOf(CASTR[j]) >= 0)
                            {
                                if (oCert.KeyUsage == 3)
                                {
                                    oMyCerts.Add(oCert);
                                }
                                if (oCert.KeyUsage == -1)
                                {
                                    oMyCerts.Add(oCert);
                                }
                            }
                        }

                    }

                }
                else if (certType == SECUINTER_CERTTYPE_ENV)
                {

                    if (netcaType == SECUINTER_NETCA_ALL)
                    {
                        if (oCert.KeyUsage == 12)
                        {
                            oMyCerts.Add(oCert);
                        }
                        if (oCert.KeyUsage == -1)
                        {
                            oMyCerts.Add(oCert);
                        }

                    }
                    else if (netcaType == SECUINTER_NETCA_YES)
                    {
                        if (issuer.IndexOf("CN=NETCA") >= 0)
                        {
                            if (oCert.KeyUsage == 12)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }
                    else if (netcaType == SECUINTER_NETCA_NO)
                    {
                        if (issuer.IndexOf("CN=NETCA") < 0)
                        {
                            if (oCert.KeyUsage == 12)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }

                    //限制可以使用NETCA证书和其他CA证书
                    else if (netcaType == SECUINTER_NETCA_OTHER)
                    {
                        for (int j = 0; j < CASTR.Length; j++)
                        {
                            if (issuer.IndexOf(CASTR[j]) >= 0)
                            {
                                if (oCert.KeyUsage == 12)
                                {
                                    oMyCerts.Add(oCert);
                                }
                                if (oCert.KeyUsage == -1)
                                {
                                    oMyCerts.Add(oCert);
                                }
                            }
                        }
                    }
                }
            }//END FOR
            return oMyCerts;

        }

        /// <summary>5.2.2 获取证书对象 2011-12-19
        /// 
        /// </summary>
        /// <param name="StoreLocation"></param>
        /// <param name="StoreName"></param>
        /// <param name="certType"></param>
        /// <param name="netcaType"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(
            SECUINTER_STORE_LOCATION StoreLocation, SECUINTER_STORE_NAME StoreName, int certType, int netcaType)
        {
            SecuInter.X509Certificates oMyCerts = getX509Certificates(StoreLocation, StoreName, certType, netcaType);
            if (oMyCerts == null)
            {
                return null;
            }
            if (oMyCerts.Count > 0)
            {
                return (SecuInter.X509Certificate)oMyCerts.SelectCertificate();
            }
            return null;

        }

        /// <summary>5.3.2 [常用]获取证书对象
        /// 使用频率：较常用；
        /// 使用场景：
        /// 选择证书通常采用此函数。1）证书绑定时，2）证书登录时；
        /// 根据全局变量定制项2、3，可通过该函数支持多CA支持；
        /// 2016-07-22 luhanmin 修订入参
        /// </summary>
        /// <param name="NETCAPKI_CERT_PURPOSE">证书用途,参见Constants.NETCAPKI_CERT_PURPOSE定义；0：所有证书;NETCAPKI_CERT_PURPOSE_SIGN=2;NETCAPKI_CERT_PURPOSE_ENCRYPT= 1;</param>
        /// <returns></returns>
        public static NetcaPkiLib.Certificate getX509Certificate(int NETCAPKI_CERT_PURPOSE)
        {
            try
            {
                NetcaPkiLib.Certificate oCertificate = new NetcaPkiLib.Certificate();
                #region Type
                string sType = "{";
                sType += "\"UIFlag\":\"default\", \"InValidity\":true,";

                if (NETCAPKI_CERT_PURPOSE == Constants.NETCAPKI_CERT_PURPOSE_SIGN)
                {
                    sType += "\"Type\":\"signature\" , ";
                }
                else if (NETCAPKI_CERT_PURPOSE == Constants.NETCAPKI_CERT_PURPOSE_ENCRYPT)
                {
                    sType += "\"Type\":\"encrypt\" , ";
                }

                if (NETCAPKI_CERTFROM.Equals("Device"))
                {
                    sType += "\"Method\":\"device\", \"Value\":\"any\"";
                }
                else
                {
                    sType += "\"Method\":\"store\", \"Value\":{\"Type\":\"current user\",\"Value\":\"my\"}";
                }
                sType += "}";
                #endregion

                #region filter
                string sFilter = "";
                sFilter = "InValidity='True'";
                if (!string.IsNullOrEmpty(getCAFilter()))
                {
                    sFilter += "&&" + getCAFilter();
                }
                if (NETCAPKI_CERT_PURPOSE == Constants.NETCAPKI_CERT_PURPOSE_SIGN)
                {
                    sFilter += "&&CertType='Signature'&&CheckPrivKey='True'";
                }
                else if (NETCAPKI_CERT_PURPOSE == Constants.NETCAPKI_CERT_PURPOSE_ENCRYPT)
                {
                    sFilter += "&&CertType='Encrypt'&&CheckPrivKey='True'";
                }
                #endregion
                oCertificate.Select(sType, sFilter);
                return oCertificate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>5.2.3 根据证书字符串获取证书
        /// 
        /// </summary>
        /// <param name="sX509Certificate"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(string sX509Certificate)
        {
            SecuInter.X509Certificate oX509Certificate = new SecuInter.X509Certificate();
            oX509Certificate.Decode(sX509Certificate);
            return oX509Certificate;
        }
        /// <summary>5.3.2.1 [常用]获取证书对象（从证书BASE64编码信息中）
        /// 使用频率：较常用
        /// </summary>
        /// <param name="sCertificate"></param>
        /// <returns></returns>
        public static NetcaPkiLib.Certificate getX509CertificateNew(string sCertificate)
        {
            try
            {
                sCertificate = clearCertHeader(sCertificate);
                NetcaPkiLib.Certificate oX509Certificate = new NetcaPkiLib.Certificate();
                //Certificate oX509Certificate = new Certificate();
                oX509Certificate.Decode(sCertificate);
                return oX509Certificate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>5.2.4	根据特定域的值，获取证书对象 2011-12-19
        /// 
        /// </summary>
        /// <param name="StoreLocation"></param>
        /// <param name="StoreName"></param>
        /// <param name="certType"></param>
        /// <param name="netcaType"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(
            SECUINTER_STORE_LOCATION StoreLocation, SECUINTER_STORE_NAME StoreName, int certType, int netcaType,
            int iValueType, String certValue)
        {
            SecuInter.X509Certificates oMyCerts = getX509Certificates(StoreLocation, StoreName, certType, netcaType);
            if (oMyCerts == null)
            {
                return null;
            }
            if (oMyCerts.Count > 0)
            {
                IEnumerator oEnum = oMyCerts.GetEnumerator();
                while (oEnum.MoveNext())
                {
                    SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)oEnum.Current;
                    if (getX509CertificateInfo(oCert, iValueType).Equals(certValue))
                    {
                        return oCert;
                    }
                }
            }
            return null;
        }

        /// <summary>5.2.5	获取证书信息*** 2012-10-29 Update
        /// 
        /// </summary>
        /// <param name="oCert"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String getX509CertificateInfo(SecuInter.X509Certificate oCert, int type)
        {
            if (oCert == null)
            {
                throw new Exception("证书为空!");
            }
            if (type == 0)//获取证书BASE64格式编码字符串 2012-12-03 modify luhanmin
            {
                String certHeader = "-----BEGIN CERTIFICATE-----\r\n";
                String certEnd = "-----END CERTIFICATE-----\r\n";
                String certPem = oCert.get_Encoded(SECUINTER_CERT_ENCODE_TYPE.SECUINTER_CERT_ENCODE_PEM).ToString();
                if (certPem.IndexOf(certHeader) >= 0)
                {
                    certPem = certPem.Substring(certHeader.Length, certPem.Length - certHeader.Length);
                    certPem = certPem.Substring(0, certPem.Length - certEnd.Length);
                }
                return certPem;
            }
            if (type == 1)//证书姆印
            {
                SecuInter.Utilities oUtil = new SecuInter.Utilities();
                return oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM)).ToUpper();
            }
            else if (type == 2)//证书序列号
            {
                return oCert.SerialNumber;
            }
            else if (type == 3)//证书Subject
            {
                return oCert.get_Subject(SECUINTER_NAMESTRING_TYPE.SECUINTER_X500_NAMESTRING);

            }
            else if (type == 4)//证书颁发者Subject
            {
                return oCert.get_Issuer(SECUINTER_NAMESTRING_TYPE.SECUINTER_X500_NAMESTRING);

            }
            else if (type == 5)//证书有效期起
            {
                return oCert.ValidFromDate.ToString();

            }
            else if (type == 6)//证书有效期止
            {
                return oCert.ValidToDate.ToString();

            }
            else if (type == 7)//KeyUsage 密钥用法
            {
                return oCert.KeyUsage.ToString();
            }
            else if (type == 9)//UsrCertNO：证书绑定值；(系统改造时,建议采用该值)
            {
                if (getX509CertificateInfo(oCert, 21).Equals("1"))
                {
                    String rt = getX509CertificateInfo(oCert, 23);//取证书唯一标识
                    if (String.IsNullOrEmpty(rt))
                    {
                        rt = getX509CertificateInfo(oCert, 36);//取证书证件号码扩展域信息
                        if (String.IsNullOrEmpty(rt))
                        {
                            rt = getX509CertificateInfo(oCert, 1);//取证书姆印
                        }
                    }
                    return rt;
                }
                if (getX509CertificateInfo(oCert, 21).Equals("2"))
                {
                    return getX509CertificateInfo(oCert, 51);
                }
            }
            else if (type == 10)//OldUsrCertNo：旧的用户证书绑定值；(证书更新后的原有9的取值)
            {
                if (getX509CertificateInfo(oCert, 21).Equals("1"))
                {
                    String rt = getX509CertificateInfo(oCert, 23);//取证书唯一标识
                    if (String.IsNullOrEmpty(rt))
                    {
                        rt = getX509CertificateInfo(oCert, 36);//取证书证件号码扩展域信息
                        if (String.IsNullOrEmpty(rt))
                        {
                            rt = getX509CertificateInfo(oCert, 31);//取证书旧姆印
                        }
                    }
                    return rt;
                }
                if (getX509CertificateInfo(oCert, 21).Equals("2"))
                {
                    return getX509CertificateInfo(oCert, 51);
                }
            }
            else if (type == 11)//证书主题名称；有CN项取CN项值；无CN项，取O的值
            {
                if (String.IsNullOrEmpty(getX509CertificateInfo(oCert, 12)))
                {
                    return getX509CertificateInfo(oCert, 13);
                }
                else
                {
                    return getX509CertificateInfo(oCert, 12);
                }
            }
            else if (type == 12)//Subject中的CN项（人名）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "CN");
                //return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_SUBJECT_SIMPLE_NAME);
            }
            else if (type == 13)//Subject中的O项（人名）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "O");

            }
            else if (type == 14)//Subject中的地址（L项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "L");
            }
            else if (type == 15)//证书颁发者的Email
            {
                return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_SUBJECT_EMAIL);
            }
            else if (type == 16)//Subject中的部门名（OU项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "OU");
            }
            else if (type == 17)//用户国家名（C项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                // oCert.GetUTF8ExtValue(
                return parseDN(subject, "C");
            }
            else if (type == 18)//用户省州名（S项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "S");
            }

            else if (type == 21)//CA ID
            {
                for (int i = 0; i < CASTR.Length; i++)
                {
                    if (getX509CertificateInfo(oCert, 4).IndexOf(CASTR[i]) > 0)
                    {
                        return "" + (i + 1);
                    }
                }
                return "0";
            }
            else if (type == 22)//证书类型
            {

                return "0";
            }
            else if (type == 23)//证书唯一标识(一般为客户号等)
            {
                if (getX509CertificateInfo(oCert, 21).Equals("1"))
                {
                    return "";
                }
                if (getX509CertificateInfo(oCert, 21).Equals("2"))
                {
                    return getX509CertificateInfo(oCert, 51);
                }
            }
            else if (type == 31)//证书旧姆印
            {
                try
                {
                    SecuInter.Utilities oUtil = new SecuInter.Utilities();
                    return oUtil.BinaryToHex(oCert.get_PrevCertThumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM)).ToUpper();
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 32)//纳税人编码
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_TAXPAYERID);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 33)//组织机构代码号
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_ORGANIZATIONCODE);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 34)//税务登记号
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_TAXATIONNUMBER);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 35)//证书来源地
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_CERTSOURCE);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 36)//证书证件号码扩展域
            {
                try
                {
                    //注意选择不同项目
                    //第1个表达式为 NETCA通用定义OID
                    //第1个表达式为 深圳项目中采用（3家CA都采用此做唯一标识）   2.16.156.112548
                    String rt = oCert.GetUTF8ExtValue("1.3.6.1.4.1.18760.1.12.11");
                    //String rt = oCert.GetUTF8ExtValue("2.16.156.112548");
                    return rt;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 51)//GDCA 证书信任号
            {
                try
                {
                    return "GDCA 未实现";
                    //return oCert.GetUTF8ExtValue("1.2.156.0.2.1");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return "";
        }

        /// <summary>5.3.4 [常用]获取证书属性信息
        /// 使用频率：较常用，多CA支持时，需定制；
        /// 注：第9项值一般作为证书的绑定值，该项值为一个复合值；
        /// 2016-07-22 luhanmin 修订取值信息
        /// 2016-12-22 qiubingfa 修订第9项证书绑定值：取值逻辑，用户证书服务号->实体唯一标识->证书姆印
        /// </summary>
        /// <param name="oCert"></param>
        /// <param name="iInfoType"></param>
        /// <returns></returns>
        public static string getX509CertificateInfo(NetcaPkiLib.Certificate oCert, int iInfoType)
        {
            if (oCert == null)
            {
                throw new Exception("证书信息为空!");
            }
            if (iInfoType == 0)//获取证书BASE64格式编码字符串 2012-12-03 modify luhanmin
            {
                string certPem = oCert.Encode(2).ToString();
                certPem = clearCertHeader(certPem);
                return certPem;
            }

            #region 证书基本信息1-8
            if (iInfoType == 1)//证书姆印
            {
                return convertHex(oCert.get_ThumbPrint(Constants.NETCAPKI_ALGORITHM_SHA1)).ToUpper();
            }
            else if (iInfoType == 2)//证书序列号
            {
                return oCert.SerialNumber;
            }
            else if (iInfoType == 3)//证书Subject
            {
                return oCert.Subject;
            }
            else if (iInfoType == 4)//证书颁发者Subject
            {
                return oCert.Issuer;
            }
            else if (iInfoType == 5)//证书有效期起
            {
                return oCert.ValidFromDate.ToString();
            }
            else if (iInfoType == 6)//证书有效期止
            {
                return oCert.ValidToDate.ToString();
            }
            else if (iInfoType == 7)//KeyUsage 密钥用法
            {
                return oCert.KeyUsage.ToString();
            }
            else if (iInfoType == 8)//证书的公钥的算法
            {
                return oCert.PublicKeyAlgorithm.ToString();
            }
            #endregion

            else if (iInfoType == 9)
            {
                //获取顺序：1）证书唯一标识；2）证书客户服务号，3）证书姆印
                #region UsrCertNO：证书绑定值；(系统改造时,建议采用该值)

                //1) 取证书证件号码扩展域信息
                string rt = getX509CertificateInfo(oCert, 36);
                if (!String.IsNullOrEmpty(rt))
                {
                    return rt;
                }

                //2) 获取证书客户服务号
                rt = getX509CertificateInfo(oCert, 23);
                if (!String.IsNullOrEmpty(rt))
                {
                    return rt;
                }

                //3）证书姆印
                rt = getX509CertificateInfo(oCert, 1);
                if (!String.IsNullOrEmpty(rt))
                {
                    return rt;
                }
                return "";
                #endregion
            }

            else if (iInfoType == 10)//OldUsrCertNo：旧的用户证书绑定值；(证书更新后的原有9的取值)
            {
                if (getX509CertificateInfo(oCert, CATITLE).Equals("NETCA"))
                {
                    return getX509CertificateInfo(oCert, 31);//取证书旧姆印
                }
            }

            #region Subject中信息（11~18）
            else if (iInfoType == 11)//证书主题名称；有CN项取CN项值；无CN项，取O的值
            {
                if (String.IsNullOrEmpty(getX509CertificateInfo(oCert, 12)))
                {
                    return getX509CertificateInfo(oCert, 13);
                }
                else
                {
                    return getX509CertificateInfo(oCert, 12);
                }
            }
            else if (iInfoType == 12)//Subject中的CN项（人名）
            {
                try
                {
                    return oCert.GetStringInfo(20);
                }
                catch
                {
                    return "";
                }

            }
            else if (iInfoType == 13)//Subject中的O项（人名）
            {
                try
                {
                    return oCert.GetStringInfo(18);
                }
                catch
                {
                    return "";
                }
            }
            else if (iInfoType == 14)//Subject中的地址（L项）
            {
                try
                {
                    return oCert.GetStringInfo(40);
                }
                catch
                {
                    return "";
                }
            }
            else if (iInfoType == 15)//证书颁发者的Email
            {
                try
                {
                    return oCert.GetStringInfo(21);
                }
                catch
                {
                    return "";
                }

            }
            else if (iInfoType == 16)//Subject中的部门名（OU项）
            {
                try
                {
                    return oCert.GetStringInfo(19);
                }
                catch
                {
                    return "";
                }
            }
            else if (iInfoType == 17)//用户国家名（C项）
            {
                try
                {
                    return oCert.GetStringInfo(17);
                }
                catch
                {
                    return "";
                }
            }
            else if (iInfoType == 18)//用户省州名（S项）
            {
                try
                {
                    return oCert.GetStringInfo(39);
                }
                catch
                {
                    return "";
                }
            }
            #endregion

            else if (iInfoType == CATITLE)//CA ID
            {

                for (int i = 0; i < NETCAPKI_SUPPORTCA.Length; i++)
                {
                    if (getX509CertificateInfo(oCert, 4).IndexOf(NETCAPKI_SUPPORTCA[i]) > 0)
                    {
                        return NETCAPKI_SUPPORTCA[i];
                    }
                }
                return "";
            }
            else if (iInfoType == 22)
            {
                #region 证书类型
                if (getX509CertificateInfo(oCert, CATITLE).Equals("NETCA"))
                {
                    try
                    {
                        NetcaPkiLib.Utilities oUtil = new NetcaPkiLib.Utilities();
                        //netca证书类型扩展OID:NETCA OID(1.3.6.1.4.1.18760.1.12.12.2)
                        //1：服务器证书;2：个人证书;3: 机构证书;4：机构员工证书;5：机构业务证书(注：该类型国密标准待定);0：其他证书
                        string netcaCaType = oUtil.DecodeASN1String(1, oCert.GetExtension("1.3.6.1.4.1.18760.1.12.12.2"));

                        #region 有扩展域情况
                        if (netcaCaType.Equals("001"))
                        {
                            return "3";
                        }
                        else if (netcaCaType.Equals("002"))
                        {
                            return "5";
                        }
                        else if (netcaCaType.Equals("003"))
                        {
                            return "4";
                        }
                        else if (netcaCaType.Equals("004"))
                        {
                            return "2";
                        }
                        #endregion
                    }
                    catch
                    {
                        #region 根据CN项和O项判断
                        string sCN = getX509CertificateInfo(oCert, 12);
                        string sO = getX509CertificateInfo(oCert, 13);
                        if (!string.IsNullOrEmpty(sCN) && string.IsNullOrEmpty(sO))
                        {
                            return "2";
                        }
                        else if ((!string.IsNullOrEmpty(sO) && string.IsNullOrEmpty(sCN)) ||
                            (!string.IsNullOrEmpty(sO) && !string.IsNullOrEmpty(sCN) && sO.Equals(sCN))
                            )
                        {
                            return "3";
                        }
                        else if (
                           !string.IsNullOrEmpty(sO) && !string.IsNullOrEmpty(sCN) && !sO.Equals(sCN))
                        {
                            return "4";
                        }
                        #endregion
                    }
                    return "0";
                }
                else
                    return "0";
                #endregion
            }
            else if (iInfoType == 23)//用户证书客服号 
            {
                if (getX509CertificateInfo(oCert, CATITLE).Equals("NETCA"))
                {
                    try
                    {
                        NetcaPkiLib.Utilities oUtil = new NetcaPkiLib.Utilities();
                        //netca 用户证书服务号OID
                        string rt = oUtil.DecodeASN1String(1, oCert.GetExtension("1.3.6.1.4.1.18760.1.14"));
                        return rt;
                    }
                    catch
                    {
                        return "";
                    }
                }
                else if (getX509CertificateInfo(oCert, CATITLE).Equals("GDCA"))
                {
                    return getX509CertificateInfo(oCert, 51);
                }
                else
                {
                    return "";
                }
            }
            else if (iInfoType == 24)//深圳地标
            {
                try
                {
                    NetcaPkiLib.Utilities oUtil = new NetcaPkiLib.Utilities();
                    //深圳地标
                    string rt = oUtil.DecodeASN1String(1, oCert.GetExtension("2.16.156.112548"));
                    return rt;
                }
                catch
                {
                    return "";
                }
            }
            #region NETCA的特定扩展域 31~49
            else if (iInfoType == 31)//证书旧姆印
            {
                try
                {
                    return convertHex(oCert.GetStringInfo(29));
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (iInfoType == 32)//纳税人编码
            {
                try
                {
                    return "";// oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_TAXPAYERID);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (iInfoType == 33)//组织机构代码号
            {
                try
                {
                    return "";//oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_ORGANIZATIONCODE);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (iInfoType == 34)//税务登记号
            {
                try
                {
                    return "";//oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_TAXATIONNUMBER);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (iInfoType == 35)//证书来源地
            {
                try
                {
                    return "";//oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_CERTSOURCE);
                }
                catch (Exception)
                {
                    return "";
                }
            }

            else if (iInfoType == 36)//证书证件号码扩展域
            {
                try
                {
                    NetcaPkiLib.Utilities oUtil = new NetcaPkiLib.Utilities();
                    //netca证书类型扩展OID
                    string rt = oUtil.DecodeASN1String(1, oCert.GetExtension(NETCAPKI_UUID));
                    return rt;
                }
                catch (Exception)
                {
                    return "";
                }
            }

            else if (iInfoType == 37)//证书证件号码扩展域
            {
                #region 证件号码解码
                string rt = getX509CertificateInfo(oCert, 36);
                if (rt.Length > 13)
                {//00011@0006PO1MTIzNDU2Nzg5MDEyMzQ1Njc4
                    int beginIndex = rt.IndexOf('@');
                    if (beginIndex == -1)
                    {
                        //alert("获取证件号码失败：无法定位@");
                        return "";
                    }
                    string iClassify = rt.Substring(beginIndex + 7, 1); //获取编码标志位
                    if (iClassify.Equals("1"))
                    {
                        NetcaPkiLib.Utilities oUtil = new NetcaPkiLib.Utilities();
                        string b64Identity = rt.Substring(beginIndex + 8);
                        object bIdentity = base64Decode(b64Identity);
                        return oUtil.Decode(bIdentity, 65001);
                    }
                    else if (iClassify == "0")
                    {
                        return rt.Substring(beginIndex + 8);
                    }
                    return "";
                }
                else
                {
                    return "";
                }
                #endregion
            }

            #endregion
            #region GDCA的特定扩展域 51
            else if (iInfoType == 51)//GDCA 证书信任号
            {
                string oid = "1.2.86.21.1.3";
                if (getX509CertificateInfo(oCert, CATITLE).Equals("GDCA"))
                {
                    try
                    {
                        NetcaPkiLib.Utilities oUtil = new NetcaPkiLib.Utilities();
                        string rt = oUtil.Decode(oCert.GetExtension(oid), 65001);
                        return rt;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
            #endregion
            return "";
        }


        /// <summary>5.2.5.1 获取证书姆印 2011-12-19
        /// 
        /// </summary>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static String getX509CertificateThumbprint(SecuInter.X509Certificate oCert)
        {
            if (oCert == null)
            {
                throw new Exception("证书为空!");
            }
            SecuInter.Utilities oUtil = new SecuInter.Utilities();
            return oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM)).ToUpper();
        }

        /// <summary>5.2.6	获取证书特定扩展域信息
        /// 
        /// </summary>
        /// <param name="oCert"></param>
        /// <param name="OID"></param>
        /// <returns>UTF8编码</returns>
        public static String getX509CertificateInfo(SecuInter.X509Certificate oCert, String OID)
        {
            return oCert.GetUTF8ExtValue(OID);
        }

        /// <summary>5.2.7	从HTTPS通信中获取证书对象（SSL用）
        /// HttpClientCertificate hCert = Request.ClientCertificate;
        /// byte[] bCert=hCert.Certificate;
        /// </summary>
        /// <param name="hCert"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(byte[] bCert)
        {
            SecuInter.X509Certificate oCert = new SecuInter.X509Certificate();
            try
            {
                oCert.Decode(bCert);
                return oCert;
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>5.2.8	获取服务器证书
        /// 注意：服务器端使用
        /// 
        /// </summary>
        /// <param name="subject">证书主题中CN项值</param>
        /// <returns></returns>
        public static String getServerCert(String subject)
        {

            SecuInter.X509Certificate oCert = getX509Certificate(
            NetCAPKI.SECUINTER_LOCAL_MACHINE_STORE, NetCAPKI.SECUINTER_MY_STORE,
            NetCAPKI.SECUINTER_CERTTYPE_ALL,
            NetCAPKI.SECUINTER_NETCA_ALL, 11,
            subject);
            return getX509CertificateInfo(oCert, 0);

        }

        #endregion

        #region 5.3	签名类

        /// <summary>5.3.1 带PIN码PKCS7签名(对应以前的sign函数) 2011-12-19 **
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="isNotHasSource"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static String signPKCS7ByPwd(String sSource, Boolean isNotHasSource, String pwd)
        {
            SecuInter.X509Certificate oCert = getX509Certificate(SECUINTER_STORE_LOCATION.SECUINTER_CURRENT_USER_STORE, SECUINTER_STORE_NAME.SECUINTER_MY_STORE, NetCAPKI.SECUINTER_CERTTYPE_SIGN, NetCAPKI.SECUINTER_NETCA_YES);
            if (oCert == null)
            {
                throw new Exception("未选择证书,请检查是否插入密钥!");
            }
            return signPKCS7ByCertificate(sSource, isNotHasSource, pwd, oCert);
        }

        /// <summary>5.3.2 使用证书进行PKCS7签名 2011-12-19  
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="isNotHasSource"></param>
        /// <param name="pwd"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static String signPKCS7ByCertificate(String sSource, Boolean isNotHasSource, String pwd, SecuInter.X509Certificate oCert)
        {
            SecuInter.Signer oSigner = new SecuInter.Signer();
            SecuInter.SignedData oSignedData = new SecuInter.SignedData();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();
            if (sSource == "")
            {
                throw new Exception("原文内容为空!");
            }


            oSigner.Certificate = oCert;
            oSigner.HashAlgorithm = SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM;
            oSigner.UseSigningCertificateAttribute = false;
            oSigner.UseSigningTime = false;
            if (!String.IsNullOrEmpty(pwd))
            {
                bool ok = oSigner.SetUserPIN(pwd);
                if (!ok)
                {
                    throw new Exception("密码有误！");

                }
            }
            oSignedData.Content = sSource;
            oSignedData.Detached = isNotHasSource;

            object arrRT = oSignedData.Sign(oSigner, SECUINTER_CMS_ENCODE_TYPE.SECUINTER_CMS_ENCODE_BASE64);
            oSignedData = null;
            oSigner = null;
            return arrRT.ToString();
        }

        /// <summary>5.3.3 PKCS7签名(常用,兼容以前代码) 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="isNotHasSource"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static String signNETCA(String sSource, Boolean isNotHasSource, String pwd)
        {
            return signPKCS7ByPwd(sSource, isNotHasSource, "");
        }
        #endregion
        #region 5.4 签名类
        /// <summary>5.4.1 [常用]PKCS7签名
        /// 使用场景：较常用，兼容以前代码改造
        /// </summary>
        /// <param name="sSource">待签名信息</param>
        /// <param name="bNoHasSource">true：不带原文;false:带原文</param>
        /// <param name="sUsbKeyPwd">用户证书PIN码</param>
        /// <returns>PKCS7签名信息</returns>
        public static string signNETCA2(string sSource, bool bNoHasSource = true, string sUsbKeyPwd = "")
        {
            return signedDataByPwd2(sSource, bNoHasSource, sUsbKeyPwd);
        }

        /// <summary>5.4.2 带PIN码PKCS7签名
        /// 使用频率：少用，一般用signNETCA;
        /// </summary>
        /// <param name="sSource">待签名信息</param>
        /// <param name="bNoHasSource">true：不带原文;false:带原文</param>
        /// <param name="sUsbKeyPwd">用户证书PIN码</param>
        /// <returns>PKCS7签名信息</returns>
        public static string signedDataByPwd2(string sSource, bool bNoHasSource, string sUsbKeyPwd = "")
        {
            try
            {
                byte[] tbs = convertByte(sSource);
                return signedDataByPwd2(tbs, bNoHasSource, sUsbKeyPwd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>5.4.2.1 带PIN码PKCS7签名（二进制）
        /// </summary>
        /// <param name="bSource">待签名信息</param>
        /// <param name="bNoHasSource">true：不带原文;false:带原文</param>
        /// <param name="sUsbKeyPwd">用户证书PIN码</param>
        /// <returns>PKCS7签名信息</returns>
        /// <returns></returns>
        public static string signedDataByPwd2(byte[] bSource, bool bNoHasSource, string sUsbKeyPwd = "")
        {
            try
            {
                NetcaPkiLib.Certificate oCert = getX509Certificate(Constants.NETCAPKI_CERT_PURPOSE_SIGN);
                if (oCert == null)
                {
                    throw new Exception("未选择证书,请检查是否插入密钥!");
                }
                return signedDataByCertificate(oCert, bSource, bNoHasSource, sUsbKeyPwd);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>5.4.3.1 使用证书进行PKCS7签名[具体代码实现]
        /// </summary>
        /// <param name="bSource"></param>
        /// <param name="bNoHasSource">true：不带原文;false:带原文</param>
        /// <param name="sUsbKeyPwd"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static string signedDataByCertificate(NetcaPkiLib.Certificate oCert, byte[] bSource, bool bNoHasSource, string sUsbKeyPwd = "")
        {
            try
            {
                NetcaPkiLib.SignedData oSignedData = new NetcaPkiLib.SignedData();
                if (oSignedData.SetSignCertificate(oCert, sUsbKeyPwd, false) == false)
                {
                    throw new Exception("设置签名证书失败");
                }
                oSignedData.SetSignAlgorithm(-1, getX509CertificateInfo(oCert, 8).Equals(Constants.NETCAPKI_ALGORITHM_RSA.ToString()) ? NETCAPKI_ALGORITHM_RSASIGN : NETCAPKI_ALGORITHM_SM2SIGN);
                oSignedData.SetIncludeCertificateOption(NETCAPKI_SIGNEDDATA_INCLUDE_CERT_OPTION);
                oSignedData.Detached = bNoHasSource == false ? false : true;// true：不带原文；false：带原文
                object arrRT = oSignedData.Sign(bSource, Constants.NETCAPKI_CMS_ENCODE_BASE64);
                oSignedData = null;
                return arrRT.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>5.3.4 PKCS7签名验证并获取证书 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="sSignature"></param>
        /// <param name="isNotHasSource"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate verifyPKCS7(String sSource, string sSignature, Boolean isNotHasSource)
        {
            SecuInter.X509Certificate oCertSign = null;
            SignedData signedData = new SignedData();
            Utilities util = new Utilities();
            if (isNotHasSource == true)
            {//不含原文情况,将原文设入签名数据中
                signedData.Content = sSource;
            }

            if (!signedData.Verify(sSignature, SecuInter.SECUINTER_SIGNEDDATA_VERIFY_FLAG.SECUINTER_SIGNEDDATA_VERIFY_SIGNATURE_ONLY))
            {
                throw new Exception("签名验证不正确");
            }
            if (isNotHasSource == false)
            {//含原文情况,比对原文和签名信息,进行验证
                if (!sSource.Equals(util.ByteArraytoString(signedData.Content)))//
                {
                    throw new Exception("发生错误,签名原文不一致!");
                }
            }
            // '判断验证结果与签名时数据是否一致
            SecuInter.Signers signers = signedData.Signers;
            IEnumerator enumer = signers.GetEnumerator();

            while (enumer.MoveNext()) //第一张证书为客户端签名证书
            {
                SecuInter.Signer signer = (SecuInter.Signer)enumer.Current;
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)signer.Certificate;

                oCertSign = oCert; //'验证通过，取签名的证书
                break;
            }
            if (oCertSign == null)
            {
                throw new Exception("签名信息中无证书!");
            }
            signedData = null;
            util = null;
            return oCertSign;
        }

        /// <summary>5.3.5 带原文PKCS7签名,验证并获取原文 2011-12-19  
        /// 含原文签名情况下使用
        /// </summary>
        /// <param name="sSignature"></param>
        /// <returns></returns>
        public static String getSourceFromPKCS7SignData(string sSignature)
        {
            String sSource = "";
            SignedData oSignedData = new SignedData();
            Utilities oUtilities = new Utilities();
            if (!oSignedData.Verify(sSignature, SecuInter.SECUINTER_SIGNEDDATA_VERIFY_FLAG.SECUINTER_SIGNEDDATA_VERIFY_SIGNATURE_ONLY))
            {
                throw new Exception("签名验证不正确");
            }
            SecuInter.Signers signers = oSignedData.Signers;
            IEnumerator enumer = signers.GetEnumerator();

            while (enumer.MoveNext()) //第一张证书为客户端签名证书
            {
                SecuInter.Signer signer = (SecuInter.Signer)enumer.Current;
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)signer.Certificate;
                oCert.Display();
            }
            sSource = oUtilities.ByteArraytoString(oSignedData.Content);
            oSignedData = null;
            oUtilities = null;
            return sSource;
        }

        /// <summary>5.3.6 PKCS1签名 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static String signPKCS1ByCert(String sSource, SecuInter.X509Certificate oCert)
        {
            SecuInter.signature oSignature = new SecuInter.signature();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();


            oSignature.Certificate = oCert;
            oSignature.Algorithm = SECUINTER_SIGNATURE_ALGORITHM.SECUINTER_SHA1WithRSA_ALGORITHM;
            object arrRT = oSignature.Sign(sSource);

            String rt = oUtil.Base64Encode(arrRT);
            oSignature = null;
            oUtil = null;
            return rt;
        }

        /// <summary>5.3.7 PKCS1签名验证 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="bSignData"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static Boolean verifyPKCS1(String sSource, String bSignData, SecuInter.X509Certificate oCert)
        {
            Boolean isOK = false;

            if (oCert == null)
            {
                throw new Exception("未选择证书!");
            }
            SecuInter.signature oSignature = new SecuInter.signature();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();

            oSignature.Certificate = oCert;
            oSignature.Algorithm = SECUINTER_SIGNATURE_ALGORITHM.SECUINTER_SHA1WithRSA_ALGORITHM;

            if (oSignature.Verify(sSource, oUtil.Base64Decode(bSignData)))
            {
                isOK = true;
            }
            else
            {
                throw new Exception("验证不通过!");
            }

            oSignature = null;
            oUtil = null;
            return isOK;
        }

        /// <summary>5.3.7	PKCS#1签名验证
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="bSignData"></param>
        /// <param name="sX509Certificate"></param>
        /// <returns></returns>
        public static Boolean verifyPKCS1(String sSource, String bSignData, String sX509Certificate)
        {
            SecuInter.X509Certificate oCert = new SecuInter.X509Certificate();
            oCert.Decode(sX509Certificate);

            Boolean isOK = false;

            if (oCert == null)
            {
                throw new Exception("未选择证书!");
            }
            SecuInter.signature oSignature = new SecuInter.signature();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();

            oSignature.Certificate = oCert;
            oSignature.Algorithm = SECUINTER_SIGNATURE_ALGORITHM.SECUINTER_SHA1WithRSA_ALGORITHM;
            byte[] bContent = Encoding.Default.GetBytes(sSource);
            if (oSignature.Verify(bContent, oUtil.Base64Decode(bSignData)))
            {
                isOK = true;
            }
            else
            {
                throw new Exception("验证不通过!");
            }

            oSignature = null;
            oUtil = null;

            return isOK;
        }

        /// <summary>
        /// PKCS#7时间戳签名
        /// </summary>
        /// <param name="bContent">签名内容</param>
        /// <param name="tsaUrl">时间戳服务器URL</param>
        /// <param name="IsNotHasSource"></param>
        /// <returns>签名值</returns>
        public static String signPKCS7WithTSA(String bContent, String tsaUrl, Boolean IsNotHasSource)
        {
            if (bContent == "")
            {
                throw new Exception("原文内容为空!");

            }
            if (tsaUrl == "")
            {
                throw new Exception("时间戳URL为空!");
            }

            SecuInter.X509Certificate oCert = getX509Certificate(SECUINTER_CURRENT_USER_STORE, SECUINTER_MY_STORE, SECUINTER_CERTTYPE_SIGN, SECUINTER_NETCA_OTHER);
            if (oCert == null)
            {
                throw new Exception("未选择证书!");
            }

            SecuInter.Signer oSigner = new SecuInter.Signer();
            SecuInter.SignedData oSignedData = new SecuInter.SignedData();
            SecuInter.X509Certificate oX509Certificate = new SecuInter.X509Certificate();
            //oX509Certificate = oCert;         
            oSigner.Certificate = oCert;
            oSigner.HashAlgorithm = SecuInter.SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM;
            oSigner.UseSigningCertificateAttribute = false;
            oSigner.UseSigningTime = true;
            oSignedData.Content = bContent;
            oSignedData.Detached = IsNotHasSource;

            Object arrRT = oSignedData.SignWithTSATimeStamp(oSigner, tsaUrl, "", oX509Certificate, SECUINTER_CMS_ENCODE_TYPE.SECUINTER_CMS_ENCODE_BASE64);
            oSignedData = null;
            oSigner = null;
            oCert = null;
            oX509Certificate = null;
            return arrRT.ToString();
        }
        #endregion
        /// <summary>5.3.4 PKCS7签名验证并获取证书 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="sSignature"></param>
        /// <param name="isNotHasSource"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate verifyPKCS7(String sSource, string sSignature, Boolean isNotHasSource, ref String signTime)
        {
            SecuInter.X509Certificate oCertSign = null;
            SignedData signedData = new SignedData();
            Utilities util = new Utilities();

            if (isNotHasSource == true)
            {//不含原文情况,将原文设入签名数据中
                signedData.Content = sSource;
            }

            if (!signedData.Verify(sSignature, SecuInter.SECUINTER_SIGNEDDATA_VERIFY_FLAG.SECUINTER_SIGNEDDATA_VERIFY_SIGNATURE_ONLY))
            {
                throw new Exception("签名验证不正确");
            }
            if (isNotHasSource == false)
            {//含原文情况,比对原文和签名信息,进行验证
                if (!sSource.Equals(util.ByteArraytoString(signedData.Content)))//
                {
                    throw new Exception("发生错误,签名原文不一致!");
                }
            }
            int iCertCount = signedData.Signers.Count;
            //获取签名时间
            if (iCertCount == 1)
            {
                if (signedData.HasTSATimestamp(0))
                {
                    signTime = (signedData.getTSATimeStamp(0).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            else
            {
                for (var i = 0; i < iCertCount; i++)
                {
                    signedData.Signers[i].Certificate.Display();
                    if (signedData.HasTSATimestamp(i))
                    {
                        signTime = (signedData.getTSATimeStamp(i).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
            }
            // '判断验证结果与签名时数据是否一致
            SecuInter.Signers signers = signedData.Signers;
            IEnumerator enumer = signers.GetEnumerator();

            while (enumer.MoveNext()) //第一张证书为客户端签名证书
            {
                SecuInter.Signer signer = (SecuInter.Signer)enumer.Current;
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)signer.Certificate;

                oCertSign = oCert; //'验证通过，取签名的证书
                break;
            }
            if (oCertSign == null)
            {
                throw new Exception("签名信息中无证书!");
            }
            signedData = null;
            util = null;
            return oCertSign;
        }
        /// <summary>5.4.6 PKCS7签名验证并获取签名证书*****
        /// </summary>
        /// <param name="sSource">原文</param>
        /// <param name="sSignature">签名信息；BASE64编码</param>
        /// <returns></returns>
        public static NetcaPkiLib.Certificate verifySignedData(string sSource, string sSignature)
        {
            byte[] tbsSrc = convertByte(sSource);
            return verifySignedData(tbsSrc, sSignature);
        }
        /// <summary>5.4.6.1 PKCS7签名验证并获取签名证书
        /// </summary>
        /// <param name="sSource">原文</param>
        /// <param name="sSignature">签名信息</param>
        /// <returns>签名证书</returns>
        public static NetcaPkiLib.Certificate verifySignedData(byte[] bSource, string sSignature)
        {
            try
            {
                #region 实现
                byte[] bSignature = base64Decode(sSignature);
                NetcaPkiLib.SignedData signedData = new NetcaPkiLib.SignedData();

                bool checkSignFormat = signedData.IsSign(bSignature);
                if (!checkSignFormat)
                {
                    throw new Exception("签名信息验签未通过:签名数据格式不正确!");
                }
                bool IsDetached = signedData.IsDetachedSign(bSignature);

                if (IsDetached)  /**不带原文的签名 ***/
                {
                    //signedData.Detached = true;
                    bool tbs = (bool)signedData.DetachedVerify(bSource, bSignature, false);
                    if (!tbs)
                    {
                        throw new Exception("签名信息验签未通过!");
                    }
                }
                else// 带原文
                {
                    object tbs = signedData.Verify(bSignature, true);
                    NetcaPkiLib.Utilities util = new NetcaPkiLib.Utilities();
                    bool isOK = util.ByteArrayCompare(tbs, bSource);
                    util = null;
                    if (!isOK)
                    {
                        throw new Exception("签名信息验签未通过:原文与签名信息不一致!");
                    }
                }

                NetcaPkiLib.Certificate certObj = signedData.GetSignCertificate(-1);
                signedData = null;
                return certObj;
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region 5.4	加密类

        /// <summary>5.4.1	PKCS#7加密
        /// 
        /// </summary>
        /// <param name="bSource"></param>
        /// <returns></returns>
        public static String encryptPKCS7(byte[] bSource)
        {
            SecuInter.X509Certificate oCert = getX509Certificate(SECUINTER_STORE_LOCATION.SECUINTER_CURRENT_USER_STORE, SECUINTER_STORE_NAME.SECUINTER_MY_STORE, NetCAPKI.SECUINTER_CERTTYPE_ENV, NetCAPKI.SECUINTER_NETCA_YES);
            if (oCert == null)
            {
                throw new Exception("未选择证书,请检查是否插入密钥!");
            }
            return encryptPKCS7(bSource, oCert);
        }

        /// <summary>5.4.2	PKCS#7加密
        /// 
        /// </summary>
        /// <param name="bSource"></param>
        /// <returns></returns>
        public static String encryptPKCS7(byte[] bSource, SecuInter.X509Certificate oCert)
        {
            if (oCert == null)
            {
                throw new Exception("未选择证书,请检查是否插入密钥!");
            }
            if (bSource.Length == 0)
            {
                throw new Exception("原文为空!");
            }



            SecuInter.EnvelopedData oEnv = new SecuInter.EnvelopedData();
            oEnv.Algorithm = SECUINTER_ENCRYPT_ALGORITHM.SECUINTER_ALGORITHM_DES;
            oEnv.Recipients.Add(oCert);
            oEnv.Content = bSource;

            object arrRT = oEnv.Encrypt(SECUINTER_CMS_ENCODE_TYPE.SECUINTER_CMS_ENCODE_BASE64);
            oEnv = null;
            return arrRT.ToString();
        }

        /// <summary>5.4.3	PKCS#7解密
        /// 
        /// </summary>
        /// <param name="sSignData"></param>
        /// <returns></returns>
        public static byte[] decryptPKCS7(String sSignData)
        {
            try
            {
                Utilities oUtilities = new Utilities();
                byte[] bSignData = (byte[])oUtilities.Base64Decode(sSignData);
                SecuInter.EnvelopedData oEnv = new SecuInter.EnvelopedData();
                oEnv.Decrypt(bSignData);
                byte[] rt = (byte[])oEnv.Content;
                //IEnumerator oenum=oEnv.Recipients.GetEnumerator();
                //((SecuInter.X509Certificate)oenum.Current).Display();
                oEnv = null;
                return rt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 5.5	工具类

        /// <summary>5.5.1	Base64编码
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String base64Encode(byte[] str)
        {
            Utilities oUtilities = new Utilities();

            return oUtilities.Base64Encode(str);
        }
        /// <summary>5.2.1 字符串转字节数组
        /// 
        /// </summary>
        /// <param name="sData"></param>
        /// <returns></returns>
        public static byte[] convertByte(string sData)
        {
            try
            {
                NetcaPkiLib.Utilities initialObj = new NetcaPkiLib.Utilities();
                object tbs = initialObj.Encode(sData, NETCAPKI_CP);
                initialObj = null;
                return (byte[])tbs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>5.5.2	Base64解码
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] base64Decode(String str)
        {
            Utilities oUtilities = new Utilities();
            return (byte[])oUtilities.Base64Decode(str);

        }

        /// <summary>5.5.3 工具：获取随机数
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String getRandom(int length)
        {
            try
            {
                NetcaPkiLib.Device deviceObj = new NetcaPkiLib.Device();
                byte[] bRandom = (byte[])deviceObj.GenerateRandom(length);
                return convertHex(bRandom);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>5.5.4	获取信息摘要码（HASH码，sha1)
        /// 2013-04-02 修订，解决传入字符串编码的问题
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String hashData(String str)
        {
            try
            {
                Utilities oUtilities = new Utilities();
                //byte[] b = Encoding.Default.GetBytes(str);

                byte[] bSignData = (byte[])oUtilities.Hash(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM, oUtilities.GBKEncode(str));
                //2013-03-26 modify luhanmin@cnca.net
                //utilobj.BinaryToHex(utilobj.Hash(SECUINTER_SHA1_ALGORITHM, strBase64));
                return oUtilities.BinaryToHex(bSignData);
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>获取信息摘要码（HASH码，MD5)
        /// 2013-04-02 修订，解决传入字符串编码的问题
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String hashDataMD5(String str)
        {
            try
            {
                Utilities oUtilities = new Utilities();
                //byte[] b =oUtilities.GBKEncode(str);
                byte[] bSignData = (byte[])oUtilities.Hash(SECUINTER_HASH_ALGORITHM.SECUINTER_MD5_ALGORITHM, oUtilities.GBKEncode(str));
                //2013-03-26 modify luhanmin@cnca.net
                //utilobj.BinaryToHex(utilobj.Hash(SECUINTER_SHA1_ALGORITHM, strBase64));

                return oUtilities.BinaryToHex(bSignData);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] readFile(String filename)
        {
            byte[] filedata;
            int filesize;
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            filesize = (int)fileStream.Length;

            filedata = (new BinaryReader(fileStream).ReadBytes(filesize));

            fileStream.Dispose();
            fileStream.Close();

            return filedata;
        }

        /// <summary>5.2.3 字节数组转Hex编码字符串
        /// 转大写
        /// </summary>
        /// <param name="bDatas"></param>
        /// <returns></returns>
        public static string convertHex(object bDatas)
        {
            try
            {
                NetcaPkiLib.Utilities initialObj = new NetcaPkiLib.Utilities();
                string tbs = initialObj.BinaryToHex(bDatas, true);
                initialObj = null;
                return tbs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 其他私有方法
        /// <summary>解析x509证书
        /// * 解析原则：
        ///* 关键是找到符合条件的字符起始、截止位。
        ///* 缺点:,会截断字符
        ///* 1.找到需要解析的字符，如"CN"，取整个字符在"CN"后面的字符
        ///* 2.判断后面的字符是否有"=",并且等号很近（C,O特例，否则必须在1到2个字符间）
        ///* 3.取等号后面的字符，取","
        ///* 4.找到=和,之间的字符
        /// * 5.上述条件不满足，就循环找下一个满足条件的。
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static String parseDN(String dn, String name)
        {

            String superDn = dn.ToUpper();//临时DN
            String superName = name.ToUpper();
            int beginDot = 0; //临时开始点


            while (true)
            {
                int beginName = superDn.IndexOf(superName); //开始点
                if (beginName < 0) return ""; //找不到

                superDn = superDn.Substring(beginName + superName.Length, superDn.Length - beginName - superName.Length); //取后面的串；
                int beginDH = superDn.IndexOf("=");
                if (beginDH < 0)
                { //后面没等号
                    return "";
                }
                else if (beginDH > 1)//后面=号过远
                {
                    beginDot = beginName + beginDot + superName.Length;
                    continue;
                }
                else if ((superName.Equals("C") || superName.Equals("O")) && beginDH == 1)
                { //区别C和CN
                    beginDot = beginDot + beginName + superName.Length;
                    continue; //后面=号过远
                }
                superDn = superDn.Substring(beginDH + "=".Length, superDn.Length - beginDH - "=".Length); //取后面的串；
                int end = superDn.IndexOf(",");
                beginDot = beginDot + beginName + superName.Length + beginDH + "=".Length;
                if (end < 0)//后面没,号
                {
                    //endDot = beginDot + superDn.Length;
                    return dn.Substring(beginDot, superDn.Length);
                }
                else//后面,号,取后面到,号的值
                {
                    //endDot = beginDot + end;
                    return dn.Substring(beginDot, end);
                }

            }

        }
        #endregion
        #region 其他方法
        /// <summary>
        /// 去除证书Base64编码的头尾部分
        /// </summary>
        /// <param name="certPem"></param>
        /// <returns></returns>
        private static string clearCertHeader(string certPem)
        {
            try
            {
                string certHeader = "-----BEGIN CERTIFICATE-----\r\n";
                string certEnd = "-----END CERTIFICATE-----\r\n";
                if (certPem.IndexOf(certHeader) >= 0)
                {
                    certPem = certPem.Substring(certHeader.Length, certPem.Length - certHeader.Length);
                    certPem = certPem.Substring(0, certPem.Length - certEnd.Length);
                }
                certHeader = "-----BEGIN CERTIFICATE-----\n";
                certEnd = "-----END CERTIFICATE-----\n";
                if (certPem.IndexOf(certHeader) >= 0)
                {
                    certPem = certPem.Substring(certHeader.Length, certPem.Length - certHeader.Length);
                    certPem = certPem.Substring(0, certPem.Length - certEnd.Length);
                }
                return certPem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
