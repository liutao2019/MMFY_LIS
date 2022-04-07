using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using System.Configuration;

namespace dcl.client.wcf
{

    public class ProxyLisDoc : ProxyBase<ILisDoc>
    {
        public override string ConfigName
        {
            get { return "svc.LisDocManager"; }
        }
    }

}
