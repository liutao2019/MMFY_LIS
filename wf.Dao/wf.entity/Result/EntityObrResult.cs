using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 结果表
    /// 旧表名:Obr_result 新表名:Lis_result
    /// </summary>
    [Serializable]
    public class EntityObrResult : EntityBase
    {
        public EntityObrResult()
        {
            IsModify = 0;
            IsNew = 0;
            ObrFlag = 1;
            ResOriginRecord = 1;
            ItmMaxDigit = 0;

            #region 历史结果等赋值
            ResRefRange = string.Empty;
            ResAllowValues = string.Empty;
            ResCustomCriticalResult = string.Empty;
            HistoryDate1 = string.Empty;
            HistoryDate2 = string.Empty;
            HistoryDate3 = string.Empty;
            HistoryResult1 = string.Empty;
            HistoryResult2 = string.Empty;
            HistoryResult3 = string.Empty;
            RefFlagHistory1 = 0;
            RefFlagHistory2 = 0;
            RefFlagHistory3 = 0;
            hasItmForillflag = false;
            #endregion
        }

        /// <summary>
        ///唯一标识
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_key", MedName = "obr_sn", WFName = "Lres_id", DBIdentity = true)]
        public Int64 ObrSn { get; set; }

        /// <summary>
        ///标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_id", MedName = "obr_id", WFName = "Lres_Pma_rep_id")]
        public String ObrId { get; set; }

        /// <summary>
        ///仪器代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_id", MedName = "obr_itr_id", WFName = "Lres_Ditr_id")]
        public String ObrItrId { get; set; }

        /// <summary>
        ///样本号
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_sid", MedName = "obr_sid", WFName = "Lres_sid")]
        public String ObrSid { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_id", MedName = "itm_id", WFName = "Lres_Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        ///项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_ecd", MedName = "itm_ename", WFName = "Lres_itm_ename")]
        public String ItmEname { get; set; }

        /// <summary>
        ///结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr", MedName = "obr_value", WFName = "Lres_value")]
        public String ObrValue { get; set; }

        /// <summary>
        ///OD结果(仪器类型为酶标时插入)
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_od_chr", MedName = "obr_value2", WFName = "Lres_value2")]
        public String ObrValue2 { get; set; }

        /// <summary>
        ///数值结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_cast_chr", MedName = "obr_convert_value", WFName = "Lres_convert_value")]
        public Decimal? ObrConvertValue { get; set; }

        /// <summary>
        ///单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_unit", MedName = "obr_unit", WFName = "Lres_unit")]
        public String ObrUnit { get; set; }

        /// <summary>
        ///价格
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_price", MedName = "itm_price", WFName = "Lres_price")]
        public Decimal? ItmPrice { get; set; }

        /// <summary>
        ///参考值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_l", MedName = "ref_lower_limit", WFName = "Lres_lower_limit")]
        public String RefLowerLimit { get; set; }

        /// <summary>
        ///参考值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_h", MedName = "ref_upper_limit", WFName = "Lres_upper_limit")]
        public String RefUpperLimit { get; set; }

        public String ResRef
        {
            get
            {
                if (string.IsNullOrEmpty(RefLowerLimit))
                {
                    return RefUpperLimit;
                }
                else if (string.IsNullOrEmpty(RefUpperLimit))
                {
                    return RefLowerLimit;
                }
                else
                {
                    return RefLowerLimit + "-" + RefUpperLimit;
                }
            }
        }

        /// <summary>
        ///参考值分期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_exp", MedName = "ref_more", WFName = "Lres_more")]
        public String RefMore { get; set; }

        /// <summary>
        ///阳性标志(阳性为'3'，未知'-1',正常为'0'）关联dict_res_ref_flag
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_flag", MedName = "ref_flag", WFName = "Lres_ref_flag")]
        public String RefFlag { get; set; }

        /// <summary>
        ///实验方法
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_meams", MedName = "obr_itm_method", WFName = "Lres_itm_method")]
        public String ObrItmMethod { get; set; }

        /// <summary>
        ///结果日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_date", MedName = "obr_date", WFName = "Lres_date")]
        public DateTime ObrDate { get; set; }

        /// <summary>
        ///有效标志 0-历史结果 1-生效结果  默认为1
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_flag", MedName = "obr_flag", WFName = "Lres_flag")]
        public Int32 ObrFlag { get; set; }

        /// <summary>
        ///结果类型 0-手工输入 1仪器传输 2计算  默认为0
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_type", MedName = "obr_type", WFName = "Lres_type")]
        public Int32 ObrType { get; set; }

        /// <summary>
        ///报告类型 0-普通 1-OD结果 默认为0
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_rep_type", MedName = "obr_report_type", WFName = "Lres_itm_report_type")]
        public Int32 ObrReportType { get; set; }

        /// <summary>
        ///组合ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_com_id", MedName = "itm_com_id", WFName = "Lres_itm_Dcom_id")]
        public String ItmComId { get; set; }

        /// <summary>
        ///报表打印时项目使用的编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itm_rep_ecd", MedName = "itm_report_code", WFName = "Lres_itm_report_code")]
        public String ItmReportCode { get; set; }

        /// <summary>
        ///仪器原始代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_itr_ori_id", MedName = "obr_source_itr_id", WFName = "Lres_source_Ditr_id")]
        public String ObrSourceItrId { get; set; }

        /// <summary>
        ///参考值类型 0=常规 1=分期
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_type", MedName = "ref_type", WFName = "Lres_ref_type")]
        public Int32 RefType { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_exp", MedName = "obr_remark", WFName = "Lres_remark")]
        public String ObrRemark { get; set; }

        /// <summary>
        ///复查标记
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_recheck_flag", MedName = "obr_recheck_flag", WFName = "Lres_recheck_flag")]
        public Int32 ObrRecheckFlag { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr2", MedName = "obr_value3", WFName = "Lres_value3")]
        public String ObrValue3 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_chr3", MedName = "obr_value4", WFName = "Lres_value4")]
        public String ObrValue4 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_Alarm_a", MedName = "obr_warn1", WFName = "Lres_warn1")]
        public String ObrWarn1 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_Alarm_b", MedName = "obr_warn2", WFName = "Lres_warn2")]
        public String ObrWarn2 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_Alarm_c", MedName = "obr_warn3", WFName = "Lres_warn3")]
        public String ObrWarn3 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_Alarm_d", MedName = "obr_warn4", WFName = "Lres_warn4")]
        public String ObrWarn4 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_Alarm_e", MedName = "obr_warn5", WFName = "Lres_warn5")]
        public String ObrWarn5 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_Alarm_f", MedName = "obr_warn6", WFName = "Lres_warn6")]
        public String ObrWarn6 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_Alarm_g", MedName = "obr_warn7", WFName = "Lres_warn7")]
        public String ObrWarn7 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_verify", MedName = "obr_register", WFName = "Lres_register")]
        public String ObrRegister { get; set; }

        /// <summary>
        ///骨髓记录
        /// </summary>   
        [FieldMapAttribute(ClabName = "obr_bone_value", MedName = "obr_bone_value", WFName = "Lres_bone_value")]
        public String ObrBoneValue { get; set; }

        /// <summary>
        ///血片记录
        /// </summary>   
        [FieldMapAttribute(ClabName = "obr_bld_value", MedName = "obr_bld_value", WFName = "Lres_bld_value")]
        public String ObrBldValue { get; set; }


        #region 附加字段 提示
        /// <summary>
        ///提示
        /// </summary>   
        [FieldMapAttribute(ClabName = "value", MedName = "tip_value", WFName = "Rtip_value", DBColumn = false)]
        public String ResTips { get; set; }
        #endregion

        /// <summary>
        ///格式化提示
        /// </summary>   
        public String ResPrompt
        {
            get
            {
                if (ResTips == "正常")
                {
                    return "";
                }
                else
                {
                    return ResTips;
                }
            }
        }

        #region 附加字段 备注（仪器原始数据查询使用）
        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "msg", MedName = "msg", WFName = "msg", DBColumn = false)]
        public String ObrMsg { get; set; }
        #endregion

        #region 附加字段 结果来源
        /// <summary>
        ///结果来源
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_or", MedName = "res_or", WFName = "res_or", DBColumn = false)]
        public String ResOr { get; set; }
        #endregion

        #region 样本号整形 （仪器原始数据查询使用）
        /// <summary>
        ///样本号整形
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_sid_int", MedName = "obr_sid_int", WFName = "obr_sid_int", DBColumn = false)]
        public Int64 ObrSidInt { get; set; }
        #endregion

        #region 附加字段 项目名称
        /// <summary>
        ///项目名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name", DBColumn = false)]
        public String ItmName { get; set; }
        #endregion

        #region 附加字段 病人表标识ID
        /// <summary>
        ///病人表标识ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_id", MedName = "rep_id", WFName = "Pma_rep_id", DBColumn = false)]
        public String RepId { get; set; }
        #endregion

        #region 附加字段 状态 0-未审核 1-已审核 2-已报告
        /// <summary>
        ///状态 0-未审核 1-已审核 2-已报告
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_flag", MedName = "rep_status", WFName = "Pma_status", DBColumn = false)]
        public Int64 RepStatus { get; set; }
        #endregion

        #region 附加字段 是否选中
        /// <summary>
        ///是否选中
        /// </summary>   
        [FieldMapAttribute(ClabName = "isselected", MedName = "isselected", WFName = "isselected", DBColumn = false)]
        public Boolean IsSelected { get; set; }
        #endregion

        #region 附加字段 用于客户端排序(组合排序)
        /// <summary>
        ///用于客户端排序(组合排序)
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_seq", MedName = "sort_no", WFName = "sort_no", DBColumn = false)]
        public int? ResComSeq { get; set; }
        #endregion

        #region 附加字段 组合明细排序
        /// <summary>
        ///组合明细排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_mi_sort", MedName = "com_mi_sort", WFName = "com_mi_sort", DBColumn = false)]
        public Int64 ComMiSort { get; set; }
        #endregion

        #region 附加字段 组合名称
        /// <summary>
        ///组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_name", MedName = "com_name", WFName = "Dcom_name", DBColumn = false)]
        public String ComName { get; set; }
        #endregion

        #region 附加字段 是否修改
        /// <summary>
        /// 是否修改
        /// </summary>
        [FieldMapAttribute(ClabName = "ismodify", MedName = "ismodify", WFName = "ismodify", DBColumn = false)]
        public int IsModify { get; set; }
        #endregion

        #region 附加字段 是否新增(可去掉特性)
        /// <summary>
        /// 是否新增
        /// </summary>
        [FieldMapAttribute(ClabName = "isnew", MedName = "isnew", WFName = "isnew", DBColumn = false)]
        public int IsNew { get; set; }
        #endregion

        #region 附加字段 计算来源项目ID(可去掉特性)
        /// <summary>
        /// 计算来源项目ID
        /// </summary>
        [FieldMapAttribute(ClabName = "calculate_source_itm_id", MedName = "calculate_source_itm_id", WFName = "calculate_source_itm_id", DBColumn = false)]
        public String CalculateSourceItmId { get; set; }
        #endregion

        #region 附加字段 计算目的地项目ID(可去掉特性)
        /// <summary>
        /// 计算Dest项目ID
        /// </summary>
        [FieldMapAttribute(ClabName = "calculate_dest_itm_id", MedName = "calculate_dest_itm_id", WFName = "calculate_dest_itm_id", DBColumn = false)]
        public String CalculateDestItmId { get; set; }
        #endregion

        #region 附加字段 参考值范围(可去掉特性)
        /// <summary>
        /// 参考值范围
        /// </summary>
        [FieldMapAttribute(ClabName = "res_ref_range", MedName = "res_ref_range", WFName = "res_ref_range", DBColumn = false)]
        public String ResRefRange { get; set; }
        #endregion

        #region 附加字段 历史结果1(可去掉特性)
        /// <summary>
        /// 历史结果1
        /// </summary>
        [FieldMapAttribute(ClabName = "history_result1", MedName = "history_result1", WFName = "history_result1", DBColumn = false)]
        public String HistoryResult1 { get; set; }
        #endregion

        #region 附加字段 历史结果2(可去掉特性)
        /// <summary>
        /// 历史结果2
        /// </summary>
        [FieldMapAttribute(ClabName = "history_result2", MedName = "history_result2", WFName = "history_result2", DBColumn = false)]
        public String HistoryResult2 { get; set; }
        #endregion

        #region 附加字段 历史结果3(可去掉特性)
        /// <summary>
        /// 历史结果3
        /// </summary>
        [FieldMapAttribute(ClabName = "history_result3", MedName = "history_result3", WFName = "history_result3", DBColumn = false)]
        public String HistoryResult3 { get; set; }
        #endregion

        #region 附加字段 历史结果日期1(可去掉特性)
        /// <summary>
        /// 历史结果1
        /// </summary>
        [FieldMapAttribute(ClabName = "history_date1", MedName = "history_date1", WFName = "history_date1", DBColumn = false)]
        public String HistoryDate1 { get; set; }
        #endregion

        #region 附加字段 历史结果日期2(可去掉特性)
        /// <summary>
        /// 历史结果2
        /// </summary>
        [FieldMapAttribute(ClabName = "history_date2", MedName = "history_date2", WFName = "history_date2", DBColumn = false)]
        public String HistoryDate2 { get; set; }
        #endregion

        #region 附加字段 历史结果日期3(可去掉特性)
        /// <summary>
        /// 历史结果3
        /// </summary>
        [FieldMapAttribute(ClabName = "history_date3", MedName = "history_date3", WFName = "history_date3", DBColumn = false)]
        public String HistoryDate3 { get; set; }
        #endregion

        #region 附加字段 历史阳性标志1(可去掉特性)
        /// <summary>
        ///历史阳性标志1(阳性为'3'，未知'-1',正常为'0'）关联dict_res_ref_flag
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_flag_h1", MedName = "res_ref_flag_h1", WFName = "res_ref_flag_h1", DBColumn = false)]
        public int RefFlagHistory1 { get; set; }
        #endregion

        #region 附加字段 历史阳性标志2(可去掉特性)
        /// <summary>
        ///历史阳性标志2(阳性为'3'，未知'-1',正常为'0'）关联dict_res_ref_flag
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_flag_h2", MedName = "res_ref_flag_h2", WFName = "res_ref_flag_h2", DBColumn = false)]
        public int RefFlagHistory2 { get; set; }
        #endregion

        #region 附加字段 历史阳性标志3(可去掉特性)
        /// <summary>
        ///历史阳性标志3(阳性为'3'，未知'-1',正常为'0'）关联dict_res_ref_flag
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_flag_h3", MedName = "res_ref_flag_h3", WFName = "res_ref_flag_h3", DBColumn = false)]
        public int RefFlagHistory3 { get; set; }
        #endregion

        #region 附加字段 是否必录(可去掉特性)
        /// <summary>
        ///是否必录
        /// </summary>   
        [FieldMapAttribute(ClabName = "isnotempty", MedName = "isnotempty", WFName = "isnotempty", DBColumn = false)]
        public String IsNotEmpty { get; set; }
        #endregion

        #region 附加字段 项目序号(可去掉特性)
        /// <summary>
        ///项目序号
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_seq", MedName = "itm_seq", WFName = "itm_seq", DBColumn = false)]
        public int ItmSeq { get; set; }
        #endregion

        #region 附加字段 组合名称
        /// <summary>
        ///组合名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_com_name", MedName = "res_com_name", WFName = "res_com_name", DBColumn = false)]
        public String ResComName { get; set; }
        #endregion

        #region 附加字段 项目数据类型(可去掉特性)
        /// <summary>
        ///项目数据类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_dtype", MedName = "itm_dtype", WFName = "itm_dtype", DBColumn = false)]
        public String ItmDtype { get; set; }
        #endregion

        #region 附加字段 小数位数(可去掉特性)
        /// <summary>
        ///小数位数
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_max_digit", MedName = "itm_max_digit", WFName = "itm_max_digit", DBColumn = false)]
        public Int32 ItmMaxDigit { get; set; }
        #endregion

        #region 附加字段 阳性提示结果(可去掉特性)
        /// <summary>
        ///阳性提示结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_positive_result", MedName = "res_positive_result", WFName = "res_positive_result", DBColumn = false)]
        public String ResPositiveResult { get; set; }
        #endregion

        #region 附加字段 自定义危急值(可去掉特性)
        /// <summary>
        ///自定义危急值
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_custom_critical_result", MedName = "res_custom_critical_result", WFName = "res_custom_critical_result", DBColumn = false)]
        public String ResCustomCriticalResult { get; set; }
        #endregion

        #region 附加字段 允许出现的结果(可去掉特性)
        /// <summary>
        ///允许出现的结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_allow_values", MedName = "res_allow_values", WFName = "res_allow_values", DBColumn = false)]
        public String ResAllowValues { get; set; }
        #endregion

        #region 附加字段 阀值上限(可去掉特性)
        /// <summary>
        ///阀值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_max", MedName = "res_max", WFName = "res_max", DBColumn = false)]
        public String ResMax { get; set; }
        #endregion

        #region 附加字段 阀值下限(可去掉特性)
        /// <summary>
        ///阀值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_min", MedName = "res_min", WFName = "res_min", DBColumn = false)]
        public String ResMin { get; set; }
        #endregion

        #region 附加字段 危急值上限(可去掉特性)
        /// <summary>
        ///危急值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_pan_h", MedName = "res_pan_h", WFName = "res_pan_h", DBColumn = false)]
        public String ResPanH { get; set; }
        #endregion

        #region 附加字段 危急值下限(可去掉特性)
        /// <summary>
        ///危急值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_pan_l", MedName = "res_pan_l", WFName = "res_pan_l", DBColumn = false)]
        public String ResPanL { get; set; }
        #endregion

        #region 附加字段 极大阀值(可去掉特性)
        /// <summary>
        ///极大阀值
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_max_cal", MedName = "res_max_cal", WFName = "res_max_cal", DBColumn = false)]
        public String ResMaxCal { get; set; }
        #endregion

        #region 附加字段 极小阀值(可去掉特性)
        /// <summary>
        ///极小阀值
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_min_cal", MedName = "res_min_cal", WFName = "res_min_cal", DBColumn = false)]
        public String ResMinCal { get; set; }
        #endregion

        #region 附加字段 极大危急值(可去掉特性)
        /// <summary>
        ///危急值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_pan_h_cal", MedName = "res_pan_h_cal", WFName = "res_pan_h_cal", DBColumn = false)]
        public String ResPanHCal { get; set; }
        #endregion

        #region 附加字段 极小危急值(可去掉特性)
        /// <summary>
        ///危急值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_pan_l_cal", MedName = "res_pan_l_cal", WFName = "res_pan_l_cal", DBColumn = false)]
        public String ResPanLCal { get; set; }
        #endregion

        #region 附加字段 极大参考值(可去掉特性)
        /// <summary>res_
        ///极大参考值
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_h_cal", MedName = "res_ref_h_cal", WFName = "res_ref_h_cal", DBColumn = false)]
        public String ResRefHCal { get; set; }
        #endregion

        #region 附加字段 极小参考值(可去掉特性)
        /// <summary>
        ///极小参考值
        /// </summary>   
        [FieldMapAttribute(ClabName = "res_ref_l_cal", MedName = "res_ref_l_cal", WFName = "res_ref_l_cal", DBColumn = false)]
        public String ResRefLCal { get; set; }
        #endregion

        #region 附加字段 来源记录
        /// <summary>
        /// 来源记录(默认1)
        /// </summary>
        public int ResOriginRecord { get; set; }
        #endregion

        #region 附加字段 是否计算项目
        /// <summary>
        ///是否计算项目
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_cal_flag", MedName = "itm_calu_flag", WFName = "Ditm_calu_flag", DBColumn = false)]
        public int? ItmCaluFlag { get; set; }
        #endregion

        #region  附加字段 用于展示和条件判断的结果类型
        /// <summary>
        /// 用于展示和条件判断的结果类型
        /// </summary>
        public int ObrTypeShow
        {
            get
            {
                if (ItmCaluFlag?.ToString().Trim() == "1")
                {
                    return 2;
                }
                else
                {
                    return ObrType;
                }
            }
        }
        #endregion

        #region 附加字段 外部报告
        /// <summary>
        /// 外部报告
        /// </summary>
        public String PatReportId { get; set; }
        #endregion

        #region 附加字段  病人姓名
        /// <summary>
        ///病人姓名
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_name", MedName = "pid_name", WFName = "Pma_pat_name", DBColumn = false)]
        public string PidName { get; set; }
        #endregion

        #region 附加字段 病人ID
        /// <summary>
        ///病人ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_in_no", MedName = "pid_in_no", WFName = "Pma_pat_in_no", DBColumn = false)]
        public string PidInNo { get; set; }
        #endregion

        #region 附加字段 病人科室
        /// <summary>
        ///病人科室
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_dep_name", MedName = "pid_dept_name", WFName = "Pma_pat_dept_name", DBColumn = false)]
        public string PidDeptName { get; set; }
        #endregion

        /// <summary>
        /// 是否需要删除
        /// </summary>
        public bool NeedDelete = false;

        public String ExtResRefFlag { get; set; }
        /// <summary>
        /// 是否为传染病项目
        /// </summary>
        public bool hasItmForillflag { get; set; }

        #region 附加字段 警告日期
        /// <summary>
        /// 警告日期
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_warn_trandate", MedName = "itr_warn_trandate", WFName = "itr_warn_trandate", DBColumn = false)]
        public String ItrWarnTrandate { get; set; }
        #endregion

        #region 附加字段 结果标志
        /// <summary>
        /// 结果标志
        /// </summary>
        public String ResChrFlag { get; set; }
        #endregion

        #region 附加字段 对照方式
        /// <summary>
        /// 对照方式
        /// </summary>
        [FieldMapAttribute(ClabName = "itm_con_ftor", MedName = "itm_contrast_factor", WFName = "Ditm_contrast_factor", DBColumn = false)]
        public String ItmContrastFactor { get; set; }
        #endregion

        #region 附加字段 仪器代码
        /// <summary>
        /// 仪器代码
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_mid", MedName = "itr_ename", WFName = "Ditr_ename", DBColumn = false)]
        public String ItrEname { get; set; }
        #endregion

        #region 附加字段 仪器原始仪器名称
        /// <summary>
        /// 仪器原始名称
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_name", MedName = "itr_name", WFName = "Ditr_name", DBColumn = false)]
        public String ObrSourceItrName { get; set; }
        #endregion

        #region 附加字段 仪器子标题（报告单类型）
        /// <summary>
        /// 仪器子标题（报告单类型）
        /// </summary>
        [FieldMapAttribute(ClabName = "itr_stitle", MedName = "itr_sub_title", WFName = "Ditr_sub_title", DBColumn = false)]
        public String DitrSubTitle { get; set; }
        #endregion

        #region 附加字段 序号(双向)
        /// <summary>
        /// 序号(双向)
        /// </summary>
        [FieldMapAttribute(ClabName = "pat_host_order", MedName = "rep_serial_num", WFName = "Pma_serial_num", DBColumn = false)]
        public String RepSerialNum { get; set; }
        #endregion

        #region 附加字段 检验者
        /// <summary>
        ///检验者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_i_code", MedName = "rep_check_user_id", WFName = "Pma_check_Buser_id", DBColumn = false)]
        public String RepCheckUserId { get; set; }
        #endregion

        #region 附加字段 审核者
        /// <summary>
        ///检验者
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_chk_code", MedName = "rep_audit_user_id", WFName = "Pma_audit_Buser_id", DBColumn = false)]
        public String RepAuditUserId { get; set; }
        #endregion

        #region 附加字段 报告日期
        /// <summary>
        ///报告日期
        /// </summary>   
        [FieldMapAttribute(ClabName = "pat_report_date", MedName = "rep_report_date", WFName = "Pma_report_date", DBColumn =false)]
        public DateTime? RepReportDate { get; set; }
        #endregion

        #region 附加字段 数值是否需要进行转换计算
        /// <summary>
        ///数值是否需要进行转换计算
        /// </summary>   
        public bool IsValueNeedCalculate { get; set; }
        #endregion

        #region 附加字段 对照代码
        /// <summary>
        ///对照代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_con_code", MedName = "itm_contrast_code", WFName = "Ditm_contrast_code", DBColumn =false)]
        public String ItmContrastCode { get; set; }
        #endregion

        #region 附加字段 实验序号（用于茂名妇幼的资料导入）
        /// <summary>
        ///实验序号（用于茂名妇幼的资料导入）
        /// </summary>                       
        public String TestSeq { get; set; }
        #endregion
    }
}
