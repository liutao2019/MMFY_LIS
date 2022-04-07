using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.tools
{
    public class TempHandleBIZ : ITempHandle
    {
        public List<EntityTemHarvester> GetTempHarvesterByProId(string proId, DateTime? datetime = null)
        {
            List<EntityTemHarvester> listTempHar = new List<EntityTemHarvester>();
            IDaoTempHandle daoTemp = DclDaoFactory.DaoHandler<IDaoTempHandle>();
            if (daoTemp != null)
            {
                listTempHar = daoTemp.GetTempHarvesterByProId(proId, datetime);
            }
            return listTempHar;
        }

        public List<EntityTemHarvester> GetHarvesterByDhId(string dhId)
        {
            List<EntityTemHarvester> listHar = new List<EntityTemHarvester>();
            IDaoTempHandle dao = DclDaoFactory.DaoHandler<IDaoTempHandle>();
            if (dao != null)
            {
                listHar = dao.GetHarvesterByDhId(dhId);
            }
            return listHar;
        }

    }
}
