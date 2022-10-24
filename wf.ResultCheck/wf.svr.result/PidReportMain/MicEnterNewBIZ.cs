using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

using dcl.svr.cache;
using dcl.dao.core;
using dcl.svr.sample;
using dcl.root.logon;
using dcl.svr.resultcheck;
using dcl.svr.resultcheck.Updater;

namespace dcl.svr.result
{
    public class MicEnterNewBIZ : IMicEnterNew
    {
        private bool Audit_UploadYss = CacheSysConfig.Current.GetSystemConfig("Audit_UploadYss") == "是";
        private bool Audit_UploadAllPatTypeYss = CacheSysConfig.Current.GetSystemConfig("Audit_UploadAllPatTypeYss") == "是";

        /// <summary>
        /// 根据细菌ID获取抗生素明细信息
        /// </summary>
        /// <param name="bacID"></param>
        /// <returns></returns>
        public List<EntityDicMicAntidetail> GetMicAntidetailList(string bacID)
        {
            List<EntityDicMicAntidetail> listResult = new List<EntityDicMicAntidetail>();

            //获取相关结果
            IMicData micDao = DclDaoFactory.DaoHandler<IMicData>();
            if (micDao != null)
            {
                listResult = micDao.GetMicAntidetailList(bacID);
            }
            return listResult;
        }

        /// <summary>
        /// 病人条件查询
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> MicPatientQuery(EntityPatientQC patientCondition)
        {
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            IMicData mainDao = DclDaoFactory.DaoHandler<IMicData>();
            if (mainDao != null)
            {
                listPat = mainDao.PatientQuery(patientCondition);
            }
            return listPat;
        }

        /// <summary>
        /// 根据病人ID获取细菌报告信息
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        public EntityQcResultList GetMicPatinentData(string repId)
        {
            EntityQcResultList resultList = new EntityQcResultList();

            EntityPidReportMain patient = new PidReportMainBIZ().GetPatientByPatId(repId, true);
            resultList.patient = patient;
            resultList.listAnti = new ObrResultAntiBIZ().GetAntiWithHistoryResultById(repId);
            resultList.listBact = new ObrResultBactBIZ().GetBactResultById(repId);
            resultList.listDesc = new ObrResultDescBIZ().GetDescResultById(repId);
            List<EntityObrResultAnti> listAntiHistroyResult = GetListAntiHistroyResult(patient);
            if (resultList.listAnti != null && resultList.listAnti.Count > 0)
            {
                foreach (EntityObrResultAnti antiResult in resultList.listAnti)
                {
                    if (!string.IsNullOrEmpty(antiResult.ObrAtypeId) &&
                       !string.IsNullOrEmpty(antiResult.ObrAntId))
                    {
                        if (listAntiHistroyResult.Count <= 0) { break; }  //如果最近没有历史报告,则跳出遍历

                        string bt_id = antiResult.ObrAtypeId;
                        string anti_id = antiResult.ObrAntId;
                        List<EntityObrResultAnti> drs = listAntiHistroyResult.FindAll(w => w.ObrAtypeId == bt_id && w.ObrAntId == anti_id);

                        if (drs.Count > 0)
                        {
                            for (int j = 0; j < drs.Count; j++)
                            {
                                if (j == 0)
                                    antiResult.HistoryResult1 = drs[j].ObrValue2;
                                if (j == 1)
                                    antiResult.HistoryResult2 = drs[j].ObrValue2;
                                //drResult["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[j]["anr_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                    }
                }
            }
            return resultList;
        }
        public List<EntityObrResultAnti> GetListAntiHistroyResult(EntityPidReportMain pat)
        {
            List<EntityObrResultAnti> listResult = new List<EntityObrResultAnti>();
            if (pat == null)
                return listResult;
            EntityPatientQC qc = new EntityPatientQC();
            List<string> ListObrId = new List<string>();
            qc.DateStart = new DateTime(2001, 01, 01);
            if (pat.RepInDate != null && !string.IsNullOrEmpty(pat.RepInDate.Value.ToString()))
            {
                qc.DateEnd = pat.RepInDate;
            }
            if (!string.IsNullOrEmpty(pat.RepId))
            {
                qc.RepId = pat.RepId;
            }
            if (!string.IsNullOrEmpty(pat.PidName))
            {
                qc.PidName = pat.PidName;
            }
            if (!string.IsNullOrEmpty(pat.RepItrId))
            {
                qc.ListItrId.Add(pat.RepItrId);
            }
            if (!string.IsNullOrEmpty(pat.PidSamId))
            {
                qc.SamId = pat.PidSamId;
            }
            if (!string.IsNullOrEmpty(pat.PidIdtId))
            {
                qc.PidIdtId = pat.PidIdtId;
            }
            if (!string.IsNullOrEmpty(pat.PidInNo))
            {
                qc.PidInNo = pat.PidInNo;
            }
            qc.NotInRepId = true;
            List<EntityPidReportMain> listPatient = new List<EntityPidReportMain>();
            if (!string.IsNullOrEmpty(qc.PidInNo))
                listPatient = new PidReportMainBIZ().PatientQuery(qc).OrderByDescending(i => i.RepInDate).ToList();
            //取前2个历史结果
            if (listPatient.Count > 0)
            {
                for (int i = 0; i < listPatient.Count; i++)
                {
                    if (i > 1)
                    {
                        break;
                    }
                    EntityPidReportMain patinfo = listPatient[i];
                    ListObrId.Add(patinfo.RepId);

                }
            }
            listResult = new ObrResultAntiBIZ().GetAntiResultByListObrId(ListObrId);
            return listResult;
        }
        public EntityOperationResult UpdateMicPatResult(EntityRemoteCallClientInfo userInfo
            , EntityQcResultList resultList)
        {
            //创建操作返回信息
            EntityOperationResult result = new EntityOperationResult();
            //现病人基本信息
            EntityPidReportMain patient = resultList.patient;

            string pat_id = patient.RepId;
            if ((patient.PidAge.ToString() == null || patient.PidAge.ToString() == "-1")
                && (patient.PidAgeExp != null && !string.IsNullOrEmpty(patient.PidAgeExp)))
            {
                try
                {
                    patient.PidAge = AgeConverter.AgeValueTextToMinute(patient.PidAgeExp);
                }
                catch
                {
                    Logger.WriteException("micInsertBLL", "UpdateMicPatResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", patient.RepId, patient.PidAgeExp));
                }
            }

            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(userInfo.LoginID, userInfo.IPAddress, SysOperationLogModule.PATIENTS, pat_id);

            //现组合
            List<EntityPidReportDetail> listPatCombine = resultList.listRepDetail;

            new PidReportMainBIZ().CreatePatCName(patient, listPatCombine);

            List<EntityObrResultAnti> listAnti = resultList.listAnti;
            List<EntityObrResultBact> listBact = resultList.listBact;
            List<EntityObrResultDesc> listDesc = resultList.listDesc;

            //原病人基本资料信息
            EntityPatientQC qc = new EntityPatientQC();
            qc.RepId = patient.RepId;
            List<EntityPidReportMain> listOriginPatInfo = new PidReportMainBIZ().PatientQuery(qc);
            //原组合信息
            List<EntityPidReportDetail> listOriginPatCombine = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);
            if (listOriginPatInfo.Count > 0 && listOriginPatInfo[0].RepStatus != null &&
                listOriginPatInfo[0].RepStatus >= 1)
            {
                result.AddMessage(EnumOperationErrorCode.Exception, "该记录已审核", EnumOperationErrorLevel.Error);
                return result;
            }
            //  string prevSID = listOriginPatInfo[0].RepSid;
            bool changePatID = false;
            //if (prevSID != patient.RepSid)
            //{
            //    //查找当前样本号是否已存在
            //    bool isExsit = new PidReportMainBIZ().ExsitSid(patient.RepSid, patient.RepItrId, patient.RepInDate.Value);
            //    if (isExsit)
            //    {
            //        //存在,返回错误信息
            //        result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
            //        return result;
            //    }
            //    else
            //    {
            //        changePatID = true;
            //    }
            //}

            //   patient.RepId= patient.RepItrId + patient.RepInDate.Value.ToString("yyyyMMdd") + patient.RepSid;
            //获取病人组合条码号
            string pat_bar_code = null;
            if (!string.IsNullOrEmpty(patient.RepBarCode))
            {
                pat_bar_code = patient.RepBarCode;
            }
            try
            {
                string newPatId = "";
                //更新病人基本信息前的一些处理
                new PidReportMainBIZ().UpdatePatientInfoBefore(patient, listOriginPatInfo[0], result, opLogger, out changePatID, out newPatId);
                List<EntityPidReportMain> listPatient = new List<EntityPidReportMain>();
                listPatient.Add(patient);
                //更新组合前的一些处理
                new PidReportDetailBIZ().UpdatePatientCombineBefore(listPatCombine, listOriginPatCombine, pat_bar_code, result, opLogger, changePatID, resultList);
                if (result.Success)
                {
                    DBManager helper = new DBManager();
                    helper.BeginTrans();
                    try
                    {
                        IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                        mainDao.Dbm = helper;

                        IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                        detailDao.Dbm = helper;

                        //病人资料
                        mainDao.UpdatePatientData(patient);

                        //组合
                        if (detailDao.DeleteReportDetail(patient.RepId))
                        {
                            foreach (EntityPidReportDetail detail in resultList.listRepDetail)
                            {
                                detail.RepId = patient.RepId;
                                detailDao.InsertNewPidReportDetail(detail);
                            }
                        }

                        //保存药敏结果
                        IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
                        if (dao != null)
                        {
                            dao.Dbm = helper;
                            dao.DeleteResultById(patient.RepId);
                            foreach (EntityObrResultAnti anti in listAnti)
                            {
                                anti.ObrId = patient.RepId;
                                dao.SaveResultAnti(anti);
                            }
                        }
                        //保存细菌结果
                        IDaoObrResultBact daobact = DclDaoFactory.DaoHandler<IDaoObrResultBact>();
                        if (daobact != null)
                        {
                            daobact.Dbm = helper;
                            daobact.DeleteResultById(patient.RepId);
                            foreach (EntityObrResultBact bact in listBact)
                            {
                                bact.ObrId = patient.RepId;
                                daobact.SaveResultBact(bact);
                            }
                        }

                        IDaoObrResultDesc daoDesc = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
                        if (daoDesc != null)
                        {
                            daoDesc.Dbm = helper;
                            daoDesc.DeleteResultById(patient.RepId);

                            foreach (EntityObrResultDesc desc in resultList.listDesc)
                            {
                                desc.ObrId = patient.RepId;
                                daoDesc.InsertObrResultDesc(desc);
                            }
                        }
                        helper.CommitTrans();
                        helper = null;
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                        result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                        helper.RollbackTrans();
                        helper = null;
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
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("UpdateMicPatResult", ex);
                result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
            }
            return result;
        }
        /// <summary>
        /// 新增细菌报告信息
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        public EntityOperateResult InsertMicPatResult(EntityRemoteCallClientInfo caller
            , EntityQcResultList resultList)
        {
            EntityOperateResult opResult = new EntityOperateResult();
            try
            {
                var patient = resultList.patient;
                List<EntityPidReportDetail> listPatCombine = resultList.listRepDetail;

                if (!string.IsNullOrEmpty(patient.RepBarCode))
                {
                    //条码号登记判断并发
                    string patId = new PidReportMainBIZ().GetPatientPatId(patient.RepItrId, patient.RepBarCode, patient.RepSid, patient.RepInDate.Value);

                    if (!string.IsNullOrEmpty(patId))
                    {
                        opResult.AddMessage(EnumOperateErrorCode.SIDExist, EnumOperateErrorLevel.Error);
                        return opResult;
                    }
                }

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
                        Logger.WriteException("MICInsertBLL", "InsertMicPatResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", patient.RepId, patient.PidAgeExp));
                    }

                }

                DateTime inDate = ServerDateTime.GetDatabaseServerDateTime();

                opResult.Data.Patient.RepSid = patient.RepSid;
                opResult.Data.Patient.PidName = patient.PidName;

                //DateTime pat_date = Convert.ToDateTime(entityPatient.RepInDate);
                string nowTime = inDate.ToString(" HH:mm:ss");
                if (patient.RepInDate == null)
                    patient.RepInDate = Convert.ToDateTime(inDate.ToString("yyyy-MM-dd") + nowTime);
                string pat_id = patient.RepItrId + patient.RepInDate.Value.ToString("yyyyMMdd") + patient.RepSid;

                patient.RepId = pat_id;
                patient.RepStatus = 0;
                patient.RepModifyFrequency = 0;//修改次数

                //时间计算方式
                string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
                if (Lab_BarcodeTimeCal == "计算签收时间")
                {
                    if (patient.SampApplyDate == null)
                    {
                        DateTime pat_jy_date = (DateTime)patient.SampCheckDate;


                        if (pat_jy_date > inDate)
                        {
                            patient.SampApplyDate = inDate;
                        }
                        else
                        {
                            patient.SampApplyDate = patient.SampCheckDate;
                        }
                    }
                }

                new PidReportMainBIZ().CreatePatCName(patient, listPatCombine);

                if (new PidReportMainBIZ().ExsitSid(patient.RepSid, patient.RepItrId, inDate))
                {
                    opResult.AddMessage(EnumOperateErrorCode.SIDExist, EnumOperateErrorLevel.Error);
                }


                //插入病人信息和病人组合
                //EntityOperateResult operateResult = new PidReportMainBIZ().SavePatient(caller, resultList.patient);

                if (opResult.Success)
                {

                    DBManager helper = new DBManager();
                    helper.BeginTrans();
                    try
                    {
                        IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                        mainDao.Dbm = helper;

                        IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                        detailDao.Dbm = helper;

                        //病人资料
                        mainDao.InsertNewPatient(patient);

                        //组合
                        List<string> listComId = new List<string>();
                        if (detailDao.DeleteReportDetail(patient.RepId))
                        {
                            foreach (EntityPidReportDetail detail in resultList.listRepDetail)
                            {
                                detail.RepId = patient.RepId;
                                listComId.Add(detail.ComId);
                                detailDao.InsertNewPidReportDetail(detail);
                            }
                        }
                        ////允许细菌录入的条码能多次录入
                        if (!string.IsNullOrEmpty(patient.RepBarCode) &&
                            listComId.Count > 0)
                        {
                            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
                            daoDetail.Dbm = helper;
                            daoDetail.UpdateSampDetailSampFlagByComId(patient.RepBarCode, listComId, "1");
                        }


                        foreach (EntityObrResultAnti anti in resultList.listAnti)
                        {
                            anti.ObrId = patient.RepId;
                        }
                        //保存药敏结果
                        new ObrResultAntiBIZ() { Dbm = helper }.SaveAntiResult(resultList.listAnti);
                        foreach (EntityObrResultBact bact in resultList.listBact)
                        {
                            bact.ObrId = patient.RepId;
                        }
                        //保存细菌结果
                        new ObrResultBactBIZ() { Dbm = helper }.SaveResultBact(resultList.listBact);
                        foreach (EntityObrResultDesc desc in resultList.listDesc)
                        {
                            desc.ObrId = patient.RepId;
                        }
                        //保存描述结果
                        new ObrResultDescBIZ() { Dbm = helper }.InsertObrResultDesc(resultList.listDesc);



                        if (!string.IsNullOrEmpty(patient.RepBarCode))
                        {
                            string barcodeRemark = string.Empty;
                            //*************************************************************************************
                            //将序号写入备注中
                            if (!string.IsNullOrEmpty(patient.RepSerialNum))
                            {
                                barcodeRemark = string.Format("仪器：{0}，样本号：{1}, 序号：{2}，登记组合：{3},日期：{4}", patient.ItrName, patient.RepSid, patient.RepSerialNum, patient.PidComName, patient.RepInDate);
                            }
                            else
                            {
                                barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", patient.ItrName, patient.RepSid, patient.PidComName, patient.RepInDate);
                            }

                            EntitySampOperation operation = new EntitySampOperation();
                            operation.OperationStatus = "20";
                            operation.OperationStatusName = "资料登记";
                            operation.OperationTime = inDate;
                            operation.OperationID = caller.LoginID;
                            operation.OperationName = caller.LoginName;
                            operation.OperationIP = caller.IPAddress;
                            operation.OperationWorkId = caller.UserID;
                            operation.Remark = barcodeRemark;
                            SampProcessDetailBIZ processDetailBIZ = new SampProcessDetailBIZ();
                            processDetailBIZ.Dbm = helper;

                            processDetailBIZ.SaveSampProcessDetail(operation, patient.RepBarCode);
                        }



                        helper.CommitTrans();
                        helper = null;
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                        opResult.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                        helper.RollbackTrans();
                        helper = null;
                    }


                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("InsertMicPatResult", ex);
                opResult.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }
            return opResult;
        }

        public List<EntityOperateResult> DeleteMicPatResult(EntityRemoteCallClientInfo caller
            , List<EntityPidReportMain> patList, bool delRes)
        {
            //创建操作返回信息
            List<EntityOperateResult> resultList = new List<EntityOperateResult>();

            if (patList == null || patList.Count == 0) return resultList;


            foreach (var patinfo in patList)
            {
                string pat_id = patinfo.RepId;

                EntityOperateResult result = new EntityOperateResult();

                EntityPatientQC qc = new EntityPatientQC();
                qc.RepId = pat_id;
                List<EntityPidReportMain> listOriginPatInfo = new PidReportMainBIZ().PatientQuery(qc);

                if (listOriginPatInfo.Count == 0) continue;
                if (listOriginPatInfo.Count > 0 &&
                    (listOriginPatInfo[0].RepStatus.HasValue && listOriginPatInfo[0].RepStatus > 0))
                {
                    result.AddMessage(EnumOperateErrorCode.Exception, "该记录已审核:" + listOriginPatInfo[0].RepSid, EnumOperateErrorLevel.Error);
                }

                var patmiList = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);
                if (result.Success)
                {
                    var patient = listOriginPatInfo[0];
                    DBManager helper = new DBManager();
                    helper.BeginTrans();
                    try
                    {
                        IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                        mainDao.Dbm = helper;

                        IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                        detailDao.Dbm = helper;

                        mainDao.DeletePatient(pat_id);
                        //组合
                        detailDao.DeleteReportDetail(patient.RepId);

                        if (delRes)
                        {
                            //药敏结果
                            IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
                            if (dao != null)
                            {
                                dao.Dbm = helper;
                                dao.DeleteResultById(patient.RepId);
                            }
                            //细菌结果
                            IDaoObrResultBact daobact = DclDaoFactory.DaoHandler<IDaoObrResultBact>();
                            if (daobact != null)
                            {
                                daobact.Dbm = helper;
                                daobact.DeleteResultById(patient.RepId);

                            }

                            IDaoObrResultDesc daoDesc = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
                            if (daoDesc != null)
                            {
                                daoDesc.Dbm = helper;
                                daoDesc.DeleteResultById(patient.RepId);
                            }
                        }


                        //清除上机标志
                        if (!string.IsNullOrEmpty(patient.RepBarCode) &&
                            patmiList.Count > 0)
                        {
                            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
                            daoDetail.Dbm = helper;
                            foreach (var mi in patmiList)
                            {
                                List<string> comIds = new List<string>();
                                comIds.Add(mi.ComId);
                                daoDetail.UpdateSampDetailSampFlagByComId(patient.RepBarCode, comIds, "0");
                            }
                        }

                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "530";
                        operation.OperationStatusName = "删除病人资料";
                        operation.OperationTime = DateTime.Now;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = "";
                        SampProcessDetailBIZ processDetailBIZ = new SampProcessDetailBIZ();
                        processDetailBIZ.Dbm = helper;

                        processDetailBIZ.SaveSampProcessDetail(operation, patient.RepBarCode);

                        helper.CommitTrans();
                        helper = null;
                    }
                    catch (Exception ex)
                    {
                        Lib.LogManager.Logger.LogException(ex);
                        result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                        helper.RollbackTrans();
                        helper = null;
                    }
                }

                resultList.Add(result);
            }
            return resultList;
        }


        /// <summary>
        /// 细菌中期报告(预报告)
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        public EntityOperationResultList MicMidReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList reList = new EntityOperationResultList();
            if (listPatientsID == null || listPatientsID.Count() == 0)
                return reList;

            List<EntityPidReportMain> listAllPat = new List<EntityPidReportMain>();
            List<EntityObrResultAnti> listAllAnti = new List<EntityObrResultAnti>();
            List<EntityObrResultBact> listAllBact = new List<EntityObrResultBact>();
            List<EntityObrResultDesc> listAllDesc = new List<EntityObrResultDesc>();

            List<string> listOutPatientsID = new List<string>();
            List<string> listCovidPatientsID = new List<string>();

            DateTime anditDate = ServerDateTime.GetDatabaseServerDateTime();
            foreach (var pat_id in listPatientsID)
            {
                EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);
                if (listOriginPatInfo == null)
                    continue;

                if (listOriginPatInfo.PidSrcId == "110")
                {
                    listOutPatientsID.Add(pat_id);
                }
                if (listOriginPatInfo.PidComName.Contains("新冠") || listOriginPatInfo.PidComName.Contains("新型冠状"))
                {
                    listCovidPatientsID.Add(pat_id);
                }

                var listAnti = new ObrResultAntiBIZ().GetAntiResultById(pat_id);
                var listDesc = new ObrResultDescBIZ().GetDescResultById(pat_id);
                var listBact = new ObrResultBactBIZ().GetBactResultById(pat_id);
                var patient = listOriginPatInfo;

                int? oldRepStatus = patient.RepStatus;

                DBManager helper = new DBManager();
                helper.BeginTrans();
                try
                {
                    IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                    mainDao.Dbm = helper;

                    
                    patient.MicReportDate = anditDate;
                    patient.MicReportFlag = 1;
                    patient.MicReportChkUserID = caller.LoginID;
                    patient.MicReportChkUserName = caller.LoginName;
                    patient.MicReportSendUserID = caller.UserID;

                    mainDao.UpdateMicReport(patient);

                    if (!string.IsNullOrEmpty(patient.RepBarCode))
                    {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "80";
                        operation.OperationStatusName = "中期报告";
                        operation.OperationTime = anditDate;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", patient.RepItrId, patient.RepSid, patient.PidComName, anditDate);
                        new SampMainBIZ() { Dbm = helper }.UpdateSampMainStatusByBarId(operation, patient.RepBarCode);

                    }
                    helper.CommitTrans();
                    helper = null;

                    //危急值使用
                    patient.RepStatus = oldRepStatus;
                    listAllPat.Add(patient);
                    listAllAnti.AddRange(listAnti);
                    listAllBact.AddRange(listBact);
                    listAllDesc.AddRange(listDesc);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    helper.RollbackTrans();
                    helper = null;
                }
            }

            new resultcheck.SendDataToMid().Run(listPatientsID.ToList(), EnumOperationCode.MidReport);

            if (Audit_UploadYss)
            {
                if (Audit_UploadAllPatTypeYss)
                {
                    if (listCovidPatientsID != null && listCovidPatientsID.Count > 0)
                    {
                        new SendDataToMid().SendYssReport(listCovidPatientsID.ToList(), EnumOperationCode.Report);
                    }
                }
                else
                {
                    if (listOutPatientsID != null && listOutPatientsID.Count > 0)
                    {
                        new SendDataToMid().SendYssReport(listOutPatientsID.ToList(), EnumOperationCode.Report);
                    }
                }
            }

            //发送危急值消息
            SendCriticalMessage scm = new SendCriticalMessage();
            EntityQcResultList res = new EntityQcResultList();
            res.listPatients = listAllPat;
            res.listAnti = listAllAnti;
            res.listDesc = listAllDesc;
            res.listBact = listAllBact;
            scm.UpdateByBacteriaForDcl(res);

            return reList;
        }


        /// <summary>
        /// 取消细菌中期报告(取消预报告)
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList UndoMicMidReport(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            //TODO
            return null;
        }

        /// <summary>
        /// 一审
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        public void MicAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            if (listPatientsID == null || listPatientsID.Count() == 0) return;

            DateTime anditDate = ServerDateTime.GetDatabaseServerDateTime();
            foreach (var pat_id in listPatientsID)
            {
                EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);

                if (listOriginPatInfo == null || listOriginPatInfo.RepStatus >= 1) continue;

                List<string> listOutPatientsID = new List<string>();
                List<string> listCovidPatientsID = new List<string>();

                if (listOriginPatInfo.PidSrcId == "110")
                {
                    listOutPatientsID.Add(pat_id);
                }
                if (listOriginPatInfo.PidComName.Contains("新冠") || listOriginPatInfo.PidComName.Contains("新型冠状"))
                {
                    listCovidPatientsID.Add(pat_id);
                }

                if (Audit_UploadYss)
                {
                    if (Audit_UploadAllPatTypeYss)
                    {
                        if (listCovidPatientsID != null && listCovidPatientsID.Count > 0)
                        {
                            new SendDataToMid().SendYssReport(listCovidPatientsID.ToList(), EnumOperationCode.Report);
                        }
                    }
                    else
                    {
                        if (listOutPatientsID != null && listOutPatientsID.Count > 0)
                        {
                            new SendDataToMid().SendYssReport(listOutPatientsID.ToList(), EnumOperationCode.Report);
                        }
                    }
                }

                var patient = listOriginPatInfo;
                DBManager helper = new DBManager();
                helper.BeginTrans();
                try
                {
                    IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                    mainDao.Dbm = helper;



                    patient.RepAuditDate = anditDate;
                    patient.RepStatus = 1;
                    patient.RepAuditUserId = caller.LoginID;

                    mainDao.UpdatePatientData(patient);

                    if (!string.IsNullOrEmpty(patient.RepBarCode))
                    {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "40";
                        operation.OperationStatusName = "审核";
                        operation.OperationTime = anditDate;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = "";
                        new SampMainBIZ() { Dbm = helper }.UpdateSampMainStatusByBarId(operation, patient.RepBarCode);

                    }
                    helper.CommitTrans();
                    helper = null;

                   
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    helper.RollbackTrans();
                    helper = null;
                }


            }
        }

        /// <summary>
        /// 取消一审
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList UndoMicAudit(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList reList = new EntityOperationResultList();
            if (listPatientsID == null || listPatientsID.Count() == 0) return reList;

            DateTime anditDate = ServerDateTime.GetDatabaseServerDateTime();
            foreach (var pat_id in listPatientsID)
            {
                EntityOperationResult op = new EntityOperationResult();
                EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);

                if (listOriginPatInfo == null
                    || listOriginPatInfo.RepStatus > 1)
                {
                    op.AddCustomMessage("12", pat_id, "状态异常", EnumOperationErrorLevel.Error);
                    continue;
                }

                List<string> listOutPatientsID = new List<string>();
                List<string> listCovidPatientsID = new List<string>();

                if (listOriginPatInfo.PidSrcId == "110")
                {
                    listOutPatientsID.Add(pat_id);
                }
                if (listOriginPatInfo.PidComName.Contains("新冠") || listOriginPatInfo.PidComName.Contains("新型冠状"))
                {
                    listCovidPatientsID.Add(pat_id);
                }

                if (Audit_UploadYss)
                {
                    if (Audit_UploadAllPatTypeYss)
                    {
                        if (listCovidPatientsID != null && listCovidPatientsID.Count > 0)
                        {
                            new SendDataToMid().SendYssReport(listCovidPatientsID.ToList(), EnumOperationCode.Report);
                        }
                    }
                    else
                    {
                        if (listOutPatientsID != null && listOutPatientsID.Count > 0)
                        {
                            new SendDataToMid().SendYssReport(listOutPatientsID.ToList(), EnumOperationCode.Report);
                        }
                    }
                }

                var patient = listOriginPatInfo;
                DBManager helper = new DBManager();
                helper.BeginTrans();
                try
                {
                    IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                    mainDao.Dbm = helper;

                    patient.RepAuditDate = null;
                    patient.RepStatus = 0;
                    patient.RepAuditUserId = null;

                    mainDao.UpdatePatientData(patient);

                    if (!string.IsNullOrEmpty(patient.RepBarCode))
                    {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "50";
                        operation.OperationStatusName = "反审";
                        operation.OperationTime = anditDate;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = "";
                        new SampMainBIZ() { Dbm = helper }.UpdateSampMainStatusByBarId(operation, patient.RepBarCode);
                    }
                    helper.CommitTrans();
                    helper = null;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    helper.RollbackTrans();
                    helper = null;
                }
            }
            return reList;
        }

        /// <summary>
        /// 二审
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList MicReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList reList = new EntityOperationResultList();
            if (listPatientsID == null || listPatientsID.Count() == 0) return reList;

            List<string> listOutPatientsID = new List<string>();
            List<string> listCovidPatientsID = new List<string>();

            List<EntityPidReportMain> listAllPat = new List<EntityPidReportMain>();
            List<EntityObrResultAnti> listAllAnti = new List<EntityObrResultAnti>();
            List<EntityObrResultBact> listAllBact = new List<EntityObrResultBact>();
            List<EntityObrResultDesc> listAllDesc = new List<EntityObrResultDesc>();

            DateTime anditDate = ServerDateTime.GetDatabaseServerDateTime();

            string caSignatureMode = CacheSysConfig.Current.GetSystemConfig("CASignMode");
            foreach (var pat_id in listPatientsID)
            {
                EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);
                if (listOriginPatInfo == null)
                    continue;

                if (listOriginPatInfo.PidSrcId == "110")
                {
                    listOutPatientsID.Add(pat_id);
                }
                if (listOriginPatInfo.PidComName.Contains("新冠") || listOriginPatInfo.PidComName.Contains("新型冠状"))
                {
                    listCovidPatientsID.Add(pat_id);
                }

                var listAnti = new ObrResultAntiBIZ().GetAntiResultById(pat_id);
                var listDesc = new ObrResultDescBIZ().GetDescResultById(pat_id);
                var listBact = new ObrResultBactBIZ().GetBactResultById(pat_id);
                var patient = listOriginPatInfo;

                int? oldRepStatus = patient.RepStatus;

                DBManager helper = new DBManager();
                helper.BeginTrans();
                try
                {
                    IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                    mainDao.Dbm = helper;

                    if (patient.RepStatus == null || patient.RepStatus == 0)
                    {
                        patient.RepAuditUserId = caller.LoginID;
                        if (!string.IsNullOrEmpty(caller.UserID))
                        {
                            patient.RepAuditUserId = caller.UserID;
                        }
                        patient.RepAuditDate = anditDate;
                    }

                    patient.RepReportDate = anditDate;
                    patient.RepStatus = 2;
                    patient.RepReportUserId = caller.LoginID;

                    mainDao.UpdatePatientData(patient);

                    if (!string.IsNullOrEmpty(patient.RepBarCode))
                    {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "60";
                        operation.OperationStatusName = "报告";
                        operation.OperationTime = anditDate;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", patient.RepItrId, patient.RepSid, patient.PidComName, anditDate);
                        new SampMainBIZ() { Dbm = helper }.UpdateSampMainStatusByBarId(operation, patient.RepBarCode);

                    }
                    helper.CommitTrans();
                    helper = null;

                    if (caSignatureMode != "无")
                    {
                        var checkCASign = new dcl.svr.ca.AuditCheckCASign(patient, listBact, listDesc, listAnti);

                        string caSignContent = checkCASign.CASignContentSplice();

                        EntityOperationResult result = new EntityOperationResult();
                        result.OperationResultData = caSignContent;
                        result.Data.Patient = patient;

                        reList.Add(result);
                    }

                    //危急值使用
                    patient.RepStatus = oldRepStatus;
                    listAllPat.Add(patient);
                    listAllAnti.AddRange(listAnti);
                    listAllBact.AddRange(listBact);
                    listAllDesc.AddRange(listDesc);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    helper.RollbackTrans();
                    helper = null;
                }
            }

            new resultcheck.SendDataToMid().Run(listPatientsID.ToList(), EnumOperationCode.Report);

            if (Audit_UploadYss)
            {
                if (Audit_UploadAllPatTypeYss)
                {
                    if (listCovidPatientsID != null && listCovidPatientsID.Count > 0)
                    {
                        new SendDataToMid().SendYssReport(listCovidPatientsID.ToList(), EnumOperationCode.Report);
                    }
                }
                else
                {
                    if (listOutPatientsID != null && listOutPatientsID.Count > 0)
                    {
                        new SendDataToMid().SendYssReport(listOutPatientsID.ToList(), EnumOperationCode.Report);
                    }
                }
            }

            //发送危急值消息
            SendCriticalMessage scm = new SendCriticalMessage();
            EntityQcResultList res = new EntityQcResultList();
            res.listPatients = listAllPat;
            res.listAnti = listAllAnti;
            res.listDesc = listAllDesc;
            res.listBact = listAllBact;
            scm.UpdateByBacteriaForDcl(res);

            return reList;
        }

        /// <summary>
        /// 取消二审
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public EntityOperationResultList UndoMicReport(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            EntityOperationResultList reList = new EntityOperationResultList();
            if (listPatientsID == null || listPatientsID.Count() == 0) return reList;

            List<string> listOutPatientsID = new List<string>();
            List<string> listCovidPatientsID = new List<string>();

            List<EntityPidReportMain> listAllPat = new List<EntityPidReportMain>();
            List<EntityObrResultAnti> listAllAnti = new List<EntityObrResultAnti>();
            List<EntityObrResultDesc> listAllDesc = new List<EntityObrResultDesc>();

            DateTime anditDate = ServerDateTime.GetDatabaseServerDateTime();
            foreach (var pat_id in listPatientsID)
            {

                EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);

                if (listOriginPatInfo == null)
                    continue;

                if (listOriginPatInfo.PidSrcId == "110")
                {
                    listOutPatientsID.Add(pat_id);
                }
                if (listOriginPatInfo.PidComName.Contains("新冠") || listOriginPatInfo.PidComName.Contains("新型冠状"))
                {
                    listCovidPatientsID.Add(pat_id);
                }

                var patient = listOriginPatInfo;

                int? oldRepStatus = patient.RepStatus;

                DBManager helper = new DBManager();
                helper.BeginTrans();
                try
                {
                    IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                    mainDao.Dbm = helper;

                    if (CacheSysConfig.Current.GetSystemConfig("OneStepCancelReport") == "是")
                    {
                        patient.RepAuditUserId = null;
                        patient.RepAuditDate = null;
                        patient.RepReportDate = null;
                        patient.RepStatus = 0;
                        patient.RepReportUserId = null;
                    }
                    else
                    {
                        patient.RepReportDate = null;
                        patient.RepStatus = 1;
                        patient.RepReportUserId = null;
                    }


                    mainDao.UpdatePatientData(patient);

                    if (!string.IsNullOrEmpty(patient.RepBarCode))
                    {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "70";
                        operation.OperationStatusName = "二审反审";
                        operation.OperationTime = anditDate;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = caller.Remarks;
                        var samBiz = new SampMainBIZ() { Dbm = helper };
                        samBiz.UpdateSampMainStatusByBarId(operation, patient.RepBarCode);

                    }
                    helper.CommitTrans();
                    helper = null;
                    //危急值使用
                    patient.RepStatus = oldRepStatus;
                    listAllPat.Add(patient);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    helper.RollbackTrans();
                    helper = null;
                }
            }

            new SendDataToMid().Run(listPatientsID, EnumOperationCode.UndoReport);

            if (Audit_UploadYss)
            {
                if (Audit_UploadAllPatTypeYss)
                {
                    if (listCovidPatientsID != null && listCovidPatientsID.Count > 0)
                    {
                        new SendDataToMid().SendYssReport(listCovidPatientsID.ToList(), EnumOperationCode.UndoReport);
                    }
                }
                else
                {
                    if (listOutPatientsID != null && listOutPatientsID.Count > 0)
                    {
                        new SendDataToMid().SendYssReport(listOutPatientsID.ToList(), EnumOperationCode.UndoReport);
                    }
                }
            }

            //发送危急值消息 for delete;
            SendCriticalMessage scm = new SendCriticalMessage();
            EntityQcResultList res = new EntityQcResultList();
            res.listPatients = listAllPat;
            //res.listAnti = listAllAnti;
            //res.listDesc = listAllDesc;
            scm.UpdateByBacteriaForDcl(res);





            return reList;
        }

        public List<EntityDicMicSmear> GetDicMicSmearByComID(string strComIDs)
        {
            List<EntityDicMicSmear> listPat = new List<EntityDicMicSmear>();
            IMicData mainDao = DclDaoFactory.DaoHandler<IMicData>();
            if (mainDao != null)
            {
                listPat = mainDao.GetDicMicSmearByComID(strComIDs);
            }
            return listPat;
        }

        /// <summary>
        /// 根据当前仪器和样本号、年份获取满足条件的日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="itr_id">仪器ID</param>
        /// <param name="currentSID">当前样本号</param>
        /// <returns></returns>
        public string GetPatDate_ByItrSID(DateTime date, string itr_id, string currentSID)
        {
            string ret = "";
            IMicData mainDao = DclDaoFactory.DaoHandler<IMicData>();
            if (mainDao != null)
            {
                ret = mainDao.GetPatDate_ByItrSID(date, itr_id, currentSID);
            }
            return ret;
        }


        public void SaveMicAntidetailList(List<EntityDicMicAntidetail> list)
        {
            IDaoDic<EntityDicMicAntidetail> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicMicAntidetail>>();
            if (dao != null)
            {
                foreach (var anSstd in list)
                {
                    dao.Save(anSstd);
                }

            }
        }

        /// <summary>
        /// 细菌仪器结果视窗
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public List<EntityMicViewData> GetMicViewList(DateTime date, string itr_id)
        {
            List<EntityMicViewData> ret = new List<EntityMicViewData>();
            IMicData mainDao = DclDaoFactory.DaoHandler<IMicData>();
            if (mainDao != null)
            {
                ret = mainDao.GetMicViewList(date, itr_id);
            }
            return ret;
        }

        /// <summary>
        /// 复制历史结果
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public bool CopyHistroyResult(EntityQcResultList resultList, string newRepId, string repItrId, string repSid)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();
                helper.BeginTrans();
                #region 先删除病人结果
                //药敏结果
                IDaoObrResultAnti dao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
                if (dao != null)
                {
                    dao.Dbm = helper;
                    dao.DeleteResultById(newRepId);
                }
                //细菌结果
                IDaoObrResultBact daobact = DclDaoFactory.DaoHandler<IDaoObrResultBact>();
                if (daobact != null)
                {
                    daobact.Dbm = helper;
                    daobact.DeleteResultById(newRepId);

                }
                //描述结果
                IDaoObrResultDesc daoDesc = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
                if (daoDesc != null)
                {
                    daoDesc.Dbm = helper;
                    daoDesc.DeleteResultById(newRepId);
                }
                #endregion
                if (resultList != null && resultList.listAnti.Count > 0)
                {
                    foreach (EntityObrResultAnti anti in resultList.listAnti)
                    {
                        anti.ObrId = newRepId;
                        anti.ObrItrId = repItrId;
                        anti.ObrDate = ServerDateTime.GetDatabaseServerDateTime();
                        if (repSid != null)
                            anti.ObrSid = Convert.ToDecimal(repSid);
                    }
                }
                if (resultList != null && resultList.listBact.Count > 0)
                {
                    foreach (EntityObrResultBact bact in resultList.listBact)
                    {
                        bact.ObrId = newRepId;
                        bact.ObrItrId = repItrId;
                        bact.ObrDate = ServerDateTime.GetDatabaseServerDateTime();
                        if (repSid != null)
                            bact.ObrSid = Convert.ToDecimal(repSid);
                    }
                }
                if (resultList != null && resultList.listDesc.Count > 0)
                {
                    foreach (EntityObrResultDesc desc in resultList.listDesc)
                    {
                        desc.ObrId = newRepId;
                        desc.ObrItrId = repItrId;
                        desc.ObrDate = ServerDateTime.GetDatabaseServerDateTime();
                        if (repSid != null)
                            desc.ObrSid = Convert.ToDecimal(repSid);
                    }
                }
                //保存药敏结果
                result = new ObrResultAntiBIZ() { Dbm = helper }.SaveAntiResult(resultList.listAnti);
                //保存细菌结果
                result = new ObrResultBactBIZ() { Dbm = helper }.SaveResultBact(resultList.listBact);
                //保存描述结果
                result = new ObrResultDescBIZ() { Dbm = helper }.InsertObrResultDesc(resultList.listDesc);
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        /// <summary>
        /// 获取药敏结果
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        public string GetAntiResult(List<string> repId)
        {
            string res = string.Empty;
            IMicData mainDao = DclDaoFactory.DaoHandler<IMicData>();
            if (mainDao != null)
            {
                res = mainDao.GetAntiResult(repId);
            }
            return res;
        }
    }
}
