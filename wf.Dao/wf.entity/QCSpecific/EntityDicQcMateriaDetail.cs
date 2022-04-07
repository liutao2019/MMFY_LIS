using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///  质控项目表
    ///  旧表名:Def_qc_materia_detail  新表名:Rel_qc_materia_detail
    /// </summary>
    [Serializable]
    public class EntityDicQcMateriaDetail : EntityBase
    {
        /// <summary>
        ///  主键ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_id", MedName = "mat_det_id", WFName = "Rmatdet_id")]
        public String MatDetId { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_itr_id", MedName = "mat_itr_id", WFName = "Rmatdet_Ditr_id")]
        public String MatItrId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_itm_ecd", MedName = "mat_itm_id", WFName = "Rmatdet_Ditm_id")]
        public String MatItmId { get; set; }

        /// <summary>
        ///  项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_name", MedName = "mat_itm_name", WFName = "Rmatdet_Ditm_name")]
        public String MatItmName { get; set; }

        /// <summary>
        /// 质控物数量
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_num", MedName = "mat_amount", WFName = "Rmatdet_amount")]
        public Decimal? MatAmount { get; set; }

        /// <summary>
        /// 默认规则
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_rule", MedName = "mat_rule_id", WFName = "Rmatdet_rule")]
        public String MatRuleId { get; set; }

        /// <summary>
        /// 单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_unit", MedName = "mat_itm_unit", WFName = "Rmatdet_itm_unit")]
        public String MatItmUnit { get; set; }
        
        /// <summary>
        /// 质控项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_par_id", MedName = "mat_id", WFName = "Rmatdet_Dmat_id")]
        public String MatId { get; set; }

        /// <summary>
        /// 靶值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_x", MedName = "mat_itm_x", WFName = "Rmatdet_itm_x")]
        public Decimal? MatItmX { get; set; }

        /// <summary>
        /// 标准差
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_sd", MedName = "mat_itm_sd", WFName = "Rmatdet_itm_sd")]
        public Decimal? MatItmSd { get; set; }

        /// <summary>
        /// CCV
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_ccv", MedName = "mat_itm_ccv", WFName = "Rmatdet_itm_ccv")]
        public Decimal? MatItmCcv { get; set; }

        /// <summary>
        /// 试剂厂商
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_reag_manu", MedName = "mat_reag_manufacturer", WFName = "Rmatdet_reag_manufacturer")]
        public String MatReagManufacturer { get; set; }
        
        /// <summary>
        /// 试剂批号
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_reag_id", MedName = "mat_reag_batchno", WFName = "Rmatdet_reag_batchno")]
        public String MatReagBatchno { get; set; }

        /// <summary>
        /// 试剂有效日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_reag_date", MedName = "mat_read_valid_date", WFName = "Rmatdet_read_valid_date")]
        public DateTime MatReadValidDate { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_m_pro", MedName = "mat_m_pro", WFName = "Rmatdet_m_pro")]
        public String MatMPro { get; set; }

        /// <summary>
        /// CV%
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_cv", MedName = "mat_itm_cv", WFName = "Rmatdet_itm_cv")]
        public Decimal? MatItmCv { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_max_value", MedName = "mat_max_value", WFName = "Rmatdet_max_value")]
        public Decimal? MatMaxValue { get; set; }
        
        /// <summary>
        /// 最小值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_min_value", MedName = "mat_min_value", WFName = "Rmatdet_min_value")]
        public Decimal? MatMinValue { get; set; }

        /// <summary>
        /// 测定类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_value_type", MedName = "mat_value_type", WFName = "Rmatdet_value_type")]
        public String MatValueType { get; set; }

        /// <summary>
        /// 允许范围CV%
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_allow_cv", MedName = "mat_allow_cv", WFName = "Rmatdet_allow_cv")]
        public Decimal? MatAllowCv { get; set; }

        #region 附加字段 项目代码
        /// <summary>
        /// 项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode", DBColumn = false)]
        public String ItmEcode { get; set; }
        #endregion

        #region 附加字段 项目名称(界面用)  
        [FieldMapAttribute(ClabName = "qcr_itm_name", MedName = "qcr_itm_name", WFName = "qcr_itm_name", DBColumn = false)]
        public String QcrItmName { get; set; }
        #endregion

        #region 附加字段 (项目字典表)项目名称  
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public String ItmName { get; set; }
        #endregion

        #region 附加字段 拼音码  
        [FieldMapAttribute(ClabName = "itm_py", MedName = "py_code", WFName = "py_code", DBColumn = false)]
        public String PyCode { get; set; }
        #endregion

        #region 附加字段 五笔码  
        [FieldMapAttribute(ClabName = "itm_wb", MedName = "wb_code", WFName = "wb_code", DBColumn = false)]
        public String WbCode { get; set; }
        #endregion
        
    }
}
