using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class BatchEditSrc
    {
        /// <summary>
        /// 病人资料日期
        /// </summary>
        public DateTime pat_date { get; set; }

        /// <summary>
        /// 仪器id
        /// </summary>
        public string pat_itr_id { get; set; }

        /// <summary>
        /// 开始样本号
        /// </summary>
        public long pat_sid_begin { get; set; }

        /// <summary>
        /// 结束样本号
        /// </summary>
        public long pat_sid_end { get; set; }

        /// <summary>
        /// 检验者
        /// </summary>
        public string pat_i_code { get; set; }

        /// <summary>
        /// 标本类别
        /// </summary>
        public string pat_sam_id { get; set; }

        /// <summary>
        /// 送检科室id
        /// </summary>
        public string pat_dep_id { get; set; }

        /// <summary>
        /// 送检科室名称
        /// </summary>
        public string pat_dep_name { get; set; }

        /// <summary>
        /// 匹配方式
        /// 0=按样本号
        /// 1=按序号
        /// </summary>
        public string MatchMode { get; set; }
    }
}
