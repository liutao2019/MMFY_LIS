using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ServiceModel;


using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IObrResultMerge
    {
        /// <summary>
        /// 获取仪器未审核纪录的病人列表
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetCurrentItrPatientList(EntityPatientQC patientQc);

        /// <summary>
        /// 获取没有病人资料的结果
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="pat_date"></param>
        /// <param name="onlyGetNonePatInfoResult"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetNonePatInfoResult(EntityResultQC resultQc);


        /// <summary>
        /// 结果合并/复制
        /// </summary>
        /// <param name="listPat"></param>
        /// <param name="isCopy">是否复制</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOperateResult> Merge(List<EntityPidReportMain> listPat,bool isCopy);
    }
}
