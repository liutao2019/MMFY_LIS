using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using System.Data;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IUserManage
    {
        /// <summary>
        /// 获取页面数据信息
        /// </summary>
        /// <param name="isPower"></param>
        /// <returns></returns>
        [OperationContract]
        EntityPowerList GetViewData(bool isPower);

        /// <summary>
        /// 获取用户与数据的关系
        /// </summary>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        [OperationContract]
        EntityPowerList GetEntityList(EntitySysUser UserInfo);

        /// <summary>
        /// 删除用户相关数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteUserInfo(EntitySysUser ds);

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddUserInfo(EntityPowerList obj);
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateUserInfo(EntityPowerList obj);

        /// <summary>
        /// 删除用户CA认证信息
        /// </summary>
        /// <param name="CerId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DelCerid(string CerId, string EntityId);

        /// <summary>
        /// 根据登录Id更新用户CA认证信息
        /// </summary>
        /// <param name="LoginId"></param>
        /// <param name="CerId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetCerid(string LoginId, string CerId, string EntityId);

        /// <summary>
        /// 根据登录Id更新用户CA认证信息
        /// </summary>
        /// <param name="LoginId"></param>
        /// <param name="CerId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertCaSign(List<EntityCaSign> CaSign);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse ChangePassword(EntitySysUser sysUser);

        /// <summary>
        /// 获取此仪器权限的用户
        /// </summary>
        /// <param name="itrId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityUserInstrmt> GetUserCanMgrIInstrmt(string itrId);

        /// <summary>
        /// 获取CaSign表数据
        /// </summary>
        /// <param name="CerId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityCaSign> GetCaSign(string CerId,string EntityId);

        /// <summary>
        /// 停用用户账号
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateUserFlag(string loginId);
    }
}
