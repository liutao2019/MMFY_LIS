using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ServiceModel;



using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISampStoreRecord
    {
        /// <summary>
        /// 获取试管架类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicTubeRack> GetCuvShelf();
        /// <summary>
        /// 获取架子信息
        /// </summary>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampTubeRack> GetDictRackList(DateTime dateTimeFrom, DateTime dateTimeTo);

        /// <summary>
        /// 查询架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetRackDetail(string strSsid);

        /// <summary>
        /// 获取架子状态
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampStoreStatus> GetSamManageStatus();

        /// <summary>
        /// 取状态值
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        int GetSamRackStatus(string strSsid);
        /// <summary>
        /// 根据条码获取病人信息
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [OperationContract]
        List<entity.EntityPidReportMain> GetPatientsInfo(string barcode);

        /// <summary>
        /// 标本是否在使用中
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [OperationContract]
        bool IsBarCodeUsing(string barCode);
        /// <summary>
        /// 根据条码号和报告ID获取病人组合明细
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="patID"></param>
        /// <returns></returns>
        [OperationContract]
        string[] GetAppendBarCode(string barcode, string patID);
        /// <summary>
        /// 新增试管存储信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertRackDetail(EntitySampStoreDetail entity);

        /// <summary>
        /// 修改架子的状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int ModifyRackStatus(EntityDicSampTubeRack entity);

        /// <summary>
        /// 新增条码操作记录
        /// </summary>
        /// <param name="operatorName"></param>
        /// <param name="operatorID"></param>
        /// <param name="barCode"></param>
        /// <param name="bcStatus"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertBcSign(string operatorName, string operatorID, string barCode, string bcStatus, string remark, string opPlace);
        /// <summary>
        /// 修改归档架子记录信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int ModifySamStoreRack(EntitySampStoreRack entity);
        /// <summary>
        /// 根据多个条件获取试管存储信息
        /// </summary>
        /// <param name="date"></param>
        /// <param name="BatchItr"></param>
        /// <param name="SamFrom"></param>
        /// <param name="SamTo"></param>
        /// <param name="selectIndex"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetBatchHandData(DateTime date, string BatchItr, string SamFrom, string SamTo, int selectIndex);
        /// <summary>
        /// 归档操作
        /// </summary>
        /// <param name="table"></param>
        /// <param name="strSsid"></param>
        /// <param name="strRackID"></param>
        /// <param name="cuvcode"></param>
        /// <param name="operatorName"></param>
        /// <param name="operatorID"></param>
        /// <param name="opPlace"></param>
        /// <param name="rowhander"></param>
        /// <param name="colHander"></param>
        /// <param name="rack_Barcode"></param>
        /// <returns></returns>
        [OperationContract]
        string BatchHandData(List<EntitySampStoreDetail> table, string strSsid, string strRackID, string cuvcode, string operatorName,
                     string operatorID, string opPlace, int rowhander, int colHander, string rack_Barcode);
        /// <summary>
        /// 删除试管存储信息
        /// </summary>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <param name="barcodeList"></param>
        /// <returns></returns>
        [OperationContract]
        int DeleteSamDetail(string strSsid, string rackID, List<string> barcodeList);
    }
}
