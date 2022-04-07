using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.users
{
    public class FuncManageProBIZ:ISysFunction
    {

        public bool DeleteFunc(EntitySysFunction func)
        {
            if (func != null)
            {

                IDaoSysFunction dao = DclDaoFactory.DaoHandler<IDaoSysFunction>();
                if (dao == null)
                {
                    Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                    return false;
                }
                else
                {
                    try
                    {
                        dao.DeleteFunc(func);
                        return true;
                    }
                    catch(Exception ex)
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

        public List<EntitySysFunction> GetFuncList(string whereSql = "")
        {
            IDaoSysFunction dao = DclDaoFactory.DaoHandler<IDaoSysFunction>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    return dao.GetFuncList(whereSql);
                }
                catch(Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
                
            }
        }

        public bool InsertAFunc(EntitySysFunction func)
        {
            if (func != null)
            {
                IDaoSysFunction dao = DclDaoFactory.DaoHandler<IDaoSysFunction>();
                if (dao == null)
                {
                    Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                    return false;
                }
                else
                {
                    try
                    {
                        dao.InsertAFunc(func);
                        return true;
                    }
                    catch(Exception ex)
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

        public bool UpdateAFunc(EntitySysFunction func)
        {
            if (func  != null)
            {
                IDaoSysFunction dao = DclDaoFactory.DaoHandler<IDaoSysFunction>();
                if (dao == null)
                {
                    Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                    return false;
                }
                else
                {
                    try
                    {
                        dao.UpdateAFunc(func);
                        return true;
                    }
                    catch(Exception ex)
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

        public List<EntitySysFunction> GetFuncName()
        {
            IDaoSysFunction dao = DclDaoFactory.DaoHandler<IDaoSysFunction>();
            List<EntitySysFunction> list = dao.GetFuncName();
            return list;
        }

    }
}
