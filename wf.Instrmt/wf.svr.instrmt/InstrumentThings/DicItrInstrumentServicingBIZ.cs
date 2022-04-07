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
    public class DicItrInstrumentServicingBIZ: IDicItrInstrumentServicing
    {
        public List<EntityDicItrInstrumentServicing> GetServicing(string itrId)
        {
            IDaoItrInstrumentServicing dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentServicing>();
            List<EntityDicItrInstrumentServicing> list = new List<EntityDicItrInstrumentServicing>();
            if (dao != null)
            {
                list = dao.GetServicing(itrId);
            }
            return list;
        }

        public List<EntityDicItrInstrumentServicing> GetServicingData(EntityDicItrInstrumentServicing strWhere)
        {
            IDaoItrInstrumentServicing dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentServicing>();
            List<EntityDicItrInstrumentServicing> list = new List<EntityDicItrInstrumentServicing>();
            if (dao != null)
            {
                list = dao.GetServicingData(strWhere);
            }
            return list;
        }
        
        public bool ServicingPutIn(EntityDicItrInstrumentServicing servicing)
        {
            IDaoItrInstrumentServicing dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentServicing>();
            bool isPutIn = false;
            if (dao != null)
            {
                isPutIn = dao.ServicingPutIn(servicing);
            }
            return isPutIn;
        }

        public bool ServingAudit(EntityDicItrInstrumentServicing servicing)
        {
            IDaoItrInstrumentServicing dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentServicing>();
            bool isPutIn = false;
            if (dao != null)
            {
                isPutIn = dao.ServingAudit(servicing);
            }
            return isPutIn;
        }

        public bool ServingHandle(EntityDicItrInstrumentServicing servicing)
        {
            IDaoItrInstrumentServicing dao = DclDaoFactory.DaoHandler<IDaoItrInstrumentServicing>();
            bool isPutIn = false;
            if (dao != null)
            {
                isPutIn = dao.ServingHandle(servicing);
            }
            return isPutIn;
        }
    }
}
