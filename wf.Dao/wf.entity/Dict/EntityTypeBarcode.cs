using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 条码确认目的地涉及实体
    /// </summary>
    [Serializable]
    public class EntityTypeBarcode : EntityBase
    {
        /// <summary>
        /// 编码（医院编码与组别编码组成）
        /// </summary>
        [FieldMapAttribute(ClabName = "type_nodeId", MedName = "type_nodeId", WFName = "type_nodeId", DBColumn = false)]
        public String TypeNodeId { get; set; }

        /// <summary>
        /// 物理组
        /// </summary>
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "pro_name", DBColumn = false)]
        public String ProName { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [FieldMapAttribute(ClabName = "type_id", MedName = "pro_id", WFName = "pro_id", DBColumn = false)]
        public String ProID { get; set; }

        /// <summary>
        /// 医院ID（别名）
        /// </summary>
        [FieldMapAttribute(ClabName = "type_node", MedName = "type_node", WFName = "type_node", DBColumn = false)]
        public String TypeNode { get; set; }

    }
}
