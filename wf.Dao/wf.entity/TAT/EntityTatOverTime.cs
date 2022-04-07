using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// TAT超时数据表
    /// 旧表名:tat_overtime_data 新表名:Lis_tat_overtime_data
    /// </summary>
    [Serializable]
    public class EntityTatOverTime : EntityBase
    {
        /// <summary>
        ///自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_id", MedName = "tat_id", WFName = "Ltat_id")]
        public Int32 TatId { get; set; }

        /// <summary>
        ///条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_bar_code", MedName = "tat_bar_code", WFName = "Ltat_Sma_bar_id")]
        public String TatBarCode { get; set; }

        /// <summary>
        ///开始状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_start_type", MedName = "tat_start_type", WFName = "Ltat_start_type")]
        public Int32 TatStartType { get; set; }

        /// <summary>
        ///结束状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_end_type", MedName = "tat_end_type", WFName = "Ltat_end_type")]
        public Int32 TatEndType { get; set; }

        /// <summary>
        ///超时时间(分钟)
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_time_over", MedName = "tat_time_over", WFName = "Ltat_time_over")]
        public Int32 TatTimeOver { get; set; }

        /// <summary>
        ///结束状态时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_end_type_time", MedName = "tat_end_type_time", WFName = "Ltat_end_type_time")]
        public DateTime TatEndTypeTime { get; set; }

        /// <summary>
        ///提示状态 0-未读取 1-已读取
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_flag", MedName = "tat_flag", WFName = "Ltat_flag")]
        public Int32 TatFlag { get; set; }

        /// <summary>
        ///处理状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_msg_flag", MedName = "tat_msg_flag", WFName = "Ltat_msg_flag")]
        public Int32 TatMsgFlag { get; set; }

        /// <summary>
        ///时间间隔(小时)
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_temp_time", MedName = "tat_temp_time", WFName = "Ltat_temp_time")]
        public Int32 TatTempTime { get; set; }

        /// <summary>
        ///预报时间(小时)
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_temp_rea_time", MedName = "tat_temp_rea_time", WFName = "Ltat_temp_rea_time")]
        public Int32 TatTempReaTime { get; set; }

        /// <summary>
        ///类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "tat_time_type", MedName = "tat_time_type", WFName = "Ltat_time_type")]
        public String TatTimeType { get; set; }

       
    }
}
