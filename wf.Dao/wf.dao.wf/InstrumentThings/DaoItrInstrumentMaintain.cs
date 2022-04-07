using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.dao.core;
using dcl.common;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoItrInstrumentMaintain))]
    public class DaoItrInstrumentMaintain : IDaoItrInstrumentMaintain
    {
        public bool AddInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_instrmt_maintain");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                StringBuilder strSql = new StringBuilder();

                values.Add("Dim_id", id);
                values.Add("Dim_Ditr_id", instrmtMaintain.MaiItrId);
                values.Add("Dim_interval", instrmtMaintain.MaiInterval);
                values.Add("Dim_content", instrmtMaintain.MaiContent);
                values.Add("Dim_type", instrmtMaintain.MaiType);

                values.Add("Dim_astrict", instrmtMaintain.MaiAstrict);
                values.Add("Dim_warning_time", instrmtMaintain.MaiWarningTime);
                values.Add("Dim_closeaudit_time", instrmtMaintain.MaiCloseAuditTime);
                values.Add("Dim_interval_year", instrmtMaintain.MaiIntervalYear);
                values.Add("Dim_interval_month", instrmtMaintain.MaiIntervalMonth);

                values.Add("Dim_interval_day", instrmtMaintain.MaiIntervalDay);
                values.Add("Dim_warning_time_year", instrmtMaintain.MaiWarningTimeYear);
                values.Add("Dim_warning_time_month", instrmtMaintain.MaiWarningTimeMonth);
                values.Add("Dim_warning_time_day", instrmtMaintain.MaiWarningTimeDay);
                values.Add("Dim_closeaudit_time_year", instrmtMaintain.MaiCloseAuditTimeYear);

                values.Add("Dim_closeaudit_time_month", instrmtMaintain.MaiCloseAuditTimeMonth);
                values.Add("Dim_closeaudit_time_day", instrmtMaintain.MaiCloseAuditTimeDay);
                values.Add("Dim_operate_tips", instrmtMaintain.MaiOperateTips);
                values.Add("Dim_fuzzy_interval_time", instrmtMaintain.FuzzyIntervalTime);
                values.Add("Dim_fuzzy_closeAudit_time", instrmtMaintain.FuzzyCloseAuditTime);

                values.Add("Dim_fuzzy_warning_time", instrmtMaintain.FuzzyWarningTime);
                values.Add("Dim_desc", instrmtMaintain.MaiDesc);

                helper.InsertOperation("Dict_instrmt_maintain", values);

                instrmtMaintain.MaiId = id;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteInstrmtMaintainByID(int mai_id)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSql = string.Format(@"delete from Dict_instrmt_maintain where Dim_id={0} ", mai_id);

                helper.ExecSql(strSql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItrInstrumentMaintain> GetInstrmtMaintains(string itr_id)
        {
            try
            {
                String sql = string.Format(@"select Dim_id,Dim_Ditr_id,Dim_interval,Dim_content,Dim_type,
Dim_astrict,Dim_warning_time,Dim_closeaudit_time,Dim_interval_year,
Dim_interval_month,Dim_interval_day,Dim_warning_time_year,
Dim_warning_time_month,Dim_warning_time_day,Dim_closeaudit_time_year,
Dim_closeaudit_time_month,Dim_closeaudit_time_day,Dim_operate_tips,
isnull(Dim_fuzzy_warning_time,'') Dim_fuzzy_warning_time,
isnull(Dim_fuzzy_closeAudit_time,'') Dim_fuzzy_closeAudit_time,
isnull(Dim_fuzzy_interval_time,'') Dim_fuzzy_interval_time,
Dim_desc
FROM Dict_instrmt_maintain ");

                if (itr_id.Trim() != "")
                {
                    sql += string.Format(@" where  Dim_Ditr_id='{0}' ", itr_id);
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicItrInstrumentMaintain> list = EntityManager<EntityDicItrInstrumentMaintain>.ConvertToList(dt).OrderBy(i => i.MaiId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItrInstrumentMaintain>();
            }
        }

        public bool UpdateInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Dim_Ditr_id", instrmtMaintain.MaiItrId);
                values.Add("Dim_interval", instrmtMaintain.MaiInterval);
                values.Add("Dim_content", instrmtMaintain.MaiContent);
                values.Add("Dim_type", instrmtMaintain.MaiType);

                values.Add("Dim_astrict", instrmtMaintain.MaiAstrict);
                values.Add("Dim_warning_time", instrmtMaintain.MaiWarningTime);
                values.Add("Dim_closeaudit_time", instrmtMaintain.MaiCloseAuditTime);
                values.Add("Dim_interval_year", instrmtMaintain.MaiIntervalYear);
                values.Add("Dim_interval_month", instrmtMaintain.MaiIntervalMonth);

                values.Add("Dim_interval_day", instrmtMaintain.MaiIntervalDay);
                values.Add("Dim_warning_time_year", instrmtMaintain.MaiWarningTimeYear);
                values.Add("Dim_warning_time_month", instrmtMaintain.MaiWarningTimeMonth);
                values.Add("Dim_warning_time_day", instrmtMaintain.MaiWarningTimeDay);
                values.Add("Dim_closeaudit_time_year", instrmtMaintain.MaiCloseAuditTimeYear);

                values.Add("Dim_closeaudit_time_month", instrmtMaintain.MaiCloseAuditTimeMonth);
                values.Add("Dim_closeaudit_time_day", instrmtMaintain.MaiCloseAuditTimeDay);
                values.Add("Dim_operate_tips", instrmtMaintain.MaiOperateTips);
                values.Add("Dim_fuzzy_interval_time", instrmtMaintain.FuzzyIntervalTime);
                values.Add("Dim_fuzzy_closeAudit_time", instrmtMaintain.FuzzyCloseAuditTime);

                values.Add("Dim_fuzzy_warning_time", instrmtMaintain.FuzzyWarningTime);

                values.Add("Dim_desc", instrmtMaintain.MaiDesc);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dim_id", instrmtMaintain.MaiId);

                helper.UpdateOperation("Dict_instrmt_maintain", values, keys);

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
