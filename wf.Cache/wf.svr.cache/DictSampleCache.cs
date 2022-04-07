using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;

namespace dcl.svr.cache
{
    /// <summary>
    /// 
    /// </summary>
    public class DictSampleCache
    {
        private static DictSampleCache _instance = null;
        private static object padlock = new object();

        public static DictSampleCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictSampleCache();
                        }
                    }
                }
                return _instance;
            }
        }


        public List<EntityDicSample>DclCache { get; private set; }


        private DictSampleCache()
        {
            ThreadRefresh();
        }

        private void ThreadRefresh()
        {
            lock (padlock)
            {
                try
                {
                    CacheDataBIZ cache = new CacheDataBIZ();
                    EntityResponse response = cache.GetCacheData("EntityDicSample");
                    DclCache = response.GetResult<List<EntityDicSample>>();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// 根据标本id获取标本中文名称
        /// </summary>
        /// <param name="sam_id"></param>
        /// <returns></returns>
        public string GetSamCNameByID(string sam_id)
        {
            if (this.DclCache == null)
            {
                return null;
            }

            var query = from item in this.DclCache
                        where item.SamId == sam_id
                        select item.SamName;

            if (query.Count() > 0)
            {
                return query.First();
            }
            else
            {
                return null;
            }
        }
    }
}
