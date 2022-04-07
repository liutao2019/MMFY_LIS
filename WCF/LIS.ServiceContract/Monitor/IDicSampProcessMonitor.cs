using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 标本进程监控:接口
    /// </summary>
    [ServiceContract]
    public interface IDicSampProcessMonitor
    {
        /// <summary>
        /// 查询条码主索引表数据(标本进程监控数据)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampProcessMonitor> GetBCPatients();


        /// <summary>
        /// 查询标本数量
        /// </summary>
        /// <param name="OperationCode">标本操作步骤</param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampProcessMonitor> GetSampCount(int OperationCode);
    }
}
