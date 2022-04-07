using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 半定量
    /// 旧表名:Def_qc_convert  新表名:Rel_qc_convert
    /// </summary>
    [Serializable]
    public class EntityDicQcConvert : EntityBase
    {
        /// <summary>
        /// 主键ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_key", MedName = "cov_sn", WFName = "Rqcv_id")]
        public String CovSn { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_itr_id", MedName = "itr_id", WFName = "Rqcv_Ditr_id")]
        public String ItrId { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_itm_ecd", MedName = "itm_id", WFName = "Rqcv_Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        /// 显示值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_meas", MedName = "itm_value", WFName = "Rqcv_value")]
        public String ItmValue { get; set; }

        /// <summary>
        /// 实际值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_cast_meas", MedName = "itm_convert_value", WFName = "Rqcv_convert_value")]
        public String ItmConvertValue { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32? SortNo { get; set; }

    }
}
