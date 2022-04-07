using dcl.servececontract;
using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.dao.core;

namespace dcl.svr.result
{
    public class ObrResultBactBIZ : DclBizBase, IObrResultBact
    {
        public bool DeleteResultById(string obrId)
        {
            bool result = false;
            IDaoObrResultBact dao = DclDaoFactory.DaoHandler<IDaoObrResultBact>();
            if(dao != null)
            {
                dao.Dbm = this.Dbm;
                result = dao.DeleteResultById(obrId);
            }
            return result;
        }

        public List<EntityObrResultBact> GetBactResultById(string obrId = "")
        {
            IDaoObrResultBact dao = DclDaoFactory.DaoHandler<IDaoObrResultBact>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    List<EntityObrResultBact> listResult = dao.GetBactResultById(obrId);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public bool SaveResultBact(List<EntityObrResultBact> listBact)
        {
            bool result = false;
            if (listBact == null || listBact.Count == 0) return result;
            IDaoObrResultBact dao = DclDaoFactory.DaoHandler<IDaoObrResultBact>();
            if (dao != null)
            {
                dao.Dbm = this.Dbm;
                //插入药敏结果前先删除
                if (DeleteResultById(listBact[0].ObrId))
                {
                    foreach (EntityObrResultBact anti in listBact)
                    {
                        result = dao.SaveResultBact(anti);
                    }
                }
            }
            return result;
        }
    }
}
