using dcl.entity;
using dcl.svr.msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.msg
{
    /// <summary>
    /// 危急值数据
    /// </summary>
    public class UrgentObrMessageCache
    {
        #region singleton
        private static object objLock = new object();

        private static UrgentObrMessageCache _instance = null;

        public static UrgentObrMessageCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UrgentObrMessageCache();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 危机值信息缓存
        /// </summary>
        public List<EntityPidReportMain> cache = null;

        /// <summary>
        /// 科室字典信息缓存
        /// </summary>
        //private DataTable cacheDepInfo = null;

        private UrgentObrMessageCache()
        {
            this.cache = new List<EntityPidReportMain>();
            //this.cache = new DataTable();
            //this.cache.TableName = "UrgentMsgCache";
        }
        #endregion

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            //this.cache = new MessageBiz().GetUrgentMsgToCache();
            this.cache = new UrgentObrMessageBIZ().GetUrgentMsgToCache();
        }

        /// <summary>
        /// 根据筛选条件获取危急值数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetDTUrgentMessage(string strWhere)
        {
            if (strWhere == null)
            {
                return this.cache;
            }
            else
            {
                try
                {
                    if (this.cache != null && this.cache.Count > 0)
                    {
                        //已经在BIZ层面过滤掉了，就不用下面的代码
                        //DataTable dtCope = this.cache.Clone();
                        //DataRow[] drArray = this.cache.Select(strWhere);

                        //foreach (DataRow drItem in drArray)
                        //{
                        //    dtCope.Rows.Add(drItem.ItemArray);
                        //}
                        //dtCope.TableName = "UrgentMsgCache";
                        //return dtCope;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取缓存危急值数据", ex);
                }
                return this.cache;
            }
        }
    }
}
