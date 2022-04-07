using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISysUserRole
    {
        /// <summary>
        /// 根据角色ID删除所包含用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteUserByRoleId(string roleId);

        /// <summary>
        /// 批量插入角色的用户信息
        /// </summary>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertUserRole(List<EntityUserRole> userRoles);

        /// <summary>
        /// 根据角色ID获取所包含用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityUserRole> GetUsersByRoleId(string roleId);

        /// <summary>
        /// 获取所有用户角色信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityUserRole> GetAllUserRole();
    }
}
