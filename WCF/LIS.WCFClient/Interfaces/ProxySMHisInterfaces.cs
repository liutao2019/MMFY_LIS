using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxySMHisInterfaces : ProxyBase<ISMHisInterfaces>
    {
        public override string ConfigName
        {
            get { return "svc.SMHisInterfaces"; }
        }
    }
}
