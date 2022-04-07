using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 项目参考值，阈值等信息
    /// </summary>
    public class EntityItmRefInfo
    {

        #region 项目字典表 字段
        /* EntityDicItmItem 项目字典表 字段*/
        /// <summary>
        ///项目ID
        /// </summary>           
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        ///项目名称
        /// </summary>     
        [FieldMapAttribute(ClabName = "itm_name", MedName = "itm_name", WFName = "Ditm_name")]
        public String ItmName { get; set; }

        /// <summary>
        ///专业组
        /// </summary>               
        [FieldMapAttribute(ClabName = "itm_ptype", MedName = "itm_pri_id", WFName = "Ditm_pri_id")]
        public String ItmPriId { get; set; }

        /// <summary>
        ///项目代码
        /// </summary>                       
        [FieldMapAttribute(ClabName = "itm_ecd", MedName = "itm_ecode", WFName = "Ditm_ecode")]
        public String ItmEcode { get; set; }

        /// <summary>
        ///排序编号(序号)
        /// </summary>                 
        [FieldMapAttribute(ClabName = "itm_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 ItmSortNo { get; set; }

        /// <summary>
        ///报表打印时使用的项目代码
        /// </summary>                 
        [FieldMapAttribute(ClabName = "itm_rep_ecd", MedName = "itm_rep_code", WFName = "Ditm_rep_code")]
        public String ItmRepCode { get; set; }

        /// <summary>
        ///实验方法
        /// </summary>            
        [FieldMapAttribute(ClabName = "itm_meams", MedName = "itm_method", WFName = "Ditm_method")]
        public String ItmMethod { get; set; }

        /// <summary>
        ///是否计算项目 0-否 1-是  (当设置为是时，在检验报告中此项目将不能手工录入，将根据项目计算公式自动计算)
        /// </summary>       
        [FieldMapAttribute(ClabName = "itm_cal_flag", MedName = "itm_calu_flag", WFName = "Ditm_calu_flag")]
        public Int32 ItmCaluFlag { get; set; }

        #region 附加字段 用于使用的报表打印时使用的项目代码
        /// <summary>
        /// 报表打印时使用的项目代码
        /// </summary>
        public String ItmRepEcd { get; set; }
        #endregion

        #endregion

        #region 项目标本表 字段
        /* EntityDicItemSample 项目标本表 字段*/
        /// <summary>
        ///标本ID 对应dict_sample表中的sam_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sam_id", MedName = "itm_sam_id", WFName = "Ritm_sam_id")]
        public String ItmSamId { get; set; }

        /// <summary>
        ///价格
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_pri", MedName = "itm_price", WFName = "Ritm_price")]
        public Decimal ItmPrice { get; set; }

        /// <summary>
        ///单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_unit", MedName = "itm_unit", WFName = "Ritm_unit")]
        public String ItmUnit { get; set; }

        /// <summary>
        ///默认值
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_defa", MedName = "itm_default", WFName = "Ritm_default")]
        public String ItmDefault { get; set; }

        /// <summary>
        ///结果类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_dtype", MedName = "itm_res_type", WFName = "Ritm_res_type")]
        public String ItmResType { get; set; }


        /// <summary>
        ///最大小数位（0表示不限定小数位）
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_max_digit", MedName = "itm_max_digit", WFName = "Ritm_max_digit")]
        public Int32 ItmMaxDigit { get; set; }


        /// <summary>
        ///阳性结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_positive_result", MedName = "itm_positive_res", WFName = "Ritm_compare_res")]
        public String ItmPositiveRes { get; set; }

        /// <summary>
        ///描述性危急值结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_urgent_result", MedName = "itm_urgent_res", WFName = "Ritm_urgent_res")]
        public String ItmUrgentRes { get; set; }
        #endregion

        #region 项目参考值表 字段
        /* EntityDicItmRefdetail 项目参考值表 字段*/
        /// <summary>
        ///性别  0-未知 1-男 2-女
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sex", MedName = "itm_sex", WFName = "Rref_sex")]
        public String ItmSex { get; set; }

        /// <summary>
        ///年龄下限(转换到分钟)
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_age_ls", MedName = "age_lower_minute", WFName = "Rref_age_lower_minute")]
        public Decimal ItmAgeLowerMinute { get; set; }

        /// <summary>
        ///年龄上限(转换到分钟)
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_age_hs", MedName = "age_upper_minute", WFName = "Rref_age_upper_minute")]
        public Decimal ItmAgeUpperMinute { get; set; }

        /// <summary>
        ///参考值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ref_l", MedName = "lower_limit_value", WFName = "Rref_lower_limit_value")]
        public String ItmLowerLimitValue { get; set; }

        /// <summary>
        ///参考值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ref_h", MedName = "upper_limit_value", WFName = "Rref_upper_limit_value")]
        public String ItmUpperLimitValue { get; set; }


        /// <summary>
        ///阀值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_max", MedName = "max_value", WFName = "Rref_max_value")]
        public String ItmMaxValue { get; set; }

        /// <summary>
        ///阀值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_min", MedName = "min_value", WFName = "Rref_min_value")]
        public String ItmMinValue { get; set; }

        /// <summary>
        ///危机值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_pan_l", MedName = "danger_lower_limit", WFName = "Rref_danger_lower_limit")]
        public String ItmDangerLowerLimit { get; set; }

        /// <summary>
        ///危机值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_pan_h", MedName = "danger_upper_limit", WFName = "Rref_danger_upper_limit")]
        public String ItmDangerUpperLimit { get; set; }

        /// <summary>
        ///参考值类型 0-默认 1-分期 
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_flag", MedName = "ref_flag", WFName = "Rref_flag")]
        public Int32 ItmRefFlag { get; set; }

        /// <summary>
        ///允许出现的结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_allow_result", MedName = "result_allow", WFName = "Rref_result_allow")]
        public String ItmResultAllow { get; set; }



        #endregion

        /// <summary>
        ///极小参考值
        /// </summary>   
        public String ItmLowerLimitValueCal { get; set; }

        /// <summary>
        ///极大参考值
        /// </summary>   
        public String ItmUpperLimitValueCal { get; set; }


        /// <summary>
        ///极大阀值
        /// </summary>   
        public String ItmMaxValueCal { get; set; }

        /// <summary>
        ///极小阀值
        /// </summary>   
        public String ItmMinValueCal { get; set; }

        /// <summary>
        ///极小危机值
        /// </summary>   
        public String ItmDangerLowerLimitCal { get; set; }

        /// <summary>
        ///极大危机值
        /// </summary>   
        public String ItmDangerUpperLimitCal { get; set; }
    }
}
