using dcl.common;
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
    [Export("wf.plugin.wf", typeof(IDaoReport))]
    public class DaoSysReport : IDaoReport
    {
        public bool SaveReport(EntitySysReport rep)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Brep_name", rep.RepName);
                values.Add("Brep_code", rep.RepCode);
                values.Add("Brep_location", rep.RepLocation);
                values.Add("Brep_sql", rep.RepSql);
                values.Add("Brep_default_sql", rep.RepDefaultSql);
                values.Add("Breap_Dconn_code", rep.RepConectCode);
                helper.InsertOperation("Base_report", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateReport(EntitySysReport rep)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("rep_id", rep.RepId);
                values.Add("Brep_name", rep.RepName);
                values.Add("Brep_code", rep.RepCode);
                values.Add("Brep_location", rep.RepLocation);
                values.Add("Brep_sql", rep.RepSql);
                values.Add("Brep_default_sql", rep.RepDefaultSql);
                values.Add("Breap_Dconn_code", rep.RepConectCode);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Brep_id", rep.RepId);
                helper.UpdateOperation("Base_report", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteReport(EntitySysReport rep)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Brep_id", rep.RepId);

                helper.DeleteOperation("Base_report", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool SaveReportParameter(EntitySysReportParameter par)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Brp_Brep_id", par.RepId);
                values.Add("Brp_type", par.RepParmType);
                values.Add("Brp_value", par.RepParmValue);
                helper.InsertOperation("Base_report_parameter", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateReportParameter(EntitySysReportParameter par)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Brp_Brep_id", par.RepId);
                values.Add("Brp_type", par.RepParmType);
                values.Add("Brp_value", par.RepParmValue);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Brp_Brep_id", par.RepId);
                helper.UpdateOperation("Base_report_parameter", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteReportParameter(EntitySysReportParameter par)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format("delete Base_report_parameter where Brp_Brep_id='{0}' And Brp_type='{1}'", par.RepId, par.RepParmType);
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
        public List<EntitySysReport> GetReport()
        {
            try
            {
                String sql = @"select cast(Brep_id as varchar) Brep_id,Brep_name,Brep_location,
Brep_sql,Brep_code,Brep_default_sql,cast(0 as bit) as rep_select,Breap_Dconn_code,
Dict_datainterfaceconnection.conn_name
from Base_report
left join Dict_datainterfaceconnection on Dict_datainterfaceconnection.conn_code = Base_report.Breap_Dconn_code";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntitySysReport> list = EntityManager<EntitySysReport>.ConvertToList(dt).OrderBy(i => i.RepId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysReport>();
            }
        }

        /// <summary>
        /// 根据报表代码获取报表信息
        /// </summary>
        /// <param name="repCode"></param>
        /// <returns></returns>
        public EntitySysReport GetReportByRepCode(string repCode)
        {
            EntitySysReport result = new EntitySysReport();
            try
            {

                String sql = String.Format(@"select cast(Brep_id as varchar) Brep_id,Brep_name,Brep_location,
Brep_sql,Brep_code,Brep_default_sql,cast(0 as bit) as rep_select,Breap_Dconn_code,
Dict_datainterfaceconnection.conn_name
from Base_report
left join Dict_datainterfaceconnection on Dict_datainterfaceconnection.conn_code = Base_report.Breap_Dconn_code
where Brep_code='{0}' or Brep_name='{0}'", repCode);
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntitySysReport> list = EntityManager<EntitySysReport>.ConvertToList(dt);

                if (list.Count > 0)
                {
                    result = list[0];
                    result.RepSql = EncryptClass.Decrypt(result.RepSql);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public List<EntitySysReportParameter> GetReportParameter(int repid)
        {
            try
            {
                if (!string.IsNullOrEmpty(repid.ToString()))
                {
                    String sql = String.Format(@"select Brp_Brep_id,Brp_type,Brp_value from Base_report_parameter where Brp_Brep_id={0}", repid);
                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);
                    List<EntitySysReportParameter> list = EntityManager<EntitySysReportParameter>.ConvertToList(dt).OrderBy(i => i.RepId).ToList();
                    return list;
                }
                else {
                    return new List<EntitySysReportParameter>();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysReportParameter>();
            }
        }

        public List<EntitySysReport> GetRepLocationByListCode(List<string> listCode)
        {
            List<EntitySysReport> listSysRep = new List<EntitySysReport>();
            try
            {
                DBManager helper = new DBManager();

                if (listCode != null && listCode.Count > 0)
                {
                    string strCode = string.Empty;
                    foreach (string item in listCode)
                    {
                        strCode += string.Format(",'{0}'", item);
                    }
                    strCode = strCode.Remove(0, 1);
                    string strReportSql = string.Format("select Brep_code,Brep_location,Brep_sql,Breap_Dconn_code from Base_report  where Brep_code in ({0})", strCode);
                    DataTable dtReport = helper.ExecuteDtSql(strReportSql);
                    listSysRep = EntityManager<EntitySysReport>.ConvertToList(dtReport).OrderBy(i => i.RepCode).ToList();

                }
                else
                {
                    string strReportSql = "select Brep_code,Brep_location,Brep_sql,Breap_Dconn_code from Base_report";
                    DataTable dtReport = helper.ExecuteDtSql(strReportSql);
                    listSysRep = EntityManager<EntitySysReport>.ConvertToList(dtReport).OrderBy(i => i.RepCode).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listSysRep;
        }



        public static string WebPath = AppDomain.CurrentDomain.BaseDirectory;


        /// <summary>
        /// 上传报表文件
        /// </summary>
        /// <param name="sysRep"></param>
        /// <returns></returns>
        public bool UpLoadReportFile(EntitySysReport sysRep)
        {
            try
            {
                string name = sysRep.FlieName;
                string base64 = sysRep.FlieBase64;
                Byte[] data = Convert.FromBase64String(base64);

                if (File.Exists(WebPath + name))
                {
                    FileInfo file = new FileInfo(WebPath + name);
                    file.IsReadOnly = false;

                    File.Copy(WebPath + name, WebPath + name + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak");
                    File.Delete(WebPath + name);
                }
                using (FileStream myFileStream = new FileStream(WebPath + name, FileMode.Create, FileAccess.Write))
                {
                    myFileStream.Write(data, 0, data.Length);
                    myFileStream.Flush();
                    myFileStream.Close();
                    myFileStream.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

    }
}
