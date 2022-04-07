using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 旧表:Sys_barcode_generator 表名：Base_barcode_generator
    /// </summary>
    [Serializable]
    public class EntitySysBarcodeGenerator
    {
        /// <summary>
        ///编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "gen_id", WFName = "Bbargen_id")]
        public String GenId { get; set; }

        /// <summary>
        ///当前条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "gen_begin_no", WFName = "Bbargen_no")]
        public Decimal GenBeginNo { get; set; }

        /// <summary>
        ///条码批次
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_frequency", MedName = "gen_batch_no", WFName = "Bbargen_batch_no")]
        public Int32 GenBatchNo { get; set; }
    }
}
