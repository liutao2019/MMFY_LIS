using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public class EntityMergeResultQC : EntityBase
    {
        /// <summary>
        /// 0-样本号，1-序号
        /// </summary>
        public string MatchMode { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public string RepItrId { get; set; }

        /// <summary>
        /// 结果日期
        /// </summary>
        public DateTime ObrDate { get; set; }

        /// <summary>
        /// 起始号
        /// </summary>
        public long IdStart { get; set; }

        /// <summary>
        /// 结束号
        /// </summary>
        public long IdEnd { get; set; }

        ///// <summary>
        ///// 条码关联
        ///// </summary>
        //public bool BarcodeRelation { get; set; }

        //public bool InNoRelation { get; set; }

        //public string TestResults_usePatDate { get; set; }
    }
}
