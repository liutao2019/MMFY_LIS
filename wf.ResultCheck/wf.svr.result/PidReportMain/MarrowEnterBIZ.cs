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
    public class MarrowEnterBIZ : IMarrowEnter
    {
        /// <summary>
        /// 获取骨髓检验明细信息
        /// </summary>
        /// <param name="report_id">主报告ID</param>
        /// <returns></returns>
        public List<EntityObrResult> GetMarrowdetailList(string report_id)
        {
            return null;
        }

        /// <summary>
        /// 病人信息查询
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> MarrowPatientQuery(EntityPatientQC patientCondition)
        {
            return null;
        }

        /// <summary>
        /// 保存细菌病人、结果信息
        /// </summary>
        /// <param name="caller">操作人</param>
        /// <param name="report">主报告记录</param>
        /// <param name="results">结果记录</param>
        /// <param name="image_results">图片结果记录</param>
        /// <returns>true成功，false失败</returns>
        public bool UpdateMarrowPatResult(EntityRemoteCallClientInfo userInfo,
            EntityPidReportMain report,
            List<EntityObrResult> results,
            List<EntityObrResultImage> image_results
            )
        {
            DBManager helper = new DBManager();

            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            mainDao.Dbm = helper;

            if (report == null)
            {
                return false;
            }


            var patient = report;

            if (!string.IsNullOrEmpty(patient.RepBarCode))
            {
                //条码号登记判断并发
                string patId = new PidReportMainBIZ().GetPatientPatId(patient.RepItrId, patient.RepBarCode, patient.RepSid,patient.RepInDate.Value);

                if (string.IsNullOrEmpty(patId))
                {
                    throw new Exception("该样本号的记录不存在" + patient.RepBarCode);
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
                    string error_msg = string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", patient.RepId, patient.PidAgeExp);
                    Logger.WriteException("更新骨髓记录失败", "UpdateMarrowPatResult", error_msg);
                    throw new Exception(error_msg);
                }

            }

            DateTime inDate = ServerDateTime.GetDatabaseServerDateTime();
            patient.RepModifyFrequency += 1;//修改次数

            try
            {
                IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                resultDao.Dbm = helper;

                IDaoObrResultImage resultImageDao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
                resultImageDao.Dbm = helper;

                IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                detailDao.Dbm = helper;

                helper.BeginTrans();

                //病人资料
                mainDao.UpdatePatientData(patient);

                // 报告组合信息
                if (detailDao.DeleteReportDetail(patient.RepId))
                {
                    foreach (EntityPidReportDetail detail in patient.ListPidReportDetail)
                    {
                        detail.RepId = patient.RepId;
                        detailDao.InsertNewPidReportDetail(detail);
                    }
                }

                resultDao.DeleteObrResultByObrId(patient.RepId);
                // 保存检验结果
                if (results != null && results.Count > 0)
                {
                    foreach (EntityObrResult result in results)
                    {
                        result.ObrId = patient.RepId;
                        result.ObrDate = inDate;
                        resultDao.InsertObrResult(result);
                    }
                }

                resultImageDao.DeleteObrResultImageByObrId(patient.RepId);
                // 保存图片报告
                if (image_results != null && image_results.Count > 0)
                {
                    foreach (EntityObrResultImage image_result in image_results)
                    {
                        image_result.ObrId = patient.RepId;
                        image_result.ObrDate = inDate;
                        resultImageDao.SaveObrResultImage(image_result);
                    }
                }

                if (!string.IsNullOrEmpty(patient.RepBarCode))
                {
                    EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(report.RepBarCode);
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

                    //创建日记记录对象
                    EntitySampOperation operation = new EntitySampOperation();
                    operation.OperationStatus = "35";
                    operation.OperationStatusName = "资料修改";
                    operation.OperationTime = inDate;
                    operation.OperationID = userInfo.LoginID;
                    operation.OperationName = userInfo.LoginName;
                    operation.OperationIP = userInfo.IPAddress;
                    operation.OperationWorkId = userInfo.UserID;
                    operation.Remark = barcodeRemark;

                    if (sampMain != null)
                    {
                        new SampProcessDetailBIZ() { Dbm = helper }.SaveSampProcessDetail(operation, sampMain);
                    }
                }

                helper.CommitTrans();
                helper = null;
                return true;

            }
            catch (Exception ex)
            {
                helper.RollbackTrans();
                Lib.LogManager.Logger.LogException("UpdateMarrowPatResult", ex);
                throw new Exception("UpdateMarrowPatResult", ex);
            }

        }

        /// <summary>
        /// 保存细菌病人、结果信息
        /// </summary>
        /// <param name="caller">操作人</param>
        /// <param name="report">主报告记录</param>
        /// <param name="results">结果记录</param>
        /// <param name="image_results">图片结果记录</param>
        /// <returns>true成功，false失败</returns>
        public bool InsertMarrowPatResult(EntityRemoteCallClientInfo caller,
            EntityPidReportMain report,
            List<EntityObrResult> results,
            List<EntityObrResultImage> image_results
            )
        {

            DBManager helper = new DBManager();

            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            mainDao.Dbm = helper;

            if (report == null)
            {
                return false;
            }

            var patient = report;

            if (!string.IsNullOrEmpty(patient.RepBarCode))
            {
                //条码号登记判断并发
                string patId = new PidReportMainBIZ().GetPatientPatId(patient.RepItrId, patient.RepBarCode, patient.RepSid,patient.RepInDate.Value);

                if (!string.IsNullOrEmpty(patId))
                {
                    throw new Exception("该样本号的记录已存在" + patient.RepBarCode);
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
                    string error_msg = string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", patient.RepId, patient.PidAgeExp);
                    Logger.WriteException("MarrowInsertBLL", "InsertMarrowPatResult", error_msg);
                    throw new Exception(error_msg);
                }

            }

            DateTime inDate = ServerDateTime.GetDatabaseServerDateTime();

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

            /// 检查样本号或者序号是否已经存在(flag为1即为查序号)
            bool ExsitSid = mainDao.ExsitSidOrHostOrder(patient.RepSid, patient.RepItrId, inDate);
            if (ExsitSid)
            {
                throw new Exception("样本号已存在" + patient.RepSid);
            }


            try
            {
                IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                resultDao.Dbm = helper;

                IDaoObrResultImage resultImageDao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
                resultImageDao.Dbm = helper;

                IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                detailDao.Dbm = helper;

                helper.BeginTrans();

                //病人资料
                mainDao.InsertNewPatient(patient);

                // 报告组合信息
                if (detailDao.DeleteReportDetail(patient.RepId))
                {
                    foreach (EntityPidReportDetail detail in patient.ListPidReportDetail)
                    {
                        detail.RepId = patient.RepId;
                        detailDao.InsertNewPidReportDetail(detail);
                    }
                }

                // 保存检验结果
                if (results != null && results.Count > 0)
                {
                    foreach(EntityObrResult result in results)
                    {
                        result.ObrId = patient.RepId;
                        result.ObrDate = inDate;
                        resultDao.InsertObrResult(result);
                    }
                }

                // 保存图片报告
                if(image_results != null && image_results.Count > 0)
                {
                    foreach (EntityObrResultImage image_result in image_results)
                    {
                        image_result.ObrId = patient.RepId;
                        image_result.ObrDate = inDate;
                        resultImageDao.SaveObrResultImage(image_result);
                    }
                }


                if (!string.IsNullOrEmpty(patient.RepBarCode))
                {
                    EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(report.RepBarCode);
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

                    if (sampMain != null)
                    {
                        new SampProcessDetailBIZ() { Dbm = helper }.SaveSampProcessDetail(operation, sampMain);
                    }
                }

                helper.CommitTrans();
                helper = null;
                return true;

            }
            catch (Exception ex)
            {
                helper.RollbackTrans();
                Lib.LogManager.Logger.LogException("InsertMarrowPatResult", ex);
                throw new Exception("InsertMarrowPatResult", ex);
            }
        }

        /// <summary>
        /// 删除骨髓报告信息
        /// </summary>
        /// <param name="caller">操作人</param>
        /// <param name="reportList">报告列表</param>
        /// <param name="delRes">是否删除结果</param>
        /// <returns></returns>
        public bool DeleteMarrowPatResult(EntityRemoteCallClientInfo caller,
            List<EntityPidReportMain> reportList, bool delRes)
        {
            if(reportList == null || reportList.Count == 0)
            {
                return false;
            }

            DBManager helper = new DBManager();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            mainDao.Dbm = helper;
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            resultDao.Dbm = helper;
            IDaoObrResultImage resultImageDao = DclDaoFactory.DaoHandler<IDaoObrResultImage>();
            resultImageDao.Dbm = helper;
            IDaoSysOperationLog operationLogDao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();
            operationLogDao.Dbm = helper;

            IDaoSampMain sampMainDao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            sampMainDao.Dbm = helper;

            try
            {
                helper.BeginTrans();

                List<EntitySampMain> samp_list = new List<EntitySampMain>();
                foreach (EntityPidReportMain report in reportList)
                {
                    mainDao.DeletePatient(report.RepId);
                    if (delRes)
                    {
                        resultDao.DeleteObrResultByObrId(report.RepId);
                        resultImageDao.DeleteObrResultImageByObrId(report.RepId);
                    }

                    //清除上机标志

                    if (!string.IsNullOrEmpty(report.RepBarCode) &&
                        reportList.Count > 0)
                    {
                        List<EntityPidReportDetail> listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(report.RepId);
                        if (listDetail != null)
                        {
                            IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
                            daoDetail.Dbm = helper;
                            foreach (var mi in listDetail)
                            {
                                List<string> comIds = new List<string>();
                                comIds.Add(mi.ComId);
                                daoDetail.UpdateSampDetailSampFlagByComId(report.RepBarCode, comIds, "0");
                            }
                        }
                    }

                    EntitySampMain sampMain = new SampMainBIZ().SampMainQueryByBarId(report.RepBarCode);
                    string remark = string.Empty;
                    if (string.IsNullOrEmpty(report.RepBarCode))  //如果条码号不存在不更新samp_main 表 只更新Samp_ process_detial
                    {
                        if (report != null && !string.IsNullOrEmpty(report.PidComName))
                        {
                          remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                          report.RepId, report.PidName, report.PidComName,
                          report.PidInNo, report.ItrName, report.RepSid);
                        }
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "530";
                        operation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
                        operation.OperationID = caller.LoginID;
                        operation.OperationIP = caller.IPAddress;
                        operation.Remark = remark;
                        operation.RepId = report.RepId;
                        new SampProcessDetailBIZ() { Dbm = helper }.SaveSampProcessDetail(operation, sampMain);
                    }
                    //条码号存在则还需要更新samp_main表中的条码状态
                    else
                    {
                        remark = string.Format(@"pat_id:{0},姓名：{1}，组合：{2}，病人ID：{3}，仪器名称：{4},样本号：{5}",
                                                            report.RepId, report.PidName, report.PidComName,
                                                            report.PidInNo, report.ItrName, report.RepSid);
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "530";
                        operation.OperationTime = ServerDateTime.GetDatabaseServerDateTime();
                        operation.OperationID = caller.LoginID;
                        operation.OperationIP = caller.IPAddress;
                        operation.Remark = remark;
                        operation.RepId = report.RepId;
                        operation.OperationName = caller.OperationName;
                        List<EntitySampMain> listSamp = new List<EntitySampMain>();
                        listSamp.Add(sampMain);
                        //更新条码操作状态 以及保存流程操作信息
                        new SampMainBIZ() { Dbm = helper}.UpdateSampMainStatus(operation, listSamp);
                    }

                    #region 填充删除病人基本信息日志实体
                    EntitySysOperationLog opLog = new EntitySysOperationLog();
                    opLog.OperatUserId = caller.LoginID;
                    opLog.OperatServername = caller.IPAddress;
                    opLog.OperatModule = "病人资料";
                    opLog.OperatKey = report.RepId;
                    opLog.OperatDate = ServerDateTime.GetDatabaseServerDateTime();
                    opLog.OperatGroup = "病人基本信息";
                    opLog.OperatAction = "删除";
                    operationLogDao.SaveSysOperationLog(opLog);
                    #endregion
                }
                // 清楚标本状态信息

                helper.CommitTrans();
                return true;
            }
            catch (Exception ex)
            {
                helper.RollbackTrans();
                Lib.LogManager.Logger.LogException("DeleteMarrowPatResult", ex);
                throw new Exception("DeleteMarrowPatResult", ex);
            }
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="listPatientsID">需要审核的病人ID集合</param>
        /// <returns>审核结果</returns>
        public bool MarrowAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            if (listPatientsID == null || listPatientsID.Count() == 0)
            {
                return false;
            }

            DBManager helper = new DBManager();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            mainDao.Dbm = helper;

            try
            {
                helper.BeginTrans();
                DateTime anditDate = ServerDateTime.GetDatabaseServerDateTime();
                foreach (var pat_id in listPatientsID)
                {
                    EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);

                    if (listOriginPatInfo == null || listOriginPatInfo.RepStatus >= 1)
                    {
                        continue;
                    }

                    var patient = listOriginPatInfo;

                    patient.RepAuditDate = anditDate;
                    patient.RepStatus = 1;
                    patient.RepAuditUserId = caller.LoginID;
                    patient.RepAuditUserName = caller.OperationName;

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
                }
                helper.CommitTrans();
                mainDao.Dbm = null;
                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                helper.RollbackTrans();
                throw ex;
            }
        }

        /// <summary>
        /// 预审核报告
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public bool MarrowPreAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return true;
        }

        /// <summary>
        /// 取消预审核报告
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public bool UndoMarrowPreAudit(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            return true;
        }

        /// <summary>
        /// 批量反审核（一审）
        /// </summary>
        /// <param name="PatientID"></param>
        public bool UndoMarrowAudit(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            if (listPatientsID == null || listPatientsID.Count() == 0)
            {
                return false;
            }

            DBManager helper = new DBManager();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            mainDao.Dbm = helper;

            try
            {
                helper.BeginTrans();
                DateTime anditDate = ServerDateTime.GetDatabaseServerDateTime();
                foreach (var pat_id in listPatientsID)
                {
                    EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);

                    if (listOriginPatInfo == null || listOriginPatInfo.RepStatus != 1)
                    {
                        continue;
                    }

                    var patient = listOriginPatInfo;

                    patient.RepAuditDate = null;
                    patient.RepStatus = 0;
                    patient.RepAuditUserId = null;
                    patient.RepAuditUserName = null;

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
                }
                helper.CommitTrans();
                mainDao.Dbm = null;
                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                helper.RollbackTrans();
                throw ex;
            }

        }

        /// <summary>
        /// 批量报告
        /// </summary>
        /// <param name="listPatientsID">需要审核的病人ID集合</param>
        /// <returns>审核结果</returns>
        public bool MarrowReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            if (listPatientsID == null || listPatientsID.Count() == 0)
            {
                return false;
            }
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            try
            {
                DateTime reportDate = ServerDateTime.GetDatabaseServerDateTime();
                foreach (var pat_id in listPatientsID)
                {
                    EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);

                    if (listOriginPatInfo == null || listOriginPatInfo.RepStatus != 1) // 已审核才能报告
                    {
                        continue;
                    }

                    var patient = listOriginPatInfo;

                    patient.RepReportDate = reportDate;
                    patient.RepStatus = 2;
                    patient.RepReportUserId = caller.LoginID;
                    patient.RepReportUserName = caller.OperationName;

                    mainDao.UpdatePatientData(patient);

                    if (!string.IsNullOrEmpty(patient.RepBarCode))
                    {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "60";
                        operation.OperationStatusName = "报告";
                        operation.OperationTime = reportDate;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", patient.RepItrId, patient.RepSid, patient.PidComName, reportDate);
                        new SampMainBIZ().UpdateSampMainStatusByBarId(operation, patient.RepBarCode);
                    }
                }
                mainDao.Dbm = null;
                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        /// <summary>
        /// 批量取消报告
        /// </summary>
        /// <param name="listPatientsID"></param>
        public bool UndoMarrowReport(List<string> listPatientsID, EntityRemoteCallClientInfo caller)
        {
            if (listPatientsID == null || listPatientsID.Count() == 0)
            {
                return false;
            }

            DBManager helper = new DBManager();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            mainDao.Dbm = helper;

            try
            {
                helper.BeginTrans();
                DateTime undoReportDate = ServerDateTime.GetDatabaseServerDateTime();
                foreach (var pat_id in listPatientsID)
                {
                    EntityPidReportMain listOriginPatInfo = new PidReportMainBIZ().GetPatientByPatId(pat_id, false);

                    if (listOriginPatInfo == null || listOriginPatInfo.RepStatus < 2) // 已报告才能取消
                    {
                        continue;
                    }

                    var patient = listOriginPatInfo;

                    patient.RepReportDate = null;
                    patient.RepStatus = 1;
                    patient.RepReportUserId = null;
                    patient.RepReportUserName = null;

                    mainDao.UpdatePatientData(patient);

                    if (!string.IsNullOrEmpty(patient.RepBarCode))
                    {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "70";
                        operation.OperationStatusName = "二审反审";
                        operation.OperationTime = undoReportDate;
                        operation.OperationID = caller.LoginID;
                        operation.OperationName = caller.LoginName;
                        operation.OperationIP = caller.IPAddress;
                        operation.OperationWorkId = caller.UserID;
                        operation.Remark = caller.Remarks;
                        new SampMainBIZ() { Dbm = helper }.UpdateSampMainStatusByBarId(operation, patient.RepBarCode);
                    }
                }
                helper.CommitTrans();
                mainDao.Dbm = null;
                return true;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                helper.RollbackTrans();
                throw ex;
            }
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
            return string.Empty;
        }

        #region 审核、报告前检查

        /// <summary>
        /// 判断结果表中是否有输入血片和骨髓结果
        /// </summary>
        /// <returns></returns>
        private bool HasValidResult(List<EntityObrResult> result_list)
        {
            if(result_list == null && result_list.Count == 0)
            {
                return false;
            }

            foreach(EntityObrResult result in result_list)
            {
                if(!string.IsNullOrEmpty(result.ObrBldValue))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断结果表中是否有必录结果为空
        /// </summary>
        /// <returns></returns>
        private void CheckNotNullResultIsNull(List<EntityObrResult> result_list,
            List<EntityPidReportDetail> report_details, EntityOperationResult opr_result)
        {
            if (result_list == null && result_list.Count == 0)
            {
                return;
            }
            if (report_details == null && report_details.Count == 0)
            {
                return;
            }

            EntityResponse response = new CacheDataBIZ().GetCacheData("EntityDicCombineDetail");
            List<EntityDicCombineDetail> listCombine = response.GetResult() as List<EntityDicCombineDetail>;

            foreach (EntityPidReportDetail report_detail in report_details)
            {
                // 所有必填项目
                List<EntityDicCombineDetail> comb_details = listCombine.FindAll(i => i.ComId == report_detail.ComId);
                foreach(EntityDicCombineDetail detail in comb_details)
                {
                    EntityObrResult result = result_list.Find(i => i.ItmId == detail.ComItmId && i.ItmComId == detail.ComId);
                    if(result == null) // 必填项目没有结果
                    {
                        opr_result.AddMessage(EnumOperationErrorCode.ItemNotNullLost, detail.ComItmEname, EnumOperationErrorLevel.Warn);
                    }
                    else
                    {   
                        // 必录信息为空
                        if(string.IsNullOrEmpty(result.ObrBldValue) && string.IsNullOrEmpty(result.ObrBoneValue))
                        {
                            opr_result.AddMessage(EnumOperationErrorCode.ItemNotNullLost, detail.ComItmEname, EnumOperationErrorLevel.Warn);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 骨髓取消报告前检查
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult SingleUndoReportCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;
            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);
            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;
                result.Data.Patient.PatAgeTxt = drPat.PatAgeTxt;
                result.Data.Patient.PidAgeExp = drPat.PidAgeExp;
                result.Data.Patient.PidAge = drPat.PidAge;
                result.Data.Patient.DeptName = drPat.DeptName;
                result.Data.Patient.PidDeptName = drPat.PidDeptName;
                result.Data.Patient.PidDiag = drPat.PidDiag;
                result.Data.Patient.RepBarCode = drPat.RepBarCode;

                //取消二审时检查是否已阅读
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("UndoAudit_Second_CheckLookcode") == "是")
                {
                    if (!string.IsNullOrEmpty(drPat.RepReadUserId))
                    {
                        result.AddCustomMessage("", "", string.Format("此报告【{0}】已阅读过不能取消二审", drPat.RepReadUserId), EnumOperationErrorLevel.Error);
                    }
                }

                if (drPat.RepStatus == 4)
                {
                    result.AddCustomMessage("", "", string.Format("病人报告已经打印，请收回病人原始报告单", drPat.RepReadUserId), EnumOperationErrorLevel.Warn);
                }


                if (!result.Success)//如果不通过就返回
                {
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// 批量取消报告前检查
        /// </summary>
        /// <param name="list_pat_id"></param>
        /// <param name="oper_type">预留-暂未用</param>
        /// <returns></returns>
        public EntityOperationResultList BatchUndoReportCheck(IEnumerable<string> list_pat_id, string oper_type)
        {
            EntityOperationResultList resultsList = new EntityOperationResultList();
            foreach (string pat_id in list_pat_id)
            {
                EntityOperationResult result = new EntityOperationResult();
                result.Data.Patient.RepId = pat_id;
                result = SingleUndoReportCheck(pat_id);
                resultsList.Add(result);
            }
            return resultsList;
        }

        /// <summary>
        /// 批量审核前检测是否审核
        /// </summary>
        /// <param name="list_pat_id"></param>
        /// <param name="isAudit"></param>
        /// <returns></returns>
        public EntityOperationResultList BatchAuditCheck(IEnumerable<string> list_pat_id, string isAudit)
        {
            EntityOperationResultList resultsList = new EntityOperationResultList();
            foreach (string pat_id in list_pat_id)
            {
                EntityOperationResult result = new EntityOperationResult();
                result.Data.Patient.RepId = pat_id;
                if (isAudit == "1")
                    result = SingleAuditCheck(pat_id);
                else if (isAudit == "2")
                    result = SingleReportCheck(pat_id);
                else if (isAudit == "0")
                    result = SinglePreAuditCheck(pat_id);
                resultsList.Add(result);
            }
            return resultsList;
        }



        /// <summary>
        /// 审核前判断是否可以审核
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public EntityOperationResult SingleAuditCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;

            string Lab_ThreeAuditItrIDs = CacheSysConfig.Current.GetSystemConfig("Lab_ThreeAuditItrIDs");

            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);

            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;
                result.Data.Patient.PatAgeTxt = drPat.PatAgeTxt;
                result.Data.Patient.PidAgeExp = drPat.PidAgeExp;
                result.Data.Patient.PidAge = drPat.PidAge;
                result.Data.Patient.DeptName = drPat.DeptName;
                result.Data.Patient.PidDeptName = drPat.PidDeptName;
                result.Data.Patient.PidDiag = drPat.PidDiag;
                result.Data.Patient.RepBarCode = drPat.RepBarCode;
                switch (strFlag)
                {
                    case "0":

                        if (strpat_pre_flag == "0" && !string.IsNullOrEmpty(Lab_ThreeAuditItrIDs)
                            && Lab_ThreeAuditItrIDs.Contains(pat_itr_id))
                        {
                            result.AddMessage(EnumOperationErrorCode.NotPreAudit, EnumOperationErrorLevel.Error);
                            break;
                        }

                        EntityResultQC resultQc = new EntityResultQC();
                        resultQc.ListObrId.Add(pat_id);
                        var listResult = new ObrResultBIZ().GetObrResultQuery(resultQc);
                        var listResultImage = new ObrResultImageBIZ().GetObrResultImage(pat_id);
                        var listReportDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);

                        if ((listResult == null || listResult.Count == 0) &&
                            (listResultImage == null || listResultImage.Count == 0))
                        {
                            result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            if (!HasValidResult(listResult))
                            {
                                result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                            }
                            else
                            {
                                CheckNotNullResultIsNull(listResult, listReportDetail, result);
                            }
                        }

                        break;
                    case "1":
                        result.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                        break;
                    case "2":
                        result.AddMessage(EnumOperationErrorCode.Reported, EnumOperationErrorLevel.Error);
                        break;
                    case "4":
                        result.AddMessage(EnumOperationErrorCode.Printed, EnumOperationErrorLevel.Error);
                        break;
                    default:
                        break;
                }
            }
            //判断当检查目的为空时的错误等级
            EnumOperationErrorLevel strNoInstention = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CanNotAuditWhileNoCheckIntention"));
            EnumOperationErrorLevel Audit_PatSpcialTimeCheck = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_PatSpcialTimeCheck"));

            string PidPurpId = drPat.PidPurpId;
            result.Data.Patient.PidName = drPat.PidName;
            result.Data.Patient.RepSid = drPat.RepSid;

            if (string.IsNullOrEmpty(PidPurpId))
            {
                result.AddCustomMessage("54t4", "检查目的", string.Format("病人姓名：{1}，样本号：{0},没有检查目的", drPat.RepSid, drPat.PidName), strNoInstention);
            }

            if (Audit_PatSpcialTimeCheck != EnumOperationErrorLevel.None)
            {

                if (drPat.SampSendDate == null)
                {
                    result.AddCustomMessage("223454676", "", "检验时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampCollectionDate == null)
                {
                    result.AddCustomMessage("233454676", "", "标本采集时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampApplyDate == null)
                {
                    result.AddCustomMessage("243454676", "", "标本接收时间为空！", Audit_PatSpcialTimeCheck);
                }

            }

            return result;
        }

        public EntityOperationResult SinglePreAuditCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;

            string Lab_ThreeAuditItrIDs = CacheSysConfig.Current.GetSystemConfig("Lab_ThreeAuditItrIDs");

            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);

            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;
                result.Data.Patient.PatAgeTxt = drPat.PatAgeTxt;
                result.Data.Patient.PidAgeExp = drPat.PidAgeExp;
                result.Data.Patient.PidAge = drPat.PidAge;
                result.Data.Patient.DeptName = drPat.DeptName;
                result.Data.Patient.PidDeptName = drPat.PidDeptName;
                result.Data.Patient.PidDiag = drPat.PidDiag;
                result.Data.Patient.RepBarCode = drPat.RepBarCode;
                switch (strFlag)
                {
                    case "0":

                        if (strpat_pre_flag == "1")
                        {
                            result.AddMessage(EnumOperationErrorCode.PreAudited, EnumOperationErrorLevel.Error);
                            break;
                        }
                        EntityResultQC resultQc = new EntityResultQC();
                        resultQc.ListObrId.Add(pat_id);
                        var listResult = new ObrResultBIZ().GetObrResultQuery(resultQc);
                        var listResultImage = new ObrResultImageBIZ().GetObrResultImage(pat_id);
                        var listReportDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);

                        if ((listResult == null || listResult.Count == 0) &&
                            (listResultImage == null || listResultImage.Count == 0))
                        {
                            result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                        }
                        else
                        {

                            if (!HasValidResult(listResult))
                            {
                                result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                            }
                            else
                            {
                                CheckNotNullResultIsNull(listResult, listReportDetail, result);
                            }
                        }

                        break;
                    case "1":
                        result.AddMessage(EnumOperationErrorCode.Audited, EnumOperationErrorLevel.Error);
                        break;
                    case "2":
                        result.AddMessage(EnumOperationErrorCode.Reported, EnumOperationErrorLevel.Error);
                        break;
                    case "4":
                        result.AddMessage(EnumOperationErrorCode.Printed, EnumOperationErrorLevel.Error);
                        break;
                    default:
                        break;
                }
            }
            //判断当检查目的为空时的错误等级
            EnumOperationErrorLevel strNoInstention = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CanNotAuditWhileNoCheckIntention"));
            EnumOperationErrorLevel Audit_PatSpcialTimeCheck = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_PatSpcialTimeCheck"));
            string PidPurpId = drPat.PidPurpId;
            result.Data.Patient.PidName = drPat.PidName;
            result.Data.Patient.RepSid = drPat.RepSid;

            if (string.IsNullOrEmpty(PidPurpId))
            {
                result.AddCustomMessage("54t4", "检查目的", string.Format("病人姓名：{1}，样本号：{0},没有检查目的", drPat.RepSid, drPat.PidName), strNoInstention);
            }

            if (Audit_PatSpcialTimeCheck != EnumOperationErrorLevel.None)
            {

                if (drPat.SampSendDate == null)
                {
                    result.AddCustomMessage("223454676", "", "检验时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampCollectionDate == null)
                {
                    result.AddCustomMessage("233454676", "", "标本采集时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampApplyDate == null)
                {
                    result.AddCustomMessage("243454676", "", "标本接收时间为空！", Audit_PatSpcialTimeCheck);
                }

            }
            return result;
        }

        public EntityOperationResult SingleReportCheck(string pat_id)
        {
            EntityOperationResult result = new EntityOperationResult();
            result.Data.Patient.RepId = pat_id;

            var drPat = new PidReportMainBIZ().GetPatientByPatId(pat_id);

            string strProp = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AllowStepAuditToReport");

            string oneStepAuditTimeZone = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_SetOneStepAuditTimeZone");
            if (!string.IsNullOrEmpty(oneStepAuditTimeZone) && oneStepAuditTimeZone.Split(',').Length == 2)
            {
                DateTime dtStart = Convert.ToDateTime(oneStepAuditTimeZone.Split(',')[0]);
                DateTime dtEnd = Convert.ToDateTime(oneStepAuditTimeZone.Split(',')[1]);

                if (dtStart <= DateTime.Now && dtStart > dtEnd)
                {
                    dtEnd = dtEnd.AddDays(1);
                }

                if (dtStart > DateTime.Now)
                {
                    if (dtStart < dtEnd)
                    {
                        dtEnd = dtEnd.AddDays(-1);
                    }
                    dtStart = dtStart.AddDays(-1);
                }
                DateTime dtNow = DateTime.Now;

                if (dtNow > dtStart && dtNow < dtEnd)
                {
                    strProp = "是";
                }
            }

            //判断当检查目的为空时的错误等级
            EnumOperationErrorLevel strNoInstention = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CanNotAuditWhileNoCheckIntention"));
            EnumOperationErrorLevel Audit_PatSpcialTimeCheck = AuditConfig.GetOpErrLv(dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Audit_PatSpcialTimeCheck"));
            if (drPat != null)
            {
                string strFlag = drPat.RepStatus.ToString();
                string pat_itr_id = drPat.RepItrId;
                string strpat_pre_flag = drPat.RepInitialFlag.ToString();
                result.Data.Patient.PidName = drPat.PidName;
                result.Data.Patient.RepSid = drPat.RepSid;
                result.Data.Patient.PatAgeTxt = drPat.PatAgeTxt;
                result.Data.Patient.PidAgeExp = drPat.PidAgeExp;
                result.Data.Patient.PidAge = drPat.PidAge;
                result.Data.Patient.DeptName = drPat.DeptName;
                result.Data.Patient.PidDeptName = drPat.PidDeptName;
                result.Data.Patient.PidDiag = drPat.PidDiag;
                result.Data.Patient.RepBarCode = drPat.RepBarCode;
                switch (strFlag)
                {
                    case "1":
                        EntityResultQC resultQc = new EntityResultQC();
                        resultQc.ListObrId.Add(pat_id);
                        var listResult = new ObrResultBIZ().GetObrResultQuery(resultQc);
                        var listResultImage = new ObrResultImageBIZ().GetObrResultImage(pat_id);
                        var listReportDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id);

                        if ((listResult == null || listResult.Count == 0) &&
                            (listResultImage == null || listResultImage.Count == 0))
                        {
                            result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            if (!HasValidResult(listResult))
                            {
                                result.AddMessage(EnumOperationErrorCode.NullResult, EnumOperationErrorLevel.Error);
                            }
                            else
                            {
                                CheckNotNullResultIsNull(listResult, listReportDetail, result);
                            }
                        }

                        if (strProp == "否")
                            result.AddMessage(EnumOperationErrorCode.NotAudit, EnumOperationErrorLevel.Error);
                        break;
                    case "2":
                        result.AddMessage(EnumOperationErrorCode.Reported, EnumOperationErrorLevel.Error);
                        break;
                    case "4":
                        result.AddMessage(EnumOperationErrorCode.Printed, EnumOperationErrorLevel.Error);
                        break;
                    default:
                        break;
                }
            }

            string PidPurpId = drPat.PidPurpId;
            result.Data.Patient.PidName = drPat.PidName;
            result.Data.Patient.RepSid = drPat.RepSid;

            if (string.IsNullOrEmpty(PidPurpId))
            {
                result.AddCustomMessage("54t4", "检查目的", string.Format("病人姓名：{1}，样本号：{0},没有检查目的", drPat.RepSid, drPat.PidName), strNoInstention);
            }

            if (Audit_PatSpcialTimeCheck != EnumOperationErrorLevel.None)
            {

                if (drPat.SampSendDate == null)
                {
                    result.AddCustomMessage("223454676", "", "检验时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampCollectionDate == null)
                {
                    result.AddCustomMessage("233454676", "", "标本采集时间为空！", Audit_PatSpcialTimeCheck);
                }
                if (drPat.SampApplyDate == null)
                {
                    result.AddCustomMessage("243454676", "", "标本接收时间为空！", Audit_PatSpcialTimeCheck);
                }

            }
            return result;
        }


        #endregion

    }
}
