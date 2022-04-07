using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ServiceModel;



using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISampStoreRack
    {
        /// <summary>
        /// 修改归档架子信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int ModifySamStoreRack(EntitySampStoreRack entity);

        /// <summary>
        /// 更新归档架子字段信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int UpdateSrAmountById(string SrId, string qc);
    }
}
