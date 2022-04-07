using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 病人信息扩展表:接口
    /// </summary>
    public interface IDaoPidReportMainExt
    {
        /// <summary>
        /// 插入病人信息扩展表数据
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        bool SavePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id);

        /// <summary>
        /// 更新病人信息扩展表数据
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        bool UpdatePidReportMainExt(EntityAuditInfo objAuditInfo, string pat_id);

        /// <summary>
        /// 扩展表是否已存在此ID
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        bool SearchPatExtExistByID(string pat_id);

        /// <summary>
        /// 根据pat_id获取病人扩展表patients_ext的信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        List<EntityDicPidReportMainExt> GetPatientExtDataByPatID(string pat_id);

        /// <summary>
        /// 更新病人扩展表patients_ext数据
        /// </summary>
        /// <param name="setSql"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        bool UpdatePatientExtInfoCMD(EntityDicPidReportMainExt ext);

        /// <summary>
        /// 新增病人扩展表patients_ext的语句
        /// </summary>
        /// <param name="ColSql"></param>
        /// <param name="valueSql"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        bool InsertPatientExtInfoCMD(EntityDicPidReportMainExt ext);

        /// <summary>
        /// 删除病人扩展信息
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        bool DeletePatientExt(string repId);
        /// <summary>
        /// 新增病人扩展信息
        /// </summary>
        /// <param name="patientExt"></param>
        /// <returns></returns>
        bool InsertPatientExt(EntityDicPidReportMainExt patientExt);

        /// <summary>
        /// 更新病人扩展信息的危急值内容
        /// </summary>
        /// <param name="patientExt"></param>
        /// <returns></returns>
        bool UpdatePatientExt(EntityDicPidReportMainExt patientExt);

        /// <summary>
        /// 将报告单电子签章写入数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        bool InsertReportCASignature(DataRow dr);
    }
}
