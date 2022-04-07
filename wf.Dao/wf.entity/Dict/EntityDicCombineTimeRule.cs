using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// TAT时间字典表
    /// 旧表名:dic_itm_combine_timerule 新表名:Dict_itm_combine_timerule
    /// </summary>
    [Serializable]
    public class EntityDicCombineTimeRule : EntityBase
    {

        public EntityDicCombineTimeRule()
        {
        }

        /// <summary>
        /// 编码（主键）
        /// </summary>
        [FieldMapAttribute(ClabName = "com_time_id", MedName = "com_time_id", WFName = "Dtr_id")]
        public String ComTimeId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [FieldMapAttribute(ClabName = "com_time_type", MedName = "com_time_type", WFName = "Dtr_type")]
        public System.String ComTimeType { get; set; }

        /// <summary>
        /// 开始类型
        /// </summary>
        [FieldMapAttribute(ClabName = "com_time_start_type", MedName = "com_time_start_type", WFName = "Dtr_start_type")]
        public System.String ComTimeStartType { get; set; }

        /// <summary>
        /// 结束类型
        /// </summary>
        [FieldMapAttribute(ClabName = "com_time_end_type", MedName = "com_time_end_type", WFName = "Dtr_end_type")]
        public System.String ComTimeEndType { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [FieldMapAttribute(ClabName = "com_time_ori_id", MedName = "com_time_ori_id", WFName = "Dtr_Dsorc_id")]
        public System.String ComTimeOriId { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        [FieldMapAttribute(ClabName = "com_time", MedName = "com_time", WFName = "Dtr_time")]
        public System.String ComTime { get; set; }

        /// <summary>
        /// 预报时间
        /// </summary>
        [FieldMapAttribute(ClabName = "com_rea_time", MedName = "com_rea_time", WFName = "Dtr_rea_time")]
        public System.String ComReaTime { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        #region 附加字段 
        /// <summary>
        /// 来源名称
        /// </summary>
        public System.String OriName { get; set; }
        /// <summary>
        /// 开始类型名称
        /// </summary>
        public System.String StartType { get; set; }
        /// <summary>
        /// 结束类型名称
        /// </summary>
        public System.String EndType { get; set; }


        /// <summary>
        /// 病人标识id
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_id", MedName = "rep_id", WFName = "Pdet_id", DBColumn = false)]
        public System.String RepId { get; set; }

        #endregion

    }
}
