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
    public interface IItemSort
    {
        /// <summary>
        /// 查询报表数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDCLPrintData GetReportData(EntityAnanlyseQC query);

        /// <summary>
        /// 获取where查询子句
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        String GetSqlString(EntityAnanlyseQC query);
    }
}
