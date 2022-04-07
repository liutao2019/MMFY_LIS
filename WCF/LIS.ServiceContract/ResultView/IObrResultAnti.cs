using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IObrResultAnti
    {
        /// <summary>
        /// 根据标识ID查找药敏结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResultAnti> GetAntiResultById(string obrId);


        /// <summary>
        /// 根据标识ID查找药敏结果及历史结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResultAnti> GetAntiWithHistoryResultById(string obrId);

        /// <summary>
        /// 根据标识ID删除结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteResultById(string obrId);

        /// <summary>
        /// 保存药敏结果
        /// </summary>
        /// <param name="listAnti"></param>
        /// <returns></returns>
        Boolean SaveAntiResult(List<EntityObrResultAnti> listAnti);

    }
}
