using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 质控物规则
    /// 旧表名:Dic_qc_rule 新表名:Dict_qc_rule 
    /// </summary>
    [Serializable]
    public class EntityDicQcRule : EntityBase
    {
        public EntityDicQcRule()
        {

        }


        public EntityDicQcRule(String rulName,
                               Int32 rulNAmount,
                               Int32 rulMAmount,
                               Decimal rulSdAmount,
                               Int32 rulLevelType,
                               Int32 sortNo,
                               String rulType,
                               Int32 rulIsMoreLevel,
                               Int32 rulIsDesc)
        {
            RulName = rulName;
            RulNAmount = rulNAmount;
            RulMAmount = rulMAmount;
            RulSdAmount = rulSdAmount;
            RulLevelType = rulLevelType;
            SortNo = sortNo;
            RulType = rulType;
            RulIsMoreLevel = rulIsMoreLevel;
            RulIsDesc = rulIsDesc;
        }

        /// <summary>
        /// 编码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_id", MedName = "rul_id", WFName = "Drule_id")]
        public String RulId { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_name", MedName = "rul_name", WFName = "Drule_name")]
        public String RulName { get; set; }

        /// <summary>
        /// 规则描述
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_exp", MedName = "rul_explain", WFName = "Drule_explain")]
        public String RulExplain { get; set; }

        /// <summary>
        /// N个结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_n_sum", MedName = "rul_n_amount", WFName = "Drule_n_amount")]
        public Int32 RulNAmount { get; set; }

        /// <summary>
        /// 几个结果
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_m_sum", MedName = "rul_m_amount", WFName = "Drule_m_amount")]
        public Int32 RulMAmount { get; set; }

        /// <summary>
        /// 标准差
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_sd_sum", MedName = "rul_sd_amount", WFName = "Drule_sd_amount")]
        public Decimal RulSdAmount { get; set; }

        /// <summary>
        /// 是否同一水平
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_n_type", MedName = "rul_level_type", WFName = "Drule_level_type")]
        public Int32 RulLevelType { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_wb", MedName = "wb_code", WFName = "wb_code")]
        public String WbCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_py", MedName = "py_code", WFName = "py_code")]
        public String PyCode { get; set; }

        /// <summary>
        /// 排序码
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_seq", MedName = "sort_no", WFName = "sort_no")]
        public Int32 SortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_del", MedName = "del_flag", WFName = "del_flag")]
        public String DelFlag { get; set; }

        /// <summary>
        /// 类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_type", MedName = "rul_type", WFName = "Drule_type")]
        public String RulType { get; set; }

        /// <summary>
        ///  多水平判断 0 单水平 1多水平
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_isMoreLevel", MedName = "rul_isMoreLevel", WFName = "Drule_ismorelevel")]
        public Int32 RulIsMoreLevel { get; set; }

        /// <summary>
        /// 渐升或渐降
        /// </summary>   
        [FieldMapAttribute(ClabName = "rule_is_desc", MedName = "rul_is_desc", WFName = "Drule_isdesc")]
        public Int32 RulIsDesc { get; set; }

        #region
        /// <summary>
        /// 质控物ID（暂用，后期多水平审核改造时将去除）
        /// </summary>
        [FieldMapAttribute(ClabName = "qcr_key", MedName = "mar_sn", WFName = "Dmat_id", DBColumn = false)]
        public String MatSn { get; set; }

        #endregion
    }
}
