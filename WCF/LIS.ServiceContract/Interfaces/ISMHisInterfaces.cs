using dcl.entity;
using System.ServiceModel;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISMHisInterfaces
    {
        /// <summary>
        /// his身份验证
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [OperationContract]
        AuditInfo HisUserAudit(AuditInfo userInfo);
    }
}
