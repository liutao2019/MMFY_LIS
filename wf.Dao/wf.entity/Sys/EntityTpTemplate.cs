using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 模板表 tp_template
    /// </summary>
    [Serializable]
    public class EntityTpTemplate : EntityBase
    {
        /// <summary>
        ///模板编码  业务主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_id", MedName = "st_id", WFName = "st_id")]
        public Int32 StId { get; set; }

        /// <summary>
        ///模板类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_type", MedName = "st_type", WFName = "st_type")]
        public String StType { get; set; }

        /// <summary>
        ///模板名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_name", MedName = "st_name", WFName = "st_name")]
        public String StName { get; set; }

        /// <summary>
        ///模板表ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_tableId", MedName = "st_tableId", WFName = "st_tableId")]
        public String StTableId { get; set; }

        /// <summary>
        ///模板表名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "st_tableName", MedName = "st_tableName", WFName = "st_tableName")]
        public String StTableName { get; set; }

        /// <summary>
        ///结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_or", MedName = "res_or", WFName = "res_or")]
        public String ResOr { get; set; }

        /// <summary>
        ///结果项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_ecd", MedName = "res_itm_ecd", WFName = "res_itm_ecd")]
        public String ResItmEcd { get; set; }

        /// <summary>
        ///新结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr", MedName = "res_chr", WFName = "res_chr")]
        public String ResChr { get; set; }

        /// <summary>
        ///旧结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_od_chr", MedName = "res_od_chr", WFName = "res_od_chr")]
        public String ResOdChr { get; set; }

        /// <summary>
        ///结果单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_unit", MedName = "res_unit", WFName = "res_unit")]
        public String ResUnit { get; set; }

    }
}
