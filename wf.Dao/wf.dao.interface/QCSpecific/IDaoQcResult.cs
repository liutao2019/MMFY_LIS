using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 质控结果表
    /// </summary>
    public interface IDaoQcResult
    {
        /// <summary>
        /// 新增质控结果表数据
        /// </summary>
        /// <param name="ItrId"></param>
        /// <param name="ItmId"></param>
        /// <param name="QcValue"></param>
        /// <param name="NoType"></param>
        /// <param name="MatSn"></param>
        /// <param name="QcDate"></param>
        /// <returns></returns>
        bool InsertQcResult(string ItrId, string ItmId, string QcValue, string NoType, string MatSn, DateTime QcDate);

        /// <summary>
        /// 获取质控图表列表
        /// </summary>
        /// <returns></returns>
        List<EntityQcTreeView> GetQcTreeView(string strItrId, DateTime dtSDate, DateTime dtEDate, QcTreeViewType viewType, bool radarView);

        /// <summary>
        /// 查询质控结果
        /// </summary>
        /// <param name="listResultQc"></param>
        /// <returns></returns>
        List<EntityObrQcResult> GetQcResult(EntityObrQcResultQC resultQc);

        /// <summary>
        /// 查询半定量字典
        /// </summary>
        /// <param name="strItrId"></param>
        /// <param name="strItmId"></param>
        /// <returns></returns>
        List<EntityDicQcConvert> GetQcConvert(String strItrId, String strItmId);

        /// <summary>
        /// 反审数据
        /// </summary>
        /// <param name="listQresSn"></param>
        /// <returns></returns>
        Boolean QcResultUndoAudit(List<String> listQresSn);

        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="listQcResult"></param>
        /// <returns></returns>
        Boolean QcResultAudit(List<EntityObrQcResult> listQcResult);

        /// <summary>
        /// 二审
        /// </summary>
        /// <param name="listQcResult"></param>
        /// <returns></returns>
        Boolean QcResultSecondAudit(List<EntityObrQcResult> listQcResult);

        /// <summary>
        /// 更新显示标识
        /// </summary>
        /// <param name="strQresSn"></param>
        /// <param name="display"></param>
        /// <returns></returns>
        Boolean UpdateQresDisplay(string strQresSn, int display);

        /// <summary>
        /// 基础查询，不关联字典等
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        List<EntityObrQcResult> QcResultQuery(EntityObrQcResultQC resultQc);

        /// <summary>
        /// 保存质控结果
        /// </summary>
        /// <param name="qcResult"></param>
        /// <returns></returns>
        Boolean SaveQcResult(EntityObrQcResult qcResult);

        /// <summary>
        /// 删除质控结果
        /// </summary>
        /// <param name="listQresSn"></param>
        /// <returns></returns>
        Boolean DeleteQcResult(List<string> listQresSn);

        /// <summary>
        /// 更新质控结果
        /// </summary>
        /// <param name="qcResult"></param>
        /// <returns></returns>
        Boolean UpdateQcResult(EntityObrQcResult qcResult);

        /// <summary>
        /// 获取质控统计分析数据
        /// </summary>
        /// <param name="lisItem"></param>
        /// <returns></returns>
        List<EntityQcStatistic> GetAnalyseData(List<EntityObrQcResultQC> lisItem);


        DataSet doNew(DataSet ds,bool isseq);

        DataTable QcReagentsCompare(DataTable QcItem, DataTable QcCompareData);
    }
}
