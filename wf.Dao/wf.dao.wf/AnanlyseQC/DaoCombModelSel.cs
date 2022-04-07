using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using dcl.entity;
using System.Data;
using Lib.LogManager;
using dcl.dao.core;
using dcl.common;
using System.Configuration;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoCombModelSel))]
    public class DaoCombModelSel : IDaoCombModelSel
    {
        #region 未改动
        //无表cdr_lis_main
        public void GetCDRReportInfo(string where, DataTable source)
        {
            if (string.IsNullOrEmpty(where))
            {
                return;
            }

            string CDRCon = string.Empty;

            if (ConfigurationManager.AppSettings["CDRConnectionString"] != string.Empty)
                CDRCon = ConfigurationManager.AppSettings["CDRConnectionString"];


            string sql = @"SELECT 
                        0 isOuterReport,--是否外部报告单
                        lismain_repno as rep_id,
                        lismain_name as pid_name,
                        lismain_sample_no as rep_sid,
                        lismain_sex pid_sex,
                        lismain_age as pid_age,
                        2 rep_status,
                        '' sort_no,
                        lismain_secondaudit_time as rep_in_date,
                        lismain_instrument_id as itr_ename,
                        [dict_instrmt_report].[ins_report_code] as itr_report_id,
                        cast(0 as bit) as pat_select,
                        lismain_hospitalno as pid_in_no,
                        lismain_sample_time as samp_send_date,
                        lismain_appdept_name as pid_dept_name,
                        lismain_bedno as pid_bed_no,
                        lismain_diag as pid_diag,
                        '' as  rep_serial_num,
                        lismain_sample_name as sam_name,
                        '' as samp_remark,
                        '' as pid_remark,
                        lismain_bar_code as rep_bar_code,
                        lismain_rep_name as pid_com_name,
                        lismain_sample_time as samp_collection_date,
                        cast(null as datetime) as rep_print_date,
                        lismain_appdr_name as doc_name,
                        lismain_receive_opername as lrName,
                        lismain_firstaudit_drname as  shName,
                        lismain_secondaudit_drname bgName,
                        lismain_app_time as samp_receive_date,
                        lismain_receive_time as samp_check_date,
                        lismain_secondaudit_time as rep_audit_date,
                        lismain_firstaudit_time as rep_report_date,
                        null as rep_print_flag,
                        '' as pid_idt_id,
                        '1' itr_report_type,
                        lismain_pattype as src_name,
                        '旧检验数据'  as rep_ctype
                        FROM [cdr_lis_main]
                        left join [dict_instrmt_report] on [dict_instrmt_report].[ins_itr_mid]=[cdr_lis_main].lismain_instrument_id
                        where 1=1 {0}";
            sql = string.Format(sql, where);

            try
            {
                //DBHelper helper = new DBHelper(CDRCon);

                //DataTable dtCDRReport = helper.GetTable(sql);
                //if (dtCDRReport != null && dtCDRReport.Rows.Count > 0)
                //{
                //    source.Merge(dtCDRReport);
                //}
            }
            catch (Exception ex)
            {
                Logger.LogException("GetCDRReportInfo获取CDR报告单\r\n" + sql, ex);
                throw ex;
            }
        }

        //无表lis_report_info
        public void GetNewOuterReportInfo(string where, DataTable source, EntityAnanlyseQC query)
        {
            if (string.IsNullOrEmpty(where))
            {
                return;
            }
            string sql = @"
SELECT 
1 isOuterReport,--是否外部报告单
cast(report_info_id as varchar(50)) as 'rep_id',
pat_name as pid_name,
'' as rep_sid,
(case pat_sex when '1' then '男' when '2' then '女' when '0' then '女' else '未知' end) pid_sex,
pat_age as pid_age,
2 rep_status,
'' sort_no,
ext_second_audit_time as rep_in_date,
ext_intstrmt_name as itr_ename,
'' itr_report_id,
cast(0 as bit) as pat_select,
lis_report_info.pat_id as pid_in_no,
ext_receive_time as samp_send_date,
dept_name as pid_dept_name,
pat_bedNo as pid_bed_no,
clinical_diag as pid_diag,
'' as  rep_serial_num,
sam_name as sam_name,
sam_state as samp_remark,
'' as pid_remark,
lis_Barcode as rep_bar_code,
ext_checkItem as pid_com_name,
blood_time as samp_collection_date,
cast(null as datetime) as rep_print_date,
doctor_name as doc_name,
ext_checker as lrName,
ext_first_audit as  shName,
ext_second_audit bgName,
ext_receive_time as samp_receive_date,
ext_check_time as samp_check_date,
ext_first_audit_time as rep_audit_date,
ext_second_audit_time as rep_report_date,
null as rep_print_flag,
'' as pid_idt_id,
'' itr_report_type,
'金域报告单'  as rep_ctype,
'' rep_code

FROM lis_report_info
where 1=1 {0}

";
            sql = string.Format(sql, where);
            try
            {
                DBManager daoKM = new DBManager(ConfigurationManager.AppSettings["KingMedMidConnectionString"]);
                bool OuterReportRepStyle = query.OuterReportRepStyle == "自定义报表";
                string reportCode = query.OuterReportCode;
                string OuterReportCommonRepCode = query.OuterReportCommonRepCode;
                DataTable an = daoKM.ExecuteDtSql(sql);
                for (int i = 0; i < an.Rows.Count; i++)
                {
                    if (OuterReportRepStyle)
                    {
                        if (an.Rows[i]["rep_code"] != null
                        && an.Rows[i]["rep_code"] != DBNull.Value
                        && !string.IsNullOrEmpty(an.Rows[i]["rep_code"].ToString()))
                        {
                            an.Rows[i]["itr_report_id"] = an.Rows[i]["rep_code"].ToString();
                        }
                        else if (!string.IsNullOrEmpty(OuterReportCommonRepCode))
                        {

                            an.Rows[i]["itr_report_id"] = OuterReportCommonRepCode;
                        }
                        else
                        {
                            an.Rows[i]["itr_report_id"] = reportCode;
                        }

                    }
                    else
                    {
                        an.Rows[i]["itr_report_id"] = reportCode;

                    }
                    an.Rows[i]["sort_no"] = (i + 1).ToString();
                }
                an.Columns.Remove("rep_code");

                if (an != null && an.Rows.Count > 0)
                {

                    source.Merge(an);


                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["KMDBConStr"]))
                {
                    DBManager daoNewKM = new DBManager(ConfigurationManager.AppSettings["KMDBConStr"]);

                    DataTable an2 = daoNewKM.ExecSel(sql);
                    for (int i = 0; i < an2.Rows.Count; i++)
                    {
                        if (OuterReportRepStyle)
                        {
                            if (an2.Rows[i]["rep_code"] != null
                            && an2.Rows[i]["rep_code"] != DBNull.Value
                            && !string.IsNullOrEmpty(an2.Rows[i]["rep_code"].ToString()))
                            {
                                an2.Rows[i]["itr_report_id"] = an2.Rows[i]["rep_code"].ToString();
                            }
                            else if (!string.IsNullOrEmpty(OuterReportCommonRepCode))
                            {

                                an2.Rows[i]["itr_report_id"] = OuterReportCommonRepCode;
                            }
                            else
                            {
                                an2.Rows[i]["itr_report_id"] = reportCode;
                            }

                        }
                        else
                        {
                            an2.Rows[i]["itr_report_id"] = reportCode;

                        }
                        an2.Rows[i]["sort_no"] = (i + 1).ToString();
                    }
                    an2.Columns.Remove("rep_code");

                    if (an2 != null && an2.Rows.Count > 0)
                    {

                        source.Merge(an2);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetNewOuterReportInfo获取金域报告单\r\n" + sql, ex);
                throw ex;
            }
        }

        //无表 KM_LIS_Result
        public void GetOuterReportInfo(string where, DataTable source, EntityAnanlyseQC query)
        {
            if (string.IsNullOrEmpty(where))
            {
                return;
            }
            string sql = @"
SELECT 
1 isOuterReport,--是否外部报告单
'' rep_id,
max(F_Name) as pid_name,
'' as rep_sid,
(case max(F_Sex) when '1' then '男' when '2' then '女' when '0' then '女' else '未知' end) pid_sex,
(cast(max(F_Age) as varchar(10))+(case max(F_AgeUnit) when '0' then '岁' when '1' then '月'  when '2' then '天'  when '3' then '小时'  when '4' then '成人'   when '5' then '儿童'   when '6' then '婴儿' else '' end)) as pid_age,
2 rep_status,
'' sort_no,
max(F_RecordTime) as rep_in_date,
'' itr_ename,
'' itr_report_id,
cast(0 as bit) as pat_select,
max(F_PatientNumber) as pid_in_no,
max(F_InputDate) as samp_send_date,
max(F_DepartmentName) as pid_dept_name,
'' as pid_bed_no,
'' as pid_diag,
'' as  rep_serial_num,
'' as sam_name,
'' as samp_remark,
'' as pid_remark,
F_HospSampleID as rep_bar_code,
F_NaturalItemName as pid_com_name,
max(F_SamplingDate) as samp_collection_date,
cast(null as datetime) as rep_print_date,
'' as doc_name,
max(F_RecorderName) as lrName,
max(F_CheckerName) as  shName,
max(F_AuthorizeName) bgName,
max(F_InputDate) as samp_receive_date,
max(F_RecordTime) as samp_check_date,
max(F_CheckTime) as rep_audit_date,
max(F_AuthorizeTime) as rep_report_date,
null as rep_print_flag,
'' as pid_idt_id,
'' itr_report_type,
'金域报告单'  as rep_ctype,
rep_code

FROM KM_LIS_Result
left join OuterReport_ItrRepRef on F_MachineName=OuterReport_ItrRepRef.itr_name
where 1=1 {0}
group by F_HospSampleID,F_NaturalItemName,rep_code

";
            sql = string.Format(sql, where);
            try
            {
                DBManager daoKM = new DBManager(ConfigurationManager.AppSettings["KingMedMidConnectionString"]);
                bool OuterReportRepStyle = query.OuterReportRepStyle == "自定义报表";
                string reportCode = query.OuterReportCode;
                string OuterReportCommonRepCode = query.OuterReportCommonRepCode;
                DataTable an = daoKM.ExecSel(sql);
                for (int i = 0; i < an.Rows.Count; i++)
                {
                    if (OuterReportRepStyle)
                    {
                        if (an.Rows[i]["rep_code"] != null
                        && an.Rows[i]["rep_code"] != DBNull.Value
                        && !string.IsNullOrEmpty(an.Rows[i]["rep_code"].ToString()))
                        {
                            an.Rows[i]["itr_report_id"] = an.Rows[i]["rep_code"].ToString();
                        }
                        else if (!string.IsNullOrEmpty(OuterReportCommonRepCode))
                        {

                            an.Rows[i]["itr_report_id"] = OuterReportCommonRepCode;
                        }
                        else
                        {
                            an.Rows[i]["itr_report_id"] = reportCode;
                        }

                    }
                    else
                    {
                        an.Rows[i]["itr_report_id"] = reportCode;

                    }
                    an.Rows[i]["sort_no"] = (i + 1).ToString();
                }
                an.Columns.Remove("rep_code");

                if (an != null && an.Rows.Count > 0)
                {

                    source.Merge(an);


                }

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["KMDBConStr"]))
                {
                    DBManager daoNewKM = new DBManager(ConfigurationManager.AppSettings["KMDBConStr"]);

                    DataTable an2 = daoNewKM.ExecSel(sql);
                    for (int i = 0; i < an2.Rows.Count; i++)
                    {
                        if (OuterReportRepStyle)
                        {
                            if (an2.Rows[i]["rep_code"] != null
                            && an2.Rows[i]["rep_code"] != DBNull.Value
                            && !string.IsNullOrEmpty(an2.Rows[i]["rep_code"].ToString()))
                            {
                                an2.Rows[i]["itr_report_id"] = an2.Rows[i]["rep_code"].ToString();
                            }
                            else if (!string.IsNullOrEmpty(OuterReportCommonRepCode))
                            {

                                an2.Rows[i]["itr_report_id"] = OuterReportCommonRepCode;
                            }
                            else
                            {
                                an2.Rows[i]["itr_report_id"] = reportCode;
                            }

                        }
                        else
                        {
                            an2.Rows[i]["itr_report_id"] = reportCode;

                        }
                        an2.Rows[i]["sort_no"] = (i + 1).ToString();
                    }
                    an2.Columns.Remove("rep_code");

                    if (an2 != null && an2.Rows.Count > 0)
                    {

                        source.Merge(an2);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.LogException("GetOuterReportInfo获取金域报告单\r\n" + sql, ex);
                throw ex;
            }
        }


        public EntityQcResultList GetPatientResultData(EntityAnanlyseQC query, DateTime patDate)
        {
            EntityQcResultList listResult = new EntityQcResultList();
            try
            {
                DateTime? startDate = null;
                if (patDate != null)
                {
                    startDate = patDate;
                }
                DBManager helper = GetSqlHelper(startDate, query);

                if (!string.IsNullOrEmpty(query.PatId))
                {
                    string rep_id = query.PatId;

                    #region 旧检验数据
                    if (!string.IsNullOrEmpty(query.PatCType) && query.PatCType == "旧检验数据")
                    {
                        string CDRCon = string.Empty;

                        if (ConfigurationManager.AppSettings["CDRConnectionString"] != string.Empty)
                            CDRCon = ConfigurationManager.AppSettings["CDRConnectionString"];

                        DBManager CDRhelper = new DBManager(CDRCon);

                        string sqlRep = string.Format("select lismain_reptype from cdr_lis_main where lismain_repno='{0}'", rep_id);
                        DataTable dtRep = helper.ExecuteDtSql(sqlRep);
                        if (dtRep != null && dtRep.Rows.Count > 0)
                        {
                            string repType = dtRep.Rows[0]["lismain_reptype"].ToString();
                            if (repType == "普通")
                            {
                                string sqlSelect = string.Format(@"
                                        SELECT 
                                        LTRIM(rtrim(lisresult_item_ename))  itm_name,
                                        lisresult_repno res_key,
                                        LTRIM(rtrim(lisresult_item_alias)) res_itm_ecd,
                                        llisresult_result res_chr,
                                        lisresult_od_result res_od_chr,
                                        (case lisresult_prompt when 'N' then '' WHEN 'H' THEN '↑' when 'L' then '↓' ELSE '' end) res_prompt,
                                        lisresult_unit res_unit,
                                        lisresult_ref_range res_ref,
                                        '' res_meams ,
                                        '' res_ref_exp,
                                        '' res_ref_flag,
                                        '' com_seq,
                                        lisresult_seq  com_sort

                                        FROM cdr_lis_result AS clr WHERE lisresult_repno='{0}'
                                        ORDER BY lisresult_seq
                                        ", rep_id);

                                DataTable dtResu = CDRhelper.ExecuteDtSql(sqlSelect);
                                dtResu.TableName = "resulto";
                                listResult.listResulto = EntityManager<EntityObrResult>.ConvertToList(dtResu);
                            }
                            else if (repType == "描述")
                            {
                                string sqlCs = "select llisresult_result as bsr_cname,'' bsr_i_flag from cdr_lis_result where lisresult_repno='" + rep_id + "'";

                                DataTable dtCs = CDRhelper.ExecuteDtSql(sqlCs);
                                dtCs.TableName = "cs_rlts";
                                listResult.listDesc = EntityManager<EntityObrResultDesc>.ConvertToList(dtCs);
                            }
                            else
                            {
                                string sqlBa = string.Format(@"
                                        SELECT 
                                        clr.lisresult_item_cname bac_cname,
                                        '' bar_bcnt,
                                        '' bar_scripe
                                        FROM cdr_lis_result AS clr WHERE lisresult_repno='{0}'
                                        GROUP BY clr.lisresult_item_cname", rep_id);

                                string sqlAn = string.Format(@"
                                        SELECT
                                        clr.lisresult_item_cname lisresult_item_ename,
                                        lisresult_ant_ename anti_code,
                                        lisresult_ant_cname anti_cname,
                                        lisresult_od_result anr_mic,
                                        llisresult_result anr_smic1,
                                        '' ss_hstd,
                                        '' ss_mstd,
                                        '' ss_lstd,
                                        '' anr_row_flag
                                        FROM cdr_lis_result AS clr WHERE lisresult_repno='{0} '
                                        ORDER BY lisresult_seq", rep_id);
                                DataTable dtBa = CDRhelper.ExecuteDtSql(sqlBa);
                                dtBa.TableName = "ba_rlts";
                                listResult.listBact = EntityManager<EntityObrResultBact>.ConvertToList(dtBa);
                                DataTable dtAn = CDRhelper.ExecuteDtSql(sqlAn);
                                dtAn.TableName = "an_rlts";
                                listResult.listAnti = EntityManager<EntityObrResultAnti>.ConvertToList(dtAn);
                            }
                        }
                        return listResult;
                    }
                    #endregion


                    else
                    {
                        string sqlRep = string.Format(@"select Dict_itr_instrument.Ditr_report_id,Dict_itr_instrument.Ditr_report_type from Pat_lis_main left join     
Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
where Pat_lis_main.Pma_rep_id='{0}'", rep_id);
                        DataTable dtRep = helper.ExecuteDtSql(sqlRep);
                        listResult.listPatients = EntityManager<EntityPidReportMain>.ConvertToList(dtRep);
                    }
                    return listResult;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
            return listResult;
        }


        #endregion

        public List<EntityPidReportMain> GetPatientsList(EntityAnanlyseQC query, string dateStart, string dateEnd)
        {
            DataSet result = new DataSet();
            try
            {
                DBManager helper = new DBManager();

                List<string> strWhere = GetWhere(query);

                DataTable where = new DataTable();

                DateTime? startDate = null;
                DateTime? endDate = null;

                if (dateStart != null)
                    startDate = Convert.ToDateTime(dateStart);
                if (dateEnd != null)
                    endDate = Convert.ToDateTime(dateEnd);


                string subSql = " ,null as Dcom_Dpro_id ";

                if (query.CombineFlag == "1")
                {
                    subSql = ",(select TOP 1 dc.Dcom_Dpro_id from Pat_lis_detail pm left join Dict_itm_combine dc on dc.Dcom_id=pm.Pdet_Dcom_id where pm.Pdet_id=Pat_lis_main.Pma_rep_id and dc.Dcom_Dpro_id is not null) as Dcom_Dpro_id ";
                }
                string code = strWhere[0];
                string sql = string.Format(@"select 
0 isOuterReport,
Pma_rep_id,
Pma_pat_name,
Pma_sid,
Pma_pat_sex,
dbo.getAge(Pat_lis_main.Pma_pat_age_exp) Pma_pat_age_exp,
Pma_pat_age,
Pma_status,
'' sort_no,
Pma_in_date,
Dict_itr_instrument.Ditr_ename,
isnull(Dict_itr_instrument.Ditr_report_id,'') Ditr_report_id,
(case Pat_lis_main.Pma_status when '2' then cast(1 as bit) else cast(0 as bit) end) as pat_select,
Pma_pat_in_no,
Pma_sam_send_date,
Pma_pat_dept_name,
Pma_pat_bed_no,
Pma_pat_diag,
(case isnull(Pat_lis_main.Pma_serial_num,'0') when '0' then Pat_lis_main.Pma_sid else Pat_lis_main.Pma_serial_num end ) Pma_serial_num,
Dsam_name,
Pma_sam_remark,
Pma_pat_work,
Pma_pat_tel,
Pma_pat_email,
Pma_pat_unit,
Pma_pat_address,
Pma_pat_pre_week,
Pma_pat_height,
Pma_pat_weight,
Pma_Dsam_id,
Pma_Dpurp_id,
Pma_Ddoctor_id,
case
when Pma_status=0 and Pma_micreport_flag=1 then Pma_micreport_senduserid
else Pma_check_Buser_id
end Pma_check_Buser_id,
case
when Pma_status=0 and Pma_micreport_flag=1 then Pma_micreport_chkuserid
else Pma_audit_Buser_id
end Pma_audit_Buser_id,
Pma_check_Buser_id,
Pma_audit_Buser_id,
Pma_social_no,
Pma_apply_date,
Pma_collection_part,
Pma_pat_Dsorc_id,
Pma_itr_analysis,
Pma_pat_remark,
Pma_bar_code,
Pma_com_name,
Pma_collection_date,
Pma_micreport_flag,
Pma_print_date,
Pat_lis_main.Pma_unique_id,
Dict_doctor.Ddoctor_name,
Pat_lis_main.Pma_doctor_name,
Pat_lis_main.Pma_remark,--备注
Base_user.Buser_name lrName,
case 
when Pma_status=0 and Pma_micreport_flag=1 and sys_power_user7.Buser_name is null then Pma_micreport_chkuserid
when Pma_status=0 and Pma_micreport_flag=1 and sys_power_user7.Buser_name is not null then sys_power_user7.Buser_name
when Pma_status!=0 and sys_power_user2.Buser_name is null then Pma_audit_Buser_id
else sys_power_user2.Buser_name 
end as shName,
case 
when Pma_status=0 and Pma_micreport_flag=1 and sys_power_user6.Buser_name is null then Pma_micreport_senduserid
when Pma_status=0 and Pma_micreport_flag=1 and sys_power_user6.Buser_name is not null then sys_power_user6.Buser_name
when Pma_status!=0 and sys_power_user3.Buser_name is null then Pma_check_Buser_id
else sys_power_user3.Buser_name
end as bgName,
Pat_lis_main.Pma_sam_receive_date,
Pat_lis_main.Pma_check_date,
Pat_lis_main.Pma_audit_date,
Pat_lis_main.Pma_report_date,
Pat_lis_main.Pma_print_flag,
Pat_lis_main.Pma_Ditr_id,
Pat_lis_main.Pma_pat_dept_id,
Pat_lis_main.Pma_status,
Pat_lis_main.Pma_input_id,
Pat_lis_main.Pma_urgent_flag,--危急值标记
Pat_lis_main.Pma_temp_flag,--中期报告标记
Pma_pat_Didt_id,
Pma_comment,
Pma_discribe,
Dict_itr_instrument.Ditr_report_type,
Dict_source.Dsorc_name,
case when Pma_ctype = '2' then '急查'
	 when Pma_ctype = '3' then '保密'
     when Pma_ctype = '4' then '溶栓'
     else '常规' end as Pma_ctype,sys_power_user5.Buser_name  as pat_look_name,Pma_read_date,isnull(Pma_read_Buser_id,'') as Pma_read_Buser_id   {0}
from Pat_lis_main with(nolock) 
left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id 
left join Dict_dept on Pat_lis_main.Pma_pat_dept_id=Dict_dept.Ddept_id and Dict_dept.del_flag='0'
LEFT OUTER JOIN Dict_doctor ON Pat_lis_main.Pma_Ddoctor_id = Dict_doctor.Ddoctor_id
LEFT OUTER JOIN Dict_sample ON Pat_lis_main.Pma_Dsam_id = Dict_sample.Dsam_id
LEFT OUTER JOIN Dict_source ON Pat_lis_main.Pma_pat_Dsorc_id = Dict_source.Dsorc_id
LEFT OUTER JOIN Base_user ON Pat_lis_main.Pma_check_Buser_id = Base_user.Buser_loginid 
LEFT OUTER JOIN Base_user AS sys_power_user2 on Pat_lis_main.Pma_audit_Buser_id=sys_power_user2.Buser_loginid 
LEFT OUTER JOIN Base_user AS sys_power_user3 on Pat_lis_main.Pma_report_Buser_id=sys_power_user3.Buser_loginid 
LEFT OUTER JOIN Base_user AS sys_power_user4 ON Dict_itr_instrument.Ditr_report_chk_id = sys_power_user4.Buser_loginid
LEFT OUTER JOIN Base_user AS sys_power_user5 ON Pat_lis_main.Pma_read_Buser_id =sys_power_user5.Buser_loginid
LEFT OUTER JOIN Base_user AS sys_power_user6 ON Pat_lis_main.Pma_micreport_senduserid =sys_power_user6.Buser_loginid
LEFT OUTER JOIN Base_user AS sys_power_user7 ON Pat_lis_main.Pma_micreport_chkuserid =sys_power_user7.Buser_loginid
where 1=1  " + code, subSql);

                DataTable an = new DataTable();
                an = helper.ExecuteDtSql(sql);
                //Lib.LogManager.Logger.LogInfo("sql:" + sql);
                an = OrderBy(an, "Pma_pat_name,Pma_com_name,Pma_ctype,Pma_in_date desc");
                an.Columns["sort_no"].ReadOnly = false;
                an.Columns["sort_no"].MaxLength = 6;

                for (int i = 0; i < an.Rows.Count; i++)
                {
                    an.Rows[i]["sort_no"] = (i + 1).ToString();
                }

                an.TableName = "patients";

                if (query.CanSearchOuterReport)//外部报告单
                {
                    if (query.SearchOuterInterfaceMode)
                    {
                        GetOuterReportInfo(strWhere[1], an, query);
                    }
                    else
                    {
                        GetNewOuterReportInfo(strWhere[1], an, query);

                    }
                }

                if (query.SearchCDR)//CDR报告单
                {
                    GetCDRReportInfo(strWhere[2], an);
                }

                if (!an.Columns.Contains("pat_sid_int"))
                {
                    an.Columns.Add("pat_sid_int", typeof(double));
                }

                if (!an.Columns.Contains("rep_serial_num_int"))
                {
                    an.Columns.Add("rep_serial_num_int", typeof(double));
                }
                DataTable lisHistoryReprotPat = GetPatList(startDate, endDate, sql);

                if (lisHistoryReprotPat != null && lisHistoryReprotPat.Rows.Count > 0)
                {

                    an.Merge(lisHistoryReprotPat);


                }





                bool Report_CheckNotPrintReport = query.ReportCheckNotPrintReport != "否";
                foreach (DataRow item in an.Rows)
                {
                    if (item["Pma_sid"] != null
                        && item["Pma_sid"] != DBNull.Value
                       && !string.IsNullOrEmpty(item["Pma_sid"].ToString())
                        )
                    {
                        string sid = item["Pma_sid"].ToString();
                        double pat_sid_int = 0;
                        if (double.TryParse(sid, out pat_sid_int))
                        {
                            item["pat_sid_int"] = pat_sid_int;
                        }

                    }
                    if (item["Pma_serial_num"] != null
                      && item["Pma_serial_num"] != DBNull.Value
                     && !string.IsNullOrEmpty(item["Pma_serial_num"].ToString())
                      )
                    {
                        string num = item["Pma_serial_num"].ToString();
                        double rep_serial_num_int = 0;
                        if (double.TryParse(num, out rep_serial_num_int))
                        {
                            item["rep_serial_num_int"] = rep_serial_num_int;
                        }
                    }

                    if (!Report_CheckNotPrintReport)
                    {
                        item["pat_select"] = false;
                    }
                }
                //List<EntityPatients> listPatients = EntityManager<EntityPatients>.ConvertToList(an).ToList();

                List<EntityPidReportMain> listPatients = new List<EntityPidReportMain>();
                foreach (DataRow drPat in an.Rows)
                {
                    EntityPidReportMain entity = new EntityPidReportMain();
                    entity.IsOuterReport = drPat["isOuterReport"].ToString();
                    entity.RepId = drPat["Pma_rep_id"].ToString();
                    entity.PidName = drPat["Pma_pat_name"].ToString();
                    entity.RepSid = drPat["Pma_sid"].ToString();
                    entity.PidSex = drPat["Pma_pat_sex"].ToString();
                    entity.PidAgeExp = drPat["Pma_pat_age_exp"].ToString();

                    if (drPat["Pma_pat_age"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_pat_age"].ToString()))
                    {
                        entity.PidAge = Convert.ToDecimal(drPat["Pma_pat_age"].ToString());
                    }
                    if (drPat["Pma_status"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_status"].ToString()))
                    {
                        entity.RepStatus = Convert.ToInt32(drPat["Pma_status"].ToString());
                    }
                    if (drPat["pat_sid_int"] != DBNull.Value && !string.IsNullOrEmpty(drPat["pat_sid_int"].ToString()))
                    {
                        entity.PatSidInt = Convert.ToInt64(drPat["pat_sid_int"].ToString());
                    }
                    if (drPat["rep_serial_num_int"] != DBNull.Value && !string.IsNullOrEmpty(drPat["rep_serial_num_int"].ToString()))
                    {
                        entity.RepSerialNumInt = Convert.ToInt64(drPat["rep_serial_num_int"].ToString());
                    }
                    entity.PatSort = drPat["sort_no"].ToString();

                    if (drPat["Pma_in_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_in_date"].ToString()))
                    {
                        entity.RepInDate = Convert.ToDateTime(drPat["Pma_in_date"].ToString());
                    }
                    entity.ItrEname = drPat["Ditr_ename"].ToString();
                    entity.ItrReportId = drPat["Ditr_report_id"].ToString();

                    if (!string.IsNullOrEmpty(drPat["pat_select"].ToString()) && drPat["pat_select"].ToString() == "1")
                        entity.PatSelect = true;
                    else
                        entity.PatSelect = false;

                    entity.PidInNo = drPat["Pma_pat_in_no"].ToString();

                    if (drPat["Pma_sam_send_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_sam_send_date"].ToString()))
                    {
                        entity.SampSendDate = Convert.ToDateTime(drPat["Pma_sam_send_date"].ToString());
                    }
                    entity.PidDeptName = drPat["Pma_pat_dept_name"].ToString();
                    entity.PidBedNo = drPat["Pma_pat_bed_no"].ToString();
                    entity.PidDiag = drPat["Pma_pat_diag"].ToString();
                    entity.RepSerialNum = drPat["Pma_serial_num"].ToString();
                    entity.SamName = drPat["Dsam_name"].ToString();
                    entity.SampRemark = drPat["Pma_sam_remark"].ToString();
                    entity.PidWork = drPat["Pma_pat_work"].ToString();
                    entity.PidTel = drPat["Pma_pat_tel"].ToString();
                    entity.PidEmail = drPat["Pma_pat_email"].ToString();
                    entity.PidUnit = drPat["Pma_pat_unit"].ToString();
                    entity.PidAddress = drPat["Pma_pat_address"].ToString();
                    entity.PidPreWeek = drPat["Pma_pat_pre_week"].ToString();
                    entity.PidHeight = drPat["Pma_pat_height"].ToString();
                    entity.PidWeight = drPat["Pma_pat_weight"].ToString();
                    entity.PidSamId = drPat["Pma_Dsam_id"].ToString();
                    entity.PidPurpId = drPat["Pma_Dpurp_id"].ToString();
                    entity.PidDoctorCode = drPat["Pma_Ddoctor_id"].ToString();
                    entity.RepCheckUserId = drPat["Pma_check_Buser_id"].ToString();
                    entity.RepAuditUserId = drPat["Pma_audit_Buser_id"].ToString();
                    entity.PidSocialNo = drPat["Pma_social_no"].ToString();

                    if (drPat["Pma_apply_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_apply_date"].ToString()))
                    {
                        entity.SampApplyDate = Convert.ToDateTime(drPat["Pma_apply_date"].ToString());
                    }
                    entity.CollectionPart = drPat["Pma_collection_part"].ToString();
                    entity.PidSrcId = drPat["Pma_pat_Dsorc_id"].ToString();
                    entity.RepItrAnalysis = drPat["Pma_itr_analysis"].ToString();
                    entity.PidRemark = drPat["Pma_pat_remark"].ToString();
                    entity.RepBarCode = drPat["Pma_bar_code"].ToString();
                    entity.PidComName = drPat["Pma_com_name"].ToString();

                    if (drPat["Pma_collection_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_collection_date"].ToString()))
                    {
                        entity.SampCollectionDate = Convert.ToDateTime(drPat["Pma_collection_date"].ToString());
                    }
                    if (drPat["Pma_print_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_print_date"].ToString()))
                    {
                        entity.RepPrintDate = Convert.ToDateTime(drPat["Pma_print_date"].ToString());
                    }
                    entity.PidUniqueId = drPat["Pma_unique_id"].ToString();
                    entity.DoctorName = drPat["Ddoctor_name"].ToString();
                    entity.PidDocName = drPat["Pma_doctor_name"].ToString();
                    entity.RepRemark = drPat["Pma_remark"].ToString();
                    entity.LrName = drPat["lrName"].ToString();
                    entity.ShName = drPat["shName"].ToString();
                    entity.BgName = drPat["bgName"].ToString();

                    if (drPat["Pma_sam_receive_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_sam_receive_date"].ToString()))
                    {
                        entity.SampReceiveDate = Convert.ToDateTime(drPat["Pma_sam_receive_date"].ToString());
                    }
                    if (drPat["Pma_check_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_check_date"].ToString()))
                    {
                        entity.SampCheckDate = Convert.ToDateTime(drPat["Pma_check_date"].ToString());
                    }
                    if (drPat["Pma_audit_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_audit_date"].ToString()))
                    {
                        entity.RepAuditDate = Convert.ToDateTime(drPat["Pma_audit_date"].ToString());
                    }
                    if (drPat["Pma_report_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_report_date"].ToString()))
                    {
                        entity.RepReportDate = Convert.ToDateTime(drPat["Pma_report_date"].ToString());
                    }
                    if (drPat["Pma_print_flag"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_print_flag"].ToString()))
                    {
                        entity.RepPrintFlag = Convert.ToInt32(drPat["Pma_print_flag"].ToString());
                    }
                    entity.RepItrId = drPat["Pma_Ditr_id"].ToString();
                    entity.PidDeptId = drPat["Pma_pat_dept_id"].ToString();

                    if (drPat["Pma_status"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_status"].ToString()))
                    {
                        entity.RepStatus = Convert.ToInt32(drPat["Pma_status"].ToString());
                    }
                    entity.RepInputId = drPat["Pma_input_id"].ToString();

                    if (drPat["Pma_urgent_flag"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_urgent_flag"].ToString()))
                    {
                        entity.RepUrgentFlag = Convert.ToInt32(drPat["Pma_urgent_flag"].ToString());
                    }
                    entity.RepTempFlag = drPat["Pma_temp_flag"].ToString();
                    entity.PidIdtId = drPat["Pma_pat_Didt_id"].ToString();
                    entity.RepComment = drPat["Pma_comment"].ToString();
                    entity.ItrReportType = drPat["Ditr_report_type"].ToString();
                    entity.SrcName = drPat["Dsorc_name"].ToString();
                    entity.RepCtype = drPat["Pma_ctype"].ToString();
                    entity.PatLookName = drPat["pat_look_name"].ToString();

                    if (drPat["Pma_read_date"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_read_date"].ToString()))
                    {
                        entity.RepReadDate = Convert.ToDateTime(drPat["Pma_read_date"].ToString());
                    }
                    entity.RepReadUserId = drPat["Pma_read_Buser_id"].ToString();
                    entity.ComPriId = drPat["Dcom_Dpro_id"].ToString();

                    if (drPat.Table.Columns.Contains("Pma_discribe") && drPat["Pma_discribe"] != DBNull.Value && !string.IsNullOrEmpty(drPat["Pma_discribe"].ToString()))
                    {
                        entity.RepDiscribe = drPat["Pma_discribe"].ToString();
                    }
                    int value;
                    if (int.TryParse(drPat["Pma_micreport_flag"].ToString(), out value))
                    {
                        entity.MicReportFlag = value;
                    }
                    listPatients.Add(entity);
                }

                return listPatients;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
        }

        public List<string> GetWhere(EntityAnanlyseQC query)
        {
            List<string> listWhere = new List<string>();
            string where = string.Empty;
            string outerRepWhere = string.Empty;
            string CDRWhere = string.Empty;

            if (query.IsReturn)
                return listWhere;

            if (query.EnbState)//是否住院调用
            {
                if (query.EnableMutliPatQuery && query.IsAuto)
                    where += " and (Pat_lis_main.Pma_status in (2,4) or Pat_lis_main.Pma_temp_flag=1 or (Pat_lis_main.Pma_status=0 and Pma_micreport_flag=1))";
                if (query.IsFilterSpecPat)
                    where += " and (Pat_lis_main.Pma_identity  not  in (1,10,12,20,30,40) or Pat_lis_main.Pma_identity is null )";
            }
            else
            {
                if (query.IsCanLookNotAuditReport || query.IsSingleSearch)
                    where += " and Pat_lis_main.Pma_status in ('0','1','2','4')";
                else
                    where += " and (Pat_lis_main.Pma_status in (2,4) or Pat_lis_main.Pma_temp_flag=1 or (Pat_lis_main.Pma_status=0 and Pma_micreport_flag=1))";
                if (query.IsFilterSpecPat && !query.IsNotOutlink)
                    where += " and (Pat_lis_main.Pma_identity  not  in (1,10,12,20,30,40) or Pat_lis_main.Pma_identity is null)";
                else
                {
                    if (query.IsGGALL)
                    {
                        string con = string.Empty;
                        List<string> list = new List<string>();
                        list.Add("10");
                        list.Add("12");
                        list.Add("20");
                        list.Add("30");
                        list.Add("40");

                        if (query.GGBJDX)
                            list.Remove("10");
                        if (query.GGGWU)
                            list.Remove("12");
                        if (query.GGTD)
                            list.Remove("20");
                        if (query.GGGR)
                            list.Remove("30");
                        if (query.GGGB)
                            list.Remove("40");

                        if (list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (i == list.Count - 1)
                                {
                                    con += list[i];
                                }
                                else
                                {
                                    con += list[i] + ",";

                                }
                            }
                            where += string.Format(" and (Pat_lis_main.Pma_identity  not  in ({0}) or Pat_lis_main.Pma_identity is null )", con);

                        }

                    }
                }
                if (!string.IsNullOrEmpty(query.listItrId))
                    where += string.Format(" and (Pat_lis_main.Pma_Ditr_id in ({0}))", query.listItrId);
            }

            if (!string.IsNullOrEmpty(query.PatId))
                where += " and Pat_lis_main.Pma_rep_id='" + query.PatId + "'";

            if (!string.IsNullOrEmpty(query.PidCheckUserId))
                where += " and Pat_lis_main.Pma_check_Buser_id='" + query.PidCheckUserId + "'";

            if (!string.IsNullOrEmpty(query.PidAuditUserId))
                where += " and Pat_lis_main.Pma_audit_Buser_id='" + query.PidAuditUserId + "'";

            if (string.IsNullOrEmpty(query.SerialNumber))
            {
                query.StrSelectTime = "Pat_lis_main.Pma_in_date";//强行改变从配置信息查出来的字符串patients.pat_date

                if (query.DateType == "报告日期")
                {
                    query.StrSelectTime = "Pat_lis_main.Pma_report_date";
                }
                else if (query.DateType == "签收日期")
                {
                    query.StrSelectTime = "Pat_lis_main.Pma_apply_date";
                }

                if (query.DateStart != null)
                {

                    where += string.Format(" and {0}>= '{1}'", query.StrSelectTime, query.DateStart.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (query.SearchOuterInterfaceMode)
                    {
                        outerRepWhere += string.Format(" and {0}>= '{1}'", "KM_LIS_Result.F_AuthorizeTime", query.DateStart.ToString("yyyy-MM-dd HH:mm:ss"));//外部报告单查询
                    }
                    else
                    {
                        outerRepWhere += string.Format(" and {0}>= '{1}'", "lis_report_info.ext_second_audit_time", query.DateStart.ToString("yyyy-MM-dd HH:mm:ss"));//外部报告单查询
                    }
                    if (query.SearchCDR)
                    {
                        CDRWhere += string.Format(" and {0}>= '{1}'", "cdr_lis_main.lismain_receive_time", query.DateStart.ToString("yyyy-MM-dd HH:mm:ss"));//外部报告单查询
                    }
                }
                if (query.DateEnd != null)
                {
                    where += string.Format(" and {0}<= '{1}'", query.StrSelectTime, query.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (query.SearchOuterInterfaceMode)
                    {
                        outerRepWhere += string.Format(" and {0}<= '{1}'", "KM_LIS_Result.F_AuthorizeTime", query.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"));//外部报告单查询
                    }
                    else
                    {
                        outerRepWhere += string.Format(" and {0}<= '{1}'", "lis_report_info.ext_second_audit_time", query.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"));//外部报告单查询

                    }
                    if (query.SearchCDR)
                    {
                        CDRWhere += string.Format(" and {0}<= '{1}'", "cdr_lis_main.lismain_receive_time", query.DateEnd.ToString("yyyy-MM-dd HH:mm:ss"));//外部报告单查询
                    }
                }
            }
            else
            {
                if (query.PatSampleReceiveDate != null)
                {
                    string strSerialNumberWhere = string.Format(" and {0}>='{1}' and {0}<'{2}'", "Pma_sam_receive_date", query.PatSampleReceiveDate.Value.Date, query.PatSampleReceiveDate.Value.Date.AddDays(1));
                    outerRepWhere += strSerialNumberWhere;
                    where += strSerialNumberWhere;
                    where += " and Pat_lis_main.Pma_pat_exam_company = '" + query.SerialNumber + "'";
                }
            }

            //申请医生姓名
            if (!string.IsNullOrEmpty(query.PatDocName))
            {
                where += string.Format(" and Pma_doctor_name like '{0}%'", query.PatDocName);
                if (query.SearchCDR)
                {
                    CDRWhere += string.Format(" and cdr_lis_main.lismain_appdr_name like '{0}%'", query.PatDocName);
                }
            }

            //体检单位
            if (!string.IsNullOrEmpty(query.PatEmpCompanyName))
            {
                where += " and Pat_lis_main.Pma_pat_exam_company like '" + query.PatEmpCompanyName + "%'";
            }

            //组合
            if (!string.IsNullOrEmpty(query.PatCName))
            {
                //where += " and Pat_lis_main.Pma_com_name like '%" + query.PatCName + "%' ";
                if (query.SearchOuterInterfaceMode)
                {
                    outerRepWhere += "and KM_LIS_Result.F_NaturalItemName like '%" + query.PatCName + "%'";//外部报告单查询
                }
                else
                {
                    outerRepWhere += "and lis_report_info.ext_checkItem like '%" + query.PatCName + "%'";//外部报告单查询

                }
                if (query.SearchCDR)
                {
                    CDRWhere += "and cdr_lis_main.lismain_rep_name like '%" + query.PatCName + "%'";
                }
            }
            //组合id
            if (!string.IsNullOrEmpty(query.PatComId))
            {
                where += string.Format("and exists (select top 1 1 from Pat_lis_detail where Pdet_Dcom_id = '{0}' and Pat_lis_detail.Pdet_id = Pat_lis_main.Pma_rep_id )", query.PatComId);
            }
            //样本号
            if (query.listSid.Count > 0)
            {
                EntitySid sid = query.listSid[0];
                if (sid.StartSid > 0)
                {
                    where += " and cast(Pat_lis_main.Pma_sid as bigint)>=" + sid.StartSid;
                    outerRepWhere += " and 1=2";//外部报告单查询,若为样本号,则不查询
                    if (query.SearchCDR)
                    {
                        CDRWhere += " and cast(cdr_lis_main.lismain_sample_no as bigint)>=" + sid.StartSid;
                    }
                }
                if (sid.EndSid != null && sid.EndSid.Value > 0)
                {
                    where += " and cast(Pat_lis_main.Pma_sid as bigint)<=" + sid.EndSid.Value;
                    outerRepWhere += " and 1=2";//外部报告单查询,若为样本号,则不查询
                    if (query.SearchCDR)
                    {
                        CDRWhere += " and cast(cdr_lis_main.lismain_sample_no as bigint)<=" + sid.EndSid.Value;
                    }
                }
            }


            //流水号
            if (query.listSort.Count > 0)
            {
                EntitySortNo sortNo = query.listSort[0];
                if (sortNo.StartNo > 0)
                {
                    where += " and cast(Pat_lis_main.Pma_serial_num as bigint)>=" + sortNo.StartNo;
                    outerRepWhere += " and 1=2";//外部报告单查询,若为序号,则不查询
                }
                if (sortNo.EndNo != null && sortNo.EndNo.Value > 0)
                {
                    where += " and cast(Pat_lis_main.Pma_serial_num as bigint)<=" + sortNo.EndNo.Value;
                    outerRepWhere += " and 1=2";//外部报告单查询,若为序号,则不查询
                }
            }


            //病人名字
            if (!string.IsNullOrEmpty(query.PatName))
            {
                //全模糊
                if (query.matchType == MatchType.QUANMOHU)
                {
                    where += " and Pat_lis_main.Pma_pat_name like '%" + query.PatName + "%'";
                }
                //半模糊
                if (query.matchType == MatchType.BANMOHU)
                {
                    where += " and Pat_lis_main.Pma_pat_name like '" + query.PatName + "%'";
                }
                //全匹配
                if (query.matchType == MatchType.QUANPIPEI)
                {
                    where += " and Pat_lis_main.Pma_pat_name = '" + query.PatName + "'";
                }
                if (query.SearchOuterInterfaceMode)
                {
                    outerRepWhere += " and KM_LIS_Result.F_Name like '%" + query.PatName + "%'";//外部报告单查询
                }
                else
                {
                    outerRepWhere += " and lis_report_info.pat_name like '%" + query.PatName + "%'";//外部报告单查询

                }
                if (query.SearchCDR)
                {
                    CDRWhere += " and cdr_lis_main.lismain_name like '%" + query.PatName + "%'";
                }
            }

            //科别
            if (!string.IsNullOrEmpty(query.DepIdFilter))
            {
                if (query.TxtInNo == string.Empty)
                {
                    if (query.DepId != null && query.PatDepName != null && query.PatWardId != null)
                    {
                        where += string.Format(" and (Pat_lis_main.Pma_pat_dept_id = '{0}' or Pat_lis_main.Pma_pat_ward_id='{0}' or Pat_lis_main.Pma_pat_dept_name='{1}') "
                            , query.DepId
                            , query.PatDepName
                            );
                    }
                    else if (!string.IsNullOrEmpty(query.PatDepName))
                    {
                        where += string.Format(" and (Pat_lis_main.Pma_pat_dept_name like '{0}%') ", query.PatDepName);
                    }
                    if (query.SearchOuterInterfaceMode)
                    {
                        outerRepWhere += " and KM_OutSourceDetail.F_SectionOffice='" + query.PatDepName + "'";
                    }
                    else
                    {
                        outerRepWhere += " and lis_report_info.dept_name='" + query.PatDepName + "'";
                    }
                }

                if (!query.EnableMutliPatQuery || query.IsAuto)
                    where += " and Pma_status not in ('0','1')";
            }
            else
            {
                if (query.DepId != null && query.PatDepName != null && query.PatWardId != null)
                {
                    where += string.Format(" and (Pat_lis_main.Pma_pat_dept_id = '{0}' or Pat_lis_main.Pma_pat_ward_id='{0}' or Pat_lis_main.Pma_pat_dept_name='{1}') "
                        , query.DepId
                        , query.PatDepName
                        );
                }
                else if (!string.IsNullOrEmpty(query.PatDepName) && query.PatDepName.Length > 0)
                {
                    where += string.Format(" and (Pat_lis_main.Pma_pat_dept_name like '{0}%') ", query.PatDepName);
                }
                if (query.SearchCDR)
                {
                    CDRWhere += string.Format(" and (cdr_lis_main.lismain_appdept_name like '%{0}%') ", query.PatDepName);
                }
            }

            //ID类型
            if (!string.IsNullOrEmpty(query.PatNoId))
            {
                where += "and Pat_lis_main.Pma_pat_Didt_id='" + query.PatNoId + "'";
            }

            //病人ID
            if (!string.IsNullOrEmpty(query.TxtInNo))
            {
                if (query.StrCisYzId != null)
                {
                    where += string.Format(" and (Pat_lis_main.Pma_bar_code in (select bc_bar_code from bc_cname where bc_yz_id='{0}' and bc_del='0')) ", query.StrCisYzId);
                }
                if (query.TxtInNoSql != null)
                {
                    if (query.EnbState)
                    {
                        if (!string.IsNullOrEmpty(query.SelectColumn))
                        {
                            if (query.isOnlyOneSelColumn)
                            {
                                where += string.Format(" and (Pat_lis_main.{0} in ({1}))", query.SelectColumn, query.TxtInNoSql);
                            }
                            else
                            {
                                where += string.Format(" and (Pat_lis_main.{0} in ({1}) or Pma_pat_in_no  in ({1}) or Pma_input_id in ({1}))", query.SelectColumn, query.TxtInNoSql);
                            }
                        }
                        else
                        {
                            where += string.Format(" and (Pat_lis_main.Pma_input_id in ({0})  or Pma_pat_in_no  in ({0}))", query.TxtInNoSql);
                        }
                    }
                    else
                    {
                        if (query.HospitalInterfaceMode == "深圳滨海医院")
                        {
                            if (query.PatEmpId != null)
                            {
                                where += string.Format(" and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_exam_no in ({0})  or Pat_lis_main.Pma_unique_id in ({0}))", query.PatEmpId);
                            }
                            else
                            {
                                where += string.Format(" and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_unique_id in ({0}))", query.PatUpid);
                            }
                        }
                        else
                        {
                            string temp_sel_noStr1 = " and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_exam_no in ({0})  or Pat_lis_main.rep_input_id in ({0}) or Pat_lis_main.Pma_social_no in ({0}))";
                            string temp_sel_noStr2 = " and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_input_id in ({0}) or Pat_lis_main.Pma_social_no in ({0}))";
                            switch (query.UseColumns)
                            {
                                case "1.pat_in_no":
                                    temp_sel_noStr1 = " and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_exam_no in ({0}))";
                                    temp_sel_noStr2 = " and (Pat_lis_main.Pma_pat_in_no in ({0}))";
                                    break;
                                case "2.pat_pid":
                                    temp_sel_noStr1 = " and (Pat_lis_main.Pma_exam_no in ({0})  or Pat_lis_main.Pma_input_id in ({0}))";
                                    temp_sel_noStr2 = " and (Pat_lis_main.Pma_input_id in ({0}))";
                                    break;
                                case "3.Pma_social_no":
                                    temp_sel_noStr1 = " and (Pat_lis_main.Pma_exam_no in ({0})  or Pat_lis_main.Pma_social_no in ({0}))";
                                    temp_sel_noStr2 = " and (Pat_lis_main.Pma_social_no in ({0}))";
                                    break;
                                case "1与2":
                                    temp_sel_noStr1 = " and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_exam_no in ({0})  or Pat_lis_main.Pma_input_id in ({0}))";
                                    temp_sel_noStr2 = " and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_input_id in ({0}))";
                                    break;
                                case "1与3":
                                    temp_sel_noStr1 = " and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_exam_no in ({0}) or Pat_lis_main.Pma_social_no in ({0}))";
                                    temp_sel_noStr2 = " and (Pat_lis_main.Pma_pat_in_no in ({0})  or Pat_lis_main.Pma_social_no in ({0}))";
                                    break;
                                case "2与3":
                                    temp_sel_noStr1 = " and (Pat_lis_main.Pma_exam_no in ({0})  or Pat_lis_main.Pma_input_id in ({0}) or Pat_lis_main.Pma_social_no in ({0}))";
                                    temp_sel_noStr2 = " and (Pat_lis_main.Pma_input_id in ({0}) or Pat_lis_main.Pma_social_no in ({0}))";
                                    break;
                                default: break;
                            }
                            if (query.TempSelNoStr1 != null)
                            {
                                where += string.Format(temp_sel_noStr1, query.TempSelNoStr1);
                            }
                            else
                            {
                                where += string.Format(temp_sel_noStr2, query.TempSelNoStr2);
                            }
                        }
                    }
                }
                if (query.SearchOuterInterfaceMode)
                {
                    outerRepWhere += string.Format(" and KM_LIS_Result.F_PatientNumber='{0}'", query.TxtInNo);
                }
                else
                {
                    outerRepWhere += string.Format(" and lis_report_info.pat_id='{0}'", query.TxtInNo);

                }
                if (query.SearchCDR)
                {
                    CDRWhere += string.Format(" and (cdr_lis_main.lismain_hospitalno='{0}' or cdr_lis_main.lismain_inpatient_no='{0}')", query.TxtInNo);
                }
            }

            //物理组别
            if (!string.IsNullOrEmpty(query.TypeId))
            {
                where += " and Dict_itr_instrument.Ditr_lab_id='" + query.TypeId + "'";
                outerRepWhere += " and 1=2";//外部报告单查询,若为专业组,则不查询
            }

            //来源
            if (!string.IsNullOrEmpty(query.OriId))
            {
                where += " and Pat_lis_main.Pma_pat_Dsorc_id='" + query.OriId + "'";
            }

            //是否有权限查看保密数据
            if (query.EnableLookSecretData)
            {
                where += " and (Pma_ctype is null or Pma_ctype!='3')";
            }

            //条码号
            if (!string.IsNullOrEmpty(query.PatBarCode))
            {
                where += " and Pat_lis_main.Pma_bar_code='" + query.PatBarCode + "'";
                if (query.SearchOuterInterfaceMode)
                {
                    //系统配置：是否金域报表自定义连接地址
                    if (query.IsKMReportDIYCon)
                    {
                        outerRepWhere = " and F_HospSampleID='" + query.PatBarCode + "'";
                    }
                    else
                    {
                        outerRepWhere += " and KM_LIS_Result.F_HospSampleID='" + query.PatBarCode + "'";
                    }
                }
                else
                {
                    outerRepWhere += " and lis_report_info.lis_Barcode='" + query.PatBarCode + "'";
                }
                if (query.SearchCDR)
                {
                    CDRWhere += string.Format(" and cdr_lis_main.lismain_bar_code='{0}'", query.PatBarCode);
                }
            }

            //判断是否有权限查看未审核数据
            if (query.IsLookNaturalReport && !query.EnableMutliPatQuery)
            {
                where += " and Pat_lis_main.Pma_status in ('2','4')";
            }

            if (query.EnbState)
            {
                where += " and (Pat_lis_main.Pma_status in ('2','4') or (Pat_lis_main.Pma_status=0 and Pma_micreport_flag=1) )";
            }

            //床号
            if (!string.IsNullOrEmpty(query.PatBedNo))
            {
                where += " and Pat_lis_main.Pma_pat_bed_no = '" + query.PatBedNo + "'";
            }

            //诊断
            if (!string.IsNullOrEmpty(query.PatDiag))
            {
                where += " and Pat_lis_main.Pma_pat_diag like '%" + query.PatDiag + "%' ";
            }

            //电话
            if (!string.IsNullOrEmpty(query.PatTel))
            {
                string tmep_where = "";
                where = " and 1=2 or Pat_lis_main.Pma_pat_tel='" + query.PatTel + "' and Pma_status in(2,4) {0} or 1=2 ";
                if (query.TypeId != null)
                {
                    tmep_where += " and Dict_itr_instrument.Ditr_lab_id='" + query.TypeId + "'";
                }
                where = string.Format(where, tmep_where);
            }

            //医院ID
            if (!string.IsNullOrEmpty(query.HospitalId))
            {
                where += " and Pat_lis_main.Pma_pat_Dorg_id = '" + query.HospitalId + "'";
            }

            listWhere.Add(where);
            listWhere.Add(outerRepWhere);
            listWhere.Add(CDRWhere);

            return listWhere;
        }

        public static DBManager GetSqlHelper(DateTime? start, EntityAnanlyseQC query)
        {
            DBManager sqlHelper;
            bool enableRead = query.LabEnableReadHistoryFromOldDB == "是";
            string lisHistoryDateStr = ConfigurationManager.AppSettings["LisHistoryDate"];
            string lisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
            //查询历史库数据
            if (start.HasValue && enableRead
                && !string.IsNullOrEmpty(lisHistoryConnectionString)
                && !string.IsNullOrEmpty(lisHistoryDateStr))
            {
                try
                {
                    DateTime lisHistoryDate = DateTime.Parse(lisHistoryDateStr);
                    if (lisHistoryDate > start.Value)
                    {
                        sqlHelper = new DBManager(lisHistoryConnectionString);
                        return sqlHelper;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return new DBManager();
        }

        public static DataTable GetPatList(DateTime? start, DateTime? end, string sql)
        {
            string LisHistoryDateStr = ConfigurationManager.AppSettings["LisHistoryDate"];
            string LisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
            //查询历史库数据
            if (start.HasValue
                && end.HasValue
                && !string.IsNullOrEmpty(LisHistoryConnectionString)
                && !string.IsNullOrEmpty(LisHistoryDateStr)
                )
            {
                try
                {
                    DateTime LisHistoryDate = DateTime.Parse(LisHistoryDateStr);
                    if (LisHistoryDate > start.Value)
                    {
                        DBManager sqlHelper = new DBManager(LisHistoryConnectionString);
                        DataTable result = sqlHelper.ExecuteDtSql(sql);
                        result.TableName = "LisHistory";
                        return result;
                    }
                }
                catch (Exception ex)
                {

                    Lib.LogManager.Logger.LogException(ex);
                }


            }
            return null;
        }



        public bool UpdatePatFlagToPrinted(EntityAnanlyseQC query)
        {
            string strWhere = string.Empty;
            strWhere = string.Format(" where Pma_rep_id in ({0}) and (Pma_status=2 or Pma_status=4) ", query.PatId);
            try
            {
                string sql = @"update Pat_lis_main set Pma_status='{0}',Pma_print_date = getdate() {1}";
                sql = string.Format(sql, "4", strWhere);
                DBManager helper = new DBManager();
                int resultCount = helper.ExecCommand(sql);
                if (resultCount > 0)
                    return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
            return false;
        }

        public List<EntityPidReportMain> GetPatBarCode(EntityAnanlyseQC query)
        {
            DBManager helper = new DBManager();
            string strWhere = string.Format(" where Pma_rep_id in ({0}) and (Pma_status=2 or Pma_status=4) ", query.PatId);
            string selectSql = string.Format(@"select  Pma_bar_code,Pma_pat_Dorg_id,Pma_rep_id from  Pat_lis_main {0}", strWhere);
            try
            {
                DataTable patDt = helper.ExecuteDtSql(selectSql);
                return EntityManager<EntityPidReportMain>.ConvertToList(patDt);
            }
            catch
            {
                return new List<EntityPidReportMain>();
                throw new Exception();
            }
        }

        public EntityDCLPrintData GetPatientReportInfo(EntityAnanlyseQC query)
        {
            DBManager helper = new DBManager();
            EntityDCLPrintData printer = new EntityDCLPrintData();
            DataSet result = new DataSet();
            EntityReport report = null;
            DataTable dtRep = helper.ExecuteDtSql("select * from Base_report where Brep_code='" + query.RepCode + "' or Brep_name ='" + query.RepCode + "'");
            if (dtRep.Rows.Count > 0)
                report = EntityManager<EntityReport>.ConvertToEntity(dtRep.Rows[0]);
            string where = GetRepCondition(query);
            if (report != null)
            {
                string sql = EncryptClass.Decrypt(report.RepSql.ToString());
                sql = sql.Replace("&where&", where);
                DataTable an = null;
                an = helper.ExecuteDtSql(sql);
                an.TableName = "可设计字段";
                result.Tables.Add(an);
                printer.ReportData = result;
                printer.ReportName = report.RepLocation.Replace(".repx", "");
                return printer;
            }
            return printer;
        }

        private string GetRepCondition(EntityAnanlyseQC query)
        {
            string where = string.Empty;
            if (query.IsOuterReport)
            {
                if (query.SearchOuterInterfaceMode)
                {
                    if (query.IsKMReportDIYCon)
                    {
                        where = " and F_HospSampleID='" + query.PatBarCode + "'";
                    }
                    else
                    {
                        where = " and kingmed.dbo.KM_LIS_ExtReport.F_HospSampleID='" + query.PatBarCode + "'";
                    }
                }
                else
                {
                    where = " and lis_report_pic.report_info_id='" + query.PatId + "'";
                }
            }
            else
            {
                if (query.PatCType == "旧检验数据")
                {
                    where = string.Format(" and cdr_lis_main.lismain_repno='{0}'", query.PatId);
                }
                else
                {
                    where = " and Pat_lis_main.Pma_rep_id='" + query.PatId + "'";
                }
            }
            return where;
        }



        /// <summary>
        /// Order By/排序
        /// </summary>
        /// <param name="input"></param>
        /// <param name="orderbyString"></param>
        /// <returns></returns>
        public static DataTable OrderBy(DataTable input, string orderbyString)
        {
            if (input != null && input.Rows.Count > 1)
            {
                input.DefaultView.Sort = orderbyString;
                return input.DefaultView.ToTable();
            }
            else
            {
                return input;
            }
        }

        public DataTable GetPatId(EntityAnanlyseQC query)
        {
            string where = string.Empty;
            DBManager helper = new DBManager();
            string classWhere = string.Empty;
            string strClassWhere = string.Empty;
            string strOtherWhere = string.Empty;
            if (query.StrClassWhere.Contains("其他类"))
            {
                strClassWhere = query.StrClassWhere.Replace("其他类", "");
                strOtherWhere = " or Dict_itm_combine.Dcom_class is null";
            }
            classWhere = string.Format(@" and Pat_lis_main.Pma_rep_id='{0}' and (Dict_itm_combine.Dcom_class in ({1}) {2})", query.PatId, strClassWhere, strOtherWhere);
            string sql = string.Format(@"select Pat_lis_main.Pma_rep_id from Pat_lis_main 
left join Pat_lis_detail on Pat_lis_main.Pma_rep_id=Pat_lis_detail.Pdet_id
left join Dict_itm_combine on Pat_lis_detail.Pdet_Dcom_id=Dict_itm_combine.Dcom_id
where 1=1 {0}", classWhere);
            DataTable dtClass = helper.ExecuteDtSql(sql);
            dtClass.TableName = "ClassPrint";
            return dtClass;
        }

        public bool DeletePatient(string repId)
        {
            DBManager helper = new DBManager();
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

        /// <summary>
        /// 报告结果是否包含异常值
        /// </summary>
        /// <param name="repId">报告单号</param>
        /// <returns></returns>
        public bool IsContainOutlier(string repId)
        {
            DBManager helper = new DBManager();
            try
            {
                string sql = string.Format("SELECT 1 FROM Lis_result a WHERE a.Lres_Pma_rep_id = '{0}' AND a.Lres_ref_flag > 0", repId);

                return helper.ExecuteDtSql(sql).Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
