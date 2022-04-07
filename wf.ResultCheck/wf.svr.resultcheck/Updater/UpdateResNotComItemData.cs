using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Lib.DAC;

using dcl.root.logon;
using System.Data;
using System.Threading;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 二审时更新所有没有组合id的项目
    /// </summary>
    public  class UpdateResNotComItemData : AbstractAuditClass, IAuditUpdater
    {
        public UpdateResNotComItemData(EntityPidReportMain pat_info, AuditConfig config, EnumOperationCode auditType)
            : base(pat_info, null, null, auditType, config)
        {
        }

        public void Update(ref EntityOperationResult chkResult)
        {
            //二审时
            if (this.auditType == EnumOperationCode.Report)
            {
                Thread t = new Thread(_Update);
                t.Start();
            }
        }

        public void _Update()
        {
            if (!string.IsNullOrEmpty(pat_info.RepId))
            {
                string pat_id = pat_info.RepId;

                #region 二审时更新无组合结果数据

                try
                {
                    EntityResultQC qc = new EntityResultQC();
                    qc.ListObrId.Add(pat_id);
                    qc.ItmComIdIsNull = true;
                    qc.ItmIdIsNull = true;
                    List<EntityObrResult> listNullComIDItems = new List<EntityObrResult>();
                    IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                    if (resultDao != null)
                    {
                        listNullComIDItems = resultDao.ObrResultQuery(qc);
                    }
                    if (listNullComIDItems.Count > 0)
                    {
                        List<EntityPidReportDetail> listatcom = new List<EntityPidReportDetail>();
                        IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
                        if (detailDao != null)
                        {
                            listatcom = detailDao.GetPidReportDetailByRepId(pat_id);
                        }
                        List<string> listComIds = new List<string>();
                        if (listatcom.Count > 0)
                        {
                            foreach (EntityPidReportDetail drPatCom in listatcom)
                            {
                                listComIds.Add( drPatCom.ComId);
                            }

                            IDaoDicCombineDetail comDetailDao = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
                            List<EntityDicCombineDetail> dtComMi = new List<EntityDicCombineDetail>();
                            if (comDetailDao != null)
                            {
                                dtComMi = comDetailDao.GetComDetailByComIds(listComIds);
                            }
  
                            if (dtComMi.Count > 0)
                            {
        
                                foreach (EntityObrResult rowNullComIDItem in listNullComIDItems)
                                {
                                    string itm_id = rowNullComIDItem.ItmId;

                                    List<EntityDicCombineDetail> drsComMi = dtComMi.Where(w=>w.ComItmId==itm_id).ToList();

                                    if (drsComMi.Count > 0)
                                    {
                                        IDaoObrResult dao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                                        if(dao != null)
                                        {
                                            dao.UpdateResultComIdByObrIdAndItmID(drsComMi[0].ComId, pat_id, itm_id);
                                        }

                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "UpdateResNotComItemData", ex.ToString());
                }

                #endregion
            }
        }
    }
    
}
