using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.entity;
using System.Linq;

namespace dcl.svr.cache
{
    public class CacheSysConfig
    {
        private static CacheSysConfig _instance = null;
        private static object padlock = new object();

        public static CacheSysConfig Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheSysConfig();
                        }
                    }
                }
                return _instance;
            }
        }

        List<EntitySysParameter> cache =new List<EntitySysParameter>();

        /// <summary>
        /// .ctor
        /// </summary>
        private CacheSysConfig()
        {
            Refresh();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void Refresh()
        {
            CacheDataBIZ cacheBIZ = new CacheDataBIZ();
            EntityResponse response = cacheBIZ.GetCacheData("EntitySysParameter");
            cache = response.GetResult<List<EntitySysParameter>>();
        }

        /// <summary>
        /// 获取用户配置组
        /// </summary>
        /// <param name="confGroup"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<EntitySysParameter> GetUserConfigGroup(string confGroup, string userID)
        {
            List<EntitySysParameter> temp = new List<EntitySysParameter>();
            if (cache != null)
            {
                List<EntitySysParameter> listPar = cache.Where(w => w.ParmGroup == confGroup && w.ParmType == userID).ToList();

                temp = EntityManager<EntitySysParameter>.ListClone(cache);

                foreach (EntitySysParameter par in listPar)
                {
                    temp.Add(par);
                }

            }

            return temp;
        }

        /// <summary>
        /// 获取系统配置组
        /// </summary>
        /// <param name="confGroup"></param>
        /// <returns></returns>
        public List<EntitySysParameter> GetSystemConfigGroup(string confGroup)
        {
            return GetUserConfigGroup(confGroup, "system");
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
            if (cache != null)
            {
                List<EntitySysParameter> listPar = cache.Where(w => w.ParmCode == configCode && w.ParmType == userID).ToList();

                if (listPar.Count > 0)
                {
                    config = listPar[0].ParmFieldValue.ToString();
                }
            }
            return config;
        }
    }
}
