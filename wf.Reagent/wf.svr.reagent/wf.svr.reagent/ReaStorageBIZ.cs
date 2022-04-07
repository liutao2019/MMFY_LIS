/*  
 * 警告：
 * 本源代码所有权归广州慧扬健康科技有限公司(下称“本公司”)所有，已采取保密措施加以保护。  受《中华人民共和国刑法》、
 * 《反不正当竞争法》和《国家工商行政管理局关于禁止侵犯商业秘密行为的若干规定》等相关法律法规的保护。未经本公司书面
 * 许可，任何人披露、使用或者允许他人使用本源代码，必将受到相关法律的严厉惩罚。
 * Warning: 
 * The ownership of this source code belongs to Guangzhou Wisefly Technology Co., Ltd.(hereinafter referred to as "the company"), 
 * which is protected by the criminal law of the People's Republic of China, the anti unfair competition law and the 
 * provisions of the State Administration for Industry and Commerce on prohibiting the infringement of business secrets, etc. 
 * Without the written permission of the company, anyone who discloses, uses or allows others to use this source code 
 * will be severely punished by the relevant laws.
*/
using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.root.logon;
using dcl.servececontract;
using dcl.svr.cache;
using dcl.svr.sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace wf.svr.reagent
{
   
    public class ReaStorageBIZ : IReaStorage
    {
        public void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace)
        {
            IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
            if (mainDao != null)
            {
                foreach (string repId in repIds)
                {
                    DateTime date = ServerDateTime.GetDatabaseServerDateTime();
                    //更新病人的状态为打印状态
                    mainDao.UpdateStorageStatus(repId, OperatorID, date);
                    EntitySampProcessDetail detail = new EntitySampProcessDetail();
                    detail.ProcDate = ServerDateTime.GetDatabaseServerDateTime();
                    detail.ProcUsercode = OperatorID;
                    detail.ProcUsername = OperatorName;
                    detail.ProcStatus = "606";
                    detail.ProcBarno = repId;
                    detail.ProcBarcode = repId;
                    detail.ProcPlace = strPlace;
                    detail.ProcContent = "试剂管理模块";
                    detail.RepId = repId;
                    //插入条码流转明细
                    new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(detail);
                }
            }
        }
        /// <summary>
        /// 更新试剂采购基本信息前的一些处理
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        public void UpdateReaInfoBefore(EntityReaStorage ReaInfo, EntityReaStorage OriginReaInfo, OperationLogger opLogger)
        {
            //采购单号
            string rea_sid = ReaInfo.Rsr_no;

            //日期
            DateTime rea_date = Convert.ToDateTime(ReaInfo.Rsr_date);

            //查找之前的样本号
            if (OriginReaInfo != null)
            {
                string prevSID = OriginReaInfo.Rsr_no;

                System.Reflection.PropertyInfo[] mPi = typeof(EntityReaStorage).GetProperties();
                for (int i = 0; i < mPi.Length; i++)
                {
                    System.Reflection.PropertyInfo pi = mPi[i];
                    if (pi.Name != "ListReaStorageDetail")
                    {
                        object oldValue = pi.GetValue(OriginReaInfo, null);
                        object newValue = pi.GetValue(ReaInfo, null);
                        if (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString())
                        {
                            string colCHS = FieldsNameConventer<ReaStorageFields>.Instance.GetDataFieldCHS<ReaStorageFields>(pi.Name);
                            if (!string.IsNullOrEmpty(colCHS))
                            {
                                opLogger.Add_ModifyLog(SysOperationLogGroup.REA_STORAGEINFO, colCHS, oldValue + "→" + newValue);
                            }
                        }
                    }
                }
            }
        }

        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
            if (mainDao != null)
            {
                return mainDao.GetReaSID_MaxPlusOne(date, stepCode);
            }
            return string.Empty;
        }

        public string GetReaBarcode_MaxPlusOne(DateTime date, string stepCode)
        {
            IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
            if (mainDao != null)
            {
                return mainDao.GetReaBarcode_MaxPlusOne(date, stepCode);
            }
            return string.Empty;
        }

        public List<EntityReaStoreCount> SearchAllReaStoreCount()
        {
            IDaoReaStoreCount mainDao = DclDaoFactory.DaoHandler<IDaoReaStoreCount>();
            if (mainDao != null)
            {
                return mainDao.SearchAll();
            }
            return new List<EntityReaStoreCount>();
        }
        /// <summary>
        /// 判断某天是否录入此样本
        /// </summary>
        /// <param name="pat_sid"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        public bool ExsitSid(string pat_sid, DateTime pat_date)
        {
            bool result = false;
            IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_sid, pat_date);
            }
            return result;
        }

        public bool ExsitBarcode(string pat_sid, DateTime pat_date)
        {
            bool result = false;
            IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
            if (mainDao != null)
            {
                result = mainDao.ExsitBarcodeOrHostOrder(pat_sid, pat_date);
            }
            return result;
        }

        public EntityReaStorage GetDataByNum(EntityReaQC patientCondition, bool withStorageDetail)
        {
            List<EntityReaStorage> data = new List<EntityReaStorage>();
            if (!string.IsNullOrEmpty(patientCondition.ReaNo))
            {
                IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
                if (mainDao != null)
                {
                    data = mainDao.QueryStorageList(patientCondition);
                }
                else
                {
                    Lib.LogManager.Logger.LogInfo("DclDaoFactory.DaoHandler<IDaoReaStorage>()=null");
                }

                if (data != null && data.Count > 0)
                {
                    //是否要查组合信息
                    if (withStorageDetail)
                    {
                        data[0].ListReaStorageDetail = new ReaStorageDetailBIZ().GetDetail(patientCondition);
                    }
                }
            }
            return data[0];
        }

        public EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t)
        {
            EntityOperateResult result = new EntityOperateResult();

            List<EntityReaStorage> list = new List<EntityReaStorage>();

            try
            {
                if (!string.IsNullOrEmpty(caller.UserID))
                {
                    t.Rsr_operator = caller.UserID;
                    t.Rsr_date = ServerDateTime.GetDatabaseServerDateTime();
                }

                list.Add(t);
                if (!InsertNewData(list))
                {
                    result.AddMessage(EnumOperateErrorCode.HostOrderExist, EnumOperateErrorLevel.Error);
                };

                #region 将修改病人信息的操作插入Samp_ process_detial表

                string remark = $"IP:{caller.IPAddress}";

                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "600";
                sampProcessDetial.ProcBarno = t.Rsr_no;
                sampProcessDetial.ProcBarcode = t.Rsr_no;
                sampProcessDetial.RepId = t.Rsr_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SaveReaStorage", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }

            return result;
        }

        public bool InsertNewData(List<EntityReaStorage> data)
        {
            bool result = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
            mainDao.Dbm = helper;
            if (mainDao != null)
            {
                try
                {
                    foreach (EntityReaStorage item in data)
                    {
                        result = mainDao.InsertNewReaStorage(item);
                        //插入病人信息成功后插入病人组合信息
                        if (result && item.ListReaStorageDetail.Count > 0)
                        {
                            int i = 0;

                            List<string> idList = new List<string>();
                            foreach (EntityReaStorageDetail detail in item.ListReaStorageDetail)
                            {
                                if (!string.IsNullOrEmpty(detail.Rsd_reaid))
                                    detail.Rsd_no = item.Rsr_no;
                                i++;
                                if (!idList.Contains(detail.Rsd_purno))
                                {
                                    idList.Add(detail.Rsd_purno);
                                }   

                            }
                            if (idList!=null&&idList.Count > 0)
                            {
                                IDaoReaPurchase purDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                                purDao.Dbm = helper;
                                foreach (var str in idList)
                                {
                                    purDao.UpdateStatus(str, 9);
                                }
                            }
                            IDaoReaStorageDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                            if (detailDao != null)
                            {
                                detailDao.Dbm = helper;
                                //插入组合前先删除
                                if (detailDao.DeleteReaStorageDetail(item.ListReaStorageDetail[0].Rsd_no,string.Empty))
                                {
                                    foreach (EntityReaStorageDetail detail in item.ListReaStorageDetail)
                                    {
                                        detail.sort_no = detail.Rsd_validdate.ToString("yyyyMMdd");
                                        
                                        if (ExsitBarcode(detail.Rsd_barcode, ServerDateTime.GetDatabaseServerDateTime()))
                                        {
                                            Lib.LogManager.Logger.LogInfo(detail.Rsd_barcode+"已存在");
                                            helper.CommitTrans();
                                            helper = null;
                                            return result;
                                        }
                                        detail.Rsd_count = detail.Rsd_reacount;
                                        result = detailDao.InsertNewReaStorageDetail(detail);
                                        //StorageCount(detail, helper);
                                    }
                                }
                            }
                        }
                    }
                    helper.CommitTrans();
                    helper = null;

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    helper.CommitTrans();
                    helper = null;
                    return result;
                }

            }
            return result;
        }

        public void StorageCount(EntityReaStorageDetail detail, DBManager helper,string operation)
        {
            IDaoReaStoreCount dao = DclDaoFactory.DaoHandler<IDaoReaStoreCount>();
            if (dao != null)
            {
                dao.Dbm = helper;
                EntityReaStoreCount entity = new EntityReaStoreCount();
                EntityReaQC reaQC = new EntityReaQC();
                reaQC.Barcode = detail.Rsd_barcode;
                reaQC.BatchNo = detail.Rsd_batchno;
                reaQC.ReaId = detail.Rsd_reaid;
                reaQC.ReaNo = detail.Rsd_no;
                List<EntityReaStoreCount> list = dao.SearchByQC(reaQC);
                if (list!= null && list.Count > 0)
                {
                    entity = list[0];
                    if (string.Equals(operation,"+"))
                    {
                        entity.Rri_Count += detail.Rsd_reacount;
                    }
                    else if (string.Equals(operation, "-"))
                    {
                        entity.Rri_Count -= detail.Rsd_reacount;
                    }
                    dao.UpdateReaStoreCount(entity);
                }
                else
                {
                    entity.Rri_Drea_id = detail.Rsd_reaid;
                    entity.Rri_Count = detail.Rsd_reacount;
                    dao.SaveReaStoreCount(entity);
                }
            }
        }

        public List<EntityReaStorage> ReaQuery(EntityReaQC patientCondition)
        {
            List<EntityReaStorage> listRea = new List<EntityReaStorage>();
            IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
            if (mainDao != null)
            {
                listRea = mainDao.QueryStorageList(patientCondition);
            }
            return listRea;
        }

        public EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rsr_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REASTORAGE, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaStorage OriginStorageInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaStorageDetail> OriginStorageDetail = OriginStorageInfo.ListReaStorageDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rsr_operator = caller.UserID;
                t.Rsr_date = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginStorageInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
                mainDao.Dbm = helper;
                IDaoReaStorageDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaStorageData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "607";
                sampProcessDetial.ProcBarno = t.Rsr_no;
                sampProcessDetial.ProcBarcode = t.Rsr_no;
                sampProcessDetial.RepId = t.Rsr_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaStorageDetailBIZ().UpdateDetailBefore(t.ListReaStorageDetail, OriginStorageDetail, opLogger);


                if (detailDao.DeleteReaStorageDetail(t.Rsr_no,string.Empty))
                {
                    foreach (EntityReaStorageDetail detail in t.ListReaStorageDetail)
                    {
                        detail.sort_no = detail.Rsd_validdate.ToString("yyyyMMdd");
                        detail.Rsd_count = detail.Rsd_reacount;
                        detailDao.InsertNewReaStorageDetail(detail);
                        //StorageCount(detail, helper);
                    }
                }
                opLogger.Log();
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("UpdateReaStorage", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rsr_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REASTORAGE, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaStorage OriginStorageInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaStorageDetail> OriginStorageDetail = OriginStorageInfo.ListReaStorageDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rsr_operator = caller.UserID;
                t.Rsr_date = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginStorageInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
                mainDao.Dbm = helper;
                IDaoReaStorageDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                detailDao.Dbm = helper;

                IDaoReaPurchaseDetail purdetailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
                purdetailDao.Dbm = helper;

                mainDao.UpdateReaStorageData(t);

                if (t.StorageMode == "完全入库")
                {
                    if (!string.IsNullOrEmpty(t.Rsr_purno))
                    {
                        IDaoReaPurchase purchaseDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                        purchaseDao.Dbm = helper;

                        purchaseDao.UpdateStatus(t.Rsr_purno, 9);
                    }
                    foreach (EntityReaStorageDetail detail in t.ListReaStorageDetail)
                    {
                        if (!string.IsNullOrEmpty(detail.Rsd_purno) && !string.IsNullOrEmpty(detail.Rsd_reaid))
                        {
                            purdetailDao.UpdateDetailStatus(detail.Rsd_purno, detail.Rsd_reaid, 1);
                        }
                    }
                }

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "601";
                sampProcessDetial.ProcBarno = t.Rsr_no;
                sampProcessDetial.ProcBarcode = t.Rsr_no;
                sampProcessDetial.RepId = t.Rsr_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaStorageDetailBIZ().UpdateDetailBefore(t.ListReaStorageDetail, OriginStorageDetail, opLogger);


                if (detailDao.DeleteReaStorageDetail(t.Rsr_no,string.Empty))
                {
                    foreach (EntityReaStorageDetail detail in t.ListReaStorageDetail)
                    {
                        detail.sort_no = detail.Rsd_validdate.ToString("yyyyMMdd");
                        detail.Rsd_count = detail.Rsd_reacount;
                        detailDao.InsertNewReaStorageDetail(detail);
                        StorageCount(detail, helper,"+");
                    }
                }
                opLogger.Log();
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("UpdateReaStorage", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaStorage> data)
        {
            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
                mainDao.Dbm = helper;
                IDaoReaStorageDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                detailDao.Dbm = helper;

                foreach (var apply in data)
                {
                    mainDao.DeleteReaStorageData(apply);
                    if (detailDao.DeleteReaStorageDetail(apply.Rsr_no,string.Empty))
                    {
                        foreach (EntityReaStorageDetail detail in apply.ListReaStorageDetail)
                        {
                            detail.Rsd_no = apply.Rsr_no;
                            detailDao.CancelReaStorageDetail(detail);
                        }
                    }

                    #region 将修改病人信息的操作插入Samp_ process_detial表
                    string remark = $"IP:{caller.IPAddress}";
                    EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                    sampProcessDetial.ProcDate = caller.Time;
                    sampProcessDetial.ProcUsercode = caller.LoginID;
                    sampProcessDetial.ProcUsername = caller.LoginName;
                    sampProcessDetial.ProcStatus = "604";
                    sampProcessDetial.ProcBarno = apply.Rsr_no;
                    sampProcessDetial.ProcBarcode = apply.Rsr_no;
                    sampProcessDetial.RepId = apply.Rsr_no;
                    sampProcessDetial.ProcContent = remark;

                    new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                    #endregion
                }

                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("DeleteReaStorage", ex);
                throw;
            }
            return res;
        }

        public EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t)
        {
            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rsr_operator = caller.UserID;
                t.Rsr_date = ServerDateTime.GetDatabaseServerDateTime();
            }
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
                mainDao.Dbm = helper;

                mainDao.UpdateReaStorageData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "602";
                sampProcessDetial.ProcBarno = t.Rsr_no;
                sampProcessDetial.ProcBarcode = t.Rsr_no;
                sampProcessDetial.RepId = t.Rsr_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("UpdateReaStorage", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaStorage t)
        {

            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaStorage mainDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
                mainDao.Dbm = helper;
                t.Rsr_operator = caller.UserID;
                t.Rsr_status = "2";
                t.Rsr_date = ServerDateTime.GetDatabaseServerDateTime();

                mainDao.ReturnReaStorageData(t);

                if (!string.IsNullOrEmpty(t.Rsr_purno))
                {
                    IDaoReaPurchase purchaseDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                    purchaseDao.Dbm = helper;

                    purchaseDao.UpdateStatus(t.Rsr_purno, 4);
                }
                foreach (EntityReaStorageDetail detail in t.ListReaStorageDetail)
                {
                    StorageCount(detail, helper,"-");
                    if (!string.IsNullOrEmpty(detail.Rsd_purno) && !string.IsNullOrEmpty(detail.Rsd_reaid))
                    {
                        IDaoReaPurchaseDetail purdetailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
                        purdetailDao.Dbm = helper;
                        purdetailDao.UpdateDetailStatus(detail.Rsd_purno, detail.Rsd_reaid, 0);
                    }

                }


                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress} ";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "605";
                sampProcessDetial.ProcBarno = t.Rsr_no;
                sampProcessDetial.ProcBarcode = t.Rsr_no;
                sampProcessDetial.RepId = t.Rsr_no;
                sampProcessDetial.ProcContent = remark + t.Rsr_returnreason;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("ReturnReaStorage", ex);
                throw;
            }
            return res;
        }
    }
}
