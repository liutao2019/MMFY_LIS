using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 院网接口明细表
    /// 旧表名:Sys_itf_contrast 新表名:Base_itf_contrast
    /// </summary>
    [Serializable()]
    public class EntitySysItfContrast : EntityBase
    {
        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_id", MedName = "cont_id", WFName = "Bitfcon_id", DBIdentity = true)]
        public Int32 ContId { get; set; }

        /// <summary>
        ///接口名称:住院,门诊
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_interface_id", MedName = "cont_itface_id", WFName = "Bitfcon_Bitf_id")]
        public String ContItfaceId { get; set; }

        /// <summary>
        ///HIS接口字段
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_interface_columns", MedName = "cont_interface_column", WFName = "Bitfcon_his_column")]
        public String ContInterfaceColumn { get; set; }

        /// <summary>
        ///LIS接口字段
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_lis_columns", MedName = "cont_sys_column", WFName = "Bitfcon_column")]
        public String ContSysColumn { get; set; }

        /// <summary>
        ///字段转换规则
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_rule", MedName = "cont_column_rule", WFName = "Bitfcon_column_rule")]
        public String ContColumnRule { get; set; }

        /// <summary>
        ///类型：0-对照表  1-中转表
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_type", MedName = "cont_type", WFName = "Bitfcon_type")]
        public Int32 ContType { get; set; }

        /// <summary>
        ///数据类型：对应c#数据类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_datatype", MedName = "cont_data_type", WFName = "Bitfcon_data_type")]
        public String ContDataType { get; set; }

        /// <summary>
        ///中转表表名
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_tablename", MedName = "cont_tablename", WFName = "Bitfcon_tablename")]
        public String ContTablename { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_desc", MedName = "cont_remark", WFName = "Bitfcon_remark")]
        public String ContRemark { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_search_seq", MedName = "cont_search_seq", WFName = "Bitfcon_search_seq")]
        public Int32 ContSearchSeq { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_script_for_search", MedName = "cont_script_for_search", WFName = "Bitfcon_script_for_search")]
        public String ContScriptForSearch { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_search_para", MedName = "cont_search_para", WFName = "Bitfcon_search_para")]
        public String ContSearchPara { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_search_para_text", MedName = "cont_search_para_text", WFName = "Bitfcon_search_para_text")]
        public String ContSearchParaText { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "con_converter", MedName = "cont_converter", WFName = "Bitfcon_converter")]
        public String ContConverter { get; set; }

        #region   附加字段  接口名称   
        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "in_name", MedName = "itface_name", WFName = "Bitf_name", DBColumn = false)]
        public String ContInterfaceName { get; set; }
        #endregion
    }
}
