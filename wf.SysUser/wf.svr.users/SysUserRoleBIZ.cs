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
    public class SysUserRoleBIZ : ISysUserRole
    {
        public bool DeleteUserByRoleId(string roleId)
        {
            IDaoUserRole dao = DclDaoFactory.DaoHandler<IDaoUserRole>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    dao.DeleteUserByRoleId(roleId);
                    return true;
                }
                catch(Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }

        public List<EntityUserRole> GetAllUserRole()
        {
            List<EntityUserRole> listUserRole = new List<EntityUserRole>();
            IDaoUserRole dao = DclDaoFactory.DaoHandler<IDaoUserRole>();
            if (dao != null)
            {
                listUserRole = dao.GetAllUserRole();
            }
            return listUserRole;
        }

        public List<EntityUserRole> GetUsersByRoleId(string roleId)
        {
            IDaoUserRole dao = DclDaoFactory.DaoHandler<IDaoUserRole>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    return dao.GetUsersByRoleId(roleId);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public bool InsertUserRole(List<EntityUserRole> userRoles)
        {
            IDaoUserRole dao = DclDaoFactory.DaoHandler<IDaoUserRole>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    dao.InsertUserRole(userRoles);
                    return true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }
    }
}
