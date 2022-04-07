using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 室间质控
    /// 旧表名:Obr_qc_exte_result 新表名:Lis_qc_eqa
    /// </summary>
    [Serializable()]
    public class EntityQcExteResult : EntityBase
    {
        /// <summary>
        /// 主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_bs", MedName = "qres_sn", WFName = "Leqa_id")]
        public String QresSn { get; set; }

        /// <summary>
        /// 质控物ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_sid", MedName = "qres_sid", WFName = "Leqa_Dmat_id")]
        public String QresSid { get; set; }

        /// <summary>
        /// 时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_date", MedName = "qres_date", WFName = "Leqa_date")]
        public DateTime QresDate { get; set; }

        /// <summary>
        ///  项目编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_itm_ecd", MedName = "qres_itm_id", WFName = "Leqa_Ditm_id")]
        public String QresItmId { get; set; }

        /// <summary>
        /// 结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_meas", MedName = "qres_value", WFName = "Leqa_value")]
        public String QresValue { get; set; }

        /// <summary>
        /// 靶值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_c_x", MedName = "qres_itm_x", WFName = "Leqa_itm_x")]
        public Decimal QresItmX { get; set; }

        /// <summary>
        /// 标准差
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_s_t", MedName = "qres_itm_sd", WFName = "Leqa_itm_sd")]
        public Decimal QresItmSd { get; set; }

        /// <summary>
        /// CCV
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_cv", MedName = "qres_itm_cv", WFName = "Leqa_itm_cv")]
        public Decimal QresItmCv { get; set; }

        /// <summary>
        /// PT上线
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_pt_l", MedName = "qres_pt_lower_limit", WFName = "Leqa_pt_lower_limit")]
        public Decimal QresPtLowerLimit { get; set; }

        /// <summary>
        /// PT下线
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_pt_t", MedName = "qres_pt_upper_limit", WFName = "Leqa_pt_upper_limit")]
        public Decimal QresPtUpperLimit { get; set; }

        /// <summary>
        /// PT得分
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_pt", MedName = "qres_pt_fraction", WFName = "Leqa_pt_fraction")]
        public Decimal QresPtFraction { get; set; }

        /// <summary>
        /// 失控原因
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_reson", MedName = "qres_reasons", WFName = "Leqa_reasons")]
        public String QresReasons { get; set; }

        /// <summary>
        /// 解决措施
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_fun", MedName = "qres_process", WFName = "Leqa_process")]
        public String QresProcess { get; set; }

    }
}
