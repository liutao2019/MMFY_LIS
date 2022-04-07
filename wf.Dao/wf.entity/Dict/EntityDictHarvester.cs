using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 采集器
    /// 旧表名:dict_harvester 新表名：Dict_harvester
    /// </summary>
    [Serializable]
    public class EntityDictHarvester : EntityBase
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        [FieldMapAttribute(ClabName = "dh_id", MedName = "dh_id", WFName = "dh_id")]
        public string DhId { get; set; }

        /// <summary>
        /// 采集器编码
        /// </summary>
        [FieldMapAttribute(ClabName = "dh_code", MedName = "dh_code", WFName = "dh_code")]
        public string DhCode { get; set; }

        /// <summary>
        /// 采集器名称
        /// </summary>
        [FieldMapAttribute(ClabName = "dh_name", MedName = "dh_name", WFName = "dh_name")]
        public string DhName { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public string PyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public string WbCode { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public int DelFlag { get; set; }
    }
}
