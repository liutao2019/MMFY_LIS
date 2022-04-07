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
    [Export("wf.plugin.wf", typeof(IDaoQcResult))]
    public class DaoObrQcResult : IDaoQcResult
    {
        public bool DeleteQcResult(List<string> listQresSn)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                foreach (string qresSn in listQresSn)
                {

                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("del_flag", "1");

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Lres_id", qresSn);

                    result = helper.UpdateOperation("Lis_qc_result", values, keys) > 0;
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public List<EntityDicQcConvert> GetQcConvert(string strItrId, string strItmId)
        {
            try
            {
                String sql = string.Format(@"select * from dbo.Rel_qc_convert where Rqcv_Ditr_id = '{0}' and Rqcv_Ditm_id='{1}'", strItrId, strItmId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDicQcConvert> list = EntityManager<EntityDicQcConvert>.ConvertToList(dt).OrderBy(i => i.SortNo).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicQcConvert>();
            }
        }

        public List<EntityObrQcResult> GetQcResult(EntityObrQcResultQC resultQc)
        {
            List<EntityObrQcResult> listResult = new List<EntityObrQcResult>();
            try
            {
                string sqlWhere = string.Empty;
                if (!resultQc.IsCheckGrubbs)
                    sqlWhere = string.Format("and Lres_date >='{0}' and Lres_date<='{1}'",
                        resultQc.StateTime.Value.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss"),
                        resultQc.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                string sql = string.Format(@"select
Lis_qc_result.*,
Dict_itm.Ditm_ecode,
Dict_itm.Ditm_name,
Rel_qc_convert.Rqcv_convert_value,
Base_User.Buser_name,
poweruserinfo2.Buser_name two_audit_name,
Rel_qc_materia_detail.Rmatdet_itm_x,
Rel_qc_materia_detail.Rmatdet_itm_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv,
Rel_qc_materia_detail.Rmatdet_max_value,
Rel_qc_materia_detail.Rmatdet_min_value,
Rel_qc_materia_detail.Rmatdet_value_type,
Dict_qc_materia.Dmat_level,
Dict_qc_materia.Dmat_batch_no,
Dict_qc_materia.Dmat_cname,
Dict_qc_materia.Dmat_ename,
Dict_qc_materia.Dmat_method,
Dict_qc_materia.Dmat_manufacturer,
Dict_qc_materia.Dmat_date_end,
Dict_qc_materia.Dmat_Buser_name,
Dict_qc_materia.Dmat_manufacturer 质控厂家,
Dict_qc_materia.Dmat_date_end 有效期
from Lis_qc_result with(nolock) LEFT OUTER JOIN Dict_itm ON Lis_qc_result.Lres_Ditm_id = Dict_itm.Ditm_id
left join Rel_qc_materia_detail on Lis_qc_result.Lres_Rmatdet_id=Rel_qc_materia_detail.Rmatdet_Dmat_id  and Lis_qc_result.Lres_Ditm_id=Rel_qc_materia_detail.Rmatdet_Ditm_id
left join Dict_qc_materia on Lis_qc_result.Lres_Rmatdet_id=Dict_qc_materia.Dmat_id
left join Rel_qc_convert on Lis_qc_result.Lres_Ditr_id =Rel_qc_convert.Rqcv_Ditr_id and Lis_qc_result.Lres_Ditm_id=Rel_qc_convert.Rqcv_Ditm_id
and Lis_qc_result.Lres_value=Rel_qc_convert.Rqcv_value
left join Base_User on Lis_qc_result.Lres_audit_Buser_id=Base_User.Buser_loginid 
left join Base_User poweruserinfo2 on Lis_qc_result.Lres_secondaudit_Buser_id=poweruserinfo2.Buser_loginid 
where Lres_Rmatdet_id='{0}' and Lres_Ditm_id='{1}' and  Lis_qc_result.del_flag='0' {2}",
                                            resultQc.QcParDetailId, resultQc.ItemId, sqlWhere);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                listResult = EntityManager<EntityObrQcResult>.ConvertToList(dt).OrderBy(w => w.QresDate).ToList();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return listResult;
        }

        public List<EntityQcTreeView> GetQcTreeView(string strItrId, DateTime dtSDate, DateTime dtEDate, QcTreeViewType viewType, bool radarView)
        {
            List<EntityQcTreeView> listView = new List<EntityQcTreeView>();

            try
            {
                string sql = string.Empty;
                string sql1 = string.Empty;

                if (viewType == QcTreeViewType.多水平)
                {
                    if (radarView)
                    {
                        sql = @"select 
Dmat_id Dpro_id,
Dmat_level+' - '+ Dmat_batch_no Dpro_name,
'-1' func_parentId,
cast(null as numeric(18,3)) type_c_x,
cast(null as numeric(18,3)) type_sd,
cast(null as numeric(18,3)) type_ccv,
cast(null as varchar(50)) Rmatdet_Dmat_id,
'0' type_type,
cast(null as datetime) Dmat_date_start,
cast(null as datetime) Dmat_date_end,
cast(null as numeric(18,3)) Rmatdet_itm_cv,
cast(null as numeric(18,3)) Rmatdet_allow_cv,
cast(null as numeric(18,3)) Rmatdet_max_value,
cast(null as numeric(18,3)) Rmatdet_min_value,
cast(null as varchar(50)) Rmatdet_value_type
from Dict_qc_materia
inner join Lis_qc_result on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id and del_flag='0'
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and del_flag='0'
group by Dmat_id,Dmat_level,Dmat_batch_no
union
select  
Lres_Rmatdet_id +'&'+Rel_itm_combine_item.Rici_Dcom_id Dpro_id,
Dcom_name Dpro_name,
Lres_Rmatdet_id func_parentId,
cast(null as  numeric(18,3)) type_c_x,
cast(null as numeric(18,3)) type_sd,
cast(null as numeric(18,3)) type_ccv,
cast(null as varchar(50)) Rmatdet_Dmat_id,
'2' type_type,
cast(null as datetime) Dmat_date_start,
cast(null as datetime) Dmat_date_end,
cast(null as numeric(18,3)) Rmatdet_itm_cv,
cast(null as numeric(18,3)) Rmatdet_allow_cv,
cast(null as numeric(18,3)) Rmatdet_max_value,
cast(null as numeric(18,3)) Rmatdet_min_value,
cast(null as varchar(50)) Rmatdet_value_type
from 
Lis_qc_result 
left join Rel_itm_combine_item on Rel_itm_combine_item.Rici_Ditm_id=Lis_qc_result.Lres_Ditm_id
inner join Dict_itm_combine on Dict_itm_combine.Dcom_id=Rel_itm_combine_item.Rici_Dcom_id and Dict_itm_combine.Dcom_qc_flag=1
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and Lis_qc_result.del_flag='0'
group by Lres_Rmatdet_id,Dcom_name,Rel_itm_combine_item.Rici_Dcom_id";

                        sql1 = @"select Rel_qc_materia_detail.Rmatdet_Dmat_id+'&'+Rmatdet_Ditm_id+'&'+Rel_itm_combine_item.Rici_Dcom_id Dpro_id,
Ditm_ecode Dpro_name,
Rel_qc_materia_detail.Rmatdet_Dmat_id+'&'+Rel_itm_combine_item.Rici_Dcom_id func_parentId,
Rel_qc_materia_detail.Rmatdet_itm_x type_c_x,
Rel_qc_materia_detail.Rmatdet_itm_sd type_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv type_ccv,
Rmatdet_Ditm_id Rmatdet_Dmat_id,
'1' type_type,
cast(null as datetime)  Dmat_date_start,
cast(null as datetime)  Dmat_date_end,
Rel_qc_materia_detail.Rmatdet_itm_cv,
Rel_qc_materia_detail.Rmatdet_allow_cv,
Rel_qc_materia_detail.Rmatdet_max_value,
Rel_qc_materia_detail.Rmatdet_min_value,
Rel_qc_materia_detail.Rmatdet_value_type,
Dict_itm_combine.sort_no,Rel_itm_combine_item.sort_no,
Dict_itm.sort_no
from Lis_qc_result  
inner join Rel_qc_materia_detail on Rel_qc_materia_detail.Rmatdet_Ditr_id=Lis_qc_result.Lres_Ditr_id and del_flag='0'
and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id and Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
LEFT OUTER JOIN Dict_qc_materia on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
left join Rel_itm_combine_item on Rel_itm_combine_item.Rici_Ditm_id=Lis_qc_result.Lres_Ditm_id
inner join Dict_itm_combine on Dict_itm_combine.Dcom_id=Rel_itm_combine_item.Rici_Dcom_id and Dict_itm_combine.Dcom_qc_flag=1
where Lis_qc_result.del_flag=0 and Rel_qc_materia_detail.Rmatdet_Dmat_id is not null and Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by 
Rel_qc_materia_detail.Rmatdet_Dmat_id,Rmatdet_Ditm_id,Ditm_ecode,Rel_qc_materia_detail.Rmatdet_Dmat_id,Rmatdet_itm_x,Rmatdet_itm_sd,Rmatdet_itm_ccv,Rmatdet_itm_cv,Rmatdet_allow_cv,Rel_qc_materia_detail.Rmatdet_max_value,Rel_qc_materia_detail.Rmatdet_min_value,Rel_qc_materia_detail.Rmatdet_value_type,Rel_itm_combine_item.Rici_Dcom_id,Dict_itm_combine.sort_no,Rel_itm_combine_item.sort_no,Dict_itm.sort_no
order by Dict_itm_combine.sort_no asc,Rel_itm_combine_item.sort_no asc,Dict_itm.sort_no asc ";
                    }
                    else
                    {
                        sql = @"select 
Dict_qc_materia.Dmat_id+'&'+Rmatdet_Ditm_id Dpro_id,
Dmat_level+' - '+Dmat_batch_no Dpro_name,
Rmatdet_Ditm_id func_parentId,
isnull(Rel_qc_materia_detail.Rmatdet_itm_x,0) type_c_x,
isnull(Rel_qc_materia_detail.Rmatdet_itm_sd,1) type_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv type_ccv,
Rmatdet_Ditm_id Rmatdet_Dmat_id,
'1' type_type,
null Dmat_date_start,
null Dmat_date_end,
Rel_qc_materia_detail.Rmatdet_itm_cv,
Rel_qc_materia_detail.Rmatdet_allow_cv,
Rel_qc_materia_detail.Rmatdet_max_value,
Rel_qc_materia_detail.Rmatdet_min_value,
Rel_qc_materia_detail.Rmatdet_value_type,
Dict_qc_materia.Dmat_date_start as qc_par_detail_sdate,
Rel_qc_materia_detail.Rmatdet_reag_manufacturer,
Rel_qc_materia_detail.Rmatdet_m_pro,
Rel_qc_materia_detail.Rmatdet_read_valid_date,
999 seq
from Lis_qc_result 
inner join Dict_qc_materia on  Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
inner join Rel_qc_materia_detail on Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and Lis_qc_result.del_flag='0'
group by Dmat_id,Dmat_level,Dmat_batch_no,Rel_qc_materia_detail.Rmatdet_Dmat_id,Rmatdet_itm_x,Rmatdet_itm_sd,Rmatdet_itm_ccv,Rmatdet_Ditm_id,Rmatdet_itm_cv,Rmatdet_allow_cv,Rel_qc_materia_detail.Rmatdet_max_value,Rel_qc_materia_detail.Rmatdet_min_value,Rel_qc_materia_detail.Rmatdet_value_type,Dict_qc_materia.Dmat_date_start ,Rel_qc_materia_detail.Rmatdet_reag_manufacturer,Rel_qc_materia_detail.Rmatdet_m_pro,Rel_qc_materia_detail.Rmatdet_read_valid_date
union
select Rmatdet_Ditm_id pro_id,Ditm_ecode pro_name,'-1' func_parentId,null type_c_x,null type_sd,null type_ccv,Rmatdet_Ditm_id mat_id,'0' type_type,null mat_date_start,null mat_date_end,null mat_itm_cv,null mat_allow_cv
,null mat_max_value,null mat_min_value,null mat_value_type,null qc_par_detail_sdate,'' mat_reag_manufacturer,'' mat_m_pro,null as mat_read_valid_date,isnull(sort_no,999) seq
from Rel_qc_materia_detail 
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
inner join Lis_qc_result on Rel_qc_materia_detail.Rmatdet_Ditr_id=Lis_qc_result.Lres_Ditr_id and Lis_qc_result.del_flag='0'
and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id and Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id
where Dict_itm.del_flag=0 and Rmatdet_Dmat_id is not null and Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by Rmatdet_Ditm_id,Ditm_ecode,Dict_itm.sort_no ";
                    }

                }
                else if (viewType == QcTreeViewType.多项目)
                {
                    sql = @"select 
Dmat_id Dpro_id,
Dmat_level+' - '+Dmat_batch_no Dpro_name,
'-1' func_parentId,
null type_c_x,
null type_sd,
null type_ccv,
null Rmatdet_Dmat_id,
'0' type_type,
null Dmat_date_start,
null Dmat_date_end,
null Rmatdet_itm_cv,
null Rmatdet_allow_cv,
null Rmatdet_max_value,
null Rmatdet_min_value,
null Rmatdet_value_type,
Dict_qc_materia.Dmat_date_start AS qc_par_detail_sdate,
'' Rmatdet_reag_manufacturer,
'' Rmatdet_m_pro,
null as Rmatdet_read_valid_date,
999 seq
from Dict_qc_materia
inner join Lis_qc_result on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id and del_flag='0'
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by Dmat_id,Dmat_level,Dmat_batch_no,Dict_qc_materia.Dmat_date_start
union
select 
Rel_qc_materia_detail.Rmatdet_Dmat_id+'&'+Rmatdet_Ditm_id Dpro_id,
Ditm_ecode Dpro_name,
Rel_qc_materia_detail.Rmatdet_Dmat_id func_parentId,
Rel_qc_materia_detail.Rmatdet_itm_x type_c_x,
Rel_qc_materia_detail.Rmatdet_itm_sd type_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv type_ccv,
Rmatdet_Ditm_id Rmatdet_Dmat_id,
'1' type_type,
null Dmat_date_start,
null Dmat_date_end,
Rel_qc_materia_detail.Rmatdet_itm_cv,
Rel_qc_materia_detail.Rmatdet_allow_cv,
Rel_qc_materia_detail.Rmatdet_max_value,
Rel_qc_materia_detail.Rmatdet_min_value,
Rel_qc_materia_detail.Rmatdet_value_type,
Dict_qc_materia.Dmat_date_start AS qc_par_detail_sdate,
Rel_qc_materia_detail.Rmatdet_reag_manufacturer,
Rel_qc_materia_detail.Rmatdet_m_pro,
Rel_qc_materia_detail.Rmatdet_read_valid_date,
isnull(sort_no,999) seq
from Rel_qc_materia_detail 
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
inner join Lis_qc_result on Rel_qc_materia_detail.Rmatdet_Ditr_id=Lis_qc_result.Lres_Ditr_id and Lis_qc_result.del_flag='0'
and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id and Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id
LEFT OUTER JOIN Dict_qc_materia on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
where Dict_itm.del_flag=0 and Rel_qc_materia_detail.Rmatdet_Dmat_id is not null and Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by Rel_qc_materia_detail.Rmatdet_Dmat_id,Rmatdet_Ditm_id,Ditm_ecode,Rel_qc_materia_detail.Rmatdet_Dmat_id,Rel_qc_materia_detail.Rmatdet_Dmat_id,Rmatdet_itm_x,Rmatdet_itm_sd,Rmatdet_itm_ccv,Rmatdet_itm_cv,Rmatdet_allow_cv,Rel_qc_materia_detail.Rmatdet_max_value,Rel_qc_materia_detail.Rmatdet_min_value,Rel_qc_materia_detail.Rmatdet_value_type,Dict_qc_materia.Dmat_date_start,Rel_qc_materia_detail.Rmatdet_reag_manufacturer,Rel_qc_materia_detail.Rmatdet_m_pro,Rel_qc_materia_detail.Rmatdet_read_valid_date,sort_no";
                }

                string sqlMain = string.Empty;
                string sqlMain1 = string.Empty;
                if (!radarView)
                {
                    sqlMain = string.Format(sql, strItrId, dtSDate.ToString("yyyy-MM-dd HH:mm:ss"), dtEDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (!string.IsNullOrEmpty(sql1))
                    {
                        sqlMain1 = string.Format(sql1, strItrId, dtSDate.ToString("yyyy-MM-dd HH:mm:ss"), dtEDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                else
                {
                    DateTime dtDate = dtEDate.AddDays(-1).AddSeconds(1);
                    sqlMain = string.Format(sql, strItrId, dtSDate.ToString("yyyy-MM-dd HH:mm:ss"), dtEDate.ToString("yyyy-MM-dd HH:mm:ss"));
                }


                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sqlMain);
                //Lib.LogManager.Logger.LogInfo("sqlMain:" + sqlMain);


                if (!string.IsNullOrEmpty(sqlMain1))
                {
                    DataTable dt2 = helper.ExecuteDtSql(sqlMain1);
                    //Lib.LogManager.Logger.LogInfo("sqlMain1:" + sqlMain1);
                    if (dt2.Rows.Count > 0)
                    {
                        if (dt2.Columns.Contains("sort_no"))
                        {
                            dt2.Columns.Remove("sort_no");
                        }
                        dt.Merge(dt2);
                    }

                }

                listView = EntityManager<EntityQcTreeView>.ConvertToList(dt).OrderBy(i=>i.TvName).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }


            return listView;
        }

        public bool InsertQcResult(string ItrId, string ItmId, string QcValue, string NoType, string MatSn, DateTime QcDate)
        {
            try
            {
                DBManager helper = new DBManager();
                string sqlStr = string.Format(@"insert into Lis_qc_result (Lres_Ditr_id, Lres_Ditm_id, Lres_date, Lres_value, Lres_level, Lres_Rmatdet_id,del_flag)
				                                           values ('{0}', '{1}', '{5}', '{2}', '{3}', '{4}','0')",
                                                           ItrId, ItmId, QcValue,
                                                           NoType, MatSn,
                                                           QcDate.ToString("yyyy-MM-dd HH:mm:ss"));
                helper.ExecSql(sqlStr);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool QcResultAudit(List<EntityObrQcResult> listQcResult)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                foreach (EntityObrQcResult item in listQcResult)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();

                    values.Add("Lres_audit_Buser_id", item.QresAuditUserId);
                    values.Add("Lres_audit_flag", item.QresAuditFlag);
                    values.Add("Lres_runaway_rule", item.QresRunawayRule);
                    values.Add("Lres_itm_x", item.MatItmX);
                    values.Add("Lres_itm_sd", item.MatItmSd);
                    values.Add("Lres_runaway_flag", item.QresRunawayFlag);
                    values.Add("Lres_Ditm_id", item.QresItmId);
                    values.Add("Lres_Rmatdet_id", item.QresMatDetId);
                    values.Add("Lres_date", item.QresDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    values.Add("Lres_remark", item.QresRemark);
                    values.Add("Lres_convert_value", item.QresConvertValue);
                    values.Add("Lres_audit_date", item.QresAuditDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    values.Add("Lres_reasons", item.QresReasons);
                    values.Add("Lres_process", item.QresProcess);

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Lres_id", item.QresSn);

                    result = helper.UpdateOperation("Lis_qc_result", values, keys) > 0;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public List<EntityObrQcResult> QcResultQuery(EntityObrQcResultQC resultQc)
        {
            List<EntityObrQcResult> listResult = new List<EntityObrQcResult>();
            try
            {
                string strSql = "select 0 checked,* from Lis_qc_result where del_flag='0' ";

                string sqlWhere = string.Empty;

                if (resultQc.StateTime != null && resultQc.EndTime != null)
                    sqlWhere += string.Format(" and Lres_date >='{0}' and Lres_date<='{1}'", resultQc.StateTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        resultQc.EndTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));

                if (!string.IsNullOrEmpty(resultQc.QcParDetailId))
                    sqlWhere += string.Format(" and Lres_Rmatdet_id = '{0}'", resultQc.QcParDetailId);

                if (!string.IsNullOrEmpty(resultQc.ItemId))
                    sqlWhere += string.Format(" and Lres_Ditm_id = '{0}'", resultQc.ItemId);

                if (!string.IsNullOrEmpty(resultQc.ItrId))
                    sqlWhere += string.Format(" and Lres_Ditr_id = '{0}'", resultQc.ItrId);

                strSql = strSql + sqlWhere;

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSql);

                listResult = EntityManager<EntityObrQcResult>.ConvertToList(dt).OrderBy(w => w.QresDate).ToList();

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listResult;
        }

        public bool QcResultSecondAudit(List<EntityObrQcResult> listQcResult)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                foreach (EntityObrQcResult item in listQcResult)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();

                    values.Add("Lres_secondaudit_date", item.QresSecondauditDate?.ToString("yyyy-MM-dd HH:mm:ss"));
                    values.Add("Lres_secondaudit_Buser_id", item.QresSecondauditUserId);
                    values.Add("Lres_secondaudit_interval", item.QresSecondauditInterval);

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Lres_id", item.QresSn);

                    result = helper.UpdateOperation("Lis_qc_result", values, keys) > 0;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool QcResultUndoAudit(List<string> listQresSn)
        {
            bool result = false;

            try
            {
                string strWhere = string.Empty;

                foreach (String item in listQresSn)
                {
                    strWhere += string.Format(",'{0}'", item);
                }

                strWhere = strWhere.Remove(0, 1);

                string sql = string.Format("update Lis_qc_result set Lres_audit_flag='0',Lres_reasons=null,Lres_process=null,Lres_audit_Buser_id=null,Lres_runaway_rule=null,Lres_runaway_flag=0,Lres_secondaudit_date=null,Lres_audit_date=null,Lres_secondaudit_Buser_id=null where Lres_id in ({0})", strWhere);

                DBManager helper = new DBManager();

                result = helper.ExecCommand(sql) > 0;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public bool SaveQcResult(EntityObrQcResult qcResult)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = helper.ConverToDBSaveParameter<EntityObrQcResult>(qcResult);

                result = helper.InsertOperation("Lis_qc_result", values) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public bool UpdateQcResult(EntityObrQcResult qcResult)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Lres_value", qcResult.QresValue);
                values.Add("Lres_reasons", qcResult.QresReasons);
                values.Add("Lres_process", qcResult.QresProcess);
                values.Add("Lres_type", qcResult.QresType);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lres_id", qcResult.QresSn);

                result = helper.UpdateOperation("Lis_qc_result", values, keys) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UpdateQresDisplay(string strQresSn, int display)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Lres_display", display);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Lres_id", strQresSn);

                result = helper.UpdateOperation("Lis_qc_result", values, keys) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public List<EntityQcStatistic> GetAnalyseData(List<EntityObrQcResultQC> lisItem)
        {
            List<EntityQcStatistic> listAnayalyData = new List<EntityQcStatistic>();

            String Sql = @"select
Dict_itm.Ditm_ecode+'-'+Dict_qc_materia.Dmat_batch_no+'-'+Dict_qc_materia.Dmat_level ITEM ,
Rel_qc_materia_detail.Rmatdet_itm_x,
Rel_qc_materia_detail.Rmatdet_itm_sd,
Rel_qc_materia_detail.Rmatdet_itm_cv,
Rel_qc_materia_detail.Rmatdet_itm_unit,
Rel_qc_materia_detail.Rmatdet_allow_cv,
Lis_qc_result.Lres_Ditm_id,
Lis_qc_result.Lres_Rmatdet_id,
count(*) N,
sum(case isnull(Lis_qc_result.Lres_runaway_flag,0) when 1 then 0 when 2 then 1 else  0 end) 失控数,
Dict_itr_instrument.Ditr_name 仪器名称,
Lis_qc_result.Lres_Ditr_id 仪器,
(cast (YEAR(Lis_qc_result.Lres_date) AS VARCHAR)+ cast (DATENAME(mm,Lis_qc_result.Lres_date) AS VARCHAR)) [MONTH],
avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) [AVG],
isnull(Stdev(cast(Lis_qc_result.Lres_value as numeric(18,2))),0) SD,
(case avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) when 0 then 0 else isnull(Stdev(cast(Lis_qc_result.Lres_value as numeric(18,2))),0)/avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) end)*100 CV                            
from Lis_qc_result 
inner join Rel_qc_materia_detail on Lis_qc_result.Lres_Rmatdet_id=Rel_qc_materia_detail.Rmatdet_Dmat_id  and Lis_qc_result.Lres_Ditm_id=Rel_qc_materia_detail.Rmatdet_Ditm_id
inner join Dict_qc_materia on Lis_qc_result.Lres_Rmatdet_id=Dict_qc_materia.Dmat_id
LEFT OUTER JOIN Dict_itm ON Lis_qc_result.Lres_Ditm_id = Dict_itm.Ditm_id
LEFT OUTER JOIN Dict_itr_instrument ON Dict_itr_instrument.Ditr_id = Lis_qc_result.Lres_Ditr_id
where  dbo.isReallyNumeric(Lis_qc_result.Lres_value)=1 and Lis_qc_result.del_flag='0'
and Lis_qc_result.Lres_value not in ('+','-')  {0}
group by (cast (YEAR(Lis_qc_result.Lres_date) AS VARCHAR)+ cast (DATENAME(mm,Lis_qc_result.Lres_date) AS VARCHAR)),
Dict_qc_materia.Dmat_level,Dict_qc_materia.Dmat_batch_no,Dict_itm.Ditm_ecode,Rel_qc_materia_detail.Rmatdet_itm_x,Rel_qc_materia_detail.Rmatdet_itm_sd,Rel_qc_materia_detail.Rmatdet_itm_cv,Rel_qc_materia_detail.Rmatdet_itm_unit,Rel_qc_materia_detail.Rmatdet_allow_cv,
Dict_itr_instrument.Ditr_name,Lis_qc_result.Lres_Ditr_id,Lis_qc_result.Lres_Ditm_id,Lis_qc_result.Lres_Rmatdet_id ";


            StringBuilder strItemId = new StringBuilder();
            StringBuilder strDetailId = new StringBuilder();

            foreach (var qcItem in lisItem)
            {
                strItemId.Append(string.Format(",'{0}'", qcItem.ItemId));
                strDetailId.Append(string.Format(",'{0}'", qcItem.QcParDetailId));
            }

            String strWhere = string.Format("and Lis_qc_result.Lres_date>='{0}' and Lis_qc_result.Lres_date<='{1}'",
                                lisItem[0].StateTime.Value.Date.AddDays(-lisItem[0].StateTime.Value.Day + 1).ToString("yyyy-MM-dd HH:mm:ss"),
                                lisItem[0].EndTime.Value.Date.AddMonths(1).AddMilliseconds(-1).AddDays(-lisItem[0].EndTime.Value.Date.AddMonths(1).Day + 1).ToString("yyyy-MM-dd HH:mm:ss"));

            String strCollectWhere = String.Empty;

            if (strItemId.Length > 0)
            {
                String strItemWhere = String.Format(" and Lis_qc_result.Lres_Ditm_id in ({0})", strItemId.Remove(0, 1).ToString());
                strWhere += strItemWhere;
                strCollectWhere += strItemWhere;
            }
            if (strDetailId.Length > 0)
            {
                String strDetailWhere = String.Format(" and Lis_qc_result.Lres_Rmatdet_id in ({0})", strDetailId.Remove(0, 1).ToString());
                strWhere += strDetailWhere;
                strCollectWhere += strDetailWhere;
            }

            DBManager helper = new DBManager();

            string strSql = String.Format(Sql, strWhere);

            DataTable dtResult = helper.ExecuteDtSql(strSql);

            dtResult.Columns.Add("ActualAVG", typeof(double));
            dtResult.Columns.Add("ActualSD", typeof(double));
            dtResult.Columns.Add("ActualCV", typeof(double));
            dtResult.Columns.Add("ActualFlag", typeof(int));

            DataTable dtActualValue = helper.ExecuteDtSql(String.Format(Sql, strWhere + " and isnull(Lis_qc_result.Lres_runaway_flag,'0')<>'2' and Lis_qc_result.Lres_display ='0'  and (Lis_qc_result.Lres_runaway_rule is null or Lis_qc_result.Lres_runaway_rule='' or Lis_qc_result.Lres_runaway_flag='0') "));

            foreach (DataRow item in dtActualValue.Rows)
            {
                DataRow[] drResult = dtResult.Select("ITEM='" + item["ITEM"] + "' and MONTH='" + item["MONTH"] + "'");
                foreach (DataRow result in drResult)
                {
                    result["ActualAVG"] = item["AVG"];
                    result["ActualSD"] = item["SD"];
                    result["ActualCV"] = item["CV"];
                    if (result["Rmatdet_allow_cv"] != DBNull.Value && Convert.ToDouble(result["ActualCV"]) > Convert.ToDouble(result["Rmatdet_allow_cv"]))
                        result["ActualFlag"] = 1;
                    else
                        result["ActualFlag"] = 0;
                }
            }

            dtResult.Columns.Add("CollectAVG", typeof(double));
            dtResult.Columns.Add("CollectSD", typeof(double));
            dtResult.Columns.Add("CollectCV", typeof(double));
            dtResult.Columns.Add("CollectN", typeof(int));


            dtResult.Columns.Add("CollectActualAVG", typeof(double));
            dtResult.Columns.Add("CollectActualSD", typeof(double));
            dtResult.Columns.Add("CollectActualCV", typeof(double));
            dtResult.Columns.Add("CollectActualN", typeof(int));

            String strCollectSql = @"select
Dict_itm.Ditm_ecode+'-'+Dict_qc_materia.Dmat_batch_no+'-'+Dict_qc_materia.Dmat_level ITEM ,
Rel_qc_materia_detail.Rmatdet_itm_x,
Rel_qc_materia_detail.Rmatdet_itm_sd,
Rel_qc_materia_detail.Rmatdet_itm_cv,
Rel_qc_materia_detail.Rmatdet_itm_unit,
count(*) N,
sum(case isnull(Lis_qc_result.Lres_runaway_flag,0) when 1 then 0 when 2 then 1 else  0 end) 失控数,
avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) [AVG],
isnull(Stdev(cast(Lis_qc_result.Lres_value as numeric(18,2))),0) SD,
(case avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) when 0 then 0 else isnull(Stdev(cast(Lis_qc_result.Lres_value as numeric(18,2))),0)/avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) end)*100 CV                            
from Lis_qc_result 
inner join Rel_qc_materia_detail on Lis_qc_result.Lres_Rmatdet_id=Rel_qc_materia_detail.Rmatdet_Dmat_id  and Lis_qc_result.Lres_Ditm_id=Rel_qc_materia_detail.Rmatdet_Ditm_id
inner join Dict_qc_materia on Lis_qc_result.Lres_Rmatdet_id=Dict_qc_materia.Dmat_id
LEFT OUTER JOIN Dict_itm ON Lis_qc_result.Lres_Ditm_id = Dict_itm.Ditm_id
where  dbo.isReallyNumeric(Lis_qc_result.Lres_value)=1 and Lis_qc_result.del_flag='0'
and Lis_qc_result.Lres_value not in ('+','-')  {0}
group by Dict_qc_materia.Dmat_level,Dict_qc_materia.Dmat_batch_no,Dict_itm.Ditm_ecode,Rel_qc_materia_detail.Rmatdet_itm_x,Rel_qc_materia_detail.Rmatdet_itm_sd,Rel_qc_materia_detail.Rmatdet_itm_cv,Rel_qc_materia_detail.Rmatdet_itm_unit ";


            foreach (DataRow result in dtResult.Rows)
            {
                string strColWhere = string.Format(@"and Lis_qc_result.Lres_Ditm_id='{1}' 
                                                     and Lis_qc_result.Lres_Rmatdet_id='{2}' 
                                                     and (cast (YEAR(Lis_qc_result.Lres_date) AS VARCHAR)+ cast (DATENAME(mm,Lis_qc_result.Lres_date) AS VARCHAR))<={0}",
                                                     result["MONTH"].ToString(), result["Lres_Ditm_id"].ToString(), result["Lres_Rmatdet_id"].ToString());

                DataTable dtCollectValue = helper.ExecuteDtSql(String.Format(strCollectSql, strColWhere)); // "QcCollect"

                if (dtCollectValue.Rows.Count > 0)
                {
                    result["CollectAVG"] = dtCollectValue.Rows[0]["AVG"];
                    result["CollectSD"] = dtCollectValue.Rows[0]["SD"];
                    result["CollectCV"] = dtCollectValue.Rows[0]["CV"];
                    result["CollectN"] = dtCollectValue.Rows[0]["N"];
                }
            }

            String strCollectActualSql = @"select
Dict_itm.Ditm_ecode+'-'+Dict_qc_materia.Dmat_batch_no+'-'+Dict_qc_materia.Dmat_level ITEM ,
Rel_qc_materia_detail.Rmatdet_itm_x,
Rel_qc_materia_detail.Rmatdet_itm_sd,
Rel_qc_materia_detail.Rmatdet_itm_cv,
Rel_qc_materia_detail.Rmatdet_itm_unit,
count(*) N,
sum(case isnull(Lis_qc_result.Lres_runaway_flag,0) when 1 then 0 when 2 then 1 else  0 end) 失控数,
avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) [AVG],
isnull(Stdev(cast(Lis_qc_result.Lres_value as numeric(18,2))),0) SD,
(case avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) when 0 then 0 else isnull(Stdev(cast(Lis_qc_result.Lres_value as numeric(18,2))),0)/avg(cast(Lis_qc_result.Lres_value as numeric(18,2))) end)*100 CV                            
from Lis_qc_result 
inner join Rel_qc_materia_detail on Lis_qc_result.Lres_Rmatdet_id=Rel_qc_materia_detail.Rmatdet_Dmat_id  and Lis_qc_result.Lres_Ditm_id=Rel_qc_materia_detail.Rmatdet_Ditm_id
inner join Dict_qc_materia on Lis_qc_result.Lres_Rmatdet_id=Dict_qc_materia.Dmat_id
LEFT OUTER JOIN Dict_itm ON Lis_qc_result.Lres_Ditm_id = Dict_itm.Ditm_id
where  dbo.isReallyNumeric(Lis_qc_result.Lres_value)=1 and isnull(Lis_qc_result.Lres_runaway_flag,'0')<>'2' and Lis_qc_result.del_flag='0'
and Lis_qc_result.Lres_value not in ('+','-') and isnull(Lis_qc_result.Lres_runaway_flag,'0')<>'2' and Lis_qc_result.Lres_display ='0'  and (Lis_qc_result.Lres_runaway_rule is null or Lis_qc_result.Lres_runaway_rule ='' or Lis_qc_result.Lres_runaway_flag='0') {0}
group by Dict_qc_materia.Dmat_level,Dict_qc_materia.Dmat_batch_no,Dict_itm.Ditm_ecode,Rel_qc_materia_detail.Rmatdet_itm_x,Rel_qc_materia_detail.Rmatdet_itm_sd,Rel_qc_materia_detail.Rmatdet_itm_cv,Rel_qc_materia_detail.Rmatdet_itm_unit";


            foreach (DataRow result in dtResult.Rows)
            {
                string strActualWhere = string.Format(@"and Lis_qc_result.Lres_Ditm_id='{1}' 
                                                        and Lis_qc_result.Lres_Rmatdet_id='{2}' 
                                                        and (cast (YEAR(Lis_qc_result.Lres_date) AS VARCHAR)+ cast (DATENAME(mm,Lis_qc_result.Lres_date) AS VARCHAR))<={0}",
                                                        result["MONTH"].ToString(), result["Lres_Ditm_id"].ToString(), result["Lres_Rmatdet_id"].ToString());

                DataTable dtCollectActualValue = helper.ExecuteDtSql(String.Format(strCollectActualSql, strActualWhere));  // "QcCollectActual"

                if (dtCollectActualValue.Rows.Count > 0)
                {
                    result["CollectActualAVG"] = dtCollectActualValue.Rows[0]["AVG"];
                    result["CollectActualSD"] = dtCollectActualValue.Rows[0]["SD"];
                    result["CollectActualCV"] = dtCollectActualValue.Rows[0]["CV"];
                    result["CollectActualN"] = dtCollectActualValue.Rows[0]["N"];
                }
            }

            listAnayalyData = EntityManager<EntityQcStatistic>.ConvertToList(dtResult).OrderBy(i => i.ITEM).ToList();

            return listAnayalyData;
        }

        public DataSet doNew(DataSet ds,bool isseq)
        {
            DataSet result = new DataSet();
            try
            {
                DBManager helper = new DBManager();

                DataTable qcAudit = ds.Tables["Audit"];
                string sql = "";
                string type = "0";
                string sql1 = "";
                if (qcAudit != null)
                {

                    if (qcAudit.Columns.Contains("type"))
                    {
                        type = qcAudit.Rows[0]["type"].ToString();
                    }
                    string showType = qcAudit.Rows[0]["showType"].ToString();
                    if (showType == "0")
                    {
                        if (type == "1")
                        {
                            sql = @"select 
Dmat_id type_id,
Dmat_level+' - '+Dmat_batch_no type_name,
'-1' parentId,
cast(null as numeric(18,3)) type_c_x,
cast(null as numeric(18,3)) type_sd,
cast(null as numeric(18,3)) type_ccv,
cast(null as varchar(50)) qcr_id,
'0' type_type,
cast(null as datetime) qcr_sdate,
cast(null as datetime) qcr_edate,
cast(null as numeric(18,3)) qcr_cv,
cast(null as numeric(18,3)) qcr_allow_cv,
cast(null as varchar(50)) qcr_c_rule,
cast(null as numeric(18,3)) qcr_max_value,
cast(null as numeric(18,3)) qcr_min_value,
cast(null as varchar(50)) qcr_value_type
from Dict_qc_materia
inner join Lis_qc_result on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id and del_flag='0'
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and del_flag='0'
group by Dmat_id,Dmat_level,Dmat_batch_no
union
select  
Lres_Rmatdet_id +'&'+Rel_itm_combine_item.Rici_Dcom_id type_id,
Dcom_name type_name,
Lres_Rmatdet_id parentId,
cast(null as  numeric(18,3)) type_c_x,
cast(null as numeric(18,3)) type_sd,
cast(null as numeric(18,3)) type_ccv,
cast(null as varchar(50)) qcr_id,
'2' type_type,cast(null as datetime) qcr_sdate,
cast(null as datetime) qcr_edate,
cast(null as numeric(18,3)) qcr_cv,
cast(null as numeric(18,3)) qcr_allow_cv,
cast(null as varchar(50)) qcr_c_rule,
cast(null as numeric(18,3)) qcr_max_value,
cast(null as numeric(18,3)) qcr_min_value,
cast(null as varchar(50)) qcr_value_type
from 
Lis_qc_result 
left join Rel_itm_combine_item on Rel_itm_combine_item.Rici_Ditm_id=Lis_qc_result.Lres_Ditm_id
inner join Dict_itm_combine on Dict_itm_combine.Dcom_id=Rel_itm_combine_item.Rici_Dcom_id and Dict_itm_combine.Dcom_qc_flag=1
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and Lis_qc_result.del_flag='0'
group by Lres_Rmatdet_id,Dcom_name,Rel_itm_combine_item.Rici_Dcom_id ";

                            sql1 = @"select Rmatdet_Dmat_id+'&'+Rmatdet_Ditm_id+'&'+Rel_itm_combine_item.Rici_Dcom_id type_id,
Ditm_ecode type_name,
Rmatdet_Dmat_id+'&'+Rel_itm_combine_item.Rici_Dcom_id parentId,
Rel_qc_materia_detail.Rmatdet_itm_x type_c_x,
Rel_qc_materia_detail.Rmatdet_itm_sd type_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv type_ccv,
Rmatdet_Ditm_id qcr_itm_ecd,
'1' type_type,
cast(null as datetime)  qcr_sdate,
cast(null as datetime)  qcr_edate,
Rel_qc_materia_detail.Rmatdet_itm_cv qcr_cv,
Rel_qc_materia_detail.Rmatdet_allow_cv qcr_allow_cv,
Rel_qc_materia_detail.Rmatdet_rule qcr_c_rule,
Rel_qc_materia_detail.Rmatdet_max_value  qcr_max_value,
Rel_qc_materia_detail.Rmatdet_min_value qcr_min_value,
Rel_qc_materia_detail.Rmatdet_value_type qcr_value_type,
Dict_itm_combine.sort_no as com_seq,
Rel_itm_combine_item.sort_no as com_sort,
Dict_itm.sort_no as itm_seq
from Lis_qc_result  
inner join Rel_qc_materia_detail on Rel_qc_materia_detail.Rmatdet_Ditr_id=Lis_qc_result.Lres_Ditr_id and Lis_qc_result.del_flag='0'
and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id and Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
LEFT OUTER JOIN Dict_qc_materia on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
left join Rel_itm_combine_item on Rel_itm_combine_item.Rici_Ditm_id=Lis_qc_result.Lres_Ditm_id
inner join Dict_itm_combine on Dict_itm_combine.Dcom_id=Rel_itm_combine_item.Rici_Dcom_id and Dict_itm_combine.Dcom_qc_flag=1
where Dict_itm.del_flag=0 and Rmatdet_Dmat_id is not null and Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by 
Rmatdet_Dmat_id,Rmatdet_Ditm_id,Ditm_ecode,Rmatdet_Dmat_id,Rel_qc_materia_detail.Rmatdet_Ditr_id,Rmatdet_itm_x,Rmatdet_itm_sd,Rmatdet_itm_ccv,Rmatdet_itm_cv,Rmatdet_allow_cv,Rel_qc_materia_detail.Rmatdet_rule,Rel_qc_materia_detail.Rmatdet_max_value,Rel_qc_materia_detail.Rmatdet_min_value,Rel_qc_materia_detail.Rmatdet_value_type,Rel_itm_combine_item.Rici_Dcom_id,Dict_itm_combine.sort_no,Rel_itm_combine_item.sort_no,Dict_itm.sort_no
order by Dict_itm_combine.sort_no asc,Rel_itm_combine_item.sort_no asc,Dict_itm.sort_no asc ";
                        }
                        else
                        {
                            sql = @"select 
Dict_qc_materia.Dmat_id+'&'+Rmatdet_Ditm_id type_id,
Dmat_level+' - '+Dmat_batch_no type_name,
Rmatdet_Ditm_id parentId,
isnull(Rel_qc_materia_detail.Rmatdet_itm_x,0) type_c_x,
isnull(Rel_qc_materia_detail.Rmatdet_itm_sd,1) type_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv type_ccv
,Rmatdet_Ditm_id qcr_id,
'1' type_type,
null qcr_sdate,
null qcr_edate,
Rel_qc_materia_detail.Rmatdet_itm_cv qcr_cv,
Rel_qc_materia_detail.Rmatdet_allow_cv qcr_allow_cv,
Rel_qc_materia_detail.Rmatdet_rule qcr_c_rule
,Rel_qc_materia_detail.Rmatdet_max_value qcr_max_value,
Rel_qc_materia_detail.Rmatdet_min_value qcr_min_value,
Rel_qc_materia_detail.Rmatdet_value_type qcr_value_type,
Dict_qc_materia.Dmat_date_start AS qc_par_detail_sdate,
Rel_qc_materia_detail.Rmatdet_reag_manufacturer qcr_reag_manu,
Rel_qc_materia_detail.Rmatdet_m_pro qcr_m_pro,
Rel_qc_materia_detail.Rmatdet_read_valid_date qcr_reag_date,
999 seq
from Lis_qc_result 
inner join Dict_qc_materia on  Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
inner join Rel_qc_materia_detail on Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and Lis_qc_result.del_flag='0'
group by Dmat_id,Dmat_level,Dmat_batch_no,Rmatdet_Dmat_id,Rmatdet_itm_x,Rmatdet_itm_sd,Rmatdet_itm_ccv,Rmatdet_Ditm_id,Rmatdet_itm_cv,Rmatdet_allow_cv,Rmatdet_rule,Rel_qc_materia_detail.Rmatdet_max_value,Rel_qc_materia_detail.Rmatdet_min_value,Rel_qc_materia_detail.Rmatdet_value_type,Dict_qc_materia.Dmat_date_start ,Rel_qc_materia_detail.Rmatdet_reag_manufacturer,Rel_qc_materia_detail.Rmatdet_m_pro,Rel_qc_materia_detail.Rmatdet_read_valid_date
union
select Rmatdet_Ditm_id type_id,
Ditm_ecode type_name,
'-1' parentId,
null type_c_x,
null type_sd,
null type_ccv,Rmatdet_Ditm_id qcr_id,
'0' type_type,
null qcr_sdate ,
null qcr_edate,
null qcr_cv ,
null qcr_allow_cv,
null qcr_c_rule,
null qcr_max_value,
null qcr_min_value,
null qcr_value_type,
null qc_par_detail_sdate,
'' qcr_reag_manu,
'' qcr_m_pro,
null as qcr_reag_date,
isnull(Dict_itm.sort_no,999) seq
from Rel_qc_materia_detail 
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
inner join Lis_qc_result on Rel_qc_materia_detail.Rmatdet_Ditr_id=Lis_qc_result.Lres_Ditr_id and Lis_qc_result.del_flag='0'
and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id and Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id
where Dict_itm.del_flag=0 and Rmatdet_Dmat_id is not null and Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by Rmatdet_Ditm_id,Ditm_ecode,Dict_itm.sort_no";
                        }

                    }
                    else if (showType == "1")
                    {
                        sql = @"select 
Dmat_id type_id,
Dmat_level+' - '+Dmat_batch_no type_name,
'-1' parentId,
null type_c_x,
null type_sd,
null type_ccv,
null qcr_id,
'0' type_type,
null qcr_sdate,
null qcr_edate,
null qcr_cv,
null qcr_allow_cv,
null qcr_c_rule,
null qcr_max_value,
null qcr_min_value,
null qcr_value_type,
Dict_qc_materia.Dmat_date_start AS qc_par_detail_sdate,
'' qcr_reag_manu,
'' qcr_m_pro,
null as qcr_reag_date,
999 seq
from Dict_qc_materia
inner join Lis_qc_result on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id and Lis_qc_result.del_flag='0'
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by Dmat_id,Dmat_level,Dmat_batch_no,Dict_qc_materia.Dmat_date_start
union
select Rmatdet_Dmat_id+'&'+Rmatdet_Ditm_id type_id,
Ditm_ecode type_name,
Rmatdet_Dmat_id parentId,
Rel_qc_materia_detail.Rmatdet_itm_x type_c_x,
Rel_qc_materia_detail.Rmatdet_itm_sd type_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv type_ccv,
Rmatdet_Ditm_id qcr_id,
'1' type_type,
null qcr_sdate,
null qcr_edate,
Rel_qc_materia_detail.Rmatdet_itm_cv qcr_cv,
Rel_qc_materia_detail.Rmatdet_allow_cv qcr_allow_cv,
Rel_qc_materia_detail.Rmatdet_rule qcr_c_rule,
Rel_qc_materia_detail.Rmatdet_max_value qcr_max_value,
Rel_qc_materia_detail.Rmatdet_min_value qcr_min_value,
Rel_qc_materia_detail.Rmatdet_value_type qcr_value_type,
Dict_qc_materia.Dmat_date_start AS qc_par_detail_sdate,
Rel_qc_materia_detail.Rmatdet_reag_manufacturer qcr_reag_manu,
Rel_qc_materia_detail.Rmatdet_m_pro qcr_m_pro,
Rel_qc_materia_detail.Rmatdet_read_valid_date qcr_reag_date,
isnull(Dict_itm.sort_no,999) seq
from Rel_qc_materia_detail 
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
inner join Lis_qc_result on Rel_qc_materia_detail.Rmatdet_Ditr_id=Lis_qc_result.Lres_Ditr_id and Lis_qc_result.del_flag='0'
and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id and Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id
LEFT OUTER JOIN Dict_qc_materia on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
where Dict_itm.del_flag=0 and Rmatdet_Dmat_id is not null and Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by Rmatdet_Dmat_id,Rmatdet_Ditm_id,Ditm_ecode,Rmatdet_Dmat_id,Rel_qc_materia_detail.Rmatdet_id,Rmatdet_itm_x,Rmatdet_itm_sd,Rmatdet_itm_ccv,Rmatdet_itm_cv,Rmatdet_allow_cv,Rel_qc_materia_detail.Rmatdet_rule,Rel_qc_materia_detail.Rmatdet_max_value,Rel_qc_materia_detail.Rmatdet_min_value,Rel_qc_materia_detail.Rmatdet_value_type,Dict_qc_materia.Dmat_date_start,Rel_qc_materia_detail.Rmatdet_reag_manufacturer,Rel_qc_materia_detail.Rmatdet_m_pro,Rel_qc_materia_detail.Rmatdet_read_valid_date,Dict_itm.sort_no";
                    }
                    else
                    {
                        sql = @"select 
Dmat_id type_id,
Dmat_level+' - '+Dmat_batch_no type_name,
'-1' parentId,
null type_c_x,
null type_sd,
null type_ccv,
null qcr_id,
'0' type_type,
null qcr_sdate,
null qcr_edate,
null qcr_cv,
null qcr_allow_cv,
null qcr_c_rule,
null qcr_max_value,
null qcr_min_value,
null qcr_value_type,
Dict_qc_materia.Dmat_date_start AS qc_par_detail_sdate
from Dict_qc_materia
inner join Lis_qc_result on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id and Lis_qc_result.del_flag='0'
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and Lis_qc_result.del_flag='0'
group by Dmat_id,Dmat_level,Dmat_batch_no,Dict_qc_materia.Dmat_date_start
union
select  
Lres_Rmatdet_id+'&'+Rel_itm_combine_item.Rici_Dcom_id type_id,
Dcom_name type_name,
Lres_Rmatdet_id parentId,
null type_c_x,
null type_sd,
null type_ccv,
null qcr_id,
'2' type_type,
null qcr_sdate,
null qcr_edate,
null qcr_cv,
null qcr_allow_cv,
null qcr_c_rule,
null qcr_max_value,
null qcr_min_value,
null qcr_value_type,
null qc_par_detail_sdate
from 
Lis_qc_result 
left join Rel_itm_combine_item on Rel_itm_combine_item.Rici_Ditm_id=Lis_qc_result.Lres_Ditm_id
inner join Dict_itm_combine on Dict_itm_combine.Dcom_id=Rel_itm_combine_item.Rici_Dcom_id and Dict_itm_combine.Dcom_qc_flag=1
inner join Rel_itr_combine on Rel_itr_combine.Ric_Ditr_id='{0}' and Rel_itr_combine.Ric_Dcom_id=Dict_itm_combine.Dcom_id
where Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}' and Lis_qc_result.del_flag='0'
group by Lres_Rmatdet_id,Dcom_name,Rel_itm_combine_item.Rici_Dcom_id
union
select Rmatdet_Dmat_id+'&'+Rmatdet_Ditm_id+'&'+Rel_itm_combine_item.Rici_Dcom_id type_id,
Ditm_ecode type_name,
Rmatdet_Dmat_id+'&'+Rel_itm_combine_item.Rici_Dcom_id parentId,
Rel_qc_materia_detail.Rmatdet_itm_x type_c_x,
Rel_qc_materia_detail.Rmatdet_itm_sd type_sd,
Rel_qc_materia_detail.Rmatdet_itm_ccv type_ccv,
Rmatdet_Ditm_id qcr_id,
'1' type_type,
null qcr_sdate,
null qcr_edate,
Rel_qc_materia_detail.Rmatdet_itm_cv qcr_cv,
Rel_qc_materia_detail.Rmatdet_allow_cv qcr_allow_cv,
Rel_qc_materia_detail.Rmatdet_rule qcr_c_rule,
Rel_qc_materia_detail.Rmatdet_max_value qcr_max_value,
Rel_qc_materia_detail.Rmatdet_min_value qcr_min_value,
Rel_qc_materia_detail.Rmatdet_value_type qcr_value_type,
Dict_qc_materia.Dmat_date_start AS qc_par_detail_sdate
from Lis_qc_result  
inner join Rel_qc_materia_detail on Rel_qc_materia_detail.Rmatdet_Ditr_id=Lis_qc_result.Lres_Ditr_id and Lis_qc_result.del_flag='0'
and Rel_qc_materia_detail.Rmatdet_Ditm_id=Lis_qc_result.Lres_Ditm_id and Rel_qc_materia_detail.Rmatdet_Dmat_id=Lis_qc_result.Lres_Rmatdet_id
LEFT OUTER JOIN Dict_itm ON Rel_qc_materia_detail.Rmatdet_Ditm_id = Dict_itm.Ditm_id 
LEFT OUTER JOIN Dict_qc_materia on Dict_qc_materia.Dmat_id=Lis_qc_result.Lres_Rmatdet_id
left join Rel_itm_combine_item on Rel_itm_combine_item.Rici_Ditm_id=Lis_qc_result.Lres_Ditm_id
inner join Dict_itm_combine on Dict_itm_combine.Dcom_id=Rel_itm_combine_item.Rici_Dcom_id and Dict_itm_combine.Dcom_qc_flag=1
inner join Rel_itr_combine on Rel_itr_combine.Ric_Ditr_id='{0}' and Rel_itr_combine.Ric_Dcom_id=Dict_itm_combine.Dcom_id
where Dict_itm.del_flag=0 and Rmatdet_Dmat_id is not null and Lis_qc_result.Lres_date>='{1}' and  Lis_qc_result.Lres_date<='{2}' and Lis_qc_result.Lres_Ditr_id='{0}'
group by Rmatdet_Dmat_id,Rmatdet_Ditm_id,Ditm_ecode,Rmatdet_Dmat_id,Rel_qc_materia_detail.Rmatdet_Ditr_id,Rmatdet_itm_x,Rmatdet_itm_sd,Rmatdet_itm_ccv,Rmatdet_itm_cv,Rmatdet_allow_cv,Rel_qc_materia_detail.Rmatdet_rule,Rel_qc_materia_detail.Rmatdet_max_value,Rel_qc_materia_detail.Rmatdet_min_value,Rel_qc_materia_detail.Rmatdet_value_type,Rel_itm_combine_item.Rici_Dcom_id,Dict_qc_materia.Dmat_date_start
                                ";
                    }


                    DataRow drAudit = qcAudit.Rows[0];
                    if (drAudit != null)
                    {
                        if (showType == "0" || showType == "1")
                        {
                            sql = string.Format(sql, drAudit["itm_id"], drAudit["sDate"], drAudit["eDate"]);
                            if (!string.IsNullOrEmpty(sql1))
                            {
                                sql1 = string.Format(sql1, drAudit["itm_id"], drAudit["sDate"], drAudit["eDate"]);

                            }
                        }
                        else
                        {
                            DateTime dtSDate = Convert.ToDateTime(drAudit["eDate"]).AddDays(-1).AddSeconds(1);
                            sql = string.Format(sql, drAudit["itm_id"], dtSDate, drAudit["eDate"]);
                        }
                    }

                }
                else
                    sql = "select * from dict_instrmt where itr_del=0";

                DataTable dt = helper.ExecSel(sql);

                if (isseq)
                {
                    if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("seq"))
                    {
                        DataView dv = new DataView(dt);
                        dv.Sort = "seq";
                        dt = dv.ToTable();
                    }
                }
                else if (dt.Columns.Contains("type_name"))
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "type_name";
                    dt = dv.ToTable();
                }


                if (!string.IsNullOrEmpty(sql1))
                {
                    DataTable dt2 = helper.ExecSel(sql1);
                    if (dt2.Rows.Count > 0)
                    {
                        if (dt2.Columns.Contains("com_seq"))
                        {
                            dt2.Columns.Remove("com_seq");
                        }
                        if (dt2.Columns.Contains("com_sort"))
                        {
                            dt2.Columns.Remove("com_sort");
                        }
                        if (dt2.Columns.Contains("itm_seq"))
                        {
                            dt2.Columns.Remove("itm_seq");
                        }
                        dt.Merge(dt2);

                    }

                }

                dt.TableName = "qc_value";
                result.Tables.Add(dt);
                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return null;
            }
        }

        public DataTable QcReagentsCompare(DataTable QcItem, DataTable QcCompareData)
        {
            DBManager helper = new DBManager();
            DataRow drQcItem = QcItem.Rows[0];
            DataRow drQccompareData = QcCompareData.Rows[0];

            string strQcValueSql = string.Format(@"select Lres_value qcm_meas,
                                                   Lres_date qcm_date 
                                                   from Lis_qc_result 
                                                   where 
                                                   Lres_Ditm_id='{0}' 
                                                   and Lres_Rmatdet_id='{1}' 
                                                   and Lres_date>='{2}'
                                                   and Lres_date<='{3}'
                                                   order by Lres_date",
                                                   drQcItem["qcm_itm_ecd"].ToString(), drQcItem["qcm_id"].ToString(),
                                                   drQcItem["sdate"].ToString(), drQcItem["edate"].ToString());

            DataTable dtQcValue = helper.ExecSel(strQcValueSql);


            string strCompareDataSql = string.Format(@"select Lres_value res_chr,
                                                       Lres_date res_date,
                                                       Lres_sid res_sid
                                                       from Lis_result  
                                                       where 
                                                       Lres_flag='1' 
                                                       and Lres_date>='{0}'
                                                       and Lres_date<='{1}'
                                                       and Lres_Ditr_id='{2}'
                                                       and Lres_Ditm_id='{3}'
                                                       and Lres_sid in ({4})
                                                       order by Lres_date",
                                                       drQccompareData["sdate"].ToString(),
                                                       drQccompareData["edate"].ToString(),
                                                       drQccompareData["itr_id"].ToString(),
                                                       drQccompareData["itm_id"].ToString(),
                                                       drQccompareData["sid"].ToString());

            DataTable dtQcCompareData = helper.ExecSel(strCompareDataSql);

            DataTable dtCompareData = new DataTable("dtCompareData");

            dtCompareData.Columns.Add("com_seq");
            dtCompareData.Columns.Add("com_qc_date");
            dtCompareData.Columns.Add("com_qc_data");
            dtCompareData.Columns.Add("com_compare_sid");
            dtCompareData.Columns.Add("com_compare_data");
            dtCompareData.Columns.Add("com_coefficients");

            int MaxCount = dtQcValue.Rows.Count > dtQcCompareData.Rows.Count ? dtQcValue.Rows.Count : dtQcCompareData.Rows.Count;

            for (int i = 0; i < MaxCount; i++)
            {
                DataRow drCompareData = dtCompareData.NewRow();

                drCompareData["com_seq"] = i;
                drCompareData["com_qc_data"] = dtQcValue.Rows.Count > i ? dtQcValue.Rows[i]["qcm_meas"] : string.Empty;
                drCompareData["com_qc_date"] = dtQcValue.Rows.Count > i ? dtQcValue.Rows[i]["qcm_date"] : string.Empty;
                drCompareData["com_compare_data"] = dtQcCompareData.Rows.Count > i ? dtQcCompareData.Rows[i]["res_chr"] : string.Empty;
                drCompareData["com_compare_sid"] = dtQcCompareData.Rows.Count > i ? dtQcCompareData.Rows[i]["res_sid"] : string.Empty;
                if (dtQcValue.Rows.Count > i && dtQcCompareData.Rows.Count > i)
                {
                    try
                    {
                        drCompareData["com_coefficients"] = (Convert.ToDouble(dtQcCompareData.Rows[i]["res_chr"]) - Convert.ToDouble(dtQcValue.Rows[i]["qcm_meas"])) / Convert.ToDouble(dtQcValue.Rows[i]["qcm_meas"]);
                    }
                    catch
                    {
                        drCompareData["com_coefficients"] = string.Empty;
                    }
                }
                dtCompareData.Rows.Add(drCompareData);
            }

            return dtCompareData;
        }
    }
}
