using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyQcMateriaRule : ProxyBase<IDicQcMateriaRule>
    {
        public override string ConfigName
        {
            get { return "svc.QcMateriaRuleService"; }
        }
    }
}
