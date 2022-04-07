using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyPidReportMainExt : ProxyBase<IDicPidReportMainExt>
    {
        public override string ConfigName
        {
            get { return "svc.PidReportMainExtService"; }
        }
    }
}
