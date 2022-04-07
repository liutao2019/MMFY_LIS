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
   
    public class ReaDeliveryBIZ : IReaDelivery
    {
        public void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace)
        {
            IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
            if (mainDao != null)
            {
                foreach (string repId in repIds)
                {
                    DateTime date = ServerDateTime.GetDatabaseServerDateTime();
                    //更新病人的状态为打印状态
                    mainDao.UpdateDeliveryStatus(repId, OperatorID, date);
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
        public void UpdateReaInfoBefore(EntityReaDelivery ReaInfo, EntityReaDelivery OriginReaInfo, OperationLogger opLogger)
        {
            //采购单号
            string rea_sid = ReaInfo.Rdl_no;

            //日期
            DateTime rea_date = Convert.ToDateTime(ReaInfo.Rdl_date);

            //查找之前的样本号
            if (OriginReaInfo != null)
            {
                string prevSID = OriginReaInfo.Rdl_no;

                System.Reflection.PropertyInfo[] mPi = typeof(EntityReaDelivery).GetProperties();
                for (int i = 0; i < mPi.Length; i++)
                {
                    System.Reflection.PropertyInfo pi = mPi[i];
                    if (pi.Name != "ListReaDeliveryDetail")
                    {
                        object oldValue = pi.GetValue(OriginReaInfo, null);
                        object newValue = pi.GetValue(ReaInfo, null);
                        if (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString())
                        {
                            string colCHS = FieldsNameConventer<ReaDeliveryFields>.Instance.GetDataFieldCHS<ReaDeliveryFields>(pi.Name);
                            if (!string.IsNullOrEmpty(colCHS))
                            {
                                opLogger.Add_ModifyLog(SysOperationLogGroup.REA_DELIVERYINFO, colCHS, oldValue + "→" + newValue);
                            }
                        }
                    }
                }
            }
        }

        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
            if (mainDao != null)
            {
                return mainDao.GetReaSID_MaxPlusOne(date, stepCode);
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
            IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_sid, pat_date);
            }
            return result;
        }

        public EntityReaDelivery GetDataByNum(EntityReaQC patientCondition, bool withDeliveryDetail)
        {
            List<EntityReaDelivery> data = new List<EntityReaDelivery>();
            if (!string.IsNullOrEmpty(patientCondition.ReaNo))
            {
                IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
                if (mainDao != null)
                {
                    data = mainDao.QueryDeliveryList(patientCondition);
                }
                else
                {
                    Lib.LogManager.Logger.LogInfo("DclDaoFactory.DaoHandler<IDaoReaDelivery>()=null");
                }

                if (data != null && data.Count > 0)
                {
                    //是否要查组合信息
                    if (withDeliveryDetail)
                    {
                        data[0].ListReaDeliveryDetail = new ReaDeliveryDetailBIZ().GetDetail(patientCondition);
                    }
                }
            }
            return data[0];
        }

        public EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t)
        {
            EntityOperateResult result = new EntityOperateResult();

            List<EntityReaDelivery> list = new List<EntityReaDelivery>();

            try
            {
                if (!string.IsNullOrEmpty(caller.UserID))
                {
                    t.Rdl_operator = caller.UserID;
                    t.Rdl_date = ServerDateTime.GetDatabaseServerDateTime();
                }

                list.Add(t);
                InsertNewData(list);

                #region 将修改病人信息的操作插入Samp_ process_detial表

                string remark = $"IP:{caller.IPAddress}";

                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "600";
                sampProcessDetial.ProcBarno = t.Rdl_no;
                sampProcessDetial.ProcBarcode = t.Rdl_no;
                sampProcessDetial.RepId = t.Rdl_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SaveReaDelivery", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }

            return result;
        }

        public bool InsertNewData(List<EntityReaDelivery> data)
        {
            bool result = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
            mainDao.Dbm = helper;
            if (mainDao != null)
            {
                try
                {
                    foreach (EntityReaDelivery item in data)
                    {
                        result = mainDao.InsertNewReaDelivery(item);
                        //插入病人信息成功后插入病人组合信息
                        if (result && item.ListReaDeliveryDetail.Count > 0)
                        {
                            int i = 0;

                            foreach (EntityReaDeliveryDetail detail in item.ListReaDeliveryDetail)
                            {
                                if (!string.IsNullOrEmpty(detail.Rdvd_reaid))
                                    detail.Rdvd_no = item.Rdl_no;
                                i++;

                            }
                            IDaoReaDeliveryDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
                            if (detailDao != null)
                            {
                                detailDao.Dbm = helper;
                                //插入组合前先删除
                                if (detailDao.DeleteReaDeliveryDetail(item.ListReaDeliveryDetail[0].Rdvd_no,string.Empty))
                                {
                                    foreach (EntityReaDeliveryDetail detail in item.ListReaDeliveryDetail)
                                    {
                                        result = detailDao.InsertNewReaDeliveryDetail(detail);
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

        public void DeliveryCount(EntityReaDeliveryDetail detail, DBManager helper,string operation)
        {
            IDaoReaStoreCount dao = DclDaoFactory.DaoHandler<IDaoReaStoreCount>();
            if (dao != null)
            {
                dao.Dbm = helper;
                EntityReaStoreCount entity = new EntityReaStoreCount();
                EntityReaQC reaQC = new EntityReaQC();
                reaQC.Barcode = detail.Rdvd_barcode;
                reaQC.ReaId = detail.Rdvd_reaid;
                reaQC.ReaNo = detail.Rdvd_no;
                List<EntityReaStoreCount> list = dao.SearchByQC(reaQC);
                if (list!= null && list.Count > 0)
                {
                    entity = list[0];
                    if (string.Equals(operation,"+"))
                    {
                        entity.Rri_Count += detail.Rdvd_reacount;
                    }
                    else if (string.Equals(operation, "-"))
                    {
                        entity.Rri_Count -= detail.Rdvd_reacount;
                    }
                    dao.UpdateReaStoreCount(entity);
                }
            }
        }

        public List<EntityReaDelivery> ReaQuery(EntityReaQC patientCondition)
        {
            List<EntityReaDelivery> listRea = new List<EntityReaDelivery>();
            IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
            if (mainDao != null)
            {
                listRea = mainDao.QueryDeliveryList(patientCondition);
            }
            return listRea;
        }

        public EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rdl_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.READELIVERY, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaDelivery OriginDeliveryInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaDeliveryDetail> OriginDeliveryDetail = OriginDeliveryInfo.ListReaDeliveryDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rdl_operator = caller.UserID;
                t.Rdl_date = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginDeliveryInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
                mainDao.Dbm = helper;
                IDaoReaDeliveryDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaDeliveryData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "607";
                sampProcessDetial.ProcBarno = t.Rdl_no;
                sampProcessDetial.ProcBarcode = t.Rdl_no;
                sampProcessDetial.RepId = t.Rdl_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaDeliveryDetailBIZ().UpdateDetailBefore(t.ListReaDeliveryDetail, OriginDeliveryDetail, opLogger);


                if (detailDao.DeleteReaDeliveryDetail(t.Rdl_no,string.Empty))
                {
                    foreach (EntityReaDeliveryDetail detail in t.ListReaDeliveryDetail)
                    {
                        detailDao.InsertNewReaDeliveryDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaDelivery", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rdl_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.READELIVERY, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaDelivery OriginDeliveryInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaDeliveryDetail> OriginDeliveryDetail = OriginDeliveryInfo.ListReaDeliveryDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rdl_operator = caller.UserID;
                t.Rdl_date = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginDeliveryInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
                mainDao.Dbm = helper;
                IDaoReaDeliveryDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
                detailDao.Dbm = helper;

                IDaoReaStorageDetail storeDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                storeDao.Dbm = helper;

                IDaoReaStorage inDao = DclDaoFactory.DaoHandler<IDaoReaStorage>();
                inDao.Dbm = helper;

                IDaoReaApply applyDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                applyDao.Dbm = helper;

                mainDao.UpdateReaDeliveryData(t);

                applyDao.UpdateStatus(t.Rdl_srno,9);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "601";
                sampProcessDetial.ProcBarno = t.Rdl_no;
                sampProcessDetial.ProcBarcode = t.Rdl_no;
                sampProcessDetial.RepId = t.Rdl_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaDeliveryDetailBIZ().UpdateDetailBefore(t.ListReaDeliveryDetail, OriginDeliveryDetail, opLogger);


                if (detailDao.DeleteReaDeliveryDetail(t.Rdl_no,string.Empty))
                {
                    foreach (EntityReaDeliveryDetail detail in t.ListReaDeliveryDetail)
                    {
                        detailDao.InsertNewReaDeliveryDetail(detail);

                        EntityReaQC query = new EntityReaQC();
                        query.ReaNo = detail.StoreNo;
                        query.Barcode = detail.Rdvd_barcode;

                        List<EntityReaStorageDetail> sDetail = storeDao.GetReaStorageDetail(query);
                        inDao.UpdateOutStorageData(detail.StoreNo);

                        storeDao.UpdateDetailStatus(detail.Rdvd_barcode, 1, sDetail[0].Rsd_count-detail.Rdvd_reacount, detail.StoreNo);
                    }
                    DeliveryCount(t.ListReaDeliveryDetail[0], helper, "-");
                }
                opLogger.Log();
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("UpdateReaDelivery", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaDelivery> data)
        {
            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
                mainDao.Dbm = helper;
                IDaoReaDeliveryDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
                detailDao.Dbm = helper;

                foreach (var apply in data)
                {
                    mainDao.DeleteReaDeliveryData(apply);
                    if (detailDao.DeleteReaDeliveryDetail(apply.Rdl_no,string.Empty))
                    {
                        foreach (EntityReaDeliveryDetail detail in apply.ListReaDeliveryDetail)
                        {
                            detail.Rdvd_no = apply.Rdl_no;
                            detailDao.CancelReaDeliveryDetail(detail);
                        }
                    }

                    #region 将修改病人信息的操作插入Samp_ process_detial表
                    string remark = $"IP:{caller.IPAddress}";
                    EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                    sampProcessDetial.ProcDate = caller.Time;
                    sampProcessDetial.ProcUsercode = caller.LoginID;
                    sampProcessDetial.ProcUsername = caller.LoginName;
                    sampProcessDetial.ProcStatus = "604";
                    sampProcessDetial.ProcBarno = apply.Rdl_no;
                    sampProcessDetial.ProcBarcode = apply.Rdl_no;
                    sampProcessDetial.RepId = apply.Rdl_no;
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
                Lib.LogManager.Logger.LogException("DeleteReaDelivery", ex);
                throw;
            }
            return res;
        }

        public EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t)
        {
            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rdl_operator = caller.UserID;
                t.Rdl_date = ServerDateTime.GetDatabaseServerDateTime();
            }
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
                mainDao.Dbm = helper;

                mainDao.UpdateReaDeliveryData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "602";
                sampProcessDetial.ProcBarno = t.Rdl_no;
                sampProcessDetial.ProcBarcode = t.Rdl_no;
                sampProcessDetial.RepId = t.Rdl_no;
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
                Lib.LogManager.Logger.LogException("UpdateReaDelivery", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaDelivery t)
        {

            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaDelivery mainDao = DclDaoFactory.DaoHandler<IDaoReaDelivery>();
                mainDao.Dbm = helper;

                IDaoReaStorageDetail storeDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                storeDao.Dbm = helper;

                IDaoReaApply applyDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                applyDao.Dbm = helper;
                
                t.Rdl_operator = caller.UserID;
                t.Rdl_status = "2";
                t.Rdl_date = ServerDateTime.GetDatabaseServerDateTime();

                mainDao.ReturnReaDeliveryData(t);

                applyDao.UpdateStatus(t.Rdl_srno, 4);

                foreach (EntityReaDeliveryDetail detail in t.ListReaDeliveryDetail)
                {
                    EntityReaQC query = new EntityReaQC();
                    query.ReaNo = detail.StoreNo;
                    query.Barcode = detail.Rdvd_barcode;

                    List<EntityReaStorageDetail> sDetail = storeDao.GetReaStorageDetail(query);
                    storeDao.UpdateDetailStatus(detail.Rdvd_barcode, 1, sDetail[0].Rsd_count + detail.Rdvd_reacount, detail.StoreNo);

                }

                DeliveryCount(t.ListReaDeliveryDetail[0], helper, "+");

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress} ";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "605";
                sampProcessDetial.ProcBarno = t.Rdl_no;
                sampProcessDetial.ProcBarcode = t.Rdl_no;
                sampProcessDetial.RepId = t.Rdl_no;
                sampProcessDetial.ProcContent = remark + t.Rdl_returnreason;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("ReturnReaDelivery", ex);
                throw;
            }
            return res;
        }
    }
}
