using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///温度范围 
    ///旧表名:dict_temperature 新表名:Dict_temperature
    /// </summary>
    [Serializable]
    public class EntityDicTemperature : EntityBase
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_id", MedName = "dt_id", WFName = "dt_id")]
        public string DtId { get; set; }

        /// <summary>
        /// 范围编码
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_code", MedName = "dt_code", WFName = "dt_code")]
        public string DtCode { get; set; }

        /// <summary>
        /// 范围名称
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_name", MedName = "dt_name", WFName = "dt_name")]
        public string DtName { get; set; }

        /// <summary>
        /// 最高温
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_h_limit", MedName = "dt_h_limit", WFName = "dt_h_limit")]
        public string DtHLimit { get; set; }

        /// <summary>
        /// 最低温
        /// </summary>
        [FieldMapAttribute(ClabName = "dt_l_limit", MedName = "dt_l_limit", WFName = "dt_l_limit")]
        public string DtLLimit { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public string WbCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public string PyCode { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public string DelFlag { get; set; }

    }
}
