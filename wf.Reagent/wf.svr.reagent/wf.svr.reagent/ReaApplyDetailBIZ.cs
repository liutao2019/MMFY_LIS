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
using System.Text;

namespace wf.svr.reagent
{
    public class ReaApplyDetailBIZ: DclBizBase,IReaApplyDetail
    {
        public bool DeleteCommonDetailByReaId(EntityLogLogin logLogin, string reaId, string rayNo)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(reaId))
            {
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();
                List<EntityReaApplyDetail> listDetail = GetReaApplyDetailByReaIN(reaId, rayNo);


                #region 填充操作日志记录
                //操作日志实体,用于插入一条记录
                EntitySysOperationLog operationLog = new EntitySysOperationLog();
                operationLog.OperatUserId = logLogin.LogLoginID.ToString();
                operationLog.OperatDate = time;
                operationLog.OperatKey = rayNo;
                operationLog.OperatServername = logLogin.LogIP;
                operationLog.OperatModule = "试剂资料";
                operationLog.OperatGroup = "试剂申领结果";
                operationLog.OperatAction = "删除";

                #endregion

                if (listDetail.Count > 0)
                {
                    EntityReaApplyDetail detail = listDetail[0];
                    operationLog.OperatObject = detail.ReagentName;
                    operationLog.OperatContent = detail.Rdet_reacount.ToString();
                }

                result = DeleteReaApplyDetailByIN(reaId,rayNo);
                if (result)
                {
                    //插入日志信息
                    result = new SysOperationLogBIZ().SaveSysOperationLog(operationLog);
                }
            }

            return result;
        }

        public bool DeleteReaApplyDetail(string repId)
        {
            bool result = false;
            IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                result = detailDao.DeleteReaApplyDetail(repId);
            }
            return result;
        }
        public bool InsertNewReaApplyDetail(List<EntityReaApplyDetail> repDetails)
        {
            bool result = false;
            IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                //插入组合前先删除
                if (DeleteReaApplyDetail(repDetails[0].Rdet_no))
                {
                    foreach (EntityReaApplyDetail detail in repDetails)
                    {
                        result = detailDao.InsertNewReaApplyDetail(detail);
                    }
                }
            }

            return result;
        }

        public List<EntityReaApplyDetail> GetReaApplyDetailByReaId(string reaId)
        {
            List<EntityReaApplyDetail> detailList = new List<EntityReaApplyDetail>();
            IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                detailList = detailDao.GetReaApplyDetailByReaId(reaId);
            }

            return detailList;
        }

        public List<EntityReaApplyDetail> GetReaApplyDetailByReaIN(string reaId,string rayno)
        {
            List<EntityReaApplyDetail> detailList = new List<EntityReaApplyDetail>();
            IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                detailList = detailDao.GetReaApplyDetailByReaIN(reaId, rayno);
            }

            return detailList;
        }

        bool DeleteReaApplyDetailByIN(string applyId, string rea_id)
        {
            bool result = false;
            IDaoReaApplyDetail detailDao = DclDaoFactory.DaoHandler<IDaoReaApplyDetail>();
            if (detailDao != null)
            {
                detailDao.Dbm = Dbm;
                result = detailDao.DeleteReaApplyDetailByIN(applyId,rea_id);
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
        public void UpdateReaDetailBefore(List<EntityReaApplyDetail> listDetail, List<EntityReaApplyDetail> listOriginDetail, OperationLogger opLogger)
        {
            #region 插入新试剂信息
            //获取需要新增的行
            List<EntityReaApplyDetail> listAddNew = listDetail.Where(w => w.IsNew == 1).ToList();
            if (listAddNew.Count > 0)
            {
                foreach (EntityReaApplyDetail entityInsert in listAddNew)
                {
                    //更新前判断该项目结果是否已经由他人录入，已录入则默认调到修改状态
                    string reaid = SQLFormater.Format(entityInsert.Rdet_reaid);
                    List<EntityReaApplyDetail> listOrigin = listOriginDetail.Where(w => w.Rdet_reaid == reaid).ToList();

                    if (listOrigin.Count > 0)
                    {
                        entityInsert.IsNew = 0;
                        continue;
                    }
                    opLogger.Add_AddLog(SysOperationLogGroup.REA_APPLYINFO, entityInsert.ReagentName, entityInsert.Rdet_reacount.ToString());
                }
            }
            #endregion

            #region 更新原结果信息
            //获取需要更新的行
            List<EntityReaApplyDetail> listUpdate = listDetail.Where(w => w.IsNew == 0).ToList();

            if (listUpdate.Count > 0)
            {

                //获取原有结果记录

                foreach (EntityReaApplyDetail entityCurrResult in listUpdate)
                {
                    string reaid = entityCurrResult.Rdet_reaid;
                    List<EntityReaApplyDetail> listOrigin = listOriginDetail.Where(w => w.Rdet_reaid == reaid).ToList();
                    if (listOrigin.Count > 0)
                    {
                        EntityReaApplyDetail entityOrigin = listOrigin[0];
                        //查找普通结果是否有更改
                        if (!ObjectEquals(entityOrigin.Rdet_reacount, entityCurrResult.Rdet_reacount))
                        {
                            string currValue = string.Empty;
                            string oldValue = string.Empty;

                            if (!string.IsNullOrEmpty(entityCurrResult.Rdet_reacount.ToString()))
                            {
                                currValue = entityCurrResult.Rdet_reacount.ToString();
                            }
                            if (!string.IsNullOrEmpty(entityOrigin.Rdet_reacount.ToString()))
                            {
                                oldValue = entityOrigin.Rdet_reacount.ToString();
                            }


                            opLogger.Add_ModifyLog(SysOperationLogGroup.REA_APPLYINFO, reaid + " " + entityCurrResult.ReagentName, oldValue + "→" + currValue);
                        }
                    }
                }
            }
            #endregion
        }

    }
}
