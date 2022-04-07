using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 中间表参数
    /// </summary>
    public class EntityDCLReportParmeter
    {
        /// <summary>
        ///报告单标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "Pma_rep_id", MedName = "Pma_rep_id", WFName = "Pma_rep_id", DBColumn = false)]
        public String PmaRepID { get; set; }

        /// <summary>
        ///中间表参数----HIS申请单ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "_ApplyID", MedName = "_ApplyID", WFName = "_ApplyID", DBColumn = false)]
        public String PmaApplyID { get; set; }

        /// <summary>
        /// 条码号（用于反审时写入接口日志）
        /// </summary>
        [FieldMapAttribute(ClabName = "Pma_bar_code", MedName = "Pma_bar_code", WFName = "Pma_bar_code", DBColumn = false)]
        public string BarCode { get; set; }


    }
}
