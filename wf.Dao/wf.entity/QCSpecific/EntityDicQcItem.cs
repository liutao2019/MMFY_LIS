using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 质控通道查询项目临时实体
    /// </summary>
    [Serializable]
    public class EntityDicQcItem : EntityBase
    {  
        /// <summary>
        ///项目ID
        /// </summary>    
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        ///项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name")]
        public String ItmName { get; set; }

        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode")]
        public String ItmEcode { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_py", MedName = "py_code", WFName = "py_code")]
        public String PyCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_wb", MedName = "wb_code", WFName = "wb_code")]
        public String WbCode { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_select", MedName = "itm_select", WFName = "itm_select")]
        public Int32 ItmSelect { get; set; }
        
    }

}
