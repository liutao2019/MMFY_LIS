using System;
using System.Collections.Generic;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    public class DictDepartCache : IServerCache<EntityDicPubDept>
    {
        private static DictDepartCache _instance = null;
        private static object padlock = new object();

        public static DictDepartCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictDepartCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntityDicPubDept> DclCache { get; private set; }

        private DictDepartCache()
        {
            ThreadRefresh();
        }

        public List<EntityDicPubDept> GetAll()
        {
            return this.DclCache;
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
                    EntityResponse response = cache.GetCacheData("EntityDicPubDept");
                    DclCache = response.GetResult<List<EntityDicPubDept>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
