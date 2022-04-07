using System;
using System.Collections.Generic;
using Lib.LogManager;
using dcl.svr.cache;
using dcl.common;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.svr.sample;

namespace dcl.svr.samstock
{
    public class SampStoreRecordBIZ : dcl.servececontract.ISampStoreRecord
    {
        #region 试管架
        /// <summary>
        /// 试管架
        /// </summary>
        /// <returns></returns>
        public List<EntityDicSampTubeRack> GetDictRackList(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
            List<EntityDicSampTubeRack> list = dao.GetDictRackList(dateTimeFrom, dateTimeTo);
            return list;
        }
        #endregion

        #region 试管规格类型

        /// <summary>
        /// 试管规格类型
        /// </summary>
        /// <returns></returns>
        public List<EntityDicTubeRack> GetCuvShelf()
        {
            List<EntityDicTubeRack> List = new List<EntityDicTubeRack>();
            try
            {
                IDaoDic<EntityDicTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicTubeRack>>();
                List = dao.Search(null);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetSpeciaty", ex);
            }

            return List;
        }

        #endregion

        #region 标本状态
        public List<EntityDicSampStoreStatus> GetSamManageStatus()
        {
            SampStockBIZ StockBIZ = new SampStockBIZ();
            List<EntityDicSampStoreStatus> list = StockBIZ.GetSamManageStatus();
            return list;
        }

        #endregion

        public int GetSamRackStatus(string strSsid)
        {
            try
            {
                SampStockBIZ StockBIZ = new SampStockBIZ();
                int strStatus = StockBIZ.GetSamRackStatus(strSsid);
                return strStatus;
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetSamRackStatus", ex);
            }
            return -1;
        }

        /// <summary>
        /// 通过扫描的条码号获得病人的信息
        /// </summary>
        /// <param name="barcode">扫描得到的条码号</param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatientsInfo(string barcode)
        {

            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
            List<EntityPidReportMain> list = dao.GetPatientsInfo(barcode);
            return list;
        }

        /// <summary>
        /// 查询架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        public List<EntitySampStoreDetail> GetRackDetail(string strSsid)
        {
            List<EntitySampStoreDetail> returnDT = null;

            try
            {
                SampStoreDetailBIZ StoreDetailBIZ = new SampStoreDetailBIZ();
                returnDT = StoreDetailBIZ.GetRackDetail(strSsid);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetRackDetail", ex);
            }
            return returnDT;

        }

        public bool IsBarCodeUsing(string barCode)
        {
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                bool IsExists = dao.IsBarCodeUsing(barCode);
                return IsExists;
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 IsBarCodeUsing", ex);
            }
            return false;
        }

        public string[] GetAppendBarCode(string barcode, string patID)
        {
            List<string> list = new List<string>();
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                List<EntityPidReportDetail> ReportDetail = dao.GetAppendBarCode(barcode, patID);
                foreach (EntityPidReportDetail row in ReportDetail)
                {
                    if (row.RepBarCode != null && row.RepBarCode != "" &&
                        !string.IsNullOrEmpty(row.RepBarCode.ToString()))
                    {
                        list.Add(row.RepBarCode.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetAppendBarCode ", ex);
            }

            return list.ToArray();
        }

        public int InsertBcSign(string operatorName, string operatorID, string barCode, string bcStatus, string remark, string palce)
        {
            int intRet = -1;
            SampMainBIZ SampMainbiz = new SampMainBIZ();
            EntitySampMain SampMain = new EntitySampMain();
            if (!string.IsNullOrEmpty(barCode))
            {
                SampMain = SampMainbiz.SampMainQueryByBarId(barCode);
            }

            EntitySampOperation SampOperation = new EntitySampOperation();
            SampOperation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
            SampOperation.OperationID = operatorID;
            SampOperation.OperationName = operatorName;
            SampOperation.OperationStatus = bcStatus;
            SampOperation.OperationPlace = palce;
            SampOperation.Remark = remark;
            try
            {
                SampProcessDetailBIZ SampProcessDetail = new SampProcessDetailBIZ();
                bool IsExists = SampProcessDetail.SaveSampProcessDetail(SampOperation, SampMain);
                if (IsExists)
                {
                    intRet = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 InsertBcSign", ex);
            }

            return intRet;
        }

        public int ModifySamStoreRack(EntitySampStoreRack entity)
        {
            int intRet = -1;
            try
            {
                SampStoreRackBIZ StoreRackBIZ = new SampStoreRackBIZ();
               intRet = StoreRackBIZ.ModifySamStoreRack(entity);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 ModifySamDetail", ex);
            }
            return intRet;
        }

        public int ModifyRackStatus(EntityDicSampTubeRack entity)
        {
            int intRet = -1;

            try
            {
                SampStockBIZ StockBIZ = new SampStockBIZ();
                intRet = StockBIZ.ModifyRackStatus(entity);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 ModifyRackStatus", ex);
            }

            return intRet;
        }

        public string BatchHandData(List<EntitySampStoreDetail> dt, string strSsid, string strRackID, string cuvcode, string operatorName, string operatorID, string opPlace, int rowhander, int colHander, string rack_Barcode)
        {
            try
            {
                SampStoreDetailBIZ StoreDetailBIZ = new SampStoreDetailBIZ();
                string message = string.Empty;
                message = StoreDetailBIZ.BatchHandData(dt, strSsid, strRackID, cuvcode, operatorName, operatorID, opPlace, rowhander, colHander, rack_Barcode);
                return message;
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 BatchHandData", ex);
                return ex.Message;
            }
        }

        public List<EntitySampStoreDetail> GetBatchHandData(DateTime date, string BatchItr, string SamForm, string SamTo, int selectIndex)
        {

            try
            {
                SampStoreDetailBIZ StoreDetailBIZ = new SampStoreDetailBIZ();
                List<EntitySampStoreDetail> returnDT = StoreDetailBIZ.GetBatchHandData(date, BatchItr, SamForm, SamTo, selectIndex);
                return returnDT;
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetBatchHandData", ex);
                return null;
            }
        }

        #region 将数据归档到表SamStore_RackDetail
        /// <summary>
        /// 将数据归档到表SamStore_RackDetail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertRackDetail(EntitySampStoreDetail entity)
        {
            int intRet = 0;
            try
            {
                SampStoreDetailBIZ StoreDetailBIZ = new SampStoreDetailBIZ();
                intRet = StoreDetailBIZ.InsertRackDetail(entity);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 InsertRackDetail", ex);
            }

            return intRet;
        }
        #endregion

        #region 删除样本的状态

        public int DeleteSamDetail(string strSsid, string rackID, List<string> barcodeList)
        {
            int intRet = -1;
            try
            {
                SampStoreDetailBIZ StoreDetailBIZ = new SampStoreDetailBIZ();
                SampStoreRackBIZ StoreRackBIZ = new SampStoreRackBIZ();
                foreach (string barcode in barcodeList)
                {
                    intRet = StoreDetailBIZ.DeleteSampStoreDetail(strSsid, barcode);

                    if (intRet == 1)
                    {
                        if (intRet == 1)
                        {
                            DeleteBcSign(barcode, "110");
                        }

                        intRet = StoreRackBIZ.UpdateSrAmountById(strSsid,"SrAmount");
                    }
                }
                intRet = StoreDetailBIZ.GetSampStoreDetailCount(strSsid);
                if (intRet == 0)
                {
                    intRet = StoreRackBIZ.UpdateSrAmountById(strSsid, "SrStatus");
                    EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                    entityRack.RackId = rackID;
                    entityRack.RackStatus = 0;
                    ModifyRackStatus(entityRack);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 DeleteSamDetail", ex);
            }
            return intRet;
        }

        public int DeleteBcSign(string barCode, string bcStatus)
        {
            int intRet = -1;
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                bool IsExists = dao.DeleteSampProcessDetail(barCode, bcStatus);
                if (IsExists)
                {
                    intRet = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 DeleteBcSign", ex);
            }

            return intRet;
        }
        #endregion

    }
}
