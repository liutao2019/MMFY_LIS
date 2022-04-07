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
   
    public class ReaPurchaseBIZ : IReaPurchase
    {
        public void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace)
        {
            IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
            if (mainDao != null)
            {
                foreach (string repId in repIds)
                {
                    DateTime date = ServerDateTime.GetDatabaseServerDateTime();
                    //更新病人的状态为打印状态
                    mainDao.UpdatePurchaseStatus(repId, OperatorID, date);
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
        public void UpdateReaInfoBefore(EntityReaPurchase ReaInfo, EntityReaPurchase OriginReaInfo, OperationLogger opLogger)
        {
            //采购单号
            string rea_sid = ReaInfo.Rpc_no;

            //日期
            DateTime rea_date = Convert.ToDateTime(ReaInfo.Rpc_date);

            //查找之前的样本号
            if (OriginReaInfo != null)
            {
                string prevSID = OriginReaInfo.Rpc_no;

                System.Reflection.PropertyInfo[] mPi = typeof(EntityReaPurchase).GetProperties();
                for (int i = 0; i < mPi.Length; i++)
                {
                    System.Reflection.PropertyInfo pi = mPi[i];
                    if (pi.Name != "ListReaPurchaseDetail")
                    {
                        object oldValue = pi.GetValue(OriginReaInfo, null);
                        object newValue = pi.GetValue(ReaInfo, null);
                        if (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString())
                        {
                            string colCHS = FieldsNameConventer<ReaPurchaseFields>.Instance.GetDataFieldCHS<ReaPurchaseFields>(pi.Name);
                            if (!string.IsNullOrEmpty(colCHS))
                            {
                                opLogger.Add_ModifyLog(SysOperationLogGroup.REA_PURCHASEINFO, colCHS, oldValue + "→" + newValue);
                            }
                        }
                    }
                }
            }
        }

        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
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
            IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_sid, pat_date);
            }
            return result;
        }

        public EntityReaPurchase GetDataByNum(EntityReaQC patientCondition, bool withPurchaseDetail)
        {
            List<EntityReaPurchase> data = new List<EntityReaPurchase>();
            if (!string.IsNullOrEmpty(patientCondition.ReaNo))
            {
                IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                if (mainDao != null)
                {
                    data = mainDao.QueryPurchaseList(patientCondition);
                }
                else
                {
                    Lib.LogManager.Logger.LogInfo("DclDaoFactory.DaoHandler<IDaoReaPurchase>()=null");
                }

                if (data != null && data.Count > 0)
                {
                    //是否要查组合信息
                    if (withPurchaseDetail)
                    {
                        data[0].ListReaPurchaseDetail = new ReaPurchaseDetailBIZ().GetDetail(patientCondition);
                    }
                }
            }
            return data[0];
        }

        public EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t)
        {
            EntityOperateResult result = new EntityOperateResult();

            List<EntityReaPurchase> list = new List<EntityReaPurchase>();

            try
            {
                if (!string.IsNullOrEmpty(caller.UserID))
                {
                    t.Rpc_applier = caller.UserID;
                    t.Rpc_applydate = ServerDateTime.GetDatabaseServerDateTime();
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
                sampProcessDetial.ProcBarno = t.Rpc_no;
                sampProcessDetial.ProcBarcode = t.Rpc_no;
                sampProcessDetial.RepId = t.Rpc_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SaveReaPurchase", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }

            return result;
        }

        public bool InsertNewData(List<EntityReaPurchase> data)
        {
            bool result = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
            mainDao.Dbm = helper;
            if (mainDao != null)
            {
                try
                {
                    foreach (EntityReaPurchase item in data)
                    {
                        result = mainDao.InsertNewReaPurchase(item);
                        //插入病人信息成功后插入病人组合信息
                        if (result && item.ListReaPurchaseDetail.Count > 0)
                        {
                            int i = 0;

                            foreach (EntityReaPurchaseDetail detail in item.ListReaPurchaseDetail)
                            {
                                if (!string.IsNullOrEmpty(detail.Rpcd_reaid))
                                    detail.Rpcd_no = item.Rpc_no;
                                i++;

                            }
                            IDaoReaPurchaseDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
                            if (detailDao != null)
                            {
                                detailDao.Dbm = helper;
                                //插入组合前先删除
                                if (detailDao.DeleteReaPurchaseDetail(item.ListReaPurchaseDetail[0].Rpcd_no,string.Empty))
                                {
                                    foreach (EntityReaPurchaseDetail detail in item.ListReaPurchaseDetail)
                                    {
                                        result = detailDao.InsertNewReaPurchaseDetail(detail);
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

        public List<EntityReaPurchase> ReaQuery(EntityReaQC patientCondition)
        {
            List<EntityReaPurchase> listRea = new List<EntityReaPurchase>();
            IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
            if (mainDao != null)
            {
                listRea = mainDao.QueryPurchaseList(patientCondition);
            }
            return listRea;
        }

        public EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rpc_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REAPURCHASE, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaPurchase OriginPurchaseInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaPurchaseDetail> OriginPurchaseDetail = OriginPurchaseInfo.ListReaPurchaseDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rpc_applier = caller.UserID;
                t.Rpc_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginPurchaseInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                mainDao.Dbm = helper;
                IDaoReaPurchaseDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaPurchaseData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "607";
                sampProcessDetial.ProcBarno = t.Rpc_no;
                sampProcessDetial.ProcBarcode = t.Rpc_no;
                sampProcessDetial.RepId = t.Rpc_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaPurchaseDetailBIZ().UpdateDetailBefore(t.ListReaPurchaseDetail, OriginPurchaseDetail, opLogger);


                if (detailDao.DeleteReaPurchaseDetail(t.Rpc_no,string.Empty))
                {
                    foreach (EntityReaPurchaseDetail detail in t.ListReaPurchaseDetail)
                    {
                        detailDao.InsertNewReaPurchaseDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaPurchase", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rpc_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REAPURCHASE, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaPurchase OriginPurchaseInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaPurchaseDetail> OriginPurchaseDetail = OriginPurchaseInfo.ListReaPurchaseDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rpc_applier = caller.UserID;
                t.Rpc_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginPurchaseInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                mainDao.Dbm = helper;
                IDaoReaPurchaseDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaPurchaseData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "601";
                sampProcessDetial.ProcBarno = t.Rpc_no;
                sampProcessDetial.ProcBarcode = t.Rpc_no;
                sampProcessDetial.RepId = t.Rpc_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaPurchaseDetailBIZ().UpdateDetailBefore(t.ListReaPurchaseDetail, OriginPurchaseDetail, opLogger);


                if (detailDao.DeleteReaPurchaseDetail(t.Rpc_no,string.Empty))
                {
                    foreach (EntityReaPurchaseDetail detail in t.ListReaPurchaseDetail)
                    {
                        detailDao.InsertNewReaPurchaseDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaPurchase", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaPurchase> data)
        {
            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                mainDao.Dbm = helper;
                IDaoReaPurchaseDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
                detailDao.Dbm = helper;

                foreach (var apply in data)
                {
                    mainDao.DeleteReaPurchaseData(apply);
                    if (detailDao.DeleteReaPurchaseDetail(apply.Rpc_no,string.Empty))
                    {
                        foreach (EntityReaPurchaseDetail detail in apply.ListReaPurchaseDetail)
                        {
                            detail.Rpcd_no = apply.Rpc_no;
                            detailDao.CancelReaPurchaseDetail(detail);
                        }
                    }

                    #region 将修改病人信息的操作插入Samp_ process_detial表
                    string remark = $"IP:{caller.IPAddress}";
                    EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                    sampProcessDetial.ProcDate = caller.Time;
                    sampProcessDetial.ProcUsercode = caller.LoginID;
                    sampProcessDetial.ProcUsername = caller.LoginName;
                    sampProcessDetial.ProcStatus = "604";
                    sampProcessDetial.ProcBarno = apply.Rpc_no;
                    sampProcessDetial.ProcBarcode = apply.Rpc_no;
                    sampProcessDetial.RepId = apply.Rpc_no;
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
                Lib.LogManager.Logger.LogException("DeleteReaPurchase", ex);
                throw;
            }
            return res;
        }

        public EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t)
        {
            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rpc_applier = caller.UserID;
                t.Rpc_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                mainDao.Dbm = helper;

                mainDao.UpdateReaPurchaseData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "602";
                sampProcessDetial.ProcBarno = t.Rpc_no;
                sampProcessDetial.ProcBarcode = t.Rpc_no;
                sampProcessDetial.RepId = t.Rpc_no;
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
                Lib.LogManager.Logger.LogException("UpdateReaPurchase", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaPurchase t)
        {

            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaPurchase mainDao = DclDaoFactory.DaoHandler<IDaoReaPurchase>();
                mainDao.Dbm = helper;
                t.Rpc_returnid = caller.UserID;
                t.Rpc_status = "2";
                t.Rpc_returndate = ServerDateTime.GetDatabaseServerDateTime();

                mainDao.ReturnReaPurchaseData(t);


                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress} ";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "605";
                sampProcessDetial.ProcBarno = t.Rpc_no;
                sampProcessDetial.ProcBarcode = t.Rpc_no;
                sampProcessDetial.RepId = t.Rpc_no;
                sampProcessDetial.ProcContent = remark + t.Rpc_returnreason;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("ReturnReaPurchase", ex);
                throw;
            }
            return res;
        }
    }
}
