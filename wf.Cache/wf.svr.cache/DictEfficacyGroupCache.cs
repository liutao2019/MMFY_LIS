using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.cache
{
    public class DictEfficacyGroupCache
    {
        private static DictEfficacyGroupCache _instance = null;
        private static object padlock = new object();

        public static DictEfficacyGroupCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictEfficacyGroupCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private DataTable cacheTable = null;
        private List<EntityDicItmCheck> cacheList = null;
        private DictEfficacyGroupCache()
        {
            this.Refresh();
        }

        public void Refresh()
        {
            CacheDataBIZ cache = new CacheDataBIZ();
            EntityResponse response = cache.GetCacheData("EntityDicItmCheck");
            cacheList = response.GetResult<List<EntityDicItmCheck>>();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllData()
        {
            return cacheTable;
        }

        /// <summary>
        /// 根据仪器id获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<EntityDicItmCheck> GetCacheData(string patItrID)
        {
            List<EntityDicItmCheck> rows = cacheList.Where(w=> w.CheckItrId== patItrID).ToList();
            return rows;
        }
    }
}
