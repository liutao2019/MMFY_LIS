using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using dcl.common;
using dcl.servececontract;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.svr.cache;
using dcl.svr.users;

namespace dcl.svr.statistical
{
    public class GeneralStatisticsBIZ : IStatistical
    {
        #region ICommonBIZ 成员

        public EntityResponse GetStatCache()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            EntityResponse respone = new EntityResponse();
            try
            {
                CacheDataBIZ cacheDao = new CacheDataBIZ();
                //新增：将实验组名称查询出来
                respone = cacheDao.GetCacheData("EntityDicInstrument");
                List<EntityDicInstrument> dtInst = respone.GetResult() as List<EntityDicInstrument>;
                dict.Add("dtInst", dtInst);
                respone = cacheDao.GetCacheData("EntityDicReaSupplier");
                List<EntityDicReaSupplier> dtReaSup = respone.GetResult() as List<EntityDicReaSupplier>;
                dict.Add("dtReaSup", dtReaSup);
                respone = cacheDao.GetCacheData("EntityReaSetting");
                List<EntityReaSetting> dtReagent = respone.GetResult() as List<EntityReaSetting>;
                dict.Add("dtReagent", dtReagent);
                respone = cacheDao.GetCacheData("EntityDicReaGroup");
                List<EntityDicReaGroup> dtGroup = respone.GetResult() as List<EntityDicReaGroup>;
                dict.Add("dtGroup", dtGroup);
                respone = cacheDao.GetCacheData("EntityDicReaProduct");
                List<EntityDicReaProduct> dtPdt = respone.GetResult() as List<EntityDicReaProduct>;
                dict.Add("dtPdt", dtPdt);
                respone = cacheDao.GetCacheData("EntityDicPubDept");
                List<EntityDicPubDept> dtDep = respone.GetResult() as List<EntityDicPubDept>;
                dict.Add("dtDep", dtDep);
                //得到诊断表
                respone = cacheDao.GetCacheData("EntityDicPubIcd");
                List<EntityDicPubIcd> dtDiag = respone.GetResult() as List<EntityDicPubIcd>;
                dict.Add("dtDiag", dtDiag);
                //得到标本
                respone = cacheDao.GetCacheData("EntityDicSample");
                List<EntityDicSample> dtSam = respone.GetResult() as List<EntityDicSample>;
                dict.Add("dtSam", dtSam);
                respone = cacheDao.GetCacheData("EntityDicPubProfession");
                List<EntityDicPubProfession> dtPhyType = respone.GetResult() as List<EntityDicPubProfession>;
                dtPhyType = dtPhyType.Where(i => i.ProType == 1).ToList();
                dict.Add("dtPhyType", dtPhyType);
                respone = cacheDao.GetCacheData("EntityDicPubProfession");
                List<EntityDicPubProfession> dtSpeType = respone.GetResult() as List<EntityDicPubProfession>;
                dtSpeType = dtSpeType.Where(i => i.ProType == 0).ToList();
                dict.Add("dtSpeType", dtSpeType);
                //得到结果表结构
                List<EntityObrResult> dtNull = new List<EntityObrResult>();
                dict.Add("dtNull", dtNull);
                //得到组合表
                respone = cacheDao.GetCacheData("EntityDicCombine");
                List<EntityDicCombine> dtCombine = respone.GetResult() as List<EntityDicCombine>;
                dict.Add("dtCombine", dtCombine);
                //得到医生表
                respone = cacheDao.GetCacheData("EntityDicDoctor");
                List<EntityDicDoctor> dtDoc = respone.GetResult() as List<EntityDicDoctor>;
                dict.Add("dtDoc", dtDoc);
                //得到用户表(报告者)
                List<EntitySysUser> dtUs = new SysUserInfoBIZ().GetAllUsers(null);
                dict.Add("dtUs", dtUs);
                //得到用户表(检验者)
                List<EntitySysUser> dtAndit = new SysUserInfoBIZ().GetAllUsers(null);
                dict.Add("dtAndit", dtAndit);
                //得到项目表
                respone = cacheDao.GetCacheData("EntityDicItmItem");
                List<EntityDicItmItem> dtItem = respone.GetResult() as List<EntityDicItmItem>;
                dict.Add("dtItem", dtItem);
                //得到病人来源
                respone = cacheDao.GetCacheData("EntityDicOrigin");
                List<EntityDicOrigin> dtOri = respone.GetResult() as List<EntityDicOrigin>;
                dict.Add("dtOri", dtOri);
                //仪器组合表
                respone = cacheDao.GetCacheData("EntityDicItrCombine");
                List<EntityDicItrCombine> dtInsCom = respone.GetResult() as List<EntityDicItrCombine>;
                dict.Add("dtInsCom", dtInsCom);
                //组合项目表
                respone = cacheDao.GetCacheData("EntityDicItmCombine");
                List<EntityDicItmCombine> dtComItem = respone.GetResult() as List<EntityDicItmCombine>;
                dict.Add("dtComItem", dtComItem);
                //项目标本表
                respone = cacheDao.GetCacheData("EntityDicItemSample");
                List<EntityDicItemSample> dtItemSam = respone.GetResult() as List<EntityDicItemSample>;
                dict.Add("dtItemSam", dtItemSam);
                //偏高偏低提示字典
                respone = cacheDao.GetCacheData("EntityDicResultTips");
                List<EntityDicResultTips> dictResRefFlag = respone.GetResult() as List<EntityDicResultTips>;
                dict.Add("dictResRefFlag", dictResRefFlag);
                //描述评价字典
                respone = cacheDao.GetCacheData("EntityDicPubEvaluate");
                List<EntityDicPubEvaluate> dictUrgent = respone.GetResult() as List<EntityDicPubEvaluate>;
                dict.Add("dictUrgent", dictUrgent);
                respone = cacheDao.GetCacheData("EntityDicMicAntibio");
                List<EntityDicMicAntibio> dictAntibio = respone.GetResult() as List<EntityDicMicAntibio>;
                dict.Add("dictAntibio", dictAntibio);
                respone = cacheDao.GetCacheData("EntityDicMicBacttype");
                List<EntityDicMicBacttype> dictBtype = respone.GetResult() as List<EntityDicMicBacttype>;
                dict.Add("dictBtype", dictBtype);
                respone = cacheDao.GetCacheData("EntityDicMicBacteria");
                List<EntityDicMicBacteria> dictBacteri = respone.GetResult() as List<EntityDicMicBacteria>;
                dict.Add("dictBacteri", dictBacteri);
                //标本状态
                respone = cacheDao.GetCacheData("EntityDicSState");
                List<EntityDicSState> dictSampStatus = respone.GetResult() as List<EntityDicSState>;
                dict.Add("dictSampStatus", dictSampStatus);

                //标本备注
                respone = cacheDao.GetCacheData("EntityDicSampRemark");
                List<EntityDicSampRemark> dictSampRemark = respone.GetResult() as List<EntityDicSampRemark>;
                dict.Add("dictSampRemark", dictSampRemark);

                respone.SetResult(dict);
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString());
                respone.Scusess = false;
                respone.ErroMsg = "获取信息出错!" + ex.ToString();
            }
            return respone;
        }

        public EntityStatisticsQC GetStatQC(List<EntityObrResult> obrResult, EntityStatisticsQC statQC)
        {
            try
            {
                IDaoStatistical dao = DclDaoFactory.DaoHandler<IDaoStatistical>();
                statQC = dao.GetStatQC(obrResult, statQC);
                return statQC;
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString());
                return null;
            }
        }
        public List<EntityTpTemplate> GetReportTemple(EntityTpTemplate temple)
        {
            try
            {
                IDaoStatistical dao = DclDaoFactory.DaoHandler<IDaoStatistical>();
                List<EntityTpTemplate> dtTem = dao.GetTemplate(temple.StName, temple.StType);
                return dtTem;
            }
            catch (Exception ex)
            {
                CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString());
                return null;
            }
        }

        public EntityDCLPrintData GetReportData(EntityStatisticsQC statQC)
        {
            EntityDCLPrintData ds = new EntityDCLPrintData();

            IDaoStatistical dao = DclDaoFactory.DaoHandler<IDaoStatistical>();
            if (dao != null)
                ds = dao.GetReportData(statQC);
            return ds;
        }
        public EntityDCLPrintData GetReagentData(EntityStatisticsQC statQC)
        {
            EntityDCLPrintData ds = new EntityDCLPrintData();

            IDaoStatistical dao = DclDaoFactory.DaoHandler<IDaoStatistical>();
            if (dao != null)
                ds = dao.GetReagentData(statQC);
            return ds;
        }
        #endregion
    }
}
