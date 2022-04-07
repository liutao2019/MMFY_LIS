using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///项目与酶标板孔位的对照表
    ///旧表名:Def_Elisa_item_plate 新表名:Rel_Elisa_item_plate
    /// </summary>
    [Serializable()]
    public class EntityDicElisaItem : EntityBase
    {
        public EntityDicElisaItem()
        {
            DelFlag = 0;
        }
        /// <summary>
        ///编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imi_id", MedName = "iplate_id", WFName = "Riplate_id")]
        public String IplateId { get; set; }

        /// <summary>
        ///项目ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "imi_itm_id", MedName = "iplate_itm_id", WFName = "Riplate_Ditm_id")]
        public String IplateItmId { get; set; }

        /// <summary>
        ///酶标板孔位序号模式表的编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imi_hm_id", MedName = "iplate_sort_id", WFName = "Riplate_Deps_id")]
        public String IplateSortId { get; set; }

        /// <summary>
        ///酶标板孔位状态模式表的编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imi_hs_id", MedName = "iplate_sta_id", WFName = "Riplate_Depsta_id")]
        public String IplateStaId { get; set; }

        /// <summary>
        ///酶标板ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "imi_s_id", MedName = "plate_id", WFName = "Riplate_Dplate_id")]
        public String PlateId { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "imi_del", MedName = "del_flag", WFName = "del_flag")]
        public Int32 DelFlag { get; set; }

        /// <summary>
        ///结果类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "imi_resulttype", MedName = "iplate_resulttype", WFName = "Riplate_resulttype")]
        public String IplateResulttype { get; set; }
    }
}
