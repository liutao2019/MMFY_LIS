using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 数据表实体类
    /// 旧表名:dict_DataInterfaceCommandParameter 新表名:dict_DataInterfaceCommandParameter
    /// </summary>
    [Serializable()]
    public class EntityDicDataInterfaceCommandParameter : EntityBase
    {
        /// <summary>
        ///主键ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_id", MedName = "cmd_id", WFName = "cmd_id")]
        public String CmdId { get; set; }

        /// <summary>
        ///参数名
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_name", MedName = "param_name", WFName = "param_name")]
        public String ParamName { get; set; }

        /// <summary>
        ///方向
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_direction", MedName = "param_direction", WFName = "param_direction")]
        public String ParamDirection { get; set; }

        /// <summary>
        ///数据类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_datatype", MedName = "param_datatype", WFName = "param_datatype")]
        public String ParamDatatype { get; set; }

        /// <summary>
        ///长度
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_length", MedName = "param_length", WFName = "param_length")]
        public Int32 ParamLength { get; set; }

        /// <summary>
        ///顺序
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_seq", MedName = "param_seq", WFName = "param_seq")]
        public Int32 ParamSeq { get; set; }

        /// <summary>
        ///绑定
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_isbound", MedName = "param_isbound", WFName = "param_isbound")]
        public Boolean ParamIsbound { get; set; }

        /// <summary>
        ///转换名/默认值
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_databind", MedName = "param_databind", WFName = "param_databind")]
        public String ParamDatabind { get; set; }

        /// <summary>
        ///参数日志
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_enabledlog", MedName = "param_enabledlog", WFName = "param_enabledlog")]
        public Boolean ParamEnabledlog { get; set; }

        /// <summary>
        ///启用
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_enabled", MedName = "param_enabled", WFName = "param_enabled")]
        public Boolean ParamEnabled { get; set; }

        /// <summary>
        ///转换规则
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_converter_id", MedName = "param_converter_id", WFName = "param_converter_id")]
        public String ParamConverterId { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "param_desc", MedName = "param_desc", WFName = "param_desc")]
        public String ParamDesc { get; set; }

    }
}
