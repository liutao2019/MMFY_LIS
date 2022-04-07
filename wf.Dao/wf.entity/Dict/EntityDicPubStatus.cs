using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 报表定义表
    /// 旧表名:  新表名:Base_report 
    /// </summary>
    [Serializable]
    public class EntityReport : EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "repid", MedName = "rep_id", WFName = "Brep_id")]
        public Int32 RepId { get; set; }

        /// <summary>
        ///报表名
        /// </summary>   
        [FieldMapAttribute(ClabName = "repname", MedName = "rep_name", WFName = "Brep_name")]
        public String RepName { get; set; }

        /// <summary>
        ///报表代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "repcode", MedName = "rep_code", WFName = "Brep_code")]
        public String RepCode { get; set; }

        /// <summary>
        ///报表地址
        /// </summary>   
        [FieldMapAttribute(ClabName = "repAddress", MedName = "rep_location", WFName = "Brep_location")]
        public String RepLocation { get; set; }

        /// <summary>
        ///报表sql
        /// </summary>   
        [FieldMapAttribute(ClabName = "repSql", MedName = "rep_sql", WFName = "Brep_sql")]
        public String RepSql { get; set; }

        /// <summary>
        ///报表默认sql
        /// </summary>   
        [FieldMapAttribute(ClabName = "repDefaultSql", MedName = "rep_default_sql", WFName = "Brep_default_sql")]
        public String RepDefaultSql { get; set; }

        /// <summary>
        ///连接码
        /// </summary>   
        [FieldMapAttribute(ClabName = "repconncode", MedName = "rep_conect_code", WFName = "Breap_Dconn_code")]
        public String RepConectCode { get; set; }
    }
}
