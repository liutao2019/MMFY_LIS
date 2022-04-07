using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using System.Collections;

namespace dcl.dao.interfaces
{
    public interface IDaoUserManage
    {
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="isPower"></param>
        /// <returns></returns>
        List<EntitySysUser> GetUserInfo(bool isPower);
        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="isPower"></param>
        /// <returns></returns>
        List<EntitySysRole> GetRoleInfo(bool isPower);
        /// <summary>
        /// 获取物理组信息
        /// </summary>
        /// <returns></returns>
        List<EntityDicPubOrganize> GetTypeInfo();
        /// <summary>
        /// 根据用户ID查询所属物理组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserLab> GetUserTypeInfoByUserId(string userId);
        /// <summary>
        /// 根据用户ID查询所属仪器
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserInstrmt> GetUserItrInfoByUserId(string userId);
        /// <summary>
        /// 根据用户ID查询所属医院
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserHospital> GetUserHosInfoByUserId(string userId);
        /// <summary>
        /// 根据用户ID查询所属科室
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserDept> GetUserDeptInfoByUserId(string userId);

        /// <summary>
        /// 根据登录ID查询所属科室
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserDept> GetUserDeptInfoByLoginID(string loginId);
        /// <summary>
        /// 根据用户ID查询用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserRole> GetUserRoleInfoByUserId(string userId);
        /// <summary>
        /// 根据用户ID查询质控物理组
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserLabQuality> GetUserLabQInfoByUserId(string userId);
        /// <summary>
        /// 根据用户ID查询质控仪器
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserItrQuality> GetUserItrQInfoByUserId(string userId);
        /// <summary>
        /// 根据用户ID查询质控医院
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<EntityUserHosQuality> GetUserHosQInfoByUserId(string userId);

        /// <summary>
        /// 获得用户码
        /// </summary>
        /// <returns></returns>
        List<EntityUserKey> GetUserkey();
        /// <summary>
        /// 根据UserId删除所有的信息
        /// </summary>
        /// <param name="StrList"></param>
        /// <returns></returns>
        int[] DeleteUserInfo(string userInfoID);

        /// <summary>
        /// 根据UserId删除用户相关表的信息
        /// </summary>
        /// <param name="StrList"></param>
        /// <returns></returns>
        int[] DeleteUserRelate(string userInfoID);

        /// <summary>
        /// 更新用户的头像信息
        /// </summary>
        /// <param name="userSign"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UpdateUserSign(byte[] userSign, string userId);

        /// <summary>
        /// 更新用户的相关信息
        /// </summary>
        /// <returns></returns>
        bool InsertUserList(EntityPowerList userList);
        /// <summary>
        /// 更新用户表信息
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        bool UpdateUserList(List<EntitySysUser> userList);
        /// <summary>
        /// 新增用户表信息
        /// </summary>
        /// <param name="userList"></param>
        /// <returns></returns>
        string AddUserList(List<EntitySysUser> userList);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        bool UpdateUserPassword(string loginId,string newPassword);

        /// <summary>
        /// 获取用户与物理组的关系
        /// </summary>
        /// <param name="loginid"></param>
        /// <returns></returns>
        List<EntityUserLab> GetLabIdByLoginId(string loginid);

        /// <summary>
        /// 获取用户与仪器关系缓存
        /// </summary>
        /// <returns></returns>
        List<EntityUserInstrmt> GetUserInstrmtCache();

        /// <summary>
        /// 删除用户CA认证信息
        /// </summary>
        /// <param name="CerId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        bool DelCerid(string CerId, string EntityId);

        /// <summary>
        /// 根据登录Id更新用户CA认证信息
        /// </summary>
        /// <param name="LoginId"></param>
        /// <param name="CerId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        bool SetCerid(string LoginId, string CerId, string EntityId);

        /// <summary>
        /// 停用用户账号
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        bool UpdateUserFlag(string loginId);
    }
}
