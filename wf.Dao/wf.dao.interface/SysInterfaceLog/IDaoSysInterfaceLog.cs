using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 操作记录Dao接口文件
    /// </summary>
    public interface IDaoSysInterfaceLog
    {
        /// <summary>
        /// 查询操作记录表数据
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        List<EntitySysInterfaceLog> GetSysInterfaceLogData(EntitySysInterfaceLog eySysInfeLog);

        /// <summary>
        /// 查询一定数量操作记录
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        List<EntitySysInterfaceLog> GetSysInterfaceLogInNumber(EntitySysInterfaceLog eySysInfeLog, int number);

        /// <summary>
        /// 保存操作记录数据
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        bool SaveSysInterfaceLog(EntitySysInterfaceLog eySysInfeLog);

        /// <summary>
        /// 删除操作记录数据
        /// </summary>
        /// <param name="strOperationKey"></param>
        /// <returns></returns>
        bool DeleteSysInterfaceLog(int strOperationKey);
    }
}
