using System;
using System.Collections.Generic;
using System.Threading;
using Lib.LogManager;
using dcl.entity;

namespace dcl.svr.cache
{
    public class CacheFunctionInfo : IServerCache<EntitySysFunction>
    {
        private static CacheFunctionInfo _instance = null;
        private static object padlock = new object();

        public static CacheFunctionInfo Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CacheFunctionInfo();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntitySysFunction> Cache { get; set; }

        private CacheFunctionInfo()
        {
            ThreadRefresh();
        }

        public List<EntitySysFunction> GetAll()
        {
            return this.Cache;
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
                    EntityResponse response = new CacheDataBIZ().GetCacheData("EntitySysFunction");
                    Cache = response.GetResult() as List<EntitySysFunction>;
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// 根据功能代码获取功能信息
        /// </summary>
        /// <param name="func_code"></param>
        /// <returns></returns>
        public EntitySysFunction GetFunctionInfoByCode(string func_code)
        {
            EntitySysFunction funcInfo = this.Cache.Find(i => i.FuncCode == func_code);
            return funcInfo;
        }
    }
}
