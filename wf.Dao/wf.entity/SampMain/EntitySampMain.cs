using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 条码主索引表
    /// 旧表名:Samp_main 新表名:Sample_main
    /// </summary>
    [Serializable]
    public class EntitySampMain : EntityBase
    {
        public EntitySampMain()
        {
            SampDate = DateTime.Now;
            SampOccDate = DateTime.Now;
            SampLastactionDate = DateTime.Now;
            PidAdmissTimes = 0;
            SampPrintFlag = 0;
            SampMinCapcity = 0;
            SampBarBatchNo = 0;
            SampUrgentStatus = 0;
            ListSampDetail = new List<EntitySampDetail>();
            ListSampProcessDetail = new List<EntitySampProcessDetail>();
        }

        /// <summary>
        ///自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "samp_sn", WFName = "Sma_id", DBIdentity = true)]
        public Int32 SampSn { get; set; }

        /// <summary>
        ///内部关联ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_no", MedName = "samp_bar_id", WFName = "Sma_bar_id")]
        public String SampBarId { get; set; }

        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "samp_bar_code", WFName = "Sma_bar_code")]
        public String SampBarCode { get; set; }

        /// <summary>
        ///条码批次
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_frequency", MedName = "samp_bar_batch_no", WFName = "Sma_bar_batch_no")]
        public Int64 SampBarBatchNo { get; set; }

        /// <summary>
        ///病人标识类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_no_id", MedName = "pid_idt_id", WFName = "Sma_pat_idt_id")]
        public String PidIdtId { get; set; }

        /// <summary>
        ///病人标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_in_no", MedName = "pid_in_no", WFName = "Sma_pat_in_no")]
        public String PidInNo { get; set; }

        /// <summary>
        ///床号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bed_no", MedName = "pid_bed_no", WFName = "Sma_pat_bed_no")]
        public String PidBedNo { get; set; }

        /// <summary>
        ///姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "pid_name", WFName = "Sma_pat_name")]
        public String PidName { get; set; }

        /// <summary>
        ///性别(0-未知 1-男 2-女)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sex", MedName = "pid_sex", WFName = "Sma_pat_sex")]
        public String PidSex { get; set; }

        /// <summary>
        ///年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_age", MedName = "pid_age", WFName = "Sma_pat_age")]
        public String PidAge { get; set; }

        /// <summary>
        ///HIS科室编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_code", MedName = "pid_dept_code", WFName = "Sma_pat_dept_code")]
        public String PidDeptCode { get; set; }

        /// <summary>
        ///HIS科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_name", MedName = "pid_dept_name", WFName = "Sma_pat_dept_name")]
        public String PidDeptName { get; set; }

        /// <summary>
        ///诊断
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_diag", MedName = "pid_diag", WFName = "Sma_pat_diag")]
        public String PidDiag { get; set; }

        /// <summary>
        ///开单医生工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_doct_code", MedName = "pid_doctor_code", WFName = "Sma_doctor_code")]
        public String PidDoctorCode { get; set; }

        /// <summary>
        ///开单医生姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_doct_name", MedName = "pid_doctor_name", WFName = "Sma_doctor_name")]
        public String PidDoctorName { get; set; }

        /// <summary>
        ///组合累加显示名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_his_name", MedName = "samp_com_name", WFName = "Sma_com_name")]
        public String SampComName { get; set; }

        /// <summary>
        ///标本编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_id", MedName = "samp_sam_id", WFName = "Sma_Dsam_id")]
        public String SampSamId { get; set; }

        /// <summary>
        ///标本名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_name", MedName = "samp_sam_name", WFName = "Sma_Dsam_name")]
        public String SampSamName { get; set; }

        /// <summary>
        ///条码生成日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_date", MedName = "samp_date", WFName = "Sma_date")]
        public DateTime SampDate { get; set; }

        /// <summary>
        ///就诊次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_times", MedName = "pid_admiss_times", WFName = "Sma_pat_admiss_times")]
        public Decimal PidAdmissTimes { get; set; }

        /// <summary>
        ///条码打印标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_print_flag", MedName = "samp_print_flag", WFName = "Sma_print_flag")]
        public Int32 SampPrintFlag { get; set; }

        /// <summary>
        ///打印时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_print_date", MedName = "samp_print_date", WFName = "Sma_print_date")]
        public DateTime? SampPrintDate { get; set; }

        /// <summary>
        ///打印者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_print_code", MedName = "samp_print_user_id", WFName = "Sma_print_user_id")]
        public String SampPrintUserId { get; set; }

        /// <summary>
        ///打印者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_print_name", MedName = "samp_print_user_name", WFName = "Sma_print_user_name")]
        public String SampPrintUserName { get; set; }

        /// <summary>
        ///采集容器编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_cuv_code", MedName = "samp_tub_code", WFName = "Sma_tub_code")]
        public String SampTubCode { get; set; }

        /// <summary>
        ///采集容器名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_cuv_name", MedName = "samp_tub_name", WFName = "Sma_tub_name")]
        public String SampTubName { get; set; }

        /// <summary>
        ///当前条码最小采集量
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_cap_sum", MedName = "samp_min_capcity", WFName = "Sma_min_capcity")]
        public Decimal SampMinCapcity { get; set; }

        /// <summary>
        ///采集量单位(固定单位 L、ML)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_cap_unit", MedName = "samp_capcity_unit", WFName = "Sma_capcity_unit")]
        public String SampCapcityUnit { get; set; }

        /// <summary>
        ///急查标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_urgent_flag", MedName = "samp_urgent_flag", WFName = "Sma_urgent_flag")]
        public Boolean SampUrgentFlag { get; set; }

        /// <summary>
        ///物理组
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ctype", MedName = "samp_type", WFName = "Sma_type")]
        public String SampType { get; set; }

        /// <summary>
        ///病人类别编码(门诊、住院，对应dict_origin表)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ori_id", MedName = "pid_src_id", WFName = "Sma_pat_src_id")]
        public String PidSrcId { get; set; }

        /// <summary>
        ///病人类别名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ori_name", MedName = "pid_src_name", WFName = "Sma_pat_src_name")]
        public String PidSrcName { get; set; }

        /// <summary>
        ///检验科接收标志(0-未接收 1-已接收)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_receiver_flag", MedName = "receiver_flag", WFName = "Sma_receiver_flag")]
        public Int32 ReceiverFlag { get; set; }

        /// <summary>
        ///检验科接收日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_receiver_date", MedName = "receiver_date", WFName = "Sma_receiver_date")]
        public DateTime? ReceiverDate { get; set; }

        /// <summary>
        ///检验科接收者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_receiver_code", MedName = "receiver_user_id", WFName = "Sma_receiver_user_id")]
        public String ReceiverUserId { get; set; }

        /// <summary>
        ///检验科接收者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_receiver_name", MedName = "receiver_user_name", WFName = "Sma_receiver_user_name")]
        public String ReceiverUserName { get; set; }

        /// <summary>
        ///标本采集时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood_flag", MedName = "collection_flag", WFName = "Sma_collection_flag")]
        public Int32 CollectionFlag { get; set; }

        /// <summary>
        ///标本采集时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood_date", MedName = "collection_date", WFName = "Sma_collection_date")]
        public DateTime? CollectionDate { get; set; }

        /// <summary>
        ///标本采集者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood_code", MedName = "collection_user_id", WFName = "Sma_collection_user_id")]
        public String CollectionUserId { get; set; }

        /// <summary>
        ///标本采集者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood_name", MedName = "collection_user_name", WFName = "Sma_collection_user_name")]
        public String CollectionUserName { get; set; }

        /// <summary>
        ///标本送检标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_send_flag", MedName = "send_flag", WFName = "Sma_send_flag")]
        public Int32 SendFlag { get; set; }

        /// <summary>
        ///标本送检时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_send_date", MedName = "send_date", WFName = "Sma_send_date")]
        public DateTime? SendDate { get; set; }

        /// <summary>
        ///标本送检者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_send_code", MedName = "send_user_id", WFName = "Sma_user_id")]
        public String SendUserId { get; set; }

        /// <summary>
        ///标本送检者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_send_name", MedName = "send_user_name", WFName = "Sma_user_name")]
        public String SendUserName { get; set; }

        /// <summary>
        ///标本送达标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_reach_flag", MedName = "reach_flag", WFName = "Sma_reach_flag")]
        public Int32 ReachFlag { get; set; }

        /// <summary>
        ///标本送达时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_reach_date", MedName = "reach_date", WFName = "Sma_reach_date")]
        public DateTime? ReachDate { get; set; }

        /// <summary>
        ///标本送达者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_reach_code", MedName = "reach_user_id", WFName = "Sma_reach_user_id")]
        public String ReachUserId { get; set; }

        /// <summary>
        ///标本送达者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_reach_name", MedName = "reach_user_name", WFName = "Sma_reach_user_name")]
        public String ReachUserName { get; set; }

        /// <summary>
        ///备注(注意事项)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_exp", MedName = "samp_remark", WFName = "Sma_remark")]
        public String SampRemark { get; set; }

        /// <summary>
        ///下载计算机
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_computer", MedName = "samp_computer", WFName = "Sma_computer")]
        public String SampComputer { get; set; }

        /// <summary>
        ///HIS卡号/医保卡(预留用)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_social_no", MedName = "pid_social_no", WFName = "Sma_social_no")]
        public String PidSocialNo { get; set; }

        /// <summary>
        ///体检病人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_emp_id", MedName = "pid_exam_no", WFName = "Sma_exam_no")]
        public String PidExamNo { get; set; }

        /// <summary>
        ///预留字段(现记录条码生成方式。手工条码会录入标识在此字段)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_info", MedName = "samp_info", WFName = "Sma_info")]
        public String SampInfo { get; set; }

        /// <summary>
        ///医院编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_hospital_id", MedName = "pid_org_id", WFName = "Sma_Dorg_id")]
        public String PidOrgId { get; set; }

        /// <summary>
        ///条码类型(0-打印条码 1-预制条码)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_type", MedName = "samp_bar_type", WFName = "Sma_bar_type")]
        public Int32 SampBarType { get; set; }

        /// <summary>
        ///标本状态
        ///0-未打印,1-打印,2-采集, 3-已收取,4-已送检,5-签收,6-检验中,
        ///7-已检验,8-二次送检,9-条码回退,20-资料登记,530-删除病人资料,
        ///40-一审,50-一审反审,60-二审,70-二审反审
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_status", MedName = "samp_status_id", WFName = "Sma_status_id")]
        public String SampStatusId { get; set; }

        /// <summary>
        ///状态中文简称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_status_cname", MedName = "samp_status_name", WFName = "Sma_status_name")]
        public String SampStatusName { get; set; }

        /// <summary>
        ///删除标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        ///病人出生日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_birthday", MedName = "pid_birthday", WFName = "Sma_pat_birthday")]
        public DateTime? PidBirthday { get; set; }

        /// <summary>
        ///抽血标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood", MedName = "samp_blood_flag", WFName = "Sma_blood_flag")]
        public Boolean SampBloodFlag { get; set; }

        /// <summary>
        ///条码医嘱时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_occ_date", MedName = "samp_occ_date", WFName = "Sma_occ_date")]
        public DateTime SampOccDate { get; set; }

        /// <summary>
        ///标本备注ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_rem_id", MedName = "samp_rem_id", WFName = "Sma_rem_id")]
        public String SampRemId { get; set; }

        /// <summary>
        ///标本备注名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_rem_name", MedName = "samp_rem_content", WFName = "Sma_rem_content")]
        public String SampRemContent { get; set; }

        /// <summary>
        ///回退标记
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_return_flag", MedName = "samp_return_flag", WFName = "Sma_return_flag")]
        public Boolean SampReturnFlag { get; set; }

        /// <summary>
        ///打印次数（读取字典,表示该条码需要打印几次）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_print_time", MedName = "samp_print_time", WFName = "Sma_print_time")]
        public Int32 SampPrintTime { get; set; }

        /// <summary>
        ///最后操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lastaction_time", MedName = "samp_lastaction_date", WFName = "Sma_lastoper_date")]
        public DateTime SampLastactionDate { get; set; }

        /// <summary>
        ///回退次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_return_times", MedName = "samp_return_times", WFName = "Sma_return_times")]
        public Int32 SampReturnTimes { get; set; }

        /// <summary>
        ///地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_address", MedName = "pid_address", WFName = "Sma_pat_address")]
        public String PidAddress { get; set; }

        /// <summary>
        ///联系电话
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_tel", MedName = "pid_tel", WFName = "Sma_pat_tel")]
        public String PidTel { get; set; }

        /// <summary>
        ///第三方系统ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_pid", MedName = "pid_patno", WFName = "Sma_pat_id")]
        public String PidPatno { get; set; }

        /// <summary>
        ///体检单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_emp_company", MedName = "pid_exam_company", WFName = "Sma_pat_exam_company")]
        public String PidExamCompany { get; set; }

        /// <summary>
        ///二次送检时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_secondsign_date", MedName = "second_send_date", WFName = "Sma_second_send_date")]
        public DateTime? SecondSendDate { get; set; }

        /// <summary>
        /// 二次送检者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_secondsign_code", MedName = "second_send_user_id", WFName = "Sma_second_send_user_id")]
        public String SecondSendUserId { get; set; }

        /// <summary>
        ///二次送检者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_secondsign_name", MedName = "second_send_user_name", WFName = "Sma_second_send_user_name")]
        public String SecondSendUserName { get; set; }

        /// <summary>
        ///二次送检标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_secondsign_flag", MedName = "second_send_flag", WFName = "Sma_second_send_flag")]
        public String SecondSendFlag { get; set; }

        /// <summary>
        ///申请单号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_app_no", MedName = "samp_apply_no", WFName = "Sma_apply_no")]
        public String SampApplyNo { get; set; }

        /// <summary>
        ///费用类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_fee_type", MedName = "pid_insu_id", WFName = "Sma_pat_Dinsu_id")]
        public String PidInsuId { get; set; }

        /// <summary>
        ///标本送检地
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_dest", MedName = "samp_send_dest", WFName = "Sma_send_dest")]
        public String SampSendDest { get; set; }

        /// <summary>
        ///体检单位名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_emp_company_name", MedName = "pid_exam_company_name", WFName = "Sma_exam_company_name")]
        public String PidExamCompanyName { get; set; }

        /// <summary>
        ///体检单位部门
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_emp_company_dept", MedName = "pid_exam_company_dept", WFName = "Sma_pat_exam_company_dept")]
        public String PidExamCompanyDept { get; set; }

        /// <summary>
        ///处理标记 仪器用
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_pre_sendflag", MedName = "samp_pre_process", WFName = "Sma_pre_process")]
        public Int32 SampPreProcess { get; set; }


        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_notice", MedName = "samp_collection_notice", WFName = "Sma_collection_notice")]
        public String SampCollectionNotice { get; set; }

        /// <summary>
        /// UPID唯一号 目前滨海使用
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_upid", MedName = "pid_unique_id", WFName = "Sma_pat_unique_id")]
        public String PidUniqueId { get; set; }

        /// <summary>
        ///保存拆分大组合(特殊合并)ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_merge_comid", MedName = "samp_merge_com_id", WFName = "Sma_merge_com_id")]
        public String SampMergeComId { get; set; }

        /// <summary>
        ///人员身份
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_identity", MedName = "pid_identity", WFName = "Sma_identity")]
        public Int32 PidIdentity { get; set; }

        /// <summary>
        ///证件号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_identity_card", MedName = "pid_identity_card", WFName = "Sma_identity_card")]
        public String PidIdentityCard { get; set; }

        /// <summary>
        /// 证件类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_identity_name", MedName = "pid_identity_name", WFName = "Sma_identity_name")]
        public string PidIdentityName { get; set; }

        /// <summary>
        ///急查状态(急，脑卒中，非急：1,2,0)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_urgent_status", MedName = "samp_urgent_status", WFName = "Sma_urgent_status")]
        public Int32 SampUrgentStatus { get; set; }

        /// <summary>
        ///最后操作地点
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lastaction_ope_place", MedName = "samp_lastaction_place", WFName = "Sma_lastoper_place")]
        public String SampLastactionPlace { get; set; }

        /// <summary>
        ///最后操作人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lastaction_ope_code", MedName = "samp_lastaction_user_id", WFName = "Sma_lastoper_Buser_id")]
        public String SampLastactionUserId { get; set; }

        /// <summary>
        ///二次签收标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_out_flag", MedName = "second_receive_flag", WFName = "Sma_second_receive_flag")]
        public Int32 SecondReceiveFlag { get; set; }

        /// <summary>
        ///批号 --茂名妇幼用此字段保存登记流水号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_batch_barcode", MedName = "samp_pack_no", WFName = "Sma_pack_no")]
        public String SampPackNo { get; set; }

        /// <summary>
        ///国籍 --（茂名妇幼导出COVID-19需要新增）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_nationality", MedName = "samp_nationality", WFName = "Sma_nationality")]
        public String SampNation { get; set; }

        /// <summary>
        ///户籍地 --（茂名妇幼导出COVID-19需要新增）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_registy_address", MedName = "samp_registy_address", WFName = "Sma_registy_address")]
        public String SampRegistyAddress { get; set; }

        /// <summary>
        ///人员身份 --（茂名妇幼导出COVID-19需要新增）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_pat_type", MedName = "samp_pat_type", WFName = "Sma_pat_type")]
        public String SampPatType { get; set; }

        /// <summary>
        ///参保类型 --（茂名妇幼导出COVID-19需要新增）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_medical_type", MedName = "samp_medical_type", WFName = "Sma_medicaltype_name")]
        public String SampMedicaltypeName { get; set; }

        /// <summary>
        ///参保地 --（茂名妇幼导出COVID-19需要新增）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_medical_place", MedName = "samp_medical_place", WFName = "Sma_medical_place")]
        public String SampMdeicalPlace { get; set; }

        /// <summary>
        ///险种 --（茂名妇幼导出COVID-19需要新增）
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_insurance_name", MedName = "samp_insurance_name", WFName = "Sma_insurance_name")]
        public String SampInsuranceName { get; set; }

        /// <summary>
        /// 粤核酸总码
        /// </summary>
        [FieldMapAttribute(ClabName = "bc_yhs_BarCode", MedName = "samp_yhs_BarCode", WFName = "Sma_yhs_BarCode")]
        public String SampYhsBarCode { get; set; }

        #region 附加字段 组别信息

        /// <summary>
        /// 组别名称
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public string ProName { get; set; }

        #endregion

        #region 附件字段 来源信息

        /// <summary>
        /// 来源编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "ori_id", MedName = "src_id", WFName = "Dsorc_id", DBColumn = false)]
        public String SrcId { get; set; }

        /// <summary>
        /// 来源名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "ori_name", MedName = "src_name", WFName = "Dsorc_name", DBColumn = false)]
        public String SrcName { get; set; }

        #endregion

        #region 附件字段 试管信息

        /// <summary>
        ///采集容器名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_name", MedName = "tub_name", WFName = "Dtub_name", DBColumn = false)]
        public String TubName { get; set; }

        /// <summary>
        ///收费代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "cuv_fee_code", MedName = "tub_charge_code", WFName = "Dtub_charge_code", DBColumn = false)]
        public String TubChargeCode { get; set; }

        #endregion

        #region 附件字段 标本信息

        /// <summary>
        /// 标本名称
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name", DBColumn = false)]
        public string SamName { get; set; }

        /// <summary>
        /// 标本代码
        /// </summary>
        [FieldMapAttribute(ClabName = "sam_code", MedName = "sam_code", WFName = "Dsam_code", DBColumn = false)]
        public string SamCode { get; set; }

        #endregion

        #region 附件字段 病人信息

        /// <summary>
        /// 仪器名称
        /// </summary>
        [FieldMapAttribute(ClabName = "Ditr_name", MedName = "Ditr_name", WFName = "Ditr_name", DBColumn = false)]
        public string DitrName { get; set; }


        /// <summary>
        /// 样本号
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_sid", MedName = "Pma_sid", WFName = "Pma_sid", DBColumn = false)]
        public string Pma_sid { get; set; }

        #endregion

        #region 附件字段 标本备注信息

        /// <summary>
        /// 编码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "rem_id", MedName = "rem_id", WFName = "Dsamr_id", DBColumn = false)]
        public String RemId { get; set; }

        /// <summary>
        /// 标本备注
        /// </summary>                       
        [FieldMapAttribute(ClabName = "rem_cont", MedName = "rem_content", WFName = "Dsamr_content", DBColumn = false)]
        public String RemContent { get; set; }

        #endregion

        #region 附加字段 其他
        /// <summary>
        /// 组合状态颜色
        /// </summary>
        [FieldMapAttribute(ClabName = "com_line_color", MedName = "com_line_color", WFName = "com_line_color", DBColumn = false)]
        public String SampComLineColor { get; set; }

        /// <summary>
        /// 条码生成类型
        /// </summary>
        public String SampCreateType
        {
            get
            {
                if (SampInfo == "122")
                    return "手工";
                else
                    return "下载";
            }
        }

        /// <summary>
        /// 收费状态
        /// </summary>
        public String SampFeeType
        {
            get
            {
                if (PidUniqueId == "2")
                    return "成功";
                else if (PidUniqueId == "1")
                    return "失败";
                else if (PidUniqueId == "0")
                    return "取消";
                else
                    return string.Empty;
            }
        }

        /// <summary>
        /// 转换后年龄
        /// </summary>
        [FieldMapAttribute(ClabName = "barcode_age", MedName = "barcode_age", WFName = "barcode_age", DBColumn = false)]
        public String SampAge { get; set; }

        /// <summary>
        /// 条码总采集量
        /// </summary>
        [FieldMapAttribute(ClabName = "ComCapSum", MedName = "ComCapSum", WFName = "ComCapSum", DBColumn = false)]
        public String ComCapSum { get; set; }

        /// <summary>
        /// 条码总金额
        /// </summary>
        [FieldMapAttribute(ClabName = "bc_price", MedName = "bc_price", WFName = "bc_price", DBColumn = false)]
        public string SampPrice { get; set; }

        /// <summary>
        /// 科室电话
        /// </summary>
        [FieldMapAttribute(ClabName = "dep_tel", MedName = "dept_tel", WFName = "Ddept_tel", DBColumn = false)]
        public String  DeptTel { get; set; }

        /// <summary>
        /// 条码明细中，his的收费代码的合并
        /// </summary>
        [FieldMapAttribute(ClabName = "Dcom_his_codes", MedName = "Dcom_his_codes", WFName = "Dcom_his_codes", DBColumn = false)]
        public String DcomHisCodes { get; set; }

        /// <summary>
        /// 特殊转换后的备注
        /// </summary>
        public String SampleRemarks
        {
            get
            {
                if (string.IsNullOrEmpty(SampRemark))
                    return RemContent;
                else
                    return SampRemark;
            }
        }

        /// <summary>
        /// 选择
        /// </summary>
        public Boolean CheckMarkSelection { get; set; }


        /// <summary>
        /// 打印类型用来判断是否是外部条码
        /// </summary>
        public string PrintType { get; set; }
        #endregion

        #region 附加信息 条码项目明细

        /// <summary>
        /// 条码项目明细
        /// </summary>
        public List<EntitySampDetail> ListSampDetail { get; set; }

        #endregion

        #region 附加信息 条码流转信息

        /// <summary>
        /// 条码流转信息
        /// </summary>
        public List<EntitySampProcessDetail> ListSampProcessDetail { get; set; }

        #endregion
    }

}
