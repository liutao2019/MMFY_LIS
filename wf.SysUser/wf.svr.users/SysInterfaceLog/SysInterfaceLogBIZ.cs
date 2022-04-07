using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.users
{
    public class SysInterfaceLogBIZ : ISysInterfaceLog
    {
        public List<EntitySysInterfaceLog> GetSysInterfaceLogData(EntitySysInterfaceLog eySysInfeLog)
        {
            List<EntitySysInterfaceLog> listSysInfeLog = new List<EntitySysInterfaceLog>();
            IDaoSysInterfaceLog dao = DclDaoFactory.DaoHandler<IDaoSysInterfaceLog>();
            if (dao != null)
            {
                listSysInfeLog = dao.GetSysInterfaceLogData(eySysInfeLog);
            }
            return listSysInfeLog;
        }

        public bool SaveSysInterfaceLog(EntitySysInterfaceLog eySysInfeLog)
        {
            bool isSave = false;
            IDaoSysInterfaceLog dao = DclDaoFactory.DaoHandler<IDaoSysInterfaceLog>();
            if (dao != null)
            {
                isSave = dao.SaveSysInterfaceLog(eySysInfeLog);
            }
            return isSave;
        }
    }
}
