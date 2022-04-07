using dcl.entity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IObrResult
    {

        /// <summary>
        /// 新增检验结果信息
        /// </summary>
        /// <param name="ObrResult"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertObrResult(EntityObrResult ObrResult);

        /// <summary>
        /// 更新检验结果信息
        /// </summary>
        /// <param name="ObrResult"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateObrResult(EntityObrResult ObrResult);

        /// <summary>
        /// 检验结果查询
        /// </summary>
        /// <param name="resultQc"></param>
        /// <param name="withHistoryResult"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> ObrResultQuery(EntityResultQC resultQc, bool withHistoryResult = false);


        /// <summary>
        /// 更新普通报告 、病人信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResult UpdatePatCommonResult(EntityRemoteCallClientInfo userInfo, EntityQcResultList resultList);

        /// <summary>
        /// 保存病人、普通结果信息
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResult InsertPatCommonResult(EntityRemoteCallClientInfo caller, EntityQcResultList resultList);

        /// <summary>
        /// 删除普通病人资料
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="rep_id"></param>
        /// <param name="delWithResult"></param>
        /// <param name="canDeleteAudited"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResult DelPatCommonResult(EntityRemoteCallClientInfo caller, string rep_id, bool delWithResult, bool canDeleteAudited);

        /// <summary>
        /// 查询检验结果表数据(中间不做其他处理)
        /// </summary>
        /// <param name="resultQc"></param>
        /// <param name="withHistoryResult"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetObrResultQuery(EntityResultQC resultQc, bool withHistoryResult = false);


        /// <summary>
        ///  检验结果查询（根据项目编码等条件）
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> LisResultQuery(EntityResultQC resultQc);

        /// <summary>
        /// 获取缺省值结果
        /// </summary>
        /// <param name="itrid">仪器id</param>
        /// <param name="comID">组合id</param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetCombineDefData(string itrid, string comID);

        /// <summary>
        /// 根据实验序号保存实验结果
        /// </summary>
        /// <param name="itrid">仪器id</param>
        /// <param name="comID">组合id</param>
        /// <returns></returns>
        [OperationContract]
        bool SaveobrresultbyTestSeq(List<EntityObrResultTestSeqVer> qcs,out string ErrorMsg);

        /// <summary>
        /// 匹配仪器传输的源结果，并且保存
        /// </summary>
        /// <param name="pat"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveOrignObrResult(EntityPidReportMain pat);
    }
}
