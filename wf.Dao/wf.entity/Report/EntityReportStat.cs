using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 报表
    /// </summary>
    [Serializable]
    public class EntityReportStat : EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_id", MedName = "rep_id",WFName = "rep_id")]
        public Int32 RepId { get; set; }

        /// <summary>
        ///报表名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_name", MedName = "rep_name", WFName = "rep_name")]
        public String RepName { get; set; }

        /// <summary>
        ///报表代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_code", MedName = "rep_code", WFName = "rep_code")]
        public String RepCode { get; set; }

        /// <summary>
        ///报表类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_type", MedName = "rep_type", WFName = "rep_type")]
        public String RepType { get; set; }

        /// <summary>
        ///报表序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_seq", MedName = "rep_seq", WFName = "rep_seq")]
        public String RepSeq { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_wb", MedName = "rep_wb", WFName = "rep_wb")]
        public String RepWb { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rep_py", MedName = "rep_py", WFName = "rep_py")]
        public String RepPy { get; set; }

    }
}
