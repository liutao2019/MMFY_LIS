using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using dcl.svr.dicbasic;
using dcl.svr.cache;
using dcl.common;
using dcl.svr.users;
using System.Collections;
using System.Data;
using System.Diagnostics;
using dcl.svr.sample;

namespace dcl.svr.result
{
    public class PatResultBIZ : IPatResult
    {
        public bool UpdatePatientResultByResKey(EntityLogLogin userInfo, EntityObrResult obrResult)
        {
            bool actionResult = false;
            if (obrResult != null)
            {
                try
                {
                    //修改结果类型和结果时间
                    DateTime time = ServerDateTime.GetDatabaseServerDateTime();
                    obrResult.ObrDate = time;
                    obrResult.ObrType = 0;

                    EntityResultQC resultQc = new EntityResultQC();
                    resultQc.ListObrId.Add(obrResult.ObrId);
                    resultQc.ItmId = obrResult.ItmId;

                    //未更新前的结果
                    EntityObrResult oldResult = null;
                    EntitySysOperationLog operationLog = null;
                    List<EntityObrResult> listObrResult = new ObrResultBIZ().ObrResultQuery(resultQc, false);
                    if (listObrResult != null && listObrResult.Count > 0)
                        oldResult = listObrResult.First();

                    #region 填充操作日志记录
                    if (oldResult != null)
                    {
                        //操作日志实体,用于插入一条记录
                        operationLog = new EntitySysOperationLog();
                        operationLog.OperatUserId = userInfo.LogLoginID.ToString();
                        operationLog.OperatDate = time;
                        operationLog.OperatKey = obrResult.ObrId;
                        operationLog.OperatServername = userInfo.LogIP;
                        operationLog.OperatModule = "病人资料";
                        operationLog.OperatGroup = "病人结果";
                        operationLog.OperatAction = "修改";
                        operationLog.OperatObject = oldResult.ItmEname;
                        operationLog.OperatContent = oldResult.ObrValue + "→" + obrResult.ObrValue;
                    }

                    #endregion

                    if (string.IsNullOrEmpty(obrResult.ObrItrId) || string.IsNullOrEmpty(obrResult.ObrSid))
                    {
                        EntityPidReportMain patient = new PidReportMainBIZ().GetPatientByPatId(obrResult.ObrId, false);
                        obrResult.ObrItrId = patient.RepItrId;
                        obrResult.ObrSid = patient.RepSid;
                    }

                    decimal devCastChr = 0;
                    if (decimal.TryParse(obrResult.ObrValue, out devCastChr))
                    {
                        obrResult.ObrConvertValue = devCastChr;
                    }
                    else
                    {
                        obrResult.ObrConvertValue = -1;
                    }
                    if (obrResult.ObrValue == string.Empty)
                    {
                        obrResult.ObrFlag = -1;
                    }

                    bool result = false;

                    //有主键ID就更新，否则插入
                    if (obrResult.ObrSn != 0)

                        result = new ObrResultBIZ().UpdateObrResult(obrResult);
                    else
                    {
                        result = new ObrResultBIZ().InsertObrResult(obrResult);
                    }

                    if (result && operationLog != null)
                    {
                        //插入日志信息
                        result = new SysOperationLogBIZ().SaveSysOperationLog(operationLog);
                    }
                    if (result)
                    {
                        actionResult = true;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

            }
            return actionResult;
        }

        public List<EntityItmRefInfo> GetItemRefInfo(List<string> itemsID, string sam_id, int age_minutes, string sex, string sam_rem, string itm_itr_id, string pat_depcode, string patDiag)
        {
            List<EntityItmRefInfo> listItmRefInfo = new List<EntityItmRefInfo>();
            listItmRefInfo = new ItemBIZ().GetItemRefInfo(itemsID, sam_id, age_minutes, sex, sam_rem, itm_itr_id, pat_depcode, patDiag);
            return listItmRefInfo;
        }

        public bool DeleteCommonResultItemByObrSn(EntityLogLogin logLogin, string obrSn, string repId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(obrSn))
            {
                EntityResultQC resultQc = new EntityResultQC();
                resultQc.ObrSn = obrSn;
                List<EntityObrResult> listResult = new ObrResultBIZ().ObrResultQuery(resultQc);
                DateTime time = ServerDateTime.GetDatabaseServerDateTime();

                #region 填充操作日志记录
                //操作日志实体,用于插入一条记录
                EntitySysOperationLog operationLog = new EntitySysOperationLog();
                operationLog.OperatUserId = logLogin.LogLoginID.ToString();
                operationLog.OperatDate = time;
                operationLog.OperatKey = repId;
                operationLog.OperatServername = logLogin.LogIP;
                operationLog.OperatModule = "病人资料";
                operationLog.OperatGroup = "病人结果";
                operationLog.OperatAction = "删除";

                #endregion

                if (listResult.Count > 0)
                {
                    EntityObrResult obrResult = listResult[0];
                    operationLog.OperatObject = obrResult.ItmEname;
                    operationLog.OperatContent = obrResult.ObrValue;
                }

                result = new ObrResultBIZ().DeleteObrResultByObrSn(obrSn);
                if (result)
                {
                    //插入日志信息
                    result = new SysOperationLogBIZ().SaveSysOperationLog(operationLog);
                }
            }

            return result;
        }

        public bool SaveColumnSort(string sort)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(sort))
            {
                SystemConfigBIZ configBiz = new SystemConfigBIZ();
                List<EntitySysParameter> listSysParam = configBiz.GetSysParaListByConfigCode("PatResultColumnSort");
                if (listSysParam.Count > 0)
                {
                    #region 更新系统配置
                    EntitySysParameter sysParam = listSysParam.First();
                    sysParam.ParmFieldValue = sort;

                    result = configBiz.UpdateSysPara(listSysParam);
                    #endregion
                }
                else
                {
                    #region 插入系统配置
                    EntitySysParameter sysParam = new EntitySysParameter();
                    sysParam.ParmCode = "PatResultColumnSort";
                    sysParam.ParmFieldName = "检验报告管理中结果列顺序";
                    sysParam.ParmGroup = "检验报告管理";
                    sysParam.ParmFieldType = "字符串";
                    sysParam.ParmFieldValue = sort;
                    sysParam.ParmModule = "system";
                    sysParam.ParmDictList = string.Empty;

                    result = configBiz.InsertSysPara(sysParam);
                    #endregion
                }
            }
            return result;
        }

        public List<EntityObrResult> GetPatCommonResultHistoryWithRef(string repId, int resultCount, DateTime? obrDate, bool containThisTime = false)
        {
            List<EntityObrResult> listHistoryResult = new List<EntityObrResult>();
            if (!string.IsNullOrEmpty(repId))
            {
                string strGetHistoryByCdr = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("CDR_GetHistory_Enable");
                if (strGetHistoryByCdr == "是")
                {
                    listHistoryResult = new ObrResultBIZ().GetCDRHistoryObrResult(repId, resultCount, obrDate, containThisTime);
                }
                else
                {
                    listHistoryResult = new ObrResultBIZ().GetHistoryObrResult(repId, resultCount, obrDate, containThisTime);
                }

            }
            return listHistoryResult;
        }

        public EntityQcResultList GetPatientCommonResult(string repId, bool withHistoryResult)
        {
            EntityQcResultList qcReuslt = new EntityQcResultList();
            List<EntityObrResult> listResult = new List<EntityObrResult>();
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Dictionary<string, string> dic = new Dictionary<string, string>();


                if (!string.IsNullOrEmpty(repId))
                {
                    #region 获取本次结果
                    EntityPidReportMain patient = new PidReportMainBIZ().GetPatientByPatId(repId, true);

                    EntityResultQC resultQc = new EntityResultQC();
                    resultQc.ListObrId.Add(repId);

                    List<EntityObrResult> listObrResult = new ObrResultBIZ().ObrResultQuery(resultQc, false);

                    //TODO 存在没人病人信息，有仪器结果的情况，，此处需要修改
                    if (patient != null)
                    {
                        if (patient.RepStatus == null)
                        {
                            patient.RepStatus = 0;
                        }
                        if (patient.RepStatus.Value == 0)
                        {
                            bool needReload = GenerateAutoCalItem(patient, listObrResult);

                            if (needReload)
                            {
                                listObrResult = new ObrResultBIZ().ObrResultQuery(resultQc, false);
                            }

                            dic = UpdateResultNotCombineItem(repId, listObrResult);
                        }


                        //过滤obrFlag=1
                        listResult = listObrResult.FindAll(i => i.ObrFlag == 1);
                        if (listResult.Count > 0)
                        {

                            List<string> itemsID = new List<string>();

                            #region //填充itemsID
                            //获取项目的list
                            foreach (EntityObrResult result in listResult)
                            {
                                if (!Compare.IsEmpty(result.ItmId))
                                {
                                    if (
                                        (Compare.IsEmpty(result.ItmComId) || result.ItmComId == "-1")
                                        &&
                                        dic.ContainsKey(result.ItmId)
                                        )
                                    {
                                        string comid = dic[result.ItmId];
                                        result.ItmComId = comid;


                                        //获取组合排序
                                        var combine = DictCombineCache.Current.GetCombineByID(comid, true);

                                        if (combine != null)
                                        {
                                            result.ResComSeq = combine.ComSortNo;
                                        }

                                        var combineMi = DictCombineMiCache2.Current.GetAll().Find(
                                            a => a.ComId == comid && a.ComItmId == result.ItmId);

                                        if (combineMi != null)
                                        {
                                                result.ComMiSort = combineMi.ComSortNo;

                                            if (combineMi.ComMustItem != null)
                                                result.IsNotEmpty = combineMi.ComMustItem.ToString();
                                        }
                                    }

                                    itemsID.Add(result.ItmId);
                                }
                            }
                            #endregion

                            if (dic.Keys.Count > 0)
                                listResult = listResult.OrderBy(i => i.ComMiSort).ToList();
                            string pat_depcode = string.Empty;
                            if (patient != null && !Compare.IsNullOrDBNull(patient.PidDeptId))
                            {
                                pat_depcode = patient.PidDeptId;
                            }
                            //获取项目参考值，危急值等信息
                            List<EntityItmRefInfo> listRefInfo = GetItemRefInfo(itemsID, patient.PidSamId, GetConfigAge(patient.PidAge),
                                GetConfigSex(patient.PidSex), patient.SampRemark, patient.RepItrId, pat_depcode, "");

                            if (patient != null && patient.RepStatus == 0)
                            {


                                if (listRefInfo.Count > 0)
                                {
                                    foreach (EntityObrResult result in listResult)
                                    {
                                        if (!Compare.IsEmpty(result.ItmId))
                                        {
                                            EntityItmRefInfo itmRefInfo = listRefInfo.Find(i => i.ItmId == result.ItmId);
                                            if (itmRefInfo != null)
                                            {
                                                if (patient.RepStatus != null && (patient.RepStatus.Value == 2 || patient.RepStatus.Value == 4))
                                                {
                                                    //如果pat_flag等于2时,为二审结果,则参考值取结果表数据
                                                    //如果pat_flag等于4时,为已打印报告,则参考值取结果表数据
                                                }
                                                else
                                                {
                                                    result.RefLowerLimit = itmRefInfo.ItmLowerLimitValue;
                                                    result.RefUpperLimit = itmRefInfo.ItmUpperLimitValue;
                                                }

                                                result.ResPanL = itmRefInfo.ItmDangerLowerLimit;
                                                result.ResPanL = itmRefInfo.ItmDangerUpperLimit;

                                                result.ResMin = itmRefInfo.ItmMinValue;
                                                result.ResMax = itmRefInfo.ItmMaxValue;

                                                //允许出现的结果
                                                result.ResAllowValues = itmRefInfo.ItmResultAllow;

                                                //阳性提示结果
                                                result.ResPositiveResult = itmRefInfo.ItmPositiveRes;

                                                //自定义危急值
                                                result.ResCustomCriticalResult = itmRefInfo.ItmUrgentRes;

                                                if (patient.RepStatus != null && (patient.RepStatus.Value == 2 || patient.RepStatus.Value == 4))
                                                {
                                                    //如果pat_flag等于2时,为二审结果,则参考值取结果表数据
                                                    //如果pat_flag等于4时,为已打印报告,则参考值取结果表数据
                                                }
                                                else
                                                {
                                                    result.ResRefLCal = itmRefInfo.ItmLowerLimitValue;
                                                    result.ResRefHCal = itmRefInfo.ItmUpperLimitValue;
                                                }

                                                if (
                                                   !string.IsNullOrEmpty(result.ResRefLCal.Trim())
                                                   && !string.IsNullOrEmpty(result.ResRefHCal.Trim()))
                                                {
                                                    result.ResRefRange = result.ResRefLCal + " - " + result.ResRefHCal;
                                                }
                                                else
                                                {
                                                    result.ResRefRange = result.ResRefLCal + result.ResRefHCal;
                                                }

                                                result.ResPanLCal = itmRefInfo.ItmDangerLowerLimit;
                                                result.ResPanHCal = itmRefInfo.ItmDangerUpperLimit;

                                                result.ResMinCal = itmRefInfo.ItmMinValue;
                                                result.ResMaxCal = itmRefInfo.ItmMaxValue;

                                                result.ObrItmMethod = itmRefInfo.ItmMethod;
                                                result.ItmDtype = itmRefInfo.ItmResType;
                                                result.ItmMaxDigit = itmRefInfo.ItmMaxDigit;
                                                result.ObrUnit = itmRefInfo.ItmUnit;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (EntityObrResult result in listResult)
                                {
                                    EntityItmRefInfo itmRefInfo = listRefInfo.Find(i => i.ItmId == result.ItmId);
                                    if (itmRefInfo != null)
                                    {
                                        result.ResPanL = itmRefInfo.ItmDangerLowerLimit;
                                        result.ResPanL = itmRefInfo.ItmDangerUpperLimit;
                                        result.ResPanLCal = itmRefInfo.ItmDangerLowerLimit;
                                        result.ResPanHCal = itmRefInfo.ItmDangerUpperLimit;
                                        result.ResRefLCal = result.RefLowerLimit;
                                        result.ResRefHCal = result.RefUpperLimit;
                                        result.ObrUnit = result.ObrUnit;
                                        //允许出现的结果
                                        result.ResAllowValues = itmRefInfo.ItmResultAllow;
                                        //阳性提示结果
                                        result.ResPositiveResult = itmRefInfo.ItmPositiveRes;
                                        //自定义危急值
                                        result.ResCustomCriticalResult = itmRefInfo.ItmUrgentRes;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 格式化阈值
                        foreach (EntityObrResult entityResult in listResult)
                        {
                            //在获取参考值,危急值时已经格式化
                        }
                        #endregion

                        #region 获取历史结果
                        if (withHistoryResult && listResult.Count > 0 && patient != null)
                        {
                            //查询前三历史结果
                            List<EntityObrResult> listHistoryResult = GetPatCommonResultHistoryWithRef(repId, 3, patient.RepInDate);

                            //获取本次检验的所有项目ID,用于过滤历史结果的项目
                            string[] itemIds = listResult.Select(i => i.ItmId).ToArray();

                            //过滤后的历史结果
                            listHistoryResult = (from x in listHistoryResult where itemIds.Contains(x.ItmId) select x).ToList();

                            #region 给本次检验的历史结果赋值
                            try
                            {
                                if (listHistoryResult.Count > 0)
                                {
                                    for (int j = 0; j < listHistoryResult.Count; j++)
                                    {
                                        EntityObrResult historyResult = listHistoryResult[0];
                                        foreach (EntityObrResult result in listResult)
                                        {
                                            if (!string.IsNullOrEmpty(result.ItmId))
                                            {
                                                EntityObrResult usefulResult = listHistoryResult.Find(i => i.ItmId == result.ItmId && i.ObrId == historyResult.ObrId);
                                                if (usefulResult != null)
                                                {
                                                    if (j == 0)
                                                    {
                                                        result.HistoryResult1 = usefulResult.ObrValue;
                                                        result.HistoryDate1 = usefulResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                        listResult[0].HistoryDate1 = usefulResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                    }
                                                    if (j == 1)
                                                    {
                                                        result.HistoryResult2 = usefulResult.ObrValue;
                                                        result.HistoryDate2 = usefulResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                        listResult[0].HistoryDate2 = usefulResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                    }
                                                    if (j == 2)
                                                    {
                                                        result.HistoryResult3 = usefulResult.ObrValue;
                                                        result.HistoryDate3 = usefulResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                        listResult[0].HistoryDate3 = usefulResult.ObrDate.ToString("yyyy-MM-dd HH:mm:ss");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Lib.LogManager.Logger.LogException("获取历史结果出错", ex);
                            }
                            #endregion

                        }
                    }
                    #endregion

                    qcReuslt.patient = patient;
                    qcReuslt.listResulto = listResult;
                }
                sw.Stop();
                // Lib.LogManager.Logger.LogInfo(string.Format("数据库:GetPatientResult,获取病人结果表,耗时 {0}ms", sw.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取病人结果出错,patID=" + repId, ex);
            }



            return qcReuslt;
        }

        /// <summary>
        /// 生成自动关联计算项目
        /// </summary>
        /// <param name="pat_id"></param>
        public bool GenerateAutoCalItem(EntityPidReportMain entityPatient, List<EntityObrResult> listResult)
        {

            bool hasUpdate = false;
            //系统配置：关闭自动关联计算项目的功能
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_stopCalItem") == "是")
            {
                return hasUpdate;//不自动关联计算项目
            }
            List<EntityObrResult> listObrResult = listResult.FindAll(i => i.ObrFlag == 1);
            if (listObrResult == null || listObrResult.Count == 0) return hasUpdate;
            //生成关联计算参数表
            Hashtable ht = new Hashtable();
            foreach (EntityObrResult drSource in listObrResult)
            {
                if (!string.IsNullOrEmpty(drSource.ObrValue) && drSource.ObrValue.Trim(null) != string.Empty)
                {
                    string item_ecd = drSource.ItmEname;

                    if (!ht.Contains(item_ecd))
                    {
                        ht.Add(item_ecd, drSource.ObrValue);
                    }
                }
            }


            List<EntityDicItmCalu> dtCalItem = DictClItemCache.Current.GetAllCalu();

            DataSet dsResult = Variable(ht, dtCalItem, entityPatient);
            DataTable dtCalResult = dsResult.Tables[0];

            if (dtCalResult.Rows.Count > 0)
            {
                DateTime now = ServerDateTime.GetDatabaseServerDateTime();

                string[] listDetail = new PidReportDetailBIZ().GetPidReportDetailByRepId(entityPatient.RepId).Select(i => i.ComId).ToArray();
                List<EntityDicCombineDetail> dtComItem = (from x in DictCombineMiCache2.Current.DclCache where listDetail.Contains(x.ComId) select x).ToList();

                List<EntityObrResult> listInsert = new List<EntityObrResult>();

                foreach (DataRow result in dtCalResult.Rows)
                {

                    string itm_id = result["cal_item_ecd"].ToString();
                    string itm_ecd = result["itm_ecd"].ToString();
                    string value = result["retu"].ToString();


                    string valueINItmProp = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        decimal dec = 0;

                        if (decimal.TryParse(value, out dec))
                        {
                            dec = decimal.Round(dec, 2);

                            valueINItmProp = dec.ToString();
                        }
                    }
                    string strProp = dcl.svr.cache.DictItemPropCache.Current.GetItmProp(itm_id, valueINItmProp);

                    value = strProp == string.Empty ? value : strProp;

                    List<EntityObrResult> existItems = listObrResult.FindAll(i => i.ItmId == itm_id);

                    List<EntityDicCombineDetail> drComItems = dtComItem.FindAll(i => i.ComItmId == itm_id);

                    if (drComItems.Count > 0 || dtComItem.Count == 0)
                    {
                        //项目不存在：添加
                        if (existItems.Count == 0 && listInsert.FindAll(i => i.ItmId == itm_id).Count == 0)
                        {
                            EntityObrResult obrResult = new EntityObrResult();
                            obrResult.ObrId = entityPatient.RepId;
                            obrResult.ItmId = itm_id;
                            obrResult.ItmEname = itm_ecd;
                            obrResult.ObrFlag = 1;
                            obrResult.ObrItrId = entityPatient.RepItrId;
                            obrResult.ObrSid = entityPatient.RepSid;
                            obrResult.ObrDate = now;
                            obrResult.ObrValue = value;
                            obrResult.ObrType = 2;
                            obrResult.ItmReportCode = itm_ecd;

                            listInsert.Add(obrResult);

                            new ObrResultBIZ().InsertObrResult(obrResult);
                            hasUpdate = true;
                        }
                        //存在：更新结果
                        else
                        {

                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AllowEditCalItem") == "是" && string.IsNullOrEmpty(existItems[0].ObrValue.Trim()))
                                continue;
                            EntityObrResult obrResult = new EntityObrResult();
                            obrResult.ObrSn = existItems[0].ObrSn;
                            obrResult.ObrValue = value;
                            obrResult.ObrConvertValue = null;
                            if (!string.IsNullOrEmpty(value))
                            {
                                decimal decValue = 0;
                                if (decimal.TryParse(value, out decValue))
                                {
                                    obrResult.ObrConvertValue = decValue;
                                }
                            }
                            //hasUpdate = true;
                            new ObrResultBIZ().UpdateResultValueByObrSn(obrResult);

                        }
                    }
                }
            }
            return hasUpdate;
        }

        //公用效验方法
        private DataSet Variable(Hashtable ht, List<EntityDicItmCalu> dtCalItem, EntityPidReportMain entityPatient)
        {
            string Pat_itr_id = entityPatient.RepItrId;
            List<EntityDicItmCalu> dci = dtCalItem;
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd");//存ID
            pb.Columns.Add("itm_ecd");//存ECD
            pb.Columns.Add("cal_sp_formula");
            pb.Columns.Add("retu");

            List<string> fmla = new List<string>();
            foreach (EntityDicItmCalu dr in dci)
            {
                string cal_item_ecd = string.Empty;
                if (!string.IsNullOrEmpty(dr.ItmId))
                {
                    cal_item_ecd = dr.ItmId;
                }
                if (!string.IsNullOrEmpty(dr.CalItrId) && !string.IsNullOrEmpty(Pat_itr_id)
                  && dr.CalItrId != Pat_itr_id)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(dr.CalExpression) &&
                   fmla.Contains(dr.CalExpression + cal_item_ecd))
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(dr.CalExpression))
                {
                    fmla.Add(dr.CalExpression + cal_item_ecd);
                }
                if (!string.IsNullOrEmpty(dr.CalVariable))
                {
                    string[] varpr = dr.CalVariable.Split(',');
                    int count = 0;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        for (int j = 0; j < varpr.Length; j++)
                        {
                            if (varpr[j].ToString() == parm[i].ToString())
                                count++;
                        }
                    }
                    if (count == varpr.Length && count > 0)
                    {
                        pb.Rows.Add(dr.CalExpression, dr.CalFlag, dr.ItmId, dr.ItmEcode, dr.CalSpFormula);
                    }
                }
            }

            for (int i = 0; i < pb.Rows.Count; i++)
            {
                string methAll = pb.Rows[i]["cal_fmla"].ToString();
                string itmID = pb.Rows[i]["cal_item_ecd"].ToString();

                if (pb.Rows[i]["cal_sp_formula"] != null &&
                 !string.IsNullOrEmpty(pb.Rows[i]["cal_sp_formula"].ToString()))
                {
                    pb.Rows[i]["retu"] = new DclCalcItemResHelper().GetCalcRes(pb.Rows[i]["cal_sp_formula"].ToString(), ht, entityPatient);
                    continue;
                }

                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";

                    double dValue = 0;
                    try
                    {
                        if (!double.TryParse(value[j], out dValue))
                        {
                            for (int n = 0; n < value[j].Length; n++)
                            {

                                if (double.TryParse(value[j].Substring(n, 1), out dValue))
                                {
                                    value[j] = value[j].Substring(n);
                                    break;

                                }

                            }

                        }
                    }
                    catch
                    { }

                    double.TryParse(value[j], out dValue);

                    string va = dValue.ToString("0.0000");

                    methAll = methAll.Replace(fam, va);
                }

                DataTable dt = new DataTable();
                try
                {
                    object objValue = dt.Compute(methAll, string.Empty);

                    decimal decVal = 0;

                    if (decimal.TryParse(objValue.ToString(), out decVal))
                    {
                        //decVal = decimal.Round(decVal, 4);
                        //pb.Rows[i]["retu"] = decVal.ToString();
                        int? itm_max_digit = null;
                        EntityDicItemSample itemSam = dcl.svr.cache.DictItemSamCache.Current.DclCache.Find(k => k.ItmId == itmID && k.ItmSamId == entityPatient.PidSamId);
                        if (itemSam != null)
                        {
                            itm_max_digit = itemSam.ItmMaxDigit;
                        }
                        if (itm_max_digit == null || itm_max_digit < 0)
                        {
                            decVal = decimal.Round(decVal, 4);
                            pb.Rows[i]["retu"] = decVal.ToString("0.00");
                        }
                        else
                        {
                            decVal = decimal.Round(decVal, itm_max_digit.Value);

                            if (itm_max_digit == 0)
                            {
                                pb.Rows[i]["retu"] = decVal.ToString();
                            }
                            else
                            {

                                pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                            }


                        }
                    }

                    //pb.Rows[i]["retu"] = .ToString();
                }
                catch
                {

                    //2013年2月28日16:45:32 叶
                    //当使用DataTable.Compute无法计算表达式的值时,比如带Math.Log()的表达式
                    //用动态编译后进行计算 
                    try
                    {
                        //2013年5月14日14:20:41 叶
                        if (methAll.Contains("[标本]"))
                        {

                            methAll = methAll.Replace("[标本]", string.Format("\"{0}\"", entityPatient.PidSamId));

                        }
                        if (methAll.Contains("[标本备注]"))
                        {
                            methAll = methAll.Replace("[标本备注]", string.Format("\"{0}\"", entityPatient.SampRemark));

                        }
                        object objValue = ExpressionCompute.CalExpression(methAll);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {
                                //decVal = decimal.Round(decVal, 4);
                                //pb.Rows[i]["retu"] = decVal.ToString();
                                int? itm_max_digit = null;
                                EntityDicItemSample itemSam = dcl.svr.cache.DictItemSamCache.Current.DclCache.Find(k => k.ItmId == itmID && k.ItmSamId == entityPatient.PidSamId);
                                if (itemSam != null)
                                {
                                    itm_max_digit = itemSam.ItmMaxDigit;
                                }
                                if (itm_max_digit == null || itm_max_digit < 0)
                                {
                                    decVal = decimal.Round(decVal, 4);
                                    pb.Rows[i]["retu"] = decVal.ToString("0.00");
                                }
                                else
                                {
                                    decVal = decimal.Round(decVal, itm_max_digit.Value);
                                    if (itm_max_digit == 0)
                                    {
                                        pb.Rows[i]["retu"] = decVal.ToString();
                                    }
                                    else
                                    {

                                        pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                                    }

                                }
                            }
                        }
                        else
                        {


                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }

        /// <summary>
        /// 更新所有没有组合id的项目
        /// </summary>
        /// <param name="pat_id"></param>
        public Dictionary<string, string> UpdateResultNotCombineItem(string pat_id, List<EntityObrResult> listResult)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            List<EntityObrResult> listNullComIDItems = listResult.FindAll(i => (string.IsNullOrEmpty(i.ItmComId) || i.ItmComId == "-1")
                                                                                && !string.IsNullOrEmpty(i.ItmId)).ToList();


            if (listNullComIDItems.Count > 0)
            {
                string[] listPatCom = new PidReportDetailBIZ().GetPidReportDetailByRepId(pat_id).Select(i => i.ComId).ToArray();
                if (listPatCom.Length > 0)
                {
                    List<EntityDicCombineDetail> listComItem = (from x in DictCombineMiCache2.Current.DclCache where listPatCom.Contains(x.ComId) select x).ToList();

                    if (listComItem.Count > 0)
                    {
                        List<EntityObrResult> listUpdate = new List<EntityObrResult>();

                        foreach (EntityObrResult rowNullComIDItem in listNullComIDItems)
                        {
                            string itm_id = rowNullComIDItem.ItmId;

                            EntityDicCombineDetail ComMi = listComItem.Find(i => i.ComItmId == itm_id);

                            if (ComMi != null)
                            {

                                if (!dic.ContainsKey(itm_id)
                                   && !string.IsNullOrEmpty(ComMi.ComId))
                                {
                                    dic.Add(itm_id, ComMi.ComId);
                                }
                                EntityObrResult updateResult = new EntityObrResult();
                                updateResult.ItmId = itm_id;
                                updateResult.ItmComId = ComMi.ComId;
                                updateResult.ObrId = rowNullComIDItem.ObrId;
                                listUpdate.Add(updateResult);
                            }
                        }

                        System.Threading.Thread tempThr =
                            new System.Threading.Thread(ThreadUpdateResultNotCombineItem);
                        tempThr.Start(listUpdate);

                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 多线程更新所有没有组合id的项目
        /// </summary>
        /// <param name="ltcmd"></param>
        public void ThreadUpdateResultNotCombineItem(object ltcmd)
        {
            try
            {
                if (ltcmd != null && ltcmd is List<EntityObrResult>)
                {
                    List<EntityObrResult> listResult = (List<EntityObrResult>)ltcmd;
                    ObrResultBIZ reusltBiz = new ObrResultBIZ();
                    foreach (EntityObrResult result in listResult)
                    {
                        reusltBiz.UpdateResultComIdByObrIdAndItmID(result.ItmComId, result.ObrId, result.ItmId);
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("多线程更新没有组合id项目结果", ex);
            }
        }

        /// <summary>
        /// 根据配置转换获取年龄分钟(当年龄为空值时)
        /// </summary>
        /// <param name="ageInput">输入(分钟)</param>
        /// <returns></returns>
        public static int GetConfigAge(object ageMinuteInput)
        {
            if (!Compare.IsEmpty(ageMinuteInput))
            {
                int ret = -1;

                if (int.TryParse(ageMinuteInput.ToString(), out ret))
                {
                    if (ret >= 0)
                    {
                        return ret;
                    }
                }
                else
                {
                }
            }
            else
            {
            }

            string configCalAge = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetRefOnNullAge");

            int calage = -1;

            if (!string.IsNullOrEmpty(configCalAge)
                && configCalAge != "不计算参考值")
            {
                calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
            }
            return calage;
        }

        public static string GetConfigSex(object sexInput)
        {
            string ret = string.Empty;
            if (Compare.IsEmpty(sexInput) || (sexInput.ToString() != "1" && sexInput.ToString() != "2" && sexInput.ToString() != "0"))//年龄为空
            {
                string configCalSex = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("GetRefOnNullSex");

                if (configCalSex == "不计算参考值")
                {
                    ret = "9";
                }
                else if (configCalSex == "默认")
                {
                    ret = "0";
                }
                else if (configCalSex == "男")
                {
                    ret = "1";
                }
                else if (configCalSex == "女")
                {
                    ret = "2";
                }
            }
            else
            {
                ret = sexInput.ToString();
            }

            return ret;
        }

        public List<EntitySampMergeRule> GetRuleByHisCode(List<string> listHisFeeCode, string strOriId)
        {
            return new ItemCombineBarcodeBIZ().GetSampMergeRuleByHisCode(listHisFeeCode, strOriId);
        }

        public bool SaveSampProcessDetail(EntitySampProcessDetail detail)
        {
            return new SampProcessDetailBIZ().SaveSampProcessDetailWithoutInterface(detail);
        }

        public EntityResponse SearchItmCheckAndDetail(string itmId)
        {
            EntityResponse response = new EntityResponse();
            if (!string.IsNullOrEmpty(itmId))
            {
                EntityRequest request = new EntityRequest();
                request.SetRequestValue(itmId);

                response = new EfficacyBIZ().Search(request);
            }
            return response;
        }
    }
}
