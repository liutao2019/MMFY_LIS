using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///报告解读评论
    ///旧表名:pid_report_comment 新表名:Pat_report_comment
    /// </summary>
    [Serializable]
    public class EntityReportComment : EntityBase
    {
        public EntityReportComment()
        { 
            RcDate = DateTime.Now;
        }

        /// <summary>
        ///唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "rc_key", MedName = "rc_key", WFName = "Prc_id",  DBIdentity = true)]
        public Int64 RcKey { get; set; }

        /// <summary>
        ///报告ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "rc_rep_id", MedName = "rc_rep_id", WFName = "Prc_rep_id")]
        public String RcRepId { get; set; }

        /// <summary>
        ///评价内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "rc_comment", MedName = "rc_comment", WFName = "Prc_comment")]
        public String RcComment { get; set; }

        /// <summary>
        ///评价时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "rc_date", MedName = "rc_date", WFName = "Prc_date")]
        public DateTime RcDate { get; set; }

        /// <summary>
        ///操作人姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "rc_opname", MedName = "rc_opname", WFName = "Prc_user_name")]
        public String RcOpname { get; set; }

        /// <summary>
        ///操作人代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rc_opcode", MedName = "rc_opcode", WFName = "Prc_user_id")]
        public String RcOpcode { get; set; }

    }
}
