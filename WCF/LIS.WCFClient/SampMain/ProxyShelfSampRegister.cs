using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;

namespace dcl.client.wcf
{
   public class ProxyShelfSampRegister : ProxyBase<IShelfSampRegister>
    {
        public override string ConfigName
        {
            get { return "svc.frmShelfSampleRegister"; }
        }
    }
}
