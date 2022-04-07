using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Data;

namespace dcl.svr.resultquery
{
    public class ObrResultBIZ : IObrResult
    {
        public List<EntityObrResult> GetPatResultByIdAndWhere(string obrId, bool filterFlag)
        {
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    List<EntityObrResult> listResult = dao.GetPatResultByIdAndWhere(obrId, filterFlag);
                    return listResult;
                }
                catch(Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public List<EntityObrResult> GetPatResultByObrId(List<EntityObrResult> dtObrResult)
        {
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    List<EntityObrResult> listResult = dao.GetPatResultByObrId(dtObrResult);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public List<EntityObrResult> GetPatResultTableById(string obrId)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                listResult = resultDao.GetPatResultTableById(obrId);
            }
            return listResult;
        }

        public bool InsertObrResult(EntityObrResult ObrResult)
        {
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    bool listResult = dao.InsertObrResult(ObrResult);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }

        public bool UpdateObrResultByObrIdAndObrItmId(EntityObrResult ObrResult)
        {
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    bool listResult = dao.UpdateObrResultByObrIdAndObrItmId(ObrResult);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }

        public List<EntityObrResult> GetPatResultByCondition(EntityPidReportMain patient, EntityAnanlyseQC ananlyseQc)
        {
            List<EntityObrResult> resultList = new List<EntityObrResult>();
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if(resultDao != null)
            {
                resultList = resultDao.GetPatResultByCondition(patient, ananlyseQc);
            }
            return resultList;
        }
    }
}
