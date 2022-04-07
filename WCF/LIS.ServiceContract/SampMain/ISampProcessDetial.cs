using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 条码流程
    /// </summary>
    [ServiceContract]
    public interface ISampProcessDetail
    {
        /// <summary>
        /// 获取条码流程
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampProcessDetail> GetSampProcessDetail(String sampBarId);

        /// <summary>
        /// 获取条码最后的流程
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        EntitySampProcessDetail GetLastSampProcessDetail(String sampBarId);

        /// <summary>
        /// 保存操作流程
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveSampProcessDetail(EntitySampOperation operation, EntitySampMain sampMain);

        /// <summary>
        /// 直接记录流程信息，不执行院网接口
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveSampProcessDetailWithoutInterface(EntitySampProcessDetail sampProcessDetail);

    }
}
