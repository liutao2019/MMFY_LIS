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

namespace dcl.dao.clab
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicQcRule>))]
    public class DaoDicrule : IDaoDic<EntityDicQcRule>
    {
        public bool Save(EntityDicQcRule rule)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_qc_rule");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Drule_id", id);
                values.Add("Drule_name", rule.RulName);
                values.Add("Drule_explain", rule.RulExplain);
                values.Add("Drule_n_amount", rule.RulNAmount);
                values.Add("Drule_m_amount", rule.RulMAmount);
                values.Add("Drule_sd_amount", rule.RulSdAmount);
                values.Add("Drule_level_type", rule.RulLevelType);
                values.Add("wb_code", rule.WbCode);
                values.Add("py_code", rule.PyCode);
                values.Add("sort_no", rule.SortNo);
                values.Add("del_flag", rule.DelFlag);
                values.Add("Drule_type", rule.RulType);
                values.Add("Drule_ismorelevel", rule.RulIsMoreLevel);
                values.Add("Drule_isdesc", rule.RulIsDesc);
                
                helper.InsertOperation("Dict_qc_rule", values);

                rule.RulId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicQcRule rule)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                
                values.Add("Drule_name", rule.RulName);
                values.Add("Drule_explain", rule.RulExplain);
                values.Add("Drule_n_amount", rule.RulNAmount);
                values.Add("Drule_m_amount", rule.RulMAmount);
                values.Add("Drule_sd_amount", rule.RulSdAmount);
                values.Add("Drule_level_type", rule.RulLevelType);
                values.Add("wb_code", rule.WbCode);
                values.Add("py_code", rule.PyCode);
                values.Add("sort_no", rule.SortNo);
                values.Add("del_flag", rule.DelFlag);
                values.Add("Drule_type", rule.RulType);
                values.Add("Drule_ismorelevel", rule.RulIsMoreLevel);
                values.Add("Drule_isdesc", rule.RulIsDesc);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Drule_id", rule.RulId);

                helper.UpdateOperation("Dict_qc_rule", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicQcRule rule)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Drule_id", rule.RulId);

                helper.UpdateOperation("Dict_qc_rule", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcRule> Search(Object obj)
        {
            try
            {
                String sql = @"select * from Dict_qc_rule where del_flag='0'";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcRule>();
            }
        }

        public List<EntityDicQcRule> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicQcRule> list = EntityManager<EntityDicQcRule>.ConvertToList(dtSour);
            
            return list.OrderBy(i => i.SortNo).ToList();
        }
    }
}
