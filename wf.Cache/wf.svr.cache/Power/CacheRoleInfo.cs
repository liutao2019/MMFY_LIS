using System;
using System.Collections.Generic;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    public class CacheRoleInfo : IServerCache<EntitySysRole>
    {
        private static CacheRoleInfo _instance = null;
        private static object padlock = new object();

        public static CacheRoleInfo Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheRoleInfo();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntitySysRole> Cache { get; set; }

        #region .ctor

        private CacheRoleInfo()
        {
            ThreadRefresh();
        }

        #endregion
        /// <summary>
        /// 刷新数据
        /// </summary>
        public void Refresh()
        {
            Thread t = new Thread(ThreadRefresh);
            t.Start();
        }

        private void ThreadRefresh()
        {
            lock (padlock)
            {
                try
                {
                    EntityResponse response = new CacheDataBIZ().GetCacheData("EntitySysRole");
                    Cache = response.GetResult() as List<EntitySysRole>;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public List<EntitySysRole> GetAll()
        {
            return this.Cache;
        }
    }
}
