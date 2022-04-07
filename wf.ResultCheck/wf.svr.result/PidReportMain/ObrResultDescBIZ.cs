using dcl.servececontract;
using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using dcl.root.logon;
using dcl.svr.sample;
using dcl.svr.cache;
using dcl.dao.core;

namespace dcl.svr.result
{
    public class ObrResultDescBIZ : DclBizBase, IObrResultDesc
    {
        public bool DeleteResultById(string obrId)
        {
            bool result = false;
            IDaoObrResultDesc dao = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
            if (dao != null)
            {
                dao.Dbm = this.Dbm;
                result = dao.DeleteResultById(obrId);
            }
            return result;
        }

        public List<EntityObrResultDesc> GetDescResultById(string obrId = "", string repFlag = "1")
        {
            IDaoObrResultDesc dao = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return null;
            }
            else
            {
                try
                {
                    List<EntityObrResultDesc> listResult = dao.GetDescResultById(obrId, repFlag);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return null;
                }
            }
        }

        public bool InsertObrResultDesc(List<EntityObrResultDesc> listObrResultDesc)
        {
            bool listResult = false;
            if (listObrResultDesc == null || listObrResultDesc.Count == 0) return listResult;
            IDaoObrResultDesc dao = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                dao.Dbm = this.Dbm;
                try
                {
                    foreach (EntityObrResultDesc desc in listObrResultDesc)
                    {
                        List<EntityObrResultDesc> resultDesc = GetDescResultById(desc.ObrId);
                        if (resultDesc == null || resultDesc.Count == 0 )
                        {
                            listResult = dao.InsertObrResultDesc(desc);
                        }
                        if((resultDesc != null && resultDesc.Count > 0) && !string.IsNullOrEmpty(desc.ObrDescribe))
                        {
                            listResult = dao.UpdateObrResultDesc(desc);
                        }
                    }
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }

        public bool UpdateObrResultDesc(EntityObrResultDesc ObrResultDesc)
        {
            bool result = false;
            IDaoObrResultDesc descDao = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
            if (descDao != null)
            {
                descDao.Dbm = this.Dbm;
                result = descDao.UpdateObrResultDesc(ObrResultDesc);
            }
            return result;
        }
        public List<EntityObrResultDesc> GetObrResultDescById(string obrId)
        {
            List<EntityObrResultDesc> list = new List<EntityObrResultDesc>();
            IDaoObrResultDesc descDao = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
            if (descDao != null)
            {
                list = descDao.GetObrResultDescById(obrId);
            }
            return list;
        }

        /// <summary>
        /// 批量删除病人描述报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public List<EntityOperationResult> BatchDelPatDescResult(EntityRemoteCallClientInfo caller, List<string> listPatID, bool delWithResult)
        {
            List<EntityOperationResult> listOpResult = new List<EntityOperationResult>();

            bool result = false;
            foreach (string pat_id in listPatID)
            {
                List<EntityPidReportDetail> listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);
                EntityPatientQC qc = new EntityPatientQC();
                qc.RepId = pat_id;
                List<EntityPidReportMain> listPatient = new PidReportMainBIZ().PatientQuery(qc);
                EntityPidReportMain patient = new EntityPidReportMain();
                if (listPatient != null && listPatient.Count > 0)
                {
                    patient = listPatient[0];
                }
                EntityOperationResult opResult = new EntityOperationResult();//.GetNew("删除描述结果病人资料");
                opResult.Data.Patient.RepId = pat_id;

                PidReportMainBIZ reportMainBIZ = new PidReportMainBIZ();

                string pat_flag = reportMainBIZ.GetPatientState(pat_id);

                if (pat_flag == LIS_Const.PATIENT_FLAG.Audited
                    || pat_flag == LIS_Const.PATIENT_FLAG.Reported
                    || pat_flag == LIS_Const.PATIENT_FLAG.Printed)
                {
                    opResult.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                }
                else
                {
                    try
                    {
                        //获取删除语句
                        result = new PidReportMainBIZ().DeletePatient(pat_id);

                        if (result)
                            result = new PidReportDetailBIZ().DeleteReportDetail(pat_id);
                        string remark = string.Empty;
                        string strComName = string.Empty;
                        #region 更新上机标志为0,并更新Samp_process_detial表
                        if (result)
                        {
                            SampDetailBIZ detailBiz = new SampDetailBIZ();
                            foreach (EntityPidReportDetail detail in listDetail)
                            {
                                List<string> comIds = new List<string>();
                                comIds.Add(detail.ComId);
                                strComName += string.Format(",'{0}'", detail.PatComName);
                                strComName = strComName.Remove(0, 1);
                                if (!string.IsNullOrEmpty(detail.RepBarCode))  //如果有条码号则更新上机标志
                                {
                                    detailBiz.UpdateSampDetailSampFlagByComId(detail.RepBarCode, comIds, "0");
                                }
                            }

                            EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(patient.RepBarCode);

                            if (patient != null && !string.IsNullOrEmpty(patient.PidComName))
                            {
                                remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                              patient.RepId, patient.PidName, patient.PidComName,
                              patient.PidInNo, patient.ItrName, patient.RepSid);
                            }
                            else
                                remark = "组合：" + strComName;
                            EntitySampOperation operation = new EntitySampOperation();
                            operation.OperationStatus = "530";
                            operation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
                            operation.OperationID = caller.LoginID;
                            operation.OperationIP = caller.IPAddress;
                            operation.Remark = remark;
                            operation.RepId = patient.RepId;
                            result = new SampProcessDetailBIZ().SaveSampProcessDetail(operation, sampMain);
                        }

                        #endregion

                        if (delWithResult)//如果同时删除结果
                        {
                            DeleteResultById(pat_id);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteException(this.GetType().Name, string.Format("BatchDelPatDescResult({0},{1})", pat_id, delWithResult), ex.ToString());
                        opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                    }
                }
                listOpResult.Add(opResult);
            }

            return listOpResult;
        }
    }
}
