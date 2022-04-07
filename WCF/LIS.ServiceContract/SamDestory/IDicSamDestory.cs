using dcl.entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
   public interface IDicSamDestory
    {
        /// <summary>
        /// 查询归档架子记录及其相关柜子冰箱信息
        /// </summary>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <param name="rackCtype"></param>
        /// <param name="iceID"></param>
        /// <param name="cupID"></param>
        /// <param name="rackID"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreRack> GetRackDataForDestory(DateTime? dateTimeFrom, DateTime? dateTimeTo, string rackCtype,
                                               string iceID, string cupID,
                                               string rackID, string barcode);

        /// <summary>
        /// 更新标本相关的冰箱柜子架子等的状态
        /// </summary>
        /// <param name="barcodeList"></param>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <param name="operatorName"></param>
        /// <param name="operatorID"></param>
        /// <param name="opPlace"></param>
        /// <param name="iecID"></param>
        /// <param name="cupID"></param>
        /// <returns></returns>
        [OperationContract]
        int DestoryRackSam(List<string> barcodeList, string strSsid, string rackID, string operatorName,
                                  string operatorID, string opPlace, string iecID, string cupID);

        /// <summary>
        /// 判断是否改变架子状态
        /// </summary>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <returns></returns>
        [OperationContract]
        bool CanRollBackDestory(string strSsid, string rackID);

        /// <summary>
        /// 更新试管存储信息，更新架子孔数量
        /// </summary>
        /// <param name="barcodeList"></param>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <returns></returns>
        [OperationContract]
        int RollBackDestory(List<string> barcodeList, string strSsid, string rackID);

        /// <summary>
        /// 查询试管存储信息数据/架子中试管的详细内容
        /// </summary>
        /// <param name="strSsid"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySampStoreDetail> GetRackDetailForDestory(string strSsid);
         
    }
}
