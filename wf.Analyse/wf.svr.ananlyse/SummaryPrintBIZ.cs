using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.ananlyse
{
    public class SummaryPrintBIZ : ISummaryPrint
    {
        public EntityDCLPrintData GetReportData(EntityAnanlyseQC query)
        {
            IDaoSummaryPrint dao = DclDaoFactory.DaoHandler<IDaoSummaryPrint>();
            EntityDCLPrintData ds = new EntityDCLPrintData();
            if (dao != null)
            {
                ds = dao.GetReportData(query);
            }
            return ds;
        }

    }
}

