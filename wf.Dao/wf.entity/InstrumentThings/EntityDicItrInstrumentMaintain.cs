using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 保养字典：（med库）dic_itr_instrument_maintain  （clab库）dict_instrmt_maintain
    /// 旧表名:dic_itr_instrument_maintain 新表名:Dict_instrmt_maintain
    /// </summary>
    [Serializable]
    public class EntityDicItrInstrumentMaintain : EntityBase
    {
        /// <summary>
        /// 编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_id", MedName = "mai_id", WFName = "Dim_id")]
        public Int32 MaiId { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_itr_id", MedName = "mai_itr_id", WFName = "Dim_Ditr_id")]
        public String MaiItrId { get; set; }

        /// <summary>
        /// 间隔时间(年月日计算成时)
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_interval", MedName = "mai_interval", WFName = "Dim_interval")]
        public Int32 MaiInterval { get; set; }

        /// <summary>
        /// 保养内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_content", MedName = "mai_content", WFName = "Dim_content")]
        public String MaiContent { get; set; }

        /// <summary>
        /// 保养类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_type", MedName = "mai_type", WFName = "Dim_type")]
        public String MaiType { get; set; }

        /// <summary>
        /// 保养限制
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_astrict", MedName = "mai_astrict", WFName = "Dim_astrict")]
        public String MaiAstrict { get; set; }

        /// <summary>
        /// 提醒时间(年月日计算成时)
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_warning_time", MedName = "mai_warning_time", WFName = "Dim_warning_time")]
        public Int32 MaiWarningTime { get; set; }

        /// <summary>
        ///  关闭审核时间（年月日计算成时）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_closeAudit_time", MedName = "mai_closeAudit_time", WFName = "Dim_closeaudit_time")]
        public Int32 MaiCloseAuditTime { get; set; }

        /// <summary>
        /// 间隔时间（年）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_interval_year", MedName = "mai_interval_year", WFName = "Dim_interval_year")]
        public Int32 MaiIntervalYear { get; set; }

        /// <summary>
        /// 间隔时间（月）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_interval_month", MedName = "mai_interval_month", WFName = "Dim_interval_month")]
        public Int32 MaiIntervalMonth { get; set; }

        /// <summary>
        /// 间隔时间（日）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_interval_day", MedName = "mai_interval_day", WFName = "Dim_interval_day")]
        public Int32 MaiIntervalDay { get; set; }

        /// <summary>
        /// 报警(提醒)时间（年）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_warning_time_year", MedName = "mai_warning_time_year", WFName = "Dim_warning_time_year")]
        public Int32 MaiWarningTimeYear { get; set; }

        /// <summary>
        /// 报警(提醒)时间（月）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_warning_time_month", MedName = "mai_warning_time_month", WFName = "Dim_warning_time_month")]
        public Int32 MaiWarningTimeMonth { get; set; }

        /// <summary>
        /// 报警(提醒)时间（日）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_warning_time_day", MedName = "mai_warning_time_day", WFName = "Dim_warning_time_day")]
        public Int32 MaiWarningTimeDay { get; set; }

        /// <summary>
        /// 关闭审核时间（年）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_closeAudit_time_year", MedName = "mai_closeAudit_time_year", WFName = "Dim_closeaudit_time_year")]
        public Int32 MaiCloseAuditTimeYear { get; set; }

        /// <summary>
        /// 关闭审核时间（月）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_closeAudit_time_month", MedName = "mai_closeAudit_time_month", WFName = "Dim_closeaudit_time_month")]
        public Int32 MaiCloseAuditTimeMonth { get; set; }

        /// <summary>
        /// 关闭审核时间（日）
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_closeAudit_time_day", MedName = "mai_closeAudit_time_day", WFName = "Dim_closeaudit_time_day")]
        public Int32 MaiCloseAuditTimeDay { get; set; }

        /// <summary>
        /// 操作提示
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_operate_tips", MedName = "mai_operate_tips", WFName = "Dim_operate_tips")]
        public String MaiOperateTips { get; set; }

        /// <summary>
        /// 提醒时间模糊计算标志(1:勾选；0：未勾选)
        /// </summary>   
        [FieldMapAttribute(ClabName = "fuzzy_warning_time", MedName = "fuzzy_warning_time", WFName = "Dim_fuzzy_warning_time")]
        public String FuzzyWarningTime { get; set; }

        /// <summary>
        /// 关闭审核时间模糊计算标志(1:勾选；0：未勾选)
        /// </summary>   
        [FieldMapAttribute(ClabName = "fuzzy_closeAudit_time", MedName = "fuzzy_closeAudit_time", WFName = "Dim_fuzzy_closeAudit_time")]
        public String FuzzyCloseAuditTime { get; set; }

        /// <summary>
        /// 间隔时间模糊计算标志(1:勾选；0：未勾选)
        /// </summary>   
        [FieldMapAttribute(ClabName = "fuzzy_interval_time", MedName = "fuzzy_interval_time", WFName = "Dim_fuzzy_interval_time")]
        public String FuzzyIntervalTime { get; set; }

        /// <summary>
        /// 描述
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_desc", MedName = "mai_desc", WFName = "Dim_desc")]
        public String MaiDesc { get; set; }

    }
}
