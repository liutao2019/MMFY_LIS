using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.entity;

namespace dcl.dao.interfaces
{
    public interface IDaoReaApplyDetail : IDaoBase
    {
        bool DeleteReaApplyDetail(string applyId);
        bool InsertNewReaApplyDetail(EntityReaApplyDetail apply);
        List<EntityReaApplyDetail> GetReaApplyDetailByReaId(string reaId);
        bool CancelReaApplyDetail(EntityReaApplyDetail detail);
        bool DeleteReaApplyDetailByIN(string applyId, string rea_id);
        List<EntityReaApplyDetail> GetReaApplyDetailByReaIN(string reaId, string rayno);
    }
}
