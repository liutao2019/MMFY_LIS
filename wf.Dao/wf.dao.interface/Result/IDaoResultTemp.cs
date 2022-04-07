using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoResultTemp
    {
        /// <summary>
        /// 根据条件获取病人样本号
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<string> GetPatientsSid(EntityAnanlyseQC query);
    }
}
