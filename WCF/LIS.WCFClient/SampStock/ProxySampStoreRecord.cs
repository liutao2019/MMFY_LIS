using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxySampStoreRecord : ProxyBase<ISampStoreRecord>
    {
        public override string ConfigName
        {
            get { return "svc.SampStoreRecord"; }
        }
    }
}
