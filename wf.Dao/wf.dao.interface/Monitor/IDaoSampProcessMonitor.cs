using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampProcessMonitor
    {
        /// <summary>
        /// 查询条码主索引表数据(标本进程监控数据)
        /// </summary>
        /// <param name="valueOutTime"></param>
        /// <returns></returns>
        List<EntitySampProcessMonitor> GetBCPatients(string valueOutTime);

        /// <summary>
        /// 统计常规标本监控
        /// </summary>
        /// <param name="OperationCode">操作步骤</param>
        /// <returns></returns>
        List<EntitySampProcessMonitor> GetSampCount(int OperationCode);
    }
}
