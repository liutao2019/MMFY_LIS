using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoSysLog
    {
        /// <summary>
        /// 保存系统日志
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool SaveSysLog(EntityLogLogin sample);
        /// <summary>
        /// 更新系统日志
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        bool UpdateSysLog(EntityLogLogin sample);
        /// <summary>
        /// 删除系统日志
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteSysLog(string timeFrom, string timeTo);

    
        /// <summary>
        /// 获得系统日志
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="module"></param>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <returns></returns>
        List<EntityLogLogin> GetSysLog(string loginId, string module, string timeFrom, string timeTo);

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        DateTime GetDatabaseServerDateTime();
    }
}
