using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ICombineSplitBarCode
    {
        /// <summary>
        /// 获取所有组合的拆分条码信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampMergeRule> GetAllCombineSplitBarCode();
    }
}
