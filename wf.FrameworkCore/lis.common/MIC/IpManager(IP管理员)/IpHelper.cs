using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text; 

namespace dcl.common
{
    public static class IpHelper
    {
        //获取内网IP
        public static string GetInternalIP()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        //获取外网IP
        public static string GetExternalIP()
        {
            string direction = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                direction = stream.ReadToEnd();
            }
            int first = direction.IndexOf("Address:") + 9;
            int last = direction.LastIndexOf("</body>");
            direction = direction.Substring(first, last - first);
            return direction;
        }

        public static bool IsValidIP(string ip)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
            {
                string[] ips = ip.Split('.');
                if (ips.Length == 4 || ips.Length == 6)
                {
                    if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            else
                return false;
        }
        /// <summary>
        /// 获取第一个符合有效ip格式的字符串
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetValidIP(string ip)
        {
            var match = System.Text.RegularExpressions.Regex.Match(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}");
            if (IsValidIP(match.Value))
                return match.Value;
            else
                return string.Empty;
        }

    }
}
