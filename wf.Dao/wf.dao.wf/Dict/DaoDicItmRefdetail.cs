
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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItmRefdetail>))]
    public class DaoDicItmRefdetail : IDaoDic<EntityDicItmRefdetail>
    {
        public bool Save(EntityDicItmRefdetail det)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Rref_Ditm_id", det.ItmId);
                values.Add("Rref_Dsam_id", det.ItmSamId);
                values.Add("Rref_sex", det.ItmSex);
                values.Add("Rref_age_lower_limit", det.ItmAgeLowerLimit);
                values.Add("Rref_age_upper_limit", det.ItmAgeUpperLimit);
                values.Add("Rref_age_lower_limit_unit", det.ItmAgeLowerLimitUnit);
                values.Add("Rref_age_upper_limit_unit", det.ItmAgeUpperLimitUnit);
                values.Add("Rref_age_lower_minute", det.ItmAgeLowerMinute);
                values.Add("Rref_age_upper_minute", det.ItmAgeUpperMinute);
                values.Add("Rref_lower_limit_value", det.ItmLowerLimitValue);
                values.Add("Rref_upper_limit_value", det.ItmUpperLimitValue);
                values.Add("Rref_name", det.ItmRefName);
                values.Add("Rref_max_value", det.ItmMaxValue);
                values.Add("Rref_min_value", det.ItmMinValue);
                values.Add("Rref_danger_lower_limit", det.ItmDangerLowerLimit);
                values.Add("Rref_danger_upper_limit", det.ItmDangerUpperLimit);
                values.Add("Rref_result_diff", det.ItmResultDiff);
                values.Add("Rref_sam_remark", det.ItmSamRemark);
                values.Add("del_flag", det.ItmDelFlag);
                values.Add("Rref_result_allow", det.ItmResultAllow);
                values.Add("Rref_Ditr_id", det.ItmItrId);
                values.Add("Rref_line_lower_limit", det.ItmLineLowerLimit);
                values.Add("Rref_line_upper_limit", det.ItmLineUpperLimit);
                values.Add("Rref_report_lower_limit", det.ItmReportLowerLimit);
                values.Add("Rref_report_upper_limit", det.ItmReportUpperLimit);
                values.Add("Rref_danger_low_mean", det.DangerLowerMean);
                values.Add("Rref_danger_up_mean", det.DangerUpperMean);
                values.Add("Rref_dec_level1", det.DecisiveLeve1);
                values.Add("Rref_dec_level2", det.DecisiveLeve2);
                values.Add("Rref_dec_level3", det.DecisiveLeve3);
                values.Add("Rref_dec_level4", det.DecisiveLeve4);
                values.Add("Rref_level1_mean", det.DecisiveLeve1Mean);
                values.Add("Rref_level2_mean", det.DecisiveLeve2Mean);
                values.Add("Rref_level3_mean", det.DecisiveLeve3Mean);
                values.Add("Rref_level4_mean", det.DecisiveLeve4Mean);
                values.Add("Rref_flag", det.ItmRefFlag);
                helper.InsertOperation("Rel_itm_refdetail", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItmRefdetail det)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rref_Ditm_id", det.ItmId);
                values.Add("Rref_Dsam_id", det.ItmSamId);
                values.Add("Rref_sex", det.ItmSex);
                values.Add("Rref_age_lower_limit", det.ItmAgeLowerLimit);
                values.Add("Rref_age_upper_limit", det.ItmAgeUpperLimit);
                values.Add("Rref_age_lower_limit_unit", det.ItmAgeLowerLimitUnit);
                values.Add("Rref_age_upper_limit_unit", det.ItmAgeUpperLimitUnit);
                values.Add("Rref_age_lower_minute", det.ItmAgeLowerMinute);
                values.Add("Rref_age_upper_minute", det.ItmAgeUpperMinute);
                values.Add("Rref_lower_limit_value", det.ItmLowerLimitValue);
                values.Add("Rref_upper_limit_value", det.ItmUpperLimitValue);
                values.Add("Rref_name", det.ItmRefName);
                values.Add("Rref_max_value", det.ItmMaxValue);
                values.Add("Rref_min_value", det.ItmMinValue);
                values.Add("Rref_danger_lower_limit", det.ItmDangerLowerLimit);
                values.Add("Rref_danger_upper_limit", det.ItmDangerUpperLimit);
                values.Add("Rref_result_diff", det.ItmResultDiff);
                values.Add("Rref_sam_remark", det.ItmSamRemark);
                values.Add("del_flag", det.ItmDelFlag);
                values.Add("Rref_result_allow", det.ItmResultAllow);
                values.Add("Rref_Ditr_id", det.ItmItrId);
                values.Add("Rref_line_lower_limit", det.ItmLineLowerLimit);
                values.Add("Rref_line_upper_limit", det.ItmLineUpperLimit);
                values.Add("Rref_report_lower_limit", det.ItmReportLowerLimit);
                values.Add("Rref_report_upper_limit", det.ItmReportUpperLimit);
                values.Add("Rref_flag", det.ItmRefFlag);
                values.Add("Rref_danger_low_mean", det.DangerLowerMean);
                values.Add("Rref_danger_up_mean", det.DangerUpperMean);
                values.Add("Rref_dec_level1", det.DecisiveLeve1);
                values.Add("Rref_dec_level2", det.DecisiveLeve2);
                values.Add("Rref_dec_level3", det.DecisiveLeve3);
                values.Add("Rref_dec_level4", det.DecisiveLeve4);
                values.Add("Rref_level1_mean", det.DecisiveLeve1Mean);
                values.Add("Rref_level2_mean", det.DecisiveLeve2Mean);
                values.Add("Rref_level3_mean", det.DecisiveLeve3Mean);
                values.Add("Rref_level4_mean", det.DecisiveLeve4Mean);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rref_Ditm_id", det.ItmId);

                helper.UpdateOperation("Rel_itm_refdetail", values, keys);
              
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItmRefdetail det)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rref_Ditm_id", det.ItmId);

                helper.DeleteOperation("Rel_itm_refdetail", keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItmRefdetail> Search(Object obj)
        {
            List<EntityDicItmRefdetail> list = new List<EntityDicItmRefdetail>();
            try
            {
                if (obj != null)
                {
                    String sql = string.Format(@"SELECT Rref_id,Rref_Ditm_id, Rref_Dsam_id, Rref_sex,cast(Rref_age_lower_limit as varchar(30)) Rref_age_lower_limit,
cast(Rref_age_upper_limit as varchar(30)) Rref_age_upper_limit, Rref_age_lower_limit_unit, Rref_age_upper_limit_unit, 
Rref_age_lower_minute, Rref_age_upper_minute, Rref_lower_limit_value, Rref_upper_limit_value, Rref_name, Rref_max_value, Rref_min_value,
Rref_danger_lower_limit, Rref_danger_upper_limit, Rref_flag, Rref_result_diff,Rref_sam_remark,del_flag,Rref_result_allow,
Rref_Ditr_id ,-1 itm_sort,Rref_line_lower_limit,Rref_line_upper_limit,Rref_report_lower_limit,Rref_report_upper_limit,Rref_danger_low_mean
,Rref_danger_up_mean,Rref_dec_level1,Rref_dec_level2,Rref_dec_level3,Rref_dec_level4,Rref_level1_mean,Rref_level2_mean,Rref_level3_mean,Rref_level4_mean
FROM Rel_itm_refdetail WHERE (Rref_Ditm_id = '{0}' )", obj);
                    if (obj != null && obj.ToString() == "cache")
                    {
                        sql = "select * from Rel_itm_refdetail";
                    }
                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);
                    list = EntityManager<EntityDicItmRefdetail>.ConvertToList(dt).OrderBy(i => i.ItmSort).ToList();
                }
           
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public List<EntityDicItmRefdetail> ConvertToEntitys(DataTable dtSour)
        {
            throw new NotImplementedException();
        }

    }
}
