using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 项目参考值表
    ///旧表名:Def_itm_refdetail 新表名:Rel_itm_refdetail
    /// </summary>
    [Serializable()]
    public class EntityDicItmRefdetail : EntityBase
    {
        public EntityDicItmRefdetail()
        {
            this.ListAllowResult = new List<string>();
        }

        /// <summary>
        ///项目ID 对应dict_item表中的itm_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_id", MedName = "itm_id", WFName = "Rref_Ditm_id")]
        public String ItmId { get; set; }

        /// <summary>
        ///标本ID 对应dict_sample表中的sam_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sam_id", MedName = "itm_sam_id", WFName = "Rref_Dsam_id")]
        public String ItmSamId { get; set; }

        /// <summary>
        ///性别  0-未知 1-男 2-女
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sex", MedName = "itm_sex", WFName = "Rref_sex")]
        public String ItmSex { get; set; }

        /// <summary>
        ///年龄下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_age_l", MedName = "age_lower_limit", WFName = "Rref_age_lower_limit")]
        public Int32 ItmAgeLowerLimit { get; set; }

        /// <summary>
        ///年龄上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_age_h", MedName = "age_upper_limit", WFName = "Rref_age_upper_limit")]
        public Int32 ItmAgeUpperLimit { get; set; }

        /// <summary>
        ///年龄下限单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_age_l_unit", MedName = "age_lower_limit_unit", WFName = "Rref_age_lower_limit_unit")]
        public String ItmAgeLowerLimitUnit { get; set; }

        /// <summary>
        ///年龄上限单位
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_age_h_unit", MedName = "age_upper_limit_unit", WFName = "Rref_age_upper_limit_unit")]
        public String ItmAgeUpperLimitUnit { get; set; }

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
        ///参考值名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ref_stages", MedName = "ref_name", WFName = "Rref_name")]
        public String ItmRefName { get; set; }

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
        ///结果差异系数
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_diff", MedName = "result_diff", WFName = "Rref_result_diff")]
        public Decimal ItmResultDiff { get; set; }

        /// <summary>
        ///自增主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_mi_id", MedName = "det_id", WFName = "Rref_id")]
        public Int32 ItmDetId { get; set; }

        /// <summary>
        ///标本备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sam_rem", MedName = "sam_remark", WFName = "Rref_sam_remark")]
        public String ItmSamRemark { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_mi_del", MedName = "del_flag", WFName = "del_flag")]
        public String ItmDelFlag { get; set; }


        private string _ItmResultAllow;

        public List<string> ListAllowResult { get; private set; }

        /// <summary>
        ///允许出现的结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_allow_result", MedName = "result_allow", WFName = "Rref_result_allow")]
        public String ItmResultAllow
        {
            get
            {
                return this._ItmResultAllow;
            }
            set
            {
                this._ItmResultAllow = value;

                this.ListAllowResult.Clear();

                if (!string.IsNullOrEmpty(this._ItmResultAllow))
                {
                    foreach (string item in this._ItmResultAllow.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        this.ListAllowResult.Add(item);
                    }
                }
            }
        }

        /// <summary>
        ///仪器ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_itr_id", MedName = "itm_itr_id", WFName = "Rref_Ditr_id")]
        public String ItmItrId { get; set; }

        /// <summary>
        ///线性值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_line_l", MedName = "line_lower_limit", WFName = "Rref_line_lower_limit")]
        public String ItmLineLowerLimit { get; set; }

        /// <summary>
        ///线性值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_line_h", MedName = "line_upper_limit", WFName = "Rref_line_upper_limit")]
        public String ItmLineUpperLimit { get; set; }

        /// <summary>
        ///可报告下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_report_l", MedName = "report_lower_limit", WFName = "Rref_report_lower_limit")]
        public String ItmReportLowerLimit { get; set; }

        /// <summary>
        ///可报告上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_report_h", MedName = "report_upper_limit", WFName = "Rref_report_upper_limit")]
        public String ItmReportUpperLimit { get; set; }


        /// <summary>
        /// 危机值下限意义
        /// </summary>
        [FieldMapAttribute(ClabName = "danger_lower_mean", MedName = "danger_lower_mean", WFName = "Rref_danger_low_mean")]
        public String DangerLowerMean { get; set; }

        /// <summary>
        /// 危机值上限意义
        /// </summary>
        [FieldMapAttribute(ClabName = "danger_upper_mean", MedName = "danger_upper_mean", WFName = "Rref_danger_up_mean")]
        public String DangerUpperMean { get; set; }

        /// <summary>
        /// 水平一
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve1", MedName = "Decisive_leve1", WFName = "Rref_dec_level1")]
        public String DecisiveLeve1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve1_mean", MedName = "Decisive_leve1_mean", WFName = "Rref_level1_mean")]
        public String DecisiveLeve1Mean { get; set; }

        /// <summary>
        /// 水平二
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve2", MedName = "Decisive_leve2", WFName = "Rref_dec_level2")]
        public String DecisiveLeve2 { get; set; }
        /// <summary>
        /// 意义
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve2_mean", MedName = "Decisive_leve2_mean", WFName = "Rref_level2_mean")]
        public String DecisiveLeve2Mean { get; set; }
        /// <summary>
        /// 水平三
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve3", MedName = "Decisive_leve3", WFName = "Rref_dec_level3")]
        public String DecisiveLeve3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve3_mean", MedName = "Decisive_leve3_mean", WFName = "Rref_level3_mean")]
        public String DecisiveLeve3Mean { get; set; }

        /// <summary>
        /// 水平四
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve4", MedName = "Decisive_leve4", WFName = "Rref_dec_level4")]
        public String DecisiveLeve4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [FieldMapAttribute(ClabName = "Decisive_leve4_mean", MedName = "Decisive_leve4_mean", WFName = "Rref_level4_mean")]
        public String DecisiveLeve4Mean { get; set; }


        #region 附加字段
        /// <summary>
        ///排序
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_sort", MedName = "itm_sort", WFName = "itm_sort", DBColumn = false)]
        public Int32 ItmSort { get; set; }
        #endregion

        #region 附加字段 参考值，阈值，危急值的计算字段
        /// <summary>
        ///参考值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ref_l_cal", MedName = "itm_ref_l_cal", WFName = "itm_ref_l_cal", DBColumn = false)]
        public String ItmLowerLimitValueCal { get; set; }

        /// <summary>
        ///参考值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_ref_h_cal", MedName = "itm_ref_h_cal", WFName = "itm_ref_h_cal", DBColumn = false)]
        public String ItmUpperLimitValueCal { get; set; }

        /// <summary>
        ///阀值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_max_cal", MedName = "itm_max_cal", WFName = "itm_max_cal", DBColumn = false)]
        public String ItmMaxValueCal { get; set; }

        /// <summary>
        ///阀值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_min_cal", MedName = "itm_min_cal", WFName = "itm_min_cal", DBColumn = false)]
        public String ItmMinValueCal { get; set; }

        /// <summary>
        ///危机值下限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_pan_l_cal", MedName = "itm_pan_l_cal", WFName = "itm_pan_l_cal", DBColumn = false)]
        public String ItmDangerLowerLimitCal { get; set; }

        /// <summary>
        ///危机值上限
        /// </summary>   
        [FieldMapAttribute(ClabName = "itm_pan_h_cal", MedName = "itm_pan_h_cal", WFName = "itm_pan_h_cal", DBColumn = false)]
        public String ItmDangerUpperLimitCal { get; set; }

        /// <summary>
        /// 极大扩展值
        /// </summary>
        public System.String ext_itm_max { get; set; }

        /// <summary>
        /// 极小扩展值
        /// </summary>
        public System.String ext_itm_min { get; set; }
        #endregion
    }
}
