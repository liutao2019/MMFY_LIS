using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.ca
{
    public class CaPKIFactory
    {
        public static string errorInfo;

        /// <summary>
        /// 各种CA公司调用实例化
        /// </summary>
        /// <param name="p_strCAMode"></param>
        /// <returns></returns>
        public static ICaPKI CreateCASignature(string p_strCAMode)
        {
            ICaPKI caPKI = null;
            if (p_strCAMode == "GDCA")
            {
                try
                {
                    caPKI = new GDCAPKI();
                }
                catch (System.Runtime.InteropServices.COMException comException)
                {
                    Lib.LogManager.Logger.LogException(comException);
                    errorInfo = "当前设备未安装数字证书驱动";
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    errorInfo = "初始化数字证书失败";
                }
            }


            return caPKI;
        }
    }
}
