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
using dcl.svr.users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace wf.svr.reagent
{
    public class ReaDeliveryDetailBIZ: DclBizBase, IReaDeliveryDetail
    {
        bool DeleteDetail(EntityReaQC qc)
        {
            bool result = false;
            IDaoReaDeliveryDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                result = pcdailDao.DeleteReaDeliveryDetail(qc.ReaNo,qc.ReaId);
            }

            return result;
        }
        private bool ObjectEquals(object obj1, object obj2)
        {
            if (
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 == null || obj2 == DBNull.Value))
                ||
                ((obj1 == null || obj1 == DBNull.Value) && (obj2 != null && obj2 != DBNull.Value && obj2.ToString() == string.Empty))
                ||
                ((obj2 == null || obj2 == DBNull.Value) && (obj1 != null && obj1 != DBNull.Value && obj1.ToString() == string.Empty))
            )
            {
                return true;
            }
            else
            {
                if (obj1.ToString() == obj2.ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public void UpdateDetailBefore(List<EntityReaDeliveryDetail> listDetail, List<EntityReaDeliveryDetail> listOriginDetail, OperationLogger opLogger)
        {
            #region 插入新试剂信息
            //获取需要新增的行
            List<EntityReaDeliveryDetail> listAddNew = listDetail.Where(w => w.IsNew == 1).ToList();
            if (listAddNew.Count > 0)
            {
                foreach (EntityReaDeliveryDetail entityInsert in listAddNew)
                {
                    //更新前判断该项目结果是否已经由他人录入，已录入则默认调到修改状态
                    string reaid = SQLFormater.Format(entityInsert.Rdvd_reaid);
                    List<EntityReaDeliveryDetail> listOrigin = listOriginDetail.Where(w => w.Rdvd_reaid == reaid).ToList();

                    if (listOrigin.Count > 0)
                    {
                        entityInsert.IsNew = 0;
                        continue;
                    }
                    opLogger.Add_AddLog(SysOperationLogGroup.REA_DELIVERYINFO, entityInsert.ReagentName, entityInsert.Rdvd_reacount.ToString());
                }
            }
            #endregion

            #region 更新原结果信息
            //获取需要更新的行
            List<EntityReaDeliveryDetail> listUpdate = listDetail.Where(w => w.IsNew == 0).ToList();

            if (listUpdate.Count > 0)
            {

                //获取原有结果记录

                foreach (EntityReaDeliveryDetail entityCurrResult in listUpdate)
                {
                    string reaid = entityCurrResult.Rdvd_reaid;
                    List<EntityReaDeliveryDetail> listOrigin = listOriginDetail.Where(w => w.Rdvd_reaid == reaid).ToList();
                    if (listOrigin.Count > 0)
                    {
                        EntityReaDeliveryDetail entityOrigin = listOrigin[0];
                        //查找普通结果是否有更改
                        if (!ObjectEquals(entityOrigin.Rdvd_reacount, entityCurrResult.Rdvd_reacount))
                        {
                            string currValue = string.Empty;
                            string oldValue = string.Empty;

                            if (!string.IsNullOrEmpty(entityCurrResult.Rdvd_reacount.ToString()))
                            {
                                currValue = entityCurrResult.Rdvd_reacount.ToString();
                            }
                            if (!string.IsNullOrEmpty(entityOrigin.Rdvd_reacount.ToString()))
                            {
                                oldValue = entityOrigin.Rdvd_reacount.ToString();
                            }


                            opLogger.Add_ModifyLog(SysOperationLogGroup.REA_DELIVERYINFO, reaid + " " + entityCurrResult.ReagentName, oldValue + "→" + currValue);
                        }
                    }
                }
            }
            #endregion
        }

        public bool DeleteObrResultByObrSn(string obrSn)
        {
            bool result = false;
            IDaoReaDeliveryDetail resultDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
            if (resultDao != null)
            {
                result = resultDao.DeleteObrResultByObrSn(obrSn);
            }
            return result;
        }

        public bool DeleteCommonResultItemByObrSn(EntityLogLogin logLogin, string obrSn, string repId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrSn))
            {
                EntityReaQC reaQc = new EntityReaQC();
                reaQc.ObrSn = obrSn;
                List<EntityReaDeliveryDetail> listResult = GetDetail(reaQc);
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();

                #region 填充操作日志记录
                //操作日志实体,用于插入一条记录
                EntitySysOperationLog operationLog = new EntitySysOperationLog();
                operationLog.OperatUserId = logLogin.LogLoginID.ToString();
                operationLog.OperatDate = time;
                operationLog.OperatKey = repId;
                operationLog.OperatServername = logLogin.LogIP;
                operationLog.OperatModule = "试剂资料";
                operationLog.OperatGroup = "试剂出库";
                operationLog.OperatAction = "删除";

                #endregion

                if (listResult.Count > 0)
                {
                    EntityReaDeliveryDetail obrResult = listResult[0];
                    operationLog.OperatObject = obrResult.ReagentName;
                    operationLog.OperatContent = obrResult.Rdvd_reacount.ToString();
                }

                result = DeleteObrResultByObrSn(obrSn);
                if (result)
                {
                    //插入日志信息
                    result = new SysOperationLogBIZ().SaveSysOperationLog(operationLog);
                }
            }

            return result;
        }

        public bool InsertNewDetail(List<EntityReaDeliveryDetail> repDetails)
        {
            bool result = false;
            IDaoReaDeliveryDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                EntityReaQC qc = new EntityReaQC();
                qc.ReaNo = repDetails[0].Rdvd_no;
                //插入组合前先删除
                if (DeleteDetail(qc))
                {
                    foreach (EntityReaDeliveryDetail pcdail in repDetails)
                    {
                        result = pcdailDao.InsertNewReaDeliveryDetail(pcdail);
                    }
                }
            }

            return result;
        }

        public List<EntityReaDeliveryDetail> GetDetail(EntityReaQC qc)
        {
            List<EntityReaDeliveryDetail> pcdailList = new List<EntityReaDeliveryDetail>();
            IDaoReaDeliveryDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaDeliveryDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                pcdailList = pcdailDao.GetReaDeliveryDetail(qc);
            }

            return pcdailList;
        }

        public bool DeleteCommonDetail(EntityLogLogin logLogin, EntityReaQC qc)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(qc.ReaId))
            {
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();
                List<EntityReaDeliveryDetail> listDetail = GetDetail(qc);

                #region 填充操作日志记录
                //操作日志实体,用于插入一条记录
                EntitySysOperationLog operationLog = new EntitySysOperationLog();
                operationLog.OperatUserId = logLogin.LogLoginID.ToString();
                operationLog.OperatDate = time;
                operationLog.OperatKey = qc.ReaNo;
                operationLog.OperatServername = logLogin.LogIP;
                operationLog.OperatModule = "试剂资料";
                operationLog.OperatGroup = "试剂出库";
                operationLog.OperatAction = "删除";

                #endregion

                if (listDetail.Count > 0)
                {
                    EntityReaDeliveryDetail pcdail = listDetail[0];
                    operationLog.OperatObject = pcdail.ReagentName;
                    operationLog.OperatContent = pcdail.Rdvd_reacount.ToString();
                }

                result = DeleteDetail(qc);
                if (result)
                {
                    //插入日志信息
                    result = new SysOperationLogBIZ().SaveSysOperationLog(operationLog);
                }
            }

            return result;
        }
    }
}
