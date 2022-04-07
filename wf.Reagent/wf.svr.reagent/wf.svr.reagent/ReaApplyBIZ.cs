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
using System.Text;

namespace wf.svr.reagent
{
    public class ReaApplyBIZ:IReaApply
    {
        public void UpdatePrintState_whitOperator(IEnumerable<string> repIds, string OperatorID, string OperatorName, string strPlace)
        {
            IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
            if (mainDao != null)
            {
                foreach (string repId in repIds)
                {
                    DateTime date = ServerDateTime.GetDatabaseServerDateTime();
                    //更新病人的状态为打印状态
                    mainDao.UpdateApplyStatus(repId, OperatorID, date);
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
        /// 更新试剂申领基本信息前的一些处理
        /// </summary>
        /// <param name="dtPatientsInfo"></param>
        /// <param name="result"></param>
        /// <param name="transHelper"></param>
        /// <returns></returns>
        public void UpdateReaApplyInfoBefore(EntityReaApply ReaApplyInfo, EntityReaApply OriginReaApplyInfo, OperationLogger opLogger)
        {
            //申领单号
            string rea_sid = ReaApplyInfo.Ray_no;

            //日期
            DateTime rea_date = Convert.ToDateTime(ReaApplyInfo.Ray_date);

            //查找之前的样本号
            if (OriginReaApplyInfo != null)
            {
                string prevSID = OriginReaApplyInfo.Ray_no;

                System.Reflection.PropertyInfo[] mPi = typeof(EntityReaApply).GetProperties();
                for (int i = 0; i < mPi.Length; i++)
                {
                    System.Reflection.PropertyInfo pi = mPi[i];
                    if (pi.Name != "ListReaApplyDetail")
                    {
                        object oldValue = pi.GetValue(OriginReaApplyInfo, null);
                        object newValue = pi.GetValue(ReaApplyInfo, null);
                        if (oldValue != null && newValue != null && oldValue.ToString() != newValue.ToString())
                        {
                            string colCHS = FieldsNameConventer<ReaApplyFields>.Instance.GetReaFieldCHS(pi.Name);
                            if (!string.IsNullOrEmpty(colCHS))
                            {
                                opLogger.Add_ModifyLog(SysOperationLogGroup.REA_APPLYINFO, colCHS, oldValue + "→" + newValue);
                            }
                        }
                    }
                }
            }
        }

        public EntityOperateResult SaveReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply)
        {
            EntityOperateResult result = new EntityOperateResult();

            List<EntityReaApply> listApplies = new List<EntityReaApply>();

            try
            {
                if (!string.IsNullOrEmpty(caller.UserID))
                {
                    apply.Ray_applier = caller.UserID;
                    apply.Ray_applydate = ServerDateTime.GetDatabaseServerDateTime();
                }

                listApplies.Add(apply);
                InsertNewApply(listApplies);

                #region 将修改病人信息的操作插入Samp_ process_detial表

                string remark = $"IP:{caller.IPAddress}";

                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "600";
                sampProcessDetial.ProcBarno = apply.Ray_no;
                sampProcessDetial.ProcBarcode = apply.Ray_no;
                sampProcessDetial.RepId = apply.Ray_no;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("SaveReaApply", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
            }

            return result;
        }

        public EntityOperateResult UpdateReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply)
        {
            string rayno = apply.Ray_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REAAPPLYGENTS, rayno);

            //原试剂基本资料信息
            EntityReaApply OriginApplyInfo = GetReaApplyByRayNo(rayno, true);

            //原试剂组合信息
            List<EntityReaApplyDetail> OriginApplyDetail = OriginApplyInfo.ListReaApplyDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                apply.Ray_applier = caller.UserID;
                apply.Ray_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaApplyInfoBefore(apply, OriginApplyInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                mainDao.Dbm = helper;
                IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaApplyData(apply);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "607";
                sampProcessDetial.ProcBarno = rayno;
                sampProcessDetial.ProcBarcode = rayno;
                sampProcessDetial.RepId = rayno;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaApplyDetailBIZ().UpdateReaDetailBefore(apply.ListReaApplyDetail, OriginApplyDetail, opLogger);


                if (detailDao.DeleteReaApplyDetail(apply.Ray_no))
                {
                    foreach (EntityReaApplyDetail detail in apply.ListReaApplyDetail)
                    {
                        detailDao.InsertNewReaApplyDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaApply", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public EntityOperateResult AuditReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply)
        {
            string rayno = apply.Ray_no;
            //创建日记记录对象
            OperationLogger opLogger = new OperationLogger(caller.LoginID, caller.IPAddress, SysOperationLogModule.REAAPPLYGENTS, rayno);

            //原试剂基本资料信息
            EntityReaApply OriginApplyInfo = GetReaApplyByRayNo(rayno, true);

            //原试剂组合信息
            List<EntityReaApplyDetail> OriginApplyDetail = OriginApplyInfo.ListReaApplyDetail;

            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                apply.Ray_applier = caller.UserID;
                apply.Ray_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }

            //更新病人基本信息前的一些处理
            UpdateReaApplyInfoBefore(apply, OriginApplyInfo, opLogger);


            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                mainDao.Dbm = helper;
                IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
                detailDao.Dbm = helper;

                mainDao.UpdateReaApplyData(apply);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "601";
                sampProcessDetial.ProcBarno = rayno;
                sampProcessDetial.ProcBarcode = rayno;
                sampProcessDetial.RepId = rayno;
                sampProcessDetial.ProcContent = remark;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion

                //更新组合前的一些处理
                new ReaApplyDetailBIZ().UpdateReaDetailBefore(apply.ListReaApplyDetail, OriginApplyDetail, opLogger);


                if (detailDao.DeleteReaApplyDetail(apply.Ray_no))
                {
                    foreach (EntityReaApplyDetail detail in apply.ListReaApplyDetail)
                    {
                        detailDao.InsertNewReaApplyDetail(detail);
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
                Lib.LogManager.Logger.LogException("UpdateReaApply", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }

        public EntityOperateResult UndoReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply)
        {
            EntityOperateResult result = new EntityOperateResult();

            if (!string.IsNullOrEmpty(caller.UserID))
            {
                apply.Ray_applier = caller.UserID;
                apply.Ray_applydate = ServerDateTime.GetDatabaseServerDateTime();
            }
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                mainDao.Dbm = helper;

                mainDao.UpdateReaApplyData(apply);

                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress}";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "602";
                sampProcessDetial.ProcBarno = apply.Ray_no;
                sampProcessDetial.ProcBarcode = apply.Ray_no;
                sampProcessDetial.RepId = apply.Ray_no;
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
                Lib.LogManager.Logger.LogException("UpdateReaApply", ex);
                result.AddMessage(EnumOperateErrorCode.Exception, ex.ToString(), EnumOperateErrorLevel.Error);
                throw;
            }
            return result;
        }
        /// <summary>
        /// 数据库基本存储
        /// </summary>
        /// <param name="patients"></param>
        /// <returns></returns>
        public bool InsertNewApply(List<EntityReaApply> applies)
        {
            bool result = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
            mainDao.Dbm = helper;
            if (mainDao != null)
            {
                try
                {
                    
                    foreach (EntityReaApply item in applies)
                    {
                        result = mainDao.InsertNewReaApply(item);
                        //插入病人信息成功后插入病人组合信息
                        if (result && item.ListReaApplyDetail.Count > 0)
                        {
                            int i = 0;
                            item.ListReaApplyDetail.OrderBy(w => w.sort_no);

                            foreach (EntityReaApplyDetail detail in item.ListReaApplyDetail)
                            {
                                if (!string.IsNullOrEmpty(detail.Rdet_reaid))
                                    detail.Rdet_no = item.Ray_no;
                                detail.sort_no = i.ToString();

                                i++;

                            }
                            IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
                            if (detailDao != null)
                            {
                                detailDao.Dbm = helper;
                                //插入组合前先删除
                                if (detailDao.DeleteReaApplyDetail(item.ListReaApplyDetail[0].Rdet_no))
                                {
                                    foreach (EntityReaApplyDetail detail in item.ListReaApplyDetail)
                                    {
                                        result = detailDao.InsertNewReaApplyDetail(detail);
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

        public string GetReaSID_MaxPlusOne(DateTime date, string stepCode)
        {
            IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
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
        /// 申领信息查询
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        public List<EntityReaApply> ReaQuery(EntityReaQC patientCondition)
        {
            List<EntityReaApply> listRea = new List<EntityReaApply>();
            IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
            if (mainDao != null)
            {
                listRea = mainDao.QueryApplyList(patientCondition);
            }
            return listRea;
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
            IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
            if (mainDao != null)
            {
                result = mainDao.ExsitSidOrHostOrder(pat_sid, pat_date);
            }
            return result;
        }

        public bool DeleteReaApply(EntityRemoteCallClientInfo caller, List<EntityReaApply> applies)
        {
            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                mainDao.Dbm = helper;
                IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
                detailDao.Dbm = helper;

                foreach (var apply in applies)
                {
                    mainDao.DeleteReaApplyData(apply);
                    if (detailDao.DeleteReaApplyDetail(apply.Ray_no))
                    {
                        foreach (EntityReaApplyDetail detail in apply.ListReaApplyDetail)
                        {
                            detail.Rdet_no = apply.Ray_no;
                            detailDao.CancelReaApplyDetail(detail);
                        }
                    }

                    #region 将修改病人信息的操作插入Samp_ process_detial表
                    string remark = $"IP:{caller.IPAddress}";
                    EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                    sampProcessDetial.ProcDate = caller.Time;
                    sampProcessDetial.ProcUsercode = caller.LoginID;
                    sampProcessDetial.ProcUsername = caller.LoginName;
                    sampProcessDetial.ProcStatus = "604";
                    sampProcessDetial.ProcBarno = apply.Ray_no;
                    sampProcessDetial.ProcBarcode = apply.Ray_no;
                    sampProcessDetial.RepId = apply.Ray_no;
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
                Lib.LogManager.Logger.LogException("DeleteReaApply", ex);
                throw;
            }
            return res;
        }

        public bool ReturnReaApply(EntityRemoteCallClientInfo caller, EntityReaApply apply)
        {
            bool res = false;
            DBManager helper = new DBManager();
            helper.BeginTrans();
            try
            {
                IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                mainDao.Dbm = helper;
                apply.Ray_returnid = caller.UserID;
                apply.Ray_status = "2";
                apply.Ray_returndate = ServerDateTime.GetDatabaseServerDateTime();

                mainDao.ReturnReaApplyData(apply);


                #region 将修改病人信息的操作插入Samp_ process_detial表
                string remark = $"IP:{caller.IPAddress} ";
                EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();
                sampProcessDetial.ProcDate = caller.Time;
                sampProcessDetial.ProcUsercode = caller.LoginID;
                sampProcessDetial.ProcUsername = caller.LoginName;
                sampProcessDetial.ProcStatus = "605";
                sampProcessDetial.ProcBarno = apply.Ray_no;
                sampProcessDetial.ProcBarcode = apply.Ray_no;
                sampProcessDetial.RepId = apply.Ray_no;
                sampProcessDetial.ProcContent = remark + apply.Ray_rejectreason;

                new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(sampProcessDetial);

                #endregion
                helper.CommitTrans();
                helper = null;
            }
            catch (Exception ex)
            {
                helper.CommitTrans();
                helper = null;
                Lib.LogManager.Logger.LogException("ReturnReaApply", ex);
                throw;
            }
            return res;
        }

        public EntityReaApply GetReaApplyByRayNo(string strRayNo, bool withApplyDetail)
        {
            EntityReaApply apply = null;
            if (!string.IsNullOrEmpty(strRayNo))
            {
                IDaoReaApply mainDao = DclDaoFactory.DaoHandler<IDaoReaApply>();
                if (mainDao != null)
                {
                    apply = mainDao.GetReaApplyInfo(strRayNo);
                }
                else
                {
                    Lib.LogManager.Logger.LogInfo("DclDaoFactory.DaoHandler<IDaoReaApply>()=null");
                }

                if (apply != null)
                {
                    //是否要查组合信息
                    if (withApplyDetail)
                    {
                        apply.ListReaApplyDetail = new ReaApplyDetailBIZ().GetReaApplyDetailByReaId(strRayNo);
                    }
                }
            }
            return apply;
        }

    }
}
