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
    /// 细菌报告
    /// </summary>
    [Export("wf.plugin.wf", typeof(IMicData))]
    public class DaoMicData :  DclDaoBase, IMicData
    {

        public List<EntityDicMicSmear> GetDicMicSmearByComID(string strComIDs)
        {
            List<EntityDicMicSmear> dtDictNobact = new List<EntityDicMicSmear>();

            if (string.IsNullOrEmpty(strComIDs))
                return dtDictNobact;

            try
            {
                string sqlInComID = "";

                foreach (string strtemp in strComIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.IsNullOrEmpty(sqlInComID))
                    {
                        sqlInComID = "'" + strtemp + "'";
                    }
                    else
                    {
                        sqlInComID += ",'" + strtemp + "'";
                    }
                }

                string sqlSelect = string.Format(@"select Dsme_id from 
Dict_mic_smear with(nolock)
where Dsme_public=1 
or Dsme_id in(select dict_nobact_com.nob_id from dict_nobact_com with(nolock) where com_id in({0}))", sqlInComID);
                DBManager helper = GetDbManager();
                var dt = helper.ExecuteDtSql(sqlSelect);
                dtDictNobact = EntityManager<EntityDicMicSmear>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }

            return dtDictNobact;
        }

        public List<EntityDicMicAntidetail> GetMicAntidetailList(string bacID)
        {
            List<EntityDicMicAntidetail> list = new List<EntityDicMicAntidetail>();
            if (!string.IsNullOrEmpty(bacID))
            {
                try
                {
                    DBManager helper = new DBManager();

                    string sql = string.Format(@"select e.*,Dant_id,a.Dbactt_id,d.Dant_cname,'' Lanti_ref,Ranti_std_middle_limit ss_zone
from Dict_mic_bacttype a,Dict_mic_bacteria b,
dic_mic_antitype c,Dict_mic_antibio d,Rel_mic_antidetail e
where
b.Dbact_Dbactt_id=a.Dbactt_id and a.Dbactt_Dantitype_id=c.Dantitype_id and c.Dantitype_id=e.Ranti_Dantitype_id
and e.Ranti_Dant_id=d.Dant_id and e.del_flag=0
and e.Ranti_flag='0'
and b.Dbact_id='{0}' order by e.sort_no ", bacID);

                    DataTable dtObrResImage = helper.ExecuteDtSql(sql);
                    list = EntityManager<EntityDicMicAntidetail>.ConvertToList(dtObrResImage).OrderBy(i => i.AnsSortNo).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("出错：ID=" + bacID, ex);
                    throw;
                }
            }
            return list;
        }

        public List<EntityPidReportMain> PatientQuery(EntityPatientQC patientCondition)
        {
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            try
            {
                if (patientCondition != null)
                {
                    DBManager helper = new DBManager();

                    string whereStr = GetPatientQueryWhere(patientCondition);

                    string sql = string.Format(@"select 
cast(Pma_sid as bigint) as pat_sid_int,
rep_status_name='',
pid_sex_name = case when Pma_pat_sex = '1' then '男'
when Pma_pat_sex = '2' then '女'
else '' end,
cast(isnull(Pat_lis_main.Pma_pat_age,0)/518400 as decimal(18,0)) Pma_pat_age,
Dict_sample.Dsam_name,
Pat_lis_main.*,
(case Pat_lis_main.Pma_serial_num when '' then null when null then null else cast(Pma_serial_num as bigint) end) rep_serial_num_int,
0 pat_select  
,user1.Buser_name as pat_check_name,
user2.Buser_name as bgName,
user3.Buser_name as lrName,
Dict_itr_instrument.Ditr_ename,0 as Obr_result_bact,
ISNULL(isnull((select top 1 1 from Lis_result_bact where Lis_result_bact.Lbac_id=Pat_lis_main.Pma_rep_id),(select top 1 1 from Lis_result_desc where Lis_result_desc.Lrd_id=Pat_lis_main.Pma_rep_id)),0) hasresult,
ISNULL(isnull((select top 1 2 from Lis_result_anti with(nolock) where Lis_result_anti.Lanti_id=Pat_lis_main.Pma_rep_id),(select top 1 1 from Lis_result_bact with(nolock) where Lis_result_bact.Lbac_id=Pat_lis_main.Pma_rep_id)),0) hasresult2

from Pat_lis_main 
left join Dict_sample on Pat_lis_main.Pma_Dsam_id = Dict_sample.Dsam_id
left join Dict_itr_instrument on Dict_itr_instrument.Ditr_id=Pat_lis_main.Pma_Ditr_id
LEFT OUTER JOIN Base_user user1 on user1.Buser_loginid = Pat_lis_main.Pma_audit_Buser_id
LEFT OUTER JOIN Base_user user2 on user2.Buser_loginid = Pat_lis_main.Pma_report_Buser_id
LEFT OUTER JOIN Base_user user3 on user3.Buser_loginid = Pat_lis_main.Pma_check_Buser_id
where 1=1 {2}", patientCondition.auditWord, patientCondition.reportWord, whereStr);

                    DataTable dt = helper.ExecuteDtSql(sql);
                    listPat = EntityManager<EntityPidReportMain>.ConvertToList(dt).OrderBy(i => i.ItrEname).ThenBy(i => i.PatSidInt).ToList();
                }
                return listPat;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }
        }


        /// <summary>
        /// 拼接查询条件
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        private string GetPatientQueryWhere(EntityPatientQC patientCondition)
        {
            string whereStr = string.Empty;

            //病人ID
            if (!string.IsNullOrEmpty(patientCondition.RepId))
            {
                whereStr += " and Pat_lis_main.Pma_rep_id='" + patientCondition.RepId + "' ";
            }

            //仪器ID组合
            if (patientCondition.ListItrId.Count > 0)
            {
                if (patientCondition.ListItrId.Count == 1)
                {
                    whereStr += " and Pat_lis_main.Pma_Ditr_id ='" + patientCondition.ListItrId[0] + "'";
                }
                else
                {
                    string strItrs = string.Empty;
                    foreach (string item in patientCondition.ListItrId)
                    {
                        strItrs += string.Format(",'{0}'", item);
                    }
                    strItrs = strItrs.Remove(0, 1);
                    whereStr += string.Format(" and Pat_lis_main.Pma_Ditr_id in ({0})", strItrs);
                }

            }

            //录入时间
            if (patientCondition.DateStart != null && patientCondition.DateEnd != null)
            {
                whereStr += string.Format(" and Pat_lis_main.Pma_in_date>='{0}' and Pat_lis_main.Pma_in_date<'{1}'",
                                           patientCondition.DateStart?.ToString("yyyy-MM-dd HH:mm:ss"),
                                           patientCondition.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss"));
            }


            //标本号
            if (!string.IsNullOrEmpty(patientCondition.RepSid))
            {
                whereStr += " and Pat_lis_main.Pma_sid='" + patientCondition.RepSid + "' ";
            }

            //状态
            if (!string.IsNullOrEmpty(patientCondition.RepStatus))
            {
                whereStr += string.Format(@" and Pat_lis_main.Pma_status='{0}' ", patientCondition.RepStatus);
            }
            if (patientCondition.RepUrgent)
            {
                whereStr += " and (Pma_urgent_flag =1 or Pma_urgent_flag =2) ";
            }

            return whereStr;
        }


        /// <summary>
        /// 根据当前仪器和样本号、年份获取满足条件的日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="itr_id">仪器ID</param>
        /// <param name="currentSID">当前样本号</param>
        /// <returns></returns>
        public string GetPatDate_ByItrSID(DateTime date, string itr_id, string currentSID)
        {
            string strRv = "";
            try
            {
                if ((!string.IsNullOrEmpty(itr_id)) && (!string.IsNullOrEmpty(currentSID)))
                {
                    string sqlSelect = string.Format(@"
select top 1 Pma_rep_id,Pma_Ditr_id,Pma_sid,Pma_in_date from Pat_lis_main
where Pma_Ditr_id='{0}' 
and Pma_sid='{1}' 
and Pma_in_date>='{2}-1-1' 
and Pma_in_date<='{2}-12-31 23:59:59.000'
", itr_id, currentSID, date.Year.ToString());


                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(sqlSelect);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dt.Rows[0]["Pma_in_date"].ToString()))
                        {
                            strRv = dt.Rows[0]["Pma_in_date"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return strRv;
        }
        public List<EntityMicViewData> GetMicViewList(DateTime data, string itrid)
        {
            List<EntityMicViewData> list = new List<EntityMicViewData>();

            //string data = dtPa.Rows[0]["pat_date"].ToString();
            DateTime edate = data.Date;
            DateTime sdate = data.Date.AddDays(1).AddMilliseconds(-1);


            try
            {
                DBManager helper = new DBManager();

                string sql = @"select '-1' type_id,'药敏-鉴定结果' type_name,'' type_mic,'' type_anti,'0' type_relevance 
union

select Lbac_id type_id,'样本号:'+cast(Lbac_Pma_sid as varchar(12)) type_name,'' type_mic,'' type_anti,'-1' type_relevance 
from Lis_result_bact where Lbac_Ditr_id='{0}' and Lbac_date>='{1}' and Lbac_date<='{2}'
group by Lbac_id,Lbac_Pma_sid
union
select  Lbac_id+'&'+cast(Lbac_Dbact_id as varchar) type_id,
(case  when isnull(Dbact_id,'')='' then '未知菌株['+cast(Lbac_Dbact_id as varchar)+']' else Dbact_cname+'('+Dbact_ename+')' end) type_name,
'' type_mic,'' type_anti,Lbac_id type_relevance 
from Lis_result_bact 
left join Dict_mic_bacteria on Dict_mic_bacteria.Dbact_id=Lis_result_bact.Lbac_Dbact_id
where Lbac_Ditr_id='{0}' and Lbac_date>='{1}' and Lbac_date<='{2}'
union

select Lanti_id+'&'+Lanti_Dbact_id+'&'+Lanti_Dant_id type_id,
(case  when isnull(Dant_id,'')='' then '未知抗生素['+Lanti_Dant_id+']' else Dant_cname+'('+Dant_ename+')' end) type_name,
Lanti_mic type_mic,Lanti_value type_anti,Lanti_id+'&'+Lanti_Dbact_id type_relevance 
from Lis_result_anti 
left join Dict_mic_antibio on Dict_mic_antibio.Dant_id=Lis_result_anti.Lanti_Dant_id
where Lanti_Ditr_id='{0}' and Lanti_date>='{1}' and Lanti_date<='{2}' and Lanti_id is not null";
                sql = string.Format(sql, itrid, edate.ToString("yyyy-MM-dd HH:mm:ss"), sdate.ToString("yyyy-MM-dd HH:mm:ss"));
                DataTable dtObrResImage = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityMicViewData>.ConvertToList(dtObrResImage).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }

            return list;
        }

        /// <summary>
        /// 获取药敏结果表
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        public string GetAntiResult(List<string> repIDs)
        {
            string strin = string.Empty;
            foreach (string repid in repIDs)
            {
                strin += string.Format("'{0}',", repid);
            }
            strin = strin.TrimEnd(',');

            DataSet ds = new DataSet();
            try
            {
                DBManager helper = new DBManager();
                string sql = @"

DECLARE @cols AS NVARCHAR (MAX)
DECLARE @query AS NVARCHAR (MAX)

--DROP table #tb1
SELECT a.Pma_sid 样本号
   , s.Dsam_name 样本类型
	--, a.Pma_bar_code 标本号
    , d.Dbact_cname  细菌鉴定结果
    , e.Dant_cname ant_cname
    --, e.Dant_code ant_code
    , c.Lanti_value anti_value
INTO #tb1
FROM Pat_lis_main a
    LEFT JOIN lis_result_anti c ON a.Pma_rep_id = c.Lanti_id
    LEFT JOIN Dict_mic_bacteria d ON c.Lanti_Dbact_id = d.Dbact_id
    LEFT JOIN Dict_mic_antibio e ON e.Dant_id = c.Lanti_Dant_id
    LEFT JOIN dict_sample s ON s.Dsam_id = a.pma_dsam_id
WHERE a.Pma_status IN ('2', '4') AND a.Pma_rep_id IN ({0})  

SELECT @cols = stuff ((SELECT DISTINCT ',' + quotename(a.ant_cname)
                       FROM #tb1 a
                           FOR XML PATH ('')
                           , TYPE).value ('.', 'NVARCHAR(MAX)'), 1, 1, '')

SET @query = N'SELECT row_number() over (order by 样本号) 序号, * from( select * from  #tb1 )x pivot (max(anti_value) for ant_cname in ('+ @cols + '))p'
EXECUTE(@query)  ";

                sql = string.Format(sql, strin);
                DataTable dt = helper.ExecuteDtSql(sql);
                ds.Tables.Add(dt);
                return ds.GetXml();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }
        }
    }
}
