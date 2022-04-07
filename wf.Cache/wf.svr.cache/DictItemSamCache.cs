using System;
using System.Collections.Generic;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    public class DictItemSamCache
    {
        private static DictItemSamCache _instance = null;
        private static object padlock = new object();

        public static DictItemSamCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictItemSamCache();
                        }
                    }
                }
                return _instance;
            }
        }

        //public List<EntityDictItemSam> Cache { get; private set; }

        public List<EntityDicItemSample> DclCache { get; private set; }



        private DictItemSamCache()
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
                    EntityResponse response = cache.GetCacheData("EntityDicItemSample");
                    DclCache = response.GetResult<List<EntityDicItemSample>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public EntityDicItemSample GetItem(string itm_id, string itm_sam_id)
        {
            EntityDicItemSample item = this.DclCache.Find(i => i.ItmId == itm_id && i.ItmSamId == itm_sam_id);
            return item;
        }
    }
}
