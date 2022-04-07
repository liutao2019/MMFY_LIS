using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 药敏信息查询实体（whonet导出用）
    /// </summary>
    [Serializable]
    public class EntityMicResult : EntityBase
    {
        public EntityMicResult()
        {
        }

        /// <summary>
        /// 标本号
        /// </summary>
        [FieldMapAttribute(WFName = "Pma_sid")]
        public String sid { get; set; }

        /// <summary>
        /// 菌名
        /// </summary>
        [FieldMapAttribute(WFName = "Dbact_cname")]
        public String bactName { get; set; }

        /// <summary>
        /// 抗生素名
        /// </summary>
        [FieldMapAttribute(WFName = "Dant_cname")]
        public string antiName { get; set; }

        /// <summary>
        /// 抗生素代码
        /// </summary>
        [FieldMapAttribute(WFName = "Dant_code")]
        public string antiCode { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        [FieldMapAttribute(WFName = "Lanti_value")]
        public string resValue { get; set; }
    }


}
