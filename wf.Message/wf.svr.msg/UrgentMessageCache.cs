using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace dcl.svr.msg
{
    /// <summary>
    /// 危急值数据
    /// </summary>
    public class UrgentMessageCache
    {
        #region singleton
        private static object objLock = new object();

        private static UrgentMessageCache _instance = null;

        public static UrgentMessageCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UrgentMessageCache();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 危机值信息缓存
        /// </summary>
        private DataTable cache = null;

        /// <summary>
        /// 科室字典信息缓存
        /// </summary>
        //private DataTable cacheDepInfo = null;

        private UrgentMessageCache()
        {
            this.cache = new DataTable();
            this.cache.TableName = "UrgentMsgCache";
        }
        #endregion

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = new MessageBiz().GetUrgentMsgToCache();
        }

        /// <summary>
        /// 根据筛选条件获取危急值数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetDTUrgentMessage(string strWhere)
        {
            if (strWhere == null)
            {
                return this.cache;
            }
            else
            {
                try
                {
                    if (this.cache != null && this.cache.Rows.Count > 0)
                    {
                        DataTable dtCope = this.cache.Clone();
                        DataRow[] drArray = this.cache.Select(strWhere);

                        foreach (DataRow drItem in drArray)
                        {
                            dtCope.Rows.Add(drItem.ItemArray);
                        }
                        dtCope.TableName = "UrgentMsgCache";
                        return dtCope;
                    }
                }
                catch(Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取缓存危急值数据", ex);
                }
                return this.cache;
            }
        }
    }
}
