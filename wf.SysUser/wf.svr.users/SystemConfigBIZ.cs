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
    public class SystemConfigBIZ : ISystemConfig
    {
        public List<EntitySysParameter> GetSysParaCaChe()
        {
            IDaoSysParameter dao = DclDaoFactory.DaoHandler<IDaoSysParameter>();
            List<EntitySysParameter> result = new List<EntitySysParameter>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("没有找到所需的Dao");
            }
            else
            {
                result = (dao.GetSysParaCache());
            }
            return result;
        }

        public List<EntitySysParameter> GetSysParaListByConfigCode(string configCode)
        {
            IDaoSysParameter dao = DclDaoFactory.DaoHandler<IDaoSysParameter>();
            List<EntitySysParameter> result = new List<EntitySysParameter>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("没有找到所需的Dao");
            }
            else
            {
                result = (dao.GetSysParaByConfigCode(configCode));
            }
            return result;
        }

        public List<EntitySysParameter> GetSysParaListByConfigType(string configType = "system")
        {
            IDaoSysParameter dao = DclDaoFactory.DaoHandler<IDaoSysParameter>();
            List<EntitySysParameter> result = new List<EntitySysParameter>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("没有找到所需的Dao");
            }
            else
            {
                result = (dao.GetSysParaByConfigType(configType));
            }
            return result;
        }

        public bool InsertSysPara(EntitySysParameter para)
        {
            IDaoSysParameter dao = DclDaoFactory.DaoHandler<IDaoSysParameter>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("没有找到所需的Dao");
            }
            else
            {
                try
                {
                    dao.InsertSysPara(para);
                    return true;
                }
                catch(Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return false;
        }

        public bool UpdateSysPara(List<EntitySysParameter> listPara)
        {
            List<EntitySysParameter> parms = listPara;
            IDaoSysParameter dao = DclDaoFactory.DaoHandler<IDaoSysParameter>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("没有找到所需的Dao");
            }
            else
            {
                try
                {
                    foreach (EntitySysParameter item in parms)
                    {
                        dao.UpdateSysParaByConfigId(item);
                    }

                    dcl.svr.cache.CacheSysConfig.Current.Refresh();//刷新系统配置表服务器缓存 SJC 2017-12-21

                    return true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

            }
            return false;
        }
    }
}
