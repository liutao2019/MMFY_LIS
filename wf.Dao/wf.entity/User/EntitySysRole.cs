using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统权限表
    /// 旧表名:sys_power_role 新表名:Base_role
    /// </summary>
    [Serializable]
    public class EntitySysRole : EntityBase
    {
        /// <summary>
        ///角色编码  业务主键
        /// </summary>   
        [FieldMapAttribute(ClabName = "roleInfoId", MedName = "role_id", WFName = "Brole_id")]
        //public Int32 RoleId { get; set; }
        public String RoleId { get; set; } //数据表中是int类型，但是为了TreeList实现分组，就要让其跟UserId类型一样

        /// <summary>
        ///角色名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "roleName", MedName = "role_name", WFName = "Brole_name")]
        public String RoleName { get; set; }

        /// <summary>
        ///角色描述
        /// </summary>   
        [FieldMapAttribute(ClabName = "roleDesc", MedName = "role_remark", WFName = "Brole_remark")]
        public String RoleRemark { get; set; }

        #region 附加字段 用户ID
        /// <summary>
        ///用户ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "userId", MedName = "userId", WFName = "userId", DBColumn = false)]
        public String UserId { get; set; }
        #endregion

        #region 附加字段 用户编码
        /// <summary>
        ///用户ID
        /// </summary>   


        #region 附加字段 用户编码 
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "user_id", DBColumn = false)]
        public String UserIdMess { get; set; }
        #endregion

        #region 附加字段 用户名称
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "userInfoId", WFName = "userInfoId", DBColumn = false)]
        public String UserInfoId { get; set; }

        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "user_name", DBColumn = false)]
        public String UserName { get; set; }
        #endregion



        #endregion

        /// <summary>
        /// 角色包含的用户
        /// </summary>
        public List<EntityUserRole> listUser { get; set; }

        /// <summary>
        /// 角色包含的功能
        /// </summary>
        public List<EntitySysRoleFunction> listFunc { get; set; }

    }
}
