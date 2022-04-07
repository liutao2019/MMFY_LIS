using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人历史报告查询条件
    /// </summary>
    [Serializable]
    public class EntityHistoryPatientQC : EntityBase
    {
        public EntityHistoryPatientQC()
        {
            ListItrId = new List<string>();
            listSamId = new List<string>();
        }

        /// <summary>
        /// 病人ID
        /// </summary>
        public String RepId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? RepInDate { get; set; }

        /// <summary>
        /// 标本类别
        /// </summary>
        public String SamId { get; set; }

        /// <summary>
        /// 标本类别集合
        /// </summary>
        public List<string> listSamId { get; set; }

        /// <summary>
        /// 病人id类型
        /// </summary>
        public String PidIdtId { get; set; }

        /// <summary>
        /// 病人Id
        /// </summary>
        public String PidInNo { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public String PidName { get; set; }

        /// <summary>
        /// 病人性别
        /// </summary>
        public String PidSex { get; set; }

        /// <summary>
        /// 个数
        /// </summary>
        public Int32 ResultCount { get; set; }

        /// <summary>
        /// 仪器id集合
        /// </summary>
        public List<string> ListItrId { get; set;  }

        /// <summary>
        /// 审核和报告状态
        /// </summary>
        public string RepStatus { get; set; }
    }
       
}
