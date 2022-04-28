using dcl.common;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Linq;
using dcl.dao.interfaces;
using dcl.svr.interfaces;
using System.Data;
using dcl.svr.dicbasic;
using dcl.svr.cache;
using dcl.dao.core;
using System.Configuration;

namespace dcl.svr.sample
{
    /// <summary>
    /// 查询条码信息
    /// </summary>
    public class SampMainBIZ : DclBizBase, ISampMain
    {
        #region 查询

        /// <summary>
        /// 查询条码信息
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        public List<EntitySampMain> SampMainQuery(EntitySampQC sampCondition)
        {
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();

            List<EntitySampMain> listSampMain = new List<EntitySampMain>();
            if (dao != null)
            {
                sampCondition.HospitalId = ConfigurationManager.AppSettings["HospitalId"];
                dao.Dbm = Dbm;
                listSampMain = dao.GetSampMain(sampCondition);
            }
            return listSampMain;
        }


        /// <summary>
        /// 简单统计条码工作量
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        public List<EntitySampMain> SimpleStatisticSamp(EntitySampQC sampCondition)
        {
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            List<EntitySampMain> listSampMain = new List<EntitySampMain>();
            if (dao != null)
            {
                dao.Dbm = Dbm;
                listSampMain = dao.SimpleStatisticSamp(sampCondition);
            }
            return listSampMain;
        }

        /// <summary>
        /// 根据条码号查询条码信息
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public EntitySampMain SampMainQueryByBarId(string sampBarId)
        {
            EntitySampMain sampMain = new EntitySampMain();

            EntitySampQC sampQC = new EntitySampQC();
            sampQC.ListSampBarId.Add(sampBarId);
            sampQC.SearchDeleteSampMain = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                dao.Dbm = Dbm;
                List<EntitySampMain> listSampMain = dao.GetSampMain(sampQC);

                if (listSampMain.Count > 0)
                {
                    sampMain = listSampMain[0];

                    sampMain.ListSampDetail = new SampDetailBIZ().GetSampDetail(sampBarId);

                    sampMain.ListSampProcessDetail = new SampProcessDetailBIZ().GetSampProcessDetail(sampBarId);
                }
            }
            return sampMain;
        }

        /// <summary>
        /// 根据病人标识或者诊疗卡号获取病人信息（手工条码）
        /// </summary>
        /// <param name="patInNo"></param>
        /// <returns></returns>
        public List<EntitySampMain> GetPatientsInfoByBcInNo(string patInNo)
        {
            List<EntitySampMain> list = new List<EntitySampMain>();
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            //手工条码病人编号查询唯一字段
            string colName = CacheSysConfig.Current.GetSystemConfig("BCManual_select_colName");
            if (dao != null)
            {
                list = dao.GetPatientsInfoByBcInNo(patInNo, colName);
            }
            return list;
        }

        /// <summary>
        /// 是否回退
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public bool Returned(string barCode)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.Returned(barCode);
            }

            return result;
        }

        /// <summary>
        /// 判断是否存在该标本
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean ExistSampMain(String sampBarId)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.ExistSampMain(sampBarId);
            }

            return result;
        }

        #endregion

        #region 更新

        /// <summary>
        /// 更新条码状态，根据条码号
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="strBarId"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainStatusByBarId(EntitySampOperation operation, string strBarId)
        {
            bool result = false;

            EntitySampQC sampQC = new EntitySampQC();
            sampQC.ListSampBarId.Add(strBarId);

            List<EntitySampMain> listSampMain = SampMainQuery(sampQC);

            if (listSampMain.Count > 0)
            {
                result = UpdateSampMainStatus(operation, listSampMain);
            }

            return result;
        }

        /// <summary>
        /// 更新条码批次
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainBatchNo(Int64 batchNo, List<EntitySampMain> listSampMain)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampMainBatchNo(batchNo, listSampMain);
            }

            return result;
        }

        /// <summary>
        /// 更新加急标志
        /// </summary>
        /// <param name="urgent"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainUrgentFlag(bool urgent, String sampBarId)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampMainUrgentFlag(urgent, sampBarId);
            }

            return result;
        }

        /// <summary>
        /// 更新标本备注
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="remarkName"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainRemark(String remarkId, String remarkName, String sampBarId)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampMainRemark(remarkId, remarkName, sampBarId);
            }

            return result;
        }

        /// <summary>
        /// 更新标本信息
        /// </summary>
        /// <param name="sampleId"></param>
        /// <param name="sampleName"></param>
        /// <param name="listSampMain"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainSampleInfo(String sampleId, String sampleName, List<EntitySampMain> listSampMain, EntitySampOperation operation)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();

            if (dao != null)
            {
                foreach (EntitySampMain sampMain in listSampMain)
                {
                    string strSourceSamId = string.Empty;
                    string strSourceSamName = string.Empty;
                    if (sampMain.SampSamId == sampleId)
                    {
                        EntitySampMain sam = dao.GetSampMainSampleInfo(sampMain.SampBarId);
                        if (sam != null)
                        {
                            strSourceSamId = sam.SampSamId;
                            strSourceSamName = sam.SampSamName;
                        }
                    }
                    else
                    {
                        strSourceSamId = sampMain.SampSamId;
                        strSourceSamName = sampMain.SampSamName;
                    }

                    if (dao.UpdateSampMainSampleInfo(sampleId, sampleName, sampMain.SampBarId))
                    {
                        string strRemark = string.Format("标本ID:{0}-->{1},标本名称：{2}-->{3}", strSourceSamId, sampleId, strSourceSamName, sampleName);


                        SampProcessDetailBIZ processBiz = new SampProcessDetailBIZ();

                        EntitySampProcessDetail sampProcessDetial = new EntitySampProcessDetail();

                        sampProcessDetial.ProcDate = operation.OperationTime;
                        sampProcessDetial.ProcUsercode = operation.OperationID;
                        sampProcessDetial.ProcUsername = operation.OperationName;
                        sampProcessDetial.ProcStatus = operation.OperationStatus;
                        sampProcessDetial.ProcBarno = sampMain.SampBarId;
                        sampProcessDetial.ProcBarcode = sampMain.SampBarId;
                        sampProcessDetial.ProcPlace = operation.OperationPlace;
                        sampProcessDetial.ProcTimes = 1;
                        sampProcessDetial.ProcContent = strRemark;

                        result = processBiz.SaveSampProcessDetailWithoutInterface(sampProcessDetial);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 更新条码姓名和性别
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainNameAndSex(String name, String sex, String sampBarId)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampMainNameAndSex(name, sex, sampBarId);
            }

            return result;
        }

        /// <summary>
        /// 更新条码信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainOtherInfo(EntitySampMain sampMain, String sampBarId)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampMainOtherInfo(sampMain, sampBarId);
            }

            return result;
        }

        /// <summary>
        /// 更新条码绑定的总码信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainYHSBarCode(EntitySampMain sampMain, String sampBarId)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampMainYHSBarCode(sampMain, sampBarId);
            }

            return result;
        }

        /// <summary>
        /// 更新条码状态
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainStatus(EntitySampOperation operation, List<EntitySampMain> listSampMain)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                dao.Dbm = Dbm;
                if (dao.UpdateSampMainStatus(operation, listSampMain))
                {
                    SampProcessDetailBIZ processBiz = new SampProcessDetailBIZ();
                    processBiz.Dbm = Dbm;
                    foreach (EntitySampMain sampMain in listSampMain)
                    {
                        result = processBiz.SaveSampProcessDetail(operation, sampMain);
                    }
                }

            }

            return result;
        }

        /// <summary>
        /// 更新预置条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <param name="sampPlaceCode"></param>
        /// <returns></returns>
        public Boolean UpdateSampMainBarCode(String sampBarId, String sampPlaceCode)
        {
            bool result = false;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampMainBarCode(sampBarId, sampPlaceCode);
            }

            return result;
        }

        /// <summary>
        /// 重置预置条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public String UndoSampMain(EntitySampOperation operation, EntitySampMain sampMain)
        {
            string result = string.Empty;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                SampMainDownloadBIZ downloadBiz = new SampMainDownloadBIZ();

                //插入操作记录
                SampProcessDetailBIZ processBiz = new SampProcessDetailBIZ();
                processBiz.SaveSampProcessDetail(operation, sampMain);

                string strNewBarCode = string.Empty;
                //新冠样本条码为14位
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Covid_14BitBarcodeLength") == "是" &&
                    (sampMain.SampComName.Contains("新冠") || sampMain.SampComName.Contains("新型冠状")))
                {
                    strNewBarCode = new SampMainDownloadBIZ().CreateCOVIDBarcodeNumber();
                }
                else
                {
                    strNewBarCode = new SampMainDownloadBIZ().CreateBarcodeNumber();
                }

                if (dao.UpdateSampMainBarCode(sampMain.SampBarId, strNewBarCode))
                {
                    dao.UndoSampMain(strNewBarCode);

                    result = strNewBarCode;
                }
            }

            return result;
        }

        /// <summary>
        /// 更新回退条码回退标志
        /// </summary>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        public bool UpdateSampReturnFlag(EntitySampMain sampMain)
        {
            bool result = false;
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateSampReturnFlag(sampMain);
            }
            return result;
        }
        /// <summary>
        /// 更新标本包号
        /// </summary>
        /// <param name="sampInfo">包号</param>
        /// <param name="listSampBarCode">条码号</param>
        /// <returns></returns>
        public bool UpdateBarcodeBale(string pidUniqueId, List<string> listSampBarCode)
        {
            bool result = false;
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.UpdateBarcodeBale(pidUniqueId, listSampBarCode);
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        public bool CreateSampMain(List<EntitySampMain> listSampMain)
        {
            bool result = false;
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                result = dao.CreateSampMain(listSampMain);
            }
            return result;
        }

        /// <summary>
        /// 手工生成条码
        /// </summary>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        public List<String> ManualCreateSampMain(EntitySampMain sampMain)
        {
            List<string> listResult = new List<string>();


            //获取拆分合并信息
            ItemCombineBarcodeBIZ comBarBiz = new ItemCombineBarcodeBIZ();

            //需要合并的规则
            List<EntitySampMergeRule> listRule = new List<EntitySampMergeRule>();

            string strOriId = sampMain.PidSrcId;

            //无拆分信息的项目
            List<EntitySampDetail> listSplitDetail = new List<EntitySampDetail>();

            //需要操作的项目集合
            List<EntitySampDetail> listDetailTotal = new List<EntitySampDetail>();

            foreach (EntitySampDetail sampDetail in sampMain.ListSampDetail)
            {
                List<string> listLisCode = new List<string>();
                listLisCode.Add(sampDetail.ComId);

                List<EntitySampMergeRule> listSampRule = comBarBiz.GetSampMergeRuleByLisCode(listLisCode, sampMain.PidSrcId).OrderByDescending(w => w.ComSrcId).ToList();

                if (listSampRule.Count > 0)
                {
                    EntitySampMergeRule rule = listSampRule[0];

                    if (rule.ComSplitFlag != null && rule.ComSplitFlag == 1)
                    {
                        List<EntitySampMergeRule> listSMRule = comBarBiz.GetSampMergeRuleRuleBySplitComId(sampDetail.ComId, strOriId);

                        if (listSMRule.Count > 0)
                        {
                            //拆分条码组合只插入一次
                            List<string> listComId = new List<string>();
                            foreach (EntitySampMergeRule sampRule in listSMRule)
                            {
                                if (!listComId.Contains(sampRule.ComId))
                                {
                                    EntitySampDetail detail = new EntitySampDetail();

                                    detail.ComId = sampRule.ComId;
                                    detail.ComName = sampRule.ComName;
                                    detail.OrderName = sampRule.ComName;
                                    detail.SampType = sampDetail.SampType;
                                    detail.OrderDate = sampDetail.OrderDate;
                                    detail.OrderOccDate = sampDetail.OrderOccDate;

                                    listDetailTotal.Add(detail);

                                    //插入到总的拆分信息中
                                    if (listRule.FindIndex(w => w.ComRulId == sampRule.ComRulId) < 0)
                                        listRule.Add((EntitySampMergeRule)sampRule.Clone());
                                }
                            }
                        }
                        else
                        {
                            listDetailTotal.Add(sampDetail);
                            listRule.Add(rule);
                        }
                    }
                    else
                    {
                        listDetailTotal.Add(sampDetail);
                        listRule.Add(rule);
                    }
                }
                else
                {
                    listSplitDetail.Add(sampDetail);
                }
            }

            SampMainDownloadBIZ downloadBiz = new SampMainDownloadBIZ();
            List<EntitySampMain> listSampMain = new List<EntitySampMain>();


            if (listRule.Count > 0)
            {
                List<string> listSplitCode = new List<string>();

                foreach (EntitySampMergeRule item in listRule)
                {
                    if (!listSplitCode.Contains(item.ComSplitCode))
                    {
                        listSplitCode.Add(item.ComSplitCode);
                    }
                }

                foreach (string splitCode in listSplitCode)
                {
                    EntitySampMain sm = EntityManager<EntitySampMain>.EntityClone(sampMain);
                    sm.PidOrgId = ConfigurationManager.AppSettings["HospitalId"];
                    sm.ListSampDetail.Clear();

                    string strBarCode = string.Empty;
                    //新冠样本条码为14位
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Covid_14BitBarcodeLength") == "是" &&
                        (sampMain.SampComName.Contains("新冠") || sampMain.SampComName.Contains("新型冠状")))
                    {
                        strBarCode = new SampMainDownloadBIZ().CreateCOVIDBarcodeNumber();
                    }
                    else
                    {
                        strBarCode = new SampMainDownloadBIZ().CreateBarcodeNumber();
                    }

                    if (sm.SampBarType == 1)
                    {
                        //sm.SampBarCode = "";
                    }
                    else
                    {
                        sm.SampBarCode = strBarCode;
                        sm.SampBarId = strBarCode;
                    }

                    List<EntitySampMergeRule> listComRule = (from score in listRule
                                                             where score.ComSplitCode == splitCode
                                                             select score).ToList();

                    List<EntitySampDetail> detail = (from score in listDetailTotal
                                                     where listComRule.FindIndex(w => w.ComId == score.ComId) >= 0
                                                     select score).ToList();

                    sm.SampComName = string.Empty;
                    foreach (var item in detail)
                    {
                        EntitySampMergeRule rule = listComRule.Find(w => w.ComId == item.ComId);
                        item.SaveNotice = rule.ComSaveNotice;
                        item.BloodNotice = rule.ComSamNotice;

                        item.SampBarId = strBarCode;
                        item.SampBarCode = strBarCode;
                        sm.SampComName += string.Format("+{0}", item.ComName);
                    }

                    sm.SampSamId = listComRule[0].ComSamId;

                    sm.SampType = listComRule[0].ComExecCode;
                    sm.SampTubCode = listComRule[0].ComTubeCode;
                    sm.SampSendDest = listComRule[0].ComTestDest;
                    sm.SampRemId = listComRule[0].ComSamRemark;
                    sm.SampPrintTime = listComRule[0].ComPrintCount;

                    sm.SampComName = sm.SampComName.Remove(0, 1);

                    sm.ListSampDetail.AddRange(detail.ToArray());

                    foreach (var item in sm.ListSampProcessDetail)
                    {
                        item.ProcBarcode = strBarCode;
                        item.ProcBarno = strBarCode;
                    }

                    listSampMain.Add(sm);
                    listResult.Add(strBarCode);
                }
            }

            foreach (var item in listSplitDetail)
            {
                EntitySampMain sm = EntityManager<EntitySampMain>.EntityClone(sampMain);// (EntitySampMain)sampMain.Clone();
                sm.ListSampDetail.Clear();

                string strBarCode = string.Empty;
                //新冠样本条码为14位
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Covid_14BitBarcodeLength") == "是" &&
                    (sampMain.SampComName.Contains("新冠") || sampMain.SampComName.Contains("新型冠状")))
                {
                    strBarCode = new SampMainDownloadBIZ().CreateCOVIDBarcodeNumber();
                }
                else
                {
                    strBarCode = new SampMainDownloadBIZ().CreateBarcodeNumber();
                }

                sm.PidOrgId = ConfigurationManager.AppSettings["HospitalId"];
                sm.SampComName = item.ComName;

                sm.SampBarCode = strBarCode;
                sm.SampBarId = strBarCode;

                item.SampBarId = strBarCode;
                item.SampBarCode = strBarCode;

                sm.ListSampDetail.Add(item);

                foreach (var process in sm.ListSampProcessDetail)
                {
                    process.ProcBarcode = strBarCode;
                    process.ProcBarno = strBarCode;
                }


                listSampMain.Add(sm);
                listResult.Add(strBarCode);
            }


            if (!this.CreateSampMain(listSampMain))
                listResult.Clear();

            return listResult;
        }

        /// <summary>
        /// 删除条码
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        public Boolean DeleteSampMain(EntitySampOperation operation, EntitySampMain sampMain)
        {
            bool result = false;
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();

            if (dao != null)
            {
                DBManager helper = new DBManager();
                helper.BeginTrans();
                try
                {
                    dao.Dbm = helper;
                    dao.DeleteSampMain(sampMain.SampBarId);

                    SampProcessDetailBIZ processBiz = new SampProcessDetailBIZ();
                    processBiz.Dbm = helper;
                    processBiz.SaveSampProcessDetail(operation, sampMain);

                    SampDetailBIZ detailBiz = new SampDetailBIZ();
                    detailBiz.Dbm = helper;
                    detailBiz.DeleteSampDetailAll(sampMain.SampBarId);

                    helper.CommitTrans();
                    helper = null;
                    result = true;

                    new SampReturnBIZ().HandleSampReturn(sampMain.SampBarId);

                }
                catch (Exception ex)
                {
                    helper.RollbackTrans();
                    helper = null;
                    Lib.LogManager.Logger.LogException(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 条码拆分
        /// </summary>
        /// <param name="sampMain"></param>
        /// <param name="listSeperateDetail"></param>
        /// <returns></returns>
        public List<EntitySampMain> SeperateSampMain(EntitySampMain sampMain, List<EntitySampDetail> listSeperateDetail)
        {
            List<EntitySampMain> result = new List<EntitySampMain>();

            List<EntitySampDetail> listDeleteDetail = EntityManager<EntitySampDetail>.ListClone(listSeperateDetail);

            string strBarCode = string.Empty;
            //新冠样本条码为14位
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Covid_14BitBarcodeLength") == "是" &&
                (sampMain.SampComName.Contains("新冠") || sampMain.SampComName.Contains("新型冠状")))
            {
                strBarCode = new SampMainDownloadBIZ().CreateCOVIDBarcodeNumber();
            }
            else
            {
                strBarCode = new SampMainDownloadBIZ().CreateBarcodeNumber();
            }


            EntitySampMain sampMainNew = EntityManager<EntitySampMain>.EntityClone(sampMain);
            sampMainNew.SampBarId = strBarCode;
            sampMainNew.SampBarType = sampMain.SampBarType;
            //拆分的条码是否为预制条码 是预制条码则无条码号
            if (sampMainNew.SampBarType == 0)
            {
                sampMainNew.SampBarCode = strBarCode;
            }
            else
            {
                sampMainNew.SampBarCode = "";
            }
            string strCName = string.Empty;
            foreach (EntitySampDetail item in listSeperateDetail)
            {
                strCName += "+" + item.ComName;
                item.SampBarId = strBarCode;
                if (sampMainNew.SampBarType == 0)
                {
                    item.SampBarCode = strBarCode;
                }
                else
                {
                    item.SampBarCode = "";
                }
            }

            strCName = strCName.Remove(0, 1);
            sampMainNew.SampComName = strCName;

            sampMainNew.ListSampDetail = listSeperateDetail;

            foreach (var item in sampMainNew.ListSampProcessDetail)
            {
                if (sampMainNew.SampBarType == 0)
                {
                    item.ProcBarcode = strBarCode;
                }
                else
                {
                    item.ProcBarcode = "";
                }
                item.ProcBarno = strBarCode;
                item.ProcContent = "拆分生成新条码";
            }

            List<EntitySampMain> listCreate = new List<EntitySampMain>();
            listCreate.Add(sampMainNew);

            if (CreateSampMain(listCreate))
            {
                new SampDetailBIZ().DeleteSampDetail(listDeleteDetail);

                string strCNameNew = string.Empty;

                foreach (EntitySampDetail item in sampMain.ListSampDetail)
                {
                    if (listSeperateDetail.FindIndex(w => w.DetSn == item.DetSn) < 0)
                    {
                        strCNameNew += "+" + item.ComName;
                    }
                }

                strCNameNew = strCNameNew.Remove(0, 1);

                IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
                if (dao != null)
                {
                    dao.UpdateSampMainComName(strCNameNew, sampMain.SampBarId);
                }
                sampMain.SampComName = strCNameNew;
                //samp_sn不会更新  需要再次查询
                EntitySampMain sampMainNewTemp = SampMainQueryByBarId(sampMainNew.SampBarId);
                EntitySampMain sampMainTemp = SampMainQueryByBarId(sampMain.SampBarId);
                result.Add(sampMainNewTemp);
                result.Add(sampMainTemp);
            }
            return result;
        }

        /// <summary>
        /// 条码合并
        /// </summary>
        /// <param name="listSampBarId"></param>
        public bool MergeSampMain(List<String> listSampBarId)
        {
            bool result = false;
            if (listSampBarId.Count > 1)
            {
                IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();

                if (dao != null)
                {
                    List<EntitySampDetail> listSampDetail = new List<EntitySampDetail>();
                    List<EntitySampMain> listSampMain = new List<EntitySampMain>();
                    SampDetailBIZ detailBiz = new SampDetailBIZ();
                    for (int i = 0; i < listSampBarId.Count; i++)
                    {
                        listSampMain.Add(SampMainQueryByBarId(listSampBarId[i]));
                        if (i > 0)
                        {
                            listSampDetail.AddRange(detailBiz.GetSampDetail(listSampBarId[i]));
                            dao.DeleteSampMain(listSampBarId[i]);
                            detailBiz.DeleteSampDetailAll(listSampBarId[i]);
                        }
                    }
                    string strCNameNew = string.Empty;
                    foreach (EntitySampDetail sampDetail in listSampDetail)
                    {
                        sampDetail.ComRemark = string.Format("由条码：{0}合并生成", sampDetail.SampBarCode);
                        sampDetail.SampBarCode = listSampBarId[0];
                        sampDetail.SampBarId = listSampBarId[0];
                    }
                    foreach (EntitySampMain sampMain in listSampMain)
                    {
                        strCNameNew += "+" + sampMain.SampComName;
                    }
                    strCNameNew = strCNameNew.Remove(0, 1);
                    if (dao != null)
                    {
                        dao.UpdateSampMainComName(strCNameNew, listSampBarId[0]);//更新条码的组合名称
                    }
                    result = detailBiz.SaveSampDetail(listSampDetail);
                }
            }
            return result;
        }

        /// <summary>
        /// 流程确认前检查
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        public string ConfirmBeforeCheck(EntitySampOperation operation, EntitySampMain sampMain)
        {
            //手工条码不执行接口
            if (!string.IsNullOrEmpty(sampMain.SampInfo) && sampMain.SampInfo == "122")
            {
                return string.Empty;
            }
            else
            {
                if (sampMain.ListSampDetail.Count == 0)
                    sampMain.ListSampDetail = new SampDetailBIZ().GetSampDetail(sampMain.SampBarId);

                return DCLExtInterfaceFactory.DCLExtInterface.ExecuteInterfaceBefore(operation, sampMain);
            }
        }

        /// <summary>
        /// 取消预制条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        public String CancelUndoSampMain(string sampBarId)
        {
            String result = string.Empty;

            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                if (dao.UpdateSampMainBarCode(sampBarId, sampBarId))
                {
                    UpdateSampBarType(sampBarId, 0);//更新条码类型
                    result = sampBarId;
                }
            }
            return result;
        }

        /// <summary>
        /// 根据内部关联ID更新条码类型(0-打印条码 1-预制条码)
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <param name="samp_bar_type"></param>
        /// <returns></returns>
        public bool UpdateSampBarType(string sampBarId, int samp_bar_type)
        {
            bool isUpdate = false;
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                isUpdate = dao.UpdateSampBarType(sampBarId, samp_bar_type);
            }
            return isUpdate;
        }

        /// <summary>
        /// 获取包数
        /// </summary>
        /// <param name="sampStatus"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public List<string> GetPackCount(string sampStatus, string deptCode)
        {
            List < string >list = new List<string>();
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                list = dao.GetPackCount(sampStatus, deptCode);
            }
            return list;
        }

        public List<EntityPidReportMain> MZImportReport(EntityInterfaceExtParameter Parameter)
        {
            return DCLExtInterfaceFactory.DCLExtInterface.GetMzPatients(Parameter);
        }

        public List<EntitySampMain> GetFaultChargeSamp(EntitySampQC qc)
        {
            List<EntitySampMain> list = new List<EntitySampMain>();
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                list = dao.GetFaultChargeSamp(qc);
            }
            return list;
        }


        /// <summary>
        /// 调用粤核酸查询接口获取采样人员信息
        /// </summary>
        public List<EntitySampMain> GetSampleInfo(List<string> listPatId)
        {
            List<EntitySampMain> entitySampMainsList = DCLExtInterfaceFactory.DCLExtInterface.GetYssPatientInfo(listPatId);
            return entitySampMainsList;
        }

        
    }
}
