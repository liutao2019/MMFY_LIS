using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.core
{
    public class MD5Helper
    {
        string key = "?;i\u001c?&\u0014?";

        // 创建Key
        public string GenerateKey()
        {
            System.Security.Cryptography.DESCryptoServiceProvider desCrypto = (
                System.Security.Cryptography.DESCryptoServiceProvider)System.Security.Cryptography.DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        // 加密字符串
        public string EncryptString(string sInputString)
        {
            string sKey = key;
            byte[] data = Encoding.UTF8.GetBytes(sInputString);
            System.Security.Cryptography.DESCryptoServiceProvider DES = new System.Security.Cryptography.DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            System.Security.Cryptography.ICryptoTransform desencrypt = DES.CreateEncryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result);
        }

        // 解密字符串
        public string DecryptString(string sInputString)
        {
            string sKey = key;
            string[] sInput = sInputString.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], System.Globalization.NumberStyles.HexNumber);
            }
            System.Security.Cryptography.DESCryptoServiceProvider DES = new System.Security.Cryptography.DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            System.Security.Cryptography.ICryptoTransform desencrypt = DES.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }
    }
}
