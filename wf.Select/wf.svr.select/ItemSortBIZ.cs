using System;
using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.resultquery
{
    class ItemSortBIZ : IItemSort
    {
        public EntityDCLPrintData GetReportData(EntityAnanlyseQC query)
        {
            IDaoItemSort dao = DclDaoFactory.DaoHandler<IDaoItemSort>();
            EntityDCLPrintData ds = new EntityDCLPrintData();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("未找到数据访问");
            }
            else
            {
                try
                {
                    ds = dao.GetReportData(query);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return ds;
        }

        public string GetSqlString(EntityAnanlyseQC query)
        {
            IDaoItemSort dao = DclDaoFactory.DaoHandler<IDaoItemSort>();
            string where = string.Empty;
            if (dao != null)
            {
                where = dao.GetSqlString(query);
            }
            return where;
        }
    }
}
