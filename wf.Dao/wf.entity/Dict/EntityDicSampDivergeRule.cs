using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///组合明细拆分表
    ///旧表名:Def_samp_diverge_rule 新表名:Rel_sample_diverge_rule
    /// </summary>
    [Serializable()]
    public class EntityDicSampDivergeRule : EntityBase
    {
        /// <summary>
        ///大组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Rsdr_Dcom_id")]
        public String ComId { get; set; }

        /// <summary>
        ///小组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_s_id", MedName = "com_split_id", WFName = "Rsdr_split_id")]
        public String ComSplitId { get; set; }

        #region 附加字段
        /// <summary>
        ///组合名称
        /// </summary>
        public String ComName { get; set; }
        /// <summary>
        ///大组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Dcom_id")]
        public String DComId { get; set; }
        #endregion
    }
}
