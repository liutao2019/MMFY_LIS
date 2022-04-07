using System;
using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
namespace dcl.svr.users
{
    class SysLogBIZ: ISysLog
    {
    
        public bool NewSysLog(EntityLogLogin logLogin)
        {
            IDaoSysLog dao = DclDaoFactory.DaoHandler<IDaoSysLog>();
            if (dao == null)
            {
                return false;
            }
            else
            {
                return dao.SaveSysLog(logLogin);
            }
        }

        public bool DeleteSysLog(string timeFrom,string timeTo)
        {
            IDaoSysLog dao = DclDaoFactory.DaoHandler<IDaoSysLog>();
            if (dao == null)
            {
                return false;
            }
            else
            {
                return dao.DeleteSysLog(timeFrom, timeTo);
            }
        }

        public bool UpdateReporSysLog(EntityRequest request)
        {
            throw new NotImplementedException();
        }

        public List<EntityLogLogin> GetSysLog(string loginId,string module,string timeFrom,string timeTo)
        {
            List<EntityLogLogin> list = new List<EntityLogLogin>();
            IDaoSysLog dao = DclDaoFactory.DaoHandler<IDaoSysLog>();
            if (dao != null)
            {
                list = dao.GetSysLog(loginId, module, timeFrom, timeTo);
            }
            return list;
        }

        public List<EntitySysFunction> GetFuncName()
        {
            List<EntitySysFunction> list = new List<EntitySysFunction>();
            FuncManageProBIZ func = new FuncManageProBIZ();
            list = func.GetFuncName();
            return list;
        }

        public List<EntitySysUser> GetLoginId()
        {
            List<EntitySysUser> list = new List<EntitySysUser>();
            SysUserInfoBIZ user = new SysUserInfoBIZ();
            list=user.GetLoginId();
            return list;
        }
    }
}
