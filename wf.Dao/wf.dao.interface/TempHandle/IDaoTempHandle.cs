using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;

namespace dcl.dao.interfaces
{

    public interface IDaoTempHandle
    {
        /// <summary>
        /// 根据组别ID查询温控箱信息
        /// </summary>
        /// <param name="proId">实验组ID</param>
        /// <param name="datetime">监控日期</param>
        /// <returns></returns>
        List<EntityTemHarvester> GetTempHarvesterByProId(string proId, DateTime? datetime = null);

        /// <summary>
        /// 根据采集器ID查询温湿度采集信息
        /// </summary>
        /// <param name="dhId"></param>
        /// <returns></returns>
        List<EntityTemHarvester> GetHarvesterByDhId(string dhId);

    }
}
