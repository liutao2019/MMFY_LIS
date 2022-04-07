using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 标本回退操作类
    /// </summary>
    [ServiceContract]
    public interface ISampReturn
    {
        [OperationContract]
        Boolean SaveSampReturn(EntitySampReturn sampReturn);

        /// <summary>
        /// 查询回退信息
        /// </summary>
        /// <param name="sampQc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampReturn> GetSampReturn(EntitySampQC sampQc);

        /// <summary>
        /// 更新条码回退标志
        /// </summary>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampReturnFlag(EntitySampMain sampMain);

        /// <summary>
        /// 更新回退条码信息
        /// </summary>
        /// <param name="sampMain"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateReturnMessage(EntitySampReturn sampReturn);


        /// <summary>
        /// 刷新回退条码信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void RefereshReturnMessage();
    }
}
