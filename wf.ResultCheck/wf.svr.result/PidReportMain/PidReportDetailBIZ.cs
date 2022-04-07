using dcl.common;
using dcl.dao.interfaces;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using System.Data;
using dcl.svr.sample;
using dcl.root.logon;
using dcl.dao.core;

namespace dcl.svr.result
{
    public class PidReportDetailBIZ : DclBizBase, IPidReportDetail
    {
        public bool DeleteReportDetail(string repId)
        {
            bool result = false;
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                result = detailDao.DeleteReportDetail(repId);
            }
            return result;
        }

        public List<EntityPidReportDetail> GetPidReportDetailByRepId(string repId)
        {
            List<EntityPidReportDetail> dtDetail = new List<EntityPidReportDetail>();
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
            if (detailDao != null)
            {
                dtDetail = detailDao.GetPidReportDetailByRepId(repId);
            }
            return dtDetail;
        }

        public bool InsertNewReportDetail(List<EntityPidReportDetail> repDetails)
        {
            bool result = false;
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                //插入组合前先删除
                if (DeleteReportDetail(repDetails[0].RepId))
                {
                    foreach (EntityPidReportDetail detail in repDetails)
                    {
                        result = detailDao.InsertNewPidReportDetail(detail);
                    }
                }
            }

            List<EntitySampDetail> list = new List<EntitySampDetail>();

            foreach (var item in repDetails)
            {
                if (!string.IsNullOrEmpty(item.RepBarCode))
                {
                    EntitySampDetail sampDetail = new EntitySampDetail();
                    sampDetail.SampBarCode = item.RepBarCode;
                    sampDetail.ComId = item.ComId;

                    list.Add(sampDetail);
                }
            }



            return result;
        }

        /// <summary>
        /// 上传病人组合明细
        /// </summary>
        /// <param name="repDetails"></param>
        /// <returns></returns>
        public bool UploadNewReportDetail(List<EntityPidReportDetail> repDetails)
        {
            bool result = false;
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                //插入组合前先删除
                if (DeleteReportDetail(repDetails[0].RepId))
                {
                    foreach (EntityPidReportDetail detail in repDetails)
                    {
                        result = detailDao.UploadNewPidReportDetail(detail);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 病人组合的信息插入前处理
        /// </summary>
        /// <param name="listPatCombine">病人组合集合</param>
        /// <param name="patient">病人信息</param>
        /// <returns></returns>
        public void ReportDetailBeforeInsert(List<EntityPidReportDetail> listPatCombine, EntityPidReportMain patient)
        {
            List<EntityPidReportDetail> listReportDdetail = new List<EntityPidReportDetail>();
            if (listPatCombine.Count > 0)
            {
                //string pat_id = string.Empty;
                //pat_id = patient.RepId;
                //List<string> listComId = new List<string>();
                //listPatCombine = listPatCombine.OrderBy(w => w.ComSeq).ToList();
                //int i = 0;
                //foreach (EntityPidReportDetail entityCombine in listPatCombine)
                //{
                //    entityCombine.SortNo = i;
                //    if (!Compare.IsEmpty(entityCombine.ComId))
                //    {
                //        entityCombine.RepId = pat_id;
                //        listComId.Add(entityCombine.ComId);
                //        listReportDdetail.Add(entityCombine);
                //    }
                //    i++;
                //}

                ////删除该标识ID病人组合明细
                //PidReportDetailBIZ detailBIZ = new PidReportDetailBIZ();
                //detailBIZ.DeleteReportDetail(pat_id);

                //如果有条码号则更新bc_cname标志
                //if (!string.IsNullOrEmpty(patient.RepBarCode) && listComId.Count > 0)
                //{
                //    SampDetailBIZ sampDetailBIZ = new SampDetailBIZ();
                //    sampDetailBIZ.UpdateSampDetailSampFlagByComId(patient.RepBarCode, listComId);
                //}
            }
        }

        /// <summary>
        /// 根据旧病人ID更新成新病人ID
        /// </summary>
        /// <param name="newRepId"></param>
        /// <param name="oldRepId"></param>
        /// <returns></returns>
        public bool UpdateDetailRepIdByOldRedId(string newRepId, string oldRepId)
        {
            bool result = false;
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
            if (detailDao != null)
            {
                result = detailDao.UpdateDetailRepIdByOldRedId(newRepId, oldRepId);
            }
            return result;
        }

        /// <summary>
        /// 批量添加组合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool BatchAddCombine(List<EntityPatientQC> PatientQcList)
        {
            bool isSuccess = false;
            try
            {
                List<EntityPidReportMain> patientList = new List<EntityPidReportMain>();
                PidReportMainBIZ reportMainBIZ = new PidReportMainBIZ();
                if (reportMainBIZ != null)
                {
                    patientList = reportMainBIZ.PatientQuery(PatientQcList[0]);
                }
                List<EntityPidReportMain> dtDataToUpdate = new List<EntityPidReportMain>();
                List<EntityPidReportDetail> dtPatientsMi = new List<EntityPidReportDetail>();
                bool comHas = false;
                bool updateFlag = false;

                if (patientList != null && patientList.Count > 0)
                {
                    foreach (EntityPidReportMain patient in patientList)
                    {
                        //未审核的才能修改
                        if (Compare.IsEmpty(patient.RepStatus) || patient.RepStatus.ToString() == "0")
                        {
                            int com_seq = 0;
                            string pat_c_name = string.Empty;
                            if (!string.IsNullOrEmpty(patient.PidComName))
                            {
                                dtPatientsMi = GetPidReportDetailByRepId(patient.RepId);
                                com_seq = dtPatientsMi.Count;
                                pat_c_name += patient.PidComName;
                            }

                            foreach (EntityPatientQC PatientQc in PatientQcList)
                            {
                                foreach (EntityPidReportDetail item in dtPatientsMi)
                                {
                                    if (item.ComId == PatientQc.ComId)
                                    {
                                        comHas = true;
                                        break;
                                    }
                                    else
                                        comHas = false;
                                }
                                if (comHas)
                                    continue;
                                if (string.IsNullOrEmpty(pat_c_name))
                                    pat_c_name = PatientQc.ComName;
                                else
                                    pat_c_name += "+" + PatientQc.ComName;
                                IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                                EntityPidReportDetail reportDetail = new EntityPidReportDetail();
                                reportDetail.RepId = patient.RepId;
                                reportDetail.ComId = PatientQc.ComId;
                                reportDetail.RepBarCode = patient.RepBarCode;
                                reportDetail.SortNo = com_seq;

                                if (detailDao != null)
                                {
                                    updateFlag = detailDao.InsertNewPidReportDetail(reportDetail);
                                }

                                com_seq++;
                            }
                            if (updateFlag)
                            {
                                patient.PidComName = pat_c_name;
                                dtDataToUpdate.Add(patient);
                            }
                        }
                        else
                            continue;
                    }
                    if (reportMainBIZ != null)
                    {
                        updateFlag = reportMainBIZ.UpdatePatientData(dtDataToUpdate);
                    }

                    if (updateFlag)
                    {
                        isSuccess = true;
                    }
                }
                else
                {
                    isSuccess = false;
                }
                return isSuccess;

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }

        }

        /// <summary>
        /// 更新病人组合前的一些处理
        /// </summary>
        /// <param name="listPatCombine"></param>
        /// <param name="listOriginPatCombine"></param>
        /// <param name="barcode"></param>
        /// <param name="result"></param>
        /// <param name="logger"></param>
        /// <param name="changePatID"></param>
        /// <param name="resultList"></param>
        public void UpdatePatientCombineBefore(List<EntityPidReportDetail> listPatCombine, List<EntityPidReportDetail> listOriginPatCombine, string barcode, EntityOperationResult result, OperationLogger logger, bool changePatID, EntityQcResultList resultList)
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

                //根据PatID改变,并转移组合信息
                UpdatePatCombineByPatIDchaged(listPatCombine, listOriginPatCombine, pat_id, result, logger, changePatID);

                //更新病人ID,并于原组合比较
                List<string> listComId = new List<string>();
                foreach (EntityPidReportDetail entityCurrentCombine in listPatCombine)
                {
                    //当前组合ID
                    string curr_pat_com_id = entityCurrentCombine.ComId;
                    entityCurrentCombine.RepId = pat_id;
                    listComId.Add(entityCurrentCombine.ComId);
                    //在原有组合内查找是否存在当前组合ID
                    //不存在,则当前组合ID为新增的组合
                    if (listOriginPatCombine.Where(w => w.ComId == curr_pat_com_id).Count() == 0)
                    {
                        logger.Add_AddLog(SysOperationLogGroup.PAT_COMBINE, curr_pat_com_id, string.Empty);
                    }
                }

                foreach (EntityPidReportDetail entityOriginCombine in listOriginPatCombine)
                {
                    //原有组合ID
                    string origin_pat_com_id = entityOriginCombine.ComId;

                    //在当前组合内查找是否存在原有组合ID
                    //不存在,则当前组合ID为已删除的组合
                    if (listPatCombine.Where(w => w.ComId == origin_pat_com_id).Count() == 0)
                    {
                        logger.Add_DelLog(SysOperationLogGroup.PAT_COMBINE, origin_pat_com_id, string.Empty);
                    }
                }

                //更新医嘱id
                //如果有使用条码，并根据配置是否更新patients_mi.pat_yz_id为bc_cname.bc_yz_id
                if (!string.IsNullOrEmpty(barcode)
                    && dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AutoFillOrderIdWhenSave") == "是"
                    && listComId.Count > 0
                    )
                {
                    //根据条码号与组合id查找出对应的医嘱(一次性取出该条码对应的医嘱)
                    List<EntitySampDetail> listOrderSn = new SampDetailBIZ().GetSampDetailByBarCodeAndComId(barcode, listComId);

                    foreach (EntityPidReportDetail entityCombine in listPatCombine)
                    {
                        if (common.Compare.IsEmpty(entityCombine.OrderSn) || entityCombine.OrderSn.Trim() != string.Empty)
                        {
                            string com_id = entityCombine.ComId;

                            List<EntitySampDetail> listYZ = listOrderSn.Where(w => w.ComId == com_id).ToList();

                            if (listYZ.Count > 0 && !common.Compare.IsEmpty(listYZ[0].OrderSn))
                            {
                                entityCombine.OrderSn = listYZ[0].OrderSn;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据PatID改变,并转移组合信息
        /// </summary>
        /// <param name="dtPatCombine">当前的组合信息</param>
        /// <param name="dtOriginPatCombine">原有的组合信息</param>
        /// <param name="nw_pat_id">新pat_id</param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <param name="logger"></param>
        private void UpdatePatCombineByPatIDchaged(List<EntityPidReportDetail> listPatCombine, List<EntityPidReportDetail> listOriginPatCombine, string nw_pat_id, EntityOperationResult result, OperationLogger logger, bool changePatID)
        {
            //如果pat_ID没改变,跳过
            if (!changePatID) return;

            if (result.Success)
            {

                //原有的组合信息,不为空时才转移
                if (listOriginPatCombine != null && listOriginPatCombine.Count > 0)
                {
                    try
                    {
                        string old_pat_id = listOriginPatCombine[0].RepId;

                        //PatId没改变则不更新
                        if (old_pat_id == nw_pat_id)
                            return;

                        //当前的组合信息,为空时才转移
                        if (listPatCombine != null && listPatCombine.Count <= 0)
                        {
                            //pat_id是否不一样,不一样才继续
                            if (old_pat_id != nw_pat_id && (!string.IsNullOrEmpty(nw_pat_id)))
                            {
                                //把原有的组合信息复制到当前的组合信息里
                                foreach (EntityPidReportDetail entityOldCombine in listOriginPatCombine)
                                {
                                    listPatCombine.Add(entityOldCombine);
                                }

                                foreach (EntityPidReportDetail entityNewCombine in listPatCombine)
                                {
                                    entityNewCombine.RepId = nw_pat_id;//更新为新pat_id
                                }

                                EntityPidReportMain patient = new EntityPidReportMain();

                                new PidReportMainBIZ().CreatePatCName(patient, listPatCombine);

                                string pat_c_name = patient.PidComName;

                                if (!string.IsNullOrEmpty(pat_c_name))
                                {
                                    //更新病人资料的组合信息
                                    new PidReportMainBIZ().UpdatePidComNameByRepId(pat_c_name, nw_pat_id);
                                }
                            }
                        }
                        //删除 原有的组合信息
                        DeleteReportDetail(old_pat_id);
                    }
                    catch (Exception ex)
                    {
                        result.AddMessage(EnumOperationErrorCode.Exception, ex.ToString(), EnumOperationErrorLevel.Error);
                        Lib.LogManager.Logger.LogException(ex);
                        //throw;
                    }
                }
            }
        }

        public List<EntityPidReportDetail> SearchPidReportDetailByMulitRepId(string mulitRepId)
        {
            List<EntityPidReportDetail> listPidDetail = new List<EntityPidReportDetail>();
            if (!string.IsNullOrEmpty(mulitRepId))
            {
                IDaoPidReportDetail dao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                if (dao != null)
                {
                    listPidDetail = dao.SearchPidReportDetailByMulitRepId(mulitRepId);
                }
            }
            return listPidDetail;
        }
    }
}
