using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISummaryPrint
    {
        /// <summary>
        /// 根据条件获取报表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDCLPrintData GetReportData(EntityAnanlyseQC query);
    }
}
