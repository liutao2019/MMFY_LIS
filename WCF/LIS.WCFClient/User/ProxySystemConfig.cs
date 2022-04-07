using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxySystemConfig : ProxyBase<ISystemConfig>
    {
        public override string ConfigName
        {
            get
            {
                return "svc.FrmSystemConfig";
            }
        }
    }
}
