using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;
using System.Data;
using dcl.svr.sample;
using System.Text.RegularExpressions;
using dcl.svr.cache;
using dcl.svr.dicbasic;
using dcl.root.logon;
using System.Diagnostics;
using dcl.svr.users;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace dcl.svr.result
{
    public class PidReportMainBIZ : IPidReportMain
    {
        #region 查询

        /// <summary>
        /// 判断该仪器某天是否录入此样本
        /// </summary>
        /// <param name="pat_sid"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public bool ExsitSid(string pat_sid, string itr_id, DateTime pat_date)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_sid, itr_id, pat_date);
            }
            return result;
        }

        /// <summary>
        /// 判断该仪器某天是否录入此序号
        /// </summary>
        /// <param name="pat_host_order"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public bool ExsitPatHostOrder(string pat_host_order, string itr_id, DateTime pat_date)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_host_order, itr_id, pat_date, "1");
            }
            return result;
        }
        /// <summary>
        /// 获取仪器的最大样本号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="excluseSeqRecord">是否排除双向录入结果</param>
        /// <returns></returns>
        public string GetItrSID_MaxPlusOne(DateTime date, string itr_id, bool excluseSeqRecord)
        {
            string strNum = string.Empty;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                //条码录入仪器获取当前两天内的最大样本号(仪器ID,仪器ID2)
                string strGetAllMaxSidItrId = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BarcodeRegisterGetAllMaxSid");
                strNum = mainDao.GetItrSID_MaxPlusOne(date, itr_id, excluseSeqRecord, strGetAllMaxSidItrId);
            }
            return strNum;
        }

        /// <summary>
        /// 获取仪器的最大序号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrHostOrder_MaxPlusOne(DateTime date, string itr_id)
        {
            string strNum = string.Empty;

            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                strNum = mainDao.GetItrHostOrder_MaxPlusOne(date, itr_id);
            }

            return strNum;
        }


        /// <summary>
        /// 根据条码号获取病人资料（条码信息转换成病人信息）
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public EntityPidReportMain GetPatientsByBarCode(string barCode)
        {
            ////获取lis病人资料表结构

            //获取条码病人资料
            SampMainBIZ sampMainBiz = new SampMainBIZ();
            EntitySampMain sampMain = sampMainBiz.SampMainQueryByBarId(barCode);


            EntityPidReportMain patient = new EntityPidReportMain();

            //List<EntityPidReportDetail> dtLISCombineToUnRegister = new List<EntityPidReportDetail>();
            //List<EntityPidReportDetail> dtLisCombineAll = new List<EntityPidReportDetail>();
            //List<EntityPatients> dtPatMergeComid = new List<EntityPatients>();

            if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampBarCode))
            {
                //填充条码病人资料到lis病人资料
                patient = ConvertSampMainToPatient(sampMain);

                //根据条码获取条码明细
                List<EntitySampDetail> dtBCCombine = sampMain.ListSampDetail;

                #region 特殊项目小组合转大组合上机

                //如果是特殊项目合并组合
                //if (!string.IsNullOrEmpty(sampMain.SampMergeComId.ToString())
                //    && dtBCCombine != null && dtBCCombine.Count > 0)
                //{
                //    string tempStrcomid = "";//组合id
                //    string tempStroriid = sampMain.PidOrgId.ToString();//来源 bc_ori_id

                //    //bc_merge_comid的值等于：递增号+,+组合id
                //    if (sampMain.SampMergeComId.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
                //    {
                //        tempStrcomid = sampMain.SampMergeComId.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[1];
                //    }

                //    dcl.pub.entities.dict.EntityDictCombineBar eCombar = dcl.svr.cache.DictCombineBarCache.Current.GetCombineBarWithComID(tempStrcomid, tempStroriid);

                //    if (eCombar != null)
                //    {
                //        if (dtBCCombine.Count > 1)
                //        {
                //            //只保留一条，清除其他
                //            EntitySampDetail tempkeepone = dtBCCombine[0];
                //            dtBCCombine.Clear();
                //            dtBCCombine.Add(tempkeepone);
                //        }

                //        dtBCCombine[0].ComId = eCombar.com_id;
                //        dtBCCombine[0].OrderCode = eCombar.com_his_fee_code;
                //        dtBCCombine[0].CombineName = eCombar.com_print_name;

                //        //如果eCombar.com_print_name没有维护字典时,再获取dict_combine.com_name as bc_lis_code_name
                //        if (string.IsNullOrEmpty(eCombar.com_print_name) && !string.IsNullOrEmpty(eCombar.com_id))
                //        {
                //            dcl.pub.entities.dict.EntityDictCombine temp_eCom = dcl.svr.cache.DictCombineCache.Current.GetCombineByID(eCombar.com_id, true);
                //            if (temp_eCom != null && !string.IsNullOrEmpty(temp_eCom.com_name))
                //            {
                //                dtBCCombine[0].CombineName = temp_eCom.com_name;
                //            }
                //        }

                //        dtPatMergeComid = GetPatientByMergeComid(sampMain.SampMergeComId.ToString());
                //    }
                //}
                #endregion

                //项目序号
                int com_seq = 0;
                foreach (EntitySampDetail drBCCombine in dtBCCombine)//条码检验组合转换为LIS中的病人检验组合
                {
                    EntityPidReportDetail reportDetail = new EntityPidReportDetail();

                    reportDetail.ComId = drBCCombine.ComId;//项目ID
                    reportDetail.OrderCode = drBCCombine.OrderCode;//组合HIS编码
                    reportDetail.OrderPrice = drBCCombine.OrderPrice.ToString();//价格
                    reportDetail.OrderSn = drBCCombine.OrderSn;//医嘱ID
                    reportDetail.SortNo = com_seq;//顺序号
                    reportDetail.SampFlag = drBCCombine.SampFlag;//上机标志
                    reportDetail.PatComName = drBCCombine.ComName;//组合名称
                    reportDetail.RepBarCode = barCode;//条码号
                    reportDetail.ApplyID = drBCCombine.ApplyID;//申请单号

                    patient.ListPidReportDetail.Add(reportDetail);
                }

            }

            return patient;
        }

        /// <summary>
        /// 根据条码信息转换成病人信息
        /// </summary>
        /// <param name="entitySampMain"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatientsBySampleMain(List<EntitySampMain> entitySampMain)
        {
            List<EntityPidReportMain> patientList = new List<EntityPidReportMain>();

            foreach (var sampMain in entitySampMain)
            {
                EntityPidReportMain patient = new EntityPidReportMain();

                if (sampMain != null && !string.IsNullOrEmpty(sampMain.SampBarCode))
                {
                    //填充条码病人资料到lis病人资料
                    patient = ConvertSampMainToPatient(sampMain);

                    //根据条码获取条码明细
                    List<EntitySampDetail> dtBCCombine = sampMain.ListSampDetail;


                    //项目序号
                    int com_seq = 0;
                    foreach (EntitySampDetail drBCCombine in dtBCCombine)//条码检验组合转换为LIS中的病人检验组合
                    {
                        EntityPidReportDetail reportDetail = new EntityPidReportDetail();

                        reportDetail.ComId = drBCCombine.ComId;//项目ID
                        reportDetail.OrderCode = drBCCombine.OrderCode;//组合HIS编码
                        reportDetail.OrderPrice = drBCCombine.OrderPrice.ToString();//价格
                        reportDetail.OrderSn = drBCCombine.OrderSn;//医嘱ID
                        reportDetail.SortNo = com_seq;//顺序号
                        reportDetail.SampFlag = drBCCombine.SampFlag;//上机标志
                        reportDetail.PatComName = drBCCombine.ComName;//组合名称
                        reportDetail.RepBarCode = sampMain.SampBarCode;//条码号
                        reportDetail.ApplyID = drBCCombine.ApplyID;//申请单号

                        patient.ListPidReportDetail.Add(reportDetail);
                    }

                }

                patientList.Add(patient);
            }

         
            return patientList;
        }

        public List<EntityPidReportMain> GetAllLabPat(string labId, DateTime startDate, DateTime endDate)
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPidReportMain> dtPat = new List<EntityPidReportMain>();
            if (mainDao != null)
            {
                dtPat = mainDao.GetAllLabPat(labId, startDate, endDate);
            }
            return dtPat;
        }
        /// <summary>
        /// 查找是否有结果的病人信息  （标本进程）
        /// </summary>
        /// <param name="listItrId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="result">是否有结果</param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatByExistResult(List<string> listItrId, DateTime startDate, DateTime endDate, bool result)
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPidReportMain> dtPat = new List<EntityPidReportMain>();
            if (mainDao != null)
            {
                dtPat = mainDao.GetPatByExistResult(listItrId, startDate, endDate, result);
            }
            return dtPat;
        }
        /// <summary>
        /// 条码特殊合并查询病人资料
        /// </summary>
        /// <param name="BcMergeComid"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatientByMergeComid(string BcMergeComid)
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPidReportMain> dtPat = new List<EntityPidReportMain>();
            if (mainDao != null)
            {
                dtPat = mainDao.GetPatientByMergeComid(BcMergeComid);
            }
            return dtPat;
        }

        /// <summary>
        /// 标本登记，扫描条码时控制并发查询
        /// </summary>
        /// <param name="patItrId">仪器ID</param>
        /// <param name="patBarcode">条码号</param>
        /// <param name="patSid">样本号</param>
        /// <returns></returns>
        public string GetPatientPatId(string patItrId, string patBarcode, string patSid, DateTime repInDate)
        {
            string patId = string.Empty;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            List<EntityPidReportMain> dtPat = new List<EntityPidReportMain>();
            if (mainDao != null)
            {
                patId = mainDao.GetPatientPatId(patItrId, patBarcode, patSid, repInDate);
            }
            return patId;
        }

        /// <summary>
        /// 病人信息查询
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> PatientQuery(EntityPatientQC patientCondition)
        {
            List<EntityPidReportMain> listPat = new List<EntityPidReportMain>();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                listPat = mainDao.PatientQuery(patientCondition);

                //暂时不开启更新TAT时间
                if (false)
                {
                    List<string> listRepId = new List<string>();
                    if (listPat.Count > 0)
                    {
                        foreach (EntityPidReportMain pat in listPat)
                        {
                            listRepId.Add(pat.RepId);
                        }
                    }
                    List<EntityDicCombineTimeRule> listTimeRule = new CombineTimeruleBIZ().GetTATComTimeByRepId(listRepId);
                    List<EntityDicCombineTimeRule> listTimeRuleTemp = new List<EntityDicCombineTimeRule>();
                    foreach (EntityPidReportMain pat in listPat)
                    {
                        listTimeRuleTemp = listTimeRule.Where(w => w.RepId == pat.RepId && w.ComTimeOriId == pat.PidSrcId && w.ComTimeType == pat.RepCtypeName).OrderBy(w => w.ComTime).ToList();
                        if (listTimeRuleTemp.Count > 0)
                        {
                            pat.TatComTime = listTimeRuleTemp[0].ComTime;
                        }
                    }
                }
                listPat = listPat.OrderBy(i => i.RepSerialNum.Length).ThenBy(i => i.RepSerialNum).ToList();
            }
            return listPat;
        }



        /// <summary>
        /// 根据PatId获取标本信息
        /// </summary>
        /// <param name="strPatId"></param>
        /// <returns></returns>
        public EntityPidReportMain GetPatientByPatId(string strPatId)
        {
            return GetPatientByPatId(strPatId, true);
        }

        #endregion

        #region 保存
        /// <summary>
        /// 保存病人资料和组合
        /// </summary>
        /// <param name="caller">操作记录</param>
        /// <param name="listDict">病人信息和明细集合</param>
        /// <returns></returns>
        public EntityOperateResult SavePatient(EntityRemoteCallClientInfo caller, EntityPidReportMain patient)
        {
            EntityOperateResult result = new EntityOperateResult();

            List<EntityPidReportDetail> listPatCombine = patient.ListPidReportDetail;

            if (!string.IsNullOrEmpty(patient.RepBarCode))
            {
                //条码号登记判断并发
                string patId = GetPatientPatId(patient.RepItrId, patient.RepBarCode, patient.RepSid, patient.RepInDate.Value);

                if (!string.IsNullOrEmpty(patId))
                {
                    result.AddMessage(EnumOperateErrorCode.SIDExist, EnumOperateErrorLevel.Error);
                    return result;
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
                    dcl.root.logon.Logger.WriteException("PatInsertBLL", "InsertPatCommonResult", string.Format("patID:{0},pat_age_exp:{1} 无法转换成pat_age", patient.RepId, patient.PidAgeExp));

                }

            }

            //patient.PidRemark = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("DefaultSampleState");

            //时间计算方式
            string Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");
            if (Lab_BarcodeTimeCal == "计算签收时间")
            {
                if (patient.SampApplyDate == null)
                {
                    DateTime pat_jy_date = (DateTime)patient.SampCheckDate;
                    DateTime now = ServerDateTime.GetDatabaseServerDateTime();

                    if (pat_jy_date > now)
                    {
                        patient.SampApplyDate = now;
                    }
                    else
                    {
                        patient.SampApplyDate = patient.SampCheckDate;
                    }
                }
            }

            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetSendingDoctorType") == "LIS编码关联")
            {
                patient.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByCode(patient.PidDoctorCode);

                if (
                    (patient.PidDoctorCode == null)
                    && patient.PidDocName != null
                    )
                {
                    patient.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocIDByName(patient.PidDocName);
                }
            }

            CreatePatCName(patient, listPatCombine);

            string barcode = string.Empty;

            if (!dcl.common.Compare.IsEmpty(patient.RepBarCode))
            {
                barcode = patient.RepBarCode;
            }

            try
            {
                DateTime inDate = ServerDateTime.GetDatabaseServerDateTime();

                result.Data.Patient.RepSid = patient.RepSid;
                result.Data.Patient.PidName = patient.PidName;

                string nowTime = DateTime.Now.ToString(" HH:mm:ss");

                //当录入时间为空时才赋予一个服务器时间
                if (patient.RepInDate == null)
                    patient.RepInDate = inDate;
                else
                    patient.RepInDate = Convert.ToDateTime(patient.RepInDate.Value.ToString("yyyy-MM-dd") + nowTime);

                string pat_id = patient.RepItrId + patient.RepInDate.Value.ToString("yyyyMMdd") + patient.RepSid;
                patient.RepId = pat_id;

                patient.RepStatus = 0;
                //刪除条码重新登记后需判断该条码是否有修改记录
                bool isModify = false;
                List<EntitySampProcessDetail> listDetail = new SampProcessDetailBIZ().GetSampProcessDetail(patient.RepBarCode);
                if (listDetail.Count > 0)
                {
                    foreach (EntitySampProcessDetail detail in listDetail)
                    {
                        if (detail.ProcStatus == "35")
                            isModify = true;
                    }
                }
                if (isModify)
                {
                    patient.RepModifyFrequency = 1;//修改次数
                }
                else
                {
                    patient.RepModifyFrequency = 0;//修改次数
                }
                result.Data.Patient.RepId = pat_id;

                //判断是否已回退
                if (!string.IsNullOrEmpty(barcode) && new SampMainBIZ().Returned(barcode))
                {
                    result.Data.Patient.RepBarCode = barcode;
                    result.AddMessage(EnumOperateErrorCode.HaveReturned, EnumOperateErrorLevel.Error);
                }

                //判断是否存在样本号
                else if (ExsitSid(patient.RepSid, patient.RepItrId, patient.RepInDate.Value))
                {
                    result.AddMessage(EnumOperateErrorCode.SIDExist, EnumOperateErrorLevel.Error);
                }
                else
                {
                    long? host_order = null;

                    if (!Compare.IsEmpty(patient.RepSerialNum))
                    {
                        host_order = Convert.ToInt64(patient.RepSerialNum);
                    }

                    InstrmtBIZ bllItr = new InstrmtBIZ();
                    if (host_order != null && bllItr.GetItrHostFlag(patient.RepItrId) == 2 && ExsitPatHostOrder(host_order.Value.ToString(), patient.RepItrId, patient.RepInDate.Value))
                    {
                        result.AddMessage(EnumOperateErrorCode.HostOrderExist, EnumOperateErrorLevel.Error);
                    }
                    else
                    {

                        if (Compare.IsEmpty(patient.SampCheckDate))
                        {
                            patient.SampCheckDate = ServerDateTime.GetDatabaseServerDateTime();
                        }


                        List<EntityPidReportMain> listPatients = new List<EntityPidReportMain>();

                        //病人组合信息插入前处理
                        //new PidReportDetailBIZ().ReportDetailBeforeInsert(listPatCombine, patient);

                        patient.ListPidReportDetail = listPatCombine;

                        //插入缺省值结果
                        if (!string.IsNullOrEmpty(barcode))
                        {
                            new ObrResultBIZ().InsertDefaultResult(patient.RepId
                                                 , patient.PidSamId
                                                 , patient.RepItrId
                                                 , patient.RepSid
                                                 , listPatCombine);
                        }
                        listPatients.Add(patient);

                        //插入病人资料
                        if (InsertNewPatient(listPatients))
                        {
                            if (!string.IsNullOrEmpty(barcode))
                            {
                                string barcodeRemark = string.Empty;
                                //*************************************************************************************
                                //将序号写入备注中
                                if (host_order.HasValue)
                                {
                                    barcodeRemark = string.Format("仪器：{0}，样本号：{1}, 序号：{2}，登记组合：{3},日期：{4}", patient.ItrName, patient.RepSid, host_order.Value, patient.PidComName, patient.RepInDate);
                                }
                                else
                                {
                                    barcodeRemark = string.Format("仪器：{0}，样本号：{1}，登记组合：{2},日期：{3}", patient.ItrName, patient.RepSid, patient.PidComName, patient.RepInDate);
                                }

                                //************************************************************************************
                                EntitySampOperation operation = new EntitySampOperation();
                                operation.OperationStatus = "20";
                                operation.OperationStatusName = "资料登记";
                                operation.OperationTime = inDate;
                                operation.OperationID = caller.LoginID;
                                operation.OperationName = caller.LoginName;
                                operation.OperationIP = caller.IPAddress;
                                operation.OperationWorkId = caller.UserID;
                                operation.Remark = barcodeRemark;
                                new SampMainBIZ().UpdateSampMainStatusByBarId(operation, barcode);
                            }
                            else
                            {
                                EntitySampProcessDetail detail = new EntitySampProcessDetail();
                                detail.ProcStatus = "20";
                                detail.ProcStatusName = "资料登记";
                                detail.ProcDate = inDate;
                                detail.ProcUsercode = caller.LoginID;
                                detail.ProcUsername = caller.LoginName;
                                detail.ProcBarno = patient.RepBarCode;
                                detail.ProcBarcode = patient.RepBarCode;
                                detail.ProcPlace = caller.IPAddress;
                                detail.ProcContent = string.Empty;
                                detail.RepId = patient.RepId;

                                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(detail);
                            }

                        }
                        else
                            result.AddMessage(EnumOperateErrorCode.Exception, EnumOperateErrorLevel.Error);

                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SaveBarCodePatient", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }
            return result;
        }

        /// <summary>
        /// 数据库基本存储
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public bool InsertNewPatient(List<EntityPidReportMain> patients)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                foreach (EntityPidReportMain item in patients)
                {
                    result = mainDao.InsertNewPatient(item);
                    //插入病人信息成功后插入病人组合信息
                    if (result && item.ListPidReportDetail.Count > 0)
                    {
                        int i = 0;
                        item.ListPidReportDetail.OrderBy(w => w.ComSeq);

                        List<string> listComId = new List<string>();
                        foreach (EntityPidReportDetail detail in item.ListPidReportDetail)
                        {
                            if (!string.IsNullOrEmpty(detail.ComId))
                                detail.RepId = item.RepId;
                            detail.SortNo = i;

                            listComId.Add(detail.ComId);

                            i++;

                        }
                        result = new PidReportDetailBIZ().InsertNewReportDetail(item.ListPidReportDetail);

                        //存在条码号就更新上机标志
                        if (result &&
                            !string.IsNullOrEmpty(item.RepBarCode) &&
                            listComId.Count > 0)
                        {
                            SampDetailBIZ sampDetailBIZ = new SampDetailBIZ();
                            sampDetailBIZ.UpdateSampDetailSampFlagByComId(item.RepBarCode, listComId, "1");
                        }
                    }
                }


            }
            return result;
        }
        /// <summary>
        /// 批量上传数据
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public string BatchUpload(List<EntityPidReportMain> patients)
        {
            string res = "";
            string result = "";
            UploadService.UploadDataSoapClient web = new UploadService.UploadDataSoapClient();
            foreach (EntityPidReportMain pat in patients)
            {
                EntityResultQC qc = new EntityResultQC();
                qc.ListObrId.Add(pat.RepId);
                List<EntityObrResult> listResult = new ObrResultBIZ().ObrResultQuery(qc);
                IFormatter formatter = new BinaryFormatter();
                MemoryStream resultStream = new MemoryStream();
                formatter.Serialize(resultStream, listResult);
                byte[] ResultByte = resultStream.ToArray();
                MemoryStream patStream = new MemoryStream();
                formatter.Serialize(patStream, pat);
                byte[] PatByte = patStream.ToArray();
                res = web.UploadPatInfoAndResult(PatByte, ResultByte);
                IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                //上传成功的数据在本地数据库更新为已审核
                if (string.IsNullOrEmpty(res))
                {
                    if (mainDao != null)
                    {
                        mainDao.UpdateRepStatus(pat.RepId, "2", false);
                    }
                }
                result += res;
            }
            return result;
        }
        #endregion

        #region 细菌保存
        /// <summary>
        /// 保存细菌结果
        /// </summary>
        /// <param name="caller">操作信息</param>
        /// <param name="resultList">参数</param>
        /// <returns></returns>
        public EntityOperateResult SaveBacterialPatient(EntityRemoteCallClientInfo caller, EntityQcResultList resultList)
        {
            EntityOperateResult opresult = new EntityOperateResult();
            EntityPidReportMain patient = resultList.patient;
            opresult = SavePatient(caller, patient);
            List<EntityObrResultAnti> listAnti = resultList.listAnti;
            List<EntityObrResultBact> listBact = resultList.listBact;
            List<EntityObrResultDesc> listDesc = resultList.listDesc;
            if (opresult.Success)
            {
                string repId = opresult.Data.Patient.RepId;
                foreach (EntityObrResultAnti anti in listAnti)
                {
                    anti.ObrId = repId;
                }
                //保存药敏结果
                new ObrResultAntiBIZ().SaveAntiResult(listAnti);
                foreach (EntityObrResultBact bact in listBact)
                {
                    bact.ObrId = repId;
                }
                //保存细菌结果
                new ObrResultBactBIZ().SaveResultBact(listBact);
                foreach (EntityObrResultDesc desc in listDesc)
                {
                    desc.ObrId = repId;
                }
                //保存描述结果
                new ObrResultDescBIZ().InsertObrResultDesc(listDesc);
            }
            return opresult;
        }
        #endregion

        #region 更新

        /// <summary>
        /// 批量修改病人资料
        /// </summary>
        /// <param name="listPat"></param>
        /// <returns></returns>
        public bool UpdatePatientData(List<EntityPidReportMain> listPat)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                try
                {
                    foreach (EntityPidReportMain patient in listPat)
                    {
                        mainDao.UpdatePatientData(patient);
                    }
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }
        /// <summary>
        /// 更新病人基本信息前的一些处理
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        public void UpdatePatientInfoBefore(EntityPidReportMain PatientInfo, EntityPidReportMain OriginPatInfo, EntityOperationResult result, OperationLogger opLogger, out bool changePatID, out string newPatId)
        {
            changePatID = false;
            newPatId = "";
            if (result.Success)
            {

                //样本号
                string pat_sid = PatientInfo.RepSid;

                //仪器ID
                string itr_id = PatientInfo.RepItrId;

                //日期
                DateTime pat_date = Convert.ToDateTime(PatientInfo.RepInDate);

                //查找之前的样本号
                if (OriginPatInfo != null)
                {
                    string prevSID = OriginPatInfo.RepSid;

                    System.Reflection.PropertyInfo[] mPi = typeof(EntityPidReportMain).GetProperties();
                    for (int i = 0; i < mPi.Length; i++)
                    {
                        System.Reflection.PropertyInfo pi = mPi[i];
                        if (pi.Name != "ListPidReportDetail" || pi.Name != "DestItmIds")
                        {
                            object oldValue = pi.GetValue(OriginPatInfo, null);
                            object newValue = pi.GetValue(PatientInfo, null);
                            if (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString())
                            {
                                string colCHS = FieldsNameConventer<PatientFields>.Instance.GetFieldCHS(pi.Name);
                                if (!string.IsNullOrEmpty(colCHS))
                                {
                                    opLogger.Add_ModifyLog(SysOperationLogGroup.PAT_INFO, colCHS, oldValue + "→" + newValue);
                                }
                            }
                        }
                    }

                    //原样本号和当前样本号不同
                    if (prevSID != PatientInfo.RepSid)
                    {
                        //查找当前样本号是否已存在
                        bool isExsit = ExsitSid(pat_sid, itr_id, pat_date);
                        if (isExsit)
                        {
                            //存在,返回错误信息
                            result.AddMessage(EnumOperationErrorCode.SIDExist, EnumOperationErrorLevel.Error);
                        }
                        else
                        {
                            changePatID = true;
                        }
                    }

                    if (result.Success)
                    {
                        try
                        {
                            //更新病人修改信息的次数 新逻辑：修改为1，未修改为0，无论修改多少次。
                            if (PatientInfo.RepModifyFrequency == 0)
                            {
                                UpdateRepModifyFrequencyByRepId(PatientInfo.RepModifyFrequency, PatientInfo.RepId);
                                PatientInfo.RepModifyFrequency = PatientInfo.RepModifyFrequency + 1;
                            }
                            if (changePatID)
                            {
                                newPatId = ResultoHelper.GenerateResID(PatientInfo.RepItrId, DateTime.Parse(PatientInfo.RepInDate.ToString()), pat_sid);

                                //有改变才更新
                                if (newPatId != PatientInfo.RepId)
                                {
                                    UpdateNewRepIdByOldRepId(newPatId, PatientInfo.RepId);
                                    PatientInfo.RepId = newPatId;
                                    Lib.LogManager.Logger.LogInfo("UpdatePatientInfo，patID改变，日志记录", "旧的patId:" + PatientInfo.RepId + "->" + "新的patId:" + newPatId);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                            Lib.LogManager.Logger.LogException("UpdatePatientInfo", ex);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 根据原来的标识ID更新新的标识ID
        /// </summary>
        /// <param name="newRepId"></param>
        /// <param name="repId"></param>
        /// <returns></returns>
        public bool UpdateNewRepIdByOldRepId(string newRepId, string repId)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.UpdateNewRepIdByOldPatId(newRepId, repId);
            }
            return result;
        }
        /// <summary>
        /// 根据病人的标识id更新病人修改信息次数
        /// </summary>
        /// <param name="repModifyFrequency">次数</param>
        /// <param name="repId">标识ID</param>
        /// <returns></returns>
        public bool UpdateRepModifyFrequencyByRepId(int repModifyFrequency, string repId)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.UpdateRepModifyFrequencyByRepId(repModifyFrequency, repId);
            }
            return result;
        }
        /// <summary>
        /// 根据标识ID更新病人的组合名称
        /// </summary>
        /// <param name="pidComName">组合名称</param>
        /// <param name="repId"></param>
        /// <returns></returns>
        public bool UpdatePidComNameByRepId(string pidComName, string repId)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.UpdatePidComNameByRepId(pidComName, repId);
            }
            return result;
        }

        /// <summary>
        /// 更新病人的状态为打印状态，更新打印时间
        /// </summary>
        /// <param name="listRepId"></param>
        /// <param name="repStatus"></param>
        public void UpdatePrintState(IEnumerable<string> listRepId, string repStatus)
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                foreach (string repId in listRepId)
                {
                    mainDao.UpdateRepStatus(repId, repStatus, true);
                }
            }
        }

        public void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string repStatus, string OperatorID, string OperatorName, string strPlace)
        {
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                foreach (string repId in repIds)
                {
                    //更新病人的状态为打印状态
                    mainDao.UpdateRepStatus(repId, repStatus, true);
                    EntityPatientQC qc = new EntityPatientQC();
                    qc.RepId = repId;
                    //查询该标识ID的病人信息
                    List<EntityPidReportMain> listPatient = PatientQuery(qc);
                    EntityPidReportMain patient = new EntityPidReportMain();
                    if (listPatient != null && listPatient.Count > 0)
                    {
                        patient = listPatient[0];
                    }
                    EntitySampProcessDetail detail = new EntitySampProcessDetail();
                    detail.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
                    detail.ProcUsercode = OperatorID;
                    detail.ProcUsername = OperatorName;
                    detail.ProcStatus = "100";
                    detail.ProcBarno = patient.RepBarCode;
                    detail.ProcBarcode = patient.RepBarCode;
                    detail.ProcPlace = strPlace;
                    detail.ProcContent = "检验报告管理模块";
                    detail.RepId = repId;
                    //插入条码流转明细
                    new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(detail);
                }
            }
        }
        #endregion

        #region 辅助方法

        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private int[] SortCombine(int[] a)
        {
            int[] tempArry = new int[a.Length];
            for (int i = 0; i < tempArry.Length; i++)
            {
                tempArry[i] = i;
            }
            for (int i = 0; i < a.Length; i++)
            {
                for (int j = 0; j < a.Length - 1; j++)
                {
                    if (a[j] > a[j + 1])
                    {
                        int temp = a[j];
                        a[j] = a[j + 1];
                        a[j + 1] = temp;

                        int temp1 = tempArry[j];
                        tempArry[j] = tempArry[j + 1];
                        tempArry[j + 1] = temp1;
                    }
                }
            }

            return tempArry;
        }

        /// <summary>
        /// 条码病人表资料转换为病人资料表
        /// </summary>
        /// <param name="sampMain"></param>
        /// <param name="dtLisPat"></param>
        private EntityPidReportMain ConvertSampMainToPatient(EntitySampMain sampMain)
        {
            EntityPidReportMain drPatInfo = new EntityPidReportMain();

            //姓名
            drPatInfo.PidName = sampMain.PidName;

            //性别
            string sex = "0";
            if (!string.IsNullOrEmpty(sampMain.PidSex))
            {
                if (sampMain.PidSex.ToString() == "男"
                    || sampMain.PidSex.ToString() == "1")
                {
                    sex = "1";
                }
                else if (sampMain.PidSex.ToString() == "女"
                    || sampMain.PidSex.ToString() == "2"
                    )
                {
                    sex = "2";
                }
                else
                {
                    sex = string.Empty;
                }
            }

            drPatInfo.PidSex = sex;

            //年龄
            if (sampMain.PidAge != null && !string.IsNullOrEmpty(sampMain.PidAge))
            {
                //目前只截取年
                string age = sampMain.PidAge.ToString();

                if (age.ToLower().Contains('y')
                && age.ToLower().Contains('m')
                && age.ToLower().Contains('d')
                && age.ToLower().Contains('h')
                && age.ToLower().Contains('i')
                )
                {
                    drPatInfo.PidAgeExp = age;
                }
                else
                {
                    int intAge;
                    age = age.Trim().Split('.')[0];
                    if (age != null && age.Length > 0)
                    {
                        if (
                            age.Contains("Y")
                            && age.Contains("M")
                            && age.Contains("D")
                            && age.Contains("H")
                            && age.Contains("I")
                            )
                        {

                        }
                        else if (int.TryParse(age, out intAge))
                        {
                            age = age + "Y0M0D0H0I";
                        }
                        else//老outlink
                        {
                            age = age.ToUpper().Replace('年', 'Y')
                                   .Replace('岁', 'Y')
                                   .Replace("个月", "M")
                                   .Replace('月', 'M')
                                   .Replace('日', 'D')
                                   .Replace('天', 'D')
                                   .Replace("小时", "H")
                                   .Replace('时', 'H')
                                   .Replace("分钟", "I")
                                   .Replace('分', 'I');

                            string patten = "(Y|D|M|H|I)";
                            string[] tmp = Regex.Split(age, patten);
                            string[] tmp2 = new string[tmp.Length];
                            int count = 0;
                            for (int i = 0; i < tmp.Length; i = i + 2)
                            {
                                if (i + 1 >= tmp.Length)
                                    continue;
                                tmp2[count] = tmp[i] + tmp[i + 1];
                                count++;
                            }
                            string year = null;
                            string month = null;
                            string day = null;
                            string hour = null;
                            string minute = null;
                            foreach (string s in tmp2)
                            {
                                if (string.IsNullOrEmpty(s))
                                    continue;

                                if (s.Contains("Y") && year == null)
                                    year = s;

                                if (s.Contains("M") && month == null)
                                    month = s;

                                if (s.Contains("D") && day == null)
                                    day = s;

                                if (s.Contains("H") && hour == null)
                                    hour = s;

                                if (s.Contains("I") && minute == null)
                                    minute = s;
                            }
                            if (year == null) year = "0Y";
                            if (month == null) month = "0M";
                            if (day == null) day = "0D";
                            if (hour == null) hour = "0H";
                            if (minute == null) minute = "0I";
                            age = year + month + day + hour + minute;
                        }
                    }
                    drPatInfo.PidAgeExp = age;
                }
            }

            //病人来源
            drPatInfo.SrcName = sampMain.PidSrcName;
            drPatInfo.PidSrcId = sampMain.PidSrcId;
            //所属院区
            if (!string.IsNullOrEmpty(sampMain.PidOrgId))
            {
                drPatInfo.PidOrgId = sampMain.PidOrgId;
            }

            if (!string.IsNullOrEmpty(drPatInfo.PidAgeExp))
                drPatInfo.PidAge = AgeConverter.AgeValueTextToMinute(drPatInfo.PidAgeExp.ToString());

            if (sampMain.ReachDate != null)//送达时间
            {
                drPatInfo.SampReachDate = Convert.ToDateTime(sampMain.ReachDate);
            }

            DateTime now = ServerDateTime.GetDatabaseServerDateTime();
            string Lab_BarcodeTimeCal = "佛山市一";
            Lab_BarcodeTimeCal = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_BarcodeTimeCal");

            if (Lab_BarcodeTimeCal == "佛山市一")
            {
                #region 佛山市一
                //如果打印时间为空，就用条码生成日期作为打印时间
                if (sampMain.SampPrintDate == null)
                {
                    sampMain.SampPrintDate = sampMain.SampDate;
                }

                //送检时间
                if (!Compare.IsEmpty(sampMain.ReachDate))//送达时间 不为空
                {
                    //如果送达时间不为空，将送达时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReachDate);
                }
                else if (!Compare.IsEmpty(sampMain.ReceiverDate))//签收时间 不为空
                {
                    //如果签收时间不为空，将签收时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReceiverDate);
                }
                else
                {
                    drPatInfo.SampSendDate = now;
                }

                //采样时间
                if (!Compare.IsEmpty(drPatInfo.PidSrcId))
                {
                    string ori_id = drPatInfo.PidSrcId.ToString();
                    DateTime dtSended = Convert.ToDateTime(drPatInfo.SampSendDate);

                    //采血(采样)时间
                    if (ori_id == "108")//住院
                    {
                        if (Compare.IsEmpty(sampMain.CollectionDate))
                        {
                            drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = Convert.ToDateTime(sampMain.CollectionDate);
                            if (bc_blood_date < dtSended.AddMinutes(-90))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                            }
                            else if (bc_blood_date > dtSended.AddMinutes(-20))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-20);
                            }
                            else
                            {
                                drPatInfo.SampCollectionDate = bc_blood_date;
                            }
                        }
                    }
                    else if (ori_id == "107" || ori_id == "109")
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
                        }
                    }
                    else
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
                        }
                    }
                }
                else
                {
                    if (sampMain.CollectionDate != null)
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                    }
                    else
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
                    }
                }
                //如果条码医嘱时间为空，将样本采集时间赋值给条码医嘱时间
                if (sampMain.SampOccDate == null)
                {
                    sampMain.SampOccDate = Convert.ToDateTime(drPatInfo.SampCollectionDate);
                }

                //医嘱执行时间(申请时间)
                drPatInfo.SampReceiveDate = sampMain.SampOccDate;
                if (Convert.ToDateTime(drPatInfo.SampCollectionDate) < Convert.ToDateTime(drPatInfo.SampReceiveDate)
                    && Convert.ToDateTime(drPatInfo.SampReceiveDate) <= Convert.ToDateTime(drPatInfo.SampSendDate)
                    )
                {
                    drPatInfo.SampCollectionDate = drPatInfo.SampReceiveDate;
                }
                #endregion
            }
            else if (Lab_BarcodeTimeCal == "清远人医")
            {
                #region 清远人医
                //如果打印时间为空，就用条码生成日期作为打印时间
                if (sampMain.SampPrintDate == null)
                {
                    sampMain.SampPrintDate = sampMain.SampDate;
                }

                //送检时间
                if (!Compare.IsEmpty(sampMain.ReachDate))//送达时间 不为空
                {
                    //如果送达时间不为空，将送达时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReachDate);
                }
                else if (!Compare.IsEmpty(sampMain.ReceiverDate))//签收时间 不为空
                {
                    //如果签收时间不为空，将签收时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.ReceiverDate);
                }
                else if (!Compare.IsEmpty(sampMain.SendDate))//送检时间 不为空
                {
                    //如果送检时间不为空，将送检时间赋值给送检时间
                    drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.SendDate);
                }
                else
                {
                    drPatInfo.SampSendDate = now;
                }


                ////采样时间
                if (!Compare.IsEmpty(drPatInfo.PidSrcId))
                {
                    string ori_id = drPatInfo.PidSrcId.ToString();
                    DateTime dtSended = Convert.ToDateTime(drPatInfo.SampSendDate);

                    //采血(采样)时间
                    if (ori_id == "108")//住院
                    {
                        if (Compare.IsEmpty(sampMain.CollectionDate))
                        {
                            drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                        }
                        else
                        {
                            DateTime bc_blood_date = Convert.ToDateTime(sampMain.CollectionDate);
                            if (bc_blood_date < dtSended.AddMinutes(-90))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-90);
                            }
                            else if (bc_blood_date > dtSended.AddMinutes(-20))
                            {
                                drPatInfo.SampCollectionDate = dtSended.AddMinutes(-20);
                            }
                            else
                            {
                                drPatInfo.SampCollectionDate = bc_blood_date;
                            }
                        }
                    }
                    else if (ori_id == "107" || ori_id == "109")
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
                        }
                    }
                    else
                    {
                        if (sampMain.CollectionDate != null)
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                        }
                        else
                        {
                            drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
                        }
                    }
                }
                else
                {
                    if (sampMain.CollectionDate != null)
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                    }
                    else
                    {
                        drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SampPrintDate);
                    }
                }

                if (sampMain.SampOccDate == null)
                {
                    sampMain.SampOccDate = Convert.ToDateTime(drPatInfo.SampCollectionDate);
                }

                //医嘱执行时间(申请时间)
                drPatInfo.SampReceiveDate = sampMain.SampOccDate;
                if (Convert.ToDateTime(drPatInfo.SampCollectionDate) < Convert.ToDateTime(drPatInfo.SampReceiveDate)
                    && Convert.ToDateTime(drPatInfo.SampReceiveDate) <= Convert.ToDateTime(drPatInfo.SampSendDate)
                    )
                {
                    drPatInfo.SampCollectionDate = drPatInfo.SampReceiveDate;
                }
                #endregion
            }
            else if (Lab_BarcodeTimeCal == "中山人医")
            {
                #region 中山人医
                //如果打印时间为空，就用条码生成日期作为打印时间
                if (sampMain.SampPrintDate == null)
                {
                    sampMain.SampPrintDate = sampMain.SampDate;
                }

                if (sampMain.CollectionDate != null)//采样时间
                {
                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                }

                //采集时间为空，则标本收取时间作为采集时间
                if (string.IsNullOrEmpty(sampMain.CollectionDate.ToString())
                    && !string.IsNullOrEmpty(sampMain.SendDate.ToString()))
                {
                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.SendDate).AddMinutes(-new Random().Next(3, 9));
                }

                if (sampMain.SendDate != null)//送检时间(收取)
                {
                    drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.SendDate);
                }

                if (sampMain.ReceiverDate != null)//接收时间
                {
                    drPatInfo.SampApplyDate = Convert.ToDateTime(sampMain.ReceiverDate);
                }

                if (sampMain.SampOccDate != null)//申请时间
                {
                    drPatInfo.SampReceiveDate = Convert.ToDateTime(sampMain.SampOccDate);
                }

                if (sampMain.ReachDate != null)//送达时间
                {
                    drPatInfo.SampReachDate = Convert.ToDateTime(sampMain.ReachDate);
                }

                #endregion
            }
            else
            {
                if (sampMain.CollectionDate != null)//采样时间
                {
                    drPatInfo.SampCollectionDate = Convert.ToDateTime(sampMain.CollectionDate);
                }

                if (sampMain.SendDate != null)//送检时间(收取)
                {
                    drPatInfo.SampSendDate = Convert.ToDateTime(sampMain.SendDate);
                }

                if (sampMain.ReceiverDate != null)//接收时间
                {
                    drPatInfo.SampApplyDate = Convert.ToDateTime(sampMain.ReceiverDate);
                }

                if (sampMain.SampOccDate != null)//申请时间
                {
                    drPatInfo.SampReceiveDate = Convert.ToDateTime(sampMain.SampOccDate);
                }

                if (sampMain.ReachDate != null)//送达时间
                {
                    drPatInfo.SampReachDate = Convert.ToDateTime(sampMain.ReachDate);
                }

            }
            int iAdmissTimes = 0;

            if (int.TryParse(sampMain.PidAdmissTimes.ToString(), out iAdmissTimes))
            {
                drPatInfo.PidAddmissTimes = iAdmissTimes;
            }
            else
            {
                drPatInfo.PidAddmissTimes = 0;
            }

            //标本备注
            drPatInfo.SampRemark = sampMain.SampRemark;

            //ID类型
            drPatInfo.PidIdtId = sampMain.PidIdtId;

            //接收时间
            drPatInfo.SampApplyDate = sampMain.ReceiverDate;

            drPatInfo.SampCheckDate = now;

            //条码
            drPatInfo.RepBarCode = sampMain.SampBarCode;

            //病床号
            drPatInfo.PidBedNo = sampMain.PidBedNo;

            //ID
            drPatInfo.PidInNo = sampMain.PidInNo;

            //病区code
            drPatInfo.PidWardId = sampMain.PidDeptCode;

            //病区名称
            drPatInfo.PidWardName = string.Empty;

            //送检科室名称
            drPatInfo.PidDeptName = sampMain.PidDeptName;

            drPatInfo.PidSocialNo = sampMain.PidSocialNo;

            //送检科室code
            drPatInfo.PidDeptId = string.Empty;

            if ((CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "通用"
                || CacheSysConfig.Current.GetSystemConfig("GetPatientsInfoType") == "outlink")
                && !Compare.IsNullOrDBNull(sampMain.PidDeptCode))
            {
                drPatInfo.PidDeptId = sampMain.PidDeptCode.ToString();
            }

            //联系地址
            drPatInfo.PidAddress = sampMain.PidAddress;

            //联系电话
            drPatInfo.PidTel = sampMain.PidTel;

            if (!Compare.IsEmpty(sampMain.PidDoctorCode))//如果医生工号不为空
            {
                drPatInfo.PidDoctorCode = sampMain.PidDoctorCode.ToString();
            }
            else
            {
                if (!Compare.IsEmpty(sampMain.PidDoctorName))//如果医生姓名不为空,则用医生姓名查找出医生code
                {
                    drPatInfo.PidDoctorCode = dcl.svr.cache.DictDoctorCache.Current.GetDocCodeByName(sampMain.PidDoctorName.ToString());
                }
            }

            //开单医生姓名
            drPatInfo.PidDocName = sampMain.PidDoctorName;
            drPatInfo.DoctorName = sampMain.PidDoctorName;

            //临床诊断
            drPatInfo.PidDiag = sampMain.PidDiag;

            if (sampMain.PidBirthday != null)//出生日期
            {
                drPatInfo.PidBirthday = Convert.ToDateTime(sampMain.PidBirthday);
            }

            //条码状态
            drPatInfo.BcStatus = sampMain.SampStatusId;

            drPatInfo.SampType = sampMain.SampType;
            //打印标志
            drPatInfo.SampPrintFlag = sampMain.SampPrintFlag.ToString();

            //标本类别
            drPatInfo.SamName = sampMain.SampSamName;
            drPatInfo.PidSamId = sampMain.SampSamId;

            //检查类型
            if (sampMain.SampUrgentFlag)
            {
                drPatInfo.RepCtype = "2";
            }
            else
            {
                drPatInfo.RepCtype = "1";
            }

            //检查类型
            if (sampMain.SampUrgentStatus.ToString() == "2")
            {
                drPatInfo.RepCtype = "4";
            }
            //+++++++++ 2010-9-26 ++++++++++++
            //自定义ID
            if (!Compare.IsNullOrDBNull(sampMain.PidPatno))
            {
                drPatInfo.RepInputId = sampMain.PidPatno.ToString();
            }

            //唯一号UPID 目前滨海使用
            if (!Compare.IsNullOrDBNull(sampMain.PidUniqueId))
            {
                drPatInfo.PidUniqueId = sampMain.PidUniqueId.ToString();
            }
            //身份证号码
            drPatInfo.PidIdentityCard = sampMain.PidIdentityCard;
            //人员身份
            if (!Compare.IsNullOrDBNull(sampMain.PidIdentity))
            {
                drPatInfo.PidIdentity = sampMain.PidIdentity;
            }
            //病人身份
            if (!Compare.IsNullOrDBNull(sampMain.PidIdentityName))
            {
                drPatInfo.PidIdentityName = sampMain.PidIdentityName;
            }
            //保存拆分大组合(特殊合并)ID
            if (!Compare.IsNullOrDBNull(sampMain.SampMergeComId))
            {
                drPatInfo.BcMergeComid = sampMain.SampMergeComId.ToString();
            }

            //申请单号
            if (!Compare.IsNullOrDBNull(sampMain.SampApplyNo)
                )
            {
                drPatInfo.PidApplyNo = sampMain.SampApplyNo.ToString();
            }

            //费用类别
            drPatInfo.PidInsuId = sampMain.PidInsuId;

            //体检id
            drPatInfo.PidExamNo = sampMain.PidExamNo;

            //体检单位id
            if (!string.IsNullOrEmpty(sampMain.PidExamCompany))
            {
                drPatInfo.PidExamCompany = sampMain.PidExamCompany;
            }
            else
            {
                drPatInfo.PidExamCompany = sampMain.PidExamCompanyName;
            }
            //如果体检ID不为空，则更新病人来源为体检
            if (!Compare.IsEmpty(sampMain.PidExamNo))
            {
                drPatInfo.PidSrcId = "109";
                drPatInfo.SrcName = "体检";
            }
            drPatInfo.HISSerialnum = sampMain.SampPackNo;
            drPatInfo.HISPatientID = sampMain.PidPatno;
            return drPatInfo;
        }

        /// <summary>
        /// 处理组合排序和拼接组合名称
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="listCombine"></param>
        public void CreatePatCName(EntityPidReportMain patient, List<EntityPidReportDetail> listCombine)
        {
            //***************************************************************************//
            //将所选组合排序
            int[] a = new int[listCombine.Count];
            for (int i = 0; i < a.Length; i++)
            {
                if ((listCombine[i].ComSeq == null) || (listCombine[i].ComSeq == ""))
                    a[i] = 99999;
                else
                    a[i] = Convert.ToInt32(listCombine[i].ComSeq);
            }
            a = SortCombine(a);

            string pat_c_name = string.Empty;

            bool needPlus = false;
            for (int i = 0; i < listCombine.Count; i++)
            {
                if (needPlus)
                {
                    pat_c_name += "+";
                }
                //根据项目组合中的顺序加入
                pat_c_name += listCombine[a[i]].PatComName;

                needPlus = true;
            }

            patient.PidComName = pat_c_name;
        }

        #endregion


        #region 删除
        public bool DeletePatient(string repId)
        {
            bool result = false;
            IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (dao != null)
            {
                result = dao.DeletePatient(repId);
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 获取病人列表(简要信息)
        /// 查找指定组别的所有病人资料时,把itr_id赋空字串
        /// </summary>
        /// <param name="dtFrom">起始日期</param>
        /// <param name="dtTo">结束日期</param>
        /// <param name="type_id">物理组ID</param>
        /// <param name="itr_id">仪器ID</param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatientsList_Resume(EntityRemoteCallClientInfo caller, DateTime dtFrom, DateTime dtTo, string type_id, string itr_id, bool allowEmptyType, bool allowEmptyItr)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<string> ItrIdList = new List<string>();

            //没有输入仪器，则查找物理组别中的所有仪器
            if (itr_id == string.Empty || itr_id == null && allowEmptyItr)
            {
                IDaoDicInstrument instrumtDao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
                List<EntityDicInstrument> listInst = new List<EntityDicInstrument>();
                if (instrumtDao != null)
                {
                    listInst = instrumtDao.Search(new List<String> { type_id });
                }
                foreach (EntityDicInstrument dr in listInst)
                {
                    ItrIdList.Add(dr.ItrId);
                }
            }
            else
            {
                ItrIdList.Add(itr_id);
            }

            string auditWord = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
            if (auditWord == string.Empty)
            {
                auditWord = "审核";
            }

            string reportWord = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
            if (reportWord == string.Empty)
            {
                reportWord = "报告";
            }

            List<EntityPidReportMain> PatientsList = new List<EntityPidReportMain>();

            List<EntityPidReportMain> dtPat = new List<EntityPidReportMain>();
            EntityPatientQC PatientQC = new EntityPatientQC();
            PatientQC.ListItrId = ItrIdList;
            PatientQC.auditWord = auditWord;
            PatientQC.reportWord = reportWord;
            PatientQC.DateStart = dtFrom.Date;
            PatientQC.DateEnd = dtTo.Date.AddDays(1);
            PatientsList = PatientQuery(PatientQC);

            //去掉sql中的排序，排序用代码实现 2014年4月10日11:27:40
            List<EntityPidReportMain> PatientsListReturn = new List<EntityPidReportMain>();
            foreach (EntityPidReportMain Patients in PatientsList)
            {
                if (!string.IsNullOrEmpty(Patients.PidAgeExp))
                {
                    string patage = Patients.PidAgeExp.ToString();

                    patage = AgeConverter.TrimZeroValue(patage);
                    patage = AgeConverter.ValueToText(patage);
                    Patients.PidAgeExp = patage;
                }


                if (!string.IsNullOrEmpty(Patients.RepCtype)
                    && Patients.RepStatus != null && Patients.RepStatus.ToString() != "" && Patients.RepStatus.ToString() != "0"
                    && Patients.RepCtype.ToString() == "3" && caller != null && !string.IsNullOrEmpty(caller.LoginID)
                    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_illReportNotAllowPrintMZ") == "是")
                {
                    bool hasFunc = CacheUserInfo.Current.HasFunctionByLoginID(caller.LoginID, 214);
                    if (!hasFunc)
                        continue;
                }
                PatientsListReturn.Add(Patients);
            }
            sw.Stop();
            Debug.WriteLine(string.Format("GetPatientsList_Resume {0}ms", sw.ElapsedMilliseconds));
            PatientsListReturn = PatientsListReturn.OrderBy(i => i.RepSerialNum.Length).ThenBy(i => i.RepSerialNum).ToList();
            return PatientsListReturn;
        }

        /// <summary>
        /// 获取病人资料状态（必录项目已齐，超时报告等）
        /// </summary>
        /// <param name="date"></param>
        /// <param name="pat_itr_id"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetPatientStatus(DateTime startDate, DateTime endDate, string pat_itr_id)
        {
            try
            {
                #region 查找是否有结果
                IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                EntityPatientQC PatientQC = new EntityPatientQC();
                PatientQC.ListItrId.Add(pat_itr_id);
                PatientQC.DateStart = startDate.Date;
                PatientQC.DateEnd = endDate.Date.AddDays(1);

                //List<EntityPatients> patientsList = PatientQuery(PatientQC);
                List<EntityPidReportMain> patientsList = GetPatientStateForQueryIsResult(PatientQC);
                #endregion

                #region 查找必录项目是否已齐全
                bool Lab_ShowColorForOverTime = CacheSysConfig.Current.GetSystemConfig("Lab_ShowColorForOverTime") == "是";

                foreach (EntityPidReportMain row in patientsList)
                {
                    string pat_id = row.RepId.ToString();
                    //查找是否有超时
                    string sql3 = string.Empty;
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ShowComEmergentStatus") == "是")
                    {
                        //惠州三院添加组合紧急状态颜色列
                        EntityPatientQC patientQC = new EntityPatientQC();
                        patientQC.RepId = pat_id;
                        List<EntityPidReportMain> PatientsList = PatientQuery(PatientQC);
                        if (PatientsList != null && PatientsList.Count > 0)
                            row.ComLineColor = Convert.ToInt32(PatientsList[0].ComLineColor);
                    }
                    else
                    {
                        row.ComLineColor = 0;
                    }

                    string pat_ctype = row.RepCtype.ToString();

                    if (row.Status.ToString() == "0")
                    {
                        row.ResStatus = 0;
                    }
                    if (row.Status.ToString() == "1" && row.RepStatus.ToString() == "0")
                    {

                        //项目名称相同的其中一个项目有结果则默认另外的项目也有结果
                        if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_SameItmName_OneItmHasResAnotherHasRes") == "是")
                        {

                            bool appsql = false;
                            List<EntityDicCombineDetail> ItmComList = new List<EntityDicCombineDetail>();
                            List<EntityObrResult> obrResultList = new List<EntityObrResult>();
                            if (CacheSysConfig.Current.GetSystemConfig("Lab_FlagTipNoInClitem") == "是")
                                appsql = true;
                            ItemCombineBIZ ItmComBIZ = new ItemCombineBIZ();
                            if (ItmComBIZ != null)
                            {
                                ItmComList = ItmComBIZ.GetItmNameByRepId(row.RepId, appsql);
                            }
                            ObrResultBIZ resultBIZ = new ObrResultBIZ();
                            if (resultBIZ != null)
                            {
                                EntityResultQC ResultQC = new EntityResultQC();
                                ResultQC.ObrFlag = "1";
                                ResultQC.ListObrId.Add(row.RepId);
                                ResultQC.ResChrIsNull = true;
                                obrResultList = resultBIZ.ObrResultQuery(ResultQC);
                            }

                            if (ItmComList != null && ItmComList.Count > 0)
                            {
                                if (obrResultList != null && obrResultList.Count > 0)
                                {
                                    foreach (EntityDicCombineDetail ItmCom in ItmComList)
                                    {
                                        if (obrResultList.Where(i => i.ItmName == ItmCom.ComItmName).ToList().Count <= 0)
                                        {
                                            row.Status = 2;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    row.Status = 2;
                                }
                            }
                        }
                        else
                        {
                            #region  判断是否缺少必录项目
                            bool appsql = false;
                            List<EntityDicCombineDetail> ItmComList = new List<EntityDicCombineDetail>();
                            List<EntityObrResult> obrResultList = new List<EntityObrResult>();
                            if (CacheSysConfig.Current.GetSystemConfig("Lab_FlagTipNoInClitem") == "是")
                                appsql = true;
                            ItemCombineBIZ ItmComBIZ = new ItemCombineBIZ();
                            if (ItmComBIZ != null)
                            {
                                ItmComList = ItmComBIZ.GetItmNameByRepId(row.RepId, appsql);
                            }
                            ObrResultBIZ resultBIZ = new ObrResultBIZ();
                            if (resultBIZ != null)
                            {
                                EntityResultQC ResultQC = new EntityResultQC();
                                ResultQC.ObrFlag = "1";
                                ResultQC.ListObrId.Add(row.RepId);
                                ResultQC.ResChrIsNull = true;
                                obrResultList = resultBIZ.ObrResultQuery(ResultQC);
                            }
                            //描述结果为空
                            if (obrResultList.Count == 0)
                            {
                               List<EntityObrResultDesc> desc = new ObrResultDescBIZ().GetDescResultById(row.RepId);
                                if(desc.Count >0 && (desc[0].ObrDescribe == null || string.IsNullOrEmpty(desc[0].ObrDescribe.ToString())))
                                {
                                    row.Status = 2;
                                }
                            }
                            if (ItmComList != null && ItmComList.Count > 0)
                            {
                                if (obrResultList != null && obrResultList.Count > 0)
                                {
                                    foreach (EntityDicCombineDetail ItmCom in ItmComList)
                                    {
                                        if (obrResultList.Where(i => i.ItmId == ItmCom.ComItmId).ToList().Count <= 0)
                                        {
                                            row.Status = 2;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    row.Status = 2;
                                }
                            }
                            #endregion
                        }
                        List<EntityPidReportMain> patientList = new List<EntityPidReportMain>();
                        EntityPatientQC patientQc = new EntityPatientQC();
                        patientQc.RepId = pat_id;
                        row.ResStatus = row.Status;
                        if (pat_ctype != "2")
                        {
                            patientQc.IsEnabled = "1";
                            if (mainDao != null)
                            {
                                patientList = mainDao.GetPatientsCount(patientQc);
                            }
                        }
                        else
                        {
                            patientQc.IsEnabled = "2";
                            if (mainDao != null)
                            {
                                patientList = mainDao.GetPatientsCount(patientQc);
                            }
                        }
                        if (patientList.Count > 0)
                            row.Status = 3;
                    }
                    string IsOverTime = string.Empty;
                    if ((row.RepStatus.ToString() == "1" || row.RepStatus.ToString() == "0") &&
                        Lab_ShowColorForOverTime)
                    {
                        if (pat_ctype != "2")
                        {
                            if (mainDao != null)
                            {
                                IsOverTime = mainDao.GetPatientIsOverTime(pat_id, "常规");
                            }
                        }
                        else
                        {
                            if (mainDao != null)
                            {
                                IsOverTime = mainDao.GetPatientIsOverTime(pat_id, "急查");
                            }
                        }
                        if (!string.IsNullOrEmpty(IsOverTime))
                        {
                            row.Status = Convert.ToInt32(IsOverTime) == 1 ? 4 : 5;
                        }
                    }
                }
                #endregion
                return patientsList;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }

        }

        public EntityPidReportMain GetPatientByPatId(string strPatId, bool withPidRepDetail)
        {
            EntityPidReportMain patient = null;
            if (!string.IsNullOrEmpty(strPatId))
            {
                IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                if (mainDao != null)
                {
                    patient = mainDao.GetPatientInfo(strPatId);
                }
                else
                {
                    Lib.LogManager.Logger.LogInfo("DclDaoFactory.DaoHandler<IDaoPidReportMain>()=null");
                }

                if (patient != null)
                {
                    //是否要查组合信息
                    if (withPidRepDetail)
                    {
                        patient.ListPidReportDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(strPatId);
                    }
                }
            }
            return patient;
        }

        /// <summary>
        /// 获取病人资料状态
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        public string GetPatientState(string repId)
        {
            string state = string.Empty;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                EntityPidReportMain patient = mainDao.GetPatientInfo(repId);

                if (patient != null)
                {
                    if (patient.RepInitialFlag != 0 && patient.RepStatus != null && patient.RepStatus.Value == 0)
                    {
                        state = "1";
                    }
                    else
                    {
                        state = patient.RepStatus.Value.ToString();
                    }
                }
            }
            return state;
        }

        public List<EntityPidReportMain> GetPatientStateForQueryIsResult(EntityPatientQC eyPatientQC)
        {
            List<EntityPidReportMain> listIsResult = new List<EntityPidReportMain>();
            if (eyPatientQC != null)
            {
                IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                if (dao != null)
                {
                    listIsResult = dao.GetPatientStateForQueryIsResult(eyPatientQC);
                }
            }
            return listIsResult;
        }

        public List<EntityPidReportMain> SearchPatientForReportCopyUse(string sbPatId)
        {
            List<EntityPidReportMain> listPats = new List<EntityPidReportMain>();
            if (!string.IsNullOrEmpty(sbPatId))
            {
                IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                if (dao != null)
                {
                    listPats = dao.SearchPatientForReportCopyUse(sbPatId);
                }
            }
            return listPats;
        }


        public bool UpdateRepRecheckFlag(EntityPidReportMain patient)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                result = mainDao.UpdateRepRecheckFlag(patient);
            }
            return result;
        }

        public bool UpdateRepReadUserId(string strOpType, string RepReadUserId, List<string> listRepId)
        {
            bool result = false;
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            EntityUserQc userQc = new EntityUserQc();
            userQc.LoginId = RepReadUserId;
            userQc.FuncCode = "FrmCombineModeSel_UnAllLookcode";
            bool unAllLookcode = new SysUserInfoBIZ().SysUserQuery(userQc).Count > 0;
            if (mainDao != null)
            {
                try
                {
                    foreach (string repId in listRepId)
                        mainDao.UpdateRepReadUserId(strOpType, RepReadUserId, repId, unAllLookcode);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }
        public List<EntityPidReportMain> GetPatientsCount(EntityPatientQC qc)
        {
            List<EntityPidReportMain> list = new List<EntityPidReportMain>();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                list = mainDao.GetPatientsCount(qc);
            }
            return list;
        }
        /// <summary>
        /// 获取该条码超时数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public List<EntityTatOverTime> GetPatTatOverTime(string barCode)
        {
            List<EntityTatOverTime> list = new TatOverTimeBIZ().GetTatOverTime(barCode);
            return list;
        }

        /// <summary>
        /// 获取条形码可用的仪器列表
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public List<EntityDicInstrument> GetAllItrForBarCode(string barCode)
        {
            List<EntityDicInstrument> result = new List<EntityDicInstrument>();
            IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if(dao != null)
            {
                result = dao.GetInstrumentByBarcode(barCode);
            }
            return result;
        }

        public bool InsertReports(List<EntityPidReportMain> Reports,out string ErrorMsg)
        {
            ErrorMsg = "";
            List<EntityPidReportMain> list = new List<EntityPidReportMain>();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                return mainDao.InsertReports(Reports,out ErrorMsg);
            }
            ErrorMsg = "请求接口失败！";
            return false;
        }

        /// <summary>
        /// 获取上传失败的报告单
        /// </summary>
        /// <param name="Reports"></param>
        /// <returns></returns>
        public List<EntityPidReportMain> GetFaultUpLoadReport(EntityPatientQC qc, string type)
        {
            List<EntityPidReportMain> list = new List<EntityPidReportMain>();
            IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            if (mainDao != null)
            {
                list = mainDao.GetFaultUpLoadReport(qc, type);
            }
            return list;
        }
    }
}
