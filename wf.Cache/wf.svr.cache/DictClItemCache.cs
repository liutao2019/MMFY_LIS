using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.cache
{
    /// <summary>
    /// 字典：计算项目缓存
    /// </summary>
    public class DictClItemCache
    {
        private static DictClItemCache _instance = null;
        private static object padlock = new object();

        public static DictClItemCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictClItemCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private DataTable cacheTable = null;
        private List<EntityDicItmCalu> DclCache = null;

        private DictClItemCache()
        {
            this.Refresh();
        }

        public void Refresh()
        {
            CacheDataBIZ cache = new CacheDataBIZ();
            EntityResponse response = cache.GetCacheData("EntityDicItmCalu");
            DclCache = response.GetResult<List<EntityDicItmCalu>>();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
            return cacheTable;
        }
        public List<EntityDicItmCalu> GetAllCalu()
        {
            return DclCache;
        }
    }
}
