using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.entity;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// 结果类型是否与字典设置的一致
    /// </summary>
    public class CheckerResultDataType : AbstractAuditClass, IAuditChecker
    {
        public CheckerResultDataType(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode aduitType, AuditConfig config)
            : base(pat_info, null, resulto, aduitType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report || this.auditType == EnumOperationCode.PreAudit)
            {
                foreach (EntityObrResult resCurrent in this.resulto)//遍历当前检验结果
                {


                    if (string.IsNullOrEmpty(resCurrent.ObrValue))
                    {
                        continue;
                    }
                    //根据项目
                    EntityDicItemSample ettItemSam = dcl.svr.cache.DictItemSamCache.Current.GetItem(resCurrent.ItmId, pat_info.PidSamId);
                    if (ettItemSam == null)
                    {
                        continue;
                    }

                    if (ettItemSam.ItmResType == "数值")
                    {
                        double douTemp = 0;
                        decimal decCurrResChr;
                        string strRes_chr = resCurrent.ObrValue.Replace(">", "").Replace("<", "").Trim();
                        if (!decimal.TryParse((double.TryParse(strRes_chr, out douTemp) ? douTemp.ToString() : strRes_chr), out decCurrResChr))
                        {



                            chkResult.AddMessage(EnumOperationErrorCode.DataTypeIncorrect
                                                , string.Format("项目{0}的结果类型不一值,该项目设置结果类型为 {1} ", resCurrent.ItmEname, ettItemSam.ItmResType)
                                                , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_ResDataTypeError : config.Audit_Second_ErrorLevel_ResDataTypeError);


                        }
                    }


                }
            }
        }

        #endregion


    }
}
