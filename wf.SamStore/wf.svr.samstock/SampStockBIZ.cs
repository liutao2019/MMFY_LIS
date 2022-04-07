using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.samstock
{
    /// <summary>
    /// 标本存储的方法定义
    /// </summary>
    public class SampStockBIZ : dcl.servececontract.ISampStock
    {
        public int AuditSamStoreRack(EntitySampStoreRack entity)
        {
            int k = -1;
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                k = dao.AuditSamStoreRack(entity);
            }
            return k;
            throw new NotImplementedException();
        }

        public int DeleteBcSign(string barCode, string bcStatus)
        {
            int delRet = -1;
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                delRet = dao.DeleteBcSign(barCode, bcStatus);
            }
            return delRet;
            throw new NotImplementedException();
        }
        public List<EntityDicSampStoreArea> GetCups()
        {
            List<EntityDicSampStoreArea> list = new List<EntityDicSampStoreArea>();
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                list = dao.GetCups();
            }
            return list;
        }
        public List<EntityDicSampTubeRack> GetDictRackInfo(string rackbarcode)
        {
            List<EntityDicSampTubeRack> list = new List<EntityDicSampTubeRack>();
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                list = dao.GetDictRackInfo(rackbarcode);
            }
            return list;
        }



        /// <summary>
        /// 试管架
        /// </summary>
        /// <returns></returns>
        public List<EntityDicSampTubeRack> GetDictRackListForSave(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            List<EntityDicSampTubeRack> list = new List<EntityDicSampTubeRack>();
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                list = dao.GetDictRackListForSave(dateTimeFrom, dateTimeTo);
            }
            return list;
        }

        public List<EntityDicSampStore> GetIceBox()
        {
            List<EntityDicSampStore> list = new List<EntityDicSampStore>();
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                list = dao.GetIceBox();
            }
            return list;
        }

        public List<EntitySampStoreDetail> GetRackDetail(string strSsid)
        {
            List<EntitySampStoreDetail> list = new List<EntitySampStoreDetail>();
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                list = dao.GetRackDetail(strSsid);
            }
            return list;
        }

        public List<EntityDicSampStoreStatus> GetSamManageStatus()
        {
            List<EntityDicSampStoreStatus> list = new List<EntityDicSampStoreStatus>();
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                list = dao.GetSamManageStatus();
            }
            return list;
        }

        public int GetSamRackStatus(string strSsid)
        {
            int k = -1;
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                k = dao.GetSamRackStatus(strSsid);
            }
            return k;
        }
        //public int InsertBcSign(string operatorName, string operatorID, string barCode, string bcStatus, string remark, string opPlace)
        //{
        //    int k = -1;
        //    IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
        //    if (dao != null)
        //    {
        //        k = dao.InsertBcSign(operatorName, operatorID, barCode, bcStatus, remark, opPlace);

        //    }
        //    return k;
        //}

        public int ModifyRackStatus(EntityDicSampTubeRack entity)
        {
            int k = -1;
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                k = dao.ModifyRackStatus(entity);
            }
            return k;
        }

        public int ModifySamDetail(EntitySampStoreDetail entity)
        {
            int k = -1;
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            if (dao != null)
            {
                k = dao.ModifySamDetail(entity);
            }
            return k;
        }
    }
}
