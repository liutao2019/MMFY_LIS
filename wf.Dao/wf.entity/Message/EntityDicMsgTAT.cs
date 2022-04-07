using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// TATMsg危急值信息表数据
    /// </summary>
    [Serializable]
    public class EntityDicMsgTAT : EntityBase
    {
        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "samp_bar_code", WFName = "Sma_bar_code")]
        public String SampBarCode { get; set; }

        /// <summary>
        ///病人类别编码(门诊、住院，对应dict_origin表)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ori_id", MedName = "pid_src_id", WFName = "Sma_pat_src_id")]
        public String PidSrcId { get; set; }

        /// <summary>
        ///条码生成日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_date", MedName = "samp_date", WFName = "Sdet_date")]
        public DateTime SampDate { get; set; }

        /// <summary>
        ///最后操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lastaction_time", MedName = "samp_lastaction_date", WFName = "Sma_lastoper_date")]
        public DateTime SampLastactionDate { get; set; }

        /// <summary>
        ///急查标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_urgent_flagStr", MedName = "samp_urgent_flagStr", WFName = "samp_urgent_flagStr")]
        public Boolean SampUrgentFlagStr { get; set; }

        /// <summary>
        ///HIS科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_name", MedName = "pid_dept_name", WFName = "Sma_pat_dept_name")]
        public String PidDeptName { get; set; }

        /// <summary>
        ///项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_com_name", MedName = "bc_com_name", WFName = "bc_com_name")]
        public String BcComName { get; set; }

        /// <summary>
        ///姓名(条码主索引表字段)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "pid_name", WFName = "Sma_pat_name")]
        public String PidName { get; set; }

        /// <summary>
        ///年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_age", MedName = "pid_age", WFName = "Sma_pat_age")]
        public String PidAge { get; set; }

        /// <summary>
        ///性别(0-未知 1-男 2-女)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sex", MedName = "pid_sex", WFName = "Sma_pat_sex")]
        public String PidSex { get; set; }

        /// <summary>
        ///床号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bed_no", MedName = "pid_bed_no", WFName = "Sma_pat_bed_no")]
        public String PidBedNo { get; set; }

        /// <summary>
        ///病人标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_in_no", MedName = "pid_in_no", WFName = "Sma_pat_in_no")]
        public String PidInNo { get; set; }

        /// <summary>
        /// UPID唯一号 目前滨海使用
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_upid", MedName = "pid_unique_id", WFName = "Sma_pat_unique_id")]
        public String PidUniqueId { get; set; }

        /// <summary>
        ///备注(注意事项)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_exp", MedName = "samp_remark", WFName = "Sma_remark")]
        public String SampRemark { get; set; }

        /// <summary>
        ///类别(预留用)-物理组
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ctype", MedName = "samp_type", WFName = "Sma_type")]
        public String SampType { get; set; }

        /// <summary>
        ///开单医生姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_doct_name", MedName = "pid_doctor_name", WFName = "Sma_doctor_name")]
        public String PidDoctorName { get; set; }

        /// <summary>
        ///条码医嘱时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_occ_date", MedName = "samp_occ_date", WFName = "Sma_occ_date")]
        public DateTime SampOccDate { get; set; }

        /// <summary>
        ///诊断
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_diag", MedName = "pid_diag", WFName = "Sma_pat_diag")]
        public String PidDiag { get; set; }

        /// <summary>
        ///就诊次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_times", MedName = "pid_admiss_times", WFName = "Sma_pat_admiss_times")]
        public Decimal PidAdmissTimes { get; set; }

        /// <summary>
        ///标本状态  (0-未打印,1-打印,2-采集, 3-已收取,4-已送检,5-签收,6-检验中,7-已检验)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_status", MedName = "samp_status_id", WFName = "Sma_status_id")]
        public String SampStatusId { get; set; }

        /// <summary>
        /// 组别名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name")]
        public string ProName { get; set; }

        /// <summary>
        ///采集容器名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_name", MedName = "tub_name", WFName = "Dtub_name")]
        public String TubName { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name")]
        public string SamName { get; set; }

        /// <summary>
        ///急查标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_urgent_flag", MedName = "samp_urgent_flag", WFName = "Sma_urgent_flag")]
        public Boolean SampUrgentFlag { get; set; }

        /// <summary>
        ///（TAT时间）类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_time_type", MedName = "com_time_type", WFName = "Dtr_type")]
        public String ComTimeType { get; set; }

        /// <summary>
        ///间隔时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "time_tat", MedName = "time_tat", WFName = "time_tat")]
        public Int32? TimeTat { get; set; }

        /// <summary>
        ///间隔天数
        /// </summary>   
        [FieldMapAttribute(ClabName = "time_mi", MedName = "time_mi", WFName = "time_mi")]
        public Int32? TimeMi { get; set; }

        /// <summary>
        ///状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "status", MedName = "status", WFName = "status")]
        public String Status { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sid", MedName = "rep_sid", WFName = "Pma_sid")]
        public String RepSid { get; set; }

        /// <summary>
        ///报告类别（0-常规 1-急查）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_ctype", MedName = "rep_ctype", WFName = "Pma_ctype")]
        public String RepCtype { get; set; }

        /// <summary>
        ///报告类别名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_ctype_name", MedName = "rep_ctype_name", WFName = "rep_ctype_name")]
        public String RepCtypeName { get; set; }

        /// <summary>
        ///仪器代码(仪器字典)
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename")]
        public String ItrEname { get; set; }

        /// <summary>
        ///超时天数
        /// </summary>   
        [FieldMapAttribute(ClabName = "over_tat", MedName = "over_tat", WFName = "over_tat")]
        public Int32 OverTat { get; set; }

        /// <summary>
        ///检验科接收日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_receiver_date", MedName = "receiver_date", WFName = "Sma_receiver_date")]
        public DateTime? ReceiverDate { get; set; }

        /// <summary>
        ///检验时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_jy_date", MedName = "samp_check_date", WFName = "Pma_check_date")]
        public DateTime? SampCheckDate { get; set; }

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
        ///仪器代码（病人信息表）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_itr_id", MedName = "rep_itr_id", WFName = "Pma_Ditr_id")]
        public String RepItrId { get; set; }

        /// <summary>
        ///TAT预警时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "time_tatw", MedName = "time_tatw", WFName = "time_tatw")]
        public Decimal? TimeTatw { get; set; }

        /// <summary>
        ///今天的分钟数
        /// </summary>   
        [FieldMapAttribute(ClabName = "time_today", MedName = "time_today", WFName = "time_today")]
        public Int32? TimeToday { get; set; }

        /// <summary>
        ///第几个星期
        /// </summary>   
        [FieldMapAttribute(ClabName = "time_weekday", MedName = "time_weekday", WFName = "time_weekday")]
        public Int32? TimeWeekday { get; set; }

        /// <summary>
        ///超时天数
        /// </summary>   
        [FieldMapAttribute(ClabName = "time_mi_over", MedName = "time_mi_over", WFName = "time_mi_over")]
        public Decimal? TimeMiOver { get; set; }

        /// <summary>
        ///(病人)标识ID 
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_id", MedName = "rep_id", WFName = "Pma_rep_id")]
        public String RepId { get; set; }

        /// <summary>
        ///录入日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_date", MedName = "rep_in_date", WFName = "Pma_in_date")]
        public DateTime? RepInDate { get; set; }

        /// <summary>
        ///状态 0-未审核 1-已审核 2-已报告
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_flag", MedName = "rep_status", WFName = "Pma_status")]
        public Int32? RepStatus { get; set; }

        /// <summary>
        ///最大TAT超时数
        /// </summary>
        [FieldMapAttribute(ClabName = "max_tat", MedName = "max_tat", WFName = "max_tat")]
        public Int32 MaxTat { get; set; }

        /// <summary>
        ///序号（双向）
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_host_order", MedName = "rep_serial_num", WFName = "Pma_serial_num")]
        public String RepSerialNum { get; set; }

        /// <summary>
        ///物理组
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_type", MedName = "itr_type", WFName = "itr_type")]
        public String ItrType { get; set; }

        /// <summary>
        ///标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "merge_flag", MedName = "merge_flag", WFName = "merge_flag")]
        public Int32 MergeFlag { get; set; }

        /// <summary>
        ///统计数
        /// </summary>   
        [FieldMapAttribute(ClabName = "pmi_c", MedName = "pmi_c", WFName = "pmi_c")]
        public Int32 PmiC { get; set; }

        /// <summary>
        ///标本采集时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood_date", MedName = "collection_date", WFName = "Sma_collection_date")]
        public DateTime? CollectionDate { get; set; }

        /// <summary>
        ///病人姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_name", MedName = "pid_name_new", WFName = "pid_name_new")]
        public String PidNameNew { get; set; }
    }
}
