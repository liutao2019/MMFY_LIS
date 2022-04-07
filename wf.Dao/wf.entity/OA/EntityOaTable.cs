using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 表单类型
    /// 旧表名:dic_oa_table 新表名:Dict_oa_type
    /// </summary>
    [Serializable]
    public class EntityOaTable: EntityBase
    {

        /// <summary>
        ///表单类型代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderTypeCode", MedName = "tab_code", WFName = "Dot_code")]
        public String TabCode { get; set; }

        /// <summary>
        ///表单类型名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderTypeName", MedName = "tab_name", WFName = "Dot_name")]
        public String TabName { get; set; }
    }
}
