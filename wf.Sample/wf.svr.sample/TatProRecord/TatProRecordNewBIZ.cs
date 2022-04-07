using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.svr.dicbasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace dcl.svr.sample
{
    /// <summary>
    /// 将TAT监控涉及的表tat_pro_record的操作方法移出来重写，方便调用
    /// </summary>
    public class TatProRecordNewBIZ
    {
        List<EntityDicCombineTimeRule> listTatTime = new List<EntityDicCombineTimeRule>();

        List<EntityDicCombineTimeRule> RuleIn = new List<EntityDicCombineTimeRule>();

        string timeType = "常规";
        public bool TatRecode(EntitySampOperation operation, EntitySampMain sampMain)
        {
            bool result = false;
            if (sampMain.SampUrgentFlag)
                timeType = "急查";
            ThradParameters parames = new ThradParameters(operation, sampMain);
            listTatTime = new List<EntityDicCombineTimeRule>();
            RuleIn = new List<EntityDicCombineTimeRule>();
            foreach (EntitySampDetail detail in sampMain.ListSampDetail)
            {
                EntityDicCombine dr = new EntityDicCombine();
                dr.ComId = detail.ComId;
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(dr);
                EntityResponse ds = new ItemCombineBIZ().Other(request);
                Dictionary<string, object> dict = ds.GetResult() as Dictionary<string, object>;
                List<EntityDicCombineTimeRule> listRule = new List<EntityDicCombineTimeRule>();
                List<EntityDicCombineTimeruleRelated> listRelated = new List<EntityDicCombineTimeruleRelated>();
                object objRule = dict["Rule"];
                object objRelated = dict["Related"];
                if (objRule != null)
                {
                    listRule = objRule as List<EntityDicCombineTimeRule>;
                }
                if (objRelated != null)
                {
                    listRelated = objRelated as List<EntityDicCombineTimeruleRelated>;
                }
                RuleIn = listRule.Where(p => listRelated.Where(g => g.ComTimeId == p.ComTimeId).Any()).ToList();
                List<EntityDicCombineTimeRule> RuleInTemp = RuleIn.FindAll(w => w.ComTimeOriId == sampMain.SrcId && w.ComTimeStartType == operation.OperationStatus && w.ComTimeType == timeType);
                listTatTime.AddRange(RuleInTemp);
            }
            listTatTime = listTatTime.GroupBy(x => x.ComTimeStartType.ToString() + "|" + x.ComTimeEndType.ToString()).Select(x => x.OrderBy(y => y.ComTime).Last()).ToList();
            if (!string.IsNullOrEmpty(sampMain.SampBarCode))
            {
                Thread t = new Thread(new ParameterizedThreadStart(InsertOrUpdateTATProRecord));
                t.Start(parames);
                result = true;
            }

            return result;
        }

        private void InsertOrUpdateTATProRecord(object obj)
        {
            string stepCode = "";
            string barCode = "";
            string dtToday = "";
            ThradParameters parmes = obj as ThradParameters;
            if (parmes != null)
            {
                stepCode = parmes.StepCode;
                barCode = parmes.BarCode;
                dtToday = parmes.DtToday.ToString("yyyy-MM-dd HH:mm:ss");

                try
                {

                    int ob = CountRecordByBarCode(barCode);

                    if (ob == 0)
                    {
                        InsertTATProRecord(stepCode, barCode, dtToday);
                    }
                    else
                    {
                        int overTime = JudgeOverTime(parmes);
                        //记录超时的时间
                        if (overTime > 0)
                            SaveOverTimeInfo(parmes, overTime);
                        UpdateTATProRecord(stepCode, barCode, dtToday);
                    }
                    if (listTatTime.Count > 0)
                    {
                        foreach (EntityDicCombineTimeRule rule in listTatTime)
                        {
                            DateTime mustTime = Convert.ToDateTime(dtToday).AddMinutes(Convert.ToDouble(rule.ComTime));
                            UpdateTATProRecord(rule.ComTimeEndType, barCode, mustTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
        }
        public int CountRecordByBarCode(string barCord)
        {
            int result = 0;
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                result = recordDao.CountRecordByBarCode(barCord);
            }
            return result;
        }

        public bool InsertTATProRecord(string stepCode, string barCode, string dtToday)
        {
            bool result = false;
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                result = recordDao.InsertTATProRecord(stepCode, barCode, dtToday);
            }
            return result;
        }

        public bool UpdateTATProRecord(string stepCode, string barCode, string dtToday)
        {
            bool result = false;
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                result = recordDao.UpdateTATProRecord(stepCode, barCode, dtToday);
            }
            return result;
        }
        public EntityTatProRecord GetTatRecord(string barCode)
        {
            EntityTatProRecord tat = new EntityTatProRecord();
            IDaoTatProRecord recordDao = DclDaoFactory.DaoHandler<IDaoTatProRecord>();
            if (recordDao != null)
            {
                tat = recordDao.GetTatRecord(barCode);
            }
            return tat;
        }

        public bool SaveOverTimeInfo(ThradParameters param, int overTime)
        {
            EntityTatOverTime EntityOverTime = new EntityTatOverTime();
            bool result = false;
            if (RuleIn.Count > 0)
            {
                RuleIn.GroupBy(x => x.ComTimeStartType.ToString() + "|" + x.ComTimeEndType.ToString()).Select(x => x.OrderBy(y => y.ComTime).Last()).ToList();
                EntityDicCombineTimeRule rule = RuleIn.Find(w => w.ComTimeEndType == param.StepCode && w.ComTimeType == timeType && w.ComTimeOriId == param.PidSrc);
                if (rule != null)
                {
                    EntityOverTime.TatBarCode = param.BarCode;
                    EntityOverTime.TatStartType = Convert.ToInt32(rule.ComTimeStartType);
                    EntityOverTime.TatEndType = Convert.ToInt32(rule.ComTimeEndType);
                    if (!string.IsNullOrEmpty(rule.ComReaTime))
                    {
                        EntityOverTime.TatTempReaTime = Convert.ToInt32(rule.ComReaTime);
                    }
                    EntityOverTime.TatTempTime = Convert.ToInt32(rule.ComTime);
                    EntityOverTime.TatTimeOver = overTime;
                    EntityOverTime.TatEndTypeTime = param.DtToday;
                    result= new TatOverTimeBIZ().SaveOverTime(EntityOverTime);
                }

            }
            return result;
        }
        /// <summary>
        /// 计算当前操作时间和设置时间差
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="stepCode"></param>
        /// <param name="operationTime"></param>
        /// <returns></returns>
        public int JudgeOverTime(ThradParameters param)
        {
            int overTime = 0;
            EntityTatProRecord tat = GetTatRecord(param.BarCode);
            string time = "";
            switch (param.StepCode)
            {
                case "2":
                    time = tat.TprBloodDate != null ? tat.TprBloodDate.ToString() : "";
                    break;
                case "3":
                    time = tat.TprSendDate != null ? tat.TprSendDate.ToString() : "";
                    break;
                case "4":
                    time = tat.TprReachDate != null ? tat.TprReachDate.ToString() : "";
                    break;
                case "5":
                    time = tat.TprReceiverDate != null ? tat.TprReceiverDate.ToString() : "";
                    break;
                case "9":
                    time = tat.TprReturnDate != null ? tat.TprReturnDate.ToString() : "";
                    break;
                case "20":
                    time = tat.TprJyDate != null ? tat.TprJyDate.ToString() : "";
                    break;
                case "40":
                    time = tat.TprCheckDate != null ? tat.TprCheckDate.ToString() : "";
                    break;
                case "60":
                    time = tat.TprReportDate != null ? tat.TprReportDate.ToString() : "";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(time))
            {
                DateTime dTime = Convert.ToDateTime(time);
                overTime = DateTime.Compare(param.DtToday, dTime);
            }
            return overTime;
        }
    }

    public class ThradParameters
    {
        public string StepCode;
        public string BarCode;
        public DateTime DtToday;
        public string PidSrc;
        public ThradParameters(EntitySampOperation operation, EntitySampMain sampMain)
        {
            this.StepCode = operation.OperationStatus;
            this.BarCode = sampMain.SampBarCode;
            this.DtToday = operation.OperationTime;
            this.PidSrc = sampMain.PidSrcId;
        }
    }
}
