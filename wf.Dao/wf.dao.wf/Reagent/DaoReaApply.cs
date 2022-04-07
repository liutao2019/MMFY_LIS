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
    [Export("wf.plugin.wf", typeof(IDaoReaApply))]
    public class DaoReaApply : DclDaoBase, IDaoReaApply
    {
        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            try
            {
                string sql = string.Format(@"select 
max(cast(Ray_no as bigint))
from Rea_apply with(nolock)
where  Ray_date >= @pat_date_from 
and Ray_date < @pat_date_to ");

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
            string column = "Ray_no";
            if (rea_sid != null && rea_in_date != null)
            {
                DBManager helper = new DBManager();
                try
                {
                    //检查样本号是否已存在
                    string sql = string.Format(@"
                    select top 1 {3} from
                    Rea_apply with(nolock)  where {3}='{0}' 
                    and Ray_date >= '{1}' 
                    and Ray_date<'{2}'", rea_sid, rea_in_date.Date.ToString("yyyy-MM-dd HH:mm:ss"),
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

        public bool InsertNewReaApply(EntityReaApply apply)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (apply != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(apply);

                    helper.InsertOperation("Rea_apply", values);

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

        public List<EntityReaApply> QueryApplyList(EntityReaQC reaQC)
        {
            string queryStr = @"SELECT *,
                     case 
                     when user1.Buser_name is null then Ray_applier
                     else user1.Buser_name
                     end as ApplierName,
                     case 
                     when user2.Buser_name is null then Ray_auditor
                     else user2.Buser_name
                     end as AuditorName,
                     case 
                     when user3.Buser_name is null then Ray_returnid
                     else user3.Buser_name
                     end as ReturnName
                     FROM Rea_apply
                     LEFT OUTER JOIN Base_user user1 on user1.Buser_loginid = Rea_apply.Ray_applier
                     LEFT OUTER JOIN Base_user user2 on user2.Buser_loginid = Rea_apply.Ray_auditor
                     LEFT OUTER JOIN Base_user user3 on user3.Buser_loginid = Rea_apply.Ray_returnid
                     where 1 = 1 and Rea_apply.del_flag = 0 {0}";

            string sqlWhere = string.Empty;

            //时间范围
            if (!string.IsNullOrEmpty(reaQC.DateStart.ToString()) && !string.IsNullOrEmpty(reaQC.DateEnd.ToString()))
            {
                sqlWhere += string.Format(" and Rea_apply.Ray_applydate BETWEEN '{0}' AND '{1}'", reaQC.DateStart.ToString(), reaQC.DateEnd.ToString());

            }
            //状态
            if (!string.IsNullOrEmpty(reaQC.ReaStatus))
            {
                sqlWhere += string.Format(@" and Rea_apply.Ray_status='{0}' ", reaQC.ReaStatus);
            }
            if (!string.IsNullOrEmpty(reaQC.ReaPrintFlag))
            {
                sqlWhere += string.Format(@" and Rea_apply.Ray_printflag='{0}' ", reaQC.ReaPrintFlag);
            }
            DBManager dBManager = GetDbManager();
            Lib.LogManager.Logger.LogInfo(string.Format(queryStr, sqlWhere));
            DataTable dt = dBManager.ExecuteDtSql(string.Format(queryStr, sqlWhere));
            List<EntityReaApply> applyMains = EntityManager<EntityReaApply>.ConvertToList(dt);
            return applyMains;
        }

        public bool UpdateReaApplyData(EntityReaApply reaApply)
        {
            bool result = false;
            if (reaApply != null)
            {
                DBManager helper = GetDbManager();
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBUpdateParameter(reaApply);

                    values.Remove("Ray_no");

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Ray_no", reaApply.Ray_no);

                    helper.UpdateOperation("Rea_apply", values, keys);
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

        public bool DeleteReaApplyData(EntityReaApply reaApply)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ray_no", reaApply.Ray_no);

                helper.UpdateOperation("Rea_apply", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool ReturnReaApplyData(EntityReaApply reaApply)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ray_returnid", reaApply.Ray_returnid);
                values.Add("Ray_returndate", reaApply.Ray_returndate);
                values.Add("Ray_rejectreason", reaApply.Ray_rejectreason);
                values.Add("Ray_status", reaApply.Ray_status);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ray_no", reaApply.Ray_no);

                helper.UpdateOperation("Rea_apply", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取单个试剂申领信息
        /// </summary>
        /// <param name="repID"></param>
        /// <returns></returns>
        public EntityReaApply GetReaApplyInfo(string rayNo)
        {
            DBManager helper = new DBManager();// GetDbManager();
            string sql = string.Format(@"SELECT *,
                     case 
                     when user1.Buser_name is null then Ray_applier
                     else user1.Buser_name
                     end as ApplierName,
                     case 
                     when user2.Buser_name is null then Ray_auditor
                     else user2.Buser_name
                     end as AuditorName,
                     case 
                     when user3.Buser_name is null then Ray_returnid
                     else user3.Buser_name
                     end as ReturnName
                     FROM Rea_apply
                     LEFT OUTER JOIN Base_user user1 on user1.Buser_loginid = Rea_apply.Ray_applier
                     LEFT OUTER JOIN Base_user user2 on user2.Buser_loginid = Rea_apply.Ray_auditor
                     LEFT OUTER JOIN Base_user user3 on user3.Buser_loginid = Rea_apply.Ray_returnid
                     where 1 = 1 and Rea_apply.del_flag = 0  and Ray_no='{0}'", rayNo);

            DataTable dt = helper.ExecuteDtSql(sql);
            var listReaApply = EntityManager<EntityReaApply>.ConvertToList(dt).ToList();

            if (listReaApply.Count > 0)
                return listReaApply[0];
            else
            {
                //新增是有可能返回空行！
                //Lib.LogManager.Logger.LogInfo("返回空行", sql);
            }

            return null;
        }

        public void UpdateStatus(string rayNo, int status)
        {
            if (!string.IsNullOrEmpty(rayNo))
            {
                try
                {
                    DBManager helper = new DBManager();

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Ray_status", status);
                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Ray_no", rayNo);

                    helper.UpdateOperation("Rea_apply", values, keys);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }

        public void UpdateApplyStatus(string rayNo, string printId, DateTime date)
        {
            if (!string.IsNullOrEmpty(rayNo))
            {
                try
                {
                    DBManager helper = new DBManager();

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Ray_printflag", 1);
                    values.Add("Ray_printdate", date);
                    values.Add("Ray_printid", printId);
                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Ray_no", rayNo);

                    helper.UpdateOperation("Rea_apply", values, keys);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }

    }
}
