using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxyOfficeAnnouncement : ProxyBase<IOfficeAnnouncement>
    {
        public override string ConfigName
        {
            get { return "svc.ProxyOfficAnnouncement"; }
        }
    }
}
