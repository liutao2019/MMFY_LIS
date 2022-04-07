using System;
using System.Collections.Generic;
using System.Threading;

using dcl.entity;

namespace dcl.svr.cache
{
    public class DictItemCache
    {
        private static DictItemCache _instance = null;
        private static object padlock = new object();

        public static DictItemCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictItemCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntityDicItmItem> DclCache { get; private set; }

        private DictItemCache()
        {
            DclCache = new List<EntityDicItmItem>();//新增(避免空值对象引用)

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
                    EntityResponse response = cache.GetCacheData("EntityDicItmItem");
                    DclCache = response.GetResult<List<EntityDicItmItem>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}
