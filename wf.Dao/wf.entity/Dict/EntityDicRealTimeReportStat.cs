using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityDicRealTimeReportStat : EntityBase
    {
        /// <summary>
        /// 编码(无实际意义)
        /// 旧表名:dic_realtimereport_stat 新表名:Dict_realtimereport_stat
        /// </summary>
        [FieldMapAttribute(ClabName = "rs_sn", MedName = "rs_sn", WFName = "Drs_sn")]
        public string RsSn { get; set; }

        /// <summary>
        /// 统计代码
        /// </summary>
        [FieldMapAttribute(ClabName = "rs_code", MedName = "rs_code", WFName = "Drs_code")]
        public string RsCode { get; set; }

        /// <summary>
        /// 统计名称
        /// </summary>
        [FieldMapAttribute(ClabName = "rs_name", MedName = "rs_name", WFName = "Drs_name")]
        public string RsName { get; set; }

        /// <summary>
        /// 父代码
        /// </summary>
        [FieldMapAttribute(ClabName = "rs_parent_code", MedName = "rs_parent_code", WFName = "Drs_parent_code")]
        public string RsParentCode { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [FieldMapAttribute(ClabName = "sort_no", MedName = "sort_no", WFName = "sort_no")]
        public int? SornNo { get; set; }
    }
}
