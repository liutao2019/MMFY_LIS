using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Configuration;
using System.Reflection;
using Lib.LogManager;
using System.Data;

namespace dcl.svr.msg
{
    /// <summary>
    /// 病人信息扩展表:BIZ
    /// </summary>
    public class PidReportMainExtBIZ : IDicPidReportMainExt
    {
        public bool AddOrUpdatePatientExt(EntityDicPidReportMainExt ext)
        {
            bool isTrue = false;
            try
            {
                IDaoPidReportMainExt dao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
                if (dao == null)
                {
                    Lib.LogManager.Logger.LogInfo("IDaoPidReportMainExt中dao为null");
                    return false;
                }
                //根据pat_id获取病人信息扩展表ID
                List<EntityDicPidReportMainExt> listPatientExt = dao.GetPatientExtDataByPatID(ext.RepId);
                if (listPatientExt.Count > 0)//如果大于零,则已经存在此条记录,则update
                {
                    //获取更新病人扩展表patients_ext的语句
                    isTrue = dao.UpdatePatientExtInfoCMD(ext);
                }
                else
                {
                    //获取新增病人扩展表patients_ext的语句
                    isTrue = dao.InsertPatientExtInfoCMD(ext);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("AddOrUpdatePatientExt", ex);
                isTrue = false;
            }

            return isTrue;
        }

        public bool UpdatePatientsExt(EntityDicPidReportMainExt PatientExt,bool update)
        {
            string strUpdateSql = string.Empty;

            bool isSuccess = false;

            string pat_id = PatientExt.RepId;
            IDaoPidReportMainExt dao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
            if (update)
            {
                if(dao != null)
                {
                    isSuccess = dao.UpdatePatientExt(PatientExt);
                }
            }
            else
            {
                if (dao != null)
                {
                    isSuccess =  dao.InsertPatientExt(PatientExt);
                }
            }


            //危急记录后,同时取消危急值内部提示
            //系统配置：危急值记录确认同时取消危急值内部提醒
            if (!string.IsNullOrEmpty(pat_id)
                && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_CriticalValueInfoAndUnInsgin") == "是")
            {
                IDaoObrMessageContent MsgConDao = DclDaoFactory.DaoHandler<IDaoObrMessageContent>();
                if(dao != null)
                {
                    isSuccess = MsgConDao.UpdateObrMsgConToInsignByRepID(pat_id);
                }
            }

            return isSuccess;
        }

        public List<EntityDicPidReportMainExt> GetPatientExtDataByPatID(string pat_id)
        {
            List<EntityDicPidReportMainExt> list = new List<EntityDicPidReportMainExt>();
            IDaoPidReportMainExt dao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
            if (dao != null)
            {
                list = dao.GetPatientExtDataByPatID(pat_id);
            }
            return list;
        }

        public bool SavePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id)
        {
            bool isSave = false;
            IDaoPidReportMainExt dao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
            if (dao != null)
            {
                isSave = dao.SavePidReportMainExt(objAuditInfo, pat_id);
            }
            return isSave;
        }

        public bool SearchPatExtExistByID(string pat_id)
        {
            bool isExisByID = false;
            IDaoPidReportMainExt dao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
            if (dao != null)
            {
                isExisByID = dao.SearchPatExtExistByID(pat_id);
            }
            return isExisByID;
        }

        public bool UpdatePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id)
        {
            bool isUpdate = false;
            IDaoPidReportMainExt dao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
            if (dao != null)
            {
                isUpdate = dao.UpdatePidReportMainExt(objAuditInfo, pat_id);
            }
            return isUpdate;
        }

        public bool DeletePidReportMainExt(string repId)
        {
            bool result = false;
            IDaoPidReportMainExt mainExtDao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
            if (mainExtDao != null)
            {
                result = mainExtDao.DeletePatientExt(repId);
            }
            return result;
        }

        public bool InsertReportCASignature(DataRow  dr)
        {
            bool result = false;
            IDaoPidReportMainExt mainExtDao = DclDaoFactory.DaoHandler<IDaoPidReportMainExt>();
            if (mainExtDao != null)
            {
                result = mainExtDao.InsertReportCASignature(dr);
            }
            return result;
        }
    }
}
