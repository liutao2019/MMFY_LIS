using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.entity;

namespace dcl.dao.interfaces
{
    public interface IDaoReaSubscribeDetail : IDaoBase
    {
        bool InsertNewReaSubscribeDetail(EntityReaSubscribeDetail Subscribe);
        List<EntityReaSubscribeDetail> GetReaSubscribeDetail(EntityReaQC reaQC);
        bool CancelReaSubscribeDetail(EntityReaSubscribeDetail detail);
        bool DeleteReaSubscribeDetail(string SubscribeId, string rea_id);
    }
}
