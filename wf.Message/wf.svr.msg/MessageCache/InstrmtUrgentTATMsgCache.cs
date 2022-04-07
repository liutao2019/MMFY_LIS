using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.msg
{
    /// <summary>
    /// 仪器危急值数据
    /// </summary>
    public class InstrmtUrgentTATMsgCache
    {
        #region singleton
        private static object objLock = new object();

        private static InstrmtUrgentTATMsgCache _instance = null;

        /// <summary>
        /// 当时是否没在处理
        /// </summary>
        private static bool IsCurrNotDisposal { get; set; }

        public static InstrmtUrgentTATMsgCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new InstrmtUrgentTATMsgCache();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 危机值信息缓存
        /// </summary>
        public List<EntityDicMsgTAT> cache = null;

        /// <summary>
        /// 仪器危急值数据
        /// </summary>
        private InstrmtUrgentTATMsgCache()
        {
            this.cache = new List<EntityDicMsgTAT>();
            IsCurrNotDisposal = true;
        }
        #endregion

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = this.GetInstrmtUrgentMsgToCache();
        }

        /// <summary>
        /// 获取仪器危急值数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetInstrmtUrgentMsgToCache()
        {
            try
            {
                //总开关--是否启动仪器危急值提醒
                bool UrgentMessage_ShowInstrmtMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Urgent_Instrmt_IsNotify") == "是";

                if (!UrgentMessage_ShowInstrmtMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            List<EntityDicMsgTAT> listbResult = new List<EntityDicMsgTAT>();
            listbResult = new InstrmtUrgentTATMsgBIZ().GetInstrmtUrgentMsgToCacheDao();

            return listbResult;
        }


    }
}
