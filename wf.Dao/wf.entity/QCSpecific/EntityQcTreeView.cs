using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 质控控制图表树形显示实体
    /// </summary>
    [Serializable]
    public class EntityQcTreeView : EntityBase
    {
        /// <summary>
        /// 节点主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_id", MedName = "pro_id", WFName = "Dpro_id")]
        public String TvId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name")]
        public String TvName { get; set; }

        /// <summary>
        /// 关联ID（主副节点关联ID）
        /// </summary>   
        [FieldMapAttribute(ClabName = "parentId", MedName = "func_parentId", WFName = "func_parentId")]
        public String TvParentId { get; set; }

        /// <summary>
        /// 靶值
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_c_x", MedName = "type_c_x", WFName = "type_c_x")]
        public Decimal? TvMatItmX { get; set; }

        /// <summary>
        /// 标准差
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_sd", MedName = "type_sd", WFName = "type_sd")]
        public Decimal? TvMatItmSd { get; set; }

        /// <summary>
        /// 变异系数
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_ccv", MedName = "type_ccv", WFName = "type_ccv")]
        public Decimal? TvMatItmCcv { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_id", MedName = "mat_id", WFName = "Rmatdet_Dmat_id")]
        public String TvMatItmId { get; set; }

        /// <summary>
        /// 主副节点标识（0父节点1子节点）
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_type", MedName = "type_type", WFName = "type_type")]
        public String TvType { get; set; }

        /// <summary>
        /// 有效期开始日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_sdate", MedName = "mat_date_start", WFName = "Dmat_date_start")]
        public DateTime? TvSDate { get; set; }

        /// <summary>
        /// 有效期结束日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_edate", MedName = "mat_date_end", WFName = "Dmat_date_end")]
        public DateTime? TvEDate { get; set; }

        /// <summary>
        /// CV%
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_cv", MedName = "mat_itm_cv", WFName = "Rmatdet_itm_cv")]
        public Decimal? TvMatItmCv { get; set; }

        /// <summary>
        /// 允许CV
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_allow_cv", MedName = "mat_allow_cv", WFName = "Rmatdet_allow_cv")]
        public Decimal? TvMatAllowCv { get; set; }

        /// <summary>
        /// 默认规则
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_rule", MedName = "mat_rule_id", WFName = "mat_rule_id")]
        public String TvMatRuleId { get; set; }

        /// <summary>
        /// 最大值(半定量)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_max_value", MedName = "mat_max_value", WFName = "Rmatdet_max_value")]
        public Decimal? TvMatMaxValue { get; set; }

        /// <summary>
        /// 最小值(半定量)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_min_value", MedName = "mat_min_value", WFName = "Rmatdet_min_value")]
        public Decimal? TvMatMinValue { get; set; }
        /// <summary>
        /// 测定类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_value_type", MedName = "mat_value_type", WFName = "Rmatdet_value_type")]
        public String TvMatValueType { get; set; }

        /// <summary>
        /// 使用开始日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qc_par_detail_sdate", MedName = "mat_date_start", WFName = "qc_par_detail_sdate")]
        public DateTime? TvMatDateStart { get; set; }

        /// <summary>
        /// 试剂厂商
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_reag_manu", MedName = "mat_reag_manufacturer", WFName = "Rmatdet_reag_manufacturer")]
        public String TvMatReagManufacturer { get; set; }

        /// <summary>
        /// 测定方法
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_m_pro", MedName = "mat_m_pro", WFName = "Rmatdet_m_pro")]
        public String TvMatMPro { get; set; }

        /// <summary>
        /// 试剂有效日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_reag_date", MedName = "mat_read_valid_date", WFName = "Rmatdet_read_valid_date")]
        public DateTime? TvMatReadValidDate { get; set; }

        /// <summary>
        /// 项目排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "seq", MedName = "seq", WFName = "seq")]
        public int TvSeq { get; set; }



    }

    public enum QcTreeViewType
    {
        多水平,
        多项目
    }
}
