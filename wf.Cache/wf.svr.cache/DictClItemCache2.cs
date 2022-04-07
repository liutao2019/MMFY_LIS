using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.cache
{
    public class DictClItemCache2
    {
         private static DictClItemCache2 _instance = null;
        private static object padlock = new object();

        public static DictClItemCache2 Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictClItemCache2();
                        }
                    }
                }
                return _instance;
            }
        }

        private List<EntityDicItmCalu> cacheTable = null;

        private DictClItemCache2()
        {
            this.Refresh();
        }

        public void Refresh()
        {
            CacheDataBIZ cache = new CacheDataBIZ();
            EntityResponse response = cache.GetCacheData("EntityDicItmCalu");
            cacheTable = response.GetResult<List<EntityDicItmCalu>>().Where(w=>w.CalFlag==2).ToList();
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<EntityDicItmCalu> GetAllData()
        {
            return cacheTable;
        }
    }
}
