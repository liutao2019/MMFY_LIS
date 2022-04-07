using System;
using System.Collections.Generic;
using dcl.svr.cache;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 质控检查
    /// </summary>
    public class CheckerQCRule : AbstractAuditClass, IAuditChecker
    {
        public CheckerQCRule(EntityPidReportMain pat_info, EnumOperationCode auditType)
            : base(pat_info, null, null, auditType, null)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
            {

                EntityDicInstrument ettItr = DictInstrmtCache.Current.GetInstructmentByID(this.pat_info.RepItrId);

                if (ettItr != null && ettItr.ItrReportType != null && ettItr.ItrQcCheck == 1)
                {
                    List<EntityDicQcRuleMes> listQcRuleMes = GetQcRuleMes(this.pat_info.RepId);//拿指控消息

                    if (listQcRuleMes.Count > 0)
                    {
                        foreach (EntityDicQcRuleMes QcRuleMes in listQcRuleMes)
                        {
                            string strRuleType = QcRuleMes.QcmType;
                            switch (strRuleType)
                            {
                                case "新增"://新增的质控数据
                                    chkResult.AddMessage(EnumOperationErrorCode.QcAddValue, QcRuleMes.ItmEcode, EnumOperationErrorLevel.Error);
                                    break;
                                case "失控":
                                    //chkResult.AddMessage(EnumOperationErrorCode.QcOutOfControl, drQcRuleMes["res_itm_ecd"].ToString(), EnumOperationErrorLevel.Warn);
                                    chkResult.AddMessage(EnumOperationErrorCode.QcOutOfControl, QcRuleMes.ItmEcode, EnumOperationErrorLevel.Error);

                                    break;
                                case "在控":
                                    DateTime deStartTime = Convert.ToDateTime(QcRuleMes.QrmStartTime);
                                    DateTime deEndTime = Convert.ToDateTime(QcRuleMes.QrmStartTime);
                                    TimeSpan tS = DateTime.Now.Subtract(deStartTime);
                                    TimeSpan tE = deEndTime.Subtract(DateTime.Now);
                                    if (tS.Seconds < 0 || tE.Seconds < 0)//如果在控并且超出时间范围
                                        chkResult.AddMessage(EnumOperationErrorCode.QcDayExpire, QcRuleMes.ItmEcode, EnumOperationErrorLevel.Error);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public List<EntityDicQcRuleMes> GetQcRuleMes(string pat_id)
        {
            List<EntityDicQcRuleMes> list = new List<EntityDicQcRuleMes>();
            IDaoQcRuleMes dao = DclDaoFactory.DaoHandler<IDaoQcRuleMes>();
            if (dao != null)
            {
                list = dao.GetQcRuleMes(pat_id);
            }
            return list;
        }
        #endregion
    }
}
