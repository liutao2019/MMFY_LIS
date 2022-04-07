using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{

    public interface IDaoCdrHistroyResult
    {
        /// <summary>
        /// 获取cdr的历史结果
        /// </summary>
        /// <param name="listObrId"></param>
        /// <returns></returns>
        List<EntityObrResult> GetCdrHistoryObrResult(List<string> listObrId);

        /// <summary>
        /// 获取cdr的历史病人数据
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetCdrHistroyPatients(EntityPatientQC qc);
    }
}
