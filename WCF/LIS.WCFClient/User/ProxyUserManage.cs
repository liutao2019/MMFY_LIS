using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using System.Configuration;

namespace dcl.client.wcf
{
    public class ProxyUserManage : ProxyBase<IUserManage>
    {
        public override string ConfigName
        {
            get { return "svc.FrmUserManagePro"; }
        }
    }
}
