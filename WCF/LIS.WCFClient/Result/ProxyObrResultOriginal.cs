using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyObrResultOriginal : ProxyBase<IObrResultOriginal>
    {
        public override string ConfigName
        {
            get { return "svc.FrmRealTimeResultView"; }
        }
    }
}
