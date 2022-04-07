using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 表单字段 单证类型  
    /// 旧表名:Def_oa_table_field 新表名:Rel_oa_field
    /// </summary>
    [Serializable]
    public class EntityOaTableField: EntityBase
    {

        /// <summary>
        ///表单类型代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderTypeCode", MedName = "tab_code", WFName = "Rof_Dot_code")]
        public String TabCode { get; set; }

        /// <summary>
        ///表单字段代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderItemCode", MedName = "field_code", WFName = "Rof_code")]
        public String FieldCode { get; set; }

        /// <summary>
        ///表单字段名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderItemName", MedName = "field_name", WFName = "Rof_name")]
        public String FieldName { get; set; }

        /// <summary>
        ///表单字段数据类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderItemType", MedName = "field_type", WFName = "Rof_type")]
        public String FieldType { get; set; }

        /// <summary>
        ///表单字段顺序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "OrderItemIndex", MedName = "field_index", WFName = "Rof_index")]
        public Int32 FieldIndex { get; set; }

        /// <summary>
        ///是否在列表中显示 0不显示 1显示
        /// </summary>   
        [FieldMapAttribute(ClabName = "ShowInList", MedName = "field_list_display", WFName = "Rof_list_display")]
        public String FieldListDisplay { get; set; }

        /// <summary>
        ///枚举字段名 非数据库管关联枚举直接用,隔开
        /// </summary>   
        [FieldMapAttribute(ClabName = "DictName", MedName = "field_dict_list", WFName = "Rof_dict_list")]
        public String FieldDictList { get; set; }

        /// <summary>
        ///枚举SQL语句
        /// </summary>   
        [FieldMapAttribute(ClabName = "DictFilter", MedName = "field_dict_sql", WFName = "Rof_dict_sql")]
        public String FieldDictSql { get; set; }


        #region 附加字段 是否在列表中显示中文 是或者否
        /// <summary>
        ///预留字段,保留单证中可能会被引用的表单字段内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "", MedName = "")]
        public String DetCharH { get
            {
                if (FieldListDisplay == "0")
                    return "否";
                else if (FieldListDisplay == "1")
                    return "是";
                else return null;
            } }
        #endregion
    }
}
