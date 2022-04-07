using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.cache;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 更新patients_mi表信息
    /// </summary>
    public class UpdatePatientsMi : AbstractAuditClass, IAuditUpdater
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="pat_info">病人基本信息</param>
        /// <param name="config">审核配置</param>
        /// <param name="auditType">审核类型</param>
        /// <param name="caller">操作者信息</param>
        public UpdatePatientsMi(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, AuditConfig config, EnumOperationCode auditType)
            : base(pat_info, patients_mi, null, auditType, config)
        {
        }

        #region IAuditUpdater 成员

        public void Update(ref EntityOperationResult chkResult)
        {
            if (this.auditType == EnumOperationCode.Audit
                ||
                (this.auditType == EnumOperationCode.Report && config.bAllowStepAuditToReport)
                )
            {
                try
                {
                    //当使用条码时，更新医嘱id
                    if (!string.IsNullOrEmpty(pat_info.RepBarCode)&& CacheSysConfig.Current.GetSystemConfig("Lab_UpdatePatMiYzid")=="是")
                    {
                        IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
                        if (daoDetail != null)
                        {
                            List<EntitySampDetail> listSampDetail = daoDetail.GetSampDetailByBarCodeAndComId(pat_info.RepBarCode, new List<string>());

                            foreach (EntityPidReportDetail pat_mi in this.patients_mi)
                            {
                                if (string.IsNullOrEmpty(pat_mi.OrderSn))
                                {
                                    EntitySampDetail sampDetail = listSampDetail.Find(i => i.ComId == pat_mi.ComId);
                                    if (sampDetail != null)
                                    {
                                        pat_mi.OrderSn = sampDetail.OrderSn;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }

        #endregion
    }
}
