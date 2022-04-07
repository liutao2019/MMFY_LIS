using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lis.dto.Entity;
using dcl.pub.entities;
using dcl.entity;

namespace dcl.svr.result.Audit
{
    public class AuditBllV2 : IAuditer
    {
        public EntityOperationResult CommonAuditCheck(string pat_id, int checkType)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult Aduit(string pat_id, EntityRemoteCallClientInfo caller)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult Report(string pat_id, EntityRemoteCallClientInfo caller)
        {
            throw new NotImplementedException();
        }
    }
}
