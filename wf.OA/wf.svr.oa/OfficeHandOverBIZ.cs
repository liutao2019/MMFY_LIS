using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.oa
{
    public class OfficeHandOverBIZ : IOfficeHandOver
    {

        public List<EntityDicHandOver> GetDictHandoverList()
        {
            List<EntityDicHandOver> list = new List<EntityDicHandOver>();
            IDaoOaHandOver dao = DclDaoFactory.DaoHandler<IDaoOaHandOver>();
            if (dao != null)
            {
                return list = dao.GetDictHandoverList() as List<EntityDicHandOver>;
            }
            else
            {
                return list;
            }
        }

        public bool UpdateDictHandoverList(List<EntityDicHandOver> list)
        {
            IDaoOaHandOver dao = DclDaoFactory.DaoHandler<IDaoOaHandOver>();
            if (dao != null)
            {
                return (dao.UpdateDictHandoverList(list));
            }
            else
            {
                return false;
            }
        }

        public bool DeleteDictHandover(string typeid)
        {
            IDaoOaHandOver dao = DclDaoFactory.DaoHandler<IDaoOaHandOver>();
            if (dao != null)
            {
                return (dao.DeleteDictHandover(typeid));
            }
            else
            {
                return false;
            }
        }
    }
}
