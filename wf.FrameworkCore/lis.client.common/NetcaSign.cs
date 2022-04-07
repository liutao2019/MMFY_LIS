using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Netca_PDFSign;
using SecuInter;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.common;
using X509Certificate = SecuInter.X509Certificate;
using dcl.entity;
using System.Linq;

namespace dcl.client.common
{
    public class NetcaSign
    {
        X509Certificate getNetCACert(SECUINTER_STORE_NAME storeType, bool isSignCert)//选择客户端证书
        {
            Utilities oUtil = null;
            Store MyStore = null;
            try
            {
                oUtil = new Utilities();
                MyStore = new Store();
                MyStore.Open(SECUINTER_STORE_LOCATION.SECUINTER_CURRENT_USER_STORE, storeType);
                X509Certificates certs = (X509Certificates)MyStore.X509Certificates;
                MyStore.Close();
                MyStore = null;

                X509Certificates MyCerts = new X509Certificates();
                for (int i = 0; i < certs.Count; i++)
                {
                    X509Certificate cert = (X509Certificate)certs[i];
                    string issuer = cert.get_Issuer(SECUINTER_NAMESTRING_TYPE.SECUINTER_X500_NAMESTRING);

                    if (issuer.IndexOf("CN=NETCA") < 0)
                    {
                        continue;
                    }
                    else
                    {
                        long iKeyUsage = cert.KeyUsage;
                        if (iKeyUsage == -1)
                        {
                            MyCerts.Add(cert);
                        }
                        else
                        {
                            if (isSignCert == true)
                            {
                                if (iKeyUsage % 2 == 1 && iKeyUsage % 4 >= 2)
                                {
                                    MyCerts.Add(cert);
                                }
                            }
                            else
                            {
                                if (iKeyUsage % 8 >= 4)
                                {
                                    MyCerts.Add(cert);
                                }
                            }
                        }
                    }
                }

                if (MyCerts.Count > 0)
                {
                    return (X509Certificate)MyCerts.SelectCertificate();
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public bool sign(string loginID, ref string returnMes)//签名（签名内容）
        {

            try
            {
                //NETCA模式
                string strNETCA_ModeType = ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType");
                if (strNETCA_ModeType == "深圳新安")
                {
                    object returnMes_obj=null;
                    bool rvbln= sign_xa(loginID,ref returnMes_obj);
                    if (rvbln)
                    {
                        returnMes=BitConverter.ToString(((byte[])returnMes_obj)).Replace("-", string.Empty);
                    }
                    return rvbln;
                }
            }
            catch
            {
            }

            bool IsNotSource = true;
            X509Certificate oCert = getNetCACert(SECUINTER_STORE_NAME.SECUINTER_MY_STORE, true);
            if (oCert == null)
            {
                MessageBox.Show("没选择证书或没插KEY!");
                return false;
            }
            //获取签名证书的微缩图
            //Utilities oUtil = new Utilities();
            //string thu = oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM));	
            //判断thu 与当前登录的证书的微缩图是否一致，如果一致则继续...

            Signer oSigner = null;
            SignedData oSignedData = null;
            try
            {
                oSigner = new Signer();
                oSignedData = new SignedData();
            }
            catch (Exception e)
            {
                return false;
            }

            oSigner.Certificate = oCert;
            oSigner.HashAlgorithm = SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM;
            oSigner.UseSigningCertificateAttribute = false;
            oSigner.UseSigningTime = false;
            oSignedData.Content = loginID;// Key;
            oSignedData.Detached = IsNotSource;

            object ob = new object();

            try
            {
                ob = oSignedData.Sign(oSigner, SECUINTER_CMS_ENCODE_TYPE.SECUINTER_CMS_ENCODE_BASE64);
            }
            catch (Exception ex)
            {
                return false;
            }


            returnMes = ob.ToString();

            oSignedData = null;
            oSigner = null;
            return true;
        }

        /// <summary>
        /// 深圳新安模式
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="returnMes"></param>
        /// <returns></returns>
        private  bool sign_xa(string loginID, ref object returnMes)
        {
            NetcaPkiLib.Certificate lpCert = getCertForSignature();
            if (lpCert == null)
            {
                MessageBox.Show("没选择证书或没插KEY!");
                return false;
            }

            object ob = new object();

            try
            {
                byte[] byteArray = System.Text.Encoding.Default.GetBytes(loginID);//原文
                ob = MakeSignedData(false, byteArray, lpCert);
            }
            catch
            {
                return false;
            }

            returnMes = ob;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subjectCN"></param>
        /// <param name="cert_code"></param>
        /// <returns></returns>
        public bool GetCertStringAttribute(ref string subjectCN, ref object cert_code)
        {
            try
            {
                NetcaPkiLib.Certificate hCert = getCertForSignature();
                if (hCert != null)
                {
                    //获取证书编码
                     cert_code = hCert.Encode(1);
                    //证书的主题CN项
                    subjectCN = hCert.GetStringInfo(20);
                    return true;
                }
            }
            catch(Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }

        /// <summary>
        /// 获取签名证书
        /// </summary>
        /// <returns></returns>
      public    NetcaPkiLib.Certificate getCertForSignature()
        {
            try
            {
                NetcaPkiLib.UtilitiesClass utilPtr = new NetcaPkiLib.UtilitiesClass();

                NetcaPkiLib.Certificate lpCert = utilPtr.CreateCertificateObject();

                utilPtr.ClearPwdCache();

                //采用默认的UI，从设备中选择有效期内的签名证书
                string wcertType = "{\"UIFlag\":\"default\", \"InValidity\":true,\"Type\":\"signature\", \"Method\":\"device\",\"Value\":\"any\"}";
                //在有效期内的颁发者CN项包括NETCA的签名证书证书
                string wfilter = "IssuerCN~'NETCA' && InValidity='True' && CertType='Signature' && CheckPrivKey='True'";

                lpCert.Select(wcertType, wfilter);

                if (lpCert.HasPrivateKey())
                {
                    return lpCert;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return null;
        }
      /// <summary>
      /// 生成SignedData签名
      ///  detech,为true表示生成不带原文的签名，false表示带签名的原文
      ///  tbs,签名原文
      /// </summary>
      /// <param name="detach">为true表示生成不带原文的签名，false表示带签名的原文</param>
      /// <param name="tbs">签名原文</param>
      /// <param name="hCert"></param>
      /// <returns></returns>
      private object MakeSignedData(bool detach, object tbs, NetcaPkiLib.Certificate hCert)
      {
          object signedData = null;
          ////if( detach)
          ////    printf("开始测试不带签名原文的SignedData！\n");
          ////else
          ////      printf("开始测试带签名原文的SignedData！\n");

          NetcaPkiLib.Utilities utilPtr = new NetcaPkiLib.Utilities();

          NetcaPkiLib.SignedData signedDataPtr = utilPtr.CreateSignedDataObject();
          //设置签名证书
          signedDataPtr.SetSignCertificate(hCert, null, false);

          //-获取证书公钥算法
          int publicAlgo = hCert.PublicKeyAlgorithm;


          int signAlgo = 0;
          if (publicAlgo == 1)
              signAlgo = 4;
          else if (publicAlgo == 4)
              signAlgo = 25;

          //设置签名算法,
          int index = 1;//设置第一个签名者的签名算法，序号从1开始
          signedDataPtr.SetSignAlgorithm(1, signAlgo);


          //设置不带原文的签名
          if (detach)
              signedDataPtr.Detached = true;
          else
              signedDataPtr.Detached = false;

          //设置签名包含的证书。如果不设置，产生的SignedData如果可能的话，将带有整个证书链。
          signedDataPtr.SetIncludeCertificateOption(3);

          //
          //string bstrURL = "http://tsa.cnca.net/NETCATimeStampServer/TSAServer.jsp";
          //signedData = signedDataPtr.SignWithTSATimeStamp(bstrURL, tbs, 1);

          //签名
          signedData = signedDataPtr.Sign(tbs, 1);

          return signedData;
      }

        public bool verify(string loginID, string bSignData)
        {
            try
            {
                //NETCA模式
                string strNETCA_ModeType = ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType");
                if (strNETCA_ModeType == "深圳新安")
                {
                    byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(loginID);//原文
                    return VerifySignedData(bSignData, byteArray);
                }
            }
            catch
            {
            }

            SignedData oSignedData = null;

            try
            {
                oSignedData = new SignedData();
                oSignedData.Content = loginID;// Key;
                oSignedData.Detached = true;
                if (!oSignedData.Verify(bSignData, 0))
                {
                    return false;
                }
                oSignedData = null;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public bool login(string loginID, ref string returnMes)
        {

            try
            {
                //NETCA模式
                string strNETCA_ModeType = ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType");
                if (strNETCA_ModeType == "深圳新安")
                {
                    return login_xa(loginID,ref returnMes);
                }
            }
            catch
            {
            }


            Signer oSigner = null;
            SignedData oSignedData = null;
            Utilities oUtil = null;

            try
            {
                string bSignData = string.Empty;
                if (sign(loginID, ref bSignData))
                {
                    oSigner = new Signer();
                    oSignedData = new SignedData();
                    oUtil = new Utilities();
                    oSignedData.Content = loginID;// Key;
                    oSignedData.Detached = true;
                    if (!oSignedData.Verify(bSignData, 0))
                    {
                        return false;
                    }

                    X509Certificate oCert = (X509Certificate)oSignedData.Signers[0].Certificate;
                    //MessageBox.Show("ValidFromDate:" + oCert.ValidFromDate);
                    //MessageBox.Show("ValidToDate:" + oCert.ValidToDate);
                    //MessageBox.Show("Thumbprint:" + oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM)));

                    returnMes = oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM));

                    oSignedData = null;
                    oSigner = null;
                    oUtil = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        /// <summary>
        /// 深圳新安模式
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="returnMes"></param>
        /// <returns></returns>
        private bool login_xa(string loginID, ref string returnMes)
        {
            try
            {
                object bSignData_obj=null;
                if (sign_xa(loginID, ref bSignData_obj))
                {
                    byte[] byteArray = System.Text.Encoding.Default.GetBytes(loginID);//原文

                    string sr_signedData2 = BitConverter.ToString(((byte[])bSignData_obj)).Replace("-", string.Empty);

                    if (!VerifySignedData(sr_signedData2, byteArray))
                    {
                        return false;
                    }

                    NetcaPkiLib.Certificate certificate = getCertForSignature();

                    if (certificate != null)
                    {
                        object SHA1 = certificate.get_ThumbPrint(8192);
                        //string str_SHA1 = System.Text.Encoding.UTF8.GetString(((byte[])SHA1));
                        string str_SHA1 = BitConverter.ToString(((byte[])SHA1)).Replace("-", string.Empty);

                        returnMes = str_SHA1;
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }

            return false;

        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="signedData"></param>
        /// <param name="tbs"></param>
        /// <returns></returns>
        private bool VerifySignedData(string signedData, object tbs)
        {
            try
            {
                NetcaPkiLib.Utilities utilPtr = new NetcaPkiLib.Utilities();
                NetcaPkiLib.SignedData signedDataPtr = utilPtr.CreateSignedDataObject();

                object signedData2 = HexToByte(signedData);

                object tbs2 = signedDataPtr.Verify(signedData2, false);
                if (tbs2 != null)
                {
                    byte[] b1 = (byte[])tbs;
                    byte[] b2 = (byte[])tbs2;

                    if (b1.Length == b2.Length)
                    {
                        if (b1 != null && b2 != null)
                        {
                            for (int i = 0; i < b1.Length; i++)
                            {
                                if (b1[i] != b2[i])
                                {
                                    return false;
                                }
                            }

                            return true;
                        }
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return false;
        }


        public string getCertThumprint()  //获取微缩图（主要用于管理员绑定证书时用）
        {
            try
            {
                //NETCA模式
                string strNETCA_ModeType = ConfigHelper.GetSysConfigValueWithoutLogin("NETCA_ModeType");
                if (strNETCA_ModeType == "深圳新安")
                {
                    NetcaPkiLib.Certificate prCert = getCertForSignature();
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

            Utilities oUtil = new Utilities();
            X509Certificate oCert = getNetCACert(SECUINTER_STORE_NAME.SECUINTER_MY_STORE, true);
            return oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM));
        }

        public X509Certificate2 Sign_Cert()  
        {
            PDFSign pdfSign = new PDFSign();
            return pdfSign.Sign_Cert();
        }

        /// <summary>
        /// pdf签章
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="loginID"></param>
        /// <returns></returns>
        public Dictionary<string, byte[]> PdfSign(Dictionary<string, MemoryStream> dic, string loginID)
        {
            Dictionary<string, byte[]> dicSign = new Dictionary<string, byte[]>();
            string pdfReportPath = @"C:\Program Files\medchange\lis\PdfReport\";
            if (!Directory.Exists(pdfReportPath))
            {
                Directory.CreateDirectory(pdfReportPath);
            }
            List<EntityUserKey> listPoweruserkey = UserInfo.entityUserInfo.PowerUserKey;
             PDFSign pdfSign = new PDFSign();
            X509Certificate2 certificate2=null;
            string password = "12345678";
            if (listPoweruserkey != null)
            {
                List<EntityUserKey> rows = listPoweruserkey.Where(w=>w.UserLoginId==loginID).ToList();
                if (rows.Count > 0 && rows[0].UserCertInfo != null && rows[0].UserCertInfo != DBNull.Value.ToString())
                {
                    certificate2=new X509Certificate2();
                    certificate2.Import((byte[])System.Text.Encoding.Default.GetBytes (rows[0].UserCertInfo));

                    if (rows[0].UserCertPassword != null && rows[0].UserCertPassword != DBNull.Value.ToString()
                        && !string.IsNullOrEmpty(rows[0].UserCertPassword.ToString()))
                    {
                        password = EncryptClass.Decrypt(rows[0].UserCertPassword.ToString());
                    }
                }
            }
            if (certificate2 == null)
            {
                return dicSign;
            }
           
            foreach (string key in dic.Keys)
            {
                string fileName = key + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".pdf";
                string fileNameSign = key + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + "_sign.pdf";
                File.WriteAllBytes(pdfReportPath + fileName, dic[key].ToArray());
                Boolean signResult = pdfSign.Pdf_Sign(certificate2, password, pdfReportPath + fileName,
                                                      pdfReportPath + fileNameSign, 1, 430, 5, "");
                if (signResult)
                {
                    byte[] pBytes = File.ReadAllBytes(pdfReportPath + fileNameSign);
                    dicSign.Add(key, pBytes);
                }
                File.Delete(pdfReportPath + fileName);
                File.Delete(pdfReportPath + fileNameSign);
            }
            return dicSign;
        }
    }
}
