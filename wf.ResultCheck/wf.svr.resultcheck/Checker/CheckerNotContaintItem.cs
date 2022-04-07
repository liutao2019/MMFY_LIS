using System.Collections.Generic;
using System.Linq;
using dcl.svr.cache;
using System.Data;
using dcl.entity;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// (不包含/多余)项目检查
    /// </summary>
    public class CheckerNotContaintItem : AbstractAuditClass, IAuditChecker
    {
        public CheckerNotContaintItem(EntityPidReportMain pat_info, List<EntityPidReportDetail> patients_mi, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, patients_mi, resulto, auditType, config)
        {

        }

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report || this.auditType == EnumOperationCode.PreAudit)
            {
                var query = from pat_com in patients_mi
                            join com_mi in DictCombineMiCache2.Current.DclCache on pat_com.ComId equals com_mi.ComId
                            join dict_item in DictItemCache.Current.DclCache on com_mi.ComItmId equals dict_item.ItmId
                            where dict_item.ItmDelFlag != "1" 
                            select dict_item;

                //记录多余项目代码
                List<string> notContaintItemList = new List<string>();
                string notContaintItems = "";

                if (query.Count() > 0 && this.resulto != null && this.resulto.Count>0)
                {
                    foreach (EntityObrResult item_res in this.resulto)
                    {
                        //检查是否多余的项目
                        if (!query.Any(i => i.ItmId == item_res.ItmId))
                        {
                            if (!notContaintItemList.Contains(item_res.ItmEname))
                            {
                                notContaintItemList.Add(item_res.ItmEname);

                                if (string.IsNullOrEmpty(notContaintItems))
                                {
                                    notContaintItems = item_res.ItmEname;
                                }
                                else
                                {
                                    notContaintItems +="," +item_res.ItmEname;
                                }
                            }
                        }
                    }
                }

                if (notContaintItemList.Count > 0)
                {
                    chkResult.AddCustomMessage("", "", string.Format("存在多余的项目：{0};", notContaintItems), EnumOperationErrorLevel.Warn);
                }
            }
        }
    }
}
