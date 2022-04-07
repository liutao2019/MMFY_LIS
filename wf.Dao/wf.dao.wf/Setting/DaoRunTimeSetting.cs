using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoRuntimeSetting))]
    public class DaoRunTimeSetting : IDaoRuntimeSetting
    {
        public void Save(string key, byte[] data)
        {
            if (data != null)
            {
                try
                {
                    string sql = string.Empty;
                    if (ExistKey(key))
                    {
                        sql = string.Format("update sysRuntimeSetting set [Data]=@data where [Key]=@key");
                    }
                    else
                    {
                        sql = string.Format("insert into sysRuntimeSetting([Key],[Data]) values(@key,@data)");
                    }
                    DBManager helper = new DBManager();
                    List<DbParameter> paramHt = new List<DbParameter>();
                    paramHt.Add(new SqlParameter("@key", key));
                    paramHt.Add(new SqlParameter("@data", data));
                    helper.ExecCommand(sql,paramHt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }

        public byte[] Load(string key)
        {
            Byte[] data = null;
            try
            {
                string sql = string.Format("select top 1 [Data] from sysRuntimeSetting where [Key] = @key");
                DBManager helper = new DBManager();
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@key", key));
                DataTable dt = helper.ExecuteDtSql(sql,paramHt);
                if (dt != null && dt.Rows.Count>0)
                {
                    data = (byte[])dt.Rows[0]["data"];
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return data;
        }

        public void Delete(string key)
        {
            try
            {
                string sql = string.Format("delete from sysRuntimeSetting where [Key] = @key");
                DBManager helper = new DBManager();
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@key", key));
                helper.ExecCommand(sql, paramHt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
        }

        public bool ExistKey(string fullkey)
        {
            string sql = string.Format("select top 1 [Key]  from sysRuntimeSetting where [Key] = '{0}'", fullkey);

            DBManager helper = new DBManager();
            DataTable dt = helper.ExecuteDtSql(sql);

            if (dt != null && dt.Rows.Count>0)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
