using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IObrResultBact
    {
        /// <summary>
        /// 根据病人标识ID查找细菌结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResultBact> GetBactResultById(string obrId = "");

        /// <summary>
        /// 根据标识ID删除结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteResultById(string obrId);

        /// <summary>
        /// 保存描述结果信息
        /// </summary>
        /// <param name="bact"></param>
        /// <returns></returns>
        Boolean SaveResultBact(List<EntityObrResultBact> bact);
    }
}
