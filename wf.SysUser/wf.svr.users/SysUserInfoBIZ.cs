using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Collections;

namespace dcl.svr.users
{
    public class SysUserInfoBIZ : ISysUserInfo
    {

        public List<EntitySysUser> GetAllUsers(string par)
        {
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    return dao.Search(par);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public List<EntitySysUser> GetLoginId()
        {
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    return dao.GetLoginId();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public List<EntitySysUser> GetPowerUserInfo()
        {
            List<EntitySysUser> list = new List<EntitySysUser>();
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    list = dao.GetPowerUserInfo();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return list;
        }


        public List<EntitySysUser> SysUserQuery(EntityUserQc userQc)
        {
            List<EntitySysUser> listSysUser = new List<EntitySysUser>();
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            {
                listSysUser = dao.SysUserQuery(userQc);
            }
            return listSysUser;
        }


        #region IPowerUser

        public int AddUserinfoKey(string loginId, string keyCode, byte[] certinfo, string password)
        {
            int reuslt = 0;
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            {
                reuslt = dao.AddUserinfoKey(loginId, keyCode, certinfo, password);
            }
            return reuslt;
        }

        public ArrayList FindDepartments(string userId)
        {
            ArrayList result = new ArrayList();
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            {
                result = dao.FindDepartments(userId);
            }
            return result;
        }

        public ArrayList FindDepartments_Code(string userId)
        {
            ArrayList result = new ArrayList();
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            {
                result = dao.FindDepartments_Code(userId);
            }
            return result;
        }

        public ArrayList FindUser(string departmentId)
        {
            ArrayList result = new ArrayList();
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            {
                result = dao.FindUser(departmentId);
            }
            return result;
        }

        public List<string> GetUserIDForRoleName(string RoleName)
        {
            List<string> list = new List<string>();
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            {
                list = dao.GetUserIDForRoleName(RoleName);
            }
            return list;
        }

        public string Getuserpwinfo(string loginId)
        {
            string result = string.Empty;
            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao != null)
            {
                result = dao.Getuserpwinfo(loginId);
            }
            return result;
        }

        #endregion

    }
}
