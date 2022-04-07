using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 归档状态表
    /// 旧表名:dic_samp_store_status 新表名:Dict_samp_store_status
    /// </summary>
    [Serializable]
    public class EntityDicSampStoreStatus : EntityBase
    {
        /// <summary>
        /// 状态ID
        /// </summary>
        [FieldMapAttribute(ClabName = "status_id", MedName = "stau_id", WFName = "Dstau_id")]
        public String StauId { get; set; }
        /// <summary>
        /// 状态描述
        /// </summary>
        [FieldMapAttribute(ClabName = "status_name", MedName = "stau_name", WFName = "Dstau_name")]
        public String StauName { get; set; }
    }
}
