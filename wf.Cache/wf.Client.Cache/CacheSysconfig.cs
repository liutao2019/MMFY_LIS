using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;

namespace dcl.client.cache
{
    public class CacheSysconfig
    {
        private static CacheSysconfig _instance = null;
        private static object padlock = new object();

        public static CacheSysconfig Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheSysconfig();
                        }
                    }
                }
                return _instance;
            }
        }

        List<EntitySysParameter> cache = null;

        /// <summary>
        /// .ctor
        /// </summary>
        private CacheSysconfig()
        {
            Refresh();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void Refresh()
        {
            ProxySystemConfig proxy = new ProxySystemConfig();
            cache = proxy.Service.GetSysParaCaChe();
        }

   
        /// <summary>
        /// 获取单个系统配置
        /// </summary>
        /// <param name="configCode"></param>
        /// <returns></returns>
        public string GetSystemConfig(string configCode)
        {
            return GetUserConfig(configCode, "system");
        }

        /// <summary>
        /// 获取单个用户配置
        /// </summary>
        /// <param name="configCode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetUserConfig(string configCode, string userID)
        {
            string config = string.Empty;

            List<EntitySysParameter> listPar = cache.Where(w => w.ParmCode == configCode && w.ParmType == userID).ToList();

            if (listPar.Count > 0)
            {
                config = listPar[0].ParmFieldValue.ToString();
            }

            return config;
        }
    }
}
