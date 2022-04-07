using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ITwoWaySelect
    {
        /// <summary>
        /// 查询条码数据
        /// </summary>
        /// <param name="sampBarcode">条码号</param>
        /// <param name="itrId">仪器ID</param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetBarcodeData(string sampBarcode,string itrId);

        /// <summary>
        /// 根据条码号来更新上机标志
        /// </summary>
        /// <param name="commFlag">上机标志</param>
        /// <param name="sampBarcode">条码号</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateSampFlag(string commFlag,string sampBarcode);

        /// <summary>
        /// 根据条码号和his项目编码来更新上机标志
        /// </summary>
        /// <param name="commFlag">上机标志</param>
        /// <param name="sampBarcode">条码号</param>
        /// <param name="orderCode">his项目编码</param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateSampFlagByCode(string commFlag,string sampBarcode,string orderCode);
    }
}
