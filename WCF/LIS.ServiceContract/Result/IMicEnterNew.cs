using dcl.entity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{

    /// <summary>
    /// 细菌管理模块服务接口
    /// </summary>
    [ServiceContract]
    public interface IMicEnterNew
    {
        /// <summary>
        /// 获取药敏明细信息
        /// </summary>
        /// <param name="bacID"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMicAntidetail> GetMicAntidetailList(string bacID);


        /// <summary>
        /// 病人信息查询
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> MicPatientQuery(EntityPatientQC patientCondition);


        /// <summary>
        /// 获取病人资料（病人基本信息、病人检验组合、病人药敏细菌描述结果）
        /// </summary>
        /// <param name="patId">病人ID</param>
        /// <returns></returns>
        [OperationContract]
        EntityQcResultList GetMicPatinentData(string patId);


        /// <summary>
        /// 更新细菌病人、结果信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResult UpdateMicPatResult(EntityRemoteCallClientInfo userInfo, EntityQcResultList resultList);

        /// <summary>
        /// 保存细菌病人、结果信息
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="resultList"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperateResult InsertMicPatResult(EntityRemoteCallClientInfo caller, EntityQcResultList resultList);


        /// <summary>
        /// 删除MIC病人信息
        /// </summary>
        /// <param name="caller"></param>
        /// <param name="patList"></param>
        /// <param name="delRes">是否删除结果</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityOperateResult> DeleteMicPatResult(EntityRemoteCallClientInfo caller, List<EntityPidReportMain> patList,bool delRes);


        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="listPatientsID">需要审核的病人ID集合</param>
        /// <returns>审核结果</returns>
        [OperationContract]
        void MicAudit(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 细菌中期报告(预报告)
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList MicMidReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);


        /// <summary>
        /// 取消细菌中期报告(取消预报告)
        /// </summary>
        /// <param name="listPatientsID"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        [OperationContract]
        EntityOperationResultList UndoMicMidReport(List<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 批量反审核
        /// </summary>
        /// <param name="PatientID"></param>
        [OperationContract]
        EntityOperationResultList UndoMicAudit(List<string> listPatientsID, EntityRemoteCallClientInfo caller);

        /// <summary>
        /// 批量报告
        /// </summary>
        /// <param name="listPatientsID">需要审核的病人ID集合</param>
        /// <returns>审核结果</returns>
        [OperationContract]
        EntityOperationResultList MicReport(IEnumerable<string> listPatientsID, EntityRemoteCallClientInfo caller);


        /// <summary>
        /// 批量取消报告
        /// </summary>
        /// <param name="listPatientsID"></param>
        [OperationContract]
        EntityOperationResultList UndoMicReport(List<string> listPatientsID, EntityRemoteCallClientInfo caller);


        /// <summary>
        /// 获取组合相关的无菌与涂片
        /// </summary>
        /// <param name="strComIDs"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicMicSmear> GetDicMicSmearByComID(string strComIDs);


        /// <summary>
        /// 根据当前仪器和样本号、年份获取满足条件的日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="itr_id">仪器ID</param>
        /// <param name="currentSID">当前样本号</param>
        /// <returns></returns>
        [OperationContract]
        string GetPatDate_ByItrSID(DateTime date, string itr_id, string currentSID);


        /// <summary>
        /// 保存药敏卡
        /// </summary>
        /// <param name="list"></param>
        [OperationContract]
        void SaveMicAntidetailList(List<EntityDicMicAntidetail> list);

        /// <summary>
        /// 细菌仪器结果视窗
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="itrid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityMicViewData> GetMicViewList(DateTime dt, string itrid);

        /// <summary>
        /// 复制历史结果
        /// </summary>
        /// <param name="resultList"></param>
        /// <param name="newRepId"></param>
        /// <param name="repItrId"></param>
        /// <param name="repSid"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean CopyHistroyResult(EntityQcResultList resultList, string newRepId, string repItrId, string repSid);

        /// <summary>
        /// 获取导出药敏结果
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        string GetAntiResult(List<string> repId);
    }
}
