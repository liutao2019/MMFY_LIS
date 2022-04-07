using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using System;
using System.Data;
using System.Collections.Generic;

namespace dcl.svr.tools
{
    public class LisDocManagerBIZ : ILisDoc
    {
        public int Delete(EntityLisDoc lisDoc)
        {
            IDaoLisDoc dao = DclDaoFactory.DaoHandler<IDaoLisDoc>();
            return dao.Delete(lisDoc);
        }

        public List<EntityLisDoc> QueryAll()
        {
            try
            {
                IDaoLisDoc dao = DclDaoFactory.DaoHandler<IDaoLisDoc>();
                List<EntityLisDoc> listDoc = dao.QueryAll();
                return listDoc;
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错！", ex.ToString());
                return null;
            }
        }

        public List<EntityLisDoc> Query(DateTime beginTime, DateTime endTime, string docType)
        {
            try
            {
                IDaoLisDoc dao = DclDaoFactory.DaoHandler<IDaoLisDoc>();
                List<EntityLisDoc> listDoc = dao.Query(beginTime, endTime, docType);
                return listDoc;
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错！", ex.ToString());
                return null;
            }
        }

        public int Save(EntityLisDoc lisDoc)
        {
            try
            {
                IDaoLisDoc dao = DclDaoFactory.DaoHandler<IDaoLisDoc>();

                return dao.Save(lisDoc);
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错！", ex.ToString());
                return -1;
            }
        }
    }
}

