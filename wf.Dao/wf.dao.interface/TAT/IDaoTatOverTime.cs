using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoTatOverTime
    {
 
        /// <summary>
        /// 保存超时信息
        /// </summary>
        /// <param name="overTime"></param>
        /// <returns></returns>
        Boolean SaveTatOverTime(EntityTatOverTime overTime);

        /// <summary>
        /// 获取条码超时信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        List<EntityTatOverTime> GetTatOverTime(string barCode);
    }
}
