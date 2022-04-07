using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 人员管理操作接口
    /// </summary>
    [ServiceContract]
    public interface IOfficeAttendance
    {
        /// <summary>
        /// 物理组
        /// </summary>
        /// <returns></returns>
        List<EntityDicPubProfession> GetPhyic();
        /// <summary>
        /// userIDandIsWork包含userID与上下班标志（true or false）
        /// 用分号;隔开    判断是否已经打卡
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        String GetAttdRecordByUID(string userIDandIsWork);

        /// <summary>
        /// 得到当天的考勤记录情况
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOaWorkAttendance> GetAttRecordByUID(DateTime sDTime, DateTime eDTime);

        /// <summary>
        /// 获得当前最大的AttdRecordID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        string GetMaxAttdRecordID();

        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertAttdRecord(EntityOaWorkAttendance sample);

        /// <summary>
        /// 更新一条记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        int ModifyAttdRecord(EntityOaWorkAttendance sample);

     

    }
}
