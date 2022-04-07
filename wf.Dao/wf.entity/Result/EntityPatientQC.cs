using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 病人报告查询条件
    /// </summary>
    [Serializable]
    public class EntityPatientQC : EntityBase
    {
        public EntityPatientQC()
        {
            ListItrId = new List<string>();
            ListSidRange = new List<EntitySid>();
            ListSortRange = new List<EntitySortNo>();
            NotInRepId = false;
            RepRecheck = false;
            RepUrgent = false;
            historySelectColumn = HistorySelectColumn.病人ID;
            NotInRepStatus = false;
            QueryTaT = false;
            DateStart = new DateTime();
            DateEnd = new DateTime();
        }

        /// <summary>
        /// 病人ID
        /// </summary>
        public String RepId { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// 审核名称
        /// </summary>
        public string auditWord { get; set; }

        /// <summary>
        /// 报告名称
        /// </summary>
        public string reportWord { get; set; }

        /// <summary>
        /// 仪器ID集合
        /// </summary>
        public List<string> ListItrId { get; set; }

        /// <summary>
        /// 标本类别
        /// </summary>
        public String SamId { get; set; }

        /// <summary>
        /// 科室编码
        /// </summary>
        public String DepId { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String PatDepName { get; set; }

        /// <summary>
        /// 组合编码
        /// </summary>
        public String ComId { get; set; }

        /// <summary>
        /// 组合名称
        /// </summary>
        public String ComName { get; set; }

        /// <summary>
        /// 报告状态 0-未审核 1-已审核 2-已报告
        /// </summary>
        public String RepStatus { get; set; }

        /// <summary>
        /// 复查标志
        /// </summary>
        public bool RepRecheck { get; set; }

        /// <summary>
        /// 危急值标志
        /// </summary>
        public bool RepUrgent { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        public String RepSrcId { get; set; }

        /// <summary>
        /// 检验者ID
        /// </summary>
        public String PidCheckUserId { get; set; }

        /// <summary>
        /// 样本范围
        /// </summary>
        public List<EntitySid> ListSidRange { get; set; }

        /// <summary>
        /// 序号范围
        /// </summary>
        public List<EntitySortNo> ListSortRange { get; set; }

        /// <summary>
        /// 病人ID类型
        /// </summary>
        public string PidIdtId { get; set; }
        /// <summary>
        /// 病人ID
        /// </summary>
        public string PidInNo { get; set; }

        /// <summary>
        /// 不等于 用于条件<> 默认是false
        /// </summary>
        public bool NotInRepId { get; set; }

        /// <summary>
        /// 不在状态中(not in)
        /// </summary>
        public bool NotInRepStatus { get; set; }

        /// <summary>
        /// 判断加载的where条件
        /// </summary>
        public String IsEnabled { get; set; }

        /// <summary>
        /// 病人名称
        /// </summary>
        public String PidName { get; set; }


        /// <summary>
        /// 状态名称
        /// </summary>
        public string RepStatusName { get; set; }

        /// <summary>
        /// 样本号
        /// </summary>
        public string RepSid { get; set; }


        /// <summary>
        /// 条码号
        /// </summary>
        public string RepBarCode { get; set; }


        /// <summary>
        /// 小于当前时间
        /// </summary>
        public DateTime? LessPatDate { get; set; }

        /// <summary>
        /// 是否查询Tat时间
        /// </summary>
        public bool QueryTaT { get; set; }
        /// <summary>
        /// 备注是否为空
        /// </summary>
        public bool RepRemarkIsNotNull { get; set; }

        public HistorySelectColumn historySelectColumn { get; set; }
    }

    [Serializable]
    public enum HistorySelectColumn
    { 
        病人ID = 0
    }
}
