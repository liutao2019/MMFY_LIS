using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.cache
{
    public class DictSysUserCache
    {

        private static DictSysUserCache _instance = null;
        private static object padlock = new object();

        public static DictSysUserCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictSysUserCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntitySysUser> Dclcache = new List<EntitySysUser>();

        private DictSysUserCache()
        {
            Refresh();
        }

        public void Refresh()
        {
            this.Dclcache = GetAllUser();
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        private List<EntitySysUser> GetAllUser()
        {

            CacheDataBIZ cache = new CacheDataBIZ();
            EntityResponse response = cache.GetCacheData("EntitySysUser");
            List<EntitySysUser> cacheUser = new List<EntitySysUser>();
            cacheUser = response.GetResult<List<EntitySysUser>>();

            return cacheUser;
        }

    }
}

