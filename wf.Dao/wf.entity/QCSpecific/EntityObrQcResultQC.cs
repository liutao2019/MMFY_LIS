using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public class EntityObrQcResultQC : EntityBase
    {
        public EntityObrQcResultQC()
        {

        }

        /// <summary>
        /// 是否启用即刻发判断
        /// </summary>
        public bool IsCheckGrubbs { get; set; }

        /// <summary>
        /// 项目id
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public string ItrId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StateTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 批号
        /// </summary>
        public string QcParDetailId { get; set; }

        /// <summary>
        /// 质控水平
        /// </summary>
        public string QanLevel { get; set; }
    }
}
