using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.qc
{
    public class QcAnalysisBIZ : IDicObrQcAnalysis
    {
        public bool DeleteQcAnalysis(EntityObrQcAnalysis qcAnalysis)
        {
            bool isDelete = false;
            IDaoObrQcAnalysis dao = DclDaoFactory.DaoHandler<IDaoObrQcAnalysis>();
            if (dao != null)
            {
                isDelete = dao.DeleteQcAnalysis(qcAnalysis);
            }
            return isDelete;
        }

        public bool InsertQcAnalysis(EntityObrQcAnalysis qcAnalysis)
        {
            bool result = false;
            IDaoObrQcAnalysis dao = DclDaoFactory.DaoHandler<IDaoObrQcAnalysis>();
            if (dao != null)
            {
                result = dao.InsertQcAnalysis(qcAnalysis);
            }
            return result;
        }

        public List<EntityObrQcAnalysis> SearchQcAnalysis(EntityObrQcResultQC qc)
        {
            List<EntityObrQcAnalysis> list = new List<EntityObrQcAnalysis>();
            IDaoObrQcAnalysis dao = DclDaoFactory.DaoHandler<IDaoObrQcAnalysis>();
            if (dao != null)
            {
                list = dao.SearchQcAnalysis(qc);
            }
            return list;
        }

        public bool UpdateQcAnalysis(EntityObrQcAnalysis qcAnalysis)
        {
            bool result = false;
            IDaoObrQcAnalysis dao = DclDaoFactory.DaoHandler<IDaoObrQcAnalysis>();
            if (dao != null)
            {
                result = dao.UpdateQcAnalysis(qcAnalysis);
            }
            return result;
        }
    }
}
