using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 用户与医院关系表
    /// </summary>
    [Serializable]
    public class EntityUserHosQuality : EntityBase
    {
        /// <summary>
        ///对应Sys_user表中的user_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "Buorg_Buser_id")]
        public String UserId { get; set; }

        /// <summary>
        ///对应dict_hospital表中的org_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "hos_id", MedName = "org_id", WFName = "Buser_Dorg_id")]
        public String OrgId { get; set; }

    }
}
