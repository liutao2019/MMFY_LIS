using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoUrgentObrMessage
    {
        /// <summary>
        /// 获取危急与急查信息
        /// </summary>
        /// <param name="pat_flag"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetUrgentMsgByPatFlag(string pat_flag);

        /// <summary>
        /// 查询内部提醒危急值(msg_content无数据时,来源为住院)
        /// </summary>
        /// <returns></returns>
        List<EntityPidReportMain> GetUrgentMsgToA();
        
        /// <summary>
        /// 查询自编危急信息
        /// </summary>
        /// <returns></returns>
        List<EntityPidReportMain> GetUrgentMsgToB();

        /// <summary>
        /// 查询回退标本信息
        /// </summary>
        /// <returns></returns>
        List<EntityPidReportMain> GetUrgentMsgToC();

        /// <summary>
        /// 根据病人信息来获取危急值处理信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetUrgentflagAndPatlookcodeByPatid(string pat_id);

        /// <summary>
        /// 获取危急值历史信息(sqlWhere)
        /// </summary>
        /// <param name="entityParame"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetUrgentHistoryMsgSqlWhere(EntityUrgentHistoryUseParame entityParame);

    }
}
