using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.svr.cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.svr.interfaces
{
    /// <summary>
    /// 生成茂名妇幼门诊报告信息的帮助类
    /// </summary>
    internal class MZReportCreater
    {
        #region 根据门诊病人ID获取条码信息（不保存的）

        internal List<EntitySampMain> GetMzBarcodeInfo(List<EntityInterfaceData> listInterfaceData,EntityInterfaceExtParameter Parameter)
        {
            List<EntitySampMain> listSampMain = new List<EntitySampMain>();
            DateTime dtOpTime = ServerDateTime.GetDatabaseServerDateTime();
            try
            {
                
                if (listInterfaceData.Count > 0)
                {
                    foreach (EntityInterfaceData interfaceData in listInterfaceData)
                    {
                        if (interfaceData.InterfaceData != null &&
                            interfaceData.InterfaceData.Tables.Count > 0)
                        {
                            //拿到接口的对照信息
                            ContrastDefineBIZ contBiz = new ContrastDefineBIZ();
                            List<EntitySysItfContrast> listContrast = contBiz.GetSysContrast(interfaceData.InterfaceID);

                            DataTable dtData = DistinctYZ(interfaceData, listContrast); 

                            //去除本地配置中设置的组合过滤选项
                            DataTable dtData2 = DistinctFilterSam(dtData, listContrast, Parameter.MzFiterSams);

                            //转换后的数据表
                            DataTable dtSampMain = ConvertToLis(dtData2, listContrast);



                            if (dtData2.Rows.Count == 0)
                                continue;

                            List<string> listHisCode = new List<string>();

                            foreach (DataRow drSampMain in dtSampMain.Rows)
                            {
                                listHisCode.Add(drSampMain["bc_his_code"].ToString());
                            }

                            //来源ID
                            string strOriId = Parameter.GetSrcId();

                            //获取拆分信息
                            List<EntitySampMergeRule> listRule = GetSampMergeRule(listHisCode, strOriId);

                            //填充拆分信息
                            FillComSplitInfo(dtSampMain, listRule, strOriId);


                            //取出不重复的人员号
                            DataView dv = new DataView(dtSampMain);
                            DataTable dtInNo = dv.ToTable(true, "bc_in_no");


                            //存放组合对应的拆分信息（同一组合只查一次，减少数据库读取量）
                            Dictionary<string, List<EntitySampMergeRule>> dicSplitCache = new Dictionary<string, List<EntitySampMergeRule>>();
                            int i = 1;

                            //循环人员  注意病区ID
                            foreach (DataRow drInNo in dtInNo.Rows)
                            {
                                string strInNo = drInNo["bc_in_no"].ToString();

                                //根据人员拿出医嘱信息
                                DataRow[] drSampMains = dtSampMain.Select("bc_in_no='" + strInNo + "'");

                                //用来存放需要拆分的医嘱信息
                                DataTable dtSampMainSplit = dtSampMain.Clone();

                                //最终需要操作的医嘱信息
                                DataTable dtSampMainTotal = dtSampMain.Clone();

                                foreach (DataRow drSM in drSampMains)
                                {
                                    //需要拆分
                                    if (drSM["ComSplitFlag"].ToString() == "1")
                                        dtSampMainSplit.Rows.Add(drSM.ItemArray);
                                    else
                                        dtSampMainTotal.Rows.Add(drSM.ItemArray);
                                }

                                //存在需要拆分的医嘱信息
                                if (dtSampMainSplit.Rows.Count > 0)
                                {
                                    //拆分医嘱
                                    dtSampMainTotal.Merge(SplitSampMain(dtSampMainSplit, strOriId, ref listRule, ref dicSplitCache));
                                }

                                //取出不重复的合并编码
                                DataView dvFilter = new DataView(dtSampMainTotal);
                                DataTable dtSplitCode = new DataTable();

                                bool existGroupId = false;
                                //是否存在分组ID
                                if (dtSampMainTotal.Columns.Contains("bc_gourp_id"))
                                {
                                    dtSplitCode = dvFilter.ToTable(true, new string[] { "ComSplitCode", "bc_gourp_id" });
                                    existGroupId = true;
                                }
                                else
                                    dtSplitCode = dvFilter.ToTable(true, new string[] { "ComSplitCode" });

                                //根据合并编码循环
                                foreach (DataRow drSplitCode in dtSplitCode.Rows)
                                {
                                    if (!string.IsNullOrEmpty(drSplitCode["ComSplitCode"].ToString()))
                                    {
                                        string strFilter = string.Empty;

                                        if (existGroupId)
                                            strFilter = string.Format("ComSplitCode='{0}' and bc_gourp_id='{1}'",
                                                                       drSplitCode["ComSplitCode"].ToString(),
                                                                       drSplitCode["bc_gourp_id"].ToString());
                                        else
                                            strFilter = string.Format("ComSplitCode='{0}'", drSplitCode["ComSplitCode"].ToString());

                                        //从常规合并信息中取出
                                        DataRow[] drSampMainSave = dtSampMainTotal.Select(strFilter);

                                        List<EntitySampMain> sampMain = CreateSampMain(drSampMainSave, listRule, Parameter, dtOpTime);

                                        listSampMain.AddRange(sampMain.ToArray());
                                    }
                                    else
                                    {
                                        string strFilter = string.Empty;

                                        if (existGroupId)
                                            strFilter = string.Format("(ComSplitCode is null or ComSplitCode='') and bc_gourp_id='{0}'",
                                                                       drSplitCode["bc_gourp_id"].ToString());
                                        else
                                            strFilter = "ComSplitCode is null or ComSplitCode=''";

                                        DataRow[] drSampMainSave = dtSampMainTotal.Select(strFilter);

                                        foreach (DataRow item in drSampMainSave)
                                        {
                                            List<EntitySampMain> sampMain = CreateSampMain(new DataRow[] { item }, listRule, Parameter, dtOpTime);

                                            listSampMain.AddRange(sampMain.ToArray());
                                        }
                                    }
                                }
                                i++;
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return listSampMain;
        }

        /// <summary>
        /// 去除重复医嘱信息和已下载医嘱
        /// </summary>
        /// <param name="dtSour"></param>
        /// <param name="interfaceId"></param>
        /// <returns></returns>
        private DataTable DistinctYZ(EntityInterfaceData interfaceData, List<EntitySysItfContrast> listContrast)
        {
            //his数据
            DataTable dtData = interfaceData.InterfaceData.Tables[0];
            
            DataTable dtResult = dtData.Clone();

            List<string> listYzId = new List<string>();


            //拿到医嘱ID对照的字段
            EntitySysItfContrast contrast = listContrast.Find(w => w.ContSysColumn == "bc_order_id");
            EntitySysItfContrast sourceColumn = listContrast.Find(w => w.ContSysColumn == "bc_ori_name"); //住院：IP，门诊：OP
            string sourceName = dtData.Rows[0][sourceColumn.ContInterfaceColumn].ToString();
            if (!string.IsNullOrEmpty(contrast.ContInterfaceColumn))
            {
                string strInterfaceColumn = contrast.ContInterfaceColumn;
                foreach (DataRow drData in dtData.Rows)
                {
                    string strYzId = drData[strInterfaceColumn].ToString();

                    string selectforDt = string.Format("{0}='{1}'", strInterfaceColumn, strYzId);

                    if (!listYzId.Contains(strYzId))
                    {
                        dtResult.Rows.Add(drData.ItemArray);
                        listYzId.Add(strYzId);
                    }
                }

                //去除已下载的医嘱
                if (listYzId.Count > 0)
                {
                    //查询医嘱ID是否已经下载
                    List<EntitySampDetail> listSampDetailTotal = new List<EntitySampDetail>();

                    IDaoSampDetail daoDetail = DclDaoFactory.DaoHandler<IDaoSampDetail>();
                    if (daoDetail != null)
                        listSampDetailTotal = daoDetail.GetSampDetailByYzId(listYzId, sourceName);


                    IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();

                    DataTable dtDistinctResult = dtData.Clone();

                    foreach (DataRow item in dtResult.Rows)
                    {
                        bool isexist = detailDao.IsExistedOrder(item[strInterfaceColumn].ToString());
                        if (!isexist && listSampDetailTotal.FindIndex(w => w.OrderSn == item[strInterfaceColumn].ToString()) < 0)
                        {
                            dtDistinctResult.Rows.Add(item.ItemArray);
                        }
                    }

                    dtResult = dtDistinctResult;
                }
            }
            else
                dtResult = dtData;

            return dtResult;
        }


        /// <summary>
        /// 去除本地配置中设置的过滤项目
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="listContrast"></param>
        /// <returns></returns>
        private DataTable DistinctFilterSam(DataTable dtData, List<EntitySysItfContrast> listContrast, string MzFilterCombines)
        {
            DataTable dtResult = dtData.Clone();
            if (string.IsNullOrEmpty(MzFilterCombines))
            {
                dtResult = dtData;
                return dtResult;
            }
            //获取组合的his编码
            List<string> combines = new List<string>(MzFilterCombines.Split(','));
            
            //拿到组合ID对照的字段
            EntitySysItfContrast contrast = listContrast.Find(w => w.ContSysColumn == "bc_sam_id");
            if (!string.IsNullOrEmpty(contrast.ContInterfaceColumn))
            {
                string strInterfaceColumn = contrast.ContInterfaceColumn;
                foreach (DataRow drData in dtData.Rows)
                {
                    string strhiscode = drData[strInterfaceColumn].ToString();
                    if (combines.Contains(strhiscode))
                    {
                        dtResult.Rows.Add(drData.ItemArray);
                    }
                }
            }
            return dtResult;
        }

        /// <summary>
        /// 获取组合拆分信息
        /// </summary>
        /// <param name="listHisCode"></param>
        /// <param name="strOriId"></param>
        /// <returns></returns>
        private List<EntitySampMergeRule> GetSampMergeRule(List<string> listHisCode, string strOriId)
        {
            List<EntitySampMergeRule> listRule = new List<EntitySampMergeRule>();
            IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            if (dao != null)
            {
                listRule = dao.SearchRuleByHisCode(listHisCode, strOriId);
            }
            return listRule;
        }

        /// <summary>
        /// 填充拆分信息
        /// </summary>
        /// <param name="dtSour"></param>
        /// <param name="listRule"></param>
        /// <param name="strOriId"></param>
        private void FillComSplitInfo(DataTable dtSour, List<EntitySampMergeRule> listRule, string strOriId)
        {
            //填充合并规则和拆分字段
            dtSour.Columns.Add("ComSplitCode");//合并规则
            dtSour.Columns.Add("ComSplitFlag");//拆分标志
            dtSour.Columns.Add("ComRulId");//规则编码
            dtSour.Columns.Add("ComId");//组合编码

            foreach (DataRow dr in dtSour.Rows)
            {
                string strHisCode = dr["bc_his_code"].ToString();

                //取出对应规则
                List<EntitySampMergeRule> rule = listRule.FindAll(w => w.ComHisFeeCode == strHisCode).ToList().
                                                          OrderByDescending(o => o.ComSource).ToList();

                if (rule != null && rule.Count > 0)
                {
                    EntitySampMergeRule smRule = rule[0];

                    dr["ComSplitCode"] = smRule.ComSplitCode;

                    if (smRule.ComSplitFlag != null)
                        dr["ComSplitFlag"] = smRule.ComSplitFlag.Value;
                    else
                        dr["ComSplitFlag"] = 0;

                    dr["ComRulId"] = smRule.ComRulId;
                    dr["ComId"] = smRule.ComId;
                }
            }

            //系统配置：允许下载HIS码未匹配的条码
            string strBarcodeAllowDownHisNotMatch = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Barcode_AllowDown_HisNotMatch");
            if (strBarcodeAllowDownHisNotMatch == "否")
            {
                DataTable dtFillSour = dtSour.Clone();
                string strLog = string.Empty;
                foreach (DataRow dr in dtSour.Rows)
                {
                    if (dr["ComSplitCode"] == null || string.IsNullOrEmpty(dr["ComSplitCode"].ToString()))
                    {
                        strLog += string.Format("在病人来源[{0}]中未找到匹配的com_his_fee_code='{1}'\r\n", strOriId, dr["bc_his_code"].ToString());
                    }
                    else
                        dtFillSour.Rows.Add(dr.ItemArray);
                }

                if (strLog != string.Empty)
                    Lib.LogManager.Logger.LogInfo("HisCodeNotMatchLisCode_log", strLog);

                dtSour = dtFillSour;
            }
        }


        /// <summary>
        /// 条码拆分
        /// </summary>
        /// <param name="dtSampMainSplit">拆分信息</param>
        /// <param name="listSkipYZID">拆分医嘱ID</param>
        /// <param name="strOriId">来源</param>
        /// <param name="listRule">总的拆分信息</param>
        /// <param name="dicSplitCache">组合对应拆分信息</param>
        /// <returns></returns>
        private DataTable SplitSampMain(DataTable dtSampMainSplit,
                                        string strOriId,
                                        ref List<EntitySampMergeRule> listRule,
                                        ref Dictionary<string, List<EntitySampMergeRule>> dicSplitCache)
        {
            //拆分后信息
            DataTable dtSampMain = dtSampMainSplit.Clone();

            //处理拆分组合
            foreach (DataRow drSMSplit in dtSampMainSplit.Rows)
            {
                //根据大小组合表拿出数据
                //生成对应条码医嘱信息
                //如果不存在加入到跳过医嘱ID判断集合

                string strComId = drSMSplit["ComId"].ToString();

                if (!string.IsNullOrEmpty(strComId))
                {
                    //如果缓存存在该拆字典则直接从缓存中取。
                    List<EntitySampMergeRule> listSMRule = new List<EntitySampMergeRule>();
                    if (!dicSplitCache.ContainsKey(strComId))
                    {
                        listSMRule = GetSampMergeRuleRuleBySplitComId(drSMSplit["ComId"].ToString(), strOriId);
                        dicSplitCache.Add(strComId, listSMRule);
                    }
                    else
                    {
                        listSMRule = dicSplitCache[strComId];
                    }
                    //listSMRule = listSMRule.FindAll(w => w.ComHisFeeCode == drSMSplit["bc_his_code"].ToString()).ToList().
                    //                                OrderByDescending(o => o.ComSource).ToList();
                    if (listSMRule.Count > 0)
                    {
                        //拆分条码组合只插入一次
                        List<string> listComId = new List<string>();
                        foreach (EntitySampMergeRule item in listSMRule)
                        {
                            if (!listComId.Contains(item.ComId))
                            {
                                DataRow drNew = dtSampMain.NewRow();
                                drNew.ItemArray = (object[])drSMSplit.ItemArray.Clone();

                                drNew["ComSplitCode"] = item.ComSplitCode;//合并规则
                                drNew["ComSplitFlag"] = "1";//item.ComSplitFlag;//拆分标志 拆分后的项目此标志默认为拆分
                                drNew["ComRulId"] = item.ComRulId;//规则编码
                                drNew["ComId"] = item.ComId;//组合编码

                                dtSampMain.Rows.Add(drNew);

                                //插入到总的拆分信息中
                                if (listRule.FindIndex(w => w.ComRulId == item.ComRulId) < 0)
                                    listRule.Add((EntitySampMergeRule)item.Clone());

                                listComId.Add(item.ComId);
                            }
                        }
                    }

                }

            }

            return dtSampMain;
        }

        private List<EntitySampMergeRule> GetSampMergeRuleRuleBySplitComId(string strComId, String strOriId)
        {
            List<EntitySampMergeRule> listResult = new List<EntitySampMergeRule>();

            IDaoSampMergeRule dao = DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            if (dao != null)
            {
                listResult = dao.SearchRuleBySplitComId(strComId, strOriId);
            }

            return listResult;
        }


        /// <summary>
        /// 生成条码信息
        /// </summary>
        /// <param name="drSampMain"></param>
        /// <returns></returns>
        private List<EntitySampMain> CreateSampMain(DataRow[] drSampMain, List<EntitySampMergeRule> listRule, EntityInterfaceExtParameter parameter, DateTime dtOpTime)
        {
            EntitySampMain sampMain = new EntitySampMain();


            string strBarId = "";

            //用于存储重复项目（开多次相同项目医嘱）
            List<DataRow> listRepeatSampMain = new List<DataRow>();

            for (int i = 0; i < drSampMain.Length; i++)
            {
                EntitySampMergeRule rule = listRule.Find(w => w.ComRulId.ToString() == drSampMain[i]["ComRulId"].ToString());
                if (rule == null)
                    rule = new EntitySampMergeRule();


                //填充条码信息
                if (i == 0)
                {
                    #region 填充SampMain
                    sampMain = FillSampMain(drSampMain[i], strBarId, rule, parameter, dtOpTime);
                    #endregion
                }

                #region 填充SampDetail
                EntitySampDetail sampDetail = FillSampDetail(drSampMain[i], strBarId, rule, dtOpTime);


                //判断是否存在相同项目
                int sampDetailIndex = sampMain.ListSampDetail.Count == 0 ? -1 :
                                      sampMain.ListSampDetail.FindIndex(w => w.OrderCode == sampDetail.OrderCode);

                //如果存在相同项目，不添加到该条码，记录起来。
                if (sampDetailIndex > -1)
                {
                    //如果医嘱ID相同，则不添加
                    if (!(sampMain.ListSampDetail[sampDetailIndex].OrderSn == sampDetail.OrderSn && sampDetail.ComSplitFlag == "0"))
                    {
                        listRepeatSampMain.Add(drSampMain[i]);
                    }
                }
                else
                {
                    sampMain.SampComName += string.Format("+{0}", sampDetail.ComName);
                    sampMain.ListSampDetail.Add(sampDetail);
                }
                #endregion
            }

            if (sampMain.SampComName.Length > 0)
                sampMain.SampComName = sampMain.SampComName.Remove(0, 1);

            List<EntitySampMain> ListSampMain = new List<EntitySampMain>();
            ListSampMain.Add(sampMain);

            //存在重复项目则递归调用，直到所有项目都生成条码
            if (listRepeatSampMain.Count > 0)
            {
                ListSampMain.AddRange(CreateSampMain(listRepeatSampMain.ToArray(), listRule, parameter, dtOpTime).ToArray());
            }

            return ListSampMain;
        }

        /// <summary>
        /// 转换成检验数据
        /// </summary>
        /// <param name="hisData"></param>
        /// <param name="listContrast"></param>
        /// <returns></returns>
        private DataTable ConvertToLis(DataTable hisData, List<EntitySysItfContrast> listContrast)
        {
            //创建转换后的条码表
            DataTable dtSampMain = new DataTable();
            foreach (EntitySysItfContrast item in listContrast)
            {
                if (!string.IsNullOrEmpty(item.ContInterfaceColumn))
                {
                    dtSampMain.Columns.Add(item.ContSysColumn);
                }
            }
            //数据转换到检验表中
            foreach (DataRow hisRow in hisData.Rows)
            {
                DataRow lisRow = dtSampMain.NewRow();

                foreach (EntitySysItfContrast item in listContrast)
                {
                    if (!string.IsNullOrEmpty(item.ContInterfaceColumn))
                    {
                        if (!string.IsNullOrEmpty(item.ContColumnRule))
                        {
                            //规则转换
                            IRule rule = IRule.CreateRule(item.ContColumnRule);

                            if (item.ContSysColumn == "bc_urgent_flag")
                            {
                                lisRow[item.ContSysColumn] = false;
                                if (hisRow[item.ContInterfaceColumn].ToString() != string.Empty)
                                {
                                    bool urgentFlag = Convert.ToBoolean(rule.ConvertRule(hisRow[item.ContInterfaceColumn].ToString()));
                                    //增加急查标本的判断
                                    if (urgentFlag)
                                    {
                                        lisRow[item.ContSysColumn] = urgentFlag;
                                    }
                                }
                            }
                            else
                            {
                                lisRow[item.ContSysColumn] = rule.ConvertRule(hisRow[item.ContInterfaceColumn].ToString());
                            }
                        }
                        else
                            lisRow[item.ContSysColumn] = hisRow[item.ContInterfaceColumn];
                    }
                }

                dtSampMain.Rows.Add(lisRow);
            }
            return dtSampMain;
        }


        /// <summary>
        /// 填充标本信息
        /// </summary>
        /// <param name="drSampMain"></param>
        /// <param name="strBarId"></param>
        /// <param name="rule"></param>
        /// <param name="parameter"></param>
        /// <param name="dtOpTime"></param>
        /// <returns></returns>
        private EntitySampMain FillSampMain(DataRow drSampMain,
                                            string strBarId,
                                            EntitySampMergeRule rule,
                                            EntityInterfaceExtParameter parameter,
                                            DateTime dtOpTime)
        {
            List<EntityDicPubDept> listDicDept = dcl.svr.cache.DictDepartCache.Current.DclCache;
            List<EntityDicSample> listDicSample = dcl.svr.cache.DictSampleCache.Current.DclCache;

            EntitySampMain sampMain = EntityManager<EntitySampMain>.ConvertToEntityByMapName(drSampMain, "clab");
            sampMain.PidOrgId = ConfigurationManager.AppSettings["HospitalId"];
            sampMain.SampComName = string.Empty;
            sampMain.SampBarId = strBarId;
            sampMain.SampDate = dtOpTime;
            sampMain.SampLastactionDate = sampMain.SampDate;
            sampMain.SampStatusId = "0";
            sampMain.SampStatusName = "生成条码";
            sampMain.PidSrcId = parameter.GetSrcId();
            sampMain.PidIdtId = parameter.GetIdtId();
            //发票号
            if (!string.IsNullOrEmpty(parameter.InvoiceID))
                sampMain.PidSocialNo = parameter.InvoiceID;

            if (drSampMain.Table.Columns.Contains("bc_exp") && drSampMain["bc_exp"] != null)
                sampMain.SampCollectionNotice = drSampMain["bc_exp"].ToString();

            //Lib.LogManager.Logger.LogInfo("FillSampMain:505");

            string strRuleSamId = string.Empty;

            if (rule != null)
                strRuleSamId = rule.ComSamId;

            if (drSampMain.Table.Columns.Contains("bc_sample_remark") && drSampMain["bc_sample_remark"] != null)
                sampMain.SampRemark = drSampMain["bc_sample_remark"].ToString();

            //标本类别
            //1.系统配置：下载条码优先取本系统组合条码信息的标本类型
            //优先匹配bc_sam_id，如果没有标本ID对照再对照标本名称。
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_BCDW_priorityDictSamID") == "是" &&
                !string.IsNullOrEmpty(strRuleSamId))
            {
                sampMain.SampSamId = strRuleSamId;
            }
            else if (drSampMain.Table.Columns.Contains("bc_sam_id") && drSampMain["bc_sam_id"].ToString() != string.Empty)
            {
                EntityDicSample dicSam = listDicSample.Find(z => z.SamCode == drSampMain["bc_sam_id"].ToString());

                //如果标本ID存在检验系统中，则去字典的ID，不存在则直接取拆分字典中的标本ID
                if (dicSam != null && !string.IsNullOrEmpty(dicSam.SamId))
                {
                    sampMain.SampSamId = dicSam.SamId;
                    //标本名称为空 则取字典的标本名称
                    if (string.IsNullOrEmpty(sampMain.SamName))
                    {
                        sampMain.SamName = dicSam.SamName;
                    }
                }
                else
                    sampMain.SampSamId = strRuleSamId;
            }
            else if (drSampMain.Table.Columns.Contains("bc_sam_name") && drSampMain["bc_sam_name"].ToString() != string.Empty)
            {
                EntityDicSample dicSam = listDicSample.Find(z => z.SamName == drSampMain["bc_sam_name"].ToString());

                //如果标本名称能在检验系统中找到，则取字典的标本ID，不存在则直接取拆分字典中的标本ID
                if (dicSam != null && !string.IsNullOrEmpty(dicSam.SamId))
                    sampMain.SampSamId = dicSam.SamId;
                else
                    sampMain.SampSamId = strRuleSamId;
            }
            else
            {
                sampMain.SampSamId = strRuleSamId;
            }

            if (!string.IsNullOrEmpty(sampMain.PidDeptCode))
            {
                EntityDicPubDept dicDept = listDicDept.Find(w => w.DeptCode == sampMain.PidDeptCode);
                if (dicDept != null && !string.IsNullOrEmpty(dicDept.DeptName))
                    sampMain.PidDeptName = dicDept.DeptName;
            }

            //Lib.LogManager.Logger.LogInfo("FillSampMain:539");

            if (!string.IsNullOrEmpty(rule.ComId))
            {
                sampMain.SampType = rule.ComExecCode;
                sampMain.SampTubCode = rule.ComTubeCode;
                sampMain.SampSendDest = rule.ComTestDest;
                sampMain.SampRemId = rule.ComSamRemark;
                sampMain.SampPrintTime = rule.ComPrintCount;
            }

            sampMain.SampReturnTimes = 0;

            if (rule.ComBarcodeType != null &&
                rule.ComBarcodeType.Value == 1)
            {
                sampMain.SampBarType = 1;
            }
            else
            {
                sampMain.SampBarType = 0;
                sampMain.SampBarCode = strBarId;
            }

            return sampMain;
        }


        /// <summary>
        /// 填充项目信息
        /// </summary>
        /// <param name="drSampMain"></param>
        /// <param name="strBarId"></param>
        /// <param name="rule"></param>
        /// <param name="dtOpTime"></param>
        /// <returns></returns>
        private EntitySampDetail FillSampDetail(DataRow drSampMain,
                                                string strBarId,
                                                EntitySampMergeRule rule,
                                                DateTime dtOpTime)
        {

            List<EntityDicCombine> listDicCombine = dcl.svr.cache.DictCombineCache.Current.DclCache;

            EntitySampDetail sampDetail = new EntitySampDetail();
            sampDetail.SampBarId = strBarId;
            sampDetail.SampBarCode = strBarId;
            sampDetail.SampFlag = 0;

            if (drSampMain.Table.Columns.Contains("ComSplitFlag"))
                sampDetail.ComSplitFlag = drSampMain["ComSplitFlag"].ToString();

            if (!string.IsNullOrEmpty(rule.ComHisFeeCode))
                sampDetail.OrderCode = rule.ComHisFeeCode;
            else
                sampDetail.OrderCode = drSampMain["bc_his_code"].ToString();

            if (!string.IsNullOrEmpty(rule.ComHisName))
                sampDetail.OrderName = rule.ComHisName;
            else
                sampDetail.OrderName = drSampMain["bc_his_name"].ToString();

            if (!string.IsNullOrEmpty(rule.ComPrintName))
                sampDetail.ComName = rule.ComPrintName;
            else if (!string.IsNullOrEmpty(rule.ComName))
                sampDetail.ComName = rule.ComName;
            else
                sampDetail.ComName = drSampMain["bc_his_name"].ToString();

            //Lib.LogManager.Logger.LogInfo("FillSampMain:579");

            sampDetail.OrderSn = drSampMain["bc_order_id"].ToString();
            sampDetail.ApplyID = drSampMain["Sdet_apply_id"].ToString();

            DateTime orderExecuteDate = new DateTime();
            if (DateTime.TryParse(drSampMain["bc_occ_date"].ToString(), out orderExecuteDate))
            {
                sampDetail.OrderOccDate = orderExecuteDate;
                sampDetail.OrderDate = orderExecuteDate;
            }

            sampDetail.EnrolFlag = 0;
            sampDetail.DisplayFlag = 0;
            sampDetail.SampDate = dtOpTime;

            EntityDicCombine dicCom = null;

            if (drSampMain.Table.Columns.Contains("ComId") &&
                !string.IsNullOrEmpty(drSampMain["ComId"].ToString()))
            {
                dicCom = listDicCombine.Find(w => w.ComId == drSampMain["ComId"].ToString());
            }
            else if (!string.IsNullOrEmpty(rule.ComId))
            {
                dicCom = listDicCombine.Find(w => w.ComId == rule.ComId);
            }

            if (dicCom != null && !string.IsNullOrEmpty(dicCom.ComId))
            {
                sampDetail.ComId = dicCom.ComId;
                sampDetail.OrderPrice = dicCom.ComPrice;
                sampDetail.ComUrgentFlag = dicCom.ComUrgentFlag;
                sampDetail.SaveNotice = rule.ComSaveNotice;
                sampDetail.BloodNotice = rule.ComSamNotice;

            }

            if (drSampMain.Table.Columns.Contains("bc_com_price") &&
                !string.IsNullOrEmpty(drSampMain["bc_com_price"].ToString()))
            {
                Decimal ComPrice = 0;
                if (Decimal.TryParse(drSampMain["bc_com_price"].ToString(), out ComPrice))
                    sampDetail.OrderPrice = ComPrice;
            }

            return sampDetail;
        }

        internal EntityPidReportMain CreateReport(EntitySampMain barcode)
        {
            DateTime dtOpTime = ServerDateTime.GetDatabaseServerDateTime();
            EntityPidReportMain report = new EntityPidReportMain();
            //report.RepSn = "";
            report.RepId = "";
            report.RepItrId = "";
            report.RepSid = "";
            report.PidName = barcode.PidName;
            report.PidSex = new SexRule().ConvertRule(barcode.PidSex);
            report.PidAge = 0;//barcode.PidAge;
            report.PidAgeUnit = "";// barcode.uni
            report.PidDeptId = barcode.PidDeptCode;
            report.PidDeptCode = barcode.PidDeptCode;
            report.PidDeptName = barcode.PidDeptName;
            report.PidIdtId = barcode.PidIdtId;
            report.PidInNo = barcode.PidInNo;
            report.PidAddmissTimes = int.Parse(barcode.PidAdmissTimes.ToString());
            report.PidBedNo = barcode.PidBedNo;
            report.PidComName = barcode.SampComName;
            report.PidDiag = barcode.PidDiag;
            report.PidRemark = barcode.SampRemark;
            report.PidWork = "";
            report.PidTel = barcode.PidTel;
            report.PidEmail = "";
            report.PidUnit = "";
            report.PidAddress = barcode.PidAddress;
            report.PidPreWeek = "";
            report.PidHeight = "";
            report.PidWeight = "";
            report.PidSamId = barcode.SampSamId;
            report.SamName = barcode.SamName;
            report.PidPurpId = "";
            report.PidDoctorCode = barcode.PidDoctorCode;
            report.PidDocName = barcode.PidDoctorName;
            report.RepCtype = "1";//急查
            report.RepSendFlag = 0;
            report.RepStatus = 0;
            report.RepDiseaseFlag = 0;
            report.RepRemark = barcode.SampRemark;
            report.RepInputId = "";
            report.RepInDate = dtOpTime;
            report.SampSendDate = barcode.SendDate;
            report.SampReciveDate = barcode.ReceiverDate;
            report.PidSocialNo = barcode.PidSocialNo;
            report.PidExamNo = barcode.PidExamNo;
            report.RepBarCode = "";
            report.RepSerialNum = "";
            report.SampApplyDate = barcode.CollectionDate;
            report.CollectionPart = "";
            report.PidSrcId = barcode.PidSrcId;
            report.PidOrgId = barcode.PidOrgId;
            report.RepModifyFrequency = 0;
            report.SampRemark = barcode.SampRemark;
            report.SampReceiveDate = barcode.ReceiverDate;
            report.PidWardName = barcode.PidDeptName;
            report.PidWardId = barcode.PidDeptCode;
            report.PidApplyNo = barcode.SampApplyNo;
            report.RepRecheckFlag = 0;
            report.PidExamCompany = barcode.PidExamCompany;
            report.SampReachDate = barcode.ReachDate;
            report.RepDangerFlag = false;
            report.PidUniqueId = barcode.PidUniqueId;
            report.RepAuditWay = 0;
            report.PidIdentity = barcode.PidIdentity;
            report.PidIdentityCard = barcode.PidIdentityCard;
            report.PidIdentityName = barcode.PidIdentityName;
            report.PidBirthday = barcode.PidBirthday;
            report.PidAgeExp = new Age_BirthdayToYMDHI_ShowMonthForUnder6().ConvertRule(barcode.PidBirthday?.ToString("yyyy-MM-dd HH:mm:ss"));
            if (!string.IsNullOrEmpty(report.PidAgeExp))
                report.PatAgeTxt = report.PidAgeExp.Replace("Y","岁").Replace("M","月").Replace("D","天").Replace("H","时").Replace("I","分");

            report.HISSerialnum = barcode.SampPackNo;
            report.HISPatientID = barcode.PidPatno;
           
            report.ListPidReportDetail = new List<EntityPidReportDetail>();
            int i = 0;
            foreach (EntitySampDetail detail in barcode.ListSampDetail)
            {
                EntityPidReportDetail reportdet = new EntityPidReportDetail();
                reportdet.RepId = report.RepId;
                reportdet.ComId = detail.ComId;
                reportdet.OrderCode = detail.OrderCode;
                reportdet.OrderPrice = detail.OrderPrice.ToString();
                reportdet.OrderSn = detail.OrderSn;
                reportdet.PatComName = detail.ComName;
                reportdet.ApplyID = detail.ApplyID;
                reportdet.SortNo = i;
                report.ListPidReportDetail.Add(reportdet);
                i += 1;
            }

            return report;
        }
        #endregion
    }
}
