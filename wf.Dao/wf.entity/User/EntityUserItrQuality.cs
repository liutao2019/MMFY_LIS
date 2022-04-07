using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 用户与仪器关系表
    /// </summary>
    [Serializable]
    public class EntityUserItrQuality : EntityBase
    {
        /// <summary>
        ///对应Sys_user表中的user_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "Buqcitr_Buser_id")]
        public String UserId { get; set; }

        /// <summary>
        ///对应dict_Instrmt表中的itr_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itrId", MedName = "itr_id", WFName = "Buqcitr_Ditr_id")]
        public String ItrId { get; set; }

    }
}
