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
    /// <summary>
    /// 组合明细拆分字典表
    /// </summary>
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSampDivergeRule>))]
    public class DaoDicSampDivergeRule : IDaoDic<EntityDicSampDivergeRule>
    {
        public bool Save(EntityDicSampDivergeRule combine)
        {
            try
            {
                //int id = IdentityHelper.GetMedIdentity("Def_samp_diverge_rule");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rsdr_Dcom_id", combine.ComId);
                values.Add("Rsdr_split_id", combine.ComSplitId);

                helper.InsertOperation("Rel_sample_diverge_rule", values);

                //combine.ComSplitId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicSampDivergeRule combine)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rsdr_split_id", combine.ComSplitId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rsdr_Dcom_id", combine.ComId);

                helper.UpdateOperation("Rel_sample_diverge_rule", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicSampDivergeRule combine)
        {
            try
            {
                DBManager helper = new DBManager();

                string sql = string.Format(@"delete Rel_sample_diverge_rule where Rsdr_Dcom_id='{0}' ", combine.ComId);
                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicSampDivergeRule> Search(Object obj)
        {
                try
                {

                List<string> list = obj as List<string>;

                var id = list[0];
                String sql = string.Empty;
                if (list[1].ToString() == "Search")
                {
                    sql = String.Format(@"select Dict_itm_combine.* from Dict_itm_combine LEFT OUTER JOIN Dict_profession ON Dict_itm_combine.Dcom_Dpro_id = Dict_profession.Dpro_id  where (Dict_itm_combine.del_flag = '0') and Dcom_id in ( select Rsdr_split_id from Rel_sample_diverge_rule where Rsdr_Dcom_id = '{0}')", id);
                }
                else
                {
                    sql = String.Format(@"select Dict_itm_combine.* from Dict_itm_combine LEFT OUTER JOIN Dict_profession ON Dict_itm_combine.Dcom_Dpro_id = Dict_profession.Dpro_id  where (Dict_itm_combine.del_flag = '0') and Dcom_id not in ( select Rsdr_split_id from Rel_sample_diverge_rule where Rsdr_Dcom_id = '{0}')", id);
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                    return ConvertToEntitys(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return new List<EntityDicSampDivergeRule>();
                }
        }

        public List<EntityDicSampDivergeRule> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicSampDivergeRule> list = new List<EntityDicSampDivergeRule>();
            foreach (DataRow item in dtSour.Rows)
            {

                EntityDicSampDivergeRule com = new EntityDicSampDivergeRule();
                com.ComSplitId = item["Dcom_id"].ToString();
                com.ComName = item["Dcom_name"].ToString();
                list.Add(com);
            }
            return list.OrderBy(i => i.ComSplitId).ToList();
        }
    }
}
