using System;

namespace dcl.entity
{
    /// <summary>
    /// 结果提示信息
    /// 旧表名:Def_itm_result_tips 新表名:Rel_itm_result_tips
    /// </summary>
    [Serializable]
    public class EntityDefItmResultTips : EntityBase
    {
        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "tip_id", MedName = "tip_id", WFName = "Rtip_id")]
        public Int32 TipId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "tip_value", MedName = "tip_value", WFName = "Rtip_value")]
        public String TipValue { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "del_flag", MedName = "del_flag", WFName = "del_flag")]
        public Boolean DelFlag { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "tip_content", MedName = "tip_content", WFName = "Rtip_content")]
        public String TipContent { get; set; }

    }
}