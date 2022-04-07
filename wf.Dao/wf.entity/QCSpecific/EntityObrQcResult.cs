using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 质控结果表
    /// 旧表名:Obr_qc_result  新表名:Lis_qc_result
    /// </summary>
    [Serializable]
    public class EntityObrQcResult : EntityBase
    {
        /// <summary>
        ///仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_itr_id", MedName = "qres_itr_id", WFName = "Lres_Ditr_id")]
        public String QresItrId { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_itm_ecd", MedName = "qres_itm_id", WFName = "Lres_Ditm_id")]
        public String QresItmId { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_date", MedName = "qres_date", WFName = "Lres_date")]
        public DateTime QresDate { get; set; }

        /// <summary>
        ///质控结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_meas", MedName = "qres_value", WFName = "Lres_value")]
        public String QresValue { get; set; }

        /// <summary>
        ///质控水平
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_c_no", MedName = "qres_level", WFName = "Lres_level")]
        public String QresLevel { get; set; }

        /// <summary>
        ///检验者
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_i_name", MedName = "qres_audit_user_id", WFName = "Lres_audit_Buser_id")]
        public String QresAuditUserId { get; set; }

        /// <summary>
        ///审核标志(0未审核 1审核)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_flg", MedName = "qres_audit_flag", WFName = "Lres_audit_flag")]
        public Int32 QresAuditFlag { get; set; }

        /// <summary>
        ///质控物ID(qc_par_detail表ID)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_id", MedName = "qres_mat_det_id", WFName = "Lres_Rmatdet_id")]
        public String QresMatDetId { get; set; }

        /// <summary>
        ///显示标志(0显示1不显示)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_view_flag", MedName = "qres_display", WFName = "Lres_display")]
        public Int32 QresDisplay { get; set; }

        /// <summary>
        ///失控规则
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_ns", MedName = "qres_runaway_rule", WFName = "Lres_runaway_rule")]
        public String QresRunawayRule { get; set; }

        /// <summary>
        ///靶值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_c_x", MedName = "qres_itm_x", WFName = "Lres_itm_x")]
        public Decimal? QresItmX { get; set; }

        /// <summary>
        ///标准差
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_c_sd", MedName = "qres_itm_sd", WFName = "Lres_itm_sd")]
        public Decimal? QresItmSd { get; set; }

        /// <summary>
        ///失控原因
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_reson", MedName = "qres_reasons", WFName = "Lres_reasons")]
        public String QresReasons { get; set; }

        /// <summary>
        ///处理方式
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_fun", MedName = "qres_process", WFName = "Lres_process")]
        public String QresProcess { get; set; }

        /// <summary>
        ///主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_bs", MedName = "qres_sn", WFName = "Lres_id", DBIdentity = true)]
        public Int32 QresSn { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        ///显示规则标志 0 正常 1警告 2失控
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_rule_flag", MedName = "qres_runaway_flag", WFName = "Lres_runaway_flag")]
        public String QresRunawayFlag { get; set; }

        /// <summary>
        ///二次审核时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_two_audit_date", MedName = "qres_secondaudit_date", WFName = "Lres_secondaudit_date")]
        public DateTime? QresSecondauditDate { get; set; }

        /// <summary>
        ///二次检验者
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_two_audit_name", MedName = "qres_secondaudit_user_id", WFName = "Lres_secondaudit_Buser_id")]
        public String QresSecondauditUserId { get; set; }

        /// <summary>
        ///二次审核间隔
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_two_audit_interval", MedName = "qres_secondaudit_interval", WFName = "Lres_secondaudit_interval")]
        public Int32 QresSecondauditInterval { get; set; }

        /// <summary>
        ///质控类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_type", MedName = "qres_type", WFName = "Lres_type")]
        public String QresType { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_rem", MedName = "qres_remark", WFName = "Lres_remark")]
        public String QresRemark { get; set; }

        /// <summary>
        ///审核时间
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_audit_date", MedName = "qres_audit_date", WFName = "Lres_audit_date")]
        public DateTime QresAuditDate { get; set; }

        /// <summary>
        ///实际值
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcm_cast_meas", MedName = "qres_convert_value", WFName = "Lres_convert_value")]
        public String QresConvertValue { get; set; }

        #region 附加字段 项目代码
        /// <summary>
        ///项目代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode", DBColumn = false)]
        public String ItmEcode { get; set; }
        #endregion

        #region 附加字段 项目名称
        /// <summary>
        ///项目名称
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public String ItmName { get; set; }
        #endregion

        #region 附加字段 实际值(半定量字典)
        /// <summary>
        ///实际值(半定量字典)
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_cast_meas", MedName = "itm_convert_value", WFName = "Rqcv_convert_value", DBColumn = false)]
        public String ItmConvertValue { get; set; }
        #endregion

        #region 附加字段 靶值（字典）
        /// <summary>
        /// 靶值（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_x", MedName = "mat_itm_x", WFName = "Rmatdet_itm_x", DBColumn = false)]
        public Double? MatItmX { get; set; }
        #endregion

        #region 附加字段 标准差（字典）
        /// <summary>
        /// 标准差（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_c_sd", MedName = "mat_itm_sd", WFName = "Rmatdet_itm_sd", DBColumn = false)]
        public double? MatItmSd { get; set; }
        #endregion

        #region 附加字段 CCV（字典）
        /// <summary>
        /// CCV（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_ccv", MedName = "mat_itm_ccv", WFName = "Rmatdet_itm_ccv", DBColumn = false)]
        public Decimal? MatItmCcv { get; set; }
        #endregion

        #region 附加字段 最大值（字典）
        /// <summary>
        /// 最大值（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_max_value", MedName = "mat_max_value", WFName = "Rmatdet_max_value", DBColumn = false)]
        public Decimal? MatMaxValue { get; set; }
        #endregion

        #region 附加字段 最小值（字典）
        /// <summary>
        /// 最小值（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_min_value", MedName = "mat_min_value", WFName = "Rmatdet_min_value", DBColumn = false)]
        public Decimal? MatMinValue { get; set; }
        #endregion

        #region 附加字段 测定类型（字典）
        /// <summary>
        /// 测定类型（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_value_type", MedName = "mat_value_type", WFName = "Rmatdet_value_type", DBColumn = false)]
        public String MatValueType { get; set; }
        #endregion

        #region 附加字段 浓度（字典）
        /// <summary>
        /// 浓度（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_no_type", MedName = "mat_level", WFName = "Dmat_level", DBColumn = false)]
        public String MatLevel { get; set; }
        #endregion

        #region 附加字段 质控批号（字典）
        /// <summary>
        /// 质控批号（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_r_no", MedName = "mat_batch_no", WFName = "Dmat_batch_no", DBColumn = false)]
        public String MatBatchNo { get; set; }
        #endregion

        #region 附加字段 质控物英文名称（字典）
        /// <summary>
        ///  质控物英文名称（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_ename", MedName = "mat_ename", WFName = "Dmat_ename", DBColumn = false)]
        public String MatEname { get; set; }
        #endregion

        #region 附加字段 质控物中文名称（字典）
        /// <summary>
        /// 质控物中文名称（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_cname", MedName = "mat_cname", WFName = "Dmat_cname", DBColumn = false)]
        public String MatCname { get; set; }
        #endregion

        #region 附加字段 框架法（字典）
        /// <summary>
        ///  框架法（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_sam_moth", MedName = "mat_method", WFName = "Dmat_method", DBColumn = false)]
        public String MatMethod { get; set; }
        #endregion

        #region 附加字段 生产厂家（字典）
        /// <summary>
        /// 生产厂家（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_yield_manu", MedName = "mat_manufacturer", WFName = "Dmat_manufacturer", DBColumn = false)]
        public String MatManufacturer { get; set; }
        #endregion

        #region 附加字段 使用结束日期（字典）
        /// <summary>
        /// 使用结束日期（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_edate", MedName = "mat_date_end", WFName = "Dmat_date_end", DBColumn = false)]
        public DateTime MatDateEnd { get; set; }
        #endregion

        #region 附加字段 操作者（字典）
        /// <summary>
        /// 操作者（字典）
        /// </summary>   
        [FieldMapAttribute(ClabName = "qcr_i_name", MedName = "mat_user_id", WFName = "Dmat_Buser_name", DBColumn = false)]
        public String MatUserId { get; set; }
        #endregion

        #region 附加字段 检验者名称
        /// <summary>
        ///检验者名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public String QresAuditUserName { get; set; }
        #endregion

        #region 附加字段 二次检验者名称
        /// <summary>
        ///二次检验者名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "two_audit_name", MedName = "two_audit_name", WFName = "two_audit_name", DBColumn = false)]
        public String QresSecondauditUserName { get; set; }
        #endregion

        #region 附加字段 分组名称 qcm_info_group
        /// <summary>
        /// 分组名称 qcm_info_group
        /// </summary>
        public String GroupName { get; set; }
        #endregion

        #region 附加字段 sd位置 qcr_nsd
        /// <summary>
        /// sd位置 qcr_nsd
        /// </summary>
        public Double NSD { get; set; }
        #endregion

        #region 附加字段 最终值 qcr_actual_value
        /// <summary>
        /// 最终值 qcr_actual_value
        /// </summary>
        public Double FinalValue { get; set; }
        #endregion

        #region 附加字段 结果顺序 qc_count
        /// <summary>
        /// 结果顺序 qc_count
        /// </summary>
        public int ValueSeq { get; set; }
        #endregion

        #region 附加字段 下次审核时间 nextAuditTime
        /// <summary>
        /// 下次审核时间 nextAuditTime
        /// </summary>
        public DateTime? NextAuditTime { get; set; }
        #endregion

        #region 附加字段 显示水平+批号 qc_level
        /// <summary>
        /// 显示水平+批号 qc_level
        /// </summary>
        public String QcShowLevel
        {
            get
            {
                return QresLevel + "-" + MatBatchNo;
            }
        }
        #endregion

        #region 附加字段 临时字段 用于Monica质控图 qcm_ave
        /// <summary>
        /// 临时字段 用于Monica质控图 qcm_ave
        /// </summary>
        public string QcAve { get; set; }
        #endregion

        #region 附加字段 临时字段 选中 qcm_select
        /// <summary>
        /// 临时字段 选中 qcm_select
        /// </summary>
        [FieldMapAttribute(ClabName = "checked", MedName = "checked", WFName = "checked", DBColumn = false)]
        public bool Checked { get; set; }
        #endregion

    }
}
