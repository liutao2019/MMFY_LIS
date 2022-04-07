using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.cache;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.sample
{
    /// <summary>
    /// 条码登记
    /// </summary>
    public class SampRegisterBIZ : ISampRegister
    {
        /// <summary>
        /// 保存试管架条码信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns>成功状态 1保存成功  -1条码号存在  -2顺序号存在  -3保存异常</returns>
        public int SaveShelfBarcode(EntitySampRegister data)
        {
            int intRet = 0;
            try
            {
                DateTime stDate = ServerDateTime.GetDatabaseServerDateTime();

                if (data.RegDate == stDate.Date)
                    data.RegDate = stDate;
                else
                    data.RegDate = data.RegDate.AddHours(stDate.Hour).AddMinutes(stDate.Minute).AddSeconds(stDate.Second);

                IDaoSampRegister dao = DclDaoFactory.DaoHandler<IDaoSampRegister>();
                if (dao != null)
                {
                    intRet = dao.SaveShelfBarcode(data);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("SaveShelfBarcode", ex);
            }

            return intRet;
        }

        public bool DeleteShelfBarcode(Int64 RegSn)
        {
            bool intRet = false;
            try
            {
                IDaoSampRegister dao = DclDaoFactory.DaoHandler<IDaoSampRegister>();
                if (dao != null)
                {
                    intRet = dao.DeleteShelfBarcode(RegSn);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException("DeleteShelfBarcode", ex);
            }
            return intRet;
        }

        public List<EntitySampRegister> GetSampRegister(long RegSn)
        {
            List<EntitySampRegister> SampRegisterList = new List<EntitySampRegister>();
            try
            {
                IDaoSampRegister dao = DclDaoFactory.DaoHandler<IDaoSampRegister>();
                if (dao != null)
                {
                    SampRegisterList = dao.GetSampRegister(RegSn);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetSampRegister", ex);
            }
            return SampRegisterList;
        }

        public List<EntitySampRegister> GetCuvetteRegisteredBarcodeInfo(string deptid, DateTime depTime)
        {
            List<EntitySampRegister> SampRegisterList = new List<EntitySampRegister>();
            try
            {
                IDaoSampRegister dao = DclDaoFactory.DaoHandler<IDaoSampRegister>();
                if (dao != null)
                {
                    SampRegisterList = dao.GetCuvetteRegisteredBarcodeInfo(deptid, depTime);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetCuvetteRegisteredBarcodeInfo", ex);
            }
            return SampRegisterList;
        }
        /// <summary>
        /// 快速排样查询条码登记信息
        /// </summary>
        /// <param name="receviceDeptID"></param>
        /// <param name="regDateFrom"></param>
        /// <param name="regDateTo"></param>
        /// <param name="shelfNoFrom"></param>
        /// <param name="shelfNoTo"></param>
        /// <param name="seqFrom"></param>
        /// <param name="seqTo"></param>
        /// <returns></returns>
        public List<EntitySampRegister> GetCuvetteShelfInfo(string receviceDeptID, DateTime regDateFrom, DateTime regDateTo, int? shelfNoFrom, int? shelfNoTo, int? seqFrom, int? seqTo)
        {
            List<EntitySampRegister> CuvetteShelfInfo = new List<EntitySampRegister>();
            try
            {
                IDaoSampRegister dao = DclDaoFactory.DaoHandler<IDaoSampRegister>();
                if (dao != null)
                {
                    CuvetteShelfInfo = dao.GetCuvetteShelfInfo(receviceDeptID, regDateFrom, regDateTo, shelfNoFrom, shelfNoTo, seqFrom, seqTo);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetCuvetteShelfInfo", ex);
            }
            return CuvetteShelfInfo;
        }
        /// <summary>
        /// 排样登记查询统计信息
        /// </summary>
        /// <param name="StatQc"></param>
        /// <returns></returns>
        public EntityDCLPrintData GetReportData(EntityStatisticsQC StatQc)
        {
            EntityDCLPrintData printData = new EntityDCLPrintData();
            try
            {
                IDaoSampRegister dao = DclDaoFactory.DaoHandler<IDaoSampRegister>();
                if (dao != null)
                {
                    printData = dao.GetReportData(StatQc);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetReportData", ex);
            }
            return printData;
        }

        public int GetSampRegisterMaxId()
        {
            int intRet = 0;
            try
            {
                IDaoSampRegister dao = DclDaoFactory.DaoHandler<IDaoSampRegister>();
                if (dao != null)
                {
                    intRet = dao.GetSampRegisterMaxId();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetSampRegisterMaxId", ex);
            }

            return intRet;
        }
    }
}
