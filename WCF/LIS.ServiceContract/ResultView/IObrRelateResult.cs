using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IObrRelateResult
    {
        /// <summary>
        /// 获取病人相关结果
        /// </summary>
        /// <param name="listStr"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetRelateResult(EntityPidReportMain patient);

    }
}
