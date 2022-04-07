using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ServiceModel;



using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISamManage
    {
        /// <summary>
        /// 试管架类型
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicTubeRack> GetCuvShelf();

        [OperationContract]
        List<EntityDicSampTubeRack> GetDictRackList(DateTime dateTimeFrom, DateTime dateTimeTo);


        [OperationContract]
        string GetMaxRack();

        [OperationContract]
        string GetMaxRackCode();

        [OperationContract]
        string GetMaxSamRackID();


        /// <summary>
        /// 获取专业组
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicPubProfession> GetPhyic();

        /// <summary>
        /// 在dict_rack表插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertIntoRack(EntityDicSampTubeRack entity);

        [OperationContract]
        bool ModifyRackRecord(EntityDicSampTubeRack entity);

        [OperationContract]
        int DeleteRackRecord(EntityDicSampTubeRack entity);



        [OperationContract]
        List<entity.EntityPidReportMain> GetPatientsInfo(string barcode);

        [OperationContract]
        string[] GetAppendBarCode(string barcode, string patID);

        /// <summary>
        /// 查询架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetRackDetail(string strSsid);

        /// <summary>
        /// 标本是否在使用中
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [OperationContract]
        bool IsBarCodeUsing(string barCode);

        /// <summary>
        /// 查询架子中试管的明细内容(无外连表)
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetSimpleRackDetail(string strSsid);

        [OperationContract]
        int InsertRackDetail(EntitySampStoreDetail entity);

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
        /// 删除条码操作记录
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="bcStatus"></param>
        /// <returns></returns>
        [OperationContract]
        int DeleteBcSign(string barCode, string bcStatus);

        /// <summary>
        /// 根据条码获取标本流传明细信息
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampProcessDetail> GetBcSign(string barCode);

        [OperationContract]
        string GetMaxRackDetail();

        /// <summary>
        /// 向SamStore_Rack中插入一条数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertSamRack(EntitySampStoreRack entity);

        /// <summary>
        /// 得到样本的信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSample> GetSample();

        [OperationContract]
        int ModifySamDetail(EntitySampStoreDetail entity);

        /// <summary>
        /// 获取架子状态
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicSampStoreStatus> GetSamManageStatus();

        [OperationContract]
        int ModifySamStoreRack(EntitySampStoreRack entity);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int AuditSamStoreRack(EntitySampStoreRack entity);

        [OperationContract]
        List<EntitySampStoreDetail> GetBatchHandData(DateTime date, string BatchItr, string SamFrom, string SamTo, int selectIndex);

        [OperationContract]
        string GetNextMaxBarCode();

        [OperationContract]
        bool IsRaclBarCodeExists(string barCode);

        [OperationContract]
        bool IsRaclBarCodePrint(string barCode);

    }
}
