using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoMergeResult
    {
        /// <summary>
        /// 获取合并结果
        /// </summary>
        /// <param name="qc"></param>
        List<EntityObrResult> GetMergeResult(EntityMergeResultQC qc);

        /// <summary>
        /// 获取待合并源结果数据
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<EntityObrResult> GetSourceResult(EntityMergeResultQC qc);

        /// <summary>
        /// 获取合并结果的病人信息
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetMergePatients(EntityMergeResultQC qc);
    }
}
