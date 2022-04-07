using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 归档查询
    /// </summary>
    [ServiceContract]
    public interface IDicSamSearch
    {
        /// <summary>
        /// 标本查询
        /// </summary>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <param name="patInNo"></param>
        /// <param name="patName"></param>
        /// <param name="storeMan"></param>
        /// <param name="rackCtype"></param>
        /// <param name="iceID"></param>
        /// <param name="cupID"></param>
        /// <param name="rackBarcode"></param>
        /// <param name="samBarcode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetRackQueryData(EntityDicSamSearchParamter samSearchParam);

        /// <summary>
        ///  根据试管条码架子条码查询标本相关信息
        /// </summary>
        /// <param name="rackBarcode"></param>
        /// <param name="samBarcode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetRackQueryDataByBarcode(string rackBarcode, string samBarcode);
        
    }
}
