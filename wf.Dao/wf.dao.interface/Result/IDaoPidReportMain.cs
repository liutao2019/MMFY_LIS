using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoPidReportMain : IDaoBase
    {

        /// <summary>
        /// 检查样本号或者序号是否已经存在(flag为1即为查序号)
        /// </summary>
        /// <param name="pat_sid"></param>
        /// <param name="pat_itr_id"></param>
        /// <param name="pat_date"></param>
        /// <returns></returns>
        bool ExsitSidOrHostOrder(string pat_sid, string pat_itr_id, DateTime pat_date, string flag = "");

        /// <summary>
        /// 插入病人资料
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        bool InsertNewPatient(EntityPidReportMain patients);

        /// <summary>
        /// 根据仪器Id、条码号、样本号获取病人的标识Id
        /// </summary>
        /// <param name="patItrId">仪器ID</param>
        /// <param name="patBarcode">条码号</param>
        /// <param name="patSid">样本号</param>
        /// <returns></returns>
        string GetPatientPatId(string patItrId, string patBarcode, string patSid, DateTime repInDate);

        /// <summary>
        /// 根据大组合(特殊合并)ID获取已上机病人信息
        /// </summary>
        /// <param name="BcMergeComid"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatientByMergeComid(string BcMergeComid);

        /// <summary>
        /// 获取单个病人信息
        /// </summary>
        /// <param name="repID"></param>
        /// <returns></returns>
        EntityPidReportMain GetPatientInfo(string repID);


        /// <summary>
        /// 获取多个病人信息
        /// </summary>
        /// <param name="repID"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatientInfo(IEnumerable<string> repID);

        /// <summary>
        /// 查询病人资料
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        List<EntityPidReportMain> PatientQuery(EntityPatientQC patientCondition);

        /// <summary>
        /// 更新病人资料，适用于更新多个字段内容
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        bool UpdatePatientData(EntityPidReportMain patient);


        /// <summary>
        /// 删除病人
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        bool DeletePatient(string repId);

        /// <summary>
        /// 根据老标识Id来更新新的标识Id
        /// </summary>
        /// <param name="newRepId">新标识Id</param>
        /// <param name="repId">老标识Id</param>
        /// <returns></returns>
        bool UpdateNewRepIdByOldPatId(string newRepId, string repId);

        /// <summary>
        /// 根据病人的标识id更新组合名称
        /// </summary>
        /// <param name="pidComName">组合名称</param>
        /// <param name="repId">标识ID</param>
        /// <returns></returns>
        bool UpdatePidComNameByRepId(string pidComName, string repId);

        /// <summary>
        /// 获取病人信息是否超时
        /// </summary>
        /// <param name="patId"></param>
        /// <param name="repCType"></param>
        /// <returns></returns>
        string GetPatientIsOverTime(string patId, string repCType);

        /// <summary>
        /// 获取病人信息的数量
        /// </summary>
        /// <param name="patientCondition"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatientsCount(EntityPatientQC patientCondition);

        /// <summary>
        /// 查找是否有结果(获取病人资料状态)
        /// </summary>
        /// <param name="eyPatientQC"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatientStateForQueryIsResult(EntityPatientQC eyPatientQC);


        /// <summary>
        /// 查询病人信息单表的所有字段(报告复制用,查询出的数据处理后还要插入到病人信息表中,故单独写方法)
        /// </summary>
        /// <param name="sbPatId"></param>
        /// <returns></returns>
        List<EntityPidReportMain> SearchPatientForReportCopyUse(string sbPatId);


        /// <summary>
        /// 根据病人ID更新复查标志
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        bool UpdateRepRecheckFlag(EntityPidReportMain patient);

        /// <summary>
        /// 修改病人查看者
        /// </summary>
        /// <param name="RepReadUserId"></param>
        /// <param name="repId"></param>
        /// <returns></returns>
        bool UpdateRepReadUserId(string strOpType, string RepReadUserId, string repId, bool unAllLookcode);

        /// <summary>
        /// 更新危急值查看标志和查看者
        /// </summary>
        /// <param name="urgent">是否急查</param>
        /// <param name="repReadUserId"></param>
        /// <param name="repID"></param>
        /// <returns></returns>
        bool UpdateReadUserIdAndUrgentFlag(bool urgent, string repReadUserId, string repID);

        /// <summary>
        /// 获取仪器最大样本号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="excluseSeqRecord">是否排除双向录入结果</param>
        /// <param name="strGetAllMaxSidItrId">条码录入仪器获取当前两天内的最大样本号(仪器ID,仪器ID2)</param>
        /// <returns></returns>
        string GetItrSID_MaxPlusOne(DateTime date, string itr_id, bool excluseSeqRecord,string strGetAllMaxSidItrId);

        /// <summary>
        /// 获取仪器最大序号+1
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        string GetItrHostOrder_MaxPlusOne(DateTime date, string itr_id);

        /// <summary>
        /// 更新病人状态
        /// </summary>
        /// <param name="repId"></param>
        /// <param name="repStatus"></param>
        /// <param name="isPrintState">是否是打印状态</param>
        /// <returns></returns>
        void UpdateRepStatus(string repId,string repStatus,bool isPrintState);

        /// <summary>
        /// 获取在该物理组的所有样本号  （标本进程）
        /// </summary>
        /// <param name="labId">物理组</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetAllLabPat(string labId,DateTime startDate,DateTime endDate);

        /// <summary>
        /// 获取是否有结果的病人信息
        /// </summary>
        /// <param name="listItrId">仪器id集合</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="result">是否有结果</param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatByExistResult(List<string >listItrId,DateTime startDate,DateTime endDate,bool result);


        /// <summary>
        /// 根据病人的标识id更新病人修改信息次数
        /// </summary>
        /// <param name="repModifyFrequency">次数</param>
        /// <param name="repId">标识ID</param>
        /// <returns></returns>
        bool UpdateRepModifyFrequencyByRepId(int repModifyFrequency, string repId);

        /// <summary>
        ///上传病人资料
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        bool UploadNewPatient(EntityPidReportMain patients);

        /// <summary>
        ///更新报告解读信息
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        bool UpdateReportSumInfo(string repID,string sumInfo);

        /// <summary>
        /// 跟新多重耐药标志
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        bool UpdateDrugfastFlag(string repId);

        /// <summary>
        /// 插入多条报告记录
        /// </summary>
        /// <param name="Reports"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        bool InsertReports(List<EntityPidReportMain> Reports,out string ErrorMsg);

        EntityPidReportMain SearchReportByTestSeq(string TestSeq);

        /// <summary>
        /// 更新中期报告单相关
        /// </summary>
        /// <returns></returns>
        bool UpdateMicReport(EntityPidReportMain Report);

        /// <summary>
        /// 获取中间表参数
        /// </summary>
        /// <param name="RepIDs"></param>
        /// <returns></returns>
        List<EntityDCLReportParmeter> GetDCLParmeter(List<string> RepIDs);

        /// <summary>
        /// 根据唯一的样本号更新备注和处理意见信息
        /// </summary>
        /// <param name="Reports"></param>
        /// <returns></returns>
        bool UpdateRemarkBySeq(List<EntityObrResultTestSeqVer> Reports);

        /// <summary>
        /// 获取上传失败的报告单
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetFaultUpLoadReport(EntityPatientQC qc, string type);

    }
}
