using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.cache
{
    public class DictEffcacyItemCache
    {
        private static DictEffcacyItemCache _instance = null;
        private static object padlock = new object();

        public static DictEffcacyItemCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictEffcacyItemCache();
                        }
                    }
                }
                return _instance;
            }
        }
        private List<EntityDicItmCheckDetail> cacheList=null;
        private DictEffcacyItemCache()
        {
            this.Refresh();
        }

        public void Refresh()
        {
            CacheDataBIZ cache = new CacheDataBIZ();
            EntityResponse response = cache.GetCacheData("EntityDicItmCheckDetail");
            cacheList = response.GetResult<List<EntityDicItmCheckDetail>>();
        }

        /// <summary>
        /// 根据仪器id获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<EntityDicItmCheckDetail> GetCacheData(string patItrID,bool isAutoAudit)
        {
            List<EntityDicItmCheckDetail> rows = new List<EntityDicItmCheckDetail>();
            if (isAutoAudit)
            {
                rows = cacheList.Where(w => w.CheckItrId == patItrID).ToList();
            }
            else {
                rows = cacheList.Where(w => w.CheckItrId == patItrID && w.CheckAuditFlag == null || w.CheckAuditFlag != "1").ToList();
            }
            return rows;
        }
    }
}
