using System;
using System.Collections.Generic;
using System.Text;
using dcl.servececontract;
using System.Configuration;

namespace dcl.client.wcf
{
    //public class ProxyAnalyse : Lib.WCFClientCore.ProxyBase<IAnalyse>
    //{
    //    public ProxyAnalyse()
    //        : base(ConfigurationManager.AppSettings["wcfAddr"] + ConfigurationManager.AppSettings["svc.FrmTATAnalyse"])
    //    { }
    //}
    public class ProxyStatTemp : ProxyBase<IStatTemp>
    {
        public override string ConfigName
        {
            get { return "svc.SaveStatTemp"; }
        }
    }
}
