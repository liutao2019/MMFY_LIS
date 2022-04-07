using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ITempHandle
    {
        /// <summary>
        /// 根据组别ID查询温控箱信息
        /// </summary>
        /// <param name="proId">实验室ID</param>
        /// <param name="datetime">监控日期</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityTemHarvester> GetTempHarvesterByProId(string proId, DateTime? datetime = null);

        /// <summary>
        /// 根据采集器ID查询温湿度采集信息
        /// </summary>
        /// <param name="dhId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityTemHarvester> GetHarvesterByDhId(string dhId);
    }
}
