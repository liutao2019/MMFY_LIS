using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.instrmt
{
    public class DicItrInstrumentRegistrationBIZ: IDicItrInstrumentRegistration
    {
        public List<EntityDicInstrmtMaintainRegistration> GetRegistration(string strItrId)
        {
            IDaoItrInstrumentRegistration dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentRegistration>();
            List<EntityDicInstrmtMaintainRegistration> list = new List<EntityDicInstrmtMaintainRegistration>();
            if (dao != null)
            {
                list = dao.GetRegistration(strItrId);
            }
            return list;
        }

        public List<EntityDicInstrmtMaintainRegistration> GetRegistrationByDate(string strmai_id)
        {
            IDaoItrInstrumentRegistration dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentRegistration>();
            List<EntityDicInstrmtMaintainRegistration> list = new List<EntityDicInstrmtMaintainRegistration>();
            if (dao != null)
            {
                list = dao.GetRegistrationByDate(strmai_id);
            }
            return list;
        }
        
        public int MaintainRegistration(List<EntityDicInstrmtMaintainRegistration> listRegis)
        {
            IDaoItrInstrumentRegistration dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentRegistration>();
            int count = 0;
            if (dao != null)
            {
                count = dao.MaintainRegistration(listRegis);
            }
            return count;
        }

        public List<EntityDicInstrmtMaintainRegistration> GetMaintainData(EntityDicInstrmtMaintainRegistration registration)
        {
            IDaoItrInstrumentRegistration dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentRegistration>();
            List<EntityDicInstrmtMaintainRegistration> list = new List<EntityDicInstrmtMaintainRegistration>();
            if (dao != null)
            {
                list = dao.GetMaintainData(registration);
            }
            return list;
        }

    }
}
