using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyTempHandle : ProxyBase<ITempHandle>
    {
        public override string ConfigName
        {
            get
            {
                return "svc.FrmTempHandle";
            }
        }
    }
}
