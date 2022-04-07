using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using dcl.root.logon;
using System.Threading;
using Lib.DAC;
using System.Data;
using dcl.svr.cache;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck.Updater
{
    /// <summary>
    /// 更新结果表-分期参考值
    /// </summary>
    public class UpdateResultoRefExp : AbstractAuditClass, IAuditUpdater
    {
        public UpdateResultoRefExp(EntityPidReportMain pat_info,  List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, resulto, auditType, config)
        {
        }

        public void Update(ref EntityOperationResult chkResult)
        {
            //||
            //(this.auditType == EnumOperationCode.Report && config.bAllowStepAuditToReport)
            if (this.auditType == EnumOperationCode.Report)
            {
                Thread t = new Thread(ThreadUpdateRefExp);
                t.Start();
            }
            else if (this.auditType == EnumOperationCode.UndoReport)
            {
                Thread t = new Thread(ThreadClearRefExp);
                t.Start();
            }
        }


        public void Update()
        {
            ThreadUpdateRefExp();
        }

        /// <summary>
        /// 二审时相应结果写入分期多参考值
        /// </summary>
        void ThreadUpdateRefExp()
        {
            try
            {
                //判断是否有分期标志
                if (resulto != null && resulto.Count > 0 && resulto.FindAll(item => (DictItemMiCache.Current.DclCache.FindAll(item2 => (item2.ItmId == item.ItmId&& item2.ItmRefFlag == 1)).Count > 0)).Count > 0)
                {
                    List<EntityObrResult> listUpdate = resulto.FindAll(item => (DictItemMiCache.Current.DclCache.FindAll(item2 => (item2.ItmId == item.ItmId && item2.ItmRefFlag == 1)).Count > 0));//有分期标志的项目

                    string sam_id = pat_info.PidSamId;//标本ID
                    string pat_sex=pat_info.PidSex;//性别
                    string pat_itm_itr_id = pat_info.RepItrId;//仪器

                    if (string.IsNullOrEmpty(sam_id))
                    {
                        return;//标本ID为空,不执行
                    }

                    if (dcl.svr.cache.DictItemMiCache.Current.DclCache != null
                            && dcl.svr.cache.DictItemMiCache.Current.DclCache.Count > 0)
                    {
                        foreach (EntityObrResult res in listUpdate)
                        {
                            //筛选分期参考值
                            //筛选条件：项目，标本，分期标志，性别
                            List<EntityDicItmRefdetail> ListdictItmMi = dcl.svr.cache.DictItemMiCache.Current.DclCache.FindAll(item => (item.ItmId == res.ItmId && item.ItmSamId == sam_id
                                && item.ItmRefFlag == 1 && (item.ItmSex == pat_sex || (item.ItmSex != "1" && item.ItmSex != "2"))));

                            //检查当前仪器是否有专用的多参考值
                            if (ListdictItmMi != null && ListdictItmMi.Count > 0
                                && !string.IsNullOrEmpty(pat_itm_itr_id)
                                && ListdictItmMi.FindAll(item => item.ItmItrId == pat_itm_itr_id).Count>0)
                            {
                                List<EntityDicItmRefdetail> ListdictItmMiTemp = ListdictItmMi.FindAll(item => item.ItmItrId == pat_itm_itr_id);
                                ListdictItmMi = ListdictItmMiTemp;
                            }

                            if (ListdictItmMi.Count > 0)
                            {
                                string strTempAllref = "";//多参考值字符串{格式:a 1-2[1]b 2-3[2]c 3-5[3]}

                                for (int j = 0; j < ListdictItmMi.Count; j++)
                                {
                                    strTempAllref += string.Format("{0} {1}{2}*", ListdictItmMi[j].ItmRefName.ToString()
                                        , ListdictItmMi[j].ItmLowerLimitValue.ToString() 
                                        + ((string.IsNullOrEmpty(ListdictItmMi[j].ItmLowerLimitValue.ToString()) || string.IsNullOrEmpty(ListdictItmMi[j].ItmUpperLimitValue.ToString())) ? "" : "-")
                                        , ListdictItmMi[j].ItmUpperLimitValue.ToString());
                                }
                                strTempAllref = strTempAllref.TrimEnd('*');

                                IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                                if (dao != null)
                                {
                                    dao.UpdateResultRefMore(res.ObrSn.ToString(), null, strTempAllref, false);//执行更新
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("ThreadUpdateRefExp", ex);
                //chkResult.AddMessage(EnumOperationErrorCode.Exception, ex.StackTrace, EnumOperationErrorLevel.Error);
            }
        }

        /// <summary>
        /// 取消二审时清空结果分期多参考值
        /// </summary>
        void ThreadClearRefExp()
        {
            try
            {
                foreach (EntityObrResult res in this.resulto)
                {
                    if (res.ObrId.Length > 0)
                    {
                        IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                        if (dao != null)
                        {
                            dao.UpdateResultRefMore(null, res.ObrId,null,true);
                        }
                        break;
                    }  
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("ThreadClearRefExp", ex);
                //chkResult.AddMessage(EnumOperationErrorCode.Exception, ex.StackTrace, EnumOperationErrorLevel.Error);
            }
        }
    }
}
