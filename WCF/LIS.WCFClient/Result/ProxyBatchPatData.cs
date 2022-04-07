using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.client.wcf
{
    /// <summary>
    /// 资料修改
    /// </summary>
    public class ProxyBatchPatData : ProxyBase<IBatchEditNew>
    {
        public override string ConfigName
        {
            get
            {
                return "svc.FrmBatchEdit";
            }
        }
    }
}
