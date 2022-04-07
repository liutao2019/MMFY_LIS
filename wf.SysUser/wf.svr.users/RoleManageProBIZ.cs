using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.users
{
    public class RoleManageProBIZ : IRoleManagePro
    {
        public bool DeleteRoleInfo(EntitySysRole role)
        {
            bool result = false;
            if (role != null)
            {

                IDaoSysRole dao = DclDaoFactory.DaoHandler<IDaoSysRole>();
                IDaoUserRole daoUR = DclDaoFactory.DaoHandler<IDaoUserRole>();
                IDaoSysRoleFunc daoRF = DclDaoFactory.DaoHandler<IDaoSysRoleFunc>();
                if (dao == null)
                {
                    Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                    return false;
                }
                else
                {
                    try
                    {
                        dao.DeleteRoleInfo(role);
                        daoUR.DeleteUserByRoleId(role.RoleId.ToString());
                        daoRF.DeleteFuncByRoleId(role.RoleId.ToString());
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public EntityResponse GetAllInfo()
        {
            EntityResponse response = new EntityResponse();
            IDaoSysRole dao = DclDaoFactory.DaoHandler<IDaoSysRole>();
            SysUserInfoBIZ userinfoBiz = new SysUserInfoBIZ();
            FuncManageProBIZ funcBiz = new FuncManageProBIZ();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string par = null;
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                dict.Add("roleList", dao.GetAllInfo());
                if (userinfoBiz != null && funcBiz != null)
                {
                    dict.Add("userList", userinfoBiz.GetAllUsers(par));
                    dict.Add("funcList", funcBiz.GetFuncList("1"));
                }
                response.SetResult(dict);
                return response;
            }
        }

        public EntitySysRole GetRoleUserAndFunc(string roleId)
        {
            IDaoUserRole daoUR = DclDaoFactory.DaoHandler<IDaoUserRole>();
            IDaoSysRoleFunc daoRF = DclDaoFactory.DaoHandler<IDaoSysRoleFunc>();
            EntitySysRole role = new EntitySysRole();
            if (daoRF == null || daoUR == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                role.listUser = daoUR.GetUsersByRoleId(roleId);
                role.listFunc = daoRF.GetFuncsByRoleId(roleId);
                return role;
            }
        }

        public bool InsertRoleInfo(EntitySysRole role)
        {
            if (role != null)
            {
                EntitySysRole roleInfo = role;
                List<EntityUserRole> userRoles = role.listUser;
                List<EntitySysRoleFunction> roleFuncs = role.listFunc;
                IDaoSysRole dao = DclDaoFactory.DaoHandler<IDaoSysRole>();
                SysUserRoleBIZ userRoleBiz = new SysUserRoleBIZ();
                SysRoleFuncBIZ roleFuncBiz = new SysRoleFuncBIZ();
                if (dao == null)
                {
                    Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                    return false;
                }
                else
                {
                    try
                    {
                        dao.InsertRoleInfo(role);
                        foreach (var item in userRoles)
                            item.RoleId = role.RoleId.ToString();
                        foreach (var item in roleFuncs)
                            item.RoleId = int.Parse(role.RoleId);
                        userRoleBiz.InsertUserRole(userRoles);
                        roleFuncBiz.InsertRoleFunc(roleFuncs);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                        return false;
                    }

                }
            }
            else
            {
                return false;
            }
        }

        public bool UpdateRoleInfo(EntitySysRole role)
        {
            if (role != null)
            {
                EntitySysRole roleInfo = role;
                List<EntityUserRole> userRoles = role.listUser;
                List<EntitySysRoleFunction> roleFuncs = role.listFunc;
                IDaoSysRole dao = DclDaoFactory.DaoHandler<IDaoSysRole>();
                SysUserRoleBIZ userRoleBiz = new SysUserRoleBIZ();
                SysRoleFuncBIZ roleFuncBiz = new SysRoleFuncBIZ();
                if (dao == null)
                {
                    Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                    return false;
                }
                else
                {
                    try
                    {
                        dao.UpdateRoleInfo(role);
                        userRoleBiz.DeleteUserByRoleId(role.RoleId.ToString());
                        userRoleBiz.InsertUserRole(userRoles);
                        roleFuncBiz.DeleteFuncByRoleId(role.RoleId.ToString());
                        roleFuncBiz.InsertRoleFunc(roleFuncs);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                        return false;
                    }

                }
            }
            else
            {
                return false;
            }
        }

        public List<EntitySysRole> GetPowerRoleUser()
        {
            IDaoSysRole daoUR = DclDaoFactory.DaoHandler<IDaoSysRole>();
            List<EntitySysRole> listRole = new List<EntitySysRole>();
            if (daoUR == null )
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
            }
            else
            {
                listRole = daoUR.GetPowerRoleUser();
            }
            return listRole;
        }
    }
}
