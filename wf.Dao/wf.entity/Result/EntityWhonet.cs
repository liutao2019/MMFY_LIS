using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// whonet导出实体
    /// </summary>
    [Serializable]
    public class EntityWhonet : EntityBase
    {
        /// <summary>
        ///抗生素WHO编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "anti_who_no", MedName = "ant_who_no", WFName = "ant_who_no")]
        public String AntWhoNo { get; set; }

        /// <summary>
        ///抗生素编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_aid", MedName = "obr_ant_id", WFName = "Lanti_Dant_id")]
        public String ObrAntId { get; set; }

        /// <summary>
        ///标识id
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_id", MedName = "obr_id", WFName = "Lanti_id")]
        public String ObrId { get; set; }
        /// <summary>
        ///细菌who编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bac_who_no", MedName = "bac_who_no", WFName = "Dbact_who_no")]
        public String BacWhoNo { get; set; }

        /// <summary>
        ///敏感度
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_mic", MedName = "obr_value", WFName = "Lanti_value")]
        public String ObrValue { get; set; }

        /// <summary>
        ///MIC值
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_smic1", MedName = "obr_value2", WFName = "Lanti_mic")]
        public String ObrValue2 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_smic2", MedName = "obr_value3", WFName = "Lanti_kb")]
        public String ObrValue3 { get; set; }

        /// <summary>
        ///细菌英文
        /// </summary>   
        [FieldMapAttribute(ClabName = "bac_ename", MedName = "bac_ename", WFName = "Dbact_ename")]
        public String BacEname { get; set; }

        /// <summary>
        ///细菌名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bac_cname", MedName = "bac_cname", WFName = "Dbact_cname")]
        public String BacCname { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "anr_mid", MedName = "obr_itr_id", WFName = "Lanti_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        ///录入日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_date", MedName = "rep_in_date", WFName = "Pma_in_date")]
        public DateTime RepInDate { get; set; }

        /// <summary>
        ///标本接收时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sample_receive_date", MedName = "samp_receive_date", WFName = "Pma_sam_receive_date")]
        public DateTime SampReceiveDate { get; set; }

        /// <summary>
        ///送检日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sdate", MedName = "samp_send_date", WFName = "Pma_sam_send_date")]
        public DateTime SampSendDate { get; set; }

        /// <summary>
        ///年龄
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_age_exp", MedName = "pid_age_exp", WFName = "Pma_pat_age_exp")]
        public String PidAgeExp { get; set; }

        /// <summary>
        ///姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_name", MedName = "pid_name", WFName = "Pma_pat_name")]
        public String PidName { get; set; }

        /// <summary>
        ///性别
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sex", MedName = "pid_sex", WFName = "Pma_pat_sex")]
        public String PidSex { get; set; }

        /// <summary>
        ///自增id
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_key", MedName = "rep_sn", WFName = "Pma_id")]
        public Int64 RepSn { get; set; }

        /// <summary>
        ///科室名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_dep_name", MedName = "pid_dept_name", WFName = "Pma_pat_dept_name")]
        public String PidDeptName { get; set; }

        /// <summary>
        ///查询码（对应His编码）
        /// </summary>   
        [FieldMapAttribute(ClabName = "dep_select_code", MedName = "org_id", WFName = "org_id")]
        public String OrgId { get; set; }

        /// <summary>
        ///科室输入码
        /// </summary>   
        [FieldMapAttribute(ClabName = "dep_incode", MedName = "dept_c_code", WFName = "dept_c_code")]
        public String DeptCCode { get; set; }

        /// <summary>
        ///科室his编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "dep_code", MedName = "dept_code", WFName = "Ddept_code")]
        public String DeptCode { get; set; }

        /// <summary>
        ///标本备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sam_rem", MedName = "samp_remark", WFName = "Pma_sam_remark")]
        public String SampRemark { get; set; }

        /// <summary>
        ///病人id
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_in_no", MedName = "pid_in_no", WFName = "Pma_pat_in_no")]
        public String PidInNo { get; set; }

        /// <summary>
        ///唯一号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_upid", MedName = "pid_unique_id", WFName = "Pma_unique_id")]
        public String PidUniqueId { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_sid", MedName = "rep_sid", WFName = "Pma_sid")]
        public String RepSid { get; set; }

        /// <summary>
        ///结果表备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_exp", MedName = "obr_remark", WFName = "obr_remark")]
        public String ObrRemark { get; set; }
        /// <summary>
        ///来源名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "ori_name", MedName = "src_name", WFName = "Dsorc_name")]
        public String SrcName { get; set; }
        /// <summary>
        ///标本Id
        /// </summary>   
        [FieldMapAttribute(ClabName = "sam_id", MedName = "sam_id", WFName = "Dsam_id")]
        public String SamId { get; set; }
        /// <summary>
        ///标本代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "sam_code", MedName = "sam_code", WFName = "Dsam_code")]
        public String SamCode { get; set; }
        /// <summary>
        ///标本类别输入码 
        /// </summary>   
        [FieldMapAttribute(ClabName = "sam_incode", MedName = "sam_c_code", WFName = "sam_c_code")]
        public String SamCCode { get; set; }
        /// <summary>
        ///标本类别
        /// </summary>   
        [FieldMapAttribute(ClabName = "sam_name", MedName = "sam_name", WFName = "Dsam_name")]
        public String SamName { get; set; }
    }
}
