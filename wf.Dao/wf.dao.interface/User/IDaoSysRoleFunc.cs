using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSysRoleFunc
    {
        /// <summary>
        /// 根据角色ID删除所包含功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        bool DeleteFuncByRoleId(string roleId);

        /// <summary>
        /// 批量插入角色的功能信息
        /// </summary>
        /// <param name="roleFuncs"></param>
        /// <returns></returns>
        bool InsertRoleFunc(List<EntitySysRoleFunction> roleFuncs);

        /// <summary>
        /// 根据角色ID获取所包含功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<EntitySysRoleFunction> GetFuncsByRoleId(string roleId);
    }
}
