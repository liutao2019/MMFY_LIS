using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    public class ProxySysUserInfo : ProxyBase<ISysUserInfo>
    {
        public override string ConfigName
        {
            get
            {
                return "svc.SysUserInfo";
            }
        }
    }
}
