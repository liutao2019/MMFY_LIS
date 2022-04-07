using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.svr.dicbasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace dcl.svr.sample
{

    public class TatOverTimeBIZ
    {
        public bool SaveOverTime(EntityTatOverTime overTime)
        {
            bool result = false;
            IDaoTatOverTime recordDao = DclDaoFactory.DaoHandler<IDaoTatOverTime>();
            if (recordDao != null)
            {
                result = recordDao.SaveTatOverTime(overTime);
            }
            return result;
        }

        public List<EntityTatOverTime> GetTatOverTime(string barCode)
        {
            List<EntityTatOverTime> listOverTime = new List<EntityTatOverTime>();
            IDaoTatOverTime recordDao = DclDaoFactory.DaoHandler<IDaoTatOverTime>();
            if (recordDao != null)
            {
                listOverTime = recordDao.GetTatOverTime(barCode);
            }
            return listOverTime;
        }
    }
}
