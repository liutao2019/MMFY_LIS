using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 用户危急值接口文件
    /// </summary>
    [ServiceContract]
    public interface IUserMessage
    {
        /// <summary>
        /// 获取用户消息，如果为空则获取所有
        /// </summary>
        /// <param name="userLoginID"></param>
        /// <returns></returns>
        [OperationContract]
        ObrMessageReceiveCollection Cache_GetUserMessage(string userLoginID);


        /// <summary>
        /// 根据消息发送者ID获取消息
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="bGetReceiver"></param>
        /// <param name="bGetDeleted"></param>
        /// <param name="bGetExpired"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetMessageBySenderID(string senderID, bool bGetReceiver, bool bGetDeleted, bool bGetExpired);

        /// <summary>
        /// 插入消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse InsertMessage(EntityDicObrMessageContent message);

        /// <summary>
        /// 删除危急值处理表数据
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="receiverType"></param>
        /// <param name="receiverID"></param>
        /// <param name="bPhiDelete"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteMessageReceiver(string messageID, EnumObrMessageReceiveType receiverType, string receiverID, bool bPhiDelete);
    }
}
