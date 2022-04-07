using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxySysOperationLog : ProxyBase<ISysOperationLog>
    {
        public override string ConfigName
        {
            get { return "svc.FrmOperationLog"; }
        }
    }
}
