using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class WhonetDataQueryArg : EntityBase
    {
        /// <summary>
        /// 检索开始日期
        /// </summary>
        public DateTime DateBegin { get; set; }

        /// <summary>
        /// 检索结束日期
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// 发送结果(全部,阴性,阳性)
        /// </summary>
        public string SenResult { get; set; }

        /// <summary>
        /// 将来异构医院用
        /// </summary>
        public string hospital { get; set; }
    }
}
