/*  
 * 警告：
 * 本源代码所有权归广州慧扬健康科技有限公司(下称“本公司”)所有，已采取保密措施加以保护。  受《中华人民共和国刑法》、
 * 《反不正当竞争法》和《国家工商行政管理局关于禁止侵犯商业秘密行为的若干规定》等相关法律法规的保护。未经本公司书面
 * 许可，任何人披露、使用或者允许他人使用本源代码，必将受到相关法律的严厉惩罚。
 * Warning: 
 * The ownership of this source code belongs to Guangzhou Wisefly Technology Co., Ltd.(hereinafter referred to as "the company"), 
 * which is protected by the criminal law of the People's Republic of China, the anti unfair competition law and the 
 * provisions of the State Administration for Industry and Commerce on prohibiting the infringement of business secrets, etc. 
 * Without the written permission of the company, anyone who discloses, uses or allows others to use this source code 
 * will be severely punished by the relevant laws.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoReaLossReport))]
    public class DaoReaLossReport : DclDaoBase, IDaoReaLossReport
    {
        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            try
            {
                string sql = string.Format(@"select 
max(cast(Rlr_no as bigint))
from Rea_lossreport with(nolock)
where  Rlr_date >= @pat_date_from 
and Rlr_date < @pat_date_to ");

                List<DbParameter> paramHt = new List<DbParameter>();

                paramHt.Add(new SqlParameter("@pat_date_from", date.Date.ToString("yyyy-MM-dd HH:mm:ss")));
                paramHt.Add(new SqlParameter("@pat_date_to", date.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")));

                DBManager helper = new DBManager();
                object objHostOrder = helper.SelOne(sql, paramHt);

                if (objHostOrder == null || objHostOrder == DBNull.Value)
                {
                    return stepCode + date.Date.ToString("yyyyMMdd") + "01";
                }
                else
                {
                    long maxHostOrder = Convert.ToInt64(objHostOrder);
                    return (maxHostOrder + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }

        }

        public bool ExsitSidOrHostOrder(string rea_sid, DateTime rea_in_date)
        {
            bool result = false;
            string column = "Rlr_no";
            if (rea_sid != null && rea_in_date != null)
            {
                DBManager helper = new DBManager();
                try
                {
                    //检查样本号是否已存在
                    string sql = string.Format(@"
                    select top 1 {3} from
                    Rea_lossreport with(nolock)  where {3}='{0}' 
                    and Rlr_date >= '{1}' 
                    and Rlr_date<'{2}'", rea_sid, rea_in_date.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    rea_in_date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"), column);
                    int rowCount = 0;
                    DataTable dt = helper.ExecuteDtSql(sql);
                    if (dt.Rows.Count > 0)
                        int.TryParse(dt.Rows[0][0].ToString(), out rowCount);
                    if (rowCount > 0)
                        result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public bool InsertNewReaLossReport(EntityReaLossReport LossReport)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (LossReport != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(LossReport);

                    helper.InsertOperation("Rea_lossreport", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }

        public List<EntityReaLossReport> QueryLossReportList(EntityReaQC reaQC)
        {
            string queryStr = @"SELECT *,
                     case 
                     when user1.Buser_name is null then Rlr_operator
                     else user1.Buser_name
                     end as ApplierName,
                     case 
                     when user2.Buser_name is null then Rlr_auditor
                     else user2.Buser_name
                     end as AuditorName,
                     case 
                     when user3.Buser_name is null then Rlr_returnid
                     else user3.Buser_name
                     end as ReturnName
                     FROM Rea_lossreport
                     LEFT OUTER JOIN Base_user user1 on user1.Buser_loginid = Rea_lossreport.Rlr_operator
                     LEFT OUTER JOIN Base_user user2 on user2.Buser_loginid = Rea_lossreport.Rlr_auditor
                     LEFT OUTER JOIN Base_user user3 on user3.Buser_loginid = Rea_lossreport.Rlr_returnid
                     where 1 = 1 and Rea_lossreport.del_flag = 0 {0}";

            string sqlWhere = string.Empty;

            //时间范围
            if (!string.IsNullOrEmpty(reaQC.DateStart.ToString()) && !string.IsNullOrEmpty(reaQC.DateEnd.ToString()))
            {
                sqlWhere += string.Format(" and Rea_lossreport.Rlr_date BETWEEN '{0}' AND '{1}'", reaQC.DateStart.ToString(), reaQC.DateEnd.ToString());

            }
            //状态
            if (!string.IsNullOrEmpty(reaQC.ReaStatus))
            {
                sqlWhere += string.Format(@" and Rea_lossreport.Rlr_status='{0}' ", reaQC.ReaStatus);
            }
            if (!string.IsNullOrEmpty(reaQC.ReaPrintFlag))
            {
                sqlWhere += string.Format(@" and Rea_lossreport.Rlr_printflag='{0}' ", reaQC.ReaPrintFlag);
            }
            if (!string.IsNullOrEmpty(reaQC.ReaNo))
            {
                sqlWhere += string.Format(@" and Rea_lossreport.Rlr_no='{0}' ", reaQC.ReaNo);
            }

            DBManager dBManager = GetDbManager();
            Lib.LogManager.Logger.LogInfo(string.Format(queryStr, sqlWhere));
            DataTable dt = dBManager.ExecuteDtSql(string.Format(queryStr, sqlWhere));
            List<EntityReaLossReport> LossReportMains = EntityManager<EntityReaLossReport>.ConvertToList(dt);
            return LossReportMains;
        }

        public bool UpdateReaLossReportData(EntityReaLossReport reaLossReport)
        {
            bool result = false;
            if (reaLossReport != null)
            {
                DBManager helper = GetDbManager();
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBUpdateParameter(reaLossReport);

                    values.Remove("Rlr_no");

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Rlr_no", reaLossReport.Rlr_no);

                    helper.UpdateOperation("Rea_lossreport", values, keys);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw ex;
                }
            }
            return result;
        }

        public bool DeleteReaLossReportData(EntityReaLossReport reaLossReport)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rlr_no", reaLossReport.Rlr_no);

                helper.UpdateOperation("Rea_lossreport", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool ReturnReaLossReportData(EntityReaLossReport reaLossReport)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rlr_returnid", reaLossReport.Rlr_returnid);
                values.Add("Rlr_returndate", reaLossReport.Rlr_returndate);
                values.Add("Rlr_returnreason", reaLossReport.Rlr_returnreason);
                values.Add("Rlr_status", reaLossReport.Rlr_status);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rlr_no", reaLossReport.Rlr_no);

                helper.UpdateOperation("Rea_lossreport", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public void UpdateLossReportStatus(string rayNo, string printId, DateTime date)
        {
            if (!string.IsNullOrEmpty(rayNo))
            {
                try
                {
                    DBManager helper = new DBManager();

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Rlr_printflag", 1);
                    values.Add("Rlr_printdate", date);
                    values.Add("Rlr_printid", printId);
                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Rlr_no", rayNo);

                    helper.UpdateOperation("Rea_lossreport", values, keys);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }
    }
}
