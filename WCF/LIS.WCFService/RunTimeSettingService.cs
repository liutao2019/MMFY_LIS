using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using dcl.root.logon;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.wcf
{
    public class RunTimeSettingService : IRunTimeSetting
    {
        public void Save(string fullkey, byte[] data)
        {
            IDaoRuntimeSetting dao = DclDaoFactory.DaoHandler<IDaoRuntimeSetting>();
            if (dao != null)
                dao.Save(fullkey, data);
        }

        public byte[] Load(string fullkey)
        {
            Byte[] data = null;
            IDaoRuntimeSetting dao = DclDaoFactory.DaoHandler<IDaoRuntimeSetting>();
            if (dao != null)
                data=dao.Load(fullkey);
            return data;
        }

        public void Delete(string fullkey)
        {
            IDaoRuntimeSetting dao = DclDaoFactory.DaoHandler<IDaoRuntimeSetting>();
            if (dao != null)
                dao.Delete(fullkey);
        }


    }
}
