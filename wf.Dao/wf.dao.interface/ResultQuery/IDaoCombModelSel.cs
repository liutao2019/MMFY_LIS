using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoCombModelSel
    {
        /// <summary>
        /// 获取病人列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatientsList(EntityAnanlyseQC query, string dateStart, string dateEnd);

        /// <summary>
        /// 更新报告状态为已打印
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Boolean UpdatePatFlagToPrinted(EntityAnanlyseQC query);

        /// <summary>
        /// 获取病人检验结果信息
        /// </summary>
        /// <param name="query"></param>
        /// <param name="patDate"></param>
        /// <returns></returns>
        EntityQcResultList GetPatientResultData(EntityAnanlyseQC query,DateTime patDate);

        /// <summary>
        /// 查询病人的信息的sql where子句
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<string> GetWhere(EntityAnanlyseQC query);

        /// <summary>
        /// 查询病人的条码号列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        List<EntityPidReportMain> GetPatBarCode(EntityAnanlyseQC query);

        /// <summary>
        /// 获得病人报表信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        EntityDCLPrintData GetPatientReportInfo(EntityAnanlyseQC query);

        /// <summary>
        /// 获取病人标识ID
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        DataTable GetPatId(EntityAnanlyseQC query);

        /// <summary>
        /// 根据标识ID删除病人
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        Boolean DeletePatient(string repId);

        /// <summary>
        /// 报告结果是否含有异常值
        /// </summary>
        /// <param name="repId"></param>
        /// <returns></returns>
        bool IsContainOutlier(string repId) ;
    }
}
