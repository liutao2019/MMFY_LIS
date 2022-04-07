using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyQcMateria : ProxyBase<IDicQcMateria>
    {
        public override string ConfigName
        {
            get { return "svc.QcMateriaService"; }
        }
    }
}
