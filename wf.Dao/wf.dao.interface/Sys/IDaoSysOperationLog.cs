using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using dcl.dao.core;

namespace dcl.dao.interfaces
{
    public interface IDaoSysOperationLog : IDaoBase
    {
        /// <summary>
        /// 保存操作日志
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool SaveSysOperationLog(EntitySysOperationLog sample);
        /// <summary>
        /// 更新操作日志
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool UpdateSysOperationLog(EntitySysOperationLog sample);
        /// <summary>
        /// 删除操作日志
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool DeleteSysOperationLog(EntitySysOperationLog sample);
        /// <summary>
        /// 获得病人信息
        /// </summary>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatients(string timeFrom,string timeTo,string patNo,string itrType,string patInNo,string patName,string patItrId,string patCheck);
       
        
        /// <summary>
        /// 查询操作日志
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<EntitySysOperationLog> SearchSysOperationLog(EntityLogQc qc);

        /// <summary>
        /// 查询质控操作日志
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        List<EntitySysOperationLog> SearchSysQcOperationLog(EntityLogQc qc);

        /// <summary>
        /// 获得病人Id
        /// </summary>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        String GetPatId(string timeFrom,string timeTo,string patID,string userName);
    }
}
