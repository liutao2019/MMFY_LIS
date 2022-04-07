using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IRoleManagePro
    {
        /// <summary>
        /// 删除角色信息及其所包含的用户信息和功能信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteRoleInfo(EntitySysRole role);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertRoleInfo(EntitySysRole role);

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateRoleInfo(EntitySysRole role);

        /// <summary>
        /// 获取所有角色.用户，功能
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetAllInfo();

        /// <summary>
        /// 根据角色ID获取用户和功能
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [OperationContract]
        EntitySysRole GetRoleUserAndFunc(string roleId);
    }
}
