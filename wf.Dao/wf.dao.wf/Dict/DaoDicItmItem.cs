
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
    [Export("wf.plugin.wf", typeof(IDaoDicItmItem))]
    public class DaoDicItmItem : IDaoDicItmItem
    {
        public bool Save(EntityDicItmItem ite)
        {
            try
            {
                DBManager helper = new DBManager();
                int id = IdentityHelper.GetMedIdentity("Dict_itm");
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ditm_id", id);
                values.Add("Ditm_name", ite.ItmName);
                values.Add("Ditm_pri_id", ite.ItmPriId);
                values.Add("Ditm_ecode", ite.ItmEcode);
                values.Add("Ditm_start_date", ite.ItmStartDate);
                values.Add("Ditm_end_date", ite.ItmEndDate);
                values.Add("Ditm_charge_flag", ite.ItmChargeFlag);
                values.Add("Ditm_qc_flag", ite.ItmQcFlag);
                values.Add("Ditm_prt_flag", ite.ItmPrtFlag);
                values.Add("Ditm_micr_flag", ite.ItmMicrFlag);
                values.Add("Ditm_calu_flag", ite.ItmCaluFlag);
                values.Add("Ditm_infection_flag", ite.ItmInfectionFlag);
                values.Add("Ditm_micr_type", ite.ItmMicrType);
                values.Add("py_code", ite.ItmPyCode);
                values.Add("wb_code", ite.ItmWbCode);
                values.Add("del_flag", ite.ItmDelFlag);
                values.Add("sort_no", ite.ItmSortNo);
                values.Add("Ditm_rep_code", ite.ItmRepCode);
                values.Add("Ditm_match_sex", ite.ItmMatchSex);
                values.Add("Ditm_print_side", ite.ItmPrintSide);
                values.Add("Ditm_price", ite.ItmPrice);
                values.Add("Ditm_cost", ite.ItmCost);
                values.Add("Ditm_method", ite.ItmMethod);
                values.Add("Ditm_his_code", ite.ItmHisCode);
                values.Add("Ditm_contrast_code", ite.ItmContrastCode);
                values.Add("Ditm_contrast_factor", ite.ItmContrastFactor);
                values.Add("Ditm_qc_type", ite.ItmQcType);
                values.Add("Ditm_content", ite.ItmContent);
                values.Add("Ditm_ref", ite.ItmRef);
                helper.InsertOperation("Dict_itm", values);
                ite.ItmId = id.ToString();
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItmItem ite)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Ditm_id", ite.ItmId);
                values.Add("Ditm_name", ite.ItmName);
                values.Add("Ditm_pri_id", ite.ItmPriId);
                values.Add("Ditm_ecode", ite.ItmEcode);
                if (ite.ItmStartDate != DateTime.MinValue)
                    values.Add("Ditm_start_date", ite.ItmStartDate);
                if (ite.ItmEndDate != DateTime.MinValue)
                    values.Add("Ditm_end_date", ite.ItmEndDate);
                values.Add("Ditm_charge_flag", ite.ItmChargeFlag);
                values.Add("Ditm_qc_flag", ite.ItmQcFlag);
                values.Add("Ditm_prt_flag", ite.ItmPrtFlag);
                values.Add("Ditm_micr_flag", ite.ItmMicrFlag);
                values.Add("Ditm_calu_flag", ite.ItmCaluFlag);
                values.Add("Ditm_infection_flag", ite.ItmInfectionFlag);
                values.Add("Ditm_micr_type", ite.ItmMicrType);
                values.Add("py_code", ite.ItmPyCode);
                values.Add("wb_code", ite.ItmWbCode);
                values.Add("del_flag", ite.ItmDelFlag);
                values.Add("sort_no", ite.ItmSortNo);
                values.Add("Ditm_rep_code", ite.ItmRepCode);
                values.Add("Ditm_match_sex", ite.ItmMatchSex);
                values.Add("Ditm_print_side", ite.ItmPrintSide);
                values.Add("Ditm_price", ite.ItmPrice);
                values.Add("Ditm_cost", ite.ItmCost);
                values.Add("Ditm_method", ite.ItmMethod);
                values.Add("Ditm_his_code", ite.ItmHisCode);
                values.Add("Ditm_contrast_code", ite.ItmContrastCode);
                values.Add("Ditm_contrast_factor", ite.ItmContrastFactor);
                values.Add("Ditm_qc_type", ite.ItmQcType);
                values.Add("Ditm_content", ite.ItmContent);
                values.Add("Ditm_ref", ite.ItmRef);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ditm_id", ite.ItmId);

                helper.UpdateOperation("Dict_itm", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItmItem ite)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Ditm_id", ite.ItmId);

                helper.UpdateOperation("Dict_itm", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItmItem> Search(string itmId, string hosId)
        {

            try
            {
                String sql = @"SELECT Dict_itm.*,Dict_profession.Dpro_name, 
Dict_itm_microscopy.Dimc_name,
(case when (select count(*) from Rel_itm_property where Rproty_Ditm_id=Dict_itm.Ditm_id) >0 then '有' else '无' end) propCount
FROM Dict_itm 
LEFT OUTER JOIN Dict_itm_microscopy ON Dict_itm.Ditm_micr_type = Dict_itm_microscopy.Dimc_id  
LEFT OUTER JOIN Dict_profession ON Dict_itm.Ditm_pri_id = Dict_profession.Dpro_id  where 1=1";

                if (!string.IsNullOrEmpty(itmId))
                {
                    sql += string.Format(@" and Dict_itm.Ditm_id='{0}' ", itmId);
                }
                if (!string.IsNullOrEmpty(hosId))
                {
                    sql += string.Format(@" and Dict_profession.Dpro_Dorg_id='{0}'", hosId);
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicItmItem>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItmItem>();
            }
        }

        public List<EntityDicItmItem> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicItmItem> list = new List<EntityDicItmItem>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicItmItem ite = new EntityDicItmItem();
                ite.ItmId = item["Ditm_id"].ToString();
                ite.ItmName = item["Ditm_name"].ToString();
                ite.ItmPriId = item["Ditm_pri_id"].ToString();
                decimal iteprice = 0.00m;
                decimal itecost = 0.00m;
                if (item["Ditm_price"] != DBNull.Value && item["Ditm_price"] != null)
                    decimal.TryParse(item["Ditm_price"].ToString(), out iteprice);
                ite.ItmPrice = iteprice;
                if (item["Ditm_cost"] != DBNull.Value)
                    decimal.TryParse(item["Ditm_cost"].ToString(), out itecost);
                ite.ItmCost = itecost;
                ite.ItmEcode = item["Ditm_ecode"].ToString();
                if (item["Ditm_start_date"] != DBNull.Value)
                    ite.ItmStartDate = Convert.ToDateTime(item["Ditm_start_date"].ToString());
                if (item["Ditm_start_date"] != DBNull.Value)
                    ite.ItmEndDate = Convert.ToDateTime(item["Ditm_end_date"].ToString());
                ite.ItmMicrType = item["Ditm_micr_type"].ToString();
                ite.ItmPyCode = item["py_code"].ToString();
                ite.ItmWbCode = item["wb_code"].ToString();
                ite.ItmDelFlag = item["del_flag"].ToString();
                ite.ItmRepCode = item["Ditm_rep_code"].ToString();
                ite.ItmMatchSex = item["Ditm_match_sex"].ToString();
                ite.ItmMethod = item["Ditm_method"].ToString();
                //ite.ItmHisCode = item["itm_his_code"].ToString();
                ite.ItmContrastCode = item["Ditm_contrast_code"].ToString();
                ite.ItmContrastFactor = item["Ditm_contrast_factor"].ToString();
                int chargeflag = 0, prtflag = 0, ugrflag = 0, calflag = 0, qcflag = 0, illflag = 0, sort = 0, repcol = 0, qctype = 0;
                if (item["Ditm_charge_flag"] != null && item["Ditm_charge_flag"] != DBNull.Value)
                    int.TryParse(item["Ditm_charge_flag"].ToString(), out chargeflag);
                ite.ItmChargeFlag = chargeflag;
                if (item["Ditm_qc_flag"] != null && item["Ditm_qc_flag"] != DBNull.Value)
                    int.TryParse(item["Ditm_qc_flag"].ToString(), out qcflag);
                ite.ItmQcFlag = qcflag;
                if (item["Ditm_prt_flag"] != null && item["Ditm_prt_flag"] != DBNull.Value)
                    int.TryParse(item["Ditm_prt_flag"].ToString(), out prtflag);
                ite.ItmPrtFlag = prtflag;
                if (item["Ditm_micr_flag"] != null && item["Ditm_micr_flag"] != DBNull.Value)
                    int.TryParse(item["Ditm_micr_flag"].ToString(), out ugrflag);
                ite.ItmMicrFlag = ugrflag;
                if (item["Ditm_calu_flag"] != null && item["Ditm_calu_flag"] != DBNull.Value)
                    int.TryParse(item["Ditm_calu_flag"].ToString(), out calflag);
                ite.ItmCaluFlag = calflag;
                if (item["Ditm_infection_flag"] != null && item["Ditm_infection_flag"] != DBNull.Value)
                    int.TryParse(item["Ditm_infection_flag"].ToString(), out illflag);
                ite.ItmInfectionFlag = illflag;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                ite.ItmSortNo = sort;
                if (item["Ditm_print_side"] != null && item["Ditm_print_side"] != DBNull.Value)
                    int.TryParse(item["Ditm_print_side"].ToString(), out repcol);
                ite.ItmPrintSide = repcol;
                if (item["Ditm_qc_type"] != null && item["Ditm_qc_type"] != DBNull.Value)
                    int.TryParse(item["Ditm_qc_type"].ToString(), out qctype);

                ite.ProName = item["Dpro_name"].ToString();
                ite.propCount = item["propCount"].ToString();
                ite.ItmContent = item["Ditm_content"].ToString();
                ite.ItmRef = item["Ditm_ref"].ToString();
                ite.ComId = item["Dcom_id"].ToString();
                ite.ComName = item["Dcom_name"].ToString();
                ite.ItmQcType = qctype;
                list.Add(ite);
            }
            return list.OrderBy(i => i.ItmSortNo).ToList();
        }


        /// <summary>
        /// 加载参考值
        /// </summary>
        /// <param name="itemsID"></param>
        /// <param name="sam_id"></param>
        /// <param name="age_minutes"></param>
        /// <param name="sex"></param>
        /// <param name="sam_rem"></param>
        /// <param name="itm_itr_id"></param>
        /// <param name="pat_depcode"></param>
        /// <returns></returns>
        public List<EntityItmRefInfo> GetItemRefInfo(List<string> itemsID, string sam_id, int age_minutes, string sex, string sam_rem, string itm_itr_id, string pat_depcode)
        {
            List<EntityItmRefInfo> listItemRefInfo = new List<EntityItmRefInfo>();
            try
            {
                string strSex = "('{0}','{1}')";
                if (sex != null && sex.Trim() == "2")
                    strSex = string.Format(strSex, 0, 2);
                else
                    strSex = string.Format(strSex, 0, 1);

                if (sam_rem == null)
                    sam_rem = "";

                StringBuilder sb = new StringBuilder();
                if (itemsID.Count() > 0)
                {
                    foreach (string item in itemsID)
                    {
                        sb.Append(string.Format(",'{0}'", item));
                    }
                    sb.Remove(0, 1);
                }
                else
                {
                    sb.Append(string.Format("'{0}'", "-1"));
                }

                string sqlSelect = string.Format(@"select 
Dict_itm.Ditm_id,
Dict_itm.Ditm_ecode,
Dict_itm.Ditm_name,
Dict_itm.Ditm_pri_id,
Dict_itm.sort_no,
isnull(Dict_itm.Ditm_calu_flag,0) as Ditm_calu_flag,
Dict_itm.Ditm_rep_code,
Dict_itm.Ditm_method,

Rel_itm_sample.Ritm_sam_id,
Rel_itm_sample.Ritm_unit,
Rel_itm_sample.Ritm_price,
Rel_itm_sample.Ritm_res_type,
Rel_itm_sample.Ritm_max_digit,
Rel_itm_sample.Ritm_default,
Rel_itm_sample.Ritm_positive_res,
Rel_itm_sample.Ritm_urgent_res,


Rel_itm_refdetail.Rref_age_lower_limit,
Rel_itm_refdetail.Rref_age_upper_limit,
Rel_itm_refdetail.Rref_sex,
Rel_itm_refdetail.Rref_lower_limit_value,
Rel_itm_refdetail.Rref_upper_limit_value,
Rel_itm_refdetail.Rref_danger_lower_limit,
Rel_itm_refdetail.Rref_danger_upper_limit,
Rel_itm_refdetail.Rref_min_value,
Rel_itm_refdetail.Rref_max_value,
Rel_itm_refdetail.Rref_result_allow,
Rel_itm_refdetail.Rref_flag

--itm_ref_l_cal = Def_itm_refdetail.lower_limit_value,
--itm_ref_h_cal = Def_itm_refdetail.upper_limit_value,
--itm_pan_l_cal = Def_itm_refdetail.danger_lower_limit,
--itm_pan_h_cal = Def_itm_refdetail.danger_upper_limit,
--itm_min_cal = Def_itm_refdetail.min_value,
--itm_max_cal = Def_itm_refdetail.max_value,

from Dict_itm
left join Rel_itm_sample on (Dict_itm.Ditm_id = Rel_itm_sample.Ritm_id and (Rel_itm_sample.Ritm_sam_id = '{1}' or Rel_itm_sample.Ritm_sam_id = '-1')
and Rel_itm_sample.Ritm_Ditr_id=(select (case when count(Ritm_Ditr_id)>=1 then '{5}' else '-1' end) from Rel_itm_sample
where  Ritm_id=Dict_itm.Ditm_id and Ritm_sam_id in('{1}','-1')
and Ritm_Ditr_id='{5}'))
left join Rel_itm_refdetail on (Dict_itm.Ditm_id = Rel_itm_refdetail.Rref_Ditm_id and Rref_id=
(select top 1 Rref_id from Rel_itm_refdetail where Dict_itm.Ditm_id = Rel_itm_refdetail.Rref_Ditm_id and 
Rel_itm_refdetail.Rref_Dsam_id in('{1}','-1') and (Rel_itm_refdetail.Rref_sex in {2}) 
and {3}>isnull(Rref_age_lower_minute,0) and  {3} <= Rref_age_upper_minute
and (Rref_flag='0' OR rref_ditm_id = '2082') --PCT分期参考值需要报危急值
and (Rref_sam_remark = '' or Rref_sam_remark is null or Rref_sam_remark=1)
and (Rref_Ditr_id=(select (case when count(Ritm_Ditr_id)>=1 then '{5}' else '-1' end) from Rel_itm_sample
where  Ritm_id=Dict_itm.Ditm_id and Ritm_sam_id in('{1}','-1')
and Ritm_Ditr_id='{5}')) ))--  
where
Dict_itm.del_flag <> '1'
and Dict_itm.Ditm_id in ({0})", sb.ToString(), sam_id, strSex, age_minutes, sam_rem, itm_itr_id);

                DBManager helper = new DBManager();
                //Lib.LogManager.Logger.LogInfo("GetItemRefInfo" + sqlSelect);
                DataTable dt = helper.ExecuteDtSql(sqlSelect);
                if (dt != null && dt.Rows.Count > 0)
                {
                    listItemRefInfo = EntityManager<EntityItmRefInfo>.ConvertToList(dt).OrderBy(i => i.ItmSamId).ToList();
                    foreach (EntityItmRefInfo refInfo in listItemRefInfo)
                    {
                        refInfo.ItmDangerLowerLimitCal = refInfo.ItmDangerLowerLimit;
                        refInfo.ItmDangerUpperLimitCal = refInfo.ItmDangerUpperLimit;
                        refInfo.ItmLowerLimitValueCal = refInfo.ItmLowerLimitValue;
                        refInfo.ItmMaxValueCal = refInfo.ItmMaxValue;
                        refInfo.ItmMinValueCal = refInfo.ItmMinValue;
                        refInfo.ItmUpperLimitValueCal = refInfo.ItmUpperLimitValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listItemRefInfo;
        }

        public List<EntityDicItmItem> GetLisSubItemsByComId(string comId)
        {
            List<EntityDicItmItem> list = new List<EntityDicItmItem>();
            try
            {
                if (!string.IsNullOrEmpty(comId))
                {
                    string sql = @"select a.*,b.Rici_Dcom_id,c.Dcom_name 
from Dict_itm a 
inner join  Rel_itm_combine_item b on a.Ditm_id = b.Rici_Ditm_id 
inner join  Dict_itm_combine c on  b.Rici_Dcom_id =c.Dcom_id
where a.del_flag<>'1' and b.Rici_Dcom_id='" + comId + "'";

                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    list = EntityManager<EntityDicItmItem>.ConvertToList(dt);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public List<EntityDicItmItem> GetLisSubItems()
        {
            List<EntityDicItmItem> list = new List<EntityDicItmItem>();
            try
            {
                string sql = @"select a.Ditm_id,a.Ditm_name,b.Rici_Dcom_id 
from (select Ditm_id,Ditm_name from Dict_itm where del_flag=0) a 
left join  (select Rici_Dcom_id,Rici_Ditm_id from Rel_itm_combine_item) b on
a.Ditm_id = b.Rici_Ditm_id";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);

                list = EntityManager<EntityDicItmItem>.ConvertToList(dt);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
    }
}
