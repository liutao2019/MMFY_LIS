using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IPidReportMainInterface
    {
        /// <summary>
        /// 从接口中获取病人信息
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        EntityPidReportMain GetPatientFromInterface(EntityInterfaceExtParameter parameter);
    }
}
