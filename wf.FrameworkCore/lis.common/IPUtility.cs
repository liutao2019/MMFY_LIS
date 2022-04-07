using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Management;
using Lib.LogManager;

namespace dcl.common
{
    public class IPUtility
    {
        public static string GetIP()
        {
            try
            {
                IPHostEntry ipHost = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];
                return ipAddr.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException("GetIP()", ex);
                return string.Empty;
            }
        }

        public static string GetMAC()
        {
            //return string.Empty;

            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac += mo["MacAddress"].ToString();
                        break;
                    }
                string[] macs = mac.Split(':');
                mac = "";
                foreach (string m in macs)
                {
                    mac += m;
                }
                moc = null;
                mc = null;
                return mac.Trim();
            }
            catch (Exception e)
            {
                return "uMnIk";
            }

        }

    }
}
