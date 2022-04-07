using dcl.entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{

    /// <summary>
    /// 骨髓模块服务接口
    /// </summary>
    [ServiceContract]
    public interface IMarrowEnter
    {
        /// <summary>
        /// 获取骨髓检验明细信息
        /// </summary>
        /// <param name="report_id">主报告ID</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResult> GetMarrowdetailList(string report_id);


        /// <summary>
        /// 病人信息查询
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> MarrowPatientQuery(EntityPatientQC patientCondition);


        ///// <summary>
        ///// 获取病人资料（病人基本信息、病人检验组合、病人药敏细菌描述结果）
        ///// </summary>
        ///// <param name="patId">病人ID</param>
        ///// <returns></returns>
        //[OperationContract]
        //EntityQcResultList GetMicPatinentData(string patId);


        /// <summary>
        /// 保存细菌病人、结果信息
        /// </summary>
        /// <param name="caller">操作人</param>
        /// <param name="report">主报告记录</param>
        /// <param name="results">结果记录</param>
        /// <param name="image_results">图片结果记录</param>
        /// <returns>true成功，false失败</returns>
        [OperationContract]
        bool UpdateMarrowPatResult(EntityRemoteCallClientInfo userInfo,
            EntityPidReportMain report,
            List<EntityObrResult> results,
            List<EntityObrResultImage> image_results
            );

        /// <summary>
        /// 保存细菌病人、结果信息
        /// </summary>
        /// <param name="caller">操作人</param>
        /// <param name="report">主报告记录</param>
        /// <param name="results">结果记录</param>
        /// <param name="image_results">图片结果记录</param>
        /// <returns>true成功，false失败</returns>
        [OperationContract]
        bool InsertMarrowPatResult(EntityRemoteCallClientInfo caller,
            EntityPidReportMain report,
            List<EntityObrResult> results,
            List<EntityObrResultImage> image_results
            );

        /// <summary>
        /// 删除骨髓报告信息
        /// </summary>
        /// <param name="caller">操作人</param>
        /// <param name="reportList">报告列表</param>
        /// <param name="delRes">是否删除结果</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteMarrowPatResult(EntityRemoteCallClientInfo caller, List<EntityPidReportMain> reportList, bool delRes);


        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="listPatientsID">需要审核的病人ID集合</param>
        /// <returns>审核结果</returns>
        [OperationContract]
        bool MarrowAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        [OperationContract]
        bool MarrowPreAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        [OperationContract]
        bool UndoMarrowPreAudit(List<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 批量反审核
        /// </summary>
        /// <param name="PatientID"></param>
        [OperationContract]
        bool UndoMarrowAudit(List<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 批量报告
        /// </summary>
        /// <param name="listPatientsID">需要审核的病人ID集合</param>
        /// <returns>审核结果</returns>
        [OperationContract]
        bool MarrowReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 批量审核前检查 1：审核 2：报告
        /// </summary>
        /// <param name="listPatientsID">需要审核的病人ID集合</param>
        /// <returns>审核结果</returns>
        [OperationContract]
        EntityOperationResultList BatchAuditCheck(IEnumerable<string> list_pat_id, string isAudit);

        /// <summary>
        /// 批量取消报告前检查
        /// </summary>
        /// <param name="list_pat_id"></param>
        /// <param name="oper_type">预留-暂未用</param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList BatchUndoReportCheck(IEnumerable<string> list_pat_id, string oper_type);
        /// <summary>
        /// 批量取消报告
        /// </summary>
        /// <param name="listPatientsID"></param>
        [OperationContract]
        bool UndoMarrowReport(List<string> listPatientsID, EntityRemoteCallClientInfo caller);


        ///// <summary>
        ///// 获取组合相关的无菌与涂片
        ///// </summary>
        ///// <param name="strComIDs"></param>
        ///// <returns></returns>
        //[OperationContract]
        //List<EntityDicMicSmear> GetDicMarrowSmearByComID(string strComIDs);


        /// <summary>
        /// 根据当前仪器和样本号、年份获取满足条件的日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="itr_id">仪器ID</param>
        /// <param name="currentSID">当前样本号</param>
        /// <returns></returns>
        [OperationContract]
        string GetPatDate_ByItrSID(DateTime date, string itr_id, string currentSID);


    }
}
