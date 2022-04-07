using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using System.Data;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IAnalyse
    {
        /// <summary>
        /// 获取报告数据信息
        /// </summary>
        /// <param name="statQC"></param>
        /// <returns></returns>
        [OperationContract]
        DataSet GetReportData(EntityStatisticsQC statQC);
        /// <summary>
        /// 细菌获取数据分析的数据
        /// </summary>
        /// <param name="statQC"></param>
        /// <returns></returns>
        [OperationContract]
        DataSet GetAnalyseData(EntityStatisticsQC statQC);
    }
}
