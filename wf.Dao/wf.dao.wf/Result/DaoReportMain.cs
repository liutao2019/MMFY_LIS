using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data.Common;
using System.Data.SqlClient;
using dcl.dao.core;
using System.Data;
using System.ComponentModel.Composition;
using dcl.common;
using System.Diagnostics;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoPidReportMain))]
    public class DaoReportMain : DclDaoBase, IDaoPidReportMain
    {

        public bool ExsitSidOrHostOrder(string rep_sid, string rep_itr_id, DateTime rep_in_date, string flag)
        {
            bool result = false;
            string column = "Pma_sid";
            if (flag == "1")
            {
                column = "Pma_serial_num";
            }
            if (rep_sid != null && rep_itr_id != null && rep_in_date != null)
            {
                DBManager helper = new DBManager();
                try
                {
                    //检查样本号是否已存在
                    string sql = string.Format(@"
                    select top 1 {4} from
                    Pat_lis_main with(nolock)  where {4}='{0}' 
                    and Pma_Ditr_id ='{1}' 
                    and Pma_in_date >= '{2}' 
                    and Pma_in_date<'{3}'", rep_sid, rep_itr_id, rep_in_date.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    rep_in_date.AddDays(1).Date.ToString("yyyy-MM-dd HH:mm:ss"), column);
                    int rowCount = 0;
                    DataTable dt = helper.ExecuteDtSql(sql);
                    if (dt.Rows.Count > 0)
                        int.TryParse(dt.Rows[0][0].ToString(), out rowCount);
                    if (rowCount > 0)
                        result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public List<EntityPidReportMain> GetPatientByMergeComid(string BcMergeComid)
        {
            try
            {
                string sql = string.Format(@"select top 1 Pma_rep_id,Pma_Ditr_id,Pma_sid,Pma_in_date,Dict_itr_instrument.Ditr_ename from Pat_lis_main with(NOLOCK)
left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
where Pma_bar_code=(
select top 1 Sample_main.Sma_bar_code from Sample_main
inner join Sample_detail on Sample_detail.Sdet_Sma_bar_id=Sample_main.Sma_bar_code
where Sample_main.del_flag='0' and Sample_detail.del_flag='0' and Sample_detail.Sdet_flag=1
and Sample_main.Sma_merge_com_id='{0}')", BcMergeComid);

                DBManager helper = new DBManager();

                DataTable dtResult = helper.ExecuteDtSql(sql);

                List<EntityPidReportMain> list = EntityManager<EntityPidReportMain>.ConvertToList(dtResult).OrderByDescending(i => i.RepId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

        public string GetPatientPatId(string patItrId, string patBarcode, string patSid, DateTime repInDate)
        {
            string patId = string.Empty;
            try
            {
                string sql = string.Format(@"select Pma_rep_id  from Pat_lis_main where Pma_Ditr_id='{0}' and Pma_bar_code='{1}' 
and Pma_sid='{2}'  and Pma_in_date>DateAdd(d,-1,'{3}') and Pma_in_date<='{3}'  ",
                                           patItrId, patBarcode, patSid, repInDate.ToString("yyyy-MM-dd HH:mm:ss"));
                DataTable dt = new DataTable();
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    patId = dt.Rows[0]["Pma_rep_id"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return patId;
        }

        /// <summary>
        /// 获取病人信息是否超时
        /// </summary>
        /// <param name="patId"></param>
        /// <param name="repCType"></param>
        /// <returns></returns>
        public string GetPatientIsOverTime(string patId, string repCType)
        {
            string IsOverTime = string.Empty;
            try
            {
                string sql = string.Format(@"select
CASE WHEN datediff(minute,Pma_apply_date,getdate()) >= Dict_itm_combine_timerule.Dtr_time THEN 1 ELSE 0 END isovertime
from Pat_lis_main with(nolock)
left join Pat_lis_detail with(nolock) on Pat_lis_detail.Pdet_id = Pat_lis_main.Pma_rep_id
left join Dict_itm_combine on Pat_lis_detail.Pdet_Dcom_id = Dict_itm_combine.Dcom_id 
LEFT JOIN Dict_itm_combine_timerule_related ON Dict_itm_combine_timerule_related.Dtrr_Dcom_id=Dict_itm_combine.Dcom_id
LEFT JOIN Dict_itm_combine_timerule ON Dict_itm_combine_timerule.Dtr_id=Dict_itm_combine_timerule_related.Dtrr_Dtr_id
AND (Pat_lis_main.Pma_pat_Dsorc_id=Dict_itm_combine_timerule.Dtr_Dsorc_id  or Dict_itm_combine_timerule.Dtr_Dsorc_id is null or Dict_itm_combine_timerule.Dtr_Dsorc_id='') 
AND (Dict_itm_combine_timerule.Dtr_start_type='5' and Dict_itm_combine_timerule.Dtr_end_type='60')
AND Dict_itm_combine_timerule.Dtr_type='{0}'
where
Pat_lis_main.Pma_rep_id = '{1}' and Dict_itm_combine_timerule.Dtr_time is not null and isnull(Pma_status,0)<>(case when Dict_itm_combine_timerule.Dtr_end_type='40' then 1 else 2 end )
and ((datediff(minute,Pma_apply_date,getdate())) >= Dict_itm_combine_timerule.Dtr_time OR (Dict_itm_combine_timerule.Dtr_rea_time>0 and datediff(minute,Pma_apply_date,getdate()) > (Dict_itm_combine_timerule.Dtr_time-Dict_itm_combine_timerule.Dtr_rea_time)))
and Dict_itm_combine_timerule.Dtr_time>0", repCType, patId);
                DataTable dt = new DataTable();
                DBManager helper = new DBManager();
                dt = helper.ExecuteDtSql(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    IsOverTime = dt.Rows[0]["isovertime"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return IsOverTime;
        }

        public bool InsertNewPatient(EntityPidReportMain patients)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (patients != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(patients);

                    helper.InsertOperation("Pat_lis_main", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }
        public bool UpdatePatientData(EntityPidReportMain patient)
        {
            bool result = false;
            if (patient != null)
            {
                DBManager helper = GetDbManager();
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBUpdateParameter(patient);
                    if (!values.Keys.Contains("Pma_remark"))
                    {
                        values.Add("Pma_remark", patient.RepRemark);
                    }

                    values.Remove("Pma_rep_id");
                    values.Remove("Pma_bar_code");

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Pma_rep_id", patient.RepId);

                    helper.UpdateOperation("Pat_lis_main", values, keys);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw ex;
                }
            }
            return result;
        }


        /// <summary>
        /// 获取单个病人信息
        /// </summary>
        /// <param name="repID"></param>
        /// <returns></returns>
        public EntityPidReportMain GetPatientInfo(string repID)
        {
            DBManager helper = new DBManager();// GetDbManager();
            string sql = string.Format(@"SELECT
Pat_lis_main.*,
cast(Pma_sid as bigint) as pat_sid_int,
Dict_sample.Dsam_name,
Dict_itr_instrument.Ditr_ename,
Dict_itr_instrument.Ditr_report_type,
rep_status_name = case when Pma_status = 0 then '未审核'
when Pma_status = 1 then '已审核'
when Pma_status = 2 then '已报告'
when Pma_status = 4 then '已打印'
else '未审核' end,
Dict_ident.Didt_name,
case 
when user1.Buser_name is null then Pma_audit_Buser_id
else user1.Buser_name 
end as pid_chk_name,
case 
when user2.Buser_name is null then Pma_report_Buser_id
else user2.Buser_name
end as bgName,
case 
when userRec.Buser_name is null then Pma_check_Buser_id
else userRec.Buser_name
end as lrName,
Sys_user6.Buser_name as Pma_micreport_sendname,
Sys_user7.Buser_name as Pma_micreport_chkname,
status = 0,
UrgStatus =0,--危急值数
(case when Pma_read_date is null or Pma_read_date='' then '' else CONVERT(varchar(25),Pma_read_date,120) end) as Pma_read_date
,Dict_organize.Dorg_id
,Dict_organize.Dorg_name
FROM Pat_lis_main with(nolock)
Left join Dict_sample on Pat_lis_main.Pma_Dsam_id = Dict_sample.Dsam_id
LEFT OUTER JOIN Dict_itr_instrument ON Pat_lis_main.Pma_Ditr_id = Dict_itr_instrument.Ditr_id
LEFT OUTER JOIN Dict_ident ON Dict_ident.Didt_id = Pat_lis_main.Pma_pat_Didt_id and Dict_ident.del_flag = '0'
LEFT OUTER JOIN Dict_doctor ON Dict_doctor.Ddoctor_id = Pat_lis_main.Pma_Ddoctor_id
LEFT OUTER JOIN Base_user user1 on user1.Buser_loginid = Pat_lis_main.Pma_audit_Buser_id
LEFT OUTER JOIN Base_user user2 on user2.Buser_loginid = Pat_lis_main.Pma_report_Buser_id
LEFT OUTER JOIN Base_user userRec on userRec.Buser_loginid = Pat_lis_main.Pma_check_Buser_id
LEFT OUTER JOIN Base_user AS Sys_user6 ON Pat_lis_main.Pma_micreport_senduserid = Sys_user6.Buser_loginid
LEFT OUTER JOIN Base_user AS Sys_user7 ON Pat_lis_main.Pma_micreport_chkuserid = Sys_user7.Buser_loginid
left join Dict_dept on Pat_lis_main.Pma_pat_dept_id = Dict_dept.Ddept_code
left join Dict_organize on Dict_dept.Ddept_Dorg_id = Dict_organize.Dorg_id
--left join pid_report_detail with(nolock) on Pid_report_main.rep_id = pid_report_detail.rep_id
--left join Dic_itm_combine on pid_report_detail.com_id = Dic_itm_combine.com_id
where Pma_rep_id='{0}'", repID);

            DataTable dt = helper.ExecuteDtSql(sql);
            var listPat = EntityManager<EntityPidReportMain>.ConvertToList(dt).ToList();

            if (listPat.Count > 0)
                return listPat[0];
            else
            {
                //新增是有可能返回空行！
                //Lib.LogManager.Logger.LogInfo("返回空行", sql);
            }

            return null;
        }


        public List<EntityPidReportMain> GetPatientInfo(IEnumerable<string> repID)
        {
            string strWhere = string.Empty;
            foreach (string item in repID)
            {
                strWhere += string.Format(",'{0}'", item);
            }
            strWhere = strWhere.Remove(0, 1);

            DBManager helper = new DBManager();// GetDbManager();
            string sql = string.Format(@"SELECT
            Pat_lis_main.*,
            cast(Pma_sid as bigint) as pat_sid_int,
            Dict_sample.Dsam_name,
            Dict_itr_instrument.Ditr_ename,
            Dict_itr_instrument.Ditr_report_type,
            rep_status_name = case when Pma_status = 0 then '未审核'
            when Pma_status = 1 then '已审核'
            when Pma_status = 2 then '已报告'
            when Pma_status = 4 then '已打印'
            else '未审核' end,
            Dict_ident.Didt_name,
            case 
            when user1.Buser_name is null then Pma_audit_Buser_id
            else user1.Buser_name 
            end as pid_chk_name,
            case 
            when user2.Buser_name is null then Pma_report_Buser_id
            else user2.Buser_name
            end as bgName,
            case 
            when userRec.Buser_name is null then Pma_check_Buser_id
            else userRec.Buser_name
            end as lrName,
            status = 0,
            UrgStatus =0,--危急值数
            (case when Pma_read_date is null or Pma_read_date='' then '' else CONVERT(varchar(25),Pma_read_date,120) end) as Pma_read_date
            ,Dict_organize.Dorg_id
            ,Dict_organize.Dorg_name
            FROM Pat_lis_main with(nolock)
            Left join Dict_sample on Pat_lis_main.Pma_Dsam_id = Dict_sample.Dsam_id
            LEFT OUTER JOIN Dict_itr_instrument ON Pat_lis_main.Pma_Ditr_id = Dict_itr_instrument.Ditr_id
            LEFT OUTER JOIN Dict_ident ON Dict_ident.Didt_id = Pat_lis_main.Pma_pat_Didt_id and Dict_ident.del_flag = '0'
            LEFT OUTER JOIN Dict_doctor ON Dict_doctor.Ddoctor_id = Pat_lis_main.Pma_Ddoctor_id
            LEFT OUTER JOIN Base_user user1 on user1.Buser_loginid = Pat_lis_main.Pma_audit_Buser_id
            LEFT OUTER JOIN Base_user user2 on user2.Buser_loginid = Pat_lis_main.Pma_report_Buser_id
            LEFT OUTER JOIN Base_user userRec on userRec.Buser_loginid = Pat_lis_main.Pma_check_Buser_id
            LEFT OUTER JOIN Base_user AS Sys_user5 ON Pat_lis_main.Pma_read_Buser_id = Sys_user5.Buser_loginid
            left join Dict_dept on Pat_lis_main.Pma_pat_dept_id = Dict_dept.Ddept_code
            left join Dict_organize on Dict_dept.Ddept_Dorg_id = Dict_organize.Dorg_id
            --left join pid_report_detail with(nolock) on Pid_report_main.rep_id = pid_report_detail.rep_id
            --left join Dic_itm_combine on pid_report_detail.com_id = Dic_itm_combine.com_id
            where Pma_rep_id in ({0})", strWhere);

            DataTable dt = helper.ExecuteDtSql(sql);
            List<EntityPidReportMain> listPat = EntityManager<EntityPidReportMain>.ToList(dt).ToList();

            return listPat;
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
                    string queryTat = string.Empty;
                    string leftTat = string.Empty;
                    if (patientCondition.QueryTaT)
                    {
                        queryTat = @" ,(case  when  Pma_status='2' or Pma_status='4' then Datediff(mi,tat_pro_record.tpr_receiver_date,Pma_report_date)  else dateDiff(mi,tat_pro_record.tpr_receiver_date,getdate())  end) as TatTime  ";
                        leftTat = @"left join tat_pro_record on Pat_lis_main.Pma_bar_code=tat_pro_record.tpr_bar_code ";
                    }
                    string sql = string.Format(@"SELECT
Pma_sid,
cast(Pma_sid as bigint) as pat_sid_int,
Pma_pat_sex,
Pma_collection_part,
Pma_in_date,
Pma_pat_name,
Pma_pat_age,
Pma_pat_age_exp,
Pma_pat_birthday,  
Pma_pat_bed_no,
Pat_lis_main.Pma_rep_id,
Pma_audit_Buser_id,--审核者
Pma_report_Buser_id,--报告者
Pma_Ditr_id,Pma_pat_diag,
Pat_lis_main.Pma_Dsam_id,
Dict_sample.Dsam_name,
Dict_itr_instrument.Ditr_ename,
Dict_itr_instrument.Ditr_report_type,
Dict_itr_instrument.Ditr_name,
Pma_status,
Pma_sam_reach_date,
Pma_remark,
Pma_pat_remark,
rep_status_name = case when Pma_status = 0 then '未{0}'
when Pma_status = 1 then '已{1}'
when Pma_status = 2 then '已{1}'
when Pma_status = 4 then '已打印'
else '未{0}' end,
Pma_com_name,
Pat_lis_main.Pma_bar_code,
Pma_serial_num,
Pma_ctype,
Pma_urgent_flag,
Pma_pat_in_no,
Pma_pat_Dsorc_id,
Pma_pat_Didt_id,
Pma_unique_id,
Pma_report_date,
Dict_ident.Didt_name,
Pma_check_date,Pma_apply_date,
Pma_sam_receive_date,
Pma_collection_date,
Pma_audit_date,
Pma_Ddoctor_id,
Pma_doctor_name,
Pma_pat_Dinsu_id,
Pma_pat_tel,
Pma_pat_address,
Dict_doctor.Ddoctor_name,
Pma_social_no,
Pma_sam_send_date,
Pma_pat_dept_name,Pma_pat_dept_id,
Pma_identity_name,--病人身份
Pma_identity_card,
Pma_input_id,
Pma_pat_exam_company,
Pma_exam_no,
Pma_modify_frequency,
case 
when user1.Buser_name is null then Pma_audit_Buser_id
else user1.Buser_name 
end as pid_chk_name,
case 
when user2.Buser_name is null then Pma_report_Buser_id
else user2.Buser_name
end as bgName,
case 
when userRec.Buser_name is null then Pma_check_Buser_id
else userRec.Buser_name
end as lrName,
Pma_check_Buser_id,
--com_line_color = (select top 1 (isnull(Dic_itm_combine.com_online_clolr,0))),
status = 0,
UrgStatus =0,--危急值数
Pat_lis_main.Pma_recheck_flag,Pat_lis_main.Pma_read_Buser_id as pat_look_name,
(case when Pma_read_date is null or Pma_read_date='' then '' else CONVERT(varchar(25),Pma_read_date,120) end) as Pma_read_date
,Pma_itr_audit_flag,Pma_print_flag,Pma_temp_flag,Pat_lis_main.Pma_sam_remark,
--,Dic_Pub_organize.org_id
--,Dic_Pub_organize.org_name,
Pma_initial_flag,Pma_initial_date,Pma_initial_user_id,Pma_Dpurp_id,Dict_dept.Ddept_tel,
Pma_comment, Pma_discribe
{3}
FROM Pat_lis_main with(nolock)
Left join Dict_sample on Pat_lis_main.Pma_Dsam_id = Dict_sample.Dsam_id
LEFT OUTER JOIN Dict_itr_instrument ON Pat_lis_main.Pma_Ditr_id = Dict_itr_instrument.Ditr_id
LEFT OUTER JOIN Dict_ident ON Dict_ident.Didt_id = Pat_lis_main.Pma_pat_Didt_id and Dict_ident.del_flag = '0'
LEFT OUTER JOIN Dict_doctor ON Dict_doctor.Ddoctor_id = Pat_lis_main.Pma_Ddoctor_id
LEFT OUTER JOIN Base_User user1 on user1.Buser_loginid = Pat_lis_main.Pma_audit_Buser_id
LEFT OUTER JOIN Base_User user2 on user2.Buser_loginid = Pat_lis_main.Pma_report_Buser_id
LEFT OUTER JOIN Base_User userRec on userRec.Buser_loginid = Pat_lis_main.Pma_check_Buser_id
LEFT OUTER JOIN Base_User AS Sys_user5 ON Pat_lis_main.Pma_read_Buser_id = Sys_user5.Buser_loginid
left join Dict_dept on Pat_lis_main.Pma_pat_dept_id = Dict_dept.Ddept_code
left join Dict_organize on Dict_dept.Ddept_Dorg_id = Dict_organize.Dorg_id
{4}
--left join Obr_result on Pid_report_main.rep_id = Obr_result.obr_id
--left join pid_report_detail with(nolock) on Pid_report_main.rep_id = pid_report_detail.rep_id
--left join Dic_itm_combine on pid_report_detail.com_id = Dic_itm_combine.com_id
where 1=1  {2}", patientCondition.auditWord, patientCondition.reportWord, whereStr, queryTat, leftTat);
                    //Stopwatch sop = new Stopwatch();
                    //sop.Start();
                    DataTable dt = helper.ExecuteDtSql(sql);

                    //Lib.LogManager.Logger.LogInfo("ExecuteDtSql:" + sql);
                    //Lib.LogManager.Logger.LogInfo("dtCount:" + dt.Rows.Count.ToString());

                    //listPat = EntityManager<EntityPatients>.ConvertToList(dt).OrderBy(i => i.ItrEname).ThenBy(i => i.PatSidInt).ToList();

                    foreach (DataRow drPat in dt.Rows)
                    {
                        EntityPidReportMain entity = new EntityPidReportMain();
                        entity.RepSid = drPat["Pma_sid"].ToString();

                        Int64 SidInt = 0;
                        Int64.TryParse(drPat["pat_sid_int"].ToString(), out SidInt);
                        entity.PatSidInt = SidInt;

                        entity.PidSex = drPat["Pma_pat_sex"].ToString();
                        entity.CollectionPart = drPat["Pma_collection_part"].ToString();

                        if (drPat["Pma_in_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_in_date"].ToString()))
                        {
                            entity.RepInDate = Convert.ToDateTime(drPat["Pma_in_date"].ToString());
                            //entity.DestRepInDate = Convert.ToDateTime(drPat["Pma_in_date"].ToString());
                        }

                        entity.PidName = drPat["Pma_pat_name"].ToString();


                        if (drPat["Pma_pat_age"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_pat_age"].ToString()))
                        {
                            entity.PidAge = Convert.ToDecimal(drPat["Pma_pat_age"].ToString());
                        }

                        entity.PidAgeExp = drPat["Pma_pat_age_exp"].ToString();

                        if (drPat["Pma_pat_birthday"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_pat_birthday"].ToString()))
                        {
                            entity.PidBirthday = Convert.ToDateTime(drPat["Pma_pat_birthday"].ToString());
                        }

                        entity.PidBedNo = drPat["Pma_pat_bed_no"].ToString();
                        entity.RepId = drPat["Pma_rep_id"].ToString();
                        entity.RepAuditUserId = drPat["Pma_audit_Buser_id"].ToString();
                        entity.RepReportUserId = drPat["Pma_report_Buser_id"].ToString();
                        entity.RepItrId = drPat["Pma_Ditr_id"].ToString();
                        entity.PidDiag = drPat["Pma_pat_diag"].ToString();
                        entity.PidSamId = drPat["Pma_Dsam_id"].ToString();
                        entity.SamName = drPat["Dsam_name"].ToString();
                        entity.ItrEname = drPat["Ditr_ename"].ToString();
                        entity.ItrReportType = drPat["Ditr_report_type"].ToString();
                        entity.ItrName = drPat["Ditr_name"].ToString();
                        if (drPat["Pma_status"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_status"].ToString()))
                        {
                            entity.RepStatus = Convert.ToInt32(drPat["Pma_status"].ToString());
                        }

                        if (drPat["Pma_sam_reach_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_sam_reach_date"].ToString()))
                        {
                            entity.SampReachDate = Convert.ToDateTime(drPat["Pma_sam_reach_date"].ToString());
                        }

                        entity.RepRemark = drPat["Pma_remark"].ToString();
                        entity.PidRemark = drPat["Pma_pat_remark"].ToString();
                        entity.RepStatusName = drPat["rep_status_name"].ToString();
                        entity.PidComName = drPat["Pma_com_name"].ToString();
                        entity.RepBarCode = drPat["Pma_bar_code"].ToString();
                        entity.RepSerialNum = drPat["Pma_serial_num"].ToString();
                        entity.RepCtype = drPat["Pma_ctype"].ToString();

                        if (drPat["Pma_urgent_flag"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_urgent_flag"].ToString()))
                        {
                            entity.RepUrgentFlag = Convert.ToInt32(drPat["Pma_urgent_flag"].ToString());
                        }
                        entity.PidInNo = drPat["Pma_pat_in_no"].ToString();
                        entity.PidSrcId = drPat["Pma_pat_Dsorc_id"].ToString();
                        entity.PidIdtId = drPat["Pma_pat_Didt_id"].ToString();
                        entity.PidUniqueId = drPat["Pma_unique_id"].ToString();

                        if (drPat["Pma_report_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_report_date"].ToString()))
                        {
                            entity.RepReportDate = Convert.ToDateTime(drPat["Pma_report_date"].ToString());
                        }
                        entity.IdtName = drPat["Didt_name"].ToString();

                        if (drPat["Pma_check_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_check_date"].ToString()))
                        {
                            entity.SampCheckDate = Convert.ToDateTime(drPat["Pma_check_date"].ToString());
                        }
                        if (drPat["Pma_apply_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_apply_date"].ToString()))
                        {
                            entity.SampApplyDate = Convert.ToDateTime(drPat["Pma_apply_date"].ToString());
                        }
                        if (drPat["Pma_sam_receive_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_sam_receive_date"].ToString()))
                        {
                            entity.SampReceiveDate = Convert.ToDateTime(drPat["Pma_sam_receive_date"].ToString());
                        }
                        if (drPat["Pma_collection_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_collection_date"].ToString()))
                        {
                            entity.SampCollectionDate = Convert.ToDateTime(drPat["Pma_collection_date"].ToString());
                        }
                        if (drPat["Pma_audit_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_audit_date"].ToString()))
                        {
                            entity.RepAuditDate = Convert.ToDateTime(drPat["Pma_audit_date"].ToString());
                        }
                        entity.PidDoctorCode = drPat["Pma_Ddoctor_id"].ToString();
                        entity.PidDocName = drPat["Pma_doctor_name"].ToString();
                        entity.PidInsuId = drPat["Pma_pat_Dinsu_id"].ToString();
                        entity.PidTel = drPat["Pma_pat_tel"].ToString();
                        entity.PidAddress = drPat["Pma_pat_address"].ToString();
                        entity.DoctorName = drPat["Ddoctor_name"].ToString();
                        entity.PidSocialNo = drPat["Pma_social_no"].ToString();

                        if (drPat["Pma_sam_send_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_sam_send_date"].ToString()))
                        {
                            entity.SampSendDate = Convert.ToDateTime(drPat["Pma_sam_send_date"].ToString());
                        }
                        entity.PidDeptName = drPat["Pma_pat_dept_name"].ToString();
                        entity.PidDeptId = drPat["Pma_pat_dept_id"].ToString();
                        entity.PidIdentityName = drPat["Pma_identity_name"].ToString();
                        entity.PidChkName = drPat["pid_chk_name"].ToString();
                        entity.BgName = drPat["bgName"].ToString();
                        entity.LrName = drPat["lrName"].ToString();
                        entity.RepCheckUserId = drPat["Pma_check_Buser_id"].ToString();
                        entity.PidIdentityCard = drPat["Pma_identity_card"].ToString();

                        Int32 status = 0;
                        Int32.TryParse(drPat["status"].ToString(), out status);
                        entity.Status = status;

                        Int32 urgStatus = 0;
                        Int32.TryParse(drPat["UrgStatus"].ToString(), out urgStatus);
                        entity.UrgStatus = urgStatus;

                        Int32 repRecheckFlag = 0;
                        Int32.TryParse(drPat["Pma_recheck_flag"].ToString(), out repRecheckFlag);
                        entity.RepRecheckFlag = repRecheckFlag;

                        entity.PatLookName = drPat["pat_look_name"].ToString();

                        if (drPat["Pma_read_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_read_date"].ToString()))
                        {
                            entity.RepReadDate = Convert.ToDateTime(drPat["Pma_read_date"].ToString());
                        }

                        Int32 repAuditWay = 0;
                        Int32.TryParse(drPat["Pma_itr_audit_flag"].ToString(), out repAuditWay);
                        entity.RepAuditWay = repAuditWay;

                        if (drPat["Pma_print_flag"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_print_flag"].ToString()))
                        {
                            entity.RepPrintFlag = Convert.ToInt32(drPat["Pma_print_flag"].ToString());
                        }
                        entity.RepTempFlag = drPat["Pma_temp_flag"].ToString();
                        entity.SampRemark = drPat["Pma_sam_remark"].ToString();
                        if (drPat["Pma_input_id"] != null)
                        {
                            entity.RepInputId = drPat["Pma_input_id"].ToString();
                        }
                        if (drPat["Pma_exam_no"] != null)
                        {
                            entity.PidExamNo = drPat["Pma_exam_no"].ToString();
                        }
                        if (drPat["Pma_pat_exam_company"] != null)
                        {
                            entity.PidExamCompany = drPat["Pma_pat_exam_company"].ToString();
                        }
                        if (drPat["Pma_modify_frequency"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_modify_frequency"].ToString()))
                        {
                            entity.RepModifyFrequency = Convert.ToInt32(drPat["Pma_modify_frequency"]);
                        }
                        Int32 repInitialFlag = 0;
                        Int32.TryParse(drPat["Pma_initial_flag"].ToString(), out repInitialFlag);
                        entity.RepInitialFlag = repInitialFlag;

                        if (drPat["Pma_initial_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_initial_date"].ToString()))
                        {
                            entity.RepInitialDate = Convert.ToDateTime(drPat["Pma_initial_date"].ToString());
                        }
                        entity.RepInitialUserId = drPat["Pma_initial_user_id"].ToString();
                        entity.PidPurpId = drPat["Pma_Dpurp_id"].ToString();
                        if (dt.Columns.Contains("TatTime") && drPat["TatTime"] != DBNull.Value && !string.IsNullOrEmpty(drPat["TatTime"].ToString()))
                        {
                            entity.TatTime = drPat["TatTime"].ToString();
                        }
                        if (dt.Columns.Contains("Ddept_tel") && drPat["Ddept_tel"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Ddept_tel"].ToString()))
                        {
                            entity.DeptTel = drPat["Ddept_tel"].ToString();
                        }
                        if (dt.Columns.Contains("Pma_comment") && drPat["Pma_comment"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_comment"].ToString()))
                        {
                            entity.RepComment = drPat["Pma_comment"].ToString();
                        }
                        if (dt.Columns.Contains("Pma_discribe") && drPat["Pma_discribe"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_discribe"].ToString()))
                        {
                            entity.RepDiscribe = drPat["Pma_discribe"].ToString();
                        }
                        listPat.Add(entity);
                    }

                    listPat = listPat.OrderBy(i => i.ItrEname).ThenBy(i => i.PatSidInt).ToList();

                    //sop.Stop();
                    //Lib.LogManager.Logger.LogInfo("ConvertToList:" + sop.ElapsedMilliseconds.ToString());

                }
                return listPat;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

        /// <summary>
        /// 获取病人信息的数量
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatientsCount(EntityPatientQC patientCondition)
        {
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            try
            {
                if (patientCondition != null)
                {
                    DBManager helper = new DBManager();

                    string whereStr = string.Empty;

                    //标识ID
                    if (patientCondition.IsEnabled == "1")
                    {
                        whereStr += string.Format(@" and Dict_itm_combine.Dcom_report_time is not null
                and(datediff(minute, Pat_lis_main.Pma_apply_date, getdate()) + Dict_itm_combine.Dcom_report_time * 0.2) >= Dict_itm_combine.Dcom_report_time
                and Dict_itm_combine.Dcom_report_time > 0");
                    }
                    else if (patientCondition.IsEnabled == "2")
                    {
                        whereStr += string.Format(@" and cast(Dict_itm_combine.Dcom_urgent_report_time as int) is not null
and (datediff(minute,Pat_lis_main.Pma_apply_date,getdate()) + cast(Dict_itm_combine.Dcom_urgent_report_time as int) * 0.2) >= cast(Dict_itm_combine.Dcom_urgent_report_time as int)
and cast(Dict_itm_combine.Dcom_urgent_report_time as int)>0");
                    }

                    //病人ID
                    if (!string.IsNullOrEmpty(patientCondition.RepId))
                    {
                        whereStr += " and Pat_lis_main.Pma_rep_id='" + patientCondition.RepId + "' ";
                    }
                    //录入时间
                    if (patientCondition.DateStart != null && patientCondition.DateEnd != null)
                    {
                        whereStr += string.Format(" and Pat_lis_main.Pma_in_date>='{0}' and Pat_lis_main.Pma_in_date<='{1}'",
                                                   patientCondition.DateStart,
                                                   patientCondition.DateEnd);
                    }
                    string sql = string.Format(@"SELECT 
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_apply_date
FROM Pat_lis_main with(nolock)
left join Pat_lis_detail with(nolock) on Pat_lis_main.Pma_rep_id = Pat_lis_detail.Pdet_id
left join Dict_itm_combine on Pat_lis_detail.Pdet_Dcom_id = Dict_itm_combine.Dcom_id
where 1=1 {0}", whereStr);

                    DataTable dt = helper.ExecuteDtSql(sql);
                    listPat = EntityManager<EntityPidReportMain>.ConvertToList(dt).OrderBy(i => i.ItrEname).ThenBy(i => i.PatSidInt).ToList();
                }
                return listPat;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
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
                if (!patientCondition.NotInRepId)
                {
                    whereStr += " and Pat_lis_main.Pma_rep_id='" + patientCondition.RepId + "' ";
                }
                else
                {
                    whereStr += " and Pat_lis_main.Pma_rep_id<>'" + patientCondition.RepId + "' ";
                }
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
            if (patientCondition.DateStart != null && patientCondition.DateStart != DateTime.MinValue && patientCondition.DateEnd != null && patientCondition.DateEnd != DateTime.MinValue)
            {
                whereStr += string.Format(" and Pat_lis_main.Pma_in_date>='{0}' and Pat_lis_main.Pma_in_date<='{1}'",
                                           patientCondition.DateStart?.ToString("yyyy-MM-dd HH:mm:ss"),
                                           patientCondition.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //病人ID
            if (!string.IsNullOrEmpty(patientCondition.PidInNo))
            {
                whereStr += "and Pma_pat_in_no ='" + patientCondition.PidInNo + "'";
            }
            //病人名称
            if (!string.IsNullOrEmpty(patientCondition.PidName))
            {
                whereStr += string.Format(" and Pat_lis_main.Pma_pat_name = '{0}'", patientCondition.PidName);
            }
            //条码号
            if (!string.IsNullOrEmpty(patientCondition.RepBarCode))
            {
                whereStr += string.Format("and Pat_lis_main.Pma_bar_code='{0}'", patientCondition.RepBarCode);
            }
            //标本类别
            if (!string.IsNullOrEmpty(patientCondition.SamId))
            {
                whereStr += " and Pat_lis_main.Pma_Dsam_id='" + patientCondition.SamId + "' ";
            }

            //科室
            if (!string.IsNullOrEmpty(patientCondition.DepId))
            {
                whereStr += " and (Pat_lis_main.Pma_pat_dept_id = '" + patientCondition.DepId + "' or Pma_pat_dept_name ='" + patientCondition.PatDepName + "') ";
            }
            //检验者
            if (!string.IsNullOrEmpty(patientCondition.PidCheckUserId))
            {
                whereStr += " and Pat_lis_main.Pma_check_Buser_id='" + patientCondition.PidCheckUserId + "' ";
            }
            //病人Id类型
            if (!string.IsNullOrEmpty(patientCondition.PidIdtId))
            {
                whereStr += "and Pma_pat_Didt_id='" + patientCondition.PidIdtId + "'";
            }

            //状态
            if (!string.IsNullOrEmpty(patientCondition.RepStatus))
            {
                if (!patientCondition.NotInRepStatus)
                {
                    if (patientCondition.RepStatus == "0")
                    {
                        whereStr += string.Format("and (Pma_status in ({0}) or Pma_status is null)", patientCondition.RepStatus);
                    }
                    else
                    {
                        whereStr += string.Format("and Pma_status in ({0})", patientCondition.RepStatus);
                    }
                }
                else
                {
                    whereStr += string.Format("and Pma_status not in ({0})", patientCondition.RepStatus);
                }
            }

            if (patientCondition.LessPatDate != null)
            {
                whereStr += string.Format(" and Pat_lis_main.Pma_in_date<'{0}' ",
                                        patientCondition.LessPatDate);
            }
            //如果用样本号查询
            if (patientCondition.ListSidRange.Count > 0)
            {
                string strSidRange = string.Empty;

                foreach (var item in patientCondition.ListSidRange)
                {
                    //范围
                    if (item.EndSid != null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_sid as BIGINT)>={0} 
                                                        and cast(Pat_lis_main.Pma_sid as BIGINT)<={1})",
                                                        item.StartSid,
                                                        item.EndSid);
                    }
                    //单个
                    else if (item.EndSid == null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_sid as BIGINT)={0})",
                                                       item.StartSid);
                    }
                }

                strSidRange = strSidRange.Remove(0, 3);

                whereStr += string.Format(" and ({0}) and isnumeric(Pat_lis_main.Pma_sid) = 1", strSidRange);

            }

            //如果用流水号查询
            if (patientCondition.ListSortRange.Count > 0)
            {
                string strSidRange = string.Empty;

                foreach (var item in patientCondition.ListSortRange)
                {
                    //范围
                    if (item.EndNo != null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_serial_num as BIGINT)>={0} 
                                                        and cast(Pat_lis_main.Pma_serial_num as BIGINT)<={1})",
                                                        item.StartNo,
                                                        item.EndNo);
                    }
                    //单个
                    else if (item.EndNo == null)
                    {
                        strSidRange += string.Format(@" or( cast(Pat_lis_main.Pma_serial_num as BIGINT)={0})",
                                                       item.StartNo);
                    }
                }

                strSidRange = strSidRange.Remove(0, 3);

                whereStr += string.Format(" and ({0}) and isnumeric(Pat_lis_main.Pma_serial_num) = 1", strSidRange);
            }
            //备注不为空
            if (patientCondition.RepRemarkIsNotNull)
            {
                whereStr += string.Format(" and Pat_lis_main.Pma_remark <>'' and Pat_lis_main.Pma_remark is not null ");
            }

            //状态
            //if (!string.IsNullOrEmpty(patientCondition.RepStatus) && !patientCondition.NotInRepStatus)
            //{
            //    whereStr += string.Format(@" and Pid_report_main.rep_status='{0}' ", patientCondition.RepStatus);
            //}

            //复查
            //if (patientCondition.RepRecheck)
            //{
            //    whereStr += " and isnull(Pid_report_main.rep_recheck_flag,0)<>0 ";
            //}

            //危急
            //if (patientCondition.RepUrgent)
            //{
            //    whereStr += " and isnull(Pid_report_main.rep_urgent_flag,0)<>0 ";
            //}

            //来源
            if (!string.IsNullOrEmpty(patientCondition.RepSrcId))
            {
                whereStr += string.Format(@" and Pat_lis_main.Pma_pat_Dsorc_id='{0}' ", patientCondition.RepSrcId);
            }

            //历史结果查询列
            if (patientCondition.historySelectColumn != HistorySelectColumn.病人ID)
            {

            }

            //组合ID
            if (!string.IsNullOrEmpty(patientCondition.ComId))
            {
                whereStr += string.Format(@" AND EXISTS(SELECT 1 FROM Sample_detail sd WHERE sd.sdet_com_id in ('{0}') AND sd.sdet_bar_code = pat_lis_main.Pma_bar_code and sd.del_flag = '0')", patientCondition.ComId);
            }
            return whereStr;
        }

        public bool UpdateNewRepIdByOldPatId(string newRepId, string repId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(newRepId) && !string.IsNullOrEmpty(repId))
            {
                DBManager helper = new DBManager();
                try
                {
                    string sql = string.Format("update Pat_lis_main set Pma_rep_id = '{0}' where Pma_rep_id ='{1}' ", newRepId, repId);
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public bool UpdatePidComNameByRepId(string pidComName, string repId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(pidComName) && !string.IsNullOrEmpty(repId))
            {
                DBManager helper = new DBManager();
                try
                {
                    string sql = string.Format("update Pat_lis_main set Pma_com_name='{0}' where Pma_rep_id ='{1}'", pidComName, repId);
                    helper.ExecCommand(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public List<EntityPidReportMain> GetPatientStateForQueryIsResult(EntityPatientQC eyPatientQC)
        {
            List<EntityPidReportMain> listIsResult = new List<EntityPidReportMain>();

            //判断录入时间是否有值并且是正确的时间
            bool isDateStart = eyPatientQC.DateStart != null && eyPatientQC.DateStart > Convert.ToDateTime("1900-01-01");
            bool isDateEnd = eyPatientQC.DateEnd != null && eyPatientQC.DateEnd > Convert.ToDateTime("1900-01-01");

            if (eyPatientQC != null && eyPatientQC.ListItrId.Count > 0 && isDateStart && isDateEnd)
            {
                try
                {
                    #region 查找是否有结果(SQL语句)
                    string sqlStr = @"
select
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_ctype,
Pat_lis_main.Pma_recheck_flag,
0 as com_line_color,
cast(null as datetime) rep_date,-1 as resstatus,
case when Pat_lis_main.Pma_status is null then 0
     else Pat_lis_main.Pma_status end as rep_status,
status = case when exists(select top 1 Lres_Pma_rep_id from Lis_result where Lis_result.Lres_Pma_rep_id = Pat_lis_main.Pma_rep_id and Lis_result.Lres_flag=1) then 1 else 0 end
|case when exists(select top 1 Lrd_id from Lis_result_desc where Lis_result_desc.Lrd_id = Pat_lis_main.Pma_rep_id and Lis_result_desc.Lrd_flag=1 ) then 1 else 0 end,
UrgStatus =(select count(Lres_Pma_rep_id) from Lis_result where Lres_Pma_rep_id=Pat_lis_main.Pma_rep_id
               and Lres_ref_flag in('6','16','24','32','40','48','56','256','384','512','640','768','896')
                 ),--危急值数
'' as msg_content 
,'' as modify_flag
,0 as hasWarningMsg
,'0' AS ResMergeFlag
from Pat_lis_main with(nolock)
where 1=1 ";
                    #endregion

                    if (!string.IsNullOrEmpty(eyPatientQC.ListItrId[0]))
                    {
                        sqlStr += string.Format("and Pma_Ditr_id = '{0}'", eyPatientQC.ListItrId[0]);
                    }
                    if (isDateStart)
                    {
                        sqlStr += string.Format("and Pma_in_date >= '{0}'", eyPatientQC.DateStart?.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    if (isDateEnd)
                    {
                        sqlStr += string.Format("and Pma_in_date < '{0}'", eyPatientQC.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    DBManager helper = new DBManager();

                    DataTable dtPatiens = helper.ExecuteDtSql(sqlStr);

                    listIsResult = EntityManager<EntityPidReportMain>.ConvertToList(dtPatiens).OrderBy(i => i.RepId).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("查询是否有结果GetPatientStateForQueryIsResult", ex);
                }
            }
            return listIsResult;
        }


        public List<EntityPidReportMain> SearchPatientForReportCopyUse(string sbPatId)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            if (!string.IsNullOrEmpty(sbPatId))
            {
                try
                {
                    DBManager helper = new DBManager();

                    string sqlStr = string.Format("Select * from Pat_lis_main where Pma_rep_id in ({0}) ", sbPatId);

                    DataTable dtPats = helper.ExecuteDtSql(sqlStr);
                    listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtPats).OrderBy(i => i.RepId).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("查询病人信息(报告复制用)SearchPatientForReportCopyUse", ex);
                }
            }
            return listPats;
        }


        public bool UpdateRepRecheckFlag(EntityPidReportMain patient)
        {
            bool result = false;
            if (patient != null)
            {
                DBManager helper = new DBManager();
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("Pma_recheck_flag", patient.RepRecheckFlag);

                    if (!string.IsNullOrEmpty(patient.RepRemark))
                        values.Add("Pma_remark", patient.RepRemark);

                    Dictionary<string, object> keys = new Dictionary<string, object>();
                    keys.Add("Pma_rep_id", patient.RepId);

                    helper.UpdateOperation("Pat_lis_main", values, keys);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }

        public bool UpdateRepReadUserId(string strOpType, string RepReadUserId, string repId, bool unAllLookcode)
        {
            bool result = false;
            string strUpdate = string.Empty;
            if (strOpType.ToLower() == "nw")//记录阅读者
            {
                strUpdate = string.Format(@"update Pat_lis_main set Pma_read_Buser_id='{0}',Pma_read_date='{1}' 
where Pma_rep_id='{0}' and (Pma_status=2 or Pma_status=4) and Pma_read_Buser_id is null and Pma_read_date is null",
RepReadUserId, DateTime.Now);
            }
            else if (strOpType.ToLower() == "un")//取消阅读者
            {
                //当前用户是否有权限取消所有阅读信息,加上是否有科室权限
                if (unAllLookcode)
                {
                    if (RepReadUserId != "admin")
                    {
                        strUpdate = @"update Pat_lis_main set Pma_read_Buser_id=null where Pma_rep_id='{0}' and Pma_read_Buser_id is not null and Pma_pat_dept_id in (
  select [Dict_dept].[Ddept_code]
from Base_user
inner join [poweruserdepart] on Base_user.Buser_id=[poweruserdepart].userInfoId
inner join Dict_dept on Dict_dept.Ddept_id=poweruserdepart.departId
where Base_user.Buser_loginid='" + RepReadUserId + "')  ";
                    }
                    else
                    {
                        strUpdate = @"update Pat_lis_main set Pma_read_Buser_id=null where Pma_rep_id='{0}' and Pma_read_Buser_id is not null  ";
                    }
                }
                else
                {
                    strUpdate = string.Format(@"update Pat_lis_main set Pma_read_Buser_id=null
                        where Pma_read_Buser_id='{0}'and Pma_rep_id='{0}'  ", RepReadUserId);
                }
            }
            else if (strOpType == "unForOwnLooker")//自己取消自己
            {
                strUpdate = string.Format(@"update Pat_lis_main set Pma_read_Buser_id=null 
                    where Pma_read_Buser_id='{0}' and Pma_rep_id='{0}' ", RepReadUserId);
            }

            try
            {
                DBManager helper = new DBManager();
                helper.ExecCommand(strUpdate);

                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public string GetItrSID_MaxPlusOne(DateTime date, string itr_id, bool excluseSeqRecord, string strGetAllMaxSidItrId)
        {
            try
            {
                string sql = string.Format(@"select 
max(cast(Pma_sid as bigint))
from Pat_lis_main with(nolock)
where 
Pma_Ditr_id = '{0}'
and Pma_in_date >= @pat_date_from 
and Pma_in_date < @pat_date_to ", itr_id);
                //是否排除双向录入结果
                if (excluseSeqRecord)
                {
                    sql += " and (Pma_serial_num = '' or Pma_serial_num is null) ";
                }
                List<DbParameter> paramHt = new List<DbParameter>();

                if (!string.IsNullOrEmpty(strGetAllMaxSidItrId) && strGetAllMaxSidItrId.Contains(itr_id))
                {
                    paramHt.Add(new SqlParameter("@pat_date_from", date.Date.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                else
                {
                    paramHt.Add(new SqlParameter("@pat_date_from", date.Date.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                paramHt.Add(new SqlParameter("@pat_date_to", date.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")));

                DBManager helper = new DBManager();
                object objHostOrder = helper.SelOne(sql, paramHt);

                if (objHostOrder == null || objHostOrder == DBNull.Value)
                {
                    return "1";
                }
                else
                {
                    long maxHostOrder = Convert.ToInt64(objHostOrder);
                    return (maxHostOrder + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }

        }
        public void UpdateRepStatus(string repId, string repStatus, bool isPrintState)
        {
            if (!string.IsNullOrEmpty(repId) && !string.IsNullOrEmpty(repStatus))
            {
                try
                {
                    string sql = "update Pat_lis_main set Pma_status = @rep_status , Pma_print_date = getdate() where Pma_rep_id = @rep_id and   (Pma_status=2 or Pma_status=4) ";
                    if (!isPrintState)
                    {
                        sql = "update Pat_lis_main set Pma_status = @rep_status where Pma_rep_id=@rep_id";
                    }
                    DBManager helper = new DBManager();
                    List<DbParameter> paramHt = new List<DbParameter>();
                    paramHt.Add(new SqlParameter("@rep_status", repStatus));
                    paramHt.Add(new SqlParameter("@rep_id", repId));
                    helper.ExecCommand(sql, paramHt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }

        public bool DeletePatient(string repId)
        {
            DBManager helper = GetDbManager();
            try
            {
                string sql = string.Format("delete Pat_lis_main where Pma_rep_id='{0}'", repId);
                helper.ExecCommand(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public string GetItrHostOrder_MaxPlusOne(DateTime date, string itr_id)
        {
            string sqlSelect = string.Format(@"select 
                                                max(cast(Pma_serial_num as bigint))
                                                from Pat_lis_main with(nolock)
                                                where 
                                                Pma_Ditr_id = '{0}'
                                                and Pma_in_date >= @rep_in_date_from 
                                                and Pma_in_date < @rep_in_date_to 
                                                ", itr_id);

            List<DbParameter> paramHt = new List<DbParameter>();

            paramHt.Add(new SqlParameter("@rep_in_date_from", date.Date.ToString("yyyy-MM-dd HH:mm:ss")));
            paramHt.Add(new SqlParameter("@rep_in_date_to", date.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")));


            DBManager helper = new DBManager();
            object objHostOrder = helper.SelOne(sqlSelect, paramHt);

            if (objHostOrder == null || objHostOrder == DBNull.Value)
            {
                return "1";
            }
            else
            {
                long maxHostOrder = Convert.ToInt64(objHostOrder);
                return (maxHostOrder + 1).ToString();
            }
        }


        public List<EntityPidReportMain> GetPatByExistResult(List<string> listItrId, DateTime startDate, DateTime endDate, bool result)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            try
            {
                DBManager helper = new DBManager();
                string strItrs = string.Empty;
                foreach (string item in listItrId)
                {
                    strItrs += string.Format(",'{0}'", item);
                }
                strItrs = strItrs.Remove(0, 1);
                string sql = string.Format(@"
SELECT distinct
Pma_rep_id,
Pma_Ditr_id,
Dict_itr_instrument.Ditr_ename,
Pma_sid,
cast(Pma_sid as bigint) as pat_sid_int, 
Pma_serial_num, --新增 流水号[序号(双向)]
Pma_pat_name,
Pma_pat_sex,
Pma_pat_dept_name,
Pma_status,
Pma_apply_date,
rep_status_name = case when Pma_status = '0' or Pma_status is null or Pma_status = '' then '未审核'
when Pma_status = '1' then '已审核'
when Pma_status = '2' then '已报告'
when Pma_status = '4' then '已打印' end,
rep_state_name = case when Pma_status = '0' or Pma_status is null or Pma_status = '' then '未审核'
when Pma_status = '1' then '未报告'
when Pma_status = '2' then '未打印'
when Pma_status = '4' then '已打印' end
FROM Pat_lis_main 
inner join Lis_result on Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id and Lis_result.Lres_flag = 1
inner join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Pat_lis_main.Pma_Ditr_id
WHERE
Pma_Ditr_id in ({0})
and (Pma_in_date >='{1}' and Pma_in_date <'{2}')", strItrs, startDate.ToString("yyyy-MM-dd HH:mm:ss"), endDate.ToString("yyyy-MM-dd HH:mm:ss"));

                if (!result)
                {
                    sql = string.Format(@"SELECT Pma_rep_id,Pma_sid,cast(Pma_sid as bigint) as pat_sid_int,
Pma_serial_num, --新增 流水号[序号(双向)]
Pma_pat_name,Pma_pat_sex,Pma_pat_dept_name,Pma_status ,Dict_itr_instrument.Ditr_ename,
Pma_com_name,  --组合名称
Pma_in_date   --录入日期
FROM Pat_lis_main 
left join Lis_result on Pat_lis_main.Pma_rep_id = Lis_result.Lres_Pma_rep_id
inner join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Pat_lis_main.Pma_Ditr_id
where 
Lis_result.Lres_Pma_rep_id is null
and Pma_Ditr_id in ({0})
and (Pma_in_date >='{1}' and Pma_in_date <'{2}')", strItrs, startDate.ToString("yyyy-MM-dd HH:mm:ss"), endDate.ToString("yyyy-MM-dd HH:mm:ss"));

                }

                DataTable dtPats = helper.ExecuteDtSql(sql);
                listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtPats).OrderBy(i => i.PatSidInt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本进程GetPatByExistResult", ex);
            }
            return listPats;
        }
        public List<EntityPidReportMain> GetAllLabPat(string labId, DateTime startDate, DateTime endDate)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"(SELECT distinct
Pma_rep_id,
Pma_Ditr_id,
Dict_itr_instrument.Ditr_ename,
Pma_sid,
cast(Pma_sid as bigint) as pat_sid_int 
,Pma_pat_name,
Pma_pat_sex,
Pma_pat_dept_name,
Pma_status,
Pma_apply_date,
Pat_lis_main.Pma_com_name,
rep_status_name = case when Pma_status = '0' or Pma_status is null or Pma_status = '' then '未审核'
when Pma_status = '1' then '已审核'
when Pma_status = '2' then '已报告'
when Pma_status = '4' then '已打印' end,

rep_state_name = case when Pma_status = '0' or Pma_status is null or Pma_status = '' then '未审核'
when Pma_status = '1' then '未报告'
when Pma_status = '2' then '未打印'
when Pma_status = '4' then '已打印' end
FROM Pat_lis_main
inner join Dict_itr_instrument on Dict_itr_instrument.Ditr_id = Pat_lis_main.Pma_Ditr_id
left join Dict_profession on Dict_itr_instrument.Ditr_lab_id=Dict_profession.Dpro_id
WHERE
Dict_itr_instrument.Ditr_lab_id = '{0}'
and (Pma_in_date >='{1}' and Pma_in_date <'{2}')
)
union
(SELECT distinct
'' as rep_id,
'' as rep_itr_id,
'' as itr_ename,
'' as rep_sid,
'' as pat_sid_int 
,Sma_pat_name as pid_name,
Sma_pat_sex as pid_sex,
Sma_pat_dept_name as pid_dept_name,
'' as rep_status,
Sma_receiver_date as samp_apply_date,
Sma_com_name as pid_com_name,
rep_status_name = '已签收',

rep_state_name = '已签收未登记'
FROM Sample_main
WHERE
Sma_type = '{0}'
and (Sma_receiver_date >='{1}' and Sma_receiver_date <'{2}') and Sma_status_id='5')", labId,
startDate.ToString("yyyy-MM-dd HH:mm:ss"), endDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DataTable dtPats = helper.ExecuteDtSql(sql);
                listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtPats).OrderBy(i => i.PatSidInt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("标本进程GetAllLabPat", ex);
            }
            return listPats;
        }

        public bool UpdateRepModifyFrequencyByRepId(int repModifyFrequency, string repId)
        {
            bool result = false;
            DBManager helper = new DBManager();
            try
            {
                string sql = string.Format("update Pat_lis_main set Pma_modify_frequency = {0} where Pma_rep_id ='{1}' ", repModifyFrequency + 1, repId);
                helper.ExecCommand(sql);
                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
        public bool UploadNewPatient(EntityPidReportMain patients)
        {
            bool result = false;

            DBManager helper = new DBManager("GHConnectionString");

            if (patients != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(patients);

                    helper.InsertOperation("Pat_lis_main", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }

        public bool UpdateReportSumInfo(string repId, string sumInfo)
        {
            bool result = false;
            DBManager helper = new DBManager();
            try
            {
                string sql = string.Format("update Pat_lis_main set rep_sum_info = '{0}' where Pma_rep_id ='{1}' ", sumInfo, repId);
                helper.ExecCommand(sql);
                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public bool UpdateReadUserIdAndUrgentFlag(bool urgent, string repReadUserId, string repID)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();
                string sql = "update Pat_lis_main set Pma_urgent_flag = 2,Pma_read_Buser_id =@rep_read_user_id, Pma_read_date = getdate() where Pma_rep_id =@rep_id ";
                if (urgent)//暂时用来急查用
                {
                    sql = "update Pat_lis_main set Pma_read_Buser_id = @rep_read_user_id, Pma_read_date = getdate() where Pma_rep_id = @rep_id";
                }
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@rep_read_user_id", repReadUserId));
                paramHt.Add(new SqlParameter("@rep_id", repID));

                result = helper.ExecCommand(sql, paramHt) > 0;
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
        public bool UpdateDrugfastFlag(string repId)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"update Pat_lis_main set Pma_drugfast_flag=2 where Pma_rep_id = '{0}' and Pma_drugfast_flag=1", repId);
                result = helper.ExecCommand(sql) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        #region 插入多条报告单记录
        /// <summary>
        /// 插入多条报告单记录
        /// </summary>
        /// <param name="Reports"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool InsertReports(List<EntityPidReportMain> Reports, out string ErrorMsg)
        {
            ErrorMsg = "";
            DBManager helper = null;
            try
            {
                helper = new DBManager();
                this.Dbm = helper;

                IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                detailDao.Dbm = helper;

                helper.BeginTrans();

                bool result = false;
                foreach (EntityPidReportMain report in Reports)
                {
                    if (!InsertNewPatient(report))
                    {
                        result = false;
                        break;
                    }

                    bool resultdetail = false;
                    foreach (EntityPidReportDetail detail in report.ListPidReportDetail)
                    {
                        if (!detailDao.InsertNewPidReportDetail(detail))
                        {
                            resultdetail = false;
                            break;
                        }
                        resultdetail = true;
                    }

                    if (!resultdetail)
                    {
                        result = false;
                        break;
                    }
                    result = true;
                }
                if (result)
                {
                    helper.CommitTrans();
                }
                else
                {
                    ErrorMsg = "保存失败！";
                    helper.RollbackTrans();
                }
                helper = null;
                return result;
            }
            catch (Exception ex)
            {
                helper?.RollbackTrans();
                ErrorMsg = "保存失败！" + ex.Message;
                Lib.LogManager.Logger.LogException(ErrorMsg, ex);
                return false;
            }
        }
        #endregion

        #region 根据实验序号获取报告单信息
        public EntityPidReportMain SearchReportByTestSeq(string TestSeq)
        {
            DBManager helper = new DBManager();// GetDbManager();
            string sql = string.Format(@"SELECT
Pat_lis_main.*,
cast(Pma_sid as bigint) as pat_sid_int,
Dict_sample.Dsam_name,
Dict_itr_instrument.Ditr_ename,
Dict_itr_instrument.Ditr_report_type,
rep_status_name = case when Pma_status = 0 then '未审核'
when Pma_status = 1 then '已审核'
when Pma_status = 2 then '已报告'
when Pma_status = 4 then '已打印'
else '未审核' end,
Dict_ident.Didt_name,
case 
when user1.Buser_name is null then Pma_audit_Buser_id
else user1.Buser_name 
end as pid_chk_name,
case 
when user2.Buser_name is null then Pma_report_Buser_id
else user2.Buser_name
end as bgName,
case 
when userRec.Buser_name is null then Pma_check_Buser_id
else userRec.Buser_name
end as lrName,
status = 0,
UrgStatus =0,--危急值数
(case when Pma_read_date is null or Pma_read_date='' then '' else CONVERT(varchar(25),Pma_read_date,120) end) as Pma_read_date
,Dict_organize.Dorg_id
,Dict_organize.Dorg_name
FROM Pat_lis_main with(nolock)
Left join Dict_sample on Pat_lis_main.Pma_Dsam_id = Dict_sample.Dsam_id
LEFT OUTER JOIN Dict_itr_instrument ON Pat_lis_main.Pma_Ditr_id = Dict_itr_instrument.Ditr_id
LEFT OUTER JOIN Dict_ident ON Dict_ident.Didt_id = Pat_lis_main.Pma_pat_Didt_id and Dict_ident.del_flag = '0'
LEFT OUTER JOIN Dict_doctor ON Dict_doctor.Ddoctor_id = Pat_lis_main.Pma_Ddoctor_id
LEFT OUTER JOIN Base_user user1 on user1.Buser_loginid = Pat_lis_main.Pma_audit_Buser_id
LEFT OUTER JOIN Base_user user2 on user2.Buser_loginid = Pat_lis_main.Pma_report_Buser_id
LEFT OUTER JOIN Base_user userRec on userRec.Buser_loginid = Pat_lis_main.Pma_check_Buser_id
LEFT OUTER JOIN Base_user AS Sys_user5 ON Pat_lis_main.Pma_read_Buser_id = Sys_user5.Buser_loginid
left join Dict_dept on Pat_lis_main.Pma_pat_dept_id = Dict_dept.Ddept_code
left join Dict_organize on Dict_dept.Ddept_Dorg_id = Dict_organize.Dorg_id
--left join pid_report_detail with(nolock) on Pid_report_main.rep_id = pid_report_detail.rep_id
--left join Dic_itm_combine on pid_report_detail.com_id = Dic_itm_combine.com_id
where Pma_sid ='{0}' AND pat_lis_main.Pma_Ditr_id IN ('100058') ", TestSeq);//Pma_serial_num

            DataTable dt = helper.ExecuteDtSql(sql);
            var listPat = EntityManager<EntityPidReportMain>.ConvertToList(dt).ToList();

            if (listPat.Count > 0)
                return listPat[0];
            else
            {
                //新增是有可能返回空行！
                //Lib.LogManager.Logger.LogInfo("返回空行", sql);
            }

            return null;
        }


        #endregion

        public bool UpdateMicReport(EntityPidReportMain Report)
        {
            bool result = false;
            try
            {
                DBManager helper = this.Dbm;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                Dictionary<string, object> dickey = new Dictionary<string, object>();
                dic.Add("Pma_micreport_date", Report.MicReportDate);
                dic.Add("Pma_micreport_chkuserid", Report.MicReportChkUserID);
                dic.Add("Pma_micreport_senduserid", Report.MicReportSendUserID);
                dic.Add("Pma_micreport_flag", 1);
                dickey.Add("Pma_rep_id", Report.RepId);
                Dbm.UpdateOperation("Pat_lis_main", dic, dickey);
                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        /// <summary>
        /// 获取申请ID
        /// </summary>
        /// <param name="RepIDs"></param>
        /// <returns></returns>
        public List<EntityDCLReportParmeter> GetDCLParmeter(List<string> RepIDs)
        {
            try
            {
                string strin = "";
                foreach (string repid in RepIDs)
                {
                    strin += string.Format("'{0}',", repid);
                }
                strin = strin.TrimEnd(',');
                DBManager helper = new DBManager();
                // 由于一个条码可能有合管然后登记不同的仪器发多份报告，而HIS，EMR都是一条医嘱一份报告
                // 如果合并多个申请ID后一份会将前一份的报告时间覆盖
                // 申请ID优先通过 pat_lis_detail 的 orderid 从 sample_detail 获取
                // 没有的话可能是登记的检验套餐被更换了(如交叉配血需要换成不规则抗体)，就从 sample_detail 中取一个申请ID出来
                string sql = string.Format(@"select 
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_bar_code,
isnull(stuff(
(select ','+Sample_detail.Sdet_apply_id  from Sample_detail 
INNER JOIN pat_lis_detail ON sample_detail.Sdet_order_sn = Pat_lis_detail.Pdet_order_sn 
AND pat_lis_detail.Pdet_id = Pat_lis_main.Pma_rep_id
WHERE Sample_detail.Sdet_bar_code=Pat_lis_main.Pma_bar_code
FOR XML PATH('')),1,1,''), (SELECT TOP 1 d.sdet_apply_id  FROM Sample_detail d WHERE  d.sdet_sma_bar_id = pat_lis_main.Pma_bar_code)) _ApplyID
from Pat_lis_main  (nolock)
where Pma_rep_id in ({0})
group by Pma_rep_id,Pma_bar_code", strin);
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDCLReportParmeter> result = EntityManager<EntityDCLReportParmeter>.ConvertToList(dt).ToList();
                return result;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDCLReportParmeter>();
            }
        }


        public bool UpdateRemarkBySeq(List<EntityObrResultTestSeqVer> Reports)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> dic = new Dictionary<string, object>();
                Dictionary<string, object> dickey = new Dictionary<string, object>();
                foreach(EntityObrResultTestSeqVer re in Reports)
                {
                    dic.Clear();
                    dickey.Clear();
                    dic.Add("Pma_comment",re.RepComment);//处理意见
                    dic.Add("Pma_remark", re.Remark);//备注
                    dickey.Add("Pma_rep_id", re.RepId);
                    helper.UpdateOperation("Pat_lis_main", dic,dickey);
                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取上传失败的报告单
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetFaultUpLoadReport(EntityPatientQC qc, string type)
        {
            string sql = string.Empty;
            if (type.Contains("粤省事"))
            {
                sql = @"select Pat_lis_main.*,
sys_interface_log.operation_content pid_res
from 
sys_interface_log(nolock)
left outer join Pat_lis_main(nolock) on Pat_lis_main.Pma_rep_id=sys_interface_log.rep_id
where operation_name='发布粤省事检验报告' and operation_success=0
and rep_id not in(
select rep_id from sys_interface_log(nolock) where operation_name='发布粤省事检验报告'
and operation_success=1
) 
AND Pat_lis_main.Pma_in_date >= '{0}'
AND Pat_lis_main.Pma_in_date <= '{1}'
";

                string sql2 = @"
UNION ALL 
select Pat_lis_main.*,
'-' pid_res
from 
Pat_lis_main(nolock)
where 1=1
and not EXISTS(
select 1 from sys_interface_log(nolock) where operation_name='发布粤省事检验报告' 
and operation_success=1
AND rep_id = Pat_lis_main.Pma_rep_id
) 
AND Pat_lis_main.Pma_pat_name LIKE '%163030%'
AND Pat_lis_main.Pma_in_date >= '{0}'
AND Pat_lis_main.Pma_in_date <= '{1}'
";

                sql = string.Format(sql, qc.DateStart?.ToString("yyyy-MM-dd HH:mm:ss"), qc.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss"));
                if (!string.IsNullOrEmpty(qc.RepBarCode))
                {
                    sql += string.Format(" and Pat_lis_main.Pma_bar_code='{0}'", qc.RepBarCode);
                    sql2 += string.Format(" and Pat_lis_main.Pma_bar_code='{0}'", qc.RepBarCode);
                }
                else if (!string.IsNullOrEmpty(qc.RepId))
                {
                    sql += string.Format(" and Pat_lis_main.Pma_rep_id='{0}'", qc.RepId);
                    sql2 += string.Format(" and Pat_lis_main.Pma_rep_id='{0}'", qc.RepId);
                }
                if (qc.ListItrId?.Count > 0 && !string.IsNullOrEmpty(qc.ListItrId[0]))
                {
                    sql += string.Format(" and Pat_lis_main.Pma_Ditr_id = '{0}' ", qc.ListItrId[0]);
                    sql2 += string.Format(" and Pat_lis_main.Pma_Ditr_id = '{0}' ", qc.ListItrId[0]);
                }
                sql2 = string.Format(sql2, qc.DateStart?.ToString("yyyy-MM-dd HH:mm:ss"), qc.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss"));
                sql += sql2;
            }
            else
            {
                sql = @"select Pat_lis_main.*,
sys_interface_log.operation_content pid_res
from 
sys_interface_log(nolock)
left outer join Pat_lis_main(nolock) on Pat_lis_main.Pma_rep_id=sys_interface_log.rep_id
where operation_name='{0}' and operation_success=0
and rep_id not in(
select rep_id from sys_interface_log where operation_name='{0}'
and operation_success=1
) ";
                if (!string.IsNullOrEmpty(type))
                {
                    sql = string.Format(sql, type);
                }
                else
                {
                    sql = string.Format(sql, "上传二审数据到中间表");
                }
                if (!string.IsNullOrEmpty(qc.RepBarCode))
                {
                    sql += string.Format(" and Pat_lis_main.Pma_bar_code='{0}'", qc.RepBarCode);
                }
                else if (!string.IsNullOrEmpty(qc.RepId))
                {
                    sql += string.Format(" and Pat_lis_main.Pma_rep_id='{0}'", qc.RepId);
                }
                else
                {
                    sql += string.Format(" and Pat_lis_main.Pma_in_date>='{0}'", qc.DateStart?.ToString("yyyy-MM-dd HH:mm:ss"));
                    sql += string.Format(" and Pat_lis_main.Pma_in_date<='{0}'", qc.DateEnd?.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (qc.ListItrId?.Count > 0 && !string.IsNullOrEmpty(qc.ListItrId[0]))
                    {
                        sql += string.Format(" and Pat_lis_main.Pma_Ditr_id = '{0}' ", qc.ListItrId[0]);
                    }
                }
            }
           
            try
            {
                Lib.LogManager.Logger.LogInfo("查询失败报告SQL：" + sql);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecSel(sql);
                return EntityManager<EntityPidReportMain>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }
    }
}
