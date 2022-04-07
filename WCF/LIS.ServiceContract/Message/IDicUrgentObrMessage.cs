using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 危急值数据:接口
    /// </summary>
    [ServiceContract]
    public interface IDicUrgentObrMessage
    {
        /// <summary>
        /// 获取所有未确认的危机值信息(默认只取最近五天数据)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentMsgToCache();

        /// <summary>
        /// 获取危急与急查信息
        /// </summary>
        /// <param name="pat_flag"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentMsgByPatFlag(string pat_flag);

        /// <summary>
        /// 查询内部提醒危急值(msg_content无数据时,来源为住院)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentMsgToA();

        /// <summary>
        /// 查询自编危急信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentMsgToB();

        /// <summary>
        /// 查询回退标本信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentMsgToC();

        /// <summary>
        /// 获取危急值历史信息(getCacheByDep)
        /// </summary>
        /// <param name="entityParame"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentHistoryMsgByDep(EntityUrgentHistoryUseParame entityParame);

        /// <summary>
        /// 获取危急值历史信息(sqlWhere)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentHistoryMsgSqlWhere(EntityUrgentHistoryUseParame entityParame);

        /// <summary>
        /// 获取危急值历史信息(getAll)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentHistoryMsgGetAll(EntityUrgentHistoryUseParame entityParame);

        /// <summary>
        /// 刷新危急值缓存信息
        /// </summary>
        [OperationContract]
        void RefreshUrgentMessage();


    }
}
