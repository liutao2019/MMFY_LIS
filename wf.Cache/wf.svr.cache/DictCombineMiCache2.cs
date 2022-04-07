using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    public class DictCombineMiCache2 : IServerCache<EntityDicCombineDetail>
    {
        private static DictCombineMiCache2 _instance = null;
        private static object padlock = new object();

        public static DictCombineMiCache2 Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictCombineMiCache2();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntityDicCombineDetail> DclCache { get; set; }
        private DictCombineMiCache2()
        {
            ThreadRefresh();
        }

        public List<EntityDicCombineDetail> GetAll()
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
                    EntityResponse response = cache.GetCacheData("EntityDicCombineDetail");
                    DclCache = response.GetResult<List<EntityDicCombineDetail>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public List<EntityDicCombineDetail> GetComMi(string com_id)
        {
            var query = from com_item in this.DclCache
                        join item in dcl.svr.cache.DictItemCache.Current.DclCache on com_item.ComItmId equals item.ItmId
                        where item.ItmDelFlag != "1" && com_item.ComId == com_id
                        select com_item;

            return new List<EntityDicCombineDetail>(query);
        }
    }
}
