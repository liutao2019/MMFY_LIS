using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;

namespace dcl.client.wcf
{
    public class ProxyResultoMerge : ProxyBase<IObrResultMerge>
    {
        public override string ConfigName
        {
            get { return "svc.ResultMerge"; }
        }
    }
}
