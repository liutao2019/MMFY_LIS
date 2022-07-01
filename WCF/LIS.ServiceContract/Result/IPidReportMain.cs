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
    public interface IPidReportMain
    {

        /// <summary>
        /// 根据条码号获取病人资料
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [OperationContract]
        EntityPidReportMain GetPatientsByBarCode(string barCode);

        /// <summary>
        /// 根据条码获取病人资料
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetPatientsBySampleMain(List<EntitySampMain> entitySampMain);

        /// <summary>
        /// 更新病人资料，适用于更新多个字段内容
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePatientData(List<EntityPidReportMain> patient);


        /// <summary>
        /// 保存病人资料和组合
        /// </summary>
        /// <param name="caller">操作记录</param>
        /// <param name="listDict">病人信息和明细集合</param>
        /// <returns></returns>
        [OperationContract]
        EntityOperateResult SavePatient(EntityRemoteCallClientInfo caller, EntityPidReportMain patient);

        /// <summary>
        /// 获取病人列表(简要信息)
        /// 查找指定组别的所有病人资料时,把itr_id赋空字串
        /// </summary>
        /// <param name="dtFrom">起始日期</param>
        /// <param name="dtTo">结束日期</param>
        /// <param name="type_id">物理组ID</param>
        /// <param name="itr_id">仪器ID</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetPatientsList_Resume(EntityRemoteCallClientInfo caller, DateTime dtFrom, DateTime dtTo, string type_id, string itr_id, bool allowEmptyType, bool allowEmptyItr);

        /// <summary>
        /// 病人信息查询
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> PatientQuery(EntityPatientQC patientCondition);

        /// <summary>
        /// 根据PatId获取标本信息
        /// </summary>
        /// <param name="strPatId"></param>
        /// <param name="withPidRepDetail">是否含有组合信息</param>
        /// <returns></returns>
        [OperationContract]
        EntityPidReportMain GetPatientByPatId(string strPatId, bool withPidRepDetail);

        /// <summary>
        /// 线程加载病人结果状态
        /// </summary>
        /// <param name="date"></param>
        /// <param name="pat_itr_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetPatientStatus(DateTime startDate, DateTime endDate ,string pat_itr_id);

        /// <summary>
        /// 线程加载病人新冠结果上传状态
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string, string> GetPatientYhsStatus(DateTime startDate, DateTime endDate);


        /// <summary>
        /// 获取病人资料状态
        /// </summary>
        /// <param name="repid"></param>
        /// <returns></returns>
        [OperationContract]
        string GetPatientState(string repId);

        /// <summary>
        /// 查找是否有结果(获取病人资料状态)
        /// </summary>
        /// <param name="eyPatientQC"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetPatientStateForQueryIsResult(EntityPatientQC eyPatientQC);


        /// <summary>
        /// 查询病人信息单表的所有字段(报告复制用,查询出的数据处理后还要插入到病人信息表中,故单独写方法)
        /// </summary>
        /// <param name="sbPatId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> SearchPatientForReportCopyUse(string sbPatId);


        /// <summary>
        /// 根据病人ID更新复查标志
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateRepRecheckFlag(EntityPidReportMain patient);

        /// <summary>
        /// 更新病人查看者
        /// </summary>
        /// <param name="strOpType"></param>
        /// <param name="RepReadUserId"></param>
        /// <param name="repId"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateRepReadUserId(string strOpType, string RepReadUserId, List<string> listRepId);


        /// <summary>
        /// 获取仪器的最大样本号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="excluseSeqRecord">是否排除双向录入结果</param>
        /// <returns></returns>
        [OperationContract]
        string GetItrSID_MaxPlusOne(DateTime date, string itr_id, bool excluseSeqRecord);

        /// <summary>
        /// 获取仪器的最大序号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        [OperationContract]
        string GetItrHostOrder_MaxPlusOne(DateTime date, string itr_id);

        /// <summary>
        /// 更新病人状态为已打印，更新打印时间
        /// </summary>
        /// <param name="listReptId"></param>
        /// <param name="repStatus"></param>
        /// <returns></returns>
        [OperationContract]
        void UpdatePrintState(IEnumerable<string>listReptId, string repStatus);

        /// <summary>
        /// 更新病人状态为已打印,更新打印时间(带操作者)
        /// </summary>
        /// <param name="patIDs"></param>
        /// <param name="OperatorID">操作者ID</param>
        /// <param name="OperatorName">操作者名称</param>
        /// <param name="strPlace">地点</param>
        [OperationContract]
        void UpdatePrintState_whitOperator(IEnumerable<string> patIDs, string status, string OperatorID, string OperatorName, string strPlace);

        /// <summary>
        /// 批量上传数据（广州12人医）
        /// </summary>
        /// <param name="listPat">病人信息</param>
        /// <returns></returns>
        [OperationContract]
        string BatchUpload(List<EntityPidReportMain> listPat);


        /// <summary>
        /// 获取病人tat超时数据
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityTatOverTime> GetPatTatOverTime(string barCode);


        /// <summary>
        /// 获取条形码可用的仪器列表
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicInstrument> GetAllItrForBarCode(string barCode);

        /// <summary>
        /// 插入门诊报告单 （常规检验-->门诊导入）
        /// </summary>
        /// <param name="Reports"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertReports(List<EntityPidReportMain> Reports, out string ErrorMsg);

        /// <summary>
        /// 获取中间表插入失败的报告单
        /// </summary>
        /// <param name="Reports"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetFaultUpLoadReport(EntityPatientQC qc, string type);
    }
}
