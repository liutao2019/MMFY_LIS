using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 质控结果表
    /// </summary>
    [ServiceContract]
    public interface IObrQcResult
    {
        /// <summary>
        /// 获取质控图表列表
        /// </summary>
        /// <param name="strItrId">仪器ID</param>
        /// <param name="dtSDate">开始时间</param>
        /// <param name="dtEDate">结束时间</param>
        /// <param name="viewType">显示类型</param>
        /// <param name="radarView">是否是雷达图</param>
        /// <returns></returns>
        [OperationContract]
        List<EntityQcTreeView> GetQcTreeView(string strItrId, DateTime dtSDate, DateTime dtEDate, QcTreeViewType viewType, bool radarView);

        /// <summary>
        /// 获取质控结果（会预处理）
        /// </summary>
        /// <param name="listResultQc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrQcResult> GetQcResult(List<EntityObrQcResultQC> listResultQc);


        /// <summary>
        /// 获取半定量字典
        /// </summary>
        /// <param name="strItrId"></param>
        /// <param name="strItmId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcConvert> GetQcConvert(String strItrId, String strItmId);

        /// <summary>
        /// 反审数据
        /// </summary>
        /// <param name="listQresSn"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean QcResultUndoAudit(List<String> listQresSn);

        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="listQcResult"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean QcResultAudit(List<EntityObrQcResult> listQcResult, string operatorID);

        /// <summary>
        /// 二审
        /// </summary>
        /// <param name="listQcResult"></param>
        /// <param name="operatorID"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean QcResultSecondAudit(List<EntityObrQcResult> listQcResult, string operatorID);

        /// <summary>
        /// 更新显示标识
        /// </summary>
        /// <param name="strQresSn"></param>
        /// <param name="display"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateQresDisplay(string strQresSn, int display);

        /// <summary>
        /// 基础查询，不关联字典等
        /// </summary>
        /// <param name="resultQc"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrQcResult> QcResultQuery(EntityObrQcResultQC resultQc);

        /// <summary>
        /// 保存质控结果
        /// </summary>
        /// <param name="qcResult"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean SaveQcResult(EntityObrQcResult qcResult);

        /// <summary>
        /// 删除质控结果
        /// </summary>
        /// <param name="listQresSn"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteQcResult(List<string> listQresSn);

        /// <summary>
        /// 更新质控结果
        /// </summary>
        /// <param name="qcResult"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateQcResult(EntityObrQcResult qcResult);

        /// <summary>
        /// 插入数据到质控结果表
        /// </summary>
        /// <param name="ItrId"></param>
        /// <param name="ItmId"></param>
        /// <param name="QcValue"></param>
        /// <param name="NoType"></param>
        /// <param name="MatSn"></param>
        /// <param name="QcDate"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean InsertQcResult(string ItrId, string ItmId, string QcValue, string NoType, string MatSn, DateTime QcDate);

        /// <summary>
        /// 获取质控统计分析数据
        /// </summary>
        /// <param name="lisItem"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityQcStatistic> GetAnalyseData(List<EntityObrQcResultQC> lisItem);



        [OperationContract]
        DataSet doNew(DataSet ds);

        [OperationContract]
        DataTable QcReagentsCompare(DataTable QcItem, DataTable QcCompareData);
    }
}
