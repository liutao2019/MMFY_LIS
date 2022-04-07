using System;
using System.Collections.Generic;
using System.Data;
using Lib.DAC;
using dcl.common;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.svr.cache;
using dcl.entity;
using System.Configuration;

namespace dcl.svr.users
{
    public class UserManageBIZ : IUserManage
    {
        #region ICommonBIZ 成员

        public EntityPowerList GetEntityList(EntitySysUser UserInfo)
        {
            EntityPowerList List = new EntityPowerList();
            try
            {
                string userInfoID = UserInfo.UserId;

                //用户物理组
                IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
                List<EntityUserLab> userTypeInfo = dao.GetUserTypeInfoByUserId(userInfoID);
                List.UserLab = userTypeInfo;


                //用户仪器
                List<EntityUserInstrmt> userItrInfo = dao.GetUserItrInfoByUserId(userInfoID);
                List.UserItr = userItrInfo;

                //用户医院
                List<EntityUserHospital> userHosInfo = dao.GetUserHosInfoByUserId(userInfoID);
                List.UserHospital = userHosInfo;

                //用户物理组_质控用
                List<EntityUserLabQuality> userLabQInfo = dao.GetUserLabQInfoByUserId(userInfoID);
                List.UserLabQuality = userLabQInfo;

                //用户仪器_质控用
                List<EntityUserItrQuality> userItrQInfo = dao.GetUserItrQInfoByUserId(userInfoID);
                List.UserItrQuality = userItrQInfo;

                //用户医院_质控用
                List<EntityUserHosQuality> userHosQInfo = dao.GetUserHosQInfoByUserId(userInfoID);
                List.UserHosQuality = userHosQInfo;

                //用户所属角色
                List<EntityUserRole> userRoleInfo = dao.GetUserRoleInfoByUserId(userInfoID);
                List.UserRole = userRoleInfo;

                //用户所属科室
                List<EntityUserDept> userDeptInfo = dao.GetUserDeptInfoByUserId(userInfoID);
                List.UserDept = userDeptInfo;

                return List;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet doOther(DataSet ds)
        {
            DataSet result = new DataSet();
            DataTable table = new DataTable();
            table.Columns.Add("message");
            DataRow row = table.NewRow();

            DataTable dtUserInfo = ds.Tables["PowerUserInfo"];

            string loginId = dtUserInfo.Rows[0]["loginId"].ToString();
            string oldPassword = dtUserInfo.Rows[0]["old_password"].ToString();
            string newPassword = dtUserInfo.Rows[0]["password"].ToString();

            SqlHelper helper = new SqlHelper();

            string selectUser = "select password from poweruserinfo where loginId = ?";
            DbCommandEx cmdSelect = helper.CreateCommandEx(selectUser);
            cmdSelect.AddParameterValue(loginId, DbType.AnsiString);
            object objOldPassword = helper.ExecuteScalar(cmdSelect);

            if (objOldPassword == null)//找不到用户
            {
                row["message"] = "找不到用户" + loginId;
            }
            else
            {
                if (objOldPassword.ToString() != oldPassword)
                {
                    row["message"] = "原始密码错误！";
                }
                else
                {
                    string sqlUpdate = "update poweruserinfo set password = ? where loginId = ?";
                    DbCommandEx cmdUpdate = helper.CreateCommandEx(sqlUpdate);
                    cmdUpdate.AddParameterValue(newPassword, DbType.AnsiString);
                    cmdUpdate.AddParameterValue(loginId, DbType.AnsiString);
                    helper.ExecuteNonQuery(cmdUpdate);
                }
            }
            table.Rows.Add(row);
            result.Tables.Add(table);
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        public EntityResponse ChangePassword(EntitySysUser sysUser)
        {
            EntityResponse response = new EntityResponse();
            string loginId = sysUser.LoginId;
            string oldPassword = sysUser.OldPassword;
            string newPassword = sysUser.UserPassword;
            List<EntitySysUser> listUser = new List<EntitySysUser>();
            IDaoSysUser daoUser = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (daoUser != null)
            {
                listUser = daoUser.GetUserInfoByLoginId(loginId, "");
            }

            if (listUser == null || listUser.Count == 0)//找不到用户
            {
                response.Scusess = false;
                response.ErroMsg = "找不到用户" + loginId;
            }
            else
            {
                if (listUser[0].UserPassword != oldPassword)
                {
                    response.Scusess = false;
                    response.ErroMsg = "原密码错误";
                }
                else
                {
                    IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
                    response.Scusess = dao.UpdateUserPassword(loginId, newPassword);
                }
            }
            return response;
        }

        public EntityPowerList GetViewData(bool isPower)
        {
            EntityPowerList List = new EntityPowerList();
            try
            {
                IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
                List<EntitySysUser> userList = dao.GetUserInfo(isPower);

                //物理组和仪器
                List<EntityDicPubOrganize> typeList = dao.GetTypeInfo();

                string hosID = ConfigurationManager.AppSettings["HospitalID"];

                if (!string.IsNullOrEmpty(hosID))
                {
                    userList = userList.FindAll(w => w.UserOrgId == hosID);
                    typeList = typeList.FindAll(w => w.OrgCode == hosID);
                }

                List.SysUser = userList;

                //角色列表  
                List<EntitySysRole> roleList = dao.GetRoleInfo(isPower);
                List.SysRole = roleList;


                List.DicType = typeList;

                //科室
                List<EntityDicPubDept> deptList = DictDepartCache.Current.GetAll();
                List.DicDept = deptList;
                return List;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdateUserInfo(EntityPowerList obj)
        {
            DataSet result = new DataSet();
            try
            {
                IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
                List<EntitySysUser> dtUserInfo = obj.SysUser;
                List<EntityUserRole> dtUserRole = obj.UserRole;
                List<EntityUserLab> dtUserType = obj.UserLab;
                List<EntityUserInstrmt> dtUserInstrmt = obj.UserItr;
                List<EntityUserHospital> dtHospital = obj.UserHospital;
                List<EntityUserLabQuality> dtUserTypeQuality = obj.UserLabQuality;
                List<EntityUserItrQuality> dtUserInstrmtQuality = obj.UserItrQuality;
                List<EntityUserHosQuality> dtHospitalQuality = obj.UserHosQuality;
                List<EntityUserDept> dtUserDepart = obj.UserDept;
                String userInfoID = dtUserInfo[0].UserId.ToString();
                List<EntitySysUser> dtUserInfoTemp = dtUserInfo;
                //更新用户表信息
                dao.UpdateUserList(dtUserInfoTemp);
                //删除用户相关信息
                dao.DeleteUserRelate(userInfoID);
                //插入用户相关信息
                dao.InsertUserList(obj);

                UpdateSignImage(dtUserInfo);

                DictSysUserCache.Current.Refresh();
                dcl.svr.cache.CacheUserInfo.Current.Refresh();
                dcl.svr.cache.CacheUserRole.Current.Refresh();
                dcl.svr.cache.CacheUserInstrmtInfo.Current.Refresh();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 停用用户账号
        /// </summary>
        /// <param name="user"></param>
        public bool UpdateUserFlag(string loginId)
        {
            if(string.IsNullOrEmpty(loginId))
            {
                return false;
            }
            try
            {
                IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
                dao.UpdateUserFlag(loginId);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// 更新用户签名图片
        /// </summary>
        /// <param name="dtUserInfo"></param>
        private void UpdateSignImage(List<EntitySysUser> dtUserInfo) // 2010-8-27
        {
            if (dtUserInfo == null || dtUserInfo.Count == 0 || dtUserInfo[0].UserSigninamge == null)
                return;
            try
            {

                IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
                dao.UpdateUserSign(dtUserInfo[0].UserSigninamge, dtUserInfo[0].UserId);
            }
            catch
            {
            }
        }

        public bool AddUserInfo(EntityPowerList obj)
        {
            try
            {
                List<EntitySysUser> dtUserInfo = obj.SysUser;
                List<EntityUserRole> dtUserRole = obj.UserRole;
                List<EntityUserLab> dtUserType = obj.UserLab;
                List<EntityUserInstrmt> dtUserInstrmt = obj.UserItr;
                List<EntityUserHospital> dtHospital = obj.UserHospital;
                List<EntityUserLabQuality> dtUserTypeQuality = obj.UserLabQuality;
                List<EntityUserItrQuality> dtUserInstrmtQuality = obj.UserItrQuality;
                List<EntityUserHosQuality> dtHospitalQuality = obj.UserHosQuality;
                List<EntityUserDept> dtUserDepart = obj.UserDept;
                IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();

                string hosID = ConfigurationManager.AppSettings["HospitalID"];
                foreach (EntitySysUser dr in dtUserInfo)
                {
                    dr.UserOrgId = hosID;
                }

                string userInfoID = dao.AddUserList(dtUserInfo);

                if (!string.IsNullOrEmpty(userInfoID))
                {
                    foreach (EntitySysUser dr in dtUserInfo)
                    {
                        dr.UserId = userInfoID;
                        dr.UserOrgId = hosID;
                    }
                    foreach (EntityUserRole dr in dtUserRole)
                    {
                        dr.UserId = userInfoID;
                    }
                    foreach (EntityUserLab dr in dtUserType)
                    {
                        dr.UserId = userInfoID;
                    }
                    foreach (EntityUserInstrmt dr in dtUserInstrmt)
                    {
                        dr.UserId = userInfoID;
                    }
                    foreach (EntityUserHospital dr in dtHospital)
                    {
                        dr.UserId = userInfoID;
                    }
                    foreach (EntityUserDept dr in dtUserDepart)
                    {
                        dr.UserId = userInfoID;
                    }
                    if (dtUserTypeQuality != null)
                    {
                        foreach (EntityUserLabQuality dr in dtUserTypeQuality)
                        {
                            dr.UserId = userInfoID;
                        }
                        foreach (EntityUserItrQuality dr in dtUserInstrmtQuality)
                        {
                            dr.UserId = userInfoID;
                        }
                        foreach (EntityUserHosQuality dr in dtHospitalQuality)
                        {
                            dr.UserId = userInfoID;
                        }
                    }
                    dao.InsertUserList(obj);
                    DictSysUserCache.Current.Refresh();
                    dcl.svr.cache.CacheUserInfo.Current.Refresh();
                    dcl.svr.cache.CacheUserRole.Current.Refresh();
                    dcl.svr.cache.CacheUserInstrmtInfo.Current.Refresh();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteUserInfo(EntitySysUser ds)
        {
            try
            {
                string userInfoID = ds.UserId.ToString();
                IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
                int[] num = dao.DeleteUserInfo(userInfoID);

                dcl.svr.cache.CacheUserInfo.Current.Refresh();
                dcl.svr.cache.CacheUserRole.Current.Refresh();
                dcl.svr.cache.CacheUserInstrmtInfo.Current.Refresh();

                if (num.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        public List<EntityUserInstrmt> GetUserCanMgrIInstrmt(string itrID)
        {
            return CacheUserInstrmtInfo.Current.GetDclUserCanMgrIInstrmt(itrID);
        }

        public bool DelCerid(string CerId, string EntityId)
        {
            IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
            bool result = dao.DelCerid(CerId, EntityId);
            return result;
        }

        public bool SetCerid(string LoginId, string CerId, string EntityId)
        {
            IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
            bool result = dao.SetCerid(LoginId, CerId, EntityId);
            return result;
        }

        public bool InsertCaSign(List<EntityCaSign> CaSign)
        {
            UserCaSignBIZ CaSignBIZ = new UserCaSignBIZ();
            bool result = CaSignBIZ.InsertCaSign(CaSign);
            return result;
        }
        public List<EntityCaSign> GetCaSign(string CerId, string EntityId)
        {
            UserCaSignBIZ CaSignBIZ = new UserCaSignBIZ();
            List<EntityCaSign> result = CaSignBIZ.GetCaSign(CerId, EntityId);
            return result;
        }
    }
}
