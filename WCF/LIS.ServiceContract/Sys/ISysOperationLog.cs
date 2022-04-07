using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 修改日志操作接口
    /// </summary>
    [ServiceContract]
    public interface ISysOperationLog
    {
        /// <summary>
        /// 获得操作记录
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysOperationLog> GetOperationLog(EntityLogQc qc);

        /// <summary>
        /// 获得质控操作记录
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysOperationLog> GetQcOperationLog(EntityLogQc qc);

        /// <summary>
        /// 获得病人信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetPatients(string timeFrom, string timeTo, string patNo, string itrType, string patInNo, string patName, string patItrId, string patCheck);

        /// <summary>
        /// 获得标本流转信息
        /// </summary>
        /// <param name="repId">条码号</param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampProcessDetail> GetSampProcessDetail(String repId);

        /// <summary>
        /// 获得病人id
        /// </summary>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="patId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPatId(string timeFrom, string timeTo, string patId, string userName);

        /// <summary>
        /// 获取已被删除的patid
        /// </summary>
        /// <param name="patId"></param>
        /// <param name="patName"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        [OperationContract]
        string GetDeletePatId(string patId, string patName, string timeFrom, string timeTo);

        /// <summary>
        /// 插入一条日志记录
        /// </summary>
        /// <param name="operationLog"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveSysOperationLog(EntitySysOperationLog operationLog);
    }
}
