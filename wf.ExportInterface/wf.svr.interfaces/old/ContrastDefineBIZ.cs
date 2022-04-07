using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;

namespace dcl.svr.interfaces
{
    public class ContrastDefineBIZ : ISysItfContrast
    {

        public bool DeleteSysContrast(string conId)
        {
            IDaoSysItfContrast dao = DclDaoFactory.DaoHandler<IDaoSysItfContrast>();
            if (dao == null)
            {
                return false;
            }
            else
            {
                return dao.DeleteSysContrast(Convert.ToInt32(conId));
            }
        }

        public List<EntitySysItfContrast> GetSysContrast(string interfaceId)
        {
            List<EntitySysItfContrast> list = new List<EntitySysItfContrast>();
            IDaoSysItfContrast dao = DclDaoFactory.DaoHandler<IDaoSysItfContrast>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetSysContrast(interfaceId);
            }
            return list;
        }

        public List<EntitySysItfInterface> GetSysInterface()
        {
            List<EntitySysItfInterface> list = new List<EntitySysItfInterface>();
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetSysInterface();
            }
            return list;
        }


        public bool SaveSysContrast(EntitySysItfContrast entity)
        {
            IDaoSysItfContrast dao = DclDaoFactory.DaoHandler<IDaoSysItfContrast>();
            if (dao == null)
            {
                return false;
            }
            else
            {
                return dao.SaveSysContrast(entity);
            }
        }


        public bool UpdateSysContrast(EntitySysItfContrast entity)
        {
            IDaoSysItfContrast dao = DclDaoFactory.DaoHandler<IDaoSysItfContrast>();
            if (dao == null)
            {
                return false;
            }
            else
            {
                return dao.UpdateSysContrast(entity);
            }
        }
    }
}
