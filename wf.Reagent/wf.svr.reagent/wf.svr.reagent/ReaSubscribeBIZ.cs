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
   
    public class ReaSubscribeBIZ : IReaSubscribe
    {
        public void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace)
        {
            IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
            if (mainDao != null)
            {
                foreach (string repId in repIds)
                {
                    DateTime date = ServerDateTime.GetDatabaseServerDateTime();
                    //更新病人的状态为打印状态
                    mainDao.UpdateSubscribeStatus(repId, OperatorID, date);
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
        public void UpdateReaInfoBefore(EntityReaSubscribe ReaInfo, EntityReaSubscribe OriginReaInfo, OperationLogger opLogger)
        {
            //采购单号
            string rea_sid = ReaInfo.Rsb_no;

            //日期
            DateTime rea_date = Convert.ToDateTime(ReaInfo.Rsb_date);

            //查找之前的样本号
            if (OriginReaInfo != null)
            {
                string prevSID = OriginReaInfo.Rsb_no;

                System.Reflection.PropertyInfo[] mPi = typeof(EntityReaSubscribe).GetProperties();
                for (int i = 0; i < mPi.Length; i++)
                {
                    System.Reflection.PropertyInfo pi = mPi[i];
                    if (pi.Name != "ListReaSubscribeDetail")
                    {
                        object oldValue = pi.GetValue(OriginReaInfo, null);
                        object newValue = pi.GetValue(ReaInfo, null);
                        if (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString())
                        {
                            string colCHS = FieldsNameConventer<ReaSubscribeFields>.Instance.GetDataFieldCHS<ReaSubscribeFields>(pi.Name);
                            if (!string.IsNullOrEmpty(colCHS))
                            {
                                opLogger.Add_ModifyLog(SysOperationLogGroup.REA_SUBSCRIBEINFO, colCHS, oldValue + "→" + newValue);
                            }
                        }
                    }
                }
            }
        }

        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
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
            IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_sid, pat_date);
            }
            return result;
        }

        public EntityReaSubscribe GetDataByNum(EntityReaQC patientCondition, bool withSubscribeDetail)
        {
            List<EntityReaSubscribe> data = new List<EntityReaSubscribe>();
            if (!string.IsNullOrEmpty(patientCondition.ReaNo))
            {
                IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
                if (mainDao != null)
                {
                    data = mainDao.QuerySubscribeList(patientCondition);
                }
                else
                {
                    Lib.LogManager.Logger.LogInfo("DclDaoFactory.DaoHandler<IDaoReaSubscribe>()=null");
                }

                if (data != null && data.Count > 0)
                {
                    //是否要查组合信息
                    if (withSubscribeDetail)
                    {
                        data[0].ListReaSubscribeDetail = new ReaSubscribeDetailBIZ().GetDetail(patientCondition);
                    }
                }
            }
            return data[0];
        }

        public EntityOperateResult SaveReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t)
        {
            EntityOperateResult result = new EntityOperateResult();

            List<EntityReaSubscribe> list = new List<EntityReaSubscribe>();

            try
            {
                if (!string.IsNullOrEmpty(caller.UserID))
                {
                    t.Rsb_applier = caller.UserID;
                    t.Rsb_applydate = ServerDateTime.GetDatabaseServerDateTime();
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
                sampProcessDetial.ProcBarno = t.Rsb_no;
                sampProcessDetial.ProcBarcode = t.Rsb_no;
                sampProcessDetial.RepId = t.Rsb_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SaveReaSubscribe", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }

            return result;
        }

        public bool InsertNewData(List<EntityReaSubscribe> data)
        {
            bool result = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
            mainDao.Dbm = helper;
            if (mainDao != null)
            {
                try
                {
                    foreach (EntityReaSubscribe item in data)
                    {
                        result = mainDao.InsertNewReaSubscribe(item);
                        //插入病人信息成功后插入病人组合信息
                        if (result && item.ListReaSubscribeDetail.Count > 0)
                        {
                            int i = 0;

                            foreach (EntityReaSubscribeDetail detail in item.ListReaSubscribeDetail)
                            {
                                if (!string.IsNullOrEmpty(detail.Rsbd_reaid))
                                    detail.Rsbd_no = item.Rsb_no;
                                i++;

                            }
                            IDaoReaSubscribeDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaSubscribeDetail>();
                            if (detailDao != null)
                            {
                                detailDao.Dbm = helper;
                                //插入组合前先删除
                                if (detailDao.DeleteReaSubscribeDetail(item.ListReaSubscribeDetail[0].Rsbd_no,string.Empty))
                                {
                                    foreach (EntityReaSubscribeDetail detail in item.ListReaSubscribeDetail)
                                    {
                                        result = detailDao.InsertNewReaSubscribeDetail(detail);
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

        public List<EntityReaSubscribe> ReaQuery(EntityReaQC patientCondition)
        {
            List<EntityReaSubscribe> listRea = new List<EntityReaSubscribe>();
            IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
            if (mainDao != null)
            {
                listRea = mainDao.QuerySubscribeList(patientCondition);
            }
            return listRea;
        }

        public EntityOperateResult UpdateReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rsb_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REAAPPLYGENTS, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaSubscribe OriginSubscribeInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaSubscribeDetail> OriginSubscribeDetail = OriginSubscribeInfo.ListReaSubscribeDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rsb_applier = caller.UserID;
                t.Rsb_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginSubscribeInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
                mainDao.Dbm = helper;
                IDaoReaSubscribeDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaSubscribeDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaSubscribeData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "607";
                sampProcessDetial.ProcBarno = t.Rsb_no;
                sampProcessDetial.ProcBarcode = t.Rsb_no;
                sampProcessDetial.RepId = t.Rsb_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaSubscribeDetailBIZ().UpdateDetailBefore(t.ListReaSubscribeDetail, OriginSubscribeDetail, opLogger);


                if (detailDao.DeleteReaSubscribeDetail(t.Rsb_no,string.Empty))
                {
                    foreach (EntityReaSubscribeDetail detail in t.ListReaSubscribeDetail)
                    {
                        detailDao.InsertNewReaSubscribeDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaSubscribe", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public EntityOperateResult AuditReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t)
        {
            EntityReaQC qc = new EntityReaQC();
            qc.ReaNo = t.Rsb_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REAAPPLYGENTS, qc.ReaNo);

            //原试剂基本资料信息
            EntityReaSubscribe OriginSubscribeInfo = GetDataByNum(qc, true);

            //原试剂组合信息
            List<EntityReaSubscribeDetail> OriginSubscribeDetail = OriginSubscribeInfo.ListReaSubscribeDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rsb_applier = caller.UserID;
                t.Rsb_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaInfoBefore(t, OriginSubscribeInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
                mainDao.Dbm = helper;
                IDaoReaSubscribeDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaSubscribeDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaSubscribeData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "601";
                sampProcessDetial.ProcBarno = t.Rsb_no;
                sampProcessDetial.ProcBarcode = t.Rsb_no;
                sampProcessDetial.RepId = t.Rsb_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaSubscribeDetailBIZ().UpdateDetailBefore(t.ListReaSubscribeDetail, OriginSubscribeDetail, opLogger);


                if (detailDao.DeleteReaSubscribeDetail(t.Rsb_no,string.Empty))
                {
                    foreach (EntityReaSubscribeDetail detail in t.ListReaSubscribeDetail)
                    {
                        detailDao.InsertNewReaSubscribeDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaSubscribe", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool DeleteReaData(EntityRemoteCallClientInfo caller, List<EntityReaSubscribe> data)
        {
            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
                mainDao.Dbm = helper;
                IDaoReaSubscribeDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaSubscribeDetail>();
                detailDao.Dbm = helper;

                foreach (var apply in data)
                {
                    mainDao.DeleteReaSubscribeData(apply);
                    if (detailDao.DeleteReaSubscribeDetail(apply.Rsb_no,string.Empty))
                    {
                        foreach (EntityReaSubscribeDetail detail in apply.ListReaSubscribeDetail)
                        {
                            detail.Rsbd_no = apply.Rsb_no;
                            detailDao.CancelReaSubscribeDetail(detail);
                        }
                    }

                    #region 将修改病人信息的操作插入Samp_ process_detial表
                    string remark = $"IP:{caller.IPAddress}";
                    EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                    sampProcessDetial.ProcDate = caller.Time;
                    sampProcessDetial.ProcUsercode = caller.LoginID;
                    sampProcessDetial.ProcUsername = caller.LoginName;
                    sampProcessDetial.ProcStatus = "604";
                    sampProcessDetial.ProcBarno = apply.Rsb_no;
                    sampProcessDetial.ProcBarcode = apply.Rsb_no;
                    sampProcessDetial.RepId = apply.Rsb_no;
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
                Lib.LogManager.Logger.LogException("DeleteReaSubscribe", ex);
                throw;
            }
            return res;
        }

        public EntityOperateResult UndoReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t)
        {
            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                t.Rsb_applier = caller.UserID;
                t.Rsb_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
                mainDao.Dbm = helper;

                mainDao.UpdateReaSubscribeData(t);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "602";
                sampProcessDetial.ProcBarno = t.Rsb_no;
                sampProcessDetial.ProcBarcode = t.Rsb_no;
                sampProcessDetial.RepId = t.Rsb_no;
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
                Lib.LogManager.Logger.LogException("UpdateReaSubscribe", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public bool ReturnReaData(EntityRemoteCallClientInfo caller, EntityReaSubscribe t)
        {

            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaSubscribe mainDao = DclDaoFactory.DaoHandler<IDaoReaSubscribe>();
                mainDao.Dbm = helper;
                t.Rsb_returnid = caller.UserID;
                t.Rsb_status = "2";
                t.Rsb_returndate = ServerDateTime.GetDatabaseServerDateTime();

                mainDao.ReturnReaSubscribeData(t);


                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress} ";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "605";
                sampProcessDetial.ProcBarno = t.Rsb_no;
                sampProcessDetial.ProcBarcode = t.Rsb_no;
                sampProcessDetial.RepId = t.Rsb_no;
                sampProcessDetial.ProcContent = remark + t.Rsb_returnreason;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("ReturnReaSubscribe", ex);
                throw;
            }
            return res;
        }
    }
}
