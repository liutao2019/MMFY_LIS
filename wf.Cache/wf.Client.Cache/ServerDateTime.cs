using System;
using System.Collections.Generic;
using System.Text;
using dcl.client.wcf;

namespace dcl.client.cache
{
    public class ServerDateTime
    {
        public readonly static string DateTimeFormat = "yyyy-MM-dd HH:mm";
        public readonly static string DateFormat = "yyyy-MM-dd";
        public readonly static string DateTimeLongFormat = "yyyy-MM-dd HH:mm:ss";
        public readonly static string OutlinkDateTimeFormat = "yyyy/MM/dd HH:mm";
        public readonly static string OutlinkDateTimeLongFormat = "yyyy/MM/dd HH:mm:ss";

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerDateTime()
        {
            ProxyCacheService proxy = new ProxyCacheService();
            return proxy.Service.GetServerDateTime();
        }
    }
}
