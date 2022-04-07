using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 试管架定义表
    /// 旧表名:Dic_tube_rack 新表名:Dict_tube_rack
    /// </summary>
    [Serializable]
    public class EntityDicTubeRack : EntityBase
    {
        /// <summary>
        /// 试管架编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "cus_code", MedName = "rack_code", WFName = "Dtrack_code")]
        public String RackCode { get; set; }

        /// <summary>
        /// 试管架名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "cus_name", MedName = "rack_name", WFName = "Dtrack_name")]
        public String RackName { get; set; }

        /// <summary>
        /// 试管架类型(备用)
        /// </summary>   
        [FieldMapAttribute(ClabName = "cus_type", MedName = "rack_type", WFName = "Dtrack_type")]
        public String RackType { get; set; }

        /// <summary>
        /// 试管横向孔数
        /// </summary>   
        [FieldMapAttribute(ClabName = "cus_x_num", MedName = "rack_x_amount", WFName = "Dtrack_x_amount")]
        public Int32 RackXAmount { get; set; }

        /// <summary>
        /// 试管纵向孔数
        /// </summary>   
        [FieldMapAttribute(ClabName = "cus_y_num", MedName = "rack_y_amount", WFName = "Dtrack_y_amount")]
        public Int32 RackYAmount { get; set; }

    }
}
