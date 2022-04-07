using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxySummaryPrint : ProxyBase<ISummaryPrint>
    {

        public override string ConfigName
        {
            get
            {
                return "svc.FrmSummaryPrint";
            }
        }
    }
}
