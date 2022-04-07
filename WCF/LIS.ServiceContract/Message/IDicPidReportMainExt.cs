using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 病人信息扩展表:接口
    /// </summary>
    [ServiceContract]
    public interface IDicPidReportMainExt
    {
        /// <summary>
        /// 插入病人信息扩展表数据
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        bool SavePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id);

        /// <summary>
        /// 更新病人信息扩展表数据
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id);

        /// <summary>
        /// 扩展表是否已存在此ID
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        bool SearchPatExtExistByID(string pat_id);

        /// <summary>
        /// 新增或更新病人扩展表(patients_ext/Pid_report_main_ext)信息
        /// </summary>
        /// <param name="colName"></param>
        /// <param name="colValue"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddOrUpdatePatientExt(EntityDicPidReportMainExt reportMainExt);

        /// <summary>
        /// 根据pat_id获取病人扩展表patients_ext的信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicPidReportMainExt> GetPatientExtDataByPatID(string pat_id);
        /// <summary>
        /// 更新病人扩展表的危急值信息
        /// </summary>
        /// <param name="PatientExt"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdatePatientsExt(EntityDicPidReportMainExt PatientExt, bool update);
    }
}
