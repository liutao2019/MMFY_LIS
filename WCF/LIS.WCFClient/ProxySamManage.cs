﻿using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using System.Configuration;

namespace dcl.client.wcf
{
    public class ProxySamManage : Lib.WCFClientCore.ProxyBase<ISamManage>
    {
        public ProxySamManage()
            : base(ConfigurationManager.AppSettings["wcfAddr"] + ConfigurationManager.AppSettings["svc.ProxySamManage"])
        { }
    }
}
