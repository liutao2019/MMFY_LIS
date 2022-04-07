using dcl.servececontract;
using System;
using System.Collections.Generic;
using dcl.entity;
using dcl.svr.dicbasic;
using dcl.dao.core;

namespace dcl.svr.result
{
    public class PatMonitorBIZ : DclBizBase, IPatMonitor
    {
        public EntityResponse GetPatMonitor(string type_id, string itr_id, DateTime patDate)
        {
            EntityResponse respone = new EntityResponse();
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                //仪器的查询条件
                List<string> listItrs = new List<string>();
                if (string.IsNullOrEmpty(itr_id))
                {
                    List<EntityDicInstrument> listItr = new InstrmtBIZ().GetInstrumentByItridOrItrType("", type_id);

                    foreach (EntityDicInstrument Itr in listItr)
                    {
                        listItrs.Add(Itr.ItrId);
                    }

                    if (listItrs.Count == 0)
                    {
                        listItrs.Add("-1");
                    }
                }
                else
                {
                    listItrs.Add(itr_id);
                }

                List<EntityPidReportMain> listNormal = new List<EntityPidReportMain>();

                //获取有结果的病人资料
                listNormal = new PidReportMainBIZ().GetPatByExistResult(listItrs, patDate.Date, patDate.Date.AddDays(1).AddMilliseconds(-1), true);
                dict.Add("normal", listNormal);
                //if (!string.IsNullOrEmpty(itr_id))
                //{
                //    listItrs = new List<string>();
                //    List<EntityDicInstrument> listItr = new InstrmtBIZ().GetInstrumentByItridOrItrType("", type_id);
                //    foreach (EntityDicInstrument rowItr in listItr)
                //    {
                //        listItrs.Add(rowItr.ItrId);
                //    }
                //    if (listItrs.Count == 0)
                //    {
                //        listItrs .Add("-1");
                //    }
                //}
                //获取没有结果的病人资料
                List<EntityPidReportMain> listNoResult = new PidReportMainBIZ().GetPatByExistResult(listItrs, patDate.Date, patDate.Date.AddDays(1).AddMilliseconds(-1), false);
                dict.Add("noresult", listNoResult);

                //获取没有病人的结果
                List<EntityObrResult> listNotPat = new ObrResultBIZ().GetObrResultByNoPat(listItrs, patDate, patDate.AddDays(1));
                dict.Add("nopat", listNotPat);

                List<EntityPidReportMain> listAllType = new List<EntityPidReportMain>();
                listAllType = new PidReportMainBIZ().GetAllLabPat(type_id, patDate.Date, patDate.Date.AddDays(1).AddMilliseconds(-1));
                dict.Add("AllTypepat", listAllType);
                respone.SetResult(dict);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取样本进程信息出错", ex);
            }
            return respone;
        }
    }
}
