using dcl.client.wcf;
using dcl.servececontract;

namespace dcl.client.frame
{
    public class ProxySystemLog : ProxyBase<ILogin>
    {
        public ProxySystemLog()
        {

        }

        public override string ConfigName
        {
            get { return "svc.FrmLogin"; }
        }

        public bool InsertSystemLog(string type, string module, string loginID, string ip, string mac, string message)
        {
            return base.Service.InsertSystemLog(type, module, loginID, ip, mac, message);
        }

    }
}

