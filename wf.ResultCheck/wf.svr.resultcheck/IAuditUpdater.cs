using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    public interface IAuditUpdater
    {
        void Update(ref EntityOperationResult chkResult);
    }
}
