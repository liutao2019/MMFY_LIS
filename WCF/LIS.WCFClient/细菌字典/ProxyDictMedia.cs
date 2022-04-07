using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using System.Configuration;

namespace dcl.client.wcf
{
    public class ProxyDictMedia : Lib.WCFClientCore.ProxyBase<IDictMedia>
    {
        public ProxyDictMedia()
            : base(ConfigurationManager.AppSettings["wcfAddr"] + ConfigurationManager.AppSettings["svc.DictMedia"])
        { }
    }
}
