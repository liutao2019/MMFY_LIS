using System.Collections.Generic;
using System.Linq;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    public class CheckerItemSample : AbstractAuditClass, IAuditChecker
    {
        public CheckerItemSample(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, resulto, auditType, config)
        {

        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (resulto.Count > 0)
            {
                List<string> listItemId = new List<string>();
                foreach (var item in this.resulto)
                {
                    listItemId.Add(item.ItmId);
                }

                List<EntityDicItemSample> ettItemSam = dcl.svr.cache.DictItemSamCache.Current.DclCache;

                var q = from itm_sam in ettItemSam
                        where itm_sam.ItmSamId == this.pat_info.PidSamId
                              && (itm_sam.ItmItrId == this.pat_info.RepItrId || itm_sam.ItmItrId == "-1")
                              && (listItemId.ToArray()).Contains(itm_sam.ItmId)

                        group itm_sam by itm_sam.ItmId;

                if (q.Count() < resulto.Count)
                {
                    chkResult.AddCustomMessage("4212", "标本类别错误", "标本类别与检验项目不符", config.Audit_First_ErrorLevel_NotAllowSample);
                }
            }
        }

        #endregion
    }
}
