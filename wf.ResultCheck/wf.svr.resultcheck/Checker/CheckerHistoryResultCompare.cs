using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using dcl.svr.cache;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;


namespace dcl.svr.resultcheck.Checker
{
    /// <summary>
    /// 历史结果对比
    /// </summary>
    public class CheckerHistoryResultCompare : AbstractAuditClass, IAuditChecker
    {
        public CheckerHistoryResultCompare(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config)
            : base(pat_info, null, resulto, auditType, config)
        { }

        public CheckerHistoryResultCompare(EntityPidReportMain pat_info, List<EntityObrResult> resulto, EnumOperationCode auditType, AuditConfig config, EntityRemoteCallClientInfo p_Caller)
            : base(pat_info, null, resulto, auditType, config)
        {
            Caller = p_Caller;
        }

        #region IAuditChecker 成员

        public void Check(ref EntityOperationResult chkResult)
        {
            if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report || this.auditType == EnumOperationCode.PreAudit)
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();

                //记录要对比项目结果的项目
                List<EntityObrResult> noteSameItemResultList = new List<EntityObrResult>();
                foreach (EntityObrResult resCurrent in this.resulto)//遍历当前检验结果
                {
                    //同一病人不同报告的相同项目结果对比的项目ID
                    if (config.strSameItemResultContrastId.Contains(resCurrent.ItmId))
                    {
                        noteSameItemResultList.Add(resCurrent);
                    }
                }

                #region 双盲法AB仪器id & C仪器id

                string DoubleBlindItrA = "";//双盲法A仪器id--目前滨海使用
                string DoubleBlindItrB = "";//双盲法B仪器id--目前滨海使用
                string DoubleBlindItrC = "";//双盲法C仪器id--目前滨海使用

                //系统配置：审核---双盲法AB仪器ID(A仪器id,B仪器id)
                if (!string.IsNullOrEmpty(CacheSysConfig.Current.GetSystemConfig("DoubleBlind_AorB_ItrID")))
                {
                    string DoubleBlind_AorB_ItrID = CacheSysConfig.Current.GetSystemConfig("DoubleBlind_AorB_ItrID");
                    if (!string.IsNullOrEmpty(DoubleBlind_AorB_ItrID))
                    {
                        string[] DoubleBlind_AorB_IDs = DoubleBlind_AorB_ItrID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (DoubleBlind_AorB_IDs.Length > 1)
                        {
                            DoubleBlindItrA = DoubleBlind_AorB_IDs[0];
                            DoubleBlindItrB = DoubleBlind_AorB_IDs[1];
                        }
                    }
                }
                //系统配置：审核---双盲法C仪器ID(C仪器id)
                if (!string.IsNullOrEmpty(CacheSysConfig.Current.GetSystemConfig("DoubleBlind_C_ItrID")))
                {
                    DoubleBlindItrC = CacheSysConfig.Current.GetSystemConfig("DoubleBlind_C_ItrID");
                }

                #endregion
                EntityResultQC historyQc = new EntityResultQC();
                historyQc.ObrFlag = "1";
                historyQc.RepId = pat_info.RepId;
                historyQc.PidSamId = pat_info.PidSamId;
                historyQc.StartRepInDate = "2011-03-15 00:00:00";
                List<EntityObrResult> listHistoryRes = null; //历史结果
                #region 根据AB仪器一审与二审的不同过滤
                if (!string.IsNullOrEmpty(DoubleBlindItrA) && !string.IsNullOrEmpty(DoubleBlindItrB)
                    && (pat_info.RepItrId == DoubleBlindItrA || pat_info.RepItrId == DoubleBlindItrB || pat_info.RepItrId == DoubleBlindItrC))
                {
                    if (auditType == EnumOperationCode.Audit || auditType == EnumOperationCode.Report)
                    {
                        if (pat_info.RepItrId == DoubleBlindItrC)
                        {
                            //一二审时,获取已一审的历史结果,非相同条码
                            if (!string.IsNullOrEmpty(pat_info.RepBarCode))
                            {
                                historyQc.RepBarCode = pat_info.RepBarCode;
                            }
                        }
                        else
                        {
                            historyQc.OnlyReport = true;
                        }
                    }
                }
                #endregion
                IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
                if (resultDao != null)
                {
                    //获取历史结果
                    if (config.bHistoryResultOnlySelectWithName)//如果病人历史结果只按姓名查询
                    {
                        historyQc.PidName = pat_info.PidName;
                        historyQc.PidSex = pat_info.PidSex;
                        historyQc.EndRepInDate = DateTime.Now.ToString();
                        listHistoryRes = resultDao.ObrResultQuery(historyQc);
                    }
                    else
                    {
                        historyQc.PidInNo = pat_info.PidInNo;
                        historyQc.PidName = pat_info.PidName;
                        historyQc.PidSex = pat_info.PidSex;
                        historyQc.EndRepInDate = DateTime.Now.ToString();
                        listHistoryRes = resultDao.ObrResultQuery(historyQc);
                    }
                }
                if (listHistoryRes != null && listHistoryRes.Count > 0)
                {
                    //排序 order by res_date desc
                    listHistoryRes = listHistoryRes.OrderByDescending(w => w.ObrDate).ToList();
                }

                if (listHistoryRes != null && listHistoryRes.Count > 0 && (pat_info.RepRemark == null || !pat_info.RepRemark.Contains("与历史结果比较偏差较大，已复查，请结合临床综合分析。")))
                {
                    #region 有历史结果,但未匹配

                    //获取历史结果
                    EnumOperationErrorLevel lvError = EnumOperationErrorLevel.Error;
                    try
                    {
                        if (Convert.ToBoolean(Caller.OperationName))
                        {
                            lvError = EnumOperationErrorLevel.Message;
                        }
                    }
                    catch
                    {

                    }
                    foreach (EntityObrResult resCurrent in this.resulto)//遍历当前检验结果
                    {
                        #region 同一病人两个相同项目一起做判断结果是否相同，不相同则不能审核
                        if (config.strSameItemResultContrastId.Contains(resCurrent.ItmId))
                        {
                            List<EntityObrResult> listResing = listHistoryRes.FindAll(w => w.ObrItrId == resCurrent.ItmId);
                            if (listResing.Count > 0)
                            {
                                //如果当前为C仪器,并且有历史结果,则一审时不判断
                                if (!string.IsNullOrEmpty(DoubleBlindItrC) && pat_info.RepItrId == DoubleBlindItrC
                                    && auditType == EnumOperationCode.Audit)
                                {
                                    return;
                                }

                                //if (pat_info.RepCheckUserId == rowsResing[0]["pat_i_code"].ToString())
                                //{
                                //    chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}两次报告的检验者不能为同一人", resCurrent.ItmEname), lvError);
                                //    return;
                                //}
                                string h_res_chr = listResing[0].ObrValue;

                                decimal decCurrResChr;
                                decimal decHistoryResChr;
                                if (decimal.TryParse(h_res_chr, out decHistoryResChr)
                                    && decimal.TryParse(resCurrent.ObrValue, out decCurrResChr))
                                {


                                    if (decCurrResChr != decHistoryResChr)
                                    {
                                        chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}两次结果不一致", resCurrent.ItmEname), lvError);
                                    }
                                }
                                else//如果历史结果或当前结果不为数字
                                {

                                    int h_res_flag = -1;//0=A型 1=B型 2=O型  3=AB型
                                    int curr_res_flag = -1;
                                    //只判断不允许有误差的情况(A型、B型、O型、AB型)
                                    //只判断历史结果与当前结果的血型差异
                                    if (h_res_chr.Contains("阴性"))
                                    {
                                        h_res_flag = 0;
                                    }
                                    else if (h_res_chr.Contains("弱阳性"))
                                    {
                                        h_res_flag = 1;
                                    }
                                    else if (h_res_chr.Contains("阳性") || h_res_chr.StartsWith("+") || h_res_chr.EndsWith("+"))
                                    {
                                        h_res_flag = 2;
                                    }

                                    if (resCurrent.ObrValue.Contains("阴性"))
                                    {
                                        curr_res_flag = 0;
                                    }
                                    else if (resCurrent.ObrValue.Contains("弱阳性"))
                                    {
                                        curr_res_flag = 1;
                                    }
                                    else if (resCurrent.ObrValue.Contains("阳性") || resCurrent.ObrValue.StartsWith("+") || resCurrent.ObrValue.EndsWith("+"))
                                    {
                                        curr_res_flag = 2;
                                    }


                                    if (h_res_flag == -1 || curr_res_flag == -1)
                                    {
                                        if (h_res_chr.Contains("A") && !h_res_chr.Contains("AB"))
                                        {
                                            h_res_flag = 0;
                                        }
                                        else if (h_res_chr.Contains("B") && !h_res_chr.Contains("AB"))
                                        {
                                            h_res_flag = 1;
                                        }
                                        else if (h_res_chr.Contains("O"))
                                        {
                                            h_res_flag = 2;
                                        }
                                        else if (h_res_chr.Contains("AB"))
                                        {
                                            h_res_flag = 3;
                                        }

                                        if (resCurrent.ObrValue.Contains("A") && !resCurrent.ObrValue.Contains("AB"))
                                        {
                                            curr_res_flag = 0;
                                        }
                                        else if (resCurrent.ObrValue.Contains("B") && !resCurrent.ObrValue.Contains("AB"))
                                        {
                                            curr_res_flag = 1;
                                        }
                                        else if (resCurrent.ObrValue.Contains("O"))
                                        {
                                            curr_res_flag = 2;
                                        }
                                        else if (resCurrent.ObrValue.Contains("AB"))
                                        {
                                            curr_res_flag = 3;
                                        }
                                    }



                                    if (h_res_flag != curr_res_flag)
                                    {
                                        //chkResult.AddMessage(EnumOperationErrorCode.ResultOverDiff
                                        //         , string.Format("{0} {1}({2}%) ", resCurrent.ItmEname, h_res_chr, ettItemSam.ItmDiff)
                                        //         , EnumOperationErrorLevel.Error);
                                        chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}两次结果不一致", resCurrent.ItmEname), lvError);
                                    }

                                }
                            }
                            else
                            {
                                //chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}只检验了一份报告", resCurrent.ItmEname), lvError);
                                #region 没有历史结果
                                EntityPatientQC patientQc = new EntityPatientQC();
                                IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                                if (!string.IsNullOrEmpty(DoubleBlindItrA) && !string.IsNullOrEmpty(DoubleBlindItrB)
                                    && (pat_info.RepItrId == DoubleBlindItrA || pat_info.RepItrId == DoubleBlindItrB))
                                {
                                    //没有历史结果时,B仪器一审时,两个的技师是否一致
                                    if (auditType == EnumOperationCode.Audit && (pat_info.RepItrId == DoubleBlindItrB || pat_info.RepItrId == DoubleBlindItrA))
                                    {
                                        //检查：同一条码的两份报告的检验者是否一致
                                        #region 检查：同一条码的两份报告的检验者是否一致

                                        if (!string.IsNullOrEmpty(pat_info.RepBarCode))
                                        {
                                            List<EntityPidReportMain> listSameBcPat = new List<EntityPidReportMain>();
                                            if (mainDao != null)
                                            {
                                                listSameBcPat = mainDao.PatientQuery(patientQc);
                                            }
                                            if (listSameBcPat != null && listSameBcPat.Count > 0)
                                            {

                                                //同一条码的两份报告的检验者是否一致，相同则提示不能审核(用录入者对比)
                                                //if (pat_info.RepCheckUserId == dtSameBcPat.Rows[0]["pat_i_code"].ToString())
                                                //{
                                                //    chkResult.AddCustomMessage("", "", string.Format("此条码正在检验的{0}两次报告的检验者不能为同一人！", resCurrent.ItmEname), lvError);
                                                //    return;
                                                //}

                                                //同一条码的两份报告的检验者是否一致，相同则提示不能审核(用一审者对比)
                                                if (!string.IsNullOrEmpty(listSameBcPat[0].RepAuditUserId.ToString()) && Caller != null && Caller.LoginID == listSameBcPat[0].RepAuditUserId)
                                                {
                                                    chkResult.AddCustomMessage("", "", string.Format("此条码正在检验的{0}两次报告的检验者不能为同一人！", resCurrent.ItmEname), lvError);
                                                    return;
                                                }
                                            }

                                        }

                                        #endregion
                                    }

                                    //没有历史结果时,A仪器二审时时要判断二者的结果是否都一审并一致;如果B没有一审,则要提示
                                    if (auditType == EnumOperationCode.Report && pat_info.RepItrId == DoubleBlindItrA)
                                    {
                                        //检查：同一条码的两份报告的结果是否一致
                                        #region 检查：同一条码的两份报告的结果是否一致

                                        if (!string.IsNullOrEmpty(pat_info.RepBarCode))
                                        {
                                            List<EntityObrResult> listSameBcPat = new List<EntityObrResult>();
                                            if (resultDao != null)
                                                listSameBcPat = resultDao.GetResultInfoByBarcode(pat_info.RepBarCode, pat_info.RepId);

                                            if (listSameBcPat != null && listSameBcPat.Count > 0)
                                            {
                                                //DataRow[] drListTempSel = dtSameBcPat.Select("res_itm_id='" + resCurrent.ItmEname + "'");
                                                List<EntityObrResult> listTempSel = listSameBcPat.FindAll(w => w.ItmId == resCurrent.ItmEname);
                                                //结果是否都一审并一致
                                                if (listTempSel != null && listTempSel.Count > 0)
                                                {
                                                    if (listTempSel[0].RepStatus.ToString() == "1"
                                                        || listTempSel[0].RepStatus.ToString() == "2"
                                                        || listTempSel[0].RepStatus.ToString() == "4")
                                                    {
                                                        #region 对比结果是否一样
                                                        string h_res_chr = listTempSel[0].ObrValue.ToString();

                                                        decimal decCurrResChr;
                                                        decimal decHistoryResChr;
                                                        if (decimal.TryParse(h_res_chr, out decHistoryResChr)
                                                            && decimal.TryParse(resCurrent.ObrValue, out decCurrResChr))
                                                        {
                                                            if (decCurrResChr != decHistoryResChr)
                                                            {
                                                                chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}两次结果不一致", resCurrent.ItmEname), lvError);
                                                            }
                                                        }
                                                        else//如果历史结果或当前结果不为数字
                                                        {
                                                            int h_res_flag = -1;//0=A型 1=B型 2=O型  3=AB型
                                                            int curr_res_flag = -1;
                                                            //只判断不允许有误差的情况(A型、B型、O型、AB型)
                                                            //只判断历史结果与当前结果的血型差异
                                                            if (h_res_chr.Contains("阴性"))
                                                            {
                                                                h_res_flag = 0;
                                                            }
                                                            else if (h_res_chr.Contains("弱阳性"))
                                                            {
                                                                h_res_flag = 1;
                                                            }
                                                            else if (h_res_chr.Contains("阳性") || h_res_chr.StartsWith("+") || h_res_chr.EndsWith("+"))
                                                            {
                                                                h_res_flag = 2;
                                                            }

                                                            if (resCurrent.ObrValue.Contains("阴性"))
                                                            {
                                                                curr_res_flag = 0;
                                                            }
                                                            else if (resCurrent.ObrValue.Contains("弱阳性"))
                                                            {
                                                                curr_res_flag = 1;
                                                            }
                                                            else if (resCurrent.ObrValue.Contains("阳性") || resCurrent.ObrValue.StartsWith("+") || resCurrent.ObrValue.EndsWith("+"))
                                                            {
                                                                curr_res_flag = 2;
                                                            }


                                                            if (h_res_flag == -1 || curr_res_flag == -1)
                                                            {
                                                                if (h_res_chr.Contains("A") && !h_res_chr.Contains("AB"))
                                                                {
                                                                    h_res_flag = 0;
                                                                }
                                                                else if (h_res_chr.Contains("B") && !h_res_chr.Contains("AB"))
                                                                {
                                                                    h_res_flag = 1;
                                                                }
                                                                else if (h_res_chr.Contains("O"))
                                                                {
                                                                    h_res_flag = 2;
                                                                }
                                                                else if (h_res_chr.Contains("AB"))
                                                                {
                                                                    h_res_flag = 3;
                                                                }

                                                                if (resCurrent.ObrValue.Contains("A") && !resCurrent.ObrValue.Contains("AB"))
                                                                {
                                                                    curr_res_flag = 0;
                                                                }
                                                                else if (resCurrent.ObrValue.Contains("B") && !resCurrent.ObrValue.Contains("AB"))
                                                                {
                                                                    curr_res_flag = 1;
                                                                }
                                                                else if (resCurrent.ObrValue.Contains("O"))
                                                                {
                                                                    curr_res_flag = 2;
                                                                }
                                                                else if (resCurrent.ObrValue.Contains("AB"))
                                                                {
                                                                    curr_res_flag = 3;
                                                                }
                                                            }


                                                            if (h_res_flag != curr_res_flag)
                                                            {
                                                                //chkResult.AddMessage(EnumOperationErrorCode.ResultOverDiff
                                                                //         , string.Format("{0} {1}({2}%) ", resCurrent.ItmEname, h_res_chr, ettItemSam.ItmDiff)
                                                                //         , EnumOperationErrorLevel.Error);
                                                                chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}两次结果不一致", resCurrent.ItmEname), lvError);
                                                            }

                                                        }
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}在另一台仪器还没一审！", resCurrent.ItmEname), lvError);
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}只检验了一份报告!", resCurrent.ItmEname), lvError);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}只检验了一份报告!", resCurrent.ItmEname), lvError);
                                                return;
                                            }
                                        }

                                        #endregion
                                    }
                                }

                                #endregion
                            }
                        }

                        #endregion

                        //根据项目
                        EntityDicItemSample ettItemSam = dcl.svr.cache.DictItemSamCache.Current.GetItem(resCurrent.ItmId, pat_info.PidSamId);
                        //历史结果有效时间
                        List<EntityObrResult> listHistory = new List<EntityObrResult>();
                        if (ettItemSam != null && ettItemSam.ItmValidDays > 0)
                        {
                            listHistory = listHistoryRes.FindAll(w => w.ObrDate >= DateTime.Now.Date.AddDays(-ettItemSam.ItmValidDays));
                        }

                        listHistory = listHistoryRes.FindAll(w => w.ItmId == resCurrent.ItmId && w.RepStatus != 0);

                        if (ettItemSam == null)
                        {
                            continue;
                        }

                        if (listHistory.Count > 0)
                        {
                            string h_res_chr = listHistory[0].ObrValue;

                            decimal decCurrResChr;
                            decimal decHistoryResChr;

                            if (decimal.TryParse(h_res_chr, out decHistoryResChr)
                                && decimal.TryParse(resCurrent.ObrValue, out decCurrResChr))
                            {
                                if (decHistoryResChr == 0 || ettItemSam.ItmDiff == 0)
                                {
                                    continue;
                                }

                                if (Math.Abs((decCurrResChr - decHistoryResChr)) / decHistoryResChr > (ettItemSam.ItmDiff / 100))
                                {
                                    chkResult.AddMessage(EnumOperationErrorCode.ResultOverDiff
                                                        , string.Format("{0} {1}({2}%) ", resCurrent.ItmEname, decHistoryResChr, ettItemSam.ItmDiff)
                                                        , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_HistoryResultCompare : config.Audit_Second_ErrorLevel_HistoryResultCompare);
                                }
                            }
                            else//如果历史结果或当前结果不为数字
                            {
                                if (ettItemSam.ItmDiff == 0)//判断阴阳性历史结果差异
                                {
                                    int h_res_flag = -1;//0=阴性 1=弱阳性 2=阳性 
                                    int curr_res_flag = -1;
                                    //只判断不允许有误差的情况(阴性、阳性、弱阳性)
                                    //只判断历史结果与当前结果的阴阳性差异

                                    if (h_res_chr.Contains("阴性"))
                                    {
                                        h_res_flag = 0;
                                    }
                                    else if (h_res_chr.Contains("弱阳性"))
                                    {
                                        h_res_flag = 1;
                                    }
                                    else if (h_res_chr.Contains("阳性") || h_res_chr.StartsWith("+") || h_res_chr.EndsWith("+"))
                                    {
                                        h_res_flag = 2;
                                    }

                                    if (resCurrent.ObrValue.Contains("阴性"))
                                    {
                                        curr_res_flag = 0;
                                    }
                                    else if (resCurrent.ObrValue.Contains("弱阳性"))
                                    {
                                        curr_res_flag = 1;
                                    }
                                    else if (resCurrent.ObrValue.Contains("阳性") || resCurrent.ObrValue.StartsWith("+") || resCurrent.ObrValue.EndsWith("+"))
                                    {
                                        curr_res_flag = 2;
                                    }

                                    if (h_res_flag != -1 && curr_res_flag != -1)
                                    {
                                        if (h_res_flag != curr_res_flag)
                                        {
                                            chkResult.AddMessage(EnumOperationErrorCode.ResultOverDiff
                                                     , string.Format("{0} {1}({2}%) ", resCurrent.ItmEname, h_res_chr, ettItemSam.ItmDiff)
                                                     , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_HistoryResultCompare : config.Audit_Second_ErrorLevel_HistoryResultCompare);
                                        }
                                    }
                                }
                                else if (ettItemSam.ItmDiff == -1)//判断ABO血型历史结果差异
                                {
                                    int h_res_flag = -1;//0=A型 1=B型 2=O型  3=AB型
                                    int curr_res_flag = -1;
                                    //只判断不允许有误差的情况(A型、B型、O型、AB型)
                                    //只判断历史结果与当前结果的血型差异

                                    if (h_res_chr.Contains("A") && !h_res_chr.Contains("AB"))
                                    {
                                        h_res_flag = 0;
                                    }
                                    else if (h_res_chr.Contains("B") && !h_res_chr.Contains("AB"))
                                    {
                                        h_res_flag = 1;
                                    }
                                    else if (h_res_chr.Contains("O"))
                                    {
                                        h_res_flag = 2;
                                    }
                                    else if (h_res_chr.Contains("AB"))
                                    {
                                        h_res_flag = 3;
                                    }

                                    if (resCurrent.ObrValue.Contains("A") && !resCurrent.ObrValue.Contains("AB"))
                                    {
                                        curr_res_flag = 0;
                                    }
                                    else if (resCurrent.ObrValue.Contains("B") && !resCurrent.ObrValue.Contains("AB"))
                                    {
                                        curr_res_flag = 1;
                                    }
                                    else if (resCurrent.ObrValue.Contains("O"))
                                    {
                                        curr_res_flag = 2;
                                    }
                                    else if (resCurrent.ObrValue.Contains("AB"))
                                    {
                                        curr_res_flag = 3;
                                    }


                                    if (h_res_flag != -1 && curr_res_flag != -1)
                                    {
                                        if (h_res_flag != curr_res_flag)
                                        {
                                            chkResult.AddMessage(EnumOperationErrorCode.ResultOverDiff
                                                     , string.Format("{0} {1}({2}%) ", resCurrent.ItmEname, h_res_chr, ettItemSam.ItmDiff)
                                                     , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_HistoryResultCompare : config.Audit_Second_ErrorLevel_HistoryResultCompare);
                                        }
                                    }
                                }
                                else //全字符匹配
                                {
                                    if (resCurrent.ObrValue.Trim() != h_res_chr.Trim())
                                    {
                                        chkResult.AddMessage(EnumOperationErrorCode.ResultOverDiff
                                                       , string.Format("{0} {1}({2}%) ", resCurrent.ItmEname, h_res_chr, ettItemSam.ItmDiff)
                                                       , auditType == EnumOperationCode.Audit ? config.Audit_First_ErrorLevel_HistoryResultCompare : config.Audit_Second_ErrorLevel_HistoryResultCompare);

                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    if (noteSameItemResultList != null && noteSameItemResultList.Count > 0)
                    {
                        #region 没有历史结果

                        if (!string.IsNullOrEmpty(DoubleBlindItrA) && !string.IsNullOrEmpty(DoubleBlindItrB)
                            && (pat_info.RepItrId == DoubleBlindItrA || pat_info.RepItrId == DoubleBlindItrB || pat_info.RepItrId == DoubleBlindItrC))
                        {
                            //没有历史结果时,B仪器一审时,两个的技师是否一致
                            if (auditType == EnumOperationCode.Audit && (pat_info.RepItrId == DoubleBlindItrB || pat_info.RepItrId == DoubleBlindItrA || pat_info.RepItrId == DoubleBlindItrC))
                            {
                                //检查：同一条码的两份报告的检验者是否一致
                                #region 检查：同一条码的两份报告的检验者是否一致

                                if (!string.IsNullOrEmpty(pat_info.RepBarCode))
                                {

                                    EntityPatientQC qc = new EntityPatientQC();
                                    qc.RepId = pat_info.RepId;
                                    qc.NotInRepId = true;
                                    qc.RepBarCode = pat_info.RepBarCode;
                                    List<EntityPidReportMain> listSameBcPat = new List<EntityPidReportMain>();
                                    IDaoPidReportMain mainDao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
                                    if (mainDao != null)
                                    {
                                        listSameBcPat = mainDao.PatientQuery(qc);
                                    }
                                    if (listSameBcPat != null && listSameBcPat.Count > 0)
                                    {
                                        EnumOperationErrorLevel lvError = EnumOperationErrorLevel.Error;
                                        try
                                        {
                                            if (Convert.ToBoolean(Caller.OperationName))
                                            {
                                                lvError = EnumOperationErrorLevel.Message;
                                            }
                                        }
                                        catch
                                        {
                                        }

                                        foreach (EntityObrResult resCurrent in noteSameItemResultList)//遍历当前检验结果
                                        {
                                            //同一条码的两份报告的检验者是否一致，相同则提示不能审核(用录入者对比)
                                            //if (pat_info.RepCheckUserId == dtSameBcPat.Rows[0]["pat_i_code"].ToString())
                                            //{
                                            //    chkResult.AddCustomMessage("", "", string.Format("此条码正在检验的{0}两次报告的检验者不能为同一人！", resCurrent.ItmEname), lvError);
                                            //    return;
                                            //}

                                            //同一条码的两份报告的检验者是否一致，相同则提示不能审核(用一审者对比)
                                            if (Caller != null && Caller.LoginID == listSameBcPat[0].RepAuditUserId)
                                            {
                                                chkResult.AddCustomMessage("", "", string.Format("此条码正在检验的{0}两次报告的检验者不能为同一人！", resCurrent.ItmEname), lvError);
                                                return;
                                            }
                                        }
                                    }

                                }

                                #endregion
                            }

                            //没有历史结果时,A仪器二审时时要判断二者的结果是否都一审并一致;如果B没有一审,则要提示
                            if (auditType == EnumOperationCode.Report && (pat_info.RepItrId == DoubleBlindItrA || pat_info.RepItrId == DoubleBlindItrC))
                            {
                                //检查：同一条码的两份报告的结果是否一致
                                #region 检查：同一条码的两份报告的结果是否一致

                                if (!string.IsNullOrEmpty(pat_info.RepBarCode))
                                {
                                    List<EntityObrResult> listSameBcPat = new List<EntityObrResult>();
                                    if (resultDao != null)
                                        listSameBcPat = resultDao.GetResultInfoByBarcode(pat_info.RepBarCode, pat_info.RepId);
                                    EnumOperationErrorLevel lvError = EnumOperationErrorLevel.Error;
                                    try
                                    {
                                        if (Convert.ToBoolean(Caller.OperationName))
                                        {
                                            lvError = EnumOperationErrorLevel.Message;
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    if (listSameBcPat != null && listSameBcPat.Count > 0)
                                    {
                                        foreach (EntityObrResult resCurrent in noteSameItemResultList)//遍历当前检验结果
                                        {
                                            List<EntityObrResult> listTempSel = listSameBcPat.FindAll(w => w.ItmId == resCurrent.ItmId);
                                            //结果是否都一审并一致
                                            if (listTempSel != null && listTempSel.Count > 0)
                                            {
                                                if (listTempSel[0].RepStatus.ToString() == "1"
                                                    || listTempSel[0].RepStatus.ToString() == "2"
                                                    || listTempSel[0].RepStatus.ToString() == "4")
                                                {
                                                    #region 对比结果是否一样
                                                    string h_res_chr = listTempSel[0].ObrValue;

                                                    decimal decCurrResChr;
                                                    decimal decHistoryResChr;
                                                    if (decimal.TryParse(h_res_chr, out decHistoryResChr)
                                                        && decimal.TryParse(resCurrent.ObrValue, out decCurrResChr))
                                                    {
                                                        if (decCurrResChr != decHistoryResChr)
                                                        {
                                                            chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}两次结果不一致", resCurrent.ItmEname), lvError);
                                                        }
                                                    }
                                                    else//如果历史结果或当前结果不为数字
                                                    {
                                                        int h_res_flag = -1;//0=A型 1=B型 2=O型  3=AB型
                                                        int curr_res_flag = -1;
                                                        //只判断不允许有误差的情况(A型、B型、O型、AB型)
                                                        //只判断历史结果与当前结果的血型差异
                                                        if (h_res_chr.Contains("阴性"))
                                                        {
                                                            h_res_flag = 0;
                                                        }
                                                        else if (h_res_chr.Contains("弱阳性"))
                                                        {
                                                            h_res_flag = 1;
                                                        }
                                                        else if (h_res_chr.Contains("阳性") || h_res_chr.StartsWith("+") || h_res_chr.EndsWith("+"))
                                                        {
                                                            h_res_flag = 2;
                                                        }

                                                        if (resCurrent.ObrValue.Contains("阴性"))
                                                        {
                                                            curr_res_flag = 0;
                                                        }
                                                        else if (resCurrent.ObrValue.Contains("弱阳性"))
                                                        {
                                                            curr_res_flag = 1;
                                                        }
                                                        else if (resCurrent.ObrValue.Contains("阳性") || resCurrent.ObrValue.StartsWith("+") || resCurrent.ObrValue.EndsWith("+"))
                                                        {
                                                            curr_res_flag = 2;
                                                        }


                                                        if (h_res_flag == -1 || curr_res_flag == -1)
                                                        {
                                                            if (h_res_chr.Contains("A") && !h_res_chr.Contains("AB"))
                                                            {
                                                                h_res_flag = 0;
                                                            }
                                                            else if (h_res_chr.Contains("B") && !h_res_chr.Contains("AB"))
                                                            {
                                                                h_res_flag = 1;
                                                            }
                                                            else if (h_res_chr.Contains("O"))
                                                            {
                                                                h_res_flag = 2;
                                                            }
                                                            else if (h_res_chr.Contains("AB"))
                                                            {
                                                                h_res_flag = 3;
                                                            }

                                                            if (resCurrent.ObrValue.Contains("A") && !resCurrent.ObrValue.Contains("AB"))
                                                            {
                                                                curr_res_flag = 0;
                                                            }
                                                            else if (resCurrent.ObrValue.Contains("B") && !resCurrent.ObrValue.Contains("AB"))
                                                            {
                                                                curr_res_flag = 1;
                                                            }
                                                            else if (resCurrent.ObrValue.Contains("O"))
                                                            {
                                                                curr_res_flag = 2;
                                                            }
                                                            else if (resCurrent.ObrValue.Contains("AB"))
                                                            {
                                                                curr_res_flag = 3;
                                                            }
                                                        }


                                                        if (h_res_flag != curr_res_flag)
                                                        {
                                                            //chkResult.AddMessage(EnumOperationErrorCode.ResultOverDiff
                                                            //         , string.Format("{0} {1}({2}%) ", resCurrent.ItmEname, h_res_chr, ettItemSam.ItmDiff)
                                                            //         , EnumOperationErrorLevel.Error);
                                                            chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}两次结果不一致", resCurrent.ItmEname), lvError);
                                                        }

                                                    }
                                                    #endregion
                                                }
                                                else
                                                {
                                                    chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}在另一台仪器还没一审！", resCurrent.ItmEname), lvError);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}只检验了一份报告!", resCurrent.ItmEname), lvError);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        chkResult.AddCustomMessage("", "", string.Format("此病人正在检验的{0}只检验了一份报告!", noteSameItemResultList[0].ItmEname), lvError);
                                        return;
                                    }
                                }

                                #endregion
                            }
                        }

                        #endregion
                    }
                }

                sw.Stop();
            }
        }
        #endregion

    }

}