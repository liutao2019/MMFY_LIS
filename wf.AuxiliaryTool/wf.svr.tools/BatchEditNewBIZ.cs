using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.svr.result;
using dcl.svr.sample;
using dcl.dao.core;


namespace dcl.svr.tools
{
    public class BatchEditNewBIZ : IBatchEditNew
    {
        public bool BatchCopyPatientData(EntityRemoteCallClientInfo caller, List<EntityPidReportMain> listPat, List<EntityDicCombine> listComb)
        {
            bool result = false;
            List<EntityPidReportDetail> listRepDetail = new List<EntityPidReportDetail>();
            List<EntityPidReportMain> patientsInsert = new List<EntityPidReportMain>();//复制到的病人列表
            string strPatItrOldId = string.Empty;
            string patID = string.Empty;
            foreach (EntityPidReportMain patient in listPat)
            {
                patient.RepStatus = 0;
                patient.RepAuditUserId = null;
                patient.RepAuditDate = null;
                patient.RepReportDate = null;
                patient.RepReportUserId = null;

                strPatItrOldId = patient.RepItrIdOld;
                patID = patient.RepId;

                EntityPatientQC patientQc = new EntityPatientQC();
                patientQc.RepId = patID;
                //判断病人是否已存在
                if (new PidReportMainBIZ().PatientQuery(patientQc).Count > 0)
                {
                    continue;
                }

                patientsInsert.Add(patient);



                //如果组合信息为空
                if (listComb == null || listComb.Count < 1)
                {
                    //资料复制时若不选择新的组合则复制现有的组合
                    if (patient.IsCopyCombine == "1")
                    {
                        string oldRepId = patient.RepIdOld;

                        //查找当前病人id的组合
                        List<EntityPidReportDetail> repDetails = new PidReportDetailBIZ().GetPidReportDetailByRepId(oldRepId);

                        if (repDetails != null && repDetails.Count > 0)
                        {
                            if (listRepDetail.Count < 1)
                            {
                                int seq = 0;
                                foreach (EntityPidReportDetail rowPatientsMI in repDetails)
                                {
                                    //把当前的加入'病人组合明细(集合)'
                                    rowPatientsMI.RepId = patID;
                                    rowPatientsMI.OrderSn = null;
                                    rowPatientsMI.SortNo = seq;

                                    listRepDetail.Add(rowPatientsMI);

                                    seq++;
                                }
                                patient.ListPidReportDetail = listRepDetail;
                            }
                        }
                    }
                }
                //如果组合信息不为空
                else if (patientsInsert.Count > 0 && listComb.Count > 0)
                {

                    int patseq = 0;
                    foreach (EntityDicCombine oneRow in listComb)
                    {
                        EntityPidReportDetail detail = new EntityPidReportDetail();
                        detail.RepId = patID;
                        detail.ComId = oneRow.ComId;
                        detail.OrderCode = oneRow.ComHisCode;
                        detail.OrderPrice = oneRow.ComPrice.ToString();
                        detail.OrderSn = string.Empty;
                        detail.PatComName = oneRow.ComName;
                        detail.SortNo = patseq;

                        listRepDetail.Add(detail);
                        patseq++;
                    }
                    patient.ListPidReportDetail = listRepDetail;
                }
                try
                {
                    new PidReportMainBIZ().SavePatient(caller, patient);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }

            return result;
        }

        public bool SetResultoBcFlag(List<string> patIdList, List<string> comCode)
        {
            bool result = false;
            try
            {
                foreach (string repid in patIdList)
                {
                    EntityResultQC resultQc = new EntityResultQC();
                    resultQc.ListObrId.Add(repid);
                    resultQc.ObrFlag = "1";

                    List<EntityObrResult> listResult = new ObrResultBIZ().ObrResultQuery(resultQc);

                    if (listResult.Count > 0)
                    {
                        List<EntityObrResult> NotInResult = (from x in listResult where !comCode.Contains(x.ItmComId) select x).ToList();
                        if (NotInResult.Count > 0)
                        {
                            foreach (EntityObrResult obrResult in NotInResult)
                            {
                                EntityResultQC qc = new EntityResultQC();
                                qc.ObrSn = obrResult.ObrSn.ToString();
                                new ObrResultBIZ().UpdateObrFlagByCondition(qc);
                            }
                        }
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }

        public List<EntityOperateResult> BatchUpdatePatientData(BatchEditSrc source, BatchEditDest dest)
        {
            List<EntityOperateResult> listOperation = new List<EntityOperateResult>();
            EntityPatientQC patientQc = new EntityPatientQC();

            #region 把BatchEditSrc条件转化为EntityPatientQC

            patientQc.ListItrId.Add(source.pat_itr_id);
            patientQc.DateStart = source.pat_date.Date;
            patientQc.DateEnd = source.pat_date.Date.AddDays(1).AddMilliseconds(-1);
            patientQc.SamId = source.pat_sam_id;
            patientQc.DepId = source.pat_dep_id;
            patientQc.PatDepName = source.pat_dep_name;
            patientQc.PidCheckUserId = source.pat_i_code;
            if (source.MatchMode == "0")
            {
                EntitySid sid = new EntitySid();
                sid.StartSid = Convert.ToInt32(source.pat_sid_begin);
                sid.EndSid = Convert.ToInt32(source.pat_sid_end);
                patientQc.ListSidRange.Add(sid);
            }
            else
            {
                EntitySortNo sortNo = new EntitySortNo();
                sortNo.StartNo = Convert.ToInt32(source.pat_sid_begin);
                sortNo.EndNo = Convert.ToInt32(source.pat_sid_end);
                patientQc.ListSortRange.Add(sortNo);
            }
            #endregion

            List<EntityPidReportMain> listPatients = new PidReportMainBIZ().PatientQuery(patientQc);
            List<EntityPidReportMain> listUpdatePat = new List<EntityPidReportMain>();
            //目标开始编号
            long dest_pat_num_begin = dest.pat_sid_begin;

            //源开始编号
            long src_pat_num_begin = source.pat_sid_begin;

            long patSampeIdDiffer = dest_pat_num_begin - src_pat_num_begin;

            DBManager helper = new DBManager();
            helper.BeginTrans();
            bool actionResult = false;
            foreach (EntityPidReportMain patient in listPatients)
            {
                EntityOperateResult result = new EntityOperateResult();
                result.Data.Patient.RepSid = patient.RepSid;
                result.Data.Patient.PidName = patient.PidName;
                result.Data.Patient.RepItrId = patient.RepItrId;
                result.Data.Patient.RepInDate = patient.RepInDate.Value;

                //未审核的才能修改
                if (patient.RepStatus == null || (patient.RepStatus != null && patient.RepStatus.Value == 0))
                {
                    //源pat_id
                    string src_pat_id = patient.RepId.ToString();

                    //源数据当前样本号
                    long src_curr_pat_sid = Convert.ToInt64(patient.RepSid);

                    //源数据当前序号
                    long? src_curr_pat_host_order = null;
                    if (!string.IsNullOrEmpty(patient.RepSerialNum))
                    {
                        src_curr_pat_host_order = Convert.ToInt64(patient.RepSerialNum);
                    }

                    //目标数据当前样本号
                    long dest_curr_pat_sid;

                    //目标数据当前序号
                    long? dest_curr_pat_host_order = null;
                    //按序号修改时的样本号
                    string dest_pat_sid = string.Empty;
                    if (dest.MatchMode == "0") //目标按样本号
                    {

                        dest_curr_pat_sid = (src_curr_pat_sid - src_pat_num_begin) + dest_pat_num_begin;
                        dest_curr_pat_host_order = src_curr_pat_host_order;
                    }
                    else //目标按序号
                    {
                        if (source.MatchMode == "1")
                        {
                            dest_curr_pat_host_order = src_curr_pat_host_order + patSampeIdDiffer;
                            dest_curr_pat_sid = src_curr_pat_sid;
                            dest_pat_sid = patient.RepSid;
                        }
                        else
                        {
                            dest_curr_pat_sid = src_curr_pat_sid + patSampeIdDiffer;
                            dest_curr_pat_host_order = dest_curr_pat_sid;
                            dest_curr_pat_sid = src_curr_pat_sid;
                        }
                    }
                    string dest_pat_itr_id = dest.pat_itr_id;

                    //目标日期
                    DateTime dest_pat_date = dest.pat_date;

                    //新的pat_id
                    string new_pat_id = dest_pat_itr_id + dest_pat_date.ToString("yyyyMMdd") + dest_curr_pat_sid;

                    //判断样本号是否重复
                    if (src_pat_id == new_pat_id || (src_pat_id != new_pat_id
                        && (dest.MatchMode == "0" && !new PidReportMainBIZ().ExsitSid(dest_curr_pat_sid.ToString(), dest_pat_itr_id, dest_pat_date)
                            || dest.MatchMode == "1" && !new PidReportMainBIZ().ExsitPatHostOrder(dest_curr_pat_host_order.ToString(), dest_pat_itr_id, dest_pat_date))))
                    {
                        if (!string.IsNullOrEmpty(dest_pat_sid))
                        {
                            patient.RepSid = dest_pat_sid;
                        }
                        else {
                            patient.RepSid = dest_curr_pat_sid.ToString();
                        }
                        patient.RepSerialNum = dest_curr_pat_host_order.ToString();
                        patient.RepItrId = dest_pat_itr_id;
                        patient.RepInDate = dest_pat_date;
                        patient.NewRepId = new_pat_id;//新病人ID,注释掉，如果加上数据库会找不到数据
                        patient.RepIdOld = src_pat_id;//旧病人ID

                        #region 更新其他信息
                        if (!string.IsNullOrEmpty(dest.pat_dep_id)
                                                && !string.IsNullOrEmpty(dest.pat_dep_name))
                        {
                            patient.PidDeptId = dest.pat_dep_id;
                            patient.PidDeptName = dest.pat_dep_name;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_doc_id))
                        {
                            patient.PidDoctorCode = dest.pat_doc_id;
                        }


                        if (!string.IsNullOrEmpty(dest.pat_i_code))
                        {
                            patient.RepCheckUserId = dest.pat_i_code;
                        }

                        if (dest.pat_jy_date != null)
                        {
                            patient.SampCheckDate = dest.pat_jy_date.Value;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_ori_id))
                        {
                            patient.PidSrcId = dest.pat_ori_id;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_rem))
                        {
                            patient.PidRemark = dest.pat_rem;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_sam_id))
                        {
                            patient.PidSamId = dest.pat_sam_id;
                        }

                        if (dest.pat_sample_date != null)
                        {
                            patient.SampCollectionDate = dest.pat_sample_date.Value;
                        }

                        if (dest.pat_sdate != null)
                        {
                            patient.SampSendDate = dest.pat_sdate.Value;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_unit))
                        {
                            patient.PidUnit = dest.pat_unit;
                        }

                        if (dest.pat_apply_date != null)
                        {
                            patient.SampApplyDate = dest.pat_apply_date.Value;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_sex))
                        {
                            patient.PidSex = dest.pat_sex;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_age_exp))
                        {
                            patient.PidAgeExp = dest.pat_age_exp;
                        }

                        if (dest.pat_age != -1)
                        {
                            patient.PidAge = dest.pat_age;
                        }
                        if (!string.IsNullOrEmpty(dest.pat_exp))
                        {
                            patient.RepRemark = dest.pat_exp;
                        }

                        if (!string.IsNullOrEmpty(dest.pat_diag))
                        {
                            patient.PidDiag = dest.pat_diag;
                        }
                        #endregion

                        //更新病人组合
                        if (dest.PatientsMi.Count > 0)
                        {
                            //new PidReportDetailBIZ().DeleteReportDetail(src_pat_id);

                            int com_seq = 0;
                            string pat_c_name = string.Empty;
                            bool needplus = false;

                            foreach (EntityPatientsMi_4Barcode entityCombine in dest.PatientsMi)
                            {
                                if (needplus)
                                {
                                    pat_c_name += "+";
                                }

                                pat_c_name += entityCombine.pat_com_name;
                                List<EntityPidReportDetail> listDetail = new List<EntityPidReportDetail>();
                                EntityPidReportDetail detail = new EntityPidReportDetail();

                                #region 填充组合信息
                                detail.RepId = new_pat_id;
                                detail.ComId = entityCombine.pat_com_id;
                                detail.OrderCode = entityCombine.pat_his_code;
                                if (entityCombine.pat_com_price != null)
                                {
                                    detail.OrderPrice = entityCombine.pat_com_price.Value.ToString();
                                }
                                detail.OrderSn = entityCombine.pat_yz_id;
                                detail.SortNo = com_seq;
                                detail.RepBarCode = string.Empty;
                                #endregion

                                listDetail.Add(detail);

                                //插入组合信息
                                actionResult = new PidReportDetailBIZ().InsertNewReportDetail(listDetail);

                                com_seq++;

                                needplus = true;
                            }

                            patient.PidComName = pat_c_name;
                        }
                        //如果不更改组合信息
                        else
                        {
                            //更新组合表病人ID
                            actionResult = new PidReportDetailBIZ().UpdateDetailRepIdByOldRedId(new_pat_id, src_pat_id);
                        }
                        listUpdatePat.Add(patient);
                    }
                    else
                    {
                        result.AddMessage(EnumOperateErrorCode.SIDExist, EnumOperateErrorLevel.Error);
                    }
                }
                else//已审核记录
                {
                    result.AddMessage(EnumOperateErrorCode.Audited, EnumOperateErrorLevel.Error);
                    if (dest.pat_chk_date != null)
                    {
                        patient.RepAuditDate = dest.pat_chk_date.Value;
                    }
                    if (dest.pat_report_date != null)
                    {
                        patient.RepReportDate = dest.pat_report_date.Value;
                    }
                    //listUpdatePat.Add(patient);
                    result.Data.Patient = patient;
                }
                listOperation.Add(result);
            }
            if (actionResult)
            {
                //更新病人除了病人ID和条码号的信息
                actionResult = new PidReportMainBIZ().UpdatePatientData(listUpdatePat);
                if (actionResult)
                {
                    try
                    {
                        //更新病人ID
                        foreach (EntityPidReportMain patient in listUpdatePat)
                        {
                            new PidReportMainBIZ().UpdateNewRepIdByOldRepId(patient.NewRepId, patient.RepIdOld);
                        }
                    }
                    catch
                    {
                        actionResult = false;
                    }
                }
            }


            //判断是否全部添加或者更新成功
            if (actionResult)
            {
                helper.CommitTrans();
                helper = null;//开个事务不知道干嘛。。、】
            }
            else
                helper.RollbackTrans();

            return listOperation;
        }
    }
}
