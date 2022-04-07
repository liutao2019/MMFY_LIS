using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoItrInstrumentServicing
    {
        /// <summary>
        /// 查询仪器故障信息数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        List<EntityDicItrInstrumentServicing> GetServicingData(EntityDicItrInstrumentServicing strWhere);

        /// <summary>
        /// 插入维修上报数据
        /// </summary>
        /// <param name="servicing"></param>
        /// <returns></returns>
        bool ServicingPutIn(EntityDicItrInstrumentServicing servicing);

        /// <summary>
        /// 更新维修上报数据
        /// </summary>
        /// <param name="servicing"></param>
        /// <returns></returns>
        bool ServingHandle(EntityDicItrInstrumentServicing servicing);

        /// <summary>
        /// 更新仪器维修日期和内容
        /// </summary>
        /// <param name="servicing"></param>
        /// <returns></returns>
        bool ServingAudit(EntityDicItrInstrumentServicing servicing);

        /// <summary>
        /// 查询维修记录数据
        /// </summary>
        /// <param name="itrId"></param>
        /// <returns></returns>
        List<EntityDicItrInstrumentServicing> GetServicing(string itrId);
        
    }
}
