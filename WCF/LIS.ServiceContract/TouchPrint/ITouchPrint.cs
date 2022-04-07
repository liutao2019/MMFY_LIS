using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ITouchPrint
    {
        /// <summary>
        /// 查询打印信息
        /// </summary>
        /// <param name="pidInNo"></param>
        /// <param name="pidSrcId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityTouchPrintData> PatientPrintQuery(string pidInNo, string pidSrcId,string PrintType = "0");
    }
}
