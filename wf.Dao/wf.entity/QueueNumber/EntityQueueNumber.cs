using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 排队叫号
    /// </summary>
    [Serializable]
    public class EntityQueueNumber : EntityBase
    {
        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "Queue_sn", MedName = "Queue_sn", WFName = "Queue_sn")]
        public Int32 QueueSn { get; set; }

        /// <summary>
        ///病人id
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_in_no", MedName = "pid_in_no", WFName = "pid_in_no")]
        public String PidInNo { get; set; }

        /// <summary>
        ///优先级
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_name", MedName = "pid_name", WFName = "pid_name")]
        public String PidName { get; set; }

        /// <summary>
        ///诊疗卡号
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_social_no", MedName = "pid_social_no", WFName = "pid_social_no")]
        public String PidSocialNo { get; set; }

        /// <summary>
        ///排队号
        /// </summary>   
        [FieldMapAttribute(ClabName = "queue_no", MedName = "queue_no", WFName = "queue_no")]
        public String QueueNo { get; set; }

        /// <summary>
        ///窗口号
        /// </summary>   
        [FieldMapAttribute(ClabName = "queue_windows_name", MedName = "queue_windows_name", WFName = "queue_windows_name")]
        public String QueueWindowsName { get; set; }

        /// <summary>
        ///排队时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "queue_date", MedName = "queue_date", WFName = "queue_date")]
        public DateTime QueueDate { get; set; }

        /// <summary>
        ///排队状态 0待采集 1正在叫号 2已过号 3已采集
        /// </summary>   
        [FieldMapAttribute(ClabName = "queue_status", MedName = "queue_status", WFName = "queue_status")]
        public String QueueStatus { get; set; }

        /// <summary>
        ///采血区域
        /// </summary>   
        [FieldMapAttribute(ClabName = "queue_windows_area", MedName = "queue_windows_area", WFName = "queue_windows_area")]
        public String QueueWindowsArea { get; set; }

        /// <summary>
        ///优先级
        /// </summary>   
        [FieldMapAttribute(ClabName = "queue_priority", MedName = "queue_priority", WFName = "queue_priority")]
        public String QueuePriority { get; set; }

        #region 排队时间(分钟)
        /// <summary>
        ///排队时间(分钟)
        /// </summary>   
        [FieldMapAttribute(ClabName = "queue_time_min", MedName = "queue_time_min", WFName = "queue_time_min", DBColumn =false)]
        public String QueueTimeMin { get; set; }
        #endregion
        #region 操作时间
        /// <summary>
        ///操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood_date", MedName = "collection_date", WFName = "collection_date", DBColumn = false)]
        public String CollectionDate { get; set; }
        #endregion
        /// <summary>
        ///开始时间
        /// </summary>   
        public DateTime DateStart { get; set; }
        /// <summary>
        ///结束时间
        /// </summary>   
        public DateTime DateEnd { get; set; }
    }
}
