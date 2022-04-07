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
    [Export("wf.plugin.wf", typeof(IDaoDicPubEvaluate))]
    public class DaoDicPubEvaluate : IDaoDicPubEvaluate
    {
        public bool Save(EntityDicPubEvaluate bsc)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_evaluate");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Deva_id", id);
                values.Add("Deva_c_code", bsc.EvaCcode);
                values.Add("Deva_content", bsc.EvaContent);
                values.Add("Deva_Dpro_id", bsc.EvaProid);
                values.Add("Deva_flag", bsc.EvaFlag);
                values.Add("sort_no", bsc.EvaSortNo);
                values.Add("Deva_Buser_id", bsc.EvaUserId);
                values.Add("Deva_Ditr_id", bsc.EvaItrId);
                values.Add("Deva_title", bsc.EvaTitle);
                values.Add("Deva_Dsam_id", bsc.EvaSamId);
                helper.InsertOperation("Dict_evaluate", values);
                bsc.EvaId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicPubEvaluate bsc)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Deva_id", bsc.EvaId);
                values.Add("Deva_c_code", bsc.EvaCcode);
                values.Add("Deva_content", bsc.EvaContent);
                values.Add("Deva_Dpro_id", bsc.EvaProid);
                values.Add("Deva_flag", bsc.EvaFlag);
                values.Add("sort_no", bsc.EvaSortNo);
                values.Add("Deva_Buser_id", bsc.EvaUserId);
                values.Add("Deva_Ditr_id", bsc.EvaItrId);
                values.Add("Deva_title", bsc.EvaTitle);
                values.Add("Deva_Dsam_id", bsc.EvaSamId);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Deva_id", bsc.EvaId);
                helper.UpdateOperation("Dict_evaluate", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicPubEvaluate refe)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Deva_id", refe.EvaId);

                helper.DeleteOperation("Dict_evaluate", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicPubEvaluate> Search(Object obj)
        {

            try
            {
                String sql = @"SELECT Dict_evaluate.*,eva_flag_name ='',Dict_sample.Dsam_name 
FROM Dict_evaluate
left join Dict_sample on Dict_evaluate.Deva_Dsam_id=Dict_sample.Dsam_id";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubEvaluate>();
            }
        }

        public List<EntityDicPubEvaluate> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicPubEvaluate> list = new List<EntityDicPubEvaluate>();
            foreach (DataRow item in dtSour.Rows)
            {

                EntityDicPubEvaluate bsc = new EntityDicPubEvaluate();
                bsc.EvaId = item["Deva_id"].ToString();
                bsc.EvaCcode = item["Deva_c_code"].ToString();
                bsc.EvaContent = item["Deva_content"].ToString();
                bsc.EvaProid = item["Deva_Dpro_id"].ToString();
                bsc.EvaFlag = item["Deva_flag"].ToString();
                bsc.EvaUserId = item["Deva_Buser_id"].ToString();
                bsc.EvaItrId = item["Deva_Ditr_id"].ToString();
                bsc.EvaTitle = item["Deva_title"].ToString();
                bsc.EvaSamId = item["Deva_Dsam_id"].ToString();
                bsc.EvaSamName = item["Dsam_name"].ToString();
                int sort = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                bsc.EvaSortNo = sort;
                list.Add(bsc);
            }
            return list.OrderBy(i => i.EvaSortNo).ToList();
        }

        public List<EntityDicPubEvaluate> SearchContent()
        {
            List<EntityDicPubEvaluate> list = new List<EntityDicPubEvaluate>();
            try
            {
                String sql = @"select Deva_id,isnull(Deva_content,'') as Deva_content from Dict_evaluate ";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                list = ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
    }
}
