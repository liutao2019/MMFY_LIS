using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IMergeResult
    {
        /// <summary>
        /// 获取合并结果
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetMergeResult(EntityMergeResultQC qc);

        /// <summary>
        /// 获取待合并原始结果
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetSourceResult(EntityMergeResultQC qc);

        /// <summary>
        /// 合并结果
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="BarcodeRelation"></param>
        /// <param name="InNoRelation"></param>
        /// <returns></returns>
        [OperationContract]
        string MergeResult(EntityMergeResultQC source, EntityMergeResultQC target, bool BarcodeRelation, bool InNoRelation);
    }
}
