using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.entity;

namespace dcl.dao.interfaces
{
    public interface IDaoReaPurchaseDetail : IDaoBase
    {
        bool InsertNewReaPurchaseDetail(EntityReaPurchaseDetail purchase);
        List<EntityReaPurchaseDetail> GetReaPurchaseDetail(EntityReaQC reaQC);
        bool CancelReaPurchaseDetail(EntityReaPurchaseDetail detail);
        bool DeleteReaPurchaseDetail(string purchaseId, string rea_id);
        void UpdateDetailStatus(string purno, string reaid, int status);
    }
}
