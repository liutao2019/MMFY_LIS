using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 费用类别
    /// </summary>
    [Serializable]
    public class EntityDicPubInsurance : EntityBase
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string  FeesTypeId { get; set; }
        /// <summary>
        /// 费用类别
        /// </summary>
        public string FeesTypeName { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string FeesTypePyCode { get; set; }
        /// <summary>
        /// 五笔码
        /// </summary>
        public string FeesTypeWbCode { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int FeesTypeSortNo { get; set; }
        /// <summary>
        /// 删除标志
        /// </summary>
        public string FeesTypeDelFlag { get; set; }
    }
}
