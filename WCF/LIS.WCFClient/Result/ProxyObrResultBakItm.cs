using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyObrResultBakItm : ProxyBase<IObrResultBakItm>
    {
        public override string ConfigName
        {
            get
            {
                return "svc.ObrResultBakItm";
            }
        }
    }
}
