using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampMain : IDaoBase
    {
        /// <summary>
        /// 获取条码列表信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        List<EntitySampMain> GetSampMain(EntitySampQC barcode);

        /// <summary>
        /// 简单统计条码工作量
        /// </summary>
        /// <param name="sampCondition"></param>
        /// <returns></returns>
        List<EntitySampMain> SimpleStatisticSamp(EntitySampQC sampCondition);

        /// <summary>
        /// 删除条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean DeleteSampMain(String sampBarId);

        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="sampMain"></param>
        Boolean CreateSampMain(List<EntitySampMain> listSampMain);

        /// <summary>
        /// 更新条码状态
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        Boolean UpdateSampMainStatus(EntitySampOperation operation, List<EntitySampMain> listSampMain);

        /// <summary>
        /// 更新条码批次
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="listSampMain"></param>
        /// <returns></returns>
        Boolean UpdateSampMainBatchNo(Int64 batchNo, List<EntitySampMain> listSampMain);

        /// <summary>
        /// 更新条码加急标志
        /// </summary>
        /// <param name="urgent"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean UpdateSampMainUrgentFlag(bool urgent, String sampBarId);

        /// <summary>
        /// 更新条码标本备注
        /// </summary>
        /// <param name="remarkId"></param>
        /// <param name="remarkName"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean UpdateSampMainRemark(String remarkId, String remarkName, String sampBarId);

        /// <summary>
        /// 更新条码组合名称
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean UpdateSampMainComName(String comName, String sampBarId);

        /// <summary>
        /// 判断条码号是否回退
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        Boolean Returned(string barCode);

        /// <summary>
        /// 获取条码标本信息
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        EntitySampMain GetSampMainSampleInfo(String sampBarId);

        /// <summary>
        /// 更新条码的标本信息
        /// </summary>
        /// <param name="sampleId"></param>
        /// <param name="sampleName"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean UpdateSampMainSampleInfo(String sampleId, String sampleName, String sampBarId);

        /// <summary>
        /// 更新条码的名称和性别
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean UpdateSampMainNameAndSex(String name, String sex, String sampBarId);

        /// <summary>
        /// 更新条码信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sex"></param>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean UpdateSampMainOtherInfo(EntitySampMain sampMain, String sampBarId);

        /// <summary>
        /// 判断是否存在该标本
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean ExistSampMain(String sampBarId);

        /// <summary>
        /// 更新条码号（预置条码）
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <param name="sampPlaceCode"></param>
        /// <returns></returns>
        Boolean UpdateSampMainBarCode(String sampBarId, String sampPlaceCode);

        /// <summary>
        /// 重置预置条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean UndoSampMain(String sampBarId);

        /// <summary>
        /// 更新回退条码回退标志
        /// </summary>
        /// <param name="sampMain"></param>
        /// <returns></returns>
        Boolean UpdateSampReturnFlag(EntitySampMain sampMain);

        /// <summary>
        /// 根据病人标识或者诊疗卡号获取病人信息（手工条码）
        /// </summary>
        /// <param name="patInNo"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        List<EntitySampMain> GetPatientsInfoByBcInNo(string patInNo, string colName);

        /// <summary>
        /// 更新标本打包包号
        /// </summary>
        /// <param name="sampInfo">包号</param>
        /// <param name="listCode">条码号</param>
        /// <returns></returns>
        Boolean UpdateBarcodeBale(string pidUniqueId, List<string> listCode);

        /// <summary>
        /// 根据内部关联ID更新条码类型(0-打印条码 1-预制条码)
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <param name="samp_bar_type"></param>
        /// <returns></returns>
        Boolean UpdateSampBarType(string sampBarId, int samp_bar_type);

        /// <summary>
        /// 获取报数
        /// </summary>
        /// <param name="sampStatus">操作状态</param>
        /// <param name="deptCode">科室代码</param>
        /// <returns></returns>
        List<string > GetPackCount(string sampStatus, string deptCode);

        /// <summary>
        /// 获取收费失败的条码
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        List<EntitySampMain> GetFaultChargeSamp(EntitySampQC qc);


        /// <summary>
        /// 获取新的条码
        /// </summary>
        /// <returns></returns>
        string GetNewBarcode();
        /// <summary>
        /// 获取旧的条码
        /// </summary>
        /// <returns></returns>
        string GetOldBarcode(int id);
        /// <summary>
        /// 更新条码
        /// </summary>
        /// <returns></returns>
        string UpdateNewBarcode(string barcode, int id);
    }
}
