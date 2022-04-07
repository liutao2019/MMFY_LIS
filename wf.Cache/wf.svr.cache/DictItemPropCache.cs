using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;

namespace dcl.svr.cache
{
    public class DictItemPropCache
    {
        #region singleton
        private static DictItemPropCache _instance = null;
        private static object padlock = new object();

        public static DictItemPropCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictItemPropCache();
                        }
                    }
                }
                return _instance;
            }
        }


        public List<EntityDefItmProperty> DclCache { get; private set; }
        #endregion

        private DictItemPropCache()
        {
            ThreadRefresh();
        }

        /// <summary>
        /// 根据项目id与输入码获取项目特征值
        /// </summary>
        /// <param name="itm_id"></param>
        /// <param name="in_code"></param>
        /// <returns></returns>
        public string GetItmProp(string itm_id, string in_code)
        {

            if (string.IsNullOrEmpty(in_code))
            {
                return string.Empty;
            }

            var query = from item in DclCache
                        where (item.PtyItmId == itm_id || item.PtyItmFlag == 1) && item.PtyCCode == in_code//获取指定的项目id 或 公共特征
                        orderby item.PtyItmFlag ascending//指定项目的id放在最顶
                        select item;

            if (query.Count() > 0)
            {
                return query.First().PtyItmProperty;//返回符合要求的第一个项目特征
            }

            return string.Empty;
        }


        /// <summary>
        /// 多线程刷新缓存方法
        /// </summary>
        private void ThreadRefresh()
        {
            lock (padlock)//线程锁
            {
                try
                {
                    CacheDataBIZ cache = new CacheDataBIZ();
                    EntityResponse response = cache.GetCacheData("EntityDefItmProperty");
                    DclCache = response.GetResult<List<EntityDefItmProperty>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}
