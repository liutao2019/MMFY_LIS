using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoObrMessageContent
    {
        /// <summary>
        /// 查询所有危急值消息数据
        /// </summary>
        /// <returns></returns>
        List<EntityDicObrMessageContent> SearchAllMessageContent();

        /// <summary>
        /// 根据报告ID获取病人组合和危急值信息
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        List<EntityDicObrMessageContent> GetBacPatientsMsg(string RepId);

        /// <summary>
        /// 保存危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        bool SaveObrMessageContent(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 更新危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        bool UpdateObrMessageContent(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 删除危急值消息数据
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        bool DeleteObrMessageContent(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据危急值信息类型及标识ID值更新危急值信息表部分字段,含标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        bool UpdateObrMessageContentHaveIn(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据危急值信息类型及标识ID值更新危急值信息表部分字段，含删除标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        bool UpdateObrMessageContentHaveInDelFlag(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据信息ID更新查看者ID姓名和类型及标志
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        bool UpdateObrMsgConToInsignByID(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 根据信息ID更新删除标志,以及查看者ID姓名和类型
        /// </summary>
        /// <param name="msgContent"></param>
        /// <returns></returns>
        bool UpdateObrMsgConToDelFlagByID(EntityDicObrMessageContent msgContent);

        /// <summary>
        /// 获取LED消息
        /// </summary>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已过期</param>
        /// <returns></returns>
        List<EntityDicObrMessageContent> GetLEDMessage(bool bGetDeleted, bool bGetExpired);

        /// <summary>
        /// 根据msg_ext1更新内部消息提示标志
        /// </summary>
        /// <param name="RepId"></param>
        /// <returns></returns>
        bool UpdateObrMsgConToInsignByRepID(string RepId);

        /// <summary>
        /// 根据条件来查危急值信息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        List<EntityDicObrMessageContent> GetMessageByCondition(EntityDicObrMessageContent content);

        /// <summary>
        /// 更新删除标志
        /// </summary>
        /// <param name="valueA">扩展字段</param>
        /// <param name="type1">危急值类型1</param>
        /// <param name="type2">危急值类型2</param>
        /// <returns></returns>
        bool UpdateMessageDelFlag(string valueA, int type1, int type2);

        /// <summary>
        /// 更新删除标志，根据扩展字段A
        /// </summary>
        /// <param name="obrValueA"></param>
        /// <returns></returns>
        bool UPdateMessageDelFlagByObrValueA(string obrValueA);

        /// <summary>
        /// 判断是否节假日
        /// </summary>
        /// <returns></returns>
        bool IsDateHoliday();

        /// <summary>
        /// 获取所有消息
        /// </summary>
        /// <param name="bGetDeleted">是否获取已删除</param>
        /// <param name="bGetExpired">是否获取已超时</param>
        /// <returns></returns>
        List<EntityDicObrMessageContent> GetAllMessage( bool bGetDeleted, bool bGetExpired);

        /// <summary>
        /// 获取自编危急值信息
        /// </summary>
        /// <param name="coondition">条件</param>
        /// <returns></returns>
        List<EntityDicObrMessageContent> GetDIYCriticalMsg(EntityDicObrMessageContent coondition);

        /// <summary>
        ///根据危急值消息和确认角色更新医生或者护士确认标志
        /// </summary>
        /// <param name="userRole">角色</param>
        /// <param name="messageID"></param>
        /// <returns></returns>
        bool UpdateReadFlag(string userRole,string obr_id);
    }
}
