using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///  归档查询:模块页面查询条件涉及的参数作为实体成员
    /// </summary>
    public class EntityDicSamSearchParamter
    {
        /// <summary>
        /// 查询条件：归档日期开始时间
        /// </summary>
        public DateTime? DateTimeFrom { get; set; }

        /// <summary>
        /// 查询条件：归档日期结束时间
        /// </summary>
        public DateTime? DateTimeTo { get; set; }

        /// <summary>
        ///  查询条件：病人ID
        /// </summary>
        public String PatInNo { get; set; }

        /// <summary>
        /// 查询条件：病人姓名
        /// </summary>
        public String PatName { get; set; }
        
        /// <summary>
        ///  查询条件：归档人
        /// </summary>
        public String StoreMan { get; set; }

        /// <summary>
        /// 查询条件：实验组
        /// </summary>
        public String RackCtype { get; set; }

        /// <summary>
        ///  查询条件：存储冰箱
        /// </summary>
        public String IceID { get; set; }

        /// <summary>
        /// 查询条件：柜子
        /// </summary>
        public String CupID { get; set; }

        /// <summary>
        /// 查询条件：架子条码
        /// </summary>
        public String RackBarcode { get; set; }

        /// <summary>
        /// 查询条件：样本条码
        /// </summary>
        public String SamBarcode { get; set; }
        
    }
}
