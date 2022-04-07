using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 质控规则关联表
    /// 旧表名:Def_qc_materia_rule  新表名:Rel_qc_materia_rule
    /// </summary>
    [Serializable]
    public class EntityDicQcMateriaRule : EntityBase
    {
        /// <summary>
        /// 质控物明细ID（水平、批号）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_key", MedName = "mat_sn", WFName = "Rmr_Dmat_id")]
        public String MatSn { get; set; }

        /// <summary>
        /// 规则ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_rule_id", MedName = "rul_id", WFName = "Rmr_Drule_id")]
        public String RulId { get; set; }

        /// <summary>
        /// 质控项目Id
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_par_id", MedName = "mat_id", WFName = "Rmr_id")]
        public String MatItmId { get; set; }

    }
}
