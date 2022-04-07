using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 数据表实体类:TAT时间关系表
    /// 旧表名:Dic_itm_combine_timerule_related 新表名:Dict_itm_combine_timerule_related
    /// </summary>
    [Serializable()]
    public class EntityDicCombineTimeruleRelated : EntityBase
    {
        /// <summary>
        ///编码 对应dict_itm_Combine表中的Comid
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_id", MedName = "com_id", WFName = "Dtrr_Dcom_id")]
        public String ComId { get; set; }

        /// <summary>
        ///对应dict_itm_combine_timerule 中的com_time_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_time_id", MedName = "com_time_id", WFName = "Dtrr_Dtr_id")]
        public String ComTimeId { get; set; }

     
    }
}
