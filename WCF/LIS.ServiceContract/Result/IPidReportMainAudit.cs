using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Data;

using dcl.entity;

namespace dcl.servececontract
{
    /// <summary>
    /// 审核接口
    /// </summary>
    [ServiceContract]
    public interface IPidReportMainAudit
    {

        /// <summary>
        /// 描述报告审核前检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType">检查方式 0:审核 1:报告 2:</param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList DesctAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller);


        /// <summary>
        /// 审核前检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType">检查方式 0:审核 1:报告 2:</param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList CommonAuditCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller);


        /// <summary>
        /// 取消二审前检查(批)
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="checkType"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList CommonUndoReoprtCheck(IEnumerable<string> listPatientsID, EnumOperationCode checkType, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList Audit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList UndoAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 预报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList PreAuditBatch(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 取消预报告
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList UndoPreAuditBatch(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);


        /// <summary>
        /// 报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList Report(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);


        /// <summary>
        /// 预览等特殊操作更新检验结果的参考值等
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateObrResult(EntityPidReportMain patinfo);

        /// <summary>
        /// 取消报告
        /// </summary>
        /// <param name="listPatID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList UndoReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 将报告单电子签章写入数据库
        /// </summary>
        /// <param name="dtCaSigin"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean InsertReportCASignature(DataTable dtCaSigin);

        /// <summary>
        /// 细菌审核前检查
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="isAudit"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList BacAuditCheck(IEnumerable<string> listPatientsID, string isAudit);

        /// <summary>
        /// 细菌取消报告前检查 
        /// </summary>
        /// <param name="list_pat_id"></param>
        /// <param name="oper_type"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList BatchUndoReportCheck(IEnumerable<string> listPatientsID, string oper_type);

        /// <summary>
        /// 检查该用户是否可以审核
        /// </summary>
        /// <param name="itr_ID"></param>
        /// <param name="auditType"></param>
        /// <param name="loginID"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean CheckCurUserCanAudit(string itr_ID, EnumOperationCode auditType, string loginID);
    }
}
