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
    public class EntityUserInstrmt : EntityBase
    {
        /// <summary>
        ///对应Sys_user表中的user_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "Buitr_Buser_id")]
        public String UserId { get; set; }

        /// <summary>
        ///对应dict_Instrmt表中的itr_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "itrId", MedName = "itr_id", WFName = "Buitr_Ditr_id")]
        public String ItrId { get; set; }

        #region 附加字段
        /// <summary>
        ///对应Sys_user表中的user_loginid
        /// </summary>   
        [FieldMapAttribute(ClabName = "loginId", MedName = "user_loginid", WFName = "Buser_loginid", DBColumn = false)]
        public String UserLoginid { get; set; }
        #endregion

        #region 附加字段
        /// <summary>
        ///对应Sys_user表中的user_name
        /// </summary>   
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public String UserName { get; set; }
        #endregion
    }
}
