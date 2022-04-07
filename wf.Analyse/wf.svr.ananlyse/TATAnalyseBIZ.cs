using System.Data;
using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.ananlyse
{
    public class TATAnalyseBIZ : IAnalyse
    {
        public DataSet GetReportData(EntityStatisticsQC statQC)
        {
            DataSet ds = new DataSet();

            IDaoAnalyse dao = DclDaoFactory.DaoHandler<IDaoAnalyse>();
            ds = dao.GetReportData(statQC);
            return ds;
        }

        public DataSet GetAnalyseData(EntityStatisticsQC statQC)
        {
            DataSet ds = new DataSet();

            IDaoStatistical dao = DclDaoFactory.DaoHandler<IDaoStatistical>();
            ds = dao.GetAnalyseData(statQC);
            return ds;
        }
    }
}
