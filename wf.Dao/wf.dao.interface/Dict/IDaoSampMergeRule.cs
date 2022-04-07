using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampMergeRule
    {
        bool Save(EntitySampMergeRule sample);

        bool Update(EntitySampMergeRule sample);

        bool Delete(EntitySampMergeRule sample);

        /// <summary>
        /// 根据组合ID查询条码拆分信息
        /// </summary>
        /// <param name="comId"></param>
        /// <returns></returns>
        List<EntitySampMergeRule> Search(String comId);

        /// <summary>
        /// 根据hiscode查询条码拆分信息
        /// </summary>
        /// <param name="listHisFeeCode"></param>
        /// <returns></returns>
        List<EntitySampMergeRule> SearchRuleByHisCode(List<string> listHisFeeCode, String strOriId);

        /// <summary>
        /// 根据liscode查询条码拆分信息
        /// </summary>
        /// <param name="listHisFeeCode"></param>
        /// <returns></returns>
        List<EntitySampMergeRule> SearchRuleByLisCode(List<string> listLisFeeCode, String strOriId);

        /// <summary>
        /// 根据大小组合码查询条码查分信息
        /// </summary>
        /// <param name="strComId"></param>
        /// <returns></returns>
        List<EntitySampMergeRule> SearchRuleBySplitComId(String strComId, String strOriId);

        /// <summary>
        /// 获取所有组合的拆分条码信息
        /// </summary>
        /// <returns></returns>
        List<EntitySampMergeRule> GetAllCombineSplitBarCode();

 

    }
}
