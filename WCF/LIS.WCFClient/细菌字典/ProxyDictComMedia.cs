using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using System.Configuration;

namespace dcl.client.wcf
{
    public class ProxyDictComMedia : Lib.WCFClientCore.ProxyBase<IDictComMedia>
    {
        public ProxyDictComMedia()
            : base(ConfigurationManager.AppSettings["wcfAddr"] + ConfigurationManager.AppSettings["svc.DictComMedia"])
        { }
    }
}
