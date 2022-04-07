using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck
{
    public class CheckerItrMaintainance : AbstractAuditClass, IAuditChecker
    {
        public CheckerItrMaintainance(EntityPidReportMain patinfo, EnumOperationCode auditType, AuditConfig config)
            : base(patinfo, null, null, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
            {
                return;
                string itr_id = this.pat_info.RepItrId;
                List<EntityDicQcRuleMes> listQcRuleMes = new List<EntityDicQcRuleMes>();
                IDaoQcRuleMes dao = DclDaoFactory.DaoHandler<IDaoQcRuleMes>();
                if (dao != null)
                {
                    listQcRuleMes = dao.GetQcRuleMesByItrId(itr_id);
                }


                EntityDicInstrument ettItr = dcl.svr.cache.DictInstrmtCache.Current.GetInstructmentByID(itr_id);

                //if (ettItr != null && ettItr.itr_del != "1")
                //{
                foreach (EntityDicQcRuleMes qcRuleMes in listQcRuleMes)
                {
                    if (qcRuleMes.QcmType == "故障")
                    {
                        chkResult.AddMessage(EnumOperationErrorCode.ItrFault, ettItr.ItrName
                            , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_ItrFalut : config.Audit_Second_ErrorLevel_ItrFalut
                            );
                    }

                    if (qcRuleMes.QcmType == "到期")
                    {
                        //DateTime dtStart = Convert.ToDateTime(row["qrm_start_time"]);
                        DateTime dtEnd = Convert.ToDateTime(qcRuleMes.QrmEndTime);

                        if (DateTime.Now > dtEnd)
                        {

                            chkResult.AddMessage(EnumOperationErrorCode.ItrMaintainDayExpire,
                                string.Format("{0}({1})"
                                               , qcRuleMes.MaiContent
                                               , Convert.ToDateTime(qcRuleMes.QrmEndTime).ToString("yyyy-MM-dd HH:mm"))
                                , EnumOperationErrorLevel.Warn);
                        }
                    }
                }
            }
        }
        #endregion
    }
}
