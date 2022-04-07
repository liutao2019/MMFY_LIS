using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoOaWorkAttendance
    {

        /// <summary>
        /// userIDandIsWork包含userID与上下班标志（true or false）
        /// 用分号;隔开
        /// </summary>
        /// <param name="userIDandIsWork"></param>
        /// <returns></returns>
        string GetAttdRecordByUID(string []userIDandIsWork);
        /// <summary>
        /// 获得当前最大的AttdRecordID
        /// </summary>
        /// <returns></returns>
        string GetMaxAttdRecordID();
        /// <summary>
        /// 得到当天的考勤记录情况
        /// </summary>
        /// <param name="sDTime"></param>
        /// <param name="eDTime"></param>
        /// <returns></returns>
        List<EntityOaWorkAttendance> GetAttRecordByUID(DateTime sDTime, DateTime eDTime);

        /// <summary>
        /// 更新下班时间
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int ModifyAttdRecord(EntityOaWorkAttendance sample);

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int InsertAttdRecord(EntityOaWorkAttendance sample);
    }
}
