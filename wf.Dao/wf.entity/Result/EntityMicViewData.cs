using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 细菌仪器结果视窗数据
    /// </summary>
    public class EntityMicViewData
    {
        [FieldMapAttribute(ClabName = "type_id", MedName = "type_id", WFName = "type_id")]
        public String TypeId { get; set; }

        [FieldMapAttribute(ClabName = "type_name", MedName = "type_name", WFName = "type_name")]
        public String TypeName { get; set; }

        [FieldMapAttribute(ClabName = "type_mic", MedName = "type_mic", WFName = "type_mic")]
        public String TypeMic { get; set; }

        [FieldMapAttribute(ClabName = "type_anti", MedName = "type_anti", WFName = "type_anti")]
        public String TypeAnti { get; set; }

        [FieldMapAttribute(ClabName = "type_relevance", MedName = "type_relevance", WFName = "type_relevance")]
        public String TypeRelevance { get; set; }

    }
}
