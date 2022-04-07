using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IGetTubeInfoByCombine
    {
        /// <summary>
        /// 根据组合信息获取合管
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicTestTube> GetTubes(List<EntityDicCombine> Combines);
    }
}
