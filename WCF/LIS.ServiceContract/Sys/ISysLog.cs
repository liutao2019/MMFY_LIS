using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 系统日志操作接口
    /// </summary>
    [ServiceContract]
    public interface ISysLog
    {
        /// <summary>
        /// 新增系统日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        bool NewSysLog(EntityLogLogin request);

        /// <summary>
        /// 删除系统日志
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteSysLog(string timeFrom,string timeTo);
        /// <summary>
        /// 更新系统日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateReporSysLog(EntityRequest request);
        /// <summary>
        /// 获得系统日志
        /// </summary>
        /// <param name="logId"></param>
        /// <param name="module"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityLogLogin> GetSysLog(string logId,string module,string timeFrom,string timeTo);
        /// <summary>
        /// 获得模块
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysFunction> GetFuncName();
        /// <summary>
        /// 获得用户ID
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysUser> GetLoginId();

    }
}
