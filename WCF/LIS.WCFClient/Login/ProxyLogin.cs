using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyLogin : ProxyBase<ILogin>
    {
        public override string ConfigName
        {
            get { return "svc.FrmLogin"; }
        }
    }
}
