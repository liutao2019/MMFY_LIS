using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSummaryPrint
    {
        /// <summary>
        /// 根据条件获取报表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        EntityDCLPrintData GetReportData(EntityAnanlyseQC query);
    }
}
