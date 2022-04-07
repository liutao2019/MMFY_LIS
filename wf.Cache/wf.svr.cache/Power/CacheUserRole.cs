using System;
using System.Collections.Generic;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    public class CacheUserRole : IServerCache<EntityUserRole>
    {
        private static CacheUserRole _instance = null;
        private static object padlock = new object();

        public static CacheUserRole Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheUserRole();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntityUserRole> Cache { get; set; }

        #region .ctor

        private CacheUserRole()
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
                    EntityResponse response = new CacheDataBIZ().GetCacheData("EntityUserRole");
                    Cache = response.GetResult() as List<EntityUserRole>;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        public List<EntityUserRole> GetAll()
        {
            return this.Cache;
        }
    }
}
