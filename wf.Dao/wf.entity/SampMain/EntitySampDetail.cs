using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 条码项目明细表
    /// 旧表名:Samp_detail 新表名:Sample_detail
    /// </summary>
    [Serializable]
    public class EntitySampDetail : EntityBase
    {
        /// <summary>
        /// 自增ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "det_sn", WFName = "Sdet_sn", DBIdentity = true)]
        public Decimal DetSn { get; set; }

        /// <summary>
        /// 内部关联ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_no", MedName = "samp_bar_id", WFName = "Sdet_Sma_bar_id")]
        public String SampBarId { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_bar_code", MedName = "samp_bar_code", WFName = "Sdet_bar_code")]
        public String SampBarCode { get; set; }

        /// <summary>
        /// 条码批次
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_frequency", MedName = "samp_bar_batch_no", WFName = "Sdet_bar_batch_no")]
        public Int32 SampBarBatchNo { get; set; }

        /// <summary>
        /// HIS项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_his_name", MedName = "order_name", WFName = "Sdet_order_name")]
        public String OrderName { get; set; }

        /// <summary>
        /// HIS项目编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_his_code", MedName = "order_code", WFName = "Sdet_order_code")]
        public String OrderCode { get; set; }

        /// <summary>
        /// 医嘱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_yz_id", MedName = "order_sn", WFName = "Sdet_order_sn")]
        public String OrderSn { get; set; }

        /// <summary>
        /// 类别(预留)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_ctype", MedName = "samp_type", WFName = "Sdet_type")]
        public String SampType { get; set; }

        /// <summary>
        /// 上机标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_flag", MedName = "samp_flag", WFName = "Sdet_flag")]
        public Int32 SampFlag { get; set; }

        /// <summary>
        /// 条码日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_date", MedName = "samp_date", WFName = "Sdet_date")]
        public DateTime SampDate { get; set; }

        /// <summary>
        /// 医嘱申请日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_apply_date", MedName = "order_date", WFName = "Sdet_order_date")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 医嘱执行日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_occ_date", MedName = "order_occ_date", WFName = "Sdet_order_occ_date")]
        public DateTime OrderOccDate { get; set; }

        /// <summary>
        /// 项目序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_item_no", MedName = "order_com_no", WFName = "Sdet_order_com_no")]
        public Int32 OrderComNo { get; set; }

        /// <summary>
        /// 确认HIS标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_confirm_flag", MedName = "confirm_flag", WFName = "Sdet_confirm_flag")]
        public Int32 ConfirmFlag { get; set; }

        /// <summary>
        /// 确认者工号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_confirm_code", MedName = "confirm_user_id", WFName = "Sdet_confirm_user_id")]
        public String ConfirmUserId { get; set; }

        /// <summary>
        /// 确认者姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_confirm_name", MedName = "confirm_user_name", WFName = "Sdet_confirm_user_name")]
        public String ConfirmUserName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_price", MedName = "order_price", WFName = "Sdet_order_price")]
        public Decimal OrderPrice { get; set; }

        /// <summary>
        /// 单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_unit", MedName = "order_unit", WFName = "Sdet_order_unit")]
        public String OrderUnit { get; set; }

        /// <summary>
        /// 上机标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_modify_flag", MedName = "comm_flag", WFName = "Sdet_comm_flag")]
        public Int32 CommFlag { get; set; }

        /// <summary>
        /// 上机时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_modify_date", MedName = "comm_date", WFName = "Sdet_comm_date")]
        public DateTime CommDate { get; set; }

        /// <summary>
        /// 上机仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_itr_id", MedName = "comm_itr_id", WFName = "Sdet_comm_Ditr_id")]
        public String CommItrId { get; set; }

        /// <summary>
        /// 显示标志(0-显示 1-不显示)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_view_flag", MedName = "display_flag", WFName = "Sdet_display_flag")]
        public Int32 DisplayFlag { get; set; }

        /// <summary>
        /// 执行专业组编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_exec_code", MedName = "exec_code", WFName = "Sdet_exec_code")]
        public String ExecCode { get; set; }

        /// <summary>
        /// 执行专业组名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_exec_name", MedName = "ecec_name", WFName = "Sdet_ecec_name")]
        public String EcecName { get; set; }

        /// <summary>
        /// 项目预计出报告时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_out_time", MedName = "plan_report_time", WFName = "Sdet_plan_report_time")]
        public Int32 PlanReportTime { get; set; }

        /// <summary>
        /// 实际出报告标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_rep_flag", MedName = "report_flag", WFName = "Sdet_report_flag")]
        public Int32 ReportFlag { get; set; }

        /// <summary>
        /// 实际出报告日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_rep_date", MedName = "report_date", WFName = "Sdet_report_date")]
        public DateTime ReportDate { get; set; }

        /// <summary>
        /// 登记标志(0-未登记 1-已登记)
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_enrol_flag", MedName = "enrol_flag", WFName = "Sdet_enrol_flag")]
        public Int32 EnrolFlag { get; set; }

        /// <summary>
        /// 预留医嘱相关信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_other_no", MedName = "order_other", WFName = "Sdet_order_other")]
        public String OrderOther { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        /// LIS组合编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lis_code", MedName = "com_id", WFName = "Sdet_com_id")]
        public String ComId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_name", MedName = "com_name", WFName = "Sdet_com_name")]
        public String ComName { get; set; }

        /// <summary>
        /// 采血注意
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_blood_notice", MedName = "blood_notice", WFName = "Sdet_blood_notice")]
        public String BloodNotice { get; set; }

        /// <summary>
        /// 保存注意
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_save_notice", MedName = "save_notice", WFName = "Sdet_save_notice")]
        public String SaveNotice { get; set; }

        /// <summary>
        /// 医嘱ID2
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_yz_id2", MedName = "order_sn2", WFName = "Sdet_order_sn2")]
        public String OrderSn2 { get; set; }

        /// <summary>
        /// 实验码
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lc_code", MedName = "lab_test_code", WFName = "Sdet_lab_test_code")]
        public String LabTestCode { get; set; }

        /// <summary>
        /// 工作单id
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lc_work_id", MedName = "lab_work_id", WFName = "Sdet_lab_work_id")]
        public String LabWorkId { get; set; }

        /// <summary>
        /// 申请单号（茂名妇幼）
        /// </summary>   
        [FieldMapAttribute(ClabName = "Sdet_apply_id", MedName = "Sdet_apply_id", WFName = "Sdet_apply_id")]
        public String ApplyID { get; set; }


        #region 附加信息 组合备注

        /// <summary>
        /// 组合备注
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_rem", MedName = "com_remark", WFName = "Dcom_remark", DBColumn = false)]
        public String ComRemark { get; set; }

        #endregion

        #region 附加信息 加急标志
        /// <summary>
        /// 加急标志
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_urgent_flag", MedName = "com_urgent_flag", WFName = "Dcom_urgent_flag", DBColumn = false)]
        public Int32 ComUrgentFlag { get; set; }

        #endregion

        #region 附加信息 排序

        /// <summary>
        /// 排序
        /// </summary>                       
        [FieldMapAttribute(ClabName = "com_seq", MedName = "sort_no", WFName = "sort_no", DBColumn = false)]
        public string SortNo { get; set; }

        #endregion

        #region 附加信息 组合拆分标志

        /// <summary>
        /// 组合拆分标志(0-不拆分 1-拆分) 标识该组合是否由大组合拆分而来
        /// </summary>
        [FieldMapAttribute(DBColumn = false)]
        public String ComSplitFlag { get; set; }

        #endregion

        #region 附加信息 病人姓名

        /// <summary>
        /// 病人姓名（）
        /// </summary>
        [FieldMapAttribute(ClabName = "pid_name", MedName = "pid_name", WFName = "pid_name", DBColumn = false)]
        public String PidName { get; set; }

        #endregion
    }
}
