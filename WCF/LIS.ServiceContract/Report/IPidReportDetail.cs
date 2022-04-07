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
    public interface IPidReportDetail
    {
        /// <summary>
        /// 根据标识ID删除病人明细
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteReportDetail(string repId);

        /// <summary>
        /// 插入病人组合明细
        /// </summary>
        /// <param name="repDetails"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean InsertNewReportDetail(List<EntityPidReportDetail> repDetails);

        /// <summary>
        /// 根据病人标识ID获取病人组合明细
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportDetail> GetPidReportDetailByRepId(string repId);

        /// <summary>
        /// 批量添加组合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [OperationContract]
        bool BatchAddCombine(List<EntityPatientQC> PatientQcList);

        /// <summary>
        /// 根据多个病人标识ID查询病人组合明细数据
        /// </summary>
        /// <param name="mulitRepId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportDetail> SearchPidReportDetailByMulitRepId(string mulitRepId);
    }
}
