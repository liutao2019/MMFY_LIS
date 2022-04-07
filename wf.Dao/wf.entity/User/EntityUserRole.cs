using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 用户与权限关系表
    /// 旧表:Sys_user_role 新表:Base_user_role
    /// </summary>
    [Serializable]
    public class EntityUserRole : EntityBase
    {
        /// <summary>
        ///对应Sys_user表中的user_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "Bur_Buser_id")]
        public String UserId { get; set; }

        /// <summary>
        ///对应sys_power_role表中的roles_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "roleInfoId", MedName = "role_id", WFName = "Bur_Brole_id")]
        public String RoleId { get; set; }

        #region 附加字段 用户登录Id
        /// <summary>
        /// 用户登录Id
        /// </summary>
        [FieldMapAttribute(ClabName = "loginId", MedName = "user_loginid", WFName = "Buser_loginid", DBColumn = false)]
        public String UserLoginId { get; set; }
        #endregion

        #region 附加字段 角色描述
        /// <summary>
        /// 角色描述
        /// </summary>
        [FieldMapAttribute(ClabName = "roleDesc", MedName = "role_remark", WFName = "Brole_remark", DBColumn = false)]
        public String RoleRemark { get; set; }
        #endregion

    }
}
