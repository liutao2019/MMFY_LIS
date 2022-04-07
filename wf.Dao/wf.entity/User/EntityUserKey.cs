using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 用户与用户码关系表
    /// </summary>
    [Serializable]
    public class EntityUserKey : EntityBase
    {
        /// <summary>
        ///对应Sys_user表中的user_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "userId", MedName = "userId", WFName = "userId")]
        public String UserId { get; set; }

        /// <summary>
        ///用户登录ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "userLoginId", MedName = "userLoginId", WFName = "userLoginId")]
        public String UserLoginId { get; set; }


        /// <summary>
        ///用户码
        /// </summary>   
        [FieldMapAttribute(ClabName = "userKey", MedName = "UserKey", WFName = "UserKey")]
        public String Userkey { get; set; }


        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "user_certinfo", MedName = "user_certinfo", WFName = "user_certinfo")]
        public String UserCertInfo { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "user_certpassword", MedName = "user_certpassword", WFName = "user_certpassword")]
        public String UserCertPassword { get; set; }
    }
}
