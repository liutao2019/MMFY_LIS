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
    public class ReaLossReportDetailBIZ: DclBizBase, IReaLossReportDetail
    {
        bool DeleteDetail(EntityReaQC qc)
        {
            bool result = false;
            IDaoReaLossReportDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                result = pcdailDao.DeleteReaLossReportDetail(qc.ReaNo,qc.ReaId);
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
        public void UpdateDetailBefore(List<EntityReaLossReportDetail> listDetail, List<EntityReaLossReportDetail> listOriginDetail, OperationLogger opLogger)
        {
            #region 插入新试剂信息
            //获取需要新增的行
            List<EntityReaLossReportDetail> listAddNew = listDetail.Where(w => w.IsNew == 1).ToList();
            if (listAddNew.Count > 0)
            {
                foreach (EntityReaLossReportDetail entityInsert in listAddNew)
                {
                    //更新前判断该项目结果是否已经由他人录入，已录入则默认调到修改状态
                    string reaid = SQLFormater.Format(entityInsert.Rld_reaid);
                    List<EntityReaLossReportDetail> listOrigin = listOriginDetail.Where(w => w.Rld_reaid == reaid).ToList();

                    if (listOrigin.Count > 0)
                    {
                        entityInsert.IsNew = 0;
                        continue;
                    }
                    opLogger.Add_AddLog(SysOperationLogGroup.REA_LOSSREPORTINFO, entityInsert.ReagentName, entityInsert.Rld_barcode.ToString());
                }
            }
            #endregion

            #region 更新原结果信息
            //获取需要更新的行
            List<EntityReaLossReportDetail> listUpdate = listDetail.Where(w => w.IsNew == 0).ToList();

            if (listUpdate.Count > 0)
            {

                //获取原有结果记录

                foreach (EntityReaLossReportDetail entityCurrResult in listUpdate)
                {
                    string reaid = entityCurrResult.Rld_reaid;
                    List<EntityReaLossReportDetail> listOrigin = listOriginDetail.Where(w => w.Rld_reaid == reaid).ToList();
                    if (listOrigin.Count > 0)
                    {
                        EntityReaLossReportDetail entityOrigin = listOrigin[0];
                        //查找普通结果是否有更改
                        if (!ObjectEquals(entityOrigin.Rld_barcode, entityCurrResult.Rld_barcode))
                        {
                            string currValue = string.Empty;
                            string oldValue = string.Empty;

                            if (!string.IsNullOrEmpty(entityCurrResult.Rld_barcode.ToString()))
                            {
                                currValue = entityCurrResult.Rld_barcode.ToString();
                            }
                            if (!string.IsNullOrEmpty(entityOrigin.Rld_barcode.ToString()))
                            {
                                oldValue = entityOrigin.Rld_barcode.ToString();
                            }


                            opLogger.Add_ModifyLog(SysOperationLogGroup.REA_LOSSREPORTINFO, reaid + " " + entityCurrResult.ReagentName, oldValue + "→" + currValue);
                        }
                    }
                }
            }
            #endregion
        }

        public bool InsertNewDetail(List<EntityReaLossReportDetail> repDetails)
        {
            bool result = false;
            IDaoReaLossReportDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                EntityReaQC qc = new EntityReaQC();
                qc.ReaNo = repDetails[0].Rld_no;
                //插入组合前先删除
                if (DeleteDetail(qc))
                {
                    foreach (EntityReaLossReportDetail pcdail in repDetails)
                    {
                        result = pcdailDao.InsertNewReaLossReportDetail(pcdail);
                    }
                }
            }

            return result;
        }

        public List<EntityReaLossReportDetail> GetDetail(EntityReaQC qc)
        {
            List<EntityReaLossReportDetail> pcdailList = new List<EntityReaLossReportDetail>();
            IDaoReaLossReportDetail pcdailDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
            if (pcdailDao != null)
            {
                pcdailDao.Dbm = Dbm;
                pcdailList = pcdailDao.GetReaLossReportDetail(qc);
            }

            return pcdailList;
        }

        public bool DeleteCommonDetail(EntityLogLogin logLogin, EntityReaQC qc)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(qc.ReaId))
            {
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();
                List<EntityReaLossReportDetail> listDetail = GetDetail(qc);

                #region 填充操作日志记录
                //操作日志实体,用于插入一条记录
                EntitySysOperationLog operationLog = new EntitySysOperationLog();
                operationLog.OperatUserId = logLogin.LogLoginID.ToString();
                operationLog.OperatDate = time;
                operationLog.OperatKey = qc.ReaNo;
                operationLog.OperatServername = logLogin.LogIP;
                operationLog.OperatModule = "试剂资料";
                operationLog.OperatGroup = "试剂报损";
                operationLog.OperatAction = "删除";

                #endregion

                if (listDetail.Count > 0)
                {
                    EntityReaLossReportDetail pcdail = listDetail[0];
                    operationLog.OperatObject = pcdail.ReagentName;
                    operationLog.OperatContent = pcdail.Rld_barcode.ToString();
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

        public bool DeleteCommonResultItemByObrSn(EntityLogLogin logLogin, string obrSn, string repId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrSn))
            {
                EntityReaQC reaQc = new EntityReaQC();
                reaQc.ObrSn = obrSn;
                List<EntityReaLossReportDetail> listResult = GetDetail(reaQc);
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();

                #region 填充操作日志记录
                //操作日志实体,用于插入一条记录
                EntitySysOperationLog operationLog = new EntitySysOperationLog();
                operationLog.OperatUserId = logLogin.LogLoginID.ToString();
                operationLog.OperatDate = time;
                operationLog.OperatKey = repId;
                operationLog.OperatServername = logLogin.LogIP;
                operationLog.OperatModule = "试剂资料";
                operationLog.OperatGroup = "试剂报损";
                operationLog.OperatAction = "删除";

                #endregion

                if (listResult.Count > 0)
                {
                    EntityReaLossReportDetail obrResult = listResult[0];
                    operationLog.OperatObject = obrResult.ReagentName;
                    operationLog.OperatContent = obrResult.Rld_barcode.ToString();
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

        public bool DeleteObrResultByObrSn(string obrSn)
        {
            bool result = false;
            IDaoReaLossReportDetail resultDao = DclDaoFactory.DaoHandler<IDaoReaLossReportDetail>();
            if (resultDao != null)
            {
                result = resultDao.DeleteObrResultByObrSn(obrSn);
            }
            return result;
        }
    }
}
