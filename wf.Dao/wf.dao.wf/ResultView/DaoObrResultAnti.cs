using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoObrResultAnti))]
    public class DaoObrResultAnti : DclDaoBase, IDaoObrResultAnti
    {
        public bool DeleteResultById(string obrId)
        {
            DBManager helper = GetDbManager();
            try
            {
                string sql = String.Format("delete from Lis_result_anti where Lanti_id='{0}'", obrId);
                helper.ExecCommand(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityObrResultAnti> GetAntiResultById(string obrId = "", string searchFlag = "1")
        {
            DBManager helper = new DBManager();
            try
            {
                string sql = string.Empty;
                if (searchFlag == "1")
                {
                    sql = string.Format("select   * from Lis_result_anti where Lanti_id='{0}'", obrId);
                }
                else if (searchFlag == "2")
                {
                    sql = string.Format(@"select 
Lis_result_anti.*,
Dict_mic_bacteria.Dbact_cname,
Dict_mic_antibio.Dant_code,
Dict_mic_antibio.Dant_cname,
Dict_mic_antibio.Dant_ename,
Rel_mic_antidetail.Ranti_std_upper_limit,
Rel_mic_antidetail.Ranti_zone_sensitive,
Rel_mic_antidetail.Ranti_std_middle_limit,
Rel_mic_antidetail.Ranti_zone_intermed,
Rel_mic_antidetail.Ranti_std_lower_limit,
Rel_mic_antidetail.Ranti_zone_durgfast
from Lis_result_anti  
left join Rel_mic_antidetail on Lis_result_anti.Lanti_Dantitype_id=Rel_mic_antidetail.Ranti_Dantitype_id and Lis_result_anti.Lanti_Dant_id=Rel_mic_antidetail.Ranti_Dant_id
and Rel_mic_antidetail.del_flag='0'
left join Dict_mic_antibio on Lis_result_anti.Lanti_Dant_id=Dict_mic_antibio.Dant_id 
left join Dict_mic_bacteria on Lis_result_anti.Lanti_Dbact_id=Dict_mic_bacteria.Dbact_id
where Lanti_id='{0}'", obrId);
                }
                else if (searchFlag == "3")
                {
                    sql = string.Format(@"select Lanti_Dbact_id ,a.Lanti_Dant_id ,c.Dant_cname,Lanti_value ,Lanti_mic ,
Lanti_Dantitype_id,Ranti_std_upper_limit,Ranti_std_middle_limit,Ranti_std_lower_limit,Ranti_zone_durgfast,Ranti_zone_intermed,Ranti_zone_sensitive,Lanti_ref,
(case when isnull(Lanti_ref,'MIC')='MIC' then Ranti_std_middle_limit else Ranti_zone_intermed end) ss_zone,
a.Lanti_count_flag,isnull(a.sort_no,1) as sort_no,a.Lanti_remark,history_result1='' --历史结果1
,history_result2=''  --历史结果2
from Lis_result_anti a 
left join Rel_mic_antidetail b on  (a.Lanti_Dant_id=b.Ranti_Dant_id and  a.Lanti_Dantitype_id=b.Ranti_Dantitype_id
and b.del_flag=0)
left join Dict_mic_antibio c on a.Lanti_Dant_id=c.Dant_id
where Lanti_id='{0}'", obrId);
                }
                else if (searchFlag == "4")
                {
                    sql = string.Format(@"select
Lis_result_bact.Lbac_id  as obr_id, 
Lis_result_bact.Lbac_Dbact_id, 
Dict_mic_bacteria.Dbact_cname,
Dict_mic_bacteria.Dbact_Dbactt_id,
Lanti_Dant_id, 
Dict_mic_antibio.Dant_cname,
Lanti_value ,isnull(Lis_result_bact.Lbac_remark,'') obr_remark,isnull(Lis_result_bact.Lbac_sterile,'') obr_sterile
FROM Lis_result_bact  with(nolock)
LEFT JOIN Lis_result_anti  ON Lis_result_anti.Lanti_id=Lis_result_bact.Lbac_id and Lis_result_anti.Lanti_Dbact_id=Lis_result_bact.Lbac_Dbact_id
left join Dict_mic_bacteria on Lis_result_bact.Lbac_Dbact_id=Dict_mic_bacteria.Dbact_id
left join Dict_mic_antibio on Lis_result_anti.Lanti_Dant_id=Dict_mic_antibio.Dant_id
where Lis_result_bact.Lbac_id = '{0}'   ", obrId);
                }
                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityObrResultAnti>.ConvertToList(dt).OrderBy(w => w.ObrBacId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }
        }

        public List<EntityObrResultAnti> GetAntiResultById(string obrId)
        {
            DBManager helper = new DBManager();
            try
            {
                string sql = string.Format(@"select 
Lis_result_anti.*,
Dict_mic_bacteria.Dbact_cname,
Dict_mic_antibio.Dant_code,
Dict_mic_antibio.Dant_cname,
Dict_mic_antibio.Dant_ename,
Rel_mic_antidetail.Ranti_std_upper_limit,
Rel_mic_antidetail.Ranti_zone_sensitive,
Rel_mic_antidetail.Ranti_std_middle_limit,
Rel_mic_antidetail.Ranti_zone_intermed,
Rel_mic_antidetail.Ranti_std_lower_limit,
Rel_mic_antidetail.Ranti_zone_durgfast,
Dict_mic_bacteria.Dbact_cname,
Dict_mic_bacteria.Dbact_Dbactt_id,
Dict_itr_instrument.Ditr_name,
Dict_itr_instrument.Ditr_ename,
isnull(Lis_result_bact.Lbac_remark,'') Lbac_remark,isnull(Lis_result_bact.Lbac_sterile,'') Lbac_sterile
from Lis_result_anti  
left join Rel_mic_antidetail on Lis_result_anti.Lanti_Dantitype_id=Rel_mic_antidetail.Ranti_Dantitype_id and Lis_result_anti.Lanti_Dant_id=Rel_mic_antidetail.Ranti_Dant_id
and Rel_mic_antidetail.del_flag='0'
left join Dict_mic_antibio on Lis_result_anti.Lanti_Dant_id=Dict_mic_antibio.Dant_id 
left join Dict_mic_bacteria on Lis_result_anti.Lanti_Dbact_id=Dict_mic_bacteria.Dbact_id
LEFT JOIN Lis_result_bact  ON Lis_result_anti.Lanti_id=Lis_result_bact.Lbac_id and Lis_result_anti.Lanti_Dbact_id=Lis_result_bact.Lbac_Dbact_id
left join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Lis_result_anti.Lanti_Ditr_id
where Lis_result_anti.Lanti_id='{0}'", obrId);

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityObrResultAnti>.ConvertToList(dt).OrderBy(w => w.ObrBacId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }
        }
        public List<EntityObrResultAnti> GetAntiWithHistoryResultById(string obrId)
        {
            DBManager helper = new DBManager();
            try
            {
                   string sql= string.Format(@"select 
Lanti_Dbact_id ,a.Lanti_Dant_id ,c.Dant_cname,Lanti_value ,Lanti_mic ,
Lanti_Dantitype_id,Ranti_std_upper_limit,Ranti_std_middle_limit,Ranti_std_lower_limit,Ranti_zone_durgfast,Ranti_zone_intermed,Ranti_zone_sensitive,Lanti_ref,
(case when isnull(Lanti_ref,'MIC')='MIC' then Ranti_std_middle_limit else Ranti_zone_intermed end) ss_zone,
a.Lanti_count_flag,isnull(a.sort_no,1) as sort_no,a.Lanti_remark,history_result1='' --历史结果1
,history_result2=''  --历史结果2
from Lis_result_anti a 
left join Rel_mic_antidetail b on  (a.Lanti_Dant_id=b.Ranti_Dant_id and  a.Lanti_Dantitype_id=b.Ranti_Dantitype_id
and b.del_flag=0)
left join Dict_mic_antibio c on a.Lanti_Dant_id=c.Dant_id
where Lanti_id='{0}'", obrId);

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityObrResultAnti>.ConvertToList(dt).OrderBy(w => w.ObrBacId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }
        }

        public List<EntityObrResultAnti> GetAntiResultByListObrId(List<string> listObrId)
        {
            List<EntityObrResultAnti> listResult = new List<EntityObrResultAnti>();
            try
            {
                DBManager helper = new DBManager();
                if (listObrId.Count > 0)
                {
                    string strObr = string.Empty;
                    foreach (string obrId in listObrId)
                    {
                        strObr += string.Format(",'{0}'", obrId);
                    }
                    strObr = strObr.Remove(0, 1);
                    string sql = string.Format(@"select 
Lanti_Dbact_id ,a.Lanti_Dant_id ,c.Dant_cname,Lanti_value ,Lanti_mic ,
Lanti_Dantitype_id,Ranti_std_upper_limit,Ranti_std_middle_limit,Ranti_std_lower_limit,Ranti_zone_durgfast,Ranti_zone_intermed,Ranti_zone_sensitive,Lanti_ref,
(case when isnull(Lanti_ref,'MIC')='MIC' then Ranti_std_middle_limit else Ranti_zone_intermed end) ss_zone,
a.Lanti_count_flag,isnull(a.sort_no,1) as sort_no,a.Lanti_remark,history_result1='' --历史结果1
,history_result2=''  --历史结果2
from Lis_result_anti a 
left join Rel_mic_antidetail b on  (a.Lanti_Dant_id=b.Ranti_Dant_id and  a.Lanti_Dantitype_id=b.Ranti_Dantitype_id
and b.del_flag=0)
left join Dict_mic_antibio c on a.Lanti_Dant_id=c.Dant_id
where Lanti_id in ({0}) ", strObr);
                    DataTable dt = helper.ExecuteDtSql(sql);
                    listResult = EntityManager<EntityObrResultAnti>.ConvertToList(dt).OrderBy(w => w.ObrBacId).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listResult;
        }

        public bool SaveResultAnti(EntityObrResultAnti antiResult)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (antiResult != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(antiResult);

                    helper.InsertOperation("Lis_result_anti", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public List<string> GetAntibosName(EntityAntiQc qc)
        {
            List<string> list = new List<string>();
            try
            {
                string sql;
                StringBuilder sqlSubItrWhere = new StringBuilder();
                if (qc.ListItrId.Count > 0)
                {
                    sqlSubItrWhere.Append("and Pat_lis_main.Pma_Ditr_id in (");
                    bool needComma = false;
                    foreach (string itr_id in qc.ListItrId)
                    {
                        if (needComma)
                            sqlSubItrWhere.Append(",");

                        sqlSubItrWhere.Append(string.Format("'{0}'", itr_id));

                        needComma = true;
                    }

                    sqlSubItrWhere.Append(")");
                }
                else
                {
                    sqlSubItrWhere.Append(" and 1 <> 1");
                }


                if (qc.ExportDataType == 1)
                {
                    #region 区分mic与kb导出数值结果
                    sql = string.Format(@"select
ant_who_no = case
when Dict_mic_antibio.Dant_who_no = 'ESBL' or Dict_mic_antibio.Dant_who_no = 'ESBLs' then 'ESBL'
when Dict_mic_antibio.Dant_who_no = 'BETA_LACT' then 'BETA_LACT'
when Dict_mic_antibio.Dant_who_no = 'MRSA' then 'MRSA'
when (Lis_result_anti.Lanti_ref = 'MIC' and Dict_mic_antibio.Dant_who_no not like '%_NM' and Dict_mic_antibio.Dant_who_no not like '%_ND') then Dict_mic_antibio.Dant_who_no + '_NM'

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and Dict_mic_antibio.Dant_kb_who_no is not null
and len(Dict_mic_antibio.Dant_kb_who_no)>0
)
then Dict_mic_antibio.Dant_kb_who_no

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and (Dict_mic_antibio.Dant_kb_who_no is null or LEN(Dict_mic_antibio.Dant_kb_who_no)=0)
)
then Dict_mic_antibio.Dant_who_no + '_ND'


when (Lis_result_anti.Lanti_ref <> 'KB' and Lis_result_anti.Lanti_ref <> 'MIC') then '实验方法录入错误'
else Dict_mic_antibio.Dant_who_no end
from Lis_result_anti
inner join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result_anti.Lanti_id
inner join Dict_mic_antibio on Lis_result_anti.Lanti_Dant_id = Dict_mic_antibio.Dant_id and Dant_who_no is not null and len(Dant_who_no)>0
where Pma_in_date >= @dateBegin and Pma_in_date < @dateEnd {0}
order by Dict_mic_antibio.sort_no asc
", sqlSubItrWhere);
                    #endregion
                }
                else
                {
                    #region 不区分mic与kb导出药敏结果
                    sql = string.Format(@"select
ant_who_no = case
when Dict_mic_antibio.Dant_who_no = 'ESBL' or Dict_mic_antibio.Dant_who_no = 'ESBLs' then 'ESBL'
when Dict_mic_antibio.Dant_who_no = 'BETA_LACT' then 'BETA_LACT'
when Dict_mic_antibio.Dant_who_no = 'MRSA' then 'MRSA'
when (Lis_result_anti.Lanti_ref = 'MIC' and Dict_mic_antibio.Dant_who_no not like '%_NM' and Dict_mic_antibio.Dant_who_no not like '%_ND') then Dict_mic_antibio.Dant_who_no + '_NM'

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and Dict_mic_antibio.Dant_kb_who_no is not null
and len(Dict_mic_antibio.Dant_kb_who_no)>0
)
then Dict_mic_antibio.Dant_kb_who_no

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and (Dict_mic_antibio.Dant_kb_who_no is null or LEN(Dict_mic_antibio.Dant_kb_who_no)=0)
)
then Dict_mic_antibio.Dant_who_no + '_ND'


when (Lis_result_anti.Lanti_ref <> 'KB' and Lis_result_anti.Lanti_ref <> 'MIC') then '实验方法录入错误'
else Dict_mic_antibio.Dant_who_no end
from Lis_result_anti
inner join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result_anti.Lanti_id
inner join Dict_mic_antibio on Lis_result_anti.Lanti_Dant_id = Dict_mic_antibio.Dant_id and Dant_who_no is not null and len(Dant_who_no)>0
where Pma_in_date >= @dateBegin and Pma_in_date < @dateEnd {0}
order by Dict_mic_antibio.sort_no asc
", sqlSubItrWhere);
                    #endregion
                }
                DBManager helper = GetDbManager();
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@dateBegin", qc.DateStart?.ToString("yyyy-MM-dd HH:mm:ss")));
                paramHt.Add(new SqlParameter("@dateEnd", qc.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss")));
                DataTable table = helper.ExecuteDtSql(sql, paramHt);
                if (table != null && table.Rows.Count > 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        string who_code = row["ant_who_no"].ToString();
                        if (!list.Contains(who_code))
                        {
                            list.Add(who_code);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public List<EntityWhonet> GetAntiData(EntityAntiQc qc)
        {
            List<EntityWhonet> list = new List<EntityWhonet>();
            try
            {
                string sql = string.Empty;
                string sqlWhere = string.Empty;
                if (qc.isAudit)
                {
                    sqlWhere += " (Pma_status = 2 or Pma_status = 4) ";
                }
                string itrIds = string.Empty;
                if (qc.ListItrId.Count > 0)
                {

                    foreach (string itr_id in qc.ListItrId)
                    {
                        itrIds += (string.Format(",'{0}'", itr_id));
                    }
                }
                if (!string.IsNullOrEmpty(itrIds))
                {
                    itrIds = itrIds.Remove(0, 1);
                    sqlWhere += string.Format("and Pat_lis_main.Pma_Ditr_id in ({0})", itrIds);
                }
                else
                {
                    sqlWhere += "and 1<>1";
                }
                if (qc.ExportDataType == 1)
                {
                    #region 区分mic与kb导出数值结果
                    sql = string.Format(@"select
ant_who_no = case
when Dict_mic_antibio.Dant_who_no = 'ESBL' or Dict_mic_antibio.Dant_who_no = 'ESBLs' then 'ESBL'
when Dict_mic_antibio.Dant_who_no = 'BETA_LACT' then 'BETA_LACT'
when Dict_mic_antibio.Dant_who_no = 'MRSA' then 'MRSA'
when (Lis_result_anti.Lanti_ref = 'MIC' and Dict_mic_antibio.Dant_who_no not like '%_NM' and Dict_mic_antibio.Dant_who_no not like '%_ND') then Dict_mic_antibio.Dant_who_no + '_NM'

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and Dict_mic_antibio.Dant_kb_who_no is not null
and len(Dict_mic_antibio.Dant_kb_who_no)>0
)
then Dict_mic_antibio.Dant_kb_who_no

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and (Dict_mic_antibio.Dant_kb_who_no is null or LEN(Dict_mic_antibio.Dant_kb_who_no)=0)
)
then Dict_mic_antibio.Dant_who_no + '_ND'


when (Lis_result_anti.Lanti_ref <> 'KB' and Lis_result_anti.Lanti_ref <> 'MIC') then '实验方法录入错误'
else Dict_mic_antibio.Dant_who_no end ,
Lis_result_anti.Lanti_id,
Dict_mic_bacteria.Dbact_who_no,
Dict_mic_bacteria.Dbact_ename,
Dict_mic_bacteria.Dbact_cname,
Pat_lis_main.Pma_in_date,
isnull(Pat_lis_main.Pma_sam_receive_date,Pat_lis_main.Pma_in_date) as samp_receive_date,
isnull(Pat_lis_main.Pma_sam_send_date,Pat_lis_main.Pma_in_date) as Pma_sam_send_date,
Pat_lis_main.Pma_pat_age_exp,
Lis_result_anti.Lanti_value,
Lis_result_anti.Lanti_mic,
Lis_result_anti.Lanti_kb,
Pat_lis_main.Pma_pat_name,
Pat_lis_main.Pma_pat_sex,
Pat_lis_main.Pma_id,
Pat_lis_main.Pma_pat_dept_name,
---Dic_pub_dept.dept_name as pid_dept_name,
Dict_dept.Ddept_select_code as org_id,--科室查询码
Dict_dept.Ddept_c_code as dept_c_code,
Dict_dept.Ddept_code,
Pat_lis_main.Pma_sam_remark,
Pat_lis_main.Pma_pat_in_no,
Pat_lis_main.Pma_unique_id,
Pat_lis_main.Pma_sid,
Lis_result_bact.Lbac_remark,
Dict_source.Dsorc_name,
Dict_sample.Dsam_id,
Dict_sample.Dsam_name,
Dict_sample.Dsam_code,
Dict_sample.Dsam_c_code as sam_c_code 
from Lis_result_anti
inner join Dict_mic_antibio on Lis_result_anti.Lanti_Dant_id = Dict_mic_antibio.Dant_id and Dant_who_no is not null and len(Dant_who_no)>0
inner join Dict_mic_bacteria on Dict_mic_bacteria.Dbact_id = Lis_result_anti.Lanti_Dbact_id
left join Lis_result_bact on Lis_result_bact.Lbac_id=Lis_result_anti.Lanti_id AND Lis_result_anti.Lanti_Dbact_id = Lis_result_bact.Lbac_Dbact_id
left join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result_anti.Lanti_id
left join Dict_dept on Dict_dept.Ddept_code = Pat_lis_main.Pma_pat_dept_id
left join Dict_source on Dict_source.Dsorc_id = Pat_lis_main.Pma_pat_Dsorc_id
left join Dict_sample on Dict_sample.Dsam_id = Pat_lis_main.Pma_Dsam_id
where Lis_result_anti.Lanti_ref in ('mic','kb') and
Pma_in_date >= @dateBegin and Pma_in_date < @dateEnd and 
 {0}", sqlWhere);
                    #endregion
                }
                else {
                    #region 不区分mic与kb导出药敏结果
                    sql = string.Format(@"select
ant_who_no = case
when Dict_mic_antibio.Dant_who_no = 'ESBL' or Dict_mic_antibio.Dant_who_no = 'ESBLs' then 'ESBL'
when Dict_mic_antibio.Dant_who_no = 'BETA_LACT' then 'BETA_LACT'
when Dict_mic_antibio.Dant_who_no = 'MRSA' then 'MRSA'
when (Lis_result_anti.Lanti_ref = 'MIC' and Dict_mic_antibio.Dant_who_no not like '%_NM' and Dict_mic_antibio.Dant_who_no not like '%_ND') then Dict_mic_antibio.Dant_who_no + '_NM'

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and Dict_mic_antibio.Dant_kb_who_no is not null
and len(Dict_mic_antibio.Dant_kb_who_no)>0
)
then Dict_mic_antibio.Dant_kb_who_no

when (Lis_result_anti.Lanti_ref = 'KB'
and Dict_mic_antibio.Dant_who_no not like '%_ND%'
and Dict_mic_antibio.Dant_who_no not like '%_NM'
and (Dict_mic_antibio.Dant_kb_who_no is null or LEN(Dict_mic_antibio.Dant_kb_who_no)=0)
)
then Dict_mic_antibio.Dant_who_no + '_ND'


when (Lis_result_anti.Lanti_ref <> 'KB' and Lis_result_anti.Lanti_ref <> 'MIC') then '实验方法录入错误'
else Dict_mic_antibio.Dant_who_no end ,

Lis_result_anti.Lanti_id,
Dict_mic_bacteria.Dbact_who_no,
Dict_mic_bacteria.Dbact_ename,
Dict_mic_bacteria.Dbact_cname,
Pat_lis_main.Pma_in_date,
isnull(Pat_lis_main.Pma_sam_receive_date,Pat_lis_main.Pma_in_date) as samp_receive_date,
isnull(Pat_lis_main.Pma_sam_send_date,Pat_lis_main.Pma_in_date) as Pma_sam_send_date,
Pat_lis_main.Pma_pat_age_exp,
Lis_result_anti.Lanti_value,
Lis_result_anti.Lanti_mic,
Lis_result_anti.Lanti_kb,
Pat_lis_main.Pma_pat_name,
Pat_lis_main.Pma_pat_sex,
Pat_lis_main.Pma_id,
Pat_lis_main.Pma_pat_dept_name,
---Dic_pub_dept.dept_name as pid_dept_name,
Dict_dept.Ddept_select_code as org_id,--科室查询码
Dict_dept.Ddept_c_code as dept_c_code,
Dict_dept.Ddept_code,
Pat_lis_main.Pma_sam_remark,
Pat_lis_main.Pma_pat_in_no,
Pat_lis_main.Pma_unique_id,
Pat_lis_main.Pma_sid,
Lis_result_bact.Lbac_remark,
Dict_source.Dsorc_name,
Dict_sample.Dsam_id,
Dict_sample.Dsam_name,
Dict_sample.Dsam_code,
Dict_sample.Dsam_c_code as sam_c_code 
from Lis_result_anti
inner join Dict_mic_antibio on Lis_result_anti.Lanti_Dant_id = Dict_mic_antibio.Dant_id and Dant_who_no is not null and len(Dant_who_no)>0
inner join Dict_mic_bacteria on Dict_mic_bacteria.Dbact_id = Lis_result_anti.Lanti_Dbact_id
left join Lis_result_bact on Lis_result_bact.Lbac_id=Lis_result_anti.Lanti_id AND Lis_result_anti.Lanti_Dbact_id = Lis_result_bact.Lbac_Dbact_id
left join Pat_lis_main on Pat_lis_main.Pma_rep_id = Lis_result_anti.Lanti_id
left join Dict_dept on Dict_dept.Ddept_code = Pat_lis_main.Pma_pat_dept_id
left join Dict_source on Dict_source.Dsorc_id = Pat_lis_main.Pma_pat_Dsorc_id
left join Dict_sample on Dict_sample.Dsam_id = Pat_lis_main.Pma_Dsam_id
where Lis_result_anti.Lanti_ref in ('mic','kb') and
Pma_in_date >= @dateBegin and Pma_in_date < @dateEnd and 
{0}", sqlWhere);
                    #endregion
                }
                DBManager helper = GetDbManager();
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@dateBegin", qc.DateStart?.ToString("yyyy-MM-dd HH:mm:ss")));
                paramHt.Add(new SqlParameter("@dateEnd", qc.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss")));
                DataTable table = helper.ExecuteDtSql(sql, paramHt);
                list = EntityManager<EntityWhonet>.ConvertToList(table).OrderBy(w => w.ObrId).ThenBy(w => w.BacWhoNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;

        }

    }
}
