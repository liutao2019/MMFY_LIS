using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 交班
    /// 旧表名:dict_hand_over 新表名:dict_hand_over
    /// </summary>
    [Serializable]
    public class EntityDicHandOver: EntityBase
    {

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ho_id", MedName = "ho_id", WFName = "ho_id")]
        public Int64 HoId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ho_type_id", MedName = "ho_type_id", WFName = "ho_type_id")]
        public String HoTypeId { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ho_time1", MedName = "ho_time1", WFName = "ho_time1")]
        public String HoTime1 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ho_time2", MedName = "ho_time2", WFName = "ho_time2")]
        public String HoTime2 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ho_time3", MedName = "ho_time3", WFName = "ho_time3")]
        public String HoTime3 { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "ho_timeInter", MedName = "ho_timeInter", WFName = "ho_timeInter")]
        public String HoTimeInter { get; set; }


        public String TypeName { get; set; }
        public Boolean IsNew { get; set; }
    }
}
