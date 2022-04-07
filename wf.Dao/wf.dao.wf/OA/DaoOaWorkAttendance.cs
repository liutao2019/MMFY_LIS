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
    [Export("wf.plugin.wf", typeof(IDaoOaWorkAttendance))]
    public class DaoOaWorkAttendance : IDaoOaWorkAttendance
    {

        public string GetAttdRecordByUID(string[] userIDandIsWork)
        {
            string strSql = null;
            string attdRecordID = string.Empty;
            DBManager helper = new DBManager();
            if (userIDandIsWork[1] == "true")
            {
                strSql = @"select dar.Oate_id
from   Oa_attendance_sheet dar
where  dar.Oate_Buser_id = '{0}' and dar.Oate_date = '{1}' and Oate_start_date is not null and (Oate_end_date is NULL OR Oate_end_date='') ";
                strSql = string.Format(strSql, userIDandIsWork[0], Convert.ToDateTime(DateTime.Now.ToString("d")));
            }
            else
            {
                //先查找是否存在跨天的未下班的考勤
                string strSql2 = @"select top 1 dar.Oate_id
from   Oa_attendance_sheet dar
where  dar.Oate_Buser_id = '{0}' and dar.Oate_date >= '{1}' and Oate_start_date is not null and (Oate_end_date is NULL OR Oate_end_date='') and Oate_shift_start_date>Oate_shift_end_date order by Oate_date desc ";
                strSql2 = string.Format(strSql2, userIDandIsWork[0], Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("d")));
                if (helper.ExecuteDtSql(strSql2).Rows.Count > 0)
                {
                    attdRecordID = helper.ExecuteDtSql(strSql2).Rows[0]["Oate_id"].ToString() == null ? "" : helper.ExecuteDtSql(strSql2).Rows[0]["Oate_id"].ToString();
                }
                else
                {
                    attdRecordID = "";
                }
                if (string.IsNullOrEmpty(attdRecordID))
                {
                    //两天内未下班的考勤，取最新时间那个
                    strSql = @"select top 1 dar.Oate_id
from   Oa_attendance_sheet dar
where  dar.Oate_Buser_id = '{0}' and dar.Oate_date >= '{1}' and Oate_start_date is not null and (Oate_end_date is NULL OR Oate_end_date='') order by Oate_date desc  ";
                    strSql = string.Format(strSql, userIDandIsWork[0],
                                           Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("d")));
                }
            }
            if (string.IsNullOrEmpty(attdRecordID))
            {
                if (helper.ExecuteDtSql(strSql).Rows.Count > 0)
                {
                    attdRecordID = helper.ExecuteDtSql(strSql).Rows[0]["Oate_id"].ToString() == null ? "" : helper.ExecuteDtSql(strSql).Rows[0]["Oate_id"].ToString();
                }
                else
                {
                    attdRecordID = "";
                }
            }

            return attdRecordID;
        }
        public List<EntityOaWorkAttendance> GetAttRecordByUID(DateTime sDTime, DateTime eDTime)
        {

            try
            {
                string strSql = @"select dar.Oate_id,
dar.Oate_Buser_id,
dar.Oate_Dduty_id,
dar.Oate_date,
dar.Oate_start_date,
dar.Oate_end_date,
dar.Oate_flag,
dar.Oate_remark,
dar.Oate_shift_start_date, 
dar.Oate_shift_end_date,
dar.Oate_workhours,
p.Buser_id,
p.Buser_loginid,
p.Buser_name ,
p.Buser_type,
p.py_code py,
p.wb_code wb_code,
dd.Dduty_name,
dd.wb_code, 
dd.py_code,
case when CAST(Oate_start_date as DATETIME)>CAST(Oate_shift_start_date as DATETIME) then 1 else 0 end lateflag
from   Oa_attendance_sheet dar
left join Base_user p on p.Buser_id = dar.Oate_Buser_id
left join Dict_oa_duty dd on dd.Dduty_id = dar.Oate_Dduty_id
where dar.Oate_date between '{0}' and '{1}'
                            ";
                strSql = string.Format(strSql, sDTime.ToString("yyyy-MM-dd HH:mm:ss"), eDTime.ToString("yyyy-MM-dd HH:mm:ss"));
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(strSql);
                List<EntityOaWorkAttendance> list = EntityManager<EntityOaWorkAttendance>.ConvertToList(dt).OrderBy(i => i.AteId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaWorkAttendance>();
            }
        }
        public string GetMaxAttdRecordID()
        {
            string strMaxID = "";
            DBManager helper = new DBManager();
            string strSql = @"select max(dar.Oate_id) ate_id from Oa_attendance_sheet dar";

            if (helper.ExecuteDtSql(strSql).Rows.Count>0 && helper.ExecuteDtSql(strSql).Rows[0]["ate_id"].ToString()==null)
            {
                strMaxID = "10001";
            }
            else
            {
                strMaxID = IdentityHelper.GetMedIdentity("Oa_attendance_sheet").ToString();
            }
            return strMaxID; ;
        }
        public int InsertAttdRecord(EntityOaWorkAttendance sample)
        {
            DBManager helper = new DBManager();
            string strSql = @"insert into Oa_attendance_sheet
                                    (
	                                    Oate_id,
	                                    Oate_Buser_id,
	                                    Oate_Dduty_id,
                                        Oate_shift_start_date,
	                                    Oate_shift_end_date,
	                                    Oate_date,
	                                    Oate_start_date,
	                                    Oate_flag,
                                        Oate_remark
                                    )
                                    values
                                    ( '{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}')";
            strSql = string.Format(strSql, sample.AteId, sample.AteUserId, sample.AteShiftId, sample.AteShiftStartDate, sample.AteShiftEndDate, DateTime.Now.ToString("d"), DateTime.Now.ToString("HH:mm:ss"), sample.AteFlag, sample.AteRemark);

            int intRet = helper.ExecCommand(strSql);

            return intRet;
        }
        public int ModifyAttdRecord(EntityOaWorkAttendance sample)
        {
            int intRet = -1;
            string strSql;
            DBManager helper = new DBManager();
            //下班操作
            if (string.IsNullOrEmpty(sample.AteEndDate))
            {
                strSql = @"select  dar.Oate_start_date,dar.Oate_shift_end_date,dar.Oate_shift_start_date,dar.Oate_date
from   Oa_attendance_sheet dar
where  dar.Oate_id = '{0}' ";
                strSql = string.Format(strSql, sample.AteId);
                string attdSdate = string.Empty;
                string dutyEDate = string.Empty;
                string dutySDate = string.Empty;

                DataTable tb = helper.ExecuteDtSql(strSql);
                if (tb.Rows.Count > 0)
                {
                    attdSdate = tb.Rows[0]["Oate_start_date"] != null && tb.Rows[0]["Oate_start_date"] != DBNull.Value
                                    ? tb.Rows[0]["Oate_start_date"].ToString()
                                    : string.Empty;
                    dutyEDate = tb.Rows[0]["Oate_shift_end_date"] != null && tb.Rows[0]["Oate_shift_end_date"] != DBNull.Value
                                    ? tb.Rows[0]["Oate_shift_end_date"].ToString()
                                    : string.Empty;
                    dutySDate = tb.Rows[0]["Oate_shift_start_date"] != null && tb.Rows[0]["Oate_shift_start_date"] != DBNull.Value
                                    ? tb.Rows[0]["Oate_shift_start_date"].ToString()
                                    : string.Empty;
                    sample.AteDate = tb.Rows[0]["Oate_date"] != null && tb.Rows[0]["Oate_date"] != DBNull.Value
                                           ? Convert.ToDateTime(tb.Rows[0]["Oate_date"].ToString())
                                           : DateTime.Now.Date;
                }
                //注释里说明情况
                if (!string.IsNullOrEmpty(attdSdate)
                    &&
                    (Convert.ToDateTime(attdSdate).ToString("HH:mm") == DateTime.Now.ToString("HH:mm") //上班与下班时间间隔1-2分钟时
                     || Convert.ToDateTime(attdSdate).ToString("HH:mm") == DateTime.Now.AddMinutes(-1).ToString("HH:mm"))
                    && string.IsNullOrEmpty(sample.AteRemark))
                {
                    sample.AteRemark = "早退 或者 忘记打卡";
                }
                else if (!string.IsNullOrEmpty(dutyEDate) && !string.IsNullOrEmpty(dutySDate)
                         &&
                         ((Convert.ToDateTime(dutyEDate) > Convert.ToDateTime(dutySDate) &&
                           DateTime.Now < Convert.ToDateTime(dutyEDate))
                          ||
                          (Convert.ToDateTime(dutyEDate) < Convert.ToDateTime(dutySDate) &&
                           DateTime.Now <
                           Convert.ToDateTime(sample.AteDate.ToString("yyy-MM-dd") + " " + dutyEDate).AddDays(1))))
                {
                    sample.AteRemark = "早退";
                }
                string attdedate = DateTime.Now.ToString("HH:mm:ss");

                if (DateTime.Now.Date > sample.AteDate.Date)
                {
                    attdedate = attdedate + "(" + DateTime.Now.ToString("MM-dd") + ")";
                }
                string workHours = (DateTime.Now -
                                    Convert.ToDateTime(sample.AteDate.ToString("yyy-MM-dd") + " " + attdSdate))
                    .TotalHours
                    .ToString(
                        "F01");
                strSql = @"update Oa_attendance_sheet
                                set    Oate_end_date = '{0}',
                                        Oate_remark = Oate_remark+'{1}',
                                        Oate_workhours={4}
                                where  Oate_id = '{2}' and Oate_date = '{3}'
                                    ";
                strSql = string.Format(strSql, attdedate, sample.AteRemark, sample.AteId, sample.AteDate, workHours);
                intRet = helper.ExecCommand(strSql);
            }
            else//更新操作
            {
                strSql = @"update Oa_attendance_sheet
                                set    Oate_remark = '{0}'
                                where  Oate_id = '{1}' and Oate_date = '{2}'
                                    ";
                strSql = string.Format(strSql, sample.AteRemark, sample.AteId, sample.AteDate);
                intRet = helper.ExecCommand(strSql);
            }

            return intRet;
        }

    }
}
