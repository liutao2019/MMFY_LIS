using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.samstock
{
    /// <summary>
    /// 标本销毁中间层
    /// </summary>
    public class SamDestoryBIZ : IDicSamDestory
    {
        public bool CanRollBackDestory(string strSsid, string rackID)
        {
            bool istrue = false;
            IDaoSamDestory dao = DclDaoFactory.DaoHandler<IDaoSamDestory>();
            if(dao!=null)
            {
                istrue = dao.CanRollBackDestory(strSsid, rackID);
            }
            return istrue;
        }

        public int DestoryRackSam(List<string> barcodeList, string strSsid, string rackID, string operatorName, string operatorID, string opPlace, string iecID, string cupID)
        {
            int isSam = -1;
            IDaoSamDestory dao = DclDaoFactory.DaoHandler<IDaoSamDestory>();
            if (dao != null)
            {
                isSam = dao.DestoryRackSam(barcodeList, strSsid, rackID, operatorName, operatorID, opPlace, iecID, cupID);
            }
            return isSam;
        }

        public List<EntitySampStoreRack> GetRackDataForDestory(DateTime? dateTimeFrom, DateTime? dateTimeTo, string rackCtype, string iceID, string cupID, string rackID, string barcode)
        {
            List<EntitySampStoreRack> listSampStoreRack = new List<EntitySampStoreRack>();
            IDaoSamDestory dao = DclDaoFactory.DaoHandler<IDaoSamDestory>();
            if (dao != null)
            {
                listSampStoreRack = dao.GetRackDataForDestory(dateTimeFrom, dateTimeTo, rackCtype, iceID, cupID, rackID, barcode);
            }
            return listSampStoreRack;
        }

        public List<EntitySampStoreDetail> GetRackDetailForDestory(string strSsid)
        {
            List<EntitySampStoreDetail> listSampStoreDetail = new List<EntitySampStoreDetail>();
            IDaoSamDestory dao = DclDaoFactory.DaoHandler<IDaoSamDestory>();
            if (dao != null)
            {
                listSampStoreDetail = dao.GetRackDetailForDestory(strSsid);
            }
            return listSampStoreDetail;
        }

        public int RollBackDestory(List<string> barcodeList, string strSsid, string rackID)
        {
            int isDestory = -1;
            IDaoSamDestory dao = DclDaoFactory.DaoHandler<IDaoSamDestory>();
            if (dao != null)
            {
                isDestory = dao.RollBackDestory(barcodeList, strSsid, rackID);
            }
            return isDestory;
        }
    }
}
