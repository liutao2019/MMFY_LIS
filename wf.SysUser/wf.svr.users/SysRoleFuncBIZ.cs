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
    public class SysRoleFuncBIZ : ISysRoleFunc
    {
        public bool DeleteFuncByRoleId(string roleId)
        {
            IDaoSysRoleFunc dao = DclDaoFactory.DaoHandler<IDaoSysRoleFunc>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    dao.DeleteFuncByRoleId(roleId);
                    return true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }

        public List<EntitySysRoleFunction> GetFuncsByRoleId(string roleId)
        {
            IDaoSysRoleFunc dao = DclDaoFactory.DaoHandler<IDaoSysRoleFunc>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    return dao.GetFuncsByRoleId(roleId);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public bool InsertRoleFunc(List<EntitySysRoleFunction> userRoles)
        {
            IDaoSysRoleFunc dao = DclDaoFactory.DaoHandler<IDaoSysRoleFunc>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    dao.InsertRoleFunc(userRoles);
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
