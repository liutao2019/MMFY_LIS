using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 仪器保养维修查询
    /// 旧表名:dic_itr_instrument_maintain_Registration 新表名:Dict_instrmt_maintain_Registration
    /// </summary>
    [Serializable]
    public class EntityDicInstrmtMaintainRegistration : EntityBase
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime DeStartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime DeEndTime { get; set; }

        /// <summary>
        /// 时间类型
        /// </summary>
        public string StrTimeType { get; set; }

        /// <summary>
        /// 小时
        /// </summary>
        public int Hour { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string StrOperate { get; set; }

        /// <summary>
        /// 仪器保养ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_id", MedName = "reg_id", WFName = "Dimr_id")]
        public Int32 RegId { get; set; }

        /// <summary>
        /// （仪器保养字典）仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_itr_id", MedName = "reg_itr_id", WFName = "Dimr_Ditr_id")]
        public String RegItrId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_register_date", MedName = "reg_register_date", WFName = "Dimr_register_date")]
        public DateTime? RegRegisterDate { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_register_code", MedName = "reg_register_code", WFName = "Dimr_register_code")]
        public String RegRegisterCode { get; set; }

        /// <summary>
        /// （保养字典表）编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_mai_id", MedName = "reg_mai_id", WFName = "Dimr_Dim_id")]
        public Int32 RegMaiId { get; set; }

        /// <summary>
        /// 保养内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_content", MedName = "reg_content", WFName = "Dimr_content")]
        public String RegContent { get; set; }

        /// <summary>
        /// 仪器保养间隔
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_interval", MedName = "reg_interval", WFName = "Dimr_interval")]
        public Int32 RegInterval { get; set; }

        /// <summary>
        /// 保养备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "reg_exp", MedName = "reg_exp", WFName = "Dimr_exp")]
        public String RegExp { get; set; }

        /// <summary>
        /// 超出保养时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "over_interval_time", MedName = "over_interval_time", WFName = "Dimr_over_interval_time")]
        //public Int32 OverIntervalTime { get; set; }
        public String OverIntervalTime { get; set; }

        #region 附加字段   仪器
        /// <summary>
        /// 仪器
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItrEname { get; set; }
        #endregion

        #region 附加字段   用户名称
        /// <summary>
        /// 用户名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public String UserName { get; set; }
        #endregion

        #region 附加字段   组别名称
        /// <summary>
        /// 组别名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_name", MedName = "pro_name", WFName = "Dpro_name", DBColumn = false)]
        public String ProName { get; set; }
        #endregion

        #region 附加字段   保养字典编码
        /// <summary>
        /// 保养字典编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_id", MedName = "mai_id", WFName = "Dim_id", DBColumn = false)]
        public Int32 MaiId { get; set; }
        #endregion

        #region 附加字段   保养内容
        /// <summary>
        /// 保养内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_content", MedName = "mai_content", WFName = "Dim_content", DBColumn = false)]
        public String MaiContent { get; set; }
        #endregion

        #region 附加字段   仪器ID
        /// <summary>
        /// 仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_itr_id", MedName = "mai_itr_id", WFName = "Dim_Ditr_id", DBColumn = false)]
        public String MaiItrId { get; set; }
        #endregion

        #region 附加字段   提醒时间
        /// <summary>
        /// 提醒时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_warning_time", MedName = "mai_warning_time", WFName = "Dim_warning_time", DBColumn = false)]
        public Int32 MaiWarningTime { get; set; }
        #endregion

        #region 附加字段   关闭审核时间
        /// <summary>
        /// 关闭审核时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_closeAudit_time", MedName = "mai_closeAudit_time", WFName = "Dim_closeaudit_time", DBColumn = false)]
        public Int32 MaiCloseAuditTime { get; set; }
        #endregion

        #region 附加字段   间隔时间
        /// <summary>
        /// 间隔时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_interval", MedName = "mai_interval", WFName = "Dim_interval", DBColumn = false)]
        public Int32 MaiInterval { get; set; }
        #endregion

        #region 附加字段   保养类型
        /// <summary>
        /// 保养类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_type", MedName = "mai_type", WFName = "Dim_type", DBColumn = false)]
        public String MaiType { get; set; }
        #endregion

        #region 附加字段   操作提示
        /// <summary>
        /// 操作提示
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_operate_tips", MedName = "mai_operate_tips", WFName = "Dim_operate_tips", DBColumn = false)]
        public String MaiOperateTips { get; set; }
        #endregion

        #region 附加字段   保养限制
        /// <summary>
        /// 保养限制
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_astrict", MedName = "mai_astrict", WFName = "Dim_astrict", DBColumn = false)]
        public String MaiAstrict { get; set; }
        #endregion


        #region 附加字段   组别编码
        /// <summary>
        /// 组别编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "type_id", MedName = "pro_id", WFName = "Dpro_id", DBColumn = false)]
        public String ProId { get; set; }
        #endregion

        #region 附加字段   仪器删除标志
        /// <summary>
        /// 仪器删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_del", MedName = "del_flag", WFName = "itr_del", DBColumn = false)]
        public String DelFlag { get; set; }
        #endregion

        #region 附加字段   仪器状态
        /// <summary>
        /// 仪器状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_status", MedName = "itr_status", WFName = "itr_status", DBColumn = false)]
        public String ItrStatus { get; set; }
        #endregion

        #region 附加字段   fuzzy_warning_time
        /// <summary>
        /// 提醒时间模糊计算标志(1:勾选；0：未勾选)
        /// </summary>   
        [FieldMapAttribute(ClabName = "fuzzy_warning_time", MedName = "fuzzy_warning_time", WFName = "Dim_fuzzy_warning_time", DBColumn = false)]
        public String FuzzyWarningTime { get; set; }
        #endregion

        #region 附加字段  关闭审核时间模糊计算标志(1:勾选；0：未勾选)
        /// <summary>
        /// 关闭审核时间模糊计算标志(1:勾选；0：未勾选)
        /// </summary>   
        [FieldMapAttribute(ClabName = "fuzzy_closeAudit_time", MedName = "fuzzy_closeAudit_time", WFName = "Dim_fuzzy_closeAudit_time", DBColumn = false)]
        public String FuzzyCloseAuditTime { get; set; }
        #endregion

        #region 附加字段  间隔时间模糊计算标志(1:勾选；0：未勾选)
        /// <summary>
        /// 间隔时间模糊计算标志(1:勾选；0：未勾选)
        /// </summary>   
        [FieldMapAttribute(ClabName = "fuzzy_interval_time", MedName = "fuzzy_interval_time", WFName = "Dim_fuzzy_interval_time", DBColumn = false)]
        public String FuzzyIntervalTime { get; set; }
        #endregion

        #region 附加字段 下一次保养时间
        /// <summary>
        /// 下一次保养时间
        /// </summary>
        public string NextMaintainTime { get; set; }
        #endregion

        #region 附加字段 超出保养时间
        /// <summary>
        /// 超出保养时间
        /// </summary>
        public string OverrunIntervalTime { get; set; }
        #endregion

        #region 附加字段 是否超过警告时间
        /// <summary>
        /// 是否超过警告时间
        /// </summary>
        public bool IsOverrunWaringTime { get; set; }
        #endregion

        #region 附加字段 是否超过审核时间
        /// <summary>
        /// 是否超过审核时间
        /// </summary>
        public bool IsOverrunAuditTime { get; set; }
        #endregion

        #region 附加字段 上一次操作时间
        /// <summary>
        /// 上一次操作时间
        /// </summary>
        public DateTime LastOperateTime { get; set; }
        #endregion

        #region 附加字段 操作方式
        /// <summary>
        /// 操作方式
        /// </summary>
        public object RegOperateType { get; set; }
        #endregion

        #region 附加字段 保养操作
        /// <summary>
        /// 保养操作
        /// </summary>
        public string RegOperateContent { get; set; }
        #endregion

        #region 描述
        /// <summary>
        /// 描述
        /// </summary>   
        [FieldMapAttribute(ClabName = "mai_desc", MedName = "mai_desc", WFName = "Dim_desc", DBColumn = false)]
        public String MaiDesc { get; set; }
        #endregion
    }
}
