using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyItrInstrumentServicing : ProxyBase<IDicItrInstrumentServicing>
    {
        public override string ConfigName
        {
            get { return "svc.ItrInstrumentServicing"; }
        }
    }
}
