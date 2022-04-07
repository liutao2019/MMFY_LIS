using System.Collections.Generic;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using dcl.svr.sample;

namespace dcl.svr.users
{
    public class SysOperationLogBIZ : ISysOperationLog
    {
        /// <summary>
        /// 获取操作日志记录
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        public List<EntitySysOperationLog> GetOperationLog(EntityLogQc qc)
        {
            IDaoSysOperationLog dao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
            if (dao == null)
            {
                return new List<EntitySysOperationLog>();
            }
            else
            {
                return dao.SearchSysOperationLog(qc);
            }
        }

        /// <summary>
        /// 获取操作日志记录
        /// </summary>
        /// <param name="patid"></param>
        /// <returns></returns>
        public List<EntitySysOperationLog> GetQcOperationLog(EntityLogQc qc)
        {
            IDaoSysOperationLog dao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
            if (dao == null)
            {
                return new List<EntitySysOperationLog>();
            }
            else
            {
                return dao.SearchSysQcOperationLog(qc);
            }
        }

        public string GetPatId(string timeFrom, string timeTo, string patId, string userName)
        {
            string patID = "";
            IDaoSysOperationLog dao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
            if (dao != null)
                patID = dao.GetPatId(timeFrom, timeTo, patId, userName);

            return patID;
        }

        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatients(string timeFrom, string timeTo, string patNo, string itrType, string patInNo, string patName, string patItrId, string patCheck)
        {
            List<EntityPidReportMain> listPatient = new List<EntityPidReportMain>();
            IDaoSysOperationLog dao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
            if (dao != null)
                listPatient = dao.GetPatients(timeFrom, timeTo, patNo, itrType, patInNo, patName, patItrId, patCheck);

            return listPatient;
        }

        /// <summary>
        /// 获取条码流转信息
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public List<EntitySampProcessDetail> GetSampProcessDetail(string repId)
        {
            List<EntitySampProcessDetail> listDetail = new List<EntitySampProcessDetail>();
            IDaoSampProcessDetail dao = DclDaoFactory.DaoHandler<IDaoSampProcessDetail>();
            if (dao != null)
                listDetail = dao.GetSamprocessDetailByRepId(repId);

            return listDetail;
        }

        public string GetDeletePatId(string patId, string patName, string timeFrom, string timeTo)
        {
            string deletePatId = string.Empty;
            SampProcessDetailBIZ samp = new SampProcessDetailBIZ();
            deletePatId = samp.GetDeletePatId(patId, patName, timeFrom, timeTo);
            return deletePatId;
        }

        public bool SaveSysOperationLog(EntitySysOperationLog operationLog)
        {
            bool result = false;
            IDaoSysOperationLog logDao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
            if (logDao != null && operationLog != null)
            {
                result = logDao.SaveSysOperationLog(operationLog);
            }
            return result;
        }
    }
}
