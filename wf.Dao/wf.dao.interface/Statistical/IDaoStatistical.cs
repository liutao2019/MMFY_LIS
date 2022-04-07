using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using System.Collections;

namespace dcl.dao.interfaces
{
    public interface IDaoStatistical
    {
        /// <summary>
        /// 获取模板表信息
        /// </summary>
        /// <returns></returns>
        List<EntityTpTemplate> GetTemplate(string stName, string stType);

        /// <summary>
        /// 获取高级查询所生成的where条件
        /// </summary>
        /// <param name="dtResult"></param>
        /// <param name="IDList"></param>
        /// <returns></returns>
        EntityStatisticsQC GetStatQC(List<EntityObrResult> dtResult, EntityStatisticsQC statQC);

        /// <summary>
        ///获取报表数据
        /// </summary>
        /// <param name="StatQc"></param>
        /// <returns></returns>
        EntityDCLPrintData GetReportData(EntityStatisticsQC StatQc);
        /// <summary>
        /// 细菌获取数据分析数据
        /// </summary>
        /// <param name="StatQc"></param>
        /// <returns></returns>
        DataSet GetAnalyseData(EntityStatisticsQC StatQc);
        /// <summary>
        ///获取试剂报表数据
        /// </summary>
        /// <param name="StatQc"></param>
        /// <returns></returns>
        EntityDCLPrintData GetReagentData(EntityStatisticsQC StatQc);
    }
}
