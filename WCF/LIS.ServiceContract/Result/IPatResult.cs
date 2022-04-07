using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IPatResult
    {
        /// <summary>
        /// 插入病人检验结果
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="listResult"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePatientResultByResKey(EntityLogLogin userInfo, EntityObrResult obrResult);

        /// <summary>
        /// 获取项目参考值
        /// </summary>
        /// <param name="itemsID"></param>
        /// <param name="sam_id"></param>
        /// <param name="age_minutes"></param>
        /// <param name="sex"></param>
        /// <param name="sam_rem"></param>
        /// <param name="itm_itr_id"></param>
        /// <param name="pat_depcode"></param>
        /// <param name="patDiag"></param>
        /// <returns></returns>
        [OperationContract]

        List<EntityItmRefInfo> GetItemRefInfo(List<string> itemsID, string sam_id, int age_minutes, string sex, string sam_rem, string itm_itr_id, string pat_depcode, string patDiag);

        /// <summary>
        /// 删除检验项目并添加操作日志记录
        /// </summary>
        /// <param name="logLogin"></param>
        /// <param name="obrSn"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCommonResultItemByObrSn(EntityLogLogin logLogin, string obrSn, string repId);

        /// <summary>
        /// 将结果列顺序保存到系统配置中
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveColumnSort(string sort);

        /// <summary>
        /// 获取病人历史检验结果
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="resultCount"></param>
        /// <param name="obrDate"></param>
        /// <param name="containThisTime">是否包含本次检验结果</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetPatCommonResultHistoryWithRef(string repId, int resultCount, DateTime? obrDate, bool containThisTime = false);

        /// <summary>
        /// 获取病人普通结果
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="withHistoryResult"></param>
        /// <returns></returns>
        [OperationContract]
        EntityQcResultList GetPatientCommonResult(string repId, bool withHistoryResult);

        /// <summary>
        /// 根据HIS收费代码获取组合合并规则
        /// </summary>
        /// <param name="listHisFeeCode"></param>
        /// <returns></returns>
        [OperationContract]

        List<EntitySampMergeRule> GetRuleByHisCode(List<string> listHisFeeCode, string strOriId);

        /// <summary>
        /// 插入条码流转信息
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveSampProcessDetail(EntitySampProcessDetail detail);

        /// <summary>
        /// 获取项目校验和校验明细
        /// </summary>
        /// <param name="itmId"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SearchItmCheckAndDetail(string itmId);
    }
}
