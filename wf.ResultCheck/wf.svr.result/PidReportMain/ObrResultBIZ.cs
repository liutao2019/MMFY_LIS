using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Data;
using dcl.svr.cache;
using dcl.svr.dicbasic;
using dcl.root.logon;
using System.Configuration;

using System.Threading;
using dcl.svr.sample;
using dcl.dao.core;
using dcl.svr.msg;
using System.Reflection;
using dcl.svr.users;

namespace dcl.svr.result
{
    public class ObrResultBIZ : IObrResult
    {

        public bool InsertObrResult(EntityObrResult ObrResult)
        {
            return InsertObrResult(ObrResult, true);
        }

        public bool InsertObrResult(EntityObrResult ObrResult, bool UpdateObrDate = true)
        {
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    if (UpdateObrDate)
                    {
                        ObrResult.ObrDate = ServerDateTime.GetDatabaseServerDateTime();
                    }
                    bool listResult = dao.InsertObrResult(ObrResult);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }

        public bool UpdateObrResultByObrIdAndObrItmId(EntityObrResult ObrResult)
        {
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    bool listResult = dao.UpdateObrResultByObrIdAndObrItmId(ObrResult);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }


        public bool UpdateObrFlagByCondition(EntityResultQC resultQc)
        {
            bool result = false;
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                result = resultDao.UpdateObrFlagByCondition(resultQc);
            }
            return result;
        }

        public bool UpdateRecheckFalgByObrSn(string obrSn)
        {
            bool result = false;
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                result = resultDao.UpdateRecheckFalgByObrSn(obrSn);
            }
            return result;
        }


        /// <summary>
        /// 插入缺省结果
        /// </summary>
        /// <param name="transHelper"></param>
        public void InsertDefaultResult(string pat_id, string pat_sam_id, string pat_itr_id, string pat_sid, List<EntityPidReportDetail> listCombine)
        {
            try
            {
                if (listCombine == null || listCombine.Count < 0)
                    return;

                DateTime dtToday = DateTime.Now;
                //细菌报告管理涂片结果允许自动保存默认值
                if (!string.IsNullOrEmpty(pat_itr_id) && CacheSysConfig.Current.GetSystemConfig("AntiLab_AutoSaveDefValue") == "是")
                {
                    var itrcache = DictInstrmtCache.Current.GetInstructmentByID(pat_itr_id);

                    if (itrcache != null && itrcache.ItrReportType == "3")
                    {
                        List<EntityObrResultDesc> listResultDesc = new ObrResultDescBIZ().GetObrResultDescById(pat_id);
                        if (listResultDesc.Count > 0) return;
                        List<EntityObrResultDesc> listResultDescInsert = new List<EntityObrResultDesc>();
                        int i = 0;
                        foreach (EntityPidReportDetail entityDetail in listCombine)
                        {
                            //组合ID
                            string com_id = entityDetail.ComId;

                            //根据 组合ID和仪器ID 获取具有缺省值的项目
                            List<string> list = GetCombineDefData(pat_itr_id, com_id);

                            if (list.Count == 0) continue;

                            foreach (string defvalue in list)
                            {
                                EntityObrResultDesc entityResultDesc = new EntityObrResultDesc();
                                entityResultDesc.ObrId = pat_id;
                                entityResultDesc.ObrItrId = pat_itr_id;
                                entityResultDesc.ObrDate = dtToday;
                                entityResultDesc.ObrSid = Convert.ToDecimal(pat_sid);
                                entityResultDesc.ObrValue = defvalue;
                                entityResultDesc.SortNo = i;
                                entityResultDesc.ObrPositiveFlag = 0;
                                i++;
                                listResultDescInsert.Add(entityResultDesc);

                            }
                        }

                        if (listResultDescInsert.Count > 0)
                        {
                            //插入描述报告结果
                            new ObrResultDescBIZ().InsertObrResultDesc(listResultDescInsert);
                        }

                        return;
                    }
                }

                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListObrId.Add(pat_id);

                //获取当前病人结果记录
                List<EntityObrResult> listResulto = ObrResultQuery(resultQc);

                List<EntityObrResult> listResultoInsert = new List<EntityObrResult>();

                //遍历当前病人的组合
                foreach (EntityPidReportDetail entityCom in listCombine)
                {
                    //组合ID
                    string com_id = entityCom.ComId;

                    //根据 组合ID和标本 仪器ID 获取具有缺省值的项目
                    List<EntityDicCombineDetail> listItem = new ItemCombineBIZ().GetCombineMiWdthDefault(com_id, pat_sam_id, pat_itr_id);

                    foreach (EntityDicCombineDetail entityItem in listItem)
                    {
                        string itm_id = entityItem.ComItmId;

                        if (listResultoInsert.Where(w => w.ItmId == itm_id).Count() == 0
                            && listResulto.Where(w => w.ItmId == itm_id).Count() == 0
                            )
                        {
                            EntityObrResult entityResult = new EntityObrResult();
                            entityResult.ObrId = pat_id;
                            entityResult.ObrItrId = pat_itr_id;
                            entityResult.ObrSid = pat_sid;
                            entityResult.ItmId = itm_id;
                            entityResult.ItmEname = entityItem.ItmEcode;
                            entityResult.ObrValue = entityItem.ItmDefault;
                            entityResult.ObrDate = dtToday;
                            entityResult.ObrFlag = 1;
                            entityResult.ObrType = 0;
                            entityResult.ItmComId = com_id;
                            entityResult.ItmReportCode = entityItem.ItmRepCode;

                            listResultoInsert.Add(entityResult);
                        }
                    }
                }

                if (listResultoInsert.Count > 0)
                {
                    foreach (EntityObrResult result in listResultoInsert)
                    {
                        InsertObrResult(result);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "InsertDefaultResult", ex.ToString());
                throw;
            }
        }

        public List<string> GetCombineDefData(string itrid, string comID)
        {

            try
            {
                Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();


                List<EntityDicCombineDetail> listDetail = new ItemCombineBIZ().GetCombineDefData(itrid, comID);

                List<EntityDicCombineDetail> listAlls = listDetail.Where(w => w.ItmItrId == itrid).ToList();

                if (listAlls.Count == 0)
                {
                    listAlls = listDetail.Where(w => w.ComId == comID).ToList();
                }

                foreach (EntityDicCombineDetail item in listAlls)
                {
                    string defData = item.ItmDefault;


                    if (defData.StartsWith("[")
                        && defData.EndsWith("]"))
                    {
                        StringBuilder itemDefData = new StringBuilder();
                        string[] codeList = defData.Replace("[", "").Replace("]", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<EntityDicPubEvaluate> listEvaluate = new BscripeBIZ().GetContent();
                        foreach (string code in codeList)
                        {
                            List<EntityDicPubEvaluate> rows = listEvaluate.Where(w => w.EvaId == code).ToList();
                            if (rows.Count > 0 && !string.IsNullOrEmpty(rows[0].EvaContent))
                            {
                                if (itemDefData.Length > 0)
                                {
                                    itemDefData.Append("^|");
                                }

                                itemDefData.Append(rows[0].EvaContent);

                            }

                        }
                        item.ItmDefault = itemDefData.ToString();
                    }
                    else if (defData.StartsWith("^")
                        && defData.EndsWith("^"))
                    {

                        StringBuilder itemDefData = new StringBuilder();
                        string[] codeList = defData.Replace("^", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        List<EntityDicMicSmear> listSmear = new DictNobactBIZ().GetMicSmear();
                        foreach (string code in codeList)
                        {
                            List<EntityDicMicSmear> rows = listSmear.Where(w => w.SmeId == code).ToList();
                            if (rows.Count > 0 && !string.IsNullOrEmpty(rows[0].SmeName))
                            {
                                if (itemDefData.Length > 0)
                                {
                                    itemDefData.Append("^|");
                                }

                                itemDefData.Append(rows[0].SmeName);

                            }

                        }
                        item.ItmDefault = itemDefData.ToString();
                    }
                }


                foreach (EntityDicCombineDetail row in listAlls)
                {

                    string defData = row.ItmDefault;


                    if (!result.ContainsKey(comID + itrid))
                    {
                        result.Add(comID + itrid, new List<string>());
                    }
                    if (defData.Contains("^|"))
                    {
                        string[] defDataList = defData.Split(new string[] { "^|" }, StringSplitOptions.RemoveEmptyEntries);

                        result[comID + itrid].AddRange(defDataList);
                    }
                    else
                    {
                        result[comID + itrid].Add(defData);
                    }

                }

                if (result.ContainsKey(comID + itrid))
                {
                    return result[comID + itrid];
                }
                if (result.ContainsKey(comID + "-1"))
                {
                    return result[comID + "-1"];
                }
                else
                {
                    return new List<string>();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "GetCombineDefData", ex.ToString());

                return new List<string>();
            }
        }

        /// <summary>
        /// 获取相关结果
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityObrResult> GetObrRelateResult(EntityPidReportMain patient)
        {
            //病人id为空时  不允许查询相关结果
            if (string.IsNullOrEmpty(patient.PidInNo))
            {
                return new List<EntityObrResult>();
            }
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            EntityPatientQC patientQc = new EntityPatientQC();
            patientQc.PidIdtId = patient.PidIdtId;
            patientQc.PidInNo = patient.PidInNo;
            patientQc.RepStatus = patient.RepStatus.ToString();
            patientQc.RepId = patient.RepId;
            patientQc.NotInRepId = true;
            patientQc.NotInRepStatus = true;
            patientQc.DateStart = patient.StartDate;
            patientQc.DateEnd = patient.EndDate;
            //获取本次检验以外,id类型和id相同的病人
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            listPat = new PidReportMainBIZ().PatientQuery(patientQc);
            List<string> listObrId = new List<string>();
            foreach (EntityPidReportMain pat in listPat)
            {
                listObrId.Add(pat.RepId);
            }
            EntityResultQC resultQc = new EntityResultQC();
            resultQc.ListObrId = listObrId;
            //获取相关结果
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                listResult = resultDao.ObrResultQuery(resultQc);
            }
            return listResult;
        }

        public List<EntityObrResult> ObrResultQueryByObrId(string obrId, bool withHistoryResult = false)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            if (!string.IsNullOrEmpty(obrId))
            {
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListObrId.Add(obrId);

                listResult = ObrResultQuery(resultQc, withHistoryResult);
            }
            return listResult;
        }
        public List<EntityObrResult> ObrResultQuery(EntityResultQC resultQc, bool withHistoryResult = false)
        {
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            if (resultQc != null)
            {
                IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                if (resultDao != null)
                {
                    listResult = resultDao.ObrResultQuery(resultQc, withHistoryResult);
                    List<EntityDicCombineDetail> listCombDetail = DictCombineMiCache2.Current.DclCache;
                    List<EntityPidReportDetail> listRepDetail = new List<EntityPidReportDetail>();
                    if (listResult.Count > 0)
                    {
                        listRepDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(listResult[0].ObrId);
                    }
                    foreach (EntityObrResult result in listResult)
                    {
                        if (!string.IsNullOrEmpty(result.ObrItrId))
                        {
                            List<EntityDicInstrument> listInstrmt = new InstrmtBIZ().GetInstrumentByItridOrItrType(result.ObrItrId);
                            if (listInstrmt.Count > 0)
                                result.ItrEname = listInstrmt[0].ItrEname;
                        }

                        //查询项目是否必录
                        int combDetailIndex = listCombDetail.FindIndex(i => i.ComId == result.ItmComId && i.ComItmId == result.ItmId);
                        if (combDetailIndex > -1)
                        {
                            if (resultQc.IsNullComPrtFlag && listCombDetail[combDetailIndex].ComPrintFlag == 0)
                            {
                                result.NeedDelete = true;
                            }
                            result.IsNotEmpty = listCombDetail[combDetailIndex].ComMustItem;
                            result.ComMiSort = listCombDetail[combDetailIndex].ComSortNo;
                        }

                        if (result.ResComSeq == null)
                        {

                            int repDetailIndex = listRepDetail.FindIndex(i => i.ComId == result.ItmComId);
                            if (repDetailIndex > -1)
                            {
                                if (listRepDetail[repDetailIndex].SortNo != null)
                                {
                                    result.ResComSeq = listRepDetail[repDetailIndex].SortNo;
                                }
                            }
                            else
                            {
                                result.ResComSeq = 99999;
                            }
                        }

                    }
                    listResult = listResult.OrderBy(i => i.ResComSeq).ThenBy(i => i.ItmSeq).ThenBy(i => i.ItmEname).ToList();
                }
            }

            return listResult;
        }

        public bool UpdateObrResult(EntityObrResult ObrResult)
        {
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao == null)
            {
                Lib.LogManager.Logger.LogInfo("查找不到此Dao");
                return false;
            }
            else
            {
                try
                {
                    //如果结果为空，则提示标志改为-1
                    if (ObrResult.ObrValue != null && ObrResult.ObrValue.Trim() == string.Empty)
                    {
                        ObrResult.RefFlag = "-1";
                    }
                    bool listResult = dao.UpdateObrResult(ObrResult);
                    return listResult;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    return false;
                }
            }
        }

        public bool UpdateResultValueByObrSn(EntityObrResult ObrResult)
        {
            bool result = false;
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                result = resultDao.UpdateResultVauleByObrSn(ObrResult.ObrValue, ObrResult.ObrConvertValue?.ToString(), ObrResult.ObrSn.ToString());
            }
            return result;
        }

        public bool DeleteObrResultByObrSn(string obrSn)
        {
            bool result = false;
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                result = resultDao.DeleteObrResultByObrSn(obrSn);
            }
            return result;
        }
        public List<EntityObrResult> GetCDRHistoryObrResult(string obrId, int resultCount, DateTime? obrDate, bool containThisTime = false)
        {
            List<EntityObrResult> listHistoryResult = new List<EntityObrResult>();
            if (!string.IsNullOrEmpty(obrId))
            {
                EntityPidReportMain patient = new PidReportMainBIZ().GetPatientByPatId(obrId, false);
                if (patient != null)
                {
                    List<string> listObrId = new List<string>();
                    EntityPatientQC patientQc = new EntityPatientQC();

                    #region 填充查询历史RepId的EntityPatientQC实体
                    patientQc.DateStart = new DateTime(2001, 01, 01);
                    if (obrDate != null)
                        patientQc.DateEnd = obrDate;
                    else
                        patientQc.DateEnd = DateTime.Now;


                    if (!Compare.IsNullOrDBNull(patient.RepItrId))
                    {
                        patientQc.ListItrId.Add(patient.RepItrId);
                    }
                    string pat_depcode = string.Empty;

                    //注释，不需要过滤科室
                    //if (!Compare.IsNullOrDBNull(patient.PidDeptId))
                    //{
                    //    patientQc.DepId = patient.PidDeptId;
                    //}
                    if (!Compare.IsNullOrDBNull(patient.PidSamId))
                    {
                        patientQc.SamId = patient.PidSamId;
                    }

                    //if (!Compare.IsNullOrDBNull(patient.PidIdtId))
                    //{
                    //    patientQc.PidIdtId = patient.PidIdtId;
                    //}

                    if (!Compare.IsNullOrDBNull(patient.PidInNo))
                    {
                        patientQc.PidInNo = patient.PidInNo;
                    }
                    #region //获取历史结果自定义查询列  2010-6-18 未修改完成Lwj

                    string historySelectColumn = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");// Lis.Server.Cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");
                    if (string.IsNullOrEmpty(historySelectColumn))
                        historySelectColumn = "PidInNo";

                    if (historySelectColumn.Contains("PidInNo"))
                    {
                        patientQc.historySelectColumn = HistorySelectColumn.病人ID;
                    }
                    #endregion

                    if (!Compare.IsNullOrDBNull(patient.PidName))
                    {
                        patientQc.PidName = patient.PidName;
                    }

                    //不限仪器ID查询历史结果
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryWithItr") == "否")
                    {
                        patientQc.ListItrId = new List<string>();
                    }

                    if (resultCount < 0)
                    {
                        resultCount = 0;
                    }
                    //历史结果关联项目专业组
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryWithPType") == "是")
                    {
                        //  List<string> listItr = new InstrmtBIZ().GetHistoryReletedInstrumentByRepId(obrId).Select(i => i.ItrId).ToList();
                        //  if (listItr != null && listItr.Count > 0)
                        //{
                        //    patientQc.ListItrId = listItr;
                        //}
                    }

                    //历史结果只显示审核后的结果
                    //if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryNeedAudit") == "是")
                    //{
                    //    patientQc.RepStatus = "2,4";
                    //}

                    //如果开启只按姓名与性别查询病人结果，按选择列查询功能无效。
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_ResultHistoryContrainName") == "是")
                    {
                        patientQc.PidInNo = string.Empty;
                    }

                    patientQc.NotInRepId = true;
                    if (resultCount == 3 || resultCount == 10)
                    {
                        patientQc.RepId = obrId;
                    }
                    else
                    {
                        patientQc.RepId = "0";
                    }


                    #endregion

                    if (//patient.PidName != string.Empty&& 
                        patient.RepItrId != string.Empty
                     && patient.PidSamId != string.Empty
                         //&& pat_in_no != string.Empty // 2010-6-23 不判断pat_in_no,因为历史结果有可能不以此字段作查询                            )
                         )
                    {
                        List<EntityPidReportMain> listPatient = new List<EntityPidReportMain>();
                        if (!string.IsNullOrEmpty(patientQc.PidInNo))
                        {
                            IDaoCdrHistroyResult resultDao = DclDaoFactory.DaoHandler<IDaoCdrHistroyResult>();
                            if (resultDao != null)
                            {
                                listPatient = resultDao.GetCdrHistroyPatients(patientQc).OrderByDescending(i => i.RepInDate).ToList();
                            }
                        }
                        else if (containThisTime)
                            listPatient.Add(patient);
                        //取前三个历史结果
                        if (listPatient.Count > 0)
                        {
                            for (int i = 0; i < listPatient.Count; i++)
                            {
                                if (i > resultCount - 1)
                                {
                                    break;
                                }
                                EntityPidReportMain pat = listPatient[i];
                                listObrId.Add(pat.RepId);

                            }
                        }

                        #region 获取历史数据库数据 还没写
                        //获取历史数据库数据 HB 2013-09-02
                        bool enableRead = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB") == "是";
                        string lisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
                        #endregion

                        if (listObrId.Count > 0)
                        {
                            IDaoCdrHistroyResult resultDao = DclDaoFactory.DaoHandler<IDaoCdrHistroyResult>();
                            if (resultDao != null)
                            {
                                listHistoryResult = resultDao.GetCdrHistoryObrResult(listObrId).OrderByDescending(i => i.ObrDate).ToList();
                            }
                            //把历史结果的日期修改为病人的报告日期，如果报告日期为空，则修改为录入日期
                            foreach (EntityObrResult obrResult in listHistoryResult)
                            {
                                EntityPidReportMain ownPatient = listPatient.Find(i => i.RepId == obrResult.ObrId);
                                if (ownPatient.RepReportDate != null)
                                    obrResult.ObrDate = ownPatient.RepReportDate.Value;
                                else
                                    obrResult.ObrDate = ownPatient.RepInDate.Value;
                            }
                        }

                    }
                }
            }
            return listHistoryResult;
        }

        public List<EntityObrResult> GetHistoryObrResult(string obrId, int resultCount, DateTime? obrDate, bool containThisTime = false)
        {
            List<EntityObrResult> listHistoryResult = new List<EntityObrResult>();
            if (!string.IsNullOrEmpty(obrId))
            {
                EntityPidReportMain patient = new PidReportMainBIZ().GetPatientByPatId(obrId, false);
                if (patient != null)
                {
                    //病人id为空时不查询历史结果
                    if (string.IsNullOrEmpty(patient.PidInNo))
                        return listHistoryResult;
                    EntityResultQC resultQc = new EntityResultQC();
                    resultQc.ObrFlag = "1";
                    EntityHistoryPatientQC patientQc = new EntityHistoryPatientQC();

                    #region 填充查询历史RepId的EntityPatientQC实体
                    //patientQc.DateStart = new DateTime(2001, 01, 01);
                    //if (obrDate != null)
                    //    patientQc.DateEnd = patient.RepInDate;
                    //else
                    //    patientQc.DateEnd = DateTime.Now;
                    if (obrDate != null)
                        patientQc.RepInDate = patient.RepInDate;
                    else
                        patientQc.RepInDate = DateTime.Now;
                    if (!Compare.IsNullOrDBNull(patient.RepItrId))
                    {
                        patientQc.ListItrId.Add(patient.RepItrId);
                    }
                    string pat_depcode = string.Empty;

                    //注释，不需要过滤科室
                    //if (!Compare.IsNullOrDBNull(patient.PidDeptId))
                    //{
                    //    patientQc.DepId = patient.PidDeptId;
                    //}
                    //历史结果关联标本
                    List<string> listSamId = new List<string>();
                    string historySamp = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryWithSam");
                    if (!string.IsNullOrEmpty(historySamp))
                    {
                        string[] historySamps = historySamp.Split(new char[] { '&' });
                        if (historySamps.Length > 0)
                        {
                            foreach (string var in historySamps)
                            {
                                if (var.Contains(patient.PidSamId))
                                {
                                    string[] samIds = var.Split(',');
                                    if (samIds.Length > 0)
                                    {
                                        foreach (string sam in samIds)
                                        {
                                            string temp = sam.Replace("<", "").Replace(">", "");
                                            listSamId.Add(temp);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    listSamId = listSamId.Distinct().ToList();
                    if (listSamId.Count > 0)
                    {
                        patientQc.listSamId = listSamId;
                    }
                    else {
                        if (!Compare.IsNullOrDBNull(patient.PidSamId))
                        {
                            patientQc.listSamId.Add(patient.PidSamId);
                        }
                    }

                    //if (!Compare.IsNullOrDBNull(patient.PidIdtId))
                    //{
                    //    patientQc.PidIdtId = patient.PidIdtId;
                    //}

                    if (!Compare.IsNullOrDBNull(patient.PidInNo))
                    {
                        patientQc.PidInNo = patient.PidInNo;
                    }

                    #region //获取历史结果自定义查询列  2010-6-18 未修改完成Lwj

                    string historySelectColumn = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");// Lis.Server.Cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");
                    if (string.IsNullOrEmpty(historySelectColumn))
                        historySelectColumn = "PidInNo";

                    if (historySelectColumn.Contains("PidInNo"))
                    {
                        //patientQc.historySelectColumn = HistorySelectColumn.病人ID;
                    }
                    #endregion

                    if (!Compare.IsNullOrDBNull(patient.PidName))
                    {
                        patientQc.PidName = patient.PidName;
                    }

                    //不限仪器ID查询历史结果
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryWithItr") == "否")
                    {
                        patientQc.ListItrId = new List<string>();
                    }

                    if (resultCount < 0)
                    {
                        resultCount = 0;
                    }
                    //历史结果关联项目专业组
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryWithPType") == "是")
                    {
                        List<string> listItr = new InstrmtBIZ().GetHistoryReletedInstrumentByRepId(obrId).Select(i => i.ItrId).ToList();
                        if (listItr != null && listItr.Count > 0)
                        {
                            patientQc.ListItrId = listItr;
                        }
                    }

                    //历史结果只显示审核后的结果
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryNeedAudit") == "是")
                    {
                        patientQc.RepStatus = "2,4";
                    }

                    //如果开启只按姓名与性别查询病人结果，按选择列查询功能无效。
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_ResultHistoryContrainName") == "是")
                    {
                        patientQc.PidInNo = string.Empty;
                    }

                    //  patientQc.NotInRepId = true;
                    if (resultCount == 3 || resultCount == 10)
                    {
                        patientQc.RepId = obrId;
                    }
                    else
                    {
                        patientQc.RepId = "0";
                    }

                    patientQc.ResultCount = resultCount;
                    #endregion

                    if (//patient.PidName != string.Empty&& 
                        patient.RepItrId != string.Empty
                     && patient.PidSamId != string.Empty
                         //&& pat_in_no != string.Empty // 2010-6-23 不判断pat_in_no,因为历史结果有可能不以此字段作查询                            )
                         )
                    {
                        #region 获取历史数据库数据 还没写
                        //获取历史数据库数据 HB 2013-09-02
                        bool enableRead = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB") == "是";
                        string lisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
                        //ReadHistoryDelegate readerDg = null;
                        //IAsyncResult iResult = null;
                        //if (!string.IsNullOrEmpty(lisHistoryConnectionString) && enableRead)
                        //{
                        //    readerDg = GetPatHistoryResultFromHistoryDataBase;
                        //    iResult = readerDg.BeginInvoke(sql, dtPat_date, lisHistoryConnectionString, null, null);
                        //}

                        //if (readerDg != null)
                        //{
                        //    DataTable lisHistoryResData = readerDg.EndInvoke(iResult);
                        //    if (lisHistoryResData != null && lisHistoryResData.Rows.Count > 0)
                        //    {
                        //        dt.Merge(lisHistoryResData);
                        //    }
                        //}
                        #endregion

                        listHistoryResult = GetHistoryObrResult(patientQc).OrderBy(i => i.ObrDate).ToList();

                    }
                }
            }
            return listHistoryResult;
        }
        /// <summary>
        /// 根据参数查询历史结果
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        private List<EntityObrResult> GetHistoryObrResult(EntityHistoryPatientQC qc)
        {
            List<EntityObrResult> listObrResult = new List<EntityObrResult>();
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                listObrResult = resultDao.GetResultHistory(qc);
            }
            return listObrResult;
        }

        #region 常规检验保存方法
        /// <summary>
        /// 更新普通报告 、病人信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public EntityOperationResult UpdatePatCommonResult(EntityRemoteCallClientInfo userInfo, EntityQcResultList resultList)
        {
            //创建操作返回信息
            EntityOperationResult result = new EntityOperationResult();//.GetNew("更新病人、结果信息");
            //现病人基本信息
            EntityPidReportMain patient = resultList.patient;

            string pat_id = patient.RepId;

            //如果pat_age=-1 而pat_age_exp不为空则进行更新
            //如果pat_age = -1 而pat_age_exp不为空则进行更新
            if ((patient.PidAge.ToString() == null || patient.PidAge.ToString() == "-1")
                && (patient.PidAgeExp != null && !string.IsNullOrEmpty(patient.PidAgeExp)))
            {
                try
                {
                    patient.PidAge = AgeConverter.AgeValueTextToMinute(patient.PidAgeExp);
                }
                catch
                {
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", patient.RepId, patient.PidAgeExp));

                }

            }

            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(userInfo.LoginID, userInfo.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

            //现组合
            List<EntityPidReportDetail> listPatCombine = resultList.listRepDetail;

            new PidReportMainBIZ().CreatePatCName(patient, listPatCombine);

            //现结果
            List<EntityObrResult> listPatResult = resultList.listResulto;

            //描述结果
            List<EntityObrResultDesc> listPatDesResult = resultList.listDesc;

            //原病人基本资料信息
            EntityPidReportMain OriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id);

            if (OriginPatInfo != null && OriginPatInfo.RepStatus.ToString() == "2")
            {
                result.AddMessage(EnumOperationErrorCode.Exception, "该记录已报告", EnumOperationErrorLevel.Error);
                return result;
            }

            //原组合信息
            List<EntityPidReportDetail> listOriginPatCombine = OriginPatInfo.ListPidReportDetail; //new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);

            //原病人结果
            EntityResultQC resultQc = new EntityResultQC();
            resultQc.ListObrId.Add(pat_id);
            List<EntityObrResult> listOriginPatResult = ObrResultQuery(resultQc);

            //获取病人组合条码号**********************************************************
            string pat_bar_code = null;
            if (!common.Compare.IsEmpty(patient.RepBarCode))
            {
                pat_bar_code = patient.RepBarCode;
            }

            //****************************************************************

            try
            {
                bool changePatID = false;
                string newPatId = "";

                //更新病人基本信息前的一些处理
                new PidReportMainBIZ().UpdatePatientInfoBefore(patient, OriginPatInfo, result, opLogger, out changePatID, out newPatId);

                List<EntityPidReportMain> listPatient = new List<EntityPidReportMain>();
                listPatient.Add(patient);

                //样本号已存在则不更新病人信息
                if (result.Success)
                {
                    //更新病人资料
                    new PidReportMainBIZ().UpdatePatientData(listPatient);
                }

                #region 将修改病人信息的操作插入Samp_ process_detial表

                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = userInfo.Time;
                sampProcessDetial.ProcUsercode = userInfo.LoginID;
                sampProcessDetial.ProcUsername = userInfo.LoginName;
                sampProcessDetial.ProcStatus = "35";
                sampProcessDetial.ProcBarno = pat_bar_code;
                sampProcessDetial.ProcBarcode = pat_bar_code;
                sampProcessDetial.RepId = patient.RepId;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new PidReportDetailBIZ().UpdatePatientCombineBefore(listPatCombine, listOriginPatCombine, pat_bar_code, result, opLogger, changePatID, resultList);

                //插入病人组合
                if (listPatCombine.Count > 0)
                {
                    new PidReportDetailBIZ().InsertNewReportDetail(listPatCombine);
                }

                //更新普通结果
                if (listPatResult.Count > 0)
                {
                    UpdatePatientResult(listPatResult, listOriginPatResult, result, opLogger, resultList);
                }

                //更新描述结果
                if (listPatDesResult != null && listPatDesResult.Count > 0)
                {
                    List<EntityObrResultDesc> listResultDesc = new ObrResultDescBIZ().GetObrResultDescById(pat_id);
                    //存在描述结果则更新 否则插入
                    if (listResultDesc.Count > 0)
                    {
                        new ObrResultDescBIZ().UpdateObrResultDesc(listPatDesResult[0]);
                    }
                    else
                    {
                        new ObrResultDescBIZ().InsertObrResultDesc(listPatDesResult);
                    }
                }


                if (opLogger.logs.Count > 0)
                {
                    string patID = pat_id;
                    if (changePatID)
                    {
                        patID = newPatId;
                        patient.RepId = newPatId;
                    }
                }

                opLogger.Log();


                //************************************************************************************
                //如果原组合与现在的组合比对少了的组合一律将bc_flag置为0
                if (!string.IsNullOrEmpty(pat_bar_code))
                {
                    Thread th = new Thread(UpdateSampFlag);
                    th.Start(pat_bar_code);
                }

                //*************************************************************************************

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("UpdatePatCommonResult", ex);
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }



        public bool SaveOrignObrResult(EntityPidReportMain pat)
        {
            //仪器中间表数据，需要校验结果，然后保存到数据库
            IDaoResultOriginal dao = DclDaoFactory.DaoHandler<IDaoResultOriginal>();
            //DateTime repInDate = (DateTime)pat.RepInDate;
            List<EntityObrResultOriginalEx> itrOrignList = dao.GetOrignObrResult(pat);
            if (itrOrignList == null || itrOrignList.Count <= 0)
                return false;


            bool flag = false;
            //DeleteObrResultByObrId(pat.RepId);
            //foreach (EntityObrResultOriginalEx original in itrOrignList)
            //{
            //    EntityObrResult result = new EntityObrResult();
            //    result.ObrId = pat.RepId;
            //    result.ObrItrId = pat.RepItrId;
            //    result.ObrSid = pat.RepSid;
            //    GenObrResult(result, original);
            //    flag = InsertObrResult(result);
            //}


            //获取已经保存在数据库的结果
            EntityResultQC qc = new EntityResultQC();
            qc.ListObrId.Add(pat.RepId);
            List<EntityObrResult> exitResult = ObrResultQuery(qc);

            foreach (EntityObrResultOriginalEx original in itrOrignList)
            {
                var exitItems = exitResult.FirstOrDefault(i => i.ItmId == original.ItmId);

                //var exitItems = exitResult.FirstOrDefault(i => i.ItmId == original.ItmId
                //         && i.ObrSid == pat.RepSid);

                if (exitItems != null)
                {
                    GenObrResult(exitItems, original);
                    flag = UpdateObrResult(exitItems);
                }
                else
                {
                    EntityObrResult result = new EntityObrResult();
                    result.ObrId = pat.RepId;
                    result.ObrItrId = pat.RepItrId;
                    result.ObrSid = pat.RepSid;
                    GenObrResult(result, original);
                    flag = InsertObrResult(result);
                }
            }
            return flag;
        }

        /// <summary>
        /// 插入普通报告 、病人信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public EntityOperationResult InsertPatCommonResult(EntityRemoteCallClientInfo caller, EntityQcResultList resultList)
        {
            EntityOperationResult opResult = new EntityOperationResult();//.GetNew("保存病人、普通结果信息");
            //有权限操作保存结果的则可以不对比历史结果
            if (caller.OperationName.Trim() != string.Empty && !Convert.ToBoolean(caller.OperationName))
            {
                #region 对比历史结果
                dcl.svr.resultcheck.Checker.CheckerHistoryResultCompare checkHistoryRes = new dcl.svr.resultcheck.Checker.CheckerHistoryResultCompare(null, null, EnumOperationCode.Unspecified, null);
                //if (!checkHistoryRes.Savecheck(dtPatInfo, dtPatResult, ref opResult))
                //{
                //    return opResult;
                //}
                #endregion
            }

            try
            {
                //插入病人信息和病人组合
                EntityOperateResult operateResult = new PidReportMainBIZ().SavePatient(caller, resultList.patient);
                if (operateResult.Success)
                {
                    //operateResult.Data.Patient = resultList.patient;

                    //仪器中间表数据，需要校验结果，然后保存到数据库
                    IDaoResultOriginal dao = DclDaoFactory.DaoHandler<IDaoResultOriginal>();
                    DateTime repInDate = (DateTime)resultList.patient.RepInDate;
                    List<EntityObrResultOriginalEx> itrOrignList = dao.GetOrignObrResult(resultList.patient);


                    //获取已经保存在数据库的结果
                    EntityResultQC qc = new EntityResultQC();
                    qc.ListObrId.Add(resultList.patient.RepId);
                    List<EntityObrResult> exitResult = ObrResultQuery(qc);

                    //首先处理拥有缺省结果，且已经保存到数据库的检验结果
                    foreach (EntityObrResult result in resultList.listResulto)
                    {
                        result.ObrId = resultList.patient.RepId;

                        var items = exitResult.FirstOrDefault(i => i.ItmId == result.ItmId
                                && i.ObrSid == resultList.patient.RepSid
                                && i.ObrId == resultList.patient.RepId);

                        //如果数据库有仪器结果 则更新结果不为空且不是仪器结果的数据
                        if (items != null)
                        {
                            //if (string.IsNullOrEmpty(result.ObrValue))
                            //    continue;

                            //if (result.ObrType.ToString() == "1")//当前项目结果或为仪器结果
                            //    continue;

                            result.ObrSn = items.ObrSn;
                            if (itrOrignList != null && itrOrignList.Count > 0)
                            {
                                var itrOrign = itrOrignList.FirstOrDefault(i => i.ItmId == result.ItmId);
                                if (itrOrign != null)
                                {

                                    GenObrResult(result, itrOrign);
                                }
                            }
                            UpdateObrResult(result);
                        }
                        else
                        {
                            if (itrOrignList != null && itrOrignList.Count > 0)
                            {
                                var itrOrign = itrOrignList.FirstOrDefault(i => i.ItmId == result.ItmId);
                                if (itrOrign != null)
                                {
                                    GenObrResult(result, itrOrign);
                                }
                            }
                            InsertObrResult(result);
                        }
                    }


                    //然后执行没有保存过到数据库的检验结果
                    if (itrOrignList != null && itrOrignList.Count > 0 && resultList.listResultoNoFliter != null)
                    {
                        foreach (EntityObrResult resultNoFliter in resultList.listResultoNoFliter)
                        {
                            var items = resultList.listResulto.FirstOrDefault(i => i.ItmId == resultNoFliter.ItmId);
                            if (items != null)
                                continue;//在上面已经执行过的，跳过不执行

                            var itrOrign = itrOrignList.FirstOrDefault(i => i.ItmId == resultNoFliter.ItmId);
                            if (itrOrign == null)
                                continue;

                            EntityObrResult result = new EntityObrResult();
                            result.ObrId = resultList.patient.RepId;
                            result.ObrItrId = resultList.patient.RepItrId;
                            result.ObrSid = resultList.patient.RepSid;
                            GenObrResult(result, itrOrign);
                            InsertObrResult(result);
                        }
                    }


                    if (resultList.listDesc.Count > 0)
                    {
                        //插入病人描述结果
                        foreach (EntityObrResultDesc result in resultList.listDesc)
                        {
                            result.ObrId = resultList.patient.RepId;
                        }
                        new ObrResultDescBIZ().InsertObrResultDesc(resultList.listDesc);
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SavePatCommonResult", ex);
                opResult.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return opResult;
        }


        private void GenObrResult(EntityObrResult saveResult, EntityObrResultOriginalEx resultOriginal)
        {
            saveResult.ItmEname = resultOriginal.ItmEname;
            saveResult.ObrValue = resultOriginal.ObrValue;
            saveResult.ObrValue2 = resultOriginal.ObrValue2;
            saveResult.ObrValue3 = resultOriginal.ObrValue3;
            saveResult.ObrType = 1;
            saveResult.ObrFlag = 1;
            saveResult.ItmId = resultOriginal.ItmId;
            saveResult.ObrRemark = resultOriginal.ObrRemark;
            saveResult.ObrSourceItrId = resultOriginal.ObrSourceItrId;
            saveResult.ObrUnit = resultOriginal.ObrUnit;
        }

        #endregion
        /// <summary>
        /// 更新标志
        /// </summary>
        /// <param name="pat_bar_code"></param>
        private void UpdateSampFlag(object pat_bar_code)
        {
            new SampDetailBIZ().UpdateSampFlagByBarCode(pat_bar_code.ToString());
        }

        /// <summary>
        /// 更新病人结果
        /// </summary>
        /// <param name="listPatientResult"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        private void UpdatePatientResult(List<EntityObrResult> listPatientResult, List<EntityObrResult> listOriginPatResult, EntityOperationResult result, OperationLogger opLogger, EntityQcResultList resultList)
        {
            if (result.Success)
            {
                EntityPidReportMain patient = new EntityPidReportMain();
                if (resultList.patient != null)
                {
                    patient = resultList.patient;
                }
                //样本号
                string pat_sid = patient.RepSid;

                //病人ID
                string pat_id = patient.RepId;

                //仪器ID
                string pat_itr_id = patient.RepItrId;

                //样本类型ID
                string pat_sam_id = patient.PidSamId;

                DateTime today = ServerDateTime.GetDatabaseServerDateTime();

                //bool isVerify = ResultExistVerify();
                //if (isVerify && !listPatientResult.Columns.Contains("res_verify"))
                //    listPatientResult.Columns.Add("res_verify");

                #region 插入新结果信息
                //获取需要新增的行
                List<EntityObrResult> listAddNew = listPatientResult.Where(w => w.IsNew == 1).ToList();
                if (listAddNew.Count > 0)
                {
                    foreach (EntityObrResult entityInsert in listAddNew)
                    {
                        //更新前判断该项目结果是否已经由他人录入，已录入则默认调到修改状态
                        string itmecd = SQLFormater.Format(entityInsert.ItmEname);
                        List<EntityObrResult> listOrigin = listOriginPatResult.Where(w => w.ItmEname == itmecd).ToList();

                        if (listOrigin.Count > 0)
                        {
                            entityInsert.IsNew = 0;
                            continue;
                        }

                        if (dcl.common.Compare.IsEmpty(entityInsert.ObrDate))
                        {
                            entityInsert.ObrDate = today;
                        }
                        entityInsert.ObrId = pat_id;
                        entityInsert.ObrSid = pat_sid;
                        entityInsert.ObrDate = today;
                        entityInsert.ObrItrId = pat_itr_id;
                        entityInsert.ObrFlag = 1;
                        entityInsert.ObrReportType = 0;
                        opLogger.Add_AddLog(SysOperationLogGroup.PAT_RESULT, entityInsert.ItmEname, entityInsert.ObrValue);
                    }
                    listAddNew = listPatientResult.Where(w => w.IsNew == 1).ToList();
                }
                #endregion

                #region 更新原结果信息
                //获取需要更新的行
                List<EntityObrResult> listUpdate = listPatientResult.Where(w => w.IsNew == 0).ToList();

                if (listUpdate.Count > 0)
                {

                    //获取原有结果记录

                    foreach (EntityObrResult entityCurrResult in listUpdate)
                    {
                        if (dcl.common.Compare.IsEmpty(entityCurrResult.ObrDate))
                        {
                            entityCurrResult.ObrDate = today;
                        }

                        entityCurrResult.ObrSid = pat_sid;
                        entityCurrResult.ObrItrId = pat_itr_id;
                        //if (isVerify)
                        //    entityCurrResult["res_verify"] = Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(entityCurrResult["res_itm_id"].ToString() + ";" + entityCurrResult["res_chr"].ToString());
                        string itmecd = SQLFormater.Format(entityCurrResult.ItmEname);
                        List<EntityObrResult> listOrigin = listOriginPatResult.Where(w => w.ItmEname == itmecd).ToList();
                        if (listOrigin.Count > 0)
                        {
                            EntityObrResult entityOrigin = listOrigin[0];
                            if (entityCurrResult.ObrSn.ToString() == null)
                            {
                                entityCurrResult.ObrSn = entityOrigin.ObrSn;
                            }
                            //查找普通结果是否有更改
                            if (!ObjectEquals(entityOrigin.ObrValue, entityCurrResult.ObrValue))
                            {
                                string currValue = string.Empty;
                                string oldValue = string.Empty;

                                if (!string.IsNullOrEmpty(entityCurrResult.ObrValue))
                                {
                                    currValue = entityCurrResult.ObrValue;
                                }
                                if (entityOrigin.ObrValue != null)
                                {
                                    oldValue = entityOrigin.ObrValue;
                                }
                                entityCurrResult.ObrDate = today;
                                //****************************************************
                                //将修改过的项目的结果类型置为手工
                                entityCurrResult.ObrType = 0;
                                //****************************************************

                                opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itmecd, oldValue + "→" + currValue);
                            }

                            //查找od结果是否有更改
                            if (!ObjectEquals(entityOrigin.ObrValue2, entityCurrResult.ObrValue2))
                            {
                                string currValue = string.Empty;
                                string oldODValue = string.Empty;

                                if (!string.IsNullOrEmpty(entityCurrResult.ObrValue2))
                                {
                                    currValue = entityCurrResult.ObrValue2;
                                }
                                if (!string.IsNullOrEmpty(entityOrigin.ObrValue2))
                                {
                                    oldODValue = entityOrigin.ObrValue2;
                                }
                                entityCurrResult.ObrDate = today;
                                opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_RESULT, itmecd, oldODValue + "→" + currValue);
                            }
                        }
                    }
                }
                #endregion

                #region 删除结果
                List<EntityObrResult> listDelete = listPatientResult.Where(w => w.IsNew == 2).ToList();
                if (listDelete.Count > 0)
                {
                    foreach (EntityObrResult entityDel in listDelete)
                    {
                        string res_itm_id = entityDel.ItmId;
                        string res_itm_ecd = entityDel.ItmEname;

                        Int64 reskey = Convert.ToInt64(entityDel.ObrSn);

                        if (!string.IsNullOrEmpty(entityDel.ItmId) && entityDel.ItmId.ToString().Trim(null) != string.Empty)
                        {
                            //在原结果表中查找被删除项目的结果值
                            List<EntityObrResult> listOriginExistedItem = listOriginPatResult.Where(w => w.ItmId == res_itm_id).ToList();
                            if (listOriginExistedItem.Count > 0)
                            {
                                string originValue = string.Empty;
                                if (!string.IsNullOrEmpty(listOriginExistedItem[0].ObrValue))
                                {
                                    originValue = listOriginExistedItem[0].ObrValue;
                                }

                                //记录删除日志
                                opLogger.Add_DelLog(SysOperationLogGroup.PAT_RESULT, res_itm_ecd, originValue);
                            }
                        }
                    }
                }
                #endregion

                try
                {
                    //执行sql(事务)
                    if (listUpdate != null && listUpdate.Count > 0)//更新结果
                    {
                        foreach (EntityObrResult obrResult in listUpdate)
                        {
                            UpdateObrResult(obrResult);
                        }
                    }

                    if (listAddNew != null && listAddNew.Count > 0)//新增结果
                    {
                        foreach (EntityObrResult obrResult in listAddNew)
                        {
                            InsertObrResult(obrResult);
                        }
                    }

                    if (listDelete != null && listDelete.Count > 0)//删除结果
                    {
                        DeleteObrResultByObrSn(listDelete[0].ObrSn.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("UpdatePatientResult", ex);
                    result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                }

            }
        }



        private bool ObjectEquals(object obj1, object obj2)
        {
            if (
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 == null || obj2 == DBNull.Value))
                ||
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 != null && obj2 != DBNull.Value && obj2.ToString() == string.Empty))
                ||
                ((obj2 == null || obj2 == DBNull.Value) && (obj1 != null && obj1 != DBNull.Value && obj1.ToString() == string.Empty))
            )
            {
                return true;
            }
            else
            {
                if (obj1.ToString() == obj2.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 根据病人ID删除病人结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        public bool DeleteObrResultByObrId(string obrId)
        {
            bool result = false;
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                result = resultDao.DeleteObrResultByObrId(obrId);
            }
            return result;
        }

        public bool DeleteObrResultByResultQc(EntityResultQC resultQc)
        {
            bool result = false;
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                result = resultDao.DeleteObrResultByQC(resultQc);
            }
            return result;
        }
        /// <summary>
        /// 删除普通病人资料
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="delWithResult"></param>
        /// <returns></returns>
        public EntityOperationResult DelPatCommonResult(EntityRemoteCallClientInfo caller, string rep_id, bool delWithResult, bool canDeleteAudited)
        {
            EntityOperationResult opResult = new EntityOperationResult();
            List<EntityPidReportDetail> listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(rep_id);
            EntityPatientQC qc = new EntityPatientQC();
            qc.RepId = rep_id;
            List<EntityPidReportMain> listPatient = new PidReportMainBIZ().PatientQuery(qc);
            EntityPidReportMain patient = new EntityPidReportMain();
            if (listPatient != null && listPatient.Count > 0)
            {
                patient = listPatient[0];
            }
            List<EntitySysOperationLog> listLog = new List<EntitySysOperationLog>();//用于更新日志
            opResult.Data.Patient.RepId = rep_id;
            bool result = false;
            string strRepStatus = new PidReportMainBIZ().GetPatientState(rep_id);

            if (!string.IsNullOrEmpty(strRepStatus))
            {
                string rep_status = strRepStatus;
                //判断病人状态是否是1,2,4
                if ((rep_status == "1"
                    || rep_status == "2"
                    || rep_status == "4")
                    && !canDeleteAudited)
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
                        result = new PidReportMainBIZ().DeletePatient(rep_id);
                        if (result)
                            result = new PidReportDetailBIZ().DeleteReportDetail(rep_id);
                        #endregion
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
                            if (string.IsNullOrEmpty(patient.RepBarCode))  //如果条码号不存在不更新samp_main 表 只更新Samp_ process_detial
                            {
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
                            //条码号存在则还需要更新samp_main表中的条码状态
                            else
                            {
                                remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                                                                    patient.RepId, patient.PidName, patient.PidComName,
                                                                    patient.PidInNo, patient.ItrName, patient.RepSid);
                                EntitySampOperation operation = new EntitySampOperation();
                                operation.OperationStatus = "530";
                                operation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
                                operation.OperationID = caller.LoginID;
                                operation.OperationIP = caller.IPAddress;
                                operation.Remark = remark;
                                operation.RepId = patient.RepId;
                                operation.OperationName = caller.OperationName;
                                List<EntitySampMain> listSamp = new List<EntitySampMain>();
                                listSamp.Add(sampMain);
                                //更新条码操作状态 以及保存流程操作信息
                                result = new SampMainBIZ().UpdateSampMainStatus(operation, listSamp);
                            }

                        }

                        #endregion

                        #region 删除病人扩展信息
                        if (result)
                        {
                            result = new PidReportMainExtBIZ().DeletePidReportMainExt(rep_id);
                        }
                        #endregion

                        #region 日志实体模板
                        EntitySysOperationLog opLog = new EntitySysOperationLog();
                        opLog.OperatUserId = caller.LoginID;
                        opLog.OperatServername = caller.IPAddress;
                        opLog.OperatModule = "病人资料";
                        opLog.OperatKey = rep_id;
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
                        if (delWithResult && result)
                        {
                            #region 填充删除病人检验结果的日志实体
                            EntityResultQC resultQc = new EntityResultQC();
                            resultQc.ListObrId.Add(rep_id);
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

                            result = new ObrResultBIZ().DeleteObrResultByObrId(rep_id);
                            #region 删除仪器报警信息
                            if (result)
                                result = new InstrmtWardingMsgBIZ().DeleteInstrmtWardMsgByPatItrId(rep_id);
                            #endregion
                            #region 删除图像结果
                            if (result)
                                result = new ObrResultImageBIZ().DeletePatPhotoResultByObrId(rep_id);
                            #endregion
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
                        helper = null;
                    }
                    else
                    {
                        helper.RollbackTrans();
                    }
                }
            }
            return opResult;
        }

        /// <summary>
        /// 获取没有病人的结果  （标本进程）
        /// </summary>
        /// <param name="listItrs"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityObrResult> GetObrResultByNoPat(List<string> listItrs, DateTime startDate, DateTime endDate)
        {
            List<EntityObrResult> list = new List<EntityObrResult>();
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (resultDao != null)
            {
                list = resultDao.GetObrResultByNoPat(listItrs, startDate, endDate);
            }
            return list;
        }

        public List<EntityObrResult> GetObrResultQuery(EntityResultQC resultQc, bool withHistoryResult = false)
        {
            List<EntityObrResult> listObrResult = new List<EntityObrResult>();
            if (resultQc != null)
            {
                IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                if (dao != null)
                {
                    listObrResult = dao.ObrResultQuery(resultQc, withHistoryResult);
                }
            }
            return listObrResult;
        }


        public List<EntityObrResult> LisResultQuery(EntityResultQC resultQc)
        {
            List<EntityObrResult> listObrResult = new List<EntityObrResult>();
            if (resultQc != null)
            {
                IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                if (dao != null)
                {
                    listObrResult = dao.LisResultQuery(resultQc);
                }
            }
            return listObrResult;
        }

        /// <summary>
        /// 更新没有组合id项目结果
        /// </summary>
        /// <param name="ItmComId">组合ID</param>
        /// <param name="ObrId">标识ID</param>
        /// <param name="ItmId">项目ID</param>
        /// <returns></returns>
        public bool UpdateResultComIdByObrIdAndItmID(string ItmComId, string ObrId, string ItmId)
        {
            bool result = false;
            IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            if (dao != null)
            {
                result = dao.UpdateResultComIdByObrIdAndItmID(ItmComId, ObrId, ItmId);
            }
            return result;
        }

        #region 根据实验序号插入实验结果（茂名资料导入）
        /// <summary>
        /// 根据实验序号插入实验结果（茂名资料导入）
        /// </summary>
        /// <param name="Obrs"></param>
        /// <returns></returns>
        public bool SaveobrresultbyTestSeq(List<EntityObrResultTestSeqVer> qcs, out string ErrorMsg)
        {
            ErrorMsg = "";
            //1.先给所有数据匹配报告单记录
            List<EntityObrResultTestSeqVer> newqcs = GetRepIdForObr(qcs);

            if (newqcs.Count == 0)
            {
                ErrorMsg = "未匹配到相应的报告单记录，保存失败！";
                return false;
            }

            try
            {
                IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                foreach (EntityObrResultTestSeqVer qc in newqcs)
                {
                    foreach (EntityObrResult obr in qc.Obrs)
                    {
                        if (obr.ObrSn == 0)
                        {
                            //插入新数据
                            resultDao.InsertObrResult(obr);
                        }
                        else
                        {
                            //更新数据
                            resultDao.UpdateObrResult(obr);
                        }
                    }
                }
                IDaoPidReportMain reportDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                reportDao.UpdateRemarkBySeq(newqcs);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                ErrorMsg = "保存失败！";
                return false;
            }



        }

        /// <summary>
        /// 匹配报告单
        /// </summary>
        /// <param name="obrs"></param>
        private List<EntityObrResultTestSeqVer> GetRepIdForObr(List<EntityObrResultTestSeqVer> qcs)
        {
            List<EntityObrResultTestSeqVer> newqcs = new List<EntityObrResultTestSeqVer>();

            IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();

            DateTime date = ServerDateTime.GetDatabaseServerDateTime();

            foreach (EntityObrResultTestSeqVer qc in qcs)
            {
                //1.查询报告单是否不存在或者已经审核，如满足上述情况过滤掉
                EntityPidReportMain patient = dao.SearchReportByTestSeq(qc.TestSeq);
                if (patient == null || patient.RepStatus >= 1)
                    continue;//不存在报告单或报告已经审核

                //2.查询该报告单已有的检验结果数据，有重合项目，需要操作为更新ObrValue
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ListObrId.Add(patient.RepId);
                List<EntityObrResult> listResulto = resultDao.ObrResultQuery(resultQc);
                foreach (EntityObrResult obr in qc.Obrs)
                {
                    string value = obr.ObrValue;
                    EntityObrResult _obr = listResulto.Find(w => w.ItmId == obr.ItmId);
                    if (_obr != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in obr.GetType().GetProperties())
                        {
                            if (p.CanWrite)
                            {
                                var yy = p.GetValue(_obr, null);
                                p.SetValue(obr, yy, null);
                            }

                        }
                        obr.ObrValue = value;
                    }
                    obr.ObrId = patient.RepId;
                    obr.ObrSid = patient.RepSid;
                    obr.ObrItrId = patient.RepItrId;
                    obr.ObrDate = date;
                    obr.ObrFlag = 1;
                    obr.ObrType = 0;
                    obr.ObrReportType = 0;
                    //obr.ItmComId = "";
                    //obr.ItmReportCode = "";
                    //obr.RefType = "";
                    obr.ObrRecheckFlag = 0;
                }
                qc.RepItrId = patient.RepItrId;
                qc.RepId = patient.RepId;
                //3.将处理完的qc加入newqc
                newqcs.Add(qc);
            }
            return newqcs;
        }
        #endregion
    }
}

