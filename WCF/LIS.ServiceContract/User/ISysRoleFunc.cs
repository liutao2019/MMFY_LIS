using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISysRoleFunc
    {
        /// <summary>
        /// 根据角色ID删除所包含功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteFuncByRoleId(string roleId);

        /// <summary>
        /// 批量插入角色的功能信息
        /// </summary>
        /// <param name="roleFuncs"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertRoleFunc(List<EntitySysRoleFunction> roleFuncs);

        /// <summary>
        /// 根据角色ID获取所包含功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysRoleFunction> GetFuncsByRoleId(string roleId);
    }
}
