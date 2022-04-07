using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysLog))]
    public class DaoSysLog : IDaoSysLog
    {

        public bool SaveSysLog(EntityLogLogin log)
        {
            DBManager helper = new DBManager();
            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("Module", log.LogModule);
            values.Add("Time", log.LogTime.ToString("yyyy-MM-dd HH:mm:ss"));
            values.Add("LoginID", log.LogLoginID);
            values.Add("IP", log.LogIP);
            values.Add("Mac", log.LogMAC);
            values.Add("Type", log.LogType);
            values.Add("Message", log.LogMessage);
            helper.InsertOperation("LogLogin", values);
            return true;
        }

        public bool UpdateSysLog(EntityLogLogin log)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("ID", log.LogID);
                values.Add("Module", log.LogModule);
                values.Add("Time", log.LogTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("LoginID", log.LogLoginID);
                values.Add("IP", log.LogIP);
                values.Add("Mac", log.LogMAC);
                values.Add("Type", log.LogType);
                values.Add("Message", log.LogMessage);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("ID", log.LogID);
                helper.UpdateOperation("LogLogin", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteSysLog(string timeFrom,string timeTo)
        {
            try
            {
                string where = " where 1=1";
                DateTime from = Convert.ToDateTime(timeFrom);
                DateTime to = Convert.ToDateTime(timeTo).AddDays(1);
                where += " and time>='" + from.ToString("yyyy-MM-dd 00:00:00") + "'";
                where += " and time<='" + to.ToString("yyyy-MM-dd 00:00:00") + "'";
                DBManager helper = new DBManager();
                string sql = string.Format("Delete From LogLogin  {0} ", where);
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityLogLogin> GetSysLog(string loginId, string module, string timeFrom, string timeTo)
        {
            List<EntityLogLogin> list = new List<EntityLogLogin>();
            string where = " where 1=1";

            if (!string.IsNullOrEmpty(loginId))
            {
                where += " and  loginId ='" + loginId + "'";
            }

            if (!string.IsNullOrEmpty(module))
            {
                where += " and  module ='" + module + "'";
            }
            DateTime from = Convert.ToDateTime(timeFrom);
            DateTime to = Convert.ToDateTime(timeTo).AddDays(1);
            where += " and time>='" + from.ToString("yyyy-MM-dd 00:00:00") + "'";
            where += " and time<='" + to.ToString("yyyy-MM-dd 00:00:00") + "'";
            if (where != null)
            {
                try
                {
                    string sql = String.Format(@"Select  ID, Module, Time, LoginID, IP, MAC, Type, Message From LogLogin  {0} ", where);
                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);
                    list = EntityManager<EntityLogLogin>.ConvertToList(dt).OrderByDescending(i => i.LogTime).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            
            }
            return list;
        }

        public DateTime GetDatabaseServerDateTime()
        {
            DateTime dtNow = DateTime.Now;
            try
            {
                string strSQL = @"SELECT GETDATE() ";
                DBManager helper = new DBManager();
                DataTable table = helper.ExecuteDtSql(strSQL);
                dtNow = Convert.ToDateTime(table.Rows[0][0]);

            }
            catch (Exception ex)
            {
            }
            return dtNow;
        }
    }
}
