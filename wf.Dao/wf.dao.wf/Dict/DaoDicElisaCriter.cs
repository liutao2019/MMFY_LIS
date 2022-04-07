using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicElisaCriter>))]
    public class DaoDicElisaCriter : IDaoDic<EntityDicElisaCriter>
    {
        public bool Save(EntityDicElisaCriter judge)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_Elisa_criter");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Recri_id", id);
                values.Add("Recri_Ditm_id", judge.CriItmId);
                values.Add("Recri_judge", judge.CriJudge);
                values.Add("Recri_value", judge.CriValue);
                values.Add("Recri_result", judge.CriResult);
                values.Add("Recri_neg_lower_limit", judge.CriNegLowerLimit);
                values.Add("Recri_neg_upper_limit", judge.CriNegUpperLimit);
                values.Add("Recri_pos_lower_limit", judge.CriPosLowerlimit);
                values.Add("Recri_pos_upper_limit", judge.CriPosUpperlimit);
                values.Add("Recri_expression", judge.CriExpression);
                values.Add("Recri_wpos_lower_limit", judge.CriWposLowerLimit);
                values.Add("Recri_wpos_upper_limit", judge.CriWposUpperLimit);
                values.Add("Recri_ignore_null_sam", judge.CriIgnoreNullSam);

                helper.InsertOperation("Rel_Elisa_criter", values);

                judge.CriId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicElisaCriter judge)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Recri_id", judge.CriId);
                values.Add("Recri_Ditm_id", judge.CriItmId);
                values.Add("Recri_judge", judge.CriJudge);
                values.Add("Recri_value", judge.CriValue);
                values.Add("Recri_result", judge.CriResult);
                values.Add("Recri_neg_lower_limit", judge.CriNegLowerLimit);
                values.Add("Recri_neg_upper_limit", judge.CriNegUpperLimit);
                values.Add("Recri_pos_lower_limit", judge.CriPosLowerlimit);
                values.Add("Recri_pos_upper_limit", judge.CriPosUpperlimit);
                values.Add("Recri_expression", judge.CriExpression);
                values.Add("Recri_wpos_lower_limit", judge.CriWposLowerLimit);
                values.Add("Recri_wpos_upper_limit", judge.CriWposUpperLimit);
                values.Add("Recri_ignore_null_sam", judge.CriIgnoreNullSam);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Recri_id", judge.CriId);

                helper.UpdateOperation("Rel_Elisa_criter", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicElisaCriter judge)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Recri_id", judge.CriId);

                helper.UpdateOperation("Rel_Elisa_criter", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicElisaCriter> Search(Object obj)
        {
            try
            {
                String sql = @"select Recri_id, Recri_Ditm_id, Recri_judge, Recri_value, Recri_result, 
Recri_neg_lower_limit, Recri_neg_upper_limit, Recri_pos_lower_limit, 
Recri_pos_upper_limit, Recri_expression, Recri_wpos_lower_limit, Recri_wpos_upper_limit, 
Recri_ignore_null_sam, del_flag from Rel_Elisa_criter where del_flag = '0' or del_flag is null";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);



                List<EntityDicElisaCriter> list = EntityManager<EntityDicElisaCriter>.ConvertToList(dt).OrderBy(i => i.CriId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicElisaCriter>();
            }
        }
    }
}
