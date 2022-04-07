using System;
using Lib.LogManager;
using dcl.common;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.samstock
{
    public class SampStoreRackBIZ : dcl.servececontract.ISampStoreRack
    {
        public int ModifySamStoreRack(EntitySampStoreRack entity)
        {
            int intRet = -1;
            try
            {
                IDaoSampStoreRack dao = DclDaoFactory.DaoHandler<IDaoSampStoreRack>();

                bool isSuccess = dao.ModifySamStoreRack(entity);
                if (isSuccess)
                {
                    intRet = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 ModifySamDetail", ex);
            }
            return intRet;
        }

        public int UpdateSrAmountById(string SrId, string qc)
        {
            int intRet = -1;
            try
            {
                IDaoSampStoreRack dao = DclDaoFactory.DaoHandler<IDaoSampStoreRack>();

                intRet = dao.UpdateSrAmountById(SrId,qc);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 UpdateSrAmountById", ex);
            }
            return intRet;
        }
    }
}
