using System;
using System.Collections.Generic;
using System.Threading;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.cache
{
    public class SampReturnCache
    {
        private static SampReturnCache _instance = null;
        private static object padlock = new object();

        public static SampReturnCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new SampReturnCache();
                        }
                    }
                }
                return _instance;
            }
        }
        public List<EntitySampReturn> DclCache { get; private set; }

        private SampReturnCache()
        {
            DclCache = new List<EntitySampReturn>();//新增(避免空值对象引用)

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
                    EntitySampQC sampQc = new EntitySampQC();
                    sampQc.StartDate = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss");
                    sampQc.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    sampQc.HandleProc = ReturnProc.全部;
                    IDaoSampReturn daoDetail = DclDaoFactory.DaoHandler<IDaoSampReturn>();
                    if (daoDetail != null)
                        DclCache = daoDetail.GetSampReturn(sampQc);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }

    }
}
