using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.servececontract;

namespace dcl.svr.statistical
{
    public class StatTempBIZ: IStatTemp
    {
        public bool InsertTpTemplate(List<EntityTpTemplate> ds)
        {
            bool isSuccess = false;
            try
             {
                 if (ds.Count>0)
                 {
                     string name = ds[0].StName.ToString();
                     string type = ds[0].StType.ToString();
                    isSuccess = DeleteStatTemp(name, type);
                 }
                if (isSuccess)
                {
                    foreach (var item in ds)
                    {
                        isSuccess = InsertStatTemp(item);
                    }
                }
                 return isSuccess;
             }
             catch (Exception ex)
             {
                 CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString());
                return isSuccess;
             }
        }

        public bool DeleteStatTemp(string name, string type)
        {
            bool isSuccess = false;
            IDaoSaveStatTemp dao = DclDaoFactory.DaoHandler<IDaoSaveStatTemp>();
            try
            {
                if (dao != null)
                {
                    isSuccess = dao.DeleteStatTemp(name, type);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString());
                return isSuccess;
            }
        }

        public bool InsertStatTemp(EntityTpTemplate tpTemp)
        {
            bool isSuccess = false;
            IDaoSaveStatTemp dao = DclDaoFactory.DaoHandler<IDaoSaveStatTemp>();
            try
            {
                if (dao != null)
                {
                    isSuccess = dao.InsertStatTemp(tpTemp);
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString());
                return isSuccess;
            }
        }
    }
}
