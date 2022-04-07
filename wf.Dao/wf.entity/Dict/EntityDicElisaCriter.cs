using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 酶标判断对照表
    /// 旧表名:Def_Elisa_criter 新表名:Rel_Elisa_criter
    /// </summary>
    [Serializable]
    public class EntityDicElisaCriter : EntityBase
    {
        public EntityDicElisaCriter()
        {
            CriDelFlag = "0";
        }

        /// <summary>
        /// 编号
        /// </summary>
        [FieldMapAttribute(ClabName = "imj_id", MedName = "cri_id", WFName = "Recri_id")]
        public String CriId { get; set; }

        /// <summary>
        /// 项目代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "imj_itm_id", MedName = "cri_itm_id", WFName = "Recri_Ditm_id")]
        public String CriItmId { get; set; }

        /// <summary>
        /// 判断值表达式，如：>,>=,<,<=
        /// </summary>   
        [FieldMapAttribute(ClabName = "imj_express", MedName = "cri_judge", WFName = "Recri_judge")]
        public String CriJudge { get; set; }

        /// <summary>
        /// 判断值 暂不使用
        /// </summary>   
        [FieldMapAttribute(ClabName = "imj_value", MedName = "cri_value", WFName = "Recri_value")]
        public decimal CriValue { get; set; }



        /// <summary>
        /// 定性 pos：阳性，neg：阴性
        /// </summary>   
        [FieldMapAttribute(ClabName = "imj_res", MedName = "cri_result", WFName = "Recri_result")]
        public String CriResult { get; set; }

        /// <summary>
        /// 阴性最小
        /// </summary>   
        [FieldMapAttribute(ClabName = "imj_neg_min", MedName = "cri_neg_lower_limit", WFName = "Recri_neg_lower_limit")]
        public Decimal CriNegLowerLimit { get; set; }
        /// <summary>
        /// 阴性最大
        /// </summary>   
        [FieldMapAttribute(ClabName = "imj_neg_max", MedName = "cri_neg_upper_limit", WFName = "Recri_neg_upper_limit")]
        public Decimal CriNegUpperLimit { get; set; }

        /// <summary>
        /// 阳性最小
        /// </summary>   
        [FieldMapAttribute(ClabName = "imj_pos_min", MedName = "cri_pos_lower_limit", WFName = "Recri_pos_lower_limit")]
        public Decimal CriPosLowerlimit { get; set; }

        /// <summary>
        /// 阳性最大
        /// </summary>    
        [FieldMapAttribute(ClabName = "imj_pos_max", MedName = "cri_pos_upper_limit", WFName = "Recri_pos_upper_limit")]
        public Decimal CriPosUpperlimit { get; set; }

        /// <summary>
        /// 判定值表达式 取代imj_value
        /// </summary>    
        [FieldMapAttribute(ClabName = "imj_valueexpress", MedName = "cri_expression", WFName = "Recri_expression")]
        public String  CriExpression { get; set; }

        /// <summary>
        ///  弱阳最小值 支持表达式
        /// </summary>    
        [FieldMapAttribute(ClabName = "imj_feebpos_min", MedName = "cri_wpos_lower_limit", WFName = "Recri_wpos_lower_limit")]
        public String CriWposLowerLimit { get; set; }

        /// <summary>
        /// 弱阳最大值 支持表达式，扩展字段
        /// </summary>
  
        [FieldMapAttribute(ClabName = "imj_feebpos_max", MedName = "cri_wpos_upper_limit", WFName = "Recri_wpos_upper_limit")]
        public String CriWposUpperLimit { get; set; }

        /// <summary>
        /// 是否减去空白对照 
        /// </summary>    
        [FieldMapAttribute(ClabName = "imj_minusnull", MedName = "cri_ignore_null_sam", WFName = "Recri_ignore_null_sam")]
        public String CriIgnoreNullSam { get; set; }

        /// <summary>
        ///  删除标志
        /// </summary>    
        [FieldMapAttribute(ClabName = "imj_del", MedName = "del_flag", WFName = "del_flag")]
        public String CriDelFlag { get; set; }


    }
}
