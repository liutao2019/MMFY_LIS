using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.msg
{
    /// <summary>
    /// 用户危急值信息缓存(改造的最新文件)
    /// </summary>
    public class UserObrMessageCache
    {
        #region singleton
        private static object objLock = new object();

        private static UserObrMessageCache _instance = null;

        public static UserObrMessageCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserObrMessageCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private UserObrMessageCache()
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
        /// 获取用户消息，如果为空则获取所有
        /// </summary>
        /// <param name="userLoginID"></param>
        /// <returns></returns>
        public ObrMessageReceiveCollection GetUserMessage(string userLoginID)
        {
            if (userLoginID == null)
            {
                return cache;
            }
            else
            {
                var query = from item in this.cache
                            where item.ObrUserId == userLoginID
                            orderby item.ObrMessageContent.ObrCreateTime descending
                            select item;

                ObrMessageReceiveCollection list = new ObrMessageReceiveCollection(query);
                return list;
            }
        }

        public void DeleteReceivedMessage(string messageID, string userLoginID, bool bRemove)
        {
            if (bRemove)
            {
                this.cache.RemoveAll(i => i.ObrUserId == userLoginID && i.ObrId == messageID);
            }
            else
            {
                List<EntityDicObrMessageReceive> list = this.cache.FindAll(i => i.ObrUserId == userLoginID && i.ObrId == messageID);

                foreach (EntityDicObrMessageReceive item in list)
                {
                    item.DelFlag = true;
                }
            }
        }

        public void DeleteMessageContent(string messageID, bool bRemove)
        {
            if (bRemove)
            {
                this.cache.RemoveAll(i => i.ObrMessageContent.ObrId == messageID);
            }
            else
            {
                List<EntityDicObrMessageReceive> list = this.cache.FindAll(i => i.ObrMessageContent.ObrId == messageID);
                foreach (EntityDicObrMessageReceive item in list)
                {
                    item.ObrMessageContent.DelFlag = true;
                }
            }
        }
    }
}
