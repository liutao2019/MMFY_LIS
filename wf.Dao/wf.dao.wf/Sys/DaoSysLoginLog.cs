using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysLoginLog))]
    public class DaoSysLoginLog : IDaoSysLoginLog
    {
        public void Log(string type, string module, string loginID, string ip, string mac, string message)
        {
            try
            {
                string sql = "insert into LogLogin(Module,[Time],LoginID,IP,MAC,Type,Message) values(@Module,getdate(),@LoginID,@IP,@MAC,@Type,@Message)";
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@Module", module));
                paramHt.Add(new SqlParameter("@LoginID", loginID));
                paramHt.Add(new SqlParameter("@IP", ip));
                paramHt.Add(new SqlParameter("@MAC", mac));
                paramHt.Add(new SqlParameter("@Type", type));
                paramHt.Add(new SqlParameter("@Message", message));
                DBManager helper = new DBManager();
                helper.ExecCommand(sql, paramHt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }
        }

        public void LogLogin(string type, string module, string loginID, string ip, string mac, string message)
        {
            Log(type, module, loginID, ip, mac, message);
        }

        public void LogLogout(string module, string loginID, string ip, string mac)
        {
            Log("退出", module, loginID, ip, mac, "退出");
        }

    }
}
