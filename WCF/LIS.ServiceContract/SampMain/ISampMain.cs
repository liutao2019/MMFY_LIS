using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISampMain
    {
        /// <summary>
        /// 查询条码
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampMain> SampMainQuery(EntitySampQC sampCondition);


        /// <summary>
        /// 简单统计条码工作量
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampMain> SimpleStatisticSamp(EntitySampQC sampCondition);

        /// <summary>
        /// 根据条码号查询条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        EntitySampMain SampMainQueryByBarId(String sampBarId);

        /// <summary>
        /// 删除条码
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteSampMain(EntitySampOperation operation, EntitySampMain sampMain);

        /// <summary>
        /// 条码拆分
        /// </summary>
        /// <param name="sampMain"></param>
        /// <param name="listSeperateDetail"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampMain> SeperateSampMain(EntitySampMain sampMain, List<EntitySampDetail> listSeperateDetail);

        /// <summary>
        /// 更新条码状态
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainStatus(EntitySampOperation operation, List<EntitySampMain> listSampMain);

        /// <summary>
        /// 更新条码批次
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainBatchNo(Int64 batchNo, List<EntitySampMain> listSampMain);

        /// <summary>
        /// 更新加急标志
        /// </summary>
        /// <param name="urgent"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainUrgentFlag(Boolean urgent, String sampBarId);

        /// <summary>
        /// 更新标本备注
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="remarkName"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainRemark(String remarkId, String remarkName, String sampBarId);

        /// <summary>
        /// 更新标本信息
        /// </summary>
        /// <param name="sampleId"></param>
        /// <param name="sampleName"></param>
        /// <param name="listSampMain"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainSampleInfo(String sampleId, String sampleName, List<EntitySampMain> listSampMain, EntitySampOperation operation);

        /// <summary>
        /// 更新条码姓名和性别
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainNameAndSex(String name, String sex, String sampBarId);

        /// <summary>
        /// 更新条码其他信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainOtherInfo(EntitySampMain sampMain, String sampBarId);

        /// <summary>
        /// 手工生成条码
        /// </summary>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        [OperationContract]
        List<String> ManualCreateSampMain(EntitySampMain sampMain);

        /// <summary>
        /// 判断是否存在该标本
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean ExistSampMain(String sampBarId);

        /// <summary>
        /// 更新条码信息（预置条码）
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <param name="sampPlaceCode"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampMainBarCode(String sampBarId, String sampPlaceCode);

        /// <summary>
        /// 标本合并
        /// </summary>
        /// <param name="listSampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean MergeSampMain(List<String> listSampBarId);

        /// <summary>
        /// 重置预制条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        String UndoSampMain(EntitySampOperation operation, EntitySampMain sampMain);

        /// <summary>
        /// 取消预制条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        [OperationContract]
        String CancelUndoSampMain(String sampBarId);

        /// <summary>
        /// 根据内部关联ID更新条码类型(0-打印条码 1-预制条码)
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <param name="samp_bar_type"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateSampBarType(String sampBarId, int samp_bar_type);

        /// <summary>
        /// 根据病人标识查询病人信息
        /// </summary>
        /// <param name="pidInNo"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampMain> GetPatientsInfoByBcInNo(string pidInNo);


        /// <summary>
        /// 更新标本打包包号
        /// </summary>
        /// <param name="sampInfo">包号</param>
        /// <param name="listSampBarCode">条码号</param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateBarcodeBale(string pidUniqueId, List<String> listSampBarCode);

        /// <summary>
        /// 流程确认前检查
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        [OperationContract]
        String ConfirmBeforeCheck(EntitySampOperation operation, EntitySampMain sampMain);

        /// <summary>
        /// 获取包数
        /// </summary>
        /// <param name="sampStatus">操作状态</param>
        /// <param name="deptCode">科室代码</param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetPackCount(string sampStatus, string deptCode);

        /// <summary>
        /// 门诊条码生成报告信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> MZImportReport(EntityInterfaceExtParameter Parameter);

        /// <summary>
        /// 获取收费失败的条码信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampMain> GetFaultChargeSamp(EntitySampQC qc);
    }
}
