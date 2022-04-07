using dcl.servececontract;

namespace dcl.client.wcf
{

    public class ProxySampProcessMonitor : ProxyBase<IDicSampProcessMonitor>
    {
        public override string ConfigName
        {
            get { return "svc.SampProcessMonitor"; }
        }
    }
}
