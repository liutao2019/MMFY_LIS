using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 药敏信息查询实体（whonet导出用）
    /// </summary>
    [Serializable]
    public class EntityAntiQc : EntityBase
    {
        public EntityAntiQc()
        {
            ListItrId = new List<string>();
            isAudit = true;
        }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public List<string> ListItrId { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public Int32 ExportDataType { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        public bool isAudit { get; set; }
    }


}
