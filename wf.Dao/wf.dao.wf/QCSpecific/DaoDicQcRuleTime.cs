using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicQcRuleTime>))]
    public class DaoDicQcRuleTime : IDaoDic<EntityDicQcRuleTime>
    {
        public bool Save(EntityDicQcRuleTime rule)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_qc_rule_time");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rqrt_start_time", rule.QrtStartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Rqrt_end_time", rule.QrtEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Rqrt_day", rule.QrtDay);
                values.Add("Rqrt_Ditr_id", rule.QrtItrId);

                helper.InsertOperation("Rel_qc_rule_time", values);

                //rule.QrtId = id;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicQcRuleTime rule)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                
                values.Add("Rqrt_start_time", rule.QrtStartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Rqrt_end_time", rule.QrtEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("Rqrt_day", rule.QrtDay);
                values.Add("Rqrt_Ditr_id", rule.QrtItrId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rqrt_id", rule.QrtId);

                helper.UpdateOperation("Rel_qc_rule_time", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicQcRuleTime rule)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = "";

                if (rule != null)
                {
                    sql = string.Format(@"delete Rel_qc_rule_time where Rqrt_id='{0}' ", rule.QrtId);
                }
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcRuleTime> Search(Object obj)
        {
            List<EntityDicQcRuleTime> listRuleTime = new List<EntityDicQcRuleTime>();
            DBManager helper = new DBManager();
            if (obj is bool)
            {
                try
                {
                    string qrtIDStr = string.Format(@"select ident_current('Rel_qc_rule_time') ");
                    List<DbParameter> paramHt = new List<DbParameter>();
                    object objQrtId = helper.SelOne(qrtIDStr, paramHt);

                    if (objQrtId != null)
                    {
                        int k = Convert.ToInt32(objQrtId);
                        EntityDicQcRuleTime ruleTime = new EntityDicQcRuleTime();
                        ruleTime.QrtId = k;
                        listRuleTime.Add(ruleTime);
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            else
            {
                try
                {
                    EntityDicQcRuleTime time = obj as EntityDicQcRuleTime;

                    String sql = string.Format(@"select * from Rel_qc_rule_time where Rqrt_Ditr_id='{0}' ", time.QrtItrId);
                    
                    DataTable dt = helper.ExecuteDtSql(sql);

                    return ConvertToEntitys(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }

            return listRuleTime;
        }

        public List<EntityDicQcRuleTime> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicQcRuleTime> list = EntityManager<EntityDicQcRuleTime>.ConvertToList(dtSour);

            return list.ToList();
        }
    }
}
