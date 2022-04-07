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
    [Export("wf.plugin.wf", typeof(IDaoOaDicShiftDetail))]
    public class DaoOaDicShiftDetail : IDaoOaDicShiftDetail
    {
        public bool CopyShiftPlan(string timeFrom ,string timeTo)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = "insert into Rel_oa_duty_detail(Rdutydet_Buser_id, Rdutydet_Dduty_id, Rdutydet_date)(select Rdutydet_Buser_id, Rdutydet_Dduty_id, '" + timeFrom + "' Rdutydet_date from Rel_oa_duty_detail  where Rdutydet_date = '" + timeTo + "')";
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteShiftPlan(string isshow, string timeFrom, string timeTo, string typeId)
        {
            try
            {
                string sql = "";
                DBManager helper = new DBManager();
                if (isshow != "是")
                {
                    sql = "delete from Rel_oa_duty_detail where  Rdutydet_date>= '" + timeFrom + "' and Rdutydet_date<= '" + timeTo + "'";
                    helper.ExecSql(sql);
                    return true;
                }
                else
                {
                    sql = "delete from Rel_oa_duty_detail where Rdutydet_type = '" + typeId + "' and Rdutydet_date>= '" + timeFrom + "' and Rdutydet_date< '" + timeTo + "'";
                    helper.ExecSql(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityOaDicShiftDetail> GetShiftPlan(DateTime sDate, DateTime eDate, string strType)
        {
            try
            {
                DBManager helper = new DBManager();
                string strSql = @"select --ddd.ddetail_id,
ddd.Rdutydet_Buser_id,
p.Buser_name,
ddd.Rdutydet_Dduty_id,
ddd.Rdutydet_date,
dd.Dduty_name,
dd.Dduty_start_date,
dd.Dduty_end_date,
dd.Dduty_Ddept_id,
ddd.Rdutydet_type,
ddd.Rdutydet_work_post,
ddd.Rdutydet_holiday_a,
ddd.Rdutydet_holiday_b,
ddd.Rdutydet_holiday_c
from   Rel_oa_duty_detail ddd
left join Dict_oa_duty dd on  dd.Dduty_id = ddd.Rdutydet_Dduty_id
left join Base_user p on  p.Buser_id = ddd.Rdutydet_Buser_id
where  ddd.Rdutydet_date between '{0}' and '{1}'";
                strSql = string.Format(strSql, sDate.ToString("yyyy-MM-dd HH:mm:ss"), eDate.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"));

                if (!strType.Equals("All"))
                {
                    strSql += " and dd.Dduty_Ddept_id = '{0}'";
                    strSql = string.Format(strSql, strType);
                }
                DataTable dt = helper.ExecuteDtSql(strSql);
                List<EntityOaDicShiftDetail> list = EntityManager<EntityOaDicShiftDetail>.ConvertToList(dt).OrderBy(w => w.DetailUserId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaDicShiftDetail>();
            }
        }


        public int InsertShiftPlan(EntityOaDicShiftDetail sample)
        {
            try
            {
                DBManager helper = new DBManager();
                string strSql = string.Format(@"insert into Rel_oa_duty_detail
                                      (
                                          Rdutydet_Buser_id,
                                          Rdutydet_Dduty_id,
                                          Rdutydet_date
                                      )
                                      values('{0}','{1}','{2}' 	)", sample.DetailUserId,sample.DetailShiftId,sample.DetailDate.ToString("yyyy-MM-dd HH:mm:ss"));

                int intRet = helper.ExecCommand(strSql);

                return intRet;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return -1;
            }
        }

    }
}
