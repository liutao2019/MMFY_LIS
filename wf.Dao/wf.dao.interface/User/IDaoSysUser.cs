using dcl.entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSysUser
    {

        List<EntitySysUser> Search(Object obj);
        /// <summary>
        /// 获得操作人员
        /// </summary>
        /// <returns></returns>
        List<EntitySysUser> GetLoginId();

        /// <summary>
        /// 根据登录ID获取用户信息
        /// </summary>
        /// <returns></returns>
        List<EntitySysUser> GetUserInfoByLoginId(string loginid, string mark);


        /// <summary>
        /// 根据登录Id获取该Id的科室信息
        /// </summary>
        /// <param name="loginid"></param>
        /// <returns></returns>
        List<EntitySysUser> GetDepartByLoginId(string loginid);

        /// <summary>
        /// 获取科室权限
        /// </summary>
        /// <returns></returns>
        List<EntitySysUser> GetPowerUserInfo();

        /// <summary>
        /// 根据角色ID获取用户信息
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        List<EntitySysUser> SysUserQuery(EntityUserQc userQc);


        #region IPowerUser

        ArrayList FindUser(string departmentId);

        ArrayList FindDepartments(string userId);

        ArrayList FindDepartments_Code(string userId);

        List<string> GetUserIDForRoleName(string RoleName);

        int AddUserinfoKey(string loginId, string keyCode, byte[] certinfo, string password);

        string Getuserpwinfo(string loginId);

        #endregion

    }
}
