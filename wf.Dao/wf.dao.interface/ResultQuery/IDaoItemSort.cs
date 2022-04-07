using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoItemSort
    {
        /// <summary>
        /// 查询报表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        EntityDCLPrintData GetReportData(EntityAnanlyseQC query);

        /// <summary>
        /// 获取where查询子句
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        String GetSqlString(EntityAnanlyseQC query);
    }
}
