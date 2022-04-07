using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 质控通道（时间表）
    /// 旧表名:qc_rule_time 新表名:Rel_qc_rule_time
    /// </summary>
    [Serializable]
    public class EntityDicQcRuleTime : EntityBase
    {
        /// <summary>
        /// 主键ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_id", MedName = "qrt_id", WFName = "Rqrt_id")]
        public Int32 QrtId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_start_time", MedName = "qrt_start_time", WFName = "Rqrt_start_time")]
        public DateTime QrtStartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_end_time", MedName = "qrt_end_time", WFName = "Rqrt_end_time")]
        public DateTime QrtEndTime { get; set; }

        /// <summary>
        /// 跨越天数
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_day", MedName = "qrt_day", WFName = "Rqrt_day")]
        public Int32 QrtDay { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qrt_itr_id", MedName = "qrt_itr_id", WFName = "Rqrt_Ditr_id")]
        public String QrtItrId { get; set; }
    }
}
