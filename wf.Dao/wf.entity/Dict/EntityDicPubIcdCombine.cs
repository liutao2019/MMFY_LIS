using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// ICD对应组合
    /// 旧表名:Dic_Pub_icd_combine 新表名:Dict_icd_combine
    /// </summary>
    [Serializable]
    public class EntityDicPubIcdCombine
    {
        /// <summary>
        /// 诊断编码
        /// </summary>
        [FieldMapAttribute(ClabName = "icd_id", MedName = "icd_id", WFName = "Dicdc_Dicd_id")]
        public string IcdId { get; set; }

        /// <summary>
        /// 组合编码
        /// </summary>
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Dicdc_Dcom_id")]
        public string ComId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [FieldMapAttribute(ClabName = "sort_no", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }
    }
}
