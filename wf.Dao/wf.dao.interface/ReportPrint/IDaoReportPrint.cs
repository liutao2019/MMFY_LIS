using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoReportPrint
    {
        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        EntityDCLPrintData GetReportData(EntityDCLPrintParameter parameter);

        /// <summary>
        /// 根据病人ID获取细菌报表编码 2018-02-01
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        string GetBacReportCode(string repId);
    }
}
