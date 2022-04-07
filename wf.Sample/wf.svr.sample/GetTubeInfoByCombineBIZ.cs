using dcl.servececontract;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.dao.core;

namespace dcl.svr.sample
{
    public class GetTubeInfoByCombineBIZ : DclBizBase, IGetTubeInfoByCombine
    {
        public List<EntityDicTestTube> GetTubes(List<EntityDicCombine> Combines)
        {
            List<EntityDicTestTube> result = new List<EntityDicTestTube>();

            if (Combines == null || Combines.Count == 0)
                return result;

            List<EntitySampMergeRule> listResult = new List<EntitySampMergeRule>();
            List<EntityDicTestTube> Tubes = new List<EntityDicTestTube>();
            List<EntityDicSampDivergeRule> divergeItems = new List<EntityDicSampDivergeRule>();

            IDaoSampMergeRule dao = common.DclDaoFactory.DaoHandler<IDaoSampMergeRule>();
            if (dao != null)
            {
                listResult = dao.GetAllCombineSplitBarCode();
            }

            IDaoDic<EntityDicTestTube> _dao = common.DclDaoFactory.DaoHandler<IDaoDic<EntityDicTestTube>>();
            IDaoDic<EntityDicSampDivergeRule> _divergeDao = common.DclDaoFactory.DaoHandler<IDaoDic<EntityDicSampDivergeRule>>();
            if (_dao != null)
            {
                Tubes = _dao.Search(null);
            }
            List<EntitySampMergeRule> ruleList = new List<EntitySampMergeRule>();
            List<EntitySampMergeRule> splitList = new List<EntitySampMergeRule>();

            foreach (EntityDicCombine combine in Combines)
            {
                EntitySampMergeRule rule = listResult.Find(o => o.ComId == combine.ComId);
                if (rule.ComSplitFlag == 1)
                {
                    var divergeRules = _divergeDao.Search(new List<string> { combine.ComId, "Search" });
                    if (divergeRules != null && divergeRules.Count > 0)
                    {
                        foreach (var diverge in divergeRules)
                        {
                            splitList.Add(listResult.Find(o => o.ComId == diverge.ComSplitId));
                        }
                    }
                    else
                    {
                        Lib.LogManager.Logger.LogInfo("没有找到拆分组合");
                    }
                }
                else
                {
                    if (!ruleList.Exists(o => o.ComSplitCode == rule.ComSplitCode))
                    {
                        ruleList.Add(rule);
                    }
                }
            }

            ruleList.AddRange(splitList);

            foreach (var rule in ruleList)
            {
                var tube = Tubes.Find(o => o.TubCode == rule.ComTubeCode);
                result.Add(tube);
            }
            return result;
        }
    }
}
