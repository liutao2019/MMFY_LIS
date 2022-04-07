using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoQcMateriaRule))]
    public class DaoQcMateriaRule : IDaoQcMateriaRule
    {
        public bool DeleteQcMateriaRule(string matSn, string matId)
        {
            try
            {
                DBManager helper = new DBManager();
                string sqlStr = string.Format(@"delete from Rel_qc_materia_rule  where Rmr_Dmat_id='{0}' and Rmr_id='{1}'  ", matSn, matId);
                helper.ExecSql(sqlStr);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcRule> GetQcRule(List<String> listMatSnItmId)
        {
            List<EntityDicQcRule> listQcRule = new List<EntityDicQcRule>();

            try
            {
                String sbWhere = string.Empty;
                foreach (String item in listMatSnItmId)
                {
                    string[] strParRule = item.Split('&');

                    if (strParRule.Length == 2)
                    {                            //or前面不能加空格,跟下面Remove(0, 2)有关
                        sbWhere += string.Format("or (Rmr_Dmat_id='{0}' and Rmr_id={1})", strParRule[0], strParRule[1]);
                    }
                }

                if (sbWhere.Length > 0)
                {
                    sbWhere = sbWhere.Remove(0, 2);
                }

                string strSql = string.Format(@"select * from Rel_qc_materia_rule
                                                left join Dict_qc_rule on Rel_qc_materia_rule.Rmr_Drule_id = Dict_qc_rule.Drule_id and Dict_qc_rule.del_flag = '0'
                                                where {0}", sbWhere);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Select("Rmr_Drule_id='-1'").Length > 0)
                        listQcRule = GetWestgardRule();
                    else
                        listQcRule = EntityManager<EntityDicQcRule>.ConvertToList(dt).OrderBy(o => o.SortNo).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listQcRule;
        }

        private List<EntityDicQcRule> GetWestgardRule()
        {
            List<EntityDicQcRule> listQcRule = new List<EntityDicQcRule>();

            listQcRule.Add(new EntityDicQcRule("1-2S", 1, 1, 2, 1, 0, "警告", 0, 0));
            listQcRule.Add(new EntityDicQcRule("1-3S", 1, 1, 3, 1, 1, "失控", 0, 0));
            listQcRule.Add(new EntityDicQcRule("2-2S", 2, 2, 2, 1, 2, "失控", 0, 0));
            listQcRule.Add(new EntityDicQcRule("2-2S多水平", 2, 2, 2, 1, 2, "失控", 1, 0));
            listQcRule.Add(new EntityDicQcRule("R-4S", 2, 2, 4, 0, 3, "失控", 1, 0));
            listQcRule.Add(new EntityDicQcRule("4-1S", 4, 4, 1, 1, 4, "失控", 0, 0));
            listQcRule.Add(new EntityDicQcRule("10-X", 10, 10, 0, 1, 5, "失控", 0, 0));
            listQcRule.Add(new EntityDicQcRule("10-X多水平", 10, 10, 0, 1, 5, "失控", 1, 0));
            return listQcRule;
        }

        public bool SaveQcMateriaRule(EntityDicQcMateriaRule qcMateriaRule)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rmr_Dmat_id", qcMateriaRule.MatSn);
                values.Add("Rmr_Drule_id", qcMateriaRule.RulId);
                values.Add("Rmr_id", qcMateriaRule.MatItmId);

                helper.InsertOperation("Rel_qc_materia_rule", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicQcMateriaRule> SearchQcMateriaRule(string matSn, string matItmId)
        {
            List<EntityDicQcMateriaRule> listQcMatRule = new List<EntityDicQcMateriaRule>();

            string sqlWhere = string.Empty;
            if (!string.IsNullOrEmpty(matSn))
            {
                sqlWhere = string.Format(@" where Rmr_Dmat_id='{0}' ", matSn);

                if (!string.IsNullOrEmpty(matItmId))
                {
                    sqlWhere += string.Format(@" and Rmr_id='{0}' ", matItmId);
                }
                try
                {
                    string strSql = string.Format("select * from dbo.Rel_qc_materia_rule {0} ", sqlWhere);

                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(strSql);

                    listQcMatRule = EntityManager<EntityDicQcMateriaRule>.ConvertToList(dt).OrderBy(i => i.MatSn).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("查询质控规则关联数据SearchQcMateriaRule", ex);
                }
            }

            return listQcMatRule;
        }

    }
}
