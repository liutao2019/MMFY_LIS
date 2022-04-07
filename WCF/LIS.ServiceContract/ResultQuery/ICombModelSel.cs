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
    public interface ICombModelSel
    {
        /// <summary>
        /// 获取病人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetPatientsList(EntityAnanlyseQC query, string dateStart, string dateEnd);

        /// <summary>
        /// 判断病人是否含异常结果
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        bool IsContainOutlier(string repId);

        /// <summary>
        /// 获取病人列表压缩后字节流
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        byte[] GetPatientsListAllBuffer(EntityAnanlyseQC query, string dateStart, string dateEnd);

        /// <summary>
        /// 更新报告状态为已打印
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePatFlagToPrinted(EntityAnanlyseQC query);

        /// <summary>
        /// 获取病人检验结果信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        EntityQcResultList GetPatientResultData(EntityAnanlyseQC query, DateTime dateStart);

        /// <summary>
        /// 获取病人描述报告结果和细菌药敏结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        EntityQcResultList GetCsAndAnResult(string obrId);

        /// <summary>
        /// 获取病人报表信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDCLPrintData GetPatientReportInfo(EntityAnanlyseQC query);

        /// <summary>
        /// 获取病人标识ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable GetPatId(EntityAnanlyseQC query);

        /// <summary>
        /// 保存列表配置
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveSysPara(string code, string patientsSelectViewSortConfigcode);

        /// <summary>
        /// 删除病人信息和结果信息
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="delFlag"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeletePatientInfo(EntityPidReportMain patient, string delFlag);

        /// <summary>
        /// 根据标识ID删除病人
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeletePatient(string repId);

        /// <summary>
        /// 获取病人组合序号
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportDetail> GetCombineSeqForPatID(string repId);

        [OperationContract]
        bool DelPatCommonResult(EntityLogLogin logLogin, EntityPidReportMain patient, bool bDelResult, bool canDeleteAuditedResult);
    }
}
