using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///病人信息表
    ///旧表名:Pid_report_main 新表名:Pat_lis_main
    /// </summary>
    [Serializable]
    public class EntityPidReportMain : EntityBase
    {
        public EntityPidReportMain()
        { 
            ListPidReportDetail = new List<EntityPidReportDetail>();
            RepAuditWay = 0;
            HasResult2 = -1;
            UrgentMsgHandle = 0;
            UrgentCount = 0;
            ModifyFlag = string.Empty;
            PatSelect = false;
            DestItmIds = new List<string>();
            ResStatus = -1;
            ComLineColor = 0;
        }
        /// <summary>
        ///标识ID 
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_id", MedName = "rep_id",WFName = "Pma_rep_id")]
        public String RepId { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_itr_id", MedName = "rep_itr_id", WFName = "Pma_Ditr_id")]
        public String RepItrId { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sid", MedName = "rep_sid", WFName = "Pma_sid")]
        public String RepSid { get; set; }

        /// <summary>
        ///姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_name", MedName = "pid_name", WFName = "Pma_pat_name")]
        public String PidName { get; set; }

        /// <summary>
        ///性别 0-未知 1-男 2-女
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sex", MedName = "pid_sex", WFName = "Pma_pat_sex")]
        public String PidSex { get; set; }

        /// <summary>
        /// 用于显示的性别
        /// </summary>
        public String PidSexExp
        {
            get
            {
                if (PidSex == "0")
                    return "未知";
                else if (PidSex == "1")
                    return "男";
                else
                {
                    return "女";
                }
            }
        }

        /// <summary>
        ///年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_age", MedName = "pid_age", WFName = "Pma_pat_age")]
        public Decimal? PidAge { get; set; }


        /// <summary>
        ///格式化年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_age_exp", MedName = "pid_age_exp", WFName = "Pma_pat_age_exp")]
        public String PidAgeExp { get; set; }

        /// <summary>
        ///年龄单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_age_unit", MedName = "pid_age_unit", WFName = "Pma_pat_age_unit")]
        public String PidAgeUnit { get; set; }

        /// <summary>
        ///科室编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_dep_id", MedName = "pid_dept_id", WFName = "Pma_pat_dept_id")]
        public String PidDeptId { get; set; }

        /// <summary>
        ///病人ID类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_no_id", MedName = "pid_idt_id", WFName = "Pma_pat_Didt_id")]
        public String PidIdtId { get; set; }

        /// <summary>
        ///病人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_in_no", MedName = "pid_in_no", WFName = "Pma_pat_in_no")]
        public String PidInNo { get; set; }

        /// <summary>
        ///就诊次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_admiss_times", MedName = "pid_addmiss_times", WFName = "Pma_pat_addmiss_times")]
        public Int32 PidAddmissTimes { get; set; }

        /// <summary>
        ///床号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_bed_no", MedName = "pid_bed_no", WFName = "Pma_pat_bed_no")]
        public String PidBedNo { get; set; }

        /// <summary>
        ///组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_c_name", MedName = "pid_com_name", WFName = "Pma_com_name")]
        public String PidComName { get; set; }

        /// <summary>
        ///诊断
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_diag", MedName = "pid_diag", WFName = "Pma_pat_diag")]
        public String PidDiag { get; set; }

        /// <summary>
        ///标本状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_rem", MedName = "pid_remark", WFName = "Pma_pat_remark")]
        public String PidRemark { get; set; }

        /// <summary>
        ///职业/工作
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_work", MedName = "pid_work", WFName = "Pma_pat_work")]
        public String PidWork { get; set; }

        /// <summary>
        ///联系电话
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_tel", MedName = "pid_tel", WFName = "Pma_pat_tel")]
        public String PidTel { get; set; }

        /// <summary>
        ///邮箱
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_email", MedName = "pid_email", WFName = "Pma_pat_email")]
        public String PidEmail { get; set; }

        /// <summary>
        ///单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_unit", MedName = "pid_unit", WFName = "Pma_pat_unit")]
        public String PidUnit { get; set; }

        /// <summary>
        ///地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_address", MedName = "pid_address", WFName = "Pma_pat_address")]
        public String PidAddress { get; set; }

        /// <summary>
        ///孕周
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_pre_week", MedName = "pid_pre_week", WFName = "Pma_pat_pre_week")]
        public String PidPreWeek { get; set; }

        /// <summary>
        ///身高
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_height", MedName = "pid_height", WFName = "Pma_pat_height")]
        public String PidHeight { get; set; }

        /// <summary>
        ///体重
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_weight", MedName = "pid_weight", WFName = "Pma_pat_weight")]
        public String PidWeight { get; set; }

        /// <summary>
        ///标本编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sam_id", MedName = "pid_sam_id", WFName = "Pma_Dsam_id")]
        public String PidSamId { get; set; }

        /// <summary>
        ///检查目的编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_chk_id", MedName = "pid_purp_id", WFName = "Pma_Dpurp_id")]
        public String PidPurpId { get; set; }

        /// <summary>
        ///申请医生
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_doc_id", MedName = "pid_doctor_code", WFName = "Pma_Ddoctor_id")]
        public String PidDoctorCode { get; set; }

        /// <summary>
        ///检验者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_i_code", MedName = "rep_check_user_id", WFName = "Pma_check_Buser_id")]
        public String RepCheckUserId { get; set; }

        /// <summary>
        ///审核者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_chk_code", MedName = "rep_audit_user_id", WFName = "Pma_audit_Buser_id")]
        public String RepAuditUserId { get; set; }

        /// <summary>
        ///发送者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_send_code", MedName = "rep_send_user_id", WFName = "Pma_send_Buser_id")]
        public String RepSendUserId { get; set; }

        /// <summary>
        ///报告者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_report_code", MedName = "rep_report_user_id", WFName = "Pma_report_Buser_id")]
        public String RepReportUserId { get; set; }

        /// <summary>
        ///报告类别（0-常规 1-急查）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_ctype", MedName = "rep_ctype", WFName = "Pma_ctype")]
        public String RepCtype { get; set; }

        /// <summary>
        ///发送标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_send_flag", MedName = "rep_send_flag", WFName = "Pma_send_flag")]
        public Int32 RepSendFlag { get; set; }

        /// <summary>
        ///打印标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_prt_flag", MedName = "rep_print_flag", WFName = "Pma_print_flag")]
        public Int32? RepPrintFlag { get; set; }

        /// <summary>
        ///状态 0-未审核 1-已审核 2-已报告 4-已打印
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_flag", MedName = "rep_status", WFName = "Pma_status")]
        public Int32? RepStatus { get; set; }

        /// <summary>
        ///传染病上传标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_reg_flag", MedName = "rep_disease_flag", WFName = "Pma_disease_flag")]
        public Int32 RepDiseaseFlag { get; set; }

        /// <summary>
        ///危急值标志 1-未查看 2-已查看
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_urgent_flag", MedName = "rep_urgent_flag", WFName = "Pma_urgent_flag")]
        public Int32? RepUrgentFlag { get; set; }

        /// <summary>
        ///查看者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_look_code", MedName = "rep_read_user_id", WFName = "Pma_read_Buser_id")]
        public String RepReadUserId { get; set; }

        /// <summary>
        ///备注/评价
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_exp", MedName = "rep_remark", WFName = "Pma_remark")]
        public String RepRemark { get; set; }

        /// <summary>
        ///输入ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_pid", MedName = "rep_input_id", WFName = "Pma_input_id")]
        public String RepInputId { get; set; }

        /// <summary>
        ///录入日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_date", MedName = "rep_in_date", WFName = "Pma_in_date")]
        public DateTime? RepInDate { get; set; }



        /// <summary>
        ///送检日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sdate", MedName = "samp_send_date", WFName = "Pma_sam_send_date")]
        public DateTime? SampSendDate { get; set; }

        /// <summary>
        ///接收日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_rec_date", MedName = "samp_recive_date", WFName = "Pma_recive_date")]
        public DateTime? SampReciveDate { get; set; }

        /// <summary>
        ///审核日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_chk_date", MedName = "rep_audit_date", WFName = "Pma_audit_date")]
        public DateTime? RepAuditDate { get; set; }

        /// <summary>
        ///报告日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_report_date", MedName = "rep_report_date", WFName = "Pma_report_date")]
        public DateTime? RepReportDate { get; set; }

        /// <summary>
        ///发送日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_send_date", MedName = "rep_send_date", WFName = "Pma_send_date")]
        public DateTime? RepSendDate { get; set; }

        /// <summary>
        ///查看日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_look_date", MedName = "rep_read_date", WFName = "Pma_read_date")]
        public DateTime? RepReadDate { get; set; }

        /// <summary>
        ///医保卡号/诊疗卡号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_social_no", MedName = "pid_social_no", WFName = "Pma_social_no")]
        public String PidSocialNo { get; set; }

        /// <summary>
        ///体检ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_emp_id", MedName = "pid_exam_no", WFName = "Pma_exam_no")]
        public String PidExamNo { get; set; }

        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_bar_code", MedName = "rep_bar_code", WFName = "Pma_bar_code")]
        public String RepBarCode { get; set; }

        /// <summary>
        ///序号（双向）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_host_order", MedName = "rep_serial_num", WFName = "Pma_serial_num")]
        public String RepSerialNum { get; set; }

        /// <summary>
        ///试管架（双向）
        /// </summary>   
        [FieldMapAttribute(ClabName = "Pat_etagere", MedName = "samp_tube_rack", WFName = "Pma_Drack_id")]
        public String SampTubeRack { get; set; }

        /// <summary>
        ///试管孔号（双向）
        /// </summary>   
        [FieldMapAttribute(ClabName = "Pat_place", MedName = "samp_rack_pos", WFName = "Pma_rack_pos")]
        public String SampRackPos { get; set; }

        /// <summary>
        ///样本采集时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sample_date", MedName = "samp_collection_date", WFName = "Pma_collection_date")]
        public DateTime? SampCollectionDate { get; set; }

        /// <summary>
        ///申请时间(目前在检验管理中此字段与"样本接收时间"掉乱)
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_apply_date", MedName = "samp_apply_date", WFName = "Pma_apply_date")]
        public DateTime? SampApplyDate { get; set; }

        /// <summary>
        ///检验时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_jy_date", MedName = "samp_check_date", WFName = "Pma_check_date")]
        public DateTime? SampCheckDate { get; set; }

        /// <summary>
        ///报告打印时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "Pat_prt_date", MedName = "rep_print_date", WFName = "Pma_print_date")]
        public DateTime? RepPrintDate { get; set; }

        /// <summary>
        ///采集部位
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sample_part", MedName = "collection_part", WFName = "Pma_collection_part")]
        public String CollectionPart { get; set; }

        /// <summary>
        ///病人来源类型 对应dict_origin
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_ori_id", MedName = "pid_src_id", WFName = "Pma_pat_Dsorc_id")]
        public String PidSrcId { get; set; }

        /// <summary>
        ///仪器检测信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_mid_info", MedName = "rep_itr_analysis", WFName = "Pma_itr_analysis")]
        public String RepItrAnalysis { get; set; }

        /// <summary>
        ///处理意见
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_comment", MedName = "rep_comment", WFName = "Pma_comment")]
        public String RepComment { get; set; }

        /// <summary>
        ///医院编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_hospital_id", MedName = "pid_org_id", WFName = "Pma_pat_Dorg_id")]
        public String PidOrgId { get; set; }

        /// <summary>
        ///修改次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_modified_times", MedName = "rep_modify_frequency", WFName = "Pma_modify_frequency")]
        public Int32 RepModifyFrequency { get; set; }

        /// <summary>
        ///费用类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_fee_type", MedName = "pid_insu_id", WFName = "Pma_pat_Dinsu_id")]
        public String PidInsuId { get; set; }

        /// <summary>
        ///标本备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sam_rem", MedName = "samp_remark", WFName = "Pma_sam_remark")]
        public String SampRemark { get; set; }

        /// <summary>
        ///自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_key", MedName = "rep_sn", WFName = "Pma_id", DBIdentity = true)]
        public Int64 RepSn { get; set; }

        /// <summary>
        ///样本接收时间(目前在检验管理中此字段与"申请时间"掉乱)
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sample_receive_date", MedName = "samp_receive_date", WFName = "Pma_sam_receive_date")]
        public DateTime? SampReceiveDate { get; set; }

        /// <summary>
        ///科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_dep_name", MedName = "pid_dept_name", WFName = "Pma_pat_dept_name")]
        public String PidDeptName { get; set; }

        /// <summary>
        ///病区名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_ward_name", MedName = "pid_ward_name", WFName = "Pma_pat_ward_name")]
        public String PidWardName { get; set; }

        /// <summary>
        ///科室代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_dep_code", MedName = "pid_dept_code", WFName = "Pma_pat_dept_code")]
        public String PidDeptCode { get; set; }

        /// <summary>
        ///病区代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_ward_id", MedName = "pid_ward_id", WFName = "Pma_pat_ward_id")]
        public String PidWardId { get; set; }

        /// <summary>
        ///申请号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_app_no", MedName = "pid_apply_no", WFName = "Pma_apply_no")]
        public String PidApplyNo { get; set; }

        /// <summary>
        ///医生名字
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_doc_name", MedName = "pid_doc_name", WFName = "Pma_doctor_name")]
        public String PidDocName { get; set; }

        /// <summary>
        ///复查标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_recheck_flag", MedName = "rep_recheck_flag", WFName = "Pma_recheck_flag")]
        public Int32 RepRecheckFlag { get; set; }

        /// <summary>
        ///体检单位名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_emp_company_name", MedName = "pid_exam_company", WFName = "Pma_pat_exam_company")]
        public String PidExamCompany { get; set; }

        /// <summary>
        ///送达时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_reach_date", MedName = "samp_reach_date", WFName = "Pma_sam_reach_date")]
        public DateTime? SampReachDate { get; set; }

        /// <summary>
        ///危急值标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_critical", MedName = "rep_danger_flag", WFName = "Pma_danger_flag")]
        public Boolean RepDangerFlag { get; set; }

        /// <summary>
        ///独特ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_upid", MedName = "pid_unique_id", WFName = "Pma_unique_id")]
        public String PidUniqueId { get; set; }

        /// <summary>
        ///仪器审计标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_itr_audit_flag", MedName = "rep_audit_way", WFName = "Pma_itr_audit_flag")]
        public Int32 RepAuditWay { get; set; }

        /// <summary>
        ///备份标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_mid_flag", MedName = "rep_temp_flag", WFName = "Pma_temp_flag")]
        public String RepTempFlag { get; set; }

        /// <summary>
        ///身份
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_identity", MedName = "pid_identity", WFName = "Pma_identity")]
        public Int32 PidIdentity { get; set; }

        /// <summary>
        ///身份证号码
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_identity_card", MedName = "pid_identity_card", WFName = "Pma_identity_card")]
        public string PidIdentityCard { get; set; }

        /// <summary>
        ///病人身份
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_identity_name", MedName = "pid_identity_name", WFName = "Pma_identity_name")]
        public String PidIdentityName { get; set; }

        /// <summary>
        ///初始标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_pre_flag", MedName = "rep_initial_flag", WFName = "Pma_initial_flag")]
        public Int32 RepInitialFlag { get; set; }

        /// <summary>
        ///初始时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_pre_date", MedName = "rep_initial_date", WFName = "Pma_initial_date")]
        public DateTime? RepInitialDate { get; set; }

        /// <summary>
        ///初始者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_pre_code", MedName = "rep_initial_user_id", WFName = "Pma_initial_user_id")]
        public String RepInitialUserId { get; set; }

        /// <summary>
        ///生日
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_birthday", MedName = "pid_birthday", WFName = "Pma_pat_birthday")]
        public DateTime? PidBirthday { get; set; }

        /// <summary>
        ///抗药标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_drugfast", MedName = "rep_drugfast_flag", WFName = "Pma_drugfast_flag")]
        public Int32 RepDrugfastFlag { get; set; }

        /// <summary>
        ///特征描述
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_discribe", MedName = "rep_discribe", WFName = "Pma_discribe")]
        public string RepDiscribe { get; set; }

        /// <summary>
        /// HIS病人流水号
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_his_serialnum", MedName = "Pma_his_serialnum", WFName = "Pma_his_serialnum")]
        public String HISSerialnum { get; set; }

        /// <summary>
        /// HIS病人ID
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_his_patientid", MedName = "Pma_his_patientid", WFName = "Pma_his_patientid")]
        public String HISPatientID { get; set; }

        /// <summary>
        /// 微生物中期报告标志
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_micreport_flag", MedName = "Pma_micreport_flag", WFName = "Pma_micreport_flag")]
        public Int32 MicReportFlag { get; set; }

        /// <summary>
        /// 微生物中期报告时间
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_micreport_date", MedName = "Pma_micreport_date", WFName = "Pma_micreport_date")]
        public DateTime? MicReportDate { get; set; }

        /// <summary>
        /// 微生物中期报告审核人工号
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_micreport_chkuserid", MedName = "Pma_micreport_chkuserid", WFName = "Pma_micreport_chkuserid")]
        public String MicReportChkUserID { get; set; }

        /// <summary>
        /// 微生物中期报告检验人工号
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_micreport_senduserid", MedName = "Pma_micreport_senduserid", WFName = "Pma_micreport_senduserid")]
        public String MicReportSendUserID { get; set; }


        #region 附加字段 专业组ID
        /// <summary>
        ///专业组ID
        /// </summary>
        [FieldMapAttribute(ClabName = "com_ptype", MedName = "com_pri_id",  WFName = "Dcom_Dpro_id", DBColumn = false)]
        public String ComPriId { get; set; }
        #endregion


        #region 附加字段 录入者
        /// <summary>
        ///录入者
        /// </summary>
        [FieldMapAttribute(ClabName = "lrName", MedName = "lrName", WFName = "lrName", DBColumn = false)]
        public String LrName { get; set; }
        #endregion

        #region 附加字段 审核者
        /// <summary>
        ///审核者
        /// </summary>
        [FieldMapAttribute(ClabName = "shName", MedName = "shName", WFName = "shName", DBColumn = false)]
        public String ShName { get; set; }

        /// <summary>
        /// 审核者（细菌申请用）
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_check_name", MedName = "pat_check_name", WFName = "pat_check_name", DBColumn = false)]
        public String RepAuditUserName { get; set; }

        #endregion

        #region 附加字段 报告者
        /// <summary>
        ///报告者（细菌申请）
        /// </summary>
        [FieldMapAttribute(ClabName = "bgName", MedName = "bgName", WFName = "bgName", DBColumn = false)]
        public String BgName { get; set; }

        /// <summary>
        ///报告者
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_report_name", MedName = "pat_report_name", WFName = "pat_report_name", DBColumn = false)]
        public String RepReportUserName { get; set; }
        #endregion


        #region 附加字段 查看者名称
        /// <summary>
        ///查看者名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_look_name", MedName = "pat_look_name", WFName = "pat_look_name", DBColumn = false)]
        public String PatLookName { get; set; }
        #endregion

        #region 附加字段 序号
        /// <summary>
        ///序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_seq", MedName = "sort_no", WFName = "sort_no", DBColumn = false)]
        public String PatSort { get; set; }
        #endregion

        #region 附加字段 是否选中
        /// <summary>
        ///是否选中
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_select", MedName = "pat_select", WFName = "pat_select", DBColumn = false)]
        public Boolean PatSelect { get; set; }
        #endregion

        #region 附加字段 专业组
        /// <summary>
        ///专业组
        /// </summary>  
        [FieldMapAttribute(ClabName = "itr_ptype", MedName = "itr_pro_id", WFName = "Ditr_Dpro_id", DBColumn = false)]
        public String ItrProId { get; set; }
        #endregion

        #region 附加字段 仪器代码
        /// <summary>
        ///仪器代码
        /// </summary>  
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItrEname { get; set; }
        #endregion

        #region 附加字段 仪器名称
        /// <summary>
        ///仪器名称
        /// </summary>  
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name", DBColumn = false)]
        public String ItrName { get; set; }
        #endregion

        #region 附加字段 标本类别
        /// <summary>
        ///标本类别
        /// </summary>  
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name", DBColumn = false)]
        public String SamName { get; set; }
        #endregion

        #region 附加字段 检查目的名称
        /// <summary>
        ///检查目的名称
        /// </summary>  
        [FieldMapAttribute(ClabName = "chk_cname", MedName = "purp_name", WFName = "Dpurp_name", DBColumn = false)]
        public String PurpName { get; set; }
        #endregion

        #region 附加字段 医生名字
        /// <summary>
        ///医生名字
        /// </summary>  
        [FieldMapAttribute(ClabName = "doc_name", MedName = "doctor_name", WFName = "Ddoctor_name", DBColumn = false)]
        public String DoctorName { get; set; }
        #endregion

        #region 附加字段 用户名称
        /// <summary>
        ///用户名称
        /// </summary>  
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public String UserName { get; set; }
        #endregion

        #region 附加字段 用户名称
        /// <summary>
        ///用户名称
        /// </summary>  
        public string PatICodeName
        {
            get
            {
                if (UserName == null) { return RepCheckUserId; }
                else { return UserName; }
            }
        }
        #endregion

        #region 附加字段 标识名称
        /// <summary>
        ///标识名称
        /// </summary>  
        [FieldMapAttribute(ClabName = "no_name", MedName = "idt_name", WFName = "Didt_name", DBColumn = false)]
        public String IdtName { get; set; }
        #endregion

        #region 附加字段 来源名称
        /// <summary>
        ///来源名称
        /// </summary>  
        [FieldMapAttribute(ClabName = "ori_name", MedName = "src_name", WFName = "Dsorc_name", DBColumn = false)]
        public String SrcName { get; set; }
        #endregion

        #region 附加字段 检查类型名称
        /// <summary>
        ///检查类型名称
        /// </summary>  
        public String PatCtypeName { get; set; }
        #endregion

        #region 附加字段 标本状态
        /// <summary>
        ///标本状态samp_type
        /// </summary>  
        [FieldMapAttribute(ClabName = "bc_status", MedName = "samp_status_id", WFName = "Sma_status_id", DBColumn = false)]
        public String BcStatus { get; set; }
        #endregion

        #region 附加字段 类别
        /// <summary>
        ///类别
        /// </summary>  
        [FieldMapAttribute(ClabName = "bc_ctype", MedName = "samp_type", WFName = "Sdet_type", DBColumn = false)]
        public String SampType { get; set; }
        #endregion

        #region 附加字段 条码打印标志
        /// <summary>
        ///条码打印标志
        /// </summary>  
        [FieldMapAttribute(ClabName = "bc_print_flag", MedName = "samp_print_flag", WFName = "Sma_print_flag", DBColumn = false)]
        public String SampPrintFlag { get; set; }
        #endregion

        #region 附加字段 科室电话
        /// <summary>
        ///电话
        /// </summary>  
        [FieldMapAttribute(ClabName = "dep_tel", MedName = "dept_tel", WFName = "Ddept_tel", DBColumn = false)]
        public String DeptTel { get; set; }
        #endregion

        #region 附加字段 保存拆分大组合(特殊合并)ID
        /// <summary>
        ///保存拆分大组合(特殊合并)ID
        /// </summary>  
        [FieldMapAttribute(ClabName = "bc_merge_comid", MedName = "samp_merge_com_id", WFName = "Sma_merge_com_id", DBColumn = false)]
        public String BcMergeComid { get; set; }
        #endregion

        #region 附加字段 格式化年龄
        /// <summary>
        ///格式化年龄
        /// </summary>  
        public String PatAgeTxt { get; set; }
        #endregion

        #region 附加字段 性别名称
        /// <summary>
        ///性别名称
        /// </summary>  
        public String PidSexName
        {
            get
            {
                if (PidSex == "1") { return "男"; }
                else if (PidSex == "2") { return "女"; }
                else
                {
                    return "未知";
                }
            }
        }
        #endregion


        #region 附加字段 报告类别名称
        /// <summary>
        ///报告类别名称
        /// </summary>  
        public String RepCtypeName
        {
            get
            {
                if (RepCtype == "1") { return "常规"; }
                else if (RepCtype == "2") { return "急查"; }
                else if (RepCtype == "3") { return "保密"; }
                else if (RepCtype == "4") { return "溶栓"; }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region 附加字段 是否外部报告
        /// <summary>
        ///是否外部报告
        /// </summary>
        [FieldMapAttribute(ClabName = "isOuterReport", MedName = "isOuterReport", WFName = "isOuterReport", DBColumn = false)]
        public String IsOuterReport { get; set; }
        #endregion

        #region 附加字段 报告类型编码
        /// <summary>
        ///报告类型编码
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_rep_id", MedName = "itr_report_id", WFName = "Ditr_report_id", DBColumn = false)]
        public String ItrReportId { get; set; }
        #endregion

        #region 附加字段 数据类型 01-普通 02-酶标  03-细菌 04-描述 05-过敏源 06-新生儿筛查 07-骨髓
        /// <summary>
        ///数据类型 01-普通 02-酶标  03-细菌 04-描述 05-过敏源 06-新生儿筛查 07-骨髓
        /// </summary>  
        [FieldMapAttribute(ClabName = "itr_rep_flag", MedName = "itr_report_type", WFName = "Ditr_report_type", DBColumn = false)]
        public String ItrReportType { get; set; }
        #endregion


        #region 附加字段  科室名称
        /// <summary>
        /// 科室名称
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_name", MedName = "dept_name", WFName = "Ddept_name", DBColumn = false)]
        public string DeptName { get; set; }
        #endregion

        #region 附加字段  危急值结果
        /// <summary>
        /// 危急值结果
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_result", MedName = "pid_result", WFName = "pid_result", DBColumn = false)]
        public string PidResult { get; set; }
        #endregion

        #region 附加字段  审核人工号
        /// <summary>
        /// 审核人工号
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_chk_number", MedName = "pid_chk_number", WFName = "pid_chk_number", DBColumn = false)]
        public string PidChkNumber { get; set; }
        #endregion

        #region 附加字段  审核人
        /// <summary>
        /// 审核人
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_chk_name", MedName = "pid_chk_name", WFName = "pid_chk_name", DBColumn = false)]
        public string PidChkName { get; set; }
        #endregion

        #region 附加字段  中期报告人姓名
        /// <summary>
        /// 中期报告人姓名
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_micreport_chkname", MedName = "Pma_micreport_chkname", WFName = "Pma_micreport_chkname", DBColumn = false)]
        public string MicReportChkUserName { get; set; }
        #endregion

        #region 附加字段  中期报告人姓名
        /// <summary>
        /// 中期报告人姓名
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_micreport_sendname", MedName = "Pma_micreport_sendname", WFName = "Pma_micreport_sendname", DBColumn = false)]
        public string MicReportSendUserName { get; set; }
        #endregion

        #region 附加字段  物理组
        /// <summary>
        /// 物理组
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_type", MedName = "itr_lab_id", WFName = "Ditr_lab_id", DBColumn = false)]
        public string ItrLabId { get; set; }
        #endregion

        #region 附加字段  （危急值消息表）扩展字段4
        /// <summary>
        /// （危急值消息表）扩展字段4
        /// </summary>
        [FieldMapAttribute(ClabName = "msg_ext4", MedName = "obr_value_d", WFName = "Lmsg_extend_d", DBColumn = false)]
        public string ObrValueD { get; set; }
        #endregion

        #region 附加字段  确认时间差
        /// <summary>
        /// 确认时间差
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_datediff", MedName = "pid_datediff", WFName = "pid_datediff", DBColumn = false)]
        public string PidDatediff { get; set; }
        #endregion

        #region 附加字段  确认方式
        /// <summary>
        /// 确认方式
        /// </summary>
        [FieldMapAttribute(ClabName = "affirm_mode", MedName = "affirm_mode", WFName = "affirm_mode", DBColumn = false)]
        public string AffirmMode { get; set; }
        #endregion

        #region 附加字段  备注1
        /// <summary>
        /// 备注1
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_res", MedName = "pid_res", WFName = "pid_res", DBColumn = false)]
        public string PidRes { get; set; }
        #endregion

        #region 附加字段  备注2
        /// <summary>
        /// 备注2
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_res2", MedName = "pid_res2", WFName = "pid_res2", DBColumn = false)]
        public string PidRes2 { get; set; }
        #endregion

        #region 附加字段  危急值信息ID
        /// <summary>
        /// 危急值信息ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_id", MedName = "obr_id_msg", WFName = "obr_id_msg", DBColumn = false)]
        public String ObrIdMsg { get; set; }
        #endregion

        #region 附加字段  危机值消息类型
        /// <summary>
        /// 危机值消息类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_type", MedName = "obr_type", WFName = "Lmsg_type", DBColumn = false)]
        public Int32 ObrType { get; set; }
        #endregion

        #region 附加字段  （信息标志）
        /// <summary>
        /// （信息标志）
        /// </summary>   
        [FieldMapAttribute(ClabName = "msf_insgin_flag", MedName = "obr_inside_flag", WFName = "Lmsg_inside_flag", DBColumn = false)]
        public String ObrInsideFlag { get; set; }
        #endregion

        #region 附加字段  信息类型文本
        /// <summary>
        /// 信息类型文本
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_type_txt", MedName = "msg_type_txt", WFName = "msg_type_txt", DBColumn = false)]
        public String MsgTypeTxt { get; set; }
        #endregion

        #region 附加字段  消息创建时间
        /// <summary>
        /// 消息创建时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_create_time", MedName = "obr_create_time", WFName = "Lmsg_create_time", DBColumn = false)]
        public DateTime? ObrCreateTime { get; set; }
        #endregion

        #region 附加字段 医生联系电话
        /// <summary>
        /// 医生联系电话
        /// </summary>   
        [FieldMapAttribute(ClabName = "doc_tel", MedName = "doctor_tel", WFName = "Ddoctor_tel", DBColumn = false)]
        public String DoctorTel { get; set; }
        #endregion

        #region 附加字段
        /// <summary>
        /// 服务器时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "server_date", MedName = "server_date", WFName = "server_date", DBColumn = false)]
        public DateTime? ServerDate { get; set; }
        #endregion

        #region 附加字段 信息发送部门编码
        /// <summary>
        /// 信息发送部门编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg_send_depcode", MedName = "obr_send_dept_code", WFName = "Lmsg_send_dept_code", DBColumn = false)]
        public String ObrSendDeptCode { get; set; }
        #endregion

        #region 附加字段 检验报告ID
        /// <summary>
        ///检验报告ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_id", MedName = "obr_id", WFName = "Lmsg_id", DBColumn = false)]
        public String ObrId { get; set; }
        #endregion

        #region 附加字段 医院ID
        /// <summary>
        ///医院ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "hos_id", MedName = "hos_id", WFName = "hos_id", DBColumn = false)]
        public String HosId { get; set; }
        #endregion

        #region 附加字段 医院名称
        /// <summary>
        ///医院名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "hos_name", MedName = "hos_name", WFName = "hos_name", DBColumn = false)]
        public String HosName { get; set; }
        #endregion

        /// <summary>
        /// 是否有检验报告 1-有 0-没有
        /// </summary>
        [FieldMapAttribute(ClabName = "hasresult", MedName = "hasresult", WFName = "hasresult", DBColumn = false)]
        public String HasResult{get;set;}

        /// <summary>
        /// 是否有检验报告 1-有 0-没有
        /// </summary>
        [FieldMapAttribute(ClabName = "hasresult2", MedName = "hasresult2", WFName = "hasresult2", DBColumn = false)]
        public Int32 HasResult2{ get; set; }

        /// <summary>
        /// 是否已录入危急值处理记录
        /// </summary>
        [FieldMapAttribute(ClabName = "urgent_msg_handle", MedName = "urgent_msg_handle", WFName = "urgent_msg_handle", DBColumn = false)]
        public Int32 UrgentMsgHandle { get; set; }

        /// <summary>
        /// 危急值数量
        /// </summary>
        [FieldMapAttribute(ClabName = "urgent_count", MedName = "urgent_count", WFName = "urgent_count", DBColumn = false)]
        public Int32 UrgentCount { get; set; }

        /// <summary>
        /// 修改标志
        /// </summary>
        [FieldMapAttribute(ClabName = "modify_flag", MedName = "modify_flag", WFName = "modify_flag", DBColumn = false)]
        public String ModifyFlag { get; set; }

       

        #region 附加字段 项目ID
        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_id", MedName = "itm_id", WFName = "Ditm_id", DBColumn = false)]
        public String ResItmId { get; set; }
        #endregion

        #region 附加字段 整数型样本号
        /// <summary>
        ///整数型样本号
        /// </summary>  
        [FieldMapAttribute(ClabName = "pat_sid_int", MedName = "pat_sid_int", WFName = "pat_sid_int", DBColumn = false)]
        public Int64 PatSidInt { get; set; }
        #endregion


        #region 附加字段 整数型序号
        /// <summary>
        ///整数型序号（双向）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_host_order_int", MedName = "rep_serial_num_int", WFName = "rep_serial_num_int", DBColumn =false)]
        public Int64 RepSerialNumInt { get; set; }
        #endregion

        #region 附加字段 状态名称
        /// <summary>
        /// 状态名称
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_flag_name", MedName = "rep_status_name", WFName = "rep_status_name", DBColumn = false)]
        public String RepStatusName { get; set; }
        #endregion

        
        #region 附加字段 状态名称分组
        /// <summary>
        /// 状态名称
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_state_name", MedName = "rep_state_name", WFName = "rep_state_name", DBColumn = false)]
        public String RepStateName { get; set; }
        #endregion

        #region 附加字段 LIS组合编码
        /// <summary>
        ///LIS组合编码
        /// </summary>  
        [FieldMapAttribute(ClabName = "bc_lis_code", MedName = "com_id", WFName = "Dcom_id", DBColumn = false)]
        public String ComId { get; set; }
        #endregion

        #region 附加字段 条码项目明细的条码号
        /// <summary>
        ///条码项目明细的条码号
        /// </summary>  
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "samp_bar_code", WFName = "Sma_bar_code", DBColumn = false)]
        public String SampBarCode { get; set; }
        #endregion

        #region 附加字段 紧急状态颜色列
        /// <summary>
        ///紧急状态颜色列
        /// </summary>  
        [FieldMapAttribute(ClabName = "com_line_color", MedName = "com_line_color", WFName = "Dcom_online_clolr", DBColumn = false)]
        public Int32 ComLineColor { get; set; }
        #endregion

        #region 附加字段 结果状态
        /// <summary>
        ///结果状态
        /// </summary>  
        [FieldMapAttribute(ClabName = "resstatus", MedName = "resstatus", WFName = "resstatus", DBColumn = false)]
        public Int32 ResStatus { get; set; }
        #endregion

        #region 附加字段 结果表的报告ID是否存在
        /// <summary>
        ///结果表的报告ID是否存在
        /// </summary>  
        [FieldMapAttribute(ClabName = "status", MedName = "status", WFName = "status", DBColumn = false)]
        public Int32 Status { get; set; }
        #endregion

        #region 附加字段 结果表危急值是否存在
        /// <summary>
        ///结果表危急值是否存在
        /// </summary>  
        [FieldMapAttribute(ClabName = "UrgStatus", MedName = "UrgStatus", WFName = "UrgStatus", DBColumn = false)]
        public Int32 UrgStatus { get; set; }
        #endregion

        #region 附加字段 消息备注
        /// <summary>
        ///消息备注
        /// </summary>  
        [FieldMapAttribute(ClabName = "msg_content", MedName = "msg_content", WFName = "msg_content", DBColumn = false)]
        public string MsgContent { get; set; }
        #endregion

        #region 附加字段 LIS组合名称
        /// <summary>
        ///LIS组合名称
        /// </summary>  
        [FieldMapAttribute(ClabName = "bc_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public String ComName { get; set; }
        #endregion

        #region 附加字段 错误信息
        /// <summary>
        /// 错误信息,资料修改模块用
        /// </summary>
        public String ErrorMessage { get; set; }
        #endregion

        #region 附加字段 是否复制组合
        /// <summary>
        /// 是否复制组合
        /// </summary>
        public String IsCopyCombine { get; set; }
        #endregion

        #region 附加字段 原病人ID
        /// <summary>
        /// 原病人ID
        /// </summary>
        public String RepIdOld { get; set; }
        #endregion

        #region 附加字段 旧病人仪器ID
        /// <summary>
        /// 旧病人仪器ID
        /// </summary>
        public String RepItrIdOld { get; set; }
        #endregion


        #region 附加字段 年龄显示的值
        [FieldMapAttribute(ClabName = "pat_age_str", MedName = "pid_age_str", WFName = "pid_age_str", DBColumn = false)]
        public String PidAgeStr { get; set; }
        #endregion

        #region 附加字段 报告日期与当前时间差(分)
        [FieldMapAttribute(ClabName = "msg_add_time", MedName = "msg_add_time", WFName = "msg_add_time", DBColumn = false)]
        public Int32 MsgAddTime { get; set; }
        #endregion

        #region 附加字段  （危急值消息表）扩展字段5
        /// <summary>
        /// 扩展字段5
        /// </summary>
        [FieldMapAttribute(ClabName = "msg_ext5", MedName = "obr_value_e", WFName = "obr_value_e", DBColumn = false)]
        public string ObrValueE { get; set; }
        #endregion

        #region 附加字段  （危急值消息表）扩展字段5
        /// <summary>
        /// 扩展字段5
        /// </summary>
        [FieldMapAttribute(ClabName = "msg_send_depname", MedName = "obr_send_depname", WFName = "obr_send_depname", DBColumn = false)]
        public string ObrSendDepname { get; set; }
        #endregion

        #region 附加字段 目标样本号
        public string DestRepSid { get; set; }
        #endregion

        #region 附加字段 目标录入日期
        public DateTime DestRepInDate { get; set; }
        #endregion

        #region 附加字段 目标仪器ID
        public string DestRepItrId { get; set; }
        #endregion

        #region 附加字段 目标标识ID
        public string DestRepId { get; set; }
        #endregion

        #region 附加字段 是否所有项目合并
        /// <summary>
        /// 是否所有项目合并 0不全部项目都合并 1全部项目都合并
        /// </summary>
        public bool DestAllItem { get; set; }
        #endregion

        #region 附加字段 要和并的项目集合
        public List<string> DestItmIds { get; set; }
        #endregion

        #region 录入者姓名
        /// <summary>
        ///录入者姓名
        /// </summary>  
        [FieldMapAttribute(ClabName = "pat_i_code_name", MedName = "pat_i_code_name", WFName = "pat_i_code_name", DBColumn = false)]
        public string RepCheckUserName { get; set; }
        #endregion

        #region tat时间
        /// <summary>
        ///tat时间
        /// </summary>  
        [FieldMapAttribute(ClabName = "TatTime", MedName = "TatTime", WFName = "TatTime", DBColumn = false)]
        public string TatTime { get; set; }
        #endregion

        #region tat时间间隔
        /// <summary>
        ///tat时间间隔
        /// </summary>  
        public string TatComTime { get; set; }
        #endregion

        /// <summary>
        /// 病人组合明细
        /// </summary>
        public List<EntityPidReportDetail> ListPidReportDetail { get; set; }

        /// <summary>
        /// 新病人ID，资料模板修改时用
        /// </summary>
        public String NewRepId { get; set; }

        #region 附加字段 自动审核
        /// <summary>
        ///自动审核状态
        /// </summary>  
        public String AutoAuditStatus
        {
            get
            {
                if (RepAuditWay == 0)
                    return "未审核";
                if (RepAuditWay == 1)
                    return "未通过";
                if (RepAuditWay == 2)
                    return "已通过";
                return string.Empty;

            }
        }

        public String AutoAuditText { get; set; }

        #endregion

        #region 附加字段 护士确认 医生确认
        /// <summary>
        /// 护士确认
        /// </summary>
        [FieldMapAttribute(ClabName = "msg_nurse_read_flag", MedName = "obr_nurse_read_flag", WFName = "Lmsg_nurse_read_flag", DBColumn = false)]
        public String ObrNurseReadFlag { get; set; }

        /// <summary>
        /// 医生确认
        /// </summary>
        [FieldMapAttribute(ClabName = "msg_doctor_read_flag", MedName = "obr_doctor_read_flag", WFName = "Lmsg_doctor_read_flag", DBColumn = false)]
        public String ObrDoctorReadFlag { get; set; }
        #endregion


        #region  报告解读
        /// <summary>
        /// 报告解读--属于数据库字段，但是不参与增改字段
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_sum_info", MedName = "rep_sum_info", WFName = "rep_sum_info", DBColumn = false)]
        public String RepSumInfo { get; set; }
        #endregion


        /// <summary>
        /// 查询报告开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 查询报告结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
