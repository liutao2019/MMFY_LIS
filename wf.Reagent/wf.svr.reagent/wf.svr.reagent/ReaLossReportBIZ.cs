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
   
    public class ReaLossReportBIZ : IReaLossReport
    {
        public void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace)
        {
            IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
            if (mainDao != null)
            {
                foreach (string repId in repIds)
                {
                    DateTime date = ServerDateTime.GetDatabaseServerDateTime();
                    //更新病人的状态为打印状态
                    mainDao.UpdateLossReportStatus(repId, OperatorID, date);
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
        public void UpdateReaInfoBefore(EntityReaLossReport ReaInfo, EntityReaLossReport OriginReaInfo, OperationLogger opLogger)
        {
            //采购单号
            string rea_sid = ReaInfo.Rlr_no;

            //日期
            DateTime rea_date = Convert.ToDateTime(ReaInfo.Rlr_date);

            //查找之前的样本号
            if (OriginReaInfo != null)
            {
                string prevSID = OriginReaInfo.Rlr_no;

                System.Reflection.PropertyInfo[] mPi = typeof(EntityReaLossReport).GetProperties();
                for (int i = 0; i < mPi.Length; i++)
                {
                    System.Reflection.PropertyInfo pi = mPi[i];
                    if (pi.Name != "ListReaLossReportDetail")
                    {
                        object oldValue = pi.GetValue(OriginReaInfo, null);
                        object newValue = pi.GetValue(ReaInfo, null);
                        if (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString())
                        {
                            string colCHS = FieldsNameConventer<ReaLossReportFields>.Instance.GetDataFieldCHS<ReaLossReportFields>(pi.Name);
                            if (!string.IsNullOrEmpty(colCHS))
                            {
                                opLogger.Add_ModifyLog(SysOperationLogGroup.REA_LOSSREPORTINFO, colCHS, oldValue + "→" + newValue);
                            }
                        }
                    }
                }
            }
        }

        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
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
            IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_sid, pat_date);
            }
            return result;
        }

        public EntityReaLossReport GetDataByNum(EntityReaQC patientCondition, bool withLossReportDetail)
        {
            List<EntityReaLossReport> data = new List<EntityReaLossReport>();
            if (!string.IsNullOrEmpty(patientCondition.ReaNo))
            {
                IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
                if (mainDao != null)
                {
                    data = mainDao.QueryLossReportList(patientCondition);
                }
                else
                {
                    Lib.LogManager.Logger.LogInfo("DclDaoFactory.DaoHandler<IDaoReaLossReport>()=null");
                }

                if (data != null && data.Count > 0)
                {
                    //是否要查组合信息
                    if (withLossReportDetail)
                    {
                        data[0].ListReaLossReportDetail = new ReaLossReportDetailBIZ().GetDetail(patientCondition);
                    }
                }
            }
            return data[0];
        }

        public EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t)
        {
            EntityOperateResult result = new EntityOperateResult();

            List<EntityReaLossReport> list = new List<EntityReaLossReport>();

            try
            {
                if (!string.IsNullOrEmpty(caller.UserID))
                {
                    t.Rlr_operator = caller.UserID;
                    t.Rlr_date = ServerDateTime.GetDatabaseServerDateTime();
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
                sampProcessDetial.ProcBarno = t.Rlr_no;
                sampProcessDetial.ProcBarcode = t.Rlr_no;
                sampProcessDetial.RepId = t.Rlr_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SaveReaLossReport", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }

            return result;
        }

        public bool InsertNewData(List<EntityReaLossReport> data)
        {
            bool result = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
            mainDao.Dbm = helper;
            if (mainDao != null)
            {
                try
                {
                    foreach (EntityReaLossReport item in data)
                    {
                        result = mainDao.InsertNewReaLossReport(item);
                        //插入病人信息成功后插入病人组合信息
                        if (result && item.ListReaLossReportDetail.Count > 0)
                        {
                            int i = 0;

                            foreach (EntityReaLossReportDetail detail in item.ListReaLossReportDetail)
                            {
                                if (!string.IsNullOrEmpty(detail.Rld_barcode))
                                    detail.Rld_no = item.Rlr_no;
                                i++;

                            }
                            IDaoReaLossReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
                            if (detailDao != null)
                            {
                                detailDao.Dbm = helper;
                                //插入组合前先删除
                                if (detailDao.DeleteReaLossReportDetail(item.ListReaLossReportDetail[0].Rld_no,string.Empty))
                                {
                                    foreach (EntityReaLossReportDetail detail in item.ListReaLossReportDetail)
                                    {
                                        result = detailDao.InsertNewReaLossReportDetail(detail);
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

        public List<EntityReaLossReport> ReaQuery(EntityReaQC patientCondition)
        {
            List<EntityReaLossReport> listRea = new List<EntityReaLossReport>();
            IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
            if (mainDao != null)
            {
                listRea = mainDao.QueryLossReportList(patientCondition);
            }
            return listRea;
        }

        public EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rlr_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REALOSSREPORT, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaLossReport OriginLossReportInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaLossReportDetail> OriginLossReportDetail = OriginLossReportInfo.ListReaLossReportDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rlr_operator = caller.UserID;
                t.Rlr_date = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginLossReportInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
                mainDao.Dbm = helper;
                IDaoReaLossReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaLossReportData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "607";
                sampProcessDetial.ProcBarno = t.Rlr_no;
                sampProcessDetial.ProcBarcode = t.Rlr_no;
                sampProcessDetial.RepId = t.Rlr_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaLossReportDetailBIZ().UpdateDetailBefore(t.ListReaLossReportDetail, OriginLossReportDetail, opLogger);


                if (detailDao.DeleteReaLossReportDetail(t.Rlr_no,string.Empty))
                {
                    foreach (EntityReaLossReportDetail detail in t.ListReaLossReportDetail)
                    {
                        detailDao.InsertNewReaLossReportDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaLossReport", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rlr_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REALOSSREPORT, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaLossReport OriginLossReportInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaLossReportDetail> OriginLossReportDetail = OriginLossReportInfo.ListReaLossReportDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rlr_operator = caller.UserID;
                t.Rlr_date = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginLossReportInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
                mainDao.Dbm = helper;
                IDaoReaLossReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaLossReportData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "601";
                sampProcessDetial.ProcBarno = t.Rlr_no;
                sampProcessDetial.ProcBarcode = t.Rlr_no;
                sampProcessDetial.RepId = t.Rlr_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaLossReportDetailBIZ().UpdateDetailBefore(t.ListReaLossReportDetail, OriginLossReportDetail, opLogger);


                if (detailDao.DeleteReaLossReportDetail(t.Rlr_no,string.Empty))
                {
                    foreach (EntityReaLossReportDetail detail in t.ListReaLossReportDetail)
                    {
                        detailDao.InsertNewReaLossReportDetail(detail);
                        //if (detail.Rld_barstatus == 0)
                        //{
                              //StorageCount(detail, helper, "-");
                        //}
                        //IDaoReaStorageDetail stoDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                        //stoDao.Dbm = helper;

                        //EntityReaQC query = new EntityReaQC();
                        //query.ReaNo = detail.StoreNo;
                        //query.Barcode = detail.Rld_barcode;

                        //List<EntityReaStorageDetail> sDetail = stoDao.GetReaStorageDetail(query);

                        //stoDao.UpdateDetailStatus(detail.Rld_barcode, 1, sDetail[0].Rsd_count - detail.Rld_reacount, detail.StoreNo);
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
                Lib.LogManager.Logger.LogException("UpdateReaLossReport", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public void StorageCount(EntityReaLossReportDetail detail, DBManager helper, string operation)
        {
            IDaoReaStoreCount dao = DclDaoFactory.DaoHandler<IDaoReaStoreCount>();
            if (dao != null)
            {
                dao.Dbm = helper;
                EntityReaStoreCount entity = new EntityReaStoreCount();
                EntityReaQC reaQC = new EntityReaQC();
                reaQC.Barcode = detail.Rld_barcode;

                reaQC.ReaId = detail.Rld_reaid;
                reaQC.ReaNo = detail.Rld_no;
                List<EntityReaStoreCount> list = dao.SearchByQC(reaQC);
                if (list != null && list.Count > 0)
                {
                    entity = list[0];
                    if (string.Equals(operation, "+"))
                    {
                        entity.Rri_Count += detail.Rld_reacount;
                    }
                    else if (string.Equals(operation, "-"))
                    {
                        entity.Rri_Count -= detail.Rld_reacount;
                    }
                    dao.UpdateReaStoreCount(entity);
                }

            }
        }

        public bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaLossReport> data)
        {
            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
                mainDao.Dbm = helper;
                IDaoReaLossReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
                detailDao.Dbm = helper;

                foreach (var apply in data)
                {
                    mainDao.DeleteReaLossReportData(apply);
                    if (detailDao.DeleteReaLossReportDetail(apply.Rlr_no,string.Empty))
                    {
                        foreach (EntityReaLossReportDetail detail in apply.ListReaLossReportDetail)
                        {
                            detail.Rld_no = apply.Rlr_no;
                            detailDao.CancelReaLossReportDetail(detail);
                        }
                    }

                    #region 将修改病人信息的操作插入Samp_ process_detial表
                    string remark = $"IP:{caller.IPAddress}";
                    EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                    sampProcessDetial.ProcDate = caller.Time;
                    sampProcessDetial.ProcUsercode = caller.LoginID;
                    sampProcessDetial.ProcUsername = caller.LoginName;
                    sampProcessDetial.ProcStatus = "604";
                    sampProcessDetial.ProcBarno = apply.Rlr_no;
                    sampProcessDetial.ProcBarcode = apply.Rlr_no;
                    sampProcessDetial.RepId = apply.Rlr_no;
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
                Lib.LogManager.Logger.LogException("DeleteReaLossReport", ex);
                throw;
            }
            return res;
        }

        public EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t)
        {
            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rlr_operator = caller.UserID;
                t.Rlr_date = ServerDateTime.GetDatabaseServerDateTime();
            }
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
                mainDao.Dbm = helper;

                mainDao.UpdateReaLossReportData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "602";
                sampProcessDetial.ProcBarno = t.Rlr_no;
                sampProcessDetial.ProcBarcode = t.Rlr_no;
                sampProcessDetial.RepId = t.Rlr_no;
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
                Lib.LogManager.Logger.LogException("UpdateReaLossReport", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaLossReport t)
        {

            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaLossReport mainDao = DclDaoFactory.DaoHandler<IDaoReaLossReport>();
                mainDao.Dbm = helper;
                t.Rlr_returnid = caller.UserID;
                t.Rlr_status = "2";
                t.Rlr_returndate = ServerDateTime.GetDatabaseServerDateTime();

                mainDao.ReturnReaLossReportData(t);

                //foreach (EntityReaLossReportDetail detail in t.ListReaLossReportDetail)
                //{
                //    IDaoReaStorageDetail stoDao = DclDaoFactory.DaoHandler<IDaoReaStorageDetail>();
                //    stoDao.Dbm = helper;
                //    EntityReaQC query = new EntityReaQC();
                //    query.ReaNo = detail.StoreNo;
                //    query.Barcode = detail.Rld_barcode;

                //    List<EntityReaStorageDetail> sDetail = stoDao.GetReaStorageDetail(query);
                //    //stoDao.UpdateDetailStatus(detail.Rld_barcode, 1, sDetail[0].Rsd_count + detail.Rld_reacount, detail.StoreNo);

                //    if (detail.Rld_barstatus == 0)
                //    {
                          //StorageCount(detail, helper, "+");
                //    }
                //}


                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress} ";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "605";
                sampProcessDetial.ProcBarno = t.Rlr_no;
                sampProcessDetial.ProcBarcode = t.Rlr_no;
                sampProcessDetial.RepId = t.Rlr_no;
                sampProcessDetial.ProcContent = remark + t.Rlr_returnreason;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("ReturnReaLossReport", ex);
                throw;
            }
            return res;
        }
    }
}
