using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.svr.users;
using dcl.svr.oa;
using dcl.svr.sample;
using dcl.svr.cache;

namespace dcl.svr.msg
{
    /// <summary>
    /// 用户危急值信息BIZ文件
    /// </summary>
    public class UserMessageBIZ : IUserMessage
    {
        public ObrMessageReceiveCollection Cache_GetUserMessage(string userLoginID)
        {
            return UserObrMessageCache.Current.GetUserMessage(userLoginID);
        }

        public bool DeleteMessageReceiver(string messageID, EnumObrMessageReceiveType receiverType, string receiverID, bool bPhiDelete)
        {
            bool isDelete = false;

            EntityDicObrMessageReceive eyMsgReceive = new EntityDicObrMessageReceive();
            eyMsgReceive.ObrId = messageID;
            eyMsgReceive.ObrType = (int)receiverType;
            eyMsgReceive.ObrUserId = receiverID;
            eyMsgReceive.LogicalDelete = bPhiDelete;//是否逻辑删除标志

            isDelete = new ObrMessageReceiveBIZ().UpdateObrMsgReciveToDateDelFlagByID(eyMsgReceive);//更新确认时间和删除标志

            UserObrMessageCache.Current.DeleteReceivedMessage(messageID, receiverID, bPhiDelete);

            return isDelete;
        }

        public EntityResponse GetMessageBySenderID(string senderID, bool bGetReceiver, bool bGetDeleted, bool bGetExpired)
        {
            EntityResponse response = new EntityResponse();

            List<EntityDicObrMessageContent> listRet = new List<EntityDicObrMessageContent>();
            listRet = new ObrMessageContentBIZ().SearchAllMessageContent();//获取全部危急值消息

            if (bGetDeleted)
            {
                listRet = listRet.Where(w => w.ObrSendUserId == senderID).ToList();
            }
            else
            {
                listRet = listRet.Where(w => w.ObrSendUserId == senderID && w.DelFlag == false).ToList();//0：是未删除,转换为bool类型就是false
            }

            if (bGetExpired)
            {

            }
            else
            {
                DateTime dateTime = ServerDateTime.GetDatabaseServerDateTime();//获取服务器时间
                listRet = listRet.Where(w => w.ObrExpirationDate == null && dateTime <= w.ObrExpirationDate).ToList();
            }

            foreach (var info in listRet)
            {
                if (bGetReceiver)//获取接收者
                {
                    info.ListObrMessageReceiver = GetMessageReceiverByMessageID(info.ObrId);
                }
            }
            response.SetResult(listRet);

            return response;
        }

        /// <summary>
        /// 根据消息ID获取消息接收者
        /// </summary>
        /// <param name="messageID"></param>
        /// <returns></returns>
        public ObrMessageReceiveCollection GetMessageReceiverByMessageID(string messageID)
        {
            ObrMessageReceiveCollection listReceiveCollection = new ObrMessageReceiveCollection();

            EntityDicObrMessageReceive eyMsgReceice = new EntityDicObrMessageReceive();
            eyMsgReceice.ObrId = messageID;

            List<EntityDicObrMessageReceive> listMsgReceive = new ObrMessageReceiveBIZ().SearchObrMessageReceive(eyMsgReceice);//查询危急值处理表数据根据信息ID

            foreach (var info in listMsgReceive)
            {
                listReceiveCollection.Add(info);
            }

            return listReceiveCollection;
        }

        public EntityResponse InsertMessage(EntityDicObrMessageContent message)
        {
            EntityResponse response = new EntityResponse();

            message.ObrId = Guid.NewGuid().ToString().ToLower().Replace("-", "");//生成新的消息ID
            message.ObrCreateTime = ServerDateTime.GetDatabaseServerDateTime();

            message.ListObrMessageReceiver = GenerateMessageReceiver(message);//生成消息接收者

            bool isSaveContent = new ObrMessageContentBIZ().SaveObrMessageContent(message);//保存危急值消息数据
            bool isSaveReceive = true;

            foreach (EntityDicObrMessageReceive item in message.ListObrMessageReceiver)//遍历接收者生成消息接收者记录
            {
                bool isSave = false;
                item.ObrId = message.ObrId;
                isSave = new ObrMessageReceiveBIZ().SaveObrMessageReceive(item); //保存危急值处理表数据
                if (isSave == false)
                {
                    isSaveReceive = false;
                }
            }

            if (isSaveContent && isSaveReceive)
            {
                response.Scusess = true;
                response.ErroMsg = "InsertMessage插入成功!";
            }
            else
            {
                response.ErroMsg = "插入信息出错InsertMessage";
            }

            return response;
        }

        /// <summary>
        /// 生成消息接收者
        /// </summary>
        /// <param name="entityMessage"></param>
        /// <returns></returns>
        private ObrMessageReceiveCollection GenerateMessageReceiver(EntityDicObrMessageContent entityMessage)
        {
            ObrMessageReceiveCollection list = new ObrMessageReceiveCollection();

            foreach (var item in entityMessage.ListObrMessageReceiver)
            {
                if (item.ObrType == 2)//如果接收者为角色，则转换为用户
                {
                    string role_id = item.ObrUserId;
                    EntityUserQc eyUserQc = new EntityUserQc();
                    eyUserQc.RoleId = role_id;
                    List<EntitySysUser> listUser = new SysUserInfoBIZ().SysUserQuery(eyUserQc);//根据角色ID获取用户权限

                    foreach (var infoUser in listUser)
                    {
                        EntityDicObrMessageReceive entityReceiver = new EntityDicObrMessageReceive();
                        entityReceiver.ObrId = entityMessage.ObrId;
                        entityReceiver.ObrType = 1;
                        entityReceiver.ObrUserId = infoUser.UserId;
                        entityReceiver.ObrUserName = infoUser.UserName;

                        if (!list.Exists(i => i.ObrUserId == entityReceiver.ObrUserId && i.ObrType == entityReceiver.ObrType))
                        {
                            list.Add(entityReceiver);
                        }
                    }
                }
                else
                {
                    item.ObrId = entityMessage.ObrId;
                    list.Add(item);
                }
            }

            return list;
        }
    }
}
