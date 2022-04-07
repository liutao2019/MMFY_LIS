using dcl.entity;
using dcl.servececontract;

namespace dcl.svr.interfaces
{
    public class SMHisInterfacesBIZ : ISMHisInterfaces
    {
        public AuditInfo HisUserAudit(AuditInfo userInfo)
        {
            return DCLExtInterfaceFactory.DCLExtInterface.IdentityVerification(userInfo);
        }
    }
}
