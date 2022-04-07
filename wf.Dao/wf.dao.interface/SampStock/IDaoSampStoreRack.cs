using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoSampStoreRack
    {
        /// <summary>
        /// 更新归档架子信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool ModifySamStoreRack(EntitySampStoreRack entity);

        /// <summary>
        /// 更新归档架子字段信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateSrAmountById(string SrId, string qc);
    }
}
