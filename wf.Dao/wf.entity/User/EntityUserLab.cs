using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 用户与物理组关系表
    /// </summary>
    [Serializable]
    public class EntityUserLab : EntityBase
    {
        /// <summary>
        ///对应Sys_user表中的user_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "Bul_Buser_id")]
        public String UserId { get; set; }

        /// <summary>
        ///对应Dic_Pub_ profession表中的pro_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "typeSourceId", MedName = "lab_id", WFName = "Bul_lab_id")]
        public String LabId { get; set; }

    }
}
