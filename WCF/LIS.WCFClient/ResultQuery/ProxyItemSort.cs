using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyItemSort : ProxyBase<IItemSort>
    {
        public override string ConfigName
        {
            get
            {
                return "svc.FrmItemSort";
            }
        }
    }
}
