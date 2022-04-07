using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///酶标板孔位序号表
    ///旧表名:Dic_Elisa_plate_sort 新表名:Dict_Elisa_plate_sort
    /// </summary>
    [Serializable()]
    public class EntityDicElisaSort : EntityBase
    {
        public EntityDicElisaSort()
        {
            DelFlag = 0;
        }
        /// <summary>
        ///编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_id", MedName = "sort_id", WFName = "Deps_id")]
        public String SortId { get; set; }

        /// <summary>
        ///孔位序号名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_name", MedName = "sort_name", WFName = "Deps_name")]
        public String SortName { get; set; }

        /// <summary>
        ///酶标板孔位序号,格式为：,1,2,3,4…
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_holemode", MedName = "sort_hole_sorting", WFName = "Deps_hole_sorting")]
        public String SortHoleSorting { get; set; }

        /// <summary>
        ///酶标板ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_s_id", MedName = "plate_id", WFName = "Deps_Dplate_id")]
        public String PlateId { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_del", MedName = "del_flag", WFName = "del_flag")]
        public Int32 DelFlag { get; set; }
    }
}
