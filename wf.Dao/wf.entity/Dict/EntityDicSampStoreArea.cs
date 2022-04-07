using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 冰箱柜子字典
    /// 旧表名:dic_samp_store_area 新表名:Dict_sample_store_position
    /// </summary>
    [Serializable]
    public class EntityDicSampStoreArea : EntityBase
    {
        /// <summary>
        ///冰箱ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "cup_ice_id", MedName = "store_id", WFName = "Dpos_Dstore_id")]
        public String StoreId { get; set; }

        /// <summary>
        ///柜子代码
        /// </summary>   
        [FieldMapAttribute(ClabName = "cup_code", MedName = "area_code", WFName = "Dpos_code")]
        public String AreaCode { get; set; }

        /// <summary>
        ///柜子名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "cup_name", MedName = "area_name", WFName = "Dpos_name")]
        public String AreaName { get; set; }

        /// <summary>
        ///柜子容量
        /// </summary>   
        [FieldMapAttribute(ClabName = "cup_capa", MedName = "area_capacity", WFName = "Dpos_capacity")]
        public String AreaCapacity { get; set; }

        /// <summary>
        ///状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "cup_status", MedName = "area_status", WFName = "Dpos_status")]
        public Int32 AreaStatus { get; set; }

        /// <summary>
        ///说明
        /// </summary>   
        [FieldMapAttribute(ClabName = "cup_exp", MedName = "area_remark", WFName = "Dpos_remark")]
        public String AreaRemark { get; set; }

        /// <summary>
        ///柜子ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "cup_id", MedName = "area_id", WFName = "Dpos_id")]
        public String AreaId { get; set; }
    }
}
