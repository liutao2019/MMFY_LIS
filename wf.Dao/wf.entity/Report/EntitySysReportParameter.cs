using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 报表
    /// 旧表名:Sys_report_parameter 新表名:Base_report_ parameter
    /// </summary>
    [Serializable]
    public class EntitySysReportParameter : EntityBase
    {

        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "repid", MedName = "rep_id", WFName = "Brp_Brep_id")]
        public Int32 RepId { get; set; }

        /// <summary>
        ///报表名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "repar", MedName = "rep_parm_type", WFName = "Brp_type")]
        public String RepParmType { get; set; }

        /// <summary>
        ///报表参数
        /// </summary>   
        [FieldMapAttribute(ClabName = "reInitPar", MedName = "rep_parm_value", WFName = "Brp_value")]
        public String RepParmValue { get; set; }
    }
}