using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    ///酶标板孔位状态表
    ///旧表名:Dic_Elisa_plate_status 新表名:Dict_Elisa_plate_status
    /// </summary>
    [Serializable()]
    public class EntityDicElisaStatus : EntityBase
    {
        public EntityDicElisaStatus()
        {
            DelFlag = 0;
        }
        /// <summary>
        ///编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_id", MedName = "sta_id", WFName = "Depsta_id")]
        public String StaId { get; set; }

        /// <summary>
        ///孔位状态名称，格式为：,1,1,1,1…
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_name", MedName = "sta_name", WFName = "Depsta_name")]
        public String StaName { get; set; }

        /// <summary>
        ///酶标板孔位状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_holestatus", MedName = "sta_hole_staus", WFName = "Depsta_hole_staus")]
        public String StaHoleStaus { get; set; }

        /// <summary>
        ///酶标板ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_s_id", MedName = "plate_id", WFName = "Depsta_Dplate_id")]
        public String PlateId { get; set; }

        /// <summary>
        ///删除标志
        /// </summary>   
        [FieldMapAttribute(ClabName = "imh_del", MedName = "del_flag", WFName = "del_flag")]
        public Int32 DelFlag { get; set; }
    }
}
