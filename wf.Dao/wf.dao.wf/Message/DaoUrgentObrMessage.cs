using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoUrgentObrMessage))]
    public class DaoUrgentObrMessage : IDaoUrgentObrMessage
    {
        public List<EntityPidReportMain> GetUrgentMsgByPatFlag(string pat_flag)
        {
            try
            {
                DBManager helper = new DBManager();

                #region 危急与急查信息
                string strSQL = string.Format(@"select 
Pat_lis_main.Pma_rep_id,
Lis_message_content.Lmsg_id obr_id_msg,
Lis_message_content.Lmsg_type,
Lis_message_content.Lmsg_inside_flag,
Lis_message_content.Lmsg_nurse_read_flag,--护工是否确认
Lis_message_content.Lmsg_doctor_read_flag,--医生是否确认
(case when Lis_message_content.Lmsg_type=1024 and Lis_message_content.Lmsg_extend_d='传染病' then '传染病' 
      when Lis_message_content.Lmsg_type=1024 or Lis_message_content.Lmsg_type=3024 then '危急' 
      when Lis_message_content.Lmsg_type=4096 and Lis_message_content.Lmsg_extend_d='多重耐药菌' then '多重耐药' 
      when Lis_message_content.Lmsg_type=4096 then '急查' 
      else '' end) as msg_type_txt,
Lis_message_content.Lmsg_create_time,
(case when isnull(Pat_lis_main.Pma_pat_dept_id,'')='' then Pat_lis_main.Pma_pat_ward_id else Pat_lis_main.Pma_pat_dept_id end) as 'Pma_pat_dept_id',
Pat_lis_main.Pma_pat_in_no,
Pat_lis_main.Pma_social_no, --医保卡号/诊疗卡号
Pat_lis_main.Pma_pat_Didt_id, --病人ID类型 
Pat_lis_main.Pma_pat_Dsorc_id,--病人来源
Pat_lis_main.Pma_Ditr_id,
(case when isnull(Dict_dept.Ddept_name,'')<>'' then Dict_dept.Ddept_name 
      when isnull(Pat_lis_main.Pma_pat_dept_name,'')<>'' then Pat_lis_main.Pma_pat_dept_name else '未知' end) as 'Ddept_name',--科室名称,
 Pat_lis_main.Pma_pat_bed_no ,--床号,
 Pat_lis_main.Pma_pat_name ,--姓名, 
(case when isnull(Dict_doctor.Ddoctor_name,'')<>'' then Dict_doctor.Ddoctor_name else Pat_lis_main.Pma_doctor_name end) as 'Ddoctor_name',--医生姓名,
Dict_doctor.Ddoctor_code as Pma_Ddoctor_id,
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '未知' end) Pma_pat_sex,--性别,
dbo.getAge(Pat_lis_main.Pma_pat_age_exp) pid_age_str,--年龄,
Pat_lis_main.Pma_com_name,--组合
Dict_doctor.Ddoctor_tel,
Pat_lis_main.Pma_pat_tel,
Pat_lis_main.Pma_bar_code,
Lis_message_content.Lmsg_extend_c as  pid_result,--危急值结果,
PowerUserInfo2.Buser_name pid_chk_name,--审核人,
Pat_lis_main.Pma_audit_date ,--一审时间,
Pat_lis_main.Pma_report_date ,--二审时间,
Dict_itr_instrument.Ditr_lab_id, --物理组
getdate() as 'server_date',--服务器时间
cast(0 as bit) as 'pat_select',
Dict_itr_instrument.Ditr_name,
Dict_itr_instrument.Ditr_report_type,
isnull(Dict_itr_instrument.Ditr_report_id,'') Ditr_report_id,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_serial_num,
Dict_sample.Dsam_name,
Dict_dept.Ddept_tel,
Lis_message_content.Lmsg_extend_d,
Lis_message_content.Lmsg_send_dept_code
from Pat_lis_main with(nolock)
      left join 
      Dict_doctor on Pat_lis_main.Pma_Ddoctor_id=Dict_doctor.Ddoctor_code and Dict_doctor.del_flag='0'
      left join
      Base_user PowerUserInfo2 on Pat_lis_main.Pma_report_Buser_id=PowerUserInfo2.Buser_loginid
      left join Dict_dept on Pat_lis_main.Pma_pat_dept_id = Dict_dept.Ddept_code and Dict_dept.del_flag='0'
      left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
      left join Dict_sample on Dict_sample.Dsam_id=Pat_lis_main.Pma_Dsam_id
      left join 
      Lis_message_content with(nolock) on Pat_lis_main.Pma_rep_id=Lis_message_content.Lmsg_extend_a
where 
Lis_message_content.del_flag=0 and
Pat_lis_main.Pma_status in ({0}) and
Pat_lis_main.Pma_rep_id in(
select Lis_message_content.Lmsg_extend_a from Lis_message_content with(nolock)
left join  Lis_message_receive with(nolock) on Lis_message_content.Lmsg_id=Lis_message_receive.Lmsgrec_Lmsg_id
where Lis_message_content.del_flag=0
and Lis_message_receive.del_flag=0
and Lis_message_content.Lmsg_type in(1024,4096,3024) --类型
and Lis_message_content.Lmsg_create_time>=Dateadd(day,-5,getdate()) --日期

) ", pat_flag);
                #endregion

                DataTable dtPats = helper.ExecuteDtSql(strSQL);
                List<EntityPidReportMain> listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtPats).OrderBy(i => i.RepId).ToList();

                return listPats;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

        public List<EntityPidReportMain> GetUrgentMsgToA()
        {
            try
            {
                DBManager helper = new DBManager();

                #region 内部提醒危急值(msg_content无数据时,来源为住院)

                string strSQLInner = @"select 
 Pat_lis_main.Pma_rep_id,
'-1$' as obr_id_msg,
1024 as Lmsg_type,
cast(null as char(1)) as Lmsg_inside_flag,
'危急' as msg_type_txt,
Pat_lis_main.Pma_report_date as Lmsg_create_time,
(case when isnull(Pat_lis_main.Pma_pat_dept_id,'')='' then Pat_lis_main.Pma_pat_ward_id else Pat_lis_main.Pma_pat_dept_id end) as 'Pma_pat_dept_id',
 Pat_lis_main.Pma_pat_in_no,
Pat_lis_main.Pma_social_no, --医保卡号/诊疗卡号
Pat_lis_main.Pma_pat_Didt_id, --病人ID类型 
Pat_lis_main.Pma_pat_Dsorc_id,--病人来源
Pat_lis_main.Pma_Ditr_id,
(case when isnull(Dict_dept.Ddept_name,'')<>'' then Dict_dept.Ddept_name 
      when isnull(Pat_lis_main.Pma_pat_dept_name,'')<>'' then Pat_lis_main.Pma_pat_dept_name 
   else '未知' end) as 'Ddept_name',--科室名称,
 Pat_lis_main.Pma_pat_bed_no ,--床号,
 Pat_lis_main.Pma_pat_name ,--姓名, 
(case when isnull(Dict_doctor.Ddoctor_name,'')<>'' then Dict_doctor.Ddoctor_name else Pat_lis_main.Pma_doctor_name end) as 'Ddoctor_name',--医生姓名,
Dict_doctor.Ddoctor_code as Pma_Ddoctor_id,
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '未知' end) Pma_pat_sex,--性别,
dbo.getAge(Pat_lis_main.Pma_pat_age_exp) pat_age_str,--年龄,
Pat_lis_main.Pma_com_name,--组合
Dict_doctor.Ddoctor_tel,
Pat_lis_main.Pma_pat_tel,
Pat_lis_main.Pma_bar_code,
'异常危急值信息' as  pid_result,--危急值结果,
PowerUserInfo2.Buser_name pid_chk_name,--审核人,
Pat_lis_main.Pma_audit_date ,--一审时间,
Pat_lis_main.Pma_report_date ,--二审时间,
Dict_itr_instrument.Ditr_lab_id, --物理组
getdate() as 'server_date',--服务器时间
cast(0 as bit) as 'pat_select',
Dict_itr_instrument.Ditr_name,
Dict_itr_instrument.Ditr_report_type,
isnull(Dict_itr_instrument.Ditr_report_id,'') Ditr_report_id,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_serial_num,
Dict_sample.Dsam_name,
Dict_dept.Ddept_tel,
cast(null as varchar(200)) as Lmsg_extend_d,
cast(null as varchar(12)) as Lmsg_send_dept_code,
Datediff(mi,Pat_lis_main.Pma_report_date,getdate()) as msg_add_time,
'' as obr_value_e,
'检验科' as obr_send_depname
from Pat_lis_main with(nolock)
      left join 
      Dict_doctor on Pat_lis_main.Pma_Ddoctor_id=Dict_doctor.Ddoctor_id and Dict_doctor.del_flag='0'
      left join
      Base_user PowerUserInfo2 on Pat_lis_main.Pma_report_Buser_id=PowerUserInfo2.Buser_loginid
      left join Dict_dept on Pat_lis_main.Pma_pat_dept_id = Dict_dept.Ddept_code and Dict_dept.del_flag='0'
      left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
      left join Dict_sample on Dict_sample.Dsam_id=Pat_lis_main.Pma_Dsam_id
where 
Pat_lis_main.Pma_report_date>=Dateadd(day,-1,getdate())
and Pat_lis_main.Pma_status in (2,4)
and (Pat_lis_main.Pma_urgent_flag=1 or Pat_lis_main.Pma_urgent_flag=2)
and (Pat_lis_main.Pma_read_Buser_id is null or Pat_lis_main.Pma_read_Buser_id='')
and Pat_lis_main.Pma_read_date is null
and exists(select top 1 1 from Lis_result with(nolock) where Lis_result.Lres_Pma_rep_id=Pat_lis_main.Pma_rep_id)
and not exists(select top 1 1 from Lis_message_content with(nolock) where Lis_message_content.Lmsg_extend_a=Pat_lis_main.Pma_rep_id)
and not exists(select top 1 1 from Pid_report_main_ext with(nolock) where Pid_report_main_ext.rep_id=Pat_lis_main.Pma_rep_id and Pid_report_main_ext.msg_doc_name is not null and Pid_report_main_ext.msg_doc_name<>'')
and Pat_lis_main.Pma_pat_Dsorc_id='108'  ";
                #endregion

                DataTable dtPatsInner = helper.ExecuteDtSql(strSQLInner);
                List<EntityPidReportMain> listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtPatsInner).OrderBy(i => i.RepId).ToList();

                return listPats;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

        public List<EntityPidReportMain> GetUrgentMsgToB()
        {
            try
            {
                DBManager helper = new DBManager();

                #region 自编危急信息
                string strSQLDIY = @"SELECT 
 Lis_message_content.Lmsg_id as Pma_rep_id,
 Lis_message_content.Lmsg_id obr_id_msg,
 Lis_message_content.Lmsg_type,
 Lis_message_content.Lmsg_inside_flag,
 '自编危急' as msg_type_txt,
 Lis_message_content.Lmsg_create_time,
 Lis_message_receive.Lmsgrec_user_id as 'Pma_pat_dept_id', 
 Lis_message_content.Lmsg_pid_in_no,
 '' as rep_itr_id,
 Lis_message_receive.Lmsgrec_user_name as 'Ddept_name',
 Lis_message_content.Lmsg_pid_bed_no,
 Lis_message_content.Lmsg_pid_name, 
 Lis_message_content.Lmsg_doctor_name as 'Ddoctor_name',
 Lis_message_content.Lmsg_doctor_code as 'Pma_Ddoctor_id',
 (case Lis_message_content.Lmsg_pid_sex when 1 then '男' when 2 then '女' else '未知' end) Lmsg_pid_sex,
 Lis_message_content.Lmsg_pid_age as Pma_pat_age,
 '' as Pma_com_name,
 Lis_message_content.Lmsg_pid_Dsorc_id,
Dict_doctor.Ddoctor_tel,
Lis_message_content.Lmsg_tel as Pma_pat_tel,
 '' as Pma_bar_code,
 Lis_message_content.Lmsg_extend_c as  pid_result,
 '' as pid_chk_name,
 Lis_message_content.Lmsg_create_time as Pma_audit_date,
 Lis_message_content.Lmsg_create_time as Pma_report_date,
 '' as Ditr_lab_id,
 getdate() as 'server_date',--服务器时间
 cast(0 as bit) as 'pat_select',
 '' as Ditr_name,
'' as Ditr_report_type,
'' as Ditr_report_id,
 '' as Pma_sid,
 '' as Pma_serial_num,
'' as Dsam_name,
'' as Ddept_tel,
Lis_message_content.Lmsg_extend_d,
Lis_message_content.Lmsg_send_dept_code
FROM  Lis_message_content with(nolock) INNER JOIN
Lis_message_receive with(nolock) ON Lis_message_content.Lmsg_id = Lis_message_receive.Lmsgrec_Lmsg_id
left join Dict_source on Dict_source.Dsorc_id=Lis_message_content.Lmsg_pid_Dsorc_id
left join Dict_doctor on Lis_message_content.Lmsg_doctor_code=Dict_doctor.Ddoctor_id and Dict_doctor.del_flag='0'
where Lis_message_content.Lmsg_type=2024 and Lis_message_content.del_flag=0
and Lis_message_receive.del_flag=0
and Lis_message_content.Lmsg_create_time>=Dateadd(day,-5,getdate())";
                #endregion

                DataTable dtPatsDIY = helper.ExecuteDtSql(strSQLDIY);
                List<EntityPidReportMain> listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtPatsDIY).OrderBy(i => i.RepId).ToList();

                return listPats;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

        public List<EntityPidReportMain> GetUrgentMsgToC()
        {
            try
            {
                DBManager helper = new DBManager();

                #region 回退标本信息
                string strReturn = @"SELECT 
 '' as Pma_rep_id,
 '' as obr_id_msg,
 16 as Lmsg_type,
 '' as Lmsg_inside_flag,
 '回退标本' as msg_type_txt,
 Sample_return.Sret_date as Lmsg_create_time,
 Sample_return.Sret_dept_code as 'Pma_pat_dept_id', 
 Sample_main.Sma_pat_in_no as Pma_pat_in_no,
 '' as Pma_Ditr_id,
 Sample_return.Sret_dept_name as 'Ddept_name',
 Sample_main.Sma_pat_bed_no as Pma_pat_bed_no,
 Sample_main.Sma_pat_name as Pma_pat_name, 
 Sample_main.Sma_doctor_name as 'Ddoctor_name',
 Sample_main.Sma_doctor_code as 'Pma_Ddoctor_id',
 Sample_main.Sma_pat_sex as Pma_pat_sex,
 dbo.getAge(Sample_main.Sma_pat_age) as pid_age_str,
 Sample_main.Sma_com_name as Pma_com_name,
 Sample_return.Sret_pat_Dsorc_id as Pma_pat_Dsorc_id,
 Dict_doctor.Ddoctor_tel,
 '' as Pma_pat_tel,
 Sample_return.Sret_Smain_bar_code as Pma_bar_code,
 Sample_return.Sret_return_reasons as  pid_result,
 Sample_return.Sret_user_name as pid_chk_name,
 getdate() as Pma_audit_date,
 getdate() as Pma_report_date,
 '' as Ditr_lab_id,
 getdate() as 'server_date',--服务器时间
 cast(0 as bit) as 'pat_select',
 '' as Ditr_name,
'' as Ditr_report_type,
'' as Ditr_report_id,
 '' as Pma_sid,
 '' as Pma_serial_num,
'' as Dsam_name,
'' as Ddept_tel,
'' as Lmsg_extend_d,
Sample_return.Sret_dept_name as Lmsg_send_dept_code
FROM  Sample_return with(nolock) 
left join Dict_source on Dict_source.Dsorc_id=Sample_return.Sret_pat_Dsorc_id
left join Dict_doctor on Sample_return.Sret_user_id=Dict_doctor.Ddoctor_id and Dict_doctor.del_flag='0'
left join Sample_main on Sample_return.Sret_Smain_bar_code=Sample_main.Sma_bar_code
where Sample_return.Sret_date>=DATEADD(d,-300,GETDATE()) and Sample_return.del_flag = 0 and  Sample_return.Sret_proc_flag = 0  ";
                #endregion

                DataTable dtPatsReturn = helper.ExecuteDtSql(strReturn);
                List<EntityPidReportMain> listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtPatsReturn).OrderBy(i => i.RepId).ToList();

                return listPats;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

        public List<EntityPidReportMain> GetUrgentflagAndPatlookcodeByPatid(string pat_id)
        {
            try
            {
                DBManager helper = new DBManager();

                string sql_content = @"select Pma_urgent_flag,Pma_read_Buser_id,Pma_read_date 
                                         from Pat_lis_main where Pma_rep_id = @rep_id";
                List<DbParameter> paramHt = new List<DbParameter>();
                if (pat_id != null)
                {
                    paramHt.Add(new SqlParameter("@rep_id ", pat_id));
                }
                DataTable dtInfo = helper.ExecuteDtSql(sql_content, paramHt);
                List<EntityPidReportMain> listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtInfo).OrderBy(i => i.RepReadUserId).ToList();
                return listPats;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityPidReportMain>();
            }
        }

        public List<EntityPidReportMain> GetUrgentHistoryMsgSqlWhere(EntityUrgentHistoryUseParame entityParame)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();

            if (entityParame != null)
            {
                string p_msg_type = entityParame.MsgType;//消息类型,默认1024,危急信息
                string p_time_start = entityParame.CreateTimeStart;//开始时间
                string p_time_end = entityParame.CreateTimeEnd;//结束时间
                string p_receiver_id = entityParame.ReceiveID;//科室ID

                string strWhere_receiver_id_In = "";//科室ID查询条件

                if (!string.IsNullOrEmpty(p_receiver_id))//过滤科室(或病区)
                {
                    if (p_receiver_id.Contains(",") && p_receiver_id.Contains("'"))
                    {
                        strWhere_receiver_id_In = string.Format(@" and (Lis_message_receive.Lmsgrec_user_id in(select Ddept_code from Dict_dept where Ddept_parent_Ddept_id in
                              (select  Ddept_parent_Ddept_id from dbo.Dict_dept where Ddept_code in ({0}))) or Lis_message_receive.Lmsgrec_user_id in(
                              select  Ddept_parent_Ddept_id from dbo.Dict_dept where Ddept_code in ({0}) and Ddept_parent_Ddept_id is not null  ))", p_receiver_id);
                    }
                    else
                    {
                        strWhere_receiver_id_In = string.Format(@" and (Lis_message_receive.Lmsgrec_user_id in(select Ddept_code from Dict_dept where Ddept_parent_Ddept_id =
                             (select top 1 Ddept_parent_Ddept_id from dbo.Dict_dept where Ddept_code='{0}')) or Lis_message_receive.Lmsgrec_user_id in(
                             select  Ddept_parent_Ddept_id from dbo.Dict_dept where Ddept_code='{0}' and Ddept_parent_Ddept_id is not null  )) ", p_receiver_id);
                    }
                }

                string strWhere_ori_id = "";//病人来源查询条件

                string p_neg_dep = entityParame.IsNeglectDep;//忽略科室(或病区)
                if ((!string.IsNullOrEmpty(p_neg_dep)) && p_neg_dep == "1")//1代表忽略
                {
                    strWhere_receiver_id_In = "";
                }

                string p_ori_id = entityParame.PatOriConfig;//病人来源配置
                if (!string.IsNullOrEmpty(p_ori_id))
                {
                    //值为1111 分别代表 住院,门诊,体检,其他
                    char[] chr = p_ori_id.ToCharArray();

                    for (int i = 0; i < chr.Length; i++)
                    {
                        if (i == 0)
                        {
                            if (chr[i] == '0') strWhere_ori_id += " and Pat_lis_main.Pma_pat_Dsorc_id<>'108' ";
                        }
                        if (i == 1)
                        {
                            if (chr[i] == '0') strWhere_ori_id += " and Pat_lis_main.Pma_pat_Dsorc_id<>'107' ";
                        }
                        if (i == 2)
                        {
                            if (chr[i] == '0') strWhere_ori_id += " and Pat_lis_main.Pma_pat_Dsorc_id'109' ";
                        }
                        if (i == 3)
                        {
                            if (chr[i] == '0') strWhere_ori_id += " and (Pat_lis_main.Pma_pat_Dsorc_id='107' or Pat_lis_main.Pma_pat_Dsorc_id='108' or Pat_lis_main.Pma_pat_Dsorc_id='109') ";
                        }
                    }
                }

                string strWhere_doc_id = "";//医生代码查询条件

                string p_doc_id = entityParame.PatDocId;//医生代码
                if (!string.IsNullOrEmpty(p_doc_id))
                {
                    strWhere_doc_id = string.Format("  and Pat_lis_main.Pma_Ddoctor_id=(select top 1 Ddoctor_id from Dict_doctor as doc where doc.Ddoctor_code='{0}') ", p_doc_id);
                }

                try
                {
                    //新查询出字段：审核人工号
                    string strSQLMW = string.Format(@"
                                 select Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_pat_in_no,
Pat_lis_main.Pma_social_no, --医保卡号/诊疗卡号
Pat_lis_main.Pma_pat_Didt_id, --病人ID类型 
Pat_lis_main.Pma_pat_Dsorc_id,--病人来源
(case when isnull(Dict_dept.Ddept_name,'')<>'' then Dict_dept.Ddept_name 
when isnull(Pat_lis_main.Pma_pat_dept_name,'')<>'' then Pat_lis_main.Pma_pat_dept_name else '未知' end) as 'Ddept_name',--科室名称,
Pat_lis_main.Pma_pat_bed_no ,--床号,
Pat_lis_main.Pma_pat_name ,--姓名, 
Pat_lis_main.Pma_doctor_name as Ddoctor_name,--医生姓名,
(case Pat_lis_main.Pma_pat_sex when 1 then '男' when 2 then '女' else '未知' end) Pma_pat_sex,--性别,
dbo.getAge(Pat_lis_main.Pma_pat_age_exp) pid_age_str,--年龄,
Lis_message_content.Lmsg_extend_c as  pid_result,--危急值结果,
PowerUserInfo2.Buser_loginid pid_chk_number,--审核人工号
PowerUserInfo2.Buser_name pid_chk_name,--审核人,
Pat_lis_main.Pma_audit_date ,--一审时间,
Pat_lis_main.Pma_report_date ,--二审时间,
Dict_itr_instrument.Ditr_lab_id, --物理组
Pat_lis_main.Pma_pat_tel,
Pat_lis_main.Pma_bar_code,
Lis_message_content.Lmsg_extend_d,
--PowerUserInfo1.user_name pat_look_name,--确认人,
Pat_lis_main.Pma_read_Buser_id pat_look_name,--确认人,
Pat_lis_main.Pma_read_date ,--确认时间,
'' pid_datediff,--确认时间差,
Dict_sample.Dsam_name,
(case when Pat_lis_main.Pma_read_Buser_id is null then '未确认'  when Lis_message_content.Lmsg_confirm_type='2' then '检验科通知确认' else '临床确认' end) as 'affirm_mode',--确认方式,
Pid_report_main_ext.msg_content pid_res,--备注
Pid_report_main_ext.msg_content2 pid_res2--备注
from Pat_lis_main with(nolock)
left join Dict_doctor on Pat_lis_main.Pma_Ddoctor_id=Dict_doctor.Ddoctor_id and Dict_doctor.del_flag='0'
left join Base_user PowerUserInfo1 on Pat_lis_main.Pma_read_Buser_id=PowerUserInfo1.Buser_loginid
left join Base_user PowerUserInfo2 on Pat_lis_main.Pma_report_Buser_id=PowerUserInfo2.Buser_loginid
left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
left join Dict_sample on Dict_sample.Dsam_id=Pat_lis_main.Pma_Dsam_id
left join Dict_dept on Pat_lis_main.Pma_pat_dept_id = Dict_dept.Ddept_code and Dict_dept.del_flag='0'
left join Lis_message_content on Pat_lis_main.Pma_rep_id=Lis_message_content.Lmsg_extend_a
left join Pid_report_main_ext with(nolock) on Pid_report_main_ext.rep_id=Lis_message_content.Lmsg_extend_a
where Pat_lis_main.Pma_rep_id in(
select Lis_message_content.Lmsg_extend_a 
from Lis_message_content with(nolock)
left join  Lis_message_receive with(nolock) on Lis_message_content.Lmsg_id=Lis_message_receive.Lmsgrec_Lmsg_id
where -- Lis_message_content.msg_del_flag=1 and 
(Lis_message_content.Lmsg_type=3024 or Lis_message_content.Lmsg_type=2024 or Lis_message_content.Lmsg_type={0}) --类型
and Lis_message_content.Lmsg_create_time>='{1}' --日期
and Lis_message_content.Lmsg_create_time<'{2}' --日期
                                                 {3}
                                               )
                                      {4}
                                      {5}
                                      --and (Pid_report_main.pid_dept_id is not null and Pid_report_main.pid_dept_id<>'')
                    ", p_msg_type, p_time_start, p_time_end, strWhere_receiver_id_In, strWhere_ori_id, strWhere_doc_id);

                    #region 自编危急值信息

                    string strSQLDIY = string.Format(@"SELECT 
  Lis_message_content.Lmsg_id as Pma_rep_id,
 Lis_message_content.Lmsg_pid_in_no,
 Lis_message_receive.Lmsgrec_user_name as 'Ddept_name',
 Lis_message_content.Lmsg_pid_bed_no,
 Lis_message_content.Lmsg_pid_name, 
 Lis_message_content.Lmsg_doctor_name as 'Ddoctor_name',
 (case Lis_message_content.Lmsg_pid_sex when 1 then '男' when 2 then '女' else '未知' end) Pma_pat_sex,
 Lis_message_content.Lmsg_pid_age as Pma_pat_age,
 Lis_message_content.Lmsg_extend_c as  pid_result,
 '' as pid_chk_name,
 Lis_message_content.Lmsg_create_time as Pma_audit_date,
 Lis_message_content.Lmsg_create_time as Pma_report_date,
Lis_message_content.Lmsg_tel as Pma_pat_tel,
 '' as Pma_bar_code,
Lis_message_content.Lmsg_extend_d,
Lis_message_content.Lmsg_audit_user_name as pat_look_name,--确认人,
Lis_message_receive.Lmsgrec_read_time  as Pma_read_date,--确认时间,
'' pid_datediff,--确认时间差,
'' as Dsam_name,
(case when isnull(Lis_message_content.Lmsg_audit_user_id,'')='' then '未确认'  when Lis_message_content.Lmsg_confirm_type='2' then '检验科通知确认' else '临床确认' end) as 'affirm_mode',--确认方式,
Pid_report_main_ext.msg_content pid_res--备注
FROM  Lis_message_content with(nolock) INNER JOIN
Lis_message_receive with(nolock) ON Lis_message_content.Lmsg_id = Lis_message_receive.Lmsgrec_Lmsg_id
left join Dict_source on Dict_source.Dsorc_id=Lis_message_content.Lmsg_pid_Dsorc_id
left join Pid_report_main_ext with(nolock) on Pid_report_main_ext.rep_id=Lis_message_content.Lmsg_extend_a
where Lis_message_content.Lmsg_type=2024 
and Lis_message_content.Lmsg_create_time>='{0}' --日期
and Lis_message_content.Lmsg_create_time<'{1}' --日期
{2}
", p_time_start, p_time_end, strWhere_receiver_id_In);

                    #endregion

                    DBManager helper = new DBManager();
                    DataTable dtSQLMW = helper.ExecuteDtSql(strSQLMW);
                    listPats = EntityManager<EntityPidReportMain>.ConvertToList(dtSQLMW).OrderBy(i => i.RepId).ToList();

                    if (true)//自编危急
                    {

                        DataTable dtbResultDIY = helper.ExecuteDtSql(strSQLDIY);
                        if (dtbResultDIY != null && dtbResultDIY.Rows.Count > 0)
                        {
                            //dtbResult.Tables[0].Merge(dtbResultDIY);
                            List<EntityPidReportMain> listDIY = EntityManager<EntityPidReportMain>.ConvertToList(dtbResultDIY).OrderBy(i => i.RepId).ToList();
                            foreach (var infoDIY in listDIY)
                            {
                                listPats.Add(infoDIY);
                            }
                        }
                    }
                     
                    //排序
                    listPats = listPats.OrderByDescending(i => i.RepAuditDate).ToList();
                }
                catch (Exception objEx)
                {
                    Lib.LogManager.Logger.LogException("获取危急值历史信息(sqlWhere)", objEx);
                }
            }
            return listPats;
        }
    }
}
