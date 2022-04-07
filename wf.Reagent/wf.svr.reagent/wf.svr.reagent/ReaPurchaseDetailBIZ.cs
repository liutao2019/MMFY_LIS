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
    public class ReaPurchaseDetailBIZ: DclBizBase, IReaPurchaseDetail
    {
        bool DeleteDetail(EntityReaQC qc)
        {
            bool result = false;
            IDaoReaPurchaseDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                result = pcdailDao.DeleteReaPurchaseDetail(qc.ReaNo,qc.ReaId);
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
        public void UpdateDetailBefore(List<EntityReaPurchaseDetail> listDetail, List<EntityReaPurchaseDetail> listOriginDetail, OperationLogger opLogger)
        {
            #region 插入新试剂信息
            //获取需要新增的行
            List<EntityReaPurchaseDetail> listAddNew = listDetail.Where(w => w.IsNew == 1).ToList();
            if (listAddNew.Count > 0)
            {
                foreach (EntityReaPurchaseDetail entityInsert in listAddNew)
                {
                    //更新前判断该项目结果是否已经由他人录入，已录入则默认调到修改状态
                    string reaid = SQLFormater.Format(entityInsert.Rpcd_reaid);
                    List<EntityReaPurchaseDetail> listOrigin = listOriginDetail.Where(w => w.Rpcd_reaid == reaid).ToList();

                    if (listOrigin.Count > 0)
                    {
                        entityInsert.IsNew = 0;
                        continue;
                    }
                    opLogger.Add_AddLog(SysOperationLogGroup.REA_PURCHASEINFO, entityInsert.ReagentName, entityInsert.Rpcd_reacount.ToString());
                }
            }
            #endregion

            #region 更新原结果信息
            //获取需要更新的行
            List<EntityReaPurchaseDetail> listUpdate = listDetail.Where(w => w.IsNew == 0).ToList();

            if (listUpdate.Count > 0)
            {

                //获取原有结果记录

                foreach (EntityReaPurchaseDetail entityCurrResult in listUpdate)
                {
                    string reaid = entityCurrResult.Rpcd_reaid;
                    List<EntityReaPurchaseDetail> listOrigin = listOriginDetail.Where(w => w.Rpcd_reaid == reaid).ToList();
                    if (listOrigin.Count > 0)
                    {
                        EntityReaPurchaseDetail entityOrigin = listOrigin[0];
                        //查找普通结果是否有更改
                        if (!ObjectEquals(entityOrigin.Rpcd_reacount, entityCurrResult.Rpcd_reacount))
                        {
                            string currValue = string.Empty;
                            string oldValue = string.Empty;

                            if (!string.IsNullOrEmpty(entityCurrResult.Rpcd_reacount.ToString()))
                            {
                                currValue = entityCurrResult.Rpcd_reacount.ToString();
                            }
                            if (!string.IsNullOrEmpty(entityOrigin.Rpcd_reacount.ToString()))
                            {
                                oldValue = entityOrigin.Rpcd_reacount.ToString();
                            }


                            opLogger.Add_ModifyLog(SysOperationLogGroup.REA_PURCHASEINFO, reaid + " " + entityCurrResult.ReagentName, oldValue + "→" + currValue);
                        }
                    }
                }
            }
            #endregion
        }

        public bool InsertNewDetail(List<EntityReaPurchaseDetail> repDetails)
        {
            bool result = false;
            IDaoReaPurchaseDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                EntityReaQC qc = new EntityReaQC();
                qc.ReaNo = repDetails[0].Rpcd_no;
                //插入组合前先删除
                if (DeleteDetail(qc))
                {
                    foreach (EntityReaPurchaseDetail pcdail in repDetails)
                    {
                        result = pcdailDao.InsertNewReaPurchaseDetail(pcdail);
                    }
                }
            }

            return result;
        }

        public List<EntityReaPurchaseDetail> GetDetail(EntityReaQC qc)
        {
            List<EntityReaPurchaseDetail> pcdailList = new List<EntityReaPurchaseDetail>();
            IDaoReaPurchaseDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaPurchaseDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                pcdailList = pcdailDao.GetReaPurchaseDetail(qc);
            }

            return pcdailList;
        }

        public bool DeleteCommonDetail(EntityLogLogin logLogin, EntityReaQC qc)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(qc.ReaId))
            {
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();
                List<EntityReaPurchaseDetail> listDetail = GetDetail(qc);

                #region 填充操作日志记录
                //操作日志实体,用于插入一条记录
                EntitySysOperationLog operationLog = new EntitySysOperationLog();
                operationLog.OperatUserId = logLogin.LogLoginID.ToString();
                operationLog.OperatDate = time;
                operationLog.OperatKey = qc.ReaNo;
                operationLog.OperatServername = logLogin.LogIP;
                operationLog.OperatModule = "试剂资料";
                operationLog.OperatGroup = "试剂采购结果";
                operationLog.OperatAction = "删除";

                #endregion

                if (listDetail.Count > 0)
                {
                    EntityReaPurchaseDetail pcdail = listDetail[0];
                    operationLog.OperatObject = pcdail.ReagentName;
                    operationLog.OperatContent = pcdail.Rpcd_reacount.ToString();
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
