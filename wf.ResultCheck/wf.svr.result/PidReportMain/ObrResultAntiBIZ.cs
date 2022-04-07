using dcl.servececontract;
using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using dcl.dao.core;

namespace dcl.svr.result
{
    public class ObrResultAntiBIZ : DclBizBase, IObrResultAnti
    {
        public bool DeleteResultById(string obrId)
        {
            bool result = false;
            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            if(dao != null)
            {
                dao.Dbm = this.Dbm;
                result = dao.DeleteResultById(obrId);
            }
            return result;
        }

        public List<EntityObrResultAnti> GetAntiResultById(string obrId)
        {
            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    List<EntityObrResultAnti> listResult = dao.GetAntiResultById(obrId);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 保存药敏结果
        /// </summary>
        /// <param name="listAnti"></param>
        /// <returns></returns>
        public bool SaveAntiResult(List<EntityObrResultAnti> listAnti)
        {
            bool result = false;
            if (listAnti == null || listAnti.Count == 0) return result;
            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            if (dao != null)
            {
                dao.Dbm = this.Dbm;
                //插入药敏结果前先删除
                if (DeleteResultById(listAnti[0].ObrId))
                {
                    foreach (EntityObrResultAnti anti in listAnti)
                    {
                        result = dao.SaveResultAnti(anti);
                    }
                }
            }
            return result;
        }

        public List<EntityObrResultAnti> GetAntiResultByListObrId(List<string> listObrId)
        {
            List<EntityObrResultAnti> listResult = new List<EntityObrResultAnti>();
            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
            }
            else
            {
                try
                {
                    listResult = dao.GetAntiResultByListObrId(listObrId);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listResult;
        }

        public List<EntityObrResultAnti> GetAntiWithHistoryResultById(string obrId)
        {
            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    List<EntityObrResultAnti> listResult = dao.GetAntiWithHistoryResultById(obrId);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }
    }
}
