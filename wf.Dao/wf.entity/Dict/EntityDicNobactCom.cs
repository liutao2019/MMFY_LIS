using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityDicNobactCom : EntityBase
    {
        /// <summary>
        /// 细菌无菌涂片ID
        /// </summary>
        [FieldMapAttribute(ClabName = "nob_id", MedName = "nob_id", WFName = "nob_id")]
        public String NobId { get; set; }
        /// <summary>
        /// 组合ID
        /// </summary>
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "com_id")]
        public String ComId { get; set; }
    }
}
