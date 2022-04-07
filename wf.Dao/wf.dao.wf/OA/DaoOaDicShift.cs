using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoOaDicShift))]
    public class DaoOaDicShift : IDaoOaDicShift
    {
        public int DeleteDutyRecord(EntityOaDicShift sample)
        {
            try
            {
                DBManager helper = new DBManager();
                string strSql = string.Format(@"update Dict_oa_duty
                                            set    del_flag = 1
                                            where  Dduty_id = '{0}'", sample.ShiftId);
                int intRet = helper.ExecCommand(strSql);
                return intRet;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return -1;
            }
        }

        public List<EntityOaDicShift> GetDutyData()
        {
            try
            {
                  DBManager helper = new DBManager();
                string strSql = @"select dd.Dduty_id,
dd.Dduty_Ddept_id,
dd.Dduty_name,
dd.Dduty_start_date,
dd.Dduty_end_date,
dd.wb_code,
dd.py_code,
dd.Dduty_remark,
dd.Dduty_flag,
dt.Dpro_name,
dt.wb_code,
dt.py_code,
dd.del_flag
from Dict_oa_duty dd
left join Dict_profession dt on  dt.Dpro_id = dd.Dduty_Ddept_id
where  dd.del_flag = 0  ";

                DataTable dt = helper.ExecuteDtSql(strSql);
                dt.TableName = "Dict_oa_duty";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Dduty_start_date"] != null && dt.Rows[i]["Dduty_start_date"] != DBNull.Value &&
                        !string.IsNullOrEmpty(dt.Rows[i]["Dduty_start_date"].ToString())
                        && dt.Rows[i]["Dduty_start_date"].ToString().IndexOf(":") > 0)
                    {
                        string[] hms = dt.Rows[i]["Dduty_start_date"].ToString().Split(':');
                        if (hms.Length > 0 && hms[0].Length == 1)
                        {
                            dt.Rows[i]["Dduty_start_date"] = "0" + dt.Rows[i]["Dduty_start_date"].ToString();
                        }
                    }

                    if (dt.Rows[i]["Dduty_end_date"] != null && dt.Rows[i]["Dduty_end_date"] != DBNull.Value &&
                       !string.IsNullOrEmpty(dt.Rows[i]["Dduty_end_date"].ToString())
                       && dt.Rows[i]["Dduty_end_date"].ToString().IndexOf(":") > 0)
                    {
                        string[] hms = dt.Rows[i]["Dduty_end_date"].ToString().Split(':');
                        if (hms.Length > 0 && hms[0].Length == 1)
                        {
                            dt.Rows[i]["Dduty_end_date"] = "0" + dt.Rows[i]["Dduty_end_date"].ToString();
                        }
                    }
                }
                List<EntityOaDicShift> list = EntityManager<EntityOaDicShift>.ConvertToList(dt).OrderBy(w => w.ShiftId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaDicShift>();
            }
        }

        public string GetMaxDutyID()
        {
            try
            {
                string strMaxID = "";
                  DBManager helper = new DBManager();
                string strSql = @"select max(dd.Dduty_id) from Dict_oa_duty dd";

                if (helper.ExecCommand(strSql).ToString() == null)
                {
                    strMaxID = "10001";
                }
                else
                {
                    strMaxID = IdentityHelper.GetMedIdentity("Dict_oa_duty").ToString();
                }
                return strMaxID; ;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return "";
            }
       
        }

        public int InsertIntoDuty(EntityOaDicShift sample)
        {
            try
            {
                  DBManager helper = new DBManager();
                string strSql = string.Format(@"insert into Dict_oa_duty
                                      (
                                        Dduty_id,
                                        Dduty_name,
                                        Dduty_start_date,
                                        Dduty_end_date,
                                        Dduty_flag,
                                        wb_code,
                                        py_code,
                                        Dduty_remark,
                                        del_flag,
                                        Dduty_Ddept_id
                                      )
                                    values
                                      ('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}'
                                      )", sample.ShiftId,
                                           sample.ShiftName,
                                           sample.ShiftStartDate,
                                           sample.ShiftEndDate,
                                           sample.ShiftFlag,
                                           sample.WbCode,
                                           sample.PyCode,
                                           sample.ShiftRemark,
                                           sample.DelFlag,
                                           sample.ShiftDeptId);

                int intRet = helper.ExecCommand(strSql);

                return intRet;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return -1;
            }
      
        }

        public int ModifyDutyRecord(EntityOaDicShift sample)
        {
            try
            {
                  DBManager helper = new DBManager();
                string sql = string.Format(@"update Dict_oa_duty
                                            set
	                                            Dduty_name ='{0}',
	                                            Dduty_start_date ='{1}',
	                                            Dduty_end_date = '{2}',
	                                            Dduty_flag = '{3}',
	                                            wb_code = '{4}',
	                                            py_code = '{5}',
	                                            Dduty_remark = '{6}',
	                                            del_flag = '{7}',
	                                            Dduty_Ddept_id = '{8}'
                                            where Dduty_id = '{9}'", sample.ShiftName, sample.ShiftStartDate, sample.ShiftEndDate, sample.ShiftFlag,
                                                sample.WbCode, sample.PyCode, sample.ShiftRemark, sample.DelFlag, sample.ShiftDeptId, sample.ShiftId);
                int ret = helper.ExecCommand(sql);
                return ret;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return -1;
            }
             
        }

    }
}
