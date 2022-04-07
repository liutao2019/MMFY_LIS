using dcl.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISysUserInfo
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysUser> GetAllUsers(string par);

        /// <summary>
        /// 根据条件实体查询用户
        /// </summary>
        /// <param name="userQc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysUser> SysUserQuery(EntityUserQc userQc);


        #region IPowerUser

        [OperationContract]
        ArrayList FindUser(string departmentId);

        [OperationContract]
        ArrayList FindDepartments(string userId);

        [OperationContract]
        ArrayList FindDepartments_Code(string userId);

        [OperationContract]
        List<string> GetUserIDForRoleName(string RoleName);

        [OperationContract]
        int AddUserinfoKey(string loginId, string keyCode, byte[] certinfo, string password);

        [OperationContract]
        string Getuserpwinfo(string loginId);

        #endregion
    }
}
