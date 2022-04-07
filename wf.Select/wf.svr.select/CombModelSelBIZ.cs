using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using dcl.dao.interfaces;
using dcl.common;
using dcl.svr.cache;
using dcl.svr.sample;
using dcl.svr.users;
using dcl.svr.result;
using dcl.dao.core;
using dcl.svr.msg;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using System.Configuration;

namespace dcl.svr.resultquery
{
    public class CombModelSelBIZ : ICombModelSel
    {
        public List<EntityPidReportMain> GetPatientsList(EntityAnanlyseQC query, string dateStart, string dateEnd)
        {
            IDaoCombModelSel dao = DclDaoFactory.DaoHandler<IDaoCombModelSel>();
            query.ReportCheckNotPrintReport = CacheSysConfig.Current.GetSystemConfig("Report_CheckNotPrintReport");
            query.OuterReportRepStyle = CacheSysConfig.Current.GetSystemConfig("OuterReportRepStyle");
            query.OuterReportCode = CacheSysConfig.Current.GetSystemConfig("OuterReportCode");
            query.OuterReportCommonRepCode = CacheSysConfig.Current.GetSystemConfig("OuterReportCommonRepCode");
            query.HospitalId= ConfigurationManager.AppSettings["HospitalId"];
            DataSet ds = new DataSet();
            List<EntityPidReportMain> listPatients = new List<EntityPidReportMain>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("未找到数据访问");
                //throw new Exception();
            }
            else
            {
                try
                {
                    listPatients = dao.GetPatientsList(query, dateStart, dateEnd);
   
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                  //  throw ex;
                }
            }
            return listPatients;
        }

        public EntityQcResultList GetPatientResultData(EntityAnanlyseQC query, DateTime dateStart)
        {
            IDaoCombModelSel dao = DclDaoFactory.DaoHandler<IDaoCombModelSel>();
            EntityQcResultList listQcResult = new EntityQcResultList();
            query.BacLabExistsAnAndCsSelAn = CacheSysConfig.Current.GetSystemConfig("BacLab_ExistsAnAndCs_SelAn");
            query.SelectFiterNoPrintItem = CacheSysConfig.Current.GetSystemConfig("Select_FiterNoPrintItem");
            query.LabEnableReadHistoryFromOldDB = CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB");
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("未找到数据访问");
               // throw new Exception();
            }
            else
            {
                try
                {
                    listQcResult = dao.GetPatientResultData(query, dateStart);
                    if (listQcResult.listPatients != null && listQcResult.listPatients.Count > 0)
                    {
                        EntityPidReportMain patient = listQcResult.listPatients[0];
                        string repCode = patient.ItrReportId.ToString();
                        string itr_rep_flag = patient.ItrReportType.ToString();
                        if (itr_rep_flag == "3" || itr_rep_flag == "4")
                        {
                            ObrResultDescBIZ descBiz = new ObrResultDescBIZ();
                            ObrResultAntiBIZ antiBiz = new ObrResultAntiBIZ();
                            List<EntityObrResultDesc> listCs = new List<EntityObrResultDesc>();
                            listCs = descBiz.GetDescResultById(query.PatId, itr_rep_flag);
                            bool IsexistsAnCs = false;//同时存在药敏与无菌结果，默认药敏
                                                      //系统配置：细菌管理同时有药敏与无菌结果时优先药敏
                            if (query.BacLabExistsAnAndCsSelAn == "是"
                                && listCs != null && listCs.Count > 0)
                            {
                                List<EntityObrResultAnti> listAn = new List<EntityObrResultAnti>();
                                listAn = antiBiz.GetAntiResultById(query.PatId);
                                if (listAn.Count > 0)
                                {
                                    IsexistsAnCs = true;
                                }
                            }
                            if (listCs != null && listCs.Count > 0 && !IsexistsAnCs)
                            {
                                listQcResult.listDesc = listCs;
                            }
                            else
                            {
                                ObrResultBactBIZ bactBiz = new ObrResultBactBIZ();
                                List<EntityObrResultBact> listBact = bactBiz.GetBactResultById(query.PatId);
                                listQcResult.listBact = listBact;

                                List<EntityObrResultAnti> listAnti = antiBiz.GetAntiResultById(query.PatId);
                                listQcResult.listAnti = listAnti;
                            }
                        }
                        else
                        {
                            ObrResultBIZ resultBiz = new ObrResultBIZ();
                            bool filterFlag = query.SelectFiterNoPrintItem == "是";

                            EntityResultQC resultQc = new EntityResultQC();
                            resultQc.ListObrId.Add(query.PatId);

                            resultQc.IsNullItmPrtFlag = filterFlag;
                            resultQc.IsNullComPrtFlag = !filterFlag;

                            listQcResult.listResulto = resultBiz.ObrResultQuery(resultQc).FindAll(i => i.NeedDelete == false);
                        }
                    }
    
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                   // throw ex;
                }

            }
            return listQcResult;
        }

        public bool UpdatePatFlagToPrinted(EntityAnanlyseQC query)
        {
            IDaoCombModelSel dao = DclDaoFactory.DaoHandler<IDaoCombModelSel>();
            bool result = false;
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("未找到数据访问");
                throw new Exception();
            }
            else
            {
                try
                {
                    result = dao.UpdatePatFlagToPrinted(query);
                    if (result)
                    {
                        List<EntityPidReportMain> listPatients = dao.GetPatBarCode(query);

                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationID = query.OperatorID;
                        operation.OperationName = query.OperatorName;
                        operation.Remark = query.StrRemark;
                        operation.OperationStatus = "100";
                        operation.OperationTime = DateTime.Now;
                        operation.OperationPlace = query.StrPlace;

                        SampProcessDetailBIZ detailBiz = new SampProcessDetailBIZ();
                        SampMainBIZ mainBiz = new SampMainBIZ();
                        if (listPatients.Count > 0)
                        {
                            foreach (var item in listPatients)
                            {
                                //更新条码信息
                                if (!string.IsNullOrEmpty(item.RepBarCode))
                                {
                                    operation.RepId = item.RepId;
                                    EntitySampMain sampMain = mainBiz.SampMainQueryByBarId(item.RepBarCode);
                                    if (sampMain != null)
                                    {
                                        detailBiz.SaveSampProcessDetail(operation, sampMain);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
                return result;
            }
        }

        public EntityQcResultList GetCsAndAnResult(string obrId)
        {
            ObrResultDescBIZ descBiz = new ObrResultDescBIZ();
            ObrResultAntiBIZ antiBiz = new ObrResultAntiBIZ();
            DataSet ds = new DataSet();
            EntityQcResultList result = new EntityQcResultList();
            if (descBiz == null || antiBiz == null)
            {
                Lib.LogManager.Logger.LogInfo("未找到数据访问");
              //  throw new Exception();
            }
            else
            {
                try
                {
                    result.listDesc = descBiz.GetDescResultById(obrId);
                    result.listAnti = antiBiz.GetAntiResultById(obrId);
 
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                 //   throw ex;
                }
            }
            return result;
        }

        public EntityDCLPrintData GetPatientReportInfo(EntityAnanlyseQC query)
        {
            IDaoCombModelSel dao = DclDaoFactory.DaoHandler<IDaoCombModelSel>();
            EntityDCLPrintData ds = new EntityDCLPrintData();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("未找到数据访问");
            }
            else
            {
                try
                {
                    ds = dao.GetPatientReportInfo(query);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return ds;
        }

        public DataTable GetPatId(EntityAnanlyseQC query)
        {
            IDaoCombModelSel dao = DclDaoFactory.DaoHandler<IDaoCombModelSel>();
            DataTable dtPatId = new DataTable();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("未找到数据访问");
           //     throw new Exception();
            }
            else
            {
                try
                {
                    dtPatId = dao.GetPatId(query);

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
            //        throw ex;
                }

            }
            return dtPatId;
        }

        public bool SaveSysPara(string sort, string patientsSelectViewSortConfigcode)
        {
            SystemConfigBIZ configBiz = new SystemConfigBIZ();
            string addRemark = string.Empty;
            if (patientsSelectViewSortConfigcode == "PatientsSelectViewSortForAlone")
            {
                addRemark = "(独立客户端)";
            }

            List<EntitySysParameter> listPara = configBiz.GetSysParaListByConfigCode(patientsSelectViewSortConfigcode);
            bool result;
            if (listPara != null && listPara.Count == 1)
            {
                listPara[0].ParmFieldValue = sort;
                result = configBiz.UpdateSysPara(listPara);
            }
            else
            {
                EntitySysParameter para = new EntitySysParameter();
                para.ParmCode = patientsSelectViewSortConfigcode;
                para.ParmGroup = "查询与统计";
                para.ParmFieldName = string.Format("检验报告查询病人列表列顺序{0}", addRemark);
                para.ParmFieldType = "字符串";
                para.ParmFieldValue = sort;
                para.ParmDictList = string.Empty;
                para.ParmType = "system";
                result = configBiz.InsertSysPara(para);
            }
            return result;
        }

        public bool DeletePatientInfo(EntityPidReportMain patient, string delFlag)
        {
            bool deleteResult = false;
            deleteResult = DeletePatient(patient.RepId);
            if (!string.IsNullOrEmpty(patient.RepBarCode))
            {
                List<EntityPidReportDetail> listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(patient.RepId);
                SampDetailBIZ detailBiz = new SampDetailBIZ();
                foreach (EntityPidReportDetail detail in listDetail)
                {
                    List<string> comIds = new List<string>();
                    comIds.Add(detail.ComId);

                    if (!string.IsNullOrEmpty(detail.RepBarCode))
                    {
                        detailBiz.UpdateSampDetailSampFlagByComId(detail.RepBarCode, comIds, "0");
                    }
                }
                //bool result = new SampDetailBIZ().UpdateSampDetailSampFlag(patient.RepId, patient.RepBarCode);

                EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(patient.RepBarCode);
                EntitySampOperation operation = new EntitySampOperation();
                operation.OperationStatus = "530";
                operation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
                bool insertResult = new SampProcessDetailBIZ().SaveSampProcessDetail(operation, sampMain);
            }
            deleteResult = new PidReportDetailBIZ().DeleteReportDetail(patient.RepId);
            if (delFlag.Trim() == "1")
            {
                deleteResult = new ObrResultAntiBIZ().DeleteResultById(patient.RepId);
                deleteResult = new ObrResultBactBIZ().DeleteResultById(patient.RepId);
                deleteResult = new ObrResultDescBIZ().DeleteResultById(patient.RepId);
            }
            return deleteResult;
        }

        public bool DeletePatient(string repId)
        {
            bool result = false;
            IDaoCombModelSel dao = DclDaoFactory.DaoHandler<IDaoCombModelSel>();
            if (dao != null)
            {
                result = dao.DeletePatient(repId);
            }
            return result;
        }

        public List<EntityPidReportDetail> GetCombineSeqForPatID(string repId)
        {
            List<EntityPidReportDetail> listDetail = new List<EntityPidReportDetail>();
            if (!string.IsNullOrEmpty(repId))
            {
                listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(repId);
            }
            return listDetail;
        }

        public bool DelPatCommonResult(EntityLogLogin logLogin, EntityPidReportMain patient, bool bDelResult, bool canDeleteAuditedResult)
        {
            EntityOperationResult opResult = new EntityOperationResult();
            opResult.Data.Patient.RepId = patient.RepId;

            List<EntityPidReportDetail> listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(patient.RepId);

            List<EntitySysOperationLog> listLog = new List<EntitySysOperationLog>();//用于更新日志

            bool result = false;

            string barCode = patient.RepBarCode;
            string repStatus = string.Empty;

            if (patient.RepStatus != null)
                repStatus = patient.RepStatus.Value.ToString();

            //判断病人状态是否是1,2,4
            if ((repStatus == "1"
                || repStatus == "2"
                || repStatus == "4")
                && !canDeleteAuditedResult)
            {
                opResult.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
            }
            else
            {
                DBManager helper = new DBManager();

                //开启事务
                helper.BeginTrans();
                try
                {

                    #region 删除病人和病人组合
                    result = DeletePatient(patient.RepId);
                    if (result)
                        result = new PidReportDetailBIZ().DeleteReportDetail(patient.RepId);
                    #endregion

                    #region 更新上机标志为0,并更新Samp_process_detial表
                    if (result)
                    {

                        SampDetailBIZ detailBiz = new SampDetailBIZ();
                        foreach (EntityPidReportDetail detail in listDetail)
                        {
                            List<string> comIds = new List<string>();
                            comIds.Add(detail.ComId);

                            if (!string.IsNullOrEmpty(detail.RepBarCode))
                            {
                                detailBiz.UpdateSampDetailSampFlagByComId(detail.RepBarCode, comIds, "0");
                            }
                        }
                        EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(patient.RepBarCode);
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "530";
                        operation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
                        operation.OperationID = logLogin.LogLoginID;
                        operation.OperationIP = logLogin.LogIP;
                        operation.RepId = patient.RepId;
                        result = new SampProcessDetailBIZ().SaveSampProcessDetail(operation, sampMain);
                    }

                    #endregion

                    #region 删除病人扩展信息
                    if (result)
                    {
                        result = new PidReportMainExtBIZ().DeletePidReportMainExt(patient.RepId);
                    }
                    #endregion

                    #region 日志实体模板
                    EntitySysOperationLog opLog = new EntitySysOperationLog();
                    opLog.OperatUserId = logLogin.LogLoginID;
                    opLog.OperatServername = logLogin.LogIP;
                    opLog.OperatModule = "病人资料";
                    opLog.OperatKey = patient.RepId;
                    opLog.OperatDate = ServerDateTime.GetDatabaseServerDateTime();
                    #endregion

                    if (result)
                    {
                        #region //记录日志实体,填充好添加到日志实体List

                        #region 填充删除病人基本信息日志实体
                        PropertyInfo[] propertys = patient.GetType().GetProperties();
                        foreach (PropertyInfo item in propertys)
                        {
                            string colCHS = DclFieldsNameConventer<EntityPatientFields>.Instance.GetFieldCHS(item.Name);
                            if (!string.IsNullOrEmpty(colCHS))
                            {
                                EntitySysOperationLog operationLog = opLog.Clone() as EntitySysOperationLog;
                                operationLog.OperatGroup = "病人基本信息";
                                operationLog.OperatAction = "删除";
                                operationLog.OperatContent = item.GetValue(patient, null) != null ? item.GetValue(patient, null).ToString() : string.Empty;
                                operationLog.OperatObject = colCHS;

                                listLog.Add(operationLog);
                            }

                        }
                        #endregion

                        #region 填充删除病人组合的日志实体
                        foreach (EntityPidReportDetail detail in listDetail)
                        {
                            string comName = string.Empty;

                            if (!string.IsNullOrEmpty(detail.PatComName))
                            {
                                comName = detail.PatComName;
                            }
                            else if (!string.IsNullOrEmpty(detail.ComId))
                            {
                                comName = detail.ComId;
                            }
                            if (comName != string.Empty)
                            {
                                EntitySysOperationLog operationLog = opLog.Clone() as EntitySysOperationLog;
                                operationLog.OperatGroup = "检验组合";
                                operationLog.OperatAction = "删除";
                                operationLog.OperatObject = comName;
                                operationLog.OperatContent = string.Empty;

                                listLog.Add(operationLog);
                            }

                        }
                        #endregion

                        #endregion
                    }
                    //判断是否删除病人结果
                    if (bDelResult && result)
                    {
                        #region 填充删除病人检验结果的日志实体
                        EntityResultQC resultQc = new EntityResultQC();
                        resultQc.ListObrId.Add(patient.RepId);
                        List<EntityObrResult> listResult = new ObrResultBIZ().ObrResultQuery(resultQc);
                        foreach (EntityObrResult obrResult in listResult)
                        {
                            EntitySysOperationLog operationLog = opLog.Clone() as EntitySysOperationLog;
                            operationLog.OperatGroup = "病人结果";
                            operationLog.OperatAction = "删除";
                            operationLog.OperatObject = obrResult.ItmEname;
                            operationLog.OperatContent = obrResult.ObrValue;
                        }
                        #endregion

                        result = new ObrResultBIZ().DeleteObrResultByObrId(patient.RepId);
                    }

                    #region 插入操作日志
                    if (result)
                    {
                        //如果以上步骤没有出错，就把插入操作日志
                        try
                        {
                            foreach (EntitySysOperationLog log in listLog)
                                new SysOperationLogBIZ().SaveSysOperationLog(log);
                            result = true;
                        }
                        catch
                        {
                            result = false;
                        }

                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogInfo("DelPatCommonResult", ex.Message);
                    opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                }

                if (result)
                {
                    helper.CommitTrans();
                    helper = null;//开个事务不知道干嘛。。、】
                }
                else
                {
                    helper.RollbackTrans();
                }
            }


            return result;
        }
        /// <summary>
        /// 获取病人列表压缩后的字节流
        /// </summary>
        /// <param name="query"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public byte[] GetPatientsListAllBuffer(EntityAnanlyseQC query, string dateStart, string dateEnd)
        {
            List<EntityPidReportMain> listPat = GetPatientsList(query, dateStart, dateEnd);
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, listPat);
            byte[] origionByte = stream.ToArray();
            byte[] compressByte = Compression.DeflateData(origionByte);
            return compressByte;
        }

        public bool IsContainOutlier(string repId)
        {
            bool result = false;
            IDaoCombModelSel dao = DclDaoFactory.DaoHandler<IDaoCombModelSel>();
            if (dao != null)
            {
                result = dao.IsContainOutlier(repId);
            }
            return result;
        }
    }
}
