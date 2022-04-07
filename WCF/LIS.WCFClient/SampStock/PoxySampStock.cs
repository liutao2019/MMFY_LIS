using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class PoxySampStock : ProxyBase<ISampStock>
    {
        public override string ConfigName
        {
            get { return "svc.SampStockService"; }
        }
    }
}
