using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// HIS-CIS医嘱数据类
    /// </summary>
    [Serializable]
    public class EntitySampOrderHL7 : EntityBase
    {
        /// <summary>
        ///病人标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_in_no", MedName = "pid_in_no",WFName = "pid_in_no")]
        public String PidInNo { get; set; }

        /// <summary>
        ///床号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bed_no", MedName = "pid_bed_no", WFName = "pid_bed_no")]
        public String PidBedNo { get; set; }

        /// <summary>
        ///姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "pid_name", WFName = "pid_name")]
        public String PidName { get; set; }

        /// <summary>
        ///性别(0-未知 1-男 2-女)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sex", MedName = "pid_sex", WFName = "pid_sex")]
        public String PidSex { get; set; }

        /// <summary>
        ///年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_age", MedName = "pid_age", WFName = "pid_age")]
        public String PidAge { get; set; }

        /// <summary>
        ///HIS科室编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_code", MedName = "pid_dept_code", WFName = "pid_dept_code")]
        public String PidDeptCode { get; set; }

        /// <summary>
        ///HIS科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_d_name", MedName = "pid_dept_name", WFName = "pid_dept_name")]
        public String PidDeptName { get; set; }

        /// <summary>
        /// 病区代码
        /// </summary>
        [FieldMapAttribute(ClabName = "bc_dep_ward", MedName = "pid_dept_ward", WFName = "pid_dept_ward")]
        public string PidDeptWard { get; set; }

        /// <summary>
        ///诊断
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_diag", MedName = "pid_diag", WFName = "pid_diag")]
        public String PidDiag { get; set; }

        /// <summary>
        ///开单医生工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_doct_code", MedName = "pid_doctor_code", WFName = "pid_doctor_code")]
        public String PidDoctorCode { get; set; }

        /// <summary>
        ///开单医生姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_doct_name", MedName = "pid_doctor_name", WFName = "pid_doctor_name")]
        public String PidDoctorName { get; set; }

        /// <summary>
        ///条码医嘱时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_occ_date", MedName = "samp_occ_date", WFName = "samp_occ_date")]
        public DateTime SampOccDate { get; set; }

        /// <summary>
        ///HIS卡号/医保卡(预留用)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_social_no", MedName = "pid_social_no", WFName = "pid_social_no")]
        public String PidSocialNo { get; set; }

        /// <summary>
        ///联系电话
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_tel", MedName = "pid_tel", WFName = "pid_tel")]
        public String PidTel { get; set; }

        /// <summary>
        ///标本编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_id", MedName = "samp_sam_id", WFName = "samp_sam_id")]
        public String SampSamId { get; set; }

        /// <summary>
        ///标本名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sam_name", MedName = "samp_sam_name", WFName = "samp_sam_name")]
        public String SampSamName { get; set; }

        /// <summary>
        /// HIS项目编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_his_code", MedName = "order_code", WFName = "order_code")]
        public String OrderCode { get; set; }

        /// <summary>
        /// HIS项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_his_name", MedName = "order_name", WFName = "order_name")]
        public String OrderName { get; set; }

        /// <summary>
        /// 医嘱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_yz_id", MedName = "order_sn", WFName = "order_sn")]
        public String OrderSn { get; set; }

        /// <summary>
        ///病人类别编码(门诊、住院，对应dict_origin表)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ori_id", MedName = "pid_src_id", WFName = "pid_src_id")]
        public String PidSrcId { get; set; }

        /// <summary>
        ///急查标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_urgent_flag", MedName = "samp_urgent_flag", WFName = "samp_urgent_flag")]
        public string SampUrgentFlag { get; set; }

        /// <summary>
        ///体检单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_emp_company", MedName = "pid_exam_company", WFName = "pid_exam_company")]
        public String PidExamCompany { get; set; }

        /// <summary>
        ///第三方系统ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_pid", MedName = "pid_pat_no", WFName = "pid_pat_no")]
        public String PidPatNo { get; set; }

        /// <summary>
        ///标本状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_status", MedName = "samp_status_id", WFName = "samp_status_id")]
        public String SampStatusId { get; set; }

        /// <summary>
        ///删除标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        /// UPID唯一号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_upid", MedName = "pid_unique_id", WFName = "pid_unique_id")]
        public String PidUniqueId { get; set; }

        /// <summary>
        ///标本备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_sample_remark", MedName = "samp_rem_content", WFName = "samp_rem_content")]
        public String SampRemContent { get; set; }

        /// <summary>
        ///就诊次数
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_times", MedName = "pid_admiss_times", WFName = "pid_admiss_times")]
        public Decimal PidAdmissTimes { get; set; }

        /// <summary>
        ///申请单号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_app_no", MedName = "samp_apply_no", WFName = "samp_apply_no")]
        public String SampApplyNo { get; set; }

        /// <summary>
        ///病人出生日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_birthday", MedName = "pid_birthday", WFName = "pid_birthday")]
        public string PidBirthday { get; set; }

        /// <summary>
        ///身份证
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_identity_card", MedName = "pid_identity_card", WFName = "pid_identity_card")]
        public string PidIdentityCard { get; set; }

        /// <summary>
        /// 医嘱类型
        /// </summary>
        public EnumSampOrderType EnumOrderType { get; set; }
    }
}

/// <summary>
/// 医嘱类型枚举
/// </summary>
[Serializable]
public enum EnumSampOrderType
{
    /// <summary>
    /// 新增医嘱
    /// </summary>
    NW = 0,

    /// <summary>
    /// 撤销医嘱
    /// </summary>
    CA = 1,

    /// <summary>
    /// 修改医嘱
    /// </summary>
    RU = 2
}