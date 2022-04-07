using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 实验序号版实验结果实体（茂名妇幼检验结果录入）
    /// </summary>
    public class EntityObrResultTestSeqVer
    {
        public string TestSeq { get; set; }

        public string RepId { get; set; }
        public string RepSid { get; set; }
        public string RepItrId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string RepComment { get; set; }

        public List<EntityObrResult> Obrs { get; set; }
    }
}
