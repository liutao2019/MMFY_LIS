using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    /// <summary>
    /// 系统配置
    /// 旧表名:sys_pub_parameter 新表名:Base_parameter
    /// </summary>
    public class EntitySysParameter : EntityBase
    {
        /// <summary>
        ///配置代码,业务主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "configId", MedName = "parm_id", WFName = "Bparm_id")]
        public Int32 ParmId { get; set; }

        /// <summary>
        ///配置代码,判断配置项的标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "configCode", MedName = "parm_code", WFName = "Bparm_code")]
        public String ParmCode { get; set; }

        /// <summary>
        ///参数分组名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "configGroup", MedName = "parm_group", WFName = "Bparm_group")]
        public String ParmGroup { get; set; }

        /// <summary>
        ///参数字段名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "configItemName", MedName = "parm_field_name", WFName = "Bparm_field_name")]
        public String ParmFieldName { get; set; }

        /// <summary>
        ///参数字段类型 字符串/数字/日期/时间/枚举
        /// </summary>   
        [FieldMapAttribute(ClabName = "configItemType", MedName = "parm_field_type", WFName = "Bparm_field_type")]
        public String ParmFieldType { get; set; }

        /// <summary>
        ///参数值
        /// </summary>   
        [FieldMapAttribute(ClabName = "configItemValue", MedName = "parm_field_value", WFName = "Bparm_field_value")]
        public String ParmFieldValue { get; set; }

        /// <summary>
        ///字段类型为枚举时表示枚举内容,格式例子: 男,女
        ///字段类型为数字时表示控件最大最小值,格式例子: 1,100
        /// </summary>   
        [FieldMapAttribute(ClabName = "configDict", MedName = "parm_dict_list", WFName = "Bparm_dic_list")]
        public String ParmDictList { get; set; }

        /// <summary>
        ///预留参数类型字段,现均默认为0
        /// </summary>   
        [FieldMapAttribute(ClabName = "configType", MedName = "parm_type", WFName = "Bparm_type")]
        public String ParmType { get; set; }

        /// <summary>
        ///system 系统配置 userdefault 用户默认配置表 数字 对应userinfoId的用户配置
        /// </summary>   
        [FieldMapAttribute(ClabName = "configModule", MedName = "parm_module", WFName = "Bparm_module")]
        public String ParmModule { get; set; }
    }
}
