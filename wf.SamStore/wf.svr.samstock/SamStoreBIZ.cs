using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Lib.LogManager;
using dcl.svr.cache;
using dcl.common;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.servececontract;

namespace dcl.svr.samstock
{
    public class SamStoreBIZ : dcl.servececontract.ISamManage
    {

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

        #region 试管架
        /// <summary>
        /// 试管架
        /// </summary>
        /// <returns></returns>
        public List<EntityDicSampTubeRack> GetDictRackList(DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
            List<EntityDicSampTubeRack> list = dao.GetDictRackList(dateTimeFrom,dateTimeTo);
            return list;
        }


        #endregion

        #region 得到ID值最大的
        /// <summary>
        /// 得到ID值最大的
        /// </summary>
        /// <returns></returns>
        public string GetMaxRack()
        {
            string strMaxID = "";
            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
            strMaxID = dao.GetMaxRack();
            if (strMaxID == null || strMaxID == "")
            {
                strMaxID = "10001";
            }
            else
            {
                strMaxID = (Convert.ToInt32(strMaxID) + 1).ToString();
            }
            return strMaxID;
        }

        /// <summary>
        /// 得到架子序号值最大的
        /// </summary>
        /// <returns></returns>
        public string GetMaxRackCode()
        {
            string strMaxID = "";

            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
            strMaxID = dao.GetMaxRackCode();

            if (strMaxID == null || strMaxID == "")
            {
                strMaxID = "0";
            }
            else
            {
                strMaxID = strMaxID.ToString();
            }
            return strMaxID;
        }

        #endregion

        #region max barcode

        public string GetNextMaxBarCode()
        {
            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
            string strMaxID = dao.GetNextMaxBarCode();
            return strMaxID;
        }

        public bool IsRaclBarCodeExists(string barCode)
        {
            try
            {
                bool isExists = false;
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                bool IsExists = dao.IsRaclBarCodeExists(barCode, isExists);
                return IsExists;
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 IsBarCodeExists", ex);
            }
            return false;
        }

        public bool IsRaclBarCodePrint(string barCode)
        {
            try
            {
                bool isExists = true;
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                bool IsExists = dao.IsRaclBarCodeExists(barCode, isExists);
                return IsExists;
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 IsBarCodeExists", ex);
            }
            return false;
        }

        #endregion

        #region 物理组别

        public List<EntityDicPubProfession> GetPhyic()
        {
            List<EntityDicPubProfession> ProfessionList = new List<EntityDicPubProfession>();
            try
            {
                ICacheData cacheDao = new CacheDataBIZ();
                EntityResponse respone = new EntityResponse();
                respone = cacheDao.GetCacheData("EntityDicPubProfession");
                ProfessionList = respone.GetResult() as List<EntityDicPubProfession>;
                ProfessionList = ProfessionList.Where(i => i.ProType == 1).ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetPhyic", ex);
            }
            return ProfessionList;
        }

        #endregion

        #region 插入一条数据

        public bool InsertIntoRack(EntityDicSampTubeRack entity)
        {
            bool intRet = false;
            try
            {
                if (string.IsNullOrEmpty(entity.RackBarcode))
                    entity.RackBarcode = GetNextMaxBarCode();
                IDaoDic<EntityDicSampTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampTubeRack>>();
                intRet = dao.Save(entity);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 InsertIntoRack", ex);
            }
            return intRet;


        }

        #endregion

        #region 修改一条数据

        public bool ModifyRackRecord(EntityDicSampTubeRack entity)
        {
            bool intRet = false;

            try
            {
                IDaoDic<EntityDicSampTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampTubeRack>>();
                intRet = dao.Update(entity);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 ModifyRackRecord", ex);
            }

            return intRet;
        }

        #endregion

        #region 删除一条记录

        public int DeleteRackRecord(EntityDicSampTubeRack entity)
        {
            int intRet = -1;
            try
            {
                IDaoDic<EntityDicSampTubeRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampTubeRack>>();
                entity.RackDelFlag = 1;
                bool isSuccess = dao.Update(entity);
                if (isSuccess)
                {
                    intRet = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 DeleteRackRecord", ex);
            }

            return intRet;
        }

        #endregion



        #region 通过扫描的条码号获得病人的信息
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

        public List<EntitySampStoreDetail> GetBatchHandData(DateTime date,string BatchItr,string SamForm,string SamTo,int selectIndex)
        {

            try
            {

                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                List<EntitySampStoreDetail> list = dao.GetSamDetail(date, BatchItr,SamForm, SamTo, selectIndex);


                List<string> barcodeList = new List<string>();
                List<EntitySampStoreDetail> returnDT = list;
                foreach (EntitySampStoreDetail row in list)
                {
                    if (row.DetBarCode == null || row.DetBarCode == "")
                    {
                        row.Checked = true;
                    }

                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Sample_SamStoreNullBarCode") == "是")
                    {
                        EntitySampStoreDetail newRow = new EntitySampStoreDetail();
                        newRow = row;
                        returnDT.Add(newRow);
                    }
                    else
                    {
                        if (row.RepBarCode== null || string.IsNullOrEmpty(row.RepBarCode.ToString()))
                            continue;
                        if (!barcodeList.Contains(row.RepBarCode.ToString()))
                        {
                            barcodeList.Add(row.RepBarCode.ToString());
                            EntitySampStoreDetail newRow = new EntitySampStoreDetail();
                            newRow = row;
                            returnDT.Add(newRow);
                        }
                    }
                }

                return returnDT;

            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetBatchHandData", ex);
                return null;
            }
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


        #endregion

        #region 查询架子中试管的详细内容
        /// <summary>
        /// 查询架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        public List<EntitySampStoreDetail> GetRackDetail(string strSsid)
        {
            List<EntitySampStoreDetail> dt = null;
            List<EntitySampStoreDetail> returnDT = null;

            try
            {
                IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
                dt = dao.GetRackDetail(strSsid);

                List<string> barcodeList = new List<string>();
                returnDT = dt;
                foreach (EntitySampStoreDetail row in dt)
                {
                    if (!barcodeList.Contains(row.DetBarCode.ToString()))
                    {
                        barcodeList.Add(row.DetBarCode.ToString());
                        returnDT.Add(row);
                    }
                }
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

        /// <summary>
        /// 查询架子的明细信息，无外连信息
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        public List<EntitySampStoreDetail> GetSimpleRackDetail(string strSsid)
        {
            List<EntitySampStoreDetail> SampDetail = new List<EntitySampStoreDetail>();
            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();

            try
            {
                SampDetail = dao.GetSimpleRackDetail();

                if (!string.IsNullOrEmpty(strSsid))
                {
                    SampDetail = SampDetail.Where(i => i.DetId == strSsid && i.DetStatus != 20).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetRackDetail", ex);
            }
            return SampDetail;
        }

        #endregion

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
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                bool IsExists = dao.AddSampStoreDetail(entity);
                if (IsExists)
                {
                    intRet++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 InsertRackDetail", ex);
            }

            return intRet;
        }



        #endregion


        #region 新增条码操作记录

        public int InsertBcSign(string operatorName, string operatorID, string barCode, string bcStatus, string remark, string palce)
        {
            int intRet = -1;
            EntitySampProcessDetail SampProcessDetial = new EntitySampProcessDetail();
            SampProcessDetial.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
            SampProcessDetial.ProcUsercode = operatorID;
            SampProcessDetial.ProcUsername = operatorName;
            SampProcessDetial.ProcStatus = bcStatus;
            SampProcessDetial.ProcBarno = barCode;
            SampProcessDetial.ProcBarcode = barCode;
            SampProcessDetial.ProcPlace = palce;
            SampProcessDetial.ProcContent = remark;
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                bool IsExists = dao.AddSampProcessDetail(SampProcessDetial);
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

        public List<EntitySampProcessDetail> GetBcSign(string barCode)
        {
            List<EntitySampProcessDetail> SampProcessDetail = new List<EntitySampProcessDetail>();
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
                //SampProcessDetail = dao.GetBcSign(barCode);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetBcSign", ex);
            }
            return SampProcessDetail;
        }

        #endregion


        #region 获取最大的归档记录
        /// <summary>
        /// 获取最大的归档记录
        /// </summary>
        /// <returns></returns>
        public string GetMaxRackDetail()
        {
            string MaxSSID = "";

            //    string strSql = @"select max(ssd_id) from SamStore_RackDetail";

            //    if (dao.DoScalar(strSql) == null)
            //    {
            //        MaxSSID = "10001";
            //    }
            //    else
            //    {
            //        MaxSSID = (Convert.ToInt16(dao.DoScalar(strSql)) + 1).ToString();
            //    }
            return MaxSSID;
        }

        #endregion

        #region 获取最大的标本试管记录ID
        /// <summary>
        /// 获取最大的标本记录ID
        /// </summary>
        /// <returns></returns>
        public string GetMaxSamRackID()
        {
            string MaxSSID = "";

            IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();
            MaxSSID = dao.GetMaxSamRackID();

            if (MaxSSID == null)
            {
                MaxSSID = "10001";
            }
            else
            {
                MaxSSID = (Convert.ToInt16(MaxSSID) + 1).ToString();
            }
            return MaxSSID;
        }

        #endregion

        #region 向SamStore_Rack中插入一条数据
        /// <summary>
        /// 向SamRack中插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertSamRack(EntitySampStoreRack entity)
        {
            bool intRet = false;
            try
            {
                IDaoDic<EntitySampStoreRack> dao = DclDaoFactory.DaoHandler<IDaoDic<EntitySampStoreRack>>();
                intRet = dao.Save(entity);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 InsertSamRack", ex);
            }

            return intRet;
        }

        #endregion


        #region 得到样本的信息
        public List<EntityDicSample> GetSample()
        {
            //ICacheData dao = DclDaoFactory.DaoHandler<ICakcheData>();
            ICacheData dao = new CacheDataBIZ();
            EntityResponse ds = dao.GetCacheData("EntityDicSample");
            List<EntityDicSample> list = ds.GetResult() as List<EntityDicSample>;
            return list;
        }

        #endregion



        #region 修改样本的状态
        public int ModifySamDetail(EntitySampStoreDetail entity)
        {
            int intRet = -1;
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();

                intRet = dao.UpdateSamDetail(entity);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 ModifySamDetail", ex);
            }
            return intRet;
        }

        #endregion


        #region 标本状态
        public List<EntityDicSampStoreStatus> GetSamManageStatus()
        {
            IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
            List<EntityDicSampStoreStatus> list = dao.GetSamManageStatus();
            return list;
        }

        #endregion

        #region 修改SamStoreRack

        public int ModifySamStoreRack(EntitySampStoreRack entity)
        {
            int intRet = -1;
            try
            {
                IDaoSampStock dao = DclDaoFactory.DaoHandler<IDaoSampStock>();

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


        public int AuditSamStoreRack(EntitySampStoreRack entity)
        {
            int intRet = -1;
            try
            {
                IDaoSamSave dao = DclDaoFactory.DaoHandler<IDaoSamSave>();
                intRet = dao.AuditSamStoreRack(entity);
                if (intRet > 0)
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


        #endregion


    }
}
