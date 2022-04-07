using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 危急值消息表:接口
    /// </summary>
    [ServiceContract]
    public interface IDicObrMessageContent
    {
        /// <summary>
        /// 查询所有危急值消息数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicObrMessageContent> SearchAllMessageContent();

        /// <summary>
        /// 根据报告ID获取病人组合和危急值信息
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicObrMessageContent> GetBacPatientsMsg(string RepId);

        /// <summary>
        /// 保存危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveObrMessageContent(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 更新危急值消息数据(全部字段)
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateObrMessageContent(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 删除危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteObrMessageContent(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据危急值信息类型及标识ID值更新危急值信息表部分字段,含标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateObrMessageContentHaveIn(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据危急值信息类型及标识ID值更新危急值信息表部分字段，含删除标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateObrMessageContentHaveInDelFlag(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据信息ID更新查看者ID姓名和类型及标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateObrMsgConToInsignByID(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据信息ID更新删除标志,以及查看者ID姓名和类型
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateObrMsgConToDelFlagByID(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 获取LED消息
        /// </summary>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已过期</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicObrMessageContent> GetLEDMessage(bool bGetDeleted, bool bGetExpired);

        /// <summary>
        /// 更新删除标志，根据扩展字段A
        /// </summary>
        /// <param name="obrValueA"></param>
        /// <returns></returns>
        [OperationContract]
        bool UPdateMessageDelFlagByObrValueA(string obrValueA);

        /// <summary>
        /// 根据条件来查危急值信息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicObrMessageContent> GetMessageByCondition(EntityDicObrMessageContent content);

        /// <summary>
        /// 判断是否节假日
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool IsDateHoliday();

        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <param name="bGetReceiver">是否获取接收者</param>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已超时</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicObrMessageContent> GetAllMessage(bool bGetReceiver, bool bGetDeleted, bool bGetExpired);


        /// <summary>
        /// 获取自编危急值信息
        /// </summary>
        /// <param name="coondition">条件</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicObrMessageContent> GetDIYCriticalMsg(EntityDicObrMessageContent coondition);

        /// <summary>
        /// 添加自编危急值信息
        /// </summary>
        /// <param name="entityContent"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddDIYCriticalMsg(EntityDicObrMessageReceive entityReceive);
    }
}
