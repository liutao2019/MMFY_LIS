using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities.Message;

namespace dcl.svr.msg
{
    public class UserMessageCache
    {
        #region singleton
        private static object objLock = new object();

        private static UserMessageCache _instance = null;

        public static UserMessageCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserMessageCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private UserMessageCache()
        {
            this.cache = new MessageReceiverCollection();
        }
        #endregion

        private MessageReceiverCollection cache = null;

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = new MessageBiz().GetMessageByReceiverID(null, EnumMessageReceiverType.User, true, true, false);
        }

        /// <summary>
        /// 获取用户消息，如果为空则获取所有
        /// </summary>
        /// <param name="userLoginID"></param>
        /// <returns></returns>
        public MessageReceiverCollection GetUserMessage(string userLoginID)
        {
            if (userLoginID == null)
            {
                return cache;
            }
            else
            {
                var query = from item in this.cache
                            where item.ReceiverID == userLoginID
                            orderby item.MessageContent.CreateTime descending
                            select item;

                MessageReceiverCollection list = new MessageReceiverCollection(query);
                return list;
            }
        }

        public void DeleteReceivedMessage(string messageID, string userLoginID, bool bRemove)
        {
            if (bRemove)
            {
                this.cache.RemoveAll(i => i.ReceiverID == userLoginID && i.MessageID == messageID);
            }
            else
            {
                List<EntityMessageReceiver> list = this.cache.FindAll(i => i.ReceiverID == userLoginID && i.MessageID == messageID);

                foreach (EntityMessageReceiver item in list)
                {
                    item.Deleted = true;
                }
            }
        }

        public void DeleteMessageContent(string messageID, bool bRemove)
        {
            if (bRemove)
            {
                this.cache.RemoveAll(i => i.MessageContent.MessageID == messageID);
            }
            else
            {
                List<EntityMessageReceiver> list = this.cache.FindAll(i => i.MessageContent.MessageID == messageID);
                foreach (EntityMessageReceiver item in list)
                {
                    item.MessageContent.Deleted = true;
                }
            }
        }

        //public EntityMessageContent GetMessageContentByMsgID(string messageID)
        //{
        //    EntityMessageContent msg = this.cache.Find(i => i.MessageID == messageID);
        //    return msg;
        //}

        //public List<EntityMessageContent> GetMessageByType(EnumMessageType messageType)
        //{
        //    List<EntityMessageContent> list = this.cache.FindAll(i => i.MessageType == messageType);
        //    return list;
        //}

        //public List<EntityMessageContent> GetMessageByReceiverID(string receiverID, EnumMessageReceiverType receiverType)
        //{
        //    var qurey = from item in cache
        //                where item.ListMessageReceiver.Exists(i => i.ReceiverID == receiverID && i.ReceiverType == receiverType)
        //                select item;

        //    return new List<EntityMessageContent>(qurey);
        //}

        //public List<EntityMessageContent> GetMessageBySenderID(string senderID)
        //{
        //    var query = from item in cache
        //                where item.SenderID == senderID
        //                orderby item.CreateTime descending
        //                select item;
        //    List<EntityMessageContent> list = new List<EntityMessageContent>(query);
        //    return list;
        //}

        ///// <summary>
        ///// 获取用户消息
        ///// </summary>
        ///// <param name="userLoginID"></param>
        ///// <returns></returns>
        //public EntityUserMessage GetUserMessage(string userLoginID)
        //{
        //    EntityUserMessage entity = new EntityUserMessage();

        //    //获取已发送消息
        //    entity.ListSendedMessage = GetMessageBySenderID(userLoginID);

        //    //获取接受者为userLoginID的消息
        //    List<EntityMessageContent> listReceived = GetMessageByReceiverID(userLoginID, EnumMessageReceiverType.User);

        //    //获取公告消息
        //    List<EntityMessageContent> listBulletin = GetMessageByType(EnumMessageType.BULLETIN);

        //    List<EntityMessageContent> listMsgReceived = new List<EntityMessageContent>();
        //    List<EntityMessageContent> listMsgDeleted = new List<EntityMessageContent>();

        //    foreach (EntityMessageContent item in listReceived)
        //    {
        //        EntityMessageReceiver currentRec = item.ListMessageReceiver.Find(i => i.ReceiverID == userLoginID);

        //        if (currentRec != null)
        //        {
        //            if (currentRec.Deleted)
        //            {
        //                listMsgDeleted.Add(item);
        //            }
        //            else
        //            {
        //                listMsgReceived.Add(item);
        //            }
        //        }
        //    }

        //    foreach (EntityMessageContent item in listBulletin)
        //    {
        //        listMsgReceived.Add(item);
        //    }

        //    var query = from item in listMsgReceived
        //                orderby item.CreateTime descending
        //                select item;

        //    entity.ListReceivedMessage = new List<EntityMessageContent>(query);


        //    query = from item in listMsgDeleted
        //            orderby item.CreateTime descending
        //            select item;

        //    entity.ListDeletedMessage = new List<EntityMessageContent>(query);

        //    return entity;
        //}

        //internal void DeleteMessageByID(string messageID)
        //{
        //    EntityMessageContent msg = GetMessageContentByMsgID(messageID);
        //    if (msg != null)
        //    {
        //        this.cache.Remove(msg);
        //    }
        //}
    }
}
