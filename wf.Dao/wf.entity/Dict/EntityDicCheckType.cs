using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 报告类别
    /// </summary>
    [Serializable]
    public class EntityDicCheckType : EntityBase
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string CheckTypeId { get; set; }
        /// <summary>
        /// 费用类别
        /// </summary>
        public string CheckTypeName { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string CheckTypePyCode { get; set; }
        /// <summary>
        /// 五笔码
        /// </summary>
        public string CheckTypeWbCode { get; set; }
    }
}
