using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;

namespace dcl.client.wcf
{

    public class ProxyCacheService : ProxyBase<ICacheService>
    {
        //结果视窗
        public override string ConfigName
        {
            get { return "svc.CacheService"; }
        }
    }
}
