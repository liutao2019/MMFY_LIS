using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ServiceModel;



using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ISampStoreDetail
    {

        /// <summary>
        /// 查询架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetRackDetail(string strSsid);
        /// <summary>
        /// 新增试管存储信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        int InsertRackDetail(EntitySampStoreDetail entity);
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
        int DeleteSampStoreDetail(string strSsid, string barcode);

        /// <summary>
        /// 根据ID获取数据数量
        /// </summary>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <param name="barcodeList"></param>
        /// <returns></returns>
        [OperationContract]
        int GetSampStoreDetailCount(string strSsid);
    }
}
