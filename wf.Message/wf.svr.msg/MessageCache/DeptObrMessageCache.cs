using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.msg
{
    /// <summary>
    /// （科室）危急值消息
    /// </summary>
    public class DeptObrMessageCache
    {
        #region singleton
        private static object objLock = new object();

        private static DeptObrMessageCache _instance = null;

        public static DeptObrMessageCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DeptObrMessageCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private DeptObrMessageCache()
        {
            this.cache = new ObrMessageReceiveCollection();
        }
        #endregion

        private ObrMessageReceiveCollection cache = null;

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = new ObrMessageReceiveBIZ().GetMessageByReceiverID(null, EnumObrMessageReceiveType.Dept, false, true, false);
        }

        /// <summary>
        /// 根据科室代码获取科室消息
        /// </summary>
        /// <param name="dept_code"></param>
        /// <returns></returns>
        public ObrMessageReceiveCollection GetMessage(string dept_code)
        {
            ObrMessageReceiveCollection msgRcvCollection = new ObrMessageReceiveCollection();

            msgRcvCollection = new ObrMessageBIZ().GetDeptMessageByDeptCode(dept_code);

            return msgRcvCollection;
        }

        /// <summary>
        /// 根据多科室代码获取科室消息
        /// </summary>
        /// <param name="dept_codes"></param>
        /// <returns></returns>
        public ObrMessageReceiveCollection GetMessageByDepts(string dept_codes)
        {
            ObrMessageReceiveCollection msgRecCollection = new ObrMessageReceiveCollection();

            msgRecCollection = new ObrMessageBIZ().GetMessageByDepts(dept_codes);

            return msgRecCollection;
        }

        public ObrMessageReceiveCollection GetMessage(string dept_code, EnumObrMessageType message_type)
        {
            if (dept_code == null)
            {
                var query = from item in this.cache
                            where item.ObrMessageContent.ObrType == Convert.ToInt32(message_type)
                            orderby item.ObrMessageContent.ObrCreateTime descending
                            select item;
                ObrMessageReceiveCollection list = new ObrMessageReceiveCollection(query);
                return list;
            }
            else
            {
                var query = from item in this.cache
                            where item.ObrUserId == dept_code && item.ObrMessageContent.ObrType == Convert.ToInt32(message_type)
                            orderby item.ObrMessageContent.ObrCreateTime descending
                            select item;

                ObrMessageReceiveCollection list = new ObrMessageReceiveCollection(query);
                return list;
            }
        }
    }

}
