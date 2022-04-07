using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 酶标字典
    /// 旧表名:dic_elisa_meaning 新表名:Dict_elisa_meaning
    /// </summary>
    [Serializable]
    public class EntityDicElisaMeaning : EntityBase
    {
        /// <summary>
        ///编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_id", MedName = "meag_id", WFName = "Delisa_id")]
        public String MeagId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_hbsag", MedName = "meag_item_a", WFName = "Delisa_item_a")]
        public String MeagItemA { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_hbsab", MedName = "meag_item_b", WFName = "Delisa_item_b")]
        public String MeagItemB { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_hbeag", MedName = "meag_item_c", WFName = "Delisa_item_c")]
        public String MeagItemC { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_hbeab", MedName = "meag_item_d", WFName = "Delisa_item_d")]
        public String MeagItemD { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_hbcab", MedName = "meag_item_e", WFName = "Delisa_item_e")]
        public String MeagItemE { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_pec", MedName = "meag_exp", WFName = "Delisa_exp")]
        public String MeagExp { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_freq", MedName = "meag_probability", WFName = "Delisa_probability")]
        public String MeagProbability { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_clv", MedName = "meag_conclusion", WFName = "Delisa_conclusion")]
        public String MeagConclusion { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "mod_abcde", MedName = "meag_remark", WFName = "Delisa_remark")]
        public String MeagRemark { get; set; }

    }

    }
