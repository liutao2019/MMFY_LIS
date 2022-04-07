using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoObrMessageReceive
    {
        /// <summary>
        /// 查询危急值处理表数据
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        List<EntityDicObrMessageReceive> SearchObrMessageReceive(EntityDicObrMessageReceive msgReceive);

        /// <summary>
        /// 保存危急值处理表数据
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        bool SaveObrMessageReceive(EntityDicObrMessageReceive msgReceive);

        /// <summary>
        /// 更新危急值处理表数据
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        bool UpdateObrMessageReceive(EntityDicObrMessageReceive msgReceive);

        /// <summary>
        /// 删除危急值处理表数据
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        bool DeleteObrMessageReceive(EntityDicObrMessageReceive msgReceive);

        /// <summary>
        /// 根据接收者ID获取消息
        /// </summary>
        /// <param name="receiverID">接收者ID,如果为null则获取所有</param>
        /// <param name="receiverType"></param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetReaded">是否获取已阅消息</param>
        /// <param name="bGetExpired">是否获取已超时消息</param>
        /// <returns></returns>
        ObrMessageReceiveCollection GetMessageByReceiverID(string receiverID, EnumObrMessageReceiveType receiverType, bool bGetDeleted, bool bGetReaded, bool bGetExpired);

        /// <summary>
        /// 更新确认时间,根据信息ID
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        bool UpdateObrMsgReciveToDateByID(EntityDicObrMessageReceive msgReceive);

        /// <summary>
        /// 更新确认时间和删除标志,根据信息ID
        /// </summary>
        /// <param name="msgReceive"></param>
        /// <returns></returns>
        bool UpdateObrMsgReciveToDateDelFlagByID(EntityDicObrMessageReceive msgReceive);
    }
}
