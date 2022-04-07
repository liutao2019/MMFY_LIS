using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{

    public interface IDaoObrResult : IDaoBase
    {

        /// <summary>
        /// 新增检验结果信息
        /// </summary>
        /// <param name="ObrResult"></param>
        /// <returns></returns>
        bool InsertObrResult(EntityObrResult ObrResult);

        /// <summary>
        /// 更新检验结果信息
        /// </summary>
        /// <param name="ObrResult"></param>
        /// <returns></returns>
        bool UpdateObrResult(EntityObrResult ObrResult);

        /// <summary>
        /// 根据报告ID和项目ID更新检验结果表部分字段
        /// </summary>
        /// <param name="ObrResult"></param>
        /// <returns></returns>
        bool UpdateObrResultByObrIdAndObrItmId(EntityObrResult ObrResult);

        /// <summary>
        /// 根据条件更新有效标志
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        bool UpdateObrFlagByCondition(EntityResultQC resultQc);

        /// <summary>
        /// 检验报告查询
        /// </summary>
        /// <param name="resultQc"></param>
        /// <param name="withHistoryResult"></param>
        /// <returns></returns>
        List<EntityObrResult> ObrResultQuery(EntityResultQC resultQc, bool withHistoryResult = false);


        /// <summary>
        ///  检验结果查询（根据项目编码等条件）
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        List<EntityObrResult> LisResultQuery(EntityResultQC resultQc);

        /// <summary>
        /// 根据主键ID删除检验项目结果
        /// </summary>
        /// <param name="obrSn"></param>
        /// <returns></returns>
        bool DeleteObrResultByObrSn(string obrSn);

        /// <summary>
        /// 根据主键ID更新复查标志
        /// </summary>
        /// <param name="obrSn"></param>
        /// <returns></returns>
        bool UpdateRecheckFalgByObrSn(string obrSn);

        /// <summary>
        /// 根据病人ID删除病人结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        bool DeleteObrResultByObrId(string obrId);

        /// <summary>
        /// 获取没有病人的结果
        /// </summary>
        /// <param name="listItrs"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        List<EntityObrResult> GetObrResultByNoPat(List<string> listItrs, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 根据条件删除结果表信息（项目结果备份用）
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        bool DeleteObrResultByQC(EntityResultQC resultQc);

        /// <summary>
        /// 根据唯一标识更新结果和数值结果
        /// </summary>
        /// <param name="obrValue">结果</param>
        /// <param name="obrConvertValue">数值结果</param>
        /// <param name="obrSn">标识ID</param>
        /// <returns></returns>
        bool UpdateResultVauleByObrSn(string obrValue,string obrConvertValue,string obrSn);

        /// <summary>
        /// 根据标识ID和项目Id更新结果的组合Id （二审时更新无组合结果数据）
        /// </summary>
        /// <param name="comId"></param>
        /// <param name="obrId"></param>
        /// <param name="itmId"></param>
        /// <returns></returns>
        bool UpdateResultComIdByObrIdAndItmID(string comId, string obrId, string itmId);

        /// <summary>
        /// 更新分期多参考值 （2审时写入 或者清除）
        /// </summary>
        /// <param name="obrSn">写入时根据唯一标识写入</param>
        /// <param name="obrId">清除时根据标识ID写入</param>
        /// <param name="refMore">分期多参考值</param>
        /// <param name="isClear">是否清除</param>
        /// <returns></returns>
        bool UpdateResultRefMore(string obrSn,string obrId,string refMore, bool isClear);

        /// <summary>
        /// 获取病人结果信息 审核时历史结果对比用于检查同一条码的两份报告的结果是否一致 关联为inner jion 所以单独写方法
        /// </summary>
        /// <param name="RepBarcode"></param>
        /// <param name="repId"></param>
        /// <returns></returns>
        List<EntityObrResult> GetResultInfoByBarcode(string RepBarcode, string repId);

        /// <summary>
        /// 获取历史结果
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<EntityObrResult> GetResultHistory(EntityHistoryPatientQC qc);
    }
}
