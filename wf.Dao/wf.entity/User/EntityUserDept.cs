using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 用户与权限关系表
    /// </summary>
    [Serializable]
    public class EntityUserDept : EntityBase
    {
        /// <summary>
        ///对应Sys_user表中的user_id
        /// </summary>   
        [FieldMapAttribute(ClabName = "userInfoId", MedName = "user_id", WFName = "Bud_Buser_id")]
        public String UserId { get; set; }

        /// <summary>
        ///对应dict_depart表中的departId
        /// </summary>   
        [FieldMapAttribute(ClabName = "dep_id", MedName = "dept_id", WFName = "Bud_Ddept_id")]
        public String DeptId { get; set; }


        #region 附加字段 用户ID
        [FieldMapAttribute(ClabName = "userId", MedName = "userId", WFName = "userId", DBColumn = false )]
        public String UserIdDep { get; set; }
        #endregion

        #region 附加字段 用户名称
        [FieldMapAttribute(ClabName = "userName", MedName = "user_name", WFName = "Buser_name", DBColumn = false)]
        public String UserName { get; set; }
        #endregion

        #region 附加字段 科室名称
        [FieldMapAttribute(ClabName = "dep_name", MedName = "dept_name", WFName = "Ddept_name", DBColumn = false)]
        public String DeptName { get; set; }
        #endregion
        
    }
}
