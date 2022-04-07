
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.common;
using System.ComponentModel.Composition;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItemSample>))]
    public class DaoDicItemSample : IDaoDic<EntityDicItemSample>
    {
        public bool Save(EntityDicItemSample sam)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ritm_id", sam.ItmId);
                values.Add("Ritm_sam_id", sam.ItmSamId);
                values.Add("Ritm_price", sam.ItmPrice);
                values.Add("Ritm_cost", sam.ItmCost);
                values.Add("Ritm_method", sam.ItmMethod);
                values.Add("Ritm_unit", sam.ItmUnit);
                values.Add("Ritm_default", sam.ItmDefault);
                values.Add("Ritm_res_type", sam.ItmResType);
                values.Add("Ritm_valid", sam.ItmValid);
                values.Add("Ritm_meaning", sam.ItmMeaning);
                values.Add("Ritm_lower_tips", sam.ItmLowerTips);
                values.Add("Ritm_upper_tips", sam.ItmUpperTips);
                values.Add("Ritm_use_method", sam.ItmUseMethod);
                values.Add("Ritm_accept_flag", sam.ItmAcceptFlag);
                values.Add("Ritm_max_digit", sam.ItmMaxDigit);
                values.Add("Ritm_compare_res", sam.ItmCompareRes);
                values.Add("del_flag", sam.ItmDelFlag);
                values.Add("Ritm_diff", sam.ItmDiff);
                values.Add("Ritm_valid_days", sam.ItmValidDays);
                values.Add("Ritm_positive_res", sam.ItmPositiveRes);
                values.Add("Ritm_urgent_res", sam.ItmUrgentRes);
                values.Add("Ritm_Ditr_id", sam.ItmItrId);
                values.Add("Ritm_danger_result", sam.DangerResult);
                values.Add("Ritm_result_influence", sam.ResultInfluence);
                values.Add("Ritm_decisive_result", sam.DecisiveResult);
                values.Add("Ritm_decisive_result_influence", sam.DecisiveResultInfluence);
                helper.InsertOperation("Rel_itm_sample", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItemSample sam)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ritm_id", sam.ItmId);
                values.Add("Ritm_sam_id", sam.ItmSamId);
                values.Add("Ritm_price", sam.ItmPrice);
                values.Add("Ritm_cost", sam.ItmCost);
                values.Add("Ritm_method", sam.ItmMethod);
                values.Add("Ritm_unit", sam.ItmUnit);
                values.Add("Ritm_default", sam.ItmDefault);
                values.Add("Ritm_res_type", sam.ItmResType);
                values.Add("Ritm_valid", sam.ItmValid);
                values.Add("Ritm_meaning", sam.ItmMeaning);
                values.Add("Ritm_lower_tips", sam.ItmLowerTips);
                values.Add("Ritm_upper_tips", sam.ItmUpperTips);
                values.Add("Ritm_use_method", sam.ItmUseMethod);
                values.Add("Ritm_accept_flag", sam.ItmAcceptFlag);
                values.Add("Ritm_max_digit", sam.ItmMaxDigit);
                values.Add("Ritm_compare_res", sam.ItmCompareRes);
                values.Add("del_flag", sam.ItmDelFlag);
                values.Add("Ritm_diff", sam.ItmDiff);
                values.Add("Ritm_valid_days", sam.ItmValidDays);
                values.Add("Ritm_positive_res", sam.ItmPositiveRes);
                values.Add("Ritm_urgent_res", sam.ItmUrgentRes);
                values.Add("Ritm_Ditr_id", sam.ItmItrId);
                values.Add("Ritm_danger_result", sam.DangerResult);
                values.Add("Ritm_result_influence", sam.ResultInfluence);
                values.Add("Ritm_decisive_result", sam.DecisiveResult);
                values.Add("Ritm_decisive_result_influence", sam.DecisiveResultInfluence);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ritm_id", sam.ItmId);

                helper.UpdateOperation("Rel_itm_sample", values, keys);
              
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItemSample sam)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ritm_id", sam.ItmId);

                helper.DeleteOperation("Rel_itm_sample",  keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItemSample> Search(Object obj)
        {

            try
            {
                String sql =string.Format(@"SELECT Rel_itm_sample.*,Dict_sample.Dsam_name,Dict_itr_instrument.Ditr_ename,
row_number() over(order by Rel_itm_sample.Ritm_sam_id) itm_sort
FROM Rel_itm_sample 
LEFT OUTER JOIN Dict_sample on Rel_itm_sample.Ritm_sam_id=Dict_sample.Dsam_id 
LEFT OUTER JOIN Dict_itr_instrument on Rel_itm_sample.Ritm_Ditr_id=Dict_itr_instrument.Ditr_id
WHERE (Ritm_id = '{0}' )", obj);
                if (obj != null && obj.ToString()=="cache")
                {
                    sql = string.Format("select Rel_itm_sample.*,Ritm_id,Ritm_sam_id  from Rel_itm_sample");
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDicItemSample> list = EntityManager<EntityDicItemSample>.ConvertToList(dt).OrderBy(i => i.ItmId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItemSample>();
            }
        }

        public List<EntityDicItemSample> ConvertToEntitys(DataTable dtSour)
        {
            throw new NotImplementedException();
        }

    }
}
