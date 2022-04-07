using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using System.Data;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IStatistical
    {
        /// <summary>
        /// 获取页面数据信息
        /// </summary>
        /// <param name="reportCon"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDCLPrintData GetReportData(EntityStatisticsQC reportCon);
        [OperationContract]
        EntityDCLPrintData GetReagentData(EntityStatisticsQC reportCon);
        /// <summary>
        /// 获取报告模板
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityTpTemplate> GetReportTemple(EntityTpTemplate temple);

        /// <summary>
        /// 获取缓存数据信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetStatCache();

        /// <summary>
        /// 获取查询条件的实体
        /// </summary>
        /// <param name="reportCon"></param>
        /// <returns></returns>
        [OperationContract]
        EntityStatisticsQC GetStatQC(List<EntityObrResult> obrResult,EntityStatisticsQC reportCon);

    }
}
