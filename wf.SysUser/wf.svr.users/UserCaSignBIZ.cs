using System.Collections.Generic;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.users
{
    public class UserCaSignBIZ
    {
        public bool InsertCaSign(List<EntityCaSign> CaSign)
        {
            IDaoUserCaSign dao = DclDaoFactory.DaoHandler<IDaoUserCaSign>();
            bool result = dao.InsertCaSign(CaSign);
            return result;
        }

        internal List<EntityCaSign> GetCaSign(string cerId, string entityId)
        {
            IDaoUserCaSign dao = DclDaoFactory.DaoHandler<IDaoUserCaSign>();
            List<EntityCaSign> result = dao.GetCaSign(cerId, entityId);
            return result;
        }
    }
}
