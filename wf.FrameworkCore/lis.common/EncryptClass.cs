using System;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;

namespace dcl.common
{
    public class EncryptClass
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public EncryptClass()
        {
        }

        /// <summary>
        /// 缺省密钥
        /// </summary>
        private static string Key = "LIJIEZUISHUAI";

        /// <summary>
        /// 缺省字符编码
        /// </summary>
        private static Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// 使用缺省密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original)
        {
            return Encrypt(original, Key);
        }

        /// <summary>
        /// 使用缺省密钥解密
        /// </summary>
        /// <param name="original">密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original)
        {
            return Decrypt(original, Key);
        }

        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">原始文字</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>密文</returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = encoding.GetBytes(original);
            byte[] kb = encoding.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        /// 使用给定密钥解密
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, encoding);
        }      

        /// <summary>
        /// 使用给定密钥解密
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        private static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            try
            {
                byte[] buff = Convert.FromBase64String(encrypted);
                byte[] kb = System.Text.Encoding.Default.GetBytes(key);
                return encoding.GetString(Decrypt(buff, kb));
            }
            catch
            {
                return encrypted;
            }
            
        }
        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        private static byte[] MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }

        /// <summary>
        /// 使用给定密钥加密
        /// </summary>
        /// <param name="original">明文</param>
        /// <param name="key">密钥</param>
        /// <returns>密文</returns>
        private static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }

        /// <summary>
        /// 使用给定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        private static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.Key = MakeMD5(key);
                des.Mode = CipherMode.ECB;
                return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
            }
            catch
            {
                return encrypted;
            }
        }

    }
}