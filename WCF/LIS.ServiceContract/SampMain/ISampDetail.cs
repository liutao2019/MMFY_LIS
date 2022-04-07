using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISampDetail
    {
        /// <summary>
        /// 查询条码明细
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampDetail> GetSampDetail(string sampBarId);

        /// <summary>
        /// 批量查询条码明细
        /// </summary>
        /// <param name="listSampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampDetail> GetSampDetailByListBarId(List<string> listSampBarId);

        /// <summary>
        /// 删除条码明细
        /// </summary>
        /// <param name="listSampDetail"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteSampDetail(List<EntitySampDetail> listSampDetail);

        /// <summary>
        /// 判断是否存在不同天的医嘱信息
        /// </summary>
        /// <param name="listSampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean ExistDifferentOCCDate(List<String> listSampBarId);

    }
}
