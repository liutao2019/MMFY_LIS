using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using dcl.entity;

namespace dcl.svr.cache
{
    public class DictInstrmtCache : IServerCache<EntityDicInstrument>
    {
        private static DictInstrmtCache _instance = null;
        private static object padlock = new object();

        public static DictInstrmtCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictInstrmtCache();
                        }
                    }
                }
                return _instance;
            }
        }


        public List<EntityDicInstrument> DclCache { get; private set; }

        private DictInstrmtCache()
        {
            ThreadRefresh();
        }

        public List<EntityDicInstrument> GetAll()
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
                    EntityResponse response = cache.GetCacheData("EntityDicInstrument");
                    DclCache = response.GetResult<List<EntityDicInstrument>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 根据仪器id获取仪器
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public EntityDicInstrument GetInstructmentByID(string itr_id)
        {
            EntityDicInstrument item = this.DclCache.Find(i => i.ItrId == itr_id);
            return item;
        }

        /// <summary>
        /// 获取仪器默认审核者
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrDefaultAuditerCode(string itr_id)
        {
            var query = from itr in this.DclCache
                        join user in CacheUserInfo.Current.Cache on itr.ItrReportChkId equals user.UserLoginid
                        select user.UserLoginid;


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
