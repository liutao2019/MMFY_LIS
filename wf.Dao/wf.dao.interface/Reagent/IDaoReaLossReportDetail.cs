using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.entity;

namespace dcl.dao.interfaces
{
    public interface IDaoReaLossReportDetail : IDaoBase
    {
        bool InsertNewReaLossReportDetail(EntityReaLossReportDetail LossReport);
        List<EntityReaLossReportDetail> GetReaLossReportDetail(EntityReaQC reaQC);
        bool CancelReaLossReportDetail(EntityReaLossReportDetail detail);
        bool DeleteReaLossReportDetail(string LossReportId, string rea_id);
        bool DeleteObrResultByObrSn(string obrSn);
    }
}
