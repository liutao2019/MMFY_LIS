using System;
using System.Collections.Generic;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    /// <summary>
    /// 组合字典缓存
    /// </summary>
    public class DictCombineCache : IServerCache<EntityDicCombine>
    {
        private static DictCombineCache _instance = null;
        private static object padlock = new object();

        public static DictCombineCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictCombineCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntityDicCombine> DclCache { get; private set; }
        private DictCombineCache()
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
                    EntityResponse response = cache.GetCacheData("EntityDicCombine");
                    DclCache = response.GetResult<List<EntityDicCombine>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 根据组合id获取组合
        /// </summary>
        /// <param name="com_id">组合id</param>
        /// <param name="onlyGetNotDeleted">是否只获取未删除的组合</param>
        /// <returns></returns>
        public EntityDicCombine GetCombineByID(string com_id, bool onlyGetNotDeleted)
        {
            EntityDicCombine combine = null;
            if (onlyGetNotDeleted)
            {
                combine = this.DclCache.Find(com => com.ComId == com_id && com.ComDelFlag != "1");
            }
            else
            {
                combine = this.DclCache.Find(com => com.ComId == com_id);
            }
            return combine;
        }

        public List<EntityDicCombine> GetAll()
        {
            return this.DclCache;
        }
    }
}
