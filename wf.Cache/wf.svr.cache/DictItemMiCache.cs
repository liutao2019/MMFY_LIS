using System;
using System.Collections.Generic;
using System.Threading;

using dcl.entity;

namespace dcl.svr.cache
{
    public class DictItemMiCache
    {
        private static DictItemMiCache _instance = null;
        private static object padlock = new object();

        public static DictItemMiCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictItemMiCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntityDicItmRefdetail> DclCache { get; private set; }
        private DictItemMiCache()
        {
            ThreadRefresh();
        }

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
                    CacheDataBIZ cache = new CacheDataBIZ();
                    EntityResponse response = cache.GetCacheData("EntityDicItmRefdetail");
                    DclCache = response.GetResult<List<EntityDicItmRefdetail>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
