using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lis.dto.Entity;
using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.result.Audit
{
    public interface IAuditer
    {
        EntityOperationResult CommonAuditCheck(string pat_id, int checkType);
        EntityOperationResult Aduit(string pat_id, EntityRemoteCallClientInfo caller);
        EntityOperationResult Report(string pat_id, EntityRemoteCallClientInfo caller);
    }
}
