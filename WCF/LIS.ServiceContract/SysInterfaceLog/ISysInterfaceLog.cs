using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 操作记录接口文件
    /// </summary>
    [ServiceContract]
    public interface ISysInterfaceLog
    {
        /// <summary>
        /// 查询操作记录表数据
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysInterfaceLog> GetSysInterfaceLogData(EntitySysInterfaceLog eySysInfeLog);

        /// <summary>
        /// 保存操作记录数据
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveSysInterfaceLog(EntitySysInterfaceLog eySysInfeLog);
    }
}
