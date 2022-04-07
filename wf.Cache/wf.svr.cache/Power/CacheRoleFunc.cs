using System;
using System.Collections.Generic;
using System.Threading;
using dcl.entity;

namespace dcl.svr.cache
{
    public class CacheRoleFunc : IServerCache<EntitySysRoleFunction>
    {
        private static CacheRoleFunc _instance = null;
        private static object padlock = new object();

        public static CacheRoleFunc Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheRoleFunc();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntitySysRoleFunction> Cache { get; set; }

        #region .ctor

        private CacheRoleFunc()
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
                    EntityResponse response = new CacheDataBIZ().GetCacheData("EntitySysRoleFunction");
                    Cache = response.GetResult() as List<EntitySysRoleFunction>;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
        }

        public List<EntitySysRoleFunction> GetAll()
        {
            return this.Cache;
        }
    }
}
