using dcl.entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.resultcheck
{
    public interface IAuditChecker
    {
        void Check(ref EntityOperationResult chkResult);
    }
}
