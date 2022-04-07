using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 快速排样插入ReportMain表数据
    /// </summary>
    [Serializable]
    public class EntityShelfSampToReportMain
    {
        /// <summary>
        /// 条码号
        /// </summary>
        public string SampBarCode { get; set; }
        /// <summary>
        /// 样本号
        /// </summary>
        public string RepSid { get; set; }
        /// <summary>
        /// 仪器Id
        /// </summary>
        public string ItrId { get; set; }
        /// <summary>
        /// 仪器名称
        /// </summary>
        public string ItrName { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        public DateTime RepInDate { get; set; }

        /// <summary>
        /// 检验时间
        /// </summary>
        public DateTime SampCheckDate { get; set; }

        /// <summary>
        /// 实验组ID
        /// </summary>
        public string ProId { get; set; }

        /// <summary>
        /// 项目ID合集
        /// </summary>
        public List<string>ComIds { get; set; }
        /// <summary>
        /// 条码项目明细自增ID
        /// </summary>
        public List<string> DetSns { get; set; }
        /// <summary>
        /// 双向
        /// </summary>
        public string RepSerialNum { get; set; }

        /// <summary>
        /// 检验者
        /// </summary>
        public string RepCheckUserId { get; set; }

        public EntityShelfSampToReportMain()
        {
            DetSns = new List<string>();
            ComIds = new List<string>();
        }
    }
}
