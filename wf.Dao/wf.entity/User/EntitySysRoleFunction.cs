using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 角色功能
    /// 旧表名:Sys_role_function 新表名:Base_role_function
    /// </summary>
    [Serializable]
    public class EntitySysRoleFunction:EntityBase
    {
        /// <summary>
        ///角色ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "roleInfoId", MedName = "role_id", WFName = "Brf_Brole_id")]
        public Int32 RoleId { get; set; }

        /// <summary>
        ///功能ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "funcInfoId", MedName = "func_id", WFName = "Brf_Bfunc_id")]
        public Int32 FuncId { get; set; }
    }
}
