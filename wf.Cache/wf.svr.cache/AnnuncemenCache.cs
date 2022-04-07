using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Lib.DAC;
using dcl.entity;

namespace dcl.svr.cache
{
    public class AnnuncemenCache
    {
        private static AnnuncemenCache _instance = null;
        private static object padlock = new object();

        public static AnnuncemenCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AnnuncemenCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public DataTable Cache { get; set; }
        public List<EntityOaAnnouncementReceive> DclCache { get; set; }
        #region .ctor

        private AnnuncemenCache()
        {
            ThreadRefresh();
        }

        #endregion
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
                    EntityResponse response = cache.GetCacheData("EntityOaAnnouncementReceive");
                    DclCache = response.GetResult<List<EntityOaAnnouncementReceive>>();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }

    }
}
